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

#region dbpfscript.txt details
/*
 * General notes on the dbpfscript.txt file
 * 
 * The parser is based on the following Backus–Naur form, with the additional meanings
 *   (x | y) - either x or y, more than two options are permitted
 *   (x)+ - one or more occurances of x (or y)
 *   (x)? - x (or y) is optional (zero or one occurance of x (or y))
 *   (x)N - x occurs exactly N times
 * 
 * The parser
 *   ignores leading white space
 *   ignores everything after // to the end of the line
 *   ignore blank lines
 *   ignores white space outside of quote delimited strings
 *   treats a semi-colon (;) as "syntatic sugar"
 * 
 * -- A script is one or more init, ods, repeat, template or end blocks
 * script ::= <blocks>
 * blocks ::= (<block> | <block> <blocks>)
 * block  ::= (<initBlock> | <odsBlock> | <templateBlock> | <repeatBlock> | <endBlock>)
 * 
 * -- A REPEAT block causes the enclosed blocks to be executed multiple times
 * repeatBlock ::= REPEAT (<varRef> | <hexConstant>) [ (<initBlock> | <odsBlock> | <templateBlock>)+ ]
 * 
 * -- The END block marks the end of the script before the end of the file
 * endBlock ::= END
 * 
 * -- Init blocks set values to be used later in the script
 * initBlock ::= INIT [ (<initContents>)+ ]
 * odsBlock ::= <odsName> [ (<initContents>)+ ]
 * odsName ::= "<fileName>.ods"
 * initContents ::= (<assert> | <comment> | <message> | <initialiser>)
 * 
 * -- For assert, see subactions below
 * 
 * -- Comments display a text string to the user DURING the script's execution
 * comment ::= comment BRA <string> KET
 * 
 * -- Messages display a text string to the user AT THE END OF the script's execution
 * message ::= message BRA <string> KET
 * 
 * -- Initialisers define a named variable and its initial value
 * initialiser ::= <varDefn> = <varValue>
 * varDefn ::= $<varName>
 * varValue ::= (<cellRef> | <varRef> | <string> | <hexConstant> | <function>)
 * 
 * -- A cell referenced in the current spreadsheet (not valid in INIT blocks).
 * -- Within an enclosing REPEAT block, the format C10+ will reference the cell C10 on the first iteration, 
 * -- C11 on the second, C12 on the third, etc
 * cellRef ::= <colRef> (PLUS)? <rowRef> (PLUS)?
 * colRef ::= <colChar> (<colChar>)?
 * rowRef ::= <rowNumFirst> (<rowNumFollowing>)+
 * colChar ::= (A | B | ... | Y | Z)
 * rowNumFirst ::= (1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9)
 * rowNumFollowing ::= (0 | <rowNumFirst>)
 * 
 * -- Template blocks define which resources are to be edited within the specified template (.package) file
 * templateBlock ::= <templateDefn> [ <actions> ]
 * templateDefn ::= <templateName> (<saveRef>)? (<conditional>)?
 * templateName ::= "<fileName>.package"
 * saveRef :: <varRef>
 * 
 * -- Conditionals apply to template blocks (see above), delete actions and TGRI actions (see below)
 * conditional ::= IF (NOT)? <condition>
 * condition ::= <varRef>
 * 
 * -- Actions are the workers of the script, making changes to resources
 * actions ::= (<action> | <action> <actions>)
 * action ::= (<initBlock> | <deleteAction> | <cloneAction> | <tgriAction>)
 * 
 * -- Delete actions delete the given resource (if the condition is true) from the template (.package) file
 * deleteAction ::= DELETE (<conditional>)? <tgri>
 * 
 * -- Clone actions copy the given resource before applying the changes, 
 * -- one of the changes should alter (at least one of) the G, R or I values
 * cloneAction ::= CLONE <tgriAction>
 * 
 * -- TGRI actions edit the specified resource
 * tgriAction ::= <tgri> (<conditional>)? [ <subactions> ]
 * 
 * -- TGRI (TypeName-GroupId-ResourceId-InstanceId) uniquely references a resource in the template (.package) file
 * tgri ::= <typeNameScriptable> - <groupId> - <resourceId> - <instanceId>
 * typeNameAll ::= (<typeNameScriptable> | <typeNameOther>)
 * typeNameScriptable ::= (BCON | BHAV | AGED | BINX | COLL | GZPS | MMAT | SDNA | VERS | XFCH | XFLR | XFNC | XHTN | XMOL | XOBJ | XROF | XSTN | XTOL | XWNT | 3IDR | NREF | OBJD | ANIM | CINE | CRES | GMDC | GMND | LAMB | LDIR | LPNT | LSPT | LIFO | SHPE | TXMT | TXTR | STR | CTSS | TTAS | TRCN | TTAB)
 * typeNameOther ::= {any other resource type name supported by the DBPF Library}
 * groupId ::= <hexConstant8>
 * resourceId ::= <hexConstant8>
 * instanceId ::= <hexConstant8>
 * 
 * -- Subactions are either asserts, assignments or indexing
 * subactions ::= (<subaction> | <subaction> <subactions>)
 * subaction ::= <assert> | <assignment> | <indexing>
 * 
 * -- Asserts confirm that the item has the given value, if not, the script is aborted
 * assert ::= assert BRA <assertItem> : <assertValue> KET
 * assertItem ::= (tools | version | type | group | resource | instance | filename | opcode)
 * assertValue ::= (ddsutils | <typeNameAll> | <hexConstant> | (<char>)+)
 * 
 * -- Assignments change the value of a property or item within the current resource being edited
 * -- Possible properties/items are dependant on the current resource being edited
 * assignment ::= <item> = <value>
 * item ::= (<string> | <name>)		// Where <string> must evaluate to a <name>
 * name ::= (type | group | instance | resource | filename | <objdName> | <cpfName> | <strName> | <bconName> | <trcnName> | <bhavName> | <ttabName> | <txtrName> | <txmtName> | <shpeName>)
 * objdName ::= (guid | {any objd index name - see ObjdIndex.cs})
 * cpfName ::= {any valid cpf property name; if the property does not exist, it will be created}
 * strName ::= (text | title | desc)
 * bconName ::= value
 * trcnName ::= label
 * bhavName ::= operand
 * ttabName ::= (stringid | action | guardian | flags | flags2)
 * txtrName ::= image
 * txmtName ::= {any valid txmt property name}
 * shpeName ::= (item | {any current subset name})
 * value ::= <varRef> | <string> | <hexConstant> | <function>
 * 
 * -- Indexing
 * indexing ::= <index> (CLONE)? [ (<subaction>)+ ]
 * index ::= (<varRef> | <indexKey>)	// Where <varRef> must evaluate to an <indexKey>
 * indexKey ::= (PLUS | (<digit>)+)
 * 
 * -- Functions for performing specific operations
 * function ::= <fnWord> | <fnByte> | <fnGuid> | <fnGroup> | <fnFamily> | <fnHash> | <fnPath> | <fnIf> | <fnInc>
 * fnWord ::= (loword | hiword) BRA <param> KET
 * fnByte ::= (lobyte | hibyte) BRA <param> KET
 * fnGuid ::= guid BRA (<param> (, <tag>)?)? KET
 * fngroup ::= group BRA (<param> (, <tag>)?)? KET
 * fnFamily ::= family BRA KET
 * fnHash ::= (ghash | rhash | ihash) BRA <param> KET
 * fnPath ::= fullpath BRA <param> KET
 * fnIf ::= if BRA <condition> , <varValue> , <varValue> KET
 * fnInc ::= (preinc | postinc) BRA <param> KET
 * param ::= <varRef>
 * tag ::= <varRef>
 * 
 * -- Filename without an extension
 * fileName ::= ({any valid filename character})+
 * 
 * -- Variable references start with a $ sign
 * varRef ::= $<varName>
 * varName ::= (<normalChar>)+
 * 
 * -- Double quote delimited text string; macro replacement is supported by {var:format}
 * string ::= "<text>"
 * text ::= (<char> | <macro>)+
 * macro ::= { ({ | <varName> (:<format>)?) }		// To get a { in a string, use {{}
 * format ::= (<digit>)+ (<caseSpecifier>)?
 * caseSpecifier ::= (l | U)
 * 
 * -- Hex constants start 0x; with the exception of TGRI identifiers, hex constants can have between 1 and 8 hex digits
 * hexConstant8 ::= <hexPrefix>(<hexDigit>)8
 * hexConstant ::= <hexPrefix>(<hexDigit>)+
 * hexPrefix ::= 0x
 * hexDigit ::= (<digit> | A | B | C | D | E | F | a | b | c | d | e | f)
 * 
 * -- Terminals
 * BRA ::= (
 * KET ::= )
 * PLUS ::= +
 * digit ::= (0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9)
 * char ::= {any valid character for the current context}
 * specialChar ::= ( BRA | KET | [ | ] | : | = | ; | ,)
 * normalChar ::= {any valid character other than the <specialChar>s or space}
 * 
 */
