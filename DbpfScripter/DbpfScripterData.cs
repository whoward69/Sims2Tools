/*
 * DBPF Scripter - a utility for scripting edits to .package files
 *               - see http://www.picknmixmods.com/Sims2/Notes/DbpfScripter/DbpfScripter.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace DbpfScripter
{
    public class ParserState
    {
        private DbpfScripterWorker myWorker;

        private TextReader scriptReader;
        private int scriptLineIndex = 0;
        private string scriptLine = null;
        private string nextToken = null;

        private List<string> storedLines = null;
        private int restoreLine = -1;

        private int storedScriptLineIndex;
        private string storedScriptLine = null;
        private string storedNextToken = null;

        internal int ScriptLineIndex => scriptLineIndex;

        public ParserState(DbpfScripterWorker myWorker, StreamReader scriptReader)
        {
            this.myWorker = myWorker;
            this.scriptReader = scriptReader;
        }

        #region Store and Restore
        internal bool IsRepeating => (storedLines != null);

        internal void StartRepeat()
        {
            Trace.Assert(storedLines == null, "Nested StartRepeat() is not supported");

            storedScriptLineIndex = scriptLineIndex;
            storedScriptLine = scriptLine;
            storedNextToken = nextToken;

            storedLines = new List<string>();
        }

        internal void NextRepeat()
        {
            Trace.Assert(storedLines != null, "NextRepeat() called without a StartRepeat()");

            scriptLineIndex = storedScriptLineIndex;
            scriptLine = storedScriptLine;
            nextToken = storedNextToken;

            restoreLine = 0;
        }

        internal void EndRepeat()
        {
            Trace.Assert(restoreLine != 0, "EndRepeat() called when restoreLine is 0, have you called NextRepeat() erroneously?");

            restoreLine = -1;
            storedLines = null;
        }
        #endregion

        #region Reads and Peeks
        internal string ReadNextToken()
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

                        if (pos >= scriptLine.Length) return myWorker.ReportErrorNull($"Missing closing {scriptLine[0]}");

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

        internal string ReadNextLine()
        {
            string line;

            if (restoreLine != -1)
            {
                do
                {
                    Trace.Assert(restoreLine < storedLines.Count, "Trying to restore more lines than were stored!");

                    line = storedLines[restoreLine++];
                    ++scriptLineIndex;
                } while (string.IsNullOrEmpty(line));

                return line;
            }

            line = scriptReader.ReadLine();
            ++scriptLineIndex;

            if (line != null)
            {
                line = line.Trim();

                if (line.Length == 0 || line.StartsWith("//"))
                {
                    if (storedLines != null)
                    {
                        storedLines.Add("");
                    }

                    line = ReadNextLine();
                }
                else
                {
                    if (storedLines != null)
                    {
                        storedLines.Add(line);
                    }
                }
            }

            return line;
        }

        internal string PeekNextToken()
        {
            if (nextToken == null)
            {
                nextToken = ReadNextToken();
            }

            return nextToken;
        }
        #endregion

        #region IsXyz
        internal bool IsSpecialChar(char c)
        {
            return (c == '[' || c == ']' || c == '(' || c == ')' || c == ':' || c == '=' || c == ';' || c == ',');
        }

        internal bool IsSpaceOrSpecialChar(char c)
        {
            return (c == ' ' || IsSpecialChar(c));
        }
        #endregion
    }
}
