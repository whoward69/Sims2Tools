/*
 * Hood Exporter - a utility for exporting a Sims 2 'hood as XML
 *               - see http://www.picknmixmods.com/Sims2/Notes/HoodExporter/HoodExporter.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */
 
using Microsoft.WindowsAPICodePack.Dialogs;

namespace HoodExporter
{
    partial class HoodExporterForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HoodExporterForm));
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.selectHoodFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectRecentHoodsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemSelect = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemConfiguration = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.menuResources = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemLots = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemFamilies = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSims = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemPrettyPrint = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemLotImages = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemFamilyImages = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSimImages = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemSaveAsPng = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSaveAsJpg = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLanguage = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTransform = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemXsltNone = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.hoodWorker = new System.ComponentModel.BackgroundWorker();
            this.lblHoodPath = new System.Windows.Forms.Label();
            this.textHoodPath = new System.Windows.Forms.TextBox();
            this.btnSelectHoodPath = new System.Windows.Forms.Button();
            this.lblProgress = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.btnGO = new System.Windows.Forms.Button();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.btnSelectSavePath = new System.Windows.Forms.Button();
            this.textSavePath = new System.Windows.Forms.TextBox();
            this.lblSavePath = new System.Windows.Forms.Label();
            this.menuMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuHelp,
            this.menuResources,
            this.menuOptions,
            this.menuLanguage,
            this.menuTransform});
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
            this.selectHoodFolderToolStripMenuItem,
            this.selectRecentHoodsToolStripMenuItem,
            this.menuItemSeparator2,
            this.menuItemSelect,
            this.toolStripSeparator1,
            this.menuItemConfiguration,
            this.toolStripSeparator2,
            this.menuItemExit});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(37, 20);
            this.menuFile.Text = "&File";
            // 
            // selectHoodFolderToolStripMenuItem
            // 
            this.selectHoodFolderToolStripMenuItem.Name = "selectHoodFolderToolStripMenuItem";
            this.selectHoodFolderToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.selectHoodFolderToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
            this.selectHoodFolderToolStripMenuItem.Text = "&Neighbourhood Path...";
            this.selectHoodFolderToolStripMenuItem.Click += new System.EventHandler(this.OnSelectHoodPathClicked);
            // 
            // selectRecentHoodsToolStripMenuItem
            // 
            this.selectRecentHoodsToolStripMenuItem.Name = "selectRecentHoodsToolStripMenuItem";
            this.selectRecentHoodsToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
            this.selectRecentHoodsToolStripMenuItem.Text = "&Recent Neighbourhoods";
            // 
            // menuItemSeparator2
            // 
            this.menuItemSeparator2.Name = "menuItemSeparator2";
            this.menuItemSeparator2.Size = new System.Drawing.Size(235, 6);
            // 
            // menuItemSelect
            // 
            this.menuItemSelect.Name = "menuItemSelect";
            this.menuItemSelect.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.menuItemSelect.Size = new System.Drawing.Size(238, 22);
            this.menuItemSelect.Text = "&Save Path...";
            this.menuItemSelect.Click += new System.EventHandler(this.OnSelectSavePathClicked);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(235, 6);
            // 
            // menuItemConfiguration
            // 
            this.menuItemConfiguration.Name = "menuItemConfiguration";
            this.menuItemConfiguration.Size = new System.Drawing.Size(238, 22);
            this.menuItemConfiguration.Text = "Configuration...";
            this.menuItemConfiguration.Click += new System.EventHandler(this.OnConfigClicked);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(235, 6);
            // 
            // menuItemExit
            // 
            this.menuItemExit.Name = "menuItemExit";
            this.menuItemExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.menuItemExit.Size = new System.Drawing.Size(238, 22);
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
            this.menuItemLots,
            this.menuItemFamilies,
            this.menuItemSims,
            this.toolStripSeparator4,
            this.menuItemPrettyPrint});
            this.menuResources.Name = "menuResources";
            this.menuResources.Size = new System.Drawing.Size(57, 20);
            this.menuResources.Text = "&Output";
            // 
            // menuItemLots
            // 
            this.menuItemLots.CheckOnClick = true;
            this.menuItemLots.Name = "menuItemLots";
            this.menuItemLots.Size = new System.Drawing.Size(160, 22);
            this.menuItemLots.Text = "&Lots";
            this.menuItemLots.Click += new System.EventHandler(this.OnLotsClicked);
            // 
            // menuItemFamilies
            // 
            this.menuItemFamilies.CheckOnClick = true;
            this.menuItemFamilies.Name = "menuItemFamilies";
            this.menuItemFamilies.Size = new System.Drawing.Size(160, 22);
            this.menuItemFamilies.Text = "&Families";
            this.menuItemFamilies.Click += new System.EventHandler(this.OnFamiliesClicked);
            // 
            // menuItemSims
            // 
            this.menuItemSims.CheckOnClick = true;
            this.menuItemSims.Name = "menuItemSims";
            this.menuItemSims.Size = new System.Drawing.Size(160, 22);
            this.menuItemSims.Text = "&Sims";
            this.menuItemSims.Click += new System.EventHandler(this.OnSimsClicked);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(157, 6);
            // 
            // menuItemPrettyPrint
            // 
            this.menuItemPrettyPrint.Name = "menuItemPrettyPrint";
            this.menuItemPrettyPrint.Size = new System.Drawing.Size(160, 22);
            this.menuItemPrettyPrint.Text = "&Pretty Print XML";
            // 
            // menuOptions
            // 
            this.menuOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemLotImages,
            this.menuItemFamilyImages,
            this.menuItemSimImages,
            this.toolStripSeparator3,
            this.menuItemSaveAsPng,
            this.menuItemSaveAsJpg});
            this.menuOptions.Name = "menuOptions";
            this.menuOptions.Size = new System.Drawing.Size(57, 20);
            this.menuOptions.Text = "&Images";
            // 
            // menuItemLotImages
            // 
            this.menuItemLotImages.CheckOnClick = true;
            this.menuItemLotImages.Name = "menuItemLotImages";
            this.menuItemLotImages.Size = new System.Drawing.Size(188, 22);
            this.menuItemLotImages.Text = "Extract &Lot Images";
            // 
            // menuItemFamilyImages
            // 
            this.menuItemFamilyImages.CheckOnClick = true;
            this.menuItemFamilyImages.Name = "menuItemFamilyImages";
            this.menuItemFamilyImages.Size = new System.Drawing.Size(188, 22);
            this.menuItemFamilyImages.Text = "Extract &Family Images";
            // 
            // menuItemSimImages
            // 
            this.menuItemSimImages.CheckOnClick = true;
            this.menuItemSimImages.Name = "menuItemSimImages";
            this.menuItemSimImages.Size = new System.Drawing.Size(188, 22);
            this.menuItemSimImages.Text = "Extract &Sim Images";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(185, 6);
            // 
            // menuItemSaveAsPng
            // 
            this.menuItemSaveAsPng.CheckOnClick = true;
            this.menuItemSaveAsPng.Name = "menuItemSaveAsPng";
            this.menuItemSaveAsPng.Size = new System.Drawing.Size(188, 22);
            this.menuItemSaveAsPng.Text = "Save As &PNG";
            this.menuItemSaveAsPng.Click += new System.EventHandler(this.OnSaveAsPngClicked);
            // 
            // menuItemSaveAsJpg
            // 
            this.menuItemSaveAsJpg.CheckOnClick = true;
            this.menuItemSaveAsJpg.Name = "menuItemSaveAsJpg";
            this.menuItemSaveAsJpg.Size = new System.Drawing.Size(188, 22);
            this.menuItemSaveAsJpg.Text = "Save As &JPG";
            this.menuItemSaveAsJpg.Click += new System.EventHandler(this.OnSaveAsJpgClicked);
            // 
            // menuLanguage
            // 
            this.menuLanguage.Name = "menuLanguage";
            this.menuLanguage.Size = new System.Drawing.Size(71, 20);
            this.menuLanguage.Text = "&Language";
            // 
            // menuTransform
            // 
            this.menuTransform.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemXsltNone,
            this.toolStripSeparator5});
            this.menuTransform.Name = "menuTransform";
            this.menuTransform.Size = new System.Drawing.Size(73, 20);
            this.menuTransform.Text = "&Transform";
            // 
            // menuItemXsltNone
            // 
            this.menuItemXsltNone.Checked = true;
            this.menuItemXsltNone.CheckOnClick = true;
            this.menuItemXsltNone.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuItemXsltNone.Name = "menuItemXsltNone";
            this.menuItemXsltNone.Size = new System.Drawing.Size(103, 22);
            this.menuItemXsltNone.Tag = "";
            this.menuItemXsltNone.Text = "None";
            this.menuItemXsltNone.Click += new System.EventHandler(this.OnTransformNoneClicked);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(100, 6);
            // 
            // hoodWorker
            // 
            this.hoodWorker.WorkerReportsProgress = true;
            this.hoodWorker.WorkerSupportsCancellation = true;
            this.hoodWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.HoodWorker_DoWork);
            this.hoodWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.HoodWorker_Progress);
            this.hoodWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.HoodWorker_Completed);
            // 
            // lblHoodPath
            // 
            this.lblHoodPath.AutoSize = true;
            this.lblHoodPath.Location = new System.Drawing.Point(10, 41);
            this.lblHoodPath.Name = "lblHoodPath";
            this.lblHoodPath.Size = new System.Drawing.Size(96, 15);
            this.lblHoodPath.TabIndex = 1;
            this.lblHoodPath.Text = "Neighbourhood:";
            // 
            // textHoodPath
            // 
            this.textHoodPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textHoodPath.Location = new System.Drawing.Point(126, 38);
            this.textHoodPath.Name = "textHoodPath";
            this.textHoodPath.Size = new System.Drawing.Size(643, 21);
            this.textHoodPath.TabIndex = 2;
            this.textHoodPath.TabStop = false;
            this.textHoodPath.WordWrap = false;
            this.textHoodPath.TextChanged += new System.EventHandler(this.OnPathsChanged);
            // 
            // btnSelectHoodPath
            // 
            this.btnSelectHoodPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectHoodPath.Location = new System.Drawing.Point(775, 33);
            this.btnSelectHoodPath.Name = "btnSelectHoodPath";
            this.btnSelectHoodPath.Size = new System.Drawing.Size(143, 30);
            this.btnSelectHoodPath.TabIndex = 3;
            this.btnSelectHoodPath.Text = "&Neighbourhood...";
            this.btnSelectHoodPath.UseVisualStyleBackColor = true;
            this.btnSelectHoodPath.Click += new System.EventHandler(this.OnSelectHoodPathClicked);
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
            this.btnGO.Text = "&EXPORT";
            this.btnGO.UseVisualStyleBackColor = true;
            this.btnGO.Click += new System.EventHandler(this.OnGoClicked);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "txt";
            this.saveFileDialog.Filter = "Normal text file|*.txt|All files|*.*";
            this.saveFileDialog.Title = "Save As";
            // 
            // btnSelectSavePath
            // 
            this.btnSelectSavePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectSavePath.Location = new System.Drawing.Point(775, 68);
            this.btnSelectSavePath.Name = "btnSelectSavePath";
            this.btnSelectSavePath.Size = new System.Drawing.Size(143, 30);
            this.btnSelectSavePath.TabIndex = 10;
            this.btnSelectSavePath.Text = "&Save Folder...";
            this.btnSelectSavePath.UseVisualStyleBackColor = true;
            this.btnSelectSavePath.Click += new System.EventHandler(this.OnSelectSavePathClicked);
            // 
            // textSavePath
            // 
            this.textSavePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textSavePath.Location = new System.Drawing.Point(126, 73);
            this.textSavePath.Name = "textSavePath";
            this.textSavePath.Size = new System.Drawing.Size(643, 21);
            this.textSavePath.TabIndex = 9;
            this.textSavePath.TabStop = false;
            this.textSavePath.WordWrap = false;
            this.textSavePath.TextChanged += new System.EventHandler(this.OnPathsChanged);
            // 
            // lblSavePath
            // 
            this.lblSavePath.AutoSize = true;
            this.lblSavePath.Location = new System.Drawing.Point(10, 76);
            this.lblSavePath.Name = "lblSavePath";
            this.lblSavePath.Size = new System.Drawing.Size(75, 15);
            this.lblSavePath.TabIndex = 8;
            this.lblSavePath.Text = "Save Folder:";
            // 
            // HoodExporterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 156);
            this.Controls.Add(this.btnSelectSavePath);
            this.Controls.Add(this.textSavePath);
            this.Controls.Add(this.lblSavePath);
            this.Controls.Add(this.btnGO);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.btnSelectHoodPath);
            this.Controls.Add(this.textHoodPath);
            this.Controls.Add(this.lblHoodPath);
            this.Controls.Add(this.menuMain);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuMain;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(800, 195);
            this.Name = "HoodExporterForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.OnLoad);
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblHoodPath;
        private System.Windows.Forms.TextBox textHoodPath;
        private System.Windows.Forms.Button btnSelectHoodPath;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button btnGO;
        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuItemSelect;
        private System.Windows.Forms.ToolStripSeparator menuItemSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuItemConfiguration;
        private System.Windows.Forms.ToolStripMenuItem menuItemExit;
        private System.Windows.Forms.ToolStripMenuItem menuHelp;
        private System.Windows.Forms.ToolStripMenuItem menuItemAbout;
        private CommonOpenFileDialog selectPathDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.ComponentModel.BackgroundWorker hoodWorker;
        private System.Windows.Forms.Button btnSelectSavePath;
        private System.Windows.Forms.TextBox textSavePath;
        private System.Windows.Forms.Label lblSavePath;
        private System.Windows.Forms.ToolStripMenuItem selectHoodFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuResources;
        private System.Windows.Forms.ToolStripMenuItem menuItemSims;
        private System.Windows.Forms.ToolStripMenuItem menuItemLots;
        private System.Windows.Forms.ToolStripMenuItem selectRecentHoodsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuOptions;
        private System.Windows.Forms.ToolStripMenuItem menuItemSaveAsPng;
        private System.Windows.Forms.ToolStripMenuItem menuItemSaveAsJpg;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem menuItemSimImages;
        private System.Windows.Forms.ToolStripMenuItem menuItemLotImages;
        private System.Windows.Forms.ToolStripMenuItem menuItemFamilyImages;
        private System.Windows.Forms.ToolStripMenuItem menuItemFamilies;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem menuItemPrettyPrint;
        private System.Windows.Forms.ToolStripMenuItem menuLanguage;
        private System.Windows.Forms.ToolStripMenuItem menuTransform;
        private System.Windows.Forms.ToolStripMenuItem menuItemXsltNone;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
    }
}

