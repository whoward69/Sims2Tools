/*
 * DBPF Scripter - a utility for scripting edits to .package files
 *               - see http://www.picknmixmods.com/Sims2/Notes/DbpfScripter/DbpfScripter.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2025
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */


using Sims2Tools;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Xml;

/*
 * General notes on the dbpfscript.txt file
 * 
 * Ignore leading spaces
 * Ignore everything after // to the end of the line
 * Ignore blank lines
 * Ignore spaces outside of quote delimited strings
 * Semi-colon (;) is "syntatic sugar"
 * 
 * The initial development was heavily influenced by automating https://hypersaline.tumblr.com/post/770353073301422080/tutorial-adding-new-shapes-to-bodyshapes-mod
 *
 * The parser is based on the following Backus–Naur form
 *
 * blocks ::= <block> | <block> <blocks>
 * block ::= INIT [ <initialisers> ] | filename.ods [ <initialisers> ] | filename.package (<var_defn>) [ <actions> ] | REPEAT <count> [ <blocks> ]
 * 
 * initialisers ::= <initialiser> | <initialiser> <initialisers>
 * initialiser ::= <assert> | <var_defn> = <function> | <cell_ref> | <string> | <constant>
 * 
 * var_defn ::= $<var_name>
 * 
 * function ::= <function_name>(<var_defn>)
 * function_name ::= guid | group | family | loword | hiword | lobyte | hibyte | ihash | rhash | fullpath | if
 * 
 * cell_ref ::= <col_ref><row_ref>
 * col_ref ::= [A-Z]
 * row_ref ::= 1 ..
 * 
 * actions ::= <action> | <action> <actions>
 * action ::= TGIR [ <subactions> ]
 * 
 * subactions ::= <subaction> | <subaction> <subactions>
 * subaction ::= <assert> | <assignment> | <index> [ <subactions> ]
 * 
 * assert ::= assert(<assert_item>:<res_name>)
 * assert_item ::= type
 * res_name ::= CRES | GZPS | 3IDR | ...
 * 
 * index ::= 0 ..
 * 
 * assignment ::= <item> = <value>
 * 
 * item ::= group | resource | instance | guid | <item_name>
 * 
 * value ::= <var_ref> | <string> | <constant>
 * 
 * var_ref ::= $<var_name>
 * var_name ::= text
 * 
 * string ::= "text with possible substitutions"
 * constant ::= 0x <hexdigits>
 * 
 */

namespace DbpfScripter
{
    public class DbpfScripterWorker
    {
        private static readonly Regex reTGIR = new Regex("^([0-9A-Z]+)-(0x[0-9A-Fa-f]{8})-(0x[0-9A-Fa-f]{8})-(0x[0-9A-Fa-f]{8})$");
        private static readonly Regex reCellRef = new Regex("^([a-z][+]?)([1-9][0-9]*[+]?)$");
        private static readonly Regex reIndex = new Regex("^([+]|[0-9]+)$");

        private readonly BackgroundWorker scriptWorker;

        private readonly string templatePath;
        private readonly string savePath;
        private readonly string baseName;

        private readonly bool isDevMode;

        private int countAssignments = 0;
        private int countResources = 0;
        private int countErrors = 0;

        private ParserState parserState;

        private readonly Stack<IDbpfScriptable> scriptableObjects = new Stack<IDbpfScriptable>();
        private IDbpfScriptable currentScriptableObject = null;

        private readonly Dictionary<string, DBPFFile> allEditedPackages = new Dictionary<string, DBPFFile>();
        private readonly HashSet<string> allSavedPackages = new HashSet<string>();
        private DBPFFile activePackage = null;

        private List<List<string>> odsRows = new List<List<string>>();

        private readonly Dictionary<string, string> variables = new Dictionary<string, string>();
        private readonly List<string> messages = new List<string>();

        private readonly Dictionary<string, string> generatedGuids = new Dictionary<string, string>();
        private readonly Dictionary<string, string> generatedGroups = new Dictionary<string, string>();

        private readonly Dictionary<string, ScriptValue> scriptConstants = new Dictionary<string, ScriptValue>();

        public DbpfScripterWorker(BackgroundWorker scriptWorker, string templatePath, string savePath, string baseName, bool isDevMode)
        {
            this.scriptWorker = scriptWorker;

            this.templatePath = templatePath;
            this.savePath = savePath;
            this.baseName = baseName;

            this.isDevMode = isDevMode;

            this.scriptConstants.Add("ddsutils", new ScriptValue(Sims2ToolsLib.Sims2DdsUtilsPath));
        }

