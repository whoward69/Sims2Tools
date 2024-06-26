﻿/*
 * SceneGraph Plus - a utility for repairing scene graphs
 *
 * William Howard - 2020-2024
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using SceneGraphPlus.Shapes;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.CPF;
using Sims2Tools.DBPF.SceneGraph.CRES;
using Sims2Tools.DBPF.SceneGraph.GMDC;
using Sims2Tools.DBPF.SceneGraph.GMND;
using Sims2Tools.DBPF.SceneGraph.GZPS;
using Sims2Tools.DBPF.SceneGraph.IDR;
using Sims2Tools.DBPF.SceneGraph.LIFO;
using Sims2Tools.DBPF.SceneGraph.MMAT;
using Sims2Tools.DBPF.SceneGraph.RCOL;
using Sims2Tools.DBPF.SceneGraph.SHPE;
using Sims2Tools.DBPF.SceneGraph.TXMT;
using Sims2Tools.DBPF.SceneGraph.TXTR;
using Sims2Tools.DBPF.SceneGraph.XMOL;
using Sims2Tools.DBPF.SceneGraph.XTOL;
using Sims2Tools.DBPF.STR;
using Sims2Tools.DBPF.Utils;
using Sims2Tools.DBPF.XFLR;
using Sims2Tools.DBPF.XFNC;
using Sims2Tools.DBPF.XOBJ;
using Sims2Tools.DBPF.XROF;
using Sims2Tools.DbpfCache;
using Sims2Tools.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SceneGraphPlus.Surface
{
    // See - https://stackoverflow.com/questions/38747027/how-to-drag-and-move-shapes-in-c-sharp
    [System.ComponentModel.DesignerCategory("")]
    public class DrawingSurface : Control
    {
        public static readonly Color ConnectorPopupBackColour = Color.FromName(Properties.Settings.Default.ConnectorPopupBackColour);
        public static readonly Color ConnectorPopupTextColour = Color.FromName(Properties.Settings.Default.ConnectorPopupTextColour);

        public static readonly int ColumnGap = Properties.Settings.Default.ColumnGap;
        public static readonly int RowGap = Properties.Settings.Default.RowGap;

        private readonly SceneGraphPlusForm owningForm;

        private readonly Label lblTooltip = new Label();

        private readonly ContextMenuStrip menuContextBlock;
        private readonly ToolStripMenuItem menuItemContextTexture;
        private readonly ToolStripMenuItem menuItemContextDelete;
        private readonly ToolStripMenuItem menuItemContextFixTgir;
        private readonly ToolStripMenuItem menuItemContextFixFileList;
        private readonly ToolStripMenuItem menuItemContextCopySgName;
        private readonly ToolStripMenuItem menuItemContextClosePackage;

        private readonly ContextMenuStrip menuContextConnector;
        private readonly ToolStripMenuItem menuItemContextSplitMulti;

        private readonly List<AbstractGraphBlock> blocks = new List<AbstractGraphBlock>();
        private readonly List<AbstractGraphConnector> connectors = new List<AbstractGraphConnector>();

        private AbstractGraphBlock selectedBlock = null;
        private AbstractGraphBlock hoverBlock = null;
        private AbstractGraphBlock editBlock = null;
        private List<AbstractGraphBlock> dropOntoBlocks = new List<AbstractGraphBlock>();
        private AbstractGraphBlock contextBlock = null;

        private AbstractGraphConnector hoverConnector = null;
        private AbstractGraphConnector dropOntoConnector = null;
        private AbstractGraphConnector contextConnector = null;

        bool moving;
        Point previousPoint = Point.Empty;

        private bool dropToGrid = false;
        private float gridScale = 1.0f;

        private bool hideMissingBlocks = false;
        private bool connectorsOver = true;


        public DrawingSurface(SceneGraphPlusForm owningForm)
        {
            this.owningForm = owningForm;

            {
                menuContextBlock = new ContextMenuStrip();
                menuItemContextTexture = new ToolStripMenuItem();
                menuItemContextDelete = new ToolStripMenuItem();
                menuItemContextFixTgir = new ToolStripMenuItem();
                menuItemContextFixFileList = new ToolStripMenuItem();
                menuItemContextCopySgName = new ToolStripMenuItem();
                menuItemContextClosePackage = new ToolStripMenuItem();
                menuContextBlock.SuspendLayout();

                menuContextBlock.Items.AddRange(new ToolStripItem[] { menuItemContextTexture, menuItemContextDelete, menuItemContextFixTgir, menuItemContextFixFileList, menuItemContextCopySgName, menuItemContextClosePackage });
                menuContextBlock.Name = "menuContextBlock";
                menuContextBlock.Size = new Size(223, 48);
                menuContextBlock.Opening += new CancelEventHandler(OnContextBlockOpening);

                menuItemContextTexture.Name = "menuItemContextTexture";
                menuItemContextTexture.Size = new Size(222, 22);
                menuItemContextTexture.Text = "Show Texture";
                menuItemContextTexture.Click += new EventHandler(OnContextBlockTexture);

                menuItemContextDelete.Name = "menuItemContextDelete";
                menuItemContextDelete.Size = new Size(222, 22);
                menuItemContextDelete.Text = "Delete";
                menuItemContextDelete.Click += new EventHandler(OnContextBlockDelete);

                menuItemContextFixTgir.Name = "menuItemContextFixTgir";
                menuItemContextFixTgir.Size = new Size(222, 22);
                menuItemContextFixTgir.Text = "Fix TGI";
                menuItemContextFixTgir.Click += new EventHandler(OnContextBlockFixTgir);

                menuItemContextFixFileList.Name = "menuItemContextFixFileList";
                menuItemContextFixFileList.Size = new Size(222, 22);
                menuItemContextFixFileList.Text = "Fix File List";
                menuItemContextFixFileList.Click += new EventHandler(OnContextBlockFixFileList);

                menuItemContextCopySgName.Name = "menuItemContextCopySgName";
                menuItemContextCopySgName.Size = new Size(222, 22);
                menuItemContextCopySgName.Text = "Copy SG Name From Parent";
                menuItemContextCopySgName.Click += new EventHandler(OnContextBlockCopySgName);

                menuItemContextClosePackage.Name = "menuItemContextClosePackage";
                menuItemContextClosePackage.Size = new Size(222, 22);
                menuItemContextClosePackage.Text = "Close Associated Package";
                menuItemContextClosePackage.Click += new EventHandler(OnContextBlockClosePackage);

                menuContextBlock.ResumeLayout(false);
            }

            {
                menuContextConnector = new ContextMenuStrip();
                menuItemContextSplitMulti = new ToolStripMenuItem();
                menuContextConnector.SuspendLayout();

                menuContextConnector.Items.AddRange(new ToolStripItem[] { menuItemContextSplitMulti });
                menuContextConnector.Name = "menuContextConnector";
                menuContextConnector.Size = new Size(223, 48);
                menuContextConnector.Opening += new CancelEventHandler(OnContextConnectorOpening);

                menuItemContextSplitMulti.Name = "menuItemContextSplitMulti";
                menuItemContextSplitMulti.Size = new Size(222, 22);
                menuItemContextSplitMulti.Text = "Split Multi-Connector";
                menuItemContextSplitMulti.Click += new EventHandler(OnContextConnectorSplitMulti);

                menuContextConnector.ResumeLayout(false);
            }

            {
                this.Controls.Add(lblTooltip);
                lblTooltip.Visible = false;
                lblTooltip.AutoSize = true;
                lblTooltip.BorderStyle = BorderStyle.FixedSingle;
                lblTooltip.BackColor = ConnectorPopupBackColour;
                lblTooltip.ForeColor = ConnectorPopupTextColour;
            }

            DoubleBuffered = true;

            Reset(false);
        }

        public bool IsDirty
        {
            get
            {
                foreach (AbstractGraphBlock block in blocks)
                {
                    if (block.IsMissingOrClone) continue;

                    if (block.IsDirty) return true;
                }

                return false;
            }
        }

        public bool HasPendingEdits(string packagePath)
        {
            foreach (AbstractGraphBlock block in blocks)
            {
                if (block.PackagePath.Equals(packagePath))
                {
                    if (block.IsDirty || block.IsClone) return true;
                }
            }

            return false;
        }

        public bool HideMissingBlocks
        {
            set
            {
                hideMissingBlocks = value;

                Invalidate();
            }
        }

        public bool ConnectorsOver
        {
            set
            {
                connectorsOver = value;

                Invalidate();
            }
        }

        public bool DropToGrid
        {
            get => dropToGrid;
            set => dropToGrid = value;
        }

        public float GridScale
        {
            get => gridScale;
            set => gridScale = value;
        }

        public void ResizeToFit()
        {
            if (MinWidth > this.Width || MinHeight > this.Height)
            {
                this.Size = new Size(Math.Max(MinWidth, this.Width), Math.Max(MinHeight, this.Height));
            }
        }

        private readonly SortedList<AbstractGraphBlock, AbstractGraphBlock> realignedBlocks = new SortedList<AbstractGraphBlock, AbstractGraphBlock>();

        public void RealignAll()
        {
            int freeCol = ColumnGap / 2;

            realignedBlocks.Clear();

            foreach (TypeTypeID typeId in SceneGraphPlusForm.UnderstoodTypeIds)
            {
                foreach (AbstractGraphBlock block in blocks)
                {
                    if (block.TypeId == typeId)
                    {
                        if (hideMissingBlocks && block.IsMissing)
                        {
                            realignedBlocks.Add(block, block);
                        }
                        else if (!realignedBlocks.ContainsKey(block))
                        {
                            RealignChain(block, ref freeCol);

                            freeCol += ColumnGap;
                        }
                    }
                }
            }

            Invalidate();
        }

        private void RealignChain(AbstractGraphBlock startBlock, ref int freeCol)
        {
            int startCol = freeCol;
            startBlock.Centre = new Point(startCol, owningForm.RowForType(startBlock.TypeId));

            realignedBlocks.Add(startBlock, startBlock);

            foreach (AbstractGraphConnector connector in startBlock.OutConnectors)
            {
                if (hideMissingBlocks && connector.EndBlock.IsMissing)
                {

                }
                else if (!realignedBlocks.ContainsKey(connector.EndBlock))
                {
                    RealignChain(connector.EndBlock, ref freeCol);

                    freeCol += ColumnGap;
                }
            }

            if (freeCol > startCol) freeCol -= ColumnGap;
        }

        public void Add(AbstractGraphShape shape)
        {
            if (shape is AbstractGraphBlock block)
            {
                blocks.Add(block);
            }
            else if (shape is AbstractGraphConnector connector)
            {
                connectors.Add(connector);
            }
            else
            {
                throw new Exception("Unknown IShape based class");
            }
        }

        public void Remove(AbstractGraphShape shape)
        {
            if (shape is AbstractGraphBlock block)
            {
                if (block.Equals(editBlock))
                {
                    editBlock = null;

                    owningForm.UpdateEditor(editBlock);
                }

                blocks.Remove(block);
            }
            else if (shape is AbstractGraphConnector connector)
            {
                connectors.Remove(connector);
            }
            else
            {
                throw new Exception("Unknown IShape based class");
            }
        }

        public void Reset(bool flushEditor = true)
        {
            hideMissingBlocks = false;

            editBlock = null;
            selectedBlock = null;
            hoverBlock = null;
            dropOntoBlocks.Clear();
            contextBlock = null;

            hoverConnector = null;
            dropOntoConnector = null;
            contextConnector = null;

            blocks.Clear();
            connectors.Clear();

            Invalidate();

            if (flushEditor)
            {
                owningForm.UpdateEditor(editBlock);
            }
        }

        public int MinWidth => NextFreeCol - ColumnGap / 2;

        public int MinHeight => NextFreeRow - RowGap / 2;

        public int NextFreeCol
        {
            get
            {
                int highestX = 0;

                if (blocks.Count > 0)
                {
                    foreach (AbstractGraphBlock block in blocks)
                    {
                        if (hideMissingBlocks && block.IsMissing) continue;

                        if (block.Centre.X > highestX) highestX = block.Centre.X;
                    }

                    highestX = ((highestX + ColumnGap - 1) / ColumnGap) * ColumnGap;
                }

                return highestX + (ColumnGap / 2);
            }
        }


        public int NextFreeRow
        {
            get
            {
                int highestY = 0;

                if (blocks.Count > 0)
                {
                    foreach (AbstractGraphBlock block in blocks)
                    {
                        if (hideMissingBlocks && block.IsMissing) continue;

                        if (block.Centre.Y > highestY) highestY = block.Centre.Y;
                    }

                    highestY = ((highestY + RowGap - 1) / RowGap) * RowGap;
                }

                return highestY + (RowGap / 2);
            }
        }

        public void ChangeEditingSgName(string sgName, bool prefixLowerCase)
        {
            if (editBlock != null)
            {
                editBlock.SetSgFullName(sgName, prefixLowerCase);
                editBlock.SetDirty();

                foreach (AbstractGraphConnector connector in editBlock.GetInConnectors())
                {
                    connector.StartBlock.SetDirty();
                }

                if (editBlock.TypeId == Txtr.TYPE)
                {
                    if (Xflr.TYPE == editBlock.SoleParent?.TypeId)
                    {
                        // Special case - also update the associated _detail TXTR
                        AbstractGraphBlock detailTxtrBlock = editBlock.SoleParent.OutConnectorByLabel("texturetname_detail").EndBlock;

                        detailTxtrBlock.SetSgFullName($"{sgName}_detail", prefixLowerCase);
                        detailTxtrBlock.SetDirty();
                    }
                }

                FixIssues(editBlock);

                owningForm.UpdateEditor(editBlock);
            }
        }

        public void ChangeEditingName(string name)
        {
            if (editBlock != null)
            {
                editBlock.BlockName = name;
                editBlock.SetDirty();

                FixIssues(editBlock);

                owningForm.UpdateEditor(editBlock);
            }
        }

        public void FixEditingIssues()
        {
            if (editBlock != null)
            {
                FixIssues(editBlock);

                owningForm.UpdateEditor(editBlock);
            }
        }

        private void FixIssues(AbstractGraphBlock fixBlock)
        {
            fixBlock.FixIssues();

            Invalidate();
        }

        public void FixEditingTgir()
        {
            if (editBlock != null)
            {
                FixTgir(editBlock);

                owningForm.UpdateEditor(editBlock);
            }
        }

        private void FixTgir(AbstractGraphBlock fixBlock)
        {
            fixBlock.FixTgir();

            List<AbstractGraphBlock> relinkBlocks = new List<AbstractGraphBlock>();

            foreach (AbstractGraphBlock block in blocks)
            {
                if (block.IsMissing)
                {
                    if (block.Key.Equals(fixBlock.Key))
                    {
                        relinkBlocks.Add(block);
                    }
                }
            }

            foreach (AbstractGraphBlock block in relinkBlocks)
            {
                // Clone the list of in connectors, so we can modify the original within the next loop
                List<AbstractGraphConnector> inConnectors = block.GetInConnectors();
                foreach (AbstractGraphConnector connector in inConnectors)
                {
                    connector.SetEndBlock(fixBlock, false);
                    block.DisconnectFrom(connector);
                }

                block.Discard();
            }

            Invalidate();
        }

        #region Context Menu
        private void OnContextBlockOpening(object sender, CancelEventArgs e)
        {
            if (contextBlock != null && !contextBlock.IsMissingOrClone)
            {
                menuItemContextTexture.Visible = (contextBlock.TypeId == Txmt.TYPE || contextBlock.TypeId == Txtr.TYPE || contextBlock.TypeId == Lifo.TYPE);

                menuItemContextDelete.Enabled = (contextBlock.IsEditable && contextBlock.GetInConnectors().Count == 0 && contextBlock.OutConnectors.Count == 0);

                menuItemContextFixTgir.Visible = !contextBlock.IsTgirValid;
                menuItemContextFixFileList.Visible = !contextBlock.IsDirty && !contextBlock.IsFileListValid;

                menuItemContextCopySgName.Enabled = (contextBlock.SoleRcolParent?.SgBaseName != null);
                menuItemContextClosePackage.Enabled = !HasPendingEdits(contextBlock.PackagePath);
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void OnContextBlockTexture(object sender, EventArgs e)
        {
            Trace.Assert(contextBlock.TypeId == Txmt.TYPE || contextBlock.TypeId == Txtr.TYPE || contextBlock.TypeId == Lifo.TYPE, "Expected TXMT, TXTR or LIFO");

            owningForm.DisplayTexture(contextBlock);
        }

        private void OnContextBlockDelete(object sender, EventArgs e)
        {
            Trace.Assert(contextBlock.GetInConnectors().Count == 0 && contextBlock.OutConnectors.Count == 0, "Cannot delete block with connectors");
            Trace.Assert(contextBlock.IsEditable, "Cannot delete a 'read-only' block");

            contextBlock.Delete();

            if (contextBlock.Equals(editBlock))
            {
                editBlock = null;
                owningForm.UpdateEditor(editBlock);
            }
        }

        private void OnContextBlockFixTgir(object sender, EventArgs e)
        {
            FixTgir(contextBlock);
        }

        private void OnContextBlockFixFileList(object sender, EventArgs e)
        {
            FixIssues(contextBlock);
        }

        private void OnContextBlockCopySgName(object sender, EventArgs e)
        {
            string sgName = contextBlock.SoleRcolParent?.SgBaseName;

            if (sgName != null)
            {
                contextBlock.SetSgFullName(sgName, owningForm.IsPrefixLowerCase);
                contextBlock.SetDirty();

                Invalidate();

                if (contextBlock.Equals(editBlock))
                {
                    owningForm.UpdateEditor(editBlock);
                }
            }
        }

        private void OnContextBlockClosePackage(object sender, EventArgs e)
        {
            owningForm.ClosePackage(contextBlock.PackagePath);
        }

        public bool ClosePackage(string packagePathToClose)
        {
            foreach (AbstractGraphBlock block in blocks)
            {
                if (block.PackagePath.Equals(packagePathToClose))
                {
                    block.Close();
                }
            }

            Invalidate();

            return true;
        }


        private void OnContextConnectorOpening(object sender, CancelEventArgs e)
        {
            if (contextConnector != null)
            {
                if (CountIdenticalConnectors(connectors.IndexOf(contextConnector)) > 1)
                {
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void OnContextConnectorSplitMulti(object sender, EventArgs e)
        {
            int shiftMultiplier = 1;

            foreach (AbstractGraphConnector connector in connectors)
            {
                if (connector == contextConnector) continue;

                if (connector.Equals(contextConnector))
                {
                    connector.SetEndBlock(connector.EndBlock.MakeClone(new Point((ColumnGap / 8) * shiftMultiplier, (RowGap / 8) * shiftMultiplier)), false);

                    ++shiftMultiplier;
                }
            }
        }
        #endregion

        #region Keys
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey || e.KeyCode == Keys.LShiftKey || e.KeyCode == Keys.RShiftKey)
            {
                if (dropOntoBlocks.Count != 0)
                {
                    lblTooltip.Visible = false;

                    foreach (AbstractGraphBlock dropOntoBlock in dropOntoBlocks)
                    {
                        dropOntoBlock.BorderVisible = false;
                    }

                    dropOntoBlocks.Clear();

                    this.Invalidate();
                }
            }
            else if (e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.LControlKey || e.KeyCode == Keys.RControlKey)
            {
                if (dropOntoConnector != null)
                {
                    lblTooltip.Visible = false;

                    dropOntoConnector.BorderVisible = false;

                    dropOntoConnector = null;

                    this.Invalidate();
                }
            }

            base.OnKeyUp(e);
        }
        #endregion

        #region Mouse
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Left-Click to drag, Shift-Left-Click to drag and drop onto a block, Ctrl-Left-Click to drag and drop onto a connector
                for (int i = blocks.Count - 1; i >= 0; i--)
                {
                    if (hideMissingBlocks && blocks[i].IsMissing) continue;

                    if (blocks[i].HitTest(e.Location))
                    {
                        selectedBlock = blocks[i];
                        break;
                    }
                }

                if (selectedBlock != null)
                {
                    moving = true;
                    previousPoint = e.Location;

                    if (selectedBlock != editBlock && editBlock != null) editBlock.IsEditing = false;

                    editBlock = selectedBlock;
                    owningForm.UpdateEditor(editBlock);
                    editBlock.IsEditing = true;
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                lblTooltip.Visible = false;

                contextBlock = null;
                contextConnector = null;

                for (int i = blocks.Count - 1; i >= 0; i--)
                {
                    if (hideMissingBlocks && blocks[i].IsMissing) continue;

                    if (blocks[i].HitTest(e.Location))
                    {
                        contextBlock = blocks[i];
                        break;
                    }
                }

                for (int i = connectors.Count - 1; i >= 0; i--)
                {
                    if (connectors[i].HitTest(e.Location))
                    {
                        contextConnector = connectors[i];
                        break;
                    }
                }

                if (connectorsOver && contextConnector != null)
                {
                    menuContextConnector.Show(Cursor.Position);
                }
                else if (contextBlock != null)
                {
                    menuContextBlock.Show(Cursor.Position);
                }
                else if (!connectorsOver && contextConnector != null)
                {
                    menuContextConnector.Show(Cursor.Position);
                }
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (moving)
                {
                    lblTooltip.Visible = false;

                    if (Form.ModifierKeys == Keys.Shift)
                    {
                        if (dropOntoBlocks.Count != 0)
                        {
                            foreach (AbstractGraphBlock dropOntoBlock in dropOntoBlocks)
                            {
                                dropOntoBlock.BorderVisible = false;

                                // Clone the list of in connectors, so we can modify the original within the next loop
                                List<AbstractGraphConnector> inConnectors = dropOntoBlock.GetInConnectors();
                                foreach (AbstractGraphConnector connector in inConnectors)
                                {
                                    connector.SetEndBlock(selectedBlock, true);
                                    dropOntoBlock.DisconnectFrom(connector);

                                    connector.StartBlock.SetDirty();
                                }

                                if (dropOntoBlock.IsMissingOrClone)
                                {
                                    if (dropOntoBlock.GetInConnectors().Count == 0) dropOntoBlock.Discard();
                                }
                            }

                            dropOntoBlocks.Clear();

                            this.Invalidate();
                        }
                    }
                    else if (Form.ModifierKeys == Keys.Control)
                    {
                        if (dropOntoConnector != null)
                        {
                            dropOntoConnector.BorderVisible = false;

                            AbstractGraphBlock disconnectedEndBlock = dropOntoConnector.EndBlock;

                            dropOntoConnector.SetEndBlock(selectedBlock, true);

                            dropOntoConnector.StartBlock.SetDirty();

                            if (disconnectedEndBlock.IsMissingOrClone)
                            {
                                if (disconnectedEndBlock.GetInConnectors().Count == 0) disconnectedEndBlock.Discard();
                            }

                            dropOntoConnector = null;

                            this.Invalidate();
                        }
                    }

                    if (dropToGrid)
                    {
                        // This may only work for ColumnGap = RowGap = 100
                        int gridX = (int)((ColumnGap / 2) * gridScale);
                        int gridY = (int)((RowGap / 2) * gridScale);

                        int offsetX = (gridX == ColumnGap) ? ColumnGap / 2 : 0;
                        int offsetY = (gridY == RowGap) ? RowGap / 2 : 0;

                        if (gridScale <= 1.0f)
                        {
                            selectedBlock.Centre = new Point(((selectedBlock.Centre.X + gridX / 2) / gridX) * gridX - offsetX, ((selectedBlock.Centre.Y + gridY / 2) / gridY) * gridY - offsetY);
                        }
                        else
                        {
                            selectedBlock.Centre = new Point(((selectedBlock.Centre.X + gridX) / gridX) * gridX - offsetX, ((selectedBlock.Centre.Y + gridY) / gridY) * gridY - offsetY);
                        }

                        this.Invalidate();
                    }

                    selectedBlock = null;
                    moving = false;
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
            }

            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (moving)
            {
                lblTooltip.Visible = false;

                selectedBlock.Move(new Point(e.X - previousPoint.X, e.Y - previousPoint.Y));

                previousPoint = e.Location;

                if (!selectedBlock.IsMissingOrClone)
                {
                    if (Form.ModifierKeys == Keys.Shift)
                    {
                        List<AbstractGraphBlock> currentDropOntoBlocks = new List<AbstractGraphBlock>();

                        for (int i = blocks.Count - 1; i >= 0; i--)
                        {
                            AbstractGraphBlock block = blocks[i];

                            if (hideMissingBlocks && block.IsMissing) continue;

                            if (block != selectedBlock && block.GetInConnectors().Count > 0 && block.TypeId == selectedBlock.TypeId && block.HitTest(selectedBlock.Centre))
                            {
                                if (!dropOntoBlocks.Contains(block))
                                {
                                    block.BorderVisible = true;
                                }

                                currentDropOntoBlocks.Add(block);
                            }
                        }

                        foreach (AbstractGraphBlock dropOntoBlock in dropOntoBlocks)
                        {
                            if (!currentDropOntoBlocks.Contains(dropOntoBlock))
                            {
                                dropOntoBlock.BorderVisible = false;
                            }
                        }

                        dropOntoBlocks.Clear();
                        dropOntoBlocks = currentDropOntoBlocks;
                    }
                    else if (Form.ModifierKeys == Keys.Control)
                    {
                        AbstractGraphConnector currentDropOntoConnector = null;

                        for (int i = 0; i < connectors.Count; ++i)
                        {
                            AbstractGraphConnector connector = connectors[i];

                            if (connector.HitTest(selectedBlock.Centre))
                            {
                                if (connector.EndBlock.TypeId == selectedBlock.TypeId)
                                {
                                    connector.BorderVisible = true;

                                    currentDropOntoConnector = connector;

                                    if (connector != dropOntoConnector)
                                    {
                                        if (dropOntoConnector != null) dropOntoConnector.BorderVisible = false;

                                        dropOntoConnector = connector;
                                    }

                                    break;
                                }
                            }
                        }

                        if (currentDropOntoConnector == null)
                        {
                            if (dropOntoConnector != null) dropOntoConnector.BorderVisible = false;

                            dropOntoConnector = null;
                        }
                    }
                }

                this.Invalidate();
            }
            else
            {
                AbstractGraphBlock currentHoverBlock = null;

                for (int i = blocks.Count - 1; i >= 0; i--)
                {
                    if (hideMissingBlocks && blocks[i].IsMissing) continue;

                    if (blocks[i].HitTest(e.Location))
                    {
                        currentHoverBlock = blocks[i];
                        break;
                    }
                }

                if (currentHoverBlock != null)
                {
                    if (currentHoverBlock != hoverBlock)
                    {
                        if (hoverBlock != null) hoverBlock.BorderVisible = false;

                        hoverBlock = currentHoverBlock;

                        string toolTip = hoverBlock.ToolTip;

                        if (!string.IsNullOrEmpty(toolTip))
                        {
                            Point popupLocation = this.PointToClient(MousePosition);
                            popupLocation.Offset(12, 20);
                            lblTooltip.Location = popupLocation;
                            lblTooltip.Text = toolTip;
                            lblTooltip.Visible = true;
                        }

                        this.Invalidate();
                    }
                }
                else
                {
                    if (hoverBlock != null)
                    {
                        lblTooltip.Visible = false;

                        hoverBlock.BorderVisible = false;

                        hoverBlock = null;

                        this.Invalidate();
                    }
                }

                AbstractGraphConnector currentHoverConnector = null;

                if (connectorsOver || currentHoverBlock == null)
                {
                    for (int i = 0; i < connectors.Count; ++i)
                    {
                        AbstractGraphConnector connector = connectors[i];

                        if (hideMissingBlocks && (connector.StartBlock.IsMissing || connector.EndBlock.IsMissing)) continue;

                        if (connector.HitTest(e.Location))
                        {
                            currentHoverConnector = connectors[i];
                            break;
                        }
                    }
                }

                if (currentHoverConnector != null)
                {
                    if (currentHoverConnector != hoverConnector)
                    {
                        if (hoverConnector != null) hoverConnector.BorderVisible = false;

                        hoverConnector = currentHoverConnector;

                        hoverConnector.BorderVisible = true;

                        string toolTip = $", {hoverConnector.ToolTip}";

                        foreach (AbstractGraphConnector connector in connectors)
                        {
                            if (connector == hoverConnector) continue;

                            if (connector.Equals(hoverConnector))
                            {
                                toolTip = $"{toolTip}, {connector.ToolTip}";
                            }
                        }

                        toolTip = toolTip.Substring(2);

                        if (!string.IsNullOrEmpty(toolTip))
                        {
                            Point popupLocation = this.PointToClient(MousePosition);
                            popupLocation.Offset(12, 20);
                            lblTooltip.Location = popupLocation;
                            lblTooltip.Text = toolTip;
                            lblTooltip.Visible = true;
                        }

                        this.Invalidate();
                    }
                }
                else
                {
                    if (hoverConnector != null)
                    {
                        lblTooltip.Visible = false;

                        hoverConnector.BorderVisible = false;

                        hoverConnector = null;

                        this.Invalidate();
                    }
                }
            }

            base.OnMouseMove(e);
        }
        #endregion

        #region Paint
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            if (connectorsOver)
            {
                PaintBlocks(e.Graphics);
                PaintConnectors(e.Graphics);
            }
            else
            {
                PaintConnectors(e.Graphics);
                PaintBlocks(e.Graphics);
            }

            // Any selected block (being dragged) is always on top
            selectedBlock?.Draw(e.Graphics, false);

            owningForm.UpdateForm();
        }

        private void PaintBlocks(Graphics g)
        {
            foreach (AbstractGraphBlock block in blocks)
            {
                if (block != selectedBlock) block.Draw(g, hideMissingBlocks);
            }
        }

        private void PaintConnectors(Graphics g)
        {
            for (int i = 0; i < connectors.Count; ++i)
            {
                if (SeenIdenticalConnector(i)) continue;

                connectors[i].Draw(g, hideMissingBlocks, CountIdenticalConnectors(i));
            }
        }

        private bool SeenIdenticalConnector(int i)
        {
            for (int j = i - 1; j > 0; --j)
            {
                if (connectors[i].Equals(connectors[j]))
                {
                    return true;
                }
            }

            return false;
        }

        private int CountIdenticalConnectors(int i)
        {
            int count = 1;

            for (int j = i + 1; j < connectors.Count; ++j)
            {
                if (connectors[i].Equals(connectors[j]))
                {
                    ++count;
                }
            }

            return count;
        }
        #endregion

        #region Save
        public void SaveAll(bool autoBackup, bool alwaysSetNames, bool alwaysClearNames, bool prefixNames, bool prefixLowerCase)
        {
            DbpfFileCache packageCache = new DbpfFileCache();
            Dictionary<string, List<AbstractGraphBlock>> dirtyBlocks = new Dictionary<string, List<AbstractGraphBlock>>();

            // Find all the dirty blocks by package
            foreach (AbstractGraphBlock block in blocks)
            {
                if (block.IsMissingOrClone) continue;

                if (block.IsDirty)
                {
                    if (!dirtyBlocks.ContainsKey(block.PackagePath))
                    {
                        dirtyBlocks.Add(block.PackagePath, new List<AbstractGraphBlock>());
                    }

                    dirtyBlocks[block.PackagePath].Add(block);
                }
            }

            // For each package that has dirty blocks ...
            foreach (KeyValuePair<string, List<AbstractGraphBlock>> kvPair in dirtyBlocks)
            {
                CacheableDbpfFile package = packageCache.GetOrAdd(kvPair.Key);

                // ... firstly delete any blocks ...
                foreach (AbstractGraphBlock block in kvPair.Value)
                {
                    if (block.IsDeleted)
                    {
                        package.Remove(block.OriginalKey);
                    }
                }

                // ... secondly update the dirty blocks' outbound refs ...
                foreach (AbstractGraphBlock block in kvPair.Value)
                {
                    if (!block.IsDeleted)
                    {
                        DBPFResource res = package.GetResourceByKey(block.OriginalKey);
                        Trace.Assert(res != null, $"Missing resource for {block.OriginalKey}");

                        UpdateRefsToChildren(package, res, block, prefixLowerCase);

                        package.Commit(res);
                    }
                }

                // ... thirdly update any block that references the dirty blocks where the dirty block's name has changed ...
                foreach (AbstractGraphBlock block in kvPair.Value)
                {
                    if (!block.IsDeleted)
                    {
                        if (block.BlockName == null && !block.SgOriginalName.Equals(block.SgFullName))
                        {
                            UpdateRefsFromParents(packageCache, block, prefixLowerCase);
                        }
                    }
                }

                // ... fourthly update the dirty blocks' names, have to DO THIS LAST as it can change the TGIR of the block ...
                foreach (AbstractGraphBlock block in kvPair.Value)
                {
                    if (!block.IsDeleted)
                    {
                        DBPFResource res = package.GetResourceByKey(block.OriginalKey);
                        Trace.Assert(res != null, $"Missing resource for {block.OriginalKey}");

                        UpdateName(res, block, alwaysSetNames, alwaysClearNames, prefixNames, prefixLowerCase);

                        package.Remove(block.OriginalKey);

                        if (res is Rcol rcol)
                        {
                            rcol.FixTGIR();
                        }

                        package.Commit(res);
                    }
                }

                // ... lastly, mark the dirty blocks as clean
                foreach (AbstractGraphBlock block in kvPair.Value)
                {
                    block.SetClean();
                }
            }

            // Finally, save every package that was updated
            foreach (CacheableDbpfFile package in packageCache)
            {
                if (package.Update(autoBackup) == null)
                {
                    MsgBox.Show($"Error trying to update {package.PackageName}, file is probably open in SimPe!\n\nChanges are in the associated .temp file.", "Package Update Error!");
                }

                package.Close();
            }

            Invalidate();
        }

        private void UpdateName(DBPFResource res, AbstractGraphBlock block, bool alwaysSetNames, bool alwaysClearNames, bool prefixNames, bool prefixLowerCase)
        {
            // "UnderstoodTypes" - when adding a new resource type, need to update this block
            if (res is Str str)
            {
                str.SetKeyName(block.BlockName);
            }
            else if (res is Mmat)
            {
                // Name is derived from associated TXMT and can't be changed
            }
            else if (res is Cpf cpf) // Do this AFTER MMAT
            {
                if (res is Gzps || res is Xfnc || res is Xmol || res is Xtol || res is Xobj || res is Xrof || res is Xflr)
                {
                    cpf.GetItem("name").StringValue = block.BlockName;
                }
                else
                {
                    Trace.Assert(true, $"Unsupported CPF resource {res}");
                }
            }
            else if (res is Rcol rcol)
            {
                if (res is Cres || res is Shpe || res is Gmnd || res is Gmdc || res is Txmt || res is Txtr || res is Lifo)
                {
                    rcol.SetKeyName($"{block.SgBaseName}_{DBPFData.TypeName(block.TypeId).ToLower()}");

                    if (res is Cres || res is Shpe || res is Gmnd)
                    {
                        UpdateOgnName(rcol, rcol.KeyName, alwaysSetNames, alwaysClearNames, prefixNames, prefixLowerCase);
                    }
                    else if (res is Txmt txmt)
                    {
                        UpdateMaterialName(txmt, block.SgBaseName, alwaysSetNames, alwaysClearNames, prefixNames, prefixLowerCase);
                    }
                }
                else
                {
                    Trace.Assert(true, $"Unsupported RCOL resource {res}");
                }
            }
            else
            {
                Trace.Assert(true, $"Unsupported resource {res}");
            }
        }

        private void UpdateOgnName(Rcol rcol, string basename, bool alwaysSetNames, bool alwaysClearNames, bool prefixNames, bool prefixLowerCase)
        {
            if (alwaysClearNames)
            {
                rcol.OgnName = "";
            }
            else if (alwaysSetNames || !string.IsNullOrEmpty(rcol.OgnName))
            {
                basename = Hashes.StripHashFromName(basename);

                if (prefixNames)
                {
                    if (prefixLowerCase)
                    {
                        basename = $"##{rcol.GroupID.ToString().ToLower()}!{basename}";
                    }
                    else
                    {
                        basename = $"##{rcol.GroupID.ToString().ToUpper()}!{basename}";
                    }
                }

                rcol.OgnName = basename;
            }
        }

        private void UpdateMaterialName(Txmt txmt, string basename, bool alwaysSetNames, bool alwaysClearNames, bool prefixNames, bool prefixLowerCase)
        {
            if (alwaysClearNames)
            {
                txmt.MaterialDefinition.FileDescription = "";
            }
            else if (alwaysSetNames || !string.IsNullOrEmpty(txmt.MaterialDefinition.FileDescription))
            {
                if (prefixNames)
                {
                    if (prefixLowerCase)
                    {
                        basename = $"##{txmt.GroupID.ToString().ToLower()}!{basename}";
                    }
                    else
                    {
                        basename = $"##{txmt.GroupID.ToString().ToUpper()}!{basename}";
                    }
                }

                txmt.MaterialDefinition.FileDescription = basename;
            }
        }

        private void UpdateRefsToChildren(CacheableDbpfFile package, DBPFResource res, AbstractGraphBlock block, bool prefixLowerCase)
        {
            (res as Txmt)?.MaterialDefinition.ClearFiles();

            foreach (AbstractGraphConnector connector in block.OutConnectors)
            {
                Trace.Assert(connector.StartBlock == block, "Out connector is not for this block!");

                UpdateRefToChild(package, res, connector.EndBlock, connector.Index, connector.Label, prefixLowerCase);
            }
        }

        private void UpdateRefToChild(CacheableDbpfFile package, DBPFResource res, AbstractGraphBlock endBlock, int index, string label, bool prefixLowerCase)
        {
            // "UnderstoodTypes" - when adding a new resource type, need to update this block
            if (res is Str str)
            {
                Trace.Assert(endBlock.TypeId == Cres.TYPE || endBlock.TypeId == Txmt.TYPE, "Expecting CRES or TXMT for EndBlock");

                List<StrItem> items = str.LanguageItems(Sims2Tools.DBPF.Data.MetaData.Languages.Default);
                Trace.Assert(index < items.Count, "Index out of range");

                if (endBlock.TypeId == Cres.TYPE)
                {
                    Trace.Assert(str.InstanceID == (TypeInstanceID)0x0085, "CRES expected for Model Names only");
                }
                else if (endBlock.TypeId == Txmt.TYPE)
                {
                    Trace.Assert(str.InstanceID == (TypeInstanceID)0x0088, "TXMT expected for Material Names only");
                }

                items[index].Title = MakeSgName(endBlock.Key.GroupID, endBlock.SgBaseName, prefixLowerCase);
            }
            else if (res is Mmat mmat)
            {
                Trace.Assert(endBlock.TypeId == Cres.TYPE || endBlock.TypeId == Txmt.TYPE, "Expecting CRES or TXMT for EndBlock");
                if (endBlock.TypeId == Cres.TYPE)
                {
                    mmat.GetItem("modelName").StringValue = endBlock.SgFullName;
                }
                else if (endBlock.TypeId == Txmt.TYPE)
                {
                    mmat.GetItem("name").StringValue = MakeSgName(endBlock.Key.GroupID, endBlock.SgBaseName, endBlock.TypeId, prefixLowerCase);
                }
            }
            else if (res is Gzps gzps)
            {
                Trace.Assert(endBlock.TypeId == Cres.TYPE || endBlock.TypeId == Shpe.TYPE || endBlock.TypeId == Txmt.TYPE, "Expecting CRES, SHPE or TXMT for EndBlock");

                Idr idr = (Idr)package.GetResourceByKey(new DBPFKey(Idr.TYPE, gzps));
                Trace.Assert(idr != null, "Cannot find associated 3IDR");

                if (endBlock.TypeId == Cres.TYPE)
                {
                    idr.SetItem(gzps.GetItem("resourcekeyidx").UIntegerValue, endBlock.Key);
                }
                else if (endBlock.TypeId == Shpe.TYPE)
                {
                    idr.SetItem(gzps.GetItem("shapekeyidx").UIntegerValue, endBlock.Key);
                }
                else if (endBlock.TypeId == Txmt.TYPE)
                {
                    Trace.Assert(index < gzps.GetItem("numoverrides").UIntegerValue, "Override out of range");

                    idr.SetItem(gzps.GetItem($"override{index}resourcekeyidx").UIntegerValue, endBlock.Key);
                }

                package.Commit(idr);
            }
            else if (res is Xfnc xfnc)
            {
                Trace.Assert(endBlock.TypeId == Cres.TYPE, "Expecting CRES for EndBlock");

                if (endBlock.TypeId == Cres.TYPE)
                {
                    Trace.Assert(index <= 2, "Invalid XFNC CRES index");
                    if (index == 0)
                    {
                        xfnc.GetItem("post").StringValue = MakeSgName(endBlock.Key.GroupID, endBlock.SgBaseName, prefixLowerCase);
                    }
                    else if (index == 1)
                    {
                        xfnc.GetItem("rail").StringValue = MakeSgName(endBlock.Key.GroupID, endBlock.SgBaseName, prefixLowerCase);
                    }
                    else if (index == 2)
                    {
                        xfnc.GetItem("diagrail").StringValue = MakeSgName(endBlock.Key.GroupID, endBlock.SgBaseName, prefixLowerCase);
                    }
                }
            }
            else if (res is Xmol xmol)
            {
                Trace.Assert(endBlock.TypeId == Cres.TYPE || endBlock.TypeId == Shpe.TYPE || endBlock.TypeId == Txmt.TYPE, "Expecting CRES, SHPE or TXMT for EndBlock");

                Idr idr = (Idr)package.GetResourceByKey(new DBPFKey(Idr.TYPE, xmol));
                Trace.Assert(idr != null, "Cannot find associated 3IDR");

                if (endBlock.TypeId == Cres.TYPE)
                {
                    Trace.Assert(index <= 1, "Invalid XMOL CRES index");
                    if (index == 0)
                    {
                        idr.SetItem(xmol.GetItem("resourcekeyidx").UIntegerValue, endBlock.Key);
                    }
                    else if (index == 1)
                    {
                        idr.SetItem(xmol.GetItem("maskresourcekeyidx").UIntegerValue, endBlock.Key);
                    }
                }
                else if (endBlock.TypeId == Shpe.TYPE)
                {
                    Trace.Assert(index <= 1, "Invalid XMOL SHPE index");
                    if (index == 0)
                    {
                        idr.SetItem(xmol.GetItem("shapekeyidx").UIntegerValue, endBlock.Key);
                    }
                    else if (index == 1)
                    {
                        idr.SetItem(xmol.GetItem("maskshapekeyidx").UIntegerValue, endBlock.Key);
                    }
                }
                else if (endBlock.TypeId == Txmt.TYPE)
                {
                    Trace.Assert(index < xmol.GetItem("numoverrides").UIntegerValue, "Override out of range");

                    idr.SetItem(xmol.GetItem($"override{index}resourcekeyidx").UIntegerValue, endBlock.Key);
                }

                package.Commit(idr);
            }
            else if (res is Xtol xtol)
            {
                Trace.Assert(endBlock.TypeId == Txmt.TYPE, "Expecting TXMT for EndBlock");

                Idr idr = (Idr)package.GetResourceByKey(new DBPFKey(Idr.TYPE, xtol));
                Trace.Assert(idr != null, "Cannot find associated 3IDR");

                if (endBlock.TypeId == Txmt.TYPE)
                {
                    idr.SetItem(xtol.GetItem("materialkeyidx").UIntegerValue, endBlock.Key);
                }

                package.Commit(idr);
            }
            else if (res is Xobj xobj)
            {
                Trace.Assert(endBlock.TypeId == Txmt.TYPE, "Expecting TXMT for EndBlock");

                if (endBlock.TypeId == Txmt.TYPE)
                {
                    Trace.Assert(index == 0, "Invalid XOBJ TXMT index");

                    xobj.GetItem("material").StringValue = MakeSgName(endBlock.Key.GroupID, endBlock.SgBaseName, endBlock.TypeId, prefixLowerCase);
                }
            }
            else if (res is Xrof xrof)
            {
                Trace.Assert(endBlock.TypeId == Txtr.TYPE, "Expecting TXTR for EndBlock");

                if (endBlock.TypeId == Txtr.TYPE)
                {
                    Trace.Assert(index <= 4, "Invalid XROF TXTR index");
                    if (index == 0)
                    {
                        xrof.GetItem("texturetop").StringValue = MakeSgName(endBlock.Key.GroupID, endBlock.SgBaseName, prefixLowerCase);
                    }
                    else if (index == 1)
                    {
                        xrof.GetItem("texturetopbump").StringValue = MakeSgName(endBlock.Key.GroupID, endBlock.SgBaseName, prefixLowerCase);
                    }
                    else if (index == 2)
                    {
                        xrof.GetItem("textureedges").StringValue = MakeSgName(endBlock.Key.GroupID, endBlock.SgBaseName, prefixLowerCase);
                    }
                    else if (index == 3)
                    {
                        xrof.GetItem("texturetrim").StringValue = MakeSgName(endBlock.Key.GroupID, endBlock.SgBaseName, prefixLowerCase);
                    }
                    else if (index == 4)
                    {
                        xrof.GetItem("textureunder").StringValue = MakeSgName(endBlock.Key.GroupID, endBlock.SgBaseName, prefixLowerCase);
                    }
                }
            }
            else if (res is Xflr xflr)
            {
                Trace.Assert(endBlock.TypeId == Txtr.TYPE, "Expecting TXTR for EndBlock");

                if (endBlock.TypeId == Txtr.TYPE)
                {
                    Trace.Assert(index <= 1, "Invalid XFLR TXTR index");
                    if (index == 0)
                    {
                        xflr.GetItem("texturetname").StringValue = MakeSgName(endBlock.Key.GroupID, endBlock.SgBaseName, prefixLowerCase);
                    }
                    else if (index == 1)
                    {
                        // This is the link to "_detail" TXTR, and the name changes was handled while editing
                    }
                }
            }
            else if (res is Cres cres)
            {
                Trace.Assert(endBlock.TypeId == Shpe.TYPE, "Expecting SHPE for EndBlock");
                cres.SetShpeKey(index, endBlock.Key);
            }
            else if (res is Shpe shpe)
            {
                Trace.Assert(endBlock.TypeId == Gmnd.TYPE || endBlock.TypeId == Txmt.TYPE, "Expecting GMND or TXMT for EndBlock");
                if (endBlock.TypeId == Gmnd.TYPE)
                {
                    Trace.Assert(index <= shpe.Shape.Items.Count, $"Invalid Item index {index}");
                    shpe.Shape.UpdateItem(index, MakeSgName(endBlock.Key.GroupID, endBlock.SgBaseName, endBlock.TypeId, prefixLowerCase));
                }
                else if (endBlock.TypeId == Txmt.TYPE)
                {
                    Trace.Assert(index <= shpe.Shape.Parts.Count, $"Invalid Part index {index}");
                    shpe.Shape.UpdatePart(index, MakeSgName(endBlock.Key.GroupID, endBlock.SgBaseName, prefixLowerCase));
                }
            }
            else if (res is Gmnd gmnd)
            {
                Trace.Assert(endBlock.TypeId == Gmdc.TYPE, "Expecting GMDC for EndBlock");
                gmnd.SetGmdcKey(index, endBlock.Key);
            }
            else if (res is Gmdc)
            {
                Trace.Assert(false, "GMDC's do not have child resources");
            }
            else if (res is Txmt txmt)
            {
                Trace.Assert(endBlock.TypeId == Txtr.TYPE, "Expecting TXTR for EndBlock");
                txmt.MaterialDefinition.SetProperty(label, MakeSgName(endBlock.Key.GroupID, endBlock.SgBaseName, prefixLowerCase));
                txmt.MaterialDefinition.AddFile(MakeSgName(endBlock.Key.GroupID, endBlock.SgBaseName, prefixLowerCase));
            }
            else if (res is Txtr txtr)
            {
                Trace.Assert(endBlock.TypeId == Lifo.TYPE, "Expecting LIFO for EndBlock");
                txtr.ImageData.SetLifoRef(index, MakeSgName(endBlock.Key.GroupID, endBlock.SgBaseName, Lifo.TYPE, prefixLowerCase));
            }
            else if (res is Lifo)
            {
                Trace.Assert(false, "LIFO's do not have child resources");
            }
            else
            {
                Trace.Assert(false, $"Unsupported resource {res}");
            }
        }

        private string MakeSgName(TypeGroupID groupId, string name, bool prefixLowerCase)
        {
            if (groupId == DBPFData.GROUP_SG_MAXIS)
            {
                return name;
            }
            else
            {
                if (prefixLowerCase)
                {
                    return $"##{groupId.ToString().ToLower()}!{name}";
                }
                else
                {
                    return $"##{groupId.ToString().ToUpper()}!{name}";
                }
            }
        }

        private string MakeSgName(TypeGroupID groupId, string name, TypeTypeID typeId, bool prefixLowerCase)
        {
            return $"{MakeSgName(groupId, name, prefixLowerCase)}_{DBPFData.TypeName(typeId).ToLower()}";
        }

        private void UpdateRefsFromParents(DbpfFileCache packageCache, AbstractGraphBlock block, bool prefixLowerCase)
        {
            foreach (AbstractGraphConnector connector in block.GetInConnectors())
            {
                Trace.Assert(connector.EndBlock == block, "In connector is not for this block");

                CacheableDbpfFile package = packageCache.GetOrAdd(connector.StartBlock.PackagePath);

                DBPFResource parentRes = package.GetResourceByKey(connector.StartBlock.OriginalKey);
                Trace.Assert(parentRes != null, "Can't locate parent resource");

                UpdateRefToChild(package, parentRes, connector.EndBlock, connector.Index, connector.Label, prefixLowerCase);

                package.Commit(parentRes);
            }
        }
        #endregion
    }
}
