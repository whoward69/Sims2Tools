/*
 * BHAV Finder - a utility for searching The Sims 2 package files for BHAV that match specified criteria
 *             - see http://www.picknmixmods.com/Sims2/Notes/BhavFinder/BhavFinder.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */
 
namespace BhavFinder
{
    partial class BhavFinderForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BhavFinderForm));
            this.lblFilePath = new System.Windows.Forms.Label();
            this.textFilePath = new System.Windows.Forms.TextBox();
            this.btnSelect = new System.Windows.Forms.Button();
            this.selectFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.lblOpCode = new System.Windows.Forms.Label();
            this.comboOpCode = new System.Windows.Forms.ComboBox();
            this.lblVersion = new System.Windows.Forms.Label();
            this.comboVersion = new System.Windows.Forms.ComboBox();
            this.btnClearOpCode = new System.Windows.Forms.Button();
            this.lblOperands = new System.Windows.Forms.Label();
            this.textOperand0 = new System.Windows.Forms.TextBox();
            this.menuContextOperands = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemPasteGUID = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemPasteSimPeOperands = new System.Windows.Forms.ToolStripMenuItem();
            this.textOperand1 = new System.Windows.Forms.TextBox();
            this.textOperand2 = new System.Windows.Forms.TextBox();
            this.textOperand3 = new System.Windows.Forms.TextBox();
            this.textOperand4 = new System.Windows.Forms.TextBox();
            this.textOperand5 = new System.Windows.Forms.TextBox();
            this.textOperand6 = new System.Windows.Forms.TextBox();
            this.textOperand7 = new System.Windows.Forms.TextBox();
            this.textOperand8 = new System.Windows.Forms.TextBox();
            this.textOperand9 = new System.Windows.Forms.TextBox();
            this.textOperand10 = new System.Windows.Forms.TextBox();
            this.textOperand11 = new System.Windows.Forms.TextBox();
            this.textOperand12 = new System.Windows.Forms.TextBox();
            this.textOperand13 = new System.Windows.Forms.TextBox();
            this.textOperand14 = new System.Windows.Forms.TextBox();
            this.textOperand15 = new System.Windows.Forms.TextBox();
            this.btnClearOperands = new System.Windows.Forms.Button();
            this.lblMasks = new System.Windows.Forms.Label();
            this.textMask0 = new System.Windows.Forms.TextBox();
            this.textMask1 = new System.Windows.Forms.TextBox();
            this.textMask2 = new System.Windows.Forms.TextBox();
            this.textMask3 = new System.Windows.Forms.TextBox();
            this.textMask4 = new System.Windows.Forms.TextBox();
            this.textMask5 = new System.Windows.Forms.TextBox();
            this.textMask6 = new System.Windows.Forms.TextBox();
            this.textMask7 = new System.Windows.Forms.TextBox();
            this.textMask8 = new System.Windows.Forms.TextBox();
            this.textMask9 = new System.Windows.Forms.TextBox();
            this.textMask10 = new System.Windows.Forms.TextBox();
            this.textMask11 = new System.Windows.Forms.TextBox();
            this.textMask12 = new System.Windows.Forms.TextBox();
            this.textMask13 = new System.Windows.Forms.TextBox();
            this.textMask14 = new System.Windows.Forms.TextBox();
            this.textMask15 = new System.Windows.Forms.TextBox();
            this.btnResetMasks = new System.Windows.Forms.Button();
            this.lblBhavInGroup = new System.Windows.Forms.Label();
            this.comboBhavInGroup = new System.Windows.Forms.ComboBox();
            this.lblOpCodeInGroup = new System.Windows.Forms.Label();
            this.comboOpCodeInGroup = new System.Windows.Forms.ComboBox();
            this.btnClearGroups = new System.Windows.Forms.Button();
            this.lblUsingOperand = new System.Windows.Forms.Label();
            this.comboUsingOperand = new System.Windows.Forms.ComboBox();
            this.lblUsingIndex = new System.Windows.Forms.Label();
            this.comboUsingSTR = new System.Windows.Forms.ComboBox();
            this.lblUsingMatches = new System.Windows.Forms.Label();
            this.textUsingRegex = new System.Windows.Forms.TextBox();
            this.btnUsingClear = new System.Windows.Forms.Button();
            this.lblProgress = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblShowNames = new System.Windows.Forms.Label();
            this.checkShowNames = new System.Windows.Forms.CheckBox();
            this.btnGO = new System.Windows.Forms.Button();
            this.gridFoundBhavs = new System.Windows.Forms.DataGridView();
            this.colBhavPackage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBhavDbpfPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBhavDbpfEntry = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBhavInstance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBhavName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBhavGroupInstance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBhavGroupName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuContextFoundBhavs = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemExtract = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTipOperands = new System.Windows.Forms.ToolTip(this.components);
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSelectPackage = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSelectFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemRecentPackages = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemSaveResultsToClipboard = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSaveResultsAs = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemConfiguration = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMode = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAdvanced = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemRestoreFilters = new System.Windows.Forms.ToolStripMenuItem();
            this.saveResultsDialog = new System.Windows.Forms.SaveFileDialog();
            this.bhavFinderWorker = new System.ComponentModel.BackgroundWorker();
            this.lblUsingIgnoreCase = new System.Windows.Forms.Label();
            this.checkUsingIgnoreCase = new System.Windows.Forms.CheckBox();
            this.lblNameRegex = new System.Windows.Forms.Label();
            this.textNameRegex = new System.Windows.Forms.TextBox();
            this.btnClearBhavDetails = new System.Windows.Forms.Button();
            this.lblParams = new System.Windows.Forms.Label();
            this.textParams = new System.Windows.Forms.TextBox();
            this.textLocals = new System.Windows.Forms.TextBox();
            this.lblLocals = new System.Windows.Forms.Label();
            this.checkNameCaseSensitive = new System.Windows.Forms.CheckBox();
            this.lblNameCaseSensitive = new System.Windows.Forms.Label();
            this.lblOperand0 = new System.Windows.Forms.Label();
            this.lblOperand2 = new System.Windows.Forms.Label();
            this.lblOperand4 = new System.Windows.Forms.Label();
            this.lblOperand6 = new System.Windows.Forms.Label();
            this.lblOperand8 = new System.Windows.Forms.Label();
            this.lblOperand10 = new System.Windows.Forms.Label();
            this.lblOperand12 = new System.Windows.Forms.Label();
            this.lblOperand14 = new System.Windows.Forms.Label();
            this.menuContextOperands.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridFoundBhavs)).BeginInit();
            this.menuContextFoundBhavs.SuspendLayout();
            this.menuMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblFilePath
            // 
            this.lblFilePath.AutoSize = true;
            this.lblFilePath.Location = new System.Drawing.Point(10, 41);
            this.lblFilePath.Name = "lblFilePath";
            this.lblFilePath.Size = new System.Drawing.Size(86, 15);
            this.lblFilePath.TabIndex = 0;
            this.lblFilePath.Text = "Package Path:";
            // 
            // textFilePath
            // 
            this.textFilePath.AllowDrop = true;
            this.textFilePath.Location = new System.Drawing.Point(111, 38);
            this.textFilePath.Name = "textFilePath";
            this.textFilePath.Size = new System.Drawing.Size(816, 21);
            this.textFilePath.TabIndex = 0;
            this.textFilePath.TabStop = false;
            this.textFilePath.WordWrap = false;
            this.textFilePath.TextChanged += new System.EventHandler(this.OnFilePathChanged);
            this.textFilePath.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnDragDrop);
            this.textFilePath.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnDragEnter);
            // 
            // btnSelect
            // 
            this.btnSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelect.Location = new System.Drawing.Point(934, 33);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(143, 30);
            this.btnSelect.TabIndex = 2;
            this.btnSelect.Text = "&Select Package...";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.OnSelectClicked);
            // 
            // lblOpCode
            // 
            this.lblOpCode.AutoSize = true;
            this.lblOpCode.Location = new System.Drawing.Point(10, 111);
            this.lblOpCode.Name = "lblOpCode";
            this.lblOpCode.Size = new System.Drawing.Size(55, 15);
            this.lblOpCode.TabIndex = 0;
            this.lblOpCode.Text = "OpCode:";
            // 
            // comboOpCode
            // 
            this.comboOpCode.FormattingEnabled = true;
            this.comboOpCode.Location = new System.Drawing.Point(111, 108);
            this.comboOpCode.Name = "comboOpCode";
            this.comboOpCode.Size = new System.Drawing.Size(245, 23);
            this.comboOpCode.TabIndex = 3;
            this.comboOpCode.SelectedValueChanged += new System.EventHandler(this.OnOpCodeChanged);
            this.comboOpCode.TextChanged += new System.EventHandler(this.OnOpCodeChanged);
            this.comboOpCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress_OpCode);
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(365, 111);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(51, 15);
            this.lblVersion.TabIndex = 0;
            this.lblVersion.Text = "Version:";
            // 
            // comboVersion
            // 
            this.comboVersion.FormattingEnabled = true;
            this.comboVersion.Items.AddRange(new object[] {
            "",
            "0x00",
            "0x01",
            "0x02",
            "0x03",
            "0x04"});
            this.comboVersion.Location = new System.Drawing.Point(420, 108);
            this.comboVersion.Name = "comboVersion";
            this.comboVersion.Size = new System.Drawing.Size(81, 23);
            this.comboVersion.TabIndex = 4;
            this.comboVersion.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress_Ignore);
            // 
            // btnClearOpCode
            // 
            this.btnClearOpCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearOpCode.Location = new System.Drawing.Point(934, 104);
            this.btnClearOpCode.Name = "btnClearOpCode";
            this.btnClearOpCode.Size = new System.Drawing.Size(143, 30);
            this.btnClearOpCode.TabIndex = 5;
            this.btnClearOpCode.Text = "Clear &OpCode";
            this.btnClearOpCode.UseVisualStyleBackColor = true;
            this.btnClearOpCode.Click += new System.EventHandler(this.OnClearOpCodeClicked);
            // 
            // lblOperands
            // 
            this.lblOperands.AutoSize = true;
            this.lblOperands.Location = new System.Drawing.Point(10, 148);
            this.lblOperands.Name = "lblOperands";
            this.lblOperands.Size = new System.Drawing.Size(64, 15);
            this.lblOperands.TabIndex = 0;
            this.lblOperands.Text = "Operands:";
            // 
            // textOperand0
            // 
            this.textOperand0.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textOperand0.ContextMenuStrip = this.menuContextOperands;
            this.textOperand0.Location = new System.Drawing.Point(111, 145);
            this.textOperand0.MaxLength = 2;
            this.textOperand0.Name = "textOperand0";
            this.textOperand0.Size = new System.Drawing.Size(35, 21);
            this.textOperand0.TabIndex = 6;
            this.textOperand0.WordWrap = false;
            this.textOperand0.TextChanged += new System.EventHandler(this.OnOperandChanged);
            this.textOperand0.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress_HexOnly);
            // 
            // menuContextOperands
            // 
            this.menuContextOperands.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemPasteGUID,
            this.menuItemPasteSimPeOperands});
            this.menuContextOperands.Name = "menuContextOperands";
            this.menuContextOperands.Size = new System.Drawing.Size(193, 48);
            this.menuContextOperands.Opening += new System.ComponentModel.CancelEventHandler(this.OnContextMenuOperandsOpening);
            // 
            // menuItemPasteGUID
            // 
            this.menuItemPasteGUID.Name = "menuItemPasteGUID";
            this.menuItemPasteGUID.Size = new System.Drawing.Size(192, 22);
            this.menuItemPasteGUID.Text = "Paste GUID";
            this.menuItemPasteGUID.Click += new System.EventHandler(this.OnPasteGuidClicked);
            // 
            // menuItemPasteSimPeOperands
            // 
            this.menuItemPasteSimPeOperands.Name = "menuItemPasteSimPeOperands";
            this.menuItemPasteSimPeOperands.Size = new System.Drawing.Size(192, 22);
            this.menuItemPasteSimPeOperands.Text = "Paste SimPe Operands";
            this.menuItemPasteSimPeOperands.Click += new System.EventHandler(this.OnPasteSimPeOperandsClicked);
            // 
            // textOperand1
            // 
            this.textOperand1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textOperand1.ContextMenuStrip = this.menuContextOperands;
            this.textOperand1.Location = new System.Drawing.Point(157, 145);
            this.textOperand1.MaxLength = 2;
            this.textOperand1.Name = "textOperand1";
            this.textOperand1.Size = new System.Drawing.Size(35, 21);
            this.textOperand1.TabIndex = 7;
            this.textOperand1.WordWrap = false;
            this.textOperand1.TextChanged += new System.EventHandler(this.OnOperandChanged);
            this.textOperand1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress_HexOnly);
            // 
            // textOperand2
            // 
            this.textOperand2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textOperand2.ContextMenuStrip = this.menuContextOperands;
            this.textOperand2.Location = new System.Drawing.Point(214, 145);
            this.textOperand2.MaxLength = 2;
            this.textOperand2.Name = "textOperand2";
            this.textOperand2.Size = new System.Drawing.Size(35, 21);
            this.textOperand2.TabIndex = 8;
            this.textOperand2.WordWrap = false;
            this.textOperand2.TextChanged += new System.EventHandler(this.OnOperandChanged);
            this.textOperand2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress_HexOnly);
            // 
            // textOperand3
            // 
            this.textOperand3.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textOperand3.ContextMenuStrip = this.menuContextOperands;
            this.textOperand3.Location = new System.Drawing.Point(260, 145);
            this.textOperand3.MaxLength = 2;
            this.textOperand3.Name = "textOperand3";
            this.textOperand3.Size = new System.Drawing.Size(35, 21);
            this.textOperand3.TabIndex = 9;
            this.textOperand3.WordWrap = false;
            this.textOperand3.TextChanged += new System.EventHandler(this.OnOperandChanged);
            this.textOperand3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress_HexOnly);
            // 
            // textOperand4
            // 
            this.textOperand4.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textOperand4.ContextMenuStrip = this.menuContextOperands;
            this.textOperand4.Location = new System.Drawing.Point(317, 145);
            this.textOperand4.MaxLength = 2;
            this.textOperand4.Name = "textOperand4";
            this.textOperand4.Size = new System.Drawing.Size(35, 21);
            this.textOperand4.TabIndex = 10;
            this.textOperand4.WordWrap = false;
            this.textOperand4.TextChanged += new System.EventHandler(this.OnOperandChanged);
            this.textOperand4.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress_HexOnly);
            // 
            // textOperand5
            // 
            this.textOperand5.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textOperand5.ContextMenuStrip = this.menuContextOperands;
            this.textOperand5.Location = new System.Drawing.Point(363, 145);
            this.textOperand5.MaxLength = 2;
            this.textOperand5.Name = "textOperand5";
            this.textOperand5.Size = new System.Drawing.Size(35, 21);
            this.textOperand5.TabIndex = 11;
            this.textOperand5.WordWrap = false;
            this.textOperand5.TextChanged += new System.EventHandler(this.OnOperandChanged);
            this.textOperand5.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress_HexOnly);
            // 
            // textOperand6
            // 
            this.textOperand6.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textOperand6.ContextMenuStrip = this.menuContextOperands;
            this.textOperand6.Location = new System.Drawing.Point(420, 145);
            this.textOperand6.MaxLength = 2;
            this.textOperand6.Name = "textOperand6";
            this.textOperand6.Size = new System.Drawing.Size(35, 21);
            this.textOperand6.TabIndex = 12;
            this.textOperand6.WordWrap = false;
            this.textOperand6.TextChanged += new System.EventHandler(this.OnOperandChanged);
            this.textOperand6.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress_HexOnly);
            // 
            // textOperand7
            // 
            this.textOperand7.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textOperand7.ContextMenuStrip = this.menuContextOperands;
            this.textOperand7.Location = new System.Drawing.Point(466, 145);
            this.textOperand7.MaxLength = 2;
            this.textOperand7.Name = "textOperand7";
            this.textOperand7.Size = new System.Drawing.Size(35, 21);
            this.textOperand7.TabIndex = 13;
            this.textOperand7.WordWrap = false;
            this.textOperand7.TextChanged += new System.EventHandler(this.OnOperandChanged);
            this.textOperand7.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress_HexOnly);
            // 
            // textOperand8
            // 
            this.textOperand8.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textOperand8.ContextMenuStrip = this.menuContextOperands;
            this.textOperand8.Location = new System.Drawing.Point(537, 145);
            this.textOperand8.MaxLength = 2;
            this.textOperand8.Name = "textOperand8";
            this.textOperand8.Size = new System.Drawing.Size(35, 21);
            this.textOperand8.TabIndex = 14;
            this.textOperand8.WordWrap = false;
            this.textOperand8.TextChanged += new System.EventHandler(this.OnOperandChanged);
            this.textOperand8.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress_HexOnly);
            // 
            // textOperand9
            // 
            this.textOperand9.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textOperand9.ContextMenuStrip = this.menuContextOperands;
            this.textOperand9.Location = new System.Drawing.Point(583, 145);
            this.textOperand9.MaxLength = 2;
            this.textOperand9.Name = "textOperand9";
            this.textOperand9.Size = new System.Drawing.Size(35, 21);
            this.textOperand9.TabIndex = 15;
            this.textOperand9.WordWrap = false;
            this.textOperand9.TextChanged += new System.EventHandler(this.OnOperandChanged);
            this.textOperand9.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress_HexOnly);
            // 
            // textOperand10
            // 
            this.textOperand10.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textOperand10.ContextMenuStrip = this.menuContextOperands;
            this.textOperand10.Location = new System.Drawing.Point(640, 145);
            this.textOperand10.MaxLength = 2;
            this.textOperand10.Name = "textOperand10";
            this.textOperand10.Size = new System.Drawing.Size(35, 21);
            this.textOperand10.TabIndex = 16;
            this.textOperand10.WordWrap = false;
            this.textOperand10.TextChanged += new System.EventHandler(this.OnOperandChanged);
            this.textOperand10.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress_HexOnly);
            // 
            // textOperand11
            // 
            this.textOperand11.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textOperand11.ContextMenuStrip = this.menuContextOperands;
            this.textOperand11.Location = new System.Drawing.Point(686, 145);
            this.textOperand11.MaxLength = 2;
            this.textOperand11.Name = "textOperand11";
            this.textOperand11.Size = new System.Drawing.Size(35, 21);
            this.textOperand11.TabIndex = 17;
            this.textOperand11.WordWrap = false;
            this.textOperand11.TextChanged += new System.EventHandler(this.OnOperandChanged);
            this.textOperand11.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress_HexOnly);
            // 
            // textOperand12
            // 
            this.textOperand12.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textOperand12.ContextMenuStrip = this.menuContextOperands;
            this.textOperand12.Location = new System.Drawing.Point(743, 145);
            this.textOperand12.MaxLength = 2;
            this.textOperand12.Name = "textOperand12";
            this.textOperand12.Size = new System.Drawing.Size(35, 21);
            this.textOperand12.TabIndex = 18;
            this.textOperand12.WordWrap = false;
            this.textOperand12.TextChanged += new System.EventHandler(this.OnOperandChanged);
            this.textOperand12.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress_HexOnly);
            // 
            // textOperand13
            // 
            this.textOperand13.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textOperand13.Location = new System.Drawing.Point(789, 145);
            this.textOperand13.MaxLength = 2;
            this.textOperand13.Name = "textOperand13";
            this.textOperand13.Size = new System.Drawing.Size(35, 21);
            this.textOperand13.TabIndex = 19;
            this.textOperand13.WordWrap = false;
            this.textOperand13.TextChanged += new System.EventHandler(this.OnOperandChanged);
            this.textOperand13.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress_HexOnly);
            // 
            // textOperand14
            // 
            this.textOperand14.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textOperand14.Location = new System.Drawing.Point(846, 145);
            this.textOperand14.MaxLength = 2;
            this.textOperand14.Name = "textOperand14";
            this.textOperand14.Size = new System.Drawing.Size(35, 21);
            this.textOperand14.TabIndex = 20;
            this.textOperand14.WordWrap = false;
            this.textOperand14.TextChanged += new System.EventHandler(this.OnOperandChanged);
            this.textOperand14.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress_HexOnly);
            // 
            // textOperand15
            // 
            this.textOperand15.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textOperand15.Location = new System.Drawing.Point(892, 145);
            this.textOperand15.MaxLength = 2;
            this.textOperand15.Name = "textOperand15";
            this.textOperand15.Size = new System.Drawing.Size(35, 21);
            this.textOperand15.TabIndex = 21;
            this.textOperand15.WordWrap = false;
            this.textOperand15.TextChanged += new System.EventHandler(this.OnOperandChanged);
            this.textOperand15.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress_HexOnly);
            // 
            // btnClearOperands
            // 
            this.btnClearOperands.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearOperands.Location = new System.Drawing.Point(934, 140);
            this.btnClearOperands.Name = "btnClearOperands";
            this.btnClearOperands.Size = new System.Drawing.Size(143, 30);
            this.btnClearOperands.TabIndex = 22;
            this.btnClearOperands.Text = "&Clear Operands";
            this.btnClearOperands.UseVisualStyleBackColor = true;
            this.btnClearOperands.Click += new System.EventHandler(this.OnClearOperandsClicked);
            // 
            // lblMasks
            // 
            this.lblMasks.AutoSize = true;
            this.lblMasks.Location = new System.Drawing.Point(10, 183);
            this.lblMasks.Name = "lblMasks";
            this.lblMasks.Size = new System.Drawing.Size(46, 15);
            this.lblMasks.TabIndex = 0;
            this.lblMasks.Text = "Masks:";
            // 
            // textMask0
            // 
            this.textMask0.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textMask0.Location = new System.Drawing.Point(111, 180);
            this.textMask0.MaxLength = 2;
            this.textMask0.Name = "textMask0";
            this.textMask0.Size = new System.Drawing.Size(35, 21);
            this.textMask0.TabIndex = 23;
            this.textMask0.Text = "FF";
            this.textMask0.WordWrap = false;
            this.textMask0.TextChanged += new System.EventHandler(this.OnMaskChanged);
            this.textMask0.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress_HexOnly);
            // 
            // textMask1
            // 
            this.textMask1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textMask1.Location = new System.Drawing.Point(157, 180);
            this.textMask1.MaxLength = 2;
            this.textMask1.Name = "textMask1";
            this.textMask1.Size = new System.Drawing.Size(35, 21);
            this.textMask1.TabIndex = 24;
            this.textMask1.Text = "FF";
            this.textMask1.WordWrap = false;
            this.textMask1.TextChanged += new System.EventHandler(this.OnMaskChanged);
            this.textMask1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress_HexOnly);
            // 
            // textMask2
            // 
            this.textMask2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textMask2.Location = new System.Drawing.Point(214, 180);
            this.textMask2.MaxLength = 2;
            this.textMask2.Name = "textMask2";
            this.textMask2.Size = new System.Drawing.Size(35, 21);
            this.textMask2.TabIndex = 25;
            this.textMask2.Text = "FF";
            this.textMask2.WordWrap = false;
            this.textMask2.TextChanged += new System.EventHandler(this.OnMaskChanged);
            this.textMask2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress_HexOnly);
            // 
            // textMask3
            // 
            this.textMask3.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textMask3.Location = new System.Drawing.Point(260, 180);
            this.textMask3.MaxLength = 2;
            this.textMask3.Name = "textMask3";
            this.textMask3.Size = new System.Drawing.Size(35, 21);
            this.textMask3.TabIndex = 26;
            this.textMask3.Text = "FF";
            this.textMask3.WordWrap = false;
            this.textMask3.TextChanged += new System.EventHandler(this.OnMaskChanged);
            this.textMask3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress_HexOnly);
            // 
            // textMask4
            // 
            this.textMask4.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textMask4.Location = new System.Drawing.Point(317, 180);
            this.textMask4.MaxLength = 2;
            this.textMask4.Name = "textMask4";
            this.textMask4.Size = new System.Drawing.Size(35, 21);
            this.textMask4.TabIndex = 27;
            this.textMask4.Text = "FF";
            this.textMask4.WordWrap = false;
            this.textMask4.TextChanged += new System.EventHandler(this.OnMaskChanged);
            this.textMask4.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress_HexOnly);
            // 
            // textMask5
            // 
            this.textMask5.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textMask5.Location = new System.Drawing.Point(363, 180);
            this.textMask5.MaxLength = 2;
            this.textMask5.Name = "textMask5";
            this.textMask5.Size = new System.Drawing.Size(35, 21);
            this.textMask5.TabIndex = 28;
            this.textMask5.Text = "FF";
            this.textMask5.WordWrap = false;
            this.textMask5.TextChanged += new System.EventHandler(this.OnMaskChanged);
            this.textMask5.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress_HexOnly);
            // 
            // textMask6
            // 
            this.textMask6.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textMask6.Location = new System.Drawing.Point(420, 180);
            this.textMask6.MaxLength = 2;
            this.textMask6.Name = "textMask6";
            this.textMask6.Size = new System.Drawing.Size(35, 21);
            this.textMask6.TabIndex = 29;
            this.textMask6.Text = "FF";
            this.textMask6.WordWrap = false;
            this.textMask6.TextChanged += new System.EventHandler(this.OnMaskChanged);
            this.textMask6.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress_HexOnly);
            // 
            // textMask7
            // 
            this.textMask7.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textMask7.Location = new System.Drawing.Point(466, 180);
            this.textMask7.MaxLength = 2;
            this.textMask7.Name = "textMask7";
            this.textMask7.Size = new System.Drawing.Size(35, 21);
            this.textMask7.TabIndex = 30;
            this.textMask7.Text = "FF";
            this.textMask7.WordWrap = false;
            this.textMask7.TextChanged += new System.EventHandler(this.OnMaskChanged);
            this.textMask7.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress_HexOnly);
            // 
            // textMask8
            // 
            this.textMask8.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textMask8.Location = new System.Drawing.Point(537, 180);
            this.textMask8.MaxLength = 2;
            this.textMask8.Name = "textMask8";
            this.textMask8.Size = new System.Drawing.Size(35, 21);
            this.textMask8.TabIndex = 31;
            this.textMask8.Text = "FF";
            this.textMask8.WordWrap = false;
            this.textMask8.TextChanged += new System.EventHandler(this.OnMaskChanged);
            this.textMask8.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress_HexOnly);
            // 
            // textMask9
            // 
            this.textMask9.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textMask9.Location = new System.Drawing.Point(583, 180);
            this.textMask9.MaxLength = 2;
            this.textMask9.Name = "textMask9";
            this.textMask9.Size = new System.Drawing.Size(35, 21);
            this.textMask9.TabIndex = 32;
            this.textMask9.Text = "FF";
            this.textMask9.WordWrap = false;
            this.textMask9.TextChanged += new System.EventHandler(this.OnMaskChanged);
            this.textMask9.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress_HexOnly);
            // 
            // textMask10
            // 
            this.textMask10.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textMask10.Location = new System.Drawing.Point(640, 180);
            this.textMask10.MaxLength = 2;
            this.textMask10.Name = "textMask10";
            this.textMask10.Size = new System.Drawing.Size(35, 21);
            this.textMask10.TabIndex = 33;
            this.textMask10.Text = "FF";
            this.textMask10.WordWrap = false;
            this.textMask10.TextChanged += new System.EventHandler(this.OnMaskChanged);
            this.textMask10.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress_HexOnly);
            // 
            // textMask11
            // 
            this.textMask11.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textMask11.Location = new System.Drawing.Point(686, 180);
            this.textMask11.MaxLength = 2;
            this.textMask11.Name = "textMask11";
            this.textMask11.Size = new System.Drawing.Size(35, 21);
            this.textMask11.TabIndex = 34;
            this.textMask11.Text = "FF";
            this.textMask11.WordWrap = false;
            this.textMask11.TextChanged += new System.EventHandler(this.OnMaskChanged);
            this.textMask11.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress_HexOnly);
            // 
            // textMask12
            // 
            this.textMask12.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textMask12.Location = new System.Drawing.Point(743, 180);
            this.textMask12.MaxLength = 2;
            this.textMask12.Name = "textMask12";
            this.textMask12.Size = new System.Drawing.Size(35, 21);
            this.textMask12.TabIndex = 35;
            this.textMask12.Text = "FF";
            this.textMask12.WordWrap = false;
            this.textMask12.TextChanged += new System.EventHandler(this.OnMaskChanged);
            this.textMask12.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress_HexOnly);
            // 
            // textMask13
            // 
            this.textMask13.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textMask13.Location = new System.Drawing.Point(789, 180);
            this.textMask13.MaxLength = 2;
            this.textMask13.Name = "textMask13";
            this.textMask13.Size = new System.Drawing.Size(35, 21);
            this.textMask13.TabIndex = 36;
            this.textMask13.Text = "FF";
            this.textMask13.WordWrap = false;
            this.textMask13.TextChanged += new System.EventHandler(this.OnMaskChanged);
            this.textMask13.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress_HexOnly);
            // 
            // textMask14
            // 
            this.textMask14.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textMask14.Location = new System.Drawing.Point(846, 180);
            this.textMask14.MaxLength = 2;
            this.textMask14.Name = "textMask14";
            this.textMask14.Size = new System.Drawing.Size(35, 21);
            this.textMask14.TabIndex = 37;
            this.textMask14.Text = "FF";
            this.textMask14.WordWrap = false;
            this.textMask14.TextChanged += new System.EventHandler(this.OnMaskChanged);
            this.textMask14.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress_HexOnly);
            // 
            // textMask15
            // 
            this.textMask15.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textMask15.Location = new System.Drawing.Point(892, 180);
            this.textMask15.MaxLength = 2;
            this.textMask15.Name = "textMask15";
            this.textMask15.Size = new System.Drawing.Size(35, 21);
            this.textMask15.TabIndex = 38;
            this.textMask15.Text = "FF";
            this.textMask15.WordWrap = false;
            this.textMask15.TextChanged += new System.EventHandler(this.OnMaskChanged);
            this.textMask15.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress_HexOnly);
            // 
            // btnResetMasks
            // 
            this.btnResetMasks.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnResetMasks.Location = new System.Drawing.Point(934, 175);
            this.btnResetMasks.Name = "btnResetMasks";
            this.btnResetMasks.Size = new System.Drawing.Size(143, 30);
            this.btnResetMasks.TabIndex = 39;
            this.btnResetMasks.Text = "&Reset Masks";
            this.btnResetMasks.UseVisualStyleBackColor = true;
            this.btnResetMasks.Click += new System.EventHandler(this.OnResetMasksClicked);
            // 
            // lblBhavInGroup
            // 
            this.lblBhavInGroup.AutoSize = true;
            this.lblBhavInGroup.Location = new System.Drawing.Point(10, 218);
            this.lblBhavInGroup.Name = "lblBhavInGroup";
            this.lblBhavInGroup.Size = new System.Drawing.Size(57, 15);
            this.lblBhavInGroup.TabIndex = 0;
            this.lblBhavInGroup.Text = "In Group:";
            // 
            // comboBhavInGroup
            // 
            this.comboBhavInGroup.FormattingEnabled = true;
            this.comboBhavInGroup.Location = new System.Drawing.Point(111, 215);
            this.comboBhavInGroup.Name = "comboBhavInGroup";
            this.comboBhavInGroup.Size = new System.Drawing.Size(369, 23);
            this.comboBhavInGroup.TabIndex = 40;
            this.comboBhavInGroup.TextChanged += new System.EventHandler(this.OnGroupChanged);
            this.comboBhavInGroup.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress_HexOnly);
            // 
            // lblOpCodeInGroup
            // 
            this.lblOpCodeInGroup.AutoSize = true;
            this.lblOpCodeInGroup.Location = new System.Drawing.Point(484, 218);
            this.lblOpCodeInGroup.Name = "lblOpCodeInGroup";
            this.lblOpCodeInGroup.Size = new System.Drawing.Size(68, 15);
            this.lblOpCodeInGroup.TabIndex = 0;
            this.lblOpCodeInGroup.Text = "OpCode In:";
            this.lblOpCodeInGroup.Visible = false;
            // 
            // comboOpCodeInGroup
            // 
            this.comboOpCodeInGroup.FormattingEnabled = true;
            this.comboOpCodeInGroup.Location = new System.Drawing.Point(553, 215);
            this.comboOpCodeInGroup.Name = "comboOpCodeInGroup";
            this.comboOpCodeInGroup.Size = new System.Drawing.Size(373, 23);
            this.comboOpCodeInGroup.TabIndex = 41;
            this.comboOpCodeInGroup.Visible = false;
            this.comboOpCodeInGroup.TextChanged += new System.EventHandler(this.OnGroupChanged);
            this.comboOpCodeInGroup.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress_HexOnly);
            // 
            // btnClearGroups
            // 
            this.btnClearGroups.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearGroups.Location = new System.Drawing.Point(934, 210);
            this.btnClearGroups.Name = "btnClearGroups";
            this.btnClearGroups.Size = new System.Drawing.Size(143, 30);
            this.btnClearGroups.TabIndex = 42;
            this.btnClearGroups.Text = "Clear &Groups";
            this.btnClearGroups.UseVisualStyleBackColor = true;
            this.btnClearGroups.Click += new System.EventHandler(this.OnClearGroupsClicked);
            // 
            // lblUsingOperand
            // 
            this.lblUsingOperand.AutoSize = true;
            this.lblUsingOperand.Location = new System.Drawing.Point(10, 253);
            this.lblUsingOperand.Name = "lblUsingOperand";
            this.lblUsingOperand.Size = new System.Drawing.Size(93, 15);
            this.lblUsingOperand.TabIndex = 0;
            this.lblUsingOperand.Text = "Using Operand:";
            // 
            // comboUsingOperand
            // 
            this.comboUsingOperand.FormattingEnabled = true;
            this.comboUsingOperand.Items.AddRange(new object[] {
            "",
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15"});
            this.comboUsingOperand.Location = new System.Drawing.Point(111, 250);
            this.comboUsingOperand.Name = "comboUsingOperand";
            this.comboUsingOperand.Size = new System.Drawing.Size(81, 23);
            this.comboUsingOperand.TabIndex = 43;
            this.comboUsingOperand.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress_Ignore);
            // 
            // lblUsingIndex
            // 
            this.lblUsingIndex.AutoSize = true;
            this.lblUsingIndex.Location = new System.Drawing.Point(199, 253);
            this.lblUsingIndex.Name = "lblUsingIndex";
            this.lblUsingIndex.Size = new System.Drawing.Size(113, 15);
            this.lblUsingIndex.TabIndex = 0;
            this.lblUsingIndex.Text = "As Index Into STR#:";
            // 
            // comboUsingSTR
            // 
            this.comboUsingSTR.FormattingEnabled = true;
            this.comboUsingSTR.Location = new System.Drawing.Point(321, 250);
            this.comboUsingSTR.Name = "comboUsingSTR";
            this.comboUsingSTR.Size = new System.Drawing.Size(159, 23);
            this.comboUsingSTR.TabIndex = 44;
            this.comboUsingSTR.TextChanged += new System.EventHandler(this.OnStrIndexChanged);
            this.comboUsingSTR.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress_HexOnly);
            // 
            // lblUsingMatches
            // 
            this.lblUsingMatches.AutoSize = true;
            this.lblUsingMatches.Location = new System.Drawing.Point(495, 253);
            this.lblUsingMatches.Name = "lblUsingMatches";
            this.lblUsingMatches.Size = new System.Drawing.Size(57, 15);
            this.lblUsingMatches.TabIndex = 0;
            this.lblUsingMatches.Text = "Matches:";
            // 
            // textUsingRegex
            // 
            this.textUsingRegex.Location = new System.Drawing.Point(553, 250);
            this.textUsingRegex.Name = "textUsingRegex";
            this.textUsingRegex.Size = new System.Drawing.Size(269, 21);
            this.textUsingRegex.TabIndex = 45;
            // 
            // btnUsingClear
            // 
            this.btnUsingClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUsingClear.Location = new System.Drawing.Point(934, 245);
            this.btnUsingClear.Name = "btnUsingClear";
            this.btnUsingClear.Size = new System.Drawing.Size(143, 30);
            this.btnUsingClear.TabIndex = 46;
            this.btnUsingClear.Text = "Clear &Matching";
            this.btnUsingClear.UseVisualStyleBackColor = true;
            this.btnUsingClear.Click += new System.EventHandler(this.OnClearMatchingClicked);
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Location = new System.Drawing.Point(10, 295);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(59, 15);
            this.lblProgress.TabIndex = 0;
            this.lblProgress.Text = "Progress:";
            this.lblProgress.Visible = false;
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(111, 292);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(605, 23);
            this.progressBar.TabIndex = 0;
            this.progressBar.Visible = false;
            // 
            // lblShowNames
            // 
            this.lblShowNames.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblShowNames.AutoSize = true;
            this.lblShowNames.Location = new System.Drawing.Point(745, 294);
            this.lblShowNames.Name = "lblShowNames";
            this.lblShowNames.Size = new System.Drawing.Size(159, 15);
            this.lblShowNames.TabIndex = 0;
            this.lblShowNames.Text = "Show Group/Object &Names:";
            // 
            // checkShowNames
            // 
            this.checkShowNames.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkShowNames.AutoSize = true;
            this.checkShowNames.Location = new System.Drawing.Point(910, 295);
            this.checkShowNames.Name = "checkShowNames";
            this.checkShowNames.Size = new System.Drawing.Size(15, 14);
            this.checkShowNames.TabIndex = 47;
            this.checkShowNames.UseVisualStyleBackColor = true;
            this.checkShowNames.CheckedChanged += new System.EventHandler(this.OnSwitchGroupChanged);
            // 
            // btnGO
            // 
            this.btnGO.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGO.Enabled = false;
            this.btnGO.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGO.Location = new System.Drawing.Point(934, 287);
            this.btnGO.Name = "btnGO";
            this.btnGO.Size = new System.Drawing.Size(143, 30);
            this.btnGO.TabIndex = 48;
            this.btnGO.Text = "FIND &BHAVs";
            this.btnGO.UseVisualStyleBackColor = true;
            this.btnGO.Click += new System.EventHandler(this.OnGoClicked);
            // 
            // gridFoundBhavs
            // 
            this.gridFoundBhavs.AllowUserToAddRows = false;
            this.gridFoundBhavs.AllowUserToDeleteRows = false;
            this.gridFoundBhavs.AllowUserToResizeRows = false;
            this.gridFoundBhavs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridFoundBhavs.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridFoundBhavs.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridFoundBhavs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridFoundBhavs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colBhavPackage,
            this.colBhavDbpfPath,
            this.colBhavDbpfEntry,
            this.colBhavInstance,
            this.colBhavName,
            this.colBhavGroupInstance,
            this.colBhavGroupName});
            this.gridFoundBhavs.ContextMenuStrip = this.menuContextFoundBhavs;
            this.gridFoundBhavs.Location = new System.Drawing.Point(14, 327);
            this.gridFoundBhavs.Name = "gridFoundBhavs";
            this.gridFoundBhavs.ReadOnly = true;
            this.gridFoundBhavs.RowHeadersVisible = false;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridFoundBhavs.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.gridFoundBhavs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridFoundBhavs.ShowCellErrors = false;
            this.gridFoundBhavs.ShowEditingIcon = false;
            this.gridFoundBhavs.Size = new System.Drawing.Size(1063, 321);
            this.gridFoundBhavs.TabIndex = 0;
            this.gridFoundBhavs.TabStop = false;
            // 
            // colBhavPackage
            // 
            this.colBhavPackage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.colBhavPackage.DataPropertyName = "Package";
            this.colBhavPackage.FillWeight = 5F;
            this.colBhavPackage.HeaderText = "Package";
            this.colBhavPackage.MaxInputLength = 10;
            this.colBhavPackage.Name = "colBhavPackage";
            this.colBhavPackage.ReadOnly = true;
            this.colBhavPackage.Visible = false;
            // 
            // colBhavDbpfPath
            // 
            this.colBhavDbpfPath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.colBhavDbpfPath.DataPropertyName = "DbpfPath";
            this.colBhavDbpfPath.FillWeight = 5F;
            this.colBhavDbpfPath.HeaderText = "Dbpf Path";
            this.colBhavDbpfPath.MaxInputLength = 6;
            this.colBhavDbpfPath.Name = "colBhavDbpfPath";
            this.colBhavDbpfPath.ReadOnly = true;
            this.colBhavDbpfPath.Visible = false;
            // 
            // colBhavDbpfEntry
            // 
            this.colBhavDbpfEntry.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.colBhavDbpfEntry.DataPropertyName = "DbpfEntry";
            this.colBhavDbpfEntry.FillWeight = 5F;
            this.colBhavDbpfEntry.HeaderText = "Dbpf Entry";
            this.colBhavDbpfEntry.MaxInputLength = 6;
            this.colBhavDbpfEntry.Name = "colBhavDbpfEntry";
            this.colBhavDbpfEntry.ReadOnly = true;
            this.colBhavDbpfEntry.Visible = false;
            // 
            // colBhavInstance
            // 
            this.colBhavInstance.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.colBhavInstance.DataPropertyName = "Instance";
            this.colBhavInstance.FillWeight = 5F;
            this.colBhavInstance.HeaderText = "Instance";
            this.colBhavInstance.MaxInputLength = 6;
            this.colBhavInstance.Name = "colBhavInstance";
            this.colBhavInstance.ReadOnly = true;
            this.colBhavInstance.Width = 78;
            // 
            // colBhavName
            // 
            this.colBhavName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colBhavName.DataPropertyName = "Name";
            this.colBhavName.HeaderText = "Name";
            this.colBhavName.MinimumWidth = 500;
            this.colBhavName.Name = "colBhavName";
            this.colBhavName.ReadOnly = true;
            // 
            // colBhavGroupInstance
            // 
            this.colBhavGroupInstance.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.colBhavGroupInstance.DataPropertyName = "GroupInstance";
            this.colBhavGroupInstance.FillWeight = 10F;
            this.colBhavGroupInstance.HeaderText = "Group";
            this.colBhavGroupInstance.MaxInputLength = 100;
            this.colBhavGroupInstance.Name = "colBhavGroupInstance";
            this.colBhavGroupInstance.ReadOnly = true;
            this.colBhavGroupInstance.Width = 66;
            // 
            // colBhavGroupName
            // 
            this.colBhavGroupName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.colBhavGroupName.DataPropertyName = "GroupName";
            this.colBhavGroupName.FillWeight = 15F;
            this.colBhavGroupName.HeaderText = "Object / Semi-Global";
            this.colBhavGroupName.MaxInputLength = 100;
            this.colBhavGroupName.Name = "colBhavGroupName";
            this.colBhavGroupName.ReadOnly = true;
            this.colBhavGroupName.Width = 145;
            // 
            // menuContextFoundBhavs
            // 
            this.menuContextFoundBhavs.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemExtract});
            this.menuContextFoundBhavs.Name = "menuContextOperands";
            this.menuContextFoundBhavs.Size = new System.Drawing.Size(155, 26);
            this.menuContextFoundBhavs.Opening += new System.ComponentModel.CancelEventHandler(this.OnContextFoundBhavsOpening);
            // 
            // menuItemExtract
            // 
            this.menuItemExtract.Name = "menuItemExtract";
            this.menuItemExtract.Size = new System.Drawing.Size(154, 22);
            this.menuItemExtract.Text = "SimPE Extract...";
            this.menuItemExtract.Click += new System.EventHandler(this.OnExtractBhavsClicked);
            // 
            // toolTipOperands
            // 
            this.toolTipOperands.AutoPopDelay = 5000;
            this.toolTipOperands.InitialDelay = 500;
            this.toolTipOperands.IsBalloon = true;
            this.toolTipOperands.ReshowDelay = 100;
            // 
            // menuMain
            // 
            this.menuMain.BackColor = System.Drawing.SystemColors.MenuBar;
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuHelp,
            this.menuMode,
            this.menuOptions});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuMain.Size = new System.Drawing.Size(1091, 24);
            this.menuMain.TabIndex = 1;
            this.menuMain.Text = "menuStrip";
            // 
            // menuFile
            // 
            this.menuFile.BackColor = System.Drawing.SystemColors.Menu;
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemSelectPackage,
            this.menuItemSelectFolder,
            this.menuItemRecentPackages,
            this.menuItemSeparator3,
            this.menuItemSaveResultsToClipboard,
            this.menuItemSaveResultsAs,
            this.menuItemSeparator1,
            this.menuItemConfiguration,
            this.menuItemSeparator2,
            this.menuItemExit});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(37, 20);
            this.menuFile.Text = "&File";
            this.menuFile.DropDownOpening += new System.EventHandler(this.OnFileOpening);
            // 
            // menuItemSelectPackage
            // 
            this.menuItemSelectPackage.Name = "menuItemSelectPackage";
            this.menuItemSelectPackage.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.menuItemSelectPackage.Size = new System.Drawing.Size(249, 22);
            this.menuItemSelectPackage.Text = "&Select Package...";
            this.menuItemSelectPackage.Click += new System.EventHandler(this.OnSelectClicked);
            // 
            // menuItemSelectFolder
            // 
            this.menuItemSelectFolder.Name = "menuItemSelectFolder";
            this.menuItemSelectFolder.Size = new System.Drawing.Size(249, 22);
            this.menuItemSelectFolder.Text = "Select Package Folder...";
            this.menuItemSelectFolder.Click += new System.EventHandler(this.OnSelectFolderClicked);
            // 
            // menuItemRecentPackages
            // 
            this.menuItemRecentPackages.Name = "menuItemRecentPackages";
            this.menuItemRecentPackages.Size = new System.Drawing.Size(249, 22);
            this.menuItemRecentPackages.Text = "Recent Packages...";
            // 
            // menuItemSeparator3
            // 
            this.menuItemSeparator3.Name = "menuItemSeparator3";
            this.menuItemSeparator3.Size = new System.Drawing.Size(246, 6);
            // 
            // menuItemSaveResultsToClipboard
            // 
            this.menuItemSaveResultsToClipboard.Name = "menuItemSaveResultsToClipboard";
            this.menuItemSaveResultsToClipboard.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.menuItemSaveResultsToClipboard.Size = new System.Drawing.Size(249, 22);
            this.menuItemSaveResultsToClipboard.Text = "Save Results To &Clipboard";
            this.menuItemSaveResultsToClipboard.Click += new System.EventHandler(this.OnSaveResultsToClipboardClicked);
            // 
            // menuItemSaveResultsAs
            // 
            this.menuItemSaveResultsAs.Name = "menuItemSaveResultsAs";
            this.menuItemSaveResultsAs.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.S)));
            this.menuItemSaveResultsAs.Size = new System.Drawing.Size(249, 22);
            this.menuItemSaveResultsAs.Text = "Save Results &As...";
            this.menuItemSaveResultsAs.Click += new System.EventHandler(this.OnSaveResultsAsClicked);
            // 
            // menuItemSeparator1
            // 
            this.menuItemSeparator1.Name = "menuItemSeparator1";
            this.menuItemSeparator1.Size = new System.Drawing.Size(246, 6);
            // 
            // menuItemConfiguration
            // 
            this.menuItemConfiguration.Name = "menuItemConfiguration";
            this.menuItemConfiguration.Size = new System.Drawing.Size(249, 22);
            this.menuItemConfiguration.Text = "Configuration...";
            this.menuItemConfiguration.Click += new System.EventHandler(this.OnConfigClicked);
            // 
            // menuItemSeparator2
            // 
            this.menuItemSeparator2.Name = "menuItemSeparator2";
            this.menuItemSeparator2.Size = new System.Drawing.Size(246, 6);
            // 
            // menuItemExit
            // 
            this.menuItemExit.Name = "menuItemExit";
            this.menuItemExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.menuItemExit.Size = new System.Drawing.Size(249, 22);
            this.menuItemExit.Text = "E&xit";
            this.menuItemExit.Click += new System.EventHandler(this.OnExitClicked);
            // 
            // menuHelp
            // 
            this.menuHelp.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.menuHelp.BackColor = System.Drawing.SystemColors.Menu;
            this.menuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemAbout});
            this.menuHelp.Name = "menuHelp";
            this.menuHelp.Size = new System.Drawing.Size(44, 20);
            this.menuHelp.Text = "&Help";
            // 
            // menuItemAbout
            // 
            this.menuItemAbout.Name = "menuItemAbout";
            this.menuItemAbout.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.menuItemAbout.Size = new System.Drawing.Size(135, 22);
            this.menuItemAbout.Text = "About...";
            this.menuItemAbout.Click += new System.EventHandler(this.OnHelpClicked);
            // 
            // menuMode
            // 
            this.menuMode.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemAdvanced});
            this.menuMode.Name = "menuMode";
            this.menuMode.Size = new System.Drawing.Size(50, 20);
            this.menuMode.Text = "Mode";
            this.menuMode.DropDownOpening += new System.EventHandler(this.OnModeOpening);
            // 
            // menuItemAdvanced
            // 
            this.menuItemAdvanced.CheckOnClick = true;
            this.menuItemAdvanced.Name = "menuItemAdvanced";
            this.menuItemAdvanced.Size = new System.Drawing.Size(127, 22);
            this.menuItemAdvanced.Text = "Advanced";
            this.menuItemAdvanced.Click += new System.EventHandler(this.OnAdvancedModeChanged);
            // 
            // menuOptions
            // 
            this.menuOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemRestoreFilters});
            this.menuOptions.Name = "menuOptions";
            this.menuOptions.Size = new System.Drawing.Size(61, 20);
            this.menuOptions.Text = "&Options";
            // 
            // menuItemRestoreFilters
            // 
            this.menuItemRestoreFilters.CheckOnClick = true;
            this.menuItemRestoreFilters.Name = "menuItemRestoreFilters";
            this.menuItemRestoreFilters.Size = new System.Drawing.Size(200, 22);
            this.menuItemRestoreFilters.Text = "Restore Last Used &Filters";
            // 
            // saveResultsDialog
            // 
            this.saveResultsDialog.DefaultExt = "txt";
            this.saveResultsDialog.Filter = "Text files|*.txt|All files|*.*";
            this.saveResultsDialog.Title = "Save Results As...";
            // 
            // bhavFinderWorker
            // 
            this.bhavFinderWorker.WorkerReportsProgress = true;
            this.bhavFinderWorker.WorkerSupportsCancellation = true;
            this.bhavFinderWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BhavFinderWorker_DoWork);
            this.bhavFinderWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.BhavFinderWorker_Progress);
            this.bhavFinderWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BhavFinderWorker_Completed);
            // 
            // lblUsingIgnoreCase
            // 
            this.lblUsingIgnoreCase.AutoSize = true;
            this.lblUsingIgnoreCase.Location = new System.Drawing.Point(828, 253);
            this.lblUsingIgnoreCase.Name = "lblUsingIgnoreCase";
            this.lblUsingIgnoreCase.Size = new System.Drawing.Size(76, 15);
            this.lblUsingIgnoreCase.TabIndex = 49;
            this.lblUsingIgnoreCase.Text = "Ignore Case:";
            // 
            // checkUsingIgnoreCase
            // 
            this.checkUsingIgnoreCase.AutoSize = true;
            this.checkUsingIgnoreCase.Location = new System.Drawing.Point(910, 254);
            this.checkUsingIgnoreCase.Name = "checkUsingIgnoreCase";
            this.checkUsingIgnoreCase.Size = new System.Drawing.Size(15, 14);
            this.checkUsingIgnoreCase.TabIndex = 50;
            this.checkUsingIgnoreCase.UseVisualStyleBackColor = true;
            this.checkUsingIgnoreCase.CheckedChanged += new System.EventHandler(this.OnUsingIgnoreCaseChanged);
            // 
            // lblNameRegex
            // 
            this.lblNameRegex.AutoSize = true;
            this.lblNameRegex.Location = new System.Drawing.Point(10, 76);
            this.lblNameRegex.Name = "lblNameRegex";
            this.lblNameRegex.Size = new System.Drawing.Size(86, 15);
            this.lblNameRegex.TabIndex = 53;
            this.lblNameRegex.Text = "Name (regex):";
            // 
            // textNameRegex
            // 
            this.textNameRegex.ContextMenuStrip = this.menuContextOperands;
            this.textNameRegex.Location = new System.Drawing.Point(111, 73);
            this.textNameRegex.Name = "textNameRegex";
            this.textNameRegex.Size = new System.Drawing.Size(245, 21);
            this.textNameRegex.TabIndex = 54;
            this.textNameRegex.WordWrap = false;
            this.textNameRegex.TextChanged += new System.EventHandler(this.OnBhavDetailsChanged);
            // 
            // btnClearBhavDetails
            // 
            this.btnClearBhavDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearBhavDetails.Location = new System.Drawing.Point(934, 68);
            this.btnClearBhavDetails.Name = "btnClearBhavDetails";
            this.btnClearBhavDetails.Size = new System.Drawing.Size(143, 30);
            this.btnClearBhavDetails.TabIndex = 55;
            this.btnClearBhavDetails.Text = "Clear &BHAV Details";
            this.btnClearBhavDetails.UseVisualStyleBackColor = true;
            this.btnClearBhavDetails.Click += new System.EventHandler(this.OnClearBhavDetailsClicked);
            // 
            // lblParams
            // 
            this.lblParams.AutoSize = true;
            this.lblParams.Location = new System.Drawing.Point(533, 76);
            this.lblParams.Name = "lblParams";
            this.lblParams.Size = new System.Drawing.Size(53, 15);
            this.lblParams.TabIndex = 56;
            this.lblParams.Text = "Params:";
            // 
            // textParams
            // 
            this.textParams.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textParams.ContextMenuStrip = this.menuContextOperands;
            this.textParams.Location = new System.Drawing.Point(590, 73);
            this.textParams.MaxLength = 2;
            this.textParams.Name = "textParams";
            this.textParams.Size = new System.Drawing.Size(81, 21);
            this.textParams.TabIndex = 57;
            this.textParams.WordWrap = false;
            this.textParams.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress_DecimalOnly);
            // 
            // textLocals
            // 
            this.textLocals.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textLocals.ContextMenuStrip = this.menuContextOperands;
            this.textLocals.Location = new System.Drawing.Point(741, 73);
            this.textLocals.MaxLength = 2;
            this.textLocals.Name = "textLocals";
            this.textLocals.Size = new System.Drawing.Size(81, 21);
            this.textLocals.TabIndex = 59;
            this.textLocals.WordWrap = false;
            this.textLocals.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress_DecimalOnly);
            // 
            // lblLocals
            // 
            this.lblLocals.AutoSize = true;
            this.lblLocals.Location = new System.Drawing.Point(684, 76);
            this.lblLocals.Name = "lblLocals";
            this.lblLocals.Size = new System.Drawing.Size(46, 15);
            this.lblLocals.TabIndex = 58;
            this.lblLocals.Text = "Locals:";
            // 
            // checkNameCaseSensitive
            // 
            this.checkNameCaseSensitive.AutoSize = true;
            this.checkNameCaseSensitive.Location = new System.Drawing.Point(486, 77);
            this.checkNameCaseSensitive.Name = "checkNameCaseSensitive";
            this.checkNameCaseSensitive.Size = new System.Drawing.Size(15, 14);
            this.checkNameCaseSensitive.TabIndex = 61;
            this.checkNameCaseSensitive.UseVisualStyleBackColor = true;
            // 
            // lblNameCaseSensitive
            // 
            this.lblNameCaseSensitive.AutoSize = true;
            this.lblNameCaseSensitive.Location = new System.Drawing.Point(392, 76);
            this.lblNameCaseSensitive.Name = "lblNameCaseSensitive";
            this.lblNameCaseSensitive.Size = new System.Drawing.Size(90, 15);
            this.lblNameCaseSensitive.TabIndex = 60;
            this.lblNameCaseSensitive.Text = "Case Sensitive:";
            // 
            // lblOperand0
            // 
            this.lblOperand0.AutoSize = true;
            this.lblOperand0.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOperand0.ForeColor = System.Drawing.Color.Teal;
            this.lblOperand0.Location = new System.Drawing.Point(108, 133);
            this.lblOperand0.Name = "lblOperand0";
            this.lblOperand0.Size = new System.Drawing.Size(13, 13);
            this.lblOperand0.TabIndex = 64;
            this.lblOperand0.Text = "0";
            // 
            // lblOperand2
            // 
            this.lblOperand2.AutoSize = true;
            this.lblOperand2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOperand2.ForeColor = System.Drawing.Color.Teal;
            this.lblOperand2.Location = new System.Drawing.Point(211, 133);
            this.lblOperand2.Name = "lblOperand2";
            this.lblOperand2.Size = new System.Drawing.Size(13, 13);
            this.lblOperand2.TabIndex = 65;
            this.lblOperand2.Text = "2";
            // 
            // lblOperand4
            // 
            this.lblOperand4.AutoSize = true;
            this.lblOperand4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOperand4.ForeColor = System.Drawing.Color.Teal;
            this.lblOperand4.Location = new System.Drawing.Point(314, 133);
            this.lblOperand4.Name = "lblOperand4";
            this.lblOperand4.Size = new System.Drawing.Size(13, 13);
            this.lblOperand4.TabIndex = 66;
            this.lblOperand4.Text = "4";
            // 
            // lblOperand6
            // 
            this.lblOperand6.AutoSize = true;
            this.lblOperand6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOperand6.ForeColor = System.Drawing.Color.Teal;
            this.lblOperand6.Location = new System.Drawing.Point(417, 133);
            this.lblOperand6.Name = "lblOperand6";
            this.lblOperand6.Size = new System.Drawing.Size(13, 13);
            this.lblOperand6.TabIndex = 67;
            this.lblOperand6.Text = "6";
            // 
            // lblOperand8
            // 
            this.lblOperand8.AutoSize = true;
            this.lblOperand8.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOperand8.ForeColor = System.Drawing.Color.Teal;
            this.lblOperand8.Location = new System.Drawing.Point(534, 133);
            this.lblOperand8.Name = "lblOperand8";
            this.lblOperand8.Size = new System.Drawing.Size(13, 13);
            this.lblOperand8.TabIndex = 68;
            this.lblOperand8.Text = "8";
            // 
            // lblOperand10
            // 
            this.lblOperand10.AutoSize = true;
            this.lblOperand10.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOperand10.ForeColor = System.Drawing.Color.Teal;
            this.lblOperand10.Location = new System.Drawing.Point(637, 133);
            this.lblOperand10.Name = "lblOperand10";
            this.lblOperand10.Size = new System.Drawing.Size(19, 13);
            this.lblOperand10.TabIndex = 69;
            this.lblOperand10.Text = "10";
            // 
            // lblOperand12
            // 
            this.lblOperand12.AutoSize = true;
            this.lblOperand12.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOperand12.ForeColor = System.Drawing.Color.Teal;
            this.lblOperand12.Location = new System.Drawing.Point(740, 133);
            this.lblOperand12.Name = "lblOperand12";
            this.lblOperand12.Size = new System.Drawing.Size(19, 13);
            this.lblOperand12.TabIndex = 70;
            this.lblOperand12.Text = "12";
            // 
            // lblOperand14
            // 
            this.lblOperand14.AutoSize = true;
            this.lblOperand14.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOperand14.ForeColor = System.Drawing.Color.Teal;
            this.lblOperand14.Location = new System.Drawing.Point(843, 133);
            this.lblOperand14.Name = "lblOperand14";
            this.lblOperand14.Size = new System.Drawing.Size(19, 13);
            this.lblOperand14.TabIndex = 71;
            this.lblOperand14.Text = "14";
            // 
            // BhavFinderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1091, 661);
            this.Controls.Add(this.lblOperand14);
            this.Controls.Add(this.lblOperand12);
            this.Controls.Add(this.lblOperand10);
            this.Controls.Add(this.lblOperand8);
            this.Controls.Add(this.lblOperand6);
            this.Controls.Add(this.lblOperand4);
            this.Controls.Add(this.lblOperand2);
            this.Controls.Add(this.lblOperand0);
            this.Controls.Add(this.checkNameCaseSensitive);
            this.Controls.Add(this.lblNameCaseSensitive);
            this.Controls.Add(this.textLocals);
            this.Controls.Add(this.lblLocals);
            this.Controls.Add(this.textParams);
            this.Controls.Add(this.lblParams);
            this.Controls.Add(this.lblNameRegex);
            this.Controls.Add(this.textNameRegex);
            this.Controls.Add(this.btnClearBhavDetails);
            this.Controls.Add(this.checkUsingIgnoreCase);
            this.Controls.Add(this.lblUsingIgnoreCase);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.lblFilePath);
            this.Controls.Add(this.textFilePath);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.lblOpCode);
            this.Controls.Add(this.comboOpCode);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.comboVersion);
            this.Controls.Add(this.btnClearOpCode);
            this.Controls.Add(this.lblOperands);
            this.Controls.Add(this.textOperand0);
            this.Controls.Add(this.textOperand1);
            this.Controls.Add(this.textOperand2);
            this.Controls.Add(this.textOperand3);
            this.Controls.Add(this.textOperand4);
            this.Controls.Add(this.textOperand5);
            this.Controls.Add(this.textOperand6);
            this.Controls.Add(this.textOperand7);
            this.Controls.Add(this.textOperand8);
            this.Controls.Add(this.textOperand9);
            this.Controls.Add(this.textOperand10);
            this.Controls.Add(this.textOperand11);
            this.Controls.Add(this.textOperand12);
            this.Controls.Add(this.textOperand13);
            this.Controls.Add(this.textOperand14);
            this.Controls.Add(this.textOperand15);
            this.Controls.Add(this.btnClearOperands);
            this.Controls.Add(this.lblMasks);
            this.Controls.Add(this.textMask0);
            this.Controls.Add(this.textMask1);
            this.Controls.Add(this.textMask2);
            this.Controls.Add(this.textMask3);
            this.Controls.Add(this.textMask4);
            this.Controls.Add(this.textMask5);
            this.Controls.Add(this.textMask6);
            this.Controls.Add(this.textMask7);
            this.Controls.Add(this.textMask8);
            this.Controls.Add(this.textMask9);
            this.Controls.Add(this.textMask10);
            this.Controls.Add(this.textMask11);
            this.Controls.Add(this.textMask12);
            this.Controls.Add(this.textMask13);
            this.Controls.Add(this.textMask14);
            this.Controls.Add(this.textMask15);
            this.Controls.Add(this.btnResetMasks);
            this.Controls.Add(this.lblBhavInGroup);
            this.Controls.Add(this.comboBhavInGroup);
            this.Controls.Add(this.lblOpCodeInGroup);
            this.Controls.Add(this.comboOpCodeInGroup);
            this.Controls.Add(this.btnClearGroups);
            this.Controls.Add(this.lblUsingOperand);
            this.Controls.Add(this.comboUsingOperand);
            this.Controls.Add(this.lblUsingIndex);
            this.Controls.Add(this.comboUsingSTR);
            this.Controls.Add(this.lblUsingMatches);
            this.Controls.Add(this.textUsingRegex);
            this.Controls.Add(this.btnUsingClear);
            this.Controls.Add(this.lblShowNames);
            this.Controls.Add(this.checkShowNames);
            this.Controls.Add(this.btnGO);
            this.Controls.Add(this.gridFoundBhavs);
            this.Controls.Add(this.menuMain);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuMain;
            this.MinimumSize = new System.Drawing.Size(1107, 700);
            this.Name = "BhavFinderForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.OnLoad);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.menuContextOperands.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridFoundBhavs)).EndInit();
            this.menuContextFoundBhavs.ResumeLayout(false);
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblFilePath;
        private System.Windows.Forms.TextBox textFilePath;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Label lblOpCode;
        private System.Windows.Forms.ComboBox comboOpCode;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.ComboBox comboVersion;
        private System.Windows.Forms.Button btnClearOpCode;
        private System.Windows.Forms.Label lblOperands;
        private System.Windows.Forms.TextBox textOperand0;
        private System.Windows.Forms.TextBox textOperand1;
        private System.Windows.Forms.TextBox textOperand2;
        private System.Windows.Forms.TextBox textOperand3;
        private System.Windows.Forms.TextBox textOperand4;
        private System.Windows.Forms.TextBox textOperand5;
        private System.Windows.Forms.TextBox textOperand6;
        private System.Windows.Forms.TextBox textOperand7;
        private System.Windows.Forms.TextBox textOperand8;
        private System.Windows.Forms.TextBox textOperand9;
        private System.Windows.Forms.TextBox textOperand10;
        private System.Windows.Forms.TextBox textOperand11;
        private System.Windows.Forms.TextBox textOperand12;
        private System.Windows.Forms.TextBox textOperand13;
        private System.Windows.Forms.TextBox textOperand14;
        private System.Windows.Forms.TextBox textOperand15;
        private System.Windows.Forms.Button btnClearOperands;
        private System.Windows.Forms.Label lblMasks;
        private System.Windows.Forms.TextBox textMask0;
        private System.Windows.Forms.TextBox textMask1;
        private System.Windows.Forms.TextBox textMask2;
        private System.Windows.Forms.TextBox textMask3;
        private System.Windows.Forms.TextBox textMask4;
        private System.Windows.Forms.TextBox textMask5;
        private System.Windows.Forms.TextBox textMask6;
        private System.Windows.Forms.TextBox textMask7;
        private System.Windows.Forms.TextBox textMask8;
        private System.Windows.Forms.TextBox textMask9;
        private System.Windows.Forms.TextBox textMask10;
        private System.Windows.Forms.TextBox textMask11;
        private System.Windows.Forms.TextBox textMask12;
        private System.Windows.Forms.TextBox textMask13;
        private System.Windows.Forms.TextBox textMask14;
        private System.Windows.Forms.TextBox textMask15;
        private System.Windows.Forms.Button btnResetMasks;
        private System.Windows.Forms.Label lblBhavInGroup;
        private System.Windows.Forms.ComboBox comboBhavInGroup;
        private System.Windows.Forms.Label lblOpCodeInGroup;
        private System.Windows.Forms.ComboBox comboOpCodeInGroup;
        private System.Windows.Forms.Button btnClearGroups;
        private System.Windows.Forms.Label lblUsingOperand;
        private System.Windows.Forms.ComboBox comboUsingOperand;
        private System.Windows.Forms.Label lblUsingIndex;
        private System.Windows.Forms.ComboBox comboUsingSTR;
        private System.Windows.Forms.Label lblUsingMatches;
        private System.Windows.Forms.TextBox textUsingRegex;
        private System.Windows.Forms.Button btnUsingClear;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblShowNames;
        private System.Windows.Forms.CheckBox checkShowNames;
        private System.Windows.Forms.Button btnGO;
        private System.Windows.Forms.DataGridView gridFoundBhavs;
        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuItemSelectPackage;
        private System.Windows.Forms.ToolStripMenuItem menuItemRecentPackages;
        private System.Windows.Forms.ToolStripSeparator menuItemSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuItemConfiguration;
        private System.Windows.Forms.ToolStripSeparator menuItemSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuItemSaveResultsToClipboard;
        private System.Windows.Forms.ToolStripMenuItem menuItemSaveResultsAs;
        private System.Windows.Forms.ToolStripSeparator menuItemSeparator3;
        private System.Windows.Forms.ToolStripMenuItem menuItemExit;
        private System.Windows.Forms.ToolStripMenuItem menuHelp;
        private System.Windows.Forms.ToolStripMenuItem menuItemAbout;
        private System.Windows.Forms.ContextMenuStrip menuContextOperands;
        private System.Windows.Forms.ToolStripMenuItem menuItemPasteGUID;
        private System.Windows.Forms.ToolTip toolTipOperands;
        private System.Windows.Forms.OpenFileDialog selectFileDialog;
        private System.Windows.Forms.SaveFileDialog saveResultsDialog;
        private System.ComponentModel.BackgroundWorker bhavFinderWorker;
        private System.Windows.Forms.ContextMenuStrip menuContextFoundBhavs;
        private System.Windows.Forms.ToolStripMenuItem menuItemExtract;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBhavPackage;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBhavDbpfPath;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBhavDbpfEntry;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBhavInstance;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBhavName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBhavGroupInstance;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBhavGroupName;
        private System.Windows.Forms.ToolStripMenuItem menuItemSelectFolder;
        private System.Windows.Forms.Label lblUsingIgnoreCase;
        private System.Windows.Forms.CheckBox checkUsingIgnoreCase;
        private System.Windows.Forms.ToolStripMenuItem menuOptions;
        private System.Windows.Forms.ToolStripMenuItem menuItemRestoreFilters;
        private System.Windows.Forms.Label lblNameRegex;
        private System.Windows.Forms.TextBox textNameRegex;
        private System.Windows.Forms.Button btnClearBhavDetails;
        private System.Windows.Forms.Label lblParams;
        private System.Windows.Forms.TextBox textParams;
        private System.Windows.Forms.TextBox textLocals;
        private System.Windows.Forms.Label lblLocals;
        private System.Windows.Forms.CheckBox checkNameCaseSensitive;
        private System.Windows.Forms.Label lblNameCaseSensitive;
        private System.Windows.Forms.ToolStripMenuItem menuItemPasteSimPeOperands;
        private System.Windows.Forms.ToolStripMenuItem menuMode;
        private System.Windows.Forms.ToolStripMenuItem menuItemAdvanced;
        private System.Windows.Forms.Label lblOperand0;
        private System.Windows.Forms.Label lblOperand2;
        private System.Windows.Forms.Label lblOperand4;
        private System.Windows.Forms.Label lblOperand6;
        private System.Windows.Forms.Label lblOperand8;
        private System.Windows.Forms.Label lblOperand10;
        private System.Windows.Forms.Label lblOperand12;
        private System.Windows.Forms.Label lblOperand14;
    }
}

