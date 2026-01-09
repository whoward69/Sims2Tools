/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.Controls;

namespace Sims2Tools
{
    public partial class ListPickerDialog : PickerDialog
    {
        public override object SelectedItem => comboPicker.SelectedItem;

        public ListPickerDialog(string title, string prompt)
        {
            InitializeComponent();

            this.Text = title;
            lblPrompt.Text = prompt;
        }

        public override void AddItem(object item)
        {
            comboPicker.Items.Add(item);

            if (comboPicker.Items.Count == 1)
            {
                comboPicker.SelectedIndex = 0;
            }
        }
    }
}
