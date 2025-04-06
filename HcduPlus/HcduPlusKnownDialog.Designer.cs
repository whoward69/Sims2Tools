/*
 * HCDU Plus - a utility for checking The Sims 2 package files for conflicts
 *           - see http://www.picknmixmods.com/Sims2/Notes/HcduPlus/HcduPlus.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2025
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */
 
namespace HcduPlus
{
    partial class HcduPlusKnownDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HcduPlusKnownDialog));
            this.gridKnownConflicts = new System.Windows.Forms.DataGridView();
            this.colLoadsEarlier = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLoadsLater = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuConflictGrid = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.btnKnownCancel = new System.Windows.Forms.Button();
            this.btnKnownOk = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemPaste = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.gridKnownConflicts)).BeginInit();
            this.menuConflictGrid.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridKnownConflicts
            // 
            this.gridKnownConflicts.AllowUserToResizeRows = false;
            this.gridKnownConflicts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridKnownConflicts.BackgroundColor = System.Drawing.SystemColors.Window;
            this.gridKnownConflicts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridKnownConflicts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colLoadsEarlier,
            this.colLoadsLater});
            this.gridKnownConflicts.ContextMenuStrip = this.menuConflictGrid;
            this.gridKnownConflicts.Location = new System.Drawing.Point(12, 12);
            this.gridKnownConflicts.Name = "gridKnownConflicts";
            this.gridKnownConflicts.RowHeadersVisible = false;
            this.gridKnownConflicts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridKnownConflicts.Size = new System.Drawing.Size(651, 291);
            this.gridKnownConflicts.TabIndex = 0;
            this.gridKnownConflicts.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellMouseEnter);
            this.gridKnownConflicts.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.OnRowValidating);
            this.gridKnownConflicts.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // colLoadsEarlier
            // 
            this.colLoadsEarlier.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.colLoadsEarlier.DataPropertyName = "Loads Earlier";
            this.colLoadsEarlier.HeaderText = "Loads Earlier";
            this.colLoadsEarlier.Name = "colLoadsEarlier";
            this.colLoadsEarlier.Width = 105;
            // 
            // colLoadsLater
            // 
            this.colLoadsLater.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colLoadsLater.DataPropertyName = "Loads Later";
            this.colLoadsLater.HeaderText = "Loads Later";
            this.colLoadsLater.Name = "colLoadsLater";
            // 
            // menuConflictGrid
            // 
            this.menuConflictGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemRemove,
            this.toolStripSeparator1,
            this.menuItemPaste});
            this.menuConflictGrid.Name = "menuConflictGrid";
            this.menuConflictGrid.Size = new System.Drawing.Size(181, 76);
            this.menuConflictGrid.Closing += new System.Windows.Forms.ToolStripDropDownClosingEventHandler(this.OnConflictMenuClosing);
            this.menuConflictGrid.Opening += new System.ComponentModel.CancelEventHandler(this.OnConflictMenuOpening);
            // 
            // menuItemRemove
            // 
            this.menuItemRemove.Name = "menuItemRemove";
            this.menuItemRemove.Size = new System.Drawing.Size(180, 22);
            this.menuItemRemove.Text = "Remove";
            this.menuItemRemove.Click += new System.EventHandler(this.OnRemoveKnownConflictClicked);
            // 
            // btnKnownCancel
            // 
            this.btnKnownCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnKnownCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnKnownCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnKnownCancel.Location = new System.Drawing.Point(371, 309);
            this.btnKnownCancel.Name = "btnKnownCancel";
            this.btnKnownCancel.Size = new System.Drawing.Size(143, 30);
            this.btnKnownCancel.TabIndex = 1;
            this.btnKnownCancel.Text = "Cancel";
            this.btnKnownCancel.UseVisualStyleBackColor = true;
            // 
            // btnKnownOk
            // 
            this.btnKnownOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnKnownOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnKnownOk.Enabled = false;
            this.btnKnownOk.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnKnownOk.Location = new System.Drawing.Point(520, 309);
            this.btnKnownOk.Name = "btnKnownOk";
            this.btnKnownOk.Size = new System.Drawing.Size(143, 30);
            this.btnKnownOk.TabIndex = 2;
            this.btnKnownOk.Text = "OK";
            this.btnKnownOk.UseVisualStyleBackColor = true;
            this.btnKnownOk.Click += new System.EventHandler(this.OnOkClicked);
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReset.Location = new System.Drawing.Point(12, 309);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(143, 30);
            this.btnReset.TabIndex = 3;
            this.btnReset.Text = "&Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.OnResetClicked);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // menuItemPaste
            // 
            this.menuItemPaste.Name = "menuItemPaste";
            this.menuItemPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.menuItemPaste.Size = new System.Drawing.Size(180, 22);
            this.menuItemPaste.Text = "Paste";
            this.menuItemPaste.Click += new System.EventHandler(this.OnPasteKnownConflictClicked);
            // 
            // HcduPlusKnownDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnKnownCancel;
            this.ClientSize = new System.Drawing.Size(675, 347);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnKnownOk);
            this.Controls.Add(this.btnKnownCancel);
            this.Controls.Add(this.gridKnownConflicts);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "HcduPlusKnownDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Known Conflicts";
            ((System.ComponentModel.ISupportInitialize)(this.gridKnownConflicts)).EndInit();
            this.menuConflictGrid.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView gridKnownConflicts;
        private System.Windows.Forms.Button btnKnownCancel;
        private System.Windows.Forms.Button btnKnownOk;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.ContextMenuStrip menuConflictGrid;
        private System.Windows.Forms.ToolStripMenuItem menuItemRemove;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLoadsEarlier;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLoadsLater;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuItemPaste;
    }
}