#endregion

namespace DbpfScripter
{
    public class DbpfScripterWorker
    {
        private static readonly Regex reVersion = new Regex("^([0-9]+)\\.([0-9]+)$");

        private static readonly Regex reTGIR = new Regex("^([0-9A-Z]+)-(0x[0-9A-Fa-f]{8})-(0x[0-9A-Fa-f]{8})-(0x[0-9A-Fa-f]{8})$");
        private static readonly Regex reCellRef = new Regex("^([a]?[a-z][+]?)([1-9][0-9]*[+]?)$");
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

        private readonly HashSet<string> allOpenedTemplates = new HashSet<string>();
        private readonly Dictionary<string, DBPFFile> allEditedPackages = new Dictionary<string, DBPFFile>();
        private readonly HashSet<string> allSavedPackages = new HashSet<string>();
        private DBPFFile activePackage = null;

        private readonly Dictionary<string, List<List<string>>> odsCache = new Dictionary<string, List<List<string>>>();
        private List<List<string>> odsRows = null;

        private readonly Dictionary<string, ScriptValue> variables = new Dictionary<string, ScriptValue>();
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

                odsCache.Clear();
                variables.Clear();
                messages.Clear();
                lastErrorLine = -1;

                generatedGuids.Clear();
                generatedGroups.Clear();

