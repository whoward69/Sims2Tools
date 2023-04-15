/*
 * DBPF Viewer - a utility for testing the DBPF Library
 *
 * William Howard - 2020-2023
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

namespace DbpfViewer
{
    partial class DbpfViewerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DbpfViewerForm));
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemReloadPackage = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemSelectPackage = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemRecentPackages = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemSaveXmlToClipboard = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSaveXmlAs = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemConfiguration = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuResources = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAll = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemNone = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemBcon = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemBhav = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemCtss = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemGlob = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemImg = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemJpg = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemObjd = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemObjf = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemNref = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSlot = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemStr = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemTprp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemTrcn = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemTtab = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemTtas = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemVers = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemPrettyPrint = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.selectFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.gridResources = new System.Windows.Forms.DataGridView();
            this.colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGroup = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInstance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHash = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuContextGrid = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemSaveRawData = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemSaveAsJpeg = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSaveAsPng = new System.Windows.Forms.ToolStripMenuItem();
            this.saveXmlDialog = new System.Windows.Forms.SaveFileDialog();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.panelImage = new System.Windows.Forms.Panel();
            this.pictImage = new System.Windows.Forms.PictureBox();
            this.menuContextImage = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemCopyImage = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemSaveJpeg = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSavePng = new System.Windows.Forms.ToolStripMenuItem();
            this.textXml = new System.Windows.Forms.TextBox();
            this.saveRawDialog = new System.Windows.Forms.SaveFileDialog();
            this.menuMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridResources)).BeginInit();
            this.menuContextGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.panelImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictImage)).BeginInit();
            this.menuContextImage.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuResources,
            this.menuHelp,
            this.menuOptions});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuMain.Size = new System.Drawing.Size(933, 24);
            this.menuMain.TabIndex = 0;
            this.menuMain.Text = "menuStrip";
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemReloadPackage,
            this.menuItemSeparator1,
            this.menuItemSelectPackage,
            this.menuItemRecentPackages,
            this.menuItemSeparator2,
            this.menuItemSaveXmlToClipboard,
            this.menuItemSaveXmlAs,
            this.menuItemSeparator3,
            this.menuItemConfiguration,
            this.toolStripSeparator1,
            this.menuItemExit});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(37, 20);
            this.menuFile.Text = "&File";
            this.menuFile.DropDownOpening += new System.EventHandler(this.OnFileOpening);
            // 
            // menuItemReloadPackage
            // 
            this.menuItemReloadPackage.Name = "menuItemReloadPackage";
            this.menuItemReloadPackage.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.menuItemReloadPackage.Size = new System.Drawing.Size(235, 22);
            this.menuItemReloadPackage.Text = "&Reload Package";
            this.menuItemReloadPackage.Click += new System.EventHandler(this.OnReloadClicked);
            // 
            // menuItemSeparator1
            // 
            this.menuItemSeparator1.Name = "menuItemSeparator1";
            this.menuItemSeparator1.Size = new System.Drawing.Size(232, 6);
            // 
            // menuItemSelectPackage
            // 
            this.menuItemSelectPackage.Name = "menuItemSelectPackage";
            this.menuItemSelectPackage.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.menuItemSelectPackage.Size = new System.Drawing.Size(235, 22);
            this.menuItemSelectPackage.Text = "&Select Package...";
            this.menuItemSelectPackage.Click += new System.EventHandler(this.OnSelectClicked);
            // 
            // menuItemRecentPackages
            // 
            this.menuItemRecentPackages.Name = "menuItemRecentPackages";
            this.menuItemRecentPackages.Size = new System.Drawing.Size(235, 22);
            this.menuItemRecentPackages.Text = "Recent Packages...";
            // 
            // menuItemSeparator2
            // 
            this.menuItemSeparator2.Name = "menuItemSeparator2";
            this.menuItemSeparator2.Size = new System.Drawing.Size(232, 6);
            // 
            // menuItemSaveXmlToClipboard
            // 
            this.menuItemSaveXmlToClipboard.Name = "menuItemSaveXmlToClipboard";
            this.menuItemSaveXmlToClipboard.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.menuItemSaveXmlToClipboard.Size = new System.Drawing.Size(235, 22);
            this.menuItemSaveXmlToClipboard.Text = "Save XML To &Clipboard";
            this.menuItemSaveXmlToClipboard.Click += new System.EventHandler(this.OnSaveXmlToClipboardClicked);
            // 
            // menuItemSaveXmlAs
            // 
            this.menuItemSaveXmlAs.Name = "menuItemSaveXmlAs";
            this.menuItemSaveXmlAs.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.S)));
            this.menuItemSaveXmlAs.Size = new System.Drawing.Size(235, 22);
            this.menuItemSaveXmlAs.Text = "Save XML &As...";
            this.menuItemSaveXmlAs.Click += new System.EventHandler(this.OnSaveXmlAsClicked);
            // 
            // menuItemSeparator3
            // 
            this.menuItemSeparator3.Name = "menuItemSeparator3";
            this.menuItemSeparator3.Size = new System.Drawing.Size(232, 6);
            // 
            // menuItemConfiguration
            // 
            this.menuItemConfiguration.Name = "menuItemConfiguration";
            this.menuItemConfiguration.Size = new System.Drawing.Size(235, 22);
            this.menuItemConfiguration.Text = "Configuration...";
            this.menuItemConfiguration.Click += new System.EventHandler(this.OnConfigurationClicked);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(232, 6);
            // 
            // menuItemExit
            // 
            this.menuItemExit.Name = "menuItemExit";
            this.menuItemExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.menuItemExit.Size = new System.Drawing.Size(235, 22);
            this.menuItemExit.Text = "E&xit";
            this.menuItemExit.Click += new System.EventHandler(this.OnExitClicked);
            // 
            // menuResources
            // 
            this.menuResources.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemAll,
            this.menuItemNone,
            this.toolStripSeparator2,
            this.menuItemBcon,
            this.menuItemBhav,
            this.menuItemCtss,
            this.menuItemGlob,
            this.menuItemImg,
            this.menuItemJpg,
            this.menuItemObjd,
            this.menuItemObjf,
            this.menuItemNref,
            this.menuItemSlot,
            this.menuItemStr,
            this.menuItemTprp,
            this.menuItemTrcn,
            this.menuItemTtab,
            this.menuItemTtas,
            this.menuItemVers});
            this.menuResources.Name = "menuResources";
            this.menuResources.Size = new System.Drawing.Size(72, 20);
            this.menuResources.Text = "&Resources";
            // 
            // menuItemAll
            // 
            this.menuItemAll.Name = "menuItemAll";
            this.menuItemAll.Size = new System.Drawing.Size(103, 22);
            this.menuItemAll.Text = "&All";
            this.menuItemAll.Click += new System.EventHandler(this.OnAllClicked);
            // 
            // menuItemNone
            // 
            this.menuItemNone.Name = "menuItemNone";
            this.menuItemNone.Size = new System.Drawing.Size(103, 22);
            this.menuItemNone.Text = "&None";
            this.menuItemNone.Click += new System.EventHandler(this.OnNoneClicked);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(100, 6);
            // 
            // menuItemBcon
            // 
            this.menuItemBcon.CheckOnClick = true;
            this.menuItemBcon.Name = "menuItemBcon";
            this.menuItemBcon.Size = new System.Drawing.Size(103, 22);
            this.menuItemBcon.Text = "Bcon";
            this.menuItemBcon.Click += new System.EventHandler(this.OnBconClicked);
            // 
            // menuItemBhav
            // 
            this.menuItemBhav.CheckOnClick = true;
            this.menuItemBhav.Name = "menuItemBhav";
            this.menuItemBhav.Size = new System.Drawing.Size(103, 22);
            this.menuItemBhav.Text = "Bhav";
            this.menuItemBhav.Click += new System.EventHandler(this.OnBhavClicked);
            // 
            // menuItemCtss
            // 
            this.menuItemCtss.CheckOnClick = true;
            this.menuItemCtss.Name = "menuItemCtss";
            this.menuItemCtss.Size = new System.Drawing.Size(103, 22);
            this.menuItemCtss.Text = "Ctss";
            this.menuItemCtss.Click += new System.EventHandler(this.OnCtssClicked);
            // 
            // menuItemGlob
            // 
            this.menuItemGlob.CheckOnClick = true;
            this.menuItemGlob.Name = "menuItemGlob";
            this.menuItemGlob.Size = new System.Drawing.Size(103, 22);
            this.menuItemGlob.Text = "Glob";
            this.menuItemGlob.Click += new System.EventHandler(this.OnGlobClicked);
            // 
            // menuItemImg
            // 
            this.menuItemImg.CheckOnClick = true;
            this.menuItemImg.Name = "menuItemImg";
            this.menuItemImg.Size = new System.Drawing.Size(103, 22);
            this.menuItemImg.Text = "Img";
            this.menuItemImg.Click += new System.EventHandler(this.OnImgClicked);
            // 
            // menuItemJpg
            // 
            this.menuItemJpg.CheckOnClick = true;
            this.menuItemJpg.Name = "menuItemJpg";
            this.menuItemJpg.Size = new System.Drawing.Size(103, 22);
            this.menuItemJpg.Text = "Jpg";
            this.menuItemJpg.Click += new System.EventHandler(this.OnJpgClicked);
            // 
            // menuItemObjd
            // 
            this.menuItemObjd.CheckOnClick = true;
            this.menuItemObjd.Name = "menuItemObjd";
            this.menuItemObjd.Size = new System.Drawing.Size(103, 22);
            this.menuItemObjd.Text = "Objd";
            this.menuItemObjd.Click += new System.EventHandler(this.OnObjdClicked);
            // 
            // menuItemObjf
            // 
            this.menuItemObjf.CheckOnClick = true;
            this.menuItemObjf.Name = "menuItemObjf";
            this.menuItemObjf.Size = new System.Drawing.Size(103, 22);
            this.menuItemObjf.Text = "Objf";
            this.menuItemObjf.Click += new System.EventHandler(this.OnObjfClicked);
            // 
            // menuItemNref
            // 
            this.menuItemNref.CheckOnClick = true;
            this.menuItemNref.Name = "menuItemNref";
            this.menuItemNref.Size = new System.Drawing.Size(103, 22);
            this.menuItemNref.Text = "Nref";
            this.menuItemNref.Click += new System.EventHandler(this.OnNrefClicked);
            // 
            // menuItemSlot
            // 
            this.menuItemSlot.CheckOnClick = true;
            this.menuItemSlot.Name = "menuItemSlot";
            this.menuItemSlot.Size = new System.Drawing.Size(103, 22);
            this.menuItemSlot.Text = "Slot";
            this.menuItemSlot.Click += new System.EventHandler(this.OnSlotClicked);
            // 
            // menuItemStr
            // 
            this.menuItemStr.CheckOnClick = true;
            this.menuItemStr.Name = "menuItemStr";
            this.menuItemStr.Size = new System.Drawing.Size(103, 22);
            this.menuItemStr.Text = "Str";
            this.menuItemStr.Click += new System.EventHandler(this.OnStrClicked);
            // 
            // menuItemTprp
            // 
            this.menuItemTprp.CheckOnClick = true;
            this.menuItemTprp.Name = "menuItemTprp";
            this.menuItemTprp.Size = new System.Drawing.Size(103, 22);
            this.menuItemTprp.Text = "Tprp";
            this.menuItemTprp.Click += new System.EventHandler(this.OnTprpClicked);
            // 
            // menuItemTrcn
            // 
            this.menuItemTrcn.CheckOnClick = true;
            this.menuItemTrcn.Name = "menuItemTrcn";
            this.menuItemTrcn.Size = new System.Drawing.Size(103, 22);
            this.menuItemTrcn.Text = "Trcn";
            this.menuItemTrcn.Click += new System.EventHandler(this.OnTrcnClicked);
            // 
            // menuItemTtab
            // 
            this.menuItemTtab.CheckOnClick = true;
            this.menuItemTtab.Name = "menuItemTtab";
            this.menuItemTtab.Size = new System.Drawing.Size(103, 22);
            this.menuItemTtab.Text = "Ttab";
            this.menuItemTtab.Click += new System.EventHandler(this.OnTtabClicked);
            // 
            // menuItemTtas
            // 
            this.menuItemTtas.CheckOnClick = true;
            this.menuItemTtas.Name = "menuItemTtas";
            this.menuItemTtas.Size = new System.Drawing.Size(103, 22);
            this.menuItemTtas.Text = "Ttas";
            this.menuItemTtas.Click += new System.EventHandler(this.OnTtasClicked);
            // 
            // menuItemVers
            // 
            this.menuItemVers.CheckOnClick = true;
            this.menuItemVers.Name = "menuItemVers";
            this.menuItemVers.Size = new System.Drawing.Size(103, 22);
            this.menuItemVers.Text = "Vers";
            this.menuItemVers.Click += new System.EventHandler(this.OnVersClicked);
            // 
            // menuHelp
            // 
            this.menuHelp.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
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
            this.menuItemAbout.Size = new System.Drawing.Size(126, 22);
            this.menuItemAbout.Text = "&About";
            this.menuItemAbout.Click += new System.EventHandler(this.OnHelpClicked);
            // 
            // menuOptions
            // 
            this.menuOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemPrettyPrint});
            this.menuOptions.Name = "menuOptions";
            this.menuOptions.Size = new System.Drawing.Size(61, 20);
            this.menuOptions.Text = "&Options";
            // 
            // menuItemPrettyPrint
            // 
            this.menuItemPrettyPrint.CheckOnClick = true;
            this.menuItemPrettyPrint.Name = "menuItemPrettyPrint";
            this.menuItemPrettyPrint.Size = new System.Drawing.Size(160, 22);
            this.menuItemPrettyPrint.Text = "XML Pretty Print";
            this.menuItemPrettyPrint.Click += new System.EventHandler(this.OnPrettyPrintClicked);
            // 
            // menuItemSeparator4
            // 
            this.menuItemSeparator4.Name = "menuItemSeparator4";
            this.menuItemSeparator4.Size = new System.Drawing.Size(232, 6);
            // 
            // gridResources
            // 
            this.gridResources.AllowUserToAddRows = false;
            this.gridResources.AllowUserToDeleteRows = false;
            this.gridResources.AllowUserToOrderColumns = true;
            this.gridResources.AllowUserToResizeRows = false;
            this.gridResources.BackgroundColor = System.Drawing.SystemColors.Window;
            this.gridResources.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridResources.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colType,
            this.colGroup,
            this.colInstance,
            this.colName,
            this.colHash});
            this.gridResources.ContextMenuStrip = this.menuContextGrid;
            this.gridResources.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridResources.Location = new System.Drawing.Point(0, 0);
            this.gridResources.MultiSelect = false;
            this.gridResources.Name = "gridResources";
            this.gridResources.ReadOnly = true;
            this.gridResources.RowHeadersVisible = false;
            this.gridResources.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridResources.Size = new System.Drawing.Size(905, 480);
            this.gridResources.TabIndex = 1;
            this.gridResources.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellContentClick);
            this.gridResources.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellMouseEnter);
            this.gridResources.CellToolTipTextNeeded += new System.Windows.Forms.DataGridViewCellToolTipTextNeededEventHandler(this.OnToolTipTextNeeded);
            // 
            // colType
            // 
            this.colType.DataPropertyName = "Type";
            this.colType.HeaderText = "Type";
            this.colType.MinimumWidth = 50;
            this.colType.Name = "colType";
            this.colType.ReadOnly = true;
            this.colType.Width = 50;
            // 
            // colGroup
            // 
            this.colGroup.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.colGroup.DataPropertyName = "Group";
            this.colGroup.HeaderText = "Group";
            this.colGroup.MinimumWidth = 100;
            this.colGroup.Name = "colGroup";
            this.colGroup.ReadOnly = true;
            // 
            // colInstance
            // 
            this.colInstance.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.colInstance.DataPropertyName = "Instance";
            this.colInstance.HeaderText = "Instance";
            this.colInstance.MinimumWidth = 65;
            this.colInstance.Name = "colInstance";
            this.colInstance.ReadOnly = true;
            this.colInstance.Width = 78;
            // 
            // colName
            // 
            this.colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colName.DataPropertyName = "Name";
            this.colName.HeaderText = "Name";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            // 
            // colHash
            // 
            this.colHash.DataPropertyName = "Hash";
            this.colHash.HeaderText = "Hash";
            this.colHash.Name = "colHash";
            this.colHash.ReadOnly = true;
            this.colHash.Visible = false;
            // 
            // menuContextGrid
            // 
            this.menuContextGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemSaveRawData,
            this.toolStripSeparator3,
            this.menuItemSaveAsJpeg,
            this.menuItemSaveAsPng});
            this.menuContextGrid.Name = "menuContextGrid";
            this.menuContextGrid.Size = new System.Drawing.Size(151, 76);
            this.menuContextGrid.Closing += new System.Windows.Forms.ToolStripDropDownClosingEventHandler(this.OnContextMenuClosing);
            this.menuContextGrid.Opening += new System.ComponentModel.CancelEventHandler(this.OnContextMenuOpening);
            // 
            // menuItemSaveRawData
            // 
            this.menuItemSaveRawData.Name = "menuItemSaveRawData";
            this.menuItemSaveRawData.Size = new System.Drawing.Size(150, 22);
            this.menuItemSaveRawData.Text = "Save Raw Data";
            this.menuItemSaveRawData.Click += new System.EventHandler(this.OnSaveRawDataClicked);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(147, 6);
            // 
            // menuItemSaveAsJpeg
            // 
            this.menuItemSaveAsJpeg.Name = "menuItemSaveAsJpeg";
            this.menuItemSaveAsJpeg.Size = new System.Drawing.Size(150, 22);
            this.menuItemSaveAsJpeg.Text = "Save As JPEG";
            this.menuItemSaveAsJpeg.Click += new System.EventHandler(this.OnSaveAsJpegClicked);
            // 
            // menuItemSaveAsPng
            // 
            this.menuItemSaveAsPng.Name = "menuItemSaveAsPng";
            this.menuItemSaveAsPng.Size = new System.Drawing.Size(150, 22);
            this.menuItemSaveAsPng.Text = "Save As PNG";
            this.menuItemSaveAsPng.Click += new System.EventHandler(this.OnSaveAsPngClicked);
            // 
            // saveXmlDialog
            // 
            this.saveXmlDialog.DefaultExt = "xml";
            this.saveXmlDialog.Filter = "XML file|*.xml|All files|*.*";
            this.saveXmlDialog.Title = "Save As XML";
            // 
            // splitContainer
            // 
            this.splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer.Location = new System.Drawing.Point(14, 27);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.gridResources);
            this.splitContainer.Panel1MinSize = 200;
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.panelImage);
            this.splitContainer.Panel2.Controls.Add(this.textXml);
            this.splitContainer.Panel2Collapsed = true;
            this.splitContainer.Panel2MinSize = 100;
            this.splitContainer.Size = new System.Drawing.Size(905, 480);
            this.splitContainer.SplitterDistance = 200;
            this.splitContainer.TabIndex = 2;
            // 
            // panelImage
            // 
            this.panelImage.AutoScroll = true;
            this.panelImage.Controls.Add(this.pictImage);
            this.panelImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelImage.Location = new System.Drawing.Point(0, 0);
            this.panelImage.Name = "panelImage";
            this.panelImage.Size = new System.Drawing.Size(150, 46);
            this.panelImage.TabIndex = 2;
            this.panelImage.Visible = false;
            // 
            // pictImage
            // 
            this.pictImage.ContextMenuStrip = this.menuContextImage;
            this.pictImage.Location = new System.Drawing.Point(0, 0);
            this.pictImage.Name = "pictImage";
            this.pictImage.Size = new System.Drawing.Size(112, 109);
            this.pictImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictImage.TabIndex = 1;
            this.pictImage.TabStop = false;
            // 
            // menuContextImage
            // 
            this.menuContextImage.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemCopyImage,
            this.toolStripSeparator4,
            this.menuItemSaveJpeg,
            this.menuItemSavePng});
            this.menuContextImage.Name = "menuContextImage";
            this.menuContextImage.Size = new System.Drawing.Size(209, 76);
            // 
            // menuItemCopyImage
            // 
            this.menuItemCopyImage.Name = "menuItemCopyImage";
            this.menuItemCopyImage.Size = new System.Drawing.Size(208, 22);
            this.menuItemCopyImage.Text = "Copy Image To Clipboard";
            this.menuItemCopyImage.Click += new System.EventHandler(this.OnCopyImageClicked);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(205, 6);
            // 
            // menuItemSaveJpeg
            // 
            this.menuItemSaveJpeg.Name = "menuItemSaveJpeg";
            this.menuItemSaveJpeg.Size = new System.Drawing.Size(208, 22);
            this.menuItemSaveJpeg.Text = "Save As JPEG";
            this.menuItemSaveJpeg.Click += new System.EventHandler(this.OnSaveJpegClicked);
            // 
            // menuItemSavePng
            // 
            this.menuItemSavePng.Name = "menuItemSavePng";
            this.menuItemSavePng.Size = new System.Drawing.Size(208, 22);
            this.menuItemSavePng.Text = "Save As PNG";
            this.menuItemSavePng.Click += new System.EventHandler(this.OnSavePngClicked);
            // 
            // textXml
            // 
            this.textXml.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textXml.Location = new System.Drawing.Point(0, 0);
            this.textXml.Multiline = true;
            this.textXml.Name = "textXml";
            this.textXml.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textXml.Size = new System.Drawing.Size(150, 46);
            this.textXml.TabIndex = 0;
            this.textXml.WordWrap = false;
            // 
            // saveRawDialog
            // 
            this.saveRawDialog.Title = "Save As Raw Data";
            // 
            // DbpfViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 519);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.menuMain);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuMain;
            this.MaximizeBox = false;
            this.Name = "DbpfViewerForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.OnLoad);
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridResources)).EndInit();
            this.menuContextGrid.ResumeLayout(false);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.panelImage.ResumeLayout(false);
            this.panelImage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictImage)).EndInit();
            this.menuContextImage.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuItemReloadPackage;
        private System.Windows.Forms.ToolStripSeparator menuItemSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuItemSelectPackage;
        private System.Windows.Forms.ToolStripMenuItem menuItemRecentPackages;
        private System.Windows.Forms.ToolStripSeparator menuItemSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuItemSaveXmlToClipboard;
        private System.Windows.Forms.ToolStripMenuItem menuItemSaveXmlAs;
        private System.Windows.Forms.ToolStripSeparator menuItemSeparator3;
        private System.Windows.Forms.ToolStripMenuItem menuItemConfiguration;
        private System.Windows.Forms.ToolStripSeparator menuItemSeparator4;
        private System.Windows.Forms.ToolStripMenuItem menuItemExit;
        private System.Windows.Forms.ToolStripMenuItem menuHelp;
        private System.Windows.Forms.ToolStripMenuItem menuItemAbout;
        private System.Windows.Forms.OpenFileDialog selectFileDialog;
        private System.Windows.Forms.DataGridView gridResources;
        private System.Windows.Forms.SaveFileDialog saveXmlDialog;
        private System.Windows.Forms.ToolStripMenuItem menuResources;
        private System.Windows.Forms.ToolStripMenuItem menuItemBcon;
        private System.Windows.Forms.ToolStripMenuItem menuItemBhav;
        private System.Windows.Forms.ToolStripMenuItem menuItemCtss;
        private System.Windows.Forms.ToolStripMenuItem menuItemGlob;
        private System.Windows.Forms.ToolStripMenuItem menuItemImg;
        private System.Windows.Forms.ToolStripMenuItem menuItemJpg;
        private System.Windows.Forms.ToolStripMenuItem menuItemObjd;
        private System.Windows.Forms.ToolStripMenuItem menuItemObjf;
        private System.Windows.Forms.ToolStripMenuItem menuItemNref;
        private System.Windows.Forms.ToolStripMenuItem menuItemSlot;
        private System.Windows.Forms.ToolStripMenuItem menuItemStr;
        private System.Windows.Forms.ToolStripMenuItem menuItemTprp;
        private System.Windows.Forms.ToolStripMenuItem menuItemTrcn;
        private System.Windows.Forms.ToolStripMenuItem menuItemTtab;
        private System.Windows.Forms.ToolStripMenuItem menuItemTtas;
        private System.Windows.Forms.ToolStripMenuItem menuItemVers;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuOptions;
        private System.Windows.Forms.ToolStripMenuItem menuItemPrettyPrint;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.TextBox textXml;
        private System.Windows.Forms.ToolStripMenuItem menuItemAll;
        private System.Windows.Forms.ToolStripMenuItem menuItemNone;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.PictureBox pictImage;
        private System.Windows.Forms.Panel panelImage;
        private System.Windows.Forms.ContextMenuStrip menuContextImage;
        private System.Windows.Forms.ToolStripMenuItem menuItemCopyImage;
        private System.Windows.Forms.ContextMenuStrip menuContextGrid;
        private System.Windows.Forms.ToolStripMenuItem menuItemSaveRawData;
        private System.Windows.Forms.SaveFileDialog saveRawDialog;
        private System.Windows.Forms.DataGridViewTextBoxColumn colType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGroup;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInstance;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHash;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem menuItemSaveAsJpeg;
        private System.Windows.Forms.ToolStripMenuItem menuItemSaveAsPng;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem menuItemSaveJpeg;
        private System.Windows.Forms.ToolStripMenuItem menuItemSavePng;
    }
}