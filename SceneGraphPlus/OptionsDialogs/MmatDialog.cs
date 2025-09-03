/*
 * SceneGraph Plus - a utility for repairing scene graphs
 *
 * William Howard - 2020-2025
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.Controls;
using Sims2Tools.DBPF.SceneGraph.MMAT;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SceneGraphPlus.Dialogs.Options
{
    public partial class MmatDialog : Form
    {
        private Mmat mmat;

        private bool originalDefMat;
        private string originalSubset;

        public MmatDialog()
        {
            InitializeComponent();
        }

        public DialogResult ShowDialog(Point location, Mmat mmat, List<string> subsets)
        {
            this.mmat = mmat;

            this.Location = new Point(location.X + 5, location.Y + 5);

            originalDefMat = mmat.DefaultMaterial;
            ckbDefaultMaterial.Checked = originalDefMat;
            btnDefMaterial.Enabled = false;

            originalSubset = mmat.SubsetName;
            grpSubset.Enabled = (subsets.Count > 0);

            if (grpSubset.Enabled)
            {
                comboSubset.Items.Clear();
                foreach (string subset in subsets)
                {
                    comboSubset.Items.Add(subset);
                }

                ControlHelper.SetDropDownWidth(comboSubset);

                comboSubset.SelectedItem = originalSubset;
                btnSubsetUpdate.Enabled = false;
            }

            return base.ShowDialog();
        }

        private void OnDefMaterialChangeClicked(object sender, EventArgs e)
        {
            if (ckbDefaultMaterial.Checked != originalDefMat)
            {
                mmat.GetItem("defaultMaterial").BooleanValue = ckbDefaultMaterial.Checked;
            }
        }

        private void OnDefMatCheckChange(object sender, EventArgs e)
        {
            btnDefMaterial.Enabled = (ckbDefaultMaterial.Checked != originalDefMat);
            grpSubset.Enabled = !btnDefMaterial.Enabled;
        }

        private void OnSubsetChanged(object sender, EventArgs e)
        {
            btnSubsetUpdate.Enabled = !originalSubset.Equals(comboSubset.SelectedItem);
            grpDefMaterial.Enabled = !btnSubsetUpdate.Enabled;
        }

        private void OnSubsetUpdateClicked(object sender, EventArgs e)
        {
            if (!originalSubset.Equals(comboSubset.SelectedItem))
            {
                mmat.GetItem("subsetName").StringValue = comboSubset.SelectedItem.ToString();
            }
        }
    }
}
