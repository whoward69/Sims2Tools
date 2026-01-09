/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

namespace SceneGraphPlus.Dialogs.Options
{
    public partial class GzpsDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GzpsDialog));
            this.btnDuplicate = new System.Windows.Forms.Button();
            this.grpDuplicate = new System.Windows.Forms.GroupBox();
            this.lblNewName = new System.Windows.Forms.Label();
            this.textGzpsNewName = new System.Windows.Forms.TextBox();
            this.grpChangeTexture = new System.Windows.Forms.GroupBox();
            this.lblTextureSubset = new System.Windows.Forms.Label();
            this.comboTextureSubset = new System.Windows.Forms.ComboBox();
            this.panelDdsOptions = new System.Windows.Forms.Panel();
            this.ckbRemoveLifos = new System.Windows.Forms.CheckBox();
            this.radioRaw32 = new System.Windows.Forms.RadioButton();
            this.radioRaw24 = new System.Windows.Forms.RadioButton();
            this.radioRaw8 = new System.Windows.Forms.RadioButton();
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
            this.selectImageDialog = new System.Windows.Forms.OpenFileDialog();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.grpDetails = new System.Windows.Forms.GroupBox();
            this.lblDesc = new System.Windows.Forms.Label();
            this.textDesc = new System.Windows.Forms.TextBox();
            this.btnDetailsUpdate = new System.Windows.Forms.Button();
            this.grpDuplicate.SuspendLayout();
            this.grpChangeTexture.SuspendLayout();
            this.panelDdsOptions.SuspendLayout();
            this.grpDetails.SuspendLayout();
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
            this.grpDuplicate.Controls.Add(this.textGzpsNewName);
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
            // textGzpsNewName
            // 
            this.textGzpsNewName.Location = new System.Drawing.Point(84, 17);
            this.textGzpsNewName.Name = "textGzpsNewName";
            this.textGzpsNewName.Size = new System.Drawing.Size(383, 21);
            this.textGzpsNewName.TabIndex = 29;
            this.textGzpsNewName.TextChanged += new System.EventHandler(this.OnGzpsNameChanged);
            this.textGzpsNewName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnGzpsNameKeyUp);
            // 
            // grpChangeTexture
            // 
            this.grpChangeTexture.Controls.Add(this.lblTextureSubset);
            this.grpChangeTexture.Controls.Add(this.comboTextureSubset);
            this.grpChangeTexture.Controls.Add(this.panelDdsOptions);
            this.grpChangeTexture.Controls.Add(this.btnSelectImage);
            this.grpChangeTexture.Controls.Add(this.textNewImage);
            this.grpChangeTexture.Controls.Add(this.lblNewImage);
            this.grpChangeTexture.Controls.Add(this.lblSubset);
            this.grpChangeTexture.Location = new System.Drawing.Point(10, 114);
            this.grpChangeTexture.Name = "grpChangeTexture";
            this.grpChangeTexture.Size = new System.Drawing.Size(562, 210);
            this.grpChangeTexture.TabIndex = 38;
            this.grpChangeTexture.TabStop = false;
            this.grpChangeTexture.Text = "Change Texture:";
            // 
            // lblTextureSubset
            // 
            this.lblTextureSubset.AutoSize = true;
            this.lblTextureSubset.Location = new System.Drawing.Point(6, 21);
            this.lblTextureSubset.Name = "lblTextureSubset";
            this.lblTextureSubset.Size = new System.Drawing.Size(69, 15);
            this.lblTextureSubset.TabIndex = 45;
            this.lblTextureSubset.Text = "For Subset:";
            // 
            // comboTextureSubset
            // 
            this.comboTextureSubset.FormattingEnabled = true;
            this.comboTextureSubset.Location = new System.Drawing.Point(84, 18);
            this.comboTextureSubset.Name = "comboTextureSubset";
            this.comboTextureSubset.Size = new System.Drawing.Size(115, 23);
            this.comboTextureSubset.TabIndex = 44;
            this.comboTextureSubset.SelectedIndexChanged += new System.EventHandler(this.OnSelectedSubsetChanged);
            // 
            // panelDdsOptions
            // 
            this.panelDdsOptions.Controls.Add(this.ckbRemoveLifos);
            this.panelDdsOptions.Controls.Add(this.radioRaw32);
            this.panelDdsOptions.Controls.Add(this.radioRaw24);
            this.panelDdsOptions.Controls.Add(this.radioRaw8);
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
            this.panelDdsOptions.Location = new System.Drawing.Point(6, 75);
            this.panelDdsOptions.Name = "panelDdsOptions";
            this.panelDdsOptions.Size = new System.Drawing.Size(461, 129);
            this.panelDdsOptions.TabIndex = 43;
            // 
            // ckbRemoveLifos
            // 
            this.ckbRemoveLifos.AutoSize = true;
            this.ckbRemoveLifos.Location = new System.Drawing.Point(139, 29);
            this.ckbRemoveLifos.Name = "ckbRemoveLifos";
            this.ckbRemoveLifos.Size = new System.Drawing.Size(107, 19);
            this.ckbRemoveLifos.TabIndex = 53;
            this.ckbRemoveLifos.Text = "Remove LIFOs";
            this.ckbRemoveLifos.UseVisualStyleBackColor = true;
            // 
            // radioRaw32
            // 
            this.radioRaw32.AutoSize = true;
            this.radioRaw32.Location = new System.Drawing.Point(388, 1);
            this.radioRaw32.Name = "radioRaw32";
            this.radioRaw32.Size = new System.Drawing.Size(67, 19);
            this.radioRaw32.TabIndex = 52;
            this.radioRaw32.TabStop = true;
            this.radioRaw32.Text = "Raw 32";
            this.radioRaw32.UseVisualStyleBackColor = true;
            this.radioRaw32.CheckedChanged += new System.EventHandler(this.OnOptionsChanged);
            // 
            // radioRaw24
            // 
            this.radioRaw24.AutoSize = true;
            this.radioRaw24.Location = new System.Drawing.Point(323, 1);
            this.radioRaw24.Name = "radioRaw24";
            this.radioRaw24.Size = new System.Drawing.Size(67, 19);
            this.radioRaw24.TabIndex = 51;
            this.radioRaw24.TabStop = true;
            this.radioRaw24.Text = "Raw 24";
            this.radioRaw24.UseVisualStyleBackColor = true;
            this.radioRaw24.CheckedChanged += new System.EventHandler(this.OnOptionsChanged);
            // 
            // radioRaw8
            // 
            this.radioRaw8.AutoSize = true;
            this.radioRaw8.Location = new System.Drawing.Point(261, 1);
            this.radioRaw8.Name = "radioRaw8";
            this.radioRaw8.Size = new System.Drawing.Size(60, 19);
            this.radioRaw8.TabIndex = 50;
            this.radioRaw8.TabStop = true;
            this.radioRaw8.Text = "Raw 8";
            this.radioRaw8.UseVisualStyleBackColor = true;
            this.radioRaw8.CheckedChanged += new System.EventHandler(this.OnOptionsChanged);
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
            this.comboSharpen.Location = new System.Drawing.Point(307, 103);
            this.comboSharpen.Name = "comboSharpen";
            this.comboSharpen.Size = new System.Drawing.Size(151, 23);
            this.comboSharpen.TabIndex = 49;
            this.comboSharpen.Text = "None";
            this.comboSharpen.SelectedValueChanged += new System.EventHandler(this.OnOptionsChanged);
            // 
            // lblSharpen
            // 
            this.lblSharpen.AutoSize = true;
            this.lblSharpen.Location = new System.Drawing.Point(244, 109);
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
            "Dither",
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
            this.ckbFilters.Location = new System.Drawing.Point(307, 27);
            this.ckbFilters.Name = "ckbFilters";
            this.ckbFilters.ScrollAlwaysVisible = true;
            this.ckbFilters.Size = new System.Drawing.Size(151, 73);
            this.ckbFilters.TabIndex = 46;
            this.ckbFilters.SelectedValueChanged += new System.EventHandler(this.OnOptionsChanged);
            // 
            // lblFilters
            // 
            this.lblFilters.AutoSize = true;
            this.lblFilters.Location = new System.Drawing.Point(258, 30);
            this.lblFilters.Name = "lblFilters";
            this.lblFilters.Size = new System.Drawing.Size(43, 15);
            this.lblFilters.TabIndex = 47;
            this.lblFilters.Text = "Filters:";
            // 
            // textLevels
            // 
            this.textLevels.Location = new System.Drawing.Point(78, 27);
            this.textLevels.Name = "textLevels";
            this.textLevels.Size = new System.Drawing.Size(55, 21);
            this.textLevels.TabIndex = 45;
            this.textLevels.TextChanged += new System.EventHandler(this.OnOptionsChanged);
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
            this.radioDxt1.CheckedChanged += new System.EventHandler(this.OnOptionsChanged);
            // 
            // radioDxt5
            // 
            this.radioDxt5.AutoSize = true;
            this.radioDxt5.Location = new System.Drawing.Point(200, 1);
            this.radioDxt5.Name = "radioDxt5";
            this.radioDxt5.Size = new System.Drawing.Size(59, 19);
            this.radioDxt5.TabIndex = 41;
            this.radioDxt5.TabStop = true;
            this.radioDxt5.Text = "DXT 5";
            this.radioDxt5.UseVisualStyleBackColor = true;
            this.radioDxt5.CheckedChanged += new System.EventHandler(this.OnOptionsChanged);
            // 
            // radioDxt3
            // 
            this.radioDxt3.AutoSize = true;
            this.radioDxt3.Location = new System.Drawing.Point(139, 1);
            this.radioDxt3.Name = "radioDxt3";
            this.radioDxt3.Size = new System.Drawing.Size(59, 19);
            this.radioDxt3.TabIndex = 40;
            this.radioDxt3.TabStop = true;
            this.radioDxt3.Text = "DXT 3";
            this.radioDxt3.UseVisualStyleBackColor = true;
            this.radioDxt3.CheckedChanged += new System.EventHandler(this.OnOptionsChanged);
            // 
            // btnSelectImage
            // 
            this.btnSelectImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectImage.Location = new System.Drawing.Point(473, 44);
            this.btnSelectImage.Name = "btnSelectImage";
            this.btnSelectImage.Size = new System.Drawing.Size(82, 26);
            this.btnSelectImage.TabIndex = 38;
            this.btnSelectImage.Text = "Select";
            this.btnSelectImage.UseVisualStyleBackColor = true;
            this.btnSelectImage.Click += new System.EventHandler(this.OnSelectImageClicked);
            // 
            // textNewImage
            // 
            this.textNewImage.Location = new System.Drawing.Point(84, 47);
            this.textNewImage.Name = "textNewImage";
            this.textNewImage.Size = new System.Drawing.Size(383, 21);
            this.textNewImage.TabIndex = 37;
            this.textNewImage.TextChanged += new System.EventHandler(this.OnImageNameChanged);
            this.textNewImage.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnImageNameKeyUp);
            // 
            // lblNewImage
            // 
            this.lblNewImage.AutoSize = true;
            this.lblNewImage.Location = new System.Drawing.Point(6, 50);
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
            // selectImageDialog
            // 
            this.selectImageDialog.DefaultExt = "png";
            this.selectImageDialog.Filter = "All Images|*.png;*.jpg;*.jpeg;*.bmp;*.gif;*.dds|PNG files|*.png|JPG files|*.jpg;*" +
    ".jpeg|BMP files|*.bmp|GIF files|*.gif|DDS files|*.dds|All files|*.*";
            this.selectImageDialog.Title = "Select Image";
            // 
            // grpDetails
            // 
            this.grpDetails.Controls.Add(this.lblDesc);
            this.grpDetails.Controls.Add(this.textDesc);
            this.grpDetails.Controls.Add(this.btnDetailsUpdate);
            this.grpDetails.Location = new System.Drawing.Point(10, 62);
            this.grpDetails.Name = "grpDetails";
            this.grpDetails.Size = new System.Drawing.Size(562, 46);
            this.grpDetails.TabIndex = 39;
            this.grpDetails.TabStop = false;
            this.grpDetails.Text = "Details:";
            // 
            // lblDesc
            // 
            this.lblDesc.AutoSize = true;
            this.lblDesc.Location = new System.Drawing.Point(40, 17);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(38, 15);
            this.lblDesc.TabIndex = 36;
            this.lblDesc.Text = "Desc:";
            // 
            // textDesc
            // 
            this.textDesc.Location = new System.Drawing.Point(84, 17);
            this.textDesc.Name = "textDesc";
            this.textDesc.Size = new System.Drawing.Size(383, 21);
            this.textDesc.TabIndex = 35;
            this.textDesc.TextChanged += new System.EventHandler(this.OnGzpsDescChanged);
            this.textDesc.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnGzpsDescKeyUp);
            // 
            // btnDetailsUpdate
            // 
            this.btnDetailsUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDetailsUpdate.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnDetailsUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDetailsUpdate.Location = new System.Drawing.Point(473, 14);
            this.btnDetailsUpdate.Name = "btnDetailsUpdate";
            this.btnDetailsUpdate.Size = new System.Drawing.Size(82, 26);
            this.btnDetailsUpdate.TabIndex = 29;
            this.btnDetailsUpdate.Text = "Update";
            this.btnDetailsUpdate.UseVisualStyleBackColor = true;
            this.btnDetailsUpdate.Click += new System.EventHandler(this.OnDetailsUpdateClicked);
            // 
            // GzpsDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(584, 326);
            this.Controls.Add(this.grpDetails);
            this.Controls.Add(this.grpChangeTexture);
            this.Controls.Add(this.grpDuplicate);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GzpsDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "GZPS Options";
            this.grpDuplicate.ResumeLayout(false);
            this.grpDuplicate.PerformLayout();
            this.grpChangeTexture.ResumeLayout(false);
            this.grpChangeTexture.PerformLayout();
            this.panelDdsOptions.ResumeLayout(false);
            this.panelDdsOptions.PerformLayout();
            this.grpDetails.ResumeLayout(false);
            this.grpDetails.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnDuplicate;
        private System.Windows.Forms.GroupBox grpDuplicate;
        private System.Windows.Forms.GroupBox grpChangeTexture;
        private System.Windows.Forms.Label lblSubset;
        private System.Windows.Forms.TextBox textGzpsNewName;
        private System.Windows.Forms.Label lblNewName;
        private System.Windows.Forms.Button btnSelectImage;
        private System.Windows.Forms.TextBox textNewImage;
        private System.Windows.Forms.Label lblNewImage;
        private System.Windows.Forms.OpenFileDialog selectImageDialog;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Panel panelDdsOptions;
        private System.Windows.Forms.CheckBox ckbRemoveLifos;
        private System.Windows.Forms.RadioButton radioRaw32;
        private System.Windows.Forms.RadioButton radioRaw24;
        private System.Windows.Forms.RadioButton radioRaw8;
        private System.Windows.Forms.ComboBox comboSharpen;
        private System.Windows.Forms.Label lblSharpen;
        private System.Windows.Forms.CheckedListBox ckbFilters;
        private System.Windows.Forms.Label lblFilters;
        private System.Windows.Forms.TextBox textLevels;
        private System.Windows.Forms.Label lblLevels;
        private System.Windows.Forms.Label lblFormat;
        private System.Windows.Forms.RadioButton radioDxt1;
        private System.Windows.Forms.RadioButton radioDxt5;
        private System.Windows.Forms.RadioButton radioDxt3;
        private System.Windows.Forms.GroupBox grpDetails;
        private System.Windows.Forms.Label lblDesc;
        private System.Windows.Forms.TextBox textDesc;
        private System.Windows.Forms.Button btnDetailsUpdate;
        private System.Windows.Forms.ComboBox comboTextureSubset;
        private System.Windows.Forms.Label lblTextureSubset;
    }
}