                allOpenedTemplates.Clear();
                allEditedPackages.Clear();
                allSavedPackages.Clear();

                using (StreamReader scriptStream = new StreamReader(scriptPath))
                {
                    parserState = new ParserState(this, scriptStream);
                    result = ProcessBlocks(0);
                }

                stopwatch.Stop();

                if (result)
                {
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
                            if (!allOpenedTemplates.Contains(templateName))
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
                else
                {
                    if (isDevMode) ReportErrorNull("Ummmm ....");
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
                                return ReportErrorFalse($"Unknown variable {value} in REPEAT");
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

                            ScriptValue sv = new ScriptValue(value);

                            if (!sv.IsNumber)
                            {
                                return ReportErrorFalse($"Unknown count (expected variable or constant got {value})");
                            }

                            int count = (int)sv;

                            if (count > 0)
                            {
                                bool processed = false;

                                parserState.StartRepeat();

                                for (int iter = 0; iter < count; ++iter)
                                {
                                    processed = ProcessBlocks(iter);

                                    if (!processed) break;

                                    if (iter < (count - 1)) parserState.NextRepeat();
                                }

                                if (!scriptWorker.CancellationPending) parserState.EndRepeat();

                                return processed && SkipCloseSquareBracket();
                            }
                            else
                            {
                                return SkipBlock();
                            }
                        }
                    }
                }
                else
                {
                    string saveVar = null;
                    bool conditionValue = true;
                    bool negateCondition = false;

                    // template.package ($saveNameVar) (IF (NOT) $conditionVar) [ ... ]
                    if (filename.EndsWith(".package"))
                    {
                        if (PeekNextToken()[0] == '$')
                        {
                            saveVar = parserState.ReadNextToken();
                        }

                        if ("IF".Equals(PeekNextToken()))
                        {
                            parserState.ReadNextToken();

                            if ("NOT".Equals(PeekNextToken()))
                            {
                                parserState.ReadNextToken();
                                negateCondition = true;
                            }

                            if (PeekNextToken()[0] == '$')
                            {
                                conditionValue = (new ScriptValue(EvaluateVariable(parserState.ReadNextToken()))).IsTrue;

                                if (negateCondition) conditionValue = !conditionValue;
                            }
                            else
                            {
                                return ReportErrorFalse($"Expected condition variable for IF, got {PeekNextToken()}");
                            }
                        }
                    }

                    if (!conditionValue)
                    {
                        if (SkipOpenSquareBracket())
                        {
                            return SkipBlock();
                        }

                        return ReportErrorFalse("Can't find block opening square bracket");
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

        private bool SkipBlock()
        {
            string token = parserState.ReadNextToken();

            while (!"]".Equals(token))
            {
                if (token == null)
                {
                    return ReportErrorFalse("Can't find block closing square bracket");
                }

                if ("[".Equals(token))
                {
                    if (!SkipBlock()) return false;
                }

                token = parserState.ReadNextToken();
            }

            return true;
        }

        private bool ProcessInitialisers(string filename, int iteration)
        {
            if (!filename.Equals("INIT"))
            {
                if (!odsCache.TryGetValue(filename, out odsRows))
                {
                    odsRows = ParseODS($"{templatePath}\\{filename}");
                    odsCache.Add(filename, odsRows);
                }
                else
                {
                    ReportProgress("Reading from cache");
                }

                if (odsRows == null) return false;
            }

            while (ProcessInitialiser(iteration)) ;

            return TestEndOfBlock();
        }

        private bool ProcessInitialiser(int iteration)
        {
            if (TestEndOfBlock()) return false;

            string nextToken = PeekNextToken();

            if (nextToken.Equals("assert", StringComparison.OrdinalIgnoreCase))
            {
                return ProcessAssert();
            }
            else if (nextToken.Equals("comment", StringComparison.OrdinalIgnoreCase))
            {
                return ProcessComment();
            }
            else if (nextToken.Equals("message", StringComparison.OrdinalIgnoreCase))
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
                        variables.Add(varDefn, new ScriptValue(value));

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
                allOpenedTemplates.Add(filename);
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

            bool isClone = false;

            string nextToken = PeekNextToken();
            string tgir = null;

            if ("INIT".Equals(nextToken))
            {
                parserState.ReadNextToken();

                if (SkipOpenSquareBracket())
                {
                    return ProcessInitialisers(nextToken, iteration) && SkipCloseSquareBracket();
                }

                return ReportErrorFalse("Missing open square bracket after INIT");
            }
            else if ("DELETE".Equals(nextToken))
            {
                parserState.ReadNextToken();

                bool conditionValue = true;
                bool negateCondition = false;

                if ("IF".Equals(PeekNextToken()))
                {
                    parserState.ReadNextToken();

                    if ("NOT".Equals(PeekNextToken()))
                    {
                        parserState.ReadNextToken();
                        negateCondition = true;
                    }

                    if (PeekNextToken()[0] == '$')
                    {
                        conditionValue = (new ScriptValue(EvaluateVariable(parserState.ReadNextToken()))).IsTrue;

                        if (negateCondition) conditionValue = !conditionValue;
                    }
                    else
                    {
                        return ReportErrorFalse($"Expected condition variable for IF, got {PeekNextToken()}");
                    }
                }

                tgir = ReadTGIR();

                if (tgir != null)
                {
                    if (!conditionValue)
                    {
                        return true;
                    }

                    ReportProgress($"Deleting {tgir}");

                    Match m = reTGIR.Match(tgir);

                    if (m.Success)
                    {
                        TypeTypeID typeId = DBPFData.TypeID(m.Groups[1].Value);
                        TypeGroupID groupId = (TypeGroupID)(uint)Int32.Parse(m.Groups[2].Value.Substring(2), System.Globalization.NumberStyles.HexNumber);
                        TypeResourceID resourceId = (TypeResourceID)(uint)Int32.Parse(m.Groups[3].Value.Substring(2), System.Globalization.NumberStyles.HexNumber);
                        TypeInstanceID instanceId = (TypeInstanceID)(uint)Int32.Parse(m.Groups[4].Value.Substring(2), System.Globalization.NumberStyles.HexNumber);

                        DBPFKey resKey = new DBPFKey(typeId, groupId, instanceId, resourceId);

                        activePackage.Remove(resKey);

                        return true;
                    }
                }

                return ReportErrorFalse("Expected TGRI after DELETE");
            }
            else if ("CLONE".Equals(nextToken))
            {
                parserState.ReadNextToken();

                isClone = true;

                // Drop through into TGIR parsing
            }

            tgir = ReadTGIR();

            if (tgir != null)
            {
                bool conditionValue = true;
                bool negateCondition = false;

                if ("IF".Equals(PeekNextToken()))
                {
                    parserState.ReadNextToken();

                    if ("NOT".Equals(PeekNextToken()))
                    {
                        parserState.ReadNextToken();
                        negateCondition = true;
                    }

                    if (PeekNextToken()[0] == '$')
                    {
                        conditionValue = (new ScriptValue(EvaluateVariable(parserState.ReadNextToken()))).IsTrue;

                        if (negateCondition) conditionValue = !conditionValue;
                    }
                    else
                    {
                        return ReportErrorFalse($"Expected condition variable for IF, got {PeekNextToken()}");
                    }
                }

                if (!conditionValue)
                {
                    if (SkipOpenSquareBracket())
                    {
                        return SkipBlock();
                    }

                    return ReportErrorFalse("Can't find TGRI action opening square bracket");
                }

                if (SkipOpenSquareBracket())
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
                                if (!isClone)
                                {
                                    activePackage.UnCommit(resKey);
                                    activePackage.Remove(resKey);
                                }

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

            string token = PeekNextToken();

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

        private bool ProcessComment()
        {
            string token = parserState.ReadNextToken();

            if (token != null && token.Equals("comment") && SkipOpenBracket())
            {
                string comment = parserState.ReadNextToken();

                if (comment != null && SkipCloseBracket())
                {
                    comment = EvaluateString(comment);

                    if (comment.StartsWith("\"") && comment.EndsWith("\""))
                    {
                        comment = comment.Substring(1, comment.Length - 2);
                    }

                    ReportProgress($"++{comment}");
                    return true;
                }
            }

            return ReportErrorFalse("Invalid COMMENT");
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
                        else if (assertItem.Equals("version"))
                        {
                            Match m = reVersion.Match(assertValue);

                            if (m.Success)
                            {
                                try
                                {
                                    int major = int.Parse(m.Groups[1].Value);
                                    int minor = int.Parse(m.Groups[2].Value);

                                    bool ok = false;

                                    if (DbpfScripterApp.AppVersionMajor > major)
                                    {
                                        ok = true;
                                    }
                                    else if (DbpfScripterApp.AppVersionMajor == major)
                                    {
                                        if (DbpfScripterApp.AppVersionMinor >= minor)
                                        {
                                            ok = true;
                                        }
                                    }

                                    if (!ok)
                                    {
                                        return ReportErrorFalse($"Assert failed, {assertItem}:{assertValue}");
                                    }

                                    ReportDebug($"  Asserted {assertItem}:{assertValue}");
                                    return ok;
                                }
                                catch (Exception)
                                {
                                    return ReportErrorFalse($"Assert failed, {assertItem}:{assertValue}");
                                }
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

                        if (value == null) return false;
                    }
                    else
                    {
                        return ReportErrorFalse("Unknown assignment (expected string or variable)");
                    }

                    // Can't test for a variable here, as that was used up by $var [ ... ] for indexing
                    if (item.StartsWith("\""))
                    {
                        item = EvaluateString(item);
                        item = item.Substring(1, item.Length - 2);
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
            bool clone = false;

            if ("CLONE".Equals(PeekNextToken()))
            {
                clone = true;
                parserState.ReadNextToken();
            }

            if (index != null && SkipOpenSquareBracket())
            {
                if (IsVariableDefn(index))
                {
                    ReportDevMode($"Indexing from ${index}");
                    index = EvaluateVariable(index);

                    if (index.ToLower().StartsWith("0x"))
                    {
                        index = Int32.Parse(index.Substring(2), System.Globalization.NumberStyles.HexNumber).ToString();
                    }
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
                        IDbpfScriptable indexer = currentScriptableObject.Indexed(Int32.Parse(index), clone);

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
                else
                {
                    ReportErrorFalse($"{index} is not a valid index");
                }
            }

            return false;
        }
        #endregion

        #region Evaluation
        private string EvaluateVariable(string var)
        {
            ScriptValue value = null;

            if (var.StartsWith("$")) var = var.Substring(1);

            if (var != null) variables.TryGetValue(var, out value);

            if (value == null)
            {
                if (var.Equals("SaveBaseName"))
                {
                    value = new ScriptValue(baseName);
                }
            }

            return value?.ToString();
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

                    if (!PeekNextIsCloseBracket())
                    {
                        value = EvaluateVariable(ReadVarDefn());

                        if (PeekNextIsComma())
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

                    if (!PeekNextIsCloseBracket())
                    {
                        value = EvaluateVariable(ReadVarDefn());

                        if (PeekNextIsComma())
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
            else if (function.Equals("ghash"))
            {
                if (SkipOpenBracket())
                {
                    string param = ReadVarDefn();

                    if (param != null && SkipCloseBracket())
                    {
                        value = EvaluateVariable(param);

                        if (value != null)
                        {
                            return Hashes.GroupIDHash(value).ToString();
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
                                    return ReportErrorNull($"File '{value}' cannot be found");
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
            else if (function.Equals("preinc"))
            {
                if (SkipOpenBracket())
                {
                    string param = ReadVarDefn();

                    if (param != null && SkipCloseBracket())
                    {
                        variables[param]?.Inc();

                        value = EvaluateVariable(param);

                        return value;
                    }
                }
            }
            else if (function.Equals("postinc"))
            {
                if (SkipOpenBracket())
                {
                    string param = ReadVarDefn();

                    if (param != null && SkipCloseBracket())
                    {
                        value = EvaluateVariable(param);

                        variables[param]?.Inc();

                        return value;
                    }
                }
            }
            // NOTE: If adding to the function list, must also update IsFunctionName below

            return ReportErrorNull($"Unknown function {function}"); ;
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

                if (macro.Equals("{"))
                {
                    value = $"{value}{macro}";
                }
                else
                {
                    string subst = EvaluateVariable(macro);

                    if (subst == null)
                    {
                        return ReportErrorNull($"Unknown variable {macro} in macro");
                    }

                    if (macroLen > 0)
                    {
                        subst = subst.Substring(0, macroLen);
                    }

                    if (lower) subst = subst.ToLower();
                    if (upper) subst = subst.ToUpper();

                    value = $"{value}{subst}";
                }

                codedString = codedString.Substring(ketPos + 1);
                braPos = codedString.IndexOf('{');
            }

            return $"{value}{codedString}";
        }
        #endregion

        #region Tests for "end of ..."
        private bool TestEndOfScript()
        {
            if (scriptWorker.CancellationPending) return true;

            string token = PeekNextToken();

            if ("END".Equals(token)) return true;

            return (token == null);
        }

        private bool TestEndOfBlock()
        {
            if (scriptWorker.CancellationPending) return true;

            string token = PeekNextToken();

            return (token != null && token.Equals("]"));
        }
        #endregion

        #region Tests for "is something"
        private bool IsFunctionName(string s)
        {
            return s.Equals("loword") || s.Equals("hiword") ||
                   s.Equals("lobyte") || s.Equals("hibyte") ||
                   s.Equals("guid") || s.Equals("group") || s.Equals("family") ||
                   s.Equals("ghash") || s.Equals("rhash") || s.Equals("ihash") ||
                   s.Equals("fullpath") ||
                   s.Equals("if") ||
                   s.Equals("preinc") || s.Equals("postinc");
        }

        private bool IsVariableDefn(string s)
        {
            return s != null && s.StartsWith("$");
        }
        #endregion

        #region Peek tokens
        private string PeekNextToken()
        {
            return parserState.PeekNextToken();
        }

        private bool PeekNextToken(string expected)
        {
            return expected.Equals(parserState.PeekNextToken(), StringComparison.OrdinalIgnoreCase);
        }

        private bool PeekNextIsComma() => PeekNextToken(",");
        private bool PeekNextIsCloseBracket() => PeekNextToken(")");
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

            string nextToken = PeekNextToken();
            if (nextToken.ToLower().StartsWith("0x"))
            {
                value = parserState.ReadNextToken();
            }
            else if (nextToken[0] == '"' || nextToken[0] == '\'')
            {
                string str = parserState.ReadNextToken();

                value = EvaluateString(str.Substring(1, str.Length - 2));
            }
            else if (nextToken[0] == '$')
            {
                string var = parserState.ReadNextToken();

                value = EvaluateVariable(var);
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

                            bool addIteration = false;

                            if (colCode.EndsWith("+"))
                            {
                                addIteration = true;
                                colCode = colCode.Substring(0, colCode.Length - 1);
                            }

                            int col = 0;

                            if (colCode.Length == 2)
                            {
                                col = 26 * (colCode.ToCharArray()[0] - 'a' + 1);

                                colCode = colCode.Substring(1);
                            }

                            col += colCode.ToCharArray()[0] - 'a';

                            if (addIteration)
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

        private List<List<string>> ParseODS(string fullPath)
        {
            List<List<string>> rows = null;
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
                                    rows = new List<List<string>>();
                                }
                                else if (reader.Name.Equals("table:table-row"))
                                {
                                    // Process the previous row when we encounter the next one in the table, this stops us processing the last row (which repeats many, many times)
                                    if (row != null)
                                    {
                                        while (rowRepeat-- > 0)
                                        {
                                            rows.Add(row);
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

                                    string span = reader.GetAttribute("table:number-columns-spanned");
                                    int cellSpan = (span == null) ? 1 : Int32.Parse(span);

                                    string repeat = reader.GetAttribute("table:number-columns-repeated");
                                    cellRepeat = (repeat == null) ? cellSpan : Int32.Parse(repeat);
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
                                            rows.Add(row);
                                        }
                                    }

                                    // Only process the first table in the first spreadsheet
                                    return rows;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReportDebug(ex.Message);
                ReportErrorNull($"Unable to open/read {fullPath}");
                return null;
            }

            ReportErrorNull($"Unable to parse {fullPath}");
            return null;
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
                newGuid = $"0x{Helper.Hex8String(TypeGUID.RandomID.AsUInt()).ToUpper()}";

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
                newGroup = $"0x{Helper.Hex8String(TypeGroupID.RandomID.AsUInt()).ToUpper()}";

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
        int lastErrorLine;
        internal string ReportErrorNull(string msg)
        {
            ReportErrorFalse(msg);

            return null;
        }

        internal bool ReportErrorFalse(string msg)
        {
            if (!scriptWorker.CancellationPending)
            {
                if (!(lastErrorLine == parserState.ScriptLineIndex))
                {
                    ReportProgress($"!!{msg} at line {parserState.ScriptLineIndex}");

                    ++countErrors;
                    lastErrorLine = parserState.ScriptLineIndex;
                }
            }

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
