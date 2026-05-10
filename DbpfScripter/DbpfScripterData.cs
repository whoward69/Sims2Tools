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

using Sims2Tools.DBPF;
using System.Collections.Generic;
using System.IO;

namespace DbpfScripter
{
    public class ParserState
    {
        private readonly DbpfScripterWorker myWorker;
        private readonly ScriptReader scriptReader;

        private readonly Stack<ParserStateData> psDataStack = new Stack<ParserStateData>();
        private ParserStateData psCurrentData;

        public ParserState(DbpfScripterWorker myWorker, string scriptPath)
        {
            this.myWorker = myWorker;
            this.scriptReader = new ScriptReader(scriptPath);

            psCurrentData = new ParserStateData(myWorker, scriptReader, "Main");
            psDataStack.Push(psCurrentData);
        }

        #region Store and Restore
        internal bool IsRepeating => (psDataStack.Count > 1);

        internal void StartRepeat(string label)
        {
            ParserStateData psNewData = new ParserStateData(myWorker, scriptReader, label, psCurrentData);
            psDataStack.Push(psNewData);

            psCurrentData = psNewData;
        }

        internal void NextRepeat()
        {
            psCurrentData.NextRepeat();
        }

        internal bool EndRepeat()
        {
            bool result = psCurrentData.EndRepeat();

            psDataStack.Pop();
            psCurrentData = psDataStack.Peek();

            return result;
        }
        #endregion

        #region Reads and Peeks
        internal string ReadNextToken() => psCurrentData.ReadNextToken();

        internal string ReadNextLine() => psCurrentData.ReadNextLine();

        internal string PeekNextToken() => psCurrentData.PeekNextToken();
        #endregion

        #region Data Access
        internal int ScriptLineIndex => psCurrentData.ScriptLineIndex;
        #endregion
    }

    internal class ParserStateData
    {
        private readonly DbpfScripterWorker myWorker;
        private readonly ScriptReader scriptReader;

        private readonly string label;

        private readonly long storedScriptPosition;

        private readonly int storedScriptLineIndex;
        private readonly string storedScriptLine = null;
        private readonly string storedNextToken = null;

        private int scriptLineIndex = 0;
        private string scriptLine = null;
        private string nextToken = null;

        internal int ScriptLineIndex => scriptLineIndex;
        public ParserStateData(DbpfScripterWorker myWorker, ScriptReader scriptReader, string label) : this(myWorker, scriptReader, label, 0, null, null)
        {
        }

        public ParserStateData(DbpfScripterWorker myWorker, ScriptReader scriptReader, string label, ParserStateData psCurrentData) : this(myWorker, scriptReader, label, psCurrentData.scriptLineIndex, psCurrentData.scriptLine, psCurrentData.nextToken)
        {
        }

        private ParserStateData(DbpfScripterWorker myWorker, ScriptReader scriptReader, string label, int scriptLineIndex, string scriptLine, string nextToken)
        {
            this.myWorker = myWorker;
            this.scriptReader = scriptReader;

            this.storedScriptPosition = scriptReader.Position;

            this.label = label;

            storedScriptLineIndex = this.scriptLineIndex = scriptLineIndex;
            storedScriptLine = this.scriptLine = scriptLine;
            storedNextToken = this.nextToken = nextToken;
        }

        #region Store and Restore
        internal void NextRepeat()
        {
            scriptLineIndex = storedScriptLineIndex;
            scriptLine = storedScriptLine;
            nextToken = storedNextToken;

            scriptReader.Position = storedScriptPosition;
        }

        internal bool EndRepeat()
        {
            return (scriptReader.Position != storedScriptPosition);
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

            line = scriptReader.ReadLine();
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

        public override string ToString()
        {
            return $"{label}";
        }
    }


    public class ScriptReader
    {
        private readonly string[] lines;
        private long position = -1;

        public ScriptReader(string scriptPath)
        {
            lines = File.ReadAllLines(scriptPath);
        }

        public long Position
        {
            get => position;
            set => position = value;
        }

        public string ReadLine()
        {
            if (position >= (lines.Length - 1)) return null;

            return lines[++position];
        }
    }


    public class Options
    {
        private readonly Dictionary<string, ScriptValue> options = new Dictionary<string, ScriptValue>();

        public void Add(string option, ScriptValue value)
        {
            options.Remove(option);

            options.Add(option, value);
        }

        public bool IsTrue(string option)
        {
            if (options.TryGetValue(option, out ScriptValue value))
            {
                return value.IsTrue;
            }

            return true;
        }
    }
}
