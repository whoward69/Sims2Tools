/*
 * DBPF Compare - a utility for comparing two DBPF packages
 *
 * William Howard - 2020-2024
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using DbpfCompare.Controls;
using Sims2Tools;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.BCON;
using Sims2Tools.DBPF.BHAV;
using Sims2Tools.DBPF.CTSS;
using Sims2Tools.DBPF.GLOB;
using Sims2Tools.DBPF.Neighbourhood.SDNA;
using Sims2Tools.DBPF.NREF;
using Sims2Tools.DBPF.OBJD;
using Sims2Tools.DBPF.OBJF;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph.AGED;
using Sims2Tools.DBPF.SceneGraph.BINX;
using Sims2Tools.DBPF.SceneGraph.COLL;
using Sims2Tools.DBPF.SceneGraph.GZPS;
using Sims2Tools.DBPF.SceneGraph.IDR;
using Sims2Tools.DBPF.SceneGraph.LAMB;
using Sims2Tools.DBPF.SceneGraph.LDIR;
using Sims2Tools.DBPF.SceneGraph.LPNT;
using Sims2Tools.DBPF.SceneGraph.LSPT;
using Sims2Tools.DBPF.SceneGraph.MMAT;
using Sims2Tools.DBPF.SceneGraph.SHPE;
using Sims2Tools.DBPF.SceneGraph.TXMT;
using Sims2Tools.DBPF.SceneGraph.XFCH;
using Sims2Tools.DBPF.SceneGraph.XHTN;
using Sims2Tools.DBPF.SceneGraph.XMOL;
using Sims2Tools.DBPF.SceneGraph.XSTN;
using Sims2Tools.DBPF.SceneGraph.XTOL;
using Sims2Tools.DBPF.SLOT;
using Sims2Tools.DBPF.STR;
using Sims2Tools.DBPF.TPRP;
using Sims2Tools.DBPF.TRCN;
using Sims2Tools.DBPF.TTAS;
using Sims2Tools.DBPF.VERS;
using Sims2Tools.DBPF.XFLR;
using Sims2Tools.DBPF.XFNC;
using Sims2Tools.DBPF.XOBJ;
using Sims2Tools.DBPF.XROF;
using Sims2Tools.DBPF.XWNT;
using Sims2Tools.Dialogs;
using Sims2Tools.Updates;
using Sims2Tools.Utils.Persistence;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DbpfCompare
{
    public partial class DbpfCompareForm : Form
    {
        private static readonly Sims2Tools.DBPF.Logger.IDBPFLogger logger = Sims2Tools.DBPF.Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        internal static readonly Color colourDiffers = Color.FromName(Properties.Settings.Default.ResDiffers);
        internal static readonly Color colourMissing = Color.FromName(Properties.Settings.Default.ResMissing);
        internal static readonly Color colourSame = Color.FromName(Properties.Settings.Default.ResSame);

        internal static readonly Color colourDirty = Color.FromName(Properties.Settings.Default.DirtyHighlight);

        private readonly Dictionary<TypeTypeID, DbpfCompareNodeTypeData> allTypeData = new Dictionary<TypeTypeID, DbpfCompareNodeTypeData>();
        private readonly SortedList<DBPFKey, DbpfCompareNodeResourceData> allResourceData = new SortedList<DBPFKey, DbpfCompareNodeResourceData>();

        private DbpfCompareNodeResourceData leftSelectedNodeData = null;

        private readonly List<TypeTypeID> comparableTypes = new List<TypeTypeID>(new TypeTypeID[] {
            Bcon.TYPE, Trcn.TYPE,
            Bhav.TYPE, Tprp.TYPE,
            Objd.TYPE, Objf.TYPE,
            Glob.TYPE, Nref.TYPE,
            Slot.TYPE,
            Idr.TYPE,
            Str.TYPE, Ctss.TYPE, Ttas.TYPE,
            Shpe.TYPE,
            Txmt.TYPE,
            Aged.TYPE,
            Binx.TYPE,
            Coll.TYPE,
            Gzps.TYPE,
            Mmat.TYPE,
            Vers.TYPE,
            Sdna.TYPE,
            Xfch.TYPE, Xhtn.TYPE, Xmol.TYPE, Xstn.TYPE, Xtol.TYPE,
            Xflr.TYPE, Xfnc.TYPE, Xrof.TYPE,
            Xobj.TYPE,
            Xwnt.TYPE,
            Lamb.TYPE, Ldir.TYPE, Lpnt.TYPE, Lspt.TYPE
        });

        private bool IsDirty
        {
            get
            {
                foreach (DbpfCompareNodeResourceData nodeData in allResourceData.Values)
                {
                    if (nodeData.IsDirty) return true;
                }

                return false;
            }
        }

        private Updater MyUpdater;

        public DbpfCompareForm()
        {
            logger.Info(DbpfCompareApp.AppProduct);

            InitializeComponent();
            this.Text = DbpfCompareApp.AppTitle;

            // Only need to do this one way
            linkedTreeViewLeft.AddLinkedTreeView(linkedTreeViewRight);
        }

        private void OnLoad(object sender, EventArgs e)
        {
            RegistryTools.LoadAppSettings(DbpfCompareApp.RegistryKey, DbpfCompareApp.AppVersionMajor, DbpfCompareApp.AppVersionMinor);
            RegistryTools.LoadFormSettings(DbpfCompareApp.RegistryKey, this);

            menuItemAutoBackup.Checked = ((int)RegistryTools.GetSetting(DbpfCompareApp.RegistryKey + @"\Mode", menuItemAutoBackup.Name, 1) != 0);

            menuItemExcludeSame.Checked = ((int)RegistryTools.GetSetting(DbpfCompareApp.RegistryKey + @"\Options", menuItemExcludeSame.Name, 0) != 0);
            menuItemExcludeLeftMissing.Checked = ((int)RegistryTools.GetSetting(DbpfCompareApp.RegistryKey + @"\Options", menuItemExcludeLeftMissing.Name, 0) != 0);
            menuItemExcludeRightMissing.Checked = ((int)RegistryTools.GetSetting(DbpfCompareApp.RegistryKey + @"\Options", menuItemExcludeRightMissing.Name, 0) != 0);

            MyUpdater = new Updater(DbpfCompareApp.RegistryKey, menuHelp);
            MyUpdater.CheckForUpdates();

            UpdateForm();
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsDirty)
            {
                if (MsgBox.Show($"There are unsaved changes, do you really want to exit?", "Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }

            RegistryTools.SaveAppSettings(DbpfCompareApp.RegistryKey, DbpfCompareApp.AppVersionMajor, DbpfCompareApp.AppVersionMinor);
            RegistryTools.SaveFormSettings(DbpfCompareApp.RegistryKey, this);
            RegistryTools.SaveSetting(DbpfCompareApp.RegistryKey, "splitter", splitContainer.SplitterDistance);

            RegistryTools.SaveSetting(DbpfCompareApp.RegistryKey + @"\Mode", menuItemAutoBackup.Name, menuItemAutoBackup.Checked ? 1 : 0);

            RegistryTools.SaveSetting(DbpfCompareApp.RegistryKey + @"\Options", menuItemExcludeSame.Name, menuItemExcludeSame.Checked ? 1 : 0);
            RegistryTools.SaveSetting(DbpfCompareApp.RegistryKey + @"\Options", menuItemExcludeLeftMissing.Name, menuItemExcludeLeftMissing.Checked ? 1 : 0);
            RegistryTools.SaveSetting(DbpfCompareApp.RegistryKey + @"\Options", menuItemExcludeRightMissing.Name, menuItemExcludeRightMissing.Checked ? 1 : 0);
        }

        private void OnFileOpening(object sender, EventArgs e)
        {
            menuItemReloadPackage.Enabled = (!string.IsNullOrEmpty(textLeftPath.Text) && !string.IsNullOrEmpty(textRightPath.Text));

            menuItemSelectLeftPackage.Enabled = !IsDirty;
            menuItemSelectRightPackage.Enabled = !IsDirty;

            menuItemSaveRightPackage.Enabled = IsDirty;

            menuItemSaveAsCsv.Enabled = menuItemReloadPackage.Enabled;
        }

        private void OnExitClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnHelpClicked(object sender, EventArgs e)
        {
            new AboutDialog(DbpfCompareApp.AppProduct).ShowDialog();
        }

        private void OnConfigurationClicked(object sender, EventArgs e)
        {
            Form config = new ConfigDialog();

            if (config.ShowDialog() == DialogResult.OK)
            {
            }
        }

        private void OnReloadClicked(object sender, EventArgs e)
        {
            PopulateTrees();
        }

        private void OnSelectLeftClicked(object sender, EventArgs e)
        {
            selectFileDialog.FileName = "*.package";
            if (selectFileDialog.ShowDialog() == DialogResult.OK)
            {
                SetLeftPath(selectFileDialog.FileName);
            }
        }

        private void OnSelectRightClicked(object sender, EventArgs e)
        {
            selectFileDialog.FileName = "*.package";
            if (selectFileDialog.ShowDialog() == DialogResult.OK)
            {
                SetRightPath(selectFileDialog.FileName);
            }
        }

        private void SetLeftPath(string path)
        {
            SetPath(path, textLeftPath);
        }

        private void SetRightPath(string path)
        {
            SetPath(path, textRightPath);
        }

        private void SetPath(string path, TextBox textBox)
        {
            textBox.Text = path;
            toolTipPaths.SetToolTip(textBox, textBox.Text);

            textBox.Select(textBox.Text.Length, 0);
            textBox.Focus();
            textBox.ScrollToCaret();

            PopulateTrees();
        }

        private void UpdateForm()
        {
            if (IsDirty)
            {
                textRightPath.BackColor = colourDirty;
                btnSaveRight.Enabled = true;
            }
            else
            {
                textRightPath.BackColor = textLeftPath.BackColor;
                btnSaveRight.Enabled = false;
            }

            btnSwitch.Enabled = (!IsDirty && !string.IsNullOrEmpty(textLeftPath.Text) && !string.IsNullOrEmpty(textRightPath.Text));
        }

        private void PopulateTrees()
        {
            if (string.IsNullOrEmpty(textLeftPath.Text) || string.IsNullOrEmpty(textRightPath.Text)) return;

            this.Text = $"{DbpfCompareApp.AppTitle} - {(new FileInfo(textLeftPath.Text)).Name} vs {(new FileInfo(textRightPath.Text)).Name}";
            menuItemReloadPackage.Enabled = false;
            menuItemSelectLeftPackage.Enabled = false;
            menuItemSelectRightPackage.Enabled = false;

            GetAllNodeData();
            UpdateLinkedTrees(true);

            menuItemSelectLeftPackage.Enabled = true;
            menuItemSelectRightPackage.Enabled = true;
            menuItemReloadPackage.Enabled = true;

            leftSelectedNodeData = null;
        }

        private void GetAllNodeData()
        {
            allTypeData.Clear();
            allResourceData.Clear();

            // Should probably be on a worker thread, but what the heck!
            using (DBPFFile packageLeft = new DBPFFile(textLeftPath.Text), packageRight = new DBPFFile(textRightPath.Text))
            {
                if (packageLeft != null && packageRight != null)
                {
                    GetNodeData(packageLeft, packageLeft, packageRight);
                    GetNodeData(packageRight, packageLeft, packageRight);
                }

                packageLeft?.Close();
                packageRight?.Close();
            }
        }

        private void GetNodeData(DBPFFile package, DBPFFile packageLeft, DBPFFile packageRight)
        {
            foreach (DBPFEntry entry in package.GetAllEntries())
            {
                if (!allResourceData.ContainsKey(entry))
                {
                    DbpfCompareNodeResourceData nodeData = new DbpfCompareNodeResourceData(entry, menuContextResource);

                    allResourceData.Add(entry, nodeData);

                    byte[] leftData = packageLeft.GetItemByKey(entry);
                    byte[] rightData = packageRight.GetItemByKey(entry);

                    if (leftData == null && rightData == null)
                    {
                        throw new Exception("Just how the feck did this happen?");
                    }
                    else if (leftData == null)
                    {
                        nodeData.SetLeftMissing();
                    }
                    else if (rightData == null)
                    {
                        nodeData.SetRightMissing();
                    }
                    else
                    {
                        if (leftData.Length != rightData.Length)
                        {
                            nodeData.SetDifferent();
                        }
                        else
                        {
                            for (int i = 0; i < leftData.Length; i++)
                            {
                                if (leftData[i] != rightData[i])
                                {
                                    nodeData.SetDifferent();
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void UpdateLinkedTrees(bool showRoot)
        {
            linkedTreeViewLeft.BeginUpdate();
            linkedTreeViewRight.BeginUpdate();

            linkedTreeViewLeft.Nodes.Clear();
            linkedTreeViewRight.Nodes.Clear();

            linkedTreeViewLeft.Nodes.Add((new FileInfo(textLeftPath.Text)).Name);
            linkedTreeViewRight.Nodes.Add((new FileInfo(textRightPath.Text)).Name);

            Color typeColour = colourSame;
            string lastTypeName = "";
            DbpfCompareNodeTypeData typeData = null;

            foreach (DbpfCompareNodeResourceData nodeData in allResourceData.Values)
            {
                string typeName = nodeData.TypeName;

                if (!typeName.Equals(lastTypeName))
                {
                    if (typeData != null)
                    {
                        if (typeData.NodeCount == 0)
                        {
                            linkedTreeViewLeft.Nodes[0].Nodes.Remove(typeData.GetLeftNode());
                            linkedTreeViewRight.Nodes[0].Nodes.Remove(typeData.GetRightNode());
                        }
                        else
                        {
                            typeData.ForeColor = typeColour;
                        }
                    }

                    if (allTypeData.TryGetValue(nodeData.TypeID, out typeData))
                    {
                        typeData.Clear();
                    }
                    else
                    {
                        typeData = new DbpfCompareNodeTypeData(nodeData.TypeID, menuContextType);
                        allTypeData.Add(nodeData.TypeID, typeData);
                    }

                    linkedTreeViewLeft.Nodes[0].Nodes.Add(typeData.GetLeftNode());
                    linkedTreeViewRight.Nodes[0].Nodes.Add(typeData.GetRightNode());

                    lastTypeName = typeName;
                    typeColour = colourSame;
                }

                if (nodeData.IsLeftMissing)
                {
                    if (!menuItemExcludeLeftMissing.Checked)
                    {
                        // Insert blank into left, entry into right (blue)
                        typeData.AddLeft(nodeData.LeftNode());
                        typeData.AddRight(nodeData.RightNode());

                        if (typeColour == colourSame) typeColour = colourMissing;
                    }
                }
                else if (nodeData.IsRightMissing)
                {
                    if (!menuItemExcludeRightMissing.Checked)
                    {
                        // Insert entry into left (blue), blank into right
                        typeData.AddLeft(nodeData.LeftNode());
                        typeData.AddRight(nodeData.RightNode());

                        if (typeColour == colourSame) typeColour = colourMissing;
                    }
                }
                else if (nodeData.IsToBeDeleted)
                {
                    // Do nothing
                }
                else
                {
                    if (!(nodeData.IsSame && menuItemExcludeSame.Checked))
                    {
                        // Insert entry into left and right (red or gray)
                        typeData.AddLeft(nodeData.LeftNode());
                        typeData.AddRight(nodeData.RightNode());

                        if (nodeData.IsDifferent) typeColour = colourDiffers;
                    }
                }
            }

            if (typeData.NodeCount == 0)
            {
                linkedTreeViewLeft.Nodes[0].Nodes.Remove(typeData.GetLeftNode());
                linkedTreeViewRight.Nodes[0].Nodes.Remove(typeData.GetRightNode());
            }
            else
            {
                typeData.ForeColor = typeColour;
            }

            linkedTreeViewLeft.EndUpdate();
            linkedTreeViewRight.EndUpdate();

            ExpandCollapse(linkedTreeViewLeft);

            if (showRoot)
            {
                linkedTreeViewLeft.Nodes[0].EnsureVisible();
                linkedTreeViewRight.Nodes[0].EnsureVisible();
            }

            UpdateForm();
        }

        private void ExpandCollapse(LinkedTreeView linkedTreeView)
        {
            // Always expand the root node
            linkedTreeView.Nodes[0].Expand();

            foreach (TreeNode typeNode in linkedTreeView.Nodes[0].Nodes)
            {
                DbpfCompareNodeTypeData typeData = typeNode.Tag as DbpfCompareNodeTypeData;
                if (typeData.Expanded)
                {
                    typeNode.Expand();
                }
                else
                {
                    typeNode.Collapse();
                }
            }
        }

        private void OnDragEnter(object sender, DragEventArgs e)
        {
            if (IsDirty) return;

            Regex rePackageName = new Regex(@"\.((package((\.V[1-9][0-9]*)?\.bak)?)|(bak|temp))$");

            DataObject data = e.Data as DataObject;

            if (data.ContainsFileDropList())
            {
                string[] rawFiles = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (rawFiles != null && rawFiles.Length <= 2)
                {
                    if (rePackageName.Match(Path.GetFileName(rawFiles[0])).Success)
                    {
                        if (rawFiles.Length == 1 || rePackageName.Match(Path.GetFileName(rawFiles[1])).Success)
                        {
                            e.Effect = DragDropEffects.Copy;
                        }
                    }
                }
            }
        }

        private void OnDragDrop(object sender, DragEventArgs e)
        {
            if (IsDirty) return;

            DataObject data = e.Data as DataObject;

            if (data.ContainsFileDropList())
            {
                string[] rawFiles = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (rawFiles != null)
                {
                    if (sender == linkedTreeViewLeft || sender == textLeftPath)
                    {
                        SetLeftPath(rawFiles[0]);

                        if (rawFiles.Length == 2)
                        {
                            SetRightPath(rawFiles[1]);
                        }
                    }
                    else
                    {
                        SetRightPath(rawFiles[0]);

                        if (rawFiles.Length == 2)
                        {
                            SetLeftPath(rawFiles[1]);
                        }
                    }
                }
            }
        }

        private void OnExcludeChanged(object sender, EventArgs e)
        {
            if (allResourceData.Count > 0)
            {
                UpdateLinkedTrees(true);
            }
        }

        TreeNode mouseNode = null;
        private void OnTreeViewMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                mouseNode = (sender as LinkedTreeView).GetNodeAt(e.Location);
            }
        }

        private void OnContextTypeOpening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DbpfCompareNodeTypeData typeData = (DbpfCompareNodeTypeData)mouseNode?.Tag;

            menuItemContextCopyAllMissingRight.Enabled = (typeData != null && typeData.AnyMissing);
        }

        private void OnContextCopyAllMissingRight(object sender, EventArgs e)
        {
            DbpfCompareNodeTypeData typeData = (DbpfCompareNodeTypeData)mouseNode?.Tag;

            if (typeData != null)
            {
                foreach (TreeNode node in typeData.GetLeftNode().Nodes)
                {
                    DbpfCompareNodeResourceData nodeData = (DbpfCompareNodeResourceData)node.Tag;

                    if (nodeData.IsRightMissing) nodeData.SetCopyLeftToRight();
                }

                if (typeData.IsDirty)
                {
                    UpdateLinkedTrees(false);

                    typeData.EnsureVisible();
                }
            }
        }

        private void OnContextResourceOpening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DbpfCompareNodeResourceData nodeData = (DbpfCompareNodeResourceData)mouseNode?.Tag;

            bool isLeftNode = mouseNode.TreeView.Name.Equals(linkedTreeViewLeft.Name);

            if (!isLeftNode)
            {
                e.Cancel = (leftSelectedNodeData == null || leftSelectedNodeData.TypeID != nodeData.TypeID);
            }
        }

        private void OnContextResourceOpened(object sender, EventArgs e)
        {
            DbpfCompareNodeResourceData nodeData = (DbpfCompareNodeResourceData)mouseNode?.Tag;

            bool isLeftNode = mouseNode.TreeView.Name.Equals(linkedTreeViewLeft.Name);

            menuItemContextSelectLeft.Visible = isLeftNode;
            menuItemContextCompare.Visible = !isLeftNode;

            menuItemContextSelectLeft.Enabled = comparableTypes.Contains(nodeData.TypeID);

            menuItemContextCopyRight.Visible = isLeftNode;
            menuItemContextCopyRight.Enabled = (nodeData != null && (nodeData.IsDifferent || nodeData.IsRightMissing));
        }

        private void OnContextSelectLeft(object sender, EventArgs e)
        {
            leftSelectedNodeData = (DbpfCompareNodeResourceData)mouseNode?.Tag;
        }

        private void OnContextCompare(object sender, EventArgs e)
        {
            DbpfCompareNodeResourceData nodeData = (DbpfCompareNodeResourceData)mouseNode?.Tag;

            Trace.Assert(leftSelectedNodeData.TypeID == nodeData?.TypeID, "Can't compare different resource types!");

            CompareNodes(leftSelectedNodeData, nodeData);
        }

        private void OnContextCopyRight(object sender, EventArgs e)
        {
            DbpfCompareNodeResourceData nodeData = (DbpfCompareNodeResourceData)mouseNode?.Tag;

            nodeData.SetCopyLeftToRight();

            if (nodeData.IsDirty)
            {
                DbpfCompareNodeTypeData typeData = nodeData.LeftNode().Parent.Tag as DbpfCompareNodeTypeData;

                UpdateLinkedTrees(false);

                typeData.EnsureVisible();
            }
        }

        private void OnSaveRightPackage(object sender, EventArgs e)
        {
            // Should probably be on a worker thread, but I'm not going to let you cancel the action anyway!
            using (DBPFFile packageLeft = new DBPFFile(textLeftPath.Text), packageRight = new DBPFFile(textRightPath.Text))
            {
                if (packageLeft != null && packageRight != null)
                {
                    foreach (DbpfCompareNodeResourceData nodeData in allResourceData.Values)
                    {
                        if (nodeData.IsDirty)
                        {
                            if (nodeData.IsToBeCopied)
                            {
                                byte[] rawData = packageLeft.GetItemByKey(nodeData.Key);

                                if (rawData != null) packageRight.Commit(nodeData.Key, rawData);
                            }
                            else if (nodeData.IsToBeDeleted)
                            {
                                packageRight.Remove(nodeData.Key);
                            }
                        }
                    }

                    packageRight.Update(menuItemAutoBackup.Checked);
                }

                packageLeft?.Close();
                packageRight?.Close();
            }

            // Assuming everything went OK, do a full reload
            PopulateTrees();
        }

        private void OnSwitchClicked(object sender, EventArgs e)
        {
            string leftPath = textLeftPath.Text;
            string rightPath = textRightPath.Text;

            textLeftPath.Text = null;
            textRightPath.Text = null;

            SetLeftPath(rightPath);
            SetRightPath(leftPath);
        }

        private void OnDoubleClick(object sender, EventArgs e)
        {
            if ((sender as LinkedTreeView)?.SelectedNode?.Tag is DbpfCompareNodeResourceData nodeData && nodeData.IsDifferent)
            {
                if (comparableTypes.Contains(nodeData.TypeID))
                {
                    CompareNodes(nodeData, nodeData);
                }
            }
        }

        private void CompareNodes(DbpfCompareNodeResourceData leftNodeData, DbpfCompareNodeResourceData rightNodeData)
        {
            (new ResCompareForm(leftNodeData, textLeftPath.Text, rightNodeData, textRightPath.Text, menuItemExcludeSame.Checked)).ShowDialog();

            if (leftNodeData.IsSame)
            {
                DbpfCompareNodeTypeData typeData = leftNodeData.LeftNode().Parent.Tag as DbpfCompareNodeTypeData;

                UpdateLinkedTrees(false);

                typeData.EnsureVisible();
            }
        }

        private void OnSaveAsCsv(object sender, EventArgs e)
        {
            saveCsvDialog.ShowDialog();

            if (saveCsvDialog.FileName != "")
            {
                try
                {
                    StreamWriter writer = new StreamWriter(saveCsvDialog.OpenFile());

                    writer.WriteLine("TGRI,Type,Group ID,Resource ID,Instance ID,State");

                    WriteNodeAsCsv(writer, linkedTreeViewLeft.Nodes[0]);

                    writer.Close();
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message);

                    MsgBox.Show(ex.Message, "Cannot save results!", MessageBoxButtons.OK);
                }
            }
        }

        private void WriteNodeAsCsv(StreamWriter writer, TreeNode node)
        {
            if (node.Nodes.Count > 0)
            {
                foreach (TreeNode childNode in node.Nodes)
                {
                    WriteNodeAsCsv(writer, childNode);
                }
            }
            else
            {
                DbpfCompareNodeResourceData data = node.Tag as DbpfCompareNodeResourceData;

                writer.WriteLine($"{data.Key},{DBPFData.TypeName(data.Key.TypeID)},{data.Key.GroupID},{data.Key.ResourceID},{data.Key.InstanceID},{(data.IsSame ? "Same" : (data.IsDifferent ? "Different" : (data.IsLeftMissing ? "Right Only" : "Left Only")))}");
            }
        }
    }
}
