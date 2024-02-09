/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2023
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System;
using System.Windows.Forms;

namespace Sims2Tools
{
    public partial class Sims2ToolsCreatorEntryDialog : Form
    {
        public Sims2ToolsCreatorEntryDialog()
        {
            InitializeComponent();
        }

        private void OnConfigLoad(object sender, EventArgs e)
        {
            textCreatorNickName.Text = Sims2ToolsLib.CreatorNickName;
            textCreatorGUID.Text = Sims2ToolsLib.CreatorGUID;
        }

        private void OnConfigOkClicked(object sender, EventArgs e)
        {
            Sims2ToolsLib.CreatorNickName = textCreatorNickName.Text;
            Sims2ToolsLib.CreatorGUID = textCreatorGUID.Text;

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
