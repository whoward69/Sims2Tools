/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System.IO;

namespace Sims2Tools.Files
{
    public class PjseStringFileReader
    {
        private readonly StreamReader reader = null;

        private string lastLine = null;

        public PjseStringFileReader(string txtPath)
        {
            reader = new StreamReader(txtPath);

            lastLine = reader.ReadLine();
        }

        public string ReadString()
        {
            return ReadText("<-String->", "<-Desc->");
        }

        public string ReadDesc()
        {
            return ReadText("<-Desc->", "<-String->");
        }

        private string ReadText(string start, string end)
        {
            string text = null;

            while (lastLine != null && !lastLine.Equals(start))
            {
                lastLine = reader.ReadLine();
            }

            if (lastLine != null && lastLine.Equals(start))
            {
                string line = reader.ReadLine();

                while (!line.Equals(end))
                {
                    if (text == null)
                    {
                        text = line;
                    }
                    else
                    {
                        text = $"{text}\n{line}";
                    }

                    line = reader.ReadLine();
                }

                if (text == null) text = "";

                lastLine = line;
            }

            return text;
        }

        public void Close()
        {
            reader?.Close();
        }
    }
}
