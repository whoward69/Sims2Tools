/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2024
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Sims2Tools
{
    public partial class ConfigDialog : Form
    {
        public ConfigDialog()
        {
            InitializeComponent();

            selectPathDialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true
            };
        }

        private void OnConfigLoad(object sender, EventArgs e)
        {
            textSims2Path.Text = Sims2ToolsLib.Sims2Path;

            if (Sims2ToolsLib.IsSims2HomePathSet)
            {
                textSims2HomePath.Text = Sims2ToolsLib.Sims2HomePath;
            }
            else
            {
                string homePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\EA Games\\";

                if (Directory.Exists($"{homePath}The Sims™ 2 Ultimate Collection"))
                {
                    textSims2HomePath.Text = $"{homePath}The Sims™ 2 Ultimate Collection";
                }
                else if (Directory.Exists($"{homePath}The Sims 2"))
                {
                    textSims2HomePath.Text = $"{homePath}The Sims 2";
                }
            }

            textSimPePath.Text = Sims2ToolsLib.SimPePath;

            ckbAllAdvancedMode.Checked = Sims2ToolsLib.AllAdvancedMode;

            UpdateFormState();
        }

        private void UpdateFormState()
        {
            Color badColour = Color.LightCoral;

            bool sims2PathOk = Directory.Exists(textSims2Path.Text);
            this.textSims2Path.BackColor = sims2PathOk ? System.Drawing.SystemColors.Window : badColour;

            bool sims2HomePathOk = Directory.Exists(textSims2HomePath.Text);
            this.textSims2HomePath.BackColor = sims2HomePathOk ? System.Drawing.SystemColors.Window : badColour;

            bool simsPePathOk = string.IsNullOrEmpty(textSimPePath.Text) || (Directory.Exists(textSimPePath.Text) && File.Exists($"{textSimPePath.Text}/Data/simpe.xreg"));
            this.textSimPePath.BackColor = simsPePathOk ? System.Drawing.SystemColors.Window : badColour;

            btnConfigOK.Enabled = (sims2PathOk && sims2HomePathOk && simsPePathOk);
        }

        private void OnSelectSim2PathClicked(object sender, EventArgs e)
        {
            selectPathDialog.InitialDirectory = textSims2Path.Text;

            if (selectPathDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                textSims2Path.Text = selectPathDialog.FileName;
            }
        }

        private void OnSelectSim2HomePathClicked(object sender, EventArgs e)
        {
            selectPathDialog.InitialDirectory = textSims2HomePath.Text;

            if (selectPathDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                textSims2HomePath.Text = selectPathDialog.FileName;
            }
        }

        private void OnSelectSimPEPathClicked(object sender, EventArgs e)
        {
            selectPathDialog.InitialDirectory = textSimPePath.Text;

            if (selectPathDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                textSimPePath.Text = selectPathDialog.FileName;
            }
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            UpdateFormState();
        }

        private void OnConfigOkClicked(object sender, EventArgs e)
        {
            string oldSims2Path = Sims2ToolsLib.Sims2Path;
            string oldSims2HomePath = Sims2ToolsLib.Sims2HomePath;
            string oldSimPePath = Sims2ToolsLib.SimPePath;

            Sims2ToolsLib.Sims2Path = textSims2Path.Text;
            Sims2ToolsLib.Sims2HomePath = textSims2HomePath.Text;
            Sims2ToolsLib.SimPePath = textSimPePath.Text;

            Sims2ToolsLib.AllAdvancedMode = ckbAllAdvancedMode.Checked;

            // As updating the global objects is a long process, it's worth checking that one of these actually changed
            if (!(textSims2Path.Text.Equals(oldSims2Path) && textSims2HomePath.Text.Equals(oldSims2HomePath) && textSimPePath.Text.Equals(oldSimPePath)))
            {
                btnConfigOK.Enabled = false;
                GameData.UpdateGlobalObjects();
            }

            this.Close();
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (Form.ModifierKeys == Keys.None && keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }

            return base.ProcessDialogKey(keyData);
        }
    }
}