        public void ProcessScript()
        {
            string scriptPath = $"{templatePath}/dbpfscript.txt";

            if (File.Exists(scriptPath))
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                ReportProgress($"Executing {scriptPath}");

                bool result = false;

                messages.Clear();

                generatedGuids.Clear();
                generatedGroups.Clear();

                using (StreamReader scriptStream = new StreamReader(scriptPath))
                {
                    parserState = new ParserState(this, scriptStream);
                    result = ProcessBlocks(0);
                }

                if (result)
                {
                    stopwatch.Stop();
                    ReportProgress($"Edits completed ({countAssignments} changes made to {countResources} resources in {stopwatch.ElapsedMilliseconds / 1000}.{stopwatch.ElapsedMilliseconds % 1000} seconds)");

                    foreach (string templateFileFullPath in Directory.GetFiles(templatePath, "*.package", SearchOption.AllDirectories))
                    {
                        string templateName = templateFileFullPath.Substring(templatePath.Length + 1);

                        string packageName = templateName.Replace("template", baseName).Replace("Template", baseName).Replace("TEMPLATE", baseName);

                        FileInfo fi = new FileInfo($"{savePath}/{packageName}");

                        Directory.CreateDirectory(fi.DirectoryName);

                        if (allEditedPackages.ContainsKey(templateName))
                        {
                            DBPFFile package = allEditedPackages[templateName];

                            ReportProgress($"Saving {templateName} into {packageName}");
                            package.SaveAs(fi.FullName);
                            package.Close();

                            allEditedPackages.Remove(templateName);
                        }
                        else
                        {
                            if (!allSavedPackages.Contains(templateName))
                            {
                                ReportProgress($"Copying {templateName} into {packageName}");

                                File.Copy(templateFileFullPath, fi.FullName, true);
                            }
                        }
                    }

                    foreach (string message in messages)
                    {
                        ReportProgress(message);
                    }

                    if (countErrors > 0)
                    {
                        ReportProgress($"!!Finished with {countErrors} error{(countErrors == 1 ? "" : "s")} :(");
                    }
                    else
                    {
                        ReportProgress("Finished :)");
                    }
                }
            }
        }

        #region BNF Based Parser
        private bool ProcessBlocks(int iteration)
        {
            while (ProcessBlock(iteration)) ;

            return TestEndOfScript() || (parserState.IsRepeating && TestEndOfBlock());
        }

        private bool ProcessBlock(int iteration)
        {
            if (TestEndOfScript()) return false;

            if (parserState.IsRepeating && TestEndOfBlock()) return false;

            string filename = ReadRepeatOrInitOrFilename();

            if (filename != null)
            {
                if (filename.Equals("REPEAT"))
                {
                    if (parserState.IsRepeating)
                    {
                        return ReportErrorFalse($"Nested REPEAT blocks are not supported");
                    }

                    string value = parserState.ReadNextToken();

                    if (value != null)
                    {
                        if (value[0] == '$')
                        {
                            string varValue = EvaluateVariable(value);

                            if (varValue == null)
                            {
                                return ReportErrorFalse($"Unknown variable {value}");
                            }

                            value = varValue;
                        }
                        else if (value.StartsWith("0x"))
                        {

                        }
                        else
                        {
                            return ReportErrorFalse("Unknown count (expected variable or constant)");
                        }

                        if (SkipOpenSquareBracket())
                        {
                            ReportProgress("");
                            ReportProgress($"Processing REPEAT");

                            int count = (int)(new ScriptValue(value));

                            bool processed = false;

                            parserState.StartRepeat();

                            for (int iter = 0; iter < count; ++iter)
                            {
                                processed = ProcessBlocks(iter);

                                if (!processed) break;

                                if (iter < (count - 1)) parserState.NextRepeat();
                            }

                            parserState.EndRepeat();

                            return processed && SkipCloseSquareBracket();
                        }
                    }
                }
                else
                {
                    string saveVar = null;

                    if (filename.EndsWith(".package"))
                    {
                        if (parserState.PeekNextToken()[0] == '$')
                        {
                            saveVar = parserState.ReadNextToken();
                        }
                    }

                    if (SkipOpenSquareBracket())
                    {
                        ReportProgress("");
                        ReportProgress($"Processing {filename}");

                        bool processed;

                        if (filename.Equals("INIT") || filename.EndsWith(".ods"))
                        {
                            processed = ProcessInitialisers(filename, iteration);
                        }
                        else if (filename.EndsWith(".package"))
                        {
                            processed = ProcessActions(filename, saveVar, iteration);
                        }
                        else
                        {
                            return ReportErrorFalse($"Unknown file extension {filename}");
                        }

                        return processed && SkipCloseSquareBracket();
                    }
                }
            }

            return ReportErrorFalse("Invalid BLOCK");
        }

