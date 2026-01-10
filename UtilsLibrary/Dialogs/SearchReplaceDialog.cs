/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Sims2Tools
{
    public partial class SearchReplaceDialog : Form
    {
        public string TextSearch => textSearch.Text;
        public string TextReplace => textReplace.Text;

        public RegexOptions RegexOptions => ckbIgnoreCase.Checked ? RegexOptions.IgnoreCase : RegexOptions.None;

        public SearchReplaceDialog(string title, string textSearch, string textReplace, RegexOptions regexOptions)
        {
            InitializeComponent();

            this.Text = title;

            this.textSearch.Text = textSearch;
            this.textReplace.Text = textReplace;

            ckbIgnoreCase.Checked = ((regexOptions & RegexOptions.IgnoreCase) == RegexOptions.IgnoreCase);
        }
    }
}
