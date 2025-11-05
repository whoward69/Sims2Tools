/*
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
            this.btnDiffCoefs = new System.Windows.Forms.Button();
            this.lblDiffCoefs = new System.Windows.Forms.Label();
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
            this.btnChangeTexture = new System.Windows.Forms.Button();
            this.selectImageDialog = new System.Windows.Forms.OpenFileDialog();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.grpNewMmat = new System.Windows.Forms.GroupBox();
            this.lblMmatSubset = new System.Windows.Forms.Label();
            this.comboAddMmatSubset = new System.Windows.Forms.ComboBox();
            this.btnMmatCreate = new System.Windows.Forms.Button();
            this.grpNewGzps = new System.Windows.Forms.GroupBox();
            this.lblGzpsName = new System.Windows.Forms.Label();
            this.textGzpsName = new System.Windows.Forms.TextBox();
            this.btnGzpsCreate = new System.Windows.Forms.Button();
            this.dlgColourPicker = new System.Windows.Forms.ColorDialog();
            this.grpStdMat = new System.Windows.Forms.GroupBox();
            this.textDiffAlpha = new System.Windows.Forms.TextBox();
            this.lblDiffAlpha = new System.Windows.Forms.Label();
            this.ckbLightingEnabled = new System.Windows.Forms.CheckBox();
            this.lblLightingEnabled = new System.Windows.Forms.Label();
            this.lblAlphaBlendMode = new System.Windows.Forms.Label();
            this.comboAlphaBlendMode = new System.Windows.Forms.ComboBox();
            this.trackDiffAlpha = new System.Windows.Forms.TrackBar();
            this.grpDuplicate.SuspendLayout();
            this.grpChangeTexture.SuspendLayout();
            this.panelDdsOptions.SuspendLayout();
            this.grpNewMmat.SuspendLayout();
            this.grpNewGzps.SuspendLayout();
            this.grpStdMat.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackDiffAlpha)).BeginInit();
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
            this.grpChangeTexture.Controls.Add(this.grpStdMat);
            this.grpChangeTexture.Controls.Add(this.panelDdsOptions);
            this.grpChangeTexture.Controls.Add(this.btnSelectImage);
            this.grpChangeTexture.Controls.Add(this.textNewImage);
            this.grpChangeTexture.Controls.Add(this.lblNewImage);
            this.grpChangeTexture.Controls.Add(this.lblSubset);
            this.grpChangeTexture.Controls.Add(this.btnChangeTexture);
            this.grpChangeTexture.Location = new System.Drawing.Point(10, 63);
            this.grpChangeTexture.Name = "grpChangeTexture";
            this.grpChangeTexture.Size = new System.Drawing.Size(562, 179);
            this.grpChangeTexture.TabIndex = 38;
            this.grpChangeTexture.TabStop = false;
            this.grpChangeTexture.Text = "Change Texture:";
            // 
            // btnDiffCoefs
            // 
            this.btnDiffCoefs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDiffCoefs.Location = new System.Drawing.Point(171, 11);
            this.btnDiffCoefs.Name = "btnDiffCoefs";
            this.btnDiffCoefs.Size = new System.Drawing.Size(103, 23);
            this.btnDiffCoefs.TabIndex = 45;
            this.btnDiffCoefs.Text = "Select";
            this.btnDiffCoefs.UseVisualStyleBackColor = true;
            this.btnDiffCoefs.Click += new System.EventHandler(this.OnSelectColourClicked);
            // 
            // lblDiffCoefs
            // 
            this.lblDiffCoefs.AutoSize = true;
            this.lblDiffCoefs.Location = new System.Drawing.Point(6, 15);
            this.lblDiffCoefs.Name = "lblDiffCoefs";
            this.lblDiffCoefs.Size = new System.Drawing.Size(90, 15);
            this.lblDiffCoefs.TabIndex = 44;
            this.lblDiffCoefs.Text = "stdMatDiffCoef:";
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
            this.panelDdsOptions.Location = new System.Drawing.Point(6, 45);
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
            this.radioDxt5.Location = new System.Drawing.Point(200, 1);
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
            this.radioDxt3.Location = new System.Drawing.Point(139, 1);
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
            this.btnChangeTexture.Location = new System.Drawing.Point(473, 147);
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
            this.grpNewMmat.Location = new System.Drawing.Point(12, 249);
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
            // grpNewGzps
            // 
            this.grpNewGzps.Controls.Add(this.lblGzpsName);
            this.grpNewGzps.Controls.Add(this.textGzpsName);
            this.grpNewGzps.Controls.Add(this.btnGzpsCreate);
            this.grpNewGzps.Location = new System.Drawing.Point(12, 249);
            this.grpNewGzps.Name = "grpNewGzps";
            this.grpNewGzps.Size = new System.Drawing.Size(560, 47);
            this.grpNewGzps.TabIndex = 39;
            this.grpNewGzps.TabStop = false;
            this.grpNewGzps.Text = "New MMAT:";
            // 
            // lblGzpsName
            // 
            this.lblGzpsName.AutoSize = true;
            this.lblGzpsName.Location = new System.Drawing.Point(28, 18);
            this.lblGzpsName.Name = "lblGzpsName";
            this.lblGzpsName.Size = new System.Drawing.Size(48, 15);
            this.lblGzpsName.TabIndex = 35;
            this.lblGzpsName.Text = "Subset:";
            // 
            // textGzpsName
            // 
            this.textGzpsName.Location = new System.Drawing.Point(82, 15);
            this.textGzpsName.Name = "textGzpsName";
            this.textGzpsName.Size = new System.Drawing.Size(241, 21);
            this.textGzpsName.TabIndex = 34;
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
            // dlgColourPicker
            // 
            this.dlgColourPicker.FullOpen = true;
            // 
            // grpStdMat
            // 
            this.grpStdMat.Controls.Add(this.trackDiffAlpha);
            this.grpStdMat.Controls.Add(this.comboAlphaBlendMode);
            this.grpStdMat.Controls.Add(this.lblAlphaBlendMode);
            this.grpStdMat.Controls.Add(this.lblLightingEnabled);
            this.grpStdMat.Controls.Add(this.ckbLightingEnabled);
            this.grpStdMat.Controls.Add(this.lblDiffAlpha);
            this.grpStdMat.Controls.Add(this.textDiffAlpha);
            this.grpStdMat.Controls.Add(this.lblDiffCoefs);
            this.grpStdMat.Controls.Add(this.btnDiffCoefs);
            this.grpStdMat.Location = new System.Drawing.Point(6, 20);
            this.grpStdMat.Name = "grpStdMat";
            this.grpStdMat.Size = new System.Drawing.Size(461, 154);
            this.grpStdMat.TabIndex = 46;
            this.grpStdMat.TabStop = false;
            // 
            // textDiffAlpha
            // 
            this.textDiffAlpha.Location = new System.Drawing.Point(171, 40);
            this.textDiffAlpha.Name = "textDiffAlpha";
            this.textDiffAlpha.Size = new System.Drawing.Size(43, 21);
            this.textDiffAlpha.TabIndex = 46;
            this.textDiffAlpha.TextChanged += new System.EventHandler(this.OnDiffAplhaEdited);
            // 
            // lblDiffAlpha
            // 
            this.lblDiffAlpha.AutoSize = true;
            this.lblDiffAlpha.Location = new System.Drawing.Point(6, 43);
            this.lblDiffAlpha.Name = "lblDiffAlpha";
            this.lblDiffAlpha.Size = new System.Drawing.Size(156, 15);
            this.lblDiffAlpha.TabIndex = 47;
            this.lblDiffAlpha.Text = "stdMatUntexturedDiffAlpha:";
            // 
            // ckbLightingEnabled
            // 
            this.ckbLightingEnabled.AutoSize = true;
            this.ckbLightingEnabled.Location = new System.Drawing.Point(171, 71);
            this.ckbLightingEnabled.Name = "ckbLightingEnabled";
            this.ckbLightingEnabled.Size = new System.Drawing.Size(15, 14);
            this.ckbLightingEnabled.TabIndex = 48;
            this.ckbLightingEnabled.UseVisualStyleBackColor = true;
            this.ckbLightingEnabled.CheckedChanged += new System.EventHandler(this.OnLightingChanged);
            // 
            // lblLightingEnabled
            // 
            this.lblLightingEnabled.AutoSize = true;
            this.lblLightingEnabled.Location = new System.Drawing.Point(6, 70);
            this.lblLightingEnabled.Name = "lblLightingEnabled";
            this.lblLightingEnabled.Size = new System.Drawing.Size(137, 15);
            this.lblLightingEnabled.TabIndex = 49;
            this.lblLightingEnabled.Text = "stdMatLightingEnabled:";
            // 
            // lblAlphaBlendMode
            // 
            this.lblAlphaBlendMode.AutoSize = true;
            this.lblAlphaBlendMode.Location = new System.Drawing.Point(6, 94);
            this.lblAlphaBlendMode.Name = "lblAlphaBlendMode";
            this.lblAlphaBlendMode.Size = new System.Drawing.Size(142, 15);
            this.lblAlphaBlendMode.TabIndex = 50;
            this.lblAlphaBlendMode.Text = "stdMatAlphaBlendMode:";
            // 
            // comboAlphaBlendMode
            // 
            this.comboAlphaBlendMode.FormattingEnabled = true;
            this.comboAlphaBlendMode.Items.AddRange(new object[] {
            "None",
            "Blend",
            "Additive"});
            this.comboAlphaBlendMode.Location = new System.Drawing.Point(171, 91);
            this.comboAlphaBlendMode.Name = "comboAlphaBlendMode";
            this.comboAlphaBlendMode.Size = new System.Drawing.Size(103, 23);
            this.comboAlphaBlendMode.TabIndex = 51;
            this.comboAlphaBlendMode.Text = "None";
            this.comboAlphaBlendMode.SelectedIndexChanged += new System.EventHandler(this.OnBlendModeChanged);
            // 
            // trackDiffAlpha
            // 
            this.trackDiffAlpha.Location = new System.Drawing.Point(220, 40);
            this.trackDiffAlpha.Maximum = 100;
            this.trackDiffAlpha.Name = "trackDiffAlpha";
            this.trackDiffAlpha.Size = new System.Drawing.Size(235, 45);
            this.trackDiffAlpha.TabIndex = 52;
            this.trackDiffAlpha.TickFrequency = 5;
            this.trackDiffAlpha.Scroll += new System.EventHandler(this.OnDiffAlphaScrolled);
            // 
            // TxmtDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(584, 306);
            this.Controls.Add(this.grpNewMmat);
            this.Controls.Add(this.grpNewGzps);
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
            this.grpNewGzps.ResumeLayout(false);
            this.grpNewGzps.PerformLayout();
            this.grpStdMat.ResumeLayout(false);
            this.grpStdMat.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackDiffAlpha)).EndInit();
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
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.GroupBox grpNewMmat;
        private System.Windows.Forms.Label lblMmatSubset;
        private System.Windows.Forms.ComboBox comboAddMmatSubset;
        private System.Windows.Forms.Button btnMmatCreate;
        private System.Windows.Forms.GroupBox grpNewGzps;
        private System.Windows.Forms.Label lblGzpsName;
        private System.Windows.Forms.TextBox textGzpsName;
        private System.Windows.Forms.Button btnGzpsCreate;
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
        private System.Windows.Forms.ColorDialog dlgColourPicker;
        private System.Windows.Forms.Label lblDiffCoefs;
        private System.Windows.Forms.Button btnDiffCoefs;
        private System.Windows.Forms.GroupBox grpStdMat;
        private System.Windows.Forms.ComboBox comboAlphaBlendMode;
        private System.Windows.Forms.Label lblAlphaBlendMode;
        private System.Windows.Forms.Label lblLightingEnabled;
        private System.Windows.Forms.CheckBox ckbLightingEnabled;
        private System.Windows.Forms.Label lblDiffAlpha;
        private System.Windows.Forms.TextBox textDiffAlpha;
        private System.Windows.Forms.TrackBar trackDiffAlpha;
    }
}
