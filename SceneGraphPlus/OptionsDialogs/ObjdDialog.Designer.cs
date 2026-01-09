/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

namespace SceneGraphPlus.Dialogs.Options
{
    public partial class ObjdDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ObjdDialog));
            this.btnSubsetsUpdate = new System.Windows.Forms.Button();
            this.grpRecolourable = new System.Windows.Forms.GroupBox();
            this.comboPrimarySubset = new System.Windows.Forms.ComboBox();
            this.comboSecondarySubset = new System.Windows.Forms.ComboBox();
            this.lblSecondarySubset = new System.Windows.Forms.Label();
            this.lblPrimarySubset = new System.Windows.Forms.Label();
            this.textGUID = new System.Windows.Forms.TextBox();
            this.btnGuidRandom = new System.Windows.Forms.Button();
            this.btnMmatCreate = new System.Windows.Forms.Button();
            this.comboAddMmatSubset = new System.Windows.Forms.ComboBox();
            this.lblAddMmat = new System.Windows.Forms.Label();
            this.grpDetails = new System.Windows.Forms.GroupBox();
            this.lblDesc = new System.Windows.Forms.Label();
            this.textDesc = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.textTitle = new System.Windows.Forms.TextBox();
            this.lblGuid = new System.Windows.Forms.Label();
            this.btnDetailsChange = new System.Windows.Forms.Button();
            this.grpNewMmat = new System.Windows.Forms.GroupBox();
            this.btnMaterials = new System.Windows.Forms.Button();
            this.grpRecolourable.SuspendLayout();
            this.grpDetails.SuspendLayout();
            this.grpNewMmat.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSubsetsUpdate
            // 
            this.btnSubsetsUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSubsetsUpdate.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSubsetsUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSubsetsUpdate.Location = new System.Drawing.Point(323, 44);
            this.btnSubsetsUpdate.Name = "btnSubsetsUpdate";
            this.btnSubsetsUpdate.Size = new System.Drawing.Size(82, 26);
            this.btnSubsetsUpdate.TabIndex = 28;
            this.btnSubsetsUpdate.Text = "Update";
            this.btnSubsetsUpdate.UseVisualStyleBackColor = true;
            this.btnSubsetsUpdate.Click += new System.EventHandler(this.OnUpdateSubsetsClicked);
            // 
            // grpRecolourable
            // 
            this.grpRecolourable.Controls.Add(this.comboPrimarySubset);
            this.grpRecolourable.Controls.Add(this.comboSecondarySubset);
            this.grpRecolourable.Controls.Add(this.lblSecondarySubset);
            this.grpRecolourable.Controls.Add(this.lblPrimarySubset);
            this.grpRecolourable.Controls.Add(this.btnSubsetsUpdate);
            this.grpRecolourable.Location = new System.Drawing.Point(10, 110);
            this.grpRecolourable.Name = "grpRecolourable";
            this.grpRecolourable.Size = new System.Drawing.Size(412, 80);
            this.grpRecolourable.TabIndex = 29;
            this.grpRecolourable.TabStop = false;
            this.grpRecolourable.Text = "Recolours:";
            // 
            // comboPrimarySubset
            // 
            this.comboPrimarySubset.FormattingEnabled = true;
            this.comboPrimarySubset.Location = new System.Drawing.Point(76, 15);
            this.comboPrimarySubset.Name = "comboPrimarySubset";
            this.comboPrimarySubset.Size = new System.Drawing.Size(241, 23);
            this.comboPrimarySubset.TabIndex = 3;
            this.comboPrimarySubset.SelectedIndexChanged += new System.EventHandler(this.OnSubsetChanged);
            // 
            // comboSecondarySubset
            // 
            this.comboSecondarySubset.FormattingEnabled = true;
            this.comboSecondarySubset.Location = new System.Drawing.Point(76, 46);
            this.comboSecondarySubset.Name = "comboSecondarySubset";
            this.comboSecondarySubset.Size = new System.Drawing.Size(241, 23);
            this.comboSecondarySubset.TabIndex = 2;
            this.comboSecondarySubset.SelectedIndexChanged += new System.EventHandler(this.OnSubsetChanged);
            // 
            // lblSecondarySubset
            // 
            this.lblSecondarySubset.AutoSize = true;
            this.lblSecondarySubset.Location = new System.Drawing.Point(12, 49);
            this.lblSecondarySubset.Name = "lblSecondarySubset";
            this.lblSecondarySubset.Size = new System.Drawing.Size(58, 15);
            this.lblSecondarySubset.TabIndex = 1;
            this.lblSecondarySubset.Text = "Subset 2:";
            // 
            // lblPrimarySubset
            // 
            this.lblPrimarySubset.AutoSize = true;
            this.lblPrimarySubset.Location = new System.Drawing.Point(12, 18);
            this.lblPrimarySubset.Name = "lblPrimarySubset";
            this.lblPrimarySubset.Size = new System.Drawing.Size(58, 15);
            this.lblPrimarySubset.TabIndex = 0;
            this.lblPrimarySubset.Text = "Subset 1:";
            // 
            // textGUID
            // 
            this.textGUID.Location = new System.Drawing.Point(76, 17);
            this.textGUID.Name = "textGUID";
            this.textGUID.Size = new System.Drawing.Size(159, 21);
            this.textGUID.TabIndex = 31;
            this.textGUID.TextChanged += new System.EventHandler(this.OnGuidChanged);
            // 
            // btnGuidRandom
            // 
            this.btnGuidRandom.Location = new System.Drawing.Point(241, 14);
            this.btnGuidRandom.Name = "btnGuidRandom";
            this.btnGuidRandom.Size = new System.Drawing.Size(76, 26);
            this.btnGuidRandom.TabIndex = 32;
            this.btnGuidRandom.Text = "Random";
            this.btnGuidRandom.UseVisualStyleBackColor = true;
            this.btnGuidRandom.Click += new System.EventHandler(this.OnRandomClicked);
            // 
            // btnMmatCreate
            // 
            this.btnMmatCreate.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnMmatCreate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMmatCreate.Location = new System.Drawing.Point(323, 12);
            this.btnMmatCreate.Name = "btnMmatCreate";
            this.btnMmatCreate.Size = new System.Drawing.Size(82, 26);
            this.btnMmatCreate.TabIndex = 33;
            this.btnMmatCreate.Text = "Create";
            this.btnMmatCreate.UseVisualStyleBackColor = true;
            this.btnMmatCreate.Click += new System.EventHandler(this.OnAddMmatClicked);
            // 
            // comboAddMmatSubset
            // 
            this.comboAddMmatSubset.FormattingEnabled = true;
            this.comboAddMmatSubset.Location = new System.Drawing.Point(76, 15);
            this.comboAddMmatSubset.Name = "comboAddMmatSubset";
            this.comboAddMmatSubset.Size = new System.Drawing.Size(241, 23);
            this.comboAddMmatSubset.TabIndex = 34;
            this.comboAddMmatSubset.SelectedIndexChanged += new System.EventHandler(this.OnCreateMmatChanged);
            // 
            // lblAddMmat
            // 
            this.lblAddMmat.AutoSize = true;
            this.lblAddMmat.Location = new System.Drawing.Point(22, 18);
            this.lblAddMmat.Name = "lblAddMmat";
            this.lblAddMmat.Size = new System.Drawing.Size(48, 15);
            this.lblAddMmat.TabIndex = 35;
            this.lblAddMmat.Text = "Subset:";
            // 
            // grpDetails
            // 
            this.grpDetails.Controls.Add(this.lblDesc);
            this.grpDetails.Controls.Add(this.textDesc);
            this.grpDetails.Controls.Add(this.lblTitle);
            this.grpDetails.Controls.Add(this.textTitle);
            this.grpDetails.Controls.Add(this.lblGuid);
            this.grpDetails.Controls.Add(this.btnDetailsChange);
            this.grpDetails.Controls.Add(this.textGUID);
            this.grpDetails.Controls.Add(this.btnGuidRandom);
            this.grpDetails.Location = new System.Drawing.Point(10, 4);
            this.grpDetails.Name = "grpDetails";
            this.grpDetails.Size = new System.Drawing.Size(412, 100);
            this.grpDetails.TabIndex = 36;
            this.grpDetails.TabStop = false;
            this.grpDetails.Text = "Details:";
            // 
            // lblDesc
            // 
            this.lblDesc.AutoSize = true;
            this.lblDesc.Location = new System.Drawing.Point(32, 74);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(38, 15);
            this.lblDesc.TabIndex = 36;
            this.lblDesc.Text = "Desc:";
            // 
            // textDesc
            // 
            this.textDesc.Location = new System.Drawing.Point(76, 71);
            this.textDesc.Name = "textDesc";
            this.textDesc.Size = new System.Drawing.Size(241, 21);
            this.textDesc.TabIndex = 35;
            this.textDesc.TextChanged += new System.EventHandler(this.OnDescChanged);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(37, 47);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(33, 15);
            this.lblTitle.TabIndex = 34;
            this.lblTitle.Text = "Title:";
            // 
            // textTitle
            // 
            this.textTitle.Location = new System.Drawing.Point(76, 44);
            this.textTitle.Name = "textTitle";
            this.textTitle.Size = new System.Drawing.Size(241, 21);
            this.textTitle.TabIndex = 33;
            this.textTitle.TextChanged += new System.EventHandler(this.OnTitleChanged);
            // 
            // lblGuid
            // 
            this.lblGuid.AutoSize = true;
            this.lblGuid.Location = new System.Drawing.Point(30, 20);
            this.lblGuid.Name = "lblGuid";
            this.lblGuid.Size = new System.Drawing.Size(40, 15);
            this.lblGuid.TabIndex = 29;
            this.lblGuid.Text = "GUID:";
            // 
            // btnDetailsChange
            // 
            this.btnDetailsChange.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDetailsChange.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnDetailsChange.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDetailsChange.Location = new System.Drawing.Point(323, 66);
            this.btnDetailsChange.Name = "btnDetailsChange";
            this.btnDetailsChange.Size = new System.Drawing.Size(82, 26);
            this.btnDetailsChange.TabIndex = 29;
            this.btnDetailsChange.Text = "Change";
            this.btnDetailsChange.UseVisualStyleBackColor = true;
            this.btnDetailsChange.Click += new System.EventHandler(this.OnDetailsChangeClicked);
            // 
            // grpNewMmat
            // 
            this.grpNewMmat.Controls.Add(this.lblAddMmat);
            this.grpNewMmat.Controls.Add(this.comboAddMmatSubset);
            this.grpNewMmat.Controls.Add(this.btnMmatCreate);
            this.grpNewMmat.Location = new System.Drawing.Point(10, 195);
            this.grpNewMmat.Name = "grpNewMmat";
            this.grpNewMmat.Size = new System.Drawing.Size(412, 47);
            this.grpNewMmat.TabIndex = 37;
            this.grpNewMmat.TabStop = false;
            this.grpNewMmat.Text = "New MMAT:";
            // 
            // btnMaterials
            // 
            this.btnMaterials.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMaterials.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnMaterials.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMaterials.Location = new System.Drawing.Point(302, 248);
            this.btnMaterials.Name = "btnMaterials";
            this.btnMaterials.Size = new System.Drawing.Size(113, 26);
            this.btnMaterials.TabIndex = 36;
            this.btnMaterials.Text = "Add Materials";
            this.btnMaterials.UseVisualStyleBackColor = true;
            this.btnMaterials.Click += new System.EventHandler(this.OnAddMaterialsClicked);
            // 
            // ObjdDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(434, 281);
            this.Controls.Add(this.btnMaterials);
            this.Controls.Add(this.grpNewMmat);
            this.Controls.Add(this.grpDetails);
            this.Controls.Add(this.grpRecolourable);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ObjdDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "OBJD Options";
            this.grpRecolourable.ResumeLayout(false);
            this.grpRecolourable.PerformLayout();
            this.grpDetails.ResumeLayout(false);
            this.grpDetails.PerformLayout();
            this.grpNewMmat.ResumeLayout(false);
            this.grpNewMmat.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnSubsetsUpdate;
        private System.Windows.Forms.GroupBox grpRecolourable;
        private System.Windows.Forms.Label lblSecondarySubset;
        private System.Windows.Forms.ComboBox comboPrimarySubset;
        private System.Windows.Forms.ComboBox comboSecondarySubset;
        private System.Windows.Forms.TextBox textGUID;
        private System.Windows.Forms.Button btnGuidRandom;
        private System.Windows.Forms.Button btnMmatCreate;
        private System.Windows.Forms.ComboBox comboAddMmatSubset;
        private System.Windows.Forms.Label lblAddMmat;
        private System.Windows.Forms.GroupBox grpDetails;
        private System.Windows.Forms.Label lblPrimarySubset;
        private System.Windows.Forms.Button btnDetailsChange;
        private System.Windows.Forms.GroupBox grpNewMmat;
        private System.Windows.Forms.Label lblDesc;
        private System.Windows.Forms.TextBox textDesc;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox textTitle;
        private System.Windows.Forms.Label lblGuid;
        private System.Windows.Forms.Button btnMaterials;
    }
}
