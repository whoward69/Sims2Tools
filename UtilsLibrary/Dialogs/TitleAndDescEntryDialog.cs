/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.Strings;
using System.Windows.Forms;

namespace Sims2Tools
{
    public partial class TitleAndDescEntryDialog : Form
    {
        public string Title
        {
            get => Sims2String.WinToSims(textTitle.Text);
            set => textTitle.Text = Sims2String.SimsToWin(value);
        }

        public string Description
        {
            get => Sims2String.WinToSims(textDescription.Text);
            set => textDescription.Text = Sims2String.SimsToWin(value);
        }

        public TitleAndDescEntryDialog(string title, string desc)
        {
            InitializeComponent();

            Title = title;
            Description = desc;
        }
    }
}
