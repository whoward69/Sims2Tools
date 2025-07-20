/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2025
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
        public ConfigDialog(bool requiresInstallPath)
        {
            InitializeComponent();

            selectPathDialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true
            };

            lblSims2InstallPath.Visible = lblHelpInstallPath.Visible = textSims2InstallPath.Visible = btnSims2InstallSelect.Visible = btnSims2EpSpSelect.Visible = requiresInstallPath;
        }

        private void OnConfigLoad(object sender, EventArgs e)
        {
            textSims2ExePath.Text = Sims2ToolsLib.Sims2Path;

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

            if (Sims2ToolsLib.IsSims2InstallPathSet)
            {
                textSims2InstallPath.Text = Sims2ToolsLib.Sims2InstallPath;
            }
            else
            {
#pragma warning disable CS0612
                string basePath = SimpeData.PathSetting("Sims2Path");

                if (!string.IsNullOrEmpty(basePath))
                {
                    if (basePath.EndsWith("Ultimate Collection\\Double Deluxe\\Base"))
                    {
                        textSims2InstallPath.Text = basePath.Substring(0, basePath.Length - 19);
                    }
                    else if (basePath.EndsWith("Legacy Collection\\Base"))
                    {
                        textSims2InstallPath.Text = basePath.Substring(0, basePath.Length - 5);
                    }
                    else if (basePath.Contains("\\EA GAMES\\"))
                    {
                        int pos = basePath.IndexOf("\\EA GAMES\\");
                        textSims2InstallPath.Text = basePath.Substring(0, pos + 9);
                    }
                }
#pragma warning restore CS0612
            }

            ckbAllAdvancedMode.Checked = Sims2ToolsLib.AllAdvancedMode;

            UpdateFormState();
        }

        private void UpdateFormState()
        {
            Color badColour = Color.LightCoral;

            bool sims2ExePathOk = File.Exists($"{textSims2ExePath.Text}\\TSData\\Res\\Objects\\objects.package");
            this.textSims2ExePath.BackColor = sims2ExePathOk ? System.Drawing.SystemColors.Window : badColour;

            bool sims2HomePathOk = Directory.Exists($"{textSims2HomePath.Text}\\Neighborhoods");
            this.textSims2HomePath.BackColor = sims2HomePathOk ? System.Drawing.SystemColors.Window : badColour;

            bool sims2InstallPathOk = !textSims2InstallPath.Visible || (string.IsNullOrEmpty(textSims2InstallPath.Text) || Directory.Exists(textSims2InstallPath.Text));
            this.textSims2InstallPath.BackColor = sims2InstallPathOk ? System.Drawing.SystemColors.Window : badColour;

            btnSims2EpSpSelect.Enabled = Directory.Exists(textSims2InstallPath.Text);

            btnConfigOK.Enabled = (sims2ExePathOk && sims2HomePathOk && sims2InstallPathOk);
        }

        private void OnSelectSims2ExePathClicked(object sender, EventArgs e)
        {
            selectPathDialog.InitialDirectory = textSims2ExePath.Text;

            if (selectPathDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                textSims2ExePath.Text = selectPathDialog.FileName;
            }
        }

        private void OnSelectSims2HomePathClicked(object sender, EventArgs e)
        {
            selectPathDialog.InitialDirectory = textSims2HomePath.Text;

            if (selectPathDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                textSims2HomePath.Text = selectPathDialog.FileName;
            }
        }

        private void OnSelectSims2InstallPathClicked(object sender, EventArgs e)
        {
            selectPathDialog.InitialDirectory = textSims2InstallPath.Text;

            if (selectPathDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                textSims2InstallPath.Text = selectPathDialog.FileName;
            }
        }

        private void OnSelectSims2EpSpPathClicked(object sender, EventArgs e)
        {
            Form epSpConfig = new EpSpConfigDialog(textSims2InstallPath.Text);

            if (epSpConfig.ShowDialog() == DialogResult.OK)
            {
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
            string oldSims2BasePath = Sims2ToolsLib.Sims2InstallPath;

            Sims2ToolsLib.Sims2Path = textSims2ExePath.Text;
            Sims2ToolsLib.Sims2HomePath = textSims2HomePath.Text;
            Sims2ToolsLib.Sims2InstallPath = textSims2InstallPath.Text;

            Sims2ToolsLib.AllAdvancedMode = ckbAllAdvancedMode.Checked;

            // As updating the global objects is a long process, it's worth checking that one of these actually changed
            if (!(textSims2ExePath.Text.Equals(oldSims2Path) && textSims2HomePath.Text.Equals(oldSims2HomePath) && textSims2InstallPath.Text.Equals(oldSims2BasePath)))
            {
                btnConfigOK.Enabled = false;
                GameData.UpdateGlobalObjects();
            }

            // Sims2ToolsLib.SimPePath = null;

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
