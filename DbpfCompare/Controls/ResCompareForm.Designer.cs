/*
 * DBPF Compare - a utility for comparing two DBPF packages
 *
 * William Howard - 2020-2024
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

namespace DbpfCompare.Controls
{
    partial class ResCompareForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResCompareForm));
            this.btnClose = new System.Windows.Forms.Button();
            this.btnKeepRight = new System.Windows.Forms.Button();
            this.gridResCompare = new System.Windows.Forms.DataGridView();
            this.colKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLeftValue1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLeftValue2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRightValue1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRightValue2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.comboVariations = new System.Windows.Forms.ComboBox();
            this.btnUseLeft = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridResCompare)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(718, 420);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnKeepRight
            // 
            this.btnKeepRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnKeepRight.Location = new System.Drawing.Point(612, 420);
            this.btnKeepRight.Name = "btnKeepRight";
            this.btnKeepRight.Size = new System.Drawing.Size(100, 23);
            this.btnKeepRight.TabIndex = 1;
            this.btnKeepRight.Text = "Keep Right";
            this.btnKeepRight.UseVisualStyleBackColor = true;
            this.btnKeepRight.Click += new System.EventHandler(this.OnKeepRight);
            // 
            // gridResCompare
            // 
            this.gridResCompare.AllowUserToAddRows = false;
            this.gridResCompare.AllowUserToDeleteRows = false;
            this.gridResCompare.AllowUserToResizeRows = false;
            this.gridResCompare.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridResCompare.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridResCompare.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridResCompare.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colKey,
            this.colLeftValue1,
            this.colLeftValue2,
            this.colRightValue1,
            this.colRightValue2});
            this.gridResCompare.Location = new System.Drawing.Point(5, 5);
            this.gridResCompare.MultiSelect = false;
            this.gridResCompare.Name = "gridResCompare";
            this.gridResCompare.ReadOnly = true;
            this.gridResCompare.RowHeadersVisible = false;
            this.gridResCompare.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridResCompare.Size = new System.Drawing.Size(788, 409);
            this.gridResCompare.TabIndex = 2;
            this.gridResCompare.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.OnCellPainting);
            this.gridResCompare.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.OnDataBindingComplete);
            this.gridResCompare.SelectionChanged += new System.EventHandler(this.OnSelectionChanged);
            this.gridResCompare.Sorted += new System.EventHandler(this.OnSorted);
            // 
            // colKey
            // 
            this.colKey.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colKey.DataPropertyName = "Key";
            this.colKey.HeaderText = "Key";
            this.colKey.Name = "colKey";
            this.colKey.ReadOnly = true;
            this.colKey.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colKey.Width = 31;
            // 
            // colLeftValue1
            // 
            this.colLeftValue1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colLeftValue1.DataPropertyName = "LeftValue1";
            this.colLeftValue1.HeaderText = "Left Value 1";
            this.colLeftValue1.Name = "colLeftValue1";
            this.colLeftValue1.ReadOnly = true;
            this.colLeftValue1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colLeftValue1.Width = 70;
            // 
            // colLeftValue2
            // 
            this.colLeftValue2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colLeftValue2.DataPropertyName = "LeftValue2";
            this.colLeftValue2.HeaderText = "Left Value 2";
            this.colLeftValue2.Name = "colLeftValue2";
            this.colLeftValue2.ReadOnly = true;
            this.colLeftValue2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colLeftValue2.Width = 70;
            // 
            // colRightValue1
            // 
            this.colRightValue1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colRightValue1.DataPropertyName = "RightValue1";
            this.colRightValue1.HeaderText = "Right Value 1";
            this.colRightValue1.Name = "colRightValue1";
            this.colRightValue1.ReadOnly = true;
            this.colRightValue1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colRightValue1.Width = 77;
            // 
            // colRightValue2
            // 
            this.colRightValue2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colRightValue2.DataPropertyName = "RightValue2";
            this.colRightValue2.HeaderText = "Right Value 2";
            this.colRightValue2.Name = "colRightValue2";
            this.colRightValue2.ReadOnly = true;
            this.colRightValue2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // comboVariations
            // 
            this.comboVariations.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboVariations.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboVariations.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboVariations.FormattingEnabled = true;
            this.comboVariations.Location = new System.Drawing.Point(5, 422);
            this.comboVariations.Name = "comboVariations";
            this.comboVariations.Size = new System.Drawing.Size(218, 21);
            this.comboVariations.TabIndex = 3;
            this.comboVariations.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnDropDownDrawItem);
            this.comboVariations.SelectedIndexChanged += new System.EventHandler(this.OnComboVariationsChanged);
            // 
            // btnUseLeft
            // 
            this.btnUseLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUseLeft.Location = new System.Drawing.Point(506, 420);
            this.btnUseLeft.Name = "btnUseLeft";
            this.btnUseLeft.Size = new System.Drawing.Size(100, 23);
            this.btnUseLeft.TabIndex = 4;
            this.btnUseLeft.Text = "Use Left";
            this.btnUseLeft.UseVisualStyleBackColor = true;
            this.btnUseLeft.Click += new System.EventHandler(this.OnUseLeft);
            // 
            // ResCompareForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnUseLeft);
            this.Controls.Add(this.comboVariations);
            this.Controls.Add(this.gridResCompare);
            this.Controls.Add(this.btnKeepRight);
            this.Controls.Add(this.btnClose);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ResCompareForm";
            this.Text = "ResCompareForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.OnLoad);
            this.Shown += new System.EventHandler(this.OnShow);
            ((System.ComponentModel.ISupportInitialize)(this.gridResCompare)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnKeepRight;
        private System.Windows.Forms.DataGridView gridResCompare;
        private System.Windows.Forms.ComboBox comboVariations;
        private System.Windows.Forms.Button btnUseLeft;
        private System.Windows.Forms.DataGridViewTextBoxColumn colKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLeftValue1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLeftValue2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRightValue1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRightValue2;
    }
}