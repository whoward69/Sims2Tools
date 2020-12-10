/*
 * DBPF Test - a utility for testing the DBPF Library
 *
 * William Howard - 2020
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

namespace Sims2Tools.DBPF
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DbpfViewerForm));
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSelectPackage = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemRecentPackages = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemSaveXmlToClipboard = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSaveXmlAs = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemConfiguration = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuResources = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemBcon = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemBhav = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemCtss = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemGlob = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemObjd = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemObjf = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemStr = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemTprp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemTrcn = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemTtab = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemTtas = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemVers = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.selectFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.gridResources = new System.Windows.Forms.DataGridView();
            this.colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGroup = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInstance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHash = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.menuMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridResources)).BeginInit();
            this.SuspendLayout();
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuResources,
            this.menuHelp});
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
            this.menuItemSelectPackage,
            this.menuItemRecentPackages,
            this.menuItemSeparator1,
            this.menuItemSaveXmlToClipboard,
            this.menuItemSaveXmlAs,
            this.menuItemSeparator2,
            this.menuItemConfiguration,
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
            // menuItemSeparator1
            // 
            this.menuItemSeparator1.Name = "menuItemSeparator1";
            this.menuItemSeparator1.Size = new System.Drawing.Size(232, 6);
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
            // menuItemSeparator2
            // 
            this.menuItemSeparator2.Name = "menuItemSeparator2";
            this.menuItemSeparator2.Size = new System.Drawing.Size(232, 6);
            // 
            // menuItemConfiguration
            // 
            this.menuItemConfiguration.Name = "menuItemConfiguration";
            this.menuItemConfiguration.Size = new System.Drawing.Size(235, 22);
            this.menuItemConfiguration.Text = "Configuration...";
            this.menuItemConfiguration.Click += new System.EventHandler(this.OnConfigurationClicked);
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
            this.menuItemBcon,
            this.menuItemBhav,
            this.menuItemCtss,
            this.menuItemGlob,
            this.menuItemObjd,
            this.menuItemObjf,
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
            // menuItemBcon
            // 
            this.menuItemBcon.CheckOnClick = true;
            this.menuItemBcon.Name = "menuItemBcon";
            this.menuItemBcon.Size = new System.Drawing.Size(101, 22);
            this.menuItemBcon.Text = "Bcon";
            this.menuItemBcon.Click += new System.EventHandler(this.OnBconClicked);
            // 
            // menuItemBhav
            // 
            this.menuItemBhav.CheckOnClick = true;
            this.menuItemBhav.Name = "menuItemBhav";
            this.menuItemBhav.Size = new System.Drawing.Size(101, 22);
            this.menuItemBhav.Text = "Bhav";
            this.menuItemBhav.Click += new System.EventHandler(this.OnBhavClicked);
            // 
            // menuItemCtss
            // 
            this.menuItemCtss.CheckOnClick = true;
            this.menuItemCtss.Name = "menuItemCtss";
            this.menuItemCtss.Size = new System.Drawing.Size(101, 22);
            this.menuItemCtss.Text = "Ctss";
            this.menuItemCtss.Click += new System.EventHandler(this.OnCtssClicked);
            // 
            // menuItemGlob
            // 
            this.menuItemGlob.CheckOnClick = true;
            this.menuItemGlob.Name = "menuItemGlob";
            this.menuItemGlob.Size = new System.Drawing.Size(101, 22);
            this.menuItemGlob.Text = "Glob";
            this.menuItemGlob.Click += new System.EventHandler(this.OnGlobClicked);
            // 
            // menuItemObjd
            // 
            this.menuItemObjd.CheckOnClick = true;
            this.menuItemObjd.Name = "menuItemObjd";
            this.menuItemObjd.Size = new System.Drawing.Size(101, 22);
            this.menuItemObjd.Text = "Objd";
            this.menuItemObjd.Click += new System.EventHandler(this.OnObjdClicked);
            // 
            // menuItemObjf
            // 
            this.menuItemObjf.CheckOnClick = true;
            this.menuItemObjf.Name = "menuItemObjf";
            this.menuItemObjf.Size = new System.Drawing.Size(101, 22);
            this.menuItemObjf.Text = "Objf";
            this.menuItemObjf.Click += new System.EventHandler(this.OnObjfClicked);
            // 
            // menuItemStr
            // 
            this.menuItemStr.CheckOnClick = true;
            this.menuItemStr.Name = "menuItemStr";
            this.menuItemStr.Size = new System.Drawing.Size(101, 22);
            this.menuItemStr.Text = "Str";
            this.menuItemStr.Click += new System.EventHandler(this.OnStrClicked);
            // 
            // menuItemTprp
            // 
            this.menuItemTprp.CheckOnClick = true;
            this.menuItemTprp.Name = "menuItemTprp";
            this.menuItemTprp.Size = new System.Drawing.Size(101, 22);
            this.menuItemTprp.Text = "Tprp";
            this.menuItemTprp.Click += new System.EventHandler(this.OnTprpClicked);
            // 
            // menuItemTrcn
            // 
            this.menuItemTrcn.CheckOnClick = true;
            this.menuItemTrcn.Name = "menuItemTrcn";
            this.menuItemTrcn.Size = new System.Drawing.Size(101, 22);
            this.menuItemTrcn.Text = "Trcn";
            this.menuItemTrcn.Click += new System.EventHandler(this.OnTrcnClicked);
            // 
            // menuItemTtab
            // 
            this.menuItemTtab.CheckOnClick = true;
            this.menuItemTtab.Name = "menuItemTtab";
            this.menuItemTtab.Size = new System.Drawing.Size(101, 22);
            this.menuItemTtab.Text = "Ttab";
            this.menuItemTtab.Click += new System.EventHandler(this.OnTtabClicked);
            // 
            // menuItemTtas
            // 
            this.menuItemTtas.CheckOnClick = true;
            this.menuItemTtas.Name = "menuItemTtas";
            this.menuItemTtas.Size = new System.Drawing.Size(101, 22);
            this.menuItemTtas.Text = "Ttas";
            this.menuItemTtas.Click += new System.EventHandler(this.OnTtasClicked);
            // 
            // menuItemVers
            // 
            this.menuItemVers.CheckOnClick = true;
            this.menuItemVers.Name = "menuItemVers";
            this.menuItemVers.Size = new System.Drawing.Size(101, 22);
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
            this.menuItemAbout.Text = "About";
            this.menuItemAbout.Click += new System.EventHandler(this.OnHelpClicked);
            // 
            // menuItemSeparator3
            // 
            this.menuItemSeparator3.Name = "menuItemSeparator3";
            this.menuItemSeparator3.Size = new System.Drawing.Size(232, 6);
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
            this.gridResources.Location = new System.Drawing.Point(14, 31);
            this.gridResources.MultiSelect = false;
            this.gridResources.Name = "gridResources";
            this.gridResources.ReadOnly = true;
            this.gridResources.RowHeadersVisible = false;
            this.gridResources.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridResources.Size = new System.Drawing.Size(905, 474);
            this.gridResources.TabIndex = 1;
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
            this.colInstance.DataPropertyName = "Instance";
            this.colInstance.HeaderText = "Instance";
            this.colInstance.MinimumWidth = 65;
            this.colInstance.Name = "colInstance";
            this.colInstance.ReadOnly = true;
            this.colInstance.Width = 65;
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
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "xml";
            this.saveFileDialog.Filter = "XML file|*.xml|All files|*.*";
            this.saveFileDialog.Title = "Save As";
            // 
            // DbpfViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 519);
            this.Controls.Add(this.gridResources);
            this.Controls.Add(this.menuMain);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuItemSelectPackage;
        private System.Windows.Forms.ToolStripMenuItem menuItemRecentPackages;
        private System.Windows.Forms.ToolStripSeparator menuItemSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuItemSaveXmlToClipboard;
        private System.Windows.Forms.ToolStripMenuItem menuItemSaveXmlAs;
        private System.Windows.Forms.ToolStripSeparator menuItemSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuItemConfiguration;
        private System.Windows.Forms.ToolStripSeparator menuItemSeparator3;
        private System.Windows.Forms.ToolStripMenuItem menuItemExit;
        private System.Windows.Forms.ToolStripMenuItem menuHelp;
        private System.Windows.Forms.ToolStripMenuItem menuItemAbout;
        private System.Windows.Forms.OpenFileDialog selectFileDialog;
        private System.Windows.Forms.DataGridView gridResources;
        private System.Windows.Forms.DataGridViewTextBoxColumn colType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGroup;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInstance;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHash;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ToolStripMenuItem menuResources;
        private System.Windows.Forms.ToolStripMenuItem menuItemBcon;
        private System.Windows.Forms.ToolStripMenuItem menuItemBhav;
        private System.Windows.Forms.ToolStripMenuItem menuItemCtss;
        private System.Windows.Forms.ToolStripMenuItem menuItemGlob;
        private System.Windows.Forms.ToolStripMenuItem menuItemObjd;
        private System.Windows.Forms.ToolStripMenuItem menuItemObjf;
        private System.Windows.Forms.ToolStripMenuItem menuItemStr;
        private System.Windows.Forms.ToolStripMenuItem menuItemTprp;
        private System.Windows.Forms.ToolStripMenuItem menuItemTrcn;
        private System.Windows.Forms.ToolStripMenuItem menuItemTtab;
        private System.Windows.Forms.ToolStripMenuItem menuItemTtas;
        private System.Windows.Forms.ToolStripMenuItem menuItemVers;
    }
}