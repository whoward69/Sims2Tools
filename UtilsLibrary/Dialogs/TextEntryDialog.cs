/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2025
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System.Windows.Forms;

namespace Sims2Tools
{
    public partial class TextEntryDialog : Form
    {
        public string TextEntry => textEntry.Text;

        public TextEntryDialog(string title, string prompt, string text)
        {
            InitializeComponent();

            this.Text = title;
            lblPrompt.Text = prompt;
            textEntry.Text = text;
        }
    }
}
