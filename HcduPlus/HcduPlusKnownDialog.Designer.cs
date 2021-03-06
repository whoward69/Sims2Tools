﻿/*
 * HCDU Plus - a utility for checking The Sims 2 package files for conflicts
 *           - see http://www.picknmixmods.com/Sims2/Notes/HcduPlus/HcduPlus.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2021
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HcduPlusKnownDialog));
            this.gridKnownConflicts = new System.Windows.Forms.DataGridView();
            this.colLoadsEarlier = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLoadsLater = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnKnownCancel = new System.Windows.Forms.Button();
            this.btnKnownOk = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridKnownConflicts)).BeginInit();
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
            this.gridKnownConflicts.Location = new System.Drawing.Point(12, 12);
            this.gridKnownConflicts.Name = "gridKnownConflicts";
            this.gridKnownConflicts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridKnownConflicts.Size = new System.Drawing.Size(651, 291);
            this.gridKnownConflicts.TabIndex = 0;
            this.gridKnownConflicts.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.OnRowValidating);
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
            // HcduPlusKnownDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnKnownCancel;
            this.ClientSize = new System.Drawing.Size(675, 347);
            this.Controls.Add(this.btnKnownOk);
            this.Controls.Add(this.btnKnownCancel);
            this.Controls.Add(this.gridKnownConflicts);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "HcduPlusKnownDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Known Conflicts";
            ((System.ComponentModel.ISupportInitialize)(this.gridKnownConflicts)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView gridKnownConflicts;
        private System.Windows.Forms.Button btnKnownCancel;
        private System.Windows.Forms.Button btnKnownOk;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLoadsEarlier;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLoadsLater;
    }
}