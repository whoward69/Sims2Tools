/*
 * HCDU Plus - a utility for checking The Sims 2 package files for conflicts
 *           - see http://www.picknmixmods.com/Sims2/Notes/HcduPlus/HcduPlus.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2021
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace HcduPlus
{
    public partial class HcduPlusKnownDialog : Form
    {
        private readonly KnownConflicts data;

        public HcduPlusKnownDialog()
        {
            InitializeComponent();
        }

        public HcduPlusKnownDialog(KnownConflicts data) : this()
        {
            gridKnownConflicts.DataSource = this.data = data;
        }

        private void OnRowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridViewRow row = gridKnownConflicts.Rows[e.RowIndex];

            row.ErrorText = String.Empty;
            btnKnownOk.Enabled = true;

            if (String.IsNullOrEmpty(row.Cells[0].Value as String) && String.IsNullOrEmpty(row.Cells[0].Value as String)) return;

            try
            {
                new Regex(row.Cells[0].Value as String);

                try
                {
                    new Regex(row.Cells[1].Value as String);

                    return;
                }
                catch (Exception)
                {
                    row.ErrorText = $"'{gridKnownConflicts.Columns[1].HeaderText}' is not a valid regular expression";
                }
            }
            catch (Exception)
            {
                row.ErrorText = $"'{gridKnownConflicts.Columns[0].HeaderText}' is not a valid regular expression";
            }

            e.Cancel = true;
            btnKnownOk.Enabled = false;
        }

        private void OnOkClicked(object sender, EventArgs e)
        {
            data.CommitEdits();
        }
    }
}
