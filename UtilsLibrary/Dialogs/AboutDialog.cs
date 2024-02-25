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
    public partial class AboutDialog : Form
    {
        AboutDialog()
        {
            InitializeComponent();
        }

        public AboutDialog(String product) : this()
        {
            textProduct.Text = product;

            textCopyright.Text = Sims2ToolsLib.Copyright;
        }

        private void OnAboutOkClicked(object sender, EventArgs e)
        {
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
