/*
 * BHAV Finder - a utility for searching The Sims 2 package files for BHAV that match specified criteria
 *             - see http://www.picknmixmods.com/Sims2/Notes/BhavFinder/BhavFinder.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2024
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using CsvHelper;
using Microsoft.WindowsAPICodePack.Dialogs;
using Sims2Tools;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.BHAV;
using Sims2Tools.DBPF.Data;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.STR;
using Sims2Tools.DBPF.Utils;
using Sims2Tools.Dialogs;
using Sims2Tools.Updates;
using Sims2Tools.Utils.Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BhavFinder
{
    public partial class BhavFinderForm : Form
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly SortedDictionary<string, string> localObjectsByGroupID = new SortedDictionary<string, string>();

        private MruList MyMruList;
        private Updater MyUpdater;

        private readonly TextBox[] operands = new TextBox[16];
        private readonly TextBox[] masks = new TextBox[16];

        private readonly Regex Hex2Regex = new Regex(@"^([0-9A-F][0-9A-F]?)$");
        private readonly Regex HexGroupRegex = new Regex(@"^(0[xX])?([0-9A-Fa-f]+)");
        private readonly Regex HexGUIDRegex = new Regex(@"^(0[xX])?([0-9A-Fa-f]+)");
        private readonly Regex HexOpCodeRegex = new Regex(@"^(0[xX])?([0-9A-Fa-f]+)");
        private readonly Regex HexInstanceRegex = new Regex(@"^(0[xX])?([0-9A-Fa-f]+)");

        private readonly BhavFinderData bhavFoundData = new BhavFinderData();

        private readonly CommonOpenFileDialog selectPathDialog;

        public BhavFinderForm()
        {
            logger.Info(BhavFinderApp.AppProduct);

            InitializeComponent();
            this.Text = BhavFinderApp.AppTitle;

            operands[0] = textOperand0;
            operands[1] = textOperand1;
            operands[2] = textOperand2;
            operands[3] = textOperand3;
            operands[4] = textOperand4;
            operands[5] = textOperand5;
            operands[6] = textOperand6;
            operands[7] = textOperand7;
            operands[8] = textOperand8;
            operands[9] = textOperand9;
            operands[10] = textOperand10;
            operands[11] = textOperand11;
            operands[12] = textOperand12;
            operands[13] = textOperand13;
            operands[14] = textOperand14;
            operands[15] = textOperand15;

            ClearOperands();

            masks[0] = textMask0;
            masks[1] = textMask1;
            masks[2] = textMask2;
            masks[3] = textMask3;
            masks[4] = textMask4;
            masks[5] = textMask5;
            masks[6] = textMask6;
            masks[7] = textMask7;
            masks[8] = textMask8;
            masks[9] = textMask9;
            masks[10] = textMask10;
            masks[11] = textMask11;
            masks[12] = textMask12;
            masks[13] = textMask13;
            masks[14] = textMask14;
            masks[15] = textMask15;

            ResetMasks();

            gridFoundBhavs.DataSource = bhavFoundData;
            this.gridFoundBhavs.Columns["colBhavPackage"].Visible = false;
            this.gridFoundBhavs.Columns["colBhavDbpfEntry"].Visible = false;

            this.comboBhavInGroup.Items.Add("");
            this.comboBhavInGroup.Items.Add($"{DBPFData.GROUP_LOCAL} {DBPFData.NAME_LOCAL}");
            this.comboBhavInGroup.Items.Add($"{DBPFData.GROUP_GLOBALS} {DBPFData.NAME_GLOBALS}");
            this.comboBhavInGroup.Items.Add($"{DBPFData.GROUP_BEHAVIOR} {DBPFData.NAME_BEHAVIOR}");

            this.comboOpCodeInGroup.Items.Add("");

            foreach (KeyValuePair<string, string> kvp in GameData.SemiGlobalsByName)
            {
                string group = $"0x{kvp.Value.ToUpper()} {kvp.Key}";

                this.comboBhavInGroup.Items.Add(group);
                this.comboOpCodeInGroup.Items.Add(group);
            }

            this.comboOpCode.Items.Add("");
            foreach (KeyValuePair<string, string> kvp in GameData.PrimitivesByOpCode)
            {
                this.comboOpCode.Items.Add($"{kvp.Key} {kvp.Value}");
            }

            this.comboUsingSTR.Items.Add("");
            foreach (KeyValuePair<string, string> kvp in GameData.TextlistsByInstance)
            {
                this.comboUsingSTR.Items.Add($"{kvp.Key} {kvp.Value}");
            }

            selectPathDialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true
            };
        }

        private void ClearOperands()
        {
            foreach (TextBox operand in operands)
            {
                operand.Text = "";
                toolTipOperands.SetToolTip(operand, "");
            }
        }

        private void ResetMasks()
        {
            string strDefaultMask = "FF";

            foreach (TextBox mask in masks)
            {
                mask.Text = strDefaultMask;
                toolTipOperands.SetToolTip(mask, "Binary: 1111 1111");
            }
        }

        private void UpdateForm()
        {
            bool opCodeOK = false;

            if (comboOpCode.Text.Length > 0)
            {
                if (comboOpCode.Text.IndexOf(":") != -1)
                {
                    string opCodeFrom = comboOpCode.Text.Substring(0, comboOpCode.Text.IndexOf(":"));
                    string opCodeTo = comboOpCode.Text.Substring(comboOpCode.Text.IndexOf(":") + 1);

                    Match mFrom = HexOpCodeRegex.Match(opCodeFrom);
                    Match mTo = HexOpCodeRegex.Match(opCodeTo);

                    if (mFrom.Success && mTo.Success)
                    {
                        uint from = Convert.ToUInt32(mFrom.Groups[2].Value, 16);
                        uint to = Convert.ToUInt32(mTo.Groups[2].Value, 16);

                        opCodeOK = (to > from);
                    }
                }
                else
                {
                    opCodeOK = true;
                }
            }

            bool filePathOk = false;

            if (opCodeOK && textFilePath.Text.Length > 0)
            {
                filePathOk = (Directory.Exists(textFilePath.Text) || File.Exists(textFilePath.Text));
            }

            btnGO.Enabled = (filePathOk && opCodeOK);
            bhavFoundData.Clear();
            lblProgress.Visible = false;
        }

        private void OnFilePathChanged(object sender, EventArgs e)
        {
            this.gridFoundBhavs.Columns["colBhavPackage"].Visible = Directory.Exists(textFilePath.Text);

            UpdateForm();
        }

        private void OnClearOperandsClicked(object sender, EventArgs e)
        {
            ClearOperands();
            UpdateForm();
        }

        private void OnOperandChanged(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;

            if (tb.Text.Length > 0)
            {
                if (!Hex2Regex.IsMatch(tb.Text))
                {
                    tb.Text = "";
                    toolTipOperands.SetToolTip(tb, "");
                }
                else
                {
                    toolTipOperands.SetToolTip(tb, $"Decimal: {Convert.ToUInt32(tb.Text, 16)}");
                }
            }
            else
            {
                toolTipOperands.SetToolTip(tb, "");
            }
        }

        private void OnResetMasksClicked(object sender, EventArgs e)
        {
            ResetMasks();
            UpdateForm();
        }

        private void OnMaskChanged(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;

            if (tb.Text.Length > 0)
            {
                if (!Hex2Regex.IsMatch(tb.Text))
                {
                    tb.Text = "";
                    toolTipOperands.SetToolTip(tb, "");
                }
                else
                {
                    string binStr = Helper.Binary8String(Convert.ToUInt32(tb.Text, 16));
                    toolTipOperands.SetToolTip(tb, $"Binary: {binStr.Substring(0, 4)} {binStr.Substring(4, 4)}");
                }
            }
            else
            {
                toolTipOperands.SetToolTip(tb, "");
            }
        }

        private void OnKeyPress_HexOnly(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= 'a' && e.KeyChar <= 'f')
            {
                e.KeyChar = (char)(e.KeyChar - 'a' + 'A');
            }

            if (e.KeyChar == 'X' || e.KeyChar == 'x')
            {
                e.KeyChar = 'x';

                if (((Control)sender).Text.Equals("0"))
                {
                    return;
                }
            }

            if (!(
                    char.IsControl(e.KeyChar) ||
                    (e.KeyChar >= '0' && e.KeyChar <= '9') ||
                    (e.KeyChar >= 'A' && e.KeyChar <= 'F')
                ))
            {
                e.Handled = true;
            }
        }

        private void OnKeyPress_HexRangeOnly(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= 'a' && e.KeyChar <= 'f')
            {
                e.KeyChar = (char)(e.KeyChar - 'a' + 'A');
            }

            if (e.KeyChar == 'X' || e.KeyChar == 'x')
            {
                e.KeyChar = 'x';

                string text = ((Control)sender).Text;

                if (text.Equals("0"))
                {
                    return;
                }

                if (text.IndexOf(":") != -1 && text.Substring(text.IndexOf(":") + 1).Equals("0"))
                {
                    return;
                }
            }

            if (e.KeyChar == ':')
            {
                if (((Control)sender).Text.IndexOf(":") == -1)
                {
                    return;
                }
            }

            if (!(
                    char.IsControl(e.KeyChar) ||
                    (e.KeyChar >= '0' && e.KeyChar <= '9') ||
                    (e.KeyChar >= 'A' && e.KeyChar <= 'F')
                ))
            {
                e.Handled = true;
            }
        }

        private void OnSelectClicked(object sender, EventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
            {
                OnSelectFolderClicked(sender, e);
            }
            else
            {
                if (selectFileDialog.ShowDialog() == DialogResult.OK)
                {
                    textFilePath.Text = selectFileDialog.FileName;
                }
            }
        }
        private void OnSelectFolderClicked(object sender, EventArgs e)
        {
            if (selectPathDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                textFilePath.Text = selectPathDialog.FileName;
            }
        }

        private void OnClearMatchingClicked(object sender, EventArgs e)
        {
            comboUsingOperand.SelectedIndex = 0;
            comboUsingSTR.SelectedIndex = 0;
            textUsingRegex.Text = "";
            UpdateForm();
        }

        private void OnClearGroupsClicked(object sender, EventArgs e)
        {
            comboBhavInGroup.SelectedIndex = 0;
            comboOpCodeInGroup.SelectedIndex = 0;
            UpdateForm();
        }

        private void OnClearOpCodeClicked(object sender, EventArgs e)
        {
            comboOpCode.SelectedIndex = 0;
            comboVersion.SelectedIndex = 0;
            UpdateForm();
        }

        private void OnOpCodeChanged(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;

            if (cb.Text.Length > 0)
            {
                if (cb.Text.IndexOf(":") != -1)
                {
                    string opCodeFrom = cb.Text.Substring(0, cb.Text.IndexOf(":"));
                    string opCodeTo = cb.Text.Substring(cb.Text.IndexOf(":") + 1);

                    Match mFrom = HexOpCodeRegex.Match(opCodeFrom);
                    Match mTo = HexOpCodeRegex.Match(opCodeTo);

                    if (mFrom.Success && (mTo.Success || opCodeTo.Length == 0))
                    {
                        uint from = Convert.ToUInt32(mFrom.Groups[2].Value, 16);
                        uint to = (opCodeTo.Length == 0) ? 0 : Convert.ToUInt32(mTo.Groups[2].Value, 16);

                        lblOpCodeInGroup.Visible = comboOpCodeInGroup.Visible = ((to > from) && (from >= 0x2000));
                    }
                    else
                    {
                        cb.Text = "";

                        lblOpCodeInGroup.Visible = comboOpCodeInGroup.Visible = false;
                    }
                }
                else
                {
                    Match m = HexOpCodeRegex.Match(cb.Text);
                    if (m.Success)
                    {
                        lblOpCodeInGroup.Visible = comboOpCodeInGroup.Visible = (Convert.ToUInt32(m.Groups[2].Value, 16) >= 0x2000);
                    }
                    else
                    {
                        cb.Text = "";

                        lblOpCodeInGroup.Visible = comboOpCodeInGroup.Visible = false;
                    }
                }
            }
            else
            {
                lblOpCodeInGroup.Visible = comboOpCodeInGroup.Visible = false;
            }

            UpdateForm();
        }

        private void OnKeyPress_Ignore(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void OnFileOpening(object sender, EventArgs e)
        {
            menuItemSaveResultsToClipboard.Enabled = menuItemSaveResultsAs.Enabled = bhavFoundData.HasResults;
        }

        private void OnExitClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnHelpClicked(object sender, EventArgs e)
        {
            new AboutDialog(BhavFinderApp.AppProduct).ShowDialog();
        }

        private void OnSaveResultsToClipboardClicked(object sender, EventArgs e)
        {
            string xml = GetResultsAsCSV();

            if (xml != null)
            {
                Clipboard.SetText(xml);
            }
        }

        private string GetResultsAsCSV()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (CsvWriter csvWriter = new CsvWriter(new StreamWriter(ms), CultureInfo.InvariantCulture))
                {
                    var records = new List<object>();

                    foreach (DataGridViewRow row in gridFoundBhavs.Rows)
                    {
                        records.Add(new
                        {
                            PackageName = row.Cells["colBhavPackage"].Value as string,
                            Instance = row.Cells["colBhavInstance"].Value as string,
                            Name = row.Cells["colBhavName"].Value as string,
                            Group = row.Cells["colBhavGroupInstance"].Value as string,
                            Object = row.Cells["colBhavGroupName"].Value as string
                        });
                    }

                    csvWriter.WriteRecords(records);
                    csvWriter.Flush();

                    ms.Position = 0;

                    using (StreamReader sr = new StreamReader(ms))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
        }

        private void OnSaveResultsAsClicked(object sender, EventArgs e)
        {
            saveResultsDialog.ShowDialog();

            if (saveResultsDialog.FileName != "")
            {
                string xml = GetResultsAsCSV();

                if (xml != null)
                {
                    StreamWriter writer = new StreamWriter(saveResultsDialog.OpenFile());
                    writer.WriteLine(xml);
                    writer.Close();
                }
            }
        }

        private void OnGroupChanged(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;

            if (cb.Text.Length > 0)
            {
                if (!HexGroupRegex.IsMatch(cb.Text))
                {
                    cb.Text = "";
                }
            }

            UpdateForm();
        }

        private void OnStrIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;

            if (cb.Text.Length > 0)
            {
                if (!HexInstanceRegex.IsMatch(cb.Text))
                {
                    cb.Text = "";
                }
            }

            UpdateForm();
        }

        private void OnConfigClicked(object sender, EventArgs e)
        {
            Form config = new ConfigDialog();

            if (config.ShowDialog() == DialogResult.OK)
            {
                UpdateForm();
            }
        }

        private void OnLoad(object sender, EventArgs e)
        {
            RegistryTools.LoadAppSettings(BhavFinderApp.RegistryKey, BhavFinderApp.AppVersionMajor, BhavFinderApp.AppVersionMinor);
            RegistryTools.LoadFormSettings(BhavFinderApp.RegistryKey, this);
            checkShowNames.Checked = bool.Parse(RegistryTools.GetSetting(BhavFinderApp.RegistryKey, checkShowNames.Name, checkShowNames.Checked.ToString()).ToString());
            OnSwitchGroupChanged(checkShowNames, null);

            MyMruList = new MruList(BhavFinderApp.RegistryKey, menuItemRecentPackages, Properties.Settings.Default.MruSize, true, true);
            MyMruList.FileSelected += MyMruList_FileSelected;

            MyUpdater = new Updater(BhavFinderApp.RegistryKey, menuHelp);
            MyUpdater.CheckForUpdates();

            UpdateForm();
        }

        private void UpdateLocalObjects()
        {
            localObjectsByGroupID.Clear();

            if (File.Exists(textFilePath.Text))
            {
                try
                {
                    using (DBPFFile package = new DBPFFile(textFilePath.Text))
                    {
                        GameData.BuildObjectsTable(package, localObjectsByGroupID, null);

                        package.Close();
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                    logger.Info(ex.StackTrace);

                    MsgBox.Show($"Unable to open/read '{textFilePath.Text}'", "Error!", MessageBoxButtons.OK);
                }

#if DEBUG
                logger.Info($"Loaded {localObjectsByGroupID.Count} local objects");
#endif
            }
        }

        private void MyMruList_FileSelected(string package)
        {
            textFilePath.Text = package;
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            RegistryTools.SaveAppSettings(BhavFinderApp.RegistryKey, BhavFinderApp.AppVersionMajor, BhavFinderApp.AppVersionMinor);
            RegistryTools.SaveFormSettings(BhavFinderApp.RegistryKey, this);
            RegistryTools.SaveSetting(BhavFinderApp.RegistryKey, checkShowNames.Name, checkShowNames.Checked.ToString());
        }

        private void OnSwitchGroupChanged(object sender, EventArgs e)
        {
            this.gridFoundBhavs.Columns["colBhavGroupInstance"].Visible = !checkShowNames.Checked;
            this.gridFoundBhavs.Columns["colBhavGroupName"].Visible = checkShowNames.Checked;
        }

        private void OnGoClicked(object sender, EventArgs e)
        {
            if (bhavFinderWorker.IsBusy)
            {
                // This is the Cancel action
                Debug.Assert(bhavFinderWorker.WorkerSupportsCancellation == true);

                // Cancel the asynchronous operation.
                bhavFinderWorker.CancelAsync();
            }
            else
            {
                strLookupByIndexGlobal = null;
                strLookupByIndexLocal = null;

                UpdateLocalObjects();

                // This is the Search action
                bhavFoundData.Clear();
                btnGO.Text = "Cancel";

                lblProgress.Text = "Progress:";
                lblProgress.Visible = true;
                progressBar.Visible = true;
                progressBar.Value = 0;

                if (comboUsingOperand.Text.Length > 0 &&
                    comboUsingSTR.Text.Length > 0 && HexOpCodeRegex.IsMatch(comboUsingSTR.Text) &&
                    textUsingRegex.Text.Length > 0)
                {
                    lblProgress.Refresh();
                    progressBar.Refresh();

                    try
                    {
                        usingRegex = new Regex(textUsingRegex.Text);
                        Match m = HexInstanceRegex.Match(comboUsingSTR.Text);
                        usingInstance = (TypeInstanceID)Convert.ToUInt32(m.Groups[2].ToString(), 16);

                        string sims2Path = Sims2ToolsLib.Sims2Path;
                        if (sims2Path.Length > 0)
                        {
                            strLookupByIndexGlobal = BuildStrLookupTable(sims2Path + GameData.objectsSubPath, usingInstance, usingRegex);
                        }

                        if (File.Exists(textFilePath.Text))
                        {
                            strLookupByIndexLocal = BuildStrLookupTable(textFilePath.Text, usingInstance, usingRegex);
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.Message);
                        logger.Info(ex.StackTrace);

                        MsgBox.Show("Unable to build STR# lookup tables", "Error!", MessageBoxButtons.OK);
                    }
                }

                bhavFinderWorker.RunWorkerAsync(GetFilters());
            }
        }

        private void BhavFinderWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            BhavFilter filter = e.Argument as BhavFilter;

            int found = 0;

            if (Directory.Exists(textFilePath.Text))
            {
                List<string> packageFiles = new List<string>(Directory.GetFiles(textFilePath.Text, "*.package", SearchOption.AllDirectories));

                int done = 0;

                foreach (string packageFile in packageFiles)
                {
                    if (strLookupByIndexGlobal != null)
                    {
                        try
                        {
                            strLookupByIndexLocal = BuildStrLookupTable(packageFile, usingInstance, usingRegex);
                        }
                        catch (Exception ex)
                        {
                            logger.Error(ex.Message);
                            logger.Info(ex.StackTrace);

                            MsgBox.Show($"Unable to build STR# lookup table for {packageFile}", "Error!", MessageBoxButtons.OK);
                        }
                    }

                    found = ProcessPackage(worker, e, packageFile, filter, found, false);

                    int percentComplete = (int)((++done / (float)packageFiles.Count) * 100.0);
                    worker.ReportProgress(percentComplete, null);
                }
            }
            else
            {
                found = ProcessPackage(worker, e, textFilePath.Text, filter, found, true);
            }

            e.Result = found;
        }

        private int ProcessPackage(BackgroundWorker worker, DoWorkEventArgs e, string packagePath, BhavFilter filter, int found, bool reportPercent)
        {
            FileInfo fi = new FileInfo(packagePath);

            using (DBPFFile package = new DBPFFile(packagePath))
            {
                List<DBPFEntry> bhavs = package.GetEntriesByType(Bhav.TYPE);
                int done = 0;

                foreach (var entry in bhavs)
                {
                    if (worker.CancellationPending == true)
                    {
                        e.Cancel = true;
                        break;
                    }
                    else
                    {
                        Bhav bhav = (Bhav)package.GetResourceByEntry(entry);

                        int percentComplete = (int)((++done / (float)bhavs.Count) * 100.0);

                        if (filter.IsWanted(bhav))
                        {
                            DataRow row = bhavFoundData.NewRow();
                            row["Package"] = fi.Name;
                            row["DbpfEntry"] = entry;
                            row["Instance"] = entry.InstanceID.ToShortString();
                            row["Name"] = bhav.KeyName;
                            row["GroupInstance"] = entry.GroupID.ToString();
                            row["GroupName"] = GameData.GroupName(entry.GroupID, localObjectsByGroupID);

                            worker.ReportProgress((reportPercent) ? percentComplete : 0, row);
#if DEBUG
                            if (reportPercent && bhavs.Count < 30) System.Threading.Thread.Sleep(300);
#endif
                            ++found;
                        }
                        else
                        {
                            if (reportPercent) worker.ReportProgress(percentComplete, null);
                        }
                    }
                }

                package.Close();
            }

            return found;
        }

        private void BhavFinderWorker_Progress(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage != 0)
            {
                progressBar.Value = e.ProgressPercentage;
            }

            if (e.UserState != null)
            {
                bhavFoundData.Append(e.UserState as DataRow);
            }
        }

        private void BhavFinderWorker_Completed(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            lblProgress.Visible = false;
            progressBar.Visible = false;

            if (e.Error != null)
            {
                MyMruList.RemoveFile(textFilePath.Text);
                textFilePath.Text = "";

                logger.Error(e.Error.Message);
                logger.Info(e.Error.StackTrace);

                MsgBox.Show("An error occured while searching", "Error!", MessageBoxButtons.OK);
            }
            else
            {
                MyMruList.AddFile(textFilePath.Text);

                if (e.Cancelled == true)
                {
                }
                else
                {
                    lblProgress.Text = $"Total: {Convert.ToInt32(e.Result)}";
                    lblProgress.Visible = true;
                }
            }

            btnGO.Text = "FIND &BHAVs";
        }

        private Dictionary<int, HashSet<TypeGroupID>> BuildStrLookupTable(string packagePath, TypeInstanceID instanceID, Regex regex)
        {
            Dictionary<int, HashSet<TypeGroupID>> lookup = new Dictionary<int, HashSet<TypeGroupID>>();

            using (DBPFFile package = new DBPFFile(packagePath))
            {
                foreach (var entry in package.GetEntriesByType(Str.TYPE))
                {
                    if (entry.InstanceID == instanceID)
                    {
                        Str str = (Str)package.GetResourceByEntry(entry);
                        List<StrItem> entries = str.LanguageItems(MetaData.Languages.Default);

                        for (int i = 0; i < entries.Count; ++i)
                        {
                            if (regex.IsMatch(entries[i].Title))
                            {

                                if (!lookup.TryGetValue(i, out HashSet<TypeGroupID> groups))
                                {
                                    groups = new HashSet<TypeGroupID>();
                                    lookup.Add(i, groups);
                                }

                                groups.Add(entry.GroupID);
                            }
                        }
                    }
                }

                package.Close();
            }

            return lookup;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5 || (e.Modifiers == Keys.Control && e.KeyCode == Keys.R))
            {
                OnGoClicked(btnGO, null);
            }
            else if (e.Modifiers == Keys.Alt && e.KeyCode == Keys.N)
            {
                checkShowNames.Checked = !checkShowNames.Checked;
            }
            else if (e.Modifiers == Keys.Alt && e.KeyCode == Keys.X)
            {
                OnClearOpCodeClicked(btnClearOpCode, null);
                OnClearOperandsClicked(btnClearOperands, null);
                OnResetMasksClicked(btnResetMasks, null);
                OnClearGroupsClicked(btnClearGroups, null);
                OnClearMatchingClicked(btnUsingClear, null);

                comboOpCode.Focus();
            }
        }

        private void OnContextMenuOperandsOpening(object sender, CancelEventArgs e)
        {
            menuItemPasteGUID.Enabled = Clipboard.ContainsText() && HexGUIDRegex.IsMatch(Clipboard.GetText(TextDataFormat.Text));
        }

        private void OnPasteGuidClicked(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                int index = Array.IndexOf(operands, ((sender as ToolStripMenuItem).Owner as ContextMenuStrip).SourceControl as TextBox);

                if (index >= 0 && index <= 12)
                {
                    Match m = HexGUIDRegex.Match(Clipboard.GetText(TextDataFormat.Text));

                    TypeGUID GUID = (TypeGUID)Convert.ToUInt32(m.Groups[2].Value, 16);

                    for (int i = 0; i < 4; ++i)
                    {
                        operands[index++].Text = Helper.Hex2String(GUID % 256);
                        GUID /= 256;
                    }
                }
            }
        }

        private void OnContextFoundBhavsOpening(object sender, CancelEventArgs e)
        {
            menuItemExtract.Enabled = (gridFoundBhavs.SelectedRows.Count > 0);
        }

        private void OnExtractBhavsClicked(object sender, EventArgs e)
        {
            if (selectPathDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string extractPath = selectPathDialog.FileName;

                string bhavSubPath = "42484156 - Behaviour Function";
                string dataFilePath = $"{extractPath}\\{bhavSubPath}";

                Directory.CreateDirectory(dataFilePath);

                using (DBPFFile package = new DBPFFile(textFilePath.Text))
                {
                    StreamWriter packageWriter = new StreamWriter($"{extractPath}\\package.xml");
                    packageWriter.WriteLine($"<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
                    packageWriter.WriteLine($"<package type=\"2\">");

                    foreach (DataGridViewRow row in gridFoundBhavs.SelectedRows)
                    {
                        DBPFEntry entry = row.Cells["colBhavDbpfEntry"].Value as DBPFEntry;
                        string dataFileName = $"{entry.ResourceID.Hex8String()}-{entry.GroupID.Hex8String()}-{entry.InstanceID.Hex8String()}.simpe";

                        byte[] data = package.GetOriginalItemByEntry(entry);

                        Stream dataWriter = new FileStream($"{dataFilePath}\\{dataFileName}", FileMode.OpenOrCreate, FileAccess.Write);
                        dataWriter.Write(data, 0, data.Length);
                        dataWriter.Close();

                        StreamWriter xmlWriter = new StreamWriter($"{dataFilePath}\\{dataFileName}.xml");
                        xmlWriter.WriteLine($"<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
                        xmlWriter.WriteLine($"<package type=\"2\">");
                        xmlWriter.WriteLine($"    <packedfile path=\"\" name=\"{dataFileName}\">");
                        xmlWriter.WriteLine($"        <type>");
                        xmlWriter.WriteLine($"            <number>1112031574</number>");
                        xmlWriter.WriteLine($"        </type>");
                        xmlWriter.WriteLine($"        <classid>0</classid>");
                        xmlWriter.WriteLine($"        <group>{entry.GroupID.IntString()}</group>");
                        xmlWriter.WriteLine($"        <instance>{entry.InstanceID.IntString()}</instance>");
                        xmlWriter.WriteLine($"    </packedfile>");
                        xmlWriter.WriteLine($"</package>");
                        xmlWriter.Close();

                        packageWriter.WriteLine($"    <packedfile path=\"42484156 - Behaviour Function\" name=\"{dataFileName}\">");
                        packageWriter.WriteLine($"        <type>");
                        packageWriter.WriteLine($"            <number>1112031574</number>");
                        packageWriter.WriteLine($"        </type>");
                        packageWriter.WriteLine($"        <classid>0</classid>");
                        packageWriter.WriteLine($"        <group>{entry.GroupID.IntString()}</group>");
                        packageWriter.WriteLine($"        <instance>{entry.InstanceID.IntString()}</instance>");
                        packageWriter.WriteLine($"    </packedfile>");
                    }

                    packageWriter.WriteLine($"</package>");
                    packageWriter.Close();

                    package.Close();
                }
            }
        }
    }
}
