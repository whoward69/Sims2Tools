/*
 * Outfit Organiser - a utility for organising Sims 2 outfits (clothing etc)
 *                  - see http://www.picknmixmods.com/Sims2/Notes/OutfitOrganiser/OutfitOrganiser.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2025
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Microsoft.WindowsAPICodePack.Dialogs;

namespace OutfitOrganiser
{
    partial class OutfitOrganiserMeshesDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OutfitOrganiserMeshesDialog));
            this.btnOK = new System.Windows.Forms.Button();
            this.gridMeshFiles = new System.Windows.Forms.DataGridView();
            this.colPackageName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSubsets = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCresName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colShpeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTxmtName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPackagePath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPackageIcon = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCresPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colShpePath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTxmtPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.thumbBox = new System.Windows.Forms.PictureBox();
            this.menuMeshDialog = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSaveToFile = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemShowSubsets = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemShowMeshes = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemShowShapes = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemShowTextures = new System.Windows.Forms.ToolStripMenuItem();
            this.saveCsvDialog = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.gridMeshFiles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.thumbBox)).BeginInit();
            this.menuMeshDialog.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Location = new System.Drawing.Point(673, 363);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(143, 30);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // gridMeshFiles
            // 
            this.gridMeshFiles.AllowUserToAddRows = false;
            this.gridMeshFiles.AllowUserToDeleteRows = false;
            this.gridMeshFiles.AllowUserToOrderColumns = true;
            this.gridMeshFiles.AllowUserToResizeRows = false;
            this.gridMeshFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridMeshFiles.BackgroundColor = System.Drawing.SystemColors.Window;
            this.gridMeshFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridMeshFiles.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPackageName,
            this.colSubsets,
            this.colCresName,
            this.colShpeName,
            this.colTxmtName,
            this.colPackagePath,
            this.colPackageIcon,
            this.colCresPath,
            this.colShpePath,
            this.colTxmtPath});
            this.gridMeshFiles.Location = new System.Drawing.Point(12, 27);
            this.gridMeshFiles.Name = "gridMeshFiles";
            this.gridMeshFiles.ReadOnly = true;
            this.gridMeshFiles.RowHeadersVisible = false;
            this.gridMeshFiles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridMeshFiles.Size = new System.Drawing.Size(804, 330);
            this.gridMeshFiles.TabIndex = 7;
            this.gridMeshFiles.MultiSelectChanged += new System.EventHandler(this.OnMeshesSelectionChanged);
            this.gridMeshFiles.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellMouseEnter);
            this.gridMeshFiles.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellMouseLeave);
            this.gridMeshFiles.CellToolTipTextNeeded += new System.Windows.Forms.DataGridViewCellToolTipTextNeededEventHandler(this.OnResourceToolTipTextNeeded);
            this.gridMeshFiles.SelectionChanged += new System.EventHandler(this.OnMeshesSelectionChanged);
            // 
            // colPackageName
            // 
            this.colPackageName.DataPropertyName = "PackageName";
            this.colPackageName.HeaderText = "Package File";
            this.colPackageName.Name = "colPackageName";
            this.colPackageName.ReadOnly = true;
            this.colPackageName.Width = 150;
            // 
            // colSubsets
            // 
            this.colSubsets.DataPropertyName = "Subsets";
            this.colSubsets.HeaderText = "Subsets";
            this.colSubsets.Name = "colSubsets";
            this.colSubsets.ReadOnly = true;
            this.colSubsets.Width = 80;
            // 
            // colCresName
            // 
            this.colCresName.DataPropertyName = "CresName";
            this.colCresName.HeaderText = "Mesh (CRES) File";
            this.colCresName.Name = "colCresName";
            this.colCresName.ReadOnly = true;
            this.colCresName.Width = 150;
            // 
            // colShpeName
            // 
            this.colShpeName.DataPropertyName = "ShpeName";
            this.colShpeName.HeaderText = "Shape (SHPE) File";
            this.colShpeName.Name = "colShpeName";
            this.colShpeName.ReadOnly = true;
            this.colShpeName.Width = 150;
            // 
            // colTxmtName
            // 
            this.colTxmtName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colTxmtName.DataPropertyName = "TxmtName";
            this.colTxmtName.HeaderText = "Texture (TXMT) File(s)";
            this.colTxmtName.Name = "colTxmtName";
            this.colTxmtName.ReadOnly = true;
            // 
            // colPackagePath
            // 
            this.colPackagePath.DataPropertyName = "PackagePath";
            this.colPackagePath.HeaderText = "Package Path";
            this.colPackagePath.Name = "colPackagePath";
            this.colPackagePath.ReadOnly = true;
            this.colPackagePath.Visible = false;
            // 
            // colPackageIcon
            // 
            this.colPackageIcon.DataPropertyName = "PackageIcon";
            this.colPackageIcon.HeaderText = "Package Icon";
            this.colPackageIcon.Name = "colPackageIcon";
            this.colPackageIcon.ReadOnly = true;
            this.colPackageIcon.Visible = false;
            // 
            // colCresPath
            // 
            this.colCresPath.DataPropertyName = "CresPath";
            this.colCresPath.HeaderText = "Mesh (CRES) Path";
            this.colCresPath.Name = "colCresPath";
            this.colCresPath.ReadOnly = true;
            this.colCresPath.Visible = false;
            // 
            // colShpePath
            // 
            this.colShpePath.DataPropertyName = "ShpePath";
            this.colShpePath.HeaderText = "Shape (SHPE) Path";
            this.colShpePath.Name = "colShpePath";
            this.colShpePath.ReadOnly = true;
            this.colShpePath.Visible = false;
            // 
            // colTxmtPath
            // 
            this.colTxmtPath.DataPropertyName = "TxmtPath";
            this.colTxmtPath.HeaderText = "Texture (TXMT) Path(s)";
            this.colTxmtPath.Name = "colTxmtPath";
            this.colTxmtPath.ReadOnly = true;
            this.colTxmtPath.Visible = false;
            // 
            // thumbBox
            // 
            this.thumbBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.thumbBox.Location = new System.Drawing.Point(23, 52);
            this.thumbBox.Name = "thumbBox";
            this.thumbBox.Size = new System.Drawing.Size(192, 192);
            this.thumbBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.thumbBox.TabIndex = 25;
            this.thumbBox.TabStop = false;
            this.thumbBox.Visible = false;
            // 
            // menuMeshDialog
            // 
            this.menuMeshDialog.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.menuMeshDialog.Location = new System.Drawing.Point(0, 0);
            this.menuMeshDialog.Name = "menuMeshDialog";
            this.menuMeshDialog.Size = new System.Drawing.Size(828, 24);
            this.menuMeshDialog.TabIndex = 26;
            this.menuMeshDialog.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemSaveToFile});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // menuItemSaveToFile
            // 
            this.menuItemSaveToFile.Name = "menuItemSaveToFile";
            this.menuItemSaveToFile.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.S)));
            this.menuItemSaveToFile.Size = new System.Drawing.Size(213, 22);
            this.menuItemSaveToFile.Text = "Save CSV &As ...";
            this.menuItemSaveToFile.Click += new System.EventHandler(this.OnSaveToFile);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemShowSubsets,
            this.menuItemShowMeshes,
            this.menuItemShowShapes,
            this.menuItemShowTextures});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "&Options";
            // 
            // menuItemShowSubsets
            // 
            this.menuItemShowSubsets.CheckOnClick = true;
            this.menuItemShowSubsets.Name = "menuItemShowSubsets";
            this.menuItemShowSubsets.Size = new System.Drawing.Size(158, 22);
            this.menuItemShowSubsets.Text = "S&ubsets";
            this.menuItemShowSubsets.Click += new System.EventHandler(this.OnShowSubsetsClicked);
            // 
            // menuItemShowMeshes
            // 
            this.menuItemShowMeshes.CheckOnClick = true;
            this.menuItemShowMeshes.Name = "menuItemShowMeshes";
            this.menuItemShowMeshes.Size = new System.Drawing.Size(158, 22);
            this.menuItemShowMeshes.Text = "&Meshes (CRES)";
            this.menuItemShowMeshes.Click += new System.EventHandler(this.OnShowMeshesClicked);
            // 
            // menuItemShowShapes
            // 
            this.menuItemShowShapes.CheckOnClick = true;
            this.menuItemShowShapes.Name = "menuItemShowShapes";
            this.menuItemShowShapes.Size = new System.Drawing.Size(158, 22);
            this.menuItemShowShapes.Text = "&Shapes (SHPE)";
            this.menuItemShowShapes.Click += new System.EventHandler(this.OnShowShapesClicked);
            // 
            // menuItemShowTextures
            // 
            this.menuItemShowTextures.CheckOnClick = true;
            this.menuItemShowTextures.Name = "menuItemShowTextures";
            this.menuItemShowTextures.Size = new System.Drawing.Size(158, 22);
            this.menuItemShowTextures.Text = "&Textures (TXMT)";
            this.menuItemShowTextures.Click += new System.EventHandler(this.OnShowTexturesClicked);
            // 
            // saveCsvDialog
            // 
            this.saveCsvDialog.DefaultExt = "csv";
            this.saveCsvDialog.Filter = "CSV file|*.csv|All files|*.*";
            this.saveCsvDialog.Title = "Save As CSV";
            // 
            // OutfitOrganiserMeshesDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(828, 405);
            this.Controls.Add(this.thumbBox);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.gridMeshFiles);
            this.Controls.Add(this.menuMeshDialog);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuMeshDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OutfitOrganiserMeshesDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Related Packages";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnDialogClosing);
            this.Load += new System.EventHandler(this.OnLoad);
            ((System.ComponentModel.ISupportInitialize)(this.gridMeshFiles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.thumbBox)).EndInit();
            this.menuMeshDialog.ResumeLayout(false);
            this.menuMeshDialog.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.DataGridView gridMeshFiles;
        private System.Windows.Forms.PictureBox thumbBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPackageName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSubsets;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCresName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colShpeName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTxmtName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPackagePath;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPackageIcon;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCresPath;
        private System.Windows.Forms.DataGridViewTextBoxColumn colShpePath;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTxmtPath;
        private System.Windows.Forms.MenuStrip menuMeshDialog;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuItemSaveToFile;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuItemShowSubsets;
        private System.Windows.Forms.ToolStripMenuItem menuItemShowMeshes;
        private System.Windows.Forms.ToolStripMenuItem menuItemShowShapes;
        private System.Windows.Forms.ToolStripMenuItem menuItemShowTextures;
        private System.Windows.Forms.SaveFileDialog saveCsvDialog;
    }
}