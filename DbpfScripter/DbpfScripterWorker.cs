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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Xml;

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

        private TextReader scriptReader;
        private string scriptLine = null;
        private string nextToken = null;

        private readonly Stack<IDbpfScriptable> scriptableObjects = new Stack<IDbpfScriptable>();
        private IDbpfScriptable currentScriptableObject = null;

        private readonly Dictionary<string, DBPFFile> allEditedPackages = new Dictionary<string, DBPFFile>();
        private DBPFFile activePackage = null;

        private List<List<string>> odsRows = new List<List<string>>();

        private readonly Dictionary<string, string> variables = new Dictionary<string, string>();

        public DbpfScripterWorker(BackgroundWorker scriptWorker, string templatePath, string savePath, string baseName)
        {
            this.scriptWorker = scriptWorker;

            this.templatePath = templatePath;
            this.savePath = savePath;
            this.baseName = baseName;
        }

        public void ProcessScript()
        {
            string scriptPath = $"{templatePath}/dbpfscript.txt";

            if (File.Exists(scriptPath))
            {
                ReportProgress($"Processing {scriptPath}");

                bool result = false;

                using (scriptReader = new StreamReader(scriptPath))
                {
                    result = ProcessBlocks();
                }

                if (result)
                {
                    ReportProgress("Script complete");

                    foreach (string templateFileFullPath in Directory.GetFiles(templatePath, "*.package", SearchOption.AllDirectories))
                    {
                        string templateName = templateFileFullPath.Substring(templatePath.Length + 1);

                        string packageName = templateName.Replace("template", baseName);

                        FileInfo fi = new FileInfo($"{savePath}/{packageName}");

                        ReportProgress($"Saving {templateName} into {packageName}");

                        /* TODO - Scripter
                            Directory.CreateDirectory(fi.DirectoryName);

                            if (allEditedPackages.ContainsKey(templateName))
                            {
                                DBPFFile package = allEditedPackages[templateName];

                                // TODO - Scripter - implement DBPFFile.SaveAs()
                                // package.SaveAs(fi.FullName);

                                package.Close();

                                allEditedPackages.Remove(templateName);
                            }
                            else
                            {
                                File.Copy(templateFileFullPath, fi.FullName);
                            }
                        */
                    }

                    ReportProgress("Finished!");
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

            string filename = ReadFilename();

            if (filename != null && SkipOpenSquareBracket())
            {
                ReportProgress($"Processing BLOCK {filename}");

                bool processed;

                if (filename.EndsWith(".ods"))
                {
                    processed = ProcessInitialisers(filename);
                }
                else if (filename.EndsWith(".package"))
                {
                    processed = ProcessActions(filename);
                }
                else
                {
                    return ReportError($"Unknown file extension {filename}");
                }

                return processed && SkipCloseSquareBracket();
            }

            return ReportError("Invalid BLOCK");
        }

        private bool ProcessInitialisers(string filename)
        {
            ParseODS($"{templatePath}\\{filename}");

            while (ProcessInitialiser()) ;

            return TestEndOfBlock();
        }

        private bool ProcessInitialiser()
        {
            if (TestEndOfBlock()) return false;

            string varDefn = ReadVarDefn();

            if (varDefn != null && SkipEqualsSign())
            {
                string funct = ReadFunction();

                if (funct != null)
                {
                    string value = null;

                    if (funct.Equals("guid"))
                    {
                        if (SkipOpenBracket() && SkipCloseBracket()) value = NewGuid();
                    }
                    else if (funct.Equals("group"))
                    {
                        if (SkipOpenBracket() && SkipCloseBracket()) value = NewGroup();
                    }
                    else if (funct.Equals("family"))
                    {
                        if (SkipOpenBracket() && SkipCloseBracket()) value = NewFamily();
                    }
                    else if (funct.Equals("loword"))
                    {
                        if (SkipOpenBracket())
                        {
                            string param = ReadVarDefn();

                            if (param != null && SkipCloseBracket())
                            {
                                if (variables.TryGetValue(param, out value))
                                {
                                    value = $"0x{value.Substring(value.Length - 4)}";
                                }
                            }
                        }
                    }
                    else if (funct.Equals("hiword"))
                    {
                        if (SkipOpenBracket())
                        {
                            string param = ReadVarDefn();

                            if (param != null && SkipCloseBracket())
                            {
                                if (variables.TryGetValue(param, out value))
                                {
                                    value = value.Substring(0, 6);
                                }
                            }
                        }
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

                                    // ReportProgress($"Cell {funct} is {value}");
                                }
                            }
                        }
                    }

                    if (value != null)
                    {
                        variables.Remove(varDefn);
                        variables.Add(varDefn, value);

                        ReportProgress($"${varDefn} = {value}");

                        return true;
                    }
                }
            }

            return ReportError("Invalid variable definition");
        }

        private bool ProcessActions(string filename)
        {
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
                ReportProgress($"ACTION {tgir}");

                Match m = reTGIR.Match(tgir);

                if (m.Success)
                {
                    TypeTypeID typeId = DBPFData.TypeID(m.Groups[1].Value);
                    TypeGroupID groupId = (TypeGroupID)(uint)Int32.Parse(m.Groups[2].Value.Substring(2), System.Globalization.NumberStyles.HexNumber);
                    TypeResourceID resourceId = (TypeResourceID)(uint)Int32.Parse(m.Groups[3].Value.Substring(2), System.Globalization.NumberStyles.HexNumber);
                    TypeInstanceID instanceId = (TypeInstanceID)(uint)Int32.Parse(m.Groups[4].Value.Substring(2), System.Globalization.NumberStyles.HexNumber);

                    DBPFResource res = activePackage.GetResourceByKey(new DBPFKey(typeId, groupId, instanceId, resourceId));

                    if (res is IDbpfScriptable scriptable)
                    {
                        currentScriptableObject = scriptable;
                        scriptableObjects.Push(currentScriptableObject);

                        bool result = ProcessSubactions() && SkipCloseSquareBracket();

                        if (result)
                        {
                            activePackage.Commit(res);
                        }

                        scriptableObjects.Pop();

                        return result;
                    }
                    else
                    {
                        return ReportError($"{tgir} is not scriptable");
                    }
                }
            }

            return ReportError("Invalid ACTION");
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
                    string assertValue = ReadNextToken();

                    if (assertValue != null && SkipCloseBracket())
                    {
                        assertItem = assertItem.ToLower();

                        ReportProgress($"ASSERT: {assertItem} is {assertValue}");
                        return true;
                        // TODO - Scripter - Scripter - return currentScriptableObject.Assert(assertItem, assertValue);
                    }
                }
            }

            return ReportError("Invalid ASSERT");
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
                        string codedString = value.Substring(1, value.Length - 2);

                        // For example "{gender:1}{agecode:1}_{type}_{basename}_{id}" -> "tf_body_sundress_red"
                        value = "";

                        int braPos = codedString.IndexOf('{');

                        while (braPos != -1)
                        {
                            value = $"{value}{codedString.Substring(0, braPos)}";
                            codedString = codedString.Substring(braPos + 1);

                            int ketPos = codedString.IndexOf('}');
                            string macro = codedString.Substring(0, ketPos);
                            int macroLen = -1;

                            int colonPos = macro.IndexOf(':');
                            if (colonPos != -1)
                            {
                                int.TryParse(macro.Substring(colonPos + 1), out macroLen);
                                macro = macro.Substring(0, colonPos);
                            }

                            if (!variables.TryGetValue(macro, out string subst))
                            {
                                return ReportError($"Unknown variable {macro}");
                            }

                            if (macroLen > 0)
                            {
                                subst = subst.Substring(0, macroLen);
                            }

                            value = $"{value}{subst}";

                            codedString = codedString.Substring(ketPos + 1);
                            braPos = codedString.IndexOf('{');
                        }

                        value = $"{value}{codedString}";
                    }
                    else if (value[0] == '$')
                    {
                        if (!variables.TryGetValue(value.Substring(1), out value))
                        {
                            return ReportError($"Unknown variable {value}");
                        }
                    }
                    else
                    {
                        return ReportError("Unknown assignment (expected string or variable");
                    }

                    ReportProgress($"ASSIGNMENT: {item} = {value}");
                    return true;
                    // TODO - Scripter - return currentScriptableObject.Assignment(item, value);
                }
            }

            return ReportError("Invalid ASSIGNMENT");
        }

        private bool ProcessIndexing()
        {
            string index = ReadNextToken();

            if (index != null && reIndex.IsMatch(index) && SkipOpenSquareBracket())
            {
                ReportProgress($"INDEX {index}");

                // TODO - Scripter - IDbpfScriptable indexer = currentScriptableObject.Indexed(Int32.Parse(index));

                // TODO - Scripter - if (indexer != null)
                {
                    // TODO - Scripter - currentScriptableObject = indexer;
                    // TODO - Scripter - scriptableObjects.Push(currentScriptableObject);

                    bool result = ProcessSubactions() && SkipCloseSquareBracket();

                    // TODO - Scripter - scriptableObjects.Pop();
                    // TODO - Scripter - currentScriptableObject = scriptableObjects.Peek();

                    return result;
                }
            }

            return false;
        }
        #endregion

        #region ODS Parsing
        private void ParseODS(string fullPath)
        {
            odsRows = new List<List<string>>();
            List<string> row = null;

            int rowRepeat = 0;

            int cellRepeat = 0;
            string cellText = "";

            using (ZipArchive zip = ZipFile.Open(fullPath, ZipArchiveMode.Read))
            {
                ZipArchiveEntry content = zip.GetEntry("content.xml");

                if (content != null)
                {
                    ReportProgress($"Reading ODS {fullPath}");

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
                            if (reader.Name.Equals("table:table"))
                            {
                                // Only process the first table in the first spreadsheet
                                return;
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region New (Random) generators
        private string NewGuid()
        {
            ReportProgress($"Generating GUID");

            return $"0x{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }

        private string NewGroup()
        {
            ReportProgress($"Generating Group");

            return $"0x{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }

        private string NewFamily()
        {
            ReportProgress($"Generating FAMILY");

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

            return ReportError($"Expected {exectedToken}");
        }
        #endregion

        #region Reads and Peeks
        private string ReadFilename()
        {
            string filename = ReadNextToken();

            if (filename != null && filename.StartsWith("\""))
            {
                if (filename.Length <= 2) return ReportNull($"Invalid file name ({filename})");

                filename = filename.Substring(1, filename.Length - 2);
            }

            if (!File.Exists($"{templatePath}/{filename}")) return ReportNull($"File ({filename}) doesn't exist");

            return filename;
        }

        private string ReadTGIR()
        {
            string tgir = ReadNextToken();

            if (tgir != null && reTGIR.IsMatch(tgir)) return tgir;

            return ReportNull("TGIR expected");
        }

        private string ReadVarDefn()
        {
            string varDefn = ReadNextToken();

            if (varDefn != null && varDefn.StartsWith("$")) return varDefn.Substring(1);

            return ReportNull("Variable definition expected");
        }

        private string ReadFunction()
        {
            string function = ReadNextToken();

            if (function != null) return function.ToLower();

            return ReportNull("Function or cell reference expected");
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

                        if (pos >= scriptLine.Length) return ReportNull($"Missing closing {scriptLine[0]}");

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

            return token;
        }

        private string ReadNextLine()
        {
            string line = scriptReader.ReadLine();

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
            return (c == '[' || c == ']' || c == '(' || c == ')' || c == ':' || c == '=');
        }

        private bool IsSpaceOrSpecialChar(char c)
        {
            return (c == ' ' || IsSpecialChar(c));
        }
        #endregion

        #region Error Reporting
        private string ReportNull(string msg)
        {
            ReportError(msg);

            return null;
        }

        private bool ReportError(string msg)
        {
            ReportProgress(msg);

            return false;
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
