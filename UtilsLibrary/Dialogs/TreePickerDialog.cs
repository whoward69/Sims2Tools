/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2024
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.Controls;
using System.Windows.Forms;

namespace Sims2Tools
{
    public partial class TreePickerDialog : PickerDialog
    {
        public override object SelectedItem => treePicker.SelectedNode.Tag;

        public TreePickerDialog(string title, string prompt)
        {
            InitializeComponent();

            this.Text = title;
            lblPrompt.Text = prompt;
        }

        public override void AddItem(object item)
        {
            string text = item.ToString();

            int index = 0;
            while (index < text.Length && !char.IsLetterOrDigit(text[index]))
            {
                ++index;
            }

            if (index >= text.Length) index = 0;

            string prefix = text.Substring(index, 1).ToUpper();

            TreeNode prefixNode = null;

            foreach (TreeNode node in treePicker.Nodes)
            {
                if (node.Text.Equals(prefix))
                {
                    prefixNode = node;
                    break;
                }
            }

            if (prefixNode == null)
            {
                prefixNode = new TreeNode(prefix);

                treePicker.Nodes.Add(prefixNode);
            }

            TreeNode itemNode = new TreeNode(text)
            {
                Tag = item
            };

            prefixNode.Nodes.Add(itemNode);
        }

        private void OnLoad(object sender, System.EventArgs e)
        {
            treePicker.Sort();

            treePicker.SelectedNode = treePicker.Nodes[0].Nodes[0];
            treePicker.Focus();
        }
    }
}
