/*
 * BSOK Editor - a utility for adding BSOK data to clothing and accessory packages
 *
 * William Howard - 2020-2024
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

#region Usings
using Microsoft.WindowsAPICodePack.Dialogs;
using Sims2Tools;
using Sims2Tools.Controls;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.Cigen;
using Sims2Tools.DBPF.CPF;
using Sims2Tools.DBPF.Data;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph.BINX;
using Sims2Tools.DBPF.SceneGraph.GZPS;
using Sims2Tools.DBPF.SceneGraph.IDR;
using Sims2Tools.DBPF.SceneGraph.XMOL;
using Sims2Tools.DBPF.SceneGraph.XTOL;
using Sims2Tools.DBPF.STR;
using Sims2Tools.DBPF.Utils;
using Sims2Tools.Dialogs;
using Sims2Tools.Updates;
using Sims2Tools.Utils.NamedValue;
using Sims2Tools.Utils.Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
#endregion

namespace BsokEditor
{
    public partial class BsokEditorForm : Form
    {
        private static readonly Sims2Tools.DBPF.Logger.IDBPFLogger logger = Sims2Tools.DBPF.Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private CigenFile cigenCache = null;

        private string folder = null;

        private MruList MyMruList;
        private Updater MyUpdater;

        private readonly ResourcesDataTable dataTableResources = new ResourcesDataTable();
        private readonly XmlElement bsokXml;

        private bool dataLoading = false;
        private bool ignoreEdits = false;

        private bool IsAutoUpdate => (!dataLoading && !ignoreEdits);

        #region Constructor and Dispose
        public BsokEditorForm()
        {
            logger.Info(BsokEditorApp.AppProduct);

            InitializeComponent();
            this.Text = BsokEditorApp.AppTitle;

            selectPathDialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true
            };

            {
                dataLoading = true;

                bsokXml = ProcessConfig()?.DocumentElement;

                LoadBsokProductComboBoxes();

                comboGender.Items.Clear();
                comboGender.Items.AddRange(new UintNamedValue[] {
                    new UintNamedValue("", 0),
                    new UintNamedValue("Female", 1),
                    new UintNamedValue("Male", 2),
                    new UintNamedValue("Unisex", 3),
                });

                comboShoe.Items.Clear();
                comboShoe.Items.AddRange(new UintNamedValue[] {
                    new UintNamedValue("", 0),
                    new UintNamedValue("Armour", 7),
                    new UintNamedValue("Barefoot", 1),
                    new UintNamedValue("Heels", 3),
                    new UintNamedValue("Heavy Boot", 2),
                    new UintNamedValue("Normal Shoe", 4),
                    new UintNamedValue("Sandal", 5),
                });

                dataLoading = false;
            }

            gridViewResources.DataSource = dataTableResources;

            if (Sims2ToolsLib.IsSims2HomePathSet)
            {
                string cigenPath = $"{Sims2ToolsLib.Sims2HomePath}\\cigen.package";

                cigenCache = new CigenFile(cigenPath);
            }
        }

        public new void Dispose()
        {
            if (cigenCache != null)
            {
                cigenCache.Close();
                cigenCache = null;
            }

            base.Dispose();
        }
        #endregion

        #region XML Config
        private XmlDocument ProcessConfig()
        {
            string[] bsokXmlFiles = Directory.GetFiles("Resources/XML", "bsok_*.xml", SearchOption.TopDirectoryOnly);

            if (bsokXmlFiles.Length > 0)
            {
                return MergeAllConfigs(bsokXmlFiles);
            }
            else
            {
                return LoadConfigFile("Resources/XML/bsok.xml");
            }

        }

        private XmlDocument MergeAllConfigs(string[] bsokXmlFiles)
        {
            XmlDocument bsokXmlDoc = LoadConfigFile(bsokXmlFiles[0]);

            if (bsokXmlDoc.DocumentElement != null)
            {
                for (int i = 1; i < bsokXmlFiles.Length; ++i)
                {
                    MergeConfig(bsokXmlDoc, bsokXmlFiles[i]);
                }
            }

            return bsokXmlDoc;
        }

        private void MergeConfig(XmlDocument bsokXmlDoc, string bsokXmlFile)
        {
            XmlDocument bsokDoc = LoadConfigFile(bsokXmlFile); ;

            if (bsokDoc.DocumentElement != null)
            {
                foreach (XmlNode genreNode in bsokDoc.DocumentElement.ChildNodes)
                {
                    foreach (XmlNode styleNode in genreNode.ChildNodes)
                    {
                        foreach (XmlNode groupNode in styleNode.ChildNodes)
                        {
                            foreach (XmlNode shapeNode in groupNode.ChildNodes)
                            {
                                MergeNode(bsokXmlDoc, genreNode, styleNode, groupNode, shapeNode);
                            }
                        }
                    }
                }
            }
        }

        private void MergeNode(XmlDocument bsokXmlDoc, XmlNode genreNode, XmlNode styleNode, XmlNode groupNode, XmlNode shapeNode)
        {
            XmlNode mergeGenreNode = GetOrAddNode(bsokXmlDoc, bsokXmlDoc.DocumentElement, genreNode);
            XmlNode mergeStyleNode = GetOrAddNode(bsokXmlDoc, mergeGenreNode, styleNode);
            XmlNode mergeGroupNode = GetOrAddNode(bsokXmlDoc, mergeStyleNode, groupNode);

            XmlNode node = mergeGroupNode.SelectSingleNode($"./{shapeNode.LocalName}[@name='{shapeNode.Attributes.GetNamedItem("name")}']");

            if (node != null)
            {
                logger.Warn($"Genre:{genreNode.Attributes.GetNamedItem("name")}, Style:{styleNode.Attributes.GetNamedItem("name")}, Group:{groupNode.Attributes.GetNamedItem("name")} already defines Shape:{shapeNode.Attributes.GetNamedItem("name")}");
            }

            mergeGroupNode.AppendChild(bsokXmlDoc.ImportNode(shapeNode, true));
        }

        private XmlNode GetOrAddNode(XmlDocument bsokXmlDoc, XmlNode parent, XmlNode child)
        {
            string predicate = $"@name='{child.Attributes.GetNamedItem("name").Value}'";

            string gender = child.Attributes.GetNamedItem("gender")?.Value;

            if (gender != null)
            {
                predicate = $"{predicate} and @gender='{gender}'";
            }

            string xpath = $"./{child.LocalName}[{predicate}]";

            XmlNode node = parent.SelectSingleNode(xpath) ?? parent.AppendChild(bsokXmlDoc.ImportNode(child, false));

            return node;
        }

        private XmlDocument LoadConfigFile(string bsokXmlFile)
        {
            XmlDocument bsokXmlDoc = new XmlDocument();

            try
            {
                bsokXmlDoc.Load(bsokXmlFile);
                logger.Info($"Loaded config {bsokXmlFile}");
            }
            catch (Exception e)
            {
                logger.Warn($"Config {bsokXmlFile} is invalid!", e);
            }

            return bsokXmlDoc;
        }
        #endregion

        #region Form Management
        private void OnLoad(object sender, EventArgs e)
        {
            RegistryTools.LoadAppSettings(BsokEditorApp.RegistryKey, BsokEditorApp.AppVersionMajor, BsokEditorApp.AppVersionMinor);
            RegistryTools.LoadFormSettings(BsokEditorApp.RegistryKey, this);

            MyMruList = new MruList(BsokEditorApp.RegistryKey, menuItemRecentFolders, Properties.Settings.Default.MruSize, false, true);
            MyMruList.FileSelected += MyMruList_FolderSelected;

            menuItemExcludeUnknown.Checked = ((int)RegistryTools.GetSetting(BsokEditorApp.RegistryKey + @"\Options", menuItemExcludeUnknown.Name, 0) != 0);
            menuItemShowNakedCategory.Checked = ((int)RegistryTools.GetSetting(BsokEditorApp.RegistryKey + @"\Options", menuItemShowNakedCategory.Name, 0) != 0); OnShowNakedCategoryClicked(menuItemShowNakedCategory, null);
            menuItemShowCategoryShoe.Checked = ((int)RegistryTools.GetSetting(BsokEditorApp.RegistryKey + @"\Options", menuItemShowCategoryShoe.Name, 0) != 0); OnShowCategoryAndShoeClicked(menuItemShowCategoryShoe, null);
            menuItemShowGenderAge.Checked = ((int)RegistryTools.GetSetting(BsokEditorApp.RegistryKey + @"\Options", menuItemShowGenderAge.Name, 0) != 0); OnShowGenderAndAgeClicked(menuItemShowGenderAge, null);

            menuItemAdvanced.Checked = ((int)RegistryTools.GetSetting(BsokEditorApp.RegistryKey + @"\Mode", menuItemAdvanced.Name, 0) != 0); OnAdvancedModeChanged(menuItemAdvanced, null);
            menuItemAutoBackup.Checked = ((int)RegistryTools.GetSetting(BsokEditorApp.RegistryKey + @"\Mode", menuItemAutoBackup.Name, 1) != 0);

            UpdateFormState();

            MyUpdater = new Updater(BsokEditorApp.RegistryKey, menuHelp);
            MyUpdater.CheckForUpdates();
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsAnyDirty())
            {
                string qualifier = IsAnyHiddenDirty() ? " HIDDEN" : "";

                if (MsgBox.Show($"There are{qualifier} unsaved changes, do you really want to exit?", "Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }

            RegistryTools.SaveAppSettings(BsokEditorApp.RegistryKey, BsokEditorApp.AppVersionMajor, BsokEditorApp.AppVersionMinor);
            RegistryTools.SaveFormSettings(BsokEditorApp.RegistryKey, this);

            RegistryTools.SaveSetting(BsokEditorApp.RegistryKey + @"\Options", menuItemExcludeUnknown.Name, menuItemExcludeUnknown.Checked ? 1 : 0);
            RegistryTools.SaveSetting(BsokEditorApp.RegistryKey + @"\Options", menuItemShowNakedCategory.Name, menuItemShowNakedCategory.Checked ? 1 : 0);
            RegistryTools.SaveSetting(BsokEditorApp.RegistryKey + @"\Options", menuItemShowCategoryShoe.Name, menuItemShowCategoryShoe.Checked ? 1 : 0);
            RegistryTools.SaveSetting(BsokEditorApp.RegistryKey + @"\Options", menuItemShowGenderAge.Name, menuItemShowGenderAge.Checked ? 1 : 0);

            RegistryTools.SaveSetting(BsokEditorApp.RegistryKey + @"\Mode", menuItemAdvanced.Name, menuItemAdvanced.Checked ? 1 : 0);
            RegistryTools.SaveSetting(BsokEditorApp.RegistryKey + @"\Mode", menuItemAutoBackup.Name, menuItemAutoBackup.Checked ? 1 : 0);
        }

        private void OnExitClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnHelpClicked(object sender, EventArgs e)
        {
            new AboutDialog(BsokEditorApp.AppProduct).ShowDialog();
        }
        #endregion

        #region Worker
        private void DoWork_FillGrid(string folder)
        {
            if (folder == null) return;

            if (IsAnyDirty())
            {
                string qualifier = IsAnyHiddenDirty() ? " HIDDEN" : "";

                if (MsgBox.Show($"There are{qualifier} unsaved changes, do you really want to exit?", "Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }
            }

            this.folder = folder;

            this.Text = $"{BsokEditorApp.AppTitle} - {(new DirectoryInfo(folder)).FullName}";
            menuItemSelectFolder.Enabled = false;
            menuItemRecentFolders.Enabled = false;

            dataLoading = true;
            dataTableResources.BeginLoadData();

            dataTableResources.Clear();
            panelEditor.Enabled = false;

            ProgressDialog progressDialog = new ProgressDialog();
            progressDialog.DoWork += new ProgressDialog.DoWorkEventHandler(DoAsyncWork_FillGrid);
            progressDialog.DoData += new ProgressDialog.DoWorkEventHandler(DoAsyncWork_FillGrid_Data);

            DialogResult result = progressDialog.ShowDialog();

            dataTableResources.EndLoadData();
            dataLoading = false;

            menuItemRecentFolders.Enabled = true;
            menuItemSelectFolder.Enabled = true;

            if (result == DialogResult.Abort)
            {
                MyMruList.RemoveFile(folder);

                logger.Error(progressDialog.Result.Error.Message);
                logger.Info(progressDialog.Result.Error.StackTrace);

                MsgBox.Show("An error occured while processing", "Error!", MessageBoxButtons.OK);
            }
            else
            {
                MyMruList.AddFile(folder);

                if (result == DialogResult.Cancel)
                {
                }
                else
                {
                    panelEditor.Enabled = true;
                    UpdateFormState();
                }
            }
        }

        private void DoAsyncWork_FillGrid(ProgressDialog sender, DoWorkEventArgs args)
        {
            // object myArgument = args.Argument; // As passed to the Sims2ToolsProgressDialog constructor

            sender.VisualMode = ProgressBarDisplayMode.CustomText;
            sender.SetProgress(0, "Loading Objects");

            string[] packages = Directory.GetFiles(folder, "*.package", SearchOption.AllDirectories);

            uint total = (uint)packages.Length;
            uint done = 0;
            uint found = 0;

            foreach (string packageFile in packages)
            {
#if !DEBUG
                try
#endif
                {
                    using (DBPFFile package = new DBPFFile(packageFile))
                    {
                        sender.VisualMode = ProgressBarDisplayMode.Percentage;

                        List<DBPFEntry> resources = package.GetEntriesByType(Binx.TYPE);

                        foreach (DBPFEntry entry in resources)
                        {
                            if (sender.CancellationPending)
                            {
                                args.Cancel = true;
                                return;
                            }

                            Binx binx = (Binx)package.GetResourceByEntry(entry);
                            Idr idr = (Idr)package.GetResourceByTGIR(Hash.TGIRHash(binx.InstanceID, DBPFData.RESOURCE_NULL, Idr.TYPE, binx.GroupID));

                            if (IsBsokPackage(package, binx, idr, out Cpf cpf))
                            {
                                DataRow row = dataTableResources.NewRow();
                                row["Visible"] = "Yes";

                                row["PackageName"] = new FileInfo(packageFile).Name;
                                row["PackagePath"] = packageFile;

                                row["ResRef"] = cpf;

                                if (cpf == null)
                                {
                                    row["Type"] = "Unknown";
                                }
                                else
                                {
                                    Str str = (Str)package.GetResourceByKey(idr.GetItem(binx.StringSetIdx));

                                    row["Name"] = str?.LanguageItems(MetaData.Languages.Default)?[0]?.Title;

                                    row["Bsok"] = BuildBsokString(cpf);

                                    row["Gender"] = BuildGenderString(cpf);
                                    row["Age"] = BuildAgeString(cpf);

                                    if (cpf is Gzps)
                                    {
                                        row["Type"] = "Clothing";

                                        row["Category"] = BuildCategoryString(cpf);
                                        row["Shoe"] = BuildShoeString(cpf);
                                    }
                                    else if (cpf is Xmol)
                                    {
                                        row["Type"] = "Accessory";

                                        row["Category"] = "";
                                        row["Shoe"] = "";
                                    }
                                    else if (cpf is Xtol)
                                    {
                                        row["Type"] = "Eyebrow";

                                        row["Category"] = "";
                                        row["Shoe"] = "";
                                    }
                                    else
                                    {
                                        row["Type"] = "Unknown";

                                        row["Category"] = "";
                                        row["Shoe"] = "";
                                    }
                                }

                                sender.SetData(row);

                                ++found;
                            }
                        }

                        sender.SetProgress((int)((++done / (float)total) * 100.0));
                        package.Close();

                        args.Result = found;
                    }
                }
#if !DEBUG
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                    logger.Info(ex.StackTrace);

                    if (MsgBox.Show($"An error occured while processing\n{packageFile}\n\nReason: {ex.Message}", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                    {
                        throw ex;
                    }
                }
#endif
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
            DataRow row = e.Argument as DataRow;
            dataTableResources.Append(row);
        }
        #endregion

        #region Worker Helpers
        private bool IsBsokPackage(DBPFFile package, Binx binx, Idr idr, out Cpf cpf)
        {
            cpf = null;

            if (idr == null) return false;

            var res = package.GetResourceByKey(idr.GetItem(binx.ObjectIdx));

            if (res is Gzps || res is Xmol || res is Xtol)
            {
                cpf = (Cpf)res;
                return true;
            }

            return false;
        }
        private void LoadBsokProductComboBoxes()
        {
            bool oldDataLoading = dataLoading;
            dataLoading = true;

            comboBsokGenre.Items.Clear();

            if (bsokXml != null)
            {
                foreach (XmlNode node in bsokXml.ChildNodes)
                {
                    if (node is XmlElement element) comboBsokGenre.Items.Add(new XmlValue(element.GetAttribute("name"), element));
                }

                comboBsokGenre.SelectedIndex = 0;
            }

            cachedBsokValue = "";

            dataLoading = oldDataLoading;
        }

        #endregion

        #region Form State
        private bool IsAnyDirty()
        {
            foreach (DataRow row in dataTableResources.Rows)
            {
                if ((row["ResRef"] as Cpf).IsDirty)
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsAnyHiddenDirty()
        {
            foreach (DataRow row in dataTableResources.Rows)
            {
                if (!row["Visible"].Equals("Yes") && (row["ResRef"] as Cpf).IsDirty)
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsVisibleObject(Cpf cpf)
        {
            // Exclude hidden objects?
            if (menuItemExcludeUnknown.Checked && cpf == null) return false;

            return true;
        }

        private bool updatingFormState = false;

        private void UpdateFormState()
        {
            if (updatingFormState) return;

            updatingFormState = true;

            btnSave.Enabled = false;

            // Update the visibility in the underlying DataTable, do NOT use the Visible property of the DataGridView rows!!!
            foreach (DataRow row in dataTableResources.Rows)
            {
                Cpf cpf = row["ResRef"] as Cpf;

                row["Visible"] = IsVisibleObject(cpf) ? "Yes" : "No";
            }

            // Update the highlight state of the rows in the DataGridView
            foreach (DataGridViewRow row in gridViewResources.Rows)
            {
                Cpf cpf = row.Cells["colResRef"].Value as Cpf;

                if (cpf.IsDirty)
                {
                    btnSave.Enabled = true;
                    row.DefaultCellStyle.BackColor = Color.FromName(Properties.Settings.Default.DirtyHighlight);
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.Empty;
                }
            }

            updatingFormState = false;
        }

        private void ReselectRows(List<Cpf> selectedData)
        {
            if (ignoreEdits) return;

            UpdateFormState();

            foreach (DataGridViewRow row in gridViewResources.Rows)
            {
                row.Selected = selectedData.Contains(row.Cells["colResRef"].Value as Cpf);
            }
        }
        #endregion

        #region File Menu Actions
        private void OnSelectFolderClicked(object sender, EventArgs e)
        {
            if (selectPathDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                DoWork_FillGrid(selectPathDialog.FileName);
            }
        }

        private void MyMruList_FolderSelected(string folder)
        {
            DoWork_FillGrid(folder);
        }

        private void OnConfigurationClicked(object sender, EventArgs e)
        {
            Form config = new ConfigDialog();

            if (config.ShowDialog() == DialogResult.OK)
            {
                // Perform any reload necessary after changing the objects.package location
            }
        }
        #endregion

        #region Options Menu Actions
        private void OnExcludeUnknown(object sender, EventArgs e)
        {
            UpdateFormState();
        }

        private void OnShowGenderAndAgeClicked(object sender, EventArgs e)
        {
            gridViewResources.Columns["colGender"].Visible = menuItemShowGenderAge.Checked;
            gridViewResources.Columns["colAge"].Visible = menuItemShowGenderAge.Checked;

            grpGender.Visible = menuItemShowGenderAge.Checked;
            grpAge.Visible = menuItemShowGenderAge.Checked;
        }

        private void OnShowCategoryAndShoeClicked(object sender, EventArgs e)
        {
            gridViewResources.Columns["colCategory"].Visible = menuItemShowCategoryShoe.Checked;
            gridViewResources.Columns["colShoe"].Visible = menuItemShowCategoryShoe.Checked;

            grpCategory.Visible = menuItemShowCategoryShoe.Checked;
            grpShoe.Visible = menuItemShowCategoryShoe.Checked;
        }

        private void OnShowNakedCategoryClicked(object sender, EventArgs e)
        {
            ckbCatNaked.Visible = menuItemShowNakedCategory.Checked;
        }
        #endregion

        #region Mode Menu Actions
        private void OnAdvancedModeChanged(object sender, EventArgs e)
        {
        }
        #endregion

        #region Tooltips and Thumbnails
        private void OnToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int index = e.RowIndex;

                if (index < dataTableResources.Rows.Count)
                {
                    DataGridViewRow row = gridViewResources.Rows[index];

                    if (row.Cells[e.ColumnIndex].OwningColumn.Name.Equals("colPackageName"))
                    {
                        e.ToolTipText = row.Cells["colPackagePath"].Value as string;
                    }
                    else if (row.Cells[e.ColumnIndex].OwningColumn.Name.Equals("colBsok"))
                    {
                        e.ToolTipText = (row.Cells["colResRef"].Value as Cpf).GetItem("product")?.StringValue;
                    }
                }
            }
        }

        private Image GetThumbnail(DataGridViewRow row)
        {
            return cigenCache?.GetThumbnail(row.Cells["colResRef"].Value as Cpf);
        }
        #endregion

        #region Grid Management
        private void OnGridSelectionChanged(object sender, EventArgs e)
        {
            if (dataLoading) return;

            ClearEditor();

            if (gridViewResources.SelectedRows.Count >= 1)
            {
                bool append = false;
                foreach (DataGridViewRow row in gridViewResources.SelectedRows)
                {
                    UpdateEditor(row.Cells["colResRef"].Value as Cpf, append);
                    append = true;
                }
            }
        }

        private void OnResourceBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (gridViewResources.SortedColumn != null)
            {
                UpdateFormState();
            }
        }
        #endregion

        #region Grid Row Fill
        private string BuildBsokString(Cpf cpf)
        {
            string bsok = "";

            CpfItem cpfItem = cpf.GetItem("product");

            if (cpfItem == null) return bsok;

            // XmlNamespaceManager nsmgr = new XmlNamespaceManager(bsokXml.OwnerDocument.NameTable);
            // nsmgr.AddNamespace("bsok", "urn:bsok-schema");

            XmlNode node = bsokXml?.SelectSingleNode($"//*[@code='{cpfItem.StringValue}']");

            if (node != null)
            {
                bsok = $"{node.ParentNode.Attributes["name"].Value} : {node.Attributes["name"].Value}";
            }

            return bsok;
        }

        private string BuildGenderString(Cpf cpf)
        {
            string gender = "";

            CpfItem cpfItem = cpf.GetItem("gender");

            if (cpfItem == null) return gender;

            switch (cpfItem.IntegerValue)
            {
                case 1:
                    gender = "Female";
                    break;
                case 2:
                    gender = "Male";
                    break;
                case 3:
                    gender = "Unisex";
                    break;
            }

            return gender;
        }

        private string BuildAgeString(Cpf cpf)
        {
            string age = "";

            CpfItem cpfItem = cpf.GetItem("age");

            if (cpfItem == null) return age;

            uint ageFlags = cpfItem.UIntegerValue;
            if ((ageFlags & 0x0020) == 0x0020) age += " ,Babies";
            if ((ageFlags & 0x0001) == 0x0001) age += " ,Toddlers";
            if ((ageFlags & 0x0002) == 0x0002) age += " ,Children";
            if ((ageFlags & 0x0004) == 0x0004) age += " ,Teens";
            if ((ageFlags & 0x0040) == 0x0040) age += " ,Young Adults";
            if ((ageFlags & 0x0008) == 0x0008) age += " ,Adults";
            if ((ageFlags & 0x0010) == 0x0010) age += " ,Elders";

            return age.Length > 0 ? age.Substring(2) : "";
        }

        private string BuildCategoryString(Cpf cpf)
        {
            string category = "";

            CpfItem cpfItem = cpf.GetItem("category");

            if (cpfItem == null) return category;

            uint categoryFlags = cpfItem.UIntegerValue;
            if (categoryFlags == 0xFF7F)
            {
                category += " ,All";
            }
            else
            {
                if ((categoryFlags & 0x0007) == 0x0007)
                {
                    category += " ,Everyday";
                }
                else
                {
                    if ((categoryFlags & 0x0001) == 0x0001) category += " ,Casual1";
                    if ((categoryFlags & 0x0002) == 0x0002) category += " ,Casual2";
                    if ((categoryFlags & 0x0004) == 0x0004) category += " ,Casual3";
                }

                if ((categoryFlags & 0x0020) == 0x0020) category += " ,Formal";
                if ((categoryFlags & 0x0200) == 0x0200) category += " ,Gym";
                if ((categoryFlags & 0x0100) == 0x0100) category += " ,Maternity";
                if ((categoryFlags & 0x1000) == 0x1000) category += " ,Outerwear";
                if ((categoryFlags & 0x0010) == 0x0010) category += " ,PJs";
                if ((categoryFlags & 0x0008) == 0x0008) category += " ,Swimwear";
                if ((categoryFlags & 0x0040) == 0x0040) category += " ,Underwear";

                if ((categoryFlags & 0x0080) == 0x0080) category += " ,Naked";
                if ((categoryFlags & 0x0400) == 0x0400) category += " ,Try On";
                if ((categoryFlags & 0x0800) == 0x0800) category += " ,NakedOverlay";
            }

            return category.Length > 0 ? category.Substring(2) : "";
        }

        private string BuildShoeString(Cpf cpf)
        {
            string shoe = "None";

            CpfItem cpfItem = cpf.GetItem("shoe");

            if (cpfItem == null) return shoe;

            switch (cpfItem.IntegerValue)
            {
                case 1:
                    shoe = "Barefoot";
                    break;
                case 2:
                    shoe = "Heavy Boot";
                    break;
                case 3:
                    shoe = "Heels";
                    break;
                case 4:
                    shoe = "Normal Shoe";
                    break;
                case 5:
                    shoe = "Sandal";
                    break;
                case 7:
                    shoe = "Armour";
                    break;
            }

            return shoe;
        }
        #endregion

        #region Grid Row Update
        private void UpdateGridRow(Cpf cpf)
        {
            foreach (DataGridViewRow row in gridViewResources.Rows)
            {
                if ((row.Cells["colResRef"].Value as Cpf).Equals(cpf))
                {
                    row.Cells["colBsok"].Value = BuildBsokString(cpf);

                    row.Cells["colGender"].Value = BuildGenderString(cpf);
                    row.Cells["colAge"].Value = BuildAgeString(cpf);

                    row.Cells["colCategory"].Value = BuildCategoryString(cpf);
                    row.Cells["colShoe"].Value = BuildShoeString(cpf);

                    UpdateFormState();

                    return;
                }
            }
        }
        #endregion

        #region Selected Row Update
        private void UpdateSelectedRows(uint data, string name)
        {
            if (ignoreEdits) return;

            List<Cpf> selectedData = new List<Cpf>();

            foreach (DataGridViewRow row in gridViewResources.SelectedRows)
            {
                selectedData.Add(row.Cells["colResRef"].Value as Cpf);
            }

            foreach (Cpf cpf in selectedData)
            {
                UpdateCpfData(cpf, name, data);
            }

            ReselectRows(selectedData);
        }

        private void UpdateSelectedRowsForcingUInt32(string data, string name)
        {
            if (ignoreEdits) return;

            List<Cpf> selectedData = new List<Cpf>();

            foreach (DataGridViewRow row in gridViewResources.SelectedRows)
            {
                selectedData.Add(row.Cells["colResRef"].Value as Cpf);
            }

            foreach (Cpf cpf in selectedData)
            {
                uint dataAsUint32;

                if (data.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                {
                    dataAsUint32 = Convert.ToUInt32(data.Substring(2), 16);
                }
                else
                {
                    dataAsUint32 = Convert.ToUInt32(data);
                }

                UpdateCpfData(cpf, name, dataAsUint32);
            }

            ReselectRows(selectedData);
        }

        private void UpdateSelectedRows(bool state, string name, ushort flag)
        {
            if (ignoreEdits) return;

            List<Cpf> selectedData = new List<Cpf>();

            foreach (DataGridViewRow row in gridViewResources.SelectedRows)
            {
                selectedData.Add(row.Cells["colResRef"].Value as Cpf);
            }

            foreach (Cpf cpf in selectedData)
            {
                uint data = cpf.GetItem(name).UIntegerValue;

                if (state)
                {
                    data |= flag;
                }
                else
                {
                    data &= (uint)(~flag & 0xffff);
                }

                UpdateCpfData(cpf, name, data);
            }

            ReselectRows(selectedData);
        }
        #endregion

        #region Resource Update
        private void UpdateCpfData(Cpf cpf, string name, uint data)
        {
            if (ignoreEdits) return;

            cpf.GetItem(name).UIntegerValue = data;

            UpdateGridRow(cpf);
        }
        #endregion

        #region Editor
        string cachedBsokValue;
        uint cachedGenderValue, cachedAgeFlags, cachedCategoryFlags, cachedShoeValue;

        private void ClearEditor()
        {
            ignoreEdits = true;

            if (!bsokGenreLocked) comboBsokGenre.SelectedIndex = -1;
            if (!bsokStyleLocked) comboBsokStyle.SelectedIndex = -1;
            comboBsokGroup.SelectedIndex = -1;
            comboBsokShape.SelectedIndex = -1;
            if (!bsokRolesLocked) comboBsokRoles.SelectedIndex = -1;

            comboGender.SelectedIndex = -1;

            ckbAgeBabies.Checked = false;
            ckbAgeToddlers.Checked = false;
            ckbAgeChildren.Checked = false;
            ckbAgeTeens.Checked = false;
            ckbAgeYoungAdults.Checked = false;
            ckbAgeAdults.Checked = false;
            ckbAgeElders.Checked = false;

            ckbCatEveryday.Checked = false;
            ckbCatFormal.Checked = false;
            ckbCatGym.Checked = false;
            ckbCatMaternity.Checked = false;
            ckbCatOuterwear.Checked = false;
            ckbCatPJs.Checked = false;
            ckbCatSwimwear.Checked = false;
            ckbCatUnderwear.Checked = false;

            comboShoe.SelectedIndex = -1;

            ignoreEdits = false;
        }

        private void UpdateEditor(Cpf cpf, bool append)
        {
            CpfItem cpfItem;
            ignoreEdits = true;

            // Need to do gender first as it affects the filter on the bsok product drop-down(s)
            cpfItem = cpf.GetItem("gender");
            uint newGenderValue = (cpfItem == null) ? 0 : cpfItem.UIntegerValue;
            if (append)
            {
                if (cachedGenderValue != newGenderValue)
                {
                    if (cachedGenderValue != 0x00)
                    {
                        comboGender.SelectedIndex = 0;
                        cachedGenderValue = 0x00;

                        LoadBsokProductComboBoxes();
                    }
                }
            }
            else
            {
                cachedGenderValue = newGenderValue;

                LoadBsokProductComboBoxes();

                foreach (object o in comboGender.Items)
                {
                    if ((o as UintNamedValue).Value == cachedGenderValue)
                    {
                        comboGender.SelectedItem = o;
                        break;
                    }
                }
            }

            cpfItem = cpf.GetItem("product");
            string newBsokValue = (cpfItem == null) ? "" : cpfItem.StringValue;
            if (append)
            {
                if (!cachedBsokValue.Equals(newBsokValue))
                {
                    if (!bsokGenreLocked) comboBsokGenre.SelectedIndex = -1;
                    if (!bsokStyleLocked) comboBsokStyle.SelectedIndex = -1;
                    comboBsokGroup.SelectedIndex = -1;
                    comboBsokShape.SelectedIndex = -1;
                    if (!bsokRolesLocked) comboBsokRoles.SelectedIndex = -1;
                }
            }
            else
            {
                cachedBsokValue = newBsokValue;

                XmlNode node = bsokXml?.SelectSingleNode($"//*[@code='{newBsokValue}']");

                if (node != null)
                {
                    foreach (object o in comboBsokGenre.Items)
                    {
                        if ((o as XmlValue).Equals(node))
                        {
                            comboBsokGenre.SelectedItem = o;
                            break;
                        }
                    }

                    foreach (object o in comboBsokStyle.Items)
                    {
                        if ((o as XmlValue).Equals(node))
                        {
                            comboBsokStyle.SelectedItem = o;
                            break;
                        }
                    }

                    foreach (object o in comboBsokGroup.Items)
                    {
                        if ((o as XmlValue).Equals(node))
                        {
                            comboBsokGroup.SelectedItem = o;
                            break;
                        }
                    }

                    foreach (object o in comboBsokShape.Items)
                    {
                        if ((o as XmlValue).Equals(node))
                        {
                            comboBsokShape.SelectedItem = o;
                            break;
                        }
                    }

                    foreach (object o in comboBsokRoles.Items)
                    {
                        if ((o as XmlValue).Equals(node))
                        {
                            comboBsokRoles.SelectedItem = o;
                            break;
                        }
                    }
                }
                else
                {
                    AutoSelectDropDown(comboBsokGenre, bsokGenreLocked);
                    AutoSelectDropDown(comboBsokStyle, bsokStyleLocked);
                    AutoSelectDropDown(comboBsokGroup, false);
                    AutoSelectDropDown(comboBsokShape, false);
                    AutoSelectDropDown(comboBsokRoles, bsokRolesLocked);
                }
            }

            cpfItem = cpf.GetItem("age");
            uint newAgeFlags = (cpfItem == null) ? 0 : cpfItem.UIntegerValue;
            if (append)
            {
                if (cachedAgeFlags != newAgeFlags)
                {
                    if ((cachedAgeFlags & 0x0020) != (newAgeFlags & 0x0020)) ckbAgeBabies.CheckState = CheckState.Indeterminate;
                    if ((cachedAgeFlags & 0x0001) != (newAgeFlags & 0x0001)) ckbAgeToddlers.CheckState = CheckState.Indeterminate;
                    if ((cachedAgeFlags & 0x0002) != (newAgeFlags & 0x0002)) ckbAgeChildren.CheckState = CheckState.Indeterminate;
                    if ((cachedAgeFlags & 0x0004) != (newAgeFlags & 0x0004)) ckbAgeTeens.CheckState = CheckState.Indeterminate;
                    if ((cachedAgeFlags & 0x0040) != (newAgeFlags & 0x0040)) ckbAgeYoungAdults.CheckState = CheckState.Indeterminate;
                    if ((cachedAgeFlags & 0x0008) != (newAgeFlags & 0x0008)) ckbAgeAdults.CheckState = CheckState.Indeterminate;
                    if ((cachedAgeFlags & 0x0010) != (newAgeFlags & 0x0010)) ckbAgeElders.CheckState = CheckState.Indeterminate;
                }
            }
            else
            {
                cachedAgeFlags = newAgeFlags;
                if ((cachedAgeFlags & 0x0020) == 0x0020) ckbAgeBabies.Checked = true;
                if ((cachedAgeFlags & 0x0001) == 0x0001) ckbAgeToddlers.Checked = true;
                if ((cachedAgeFlags & 0x0002) == 0x0002) ckbAgeChildren.Checked = true;
                if ((cachedAgeFlags & 0x0004) == 0x0004) ckbAgeTeens.Checked = true;
                if ((cachedAgeFlags & 0x0040) == 0x0040) ckbAgeYoungAdults.Checked = true;
                if ((cachedAgeFlags & 0x0008) == 0x0008) ckbAgeAdults.Checked = true;
                if ((cachedAgeFlags & 0x0010) == 0x0010) ckbAgeElders.Checked = true;
            }

            cpfItem = cpf.GetItem("category");
            uint newCategoryFlags = (cpfItem == null) ? 0 : cpfItem.UIntegerValue;
            if (append)
            {
                if (cachedCategoryFlags != newCategoryFlags)
                {
                    if ((cachedCategoryFlags & 0x0007) != (newCategoryFlags & 0x0007)) ckbCatEveryday.CheckState = CheckState.Indeterminate;
                    if ((cachedCategoryFlags & 0x0020) != (newCategoryFlags & 0x0020)) ckbCatFormal.CheckState = CheckState.Indeterminate;
                    if ((cachedCategoryFlags & 0x0200) != (newCategoryFlags & 0x0200)) ckbCatGym.CheckState = CheckState.Indeterminate;
                    if ((cachedCategoryFlags & 0x0100) != (newCategoryFlags & 0x0100)) ckbCatMaternity.CheckState = CheckState.Indeterminate;
                    if ((cachedCategoryFlags & 0x1000) != (newCategoryFlags & 0x1000)) ckbCatOuterwear.CheckState = CheckState.Indeterminate;
                    if ((cachedCategoryFlags & 0x0010) != (newCategoryFlags & 0x0010)) ckbCatPJs.CheckState = CheckState.Indeterminate;
                    if ((cachedCategoryFlags & 0x0008) != (newCategoryFlags & 0x0008)) ckbCatSwimwear.CheckState = CheckState.Indeterminate;
                    if ((cachedCategoryFlags & 0x0040) != (newCategoryFlags & 0x0040)) ckbCatUnderwear.CheckState = CheckState.Indeterminate;
                }
            }
            else
            {
                cachedCategoryFlags = newCategoryFlags;
                if ((cachedCategoryFlags & 0x0007) == 0x0007) ckbCatEveryday.Checked = true;
                if ((cachedCategoryFlags & 0x0020) == 0x0020) ckbCatFormal.Checked = true;
                if ((cachedCategoryFlags & 0x0200) == 0x0200) ckbCatGym.Checked = true;
                if ((cachedCategoryFlags & 0x0100) == 0x0100) ckbCatMaternity.Checked = true;
                if ((cachedCategoryFlags & 0x1000) == 0x1000) ckbCatOuterwear.Checked = true;
                if ((cachedCategoryFlags & 0x0010) == 0x0010) ckbCatPJs.Checked = true;
                if ((cachedCategoryFlags & 0x0008) == 0x0008) ckbCatSwimwear.Checked = true;
                if ((cachedCategoryFlags & 0x0040) == 0x0040) ckbCatUnderwear.Checked = true;
            }

            cpfItem = cpf.GetItem("shoe");
            uint newShoeValue = (cpfItem == null) ? 0 : cpfItem.UIntegerValue;
            if (append)
            {
                if (cachedShoeValue != newShoeValue)
                {
                    comboShoe.SelectedIndex = 0;
                }
            }
            else
            {
                cachedShoeValue = newShoeValue;

                foreach (object o in comboShoe.Items)
                {
                    if ((o as UintNamedValue).Value == cachedShoeValue)
                    {
                        comboShoe.SelectedItem = o;
                        break;
                    }
                }
            }

            ignoreEdits = false;
        }

        private void AutoSelectDropDown(ComboBox comboBsok, bool locked)
        {
            if (comboBsok.Items.Count == 1)
            {
                comboBsok.SelectedIndex = 0;
            }
            else
            {
                if (!locked && comboBsok.Items.Count > 0) comboBsok.SelectedIndex = 0;
            }
        }
        #endregion

        #region BSOK Dropdown Events
        private void OnBsokGenreChanged(object sender, EventArgs e)
        {
            if (comboBsokGenre.SelectedIndex != -1)
            {
                UpdateLockButtons();
                BsokLevelChanged(comboBsokGenre, comboBsokStyle);
            }

            if (IsAutoUpdate && comboBsokRoles.SelectedItem != null && (comboBsokRoles.SelectedItem as XmlValue).Element != null) UpdateSelectedRowsForcingUInt32((comboBsokRoles.SelectedItem as XmlValue).Element.GetAttribute("code"), "product");
        }

        private void OnBsokStyleChanged(object sender, EventArgs e)
        {
            if (comboBsokStyle.SelectedIndex != -1)
            {
                UpdateLockButtons();
                BsokLevelChanged(comboBsokStyle, comboBsokGroup);
            }

            if (IsAutoUpdate && comboBsokRoles.SelectedItem != null && (comboBsokRoles.SelectedItem as XmlValue).Element != null) UpdateSelectedRowsForcingUInt32((comboBsokRoles.SelectedItem as XmlValue).Element.GetAttribute("code"), "product");
        }

        private void OnBsokGroupChanged(object sender, EventArgs e)
        {
            if (comboBsokGroup.SelectedIndex != -1)
            {
                UpdateLockButtons();
                BsokLevelChanged(comboBsokGroup, comboBsokShape);
            }

            if (IsAutoUpdate && comboBsokRoles.SelectedItem != null && (comboBsokRoles.SelectedItem as XmlValue).Element != null) UpdateSelectedRowsForcingUInt32((comboBsokRoles.SelectedItem as XmlValue).Element.GetAttribute("code"), "product");
        }

        private void OnBsokShapeChanged(object sender, EventArgs e)
        {
            if (comboBsokShape.SelectedIndex != -1)
            {
                UpdateLockButtons();
                BsokLevelChanged(comboBsokShape, comboBsokRoles);
            }

            if (IsAutoUpdate && comboBsokRoles.SelectedItem != null && (comboBsokRoles.SelectedItem as XmlValue).Element != null) UpdateSelectedRowsForcingUInt32((comboBsokRoles.SelectedItem as XmlValue).Element.GetAttribute("code"), "product");
        }

        private void OnBsokRoleChanged(object sender, EventArgs e)
        {
            if (comboBsokRoles.SelectedIndex != -1)
            {
                UpdateLockButtons();
            }

            if (IsAutoUpdate && comboBsokRoles.SelectedItem != null && (comboBsokRoles.SelectedItem as XmlValue).Element != null) UpdateSelectedRowsForcingUInt32((comboBsokRoles.SelectedItem as XmlValue).Element.GetAttribute("code"), "product");
        }

        private void BsokLevelChanged(ComboBox comboBsokParent, ComboBox comboBsokChild)
        {
            bool oldDataLoading = dataLoading;
            dataLoading = true;

            int oldSelectedIndex = comboBsokChild.SelectedIndex;

            comboBsokChild.Items.Clear();

            bool singular = ((comboBsokParent.SelectedItem as XmlValue).Element == null || (comboBsokParent.SelectedItem as XmlValue).Element.ChildNodes.Count == 1);

            if (!singular) comboBsokChild.Items.Add(new XmlValue("", null));

            if ((comboBsokParent.SelectedItem as XmlValue).Element != null)
            {
                foreach (XmlNode node in (comboBsokParent.SelectedItem as XmlValue).Element.ChildNodes)
                {
                    if (node is XmlElement element)
                    {
                        string name = element.GetAttribute("name");
                        string gender = element.GetAttribute("gender").ToLower();

                        if (!string.IsNullOrWhiteSpace(gender))
                        {
                            if ("female".Equals(gender))
                            {
                                if (cachedGenderValue != 0x01) continue;
                            }
                            else if ("male".Equals(gender))
                            {
                                if (cachedGenderValue != 0x02) continue;
                            }
                            else if ("unisex".Equals(gender))
                            {
                                // if (cachedGenderValue == 0x00) continue;
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(gender))
                        {
                            name = $"{name} ({gender})";
                        }

                        comboBsokChild.Items.Add(new XmlValue(name, element));
                    }
                }
            }

            if (comboBsokChild == comboBsokGenre && bsokGenreLocked && comboBsokChild.Items.Count > oldSelectedIndex)
            {
                comboBsokChild.SelectedIndex = oldSelectedIndex;
            }
            else if (comboBsokChild == comboBsokStyle && bsokStyleLocked && comboBsokChild.Items.Count > oldSelectedIndex)
            {
                comboBsokChild.SelectedIndex = oldSelectedIndex;
            }
            else if (comboBsokChild == comboBsokRoles && bsokRolesLocked && comboBsokChild.Items.Count > oldSelectedIndex)
            {
                comboBsokChild.SelectedIndex = oldSelectedIndex;
            }
            else
            {
                comboBsokChild.SelectedIndex = Math.Min(comboBsokChild.Items.Count - 1, (singular ? (((comboBsokParent.SelectedItem as XmlValue).Element == null) ? -1 : 0) : 1));
            }

            dataLoading = oldDataLoading;
        }
        #endregion

        #region BSOK Lock Events
        private bool bsokGenreLocked = true;
        private bool bsokStyleLocked = true;
        private bool bsokRolesLocked = true;

        private void OnBsokGenreLockClicked(object sender, EventArgs e)
        {
            bsokGenreLocked = !bsokGenreLocked;

            UpdateLockButtons();
        }

        private void OnBsokStyleLockClicked(object sender, EventArgs e)
        {
            bsokStyleLocked = !bsokStyleLocked;

            UpdateLockButtons();
        }

        private void OnBsokRoleLockClicked(object sender, EventArgs e)
        {
            bsokRolesLocked = !bsokRolesLocked;

            UpdateLockButtons();
        }

        private void UpdateLockButtons()
        {
            Image ln = Properties.Resources.Padlock_locked_16;
            Image lg = Properties.Resources.Padlock_locked_gray_16;
            Image un = Properties.Resources.Padlock_unlocked_16;
            Image ug = Properties.Resources.Padlock_unlocked_gray_16;

            btnBsokGenre.Enabled = false; // (comboBsokGenre.Items.Count > 1);
            btnBsokStyle.Enabled = false; // (comboBsokStyle.Items.Count > 1);
            btnBsokRoles.Enabled = false; // (comboBsokRoles.Items.Count > 1);

            bsokGenreLocked = bsokGenreLocked || (comboBsokGenre.Items.Count <= 1);
            bsokStyleLocked = bsokStyleLocked || (comboBsokStyle.Items.Count <= 1);
            bsokRolesLocked = bsokRolesLocked || (comboBsokRoles.Items.Count <= 1);

            btnBsokGenre.Image = (btnBsokGenre.Enabled) ? (bsokGenreLocked ? ln : un) : (bsokGenreLocked ? lg : ug);
            btnBsokStyle.Image = (btnBsokStyle.Enabled) ? (bsokStyleLocked ? ln : un) : (bsokStyleLocked ? lg : ug);
            btnBsokRoles.Image = (btnBsokRoles.Enabled) ? (bsokRolesLocked ? ln : un) : (bsokRolesLocked ? lg : ug);
        }
        #endregion

        #region Dropdown Events
        private void OnGenderChanged(object sender, EventArgs e)
        {
            if (comboGender.SelectedIndex != -1)
            {
                if (IsAutoUpdate)
                {
                    cachedGenderValue = (comboGender.SelectedItem as UintNamedValue).Value;
                    UpdateSelectedRows(cachedGenderValue, "gender");
                }

                LoadBsokProductComboBoxes();
            }
        }

        private void OnShoeChanged(object sender, EventArgs e)
        {
            if (comboShoe.SelectedIndex != -1)
            {
                if (IsAutoUpdate) UpdateSelectedRows((comboShoe.SelectedItem as UintNamedValue).Value, "shoe");
            }
        }
        #endregion

        #region Checkbox Events
        private void OnCatEverydayClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedRows(ckbCatEveryday.Checked, "category", 0x0007);
        }

        private void OnCatFormalClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedRows(ckbCatFormal.Checked, "category", 0x0020);
        }

        private void OnCatGymClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedRows(ckbCatGym.Checked, "category", 0x0200);
        }

        private void OnCatMaternityClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedRows(ckbCatMaternity.Checked, "category", 0x0100);
        }

        private void OnCatOuterwearClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedRows(ckbCatOuterwear.Checked, "category", 0x1000);
        }

        private void OnCatPJsClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedRows(ckbCatPJs.Checked, "category", 0x0010);
        }

        private void OnCatNakedClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedRows(ckbCatNaked.Checked, "category", 0x0080);
        }

        private void OnCatSwimwearClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedRows(ckbCatSwimwear.Checked, "category", 0x0008);
        }

        private void OnCatUnderwearClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedRows(ckbCatUnderwear.Checked, "category", 0x0040);
        }

        private void OnAgeBabiesClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedRows(ckbAgeBabies.Checked, "age", 0x0020);
        }

        private void OnAgeToddlersClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedRows(ckbAgeToddlers.Checked, "age", 0x0001);
        }

        private void OnAgeChildrenClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedRows(ckbAgeChildren.Checked, "age", 0x0002);
        }

        private void OnAgeTeensClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedRows(ckbAgeTeens.Checked, "age", 0x0004);
        }

        private void OnAgeYoungAdultsClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedRows(ckbAgeYoungAdults.Checked, "age", 0x0040);
        }

        private void OnAgeAdultsClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedRows(ckbAgeAdults.Checked, "age", 0x0008);
        }

        private void OnAgeEldersClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedRows(ckbAgeElders.Checked, "age", 0x0010);
        }
        #endregion

        #region Textbox Events
        #endregion

        #region Mouse Management
        private DataGridViewCellEventArgs mouseLocation = null;
        readonly DataGridViewRow highlightRow = null;
        readonly Color highlightColor = Color.Empty;

        private void OnCellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            mouseLocation = e;
            Point MousePosition = Cursor.Position;

            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.RowIndex < gridViewResources.RowCount && e.ColumnIndex < gridViewResources.ColumnCount)
            {
                DataGridViewRow row = gridViewResources.Rows[e.RowIndex];

                if (row.Cells[e.ColumnIndex].OwningColumn.Name.Equals("colType") || row.Cells[e.ColumnIndex].OwningColumn.Name.Equals("colPackageName"))
                {
                    Image thumbnail = GetThumbnail(row);

                    if (thumbnail != null)
                    {
                        thumbBox.Image = thumbnail;
                        thumbBox.Location = new System.Drawing.Point(MousePosition.X - this.Location.X, MousePosition.Y - this.Location.Y);
                        thumbBox.Visible = true;
                    }
                }
            }
        }

        private void OnCellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            thumbBox.Visible = false;
        }
        #endregion

        #region Context Menu
        private void OnContextMenuOpening(object sender, CancelEventArgs e)
        {
            if (mouseLocation == null || mouseLocation.RowIndex == -1)
            {
                e.Cancel = true;
                return;
            }

            foreach (DataGridViewRow selectedRow in gridViewResources.SelectedRows)
            {
                if (mouseLocation.RowIndex == selectedRow.Index && (selectedRow.Cells["colResRef"].Value as Cpf).IsDirty)
                {
                    return;
                }
            }

            e.Cancel = true;
            return;
        }

        private void OnContextMenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            if (highlightRow != null)
            {
                highlightRow.DefaultCellStyle.BackColor = highlightColor;
            }
        }

        private void OnRowRevertClicked(object sender, EventArgs e)
        {
            List<Cpf> selectedData = new List<Cpf>();

            foreach (DataGridViewRow row in gridViewResources.SelectedRows)
            {
                Cpf cpf = row.Cells["colResRef"].Value as Cpf;

                if (cpf.IsDirty)
                {
                    selectedData.Add(cpf);
                }
            }

            foreach (Cpf cpf in selectedData)
            {
                foreach (DataGridViewRow row in gridViewResources.Rows)
                {
                    if ((row.Cells["colResRef"].Value as Cpf).Equals(cpf))
                    {
                        string packageFile = row.Cells["colPackagePath"].Value as string;

                        using (DBPFFile package = new DBPFFile(packageFile))
                        {
                            Cpf originalCpf = (Cpf)package.GetResourceByKey(cpf);

                            row.Cells["colResRef"].Value = originalCpf;

                            package.Close();

                            UpdateGridRow(originalCpf);
                        }
                    }
                }
            }
        }
        #endregion

        #region Save Button
        private void OnSaveClicked(object sender, EventArgs e)
        {
            Save();

            UpdateFormState();
        }

        private void Save()
        {
            Dictionary<string, List<Cpf>> dirtyCpfsByPackage = new Dictionary<string, List<Cpf>>();

            foreach (DataGridViewRow row in gridViewResources.Rows)
            {
                Cpf editedCpf = row.Cells["colResRef"].Value as Cpf;

                if (editedCpf.IsDirty)
                {
                    string packageFile = row.Cells["colPackagePath"].Value as string;

                    if (!dirtyCpfsByPackage.ContainsKey(packageFile))
                    {
                        dirtyCpfsByPackage.Add(packageFile, new List<Cpf>());
                    }

                    dirtyCpfsByPackage[packageFile].Add(editedCpf);

                    row.DefaultCellStyle.BackColor = Color.Empty;
                }
            }

            foreach (string packageFile in dirtyCpfsByPackage.Keys)
            {
                using (DBPFFile dbpfPackage = new DBPFFile(packageFile))
                {
                    List<Cpf> editedCpfs = new List<Cpf>();

                    foreach (Cpf editedCpf in dirtyCpfsByPackage[packageFile])
                    {
                        editedCpf.GetItem("creator").StringValue = "00000000-0000-0000-0000-000000000000";

                        dbpfPackage.Commit(editedCpf);

                        editedCpfs.Add(editedCpf);
                    }

                    try
                    {
                        if (dbpfPackage.IsDirty) dbpfPackage.Update(menuItemAutoBackup.Checked);
                    }
                    catch (Exception)
                    {
                        MsgBox.Show($"Error trying to update {dbpfPackage.PackageName}", "Package Update Error!");
                    }

                    foreach (Cpf editedCpf in editedCpfs)
                    {
                        editedCpf.SetClean();
                    }

                    dbpfPackage.Close();
                }
            }
        }
        #endregion
    }
}
