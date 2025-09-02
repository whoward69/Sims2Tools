/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2025
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System;
using System.Windows.Forms;

namespace Sims2Tools
{
    public partial class ThumbnailWarningDialog : Form
    {
        public ThumbnailWarningDialog(string warning)
        {
            InitializeComponent();

            textThumbnailWarning.Text = warning;
        }

        private void OnConfigLoad(object sender, EventArgs e)
        {
            ckbMuteThumbnailWarnings.Visible = Sims2ToolsLib.AllAdvancedMode;
            ckbMuteThumbnailWarnings.Checked = Sims2ToolsLib.MuteThumbnailWarnings;
        }

        private void OnConfigOkClicked(object sender, EventArgs e)
        {
            Sims2ToolsLib.MuteThumbnailWarnings = ckbMuteThumbnailWarnings.Checked;

            this.Close();
        }
    }
}