        private bool ProcessInitialisers(string filename, int iteration)
        {
            if (!filename.Equals("INIT"))
            {
                if (!ParseODS($"{templatePath}\\{filename}")) return false;
            }

            while (ProcessInitialiser(iteration)) ;

            return TestEndOfBlock();
        }

        private bool ProcessInitialiser(int iteration)
        {
            if (TestEndOfBlock()) return false;

            string nextToken = parserState.PeekNextToken();

            if (nextToken.Equals("assert"))
            {
                return ProcessAssert();
            }
            else if (nextToken.Equals("message"))
            {
                return ProcessMessage();
            }
            else
            {
                string varDefn = ReadVarDefn();

                if (varDefn != null && SkipEqualsSign())
                {
                    string value = ReadVarValue(iteration);

                    if (value != null)
                    {
                        variables.Remove(varDefn);
                        variables.Add(varDefn, value);

                        ReportDevMode($"  ${varDefn} = {value}");

                        return true;
                    }
                }

                return ReportErrorFalse($"Invalid variable definition");
            }
        }

        private bool ProcessActions(string filename, string saveVar, int iteration)
        {
            if (isDevMode)
            {
                if (!(filename.Contains("template") || filename.Contains("Template") || filename.Contains("TEMPLATE")))
                {
                    ReportErrorNull($"File ({filename}) doesn't conform to required naming convention");
                }
            }

            if (allEditedPackages.ContainsKey(filename))
            {
                activePackage = allEditedPackages[filename];
            }
            else
            {
                activePackage = new DBPFFile($"{templatePath}/{filename}");
                allEditedPackages.Add(filename, activePackage);
            }

            while (ProcessAction(iteration)) ;

            if (saveVar != null)
            {
                string saveName = EvaluateVariable(saveVar);

                if (saveName != null)
                {
                    Directory.CreateDirectory(savePath);

                    ReportProgress($"Saving {filename} into {saveName}");
                    activePackage.SaveAs($"{savePath}\\{saveName}");
                    activePackage.Close();

                    allEditedPackages.Remove(filename);
                    allSavedPackages.Add(filename);
                }
                else
                {
                    return ReportErrorFalse($"{saveVar} does not evaluate to a file name");
                }
            }

            return TestEndOfBlock();
        }

        private bool ProcessAction(int iteration)
        {
            if (TestEndOfBlock()) return false;

            string tgir = ReadTGIR();

            if (tgir != null && SkipOpenSquareBracket())
            {
                ReportProgress($"Editing {tgir}");

                Match m = reTGIR.Match(tgir);

                if (m.Success)
                {
                    TypeTypeID typeId = DBPFData.TypeID(m.Groups[1].Value);
                    TypeGroupID groupId = (TypeGroupID)(uint)Int32.Parse(m.Groups[2].Value.Substring(2), System.Globalization.NumberStyles.HexNumber);
                    TypeResourceID resourceId = (TypeResourceID)(uint)Int32.Parse(m.Groups[3].Value.Substring(2), System.Globalization.NumberStyles.HexNumber);
                    TypeInstanceID instanceId = (TypeInstanceID)(uint)Int32.Parse(m.Groups[4].Value.Substring(2), System.Globalization.NumberStyles.HexNumber);

                    DBPFKey resKey = new DBPFKey(typeId, groupId, instanceId, resourceId);
                    DBPFResource res = activePackage.GetResourceByKey(resKey);

                    if (res is IDbpfScriptable scriptable)
                    {
                        ++countResources;

                        currentScriptableObject = scriptable;
                        scriptableObjects.Push(currentScriptableObject);

                        bool result = ProcessSubactions(iteration) && SkipCloseSquareBracket();

                        if (result)
                        {
                            activePackage.Remove(resKey);
                            activePackage.Commit(res, true);
                        }

                        scriptableObjects.Pop();

                        return result;
                    }
                    else
                    {
                        if (res == null)
                        {
                            return ReportErrorFalse($"{tgir} not found");
                        }
                        else
                        {
                            return ReportErrorFalse($"{tgir} is not scriptable");
                        }
                    }
                }
            }

            return ReportErrorFalse("Invalid ACTION");
        }

        private bool ProcessSubactions(int iteration)
        {
            while (ProcessSubaction(iteration)) ;

            return TestEndOfBlock();
        }

