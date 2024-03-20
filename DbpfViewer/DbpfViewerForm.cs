/*
 * DBPF Viewer - a utility for testing the DBPF Library
 *
 * William Howard - 2020-2024
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools;
using Sims2Tools.Controls;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.BCON;
using Sims2Tools.DBPF.BHAV;
using Sims2Tools.DBPF.CTSS;
using Sims2Tools.DBPF.GLOB;
using Sims2Tools.DBPF.Images.IMG;
using Sims2Tools.DBPF.Images.JPG;
using Sims2Tools.DBPF.NREF;
using Sims2Tools.DBPF.OBJD;
using Sims2Tools.DBPF.OBJF;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SLOT;
using Sims2Tools.DBPF.STR;
using Sims2Tools.DBPF.TPRP;
using Sims2Tools.DBPF.TRCN;
using Sims2Tools.DBPF.TTAB;
using Sims2Tools.DBPF.TTAS;
using Sims2Tools.DBPF.Utils;
using Sims2Tools.DBPF.VERS;
using Sims2Tools.Dialogs;
using Sims2Tools.Updates;
using Sims2Tools.Utils.Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace DbpfViewer
{
    public partial class DbpfViewerForm : Form
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string packageFile = null;
        private readonly SortedDictionary<string, string> localObjectsByGroupID = new SortedDictionary<string, string>();

        private MruList MyMruList;
        private Updater MyUpdater;

        private readonly HashSet<TypeTypeID> enabledResources = new HashSet<TypeTypeID>();

        private readonly DbpfViewerData dbpfData = new DbpfViewerData();

        private string pictName;

        public DbpfViewerForm()
        {
            logger.Info(DbpfViewerApp.AppProduct);

            InitializeComponent();
            this.Text = DbpfViewerApp.AppTitle;

            gridResources.DataSource = dbpfData;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            RegistryTools.LoadAppSettings(DbpfViewerApp.RegistryKey, DbpfViewerApp.AppVersionMajor, DbpfViewerApp.AppVersionMinor);
            RegistryTools.LoadFormSettings(DbpfViewerApp.RegistryKey, this);

            MyMruList = new MruList(DbpfViewerApp.RegistryKey, menuItemRecentPackages, Properties.Settings.Default.MruSize, true, false);
            MyMruList.FileSelected += MyMruList_FileSelected;

            menuItemBcon.Checked = ((int)RegistryTools.GetSetting(DbpfViewerApp.RegistryKey + @"\Resources", Bcon.NAME, 1) != 0); OnBconClicked(menuItemBcon, null);
            menuItemBhav.Checked = ((int)RegistryTools.GetSetting(DbpfViewerApp.RegistryKey + @"\Resources", Bhav.NAME, 1) != 0); OnBhavClicked(menuItemBhav, null);
            menuItemCtss.Checked = ((int)RegistryTools.GetSetting(DbpfViewerApp.RegistryKey + @"\Resources", Ctss.NAME, 0) != 0); OnCtssClicked(menuItemCtss, null);
            menuItemGlob.Checked = ((int)RegistryTools.GetSetting(DbpfViewerApp.RegistryKey + @"\Resources", Glob.NAME, 0) != 0); OnGlobClicked(menuItemGlob, null);
            menuItemImg.Checked = ((int)RegistryTools.GetSetting(DbpfViewerApp.RegistryKey + @"\Resources", Img.NAME, 0) != 0); OnImgClicked(menuItemImg, null);
            menuItemJpg.Checked = ((int)RegistryTools.GetSetting(DbpfViewerApp.RegistryKey + @"\Resources", Jpg.NAME, 0) != 0); OnJpgClicked(menuItemJpg, null);
            menuItemObjd.Checked = ((int)RegistryTools.GetSetting(DbpfViewerApp.RegistryKey + @"\Resources", Objd.NAME, 0) != 0); OnObjdClicked(menuItemObjd, null);
            menuItemObjf.Checked = ((int)RegistryTools.GetSetting(DbpfViewerApp.RegistryKey + @"\Resources", Objf.NAME, 0) != 0); OnObjfClicked(menuItemObjf, null);
            menuItemNref.Checked = ((int)RegistryTools.GetSetting(DbpfViewerApp.RegistryKey + @"\Resources", Nref.NAME, 0) != 0); OnNrefClicked(menuItemNref, null);
            menuItemSlot.Checked = ((int)RegistryTools.GetSetting(DbpfViewerApp.RegistryKey + @"\Resources", Slot.NAME, 0) != 0); OnSlotClicked(menuItemSlot, null);
            menuItemStr.Checked = ((int)RegistryTools.GetSetting(DbpfViewerApp.RegistryKey + @"\Resources", Str.NAME, 0) != 0); OnStrClicked(menuItemStr, null);
            menuItemTprp.Checked = ((int)RegistryTools.GetSetting(DbpfViewerApp.RegistryKey + @"\Resources", Tprp.NAME, 0) != 0); OnTprpClicked(menuItemTprp, null);
            menuItemTrcn.Checked = ((int)RegistryTools.GetSetting(DbpfViewerApp.RegistryKey + @"\Resources", Trcn.NAME, 0) != 0); OnTrcnClicked(menuItemTrcn, null);
            menuItemTtab.Checked = ((int)RegistryTools.GetSetting(DbpfViewerApp.RegistryKey + @"\Resources", Ttab.NAME, 0) != 0); OnTtabClicked(menuItemTtab, null);
            menuItemTtas.Checked = ((int)RegistryTools.GetSetting(DbpfViewerApp.RegistryKey + @"\Resources", Ttas.NAME, 0) != 0); OnTtasClicked(menuItemTtas, null);
            menuItemVers.Checked = ((int)RegistryTools.GetSetting(DbpfViewerApp.RegistryKey + @"\Resources", Vers.NAME, 0) != 0); OnVersClicked(menuItemVers, null);

            menuItemPrettyPrint.Checked = ((int)RegistryTools.GetSetting(DbpfViewerApp.RegistryKey + @"\Options", menuItemPrettyPrint.Name, 1) != 0);

            MyUpdater = new Updater(DbpfViewerApp.RegistryKey, menuHelp);
            MyUpdater.CheckForUpdates();
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            RegistryTools.SaveAppSettings(DbpfViewerApp.RegistryKey, DbpfViewerApp.AppVersionMajor, DbpfViewerApp.AppVersionMinor);
            RegistryTools.SaveFormSettings(DbpfViewerApp.RegistryKey, this);
            RegistryTools.SaveSetting(DbpfViewerApp.RegistryKey, "splitter", splitContainer.SplitterDistance);
        }

        private void OnFileOpening(object sender, EventArgs e)
        {
            menuItemReloadPackage.Enabled = (packageFile != null);
            menuItemSaveXmlToClipboard.Enabled = (packageFile != null);
            menuItemSaveXmlAs.Enabled = (packageFile != null);
        }

        private void OnExitClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnHelpClicked(object sender, EventArgs e)
        {
            new AboutDialog(DbpfViewerApp.AppProduct).ShowDialog();
        }

        private void OnSaveXmlToClipboardClicked(object sender, EventArgs e)
        {
            string xml = GetXMl();

            if (xml != null)
            {
                Clipboard.SetText(xml);
            }
        }

        private void OnSaveXmlAsClicked(object sender, EventArgs e)
        {
            saveXmlDialog.ShowDialog();

            if (saveXmlDialog.FileName != "")
            {
                string xml = GetXMl();

                if (xml != null)
                {
                    StreamWriter writer = new StreamWriter(saveXmlDialog.OpenFile());
                    writer.WriteLine(xml);
                    writer.Close();
                }
            }
        }

        private string GetXMl()
        {
            XmlDocument doc = new XmlDocument();

            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = doc.DocumentElement;
            doc.InsertBefore(xmlDeclaration, root);

            DateTime now = DateTime.Now;
            doc.AppendChild(doc.CreateComment($"{now.ToShortDateString()} {now.ToShortTimeString()}"));

            XmlElement eleDbpf = doc.CreateElement(string.Empty, "dbpf", string.Empty);
            doc.AppendChild(eleDbpf);
            eleDbpf.SetAttribute("file", packageFile);

            ProgressDialog progressDialog = new ProgressDialog(eleDbpf);
            progressDialog.DoWork += new ProgressDialog.DoWorkEventHandler(DoAsyncWork_GetXml);

            DialogResult result = progressDialog.ShowDialog();

            if (result == DialogResult.Abort)
            {
                logger.Error(progressDialog.Result.Error.Message);
                logger.Info(progressDialog.Result.Error.StackTrace);

                MsgBox.Show("An error occured while processing", "Error!", MessageBoxButtons.OK);
            }
            else
            {
                if (result == DialogResult.OK)
                {
                    if (menuItemPrettyPrint.Checked)
                    {
                        return XDocument.Parse(doc.OuterXml).ToString();
                    }
                    else
                    {
                        return doc.OuterXml;
                    }
                }
            }

            return null;
        }

        private void OnConfigurationClicked(object sender, EventArgs e)
        {
            Form config = new ConfigDialog();

            if (config.ShowDialog() == DialogResult.OK)
            {
                // Perform any reload necessary after changing the objects.package location
            }
        }

        private void OnToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            /*
            if (e.RowIndex >= 0)
            {
                int index = e.RowIndex;

                if (index < dbpfData.Rows.Count)
                {
                    DataGridViewRow row = gridResources.Rows[index];

                    if (row.Tag == null)
                    {
                        DBPFResource res = package.GetResourceByEntry(package.GetEntryByFullID((int)dbpfData.Rows[index]["Hash"]));

                        XmlDocument doc = new XmlDocument();

                        XmlElement eleDbpf = doc.CreateElement(string.Empty, "dbpf", string.Empty);
                        doc.AppendChild(eleDbpf);

                        res.AddXml(eleDbpf);

                        row.Tag = XDocument.Parse(doc.DocumentElement.InnerXml).ToString();
                    }


                    e.ToolTipText = row.Tag as String;
                }
            }
            */
        }

        private void OnBconClicked(object sender, EventArgs e)
        {
            bool enabled = ((ToolStripMenuItem)sender).Checked;

            if (enabled)
                enabledResources.Add(Bcon.TYPE);
            else
                enabledResources.Remove(Bcon.TYPE);

            RegistryTools.SaveSetting(DbpfViewerApp.RegistryKey + @"\Resources", Bcon.NAME, enabled ? 1 : 0);
        }

        private void OnBhavClicked(object sender, EventArgs e)
        {
            bool enabled = ((ToolStripMenuItem)sender).Checked;

            if (enabled)
                enabledResources.Add(Bhav.TYPE);
            else
                enabledResources.Remove(Bhav.TYPE);

            RegistryTools.SaveSetting(DbpfViewerApp.RegistryKey + @"\Resources", Bhav.NAME, enabled ? 1 : 0);
        }

        private void OnCtssClicked(object sender, EventArgs e)
        {
            bool enabled = ((ToolStripMenuItem)sender).Checked;

            if (enabled)
                enabledResources.Add(Ctss.TYPE);
            else
                enabledResources.Remove(Ctss.TYPE);

            RegistryTools.SaveSetting(DbpfViewerApp.RegistryKey + @"\Resources", Ctss.NAME, enabled ? 1 : 0);
        }

        private void OnGlobClicked(object sender, EventArgs e)
        {
            bool enabled = ((ToolStripMenuItem)sender).Checked;

            if (enabled)
                enabledResources.Add(Glob.TYPE);
            else
                enabledResources.Remove(Glob.TYPE);

            RegistryTools.SaveSetting(DbpfViewerApp.RegistryKey + @"\Resources", Glob.NAME, enabled ? 1 : 0);
        }

        private void OnImgClicked(object sender, EventArgs e)
        {
            bool enabled = ((ToolStripMenuItem)sender).Checked;

            if (enabled)
                enabledResources.Add(Img.TYPE);
            else
                enabledResources.Remove(Img.TYPE);

            RegistryTools.SaveSetting(DbpfViewerApp.RegistryKey + @"\Resources", Img.NAME, enabled ? 1 : 0);
        }

        private void OnJpgClicked(object sender, EventArgs e)
        {
            bool enabled = ((ToolStripMenuItem)sender).Checked;

            if (enabled)
                enabledResources.Add(Jpg.TYPE);
            else
                enabledResources.Remove(Jpg.TYPE);

            RegistryTools.SaveSetting(DbpfViewerApp.RegistryKey + @"\Resources", Jpg.NAME, enabled ? 1 : 0);
        }

        private void OnObjdClicked(object sender, EventArgs e)
        {
            bool enabled = ((ToolStripMenuItem)sender).Checked;

            if (enabled)
                enabledResources.Add(Objd.TYPE);
            else
                enabledResources.Remove(Objd.TYPE);

            RegistryTools.SaveSetting(DbpfViewerApp.RegistryKey + @"\Resources", Objd.NAME, enabled ? 1 : 0);
        }

        private void OnObjfClicked(object sender, EventArgs e)
        {
            bool enabled = ((ToolStripMenuItem)sender).Checked;

            if (enabled)
                enabledResources.Add(Objf.TYPE);
            else
                enabledResources.Remove(Objf.TYPE);

            RegistryTools.SaveSetting(DbpfViewerApp.RegistryKey + @"\Resources", Objf.NAME, enabled ? 1 : 0);
        }

        private void OnNrefClicked(object sender, EventArgs e)
        {
            bool enabled = ((ToolStripMenuItem)sender).Checked;

            if (enabled)
                enabledResources.Add(Nref.TYPE);
            else
                enabledResources.Remove(Nref.TYPE);

            RegistryTools.SaveSetting(DbpfViewerApp.RegistryKey + @"\Resources", Nref.NAME, enabled ? 1 : 0);
        }

        private void OnSlotClicked(object sender, EventArgs e)
        {
            bool enabled = ((ToolStripMenuItem)sender).Checked;

            if (enabled)
                enabledResources.Add(Slot.TYPE);
            else
                enabledResources.Remove(Slot.TYPE);

            RegistryTools.SaveSetting(DbpfViewerApp.RegistryKey + @"\Resources", Slot.NAME, enabled ? 1 : 0);
        }

        private void OnStrClicked(object sender, EventArgs e)
        {
            bool enabled = ((ToolStripMenuItem)sender).Checked;

            if (enabled)
                enabledResources.Add(Str.TYPE);
            else
                enabledResources.Remove(Str.TYPE);

            RegistryTools.SaveSetting(DbpfViewerApp.RegistryKey + @"\Resources", Str.NAME, enabled ? 1 : 0);
        }

        private void OnTprpClicked(object sender, EventArgs e)
        {
            bool enabled = ((ToolStripMenuItem)sender).Checked;

            if (enabled)
                enabledResources.Add(Tprp.TYPE);
            else
                enabledResources.Remove(Tprp.TYPE);

            RegistryTools.SaveSetting(DbpfViewerApp.RegistryKey + @"\Resources", Tprp.NAME, enabled ? 1 : 0);
        }

        private void OnTrcnClicked(object sender, EventArgs e)
        {
            bool enabled = ((ToolStripMenuItem)sender).Checked;

            if (enabled)
                enabledResources.Add(Trcn.TYPE);
            else
                enabledResources.Remove(Trcn.TYPE);

            RegistryTools.SaveSetting(DbpfViewerApp.RegistryKey + @"\Resources", Trcn.NAME, enabled ? 1 : 0);
        }

        private void OnTtabClicked(object sender, EventArgs e)
        {
            bool enabled = ((ToolStripMenuItem)sender).Checked;

            if (enabled)
                enabledResources.Add(Ttab.TYPE);
            else
                enabledResources.Remove(Ttab.TYPE);

            RegistryTools.SaveSetting(DbpfViewerApp.RegistryKey + @"\Resources", Ttab.NAME, enabled ? 1 : 0);
        }

        private void OnTtasClicked(object sender, EventArgs e)
        {
            bool enabled = ((ToolStripMenuItem)sender).Checked;

            if (enabled)
                enabledResources.Add(Ttas.TYPE);
            else
                enabledResources.Remove(Ttas.TYPE);

            RegistryTools.SaveSetting(DbpfViewerApp.RegistryKey + @"\Resources", Ttas.NAME, enabled ? 1 : 0);
        }

        private void OnVersClicked(object sender, EventArgs e)
        {
            bool enabled = ((ToolStripMenuItem)sender).Checked;

            if (enabled)
                enabledResources.Add(Vers.TYPE);
            else
                enabledResources.Remove(Vers.TYPE);

            RegistryTools.SaveSetting(DbpfViewerApp.RegistryKey + @"\Resources", Vers.NAME, enabled ? 1 : 0);
        }

        private void MyMruList_FileSelected(string package)
        {
            DoWork_FillGrid(package);
        }

        private void OnReloadClicked(object sender, EventArgs e)
        {
            DoWork_FillGrid(packageFile);
        }

        private void OnSelectClicked(object sender, EventArgs e)
        {
            selectFileDialog.FileName = "*.package";
            if (selectFileDialog.ShowDialog() == DialogResult.OK)
            {
                DoWork_FillGrid(selectFileDialog.FileName);
            }
        }

        private void DoWork_FillGrid(string packageFile)
        {
            this.packageFile = packageFile;

            this.Text = $"{DbpfViewerApp.AppTitle} - {(new FileInfo(packageFile)).Name}";
            menuItemReloadPackage.Enabled = false;
            menuItemSelectPackage.Enabled = false;
            menuItemRecentPackages.Enabled = false;

            dbpfData.Clear();

            ProgressDialog progressDialog = new ProgressDialog();
            progressDialog.DoWork += new ProgressDialog.DoWorkEventHandler(DoAsyncWork_FillGrid);
            progressDialog.DoData += new ProgressDialog.DoWorkEventHandler(DoAsyncWork_FillGrid_Data);

            DialogResult result = progressDialog.ShowDialog();

            menuItemRecentPackages.Enabled = true;
            menuItemSelectPackage.Enabled = true;
            menuItemReloadPackage.Enabled = true;

            if (result == DialogResult.Abort)
            {
                MyMruList.RemoveFile(packageFile);

                logger.Error(progressDialog.Result.Error.Message);
                logger.Info(progressDialog.Result.Error.StackTrace);

                MsgBox.Show("An error occured while processing", "Error!", MessageBoxButtons.OK);
            }
            else
            {
                MyMruList.AddFile(packageFile);

                if (result == DialogResult.Cancel)
                {
                }
                else
                {
                }
            }
        }

        private void DoAsyncWork_FillGrid(ProgressDialog sender, DoWorkEventArgs args)
        {
            // object myArgument = args.Argument; // As passed to the Sims2ToolsProgressDialog constructor

            sender.VisualMode = ProgressBarDisplayMode.CustomText;
            sender.SetProgress(0, "Loading Objects");

            try
            {
                using (DBPFFile package = new DBPFFile(packageFile))
                {
                    localObjectsByGroupID.Clear();

                    try
                    {
                        GameData.BuildObjectsTable(package, localObjectsByGroupID, null);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.Message);
                        logger.Info(ex.StackTrace);
                    }

                    sender.VisualMode = ProgressBarDisplayMode.Percentage;

                    uint total = package.ResourceCount;
                    uint done = 0;
                    uint found = 0;

                    foreach (TypeTypeID type in DBPFData.AllTypes)
                    {
                        if (enabledResources.Contains(type))
                        {
                            List<DBPFEntry> resources = package.GetEntriesByType(type);

                            foreach (var entry in resources)
                            {
                                if (sender.CancellationPending)
                                {
                                    args.Cancel = true;
                                    return;
                                }

                                DBPFResource resource = package.GetResourceByEntry(entry);

                                if (resource != null)
                                {
                                    DataRow row = dbpfData.NewRow();
                                    row["Type"] = DBPFData.TypeName(type);
                                    row["Group"] = GameData.GroupName(entry.GroupID, localObjectsByGroupID);
                                    row["Instance"] = entry.InstanceID.ToString();
                                    row["Name"] = resource.KeyName;

                                    row["Hash"] = Hash.TGIRHash(entry.InstanceID, entry.ResourceID, entry.TypeID, entry.GroupID);

                                    sender.SetData(row);
                                    sender.SetProgress((int)((++done / (float)total) * 100.0));

                                    ++found;
                                }
                            }
                        }
                    }

                    package.Close();

                    args.Result = found;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Info(ex.StackTrace);

                if (MsgBox.Show($"An error occured while processing\n{packageFile}\n\nReason: {ex.Message}", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                {
                    throw ex;
                }
            }
        }

        private void DoAsyncWork_FillGrid_Data(ProgressDialog sender, DoWorkEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { DoAsyncWork_FillGrid_Data(sender, e); });
                return;
            }

            // This will be run on main (UI) thread 
            dbpfData.Append(e.Argument as DataRow);
        }

        private void DoAsyncWork_GetXml(ProgressDialog sender, DoWorkEventArgs args)
        {
            XmlElement eleDbpf = args.Argument as XmlElement;

            sender.VisualMode = ProgressBarDisplayMode.Percentage;

            try
            {
                using (DBPFFile package = new DBPFFile(packageFile))
                {
                    uint total = package.ResourceCount;
                    uint done = 0;

                    foreach (TypeTypeID type in DBPFData.AllTypes)
                    {
                        if (enabledResources.Contains(type))
                        {
                            List<DBPFEntry> resources = package.GetEntriesByType(type);
                            SortedDictionary<DBPFKey, DBPFEntry> sortedResources = new SortedDictionary<DBPFKey, DBPFEntry>();

                            foreach (var entry in resources)
                            {
                                try
                                {
                                    sortedResources.Add(entry, entry);
                                }
                                catch (Exception)
                                {
                                    MsgBox.Show($"The resource {entry} is duplicated - second occurrence has been ignored", "Duplicate Resource Found");
                                }
                            }

                            foreach (var entry in sortedResources.Values)
                            {
                                if (sender.CancellationPending)
                                {
                                    args.Cancel = true;
                                    return;
                                }

                                DBPFResource resource = package.GetResourceByEntry(entry);

                                if (resource != null)
                                {
                                    resource.AddXml(eleDbpf);
                                    sender.SetProgress((int)((++done / (float)total) * 100.0));
                                }
                            }
                        }
                    }

                    package.Close();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Info(ex.StackTrace);

                if (MsgBox.Show($"An error occured while processing\n{packageFile}\n\nReason: {ex.Message}", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                {
                    throw ex;
                }
            }
        }

        private void OnCellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = (sender as DataGridView).Rows[e.RowIndex];

            using (DBPFFile package = new DBPFFile(packageFile))
            {
                DBPFEntry entry = package.GetEntryByTGIR((int)row.Cells[4].Value);

                if (entry != null)
                {
                    DBPFResource res = package.GetResourceByEntry(entry);

                    if (DBPFData.IsKnownImgType(entry.TypeID))
                    {
                        Img img = (res as Img);

                        pictName = res.InstanceID.ToString();
                        pictImage.Image = img.Image;
                        pictImage.Visible = true;
                        panelImage.Visible = true;
                        textXml.Visible = false;
                    }
                    else
                    {
                        XmlDocument doc = new XmlDocument();
                        XmlElement eleDbpf = doc.CreateElement(string.Empty, "dbpf", string.Empty);
                        doc.AppendChild(eleDbpf);

                        res.AddXml(eleDbpf);

                        string xml;
                        if (menuItemPrettyPrint.Checked)
                        {
                            xml = XDocument.Parse(eleDbpf.InnerXml).ToString();
                        }
                        else
                        {
                            xml = eleDbpf.InnerXml;
                        }

                        textXml.Text = xml;
                        textXml.Visible = true;
                        pictImage.Visible = false;
                        panelImage.Visible = false;
                    }

                    if (splitContainer.Panel2Collapsed)
                    {
                        splitContainer.Panel2Collapsed = false;
                        splitContainer.SplitterDistance = (int)RegistryTools.GetSetting(DbpfViewerApp.RegistryKey, "splitter", splitContainer.SplitterDistance);
                    }
                }

                package.Close();
            }
        }

        private void OnPrettyPrintClicked(object sender, EventArgs e)
        {
            bool enabled = ((ToolStripMenuItem)sender).Checked;

            RegistryTools.SaveSetting(DbpfViewerApp.RegistryKey + @"\Options", menuItemPrettyPrint.Name, enabled ? 1 : 0);
        }

        private void OnNoneClicked(object sender, EventArgs e)
        {
            if (menuItemBcon.Checked) { menuItemBcon.Checked = false; OnBconClicked(menuItemBcon, null); }
            if (menuItemBhav.Checked) { menuItemBhav.Checked = false; OnBhavClicked(menuItemBhav, null); }
            if (menuItemCtss.Checked) { menuItemCtss.Checked = false; OnCtssClicked(menuItemCtss, null); }
            if (menuItemGlob.Checked) { menuItemGlob.Checked = false; OnGlobClicked(menuItemGlob, null); }
            if (menuItemImg.Checked) { menuItemImg.Checked = false; OnImgClicked(menuItemImg, null); }
            if (menuItemJpg.Checked) { menuItemJpg.Checked = false; OnJpgClicked(menuItemJpg, null); }
            if (menuItemObjd.Checked) { menuItemObjd.Checked = false; OnObjdClicked(menuItemObjd, null); }
            if (menuItemObjf.Checked) { menuItemObjf.Checked = false; OnObjfClicked(menuItemObjf, null); }
            if (menuItemNref.Checked) { menuItemNref.Checked = false; OnNrefClicked(menuItemNref, null); }
            if (menuItemSlot.Checked) { menuItemSlot.Checked = false; OnSlotClicked(menuItemSlot, null); }
            if (menuItemStr.Checked) { menuItemStr.Checked = false; OnStrClicked(menuItemStr, null); }
            if (menuItemTprp.Checked) { menuItemTprp.Checked = false; OnTprpClicked(menuItemTprp, null); }
            if (menuItemTrcn.Checked) { menuItemTrcn.Checked = false; OnTrcnClicked(menuItemTrcn, null); }
            if (menuItemTtab.Checked) { menuItemTtab.Checked = false; OnTtabClicked(menuItemTtab, null); }
            if (menuItemTtas.Checked) { menuItemTtas.Checked = false; OnTtasClicked(menuItemTtas, null); }
            if (menuItemVers.Checked) { menuItemVers.Checked = false; OnVersClicked(menuItemVers, null); }
        }

        private void OnAllClicked(object sender, EventArgs e)
        {
            if (!menuItemBcon.Checked) { menuItemBcon.Checked = true; OnBconClicked(menuItemBcon, null); }
            if (!menuItemBhav.Checked) { menuItemBhav.Checked = true; OnBhavClicked(menuItemBhav, null); }
            if (!menuItemCtss.Checked) { menuItemCtss.Checked = true; OnCtssClicked(menuItemCtss, null); }
            if (!menuItemGlob.Checked) { menuItemGlob.Checked = true; OnGlobClicked(menuItemGlob, null); }
            if (!menuItemImg.Checked) { menuItemImg.Checked = true; OnImgClicked(menuItemImg, null); }
            if (!menuItemJpg.Checked) { menuItemJpg.Checked = true; OnJpgClicked(menuItemJpg, null); }
            if (!menuItemObjd.Checked) { menuItemObjd.Checked = true; OnObjdClicked(menuItemObjd, null); }
            if (!menuItemObjf.Checked) { menuItemObjf.Checked = true; OnObjfClicked(menuItemObjf, null); }
            if (!menuItemNref.Checked) { menuItemNref.Checked = true; OnNrefClicked(menuItemNref, null); }
            if (!menuItemSlot.Checked) { menuItemSlot.Checked = true; OnSlotClicked(menuItemSlot, null); }
            if (!menuItemStr.Checked) { menuItemStr.Checked = true; OnStrClicked(menuItemStr, null); }
            if (!menuItemTprp.Checked) { menuItemTprp.Checked = true; OnTprpClicked(menuItemTprp, null); }
            if (!menuItemTrcn.Checked) { menuItemTrcn.Checked = true; OnTrcnClicked(menuItemTrcn, null); }
            if (!menuItemTtab.Checked) { menuItemTtab.Checked = true; OnTtabClicked(menuItemTtab, null); }
            if (!menuItemTtas.Checked) { menuItemTtas.Checked = true; OnTtasClicked(menuItemTtas, null); }
            if (!menuItemVers.Checked) { menuItemVers.Checked = true; OnVersClicked(menuItemVers, null); }
        }

        private void OnCopyImageClicked(object sender, EventArgs e)
        {
            Clipboard.SetImage(pictImage.Image);
        }

        private DataGridViewCellEventArgs mouseLocation = null;
        DataGridViewRow highlightRow = null;

        private void OnCellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            mouseLocation = e;
        }

        private void OnContextMenuOpening(object sender, CancelEventArgs e)
        {
            if (mouseLocation == null || mouseLocation.RowIndex == -1)
            {
                e.Cancel = true;
                return;
            }

            if (mouseLocation.RowIndex != gridResources.SelectedRows[0].Index)
            {
                highlightRow = gridResources.Rows[mouseLocation.RowIndex];
                highlightRow.DefaultCellStyle.BackColor = Color.FromName(Properties.Settings.Default.SaveRawHighlight); // MistyRose or LightPink
            }
            else
            {
                highlightRow = null;
            }
        }

        private void OnContextMenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            if (highlightRow != null)
            {
                highlightRow.DefaultCellStyle.BackColor = Color.Empty;
            }
        }

        private void OnSaveRawDataClicked(object sender, EventArgs e)
        {
            DoImageSaveFromGrid(null);
        }

        private void OnSaveAsJpegClicked(object sender, EventArgs e)
        {
            DoImageSaveFromGrid("jpg");
        }

        private void OnSaveAsPngClicked(object sender, EventArgs e)
        {
            DoImageSaveFromGrid("png");
        }

        private void DoImageSaveFromGrid(string type)
        {
            if (mouseLocation.RowIndex >= 0)
            {
                DataGridViewRow row = gridResources.Rows[mouseLocation.RowIndex];

                using (DBPFFile package = new DBPFFile(packageFile))
                {
                    DBPFEntry entry = package.GetEntryByTGIR((int)row.Cells[4].Value);
                    byte[] data = package.GetOriginalItemByEntry(entry);

                    if (data != null)
                    {
                        string typeName;
                        if (type == null)
                        {
                            typeName = DBPFData.TypeName(entry.TypeID);
                        }
                        else
                        {
                            typeName = type.ToUpper();
                        }

                        if (type == null)
                        {
                            DoImageSave(null, entry.InstanceID.ToString(), type, typeName, null);
                        }
                        else
                        {
                            Img img = (Img)package.GetResourceByEntry(entry);
                            DoImageSave(img.Image, entry.InstanceID.ToString(), type, typeName, null);
                        }
                    }

                    package.Close();
                }
            }
        }

        private void DoImageSave(Image image, string name, string type, string typeName, byte[] data)
        {
            saveRawDialog.DefaultExt = typeName.ToLower();
            saveRawDialog.Filter = $"{typeName} file|*.{typeName.ToLower()}|All files|*.*";
            saveRawDialog.FileName = $"{name}.{typeName.ToLower()}";

            saveRawDialog.ShowDialog();

            if (saveRawDialog.FileName != "")
            {
                using (Stream stream = saveRawDialog.OpenFile())
                {
                    if (type == null)
                    {
                        stream.Write(data, 0, data.Length);
                    }
                    else
                    {
                        if (type.ToLower().Equals("png"))
                        {
                            image.Save(stream, ImageFormat.Png);
                        }
                        else
                        {
                            image.Save(stream, ImageFormat.Jpeg);
                        }
                    }

                    stream.Close();
                }
            }
        }

        private void OnSaveJpegClicked(object sender, EventArgs e)
        {
            DoImageSave(pictImage.Image, pictName, "jpg", "JPG", null);
        }

        private void OnSavePngClicked(object sender, EventArgs e)
        {
            DoImageSave(pictImage.Image, pictName, "png", "PNG", null);
        }
    }
}
