/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2024
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.Controls;
using Sims2Tools.DBPF.CPF;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph.GZPS;
using Sims2Tools.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Sims2Tools
{
    public partial class CreatorConfigDialog : Form
    {
        public CreatorConfigDialog()
        {
            InitializeComponent();
        }

        private void OnConfigLoad(object sender, EventArgs e)
        {
            textCreatorNickName.Text = Sims2ToolsLib.CreatorNickName;
            textCreatorGUID.Text = Sims2ToolsLib.CreatorGUID;
        }

        private void OnConfigOkClicked(object sender, EventArgs e)
        {
            Sims2ToolsLib.CreatorNickName = textCreatorNickName.Text;
            Sims2ToolsLib.CreatorGUID = textCreatorGUID.Text;

            this.Close();
        }

        private void OnRandomClicked(object sender, EventArgs e)
        {
            textCreatorGUID.Text = Guid.NewGuid().ToString();
        }

        private void OnFindClicked(object sender, EventArgs e)
        {
            HashSet<string> allCreatorGuids = new HashSet<string>();

            foreach (string savedSimPath in Directory.GetFiles($"{Sims2ToolsLib.Sims2HomePath}\\SavedSims", "*.package", SearchOption.AllDirectories))
            {
                using (DBPFFile savedSim = new DBPFFile(savedSimPath))
                {
                    foreach (DBPFEntry entry in savedSim.GetEntriesByType(Gzps.TYPE))
                    {
                        Gzps gzps = (Gzps)savedSim.GetResourceByEntry(entry);

                        CpfItem creator = gzps?.GetItem("creator");

                        if (creator != null)
                        {
                            string creatorGuid = creator.StringValue;

                            if (!creatorGuid.Equals("00000000-0000-0000-0000-000000000000"))
                            {
                                allCreatorGuids.Add(creatorGuid);
                            }
                        }
                    }

                    savedSim.Close();
                }
            }

            if (allCreatorGuids.Count == 0)
            {
                MsgBox.Show("No creator GUIDs found", "Find GUIDs", MessageBoxButtons.OK);
            }
            else if (allCreatorGuids.Count == 1)
            {
                IEnumerator<string> enumerator = allCreatorGuids.GetEnumerator();
                enumerator.MoveNext();
                textCreatorGUID.Text = enumerator.Current;
            }
            else
            {
                PickerDialog pickerDialog = new ListPickerDialog("Creator config GUIDs", "Select creator GUID:");

                foreach (string creatorGuid in allCreatorGuids)
                {
                    pickerDialog.AddItem(creatorGuid);
                }

                if (pickerDialog.ShowDialog() == DialogResult.OK)
                {
                    textCreatorGUID.Text = pickerDialog.SelectedItem as string;
                }
            }
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (Form.ModifierKeys == Keys.None && keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }

            return base.ProcessDialogKey(keyData);
        }
    }
}
