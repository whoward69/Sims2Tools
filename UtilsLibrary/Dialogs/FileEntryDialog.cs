/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System.Linq;
using System.Windows.Forms;

namespace Sims2Tools.Dialogs
{
    public partial class FileEntryDialog : Form
    {
        private readonly string defExtn;

        public string FileEntry
        {
            get
            {
                string file = fileEntry.Text;

                if (!file.EndsWith(defExtn))
                {
                    file += defExtn;
                }

                return file;
            }
        }


        public FileEntryDialog(string title, string prompt, string file, string defExtn)
        {
            InitializeComponent();

            this.Text = title;
            lblPrompt.Text = prompt;
            fileEntry.Text = file;
            this.defExtn = defExtn;

            int pos = file.LastIndexOf(".");
            if (pos > 0)
            {
                fileEntry.SelectionStart = 0;
                fileEntry.SelectionLength = pos;
            }
        }
    }
}
