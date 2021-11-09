/*
 * Log Watcher - a utility for monitoring Sims 2 ObjectError logs
 *
 * William Howard - 2020-2021
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace LogWatcher.Controls
{
    public partial class LogViewer : UserControl
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly Dictionary<int, String> dataGlobal = new Dictionary<int, string>();
        private static readonly Dictionary<int, String> dataGeneral = new Dictionary<int, string>();
        private static readonly Dictionary<int, String> dataPerson = new Dictionary<int, string>();
        private static readonly Dictionary<int, String> dataMotive = new Dictionary<int, string>();

        static LogViewer()
        {
            ParseXml("Resources/XML/globaldata.xml", "label", dataGlobal);
            ParseXml("Resources/XML/generaldata.xml", "label", dataGeneral);
            ParseXml("Resources/XML/persondata.xml", "label", dataPerson);
            ParseXml("Resources/XML/motivedata.xml", "label", dataMotive);
        }

        private static void ParseXml(String xml, String element, Dictionary<int, String> byValue)
        {
            XmlReader reader = XmlReader.Create(xml);

            int value = -1;
            String name = null;

            reader.MoveToContent();
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name.Equals("value"))
                    {
                        reader.Read();
                        value = Int32.Parse(reader.Value);
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

        private String logFilePath;
        private String tabName;

        public String TabName => tabName;

        public String LogFilePath
        {
            get => logFilePath;
            set
            {
                logFilePath = value;
                tabName = (new FileInfo(logFilePath)).Name;

                Reload();
            }
        }

        public void Reload()
        {
            FileInfo logFileInfo = new FileInfo(logFilePath);

            try
            {
                LogXml logXml = new LogXml(logFilePath);

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
                if (!grandchild.Name.Equals("line") && !grandchild.Name.Equals("tokenGuid") && !grandchild.Name.Equals("data") && !grandchild.Name.Equals("attr") && !grandchild.Name.Equals("lotobj") && !grandchild.Name.Equals("cheat"))
                {
                    ProcessNode(node, grandchild);
                }
            }
        }

        public LogViewer()
        {
            InitializeComponent();
        }

        private void OnTreeNodeClicked(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Nodes.Count > 0)
            {
                textBox.Text = "";
            }
            else
            {
                PopulateSectionView((XmlNode)e.Node.Tag);
            }
        }

        private void PopulateSectionView(XmlNode nodeData)
        {
            textBox.Visible = false;
            gridLotObjects.Visible = false;
            gridAttributes.Visible = false;

            if (nodeData.ChildNodes.Count > 0)
            {
                String type = nodeData.ChildNodes[0].Name;

                if (type.Equals("data"))
                {
                    gridAttributes.Rows.Clear();
                    gridAttributes.Columns["colAttrIndex"].Visible = true;

                    int index = 0;

                    foreach (XmlElement attr in nodeData.ChildNodes)
                    {
                        String key = "";
                        if (nodeData.Name.Equals("globals"))
                        {
                            key = dataGlobal[index];
                        }
                        else if (nodeData.Name.Equals("general"))
                        {
                            key = dataGeneral[index];
                        }
                        else if (nodeData.Name.Equals("person"))
                        {
                            key = dataPerson[index];
                        }
                        else if (nodeData.Name.Equals("motives"))
                        {
                            key = dataMotive[index];
                        }

                        gridAttributes.Rows.Add(index++, key, GetNumAttr(attr, "value"));
                    }

                    gridAttributes.Sort(gridAttributes.Columns["colAttrIndex"], ListSortDirection.Ascending);
                    gridAttributes.Visible = true;
                }
                else if (type.Equals("attr"))
                {
                    gridAttributes.Rows.Clear();
                    gridAttributes.Columns["colAttrIndex"].Visible = true;

                    foreach (XmlElement attr in nodeData.ChildNodes)
                    {
                        gridAttributes.Rows.Add(GetIntAttr(attr, "index"), attr.GetAttribute("key"), GetIntAttr(attr, "value"));
                    }

                    gridAttributes.Sort(gridAttributes.Columns["colAttrIndex"], ListSortDirection.Ascending);
                    gridAttributes.Visible = true;
                }
                else if (type.Equals("lotobj"))
                {
                    gridLotObjects.Rows.Clear();

                    foreach (XmlElement lotObj in nodeData.ChildNodes)
                    {
                        gridLotObjects.Rows.Add(GetIntAttr(lotObj, "oid"), lotObj.GetAttribute("object"), GetIntAttr(lotObj, "room"), GetIntAttr(lotObj, "container"), GetIntAttr(lotObj, "slot"));
                    }

                    gridLotObjects.Visible = true;
                }
                else if (type.Equals("cheat"))
                {
                    gridAttributes.Rows.Clear();
                    gridAttributes.Columns["colAttrIndex"].Visible = false;

                    foreach (XmlElement cheat in nodeData.ChildNodes)
                    {
                        gridAttributes.Rows.Add("", cheat.GetAttribute("key"), cheat.GetAttribute("value"));
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

                        if (child.Name.Equals("tokenGuid"))
                        {
                            Color was = textBox.SelectionColor;
                            textBox.SelectionColor = Color.FromName(Properties.Settings.Default.InvGuidColour);
                            textBox.AppendText(child.InnerText);
                            textBox.SelectionColor = was;
                        }
                        else
                        {
                            textBox.AppendText(child.InnerText);
                        }

                        addNL = true;
                    }

                    textBox.Visible = true;
                }
            }
            else
            {
                textBox.Text = "";
                textBox.Visible = true;
            }
        }

        private Object GetIntAttr(XmlElement element, String attrName)
        {
            try
            {
                String value = element.GetAttribute(attrName);

                if (value.Length > 0)
                {
                    return Int32.Parse(value);
                }
            }
            catch (Exception) { }

            return "";
        }
        private Object GetNumAttr(XmlElement element, String attrName)
        {
            try
            {
                String value = element.GetAttribute(attrName);

                if (value.Length > 0)
                {
                    return Double.Parse(value);
                }
            }
            catch (Exception) { }

            return "";
        }
    }
}