        private bool ProcessSubaction(int iteration)
        {
            if (TestEndOfBlock()) return false;

            string token = parserState.PeekNextToken();

            if (token.Equals("assert", StringComparison.OrdinalIgnoreCase))
            {
                return ProcessAssert();
            }
            else if (IsVariableDefn(token) || reIndex.IsMatch(token))
            {
                return ProcessIndexing(iteration);
            }
            else
            {
                return ProcessAssignment(iteration);
            }
        }

        private bool ProcessMessage()
        {
            string token = parserState.ReadNextToken();

            if (token != null && token.Equals("message") && SkipOpenBracket())
            {
                string message = parserState.ReadNextToken();

                if (message != null && SkipCloseBracket())
                {
                    message = EvaluateString(message);

                    if (message.StartsWith("\"") && message.EndsWith("\""))
                    {
                        message = message.Substring(1, message.Length - 2);
                    }

                    messages.Add(message);
                    return true;
                }
            }

            return ReportErrorFalse("Invalid MESSAGE");
        }

        private bool ProcessAssert()
        {
            string token = parserState.ReadNextToken();

            if (token != null && token.Equals("assert") && SkipOpenBracket())
            {
                string assertItem = parserState.ReadNextToken();

                if (assertItem != null && SkipColon())
                {
                    ScriptValue assertValue = new ScriptValue(parserState.ReadNextToken());

                    if (assertValue != null && SkipCloseBracket())
                    {
                        assertItem = assertItem.ToLower();

                        if (assertItem.Equals("tools"))
                        {
                            if (assertValue.ToString().Equals("ddsutils"))
                            {
                                bool ok = File.Exists($"{Sims2ToolsLib.Sims2DdsUtilsPath}\\nvdxt.exe");

                                if (!ok)
                                {
                                    return ReportErrorFalse($"Assert failed, {assertItem}:{assertValue} is NOT available");
                                }

                                ReportDebug($"  Asserted {assertItem}:{assertValue}");
                                return ok;
                            }
                            else
                            {
                                return ReportErrorFalse($"Assert failed, {assertItem} is NOT {assertValue}");
                            }
                        }
                        else
                        {
                            bool ok = currentScriptableObject.Assert(assertItem, assertValue);

                            if (!ok)
                            {
                                return ReportErrorFalse($"Assert failed, {assertItem} is NOT {assertValue}");
                            }

                            ReportDebug($"  Asserted {assertItem} is {assertValue}");
                            return ok;
                        }
                    }
                }
            }

            return ReportErrorFalse("Invalid ASSERT");
        }

        private bool ProcessAssignment(int iteration)
        {
            string item = parserState.ReadNextToken();

            if (item != null && SkipEqualsSign())
            {
                string value = parserState.ReadNextToken();

                if (value != null)
                {
                    if (value[0] == '"' || value[0] == '\'')
                    {
                        value = EvaluateString(value.Substring(1, value.Length - 2));

                        if (value == null) return false;
                    }
                    else if (value[0] == '$')
                    {
                        string varValue = EvaluateVariable(value);

                        if (varValue == null)
                        {
                            return ReportErrorFalse($"Unknown variable {value}");
                        }

                        value = varValue;
                    }
                    else if (value.StartsWith("0x"))
                    {

                    }
                    else if (IsFunctionName(value))
                    {
                        value = EvaluateFunction(value, iteration);

                        if (value == null) return ReportErrorFalse("Invalid function");
                    }
                    else
                    {
                        return ReportErrorFalse("Unknown assignment (expected string or variable)");
                    }

                    bool ok = currentScriptableObject.Assignment(item, new ScriptValue(value, scriptConstants));
                    if (ok)
                    {
                        ++countAssignments;
                        ReportDevMode($"  {item} = {value}");
                    }
                    else
                    {
                        ReportErrorFalse($"Invalid ASSIGNMENT {item} = {value}");
                    }
                    return ok;
                }
            }

            return ReportErrorFalse("Invalid ASSIGNMENT");
        }

        private bool ProcessIndexing(int iteration)
        {
            string index = parserState.ReadNextToken();

            if (index != null && SkipOpenSquareBracket())
            {
                if (IsVariableDefn(index))
                {
                    ReportDevMode($"Indexing from ${index}");
                    index = EvaluateVariable(index);
                }

                ReportDevMode($"Index [{index}]");

                if (reIndex.IsMatch(index))
                {
                    if (index.Equals("+"))
                    {
                        index = "-1";
                    }

                    try
                    {
                        IDbpfScriptable indexer = currentScriptableObject.Indexed(Int32.Parse(index));

                        if (indexer != null)
                        {
                            currentScriptableObject = indexer;
                            scriptableObjects.Push(currentScriptableObject);

                            bool result = ProcessSubactions(iteration) && SkipCloseSquareBracket();

                            scriptableObjects.Pop();
                            currentScriptableObject = scriptableObjects.Peek();

                            return result;
                        }
                    }
                    catch (Exception)
                    {
                        return ReportErrorFalse($"Invalid index {index}");
                    }
                }
            }

            return false;
        }
        #endregion

