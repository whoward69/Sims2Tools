/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2021
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Windows.Forms;

namespace Sims2Tools
{
    public partial class Sims2ToolsConfigDialog : Form
    {
        public Sims2ToolsConfigDialog()
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
            String oldSims2Path = Sims2ToolsLib.Sims2Path;
            String oldSimPePath = Sims2ToolsLib.SimPePath;

            Sims2ToolsLib.Sims2Path = textSims2Path.Text;
            Sims2ToolsLib.SimPePath = textSimPEPath.Text;

            this.Close();

            // As updating the global objects is a long process, it's worth checking that one of these actually changed
            if (!(textSims2Path.Text.Equals(oldSims2Path) && textSimPEPath.Text.Equals(oldSimPePath)))
            {
                GameData.UpdateGlobalObjects();
            }
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
