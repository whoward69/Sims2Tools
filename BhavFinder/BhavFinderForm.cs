/*
 * BHAV Finder - a utility for searching The Sims 2 package files for BHAV that match specified criteria
 *             - see http://www.picknmixmods.com/Sims2/Notes/BhavFinder/BhavFinder.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.BHAV;
using Sims2Tools.DBPF.Data;
using Sims2Tools.DBPF.STR;
using Sims2Tools.DBPF.Utils;
using Sims2Tools.Utils.Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BhavFinder
{
    public partial class BhavFinderForm : Form
    {
        private readonly SortedDictionary<String, String> localObjectsByGroupID = new SortedDictionary<string, string>();

        private MruList MyMruList;

        private readonly TextBox[] operands = new TextBox[16];
        private readonly TextBox[] masks = new TextBox[16];

        private readonly Regex Hex2Regex = new Regex(@"^([0-9A-F][0-9A-F]?)$");
        private readonly Regex HexGroupRegex = new Regex(@"^(0[xX])?([0-9A-Fa-f]+)");
        private readonly Regex HexGUIDRegex = new Regex(@"^(0[xX])?([0-9A-Fa-f]+)");
        private readonly Regex HexOpCodeRegex = new Regex(@"^(0[xX])?([0-9A-Fa-f]+)");
        private readonly Regex HexInstanceRegex = new Regex(@"^(0[xX])?([0-9A-Fa-f]+)");

        private readonly BhavFinderData bhavData = new BhavFinderData();

        public BhavFinderForm()
        {
            InitializeComponent();
            this.Text = BhavFinderApp.AppName;

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

            gridBhavs.DataSource = bhavData;

            this.comboBhavInGroup.Items.Add("");
            this.comboBhavInGroup.Items.Add(Helper.Hex8PrefixString(DBPFData.GROUP_LOCAL) + " " + DBPFData.NAME_LOCAL);
            this.comboBhavInGroup.Items.Add(Helper.Hex8PrefixString(DBPFData.GROUP_GLOBALS) + " " + DBPFData.NAME_GLOBALS);
            this.comboBhavInGroup.Items.Add(Helper.Hex8PrefixString(DBPFData.GROUP_BEHAVIOR) + " " + DBPFData.NAME_BEHAVIOR);

            this.comboOpCodeInGroup.Items.Add("");

            foreach (KeyValuePair<String, String> kvp in GameData.semiGlobalsByName)
            {
                String group = "0x" + kvp.Value.ToUpper() + " " + kvp.Key;

                this.comboBhavInGroup.Items.Add(group);
                this.comboOpCodeInGroup.Items.Add(group);
            }

            this.comboOpCode.Items.Add("");
            foreach (KeyValuePair<String, String> kvp in GameData.primitivesByOpCode)
            {
                this.comboOpCode.Items.Add(kvp.Key + " " + kvp.Value);
            }

            this.comboUsingSTR.Items.Add("");
            foreach (KeyValuePair<String, String> kvp in GameData.textlistsByInstance)
            {
                this.comboUsingSTR.Items.Add(kvp.Key + " " + kvp.Value);
            }
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
            String strDefaultMask = "FF";

            foreach (TextBox mask in masks)
            {
                mask.Text = strDefaultMask;
                toolTipOperands.SetToolTip(mask, "Binary: 1111 1111");
            }
        }

        private void UpdateForm()
        {
            btnGO.Enabled = (textFilePath.Text.Length > 0 && comboOpCode.Text.Length > 0);
            bhavData.Clear();
            lblProgress.Visible = false;
        }

        private void OnFilePathChanged(object sender, EventArgs e)
        {
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
                    toolTipOperands.SetToolTip(tb, "Decimal: " + Convert.ToUInt32(tb.Text, 16));
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
                    String binStr = Convert.ToString(Convert.ToUInt32(tb.Text, 16), 2);
                    binStr = "0000000" + binStr;
                    binStr = binStr.Substring(binStr.Length - 8, 8);
                    toolTipOperands.SetToolTip(tb, "Binary: " + binStr.Substring(0, 4) + " " + binStr.Substring(4, 4));
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
                    Char.IsControl(e.KeyChar) ||
                    (e.KeyChar >= '0' && e.KeyChar <= '9') ||
                    (e.KeyChar >= 'A' && e.KeyChar <= 'F')
                ))
            {
                e.Handled = true;
            }
        }

        private void OnSelectClicked(object sender, EventArgs e)
        {
            if (selectFileDialog.ShowDialog() == DialogResult.OK)
            {
                textFilePath.Text = selectFileDialog.FileName;
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

        private void OnExitClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnHelpClicked(object sender, EventArgs e)
        {
            new Sims2ToolsAboutDialog(BhavFinderApp.AppProduct).ShowDialog();
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
            Form config = new Sims2ToolsConfigDialog();

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

            MyMruList = new MruList(BhavFinderApp.RegistryKey, menuItemRecentPackages, 8);
            MyMruList.FileSelected += MyMruList_FileSelected;

            UpdateForm();
        }

        private void UpdateLocalObjects()
        {
            localObjectsByGroupID.Clear();

            try
            {
                DBPFFile package = new DBPFFile(textFilePath.Text);

                GameData.BuildObjectsTable(package, localObjectsByGroupID);
            }
#if DEBUG
            catch (Exception ex)
#else
            catch (Exception)
#endif
            {
                MessageBox.Show("Unable to open/read '" + textFilePath.Text + "'", "Error!", MessageBoxButtons.OK);
#if DEBUG
                Console.WriteLine(ex.Message);
#endif
            }

#if DEBUG
            Console.WriteLine("Loaded " + localObjectsByGroupID.Count + " local objects");
#endif
        }

        private void MyMruList_FileSelected(String package)
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
            this.gridBhavs.Columns["colBhavGroupInstance"].Visible = !checkShowNames.Checked;
            this.gridBhavs.Columns["colBhavGroupName"].Visible = checkShowNames.Checked;
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
                Dictionary<int, HashSet<uint>> strLookupByIndexLocal = null;
                Dictionary<int, HashSet<uint>> strLookupByIndexGlobal = null;

                UpdateLocalObjects();

                // This is the Search action
                bhavData.Clear();
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
                        Regex regex = new Regex(textUsingRegex.Text);
                        int operand = Convert.ToInt32(comboUsingOperand.Text, 10);
                        Match m = HexInstanceRegex.Match(comboUsingSTR.Text);
                        uint instance = Convert.ToUInt32(m.Groups[2].ToString(), 16);

                        String sims2Path = RegistryTools.GetSetting(BhavFinderApp.RegistryKey, Sims2Tools.Sims2ToolsLib.Sims2PathKey, "") as String;
                        if (sims2Path.Length > 0)
                        {
                            strLookupByIndexGlobal = BuildStrLookupTable(sims2Path + GameData.objectsSubPath, instance, regex);
                        }

                        strLookupByIndexLocal = BuildStrLookupTable(textFilePath.Text, instance, regex);
                    }
#if DEBUG
                    catch (Exception ex)
#else
                    catch (Exception)
#endif
                    {
                        MessageBox.Show("Unable to build STR# lookup tables", "Error!", MessageBoxButtons.OK);
#if DEBUG
                        Console.WriteLine(ex.Message);
#endif
                    }
                }

                BhavFilter filter = GetFilters(strLookupByIndexLocal, strLookupByIndexGlobal);

                bhavFinderWorker.RunWorkerAsync(filter);
            }
        }

        private void BhavFinderWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            BhavFilter filter = e.Argument as BhavFilter;

            DBPFFile package = new DBPFFile(textFilePath.Text);

            List<DBPFEntry> bhavs = package.GetEntriesByType(Bhav.TYPE);
            int done = 0;
            int found = 0;

            foreach (var entry in bhavs)
            {
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    Bhav bhav = new Bhav(entry, package.GetIoBuffer(entry));

                    int percentComplete = (int)((++done / (float)bhavs.Count) * 100.0);

                    if (filter.IsWanted(bhav))
                    {
                        DataRow row = bhavData.NewRow();
                        row["Instance"] = Helper.Hex4PrefixString(entry.InstanceID);
                        row["Name"] = bhav.FileName;
                        row["GroupInstance"] = Helper.Hex8PrefixString(entry.GroupID);
                        row["GroupName"] = GameData.GroupName(entry.GroupID, localObjectsByGroupID);

                        worker.ReportProgress(percentComplete, row);
#if DEBUG
                        if (bhavs.Count < 30) System.Threading.Thread.Sleep(300);
#endif
                        ++found;
                    }
                    else
                    {
                        worker.ReportProgress(percentComplete, null);
                    }
                }
            }

            e.Result = found;
        }

        private void BhavFinderWorker_Progress(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage != 0)
            {
                progressBar.Value = e.ProgressPercentage;
            }

            if (e.UserState != null)
            {
                bhavData.Append(e.UserState as DataRow);
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

                MessageBox.Show("An error occured while searching", "Error!", MessageBoxButtons.OK);
#if DEBUG
                Console.WriteLine(e.Error.Message);
#endif
            }
            else
            {
                MyMruList.AddFile(textFilePath.Text);

                if (e.Cancelled == true)
                {
                }
                else
                {
                    lblProgress.Text = "Total: " + Convert.ToInt32(e.Result);
                    lblProgress.Visible = true;
                }
            }

            btnGO.Text = "FIND &BHAVs";
        }

        private Dictionary<int, HashSet<uint>> BuildStrLookupTable(String packagePath, uint instanceID, Regex regex)
        {
            Dictionary<int, HashSet<uint>> lookup = new Dictionary<int, HashSet<uint>>();

            DBPFFile package = new DBPFFile(packagePath);

            foreach (var entry in package.GetEntriesByType(Str.TYPE))
            {
                if (entry.InstanceID == instanceID)
                {
                    Str str = new Str(entry, package.GetIoBuffer(entry));
                    StrItemList entries = str.LanguageItems(MetaData.Languages.English);

                    for (int i = 0; i < entries.Length; ++i)
                    {
                        if (regex.IsMatch(entries[i].Title))
                        {

                            if (!lookup.TryGetValue(i, out HashSet<uint> groups))
                            {
                                groups = new HashSet<uint>();
                                lookup.Add(i, groups);
                            }

                            groups.Add(entry.GroupID);
                        }
                    }
                }
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

        private void PasteGuidClicked(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                int index = Array.IndexOf(operands, ((sender as ToolStripMenuItem).Owner as ContextMenuStrip).SourceControl as TextBox);

                if (index >= 0 && index <= 12)
                {
                    Match m = HexGUIDRegex.Match(Clipboard.GetText(TextDataFormat.Text));

                    UInt32 GUID = Convert.ToUInt32(m.Groups[2].Value, 16);

                    for (int i = 0; i < 4; ++i)
                    {
                        operands[index++].Text = Helper.Hex2String((byte)(GUID % 256));
                        GUID /= 256;
                    }
                }
            }
        }
    }
}
