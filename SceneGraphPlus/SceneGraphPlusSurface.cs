/*
 * SceneGraph Plus - a utility for repairing scene graphs
 *
 * William Howard - 2020-2024
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Microsoft.WindowsAPICodePack.Dialogs;
using SceneGraphPlus.Shapes;
using Sims2Tools;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.CPF;
using Sims2Tools.DBPF.SceneGraph.AGED;
using Sims2Tools.DBPF.SceneGraph.BINX;
using Sims2Tools.DBPF.SceneGraph.CRES;
using Sims2Tools.DBPF.SceneGraph.GMDC;
using Sims2Tools.DBPF.SceneGraph.GMND;
using Sims2Tools.DBPF.SceneGraph.GZPS;
using Sims2Tools.DBPF.SceneGraph.IDR;
using Sims2Tools.DBPF.SceneGraph.LAMB;
using Sims2Tools.DBPF.SceneGraph.LDIR;
using Sims2Tools.DBPF.SceneGraph.LGHT;
using Sims2Tools.DBPF.SceneGraph.LIFO;
using Sims2Tools.DBPF.SceneGraph.LPNT;
using Sims2Tools.DBPF.SceneGraph.LSPT;
using Sims2Tools.DBPF.SceneGraph.MMAT;
using Sims2Tools.DBPF.SceneGraph.RCOL;
using Sims2Tools.DBPF.SceneGraph.SHPE;
using Sims2Tools.DBPF.SceneGraph.TXMT;
using Sims2Tools.DBPF.SceneGraph.TXTR;
using Sims2Tools.DBPF.SceneGraph.XMOL;
using Sims2Tools.DBPF.SceneGraph.XTOL;
using Sims2Tools.DBPF.Sounds;
using Sims2Tools.DBPF.Sounds.HLS;
using Sims2Tools.DBPF.Sounds.TRKS;
using Sims2Tools.DBPF.STR;
using Sims2Tools.DBPF.Utils;
using Sims2Tools.DBPF.XFLR;
using Sims2Tools.DBPF.XFNC;
using Sims2Tools.DBPF.XOBJ;
using Sims2Tools.DBPF.XROF;
using Sims2Tools.DbpfCache;
using Sims2Tools.Dialogs;
using Sims2Tools.Exporter;
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
        private readonly PictureBox picThumbnail = new PictureBox();

        private readonly CommonOpenFileDialog selectPathDialog;

        private readonly ContextMenuStrip menuContextBlock;
        private readonly ToolStripMenuItem menuItemContextTexture;
        private readonly ToolStripMenuItem menuItemContextDelete;
        private readonly ToolStripMenuItem menuItemContextDeleteChain;
        private readonly ToolStripMenuItem menuItemContextExtract;
        private readonly ToolStripMenuItem menuItemContextHide;
        private readonly ToolStripMenuItem menuItemContextHideChain;
        private readonly ToolStripMenuItem menuItemContextFixTgir;
        private readonly ToolStripMenuItem menuItemContextFixFileList;
        private readonly ToolStripMenuItem menuItemContextFixLight;
        private readonly ToolStripMenuItem menuItemContextCopySgName;
        private readonly ToolStripMenuItem menuItemContextClosePackage;
        private readonly ToolStripMenuItem menuItemContextOpenPackage;
        private readonly ToolStripMenuItem menuItemContextSplitBlock;

        private readonly ContextMenuStrip menuContextConnector;
        private readonly ToolStripMenuItem menuItemContextSplitMulti;

        private readonly List<AbstractGraphBlock> blocks = new List<AbstractGraphBlock>();
        private readonly List<AbstractGraphConnector> connectors = new List<AbstractGraphConnector>();

        private AbstractGraphBlock selectedBlock = null;
        private HashSet<AbstractGraphBlock> selectedBlockChain = null;
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

        private bool advancedMode = false;
        private bool hideMissingBlocks = false;
        private bool connectorsOver = true;


        public DrawingSurface(SceneGraphPlusForm owningForm)
        {
            this.owningForm = owningForm;

            {
                menuContextBlock = new ContextMenuStrip();
                menuItemContextTexture = new ToolStripMenuItem();
                menuItemContextDelete = new ToolStripMenuItem();
                menuItemContextDeleteChain = new ToolStripMenuItem();
                menuItemContextHide = new ToolStripMenuItem();
                menuItemContextHideChain = new ToolStripMenuItem();
                menuItemContextExtract = new ToolStripMenuItem();
                menuItemContextFixTgir = new ToolStripMenuItem();
                menuItemContextFixFileList = new ToolStripMenuItem();
                menuItemContextFixLight = new ToolStripMenuItem();
                menuItemContextCopySgName = new ToolStripMenuItem();
                menuItemContextClosePackage = new ToolStripMenuItem();
                menuItemContextOpenPackage = new ToolStripMenuItem();
                menuItemContextSplitBlock = new ToolStripMenuItem();
                menuContextBlock.SuspendLayout();

                menuContextBlock.Items.AddRange(new ToolStripItem[] {
                    menuItemContextTexture,
                    menuItemContextHide, menuItemContextHideChain,
                    menuItemContextExtract,
                    menuItemContextFixTgir, menuItemContextFixFileList, menuItemContextFixLight,
                    menuItemContextCopySgName,
                    menuItemContextClosePackage, menuItemContextOpenPackage,
                    menuItemContextSplitBlock,
                    menuItemContextDelete, menuItemContextDeleteChain });
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

                menuItemContextDeleteChain.Name = "menuItemContextDeleteChain";
                menuItemContextDeleteChain.Size = new Size(222, 22);
                menuItemContextDeleteChain.Text = "Delete Chain";
                menuItemContextDeleteChain.Click += new EventHandler(OnContextBlockDeleteChain);

                menuItemContextHide.Name = "menuItemContextHide";
                menuItemContextHide.Size = new Size(222, 22);
                menuItemContextHide.Text = "Hide";
                menuItemContextHide.Click += new EventHandler(OnContextBlockHide);

                menuItemContextHideChain.Name = "menuItemContextHideChain";
                menuItemContextHideChain.Size = new Size(222, 22);
                menuItemContextHideChain.Text = "Hide Chain";
                menuItemContextHideChain.Click += new EventHandler(OnContextBlockHideChain);

                menuItemContextExtract.Name = "menuItemContextExtract";
                menuItemContextExtract.Size = new Size(222, 22);
                menuItemContextExtract.Text = "Extract";
                menuItemContextExtract.Click += new EventHandler(OnContextBlockExtract);

                menuItemContextFixTgir.Name = "menuItemContextFixTgir";
                menuItemContextFixTgir.Size = new Size(222, 22);
                menuItemContextFixTgir.Text = "Fix TGI";
                menuItemContextFixTgir.Click += new EventHandler(OnContextBlockFixTgir);

                menuItemContextFixFileList.Name = "menuItemContextFixFileList";
                menuItemContextFixFileList.Size = new Size(222, 22);
                menuItemContextFixFileList.Text = "Fix File List";
                menuItemContextFixFileList.Click += new EventHandler(OnContextBlockFixFileList);

                menuItemContextFixLight.Name = "menuItemContextFixLight";
                menuItemContextFixLight.Size = new Size(222, 22);
                menuItemContextFixLight.Text = "Fix Light";
                menuItemContextFixLight.Click += new EventHandler(OnContextBlockFixLight);

                menuItemContextCopySgName.Name = "menuItemContextCopySgName";
                menuItemContextCopySgName.Size = new Size(222, 22);
                menuItemContextCopySgName.Text = "Copy SG Name From Parent";
                menuItemContextCopySgName.Click += new EventHandler(OnContextBlockCopySgName);

                menuItemContextClosePackage.Name = "menuItemContextClosePackage";
                menuItemContextClosePackage.Size = new Size(222, 22);
                menuItemContextClosePackage.Text = "Close Associated Package";
                menuItemContextClosePackage.Click += new EventHandler(OnContextBlockClosePackage);

                menuItemContextOpenPackage.Name = "menuItemContextOpenPackage";
                menuItemContextOpenPackage.Size = new Size(222, 22);
                menuItemContextOpenPackage.Text = "Open Associated Package";
                menuItemContextOpenPackage.Click += new EventHandler(OnContextBlockOpenPackage);

                menuItemContextSplitBlock.Name = "menuItemContextSplitBlock";
                menuItemContextSplitBlock.Size = new Size(222, 22);
                menuItemContextSplitBlock.Text = "Make Clone(s)";
                menuItemContextSplitBlock.Click += new EventHandler(OnContextBlockSplitBlock);

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
                lblTooltip.Visible = false;
                lblTooltip.AutoSize = true;
                lblTooltip.BorderStyle = BorderStyle.FixedSingle;
                lblTooltip.BackColor = ConnectorPopupBackColour;
                lblTooltip.ForeColor = ConnectorPopupTextColour;
                this.Controls.Add(lblTooltip);

                picThumbnail.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
                picThumbnail.Location = new System.Drawing.Point(10, 40);
                picThumbnail.Size = new System.Drawing.Size(192, 192);
                picThumbnail.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
                picThumbnail.Visible = false;
                this.Controls.Add(picThumbnail);
            }

            selectPathDialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true
            };

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

                    if (block.IsDirty)
                    {
                        return true;
                    }
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

        public bool AdvancedMode
        {
            set
            {
                advancedMode = value;

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
                        if (block.IsHidden || (hideMissingBlocks && block.IsMissing))
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
            int startRow = owningForm.RowForType(startBlock.TypeId);

            if (startBlock.TypeId == Gzps.TYPE)
            {
                foreach (AbstractGraphConnector inConnector in startBlock.GetInConnectors())
                {
                    if (inConnector.StartBlock.TypeId == Aged.TYPE)
                    {
                        startRow += RowGap / 2;
                        break;
                    }
                }
            }

            startBlock.Centre = new Point(startCol, startRow);

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
            selectedBlockChain = null;
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

        private HashSet<AbstractGraphBlock> GetBlockChain(AbstractGraphBlock startBlock)
        {
            HashSet<AbstractGraphBlock> chain = GetOutBlockChain(startBlock);

            return chain;
        }

        private HashSet<AbstractGraphBlock> GetOutBlockChain(AbstractGraphBlock startBlock)
        {
            HashSet<AbstractGraphBlock> chain = new HashSet<AbstractGraphBlock>
            {
                startBlock
            };

            foreach (AbstractGraphConnector connector in startBlock.OutConnectors)
            {
                chain.UnionWith(GetOutBlockChain(connector.EndBlock));
            }

            return chain;
        }

        public void ChangeEditingSgName(string sgName, bool prefixLowerCase)
        {
            if (editBlock != null)
            {
                ChangeSgName(editBlock, sgName, prefixLowerCase);

                owningForm.UpdateEditor(editBlock);
            }
        }

        private void ChangeSgName(AbstractGraphBlock block, string sgName, bool prefixLowerCase)
        {
            if (block != null)
            {
                block.SetSgFullName(sgName, prefixLowerCase);
                block.SetDirty();

                foreach (AbstractGraphConnector connector in block.GetInConnectors())
                {
                    connector.StartBlock.SetDirty();
                }

                if (block.TypeId == Txtr.TYPE)
                {
                    if (Xflr.TYPE == block.SoleParent?.TypeId)
                    {
                        // Special case - also update the associated _detail TXTR
                        AbstractGraphBlock detailTxtrBlock = block.SoleParent.OutConnectorByLabel("texturetname_detail").EndBlock;

                        detailTxtrBlock.SetSgFullName($"{sgName}_detail", prefixLowerCase);
                        detailTxtrBlock.SetDirty();
                    }
                }
                else if (block.TypeId == Cres.TYPE)
                {
                    foreach (AbstractGraphConnector connector in block.OutConnectors)
                    {
                        if (connector.EndBlock.TypeId == Lamb.TYPE || connector.EndBlock.TypeId == Ldir.TYPE || connector.EndBlock.TypeId == Lpnt.TYPE || connector.EndBlock.TypeId == Lspt.TYPE)
                        {
                            ChangeSgName(connector.EndBlock, $"{sgName}_{connector.Label}_lght", prefixLowerCase);
                        }
                    }
                }

                FixIssues(block);
            }
        }

        public void ChangeEditingName(string name)
        {
            if (editBlock != null)
            {
                editBlock.BlockName = name;
                editBlock.SetDirty();

                FixIssues(editBlock);

                if (editBlock.TypeId == Hls.TYPE || editBlock.TypeId == Trks.TYPE || editBlock.TypeId == Audio.TYPE)
                {
                    foreach (AbstractGraphConnector connector in editBlock.SoleParent.OutConnectors)
                    {
                        connector.EndBlock.UpdateSoundName(name);
                    }

                    foreach (AbstractGraphConnector connector in editBlock.GetInConnectors())
                    {
                        connector.StartBlock.SetDirty();
                    }
                }

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
            if (!fixBlock.IsLightValid)
            {
                string cresName = fixBlock.GetInConnectors()[0].StartBlock.SgBaseName;
                string lightName = fixBlock.GetInConnectors()[0].Label;

                Trace.Assert(!string.IsNullOrWhiteSpace(cresName), "Invalid CRES name");
                Trace.Assert(!string.IsNullOrWhiteSpace(lightName), "Invalid light name");

                if (!fixBlock.SgBaseName.Equals($"{cresName}_{lightName}", StringComparison.OrdinalIgnoreCase))
                {
                    ChangeSgName(fixBlock, $"{cresName}_{lightName}_lght", owningForm.IsPrefixLowerCase);
                }

                // Any issues with cLightT.Name and OGN are caught as the light is updated in the .package file

                fixBlock.IsLightValid = true;
                fixBlock.SetDirty();
            }
            else if (fixBlock.TypeId == Txmt.TYPE)
            {
                fixBlock.FixFileListIssues();
            }

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
            if (contextBlock != null)
            {
                if (!contextBlock.IsMissingOrClone)
                {
                    menuItemContextTexture.Visible = (contextBlock.TypeId == Txmt.TYPE || contextBlock.TypeId == Txtr.TYPE || contextBlock.TypeId == Lifo.TYPE);

                    menuItemContextDelete.Visible = advancedMode;
                    menuItemContextDelete.Enabled = (contextBlock.IsEditable && contextBlock.GetInConnectors().Count == 0 && contextBlock.OutConnectors.Count == 0);

                    menuItemContextDeleteChain.Visible = advancedMode;
                    menuItemContextDeleteChain.Enabled = false;
                    if (contextBlock.IsEditable && contextBlock.GetInConnectors().Count == 0)
                    {
                        menuItemContextDeleteChain.Enabled = true;

                        foreach (AbstractGraphBlock block in GetBlockChain(contextBlock))
                        {
                            if (!block.IsEditable)
                            {
                                menuItemContextDeleteChain.Enabled = false;
                                break;
                            }
                        }
                    }

                    menuItemContextHide.Visible = advancedMode;
                    menuItemContextHide.Enabled = (contextBlock.GetInConnectors().Count == 0);

                    menuItemContextHideChain.Visible = advancedMode;
                    menuItemContextHideChain.Enabled = (contextBlock.GetInConnectors().Count == 0);

                    menuItemContextExtract.Visible = false;
                    if (contextBlock.TypeId == Cres.TYPE)
                    {
                        menuItemContextExtract.Text = "Extract Mesh";
                        menuItemContextExtract.Visible = true;
                    }
                    else if (contextBlock.TypeId == Gzps.TYPE || contextBlock.TypeId == Mmat.TYPE)
                    {
                        menuItemContextExtract.Text = "Extract Recolour";
                        menuItemContextExtract.Visible = true;
                    }

                    menuItemContextFixTgir.Visible = !contextBlock.IsTgirValid;
                    menuItemContextFixFileList.Visible = (contextBlock.TypeId == Txmt.TYPE) && !contextBlock.IsDirty && !contextBlock.IsFileListValid;
                    menuItemContextFixLight.Visible = (contextBlock.TypeId == Lamb.TYPE || contextBlock.TypeId == Ldir.TYPE || contextBlock.TypeId == Lpnt.TYPE || contextBlock.TypeId == Lspt.TYPE) && !contextBlock.IsDirty && !contextBlock.IsLightValid;

                    menuItemContextCopySgName.Visible = (contextBlock.SoleRcolParent != null);
                    menuItemContextCopySgName.Enabled = (contextBlock.SoleRcolParent?.SgBaseName != null && !(contextBlock.TypeId == Lamb.TYPE || contextBlock.TypeId == Ldir.TYPE || contextBlock.TypeId == Lpnt.TYPE || contextBlock.TypeId == Lspt.TYPE));

                    menuItemContextClosePackage.Visible = true;
                    menuItemContextClosePackage.Enabled = !HasPendingEdits(contextBlock.PackagePath);
                    menuItemContextOpenPackage.Visible = false;

                    menuItemContextSplitBlock.Visible = advancedMode;
                    menuItemContextSplitBlock.Enabled = (contextBlock.GetInConnectors().Count > 1);
                }
                else
                {
                    foreach (ToolStripItem item in menuContextBlock.Items)
                    {
                        item.Visible = false;
                    }

                    e.Cancel = true;

                    if (contextBlock.IsAvailable && !contextBlock.IsMaxis)
                    {
                        menuItemContextOpenPackage.Visible = true;
                        menuItemContextOpenPackage.Enabled = true;

                        e.Cancel = false;
                    }

                    if (contextBlock.IsMaxis && contextBlock.TypeId == Cres.TYPE)
                    {
                        if (GameData.GetMaxisResource(contextBlock.TypeId, contextBlock.Key) != null)
                        {
                            menuItemContextExtract.Text = "Extract Maxis Mesh";
                            menuItemContextExtract.Visible = true;
                            menuItemContextExtract.Enabled = true;

                            e.Cancel = false;
                        }
                    }

                    if (advancedMode && contextBlock.GetInConnectors().Count > 1)
                    {
                        menuItemContextSplitBlock.Visible = true;
                        menuItemContextSplitBlock.Enabled = true;

                        e.Cancel = false;
                    }
                }
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

        private void OnContextBlockDeleteChain(object sender, EventArgs e)
        {
            Trace.Assert(contextBlock.GetInConnectors().Count == 0, "Cannot delete chain with in connectors");

            DeleteChain(contextBlock);

            Invalidate();
        }

        private void DeleteChain(AbstractGraphBlock startBlock)
        {
            Trace.Assert(contextBlock.IsEditable, "Cannot delete a 'read-only' block");

            foreach (AbstractGraphConnector connector in startBlock.GetOutConnectors()) // GetOutConnectors() allows us to use DisconnectFrom() below
            {
                if (connector.EndBlock.GetInConnectors().Count == 1)
                {
                    DeleteChain(connector.EndBlock);

                    connector.EndBlock.DisconnectFrom(connector);
                }
            }

            startBlock.Delete();

            if (startBlock.Equals(editBlock))
            {
                editBlock = null;
                owningForm.UpdateEditor(editBlock);
            }
        }

        private void OnContextBlockHide(object sender, EventArgs e)
        {
            Trace.Assert(contextBlock.GetInConnectors().Count == 0, "Cannot hide block with in connectors");

            contextBlock.Hide();

            if (contextBlock.Equals(editBlock))
            {
                editBlock = null;
                owningForm.UpdateEditor(editBlock);
            }

            Invalidate();
        }

        private void OnContextBlockHideChain(object sender, EventArgs e)
        {
            Trace.Assert(contextBlock.GetInConnectors().Count == 0, "Cannot hide chain with in connectors");

            HideChain(contextBlock);

            Invalidate();
        }

        private void HideChain(AbstractGraphBlock startBlock)
        {
            startBlock.Hide();

            foreach (AbstractGraphConnector outConnector in startBlock.OutConnectors)
            {
                List<AbstractGraphConnector> visibleInConnectors = new List<AbstractGraphConnector>();

                foreach (AbstractGraphConnector inConnector in outConnector.EndBlock.GetInConnectors())
                {
                    if (!inConnector.StartBlock.IsHidden)
                    {
                        visibleInConnectors.Add(inConnector);
                    }
                }

                if (visibleInConnectors.Count == 0)
                {
                    HideChain(outConnector.EndBlock);
                }
            }

            if (startBlock.Equals(editBlock))
            {
                editBlock = null;
                owningForm.UpdateEditor(editBlock);
            }
        }

        public void CloseFilters()
        {
            owningForm.CloseFilters();
        }

        public void ApplyFilters(BlockFilters filters)
        {
            // Do NOT merge these two loops!!!
            foreach (AbstractGraphBlock block in blocks)
            {
                block.Filter(false);
            }

            foreach (AbstractGraphBlock block in blocks)
            {
                if (block.TypeId == Gzps.TYPE || block.TypeId == Aged.TYPE || block.TypeId == Xmol.TYPE || block.TypeId == Xtol.TYPE)
                {
                    if (filters.Exclude(block.Text))
                    {
                        FilterChain(block);
                    }
                }
            }
        }

        private void FilterChain(AbstractGraphBlock startBlock)
        {
            startBlock.Filter(true);

            foreach (AbstractGraphConnector outConnector in startBlock.OutConnectors)
            {
                List<AbstractGraphConnector> visibleInConnectors = new List<AbstractGraphConnector>();

                foreach (AbstractGraphConnector inConnector in outConnector.EndBlock.GetInConnectors())
                {
                    if (!inConnector.StartBlock.IsHidden)
                    {
                        visibleInConnectors.Add(inConnector);
                    }
                }

                if (visibleInConnectors.Count == 0)
                {
                    FilterChain(outConnector.EndBlock);
                }
            }

            if (startBlock.Equals(editBlock))
            {
                editBlock = null;
                owningForm.UpdateEditor(editBlock);
            }
        }

        private void OnContextBlockExtract(object sender, EventArgs e)
        {
            if (selectPathDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                Exporter exporter = new Exporter();

                exporter.Open(selectPathDialog.FileName);

                if (contextBlock.IsMaxis)
                {
                    ExtractMaxisMesh(exporter);
                }
                else
                {
                    ExtractCustomMeshOrRecolour(exporter);
                }

                exporter.Close();
            }
        }

        private void ExtractCustomMeshOrRecolour(Exporter exporter)
        {
            foreach (AbstractGraphBlock block in GetBlockChain(contextBlock))
            {
                if (contextBlock.TypeId == Cres.TYPE && (block.TypeId == Cres.TYPE || block.TypeId == Shpe.TYPE || block.TypeId == Gmnd.TYPE || block.TypeId == Gmdc.TYPE))
                {
                    exporter.Export(block.PackagePath, block.Key);
                }
                else if (contextBlock.TypeId == Mmat.TYPE && (block.TypeId == Mmat.TYPE || block.TypeId == Txmt.TYPE || block.TypeId == Txtr.TYPE || block.TypeId == Lifo.TYPE))
                {
                    exporter.Export(block.PackagePath, block.Key);
                }
                else if (contextBlock.TypeId == Gzps.TYPE && (block.TypeId == Gzps.TYPE || block.TypeId == Txmt.TYPE || block.TypeId == Txtr.TYPE || block.TypeId == Lifo.TYPE))
                {
                    if (block.TypeId == Gzps.TYPE)
                    {
                        exporter.Export(block.PackagePath, new DBPFKey(Idr.TYPE, block.Key));
                        exporter.Export(block.PackagePath, new DBPFKey(Binx.TYPE, block.Key));
                    }

                    exporter.Export(block.PackagePath, block.Key);
                }
            }
        }

        private void ExtractMaxisMesh(Exporter exporter)
        {
            // TODO - SceneGraph Plus - 3 - extend this to include GZPS and MMAT recolours
            if (contextBlock.TypeId == Cres.TYPE)
            {
                DBPFKey cresKey = contextBlock.Key;

                string maxisPath = GameData.GetMaxisPackagePath(Cres.TYPE, cresKey, true);

                if (maxisPath != null)
                {
                    Cres cres = (Cres)GameData.GetMaxisResource(Cres.TYPE, cresKey, true);

                    exporter.Export(maxisPath, cres);

                    foreach (DBPFKey shpeKey in cres.ShpeKeys)
                    {
                        maxisPath = GameData.GetMaxisPackagePath(Shpe.TYPE, shpeKey, true);

                        if (maxisPath != null)
                        {
                            Shpe shpe = (Shpe)GameData.GetMaxisResource(Shpe.TYPE, shpeKey, true);

                            exporter.Export(maxisPath, shpe);

                            foreach (string gmndName in shpe.GmndNames)
                            {
                                Gmnd gmnd = (Gmnd)GameData.GetMaxisResource(Gmnd.TYPE, gmndName, true);

                                if (gmnd != null)
                                {
                                    maxisPath = GameData.GetMaxisPackagePath(Gmnd.TYPE, gmnd, true);

                                    exporter.Export(maxisPath, gmnd);

                                    foreach (DBPFKey gmdcKey in gmnd.GmdcKeys)
                                    {
                                        maxisPath = GameData.GetMaxisPackagePath(Gmdc.TYPE, gmdcKey, true);

                                        if (maxisPath != null)
                                        {
                                            exporter.Export(maxisPath, gmdcKey);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
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

        private void OnContextBlockFixLight(object sender, EventArgs e)
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

        private void OnContextBlockSplitBlock(object sender, EventArgs e)
        {
            int shiftMultiplier = 0;

            foreach (AbstractGraphConnector connector in contextBlock.GetInConnectors())
            {
                if (shiftMultiplier > 0)
                {
                    connector.SetEndBlock(connector.EndBlock.MakeClone(new Point((ColumnGap / 8) * shiftMultiplier, (RowGap / 8) * shiftMultiplier)), false);
                }

                ++shiftMultiplier;
            }
        }

        private void OnContextBlockOpenPackage(object sender, EventArgs e)
        {
            owningForm.OpenPackage(contextBlock.Key);
        }

        public void UpdateAvailableBlocks()
        {
            foreach (AbstractGraphBlock block in blocks)
            {
                block.IsAvailable = owningForm.IsAvailable(block.Key);
            }

            Invalidate();
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
                    lblTooltip.Visible = picThumbnail.Visible = false;

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
                    lblTooltip.Visible = picThumbnail.Visible = false;

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
                lblTooltip.Visible = picThumbnail.Visible = false;

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
                    lblTooltip.Visible = picThumbnail.Visible = false;

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

                            Invalidate();
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

                        Point newCentre;

                        if (gridScale <= 1.0f)
                        {
                            newCentre = new Point(((selectedBlock.Centre.X + gridX / 2) / gridX) * gridX - offsetX, ((selectedBlock.Centre.Y + gridY / 2) / gridY) * gridY - offsetY);
                        }
                        else
                        {
                            newCentre = new Point(((selectedBlock.Centre.X + gridX) / gridX) * gridX - offsetX, ((selectedBlock.Centre.Y + gridY) / gridY) * gridY - offsetY);
                        }

                        Point delta = new Point(newCentre.X - selectedBlock.Centre.X, newCentre.Y - selectedBlock.Centre.Y);

                        if (selectedBlockChain != null)
                        {
                            foreach (AbstractGraphBlock block in selectedBlockChain)
                            {
                                block.Move(delta);
                            }
                        }
                        else
                        {
                            selectedBlock.Centre = newCentre;
                        }

                        this.Invalidate();
                    }

                    selectedBlock = null;
                    selectedBlockChain = null;
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
                lblTooltip.Visible = picThumbnail.Visible = false;

                Point delta = new Point(e.X - previousPoint.X, e.Y - previousPoint.Y);

                if (Form.ModifierKeys == Keys.Alt)
                {
                    if (owningForm.IsAdvancedMode)
                    {
                        if (selectedBlockChain == null)
                        {
                            selectedBlockChain = GetBlockChain(selectedBlock);
                        }

                        foreach (AbstractGraphBlock block in selectedBlockChain)
                        {
                            block.Move(delta);
                        }
                    }
                }
                else
                {
                    selectedBlock.Move(delta);
                }

                previousPoint = e.Location;

                bool canDrop = (!selectedBlock.IsClone) && (Form.ModifierKeys != Keys.Alt);
                if (!owningForm.IsAdvancedMode && selectedBlock.IsMissing) canDrop = false;

                if (canDrop)
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

                        Point popupLocation = this.PointToClient(MousePosition);
                        popupLocation.Offset(12, 20);

                        Image thumbnail = null;

                        if (Form.ModifierKeys == Keys.Shift)
                        {
                            thumbnail = owningForm.GetThumbnail(hoverBlock.Key);

                            if (thumbnail != null)
                            {
                                picThumbnail.Location = popupLocation;
                                picThumbnail.Image = thumbnail;
                                picThumbnail.Visible = true;
                            }
                        }

                        if (thumbnail == null)
                        {
                            string toolTip = hoverBlock.ToolTip;

                            if (!string.IsNullOrEmpty(toolTip))
                            {
                                if (hoverBlock.IsAvailable)
                                {
                                    toolTip = $"{toolTip}\r\nin {owningForm.GetAvailablePath(hoverBlock.Key)}";
                                }

                                lblTooltip.Location = popupLocation;
                                lblTooltip.Text = toolTip;
                                lblTooltip.Visible = true;
                            }
                        }

                        this.Invalidate();
                    }
                }
                else
                {
                    if (hoverBlock != null)
                    {
                        lblTooltip.Visible = picThumbnail.Visible = false;

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
                        lblTooltip.Visible = picThumbnail.Visible = false;

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
        public new void Invalidate()
        {
            base.Invalidate();

            ResizeToFit();
        }

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
            Dictionary<string, List<AbstractGraphBlock>> dirtyBlocksByPackage = new Dictionary<string, List<AbstractGraphBlock>>();

            // Find all the dirty blocks by package
            foreach (AbstractGraphBlock block in blocks)
            {
                if (block.IsMissingOrClone) continue;

                if (block.IsDirty)
                {
                    if (!dirtyBlocksByPackage.ContainsKey(block.PackagePath))
                    {
                        dirtyBlocksByPackage.Add(block.PackagePath, new List<AbstractGraphBlock>());
                    }

                    dirtyBlocksByPackage[block.PackagePath].Add(block);
                }
            }

            // For each package that has dirty blocks ...
            foreach (KeyValuePair<string, List<AbstractGraphBlock>> kvPair in dirtyBlocksByPackage)
            {
                CacheableDbpfFile package = packageCache.GetOrAdd(kvPair.Key);

                // ... firstly delete any blocks ...
                foreach (AbstractGraphBlock block in kvPair.Value)
                {
                    if (block.IsDeleteMe)
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
                        Trace.Assert(res != null, $"Refs: Missing resource for {block.OriginalKey}");

                        UpdateRefsToChildren(package, res, block, prefixLowerCase);

                        package.Commit(res);
                    }
                }

                // ... thirdly update any block that references the dirty blocks where the dirty block's name has changed ...
                foreach (AbstractGraphBlock block in kvPair.Value)
                {
                    if (!block.IsDeleted)
                    {
                        if (block.BlockName == null && block.SgOriginalName != null && !block.SgOriginalName.Equals(block.SgFullName))
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
                        Trace.Assert(res != null, $"Name: Missing resource for {block.OriginalKey}");

                        UpdateName(res, block, alwaysSetNames, alwaysClearNames, prefixNames, prefixLowerCase);

                        package.Remove(block.OriginalKey);

                        if (res is Rcol rcol)
                        {
                            rcol.FixTGIR();
                        }
                        else if (res is Hls || res is Trks || res is Audio)
                        {
                            res.ChangeIR(block.Key.InstanceID, block.Key.ResourceID);
                        }

                        package.Commit(res, true); // ALWAYS commit, as we removed the resource above
                    }
                }

                // ... lastly, mark the dirty blocks as clean
                foreach (AbstractGraphBlock block in kvPair.Value)
                {
                    block.SetClean();
                }
            }

            // Finally, save every package that was updated
            foreach (string packagePath in dirtyBlocksByPackage.Keys)
            {
                CacheableDbpfFile package = packageCache.GetOrOpen(packagePath);

                if (package.Update(autoBackup) == null)
                {
                    MsgBox.Show($"Error trying to update {package.PackageName}, file is probably open in SimPe!\n\nChanges are in the associated .temp file.", "Package Update Error!");
                }

                packageCache.SetClean(package);
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
            else if (res is Aged)
            {
                // AGED has no name
            }
            else if (res is Hls)
            {
                // HLS has no name
            }
            else if (res is Trks)
            {
                // TRKS has no name
            }
            else if (res is Audio)
            {
                // AUDIO has no name
            }
            else if (res is Cpf cpf) // Do this AFTER MMAT and AGED
            {
                if (res is Gzps || res is Xfnc || res is Xmol || res is Xtol || res is Xobj || res is Xrof || res is Xflr)
                {
                    cpf.GetItem("name").StringValue = block.BlockName;
                }
                else
                {
                    Trace.Assert(false, $"Unsupported CPF resource {res}");
                }
            }
            else if (res is Lght lght)
            {
                List<AbstractGraphConnector> inConnectors = block.GetInConnectors();
                Trace.Assert(inConnectors.Count == 1, $"Multiple 'in connectors' for light {block.SgFullName}");
                Trace.Assert(inConnectors[0].StartBlock.TypeId == Cres.TYPE, $"Expected a parent CRES for light {block.SgFullName}");

                if (!lght.IsLightValid(inConnectors[0].StartBlock.SgBaseName))
                {
                    lght.SetKeyName($"{block.SgBaseName}_lght");
                    lght.OgnName = "";
                    lght.BaseLight.LightT.NameResource.FileName = lght.KeyName;
                }
                else if (!block.IsOriginalTgirValid)
                {
                }
                else
                {
                    Trace.Assert(false, "Attempting to update the name of a light (LPNT, LSPT, LAMB, LDIR) resource");
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
                    Trace.Assert(false, $"Unsupported RCOL resource {res}");
                }
            }
            else
            {
                Trace.Assert(false, $"Unsupported resource {res}");
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
            /* if (alwaysClearNames)
            {
                // This will cause the game to crash if the Type is SimSkin (and possibly others)
                txmt.MaterialDefinition.FileDescription = "";
            }
            else */
            if (alwaysSetNames || !string.IsNullOrEmpty(txmt.MaterialDefinition.FileDescription))
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
                Trace.Assert(endBlock.TypeId == Cres.TYPE || endBlock.TypeId == Txmt.TYPE || endBlock.TypeId == Hls.TYPE, "Expecting CRES, TXMT or HLS for EndBlock");

                List<StrItem> items = str.LanguageItems(Sims2Tools.DBPF.Data.MetaData.Languages.Default);
                Trace.Assert(index < items.Count, "Index out of range");

                if (endBlock.TypeId == Hls.TYPE)
                {
                    Trace.Assert(str.InstanceID == (TypeInstanceID)0x4132, "HLS expected for Sounds");

                    items[index].Title = endBlock.BlockName;
                }
                else
                {
                    if (endBlock.TypeId == Cres.TYPE)
                    {
                        Trace.Assert(str.InstanceID == (TypeInstanceID)0x0085, "CRES expected for Model Names");
                    }
                    else if (endBlock.TypeId == Txmt.TYPE)
                    {
                        Trace.Assert(str.InstanceID == (TypeInstanceID)0x0088, "TXMT expected for Material Names");
                    }

                    items[index].Title = MakeSgName(endBlock.Key.GroupID, endBlock.SgBaseName, prefixLowerCase);
                }
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
                    mmat.GetItem("name").StringValue = MakeSgName(endBlock.Key.GroupID, endBlock.SgBaseName, prefixLowerCase);
                }
            }
            else if (res is Aged aged)
            {
                Trace.Assert(SceneGraphPlusForm.UnderstoodTypeIds.Contains(endBlock.TypeId), $"Unexpected {DBPFData.TypeName(endBlock.TypeId)} for EndBlock");

                Idr idr = (Idr)package.GetResourceByKey(new DBPFKey(Idr.TYPE, aged));
                Trace.Assert(idr != null, "Cannot find associated 3IDR");

                idr.SetItem((uint)index, endBlock.Key);

                package.Commit(idr);
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
                Trace.Assert(endBlock.TypeId == Shpe.TYPE || endBlock.TypeId == Lamb.TYPE || endBlock.TypeId == Ldir.TYPE || endBlock.TypeId == Lpnt.TYPE || endBlock.TypeId == Lspt.TYPE, "Expecting SHPE or LGHT for EndBlock");
                if (endBlock.TypeId == Shpe.TYPE)
                {
                    cres.SetShpeKey(index, endBlock.Key);
                }
                else if (endBlock.TypeId == Lamb.TYPE || endBlock.TypeId == Ldir.TYPE || endBlock.TypeId == Lpnt.TYPE || endBlock.TypeId == Lspt.TYPE)
                {
                    cres.SetLghtKey(index, endBlock.Key);
                }
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
            else if (res is Lght)
            {
                Trace.Assert(false, "Lights (LAMB, LDIR, LPNT, LSPT) do not have child resources");
            }
            else if (res is Hls hls)
            {
                Trace.Assert(endBlock.TypeId == Trks.TYPE, "Expecting TRKS for EndBlock");
                hls.Items[index].InstanceLo = endBlock.Key.InstanceID;
                hls.Items[index].InstanceHi = endBlock.Key.ResourceID;
            }
            else if (res is Trks trks)
            {
                Trace.Assert(endBlock.TypeId == Audio.TYPE, "Expecting AUDIO for EndBlock");
                trks.SetItemUInteger("0xff3c2160", endBlock.Key.InstanceID.AsUInt());
                trks.SetItemUInteger("0xff99d2d5", endBlock.Key.ResourceID.AsUInt());
            }
            else if (res is Audio)
            {
                Trace.Assert(false, "AUDIO's do not have child resources");
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