        #region Evaluation
        private string EvaluateVariable(string var)
        {
            string value = null;

            if (var.StartsWith("$")) var = var.Substring(1);

            if (var != null) variables.TryGetValue(var, out value);

            if (value == null)
            {
                if (var.Equals("SaveBaseName"))
                {
                    value = baseName;
                }
            }

            return value;
        }

        private string EvaluateFunction(string function, int iteration)
        {
            string value;

            // NOTE: If adding to the function list, must also update IsFunctionName in ParserState
            if (function.Equals("guid"))
            {
                if (SkipOpenBracket())
                {
                    value = null;
                    string tag = null;

                    if (!parserState.PeekNextToken().Equals(")"))
                    {
                        value = EvaluateVariable(ReadVarDefn());

                        if (parserState.PeekNextToken().Equals(","))
                        {
                            SkipComma();

                            tag = EvaluateVariable(ReadVarDefn());
                        }
                    }

                    if (SkipCloseBracket()) return NewGuid(value, tag);
                }
            }
            else if (function.Equals("group"))
            {
                if (SkipOpenBracket())
                {
                    value = null;
                    string tag = null;

                    if (!parserState.PeekNextToken().Equals(")"))
                    {
                        value = EvaluateVariable(ReadVarDefn());

                        if (parserState.PeekNextToken().Equals(","))
                        {
                            SkipComma();

                            tag = EvaluateVariable(ReadVarDefn());
                        }
                    }

                    if (SkipCloseBracket()) return NewGroup(value, tag);
                }
            }
            else if (function.Equals("family"))
            {
                if (SkipOpenBracket() && SkipCloseBracket()) return NewFamily();
            }
            else if (function.Equals("loword"))
            {
                if (SkipOpenBracket())
                {
                    string param = ReadVarDefn();

                    if (param != null && SkipCloseBracket())
                    {
                        value = EvaluateVariable(param);

                        if (value != null)
                        {
                            return Helper.Hex4PrefixString((new ScriptValue(value)).LoWord());
                        }
                    }
                }
            }
            else if (function.Equals("hiword"))
            {
                if (SkipOpenBracket())
                {
                    string param = ReadVarDefn();

                    if (param != null && SkipCloseBracket())
                    {
                        value = EvaluateVariable(param);

                        if (value != null)
                        {
                            return Helper.Hex4PrefixString((new ScriptValue(value)).HiWord());
                        }
                    }
                }
            }
            else if (function.Equals("lobyte"))
            {
                if (SkipOpenBracket())
                {
                    string param = ReadVarDefn();

                    if (param != null && SkipCloseBracket())
                    {
                        value = EvaluateVariable(param);

                        if (value != null)
                        {
                            return Helper.Hex2PrefixString((new ScriptValue(value)).LoByte());
                        }
                    }
                }
            }
            else if (function.Equals("hibyte"))
            {
                if (SkipOpenBracket())
                {
                    string param = ReadVarDefn();

                    if (param != null && SkipCloseBracket())
                    {
                        value = EvaluateVariable(param);

                        if (value != null)
                        {
                            return Helper.Hex2PrefixString((new ScriptValue(value)).HiByte());
                        }
                    }
                }
            }
            else if (function.Equals("ihash"))
            {
                if (SkipOpenBracket())
                {
                    string param = ReadVarDefn();

                    if (param != null && SkipCloseBracket())
                    {
                        value = EvaluateVariable(param);

                        if (value != null)
                        {
                            return Hashes.InstanceIDHash(value).ToString();
                        }
                    }
                }
            }
            else if (function.Equals("rhash"))
            {
                if (SkipOpenBracket())
                {
                    string param = ReadVarDefn();

                    if (param != null && SkipCloseBracket())
                    {
                        value = EvaluateVariable(param);

                        if (value != null)
                        {
                            return Hashes.ResourceIDHash(value).ToString();
                        }
                    }
                }
            }
            else if (function.Equals("fullpath"))
            {
                if (SkipOpenBracket())
                {
                    string param = ReadVarDefn();

                    if (param != null && SkipCloseBracket())
                    {
                        value = EvaluateVariable(param);

                        if (value != null)
                        {
                            string fullpath = $"{templatePath}\\{value}";

                            if (!File.Exists(fullpath))
                            {
                                fullpath = $"{savePath}\\{value}";

                                if (!File.Exists(fullpath))
                                {
                                    return ReportErrorNull($"{value} cannot be found");
                                }
                            }

                            return fullpath;
                        }
                    }
                }
            }
            else if (function.Equals("if"))
            {
                if (SkipOpenBracket())
                {
                    string param = ReadVarDefn();

                    value = EvaluateVariable(param);

                    if (value != null && SkipComma())
                    {
                        string exprT = ReadVarValue(iteration);

                        if (exprT != null && SkipComma())
                        {
                            string exprF = ReadVarValue(iteration);

                            if (exprF != null && SkipCloseBracket())
                            {
                                if ((new ScriptValue(value)).IsTrue)
                                {
                                    return exprT;
                                }
                                else
                                {
                                    return exprF;
                                }
                            }
                        }
                    }
                }
            }
            // NOTE: If adding to the function list, must also update IsFunctionName in ParserState

            return null;
        }

