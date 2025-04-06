/*
 * Log Watcher - a utility for monitoring Sims 2 ObjectError logs
 *
 * William Howard - 2020-2025
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

namespace LogWatcher.Controls
{
    public partial class LogViewer : UserControl
    {
        private static readonly Sims2Tools.DBPF.Logger.IDBPFLogger logger = Sims2Tools.DBPF.Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly Dictionary<int, string> dataGlobal = new Dictionary<int, string>();
        private static readonly Dictionary<int, string> dataGeneral = new Dictionary<int, string>();
        private static readonly Dictionary<int, string> dataPerson = new Dictionary<int, string>();
        private static readonly Dictionary<int, string> dataMotive = new Dictionary<int, string>();

        private readonly Regex reProp = new Regex("Property ([0-9]+): (-?[0-9]+)");

        static LogViewer()
        {
            ParseXml("Resources/XML/globaldata.xml", "label", dataGlobal);
            ParseXml("Resources/XML/generaldata.xml", "label", dataGeneral);
            ParseXml("Resources/XML/persondata.xml", "label", dataPerson);
            ParseXml("Resources/XML/motivedata.xml", "label", dataMotive);
        }

        private static void ParseXml(string xml, string element, Dictionary<int, string> byValue)
        {
            XmlReader reader = XmlReader.Create(xml);

            int value = -1;
            string name = null;

            reader.MoveToContent();
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name.Equals("value"))
                    {
                        reader.Read();
                        value = int.Parse(reader.Value);
                    }
                    else if (reader.Name.Equals("name"))
                    {
                        reader.Read();
                        name = reader.Value;
                    }
                }
                else if (reader.NodeType == XmlNodeType.EndElement && reader.Name.Equals(element))
                {
                    byValue.Add(value, name);
                }
            }
        }

        private readonly AttributesDataTable dataTableAttributes = new AttributesDataTable();
        private readonly LotObjectsDataTable dataTableLotObjects = new LotObjectsDataTable();

        private ISearcher searcher;

        private string logFilePath;
        private string tabName;

        private LogXml logXml;

        private bool incPropIndex;

        public bool IncPropIndex
        {
            get => incPropIndex;
            set
            {
                incPropIndex = value;

                Reload();
            }
        }

        public string TabName => tabName;

        public string LogFilePath
        {
            get => logFilePath;
            set
            {
                logFilePath = value;
                tabName = new FileInfo(logFilePath).Name;

                Reload();
            }
        }

        public ISearcher Searcher
        {
            get => searcher;
            set => searcher = value;
        }

        public void Reload()
        {
            FileInfo logFileInfo = new FileInfo(logFilePath);

            try
            {
                logXml = new LogXml(logFilePath);

                treeView.Nodes.Clear();
                TreeNode root = treeView.Nodes.Add(logFileInfo.Name.Substring(0, logFileInfo.Name.Length - 4));

                foreach (XmlElement child in logXml.Root.ChildNodes)
                {
                    ProcessNode(root, child);
                }

                root.Expand();

                PopulateSectionView(logXml.Header);
            }
            catch (IOException ex)
            {
                logger.Error(ex.Message);
                logger.Info(ex.StackTrace);
            }
        }

        private void ProcessNode(TreeNode root, XmlElement child)
        {
            if (child.ChildNodes.Count != 0)
            {
                TreeNode node = root.Nodes.Add(child.GetAttribute("name"));
                node.Tag = child;

                ProcessChildNodes(node, child);
            }
        }

        private void ProcessChildNodes(TreeNode node, XmlElement child)
        {
            foreach (XmlElement grandchild in child.ChildNodes)
            {
                if (!grandchild.Name.Equals("line") && !grandchild.Name.Equals("tokenGuid") && !grandchild.Name.Equals("tokenProp") && !grandchild.Name.Equals("data") && !grandchild.Name.Equals("attr") && !grandchild.Name.Equals("lotobj") && !grandchild.Name.Equals("cheat"))
                {
                    ProcessNode(node, grandchild);
                }
            }
        }

        public LogViewer()
        {
            InitializeComponent();

            gridAttributes.DataSource = dataTableAttributes;
            gridLotObjects.DataSource = dataTableLotObjects;
        }

        private void OnTreeNodeClicked(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Nodes.Count > 0)
            {
                if (e.Node.Level == 0)
                {
                    FileInfo logFileInfo = new FileInfo(logFilePath);

                    textBox.Text = $"Occured: {logFileInfo.LastWriteTime.ToLongDateString()} {logFileInfo.LastWriteTime.ToLongTimeString()}";
                }
                else
                {
                    textBox.Text = "";
                }
            }
            else
            {
                PopulateSectionView((XmlNode)e.Node.Tag);
            }
        }

        private void PopulateSectionView(XmlNode nodeData)
        {
            textBox.HideSelection = true;
            textBox.Visible = false;
            gridLotObjects.Visible = false;
            gridAttributes.Visible = false;

            searcher.Reset(false);

            if (nodeData.ChildNodes.Count > 0)
            {
                string type = nodeData.ChildNodes[0].Name;

                if (type.Equals("data"))
                {
                    dataTableAttributes.Clear();

                    gridAttributes.Columns["colAttrIndex"].Visible = true;
                    gridAttributes.Columns["colAttrValueHex"].Visible = true;

                    int index = 0;

                    foreach (XmlElement attr in nodeData.ChildNodes)
                    {
                        if (nodeData.Name.Equals("globals"))
                        {
                            dataTableAttributes.Append(index, dataGlobal[index], GetIntAttr(attr, "value"), GetIntAttrAsHex(attr, "value"));
                        }
                        else if (nodeData.Name.Equals("general"))
                        {
                            dataTableAttributes.Append(index, dataGeneral[index], GetIntAttr(attr, "value"), GetIntAttrAsHex(attr, "value"));
                        }
                        else if (nodeData.Name.Equals("person"))
                        {
                            dataTableAttributes.Append(index, dataPerson[index], GetIntAttr(attr, "value"), GetIntAttrAsHex(attr, "value"));
                        }
                        else if (nodeData.Name.Equals("motives"))
                        {
                            dataTableAttributes.Append(index, dataMotive[index], GetNumAttr(attr, "value"), null);
                            gridAttributes.Columns["colAttrValueHex"].Visible = false;
                        }

                        ++index;
                    }

                    gridAttributes.Sort(gridAttributes.Columns["colAttrIndex"], ListSortDirection.Ascending);
                    gridAttributes.Visible = true;
                }
                else if (type.Equals("attr"))
                {
                    dataTableAttributes.Clear();

                    gridAttributes.Columns["colAttrIndex"].Visible = true;
                    gridAttributes.Columns["colAttrValueHex"].Visible = true;

                    foreach (XmlElement attr in nodeData.ChildNodes)
                    {
                        dataTableAttributes.Append(GetIntAttr(attr, "index"), attr.GetAttribute("key"), GetIntAttr(attr, "value"), GetIntAttrAsHex(attr, "value"));
                    }

                    gridAttributes.Sort(gridAttributes.Columns["colAttrIndex"], ListSortDirection.Ascending);
                    gridAttributes.Visible = true;
                }
                else if (type.Equals("lotobj"))
                {
                    dataTableLotObjects.Clear();

                    foreach (XmlElement lotObj in nodeData.ChildNodes)
                    {
                        dataTableLotObjects.Append(GetIntAttr(lotObj, "oid"), lotObj.GetAttribute("object"), GetIntAttr(lotObj, "room"), GetIntAttr(lotObj, "container"), GetIntAttr(lotObj, "slot"));
                    }

                    gridLotObjects.Visible = true;
                }
                else if (type.Equals("cheat"))
                {
                    dataTableAttributes.Clear();

                    gridAttributes.Columns["colAttrIndex"].Visible = false;
                    gridAttributes.Columns["colAttrValueHex"].Visible = false;

                    foreach (XmlElement cheat in nodeData.ChildNodes)
                    {
                        dataTableAttributes.Append("", cheat.GetAttribute("key"), cheat.GetAttribute("value"), null);
                    }

                    gridAttributes.Sort(gridAttributes.Columns["colAttrKey"], ListSortDirection.Ascending);
                    gridAttributes.Visible = true;
                }
                else
                {
                    textBox.Text = "";
                    bool addNL = false;

                    foreach (XmlElement child in nodeData.ChildNodes)
                    {
                        if (addNL) textBox.AppendText(Environment.NewLine);

                        string colour = child.GetAttribute("colour");
                        if (colour != null && !colour.Equals(""))
                        {
                            Color wasColour = textBox.SelectionColor;

                            try
                            {
                                textBox.SelectionColor = Color.FromName(colour);
                            }
                            catch (Exception) { }
                            textBox.AppendText(child.InnerText);

                            textBox.SelectionColor = wasColour;
                        }
                        else
                        {
                            if (child.Name.Equals("tokenProp"))
                            {
                                Match m = reProp.Match(child.InnerText);
                                int index = short.Parse(m.Groups[1].Value);

                                if (incPropIndex)
                                {
                                    ++index;
                                }

                                string propDecValue = m.Groups[2].Value;
                                string propHexValue = Helper.Hex4PrefixString(int.Parse(propDecValue));

                                textBox.AppendText($"\tProperty {index}: {propHexValue} ({propDecValue})");
                            }
                            else
                            {
                                textBox.AppendText(child.InnerText);
                            }
                        }

                        addNL = true;
                    }

                    textBox.SelectionStart = 0;
                    textBox.Visible = true;
                    searcher.Reset(true);
                }
            }
            else
            {
                textBox.Text = "";
                textBox.Visible = true;
            }
        }

        private object GetIntAttr(XmlElement element, string attrName)
        {
            try
            {
                string value = element.GetAttribute(attrName);

                if (value.Length > 0)
                {
                    return int.Parse(value);
                }
            }
            catch (Exception) { }

            return null;
        }

        private object GetIntAttrAsHex(XmlElement element, string attrName)
        {
            try
            {
                string value = element.GetAttribute(attrName);

                if (value.Length > 0)
                {
                    return Helper.Hex4PrefixString(int.Parse(value));
                }
            }
            catch (Exception) { }

            return null;
        }

        private object GetNumAttr(XmlElement element, string attrName)
        {
            try
            {
                string value = element.GetAttribute(attrName);

                if (value.Length > 0)
                {
                    return double.Parse(value);
                }
            }
            catch (Exception) { }

            return null;
        }

        private int lastSearchPos;

        public void FindFirst(string text)
        {
            textBox.HideSelection = false;
            lastSearchPos = textBox.Find(text);
        }

        public void FindNext(string text)
        {
            lastSearchPos = textBox.Find(text, lastSearchPos + 1, RichTextBoxFinds.None);
        }

        private void OnTextBoxMouseMove(object sender, MouseEventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox.Text))
            {
                int pos = textBox.GetCharIndexFromPosition(e.Location);
                char c = textBox.Text[pos];

                if (char.IsDigit(c))
                {
                    int prevSpacePos = textBox.Text.LastIndexOf(" ", pos);
                    int prevTabPos = textBox.Text.LastIndexOf("\t", pos);
                    int prevNlPos = textBox.Text.LastIndexOf("\n", pos);

                    if (prevNlPos > prevSpacePos) prevSpacePos = prevNlPos;
                    if (prevTabPos > prevSpacePos) prevSpacePos = prevTabPos;
                    if (prevSpacePos == -1) prevSpacePos = 0; else prevSpacePos += 1;

                    int nextSpacePos = textBox.Text.IndexOf(" ", pos);
                    int nextTabPos = textBox.Text.IndexOf("\t", pos);
                    int nextNlPos = textBox.Text.IndexOf("\n", pos);

                    if (nextNlPos != -1 && nextNlPos < nextSpacePos) nextSpacePos = nextNlPos;
                    if (nextTabPos != -1 && nextTabPos < nextSpacePos) nextSpacePos = nextTabPos;
                    if (nextSpacePos == -1) nextSpacePos = textBox.Text.Length - 1; else nextSpacePos -= 1;

                    string s = textBox.Text.Substring(prevSpacePos, nextSpacePos - prevSpacePos + 1);

                    if (short.TryParse(s, out short i))
                    {
                        string tt = Helper.Hex4PrefixString(i);

                        XmlNode node = logXml.Root.SelectSingleNode($"lotObjects/lotobj[@oid='{s}']");

                        if (node != null)
                        {
                            tt = $"{tt} - {node.Attributes.GetNamedItem("object")?.Value}";
                        }

                        toolTipTextBox.Show(tt, textBox, e.X, e.Y + 10);
                        return;
                    }
                }
            }

            toolTipTextBox.Hide(textBox);
        }

        private void OnToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string s = null;

                if (sender == gridLotObjects && e.ColumnIndex == 3)
                {
                    s = gridLotObjects.Rows[e.RowIndex].Cells[e.ColumnIndex].Value as string;
                }
                else if (sender == gridAttributes && (e.ColumnIndex == 2 || e.ColumnIndex == 3))
                {
                    s = gridAttributes.Rows[e.RowIndex].Cells[2].Value as string;
                }

                if (s != null && short.TryParse(s, out short i))
                {
                    string tt = Helper.Hex4PrefixString(i);

                    XmlNode node = logXml.Root.SelectSingleNode($"lotObjects/lotobj[@oid='{s}']");

                    if (node != null)
                    {
                        tt = $"{tt} - {node.Attributes.GetNamedItem("object")?.Value}";
                    }

                    e.ToolTipText = tt;
                }
            }
        }
    }
}
