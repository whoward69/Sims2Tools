/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2023
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Microsoft.WindowsAPICodePack.Dialogs;
using System;
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

            textSimPEPath.Text = Sims2ToolsLib.SimPePath;
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
            selectPathDialog.InitialDirectory = textSimPEPath.Text;

            if (selectPathDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                textSimPEPath.Text = selectPathDialog.FileName;
            }
        }

        private void OnConfigOkClicked(object sender, EventArgs e)
        {
            string oldSims2Path = Sims2ToolsLib.Sims2Path;
            string oldSims2HomePath = Sims2ToolsLib.Sims2HomePath;
            string oldSimPePath = Sims2ToolsLib.SimPePath;

            Sims2ToolsLib.Sims2Path = textSims2Path.Text;
            Sims2ToolsLib.Sims2HomePath = textSims2HomePath.Text;
            Sims2ToolsLib.SimPePath = textSimPEPath.Text;

            // As updating the global objects is a long process, it's worth checking that one of these actually changed
            if (!(textSims2Path.Text.Equals(oldSims2Path) && textSims2HomePath.Text.Equals(oldSims2HomePath) && textSimPEPath.Text.Equals(oldSimPePath)))
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