        private string EvaluateString(string codedString)
        {
            // For example "{gender:1}{agecode:1}_{type}_{basename}_{id}" -> "tf_body_sundress_red"
            string value = "";

            int braPos = codedString.IndexOf('{');

            while (braPos != -1)
            {
                bool lower = false;
                bool upper = false;

                value = $"{value}{codedString.Substring(0, braPos)}";
                codedString = codedString.Substring(braPos + 1);

                int ketPos = codedString.IndexOf('}');
                string macro = codedString.Substring(0, ketPos);
                int macroLen = -1;

                int colonPos = macro.IndexOf(':');
                if (colonPos != -1)
                {
                    string format = macro.Substring(colonPos + 1);
                    if (format.EndsWith("l") || format.EndsWith("U"))
                    {
                        lower = format.EndsWith("l");
                        upper = format.EndsWith("U");

                        format = format.Substring(0, format.Length - 1);
                    }

                    if (format.Length > 0)
                    {
                        int.TryParse(format, out macroLen);
                    }

                    macro = macro.Substring(0, colonPos);
                }

                string subst = EvaluateVariable(macro);

                if (subst == null)
                {
                    return ReportErrorNull($"Unknown variable {macro}");
                }

                if (macroLen > 0)
                {
                    subst = subst.Substring(0, macroLen);
                }

                if (lower) subst = subst.ToLower();
                if (upper) subst = subst.ToUpper();

                value = $"{value}{subst}";

                codedString = codedString.Substring(ketPos + 1);
                braPos = codedString.IndexOf('{');
            }

            return $"{value}{codedString}";
        }
        #endregion

        #region Tests for "end of ..."
        private bool TestEndOfScript()
        {
            string token = parserState.PeekNextToken();

            return (token == null);
        }

        private bool TestEndOfBlock()
        {
            string token = parserState.PeekNextToken();

            return (token != null && token.Equals("]"));
        }
        #endregion

        #region Tests for "is something"
        private bool IsFunctionName(string s)
        {
            return s.Equals("loword") || s.Equals("hiword") ||
                   s.Equals("lobyte") || s.Equals("hibyte") ||
                   s.Equals("guid") || s.Equals("group") || s.Equals("family") ||
                   s.Equals("rhash") || s.Equals("ihash") ||
                   s.Equals("fullpath") ||
                   s.Equals("if");
        }

        private bool IsVariableDefn(string s)
        {
            return s != null && s.StartsWith("$");
        }
        #endregion

        #region Skip expected tokens
        private bool SkipOpenSquareBracket() => SkipToken("[");
        private bool SkipCloseSquareBracket() => SkipToken("]");
        private bool SkipOpenBracket() => SkipToken("(");
        private bool SkipCloseBracket() => SkipToken(")");
        private bool SkipEqualsSign() => SkipToken("=");
        private bool SkipColon() => SkipToken(":");
        private bool SkipComma() => SkipToken(",");

        private bool SkipToken(string exectedToken)
        {
            string token = parserState.ReadNextToken();

            if (token != null && token.Equals(exectedToken))
            {
                return true;
            }

            return ReportErrorFalse($"Expected {exectedToken}");
        }
        #endregion

        #region Reads and Peeks
        private string ReadRepeatOrInitOrFilename()
        {
            string filename = parserState.ReadNextToken();

            if (filename.Equals("REPEAT")) return filename;

            if (filename.Equals("INIT")) return filename;

            if (filename != null && filename.StartsWith("\""))
            {
                if (filename.Length <= 2) return ReportErrorNull($"Invalid file name ({filename})");

                filename = filename.Substring(1, filename.Length - 2);
            }

            if (!File.Exists($"{templatePath}/{filename}")) return ReportErrorNull($"File ({filename}) doesn't exist");

            return filename;
        }

