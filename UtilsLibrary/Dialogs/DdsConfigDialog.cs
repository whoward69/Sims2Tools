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
    public partial class DdsConfigDialog : Form
    {
        public DdsConfigDialog()
        {
            InitializeComponent();

            selectPathDialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true
            };
        }

        private void OnConfigLoad(object sender, EventArgs e)
        {
            textDdsUtilsPath.Text = Sims2ToolsLib.Sims2DdsUtilsPath;

            UpdateFormState();
        }

        private void OnDdsUtilsPathChanged(object sender, EventArgs e)
        {
            btnConfigOK.Enabled = false;

            if (!string.IsNullOrWhiteSpace(textDdsUtilsPath.Text))
            {
                btnConfigOK.Enabled = File.Exists($"{textDdsUtilsPath.Text}\\nvdxt.exe");
            }
        }

        private void OnDdsUtilsPathKeyUp(object sender, KeyEventArgs e)
        {
            OnDdsUtilsPathChanged(textDdsUtilsPath, null);
        }

        private void OnConfigOkClicked(object sender, EventArgs e)
        {
            Sims2ToolsLib.Sims2DdsUtilsPath = textDdsUtilsPath.Text;

            this.Close();
        }

        private void OnSelectClicked(object sender, EventArgs e)
        {
            selectPathDialog.InitialDirectory = textDdsUtilsPath.Text;

            if (string.IsNullOrWhiteSpace(selectPathDialog.InitialDirectory)) selectPathDialog.InitialDirectory = Sims2ToolsLib.Sims2DdsUtilsPath;

            if (selectPathDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                textDdsUtilsPath.Text = selectPathDialog.FileName;
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

        private void OnTextChanged(object sender, EventArgs e)
        {
            UpdateFormState();
        }

        private void UpdateFormState()
        {
            Color badColour = Color.LightCoral;

            bool ddsUtilsPathOk = (string.IsNullOrEmpty(textDdsUtilsPath.Text) || File.Exists($"{textDdsUtilsPath.Text}\\nvdxt.exe"));
            this.textDdsUtilsPath.BackColor = ddsUtilsPathOk ? SystemColors.Window : badColour;

            btnConfigOK.Enabled = ddsUtilsPathOk;
        }

    }
}
