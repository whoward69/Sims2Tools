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


using Sims2Tools.DBPF;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
 * blocks ::= <block> | <block> | <blocks>
 * block ::= INIT [ <initialisers> ] | filename.ods [ <initialisers> ] | filename.package [ <actions> ]
 * 
 * initialisers ::= <initialiser> | <initialiser> <initialisers>
 * initialiser ::= <var_defn> = <function> | <cell_ref> | <string> | <constant>
 * 
 * var_defn ::= $<var_name>
 * 
 * function ::= <function_name>(<var_defn>)
 * function_name ::= guid | group | family | loword | hiword | lobyte | hibyte
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
        private static readonly Regex reCellRef = new Regex("^([a-z])([1-9][0-9]*)$");
        private static readonly Regex reIndex = new Regex("^[0-9]*$");

        private readonly BackgroundWorker scriptWorker;

        private readonly string templatePath;
        private readonly string savePath;
        private readonly string baseName;

        private readonly bool isDevMode;

        private int countAssignments = 0;
        private int countResources = 0;
        private int countErrors = 0;

        private TextReader scriptReader;
        private int scriptLineIndex = 0;
        private string scriptLine = null;
        private string nextToken = null;

        private readonly Stack<IDbpfScriptable> scriptableObjects = new Stack<IDbpfScriptable>();
        private IDbpfScriptable currentScriptableObject = null;

        private readonly Dictionary<string, DBPFFile> allEditedPackages = new Dictionary<string, DBPFFile>();
        private DBPFFile activePackage = null;

        private List<List<string>> odsRows = new List<List<string>>();

        private readonly Dictionary<string, string> variables = new Dictionary<string, string>();

        public DbpfScripterWorker(BackgroundWorker scriptWorker, string templatePath, string savePath, string baseName, bool isDevMode)
        {
            this.scriptWorker = scriptWorker;

            this.templatePath = templatePath;
            this.savePath = savePath;
            this.baseName = baseName;

            this.isDevMode = isDevMode;
        }

        public void ProcessScript()
        {
            string scriptPath = $"{templatePath}/dbpfscript.txt";

            if (File.Exists(scriptPath))
            {
                ReportProgress($"Executing {scriptPath}");

                bool result = false;

                using (scriptReader = new StreamReader(scriptPath))
                {
                    result = ProcessBlocks();
                }

                if (result)
                {
                    ReportProgress($"Edits completed ({countAssignments} changes made to {countResources} resources)");

                    foreach (string templateFileFullPath in Directory.GetFiles(templatePath, "*.package", SearchOption.AllDirectories))
                    {
                        string templateName = templateFileFullPath.Substring(templatePath.Length + 1);

                        string packageName = templateName.Replace("template", baseName).Replace("Template", baseName).Replace("TEMPLATE", baseName);

                        FileInfo fi = new FileInfo($"{savePath}/{packageName}");

                        ReportProgress($"Saving {templateName} into {packageName}");

                        Directory.CreateDirectory(fi.DirectoryName);

                        if (allEditedPackages.ContainsKey(templateName))
                        {
                            DBPFFile package = allEditedPackages[templateName];

                            package.SaveAs(fi.FullName);
                            package.Close();

                            allEditedPackages.Remove(templateName);
                        }
                        else
                        {
                            File.Copy(templateFileFullPath, fi.FullName, true);
                        }
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
        private bool ProcessBlocks()
        {
            while (ProcessBlock()) ;

            return TestEndOfScript();
        }

        private bool ProcessBlock()
        {
            if (TestEndOfScript()) return false;

            string filename = ReadInitOrFilename();

            if (filename != null && SkipOpenSquareBracket())
            {
                ReportProgress("");
                ReportProgress($"Processing {filename}");

                bool processed;

                if (filename.Equals("INIT") || filename.EndsWith(".ods"))
                {
                    processed = ProcessInitialisers(filename);
                }
                else if (filename.EndsWith(".package"))
                {
                    processed = ProcessActions(filename);
                }
                else
                {
                    return ReportErrorFalse($"Unknown file extension {filename}");
                }

                return processed && SkipCloseSquareBracket();
            }

            return ReportErrorFalse("Invalid BLOCK");
        }

        private bool ProcessInitialisers(string filename)
        {
            if (!filename.Equals("INIT"))
            {
                if (!ParseODS($"{templatePath}\\{filename}")) return false;
            }

            while (ProcessInitialiser()) ;

            return TestEndOfBlock();
        }

        private bool ProcessInitialiser()
        {
            if (TestEndOfBlock()) return false;

            string varDefn = ReadVarDefn();
            string value = null;

            if (varDefn != null && SkipEqualsSign())
            {
                string nextToken = PeekNextToken();
                if (nextToken.ToLower().StartsWith("0x"))
                {
                    value = ReadNextToken();
                }
                else if (nextToken[0] == '"' || nextToken[0] == '\'')
                {
                    value = ReadNextToken();

                    value = EvaluateString(value.Substring(1, value.Length - 2));
                }
                else
                {
                    string funct = ReadFunctionOrCellRef();

                    if (funct != null)
                    {
                        if (IsFunctionName(funct))
                        {
                            value = EvaluateFunction(funct);
                        }
                        else
                        {
                            Match m = reCellRef.Match(funct);

                            if (m.Success)
                            {
                                int col = m.Groups[1].Value.ToCharArray()[0] - 'a';
                                int row = Int16.Parse(m.Groups[2].Value) - 1;

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

        private bool ProcessActions(string filename)
        {
            if (isDevMode)
            {
                if (!(filename.Contains("template") || filename.Contains("Template") || filename.Contains("TEMPLATE")))
                {
                    ReportErrorNull($"File ({filename}) doesn't conform to required naming convention");
                }
            }

            activePackage = new DBPFFile($"{templatePath}/{filename}");
            allEditedPackages.Add(filename, activePackage);

            while (ProcessAction()) ;

            return TestEndOfBlock();
        }

        private bool ProcessAction()
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

                        bool result = ProcessSubactions() && SkipCloseSquareBracket();

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

        private bool ProcessSubactions()
        {
            while (ProcessSubaction()) ;

            return TestEndOfBlock();
        }

        private bool ProcessSubaction()
        {
            if (TestEndOfBlock()) return false;

            string token = PeekNextToken();

            if (token.Equals("assert", StringComparison.OrdinalIgnoreCase))
            {
                return ProcessAssert();
            }
            else if (reIndex.IsMatch(token))
            {
                return ProcessIndexing();
            }
            else
            {
                return ProcessAssignment();
            }
        }

        private bool ProcessAssert()
        {
            string token = ReadNextToken();

            if (token != null && token.Equals("assert") && SkipOpenBracket())
            {
                string assertItem = ReadNextToken();

                if (assertItem != null && SkipColon())
                {
                    ScriptValue assertValue = new ScriptValue(ReadNextToken());

                    if (assertValue != null && SkipCloseBracket())
                    {
                        assertItem = assertItem.ToLower();

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

            return ReportErrorFalse("Invalid ASSERT");
        }

        private bool ProcessAssignment()
        {
            string item = ReadNextToken();

            if (item != null && SkipEqualsSign())
            {
                string value = ReadNextToken();

                if (value != null)
                {
                    if (value[0] == '"' || value[0] == '\'')
                    {
                        value = EvaluateString(value.Substring(1, value.Length - 2));

                        if (value == null) return false;
                    }
                    else if (value[0] == '$')
                    {
                        if (!variables.TryGetValue(value.Substring(1), out string varValue))
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
                        value = EvaluateFunction(value);

                        if (value == null) return ReportErrorFalse("Invalid function");
                    }
                    else
                    {
                        return ReportErrorFalse("Unknown assignment (expected string or variable)");
                    }

                    bool ok = currentScriptableObject.Assignment(item, new ScriptValue(value));
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

        private bool ProcessIndexing()
        {
            string index = ReadNextToken();

            if (index != null && reIndex.IsMatch(index) && SkipOpenSquareBracket())
            {
                ReportDevMode($"Index [{index}]");

                try
                {
                    IDbpfScriptable indexer = currentScriptableObject.Indexed(Int32.Parse(index));

                    if (indexer != null)
                    {
                        currentScriptableObject = indexer;
                        scriptableObjects.Push(currentScriptableObject);

                        bool result = ProcessSubactions() && SkipCloseSquareBracket();

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

            return false;
        }
        #endregion

        #region Evaluation
        private string EvaluateFunction(string function)
        {
            string value;

            if (function.Equals("guid"))
            {
                if (SkipOpenBracket() && SkipCloseBracket()) return NewGuid();
            }
            else if (function.Equals("group"))
            {
                if (SkipOpenBracket() && SkipCloseBracket()) return NewGroup();
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
                        if (variables.TryGetValue(param, out value))
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
                        if (variables.TryGetValue(param, out value))
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
                        if (variables.TryGetValue(param, out value))
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
                        if (variables.TryGetValue(param, out value))
                        {
                            return Helper.Hex2PrefixString((new ScriptValue(value)).HiByte());
                        }
                    }
                }
            }

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

                if (!variables.TryGetValue(macro, out string subst))
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
                            if (reader.NodeType == XmlNodeType.Element)
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
                                    // Process the previous cell when we encounter the next one in the row, this stops us processing the last cell (which repeats many, many times)
                                    while (cellRepeat-- > 0)
                                    {
                                        row.Add(cellText);
                                    }

                                    string repeat = reader.GetAttribute("table:number-columns-repeated");
                                    cellRepeat = (repeat == null) ? 1 : Int32.Parse(repeat);
                                    cellText = "";
                                }
                                else if (reader.Name.Equals("text:p"))
                                {
                                    cellText = reader.ReadElementContentAsString();
                                }
                            }
                            else if (reader.NodeType == XmlNodeType.EndElement)
                            {
                                if (reader.Name.Equals("table:table-row"))
                                {
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
        private string NewGuid()
        {
            ReportDebug($"GENERATING Guid");

            return TypeGUID.RandomID.ToString().ToUpper();
        }

        private string NewGroup()
        {
            ReportDebug($"GENERATING Group");

            return TypeGroupID.RandomID.ToString().ToUpper();
        }

        private string NewFamily()
        {
            ReportDebug($"GENERATING Family");

            return Guid.NewGuid().ToString();
        }
        #endregion

        #region Tests for "end of ..."
        private bool TestEndOfScript()
        {
            string token = PeekNextToken();

            return (token == null);
        }

        private bool TestEndOfBlock()
        {
            string token = PeekNextToken();

            return (token != null && token.Equals("]"));
        }
        #endregion

        #region Skip expected tokens
        private bool SkipOpenSquareBracket() => SkipToken("[");
        private bool SkipCloseSquareBracket() => SkipToken("]");
        private bool SkipOpenBracket() => SkipToken("(");
        private bool SkipCloseBracket() => SkipToken(")");
        private bool SkipEqualsSign() => SkipToken("=");
        private bool SkipColon() => SkipToken(":");

        private bool SkipToken(string exectedToken)
        {
            string token = ReadNextToken();

            if (token != null && token.Equals(exectedToken))
            {
                return true;
            }

            return ReportErrorFalse($"Expected {exectedToken}");
        }
        #endregion

        #region Reads and Peeks
        private string ReadInitOrFilename()
        {
            string filename = ReadNextToken();

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
            string tgir = ReadNextToken();

            if (tgir != null && reTGIR.IsMatch(tgir)) return tgir;

            return ReportErrorNull($"TGIR expected");
        }

        private string ReadVarDefn()
        {
            string varDefn = ReadNextToken();

            if (varDefn != null && varDefn.StartsWith("$")) return varDefn.Substring(1);

            return ReportErrorNull($"Variable definition expected");
        }

        private string ReadFunctionOrCellRef()
        {
            string function = ReadNextToken();

            if (function != null) return function.ToLower();

            return ReportErrorNull($"Function or cell reference expected");
        }

        private string ReadNextToken()
        {
            string token = null;

            if (nextToken != null)
            {
                token = nextToken;
                nextToken = null;
            }
            else
            {
                if (scriptLine == null || scriptLine.Length == 0 || scriptLine.StartsWith("//"))
                {
                    scriptLine = ReadNextLine();
                }

                if (scriptLine != null)
                {
                    if (IsSpecialChar(scriptLine[0]))
                    {
                        token = scriptLine.Substring(0, 1);
                    }
                    else if (scriptLine[0] == '"' || scriptLine[0] == '\'')
                    {
                        int pos = 1;

                        while (pos < scriptLine.Length && (scriptLine[pos] != scriptLine[0]))
                        {
                            ++pos;
                        }

                        if (pos >= scriptLine.Length) return ReportErrorNull($"Missing closing {scriptLine[0]}");

                        token = scriptLine.Substring(0, pos + 1);
                    }
                    else
                    {
                        int pos = 0;

                        char c = scriptLine[pos++];

                        while ((pos < scriptLine.Length) && !IsSpaceOrSpecialChar(c))
                        {
                            c = scriptLine[pos++];
                        }

                        if (pos == scriptLine.Length && !IsSpecialChar(c))
                        {
                            token = scriptLine;
                        }
                        else
                        {
                            token = scriptLine.Substring(0, pos - 1);
                        }
                    }

                    scriptLine = scriptLine.Substring(token.Length).TrimStart();
                }
            }

            if (token != null && token.Equals(";")) token = ReadNextToken();

            return token;
        }

        private string ReadNextLine()
        {
            string line = scriptReader.ReadLine();
            ++scriptLineIndex;

            if (line != null)
            {
                line = line.Trim();

                if (line.Length == 0 || line.StartsWith("//"))
                {
                    line = ReadNextLine();
                }
            }

            return line;
        }

        private string PeekNextToken()
        {
            if (nextToken == null)
            {
                nextToken = ReadNextToken();
            }

            return nextToken;
        }

        private bool IsSpecialChar(char c)
        {
            return (c == '[' || c == ']' || c == '(' || c == ')' || c == ':' || c == '=' || c == ';');
        }

        private bool IsSpaceOrSpecialChar(char c)
        {
            return (c == ' ' || IsSpecialChar(c));
        }

        private bool IsFunctionName(string s)
        {
            return s.Equals("loword") || s.Equals("hiword") || s.Equals("lobyte") || s.Equals("hibyte") || s.Equals("guid") || s.Equals("group") || s.Equals("family");
        }
        #endregion

        #region Error Reporting
        private string ReportErrorNull(string msg)
        {
            ReportErrorFalse(msg);

            return null;
        }

        private bool ReportErrorFalse(string msg)
        {
            ReportProgress($"!!{msg} at line {scriptLineIndex}");

            ++countErrors;

            return false;
        }

        private void ReportDevMode(string msg)
        {
#if DEBUG
            ReportProgress($"--{msg}");
#else
            if (isDevMode) ReportProgress($"--{msg}");
#endif
        }

        private void ReportDebug(string msg)
        {
#if DEBUG
            ReportProgress($"--{msg}");
#endif
        }

        private void ReportProgress(string msg)
        {
            if (msg != null)
            {
                scriptWorker.ReportProgress(0, msg);
            }
        }
        #endregion
    }
}
