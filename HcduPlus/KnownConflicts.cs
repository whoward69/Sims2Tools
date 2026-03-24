/*
 * HCDU Plus - a utility for checking The Sims 2 package files for conflicts
 *           - see http://www.picknmixmods.com/Sims2/Notes/HcduPlus/HcduPlus.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.Dialogs;
using Sims2Tools.Utils.Persistence;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace HcduPlus.Conflict
{
    public class ConflictRegexPair
    {
        public Regex RegexA { get; }
        public Regex RegexB { get; }

        public bool IsValid { get; }

        public ConflictRegexPair(string regexA, string regexB)
        {
            this.IsValid = !(string.IsNullOrWhiteSpace(regexA) || string.IsNullOrWhiteSpace(regexB));

            if (IsValid)
            {
                this.RegexA = new Regex(regexA);
                this.RegexB = new Regex(regexB);
            }
        }
    }

    [System.ComponentModel.DesignerCategory("")]
    public class KnownConflicts : DataTable
    {
        private static readonly Sims2Tools.DBPF.Logger.IDBPFLogger logger = Sims2Tools.DBPF.Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly string KnownRegistryKey = HcduPlusApp.RegistryKey + @"\KnownConflicts";

        private readonly DataColumn colRegexEarlier = new DataColumn("Loads Earlier", typeof(string));
        private readonly DataColumn colRegexLater = new DataColumn("Loads Later", typeof(string));

        readonly List<ConflictRegexPair> reKnownConflicts = new List<ConflictRegexPair>();

        public KnownConflicts()
        {
            this.Columns.Add(colRegexEarlier);
            this.Columns.Add(colRegexLater);
        }

        public void Add(string reA, string reB)
        {
            this.Rows.Add(reA, reB);
            reKnownConflicts.Add(new ConflictRegexPair(reA, reB));
        }

        public void AddFromGrid(string packageA, string packageB)
        {
            string reA = packageA;
            string reB = packageB;

            int pos = packageA.LastIndexOf('\\');
            if (pos >= 0)
            {
                reA = packageA.Substring(pos + 1);
            }

            pos = packageB.LastIndexOf('\\');
            if (pos >= 0)
            {
                reB = packageB.Substring(pos + 1);
            }

            reA = reA.Replace("(", @"\(").Replace(")", @"\)").Replace("[", @"\[").Replace("]", @"\]").Replace("{", @"\{").Replace("}", @"\}").Replace(".", @"\.").Replace("*", @"\*").Replace("+", @"\+").Replace("?", @"\?");
            reB = reB.Replace("(", @"\(").Replace(")", @"\)").Replace("[", @"\[").Replace("]", @"\]").Replace("{", @"\{").Replace("}", @"\}").Replace(".", @"\.").Replace("*", @"\*").Replace("+", @"\+").Replace("?", @"\?");

            this.Rows.Add(reA, reB);
            reKnownConflicts.Add(new ConflictRegexPair(reA, reB));
        }

        public void Paste()
        {
            if (Clipboard.ContainsText(TextDataFormat.Text))
            {
                string[] lines = Clipboard.GetText(TextDataFormat.Text).Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

                Regex re = new Regex("^\\s*(.*?\\.package)\\s+(.*?\\.package)");

                foreach (string line in lines)
                {
                    Match m = re.Match(line);

                    if (m.Success)
                    {
                        Add(m.Groups[1].Value, m.Groups[2].Value);
                    }
                }
            }
        }

        public bool IsKnown(ConflictPair cp)
        {
            foreach (ConflictRegexPair reKnown in reKnownConflicts)
            {
                if (reKnown.IsValid)
                {
                    if (reKnown.RegexA.IsMatch(cp.PackageA) && reKnown.RegexB.IsMatch(cp.PackageB))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void CommitEdits()
        {
            reKnownConflicts.Clear();

            for (int i = 0; i < this.Rows.Count; ++i)
            {
                reKnownConflicts.Add(new ConflictRegexPair(this.Rows[i].ItemArray[0].ToString(), this.Rows[i].ItemArray[1].ToString()));
            }
        }

        public void ResetRegexs()
        {
            LoadXml("Resources/XML/defaultconflicts.xml");
        }

        public void LoadRegexs()
        {
            LoadRegexs((int)RegistryTools.GetSetting(KnownRegistryKey, "Count", -1));
        }

        private void LoadRegexs(int count)
        {
            if (count == -1)
            {
                ResetRegexs();
            }
            else
            {
                this.Clear();
                reKnownConflicts.Clear();

                for (int i = 0; i < count; i++)
                {
                    if (RegistryTools.GetSetting(KnownRegistryKey, $"Earlier{i}", null) is string reA &&
                        RegistryTools.GetSetting(KnownRegistryKey, $"Later{i}", null) is string reB)
                    {
                        Add(reA, reB);
                    }
                }
            }
        }

        public void SaveRegexs()
        {
            int count = (int)RegistryTools.GetSetting(KnownRegistryKey, "Count", 0);

            // Delete the saved entries.
            for (int i = 0; i < count; i++)
            {
                RegistryTools.DeleteSetting(KnownRegistryKey, $"Earlier{i}");
                RegistryTools.DeleteSetting(KnownRegistryKey, $"Later{i}");
            }

            // Save the current entries.
            RegistryTools.SaveSetting(KnownRegistryKey, "Count", reKnownConflicts.Count);
            count = 0;

            foreach (ConflictRegexPair reKnown in reKnownConflicts)
            {
                if (reKnown.IsValid)
                {
                    RegistryTools.SaveSetting(KnownRegistryKey, $"Earlier{count}", reKnown.RegexA.ToString());
                    RegistryTools.SaveSetting(KnownRegistryKey, $"Later{count}", reKnown.RegexB.ToString());

                    ++count;
                }
            }
        }

        public void LoadXml(string xmlFilePath)
        {
            this.Clear();
            reKnownConflicts.Clear();

            ParseXml(xmlFilePath);
        }

        private void ParseXml(string xmlFilePath)
        {
            try
            {
                XmlReader reader = XmlReader.Create(xmlFilePath);

                string earlier = null;
                string later = null;

                reader.MoveToContent();
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name.Equals("earlier"))
                        {
                            reader.Read();
                            earlier = reader.Value;
                        }
                        else if (reader.Name.Equals("later"))
                        {
                            reader.Read();
                            later = reader.Value;
                        }
                    }
                    else if (reader.NodeType == XmlNodeType.EndElement && reader.Name.Equals("conflict"))
                    {
                        Add(earlier, later);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Info(ex.StackTrace);

                MsgBox.Show("An error occured while reading default conflicts", "Error!", MessageBoxButtons.OK);
            }
        }

        public void SaveXml(string xmlFilePath)
        {
            using (StreamWriter writer = new StreamWriter(xmlFilePath))
            {
                XmlDocument doc = new XmlDocument();

                XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                XmlElement root = doc.DocumentElement;
                doc.InsertBefore(xmlDeclaration, root);

                DateTime now = DateTime.Now;
                doc.AppendChild(doc.CreateComment($"{now:d} {now:t}"));

                XmlElement eleConflicts = doc.CreateElement(string.Empty, "conflicts", string.Empty);
                doc.AppendChild(eleConflicts);

                foreach (ConflictRegexPair reKnown in reKnownConflicts)
                {
                    if (reKnown.IsValid)
                    {
                        XmlElement eleConflict = doc.CreateElement(string.Empty, "conflict", string.Empty);
                        eleConflicts.AppendChild(eleConflict);

                        XmlElement eleEarlier = doc.CreateElement(string.Empty, "earlier", string.Empty);
                        XmlText eleEarlierText = doc.CreateTextNode(reKnown.RegexA.ToString());
                        eleEarlier.AppendChild(eleEarlierText);
                        eleConflict.AppendChild(eleEarlier);

                        XmlElement eleLater = doc.CreateElement(string.Empty, "later", string.Empty);
                        XmlText eleLaterText = doc.CreateTextNode(reKnown.RegexB.ToString());
                        eleLater.AppendChild(eleLaterText);
                        eleConflict.AppendChild(eleLater);
                    }
                }

                writer.Write(XDocument.Parse(doc.OuterXml).ToString());

                writer.Close();
            }
        }
    }
}
