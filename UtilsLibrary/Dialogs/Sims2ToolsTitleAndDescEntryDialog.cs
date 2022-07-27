/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2022
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System.Windows.Forms;

namespace Sims2Tools
{
    public partial class Sims2ToolsTitleAndDescEntryDialog : Form
    {
        public string Title => textTitle.Text;
        public string Description => textDescription.Text;

        public Sims2ToolsTitleAndDescEntryDialog(string title, string desc)
        {
            InitializeComponent();

            textTitle.Text = title;
            textDescription.Text = desc;
        }
    }
}
