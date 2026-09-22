/*
 * Collection Manager - a utility for managing Sims 2 collections
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

namespace CollectionManager.Controls
{
    partial class CollectionViewer
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pictCollIcon = new System.Windows.Forms.PictureBox();
            this.menuContextIcon = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemIconContextChangeIcon = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemIconContextClipboardCopyTo = new System.Windows.Forms.ToolStripMenuItem();
            this.textCollName = new System.Windows.Forms.TextBox();
            this.comboCollType = new System.Windows.Forms.ComboBox();
            this.textCollSort = new System.Windows.Forms.TextBox();
            this.lblCollSort = new System.Windows.Forms.Label();
            this.gridCollItems = new System.Windows.Forms.DataGridView();
            this.colSort = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBinxKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuContextCollItems = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemCollContextBefore = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemCollContextAfter = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemCollContextClipboardCopyTo = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemCollContextClipboardBefore = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemCollContextClipboardAfter = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemCollContextDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.thumbBox = new System.Windows.Forms.PictureBox();
            this.panelViewer = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.toolTipViewer = new System.Windows.Forms.ToolTip(this.components);
            this.saveAsFileDialog = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.pictCollIcon)).BeginInit();
            this.menuContextIcon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridCollItems)).BeginInit();
            this.menuContextCollItems.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.thumbBox)).BeginInit();
            this.panelViewer.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictCollIcon
            // 
            this.pictCollIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictCollIcon.ContextMenuStrip = this.menuContextIcon;
            this.pictCollIcon.Location = new System.Drawing.Point(3, 3);
            this.pictCollIcon.Name = "pictCollIcon";
            this.pictCollIcon.Size = new System.Drawing.Size(28, 22);
            this.pictCollIcon.TabIndex = 1;
            this.pictCollIcon.TabStop = false;
            this.pictCollIcon.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnDragDrop_Icon);
            this.pictCollIcon.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnDragEnter_Icon);
            // 
            // menuContextIcon
            // 
            this.menuContextIcon.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemIconContextChangeIcon,
            this.toolStripSeparator3,
            this.menuItemIconContextClipboardCopyTo});
            this.menuContextIcon.Name = "menuContextIcon";
            this.menuContextIcon.Size = new System.Drawing.Size(174, 54);
            // 
            // menuItemIconContextChangeIcon
            // 
            this.menuItemIconContextChangeIcon.Name = "menuItemIconContextChangeIcon";
            this.menuItemIconContextChangeIcon.Size = new System.Drawing.Size(173, 22);
            this.menuItemIconContextChangeIcon.Text = "Change Icon";
            this.menuItemIconContextChangeIcon.Click += new System.EventHandler(this.OnIconContext_ChangeIcon);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(170, 6);
            // 
            // menuItemIconContextClipboardCopyTo
            // 
            this.menuItemIconContextClipboardCopyTo.Name = "menuItemIconContextClipboardCopyTo";
            this.menuItemIconContextClipboardCopyTo.Size = new System.Drawing.Size(173, 22);
            this.menuItemIconContextClipboardCopyTo.Text = "Copy To Clipboard";
            this.menuItemIconContextClipboardCopyTo.Click += new System.EventHandler(this.OnIconContext_ClipboardCopyTo);
            // 
            // textCollName
            // 
            this.textCollName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textCollName.Location = new System.Drawing.Point(163, 4);
            this.textCollName.Name = "textCollName";
            this.textCollName.Size = new System.Drawing.Size(290, 20);
            this.textCollName.TabIndex = 2;
            this.textCollName.TextChanged += new System.EventHandler(this.OnCollNameChanged);
            // 
            // comboCollType
            // 
            this.comboCollType.FormattingEnabled = true;
            this.comboCollType.Location = new System.Drawing.Point(37, 4);
            this.comboCollType.Name = "comboCollType";
            this.comboCollType.Size = new System.Drawing.Size(120, 21);
            this.comboCollType.TabIndex = 3;
            this.comboCollType.SelectedIndexChanged += new System.EventHandler(this.OnCollTypeChanged);
            // 
            // textCollSort
            // 
            this.textCollSort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textCollSort.Location = new System.Drawing.Point(488, 4);
            this.textCollSort.Name = "textCollSort";
            this.textCollSort.Size = new System.Drawing.Size(50, 20);
            this.textCollSort.TabIndex = 4;
            this.textCollSort.TextChanged += new System.EventHandler(this.OnCollSortValueChanged);
            // 
            // lblCollSort
            // 
            this.lblCollSort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCollSort.AutoSize = true;
            this.lblCollSort.Location = new System.Drawing.Point(458, 7);
            this.lblCollSort.Name = "lblCollSort";
            this.lblCollSort.Size = new System.Drawing.Size(29, 13);
            this.lblCollSort.TabIndex = 5;
            this.lblCollSort.Text = "Sort:";
            // 
            // gridCollItems
            // 
            this.gridCollItems.AllowDrop = true;
            this.gridCollItems.AllowUserToAddRows = false;
            this.gridCollItems.AllowUserToDeleteRows = false;
            this.gridCollItems.AllowUserToResizeRows = false;
            this.gridCollItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridCollItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridCollItems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSort,
            this.colType,
            this.colName,
            this.colItemKey,
            this.colBinxKey,
            this.colKey,
            this.colData});
            this.gridCollItems.ContextMenuStrip = this.menuContextCollItems;
            this.gridCollItems.Location = new System.Drawing.Point(3, 42);
            this.gridCollItems.Name = "gridCollItems";
            this.gridCollItems.ReadOnly = true;
            this.gridCollItems.RowHeadersVisible = false;
            this.gridCollItems.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.gridCollItems.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridCollItems.Size = new System.Drawing.Size(594, 305);
            this.gridCollItems.TabIndex = 6;
            this.gridCollItems.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.OnCellMouseDown);
            this.gridCollItems.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellMouseEnter);
            this.gridCollItems.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellMouseLeave);
            this.gridCollItems.CellMouseMove += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.OnCellMouseMove);
            this.gridCollItems.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.OnCellMouseUp);
            this.gridCollItems.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnDragDrop_GridCollItems);
            this.gridCollItems.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnDragEnter_GridCollItems);
            // 
            // colSort
            // 
            this.colSort.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colSort.DataPropertyName = "Sort";
            this.colSort.HeaderText = "Sort";
            this.colSort.Name = "colSort";
            this.colSort.ReadOnly = true;
            this.colSort.Width = 51;
            // 
            // colType
            // 
            this.colType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colType.DataPropertyName = "Type";
            this.colType.HeaderText = "Type";
            this.colType.Name = "colType";
            this.colType.ReadOnly = true;
            this.colType.Width = 56;
            // 
            // colName
            // 
            this.colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colName.DataPropertyName = "Name";
            this.colName.HeaderText = "Name";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            // 
            // colItemKey
            // 
            this.colItemKey.DataPropertyName = "ItemKey";
            this.colItemKey.HeaderText = "ItemKey";
            this.colItemKey.Name = "colItemKey";
            this.colItemKey.ReadOnly = true;
            this.colItemKey.Visible = false;
            // 
            // colBinxKey
            // 
            this.colBinxKey.DataPropertyName = "BinxKey";
            this.colBinxKey.HeaderText = "BinxKey";
            this.colBinxKey.Name = "colBinxKey";
            this.colBinxKey.ReadOnly = true;
            this.colBinxKey.Visible = false;
            // 
            // colKey
            // 
            this.colKey.DataPropertyName = "Key";
            this.colKey.HeaderText = "Key";
            this.colKey.Name = "colKey";
            this.colKey.ReadOnly = true;
            this.colKey.Visible = false;
            // 
            // colData
            // 
            this.colData.DataPropertyName = "Data";
            this.colData.HeaderText = "Data";
            this.colData.Name = "colData";
            this.colData.ReadOnly = true;
            this.colData.Visible = false;
            // 
            // menuContextCollItems
            // 
            this.menuContextCollItems.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemCollContextBefore,
            this.menuItemCollContextAfter,
            this.toolStripSeparator2,
            this.menuItemCollContextClipboardCopyTo,
            this.menuItemCollContextClipboardBefore,
            this.menuItemCollContextClipboardAfter,
            this.toolStripSeparator1,
            this.menuItemCollContextDelete});
            this.menuContextCollItems.Name = "menuContextCollItems";
            this.menuContextCollItems.Size = new System.Drawing.Size(233, 148);
            this.menuContextCollItems.Opening += new System.ComponentModel.CancelEventHandler(this.OnCollItems_Opening);
            this.menuContextCollItems.Opened += new System.EventHandler(this.OnCollItemsContext_Opened);
            // 
            // menuItemCollContextBefore
            // 
            this.menuItemCollContextBefore.Name = "menuItemCollContextBefore";
            this.menuItemCollContextBefore.Size = new System.Drawing.Size(232, 22);
            this.menuItemCollContextBefore.Text = "Move Selected &BEFORE";
            this.menuItemCollContextBefore.Click += new System.EventHandler(this.OnCollItemsContext_MoveBefore);
            // 
            // menuItemCollContextAfter
            // 
            this.menuItemCollContextAfter.Name = "menuItemCollContextAfter";
            this.menuItemCollContextAfter.Size = new System.Drawing.Size(232, 22);
            this.menuItemCollContextAfter.Text = "Move Selected &AFTER";
            this.menuItemCollContextAfter.Click += new System.EventHandler(this.OnCollItemsContext_MoveAfter);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(229, 6);
            // 
            // menuItemCollContextClipboardCopyTo
            // 
            this.menuItemCollContextClipboardCopyTo.Name = "menuItemCollContextClipboardCopyTo";
            this.menuItemCollContextClipboardCopyTo.Size = new System.Drawing.Size(232, 22);
            this.menuItemCollContextClipboardCopyTo.Text = "Copy To Clipboard";
            this.menuItemCollContextClipboardCopyTo.Click += new System.EventHandler(this.OnCollItemsContext_ClipboardCopyTo);
            // 
            // menuItemCollContextClipboardBefore
            // 
            this.menuItemCollContextClipboardBefore.Name = "menuItemCollContextClipboardBefore";
            this.menuItemCollContextClipboardBefore.Size = new System.Drawing.Size(232, 22);
            this.menuItemCollContextClipboardBefore.Text = "Paste From Clipboard BEFORE";
            this.menuItemCollContextClipboardBefore.Click += new System.EventHandler(this.OnCollItemsContext_ClipboardPasteBefore);
            // 
            // menuItemCollContextClipboardAfter
            // 
            this.menuItemCollContextClipboardAfter.Name = "menuItemCollContextClipboardAfter";
            this.menuItemCollContextClipboardAfter.Size = new System.Drawing.Size(232, 22);
            this.menuItemCollContextClipboardAfter.Text = "Paste From Clipboards AFTER";
            this.menuItemCollContextClipboardAfter.Click += new System.EventHandler(this.OnCollItemsContext_ClipboardPasteAfter);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(229, 6);
            // 
            // menuItemCollContextDelete
            // 
            this.menuItemCollContextDelete.Name = "menuItemCollContextDelete";
            this.menuItemCollContextDelete.Size = new System.Drawing.Size(232, 22);
            this.menuItemCollContextDelete.Text = "Delete Selected";
            this.menuItemCollContextDelete.Click += new System.EventHandler(this.OnCollItemsContext_Delete);
            // 
            // thumbBox
            // 
            this.thumbBox.BackColor = System.Drawing.Color.WhiteSmoke;
            this.thumbBox.Location = new System.Drawing.Point(319, 30);
            this.thumbBox.Name = "thumbBox";
            this.thumbBox.Size = new System.Drawing.Size(128, 128);
            this.thumbBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.thumbBox.TabIndex = 25;
            this.thumbBox.TabStop = false;
            this.thumbBox.Visible = false;
            // 
            // panelViewer
            // 
            this.panelViewer.Controls.Add(this.btnSave);
            this.panelViewer.Controls.Add(this.thumbBox);
            this.panelViewer.Controls.Add(this.textCollSort);
            this.panelViewer.Controls.Add(this.pictCollIcon);
            this.panelViewer.Controls.Add(this.comboCollType);
            this.panelViewer.Controls.Add(this.textCollName);
            this.panelViewer.Controls.Add(this.lblCollSort);
            this.panelViewer.Controls.Add(this.gridCollItems);
            this.panelViewer.Location = new System.Drawing.Point(0, 0);
            this.panelViewer.Name = "panelViewer";
            this.panelViewer.Size = new System.Drawing.Size(600, 350);
            this.panelViewer.TabIndex = 26;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(547, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(50, 20);
            this.btnSave.TabIndex = 26;
            this.btnSave.Text = "Save";
            this.toolTipViewer.SetToolTip(this.btnSave, "Shift-click for Save All, Ctrl-click for Save As");
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.OnSaveClicked);
            // 
            // saveAsFileDialog
            // 
            this.saveAsFileDialog.Filter = "DBPF Package|*.package";
            this.saveAsFileDialog.Title = "Save As ...";
            // 
            // CollectionViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelViewer);
            this.Name = "CollectionViewer";
            this.Size = new System.Drawing.Size(600, 350);
            this.Resize += new System.EventHandler(this.OnResize);
            ((System.ComponentModel.ISupportInitialize)(this.pictCollIcon)).EndInit();
            this.menuContextIcon.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridCollItems)).EndInit();
            this.menuContextCollItems.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.thumbBox)).EndInit();
            this.panelViewer.ResumeLayout(false);
            this.panelViewer.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictCollIcon;
        private System.Windows.Forms.TextBox textCollName;
        private System.Windows.Forms.ComboBox comboCollType;
        private System.Windows.Forms.TextBox textCollSort;
        private System.Windows.Forms.Label lblCollSort;
        private System.Windows.Forms.DataGridView gridCollItems;
        private System.Windows.Forms.PictureBox thumbBox;
        private System.Windows.Forms.Panel panelViewer;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ContextMenuStrip menuContextCollItems;
        private System.Windows.Forms.ToolStripMenuItem menuItemCollContextBefore;
        private System.Windows.Forms.ToolStripMenuItem menuItemCollContextAfter;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuItemCollContextDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuItemCollContextClipboardBefore;
        private System.Windows.Forms.ToolStripMenuItem menuItemCollContextClipboardAfter;
        private System.Windows.Forms.ToolStripMenuItem menuItemCollContextClipboardCopyTo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSort;
        private System.Windows.Forms.DataGridViewTextBoxColumn colType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBinxKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn colKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn colData;
        private System.Windows.Forms.ContextMenuStrip menuContextIcon;
        private System.Windows.Forms.ToolStripMenuItem menuItemIconContextClipboardCopyTo;
        private System.Windows.Forms.ToolTip toolTipViewer;
        private System.Windows.Forms.SaveFileDialog saveAsFileDialog;
        private System.Windows.Forms.ToolStripMenuItem menuItemIconContextChangeIcon;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    }
}
