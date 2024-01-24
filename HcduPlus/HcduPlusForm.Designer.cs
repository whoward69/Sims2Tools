/*
 * HCDU Plus - a utility for checking The Sims 2 package files for conflicts
 *           - see http://www.picknmixmods.com/Sims2/Notes/HcduPlus/HcduPlus.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2023
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */
 
using Microsoft.WindowsAPICodePack.Dialogs;

namespace HcduPlus
{
    partial class HcduPlusForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HcduPlusForm));
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.selectModsFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemSelect = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemRecentFolders = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemSaveToClipboard = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemConfiguration = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.menuResources = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAll = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemNone = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemBcon = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemBhav = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemColl = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemCtss = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemGlob = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemGzps = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemImg = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemObjd = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemObjf = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSlot = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemStr = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemTprp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemTrcn = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemTtab = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemTtas = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemUi = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemVers = new System.Windows.Forms.ToolStripMenuItem();
            this.menuConflicts = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemGuidConflicts = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemInternalConflicts = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemHomeCrafterConflicts = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemStoreVersionConflicts = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemCastawaysConflicts = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemIncludeKnownConflicts = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemKnownConflicts = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemOptionNoLoad = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTipGridByPackage = new System.Windows.Forms.ToolTip(this.components);
            this.hcduWorker = new System.ComponentModel.BackgroundWorker();
            this.lblModsPath = new System.Windows.Forms.Label();
            this.textModsPath = new System.Windows.Forms.TextBox();
            this.btnSelectModsPath = new System.Windows.Forms.Button();
            this.lblProgress = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.btnGO = new System.Windows.Forms.Button();
            this.tabConflicts = new System.Windows.Forms.TabControl();
            this.tabByPackage = new System.Windows.Forms.TabPage();
            this.gridByPackage = new System.Windows.Forms.DataGridView();
            this.colHcduPackageA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHcduPackageB = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuContextGrid = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemAddAsKnownConflict = new System.Windows.Forms.ToolStripMenuItem();
            this.tabByResource = new System.Windows.Forms.TabPage();
            this.gridByResource = new System.Windows.Forms.DataGridView();
            this.colHcduType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHcduGroup = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHcduInstance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHcduName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHcduPackages = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.btnSelectScanPath = new System.Windows.Forms.Button();
            this.textScanPath = new System.Windows.Forms.TextBox();
            this.lblScanPath = new System.Windows.Forms.Label();
            this.checkModsSavedSims = new System.Windows.Forms.CheckBox();
            this.checkScanSavedSims = new System.Windows.Forms.CheckBox();
            this.menuMain.SuspendLayout();
            this.tabConflicts.SuspendLayout();
            this.tabByPackage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridByPackage)).BeginInit();
            this.menuContextGrid.SuspendLayout();
            this.tabByResource.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridByResource)).BeginInit();
            this.SuspendLayout();
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuHelp,
            this.menuResources,
            this.menuConflicts,
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
            this.selectModsFolderToolStripMenuItem,
            this.menuItemSeparator1,
            this.menuItemSelect,
            this.menuItemRecentFolders,
            this.menuItemSeparator2,
            this.menuItemSaveToClipboard,
            this.menuItemSaveAs,
            this.menuItemSeparator3,
            this.menuItemConfiguration,
            this.toolStripSeparator1,
            this.menuItemExit});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(37, 20);
            this.menuFile.Text = "&File";
            this.menuFile.DropDownOpened += new System.EventHandler(this.OnFileDropDown);
            // 
            // selectModsFolderToolStripMenuItem
            // 
            this.selectModsFolderToolStripMenuItem.Name = "selectModsFolderToolStripMenuItem";
            this.selectModsFolderToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.selectModsFolderToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.selectModsFolderToolStripMenuItem.Text = "Set &Downloads Folder";
            this.selectModsFolderToolStripMenuItem.Click += new System.EventHandler(this.OnSelectModsClicked);
            // 
            // menuItemSeparator1
            // 
            this.menuItemSeparator1.Name = "menuItemSeparator1";
            this.menuItemSeparator1.Size = new System.Drawing.Size(227, 6);
            // 
            // menuItemSelect
            // 
            this.menuItemSelect.Name = "menuItemSelect";
            this.menuItemSelect.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.menuItemSelect.Size = new System.Drawing.Size(230, 22);
            this.menuItemSelect.Text = "&Select Scan Path...";
            this.menuItemSelect.Click += new System.EventHandler(this.OnSelectScanPathClicked);
            // 
            // menuItemRecentFolders
            // 
            this.menuItemRecentFolders.Name = "menuItemRecentFolders";
            this.menuItemRecentFolders.Size = new System.Drawing.Size(230, 22);
            this.menuItemRecentFolders.Text = "Recent Scan Paths...";
            // 
            // menuItemSeparator2
            // 
            this.menuItemSeparator2.Name = "menuItemSeparator2";
            this.menuItemSeparator2.Size = new System.Drawing.Size(227, 6);
            // 
            // menuItemSaveToClipboard
            // 
            this.menuItemSaveToClipboard.Name = "menuItemSaveToClipboard";
            this.menuItemSaveToClipboard.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.menuItemSaveToClipboard.Size = new System.Drawing.Size(230, 22);
            this.menuItemSaveToClipboard.Text = "Save To &Clipboard";
            this.menuItemSaveToClipboard.Click += new System.EventHandler(this.OnSaveToClipboardClicked);
            // 
            // menuItemSaveAs
            // 
            this.menuItemSaveAs.Name = "menuItemSaveAs";
            this.menuItemSaveAs.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.S)));
            this.menuItemSaveAs.Size = new System.Drawing.Size(230, 22);
            this.menuItemSaveAs.Text = "Save &As...";
            this.menuItemSaveAs.Click += new System.EventHandler(this.OnSaveAsClicked);
            // 
            // menuItemSeparator3
            // 
            this.menuItemSeparator3.Name = "menuItemSeparator3";
            this.menuItemSeparator3.Size = new System.Drawing.Size(227, 6);
            // 
            // menuItemConfiguration
            // 
            this.menuItemConfiguration.Name = "menuItemConfiguration";
            this.menuItemConfiguration.Size = new System.Drawing.Size(230, 22);
            this.menuItemConfiguration.Text = "Configuration...";
            this.menuItemConfiguration.Click += new System.EventHandler(this.OnConfigClicked);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(227, 6);
            // 
            // menuItemExit
            // 
            this.menuItemExit.Name = "menuItemExit";
            this.menuItemExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.menuItemExit.Size = new System.Drawing.Size(230, 22);
            this.menuItemExit.Text = "E&xit";
            this.menuItemExit.Click += new System.EventHandler(this.OnExitClicked);
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
            this.menuItemAbout.Size = new System.Drawing.Size(135, 22);
            this.menuItemAbout.Text = "About...";
            this.menuItemAbout.Click += new System.EventHandler(this.OnHelpClicked);
            // 
            // menuResources
            // 
            this.menuResources.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemAll,
            this.menuItemNone,
            this.toolStripSeparator4,
            this.menuItemBcon,
            this.menuItemBhav,
            this.menuItemColl,
            this.menuItemCtss,
            this.menuItemGlob,
            this.menuItemGzps,
            this.menuItemImg,
            this.menuItemObjd,
            this.menuItemObjf,
            this.menuItemSlot,
            this.menuItemStr,
            this.menuItemTprp,
            this.menuItemTrcn,
            this.menuItemTtab,
            this.menuItemTtas,
            this.menuItemUi,
            this.menuItemVers});
            this.menuResources.Name = "menuResources";
            this.menuResources.Size = new System.Drawing.Size(72, 20);
            this.menuResources.Text = "&Resources";
            // 
            // menuItemAll
            // 
            this.menuItemAll.Name = "menuItemAll";
            this.menuItemAll.Size = new System.Drawing.Size(180, 22);
            this.menuItemAll.Text = "&All";
            this.menuItemAll.Click += new System.EventHandler(this.OnAllClicked);
            // 
            // menuItemNone
            // 
            this.menuItemNone.Name = "menuItemNone";
            this.menuItemNone.Size = new System.Drawing.Size(180, 22);
            this.menuItemNone.Text = "&None";
            this.menuItemNone.Click += new System.EventHandler(this.OnNoneClicked);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(177, 6);
            // 
            // menuItemBcon
            // 
            this.menuItemBcon.CheckOnClick = true;
            this.menuItemBcon.Name = "menuItemBcon";
            this.menuItemBcon.Size = new System.Drawing.Size(180, 22);
            this.menuItemBcon.Text = "Bcon";
            this.menuItemBcon.Click += new System.EventHandler(this.OnBconClicked);
            // 
            // menuItemBhav
            // 
            this.menuItemBhav.CheckOnClick = true;
            this.menuItemBhav.Name = "menuItemBhav";
            this.menuItemBhav.Size = new System.Drawing.Size(180, 22);
            this.menuItemBhav.Text = "Bhav";
            this.menuItemBhav.Click += new System.EventHandler(this.OnBhavClicked);
            // 
            // menuItemColl
            // 
            this.menuItemColl.CheckOnClick = true;
            this.menuItemColl.Name = "menuItemColl";
            this.menuItemColl.Size = new System.Drawing.Size(180, 22);
            this.menuItemColl.Text = "Coll";
            this.menuItemColl.Click += new System.EventHandler(this.OnCollClicked);
            // 
            // menuItemCtss
            // 
            this.menuItemCtss.CheckOnClick = true;
            this.menuItemCtss.Name = "menuItemCtss";
            this.menuItemCtss.Size = new System.Drawing.Size(180, 22);
            this.menuItemCtss.Text = "Ctss";
            this.menuItemCtss.Click += new System.EventHandler(this.OnCtssClicked);
            // 
            // menuItemGlob
            // 
            this.menuItemGlob.CheckOnClick = true;
            this.menuItemGlob.Name = "menuItemGlob";
            this.menuItemGlob.Size = new System.Drawing.Size(180, 22);
            this.menuItemGlob.Text = "Glob";
            this.menuItemGlob.Click += new System.EventHandler(this.OnGlobClicked);
            // 
            // menuItemGzps
            // 
            this.menuItemGzps.CheckOnClick = true;
            this.menuItemGzps.Name = "menuItemGzps";
            this.menuItemGzps.Size = new System.Drawing.Size(180, 22);
            this.menuItemGzps.Text = "Gzps";
            this.menuItemGzps.Click += new System.EventHandler(this.OnGzpsClicked);
            // 
            // menuItemImg
            // 
            this.menuItemImg.CheckOnClick = true;
            this.menuItemImg.Name = "menuItemImg";
            this.menuItemImg.Size = new System.Drawing.Size(180, 22);
            this.menuItemImg.Text = "Img";
            this.menuItemImg.Click += new System.EventHandler(this.OnImgClicked);
            // 
            // menuItemObjd
            // 
            this.menuItemObjd.CheckOnClick = true;
            this.menuItemObjd.Name = "menuItemObjd";
            this.menuItemObjd.Size = new System.Drawing.Size(180, 22);
            this.menuItemObjd.Text = "Objd";
            this.menuItemObjd.Click += new System.EventHandler(this.OnObjdClicked);
            // 
            // menuItemObjf
            // 
            this.menuItemObjf.CheckOnClick = true;
            this.menuItemObjf.Name = "menuItemObjf";
            this.menuItemObjf.Size = new System.Drawing.Size(180, 22);
            this.menuItemObjf.Text = "Objf";
            this.menuItemObjf.Click += new System.EventHandler(this.OnObjfClicked);
            // 
            // menuItemSlot
            // 
            this.menuItemSlot.CheckOnClick = true;
            this.menuItemSlot.Name = "menuItemSlot";
            this.menuItemSlot.Size = new System.Drawing.Size(180, 22);
            this.menuItemSlot.Text = "Slot";
            this.menuItemSlot.Click += new System.EventHandler(this.OnSlotClicked);
            // 
            // menuItemStr
            // 
            this.menuItemStr.CheckOnClick = true;
            this.menuItemStr.Name = "menuItemStr";
            this.menuItemStr.Size = new System.Drawing.Size(180, 22);
            this.menuItemStr.Text = "Str";
            this.menuItemStr.Click += new System.EventHandler(this.OnStrClicked);
            // 
            // menuItemTprp
            // 
            this.menuItemTprp.CheckOnClick = true;
            this.menuItemTprp.Name = "menuItemTprp";
            this.menuItemTprp.Size = new System.Drawing.Size(180, 22);
            this.menuItemTprp.Text = "Tprp";
            this.menuItemTprp.Click += new System.EventHandler(this.OnTprpClicked);
            // 
            // menuItemTrcn
            // 
            this.menuItemTrcn.CheckOnClick = true;
            this.menuItemTrcn.Name = "menuItemTrcn";
            this.menuItemTrcn.Size = new System.Drawing.Size(180, 22);
            this.menuItemTrcn.Text = "Trcn";
            this.menuItemTrcn.Click += new System.EventHandler(this.OnTrcnClicked);
            // 
            // menuItemTtab
            // 
            this.menuItemTtab.CheckOnClick = true;
            this.menuItemTtab.Name = "menuItemTtab";
            this.menuItemTtab.Size = new System.Drawing.Size(180, 22);
            this.menuItemTtab.Text = "Ttab";
            this.menuItemTtab.Click += new System.EventHandler(this.OnTtabClicked);
            // 
            // menuItemTtas
            // 
            this.menuItemTtas.CheckOnClick = true;
            this.menuItemTtas.Name = "menuItemTtas";
            this.menuItemTtas.Size = new System.Drawing.Size(180, 22);
            this.menuItemTtas.Text = "Ttas";
            this.menuItemTtas.Click += new System.EventHandler(this.OnTtasClicked);
            // 
            // menuItemUi
            // 
            this.menuItemUi.CheckOnClick = true;
            this.menuItemUi.Name = "menuItemUi";
            this.menuItemUi.Size = new System.Drawing.Size(180, 22);
            this.menuItemUi.Text = "UI";
            this.menuItemUi.Click += new System.EventHandler(this.OnUiClicked);
            // 
            // menuItemVers
            // 
            this.menuItemVers.CheckOnClick = true;
            this.menuItemVers.Name = "menuItemVers";
            this.menuItemVers.Size = new System.Drawing.Size(180, 22);
            this.menuItemVers.Text = "Vers";
            this.menuItemVers.Click += new System.EventHandler(this.OnVersClicked);
            // 
            // menuConflicts
            // 
            this.menuConflicts.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemGuidConflicts,
            this.toolStripSeparator2,
            this.menuItemInternalConflicts,
            this.menuItemHomeCrafterConflicts,
            this.menuItemStoreVersionConflicts,
            this.menuItemCastawaysConflicts,
            this.toolStripSeparator3,
            this.menuItemIncludeKnownConflicts,
            this.menuItemKnownConflicts});
            this.menuConflicts.Name = "menuConflicts";
            this.menuConflicts.Size = new System.Drawing.Size(66, 20);
            this.menuConflicts.Text = "&Conflicts";
            // 
            // menuItemGuidConflicts
            // 
            this.menuItemGuidConflicts.Checked = true;
            this.menuItemGuidConflicts.CheckOnClick = true;
            this.menuItemGuidConflicts.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuItemGuidConflicts.Name = "menuItemGuidConflicts";
            this.menuItemGuidConflicts.Size = new System.Drawing.Size(230, 22);
            this.menuItemGuidConflicts.Text = "Check For GUID Conflicts";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(227, 6);
            // 
            // menuItemInternalConflicts
            // 
            this.menuItemInternalConflicts.Checked = true;
            this.menuItemInternalConflicts.CheckOnClick = true;
            this.menuItemInternalConflicts.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuItemInternalConflicts.Name = "menuItemInternalConflicts";
            this.menuItemInternalConflicts.Size = new System.Drawing.Size(230, 22);
            this.menuItemInternalConflicts.Text = "Ignore Internal Conflicts";
            // 
            // menuItemHomeCrafterConflicts
            // 
            this.menuItemHomeCrafterConflicts.Checked = true;
            this.menuItemHomeCrafterConflicts.CheckOnClick = true;
            this.menuItemHomeCrafterConflicts.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuItemHomeCrafterConflicts.Name = "menuItemHomeCrafterConflicts";
            this.menuItemHomeCrafterConflicts.Size = new System.Drawing.Size(230, 22);
            this.menuItemHomeCrafterConflicts.Text = "Ignore HomeCrafter Conflicts";
            // 
            // menuItemStoreVersionConflicts
            // 
            this.menuItemStoreVersionConflicts.Checked = true;
            this.menuItemStoreVersionConflicts.CheckOnClick = true;
            this.menuItemStoreVersionConflicts.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuItemStoreVersionConflicts.Name = "menuItemStoreVersionConflicts";
            this.menuItemStoreVersionConflicts.Size = new System.Drawing.Size(230, 22);
            this.menuItemStoreVersionConflicts.Text = "Ignore Store Version Conflicts";
            // 
            // menuItemCastawaysConflicts
            // 
            this.menuItemCastawaysConflicts.Checked = true;
            this.menuItemCastawaysConflicts.CheckOnClick = true;
            this.menuItemCastawaysConflicts.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuItemCastawaysConflicts.Name = "menuItemCastawaysConflicts";
            this.menuItemCastawaysConflicts.Size = new System.Drawing.Size(230, 22);
            this.menuItemCastawaysConflicts.Text = "Ignore Castaways Conflicts";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(227, 6);
            // 
            // menuItemIncludeKnownConflicts
            // 
            this.menuItemIncludeKnownConflicts.CheckOnClick = true;
            this.menuItemIncludeKnownConflicts.Name = "menuItemIncludeKnownConflicts";
            this.menuItemIncludeKnownConflicts.Size = new System.Drawing.Size(230, 22);
            this.menuItemIncludeKnownConflicts.Text = "Include Known Conflicts";
            this.menuItemIncludeKnownConflicts.Click += new System.EventHandler(this.OnIncludeKnownConflictsClicked);
            // 
            // menuItemKnownConflicts
            // 
            this.menuItemKnownConflicts.Name = "menuItemKnownConflicts";
            this.menuItemKnownConflicts.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.K)));
            this.menuItemKnownConflicts.Size = new System.Drawing.Size(230, 22);
            this.menuItemKnownConflicts.Text = "&Known Conflicts...";
            this.menuItemKnownConflicts.Click += new System.EventHandler(this.OnKnownConflictsClicked);
            // 
            // menuOptions
            // 
            this.menuOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemOptionNoLoad});
            this.menuOptions.Name = "menuOptions";
            this.menuOptions.Size = new System.Drawing.Size(61, 20);
            this.menuOptions.Text = "&Options";
            // 
            // menuItemOptionNoLoad
            // 
            this.menuItemOptionNoLoad.CheckOnClick = true;
            this.menuItemOptionNoLoad.Name = "menuItemOptionNoLoad";
            this.menuItemOptionNoLoad.Size = new System.Drawing.Size(208, 22);
            this.menuItemOptionNoLoad.Text = "Include .noload packages";
            this.menuItemOptionNoLoad.Click += new System.EventHandler(this.OnNoLoads);
            // 
            // hcduWorker
            // 
            this.hcduWorker.WorkerReportsProgress = true;
            this.hcduWorker.WorkerSupportsCancellation = true;
            this.hcduWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.HcduWorker_DoWork);
            this.hcduWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.HcduWorker_Progress);
            this.hcduWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.HcduWorker_Completed);
            // 
            // lblModsPath
            // 
            this.lblModsPath.AutoSize = true;
            this.lblModsPath.Location = new System.Drawing.Point(10, 41);
            this.lblModsPath.Name = "lblModsPath";
            this.lblModsPath.Size = new System.Drawing.Size(110, 15);
            this.lblModsPath.TabIndex = 1;
            this.lblModsPath.Text = "Downloads Folder:";
            // 
            // textModsPath
            // 
            this.textModsPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textModsPath.Location = new System.Drawing.Point(126, 38);
            this.textModsPath.Name = "textModsPath";
            this.textModsPath.Size = new System.Drawing.Size(534, 21);
            this.textModsPath.TabIndex = 2;
            this.textModsPath.TabStop = false;
            this.textModsPath.WordWrap = false;
            this.textModsPath.TextChanged += new System.EventHandler(this.OnPathsChanged);
            // 
            // btnSelectModsPath
            // 
            this.btnSelectModsPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectModsPath.Location = new System.Drawing.Point(775, 33);
            this.btnSelectModsPath.Name = "btnSelectModsPath";
            this.btnSelectModsPath.Size = new System.Drawing.Size(143, 30);
            this.btnSelectModsPath.TabIndex = 3;
            this.btnSelectModsPath.Text = "&Downloads Folder...";
            this.btnSelectModsPath.UseVisualStyleBackColor = true;
            this.btnSelectModsPath.Click += new System.EventHandler(this.OnSelectModsClicked);
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Location = new System.Drawing.Point(10, 121);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(59, 15);
            this.lblProgress.TabIndex = 4;
            this.lblProgress.Text = "Progress:";
            this.lblProgress.Visible = false;
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(75, 117);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(694, 23);
            this.progressBar.TabIndex = 5;
            this.progressBar.Visible = false;
            // 
            // btnGO
            // 
            this.btnGO.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGO.Enabled = false;
            this.btnGO.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGO.Location = new System.Drawing.Point(775, 113);
            this.btnGO.Name = "btnGO";
            this.btnGO.Size = new System.Drawing.Size(143, 30);
            this.btnGO.TabIndex = 6;
            this.btnGO.Text = "S&CAN";
            this.btnGO.UseVisualStyleBackColor = true;
            this.btnGO.Click += new System.EventHandler(this.OnGoClicked);
            // 
            // tabConflicts
            // 
            this.tabConflicts.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabConflicts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabConflicts.Controls.Add(this.tabByPackage);
            this.tabConflicts.Controls.Add(this.tabByResource);
            this.tabConflicts.Location = new System.Drawing.Point(12, 149);
            this.tabConflicts.Name = "tabConflicts";
            this.tabConflicts.SelectedIndex = 0;
            this.tabConflicts.Size = new System.Drawing.Size(910, 411);
            this.tabConflicts.TabIndex = 7;
            // 
            // tabByPackage
            // 
            this.tabByPackage.Controls.Add(this.gridByPackage);
            this.tabByPackage.Location = new System.Drawing.Point(4, 4);
            this.tabByPackage.Name = "tabByPackage";
            this.tabByPackage.Padding = new System.Windows.Forms.Padding(3);
            this.tabByPackage.Size = new System.Drawing.Size(902, 383);
            this.tabByPackage.TabIndex = 0;
            this.tabByPackage.Text = "By Package";
            this.tabByPackage.UseVisualStyleBackColor = true;
            // 
            // gridByPackage
            // 
            this.gridByPackage.AllowUserToAddRows = false;
            this.gridByPackage.AllowUserToDeleteRows = false;
            this.gridByPackage.AllowUserToResizeRows = false;
            this.gridByPackage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridByPackage.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridByPackage.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridByPackage.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridByPackage.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colHcduPackageA,
            this.colHcduPackageB});
            this.gridByPackage.ContextMenuStrip = this.menuContextGrid;
            this.gridByPackage.Location = new System.Drawing.Point(0, 0);
            this.gridByPackage.MultiSelect = false;
            this.gridByPackage.Name = "gridByPackage";
            this.gridByPackage.ReadOnly = true;
            this.gridByPackage.RowHeadersVisible = false;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridByPackage.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.gridByPackage.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridByPackage.ShowCellErrors = false;
            this.gridByPackage.ShowEditingIcon = false;
            this.gridByPackage.Size = new System.Drawing.Size(898, 377);
            this.gridByPackage.TabIndex = 0;
            this.gridByPackage.TabStop = false;
            this.gridByPackage.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellMouseEnter);
            this.gridByPackage.CellToolTipTextNeeded += new System.Windows.Forms.DataGridViewCellToolTipTextNeededEventHandler(this.OnToolTipTextNeeded);
            // 
            // colHcduPackageA
            // 
            this.colHcduPackageA.DataPropertyName = "Loads Earlier";
            this.colHcduPackageA.HeaderText = "Loads Earlier";
            this.colHcduPackageA.Name = "colHcduPackageA";
            this.colHcduPackageA.ReadOnly = true;
            this.colHcduPackageA.Width = 445;
            // 
            // colHcduPackageB
            // 
            this.colHcduPackageB.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colHcduPackageB.DataPropertyName = "Loads Later";
            this.colHcduPackageB.HeaderText = "Loads Later";
            this.colHcduPackageB.Name = "colHcduPackageB";
            this.colHcduPackageB.ReadOnly = true;
            // 
            // menuContextGrid
            // 
            this.menuContextGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemAddAsKnownConflict});
            this.menuContextGrid.Name = "menuContextGrid";
            this.menuContextGrid.Size = new System.Drawing.Size(198, 26);
            this.menuContextGrid.Closing += new System.Windows.Forms.ToolStripDropDownClosingEventHandler(this.OnContextMenuClosing);
            this.menuContextGrid.Opening += new System.ComponentModel.CancelEventHandler(this.OnContextMenuOpening);
            // 
            // menuItemAddAsKnownConflict
            // 
            this.menuItemAddAsKnownConflict.Name = "menuItemAddAsKnownConflict";
            this.menuItemAddAsKnownConflict.Size = new System.Drawing.Size(197, 22);
            this.menuItemAddAsKnownConflict.Text = "Add As Known Conflict";
            this.menuItemAddAsKnownConflict.Click += new System.EventHandler(this.OnAddAsKnownConflictClicked);
            // 
            // tabByResource
            // 
            this.tabByResource.Controls.Add(this.gridByResource);
            this.tabByResource.Location = new System.Drawing.Point(4, 4);
            this.tabByResource.Name = "tabByResource";
            this.tabByResource.Padding = new System.Windows.Forms.Padding(3);
            this.tabByResource.Size = new System.Drawing.Size(902, 383);
            this.tabByResource.TabIndex = 1;
            this.tabByResource.Text = "By Resource";
            this.tabByResource.UseVisualStyleBackColor = true;
            // 
            // gridByResource
            // 
            this.gridByResource.AllowUserToAddRows = false;
            this.gridByResource.AllowUserToDeleteRows = false;
            this.gridByResource.AllowUserToOrderColumns = true;
            this.gridByResource.AllowUserToResizeRows = false;
            this.gridByResource.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridByResource.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridByResource.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.gridByResource.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridByResource.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colHcduType,
            this.colHcduGroup,
            this.colHcduInstance,
            this.colHcduName,
            this.colHcduPackages});
            this.gridByResource.Location = new System.Drawing.Point(0, 0);
            this.gridByResource.MultiSelect = false;
            this.gridByResource.Name = "gridByResource";
            this.gridByResource.ReadOnly = true;
            this.gridByResource.RowHeadersVisible = false;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridByResource.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.gridByResource.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridByResource.ShowCellErrors = false;
            this.gridByResource.ShowEditingIcon = false;
            this.gridByResource.Size = new System.Drawing.Size(898, 377);
            this.gridByResource.TabIndex = 0;
            this.gridByResource.TabStop = false;
            this.gridByResource.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellMouseEnter);
            // 
            // colHcduType
            // 
            this.colHcduType.DataPropertyName = "Type";
            this.colHcduType.HeaderText = "Type";
            this.colHcduType.Name = "colHcduType";
            this.colHcduType.ReadOnly = true;
            this.colHcduType.Width = 75;
            // 
            // colHcduGroup
            // 
            this.colHcduGroup.DataPropertyName = "Group";
            this.colHcduGroup.HeaderText = "Group";
            this.colHcduGroup.Name = "colHcduGroup";
            this.colHcduGroup.ReadOnly = true;
            this.colHcduGroup.Width = 75;
            // 
            // colHcduInstance
            // 
            this.colHcduInstance.DataPropertyName = "Instance";
            this.colHcduInstance.HeaderText = "Instance";
            this.colHcduInstance.Name = "colHcduInstance";
            this.colHcduInstance.ReadOnly = true;
            this.colHcduInstance.Width = 75;
            // 
            // colHcduName
            // 
            this.colHcduName.DataPropertyName = "Name";
            this.colHcduName.HeaderText = "Name";
            this.colHcduName.Name = "colHcduName";
            this.colHcduName.ReadOnly = true;
            this.colHcduName.Width = 250;
            // 
            // colHcduPackages
            // 
            this.colHcduPackages.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colHcduPackages.DataPropertyName = "Packages";
            this.colHcduPackages.HeaderText = "Packages";
            this.colHcduPackages.Name = "colHcduPackages";
            this.colHcduPackages.ReadOnly = true;
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "txt";
            this.saveFileDialog.Filter = "Normal text file|*.txt|All files|*.*";
            this.saveFileDialog.Title = "Save As";
            // 
            // btnSelectScanPath
            // 
            this.btnSelectScanPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectScanPath.Location = new System.Drawing.Point(775, 68);
            this.btnSelectScanPath.Name = "btnSelectScanPath";
            this.btnSelectScanPath.Size = new System.Drawing.Size(143, 30);
            this.btnSelectScanPath.TabIndex = 10;
            this.btnSelectScanPath.Text = "&Scan Folder...";
            this.btnSelectScanPath.UseVisualStyleBackColor = true;
            this.btnSelectScanPath.Click += new System.EventHandler(this.OnSelectScanPathClicked);
            // 
            // textScanPath
            // 
            this.textScanPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textScanPath.Location = new System.Drawing.Point(126, 73);
            this.textScanPath.Name = "textScanPath";
            this.textScanPath.Size = new System.Drawing.Size(534, 21);
            this.textScanPath.TabIndex = 9;
            this.textScanPath.TabStop = false;
            this.textScanPath.WordWrap = false;
            this.textScanPath.TextChanged += new System.EventHandler(this.OnPathsChanged);
            // 
            // lblScanPath
            // 
            this.lblScanPath.AutoSize = true;
            this.lblScanPath.Location = new System.Drawing.Point(10, 76);
            this.lblScanPath.Name = "lblScanPath";
            this.lblScanPath.Size = new System.Drawing.Size(76, 15);
            this.lblScanPath.TabIndex = 8;
            this.lblScanPath.Text = "Scan Folder:";
            // 
            // checkModsSavedSims
            // 
            this.checkModsSavedSims.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkModsSavedSims.AutoSize = true;
            this.checkModsSavedSims.Location = new System.Drawing.Point(666, 40);
            this.checkModsSavedSims.Name = "checkModsSavedSims";
            this.checkModsSavedSims.Size = new System.Drawing.Size(107, 19);
            this.checkModsSavedSims.TabIndex = 11;
            this.checkModsSavedSims.Text = "Inc SavedSims";
            this.checkModsSavedSims.UseVisualStyleBackColor = true;
            this.checkModsSavedSims.Click += new System.EventHandler(this.OnSavedSimsDownloads);
            // 
            // checkScanSavedSims
            // 
            this.checkScanSavedSims.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkScanSavedSims.AutoSize = true;
            this.checkScanSavedSims.Location = new System.Drawing.Point(666, 75);
            this.checkScanSavedSims.Name = "checkScanSavedSims";
            this.checkScanSavedSims.Size = new System.Drawing.Size(107, 19);
            this.checkScanSavedSims.TabIndex = 12;
            this.checkScanSavedSims.Text = "Inc SavedSims";
            this.checkScanSavedSims.UseVisualStyleBackColor = true;
            this.checkScanSavedSims.Click += new System.EventHandler(this.OnSavedSimsScan);
            // 
            // HcduPlusForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 572);
            this.Controls.Add(this.checkScanSavedSims);
            this.Controls.Add(this.checkModsSavedSims);
            this.Controls.Add(this.btnSelectScanPath);
            this.Controls.Add(this.textScanPath);
            this.Controls.Add(this.lblScanPath);
            this.Controls.Add(this.tabConflicts);
            this.Controls.Add(this.btnGO);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.btnSelectModsPath);
            this.Controls.Add(this.textModsPath);
            this.Controls.Add(this.lblModsPath);
            this.Controls.Add(this.menuMain);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuMain;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(800, 500);
            this.Name = "HcduPlusForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.OnLoad);
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.tabConflicts.ResumeLayout(false);
            this.tabByPackage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridByPackage)).EndInit();
            this.menuContextGrid.ResumeLayout(false);
            this.tabByResource.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridByResource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblModsPath;
        private System.Windows.Forms.TextBox textModsPath;
        private System.Windows.Forms.Button btnSelectModsPath;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button btnGO;
        private System.Windows.Forms.TabControl tabConflicts;
        private System.Windows.Forms.TabPage tabByPackage;
        private System.Windows.Forms.DataGridView gridByPackage;
        private System.Windows.Forms.TabPage tabByResource;
        private System.Windows.Forms.DataGridView gridByResource;
        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuItemSelect;
        private System.Windows.Forms.ToolStripMenuItem menuItemRecentFolders;
        private System.Windows.Forms.ToolStripSeparator menuItemSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuItemSaveToClipboard;
        private System.Windows.Forms.ToolStripMenuItem menuItemSaveAs;
        private System.Windows.Forms.ToolStripSeparator menuItemSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuItemConfiguration;
        private System.Windows.Forms.ToolStripSeparator menuItemSeparator3;
        private System.Windows.Forms.ToolStripMenuItem menuItemExit;
        private System.Windows.Forms.ToolStripMenuItem menuHelp;
        private System.Windows.Forms.ToolStripMenuItem menuItemAbout;
        private System.Windows.Forms.ContextMenuStrip menuContextGrid;
        private System.Windows.Forms.ToolStripMenuItem menuItemAddAsKnownConflict;
        private System.Windows.Forms.ToolTip toolTipGridByPackage;
        private CommonOpenFileDialog selectPathDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.ComponentModel.BackgroundWorker hcduWorker;
        private System.Windows.Forms.Button btnSelectScanPath;
        private System.Windows.Forms.TextBox textScanPath;
        private System.Windows.Forms.Label lblScanPath;
        private System.Windows.Forms.ToolStripMenuItem selectModsFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuResources;
        private System.Windows.Forms.ToolStripMenuItem menuItemBcon;
        private System.Windows.Forms.ToolStripMenuItem menuItemBhav;
        private System.Windows.Forms.ToolStripMenuItem menuItemColl;
        private System.Windows.Forms.ToolStripMenuItem menuItemCtss;
        private System.Windows.Forms.ToolStripMenuItem menuItemGlob;
        private System.Windows.Forms.ToolStripMenuItem menuItemGzps;
        private System.Windows.Forms.ToolStripMenuItem menuItemImg;
        private System.Windows.Forms.ToolStripMenuItem menuItemObjd;
        private System.Windows.Forms.ToolStripMenuItem menuItemObjf;
        private System.Windows.Forms.ToolStripMenuItem menuItemSlot;
        private System.Windows.Forms.ToolStripMenuItem menuItemStr;
        private System.Windows.Forms.ToolStripMenuItem menuItemTprp;
        private System.Windows.Forms.ToolStripMenuItem menuItemTrcn;
        private System.Windows.Forms.ToolStripMenuItem menuItemTtab;
        private System.Windows.Forms.ToolStripMenuItem menuItemTtas;
        private System.Windows.Forms.ToolStripMenuItem menuItemUi;
        private System.Windows.Forms.ToolStripMenuItem menuItemVers;
        private System.Windows.Forms.ToolStripMenuItem menuConflicts;
        private System.Windows.Forms.ToolStripMenuItem menuItemInternalConflicts;
        private System.Windows.Forms.ToolStripMenuItem menuItemIncludeKnownConflicts;
        private System.Windows.Forms.ToolStripMenuItem menuItemKnownConflicts;
        private System.Windows.Forms.ToolStripMenuItem menuItemHomeCrafterConflicts;
        private System.Windows.Forms.ToolStripMenuItem menuItemStoreVersionConflicts;
        private System.Windows.Forms.ToolStripMenuItem menuItemCastawaysConflicts;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuItemGuidConflicts;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHcduPackageA;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHcduPackageB;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHcduType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHcduGroup;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHcduInstance;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHcduName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHcduPackages;
        private System.Windows.Forms.CheckBox checkModsSavedSims;
        private System.Windows.Forms.CheckBox checkScanSavedSims;
        private System.Windows.Forms.ToolStripMenuItem menuOptions;
        private System.Windows.Forms.ToolStripMenuItem menuItemOptionNoLoad;
        private System.Windows.Forms.ToolStripMenuItem menuItemAll;
        private System.Windows.Forms.ToolStripMenuItem menuItemNone;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
    }
}

