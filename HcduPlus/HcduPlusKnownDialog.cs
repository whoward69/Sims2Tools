/*
 * HCDU Plus - a utility for checking The Sims 2 package files for conflicts
 *           - see http://www.picknmixmods.com/Sims2/Notes/HcduPlus/HcduPlus.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using HcduPlus.Conflict;
using System;
using System.Drawing;
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

            row.ErrorText = string.Empty;
            btnKnownOk.Enabled = true;

            if (string.IsNullOrEmpty(row.Cells[0].Value as string) && string.IsNullOrEmpty(row.Cells[0].Value as string)) return;

            try
            {
                new Regex(row.Cells[0].Value as string);

                try
                {
                    new Regex(row.Cells[1].Value as string);

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

        private void OnResetClicked(object sender, EventArgs e)
        {
            data.ResetRegexs();
        }

        private DataGridViewCellEventArgs mouseLocation = null;
        DataGridViewRow highlightRow = null;

        private void OnCellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            mouseLocation = e;
        }

        private void OnConflictMenuOpening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (mouseLocation == null || mouseLocation.RowIndex == -1 || mouseLocation.RowIndex == (gridKnownConflicts.RowCount - 1))
            {
                e.Cancel = true;
                return;
            }

            if (mouseLocation.RowIndex != gridKnownConflicts.SelectedRows[0].Index)
            {
                highlightRow = gridKnownConflicts.Rows[mouseLocation.RowIndex];
                highlightRow.DefaultCellStyle.BackColor = HcduPlusForm.colourAddKnownHighlight;
            }
            else
            {
                highlightRow = null;
            }

            menuItemPaste.Enabled = Clipboard.ContainsText(TextDataFormat.Text);
        }

        private void OnConflictMenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            if (highlightRow != null)
            {
                highlightRow.DefaultCellStyle.BackColor = Color.Empty;
            }
        }

        private void OnRemoveKnownConflictClicked(object sender, EventArgs e)
        {
            if (mouseLocation.RowIndex >= 0)
            {
                gridKnownConflicts.Rows.RemoveAt(mouseLocation.RowIndex);
            }
        }

        private void OnPasteKnownConflictClicked(object sender, EventArgs e)
        {
            data.Paste();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.V && e.Control)
            {
                data.Paste();
            }
        }
    }
}
