﻿/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2025
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

namespace SceneGraphPlus.Dialogs.Options
{
    public partial class TxmtDialog
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TxmtDialog));
            this.btnDuplicate = new System.Windows.Forms.Button();
            this.grpDuplicate = new System.Windows.Forms.GroupBox();
            this.lblNewName = new System.Windows.Forms.Label();
            this.textTxmtNewName = new System.Windows.Forms.TextBox();
            this.grpChangeTexture = new System.Windows.Forms.GroupBox();
            this.panelDdsOptions = new System.Windows.Forms.Panel();
            this.comboSharpen = new System.Windows.Forms.ComboBox();
            this.lblSharpen = new System.Windows.Forms.Label();
            this.ckbFilters = new System.Windows.Forms.CheckedListBox();
            this.lblFilters = new System.Windows.Forms.Label();
            this.textLevels = new System.Windows.Forms.TextBox();
            this.lblLevels = new System.Windows.Forms.Label();
            this.lblFormat = new System.Windows.Forms.Label();
            this.radioDxt1 = new System.Windows.Forms.RadioButton();
            this.radioDxt5 = new System.Windows.Forms.RadioButton();
            this.radioDxt3 = new System.Windows.Forms.RadioButton();
            this.btnSelectImage = new System.Windows.Forms.Button();
            this.textNewImage = new System.Windows.Forms.TextBox();
            this.lblNewImage = new System.Windows.Forms.Label();
            this.lblSubset = new System.Windows.Forms.Label();
            this.btnChangeTexture = new System.Windows.Forms.Button();
            this.selectImageDialog = new System.Windows.Forms.OpenFileDialog();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.grpNewMmat = new System.Windows.Forms.GroupBox();
            this.lblMmatSubset = new System.Windows.Forms.Label();
            this.comboAddMmatSubset = new System.Windows.Forms.ComboBox();
            this.btnMmatCreate = new System.Windows.Forms.Button();
            this.grpNewGZPS = new System.Windows.Forms.GroupBox();
            this.lblGzpsName = new System.Windows.Forms.Label();
            this.btnGzpsCreate = new System.Windows.Forms.Button();
            this.textGzpsNewName = new System.Windows.Forms.TextBox();
            this.grpDuplicate.SuspendLayout();
            this.grpChangeTexture.SuspendLayout();
            this.panelDdsOptions.SuspendLayout();
            this.grpNewMmat.SuspendLayout();
            this.grpNewGZPS.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnDuplicate
            // 
            this.btnDuplicate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDuplicate.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnDuplicate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDuplicate.Location = new System.Drawing.Point(473, 14);
            this.btnDuplicate.Name = "btnDuplicate";
            this.btnDuplicate.Size = new System.Drawing.Size(82, 26);
            this.btnDuplicate.TabIndex = 28;
            this.btnDuplicate.Text = "Duplicate";
            this.btnDuplicate.UseVisualStyleBackColor = true;
            this.btnDuplicate.Click += new System.EventHandler(this.OnDuplicateClicked);
            // 
            // grpDuplicate
            // 
            this.grpDuplicate.Controls.Add(this.lblNewName);
            this.grpDuplicate.Controls.Add(this.textTxmtNewName);
            this.grpDuplicate.Controls.Add(this.btnDuplicate);
            this.grpDuplicate.Location = new System.Drawing.Point(10, 6);
            this.grpDuplicate.Name = "grpDuplicate";
            this.grpDuplicate.Size = new System.Drawing.Size(562, 50);
            this.grpDuplicate.TabIndex = 29;
            this.grpDuplicate.TabStop = false;
            this.grpDuplicate.Text = "Duplicate:";
            // 
            // lblNewName
            // 
            this.lblNewName.AutoSize = true;
            this.lblNewName.Location = new System.Drawing.Point(6, 20);
            this.lblNewName.Name = "lblNewName";
            this.lblNewName.Size = new System.Drawing.Size(72, 15);
            this.lblNewName.TabIndex = 30;
            this.lblNewName.Text = "New Name:";
            // 
            // textTxmtNewName
            // 
            this.textTxmtNewName.Location = new System.Drawing.Point(84, 17);
            this.textTxmtNewName.Name = "textTxmtNewName";
            this.textTxmtNewName.Size = new System.Drawing.Size(383, 21);
            this.textTxmtNewName.TabIndex = 29;
            this.textTxmtNewName.TextChanged += new System.EventHandler(this.OnTxmtNameChanged);
            this.textTxmtNewName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnTxtrNameKeyUp);
            // 
            // grpChangeTexture
            // 
            this.grpChangeTexture.Controls.Add(this.panelDdsOptions);
            this.grpChangeTexture.Controls.Add(this.btnSelectImage);
            this.grpChangeTexture.Controls.Add(this.textNewImage);
            this.grpChangeTexture.Controls.Add(this.lblNewImage);
            this.grpChangeTexture.Controls.Add(this.lblSubset);
            this.grpChangeTexture.Controls.Add(this.btnChangeTexture);
            this.grpChangeTexture.Location = new System.Drawing.Point(10, 63);
            this.grpChangeTexture.Name = "grpChangeTexture";
            this.grpChangeTexture.Size = new System.Drawing.Size(562, 145);
            this.grpChangeTexture.TabIndex = 38;
            this.grpChangeTexture.TabStop = false;
            this.grpChangeTexture.Text = "Change Texture:";
            // 
            // panelDdsOptions
            // 
            this.panelDdsOptions.Controls.Add(this.comboSharpen);
            this.panelDdsOptions.Controls.Add(this.lblSharpen);
            this.panelDdsOptions.Controls.Add(this.ckbFilters);
            this.panelDdsOptions.Controls.Add(this.lblFilters);
            this.panelDdsOptions.Controls.Add(this.textLevels);
            this.panelDdsOptions.Controls.Add(this.lblLevels);
            this.panelDdsOptions.Controls.Add(this.lblFormat);
            this.panelDdsOptions.Controls.Add(this.radioDxt1);
            this.panelDdsOptions.Controls.Add(this.radioDxt5);
            this.panelDdsOptions.Controls.Add(this.radioDxt3);
            this.panelDdsOptions.Location = new System.Drawing.Point(6, 45);
            this.panelDdsOptions.Name = "panelDdsOptions";
            this.panelDdsOptions.Size = new System.Drawing.Size(461, 95);
            this.panelDdsOptions.TabIndex = 42;
            // 
            // comboSharpen
            // 
            this.comboSharpen.FormattingEnabled = true;
            this.comboSharpen.Items.AddRange(new object[] {
            "None",
            "Negative",
            "Lighter",
            "Darker",
            "ContrastMore",
            "ContrastLess",
            "Smoothen",
            "SharpenSoft",
            "SharpenMedium",
            "SharpenStrong",
            "FindEdges",
            "Contour",
            "EdgeDetect",
            "EdgeDetectSoft",
            "Emboss",
            "MeanRemoval"});
            this.comboSharpen.Location = new System.Drawing.Point(78, 54);
            this.comboSharpen.Name = "comboSharpen";
            this.comboSharpen.Size = new System.Drawing.Size(153, 23);
            this.comboSharpen.TabIndex = 49;
            this.comboSharpen.Text = "None";
            // 
            // lblSharpen
            // 
            this.lblSharpen.AutoSize = true;
            this.lblSharpen.Location = new System.Drawing.Point(15, 57);
            this.lblSharpen.Name = "lblSharpen";
            this.lblSharpen.Size = new System.Drawing.Size(57, 15);
            this.lblSharpen.TabIndex = 48;
            this.lblSharpen.Text = "Sharpen:";
            // 
            // ckbFilters
            // 
            this.ckbFilters.CheckOnClick = true;
            this.ckbFilters.FormattingEnabled = true;
            this.ckbFilters.IntegralHeight = false;
            this.ckbFilters.Items.AddRange(new object[] {
            "dither",
            "Point",
            "Box",
            "Triangle",
            "Quadratic",
            "Cubic",
            "Catrom",
            "Mitchell",
            "Gaussian",
            "Sinc",
            "Bessel",
            "Hanning",
            "Hamming",
            "Blackman",
            "Kaiser"});
            this.ckbFilters.Location = new System.Drawing.Point(281, 27);
            this.ckbFilters.Name = "ckbFilters";
            this.ckbFilters.ScrollAlwaysVisible = true;
            this.ckbFilters.Size = new System.Drawing.Size(177, 62);
            this.ckbFilters.TabIndex = 46;
            // 
            // lblFilters
            // 
            this.lblFilters.AutoSize = true;
            this.lblFilters.Location = new System.Drawing.Point(232, 30);
            this.lblFilters.Name = "lblFilters";
            this.lblFilters.Size = new System.Drawing.Size(43, 15);
            this.lblFilters.TabIndex = 47;
            this.lblFilters.Text = "Filters:";
            // 
            // textLevels
            // 
            this.textLevels.Location = new System.Drawing.Point(78, 27);
            this.textLevels.Name = "textLevels";
            this.textLevels.Size = new System.Drawing.Size(59, 21);
            this.textLevels.TabIndex = 45;
            this.textLevels.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnLevelsKeyPress);
            // 
            // lblLevels
            // 
            this.lblLevels.AutoSize = true;
            this.lblLevels.Location = new System.Drawing.Point(27, 30);
            this.lblLevels.Name = "lblLevels";
            this.lblLevels.Size = new System.Drawing.Size(45, 15);
            this.lblLevels.TabIndex = 44;
            this.lblLevels.Text = "Levels:";
            this.toolTip.SetToolTip(this.lblLevels, resources.GetString("lblLevels.ToolTip"));
            // 
            // lblFormat
            // 
            this.lblFormat.AutoSize = true;
            this.lblFormat.Location = new System.Drawing.Point(23, 3);
            this.lblFormat.Name = "lblFormat";
            this.lblFormat.Size = new System.Drawing.Size(49, 15);
            this.lblFormat.TabIndex = 43;
            this.lblFormat.Text = "Format:";
            // 
            // radioDxt1
            // 
            this.radioDxt1.AutoSize = true;
            this.radioDxt1.Location = new System.Drawing.Point(78, 1);
            this.radioDxt1.Name = "radioDxt1";
            this.radioDxt1.Size = new System.Drawing.Size(59, 19);
            this.radioDxt1.TabIndex = 39;
            this.radioDxt1.TabStop = true;
            this.radioDxt1.Text = "DXT 1";
            this.radioDxt1.UseVisualStyleBackColor = true;
            // 
            // radioDxt5
            // 
            this.radioDxt5.AutoSize = true;
            this.radioDxt5.Location = new System.Drawing.Point(208, 1);
            this.radioDxt5.Name = "radioDxt5";
            this.radioDxt5.Size = new System.Drawing.Size(59, 19);
            this.radioDxt5.TabIndex = 41;
            this.radioDxt5.TabStop = true;
            this.radioDxt5.Text = "DXT 5";
            this.radioDxt5.UseVisualStyleBackColor = true;
            // 
            // radioDxt3
            // 
            this.radioDxt3.AutoSize = true;
            this.radioDxt3.Location = new System.Drawing.Point(143, 1);
            this.radioDxt3.Name = "radioDxt3";
            this.radioDxt3.Size = new System.Drawing.Size(59, 19);
            this.radioDxt3.TabIndex = 40;
            this.radioDxt3.TabStop = true;
            this.radioDxt3.Text = "DXT 3";
            this.radioDxt3.UseVisualStyleBackColor = true;
            // 
            // btnSelectImage
            // 
            this.btnSelectImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectImage.Location = new System.Drawing.Point(473, 12);
            this.btnSelectImage.Name = "btnSelectImage";
            this.btnSelectImage.Size = new System.Drawing.Size(82, 26);
            this.btnSelectImage.TabIndex = 38;
            this.btnSelectImage.Text = "Select";
            this.btnSelectImage.UseVisualStyleBackColor = true;
            this.btnSelectImage.Click += new System.EventHandler(this.OnSelectImageClicked);
            // 
            // textNewImage
            // 
            this.textNewImage.Location = new System.Drawing.Point(84, 17);
            this.textNewImage.Name = "textNewImage";
            this.textNewImage.Size = new System.Drawing.Size(383, 21);
            this.textNewImage.TabIndex = 37;
            this.textNewImage.TextChanged += new System.EventHandler(this.OnImageNameChanged);
            this.textNewImage.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnImageNameKeyUp);
            // 
            // lblNewImage
            // 
            this.lblNewImage.AutoSize = true;
            this.lblNewImage.Location = new System.Drawing.Point(6, 20);
            this.lblNewImage.Name = "lblNewImage";
            this.lblNewImage.Size = new System.Drawing.Size(73, 15);
            this.lblNewImage.TabIndex = 36;
            this.lblNewImage.Text = "New Image:";
            // 
            // lblSubset
            // 
            this.lblSubset.AutoSize = true;
            this.lblSubset.Location = new System.Drawing.Point(22, 17);
            this.lblSubset.Name = "lblSubset";
            this.lblSubset.Size = new System.Drawing.Size(0, 15);
            this.lblSubset.TabIndex = 35;
            // 
            // btnChangeTexture
            // 
            this.btnChangeTexture.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChangeTexture.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnChangeTexture.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChangeTexture.Location = new System.Drawing.Point(473, 113);
            this.btnChangeTexture.Name = "btnChangeTexture";
            this.btnChangeTexture.Size = new System.Drawing.Size(82, 26);
            this.btnChangeTexture.TabIndex = 33;
            this.btnChangeTexture.Text = "Change";
            this.btnChangeTexture.UseVisualStyleBackColor = true;
            this.btnChangeTexture.Click += new System.EventHandler(this.OnChangeTextureClicked);
            // 
            // selectImageDialog
            // 
            this.selectImageDialog.DefaultExt = "png";
            this.selectImageDialog.Filter = "All Images|*.png;*.jpg;*.jpeg;*.bmp;*.gif;*.dds|PNG files|*.png|JPG files|*.jpg;*" +
    ".jpeg|BMP files|*.bmp|GIF files|*.gif|DDS files|*.dds|All files|*.*";
            this.selectImageDialog.Title = "Select Image";
            // 
            // grpNewMmat
            // 
            this.grpNewMmat.Controls.Add(this.lblMmatSubset);
            this.grpNewMmat.Controls.Add(this.comboAddMmatSubset);
            this.grpNewMmat.Controls.Add(this.btnMmatCreate);
            this.grpNewMmat.Location = new System.Drawing.Point(12, 214);
            this.grpNewMmat.Name = "grpNewMmat";
            this.grpNewMmat.Size = new System.Drawing.Size(560, 47);
            this.grpNewMmat.TabIndex = 39;
            this.grpNewMmat.TabStop = false;
            this.grpNewMmat.Text = "New MMAT:";
            // 
            // lblMmatSubset
            // 
            this.lblMmatSubset.AutoSize = true;
            this.lblMmatSubset.Location = new System.Drawing.Point(28, 18);
            this.lblMmatSubset.Name = "lblMmatSubset";
            this.lblMmatSubset.Size = new System.Drawing.Size(48, 15);
            this.lblMmatSubset.TabIndex = 35;
            this.lblMmatSubset.Text = "Subset:";
            // 
            // comboAddMmatSubset
            // 
            this.comboAddMmatSubset.FormattingEnabled = true;
            this.comboAddMmatSubset.Location = new System.Drawing.Point(82, 15);
            this.comboAddMmatSubset.Name = "comboAddMmatSubset";
            this.comboAddMmatSubset.Size = new System.Drawing.Size(241, 23);
            this.comboAddMmatSubset.TabIndex = 34;
            this.comboAddMmatSubset.SelectedIndexChanged += new System.EventHandler(this.OnCreateMmatChanged);
            // 
            // btnMmatCreate
            // 
            this.btnMmatCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMmatCreate.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnMmatCreate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMmatCreate.Location = new System.Drawing.Point(471, 12);
            this.btnMmatCreate.Name = "btnMmatCreate";
            this.btnMmatCreate.Size = new System.Drawing.Size(82, 26);
            this.btnMmatCreate.TabIndex = 33;
            this.btnMmatCreate.Text = "Create";
            this.btnMmatCreate.UseVisualStyleBackColor = true;
            this.btnMmatCreate.Click += new System.EventHandler(this.OnAddMmatClicked);
            // 
            // grpNewGZPS
            // 
            this.grpNewGZPS.Controls.Add(this.textGzpsNewName);
            this.grpNewGZPS.Controls.Add(this.lblGzpsName);
            this.grpNewGZPS.Controls.Add(this.btnGzpsCreate);
            this.grpNewGZPS.Location = new System.Drawing.Point(12, 214);
            this.grpNewGZPS.Name = "grpNewGZPS";
            this.grpNewGZPS.Size = new System.Drawing.Size(560, 47);
            this.grpNewGZPS.TabIndex = 40;
            this.grpNewGZPS.TabStop = false;
            this.grpNewGZPS.Text = "New GZPS:";
            // 
            // lblGzpsName
            // 
            this.lblGzpsName.AutoSize = true;
            this.lblGzpsName.Location = new System.Drawing.Point(6, 18);
            this.lblGzpsName.Name = "lblGzpsName";
            this.lblGzpsName.Size = new System.Drawing.Size(72, 15);
            this.lblGzpsName.TabIndex = 35;
            this.lblGzpsName.Text = "New Name:";
            // 
            // btnGzpsCreate
            // 
            this.btnGzpsCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGzpsCreate.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnGzpsCreate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGzpsCreate.Location = new System.Drawing.Point(471, 12);
            this.btnGzpsCreate.Name = "btnGzpsCreate";
            this.btnGzpsCreate.Size = new System.Drawing.Size(82, 26);
            this.btnGzpsCreate.TabIndex = 33;
            this.btnGzpsCreate.Text = "Create";
            this.btnGzpsCreate.UseVisualStyleBackColor = true;
            this.btnGzpsCreate.Click += new System.EventHandler(this.OnAddGzpsClicked);
            // 
            // textGzpsNewName
            // 
            this.textGzpsNewName.Location = new System.Drawing.Point(84, 15);
            this.textGzpsNewName.Name = "textGzpsNewName";
            this.textGzpsNewName.Size = new System.Drawing.Size(383, 21);
            this.textGzpsNewName.TabIndex = 36;
            this.textGzpsNewName.TextChanged += new System.EventHandler(this.OnGzpsNameChanged);
            this.textGzpsNewName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnGzpsNameKeyUp);
            // 
            // TxmtDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(584, 266);
            this.Controls.Add(this.grpNewGZPS);
            this.Controls.Add(this.grpNewMmat);
            this.Controls.Add(this.grpChangeTexture);
            this.Controls.Add(this.grpDuplicate);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TxmtDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "TXMT Options";
            this.grpDuplicate.ResumeLayout(false);
            this.grpDuplicate.PerformLayout();
            this.grpChangeTexture.ResumeLayout(false);
            this.grpChangeTexture.PerformLayout();
            this.panelDdsOptions.ResumeLayout(false);
            this.panelDdsOptions.PerformLayout();
            this.grpNewMmat.ResumeLayout(false);
            this.grpNewMmat.PerformLayout();
            this.grpNewGZPS.ResumeLayout(false);
            this.grpNewGZPS.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnDuplicate;
        private System.Windows.Forms.GroupBox grpDuplicate;
        private System.Windows.Forms.GroupBox grpChangeTexture;
        private System.Windows.Forms.Label lblSubset;
        private System.Windows.Forms.Button btnChangeTexture;
        private System.Windows.Forms.TextBox textTxmtNewName;
        private System.Windows.Forms.Label lblNewName;
        private System.Windows.Forms.Button btnSelectImage;
        private System.Windows.Forms.TextBox textNewImage;
        private System.Windows.Forms.Label lblNewImage;
        private System.Windows.Forms.OpenFileDialog selectImageDialog;
        private System.Windows.Forms.Panel panelDdsOptions;
        private System.Windows.Forms.RadioButton radioDxt1;
        private System.Windows.Forms.RadioButton radioDxt5;
        private System.Windows.Forms.RadioButton radioDxt3;
        private System.Windows.Forms.TextBox textLevels;
        private System.Windows.Forms.Label lblLevels;
        private System.Windows.Forms.Label lblFormat;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label lblFilters;
        private System.Windows.Forms.CheckedListBox ckbFilters;
        private System.Windows.Forms.ComboBox comboSharpen;
        private System.Windows.Forms.Label lblSharpen;
        private System.Windows.Forms.GroupBox grpNewMmat;
        private System.Windows.Forms.Label lblMmatSubset;
        private System.Windows.Forms.ComboBox comboAddMmatSubset;
        private System.Windows.Forms.Button btnMmatCreate;
        private System.Windows.Forms.GroupBox grpNewGZPS;
        private System.Windows.Forms.TextBox textGzpsNewName;
        private System.Windows.Forms.Label lblGzpsName;
        private System.Windows.Forms.Button btnGzpsCreate;
    }
}
