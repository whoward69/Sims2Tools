/*
 * SceneGraph Plus - a utility for repairing scene graphs
 *
 * William Howard - 2020-2025
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.Controls;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.CTSS;
using Sims2Tools.DBPF.Data;
using Sims2Tools.DBPF.OBJD;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph.CRES;
using Sims2Tools.DBPF.SceneGraph.GMDC;
using Sims2Tools.DBPF.SceneGraph.GMND;
using Sims2Tools.DBPF.SceneGraph.MMAT;
using Sims2Tools.DBPF.SceneGraph.SHPE;
using Sims2Tools.DBPF.STR;
using Sims2Tools.DbpfCache;
using Sims2Tools.Dialogs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SceneGraphPlus.Dialogs.Options
{
    public partial class ObjdDialog : Form
    {
        private SceneGraphPlusForm form;
        private CacheableDbpfFile package;
        private Objd objd;
        private Ctss ctss;
        private Cres cres;
        private Shpe shpe;
        private Gmnd gmnd;

        private string originalGuid;

        private string originalTitle = "";
        private string originalDesc = "";

        private string originalPrimarySubset = null;
        private string originalSecondarySubset = null;

        public ObjdDialog()
        {
            InitializeComponent();
        }

        public DialogResult ShowDialog(SceneGraphPlusForm form, Point location, CacheableDbpfFile package, Objd objd, Ctss ctss, Cres cres, Shpe shpe, Gmnd gmnd, Gmdc gmdc)
        {
            this.form = form;
            this.package = package;
            this.objd = objd;
            this.ctss = ctss;
            this.cres = cres;
            this.shpe = shpe;
            this.gmnd = gmnd;

            this.Location = new Point(location.X + 5, location.Y + 5);

            originalGuid = objd.Guid.ToString();
            textGUID.Text = originalGuid;

            textTitle.Enabled = textDesc.Enabled = false;
            if (ctss != null)
            {
                List<StrItem> defStrings = ctss.LanguageItems(MetaData.Languages.Default);
                if (defStrings != null)
                {
                    if (defStrings.Count > 0)
                    {
                        originalTitle = defStrings[0].Title;
                        textTitle.Text = originalTitle;
                        textTitle.Enabled = true;

                        if (defStrings.Count > 1)
                        {
                            originalDesc = defStrings[1].Title;
                            textDesc.Text = originalDesc;
                            textDesc.Enabled = true;
                        }
                    }
                }
            }

            grpRecolourable.Enabled = (gmdc != null);
            if (grpRecolourable.Enabled)
            {
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

                if (enabledSubsets.Count == 1)
                {
                    originalPrimarySubset = enabledSubsets[0];
                }
                else if (enabledSubsets.Count > 1)
                {
                    originalPrimarySubset = enabledSubsets[0];
                    originalSecondarySubset = enabledSubsets[1];
                }

                if (originalPrimarySubset != null)
                {
                    comboPrimarySubset.SelectedItem = originalPrimarySubset;
                }

                if (originalSecondarySubset != null)
                {
                    comboSecondarySubset.SelectedItem = originalSecondarySubset;
                }
            }

            grpNewMmat.Enabled = (gmnd != null);
            if (grpNewMmat.Enabled)
            {
                comboAddMmatSubset.Items.Clear();
                comboAddMmatSubset.Items.Add("");

                foreach (string subset in gmnd.GetDesignModeEnabledSubsets())
                {
                    comboAddMmatSubset.Items.Add(subset);
                }
            }

            btnDetailsChange.Enabled = false;
            btnSubsetsUpdate.Enabled = false;
            btnMmatCreate.Enabled = false;

            return base.ShowDialog();
        }

        private void OnGuidChanged(object sender, EventArgs e)
        {
            if (textGUID.Text.Length == 0) textGUID.Text = originalGuid;

            btnDetailsChange.Enabled = !originalGuid.Equals(textGUID.Text) || !originalTitle.Equals(textTitle.Text) || !originalDesc.Equals(textDesc.Text);
            grpRecolourable.Enabled = grpNewMmat.Enabled = !btnDetailsChange.Enabled;
        }

        private void OnRandomClicked(object sender, EventArgs e)
        {
            textGUID.Text = TypeGUID.RandomID.ToString();
        }

        private void OnTitleChanged(object sender, EventArgs e)
        {
            if (textTitle.Text.Length == 0) textTitle.Text = originalTitle;

            btnDetailsChange.Enabled = !originalGuid.Equals(textGUID.Text) || !originalTitle.Equals(textTitle.Text) || !originalDesc.Equals(textDesc.Text);
            grpRecolourable.Enabled = grpNewMmat.Enabled = !btnDetailsChange.Enabled;
        }

        private void OnDescChanged(object sender, EventArgs e)
        {
            if (textDesc.Text.Length == 0) textDesc.Text = originalDesc;

            btnDetailsChange.Enabled = !originalGuid.Equals(textGUID.Text) || !originalTitle.Equals(textTitle.Text) || !originalDesc.Equals(textDesc.Text);
            grpRecolourable.Enabled = grpNewMmat.Enabled = !btnDetailsChange.Enabled;
        }

        private void OnDetailsChangeClicked(object sender, EventArgs e)
        {
            if (!originalGuid.ToUpper().Equals(textGUID.Text.ToUpper()))
            {
                objd.SetGuid((TypeGUID)textGUID.Text);
            }

            if (ctss != null)
            {
                List<StrItem> defStrings = ctss.LanguageItems(MetaData.Languages.Default);

                if (textTitle.Enabled && !originalTitle.Equals(textTitle.Text))
                {
                    defStrings[0].Title = textTitle.Text;
                }

                if (textDesc.Enabled && !originalDesc.Equals(textDesc.Text))
                {
                    ctss.LanguageItems(MetaData.Languages.Default)[1].Title = textDesc.Text;
                }
            }
        }

        private void OnCreateMmatChanged(object sender, EventArgs e)
        {
            btnMmatCreate.Enabled = (comboAddMmatSubset.SelectedIndex > 0);
            grpDetails.Enabled = grpRecolourable.Enabled = !btnMmatCreate.Enabled;
        }

        private void OnAddMmatClicked(object sender, EventArgs e)
        {
            if (comboAddMmatSubset.SelectedIndex != 0)
            {
                string subsetMaterial = shpe.GetSubsetMaterial(comboAddMmatSubset.Text);

                if (subsetMaterial != null)
                {
                    uint nextInstance = 0x5000;

                    foreach (DBPFEntry entry in package.GetEntriesByType(Mmat.TYPE))
                    {
                        if (entry.InstanceID.AsUInt() >= nextInstance)
                        {
                            nextInstance = entry.InstanceID.AsUInt() + 1;
                        }
                    }

                    DBPFEntry mmatEntry = new DBPFEntry(Mmat.TYPE, objd.GroupID, (TypeInstanceID)nextInstance, DBPFData.RESOURCE_NULL);
                    Mmat mmat = new Mmat(mmatEntry, null);

                    mmat.GetOrAddItem("flags", MetaData.DataTypes.dtUInteger).UIntegerValue = 0x00000000;
                    mmat.GetOrAddItem("creator", MetaData.DataTypes.dtString).StringValue = "00000000-0000-0000-0000-000000000000";
                    mmat.GetOrAddItem("type", MetaData.DataTypes.dtString).StringValue = "modelMaterial";
                    mmat.GetOrAddItem("materialStateFlags", MetaData.DataTypes.dtUInteger).UIntegerValue = 0x00000000;
                    mmat.GetOrAddItem("objectStateIndex", MetaData.DataTypes.dtUInteger).UIntegerValue = 0xFFFFFFFF;
                    mmat.GetOrAddItem("family", MetaData.DataTypes.dtString).StringValue = Guid.NewGuid().ToString();
                    mmat.GetOrAddItem("defaultMaterial", MetaData.DataTypes.dtBoolean).BooleanValue = false;

                    mmat.GetOrAddItem("objectGUID", MetaData.DataTypes.dtUInteger).UIntegerValue = objd.Guid.AsUInt();
                    mmat.GetOrAddItem("modelName", MetaData.DataTypes.dtString).StringValue = cres.SgName;
                    mmat.GetOrAddItem("subsetName", MetaData.DataTypes.dtString).StringValue = comboAddMmatSubset.Text; ;
                    mmat.GetOrAddItem("name", MetaData.DataTypes.dtString).StringValue = subsetMaterial;

                    package.Commit(mmat);

                    form.AddResource(package, mmat, true);
                }
                else
                {
                    MsgBox.Show($"Cannot find specified subset in SHPE's 'Parts' list", "Create MMAT Error!");
                }
            }
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

            btnSubsetsUpdate.Enabled = enableButton;
            grpDetails.Enabled = grpNewMmat.Enabled = !btnSubsetsUpdate.Enabled;
        }

        private void OnUpdateSubsetsClicked(object sender, EventArgs e)
        {
            if (originalPrimarySubset != null) gmnd.RemoveDesignModeEnabled(originalPrimarySubset);

            if (comboPrimarySubset.SelectedItem != null)
            {
                if (comboPrimarySubset.SelectedIndex > 0)
                {
                    gmnd.AddDesignModeEnabled(comboPrimarySubset.SelectedItem.ToString());
                }
            }

            if (originalSecondarySubset != null) gmnd.RemoveDesignModeEnabled(originalSecondarySubset);

            if (comboSecondarySubset.SelectedItem != null)
            {
                if (comboSecondarySubset.SelectedIndex > 0)
                {
                    gmnd.AddDesignModeEnabled(comboSecondarySubset.SelectedItem.ToString());
                }
            }
        }
    }
}