        private string ReadTGIR()
        {
            string tgir = parserState.ReadNextToken();

            if (tgir != null && reTGIR.IsMatch(tgir)) return tgir;

            return ReportErrorNull($"TGIR expected");
        }

        private string ReadVarDefn()
        {
            string varDefn = parserState.ReadNextToken();

            if (IsVariableDefn(varDefn)) return varDefn.Substring(1);

            return ReportErrorNull($"Variable definition expected");
        }

        private string ReadVarValue(int iteration)
        {
            string value = null;

            string nextToken = parserState.PeekNextToken();
            if (nextToken.ToLower().StartsWith("0x"))
            {
                value = parserState.ReadNextToken();
            }
            else if (nextToken[0] == '"' || nextToken[0] == '\'')
            {
                value = parserState.ReadNextToken();

                value = EvaluateString(value.Substring(1, value.Length - 2));
            }
            else
            {
                string funct = ReadFunctionOrCellRef();

                if (funct != null)
                {
                    if (IsFunctionName(funct))
                    {
                        value = EvaluateFunction(funct, iteration);
                    }
                    else
                    {
                        Match m = reCellRef.Match(funct);

                        if (m.Success)
                        {
                            string colCode = m.Groups[1].Value;
                            string rowCode = m.Groups[2].Value;

                            int col = colCode.ToCharArray()[0] - 'a';
                            if (colCode.EndsWith("+"))
                            {
                                col += iteration;
                            }

                            string rowNum = rowCode.EndsWith("+") ? rowCode.Substring(0, rowCode.Length - 1) : rowCode;
                            int row = Int16.Parse(rowNum) - 1;
                            if (rowCode.EndsWith("+"))
                            {
                                row += iteration;
                            }

                            if (odsRows.Count > row)
                            {
                                List<string> odsRow = odsRows[row];

                                if (odsRow.Count > col)
                                {
                                    value = odsRow[col];

                                    // ReportDebug($"Cell {funct} is {value}");
                                }
                            }
                        }
                    }
                }
            }

            return value;
        }

        private string ReadFunctionOrCellRef()
        {
            string function = parserState.ReadNextToken();

            if (function != null) return function.ToLower();

            return ReportErrorNull($"Function or cell reference expected");
        }
        #endregion

        #region ODS Parsing
        private const int MAX_ROW_REPEAT = 25;
        private const int MAX_CELL_REPEAT = 25;

