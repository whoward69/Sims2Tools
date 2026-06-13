/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System.Windows.Forms;

namespace Sims2Tools
{
    public partial class TextAndTextEntryDialog : Form
    {
        public string TextEntry1 => textEntry1.Text;
        public string TextEntry2 => textEntry2.Text;

        public TextAndTextEntryDialog(string title, string prompt1, string text1, string prompt2, string text2)
        {
            InitializeComponent();

            this.Text = title;
            lblPrompt1.Text = prompt1;
            textEntry1.Text = text1;
            lblPrompt2.Text = prompt2;
            textEntry2.Text = text2;
        }
    }
}
