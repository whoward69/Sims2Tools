/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

namespace SceneGraphPlus.Dialogs.Options
{
    public partial class MmatDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MmatDialog));
            this.ckbDefaultMaterial = new System.Windows.Forms.CheckBox();
            this.btnDefMaterial = new System.Windows.Forms.Button();
            this.grpDefMaterial = new System.Windows.Forms.GroupBox();
            this.grpSubset = new System.Windows.Forms.GroupBox();
            this.lblSubset = new System.Windows.Forms.Label();
            this.comboSubset = new System.Windows.Forms.ComboBox();
            this.btnSubsetUpdate = new System.Windows.Forms.Button();
            this.grpDefMaterial.SuspendLayout();
            this.grpSubset.SuspendLayout();
            this.SuspendLayout();
            // 
            // ckbDefaultMaterial
            // 
            this.ckbDefaultMaterial.AutoSize = true;
            this.ckbDefaultMaterial.Location = new System.Drawing.Point(76, 21);
            this.ckbDefaultMaterial.Name = "ckbDefaultMaterial";
            this.ckbDefaultMaterial.Size = new System.Drawing.Size(15, 14);
            this.ckbDefaultMaterial.TabIndex = 7;
            this.ckbDefaultMaterial.UseVisualStyleBackColor = true;
            this.ckbDefaultMaterial.CheckedChanged += new System.EventHandler(this.OnDefMatCheckChange);
            // 
            // btnDefMaterial
            // 
            this.btnDefMaterial.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDefMaterial.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnDefMaterial.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDefMaterial.Location = new System.Drawing.Point(323, 14);
            this.btnDefMaterial.Name = "btnDefMaterial";
            this.btnDefMaterial.Size = new System.Drawing.Size(82, 26);
            this.btnDefMaterial.TabIndex = 28;
            this.btnDefMaterial.Text = "Change";
            this.btnDefMaterial.UseVisualStyleBackColor = true;
            this.btnDefMaterial.Click += new System.EventHandler(this.OnDefMaterialChangeClicked);
            // 
            // grpDefMaterial
            // 
            this.grpDefMaterial.Controls.Add(this.ckbDefaultMaterial);
            this.grpDefMaterial.Controls.Add(this.btnDefMaterial);
            this.grpDefMaterial.Location = new System.Drawing.Point(10, 6);
            this.grpDefMaterial.Name = "grpDefMaterial";
            this.grpDefMaterial.Size = new System.Drawing.Size(412, 50);
            this.grpDefMaterial.TabIndex = 29;
            this.grpDefMaterial.TabStop = false;
            this.grpDefMaterial.Text = "Def Material:";
            // 
            // grpSubset
            // 
            this.grpSubset.Controls.Add(this.lblSubset);
            this.grpSubset.Controls.Add(this.comboSubset);
            this.grpSubset.Controls.Add(this.btnSubsetUpdate);
            this.grpSubset.Location = new System.Drawing.Point(10, 63);
            this.grpSubset.Name = "grpSubset";
            this.grpSubset.Size = new System.Drawing.Size(412, 50);
            this.grpSubset.TabIndex = 38;
            this.grpSubset.TabStop = false;
            this.grpSubset.Text = "Subset:";
            // 
            // lblSubset
            // 
            this.lblSubset.AutoSize = true;
            this.lblSubset.Location = new System.Drawing.Point(22, 17);
            this.lblSubset.Name = "lblSubset";
            this.lblSubset.Size = new System.Drawing.Size(0, 15);
            this.lblSubset.TabIndex = 35;
            // 
            // comboSubset
            // 
            this.comboSubset.FormattingEnabled = true;
            this.comboSubset.Location = new System.Drawing.Point(76, 17);
            this.comboSubset.Name = "comboSubset";
            this.comboSubset.Size = new System.Drawing.Size(241, 23);
            this.comboSubset.TabIndex = 34;
            this.comboSubset.SelectedIndexChanged += new System.EventHandler(this.OnSubsetChanged);
            // 
            // btnSubsetUpdate
            // 
            this.btnSubsetUpdate.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSubsetUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSubsetUpdate.Location = new System.Drawing.Point(323, 14);
            this.btnSubsetUpdate.Name = "btnSubsetUpdate";
            this.btnSubsetUpdate.Size = new System.Drawing.Size(82, 26);
            this.btnSubsetUpdate.TabIndex = 33;
            this.btnSubsetUpdate.Text = "Update";
            this.btnSubsetUpdate.UseVisualStyleBackColor = true;
            this.btnSubsetUpdate.Click += new System.EventHandler(this.OnSubsetUpdateClicked);
            // 
            // MmatDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(434, 121);
            this.Controls.Add(this.grpSubset);
            this.Controls.Add(this.grpDefMaterial);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MmatDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "MMAT Options";
            this.grpDefMaterial.ResumeLayout(false);
            this.grpDefMaterial.PerformLayout();
            this.grpSubset.ResumeLayout(false);
            this.grpSubset.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.CheckBox ckbDefaultMaterial;
        private System.Windows.Forms.Button btnDefMaterial;
        private System.Windows.Forms.GroupBox grpDefMaterial;
        private System.Windows.Forms.GroupBox grpSubset;
        private System.Windows.Forms.Label lblSubset;
        private System.Windows.Forms.ComboBox comboSubset;
        private System.Windows.Forms.Button btnSubsetUpdate;
    }
}
