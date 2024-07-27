/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2024
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

namespace SceneGraphPlus.Dialogs
{
    public partial class FiltersDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FiltersDialog));
            this.grpAge = new System.Windows.Forms.GroupBox();
            this.ckbAgeYoungAdults = new System.Windows.Forms.CheckBox();
            this.ckbAgeBabies = new System.Windows.Forms.CheckBox();
            this.ckbAgeToddlers = new System.Windows.Forms.CheckBox();
            this.ckbAgeElders = new System.Windows.Forms.CheckBox();
            this.ckbAgeAdults = new System.Windows.Forms.CheckBox();
            this.ckbAgeTeens = new System.Windows.Forms.CheckBox();
            this.ckbAgeChildren = new System.Windows.Forms.CheckBox();
            this.grpGender = new System.Windows.Forms.GroupBox();
            this.ckbGenderFemale = new System.Windows.Forms.CheckBox();
            this.ckbGenderMale = new System.Windows.Forms.CheckBox();
            this.ckbGenderUnisex = new System.Windows.Forms.CheckBox();
            this.grpSubset = new System.Windows.Forms.GroupBox();
            this.textSubsets = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnRealign = new System.Windows.Forms.Button();
            this.grpAge.SuspendLayout();
            this.grpGender.SuspendLayout();
            this.grpSubset.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpAge
            // 
            this.grpAge.Controls.Add(this.ckbAgeYoungAdults);
            this.grpAge.Controls.Add(this.ckbAgeBabies);
            this.grpAge.Controls.Add(this.ckbAgeToddlers);
            this.grpAge.Controls.Add(this.ckbAgeElders);
            this.grpAge.Controls.Add(this.ckbAgeAdults);
            this.grpAge.Controls.Add(this.ckbAgeTeens);
            this.grpAge.Controls.Add(this.ckbAgeChildren);
            this.grpAge.Location = new System.Drawing.Point(100, 10);
            this.grpAge.Name = "grpAge";
            this.grpAge.Size = new System.Drawing.Size(110, 140);
            this.grpAge.TabIndex = 2;
            this.grpAge.TabStop = false;
            this.grpAge.Text = "Age:";
            // 
            // ckbAgeYoungAdults
            // 
            this.ckbAgeYoungAdults.AutoSize = true;
            this.ckbAgeYoungAdults.Location = new System.Drawing.Point(10, 83);
            this.ckbAgeYoungAdults.Name = "ckbAgeYoungAdults";
            this.ckbAgeYoungAdults.Size = new System.Drawing.Size(97, 19);
            this.ckbAgeYoungAdults.TabIndex = 8;
            this.ckbAgeYoungAdults.Text = "Young Adults";
            this.ckbAgeYoungAdults.UseVisualStyleBackColor = true;
            this.ckbAgeYoungAdults.Click += new System.EventHandler(this.OnAgeClicked);
            // 
            // ckbAgeBabies
            // 
            this.ckbAgeBabies.AutoSize = true;
            this.ckbAgeBabies.Location = new System.Drawing.Point(10, 15);
            this.ckbAgeBabies.Name = "ckbAgeBabies";
            this.ckbAgeBabies.Size = new System.Drawing.Size(64, 19);
            this.ckbAgeBabies.TabIndex = 7;
            this.ckbAgeBabies.Text = "Babies";
            this.ckbAgeBabies.UseVisualStyleBackColor = true;
            this.ckbAgeBabies.Click += new System.EventHandler(this.OnAgeClicked);
            // 
            // ckbAgeToddlers
            // 
            this.ckbAgeToddlers.AutoSize = true;
            this.ckbAgeToddlers.Location = new System.Drawing.Point(10, 32);
            this.ckbAgeToddlers.Name = "ckbAgeToddlers";
            this.ckbAgeToddlers.Size = new System.Drawing.Size(74, 19);
            this.ckbAgeToddlers.TabIndex = 6;
            this.ckbAgeToddlers.Text = "Toddlers";
            this.ckbAgeToddlers.UseVisualStyleBackColor = true;
            this.ckbAgeToddlers.Click += new System.EventHandler(this.OnAgeClicked);
            // 
            // ckbAgeElders
            // 
            this.ckbAgeElders.AutoSize = true;
            this.ckbAgeElders.Location = new System.Drawing.Point(10, 117);
            this.ckbAgeElders.Name = "ckbAgeElders";
            this.ckbAgeElders.Size = new System.Drawing.Size(61, 19);
            this.ckbAgeElders.TabIndex = 4;
            this.ckbAgeElders.Text = "Elders";
            this.ckbAgeElders.UseVisualStyleBackColor = true;
            this.ckbAgeElders.Click += new System.EventHandler(this.OnAgeClicked);
            // 
            // ckbAgeAdults
            // 
            this.ckbAgeAdults.AutoSize = true;
            this.ckbAgeAdults.Location = new System.Drawing.Point(10, 100);
            this.ckbAgeAdults.Name = "ckbAgeAdults";
            this.ckbAgeAdults.Size = new System.Drawing.Size(59, 19);
            this.ckbAgeAdults.TabIndex = 3;
            this.ckbAgeAdults.Text = "Adults";
            this.ckbAgeAdults.UseVisualStyleBackColor = true;
            this.ckbAgeAdults.Click += new System.EventHandler(this.OnAgeClicked);
            // 
            // ckbAgeTeens
            // 
            this.ckbAgeTeens.AutoSize = true;
            this.ckbAgeTeens.Location = new System.Drawing.Point(10, 66);
            this.ckbAgeTeens.Name = "ckbAgeTeens";
            this.ckbAgeTeens.Size = new System.Drawing.Size(60, 19);
            this.ckbAgeTeens.TabIndex = 2;
            this.ckbAgeTeens.Text = "Teens";
            this.ckbAgeTeens.UseVisualStyleBackColor = true;
            this.ckbAgeTeens.Click += new System.EventHandler(this.OnAgeClicked);
            // 
            // ckbAgeChildren
            // 
            this.ckbAgeChildren.AutoSize = true;
            this.ckbAgeChildren.Location = new System.Drawing.Point(10, 49);
            this.ckbAgeChildren.Name = "ckbAgeChildren";
            this.ckbAgeChildren.Size = new System.Drawing.Size(72, 19);
            this.ckbAgeChildren.TabIndex = 1;
            this.ckbAgeChildren.Text = "Children";
            this.ckbAgeChildren.UseVisualStyleBackColor = true;
            this.ckbAgeChildren.Click += new System.EventHandler(this.OnAgeClicked);
            // 
            // grpGender
            // 
            this.grpGender.Controls.Add(this.ckbGenderFemale);
            this.grpGender.Controls.Add(this.ckbGenderMale);
            this.grpGender.Controls.Add(this.ckbGenderUnisex);
            this.grpGender.Location = new System.Drawing.Point(10, 10);
            this.grpGender.Name = "grpGender";
            this.grpGender.Size = new System.Drawing.Size(80, 140);
            this.grpGender.TabIndex = 9;
            this.grpGender.TabStop = false;
            this.grpGender.Text = "Gender:";
            // 
            // ckbGenderFemale
            // 
            this.ckbGenderFemale.AutoSize = true;
            this.ckbGenderFemale.Location = new System.Drawing.Point(10, 15);
            this.ckbGenderFemale.Name = "ckbGenderFemale";
            this.ckbGenderFemale.Size = new System.Drawing.Size(68, 19);
            this.ckbGenderFemale.TabIndex = 7;
            this.ckbGenderFemale.Text = "Female";
            this.ckbGenderFemale.UseVisualStyleBackColor = true;
            this.ckbGenderFemale.Click += new System.EventHandler(this.OnGenderClicked);
            // 
            // ckbGenderMale
            // 
            this.ckbGenderMale.AutoSize = true;
            this.ckbGenderMale.Location = new System.Drawing.Point(10, 32);
            this.ckbGenderMale.Name = "ckbGenderMale";
            this.ckbGenderMale.Size = new System.Drawing.Size(54, 19);
            this.ckbGenderMale.TabIndex = 6;
            this.ckbGenderMale.Text = "Male";
            this.ckbGenderMale.UseVisualStyleBackColor = true;
            this.ckbGenderMale.Click += new System.EventHandler(this.OnGenderClicked);
            // 
            // ckbGenderUnisex
            // 
            this.ckbGenderUnisex.AutoSize = true;
            this.ckbGenderUnisex.Location = new System.Drawing.Point(10, 49);
            this.ckbGenderUnisex.Name = "ckbGenderUnisex";
            this.ckbGenderUnisex.Size = new System.Drawing.Size(64, 19);
            this.ckbGenderUnisex.TabIndex = 1;
            this.ckbGenderUnisex.Text = "Unisex";
            this.ckbGenderUnisex.UseVisualStyleBackColor = true;
            this.ckbGenderUnisex.Click += new System.EventHandler(this.OnGenderClicked);
            // 
            // grpSubset
            // 
            this.grpSubset.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpSubset.Controls.Add(this.textSubsets);
            this.grpSubset.Location = new System.Drawing.Point(220, 10);
            this.grpSubset.Name = "grpSubset";
            this.grpSubset.Size = new System.Drawing.Size(202, 50);
            this.grpSubset.TabIndex = 27;
            this.grpSubset.TabStop = false;
            this.grpSubset.Text = "Subset:";
            // 
            // textSubsets
            // 
            this.textSubsets.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textSubsets.Location = new System.Drawing.Point(6, 20);
            this.textSubsets.Name = "textSubsets";
            this.textSubsets.Size = new System.Drawing.Size(190, 21);
            this.textSubsets.TabIndex = 0;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Location = new System.Drawing.Point(340, 157);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(82, 26);
            this.btnOK.TabIndex = 28;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.OnOK);
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.Location = new System.Drawing.Point(340, 124);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(82, 26);
            this.btnApply.TabIndex = 29;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.OnApply);
            // 
            // btnRealign
            // 
            this.btnRealign.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRealign.Location = new System.Drawing.Point(252, 124);
            this.btnRealign.Name = "btnRealign";
            this.btnRealign.Size = new System.Drawing.Size(82, 26);
            this.btnRealign.TabIndex = 30;
            this.btnRealign.Text = "Realign";
            this.btnRealign.UseVisualStyleBackColor = true;
            this.btnRealign.Click += new System.EventHandler(this.OnRealign);
            // 
            // FiltersDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(434, 194);
            this.Controls.Add(this.btnRealign);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.grpSubset);
            this.Controls.Add(this.grpGender);
            this.Controls.Add(this.grpAge);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FiltersDialog";
            this.Text = "Filters";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.OnLoad);
            this.grpAge.ResumeLayout(false);
            this.grpAge.PerformLayout();
            this.grpGender.ResumeLayout(false);
            this.grpGender.PerformLayout();
            this.grpSubset.ResumeLayout(false);
            this.grpSubset.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox grpAge;
        private System.Windows.Forms.CheckBox ckbAgeYoungAdults;
        private System.Windows.Forms.CheckBox ckbAgeBabies;
        private System.Windows.Forms.CheckBox ckbAgeToddlers;
        private System.Windows.Forms.CheckBox ckbAgeElders;
        private System.Windows.Forms.CheckBox ckbAgeAdults;
        private System.Windows.Forms.CheckBox ckbAgeTeens;
        private System.Windows.Forms.CheckBox ckbAgeChildren;
        private System.Windows.Forms.GroupBox grpGender;
        private System.Windows.Forms.CheckBox ckbGenderFemale;
        private System.Windows.Forms.CheckBox ckbGenderMale;
        private System.Windows.Forms.CheckBox ckbGenderUnisex;
        private System.Windows.Forms.GroupBox grpSubset;
        private System.Windows.Forms.TextBox textSubsets;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnRealign;
    }
}
