
namespace LogWatcher.Controls
{
    partial class LogViewer
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
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.treeView = new System.Windows.Forms.TreeView();
            this.panelMultiView = new System.Windows.Forms.Panel();
            this.textBox = new System.Windows.Forms.RichTextBox();
            this.gridLotObjects = new System.Windows.Forms.DataGridView();
            this.colLotObjId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLotObjName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLotRoom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLotContainer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLotSlot = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridAttributes = new System.Windows.Forms.DataGridView();
            this.toolTipTextBox = new System.Windows.Forms.ToolTip(this.components);
            this.colAttrIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAttrKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAttrValueDec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAttrValueHex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.panelMultiView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridLotObjects)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridAttributes)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.treeView);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.panelMultiView);
            this.splitContainer.Size = new System.Drawing.Size(633, 448);
            this.splitContainer.SplitterDistance = 210;
            this.splitContainer.TabIndex = 0;
            // 
            // treeView
            // 
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(210, 448);
            this.treeView.TabIndex = 0;
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnTreeNodeClicked);
            // 
            // panelMultiView
            // 
            this.panelMultiView.Controls.Add(this.textBox);
            this.panelMultiView.Controls.Add(this.gridLotObjects);
            this.panelMultiView.Controls.Add(this.gridAttributes);
            this.panelMultiView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMultiView.Location = new System.Drawing.Point(0, 0);
            this.panelMultiView.Name = "panelMultiView";
            this.panelMultiView.Size = new System.Drawing.Size(419, 448);
            this.panelMultiView.TabIndex = 0;
            // 
            // textBox
            // 
            this.textBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox.Location = new System.Drawing.Point(0, 0);
            this.textBox.Name = "textBox";
            this.textBox.ReadOnly = true;
            this.textBox.Size = new System.Drawing.Size(419, 448);
            this.textBox.TabIndex = 0;
            this.textBox.Text = "";
            this.textBox.WordWrap = false;
            this.textBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OnTextBoxMouseMove);
            // 
            // gridLotObjects
            // 
            this.gridLotObjects.AllowUserToAddRows = false;
            this.gridLotObjects.AllowUserToDeleteRows = false;
            this.gridLotObjects.AllowUserToOrderColumns = true;
            this.gridLotObjects.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridLotObjects.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colLotObjId,
            this.colLotObjName,
            this.colLotRoom,
            this.colLotContainer,
            this.colLotSlot});
            this.gridLotObjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridLotObjects.Location = new System.Drawing.Point(0, 0);
            this.gridLotObjects.Name = "gridLotObjects";
            this.gridLotObjects.ReadOnly = true;
            this.gridLotObjects.RowHeadersVisible = false;
            this.gridLotObjects.Size = new System.Drawing.Size(419, 448);
            this.gridLotObjects.TabIndex = 1;
            this.gridLotObjects.Visible = false;
            // 
            // colLotObjId
            // 
            this.colLotObjId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colLotObjId.HeaderText = "Id";
            this.colLotObjId.Name = "colLotObjId";
            this.colLotObjId.ReadOnly = true;
            this.colLotObjId.Width = 41;
            // 
            // colLotObjName
            // 
            this.colLotObjName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colLotObjName.HeaderText = "Object";
            this.colLotObjName.Name = "colLotObjName";
            this.colLotObjName.ReadOnly = true;
            // 
            // colLotRoom
            // 
            this.colLotRoom.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colLotRoom.HeaderText = "Room";
            this.colLotRoom.Name = "colLotRoom";
            this.colLotRoom.ReadOnly = true;
            this.colLotRoom.Width = 60;
            // 
            // colLotContainer
            // 
            this.colLotContainer.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colLotContainer.HeaderText = "Container";
            this.colLotContainer.Name = "colLotContainer";
            this.colLotContainer.ReadOnly = true;
            this.colLotContainer.Width = 77;
            // 
            // colLotSlot
            // 
            this.colLotSlot.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colLotSlot.HeaderText = "Slot";
            this.colLotSlot.Name = "colLotSlot";
            this.colLotSlot.ReadOnly = true;
            this.colLotSlot.Width = 50;
            // 
            // gridAttributes
            // 
            this.gridAttributes.AllowUserToAddRows = false;
            this.gridAttributes.AllowUserToDeleteRows = false;
            this.gridAttributes.AllowUserToOrderColumns = true;
            this.gridAttributes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridAttributes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colAttrIndex,
            this.colAttrKey,
            this.colAttrValueDec,
            this.colAttrValueHex});
            this.gridAttributes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridAttributes.Location = new System.Drawing.Point(0, 0);
            this.gridAttributes.Name = "gridAttributes";
            this.gridAttributes.ReadOnly = true;
            this.gridAttributes.RowHeadersVisible = false;
            this.gridAttributes.Size = new System.Drawing.Size(419, 448);
            this.gridAttributes.TabIndex = 1;
            this.gridAttributes.Visible = false;
            // 
            // colAttrIndex
            // 
            this.colAttrIndex.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colAttrIndex.HeaderText = "Index";
            this.colAttrIndex.Name = "colAttrIndex";
            this.colAttrIndex.ReadOnly = true;
            this.colAttrIndex.Width = 58;
            // 
            // colAttrKey
            // 
            this.colAttrKey.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colAttrKey.HeaderText = "Name";
            this.colAttrKey.Name = "colAttrKey";
            this.colAttrKey.ReadOnly = true;
            this.colAttrKey.Width = 60;
            // 
            // colAttrValueDec
            // 
            this.colAttrValueDec.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colAttrValueDec.HeaderText = "Value";
            this.colAttrValueDec.Name = "colAttrValueDec";
            this.colAttrValueDec.ReadOnly = true;
            // 
            // colAttrValueHex
            // 
            this.colAttrValueHex.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colAttrValueHex.HeaderText = "Hex";
            this.colAttrValueHex.Name = "colAttrValueHex";
            this.colAttrValueHex.ReadOnly = true;
            // 
            // LogViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer);
            this.Name = "LogViewer";
            this.Size = new System.Drawing.Size(633, 448);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.panelMultiView.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridLotObjects)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridAttributes)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.Panel panelMultiView;
        private System.Windows.Forms.RichTextBox textBox;
        private System.Windows.Forms.DataGridView gridLotObjects;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLotObjId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLotObjName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLotRoom;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLotContainer;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLotSlot;
        private System.Windows.Forms.DataGridView gridAttributes;
        private System.Windows.Forms.ToolTip toolTipTextBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAttrIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAttrKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAttrValueDec;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAttrValueHex;
    }
}
