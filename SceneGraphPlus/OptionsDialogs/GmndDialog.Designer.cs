/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2025
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

namespace SceneGraphPlus.Dialogs.Options
{
    public partial class GmndDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GmndDialog));
            this.btnChangeSubsets = new System.Windows.Forms.Button();
            this.grpRecolourable = new System.Windows.Forms.GroupBox();
            this.comboPrimarySubset = new System.Windows.Forms.ComboBox();
            this.comboSecondarySubset = new System.Windows.Forms.ComboBox();
            this.lblSecondarySubset = new System.Windows.Forms.Label();
            this.lblPrimarySubset = new System.Windows.Forms.Label();
            this.grpRecolourable.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnChangeSubsets
            // 
            this.btnChangeSubsets.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChangeSubsets.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnChangeSubsets.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChangeSubsets.Location = new System.Drawing.Point(323, 49);
            this.btnChangeSubsets.Name = "btnChangeSubsets";
            this.btnChangeSubsets.Size = new System.Drawing.Size(82, 26);
            this.btnChangeSubsets.TabIndex = 28;
            this.btnChangeSubsets.Text = "Update";
            this.btnChangeSubsets.UseVisualStyleBackColor = true;
            this.btnChangeSubsets.Click += new System.EventHandler(this.OnSubsetUpdate);
            // 
            // grpRecolourable
            // 
            this.grpRecolourable.Controls.Add(this.comboPrimarySubset);
            this.grpRecolourable.Controls.Add(this.btnChangeSubsets);
            this.grpRecolourable.Controls.Add(this.comboSecondarySubset);
            this.grpRecolourable.Controls.Add(this.lblSecondarySubset);
            this.grpRecolourable.Controls.Add(this.lblPrimarySubset);
            this.grpRecolourable.Location = new System.Drawing.Point(10, 4);
            this.grpRecolourable.Name = "grpRecolourable";
            this.grpRecolourable.Size = new System.Drawing.Size(412, 85);
            this.grpRecolourable.TabIndex = 29;
            this.grpRecolourable.TabStop = false;
            this.grpRecolourable.Text = "Recolours:";
            // 
            // comboPrimarySubset
            // 
            this.comboPrimarySubset.FormattingEnabled = true;
            this.comboPrimarySubset.Location = new System.Drawing.Point(76, 17);
            this.comboPrimarySubset.Name = "comboPrimarySubset";
            this.comboPrimarySubset.Size = new System.Drawing.Size(241, 23);
            this.comboPrimarySubset.TabIndex = 3;
            this.comboPrimarySubset.SelectedIndexChanged += new System.EventHandler(this.OnSubsetChanged);
            // 
            // comboSecondarySubset
            // 
            this.comboSecondarySubset.FormattingEnabled = true;
            this.comboSecondarySubset.Location = new System.Drawing.Point(76, 51);
            this.comboSecondarySubset.Name = "comboSecondarySubset";
            this.comboSecondarySubset.Size = new System.Drawing.Size(241, 23);
            this.comboSecondarySubset.TabIndex = 2;
            this.comboSecondarySubset.SelectedIndexChanged += new System.EventHandler(this.OnSubsetChanged);
            // 
            // lblSecondarySubset
            // 
            this.lblSecondarySubset.AutoSize = true;
            this.lblSecondarySubset.Location = new System.Drawing.Point(12, 54);
            this.lblSecondarySubset.Name = "lblSecondarySubset";
            this.lblSecondarySubset.Size = new System.Drawing.Size(58, 15);
            this.lblSecondarySubset.TabIndex = 1;
            this.lblSecondarySubset.Text = "Subset 2:";
            // 
            // lblPrimarySubset
            // 
            this.lblPrimarySubset.AutoSize = true;
            this.lblPrimarySubset.Location = new System.Drawing.Point(12, 20);
            this.lblPrimarySubset.Name = "lblPrimarySubset";
            this.lblPrimarySubset.Size = new System.Drawing.Size(58, 15);
            this.lblPrimarySubset.TabIndex = 0;
            this.lblPrimarySubset.Text = "Subset 1:";
            // 
            // GmndDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(434, 101);
            this.Controls.Add(this.grpRecolourable);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GmndDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "GMND Options";
            this.grpRecolourable.ResumeLayout(false);
            this.grpRecolourable.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnChangeSubsets;
        private System.Windows.Forms.GroupBox grpRecolourable;
        private System.Windows.Forms.Label lblSecondarySubset;
        private System.Windows.Forms.Label lblPrimarySubset;
        private System.Windows.Forms.ComboBox comboPrimarySubset;
        private System.Windows.Forms.ComboBox comboSecondarySubset;
    }
}
