/*
 * Collection Manager - a utility for managing Sims 2 collections
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

namespace CollectionManager
{
    partial class CollectionManagerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CollectionManagerForm));
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSelectCollection = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemRecentCollections = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemNewCollection = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemSaveTab = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSaveAsTab = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSaveAllTab = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemCloseTab = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemCloseAllTabs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemConfiguration = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemMode = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAdvanced = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemAutoBackup = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator17 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemConfirmDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCollection = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemCollChangeIcon = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemCollRename = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemCollDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.menuReorder = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemReorderOrderTabsBySort = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemReorderSetSortByOrder = new System.Windows.Forms.ToolStripMenuItem();
            this.menuWindows = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemOpenIn = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemOpenInTab = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemOpenInWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemMouseDrop = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemMouseDropBefore = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemMouseDropAfter = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemMouseDropInternal = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemMouseDropExternal = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCaching = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemCachingUpdateMaxisObjects = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemCachingUpdateCustomObjects = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemCachingUpdateMaxisClothes = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemCachingUpdateCustomClothes = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemCachingRemoveThumbnails = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.selectFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.menuContextTab = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemTabContextFloat = new System.Windows.Forms.ToolStripMenuItem();
            this.sep1ContextMenuItem = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemTabContextSave = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemTabContextSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemTabContextSaveAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemTabContextChangeIcon = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemTabContextRenamePackage = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemTabContextDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemTabContextClose = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemTabContextCloseAll = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMain.SuspendLayout();
            this.menuContextTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuHelp,
            this.menuItemMode,
            this.menuCollection,
            this.menuReorder,
            this.menuWindows,
            this.menuOptions,
            this.menuCaching});
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
            this.menuItemSelectCollection,
            this.menuItemRecentCollections,
            this.menuItemNewCollection,
            this.toolStripSeparator1,
            this.menuItemSaveTab,
            this.menuItemSaveAsTab,
            this.menuItemSaveAllTab,
            this.toolStripSeparator14,
            this.menuItemCloseTab,
            this.menuItemCloseAllTabs,
            this.toolStripSeparator3,
            this.menuItemConfiguration,
            this.menuItemSeparator2,
            this.menuItemExit});
            this.menuFile.Name = "menuFile";
            this.menuFile.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.menuFile.Size = new System.Drawing.Size(37, 20);
            this.menuFile.Text = "&File";
            this.menuFile.DropDownOpening += new System.EventHandler(this.OnFileOpening);
            // 
            // menuItemSelectCollection
            // 
            this.menuItemSelectCollection.Name = "menuItemSelectCollection";
            this.menuItemSelectCollection.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.menuItemSelectCollection.Size = new System.Drawing.Size(227, 22);
            this.menuItemSelectCollection.Text = "Select Collection(s)...";
            this.menuItemSelectCollection.Click += new System.EventHandler(this.OnSelectClicked);
            // 
            // menuItemRecentCollections
            // 
            this.menuItemRecentCollections.Name = "menuItemRecentCollections";
            this.menuItemRecentCollections.Size = new System.Drawing.Size(227, 22);
            this.menuItemRecentCollections.Text = "Recent Collections...";
            // 
            // menuItemNewCollection
            // 
            this.menuItemNewCollection.Name = "menuItemNewCollection";
            this.menuItemNewCollection.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.menuItemNewCollection.Size = new System.Drawing.Size(227, 22);
            this.menuItemNewCollection.Text = "&New Collection...";
            this.menuItemNewCollection.Click += new System.EventHandler(this.OnCollection_New);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(224, 6);
            // 
            // menuItemSaveTab
            // 
            this.menuItemSaveTab.Name = "menuItemSaveTab";
            this.menuItemSaveTab.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.menuItemSaveTab.Size = new System.Drawing.Size(227, 22);
            this.menuItemSaveTab.Text = "&Save";
            this.menuItemSaveTab.Click += new System.EventHandler(this.OnTab_Save);
            // 
            // menuItemSaveAsTab
            // 
            this.menuItemSaveAsTab.Name = "menuItemSaveAsTab";
            this.menuItemSaveAsTab.Size = new System.Drawing.Size(227, 22);
            this.menuItemSaveAsTab.Text = "Save As...";
            this.menuItemSaveAsTab.Click += new System.EventHandler(this.OnTab_SaveAs);
            // 
            // menuItemSaveAllTab
            // 
            this.menuItemSaveAllTab.Name = "menuItemSaveAllTab";
            this.menuItemSaveAllTab.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.menuItemSaveAllTab.Size = new System.Drawing.Size(227, 22);
            this.menuItemSaveAllTab.Text = "Save All";
            this.menuItemSaveAllTab.Click += new System.EventHandler(this.OnTab_SaveAll);
            // 
            // toolStripSeparator14
            // 
            this.toolStripSeparator14.Name = "toolStripSeparator14";
            this.toolStripSeparator14.Size = new System.Drawing.Size(224, 6);
            // 
            // menuItemCloseTab
            // 
            this.menuItemCloseTab.Name = "menuItemCloseTab";
            this.menuItemCloseTab.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F4)));
            this.menuItemCloseTab.Size = new System.Drawing.Size(227, 22);
            this.menuItemCloseTab.Text = "&Close Tab";
            this.menuItemCloseTab.Click += new System.EventHandler(this.OnTab_Close);
            // 
            // menuItemCloseAllTabs
            // 
            this.menuItemCloseAllTabs.Name = "menuItemCloseAllTabs";
            this.menuItemCloseAllTabs.Size = new System.Drawing.Size(227, 22);
            this.menuItemCloseAllTabs.Text = "Close &All Tabs";
            this.menuItemCloseAllTabs.Click += new System.EventHandler(this.OnTab_CloseAll);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(224, 6);
            // 
            // menuItemConfiguration
            // 
            this.menuItemConfiguration.Name = "menuItemConfiguration";
            this.menuItemConfiguration.Size = new System.Drawing.Size(227, 22);
            this.menuItemConfiguration.Text = "Configuration...";
            this.menuItemConfiguration.Click += new System.EventHandler(this.OnConfigurationClicked);
            // 
            // menuItemSeparator2
            // 
            this.menuItemSeparator2.Name = "menuItemSeparator2";
            this.menuItemSeparator2.Size = new System.Drawing.Size(224, 6);
            // 
            // menuItemExit
            // 
            this.menuItemExit.Name = "menuItemExit";
            this.menuItemExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.menuItemExit.Size = new System.Drawing.Size(227, 22);
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
            this.menuItemAbout.Size = new System.Drawing.Size(126, 22);
            this.menuItemAbout.Text = "&About";
            this.menuItemAbout.Click += new System.EventHandler(this.OnHelpClicked);
            // 
            // menuItemMode
            // 
            this.menuItemMode.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemAdvanced,
            this.toolStripSeparator15,
            this.menuItemAutoBackup,
            this.toolStripSeparator17,
            this.menuItemConfirmDelete});
            this.menuItemMode.Name = "menuItemMode";
            this.menuItemMode.Size = new System.Drawing.Size(50, 20);
            this.menuItemMode.Text = "&Mode";
            // 
            // menuItemAdvanced
            // 
            this.menuItemAdvanced.CheckOnClick = true;
            this.menuItemAdvanced.Name = "menuItemAdvanced";
            this.menuItemAdvanced.Size = new System.Drawing.Size(154, 22);
            this.menuItemAdvanced.Text = "&Advanced";
            this.menuItemAdvanced.Click += new System.EventHandler(this.OnAdvancedModeChanged);
            // 
            // toolStripSeparator15
            // 
            this.toolStripSeparator15.Name = "toolStripSeparator15";
            this.toolStripSeparator15.Size = new System.Drawing.Size(151, 6);
            // 
            // menuItemAutoBackup
            // 
            this.menuItemAutoBackup.CheckOnClick = true;
            this.menuItemAutoBackup.Name = "menuItemAutoBackup";
            this.menuItemAutoBackup.Size = new System.Drawing.Size(154, 22);
            this.menuItemAutoBackup.Text = "Auto-&Backup";
            // 
            // toolStripSeparator17
            // 
            this.toolStripSeparator17.Name = "toolStripSeparator17";
            this.toolStripSeparator17.Size = new System.Drawing.Size(151, 6);
            // 
            // menuItemConfirmDelete
            // 
            this.menuItemConfirmDelete.CheckOnClick = true;
            this.menuItemConfirmDelete.Name = "menuItemConfirmDelete";
            this.menuItemConfirmDelete.Size = new System.Drawing.Size(154, 22);
            this.menuItemConfirmDelete.Text = "Confirm &Delete";
            // 
            // menuCollection
            // 
            this.menuCollection.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemCollChangeIcon,
            this.toolStripSeparator9,
            this.menuItemCollRename,
            this.toolStripSeparator8,
            this.menuItemCollDelete});
            this.menuCollection.Name = "menuCollection";
            this.menuCollection.Size = new System.Drawing.Size(73, 20);
            this.menuCollection.Text = "&Collection";
            this.menuCollection.DropDownOpening += new System.EventHandler(this.OnCollection_Opening);
            // 
            // menuItemCollChangeIcon
            // 
            this.menuItemCollChangeIcon.Name = "menuItemCollChangeIcon";
            this.menuItemCollChangeIcon.Size = new System.Drawing.Size(173, 22);
            this.menuItemCollChangeIcon.Text = "Change &Icon...";
            this.menuItemCollChangeIcon.Click += new System.EventHandler(this.OnCollection_ChangeIcon);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(170, 6);
            // 
            // menuItemCollRename
            // 
            this.menuItemCollRename.Name = "menuItemCollRename";
            this.menuItemCollRename.Size = new System.Drawing.Size(173, 22);
            this.menuItemCollRename.Text = "&Rename Package...";
            this.menuItemCollRename.Click += new System.EventHandler(this.OnCollection_RenamePackage);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(170, 6);
            // 
            // menuItemCollDelete
            // 
            this.menuItemCollDelete.Name = "menuItemCollDelete";
            this.menuItemCollDelete.Size = new System.Drawing.Size(173, 22);
            this.menuItemCollDelete.Text = "Delete";
            this.menuItemCollDelete.Click += new System.EventHandler(this.OnCollection_Delete);
            // 
            // menuReorder
            // 
            this.menuReorder.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemReorderOrderTabsBySort,
            this.menuItemReorderSetSortByOrder});
            this.menuReorder.Name = "menuReorder";
            this.menuReorder.Size = new System.Drawing.Size(60, 20);
            this.menuReorder.Text = "&Reorder";
            // 
            // menuItemReorderOrderTabsBySort
            // 
            this.menuItemReorderOrderTabsBySort.Name = "menuItemReorderOrderTabsBySort";
            this.menuItemReorderOrderTabsBySort.Size = new System.Drawing.Size(216, 22);
            this.menuItemReorderOrderTabsBySort.Text = "&Order Tabs By Sort Value";
            this.menuItemReorderOrderTabsBySort.Click += new System.EventHandler(this.OnReorder_OrderTabsBySort);
            // 
            // menuItemReorderSetSortByOrder
            // 
            this.menuItemReorderSetSortByOrder.Name = "menuItemReorderSetSortByOrder";
            this.menuItemReorderSetSortByOrder.Size = new System.Drawing.Size(216, 22);
            this.menuItemReorderSetSortByOrder.Text = "&Set Sort Value By Tab Order";
            this.menuItemReorderSetSortByOrder.Click += new System.EventHandler(this.OnReorder_SortTabsByOrder);
            // 
            // menuWindows
            // 
            this.menuWindows.Enabled = false;
            this.menuWindows.Name = "menuWindows";
            this.menuWindows.Size = new System.Drawing.Size(68, 20);
            this.menuWindows.Text = "&Windows";
            this.menuWindows.DropDownOpening += new System.EventHandler(this.OnWindowsOpening);
            // 
            // menuOptions
            // 
            this.menuOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemOpenIn,
            this.menuItemMouseDrop});
            this.menuOptions.Name = "menuOptions";
            this.menuOptions.Size = new System.Drawing.Size(61, 20);
            this.menuOptions.Text = "&Options";
            // 
            // menuItemOpenIn
            // 
            this.menuItemOpenIn.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemOpenInTab,
            this.menuItemOpenInWindow});
            this.menuItemOpenIn.Name = "menuItemOpenIn";
            this.menuItemOpenIn.Size = new System.Drawing.Size(148, 22);
            this.menuItemOpenIn.Text = "Open In...";
            // 
            // menuItemOpenInTab
            // 
            this.menuItemOpenInTab.Name = "menuItemOpenInTab";
            this.menuItemOpenInTab.Size = new System.Drawing.Size(118, 22);
            this.menuItemOpenInTab.Text = "Tab";
            this.menuItemOpenInTab.Click += new System.EventHandler(this.OnOpenInClicked);
            // 
            // menuItemOpenInWindow
            // 
            this.menuItemOpenInWindow.Name = "menuItemOpenInWindow";
            this.menuItemOpenInWindow.Size = new System.Drawing.Size(118, 22);
            this.menuItemOpenInWindow.Text = "Window";
            this.menuItemOpenInWindow.Click += new System.EventHandler(this.OnOpenInClicked);
            // 
            // menuItemMouseDrop
            // 
            this.menuItemMouseDrop.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemMouseDropBefore,
            this.menuItemMouseDropAfter,
            this.menuItemMouseDropInternal,
            this.menuItemMouseDropExternal});
            this.menuItemMouseDrop.Name = "menuItemMouseDrop";
            this.menuItemMouseDrop.Size = new System.Drawing.Size(148, 22);
            this.menuItemMouseDrop.Text = "&Mouse Drop...";
            // 
            // menuItemMouseDropBefore
            // 
            this.menuItemMouseDropBefore.Name = "menuItemMouseDropBefore";
            this.menuItemMouseDropBefore.Size = new System.Drawing.Size(115, 22);
            this.menuItemMouseDropBefore.Text = "&Before";
            this.menuItemMouseDropBefore.Click += new System.EventHandler(this.OnMouseDropClicked);
            // 
            // menuItemMouseDropAfter
            // 
            this.menuItemMouseDropAfter.Name = "menuItemMouseDropAfter";
            this.menuItemMouseDropAfter.Size = new System.Drawing.Size(115, 22);
            this.menuItemMouseDropAfter.Text = "&After";
            this.menuItemMouseDropAfter.Click += new System.EventHandler(this.OnMouseDropClicked);
            // 
            // menuItemMouseDropInternal
            // 
            this.menuItemMouseDropInternal.Name = "menuItemMouseDropInternal";
            this.menuItemMouseDropInternal.Size = new System.Drawing.Size(115, 22);
            this.menuItemMouseDropInternal.Text = "&Internal";
            this.menuItemMouseDropInternal.Click += new System.EventHandler(this.OnMouseDropClicked);
            // 
            // menuItemMouseDropExternal
            // 
            this.menuItemMouseDropExternal.Name = "menuItemMouseDropExternal";
            this.menuItemMouseDropExternal.Size = new System.Drawing.Size(115, 22);
            this.menuItemMouseDropExternal.Text = "&External";
            this.menuItemMouseDropExternal.Click += new System.EventHandler(this.OnMouseDropClicked);
            // 
            // menuCaching
            // 
            this.menuCaching.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemCachingUpdateMaxisObjects,
            this.menuItemCachingUpdateCustomObjects,
            this.toolStripSeparator4,
            this.menuItemCachingUpdateMaxisClothes,
            this.menuItemCachingUpdateCustomClothes,
            this.toolStripSeparator7,
            this.menuItemCachingRemoveThumbnails});
            this.menuCaching.Name = "menuCaching";
            this.menuCaching.Size = new System.Drawing.Size(63, 20);
            this.menuCaching.Text = "Caching";
            this.menuCaching.DropDownOpening += new System.EventHandler(this.OnCachingOpening);
            // 
            // menuItemCachingUpdateMaxisObjects
            // 
            this.menuItemCachingUpdateMaxisObjects.Name = "menuItemCachingUpdateMaxisObjects";
            this.menuItemCachingUpdateMaxisObjects.Size = new System.Drawing.Size(242, 22);
            this.menuItemCachingUpdateMaxisObjects.Text = "Update Maxis Objects Cache";
            this.menuItemCachingUpdateMaxisObjects.Click += new System.EventHandler(this.OnCachingUpdateMaxisObjects);
            // 
            // menuItemCachingUpdateCustomObjects
            // 
            this.menuItemCachingUpdateCustomObjects.Name = "menuItemCachingUpdateCustomObjects";
            this.menuItemCachingUpdateCustomObjects.Size = new System.Drawing.Size(242, 22);
            this.menuItemCachingUpdateCustomObjects.Text = "Update Custom Objects Cache";
            this.menuItemCachingUpdateCustomObjects.Click += new System.EventHandler(this.OnCachingUpdateCustomObjects);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(239, 6);
            // 
            // menuItemCachingUpdateMaxisClothes
            // 
            this.menuItemCachingUpdateMaxisClothes.Name = "menuItemCachingUpdateMaxisClothes";
            this.menuItemCachingUpdateMaxisClothes.Size = new System.Drawing.Size(242, 22);
            this.menuItemCachingUpdateMaxisClothes.Text = "Update Maxis Clothing Cache";
            this.menuItemCachingUpdateMaxisClothes.Click += new System.EventHandler(this.OnCachingUpdateMaxisOutfits);
            // 
            // menuItemCachingUpdateCustomClothes
            // 
            this.menuItemCachingUpdateCustomClothes.Name = "menuItemCachingUpdateCustomClothes";
            this.menuItemCachingUpdateCustomClothes.Size = new System.Drawing.Size(242, 22);
            this.menuItemCachingUpdateCustomClothes.Text = "Update Custom Clothing Cache";
            this.menuItemCachingUpdateCustomClothes.Click += new System.EventHandler(this.OnCachingUpdateCustomOutfits);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(239, 6);
            // 
            // menuItemCachingRemoveThumbnails
            // 
            this.menuItemCachingRemoveThumbnails.Name = "menuItemCachingRemoveThumbnails";
            this.menuItemCachingRemoveThumbnails.Size = new System.Drawing.Size(242, 22);
            this.menuItemCachingRemoveThumbnails.Text = "Remove Thumbnails Cache";
            this.menuItemCachingRemoveThumbnails.Click += new System.EventHandler(this.OnCachingRemoveThumbnails);
            // 
            // menuItemSeparator4
            // 
            this.menuItemSeparator4.Name = "menuItemSeparator4";
            this.menuItemSeparator4.Size = new System.Drawing.Size(232, 6);
            // 
            // selectFileDialog
            // 
            this.selectFileDialog.Multiselect = true;
            // 
            // tabControl
            // 
            this.tabControl.AllowDrop = true;
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 24);
            this.tabControl.MinimumSize = new System.Drawing.Size(450, 200);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(933, 495);
            this.tabControl.TabIndex = 1;
            this.tabControl.Selected += new System.Windows.Forms.TabControlEventHandler(this.OnTabChanged);
            this.tabControl.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnDragDrop_TabControl);
            this.tabControl.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnDragEnter_TabControl);
            this.tabControl.DoubleClick += new System.EventHandler(this.OnDoubleClick);
            this.tabControl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnTabControlMouseClick);
            // 
            // menuContextTab
            // 
            this.menuContextTab.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemTabContextFloat,
            this.sep1ContextMenuItem,
            this.menuItemTabContextSave,
            this.menuItemTabContextSaveAs,
            this.menuItemTabContextSaveAll,
            this.toolStripSeparator13,
            this.menuItemTabContextChangeIcon,
            this.toolStripSeparator5,
            this.menuItemTabContextRenamePackage,
            this.toolStripSeparator2,
            this.menuItemTabContextDelete,
            this.toolStripSeparator10,
            this.menuItemTabContextClose,
            this.menuItemTabContextCloseAll});
            this.menuContextTab.Name = "menuContextTab";
            this.menuContextTab.Size = new System.Drawing.Size(174, 232);
            this.menuContextTab.Text = "Tab Options";
            this.menuContextTab.Opening += new System.ComponentModel.CancelEventHandler(this.OnTab_Opening);
            // 
            // menuItemTabContextFloat
            // 
            this.menuItemTabContextFloat.Name = "menuItemTabContextFloat";
            this.menuItemTabContextFloat.Size = new System.Drawing.Size(173, 22);
            this.menuItemTabContextFloat.Text = "&Float";
            this.menuItemTabContextFloat.Click += new System.EventHandler(this.OnTab_Float);
            // 
            // sep1ContextMenuItem
            // 
            this.sep1ContextMenuItem.Name = "sep1ContextMenuItem";
            this.sep1ContextMenuItem.Size = new System.Drawing.Size(170, 6);
            // 
            // menuItemTabContextSave
            // 
            this.menuItemTabContextSave.Name = "menuItemTabContextSave";
            this.menuItemTabContextSave.Size = new System.Drawing.Size(173, 22);
            this.menuItemTabContextSave.Text = "&Save";
            this.menuItemTabContextSave.Click += new System.EventHandler(this.OnTab_Save);
            // 
            // menuItemTabContextSaveAs
            // 
            this.menuItemTabContextSaveAs.Name = "menuItemTabContextSaveAs";
            this.menuItemTabContextSaveAs.Size = new System.Drawing.Size(173, 22);
            this.menuItemTabContextSaveAs.Text = "Save As...";
            this.menuItemTabContextSaveAs.Click += new System.EventHandler(this.OnTab_SaveAs);
            // 
            // menuItemTabContextSaveAll
            // 
            this.menuItemTabContextSaveAll.Name = "menuItemTabContextSaveAll";
            this.menuItemTabContextSaveAll.Size = new System.Drawing.Size(173, 22);
            this.menuItemTabContextSaveAll.Text = "Save All";
            this.menuItemTabContextSaveAll.Click += new System.EventHandler(this.OnTab_SaveAll);
            // 
            // toolStripSeparator13
            // 
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            this.toolStripSeparator13.Size = new System.Drawing.Size(170, 6);
            // 
            // menuItemTabContextChangeIcon
            // 
            this.menuItemTabContextChangeIcon.Name = "menuItemTabContextChangeIcon";
            this.menuItemTabContextChangeIcon.Size = new System.Drawing.Size(173, 22);
            this.menuItemTabContextChangeIcon.Text = "Change &Icon...";
            this.menuItemTabContextChangeIcon.Click += new System.EventHandler(this.OnTab_ChangeIcon);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(170, 6);
            // 
            // menuItemTabContextRenamePackage
            // 
            this.menuItemTabContextRenamePackage.Name = "menuItemTabContextRenamePackage";
            this.menuItemTabContextRenamePackage.Size = new System.Drawing.Size(173, 22);
            this.menuItemTabContextRenamePackage.Text = "&Rename Package...";
            this.menuItemTabContextRenamePackage.Click += new System.EventHandler(this.OnCollection_RenamePackage);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(170, 6);
            // 
            // menuItemTabContextDelete
            // 
            this.menuItemTabContextDelete.Name = "menuItemTabContextDelete";
            this.menuItemTabContextDelete.Size = new System.Drawing.Size(173, 22);
            this.menuItemTabContextDelete.Text = "Delete";
            this.menuItemTabContextDelete.Click += new System.EventHandler(this.OnTab_Delete);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(170, 6);
            // 
            // menuItemTabContextClose
            // 
            this.menuItemTabContextClose.Name = "menuItemTabContextClose";
            this.menuItemTabContextClose.Size = new System.Drawing.Size(173, 22);
            this.menuItemTabContextClose.Text = "&Close Tab";
            this.menuItemTabContextClose.Click += new System.EventHandler(this.OnTab_Close);
            // 
            // menuItemTabContextCloseAll
            // 
            this.menuItemTabContextCloseAll.Name = "menuItemTabContextCloseAll";
            this.menuItemTabContextCloseAll.Size = new System.Drawing.Size(173, 22);
            this.menuItemTabContextCloseAll.Text = "Close &All Tabs";
            this.menuItemTabContextCloseAll.Click += new System.EventHandler(this.OnTab_CloseAll);
            // 
            // CollectionManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 519);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.menuMain);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuMain;
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "CollectionManagerForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.OnLoad);
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.menuContextTab.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuItemSelectCollection;
        private System.Windows.Forms.ToolStripMenuItem menuItemRecentCollections;
        private System.Windows.Forms.ToolStripSeparator menuItemSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuItemConfiguration;
        private System.Windows.Forms.ToolStripSeparator menuItemSeparator4;
        private System.Windows.Forms.ToolStripMenuItem menuItemExit;
        private System.Windows.Forms.ToolStripMenuItem menuHelp;
        private System.Windows.Forms.ToolStripMenuItem menuItemAbout;
        private System.Windows.Forms.OpenFileDialog selectFileDialog;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.ToolStripMenuItem menuItemCloseTab;
        private System.Windows.Forms.ToolStripMenuItem menuItemCloseAllTabs;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ContextMenuStrip menuContextTab;
        private System.Windows.Forms.ToolStripMenuItem menuItemTabContextClose;
        private System.Windows.Forms.ToolStripMenuItem menuItemTabContextRenamePackage;
        private System.Windows.Forms.ToolStripSeparator sep1ContextMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuItemTabContextCloseAll;
        private System.Windows.Forms.ToolStripMenuItem menuItemTabContextFloat;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuCaching;
        private System.Windows.Forms.ToolStripMenuItem menuItemCachingUpdateMaxisClothes;
        private System.Windows.Forms.ToolStripMenuItem menuItemCachingUpdateCustomClothes;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem menuItemCachingRemoveThumbnails;
        private System.Windows.Forms.ToolStripMenuItem menuItemCachingUpdateMaxisObjects;
        private System.Windows.Forms.ToolStripMenuItem menuItemCachingUpdateCustomObjects;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem menuItemTabContextChangeIcon;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem menuCollection;
        private System.Windows.Forms.ToolStripMenuItem menuItemCollChangeIcon;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem menuItemCollRename;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem menuItemCollDelete;
        private System.Windows.Forms.ToolStripMenuItem menuItemTabContextDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripMenuItem menuItemTabContextSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
        private System.Windows.Forms.ToolStripMenuItem menuItemSaveTab;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator14;
        private System.Windows.Forms.ToolStripMenuItem menuItemMode;
        private System.Windows.Forms.ToolStripMenuItem menuItemAdvanced;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator15;
        private System.Windows.Forms.ToolStripMenuItem menuItemAutoBackup;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator17;
        private System.Windows.Forms.ToolStripMenuItem menuItemConfirmDelete;
        private System.Windows.Forms.ToolStripMenuItem menuReorder;
        private System.Windows.Forms.ToolStripMenuItem menuItemReorderOrderTabsBySort;
        private System.Windows.Forms.ToolStripMenuItem menuItemReorderSetSortByOrder;
        private System.Windows.Forms.ToolStripMenuItem menuItemSaveAllTab;
        private System.Windows.Forms.ToolStripMenuItem menuItemTabContextSaveAll;
        private System.Windows.Forms.ToolStripMenuItem menuOptions;
        private System.Windows.Forms.ToolStripMenuItem menuItemMouseDrop;
        private System.Windows.Forms.ToolStripMenuItem menuItemMouseDropBefore;
        private System.Windows.Forms.ToolStripMenuItem menuItemMouseDropAfter;
        private System.Windows.Forms.ToolStripMenuItem menuItemMouseDropInternal;
        private System.Windows.Forms.ToolStripMenuItem menuItemMouseDropExternal;
        private System.Windows.Forms.ToolStripMenuItem menuItemOpenIn;
        private System.Windows.Forms.ToolStripMenuItem menuItemOpenInTab;
        private System.Windows.Forms.ToolStripMenuItem menuItemOpenInWindow;
        private System.Windows.Forms.ToolStripMenuItem menuWindows;
        private System.Windows.Forms.ToolStripMenuItem menuItemNewCollection;
        private System.Windows.Forms.ToolStripMenuItem menuItemSaveAsTab;
        private System.Windows.Forms.ToolStripMenuItem menuItemTabContextSaveAs;
    }
}