/*
 * BSOK Editor - a utility for adding BSOK data to clothing and accessory packages
 *
 * William Howard - 2020-2022
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Microsoft.WindowsAPICodePack.Dialogs;
using Sims2Tools;
using Sims2Tools.Controls;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.CPF;
using Sims2Tools.DBPF.Data;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph.BINX;
using Sims2Tools.DBPF.SceneGraph.GZPS;
using Sims2Tools.DBPF.SceneGraph.IDR;
using Sims2Tools.DBPF.SceneGraph.XMOL;
using Sims2Tools.DBPF.STR;
using Sims2Tools.DBPF.Utils;
using Sims2Tools.Dialogs;
using Sims2Tools.Updates;
using Sims2Tools.Utils.Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace BsokEditor
{
    public partial class BsokEditorForm : Form
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string folder = null;

        private MruList MyMruList;
        private Updater MyUpdater;

        private readonly BsokEditorData bsokData = new BsokEditorData();
        private readonly XmlElement bsokXml;

        private bool dataLoading = false;
        private bool ignoreEdits = false;

        private bool IsAutoUpdate => (!dataLoading && !ignoreEdits);

        public BsokEditorForm()
        {
            logger.Info(BsokEditorApp.AppProduct);

            InitializeComponent();
            this.Text = BsokEditorApp.AppName;

            selectPathDialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true
            };

            {
                dataLoading = true;

                XmlDocument bsokXmlDoc = new XmlDocument();
                bsokXmlDoc.Load("Resources/XML/bsok.xml");

                bsokXml = bsokXmlDoc.DocumentElement;

                LoadBsokProductComboBoxes();

                comboGender.Items.Clear();
                comboGender.Items.AddRange(new NamedValue[] {
                    new NamedValue("", 0),
                    new NamedValue("Female", 1),
                    new NamedValue("Male", 2),
                    new NamedValue("Unisex", 3),
                });

                comboShoe.Items.Clear();
                comboShoe.Items.AddRange(new NamedValue[] {
                    new NamedValue("", 0),
                    new NamedValue("Armour", 7),
                    new NamedValue("Barefoot", 1),
                    new NamedValue("Heels", 3),
                    new NamedValue("Heavy Boot", 2),
                    new NamedValue("Normal Shoe", 4),
                    new NamedValue("Sandal", 5),
                });

                dataLoading = false;
            }

            gridObjects.DataSource = bsokData;
        }

        private void LoadBsokProductComboBoxes()
        {
            bool oldDataLoading = dataLoading;
            dataLoading = true;

            comboBsokGenre.Items.Clear();

            foreach (XmlNode node in bsokXml.ChildNodes)
            {
                if (node is XmlElement element) comboBsokGenre.Items.Add(new XmlValue(element.GetAttribute("name"), element));
            }

            comboBsokGenre.SelectedIndex = 0;

            cachedBsokValue = "";

            dataLoading = oldDataLoading;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            RegistryTools.LoadAppSettings(BsokEditorApp.RegistryKey, BsokEditorApp.AppVersionMajor, BsokEditorApp.AppVersionMinor);
            RegistryTools.LoadFormSettings(BsokEditorApp.RegistryKey, this);

            MyMruList = new MruList(BsokEditorApp.RegistryKey, menuItemRecentFolders, Properties.Settings.Default.MruSize);
            MyMruList.FileSelected += MyMruList_FolderSelected;

            menuItemExcludeUnknown.Checked = ((int)RegistryTools.GetSetting(BsokEditorApp.RegistryKey + @"\Options", menuItemExcludeUnknown.Name, 0) != 0);
            menuItemShowCategoryShoe.Checked = ((int)RegistryTools.GetSetting(BsokEditorApp.RegistryKey + @"\Options", menuItemShowCategoryShoe.Name, 0) != 0); OnShowCategoryAndShoeClicked(menuItemShowCategoryShoe, null);
            menuItemShowGenderAge.Checked = ((int)RegistryTools.GetSetting(BsokEditorApp.RegistryKey + @"\Options", menuItemShowGenderAge.Name, 0) != 0); OnShowGenderAndAgeClicked(menuItemShowGenderAge, null);

            menuItemAutoBackup.Checked = ((int)RegistryTools.GetSetting(BsokEditorApp.RegistryKey + @"\Mode", menuItemAutoBackup.Name, 1) != 0);

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
            RegistryTools.SaveSetting(BsokEditorApp.RegistryKey + @"\Options", menuItemShowCategoryShoe.Name, menuItemShowCategoryShoe.Checked ? 1 : 0);
            RegistryTools.SaveSetting(BsokEditorApp.RegistryKey + @"\Options", menuItemShowGenderAge.Name, menuItemShowGenderAge.Checked ? 1 : 0);

            RegistryTools.SaveSetting(BsokEditorApp.RegistryKey + @"\Mode", menuItemAutoBackup.Name, menuItemAutoBackup.Checked ? 1 : 0);
        }

        private bool IsAnyDirty()
        {
            foreach (DataGridViewRow row in gridObjects.Rows)
            {
                if ((row.Cells["colCpf"].Value as Cpf).IsDirty)
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsAnyHiddenDirty()
        {
            foreach (DataGridViewRow row in gridObjects.Rows)
            {
                if (row.Visible == false && (row.Cells["colCpf"].Value as Cpf).IsDirty)
                {
                    return true;
                }
            }

            return false;
        }

        private void OnExitClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnHelpClicked(object sender, EventArgs e)
        {
            new Sims2ToolsAboutDialog(BsokEditorApp.AppProduct).ShowDialog();
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
            if (e.RowIndex >= 0)
            {
                int index = e.RowIndex;

                if (index < bsokData.Rows.Count)
                {
                    DataGridViewRow row = gridObjects.Rows[index];

                    if (row.Cells[e.ColumnIndex].OwningColumn.Name.Equals("colPackageName"))
                    {
                        e.ToolTipText = row.Cells["colPackagePath"].Value as string;
                    }
                    else if (row.Cells[e.ColumnIndex].OwningColumn.Name.Equals("colBsok"))
                    {
                        e.ToolTipText = (row.Cells["colCpf"].Value as Cpf).GetItem("product").StringValue;
                    }
                }
            }
        }

        private void MyMruList_FolderSelected(string folder)
        {
            DoWork_FillGrid(folder);
        }

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

            this.Text = $"{BsokEditorApp.AppName} - {(new DirectoryInfo(folder)).FullName}";
            menuItemSelectFolder.Enabled = false;
            menuItemRecentFolders.Enabled = false;

            dataLoading = true;

            bsokData.Clear();
            panelEditor.Enabled = false;

            Sims2ToolsProgressDialog progressDialog = new Sims2ToolsProgressDialog();
            progressDialog.DoWork += new Sims2ToolsProgressDialog.DoWorkEventHandler(DoAsyncWork_FillGrid);
            progressDialog.DoData += new Sims2ToolsProgressDialog.DoWorkEventHandler(DoAsyncWork_FillGrid_Data);

            DialogResult result = progressDialog.ShowDialog();

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

        private void DoAsyncWork_FillGrid(Sims2ToolsProgressDialog sender, DoWorkEventArgs args)
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
                                DataRow row = bsokData.NewRow();
                                row["PackageName"] = new FileInfo(packageFile).Name;
                                row["PackagePath"] = packageFile;

                                row["Cpf"] = cpf;

                                if (cpf == null)
                                {
                                    row["Type"] = "Unknown";
                                }
                                else
                                {
                                    Str str = (Str)package.GetResourceByKey(idr.Items[binx.StringSetIdx]);

                                    row["Name"] = str?.LanguageItems(MetaData.Languages.English)?[0]?.Title;

                                    row["Bsok"] = BuildBsokString(cpf);

                                    row["Gender"] = BuildGenderString(cpf);
                                    row["Age"] = BuildAgeString(cpf);

                                    if (cpf is Gzps)
                                    {
                                        row["Type"] = "Clothing";

                                        row["Category"] = BuildCategoryString(cpf);
                                        row["Shoe"] = BuildShoeString(cpf);
                                    }
                                    else
                                    {
                                        row["Type"] = "Accessory";

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

        private bool IsBsokPackage(DBPFFile package, Binx binx, Idr idr, out Cpf cpf)
        {
            cpf = null;

            if (idr == null) return false;

            var res = package.GetResourceByKey(idr.Items[binx.ObjectIdx]);

            if (res is Gzps || res is Xmol)
            {
                cpf = (Cpf)res;
                return true;
            }

            return false;
        }

        private void UpdateRowVisibility()
        {
            gridObjects.CurrentCell = null;
            gridObjects.ClearSelection();

            foreach (DataGridViewRow row in gridObjects.Rows)
            {
                row.Visible = IsVisibleObject(row.Cells["colCpf"].Value as Cpf);
            }
        }

        private bool IsVisibleObject(Cpf cpf)
        {
            // Exclude hidden objects?
            if (menuItemExcludeUnknown.Checked && cpf == null) return false;

            return true;
        }

        private string BuildBsokString(Cpf cpf)
        {
            string bsok = "";

            CpfItem cpfItem = cpf.GetItem("product");

            if (cpfItem == null) return bsok;

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(bsokXml.OwnerDocument.NameTable);
            nsmgr.AddNamespace("bsok", "urn:bsok-schema");

            XmlNode node = bsokXml.SelectSingleNode($"//*[@code='{cpfItem.StringValue}']");

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

                if ((categoryFlags & 0x0080) == 0x0080) category += " ,Skin";
                if ((categoryFlags & 0x0400) == 0x0400) category += " ,Try On";
                if ((categoryFlags & 0x0800) == 0x0800) category += " ,Naked";
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

        private void DoAsyncWork_FillGrid_Data(Sims2ToolsProgressDialog sender, DoWorkEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { DoAsyncWork_FillGrid_Data(sender, e); });
                return;
            }

            // This will be run on main (UI) thread 
            DataRow row = e.Argument as DataRow;
            bsokData.Append(row);
            gridObjects.CurrentCell = null;
            gridObjects.Rows[gridObjects.RowCount - 1].Visible = IsVisibleObject(row["Cpf"] as Cpf);
        }

        private DataGridViewCellEventArgs mouseLocation = null;
        readonly DataGridViewRow highlightRow = null;
        readonly Color highlightColor = Color.Empty;

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

            foreach (DataGridViewRow selectedRow in gridObjects.SelectedRows)
            {
                if (selectedRow.Visible && mouseLocation.RowIndex == selectedRow.Index && (selectedRow.Cells["colCpf"].Value as Cpf).IsDirty)
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

        private void UpdateFormState()
        {
            btnSave.Enabled = false;

            foreach (DataGridViewRow row in gridObjects.Rows)
            {
                if (row.Visible)
                {
                    if (row.Cells["colCpf"].Value is Cpf cpf && cpf.IsDirty)
                    {
                        btnSave.Enabled = true;
                        break;
                    }
                }
            }
        }

        private void OnSelectFolderClicked(object sender, EventArgs e)
        {
            if (selectPathDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                DoWork_FillGrid(selectPathDialog.FileName);
            }
        }

        private void OnExcludeHidden(object sender, EventArgs e)
        {
            UpdateRowVisibility();
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
                                if (cachedGenderValue == 0x00) continue;
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

        private void OnBsokGenreChanged(object sender, EventArgs e)
        {
            if (comboBsokGenre.SelectedIndex != -1)
            {
                UpdateLockButtons();
                BsokLevelChanged(comboBsokGenre, comboBsokStyle);
            }

            if (IsAutoUpdate && comboBsokRoles.SelectedItem != null && (comboBsokRoles.SelectedItem as XmlValue).Element != null) UpdateSelectedValueForcingUInt32((comboBsokRoles.SelectedItem as XmlValue).Element.GetAttribute("code"), "product");
        }

        private void OnBsokStyleChanged(object sender, EventArgs e)
        {
            if (comboBsokStyle.SelectedIndex != -1)
            {
                UpdateLockButtons();
                BsokLevelChanged(comboBsokStyle, comboBsokGroup);
            }

            if (IsAutoUpdate && comboBsokRoles.SelectedItem != null && (comboBsokRoles.SelectedItem as XmlValue).Element != null) UpdateSelectedValueForcingUInt32((comboBsokRoles.SelectedItem as XmlValue).Element.GetAttribute("code"), "product");
        }

        private void OnBsokGroupChanged(object sender, EventArgs e)
        {
            if (comboBsokGroup.SelectedIndex != -1)
            {
                UpdateLockButtons();
                BsokLevelChanged(comboBsokGroup, comboBsokShape);
            }

            if (IsAutoUpdate && comboBsokRoles.SelectedItem != null && (comboBsokRoles.SelectedItem as XmlValue).Element != null) UpdateSelectedValueForcingUInt32((comboBsokRoles.SelectedItem as XmlValue).Element.GetAttribute("code"), "product");
        }

        private void OnBsokShapeChanged(object sender, EventArgs e)
        {
            if (comboBsokShape.SelectedIndex != -1)
            {
                UpdateLockButtons();
                BsokLevelChanged(comboBsokShape, comboBsokRoles);
            }

            if (IsAutoUpdate && comboBsokRoles.SelectedItem != null && (comboBsokRoles.SelectedItem as XmlValue).Element != null) UpdateSelectedValueForcingUInt32((comboBsokRoles.SelectedItem as XmlValue).Element.GetAttribute("code"), "product");
        }

        private void OnBsokRoleChanged(object sender, EventArgs e)
        {
            if (comboBsokRoles.SelectedIndex != -1)
            {
                UpdateLockButtons();
            }

            if (IsAutoUpdate && comboBsokRoles.SelectedItem != null && (comboBsokRoles.SelectedItem as XmlValue).Element != null) UpdateSelectedValueForcingUInt32((comboBsokRoles.SelectedItem as XmlValue).Element.GetAttribute("code"), "product");
        }

        private void OnGenderChanged(object sender, EventArgs e)
        {
            if (comboGender.SelectedIndex != -1)
            {
                if (IsAutoUpdate)
                {
                    cachedGenderValue = (comboGender.SelectedItem as NamedValue).Value;
                    UpdateSelectedValue(cachedGenderValue, "gender");
                }

                LoadBsokProductComboBoxes();
            }
        }

        private void OnShoeChanged(object sender, EventArgs e)
        {
            if (comboShoe.SelectedIndex != -1)
            {
                if (IsAutoUpdate) UpdateSelectedValue((comboShoe.SelectedItem as NamedValue).Value, "shoe");
            }
        }

        private void OnGridSelectionChanged(object sender, EventArgs e)
        {
            if (dataLoading) return;

            ClearEditor();

            if (gridObjects.SelectedRows.Count >= 1)
            {
                bool append = false;
                foreach (DataGridViewRow row in gridObjects.SelectedRows)
                {
                    if (row.Visible)
                    {
                        UpdateEditor(row.Cells["colCpf"].Value as Cpf, append);
                        append = true;
                    }
                }
            }
        }


        string cachedBsokValue;
        uint cachedGenderValue, cachedAgeFlags, cachedCategoryFlags, cachedShoeValue;

        private void UpdateGridRow(DataGridViewRow row, Cpf cpf)
        {
            if (cpf == null)
            {
                cpf = row.Cells["colCpf"].Value as Cpf;
            }

            row.Cells["colBsok"].Value = BuildBsokString(cpf);

            row.Cells["colGender"].Value = BuildGenderString(cpf);
            row.Cells["colAge"].Value = BuildAgeString(cpf);

            row.Cells["colCategory"].Value = BuildCategoryString(cpf);
            row.Cells["colShoe"].Value = BuildShoeString(cpf);

            UpdateFormState();
        }

        private void UpdateCpfData(Cpf cpf, string name, uint data, DataGridViewRow row)
        {
            if (ignoreEdits) return;

            cpf.GetItem(name).UIntegerValue = data;

            if (cpf.IsDirty)
            {
                row.DefaultCellStyle.BackColor = Color.FromName(Properties.Settings.Default.DirtyHighlight);
            }

            UpdateGridRow(row, cpf);
        }

        private void UpdateSelectedValue(uint data, string name)
        {
            foreach (DataGridViewRow row in gridObjects.SelectedRows)
            {
                if (row.Visible)
                {
                    Cpf cpf = row.Cells["colCpf"].Value as Cpf;

                    UpdateCpfData(cpf, name, data, row);
                }
            }
        }

        private void UpdateSelectedValueForcingUInt32(string data, string name)
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

            foreach (DataGridViewRow row in gridObjects.SelectedRows)
            {
                if (row.Visible)
                {
                    Cpf cpf = row.Cells["colCpf"].Value as Cpf;

                    UpdateCpfData(cpf, name, dataAsUint32, row);
                }
            }
        }

        private void UpdateSelectedFlag(bool state, string name, ushort flag)
        {
            foreach (DataGridViewRow row in gridObjects.SelectedRows)
            {
                if (row.Visible)
                {
                    Cpf cpf = row.Cells["colCpf"].Value as Cpf;

                    uint data = cpf.GetItem(name).UIntegerValue;

                    if (state)
                    {
                        data |= flag;
                    }
                    else
                    {
                        data &= (uint)(~flag & 0xffff);
                    }

                    UpdateCpfData(cpf, name, data, row);
                }
            }
        }

        private void OnCatEverydayClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedFlag(ckbCatEveryday.Checked, "category", 0x0007);
        }

        private void OnCatFormalClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedFlag(ckbCatFormal.Checked, "category", 0x0020);
        }

        private void OnCatGymClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedFlag(ckbCatGym.Checked, "category", 0x0200);
        }

        private void OnCatMaternityClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedFlag(ckbCatMaternity.Checked, "category", 0x0100);
        }

        private void OnCatOuterwearClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedFlag(ckbCatOuterwear.Checked, "category", 0x1000);
        }

        private void OnCatPJsClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedFlag(ckbCatPJs.Checked, "category", 0x0010);
        }

        private void OnCatSwimwearClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedFlag(ckbCatSwimwear.Checked, "category", 0x0008);
        }

        private void OnCatUnderwearClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedFlag(ckbCatUnderwear.Checked, "category", 0x0040);
        }

        private void OnAgeBabiesClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedFlag(ckbAgeBabies.Checked, "age", 0x0020);
        }

        private void OnAgeToddlersClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedFlag(ckbAgeToddlers.Checked, "age", 0x0001);
        }

        private void OnAgeChildrenClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedFlag(ckbAgeChildren.Checked, "age", 0x0002);
        }

        private void OnAgeTeensClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedFlag(ckbAgeTeens.Checked, "age", 0x0004);
        }

        private void OnAgeYoungAdultsClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedFlag(ckbAgeAdults.Checked, "age", 0x0008);
        }

        private void OnAgeAdultsClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedFlag(ckbAgeAdults.Checked, "age", 0x0040);
        }

        private void OnAgeEldersClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedFlag(ckbAgeElders.Checked, "age", 0x0010);
        }

        private void OnRowRevertClicked(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in gridObjects.SelectedRows)
            {
                if (row.Visible)
                {
                    Cpf cpf = row.Cells["colCpf"].Value as Cpf;

                    if (cpf.IsDirty)
                    {
                        string packageFile = row.Cells["colPackagePath"].Value as string;

                        using (DBPFFile package = new DBPFFile(packageFile))
                        {
                            Cpf originalCpf = (Cpf)package.GetResourceByKey(cpf);

                            row.Cells["colCpf"].Value = originalCpf;

                            package.Close();

                            UpdateGridRow(row, originalCpf);
                            row.DefaultCellStyle.BackColor = Color.Empty;
                        }
                    }
                }
            }
        }

        private void OnShowGenderAndAgeClicked(object sender, EventArgs e)
        {
            gridObjects.Columns["colGender"].Visible = menuItemShowGenderAge.Checked;
            gridObjects.Columns["colAge"].Visible = menuItemShowGenderAge.Checked;

            grpGender.Visible = menuItemShowGenderAge.Checked;
            grpAge.Visible = menuItemShowGenderAge.Checked;
        }

        private void OnShowCategoryAndShoeClicked(object sender, EventArgs e)
        {
            gridObjects.Columns["colCategory"].Visible = menuItemShowCategoryShoe.Checked;
            gridObjects.Columns["colShoe"].Visible = menuItemShowCategoryShoe.Checked;

            grpCategory.Visible = menuItemShowCategoryShoe.Checked;
            grpShoe.Visible = menuItemShowCategoryShoe.Checked;
        }

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

                foreach (Object o in comboGender.Items)
                {
                    if ((o as NamedValue).Value == cachedGenderValue)
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

                XmlNode node = bsokXml.SelectSingleNode($"//*[@code='{newBsokValue}']");

                if (node != null)
                {
                    foreach (Object o in comboBsokGenre.Items)
                    {
                        if ((o as XmlValue).Equals(node))
                        {
                            comboBsokGenre.SelectedItem = o;
                            break;
                        }
                    }

                    foreach (Object o in comboBsokStyle.Items)
                    {
                        if ((o as XmlValue).Equals(node))
                        {
                            comboBsokStyle.SelectedItem = o;
                            break;
                        }
                    }

                    foreach (Object o in comboBsokGroup.Items)
                    {
                        if ((o as XmlValue).Equals(node))
                        {
                            comboBsokGroup.SelectedItem = o;
                            break;
                        }
                    }

                    foreach (Object o in comboBsokShape.Items)
                    {
                        if ((o as XmlValue).Equals(node))
                        {
                            comboBsokShape.SelectedItem = o;
                            break;
                        }
                    }

                    foreach (Object o in comboBsokRoles.Items)
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

                foreach (Object o in comboShoe.Items)
                {
                    if ((o as NamedValue).Value == cachedShoeValue)
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

        private void OnSaveClicked(object sender, EventArgs e)
        {
            Save();

            UpdateFormState();
        }

        private void Save()
        {
            Dictionary<string, List<Cpf>> dirtyCpfsByPackage = new Dictionary<string, List<Cpf>>();

            foreach (DataGridViewRow row in gridObjects.Rows)
            {
                if (row.Visible)
                {
                    Cpf editedCpf = row.Cells["colCpf"].Value as Cpf;

                    if (editedCpf.IsDirty)
                    {
                        String packageFile = row.Cells["colPackagePath"].Value as string;

                        if (!dirtyCpfsByPackage.ContainsKey(packageFile))
                        {
                            dirtyCpfsByPackage.Add(packageFile, new List<Cpf>());
                        }

                        dirtyCpfsByPackage[packageFile].Add(editedCpf);

                        row.DefaultCellStyle.BackColor = Color.Empty;
                    }
                }
            }

            foreach (string packageFile in dirtyCpfsByPackage.Keys)
            {
                using (DBPFFile dbpfPackage = new DBPFFile(packageFile))
                {
                    foreach (Cpf editedCpf in dirtyCpfsByPackage[packageFile])
                    {
                        editedCpf.GetItem("creator").StringValue = "00000000-0000-0000-0000-000000000000";
                        dbpfPackage.Commit(editedCpf);
                        editedCpf.SetClean();
                    }

                    if (dbpfPackage.IsDirty) dbpfPackage.Update(menuItemAutoBackup.Checked);

                    dbpfPackage.Close();
                }
            }
        }

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
    }
}
