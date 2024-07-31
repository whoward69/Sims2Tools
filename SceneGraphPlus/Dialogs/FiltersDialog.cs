/*
 * SceneGraph Plus - a utility for repairing scene graphs
 *
 * William Howard - 2020-2024
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using SceneGraphPlus.Shapes;
using SceneGraphPlus.Surface;
using Sims2Tools.Utils.Persistence;
using System;
using System.Windows.Forms;

namespace SceneGraphPlus.Dialogs
{
    public partial class FiltersDialog : Form
    {
        private DrawingSurface surface = null;
        private BlockFilters filters;

        public FiltersDialog()
        {
            InitializeComponent();
        }

        private void OnLoad(object sender, EventArgs e)
        {
            RegistryTools.LoadPopupSettings(SceneGraphPlusApp.RegistryKey, this);
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            RegistryTools.SavePopupSettings(SceneGraphPlusApp.RegistryKey, this);
        }

        public void Show(BlockFilters filters, DrawingSurface surface)
        {
            this.surface = surface;
            this.filters = filters;

            ckbGenderFemale.Checked = filters.Female;
            ckbGenderMale.Checked = filters.Male;
            ckbGenderUnisex.Checked = filters.Unisex;

            ckbAgeBabies.Checked = filters.Babies;
            ckbAgeToddlers.Checked = filters.Toddlers;
            ckbAgeChildren.Checked = filters.Children;
            ckbAgeTeens.Checked = filters.Teens;
            ckbAgeYoungAdults.Checked = filters.YoungAdults;
            ckbAgeAdults.Checked = filters.Adults;
            ckbAgeElders.Checked = filters.Elders;

            textSubsets.Text = filters.Subsets;

            btnApply.Enabled = (surface != null);

            base.Show();
        }

        private void OnOK(object sender, EventArgs e)
        {
            OnApply(sender, e);

            surface.CloseFilters();
        }

        private void OnApply(object sender, EventArgs e)
        {
            filters.Female = ckbGenderFemale.Checked;
            filters.Male = ckbGenderMale.Checked;
            filters.Unisex = ckbGenderUnisex.Checked;

            filters.Babies = ckbAgeBabies.Checked;
            filters.Toddlers = ckbAgeToddlers.Checked;
            filters.Children = ckbAgeChildren.Checked;
            filters.Teens = ckbAgeTeens.Checked;
            filters.YoungAdults = ckbAgeYoungAdults.Checked;
            filters.Adults = ckbAgeAdults.Checked;
            filters.Elders = ckbAgeElders.Checked;

            filters.Subsets = textSubsets.Text;

            surface.ApplyFilters(filters);
            surface.Invalidate();
        }

        private void OnRealign(object sender, EventArgs e)
        {
            surface.RealignAll();
        }

        private void OnGenderClicked(object sender, EventArgs e)
        {
            if (Form.ModifierKeys == Keys.Control)
            {
                ckbGenderFemale.Checked = ckbGenderMale.Checked = ckbGenderUnisex.Checked = false;

                (sender as CheckBox).Checked = true;
            }
        }

        private void OnAgeClicked(object sender, EventArgs e)
        {
            if (Form.ModifierKeys == Keys.Control)
            {
                ckbAgeBabies.Checked = ckbAgeToddlers.Checked = ckbAgeChildren.Checked = ckbAgeTeens.Checked = ckbAgeYoungAdults.Checked = ckbAgeAdults.Checked = ckbAgeElders.Checked = false;

                (sender as CheckBox).Checked = true;
            }
        }

        private void OnAllClicked(object sender, EventArgs e)
        {
            ckbGenderFemale.Checked = ckbGenderMale.Checked = ckbGenderUnisex.Checked = true;
            ckbAgeBabies.Checked = ckbAgeToddlers.Checked = ckbAgeChildren.Checked = ckbAgeTeens.Checked = ckbAgeYoungAdults.Checked = ckbAgeAdults.Checked = ckbAgeElders.Checked = true;
        }
    }
}
