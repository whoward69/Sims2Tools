/*
 * SceneGraph Plus - a utility for repairing scene graphs
 *
 * William Howard - 2020-2025
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.Controls;
using Sims2Tools.DBPF.SceneGraph.GMDC;
using Sims2Tools.DBPF.SceneGraph.GMND;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SceneGraphPlus.Dialogs.Options
{
    public partial class GmndDialog : Form
    {
        private Gmnd gmnd;

        private string originalPrimarySubset = null;
        private string originalSecondarySubset = null;

        public GmndDialog()
        {
            InitializeComponent();
        }

        public DialogResult ShowDialog(Point location, Gmnd gmnd, Gmdc gmdc)
        {
            this.gmnd = gmnd;

            this.Location = new Point(location.X + 5, location.Y + 5);

            if (gmdc != null)
            {
                grpRecolourable.Visible = true;

                comboPrimarySubset.Items.Clear();
                comboPrimarySubset.Items.Add("");
                comboSecondarySubset.Items.Clear();
                comboSecondarySubset.Items.Add("");

                foreach (string subset in gmdc.Subsets)
                {
                    comboPrimarySubset.Items.Add(subset);
                    comboSecondarySubset.Items.Add(subset);
                }

                ControlHelper.SetDropDownWidth(comboPrimarySubset);
                ControlHelper.SetDropDownWidth(comboSecondarySubset);

                List<string> enabledSubsets = gmnd.GetDesignModeEnabledSubsets();

                if (enabledSubsets.Count == 0)
                {
                }
                else
                {
                    originalPrimarySubset = enabledSubsets[0];
                    comboPrimarySubset.SelectedItem = originalPrimarySubset;

                    if (enabledSubsets.Count != 1)
                    {
                        originalSecondarySubset = enabledSubsets[1];
                        comboSecondarySubset.SelectedItem = originalSecondarySubset;
                    }
                }
            }
            else
            {
                grpRecolourable.Visible = false;
            }

            btnChangeSubsets.Enabled = false;

            return base.ShowDialog();
        }

        private void OnSubsetChanged(object sender, EventArgs e)
        {
            bool enableButton = false;

            if (comboPrimarySubset.SelectedIndex == -1)
            {
                if (originalPrimarySubset != null) enableButton = true;
            }
            else
            {
                if (originalPrimarySubset == null)
                {
                    if (comboPrimarySubset.SelectedIndex > 0) enableButton = true;
                }
                else
                {
                    if (!comboPrimarySubset.SelectedItem.Equals(originalPrimarySubset)) enableButton = true;
                }
            }

            if (comboSecondarySubset.SelectedIndex == -1)
            {
                if (originalSecondarySubset != null) enableButton = true;
            }
            else
            {
                if (originalSecondarySubset == null)
                {
                    if (comboSecondarySubset.SelectedIndex > 0) enableButton = true;
                }
                else
                {
                    if (!comboSecondarySubset.SelectedItem.Equals(originalSecondarySubset)) enableButton = true;
                }
            }

            btnChangeSubsets.Enabled = enableButton;
        }

        private void OnSubsetUpdate(object sender, EventArgs e)
        {
            if (originalPrimarySubset != null) gmnd.RemoveDesignModeEnabledSubset(originalPrimarySubset);

            if (comboPrimarySubset.SelectedItem != null)
            {
                if (comboPrimarySubset.SelectedIndex > 0)
                {
                    gmnd.AddDesignModeEnabledSubset(comboPrimarySubset.SelectedItem.ToString());
                }
            }

            if (originalSecondarySubset != null) gmnd.RemoveDesignModeEnabledSubset(originalSecondarySubset);

            if (comboSecondarySubset.SelectedItem != null)
            {
                if (comboSecondarySubset.SelectedIndex > 0)
                {
                    gmnd.AddDesignModeEnabledSubset(comboSecondarySubset.SelectedItem.ToString());
                }
            }
        }
    }
}
