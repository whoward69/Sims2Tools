/*
 * DBPF Test - a utility for testing the DBPF Library
 *
 * William Howard - 2020
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.Controls;
using Sims2Tools.DBPF.BCON;
using Sims2Tools.DBPF.BHAV;
using Sims2Tools.DBPF.CTSS;
using Sims2Tools.DBPF.GLOB;
using Sims2Tools.DBPF.OBJD;
using Sims2Tools.DBPF.OBJF;
using Sims2Tools.DBPF.STR;
using Sims2Tools.DBPF.TPRP;
using Sims2Tools.DBPF.TRCN;
using Sims2Tools.DBPF.TTAB;
using Sims2Tools.DBPF.TTAS;
using Sims2Tools.DBPF.Utils;
using Sims2Tools.DBPF.VERS;
using Sims2Tools.Utils.Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Sims2Tools.DBPF
{
    public partial class DbpfViewerForm : Form
    {
        private DBPFFile package = null;
        private String packageFile = null;
        private readonly SortedDictionary<String, String> localObjectsByGroupID = new SortedDictionary<string, string>();

        private MruList MyMruList;

        private readonly HashSet<uint> enabledResources = new HashSet<uint>();

        private readonly DbpfViewerData dbpfData = new DbpfViewerData();

        public DbpfViewerForm()
        {
            InitializeComponent();
            this.Text = DbpfViewerApp.AppName;

            gridResources.DataSource = dbpfData;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            RegistryTools.LoadAppSettings(DbpfViewerApp.RegistryKey, DbpfViewerApp.AppVersionMajor, DbpfViewerApp.AppVersionMinor);
            RegistryTools.LoadFormSettings(DbpfViewerApp.RegistryKey, this);

            MyMruList = new MruList(DbpfViewerApp.RegistryKey, menuItemRecentPackages, 8);
            MyMruList.FileSelected += MyMruList_FileSelected;

            menuItemBcon.Checked = ((int)RegistryTools.GetSetting(DbpfViewerApp.RegistryKey + @"\Resources", Bcon.NAME, 1) != 0); OnBconClicked(menuItemBcon, null);
            menuItemBhav.Checked = ((int)RegistryTools.GetSetting(DbpfViewerApp.RegistryKey + @"\Resources", Bhav.NAME, 1) != 0); OnBhavClicked(menuItemBhav, null);
            menuItemCtss.Checked = ((int)RegistryTools.GetSetting(DbpfViewerApp.RegistryKey + @"\Resources", Ctss.NAME, 0) != 0); OnCtssClicked(menuItemCtss, null);
            menuItemGlob.Checked = ((int)RegistryTools.GetSetting(DbpfViewerApp.RegistryKey + @"\Resources", Glob.NAME, 0) != 0); OnGlobClicked(menuItemGlob, null);
            menuItemObjd.Checked = ((int)RegistryTools.GetSetting(DbpfViewerApp.RegistryKey + @"\Resources", Objd.NAME, 0) != 0); OnObjdClicked(menuItemObjd, null);
            menuItemObjf.Checked = ((int)RegistryTools.GetSetting(DbpfViewerApp.RegistryKey + @"\Resources", Objf.NAME, 0) != 0); OnObjfClicked(menuItemObjf, null);
            menuItemStr.Checked = ((int)RegistryTools.GetSetting(DbpfViewerApp.RegistryKey + @"\Resources", Str.NAME, 0) != 0); OnStrClicked(menuItemStr, null);
            menuItemTprp.Checked = ((int)RegistryTools.GetSetting(DbpfViewerApp.RegistryKey + @"\Resources", Tprp.NAME, 0) != 0); OnTprpClicked(menuItemTprp, null);
            menuItemTrcn.Checked = ((int)RegistryTools.GetSetting(DbpfViewerApp.RegistryKey + @"\Resources", Trcn.NAME, 0) != 0); OnTrcnClicked(menuItemTrcn, null);
            menuItemTtab.Checked = ((int)RegistryTools.GetSetting(DbpfViewerApp.RegistryKey + @"\Resources", Ttab.NAME, 0) != 0); OnTtabClicked(menuItemTtab, null);
            menuItemTtas.Checked = ((int)RegistryTools.GetSetting(DbpfViewerApp.RegistryKey + @"\Resources", Ttas.NAME, 0) != 0); OnTtasClicked(menuItemTtas, null);
            menuItemVers.Checked = ((int)RegistryTools.GetSetting(DbpfViewerApp.RegistryKey + @"\Resources", Vers.NAME, 0) != 0); OnVersClicked(menuItemVers, null);
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            RegistryTools.SaveAppSettings(DbpfViewerApp.RegistryKey, DbpfViewerApp.AppVersionMajor, DbpfViewerApp.AppVersionMinor);
            RegistryTools.SaveFormSettings(DbpfViewerApp.RegistryKey, this);
        }

        private void OnFileOpening(object sender, EventArgs e)
        {
            menuItemSaveXmlToClipboard.Enabled = (package != null);
            menuItemSaveXmlAs.Enabled = (package != null);
        }

        private void OnExitClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnHelpClicked(object sender, EventArgs e)
        {
            new Sims2ToolsAboutDialog(DbpfViewerApp.AppProduct).ShowDialog();
        }

        private void OnSaveXmlToClipboardClicked(object sender, EventArgs e)
        {
            String xml = GetXMl();

            if (xml != null)
            {
                Clipboard.SetText(GetXMl());
            }
        }

        private void OnSaveXmlAsClicked(object sender, EventArgs e)
        {
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName != "")
            {
                String xml = GetXMl();

                if (xml != null)
                {
                    StreamWriter writer = new StreamWriter(saveFileDialog.OpenFile());
                    writer.WriteLine(xml);
                    writer.Close();
                }
            }
        }

        private String GetXMl()
        {
            XmlDocument doc = new XmlDocument();

            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = doc.DocumentElement;
            doc.InsertBefore(xmlDeclaration, root);

            DateTime now = DateTime.Now;
            doc.AppendChild(doc.CreateComment(now.ToShortDateString() + " " + now.ToShortTimeString()));

            XmlElement eleDbpf = doc.CreateElement(string.Empty, "dbpf", string.Empty);
            doc.AppendChild(eleDbpf);
            eleDbpf.SetAttribute("file", packageFile);

            Sims2ToolsProgressDialog progressDialog = new Sims2ToolsProgressDialog(eleDbpf);
            progressDialog.DoWork += new Sims2ToolsProgressDialog.DoWorkEventHandler(DoAsyncWork_GetXml);

            DialogResult result = progressDialog.ShowDialog();

            if (result == DialogResult.Abort)
            {
                MessageBox.Show("An error occured while processing", "Error!", MessageBoxButtons.OK);
#if DEBUG
                Console.WriteLine(progressDialog.Result.Error.Message);
#endif
            }
            else
            {
                if (result == DialogResult.OK)
                {
                    return XDocument.Parse(doc.OuterXml).ToString();
                }
            }

            return null;
        }

        private void OnConfigurationClicked(object sender, EventArgs e)
        {
            Form config = new Sims2ToolsConfigDialog();

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

        private void MyMruList_FileSelected(String package)
        {
            DoWork_FillGrid(package);
        }

        private void OnSelectClicked(object sender, EventArgs e)
        {
            if (selectFileDialog.ShowDialog() == DialogResult.OK)
            {
                DoWork_FillGrid(selectFileDialog.FileName);
            }
        }

        private void DoWork_FillGrid(String packageFile)
        {
            this.packageFile = packageFile;

            this.Text = DbpfViewerApp.AppName + " - " + (new FileInfo(packageFile)).Name;
            menuItemSelectPackage.Enabled = false;

            dbpfData.Clear();

            Sims2ToolsProgressDialog progressDialog = new Sims2ToolsProgressDialog();
            progressDialog.DoWork += new Sims2ToolsProgressDialog.DoWorkEventHandler(DoAsyncWork_FillGrid);
            progressDialog.DoData += new Sims2ToolsProgressDialog.DoWorkEventHandler(DoAsyncWork_FillGrid_Data);

            DialogResult result = progressDialog.ShowDialog();

            menuItemSelectPackage.Enabled = true;

            if (result == DialogResult.Abort)
            {
                MyMruList.RemoveFile(packageFile);

                MessageBox.Show("An error occured while processing", "Error!", MessageBoxButtons.OK);
#if DEBUG
                Console.WriteLine(progressDialog.Result.Error.Message);
#endif
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

        private void DoAsyncWork_FillGrid(Sims2ToolsProgressDialog sender, DoWorkEventArgs e)
        {
            object myArgument = e.Argument; // As passed to the Sims2ToolsProgressDialog constructor

            sender.VisualMode = ProgressBarDisplayMode.CustomText;
            sender.SetProgress(0, "Loading Objects");

            package = new DBPFFile(packageFile);

            localObjectsByGroupID.Clear();

            try
            {
                GameData.BuildObjectsTable(package, localObjectsByGroupID);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

#if DEBUG
            Thread.Sleep(500);
#endif

            sender.VisualMode = ProgressBarDisplayMode.Percentage;

            uint total = package.NumEntries;
            uint done = 0;
            uint found = 0;

            foreach (uint type in DBPFData.Types)
            {
                if (enabledResources.Contains(type))
                {
                    List<DBPFEntry> resources = package.GetEntriesByType(type);

                    foreach (var entry in resources)
                    {
                        if (sender.CancellationPending)
                        {
                            e.Cancel = true;
                            return;
                        }

                        DBPFResource resource = package.GetResourceByEntry(entry);

                        if (resource != null)
                        {
                            DataRow row = dbpfData.NewRow();
                            row["Type"] = DBPFData.TypeName(type);
                            row["Group"] = GameData.GroupName(entry.GroupID, localObjectsByGroupID);
                            row["Instance"] = Helper.Hex4PrefixString(entry.InstanceID);
                            row["Name"] = resource.FileName;

                            row["Hash"] = Hash.TGIRHash(entry.InstanceID, entry.InstanceID2, entry.TypeID, entry.GroupID);

                            sender.SetData(row);
                            sender.SetProgress((int)((++done / (float)total) * 100.0));

                            ++found;
                        }
                    }
                }
            }

            e.Result = found;
        }

        private void DoAsyncWork_FillGrid_Data(Sims2ToolsProgressDialog sender, DoWorkEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { DoAsyncWork_FillGrid_Data(sender, e); });
                return;
            }

            // This will be run on main (UI) thread 
            dbpfData.Append(e.Argument as DataRow);
        }

        private void DoAsyncWork_GetXml(Sims2ToolsProgressDialog sender, DoWorkEventArgs e)
        {
            XmlElement eleDbpf = e.Argument as XmlElement;

            sender.VisualMode = ProgressBarDisplayMode.Percentage;

            uint total = package.NumEntries;
            uint done = 0;

            foreach (uint type in DBPFData.Types)
            {
                if (enabledResources.Contains(type))
                {
                    List<DBPFEntry> resources = package.GetEntriesByType(type);

                    foreach (var entry in resources)
                    {
                        if (sender.CancellationPending)
                        {
                            e.Cancel = true;
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
        }
    }
}