        private bool ParseODS(string fullPath)
        {
            odsRows = new List<List<string>>();
            List<string> row = null;

            int rowRepeat = 0;

            int cellRepeat = 0;
            string cellText = "";
            bool wantText = false;

            try
            {
                using (ZipArchive zip = ZipFile.Open(fullPath, ZipArchiveMode.Read))
                {
                    ZipArchiveEntry content = zip.GetEntry("content.xml");

                    if (content != null)
                    {
                        ReportDebug($"Reading {fullPath}");

                        XmlReader reader = XmlReader.Create(content.Open());

                        reader.MoveToContent();
                        while (reader.Read())
                        {
                            if (reader.IsStartElement())
                            {
                                if (reader.Name.Equals("table:table"))
                                {
                                    odsRows = new List<List<string>>();
                                }
                                else if (reader.Name.Equals("table:table-row"))
                                {
                                    // Process the previous row when we encounter the next one in the table, this stops us processing the last row (which repeats many, many times)
                                    if (row != null)
                                    {
                                        while (rowRepeat-- > 0)
                                        {
                                            odsRows.Add(row);
                                        }
                                    }

                                    string repeat = reader.GetAttribute("table:number-rows-repeated");
                                    rowRepeat = (repeat == null) ? 1 : Int32.Parse(repeat);

                                    row = new List<string>();

                                    cellRepeat = 0;
                                }
                                else if (reader.Name.Equals("table:table-cell"))
                                {
                                    if (cellText.Length >= 2) cellText = cellText.Substring(2);

                                    // Process the previous cell when we encounter the next one in the row, this stops us processing the last cell (which repeats many, many times)
                                    while (cellRepeat-- > 0)
                                    {
                                        row.Add(cellText);
                                    }

                                    string repeat = reader.GetAttribute("table:number-columns-repeated");
                                    cellRepeat = (repeat == null) ? 1 : Int32.Parse(repeat);
                                    cellText = "";
                                    wantText = !reader.IsEmptyElement;
                                }
                                else if (reader.Name.Equals("text:p"))
                                {
                                    // This all gets confusing, as a <text:p> node can contain <text:s> nodes and/or can be followed by <text:p/> nodes,
                                    // all of which confuse XmlReader's Read() methods!

                                    // Don't use reader.ReadElementContentAsString(); as this skips empty elements, eg <text:p/> after this node
                                    // Don't use reader.ReadString(); as this skips inner elemens, eg <text:s/> within this node
                                    // Don't use reader.ReadInnerXml(); as this skips empty elements, eg <text:p/> after this node

                                    // Just add the CRLF here, the actual text will be added from the Text node below
                                    cellText = $"{cellText}\r\n";
                                }
                                else if (reader.Name.Equals("text:s"))
                                {
                                    int count = int.Parse(reader.GetAttribute("text:c") ?? "1");
                                    string spaces = new string(' ', count);
                                    cellText = $"{cellText}{spaces}";
                                }
                            }
                            else if (reader.NodeType == XmlNodeType.Text)
                            {
                                if (wantText)
                                {
                                    Trace.Assert(reader.HasValue, "Expected a text node to have a value!");

                                    // Don't use reader.ReadContentAsString(); as this skips empty elements, eg <text:s/> after this text
                                    cellText = $"{cellText}{reader.Value}";
                                }
                            }
                            else if (reader.NodeType == XmlNodeType.EndElement)
                            {
                                if (reader.Name.Equals("table:table-cell"))
                                {
                                    wantText = false;
                                }
                                if (reader.Name.Equals("table:table-row"))
                                {
                                    if (cellText.Length >= 2) cellText = cellText.Substring(2);

                                    if (cellRepeat > 0 && cellRepeat < MAX_CELL_REPEAT)
                                    {
                                        while (cellRepeat-- > 0)
                                        {
                                            row.Add(cellText);
                                        }
                                    }
                                }
                                else if (reader.Name.Equals("table:table"))
                                {
                                    if (row != null && rowRepeat < MAX_ROW_REPEAT)
                                    {
                                        while (rowRepeat-- > 0)
                                        {
                                            odsRows.Add(row);
                                        }
                                    }

                                    // Only process the first table in the first spreadsheet
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReportDebug(ex.Message);
                return ReportErrorFalse($"Unable to open/read {fullPath}");
            }

            return ReportErrorFalse($"Unable to parse {fullPath}");
        }
        #endregion

        #region New (Random) generators
        private string NewGuid(string guid, string tag)
        {
            if (guid != null && !string.IsNullOrWhiteSpace(guid))
            {
                return guid;
            }
            else
            {
                string newGuid;

                if (tag != null && generatedGuids.TryGetValue(tag, out newGuid))
                {
                    return newGuid;
                }

                ReportDebug($"GENERATING Guid");
                newGuid = TypeGUID.RandomID.ToString().ToUpper();

                if (tag != null)
                {
                    generatedGuids.Add(tag, newGuid);
                }

                return newGuid;
            }
        }

        private string NewGroup(string group, string tag)
        {
            if (group != null && !string.IsNullOrWhiteSpace(group))
            {
                return group;
            }
            else
            {
                string newGroup;

                if (tag != null && generatedGroups.TryGetValue(tag, out newGroup))
                {
                    return newGroup;
                }

                ReportDebug($"GENERATING Group");
                newGroup = TypeGroupID.RandomID.ToString().ToUpper();

                if (tag != null)
                {
                    generatedGroups.Add(tag, newGroup);
                }

                return newGroup;
            }
        }

        private string NewFamily()
        {
            ReportDebug($"GENERATING Family");

            return Guid.NewGuid().ToString();
        }
        #endregion

        #region Error Reporting
        internal string ReportErrorNull(string msg)
        {
            ReportErrorFalse(msg);

            return null;
        }

        internal bool ReportErrorFalse(string msg)
        {
            ReportProgress($"!!{msg} at line {parserState.ScriptLineIndex}");

            ++countErrors;

            return false;
        }

        internal void ReportDevMode(string msg)
        {
#if DEBUG
            ReportProgress($"--{msg}");
#else
            if (isDevMode) ReportProgress($"--{msg}");
#endif
        }

        internal void ReportDebug(string msg)
        {
#if DEBUG
            ReportProgress($"--{msg}");
#endif
        }

        internal void ReportProgress(string msg)
        {
            if (msg != null)
            {
                scriptWorker.ReportProgress(0, msg);
            }
        }
        #endregion
    }
}
