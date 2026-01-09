/*
 * SceneGraph Plus - a utility for repairing scene graphs
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

#region Usings
using Microsoft.WindowsAPICodePack.Dialogs;
using SceneGraphPlus.Data;
using SceneGraphPlus.Dialogs.Options;
using SceneGraphPlus.OptionsDialogs.Helpers;
using SceneGraphPlus.Shapes;
using Sims2Tools;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.CPF;
using Sims2Tools.DBPF.CTSS;
using Sims2Tools.DBPF.OBJD;
using Sims2Tools.DBPF.Package;
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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using static Sims2Tools.DBPF.Data.MetaData;
#endregion

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

        public readonly Cursor CursorArrowPlus;

        #region Surface Dynamic Controls (context menus, dialogs, tooltip and thumbnail)
        private readonly Label lblTooltip = new Label();
        private readonly PictureBox picThumbnail = new PictureBox();

        private readonly CommonOpenFileDialog selectPathDialog;
        private readonly SaveFileDialog selectFileDialog;

        private readonly ContextMenuStrip menuContextBlock;
        private readonly ToolStripMenuItem menuItemContextBlockTexture;
        private readonly ToolStripMenuItem menuItemContextBlockDelete;
        private readonly ToolStripMenuItem menuItemContextBlockDeleteChain;
        private readonly ToolStripMenuItem menuItemContextBlockExtract;
        private readonly ToolStripMenuItem menuItemContextBlockExport;
        private readonly ToolStripMenuItem menuItemContextBlockHide;
        private readonly ToolStripMenuItem menuItemContextBlockHideChain;
        private readonly ToolStripMenuItem menuItemContextBlockFixTgir;
        private readonly ToolStripMenuItem menuItemContextBlockFixFileList;
        private readonly ToolStripMenuItem menuItemContextBlockFixLight;
        private readonly ToolStripMenuItem menuItemContextBlockFixLanguages;
        private readonly ToolStripMenuItem menuItemContextBlockCopySgName;
        private readonly ToolStripMenuItem menuItemContextBlockClosePackage;
        private readonly ToolStripMenuItem menuItemContextBlockOpenPackage;
        private readonly ToolStripMenuItem menuItemContextBlockSplitBlock;

        private readonly ContextMenuStrip menuContextConnector;
        private readonly ToolStripMenuItem menuItemContextConnectorSplitMulti;
        private readonly ToolStripMenuItem menuItemContextConnectorUnlink;
        #endregion

        #region Surface Options and Mode Tracking
        private bool advancedMode = false;
        private bool hideMissingBlocks = false;
        private bool connectorsOver = true;

        private bool dropToGrid = false;
        private float gridScale = 1.0f;

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
        #endregion

        #region Surface Selected Shape Tracking
        private List<GraphBlock> selectedBlocks = new List<GraphBlock>();
        private GraphBlock firstSelectedBlock = null;
        private HashSet<GraphBlock> selectedBlockChain = null;
        private GraphBlock hoverBlock = null;
        private GraphBlock editBlock = null;
        private List<GraphBlock> dropOntoBlocks = new List<GraphBlock>();
        private GraphBlock contextBlock = null;

        private GraphConnector hoverConnector = null;
        private GraphConnector dropOntoConnector = null;
        private GraphConnector contextConnector = null;

        private bool IsSingleSelect => (selectedBlocks.Count == 1);
        private bool IsMultiSelect => (selectedBlocks.Count > 1);
        #endregion

        #region Surface Mouse Tracking
        private bool moving;
        private Point startPoint = Point.Empty;
        private Point previousPoint = Point.Empty;

        private bool banding;
        private Point bandingStart = Point.Empty;
        private Point bandingLast = Point.Empty;

        private DateTime mouseLastClick;
        private bool inDoubleClick = false;
        private Rectangle doubleClickArea;
        private TimeSpan doubleClickMaxTime;
        private readonly Timer mouseClickTimer;
        #endregion

        private readonly SceneGraphPlusForm owningForm;
        private readonly DbpfFileCache packageCache;

        private readonly List<GraphBlock> allBlocks = new List<GraphBlock>();
        private readonly List<GraphConnector> allConnectors = new List<GraphConnector>();

        #region Surface Constructor and Reset
        public DrawingSurface(SceneGraphPlusForm owningForm, DbpfFileCache packageCache)
        {
            this.owningForm = owningForm;
            this.packageCache = packageCache;

            MemoryStream cursorMemoryStream = new MemoryStream(Properties.Resources.ArrowPlus);
            this.CursorArrowPlus = new Cursor(cursorMemoryStream);

            #region Block Context Menu Initialisation
            {
                menuContextBlock = new ContextMenuStrip();
                menuItemContextBlockTexture = new ToolStripMenuItem();
                menuItemContextBlockDelete = new ToolStripMenuItem();
                menuItemContextBlockDeleteChain = new ToolStripMenuItem();
                menuItemContextBlockHide = new ToolStripMenuItem();
                menuItemContextBlockHideChain = new ToolStripMenuItem();
                menuItemContextBlockExtract = new ToolStripMenuItem();
                menuItemContextBlockExport = new ToolStripMenuItem();
                menuItemContextBlockFixTgir = new ToolStripMenuItem();
                menuItemContextBlockFixFileList = new ToolStripMenuItem();
                menuItemContextBlockFixLight = new ToolStripMenuItem();
                menuItemContextBlockFixLanguages = new ToolStripMenuItem();
                menuItemContextBlockCopySgName = new ToolStripMenuItem();
                menuItemContextBlockClosePackage = new ToolStripMenuItem();
                menuItemContextBlockOpenPackage = new ToolStripMenuItem();
                menuItemContextBlockSplitBlock = new ToolStripMenuItem();

                menuContextBlock.SuspendLayout();

                menuContextBlock.Items.AddRange(new ToolStripItem[] {
                    menuItemContextBlockTexture,
                    menuItemContextBlockHide, menuItemContextBlockHideChain,
                    menuItemContextBlockExtract, menuItemContextBlockExport,
                    menuItemContextBlockFixTgir, menuItemContextBlockFixFileList, menuItemContextBlockFixLight, menuItemContextBlockFixLanguages,
                    menuItemContextBlockCopySgName,
                    menuItemContextBlockClosePackage, menuItemContextBlockOpenPackage,
                    menuItemContextBlockSplitBlock,
                    menuItemContextBlockDelete, menuItemContextBlockDeleteChain
                });
                menuContextBlock.Name = "menuContextBlock";
                menuContextBlock.Size = new Size(223, 48);
                menuContextBlock.Opening += new CancelEventHandler(OnContextBlockOpening);

                menuItemContextBlockTexture.Name = "menuItemContextBlockTexture";
                menuItemContextBlockTexture.Size = new Size(222, 22);
                menuItemContextBlockTexture.Text = "Show Texture";
                menuItemContextBlockTexture.Click += new EventHandler(OnContextBlockTexture);

                menuItemContextBlockDelete.Name = "menuItemContextBlockDelete";
                menuItemContextBlockDelete.Size = new Size(222, 22);
                menuItemContextBlockDelete.Text = "Delete";
                menuItemContextBlockDelete.Click += new EventHandler(OnContextBlockDelete);

                menuItemContextBlockDeleteChain.Name = "menuItemContextBlockDeleteChain";
                menuItemContextBlockDeleteChain.Size = new Size(222, 22);
                menuItemContextBlockDeleteChain.Text = "Delete Chain";
                menuItemContextBlockDeleteChain.Click += new EventHandler(OnContextBlockDeleteChain);

                menuItemContextBlockHide.Name = "menuItemContextBlockHide";
                menuItemContextBlockHide.Size = new Size(222, 22);
                menuItemContextBlockHide.Text = "Hide";
                menuItemContextBlockHide.Click += new EventHandler(OnContextBlockHide);

                menuItemContextBlockHideChain.Name = "menuItemContextBlockHideChain";
                menuItemContextBlockHideChain.Size = new Size(222, 22);
                menuItemContextBlockHideChain.Text = "Hide Chain";
                menuItemContextBlockHideChain.Click += new EventHandler(OnContextBlockHideChain);

                menuItemContextBlockExtract.Name = "menuItemContextBlockExtract";
                menuItemContextBlockExtract.Size = new Size(222, 22);
                menuItemContextBlockExtract.Text = "Extract";
                menuItemContextBlockExtract.ToolTipText = "Extract in SimPe package.xml format";
                menuItemContextBlockExtract.Click += new EventHandler(OnContextBlockExtract);

                menuItemContextBlockExport.Name = "menuItemContextBlockExport";
                menuItemContextBlockExport.Size = new Size(222, 22);
                menuItemContextBlockExport.Text = "Export";
                menuItemContextBlockExport.ToolTipText = "Export as a .package file";
                menuItemContextBlockExport.Click += new EventHandler(OnContextBlockExport);

                menuItemContextBlockFixTgir.Name = "menuItemContextBlockFixTgir";
                menuItemContextBlockFixTgir.Size = new Size(222, 22);
                menuItemContextBlockFixTgir.Text = "Fix TGI";
                menuItemContextBlockFixTgir.Click += new EventHandler(OnContextBlockFixTgir);

                menuItemContextBlockFixFileList.Name = "menuItemContextBlockFixFileList";
                menuItemContextBlockFixFileList.Size = new Size(222, 22);
                menuItemContextBlockFixFileList.Text = "Fix File List";
                menuItemContextBlockFixFileList.Click += new EventHandler(OnContextBlockFixFileList);

                menuItemContextBlockFixLight.Name = "menuItemContextBlockFixLight";
                menuItemContextBlockFixLight.Size = new Size(222, 22);
                menuItemContextBlockFixLight.Text = "Fix Light";
                menuItemContextBlockFixLight.Click += new EventHandler(OnContextBlockFixLight);

                menuItemContextBlockFixLanguages.Name = "menuItemContextBlockFixLanguages";
                menuItemContextBlockFixLanguages.Size = new Size(222, 22);
                menuItemContextBlockFixLanguages.Text = "Fix Languages";
                menuItemContextBlockFixLanguages.Click += new EventHandler(OnContextBlockFixLanguages);

                menuItemContextBlockCopySgName.Name = "menuItemContextBlockCopySgName";
                menuItemContextBlockCopySgName.Size = new Size(222, 22);
                menuItemContextBlockCopySgName.Text = "Copy SG Name From Parent";
                menuItemContextBlockCopySgName.Click += new EventHandler(OnContextBlockCopySgName);

                menuItemContextBlockClosePackage.Name = "menuItemContextBlockClosePackage";
                menuItemContextBlockClosePackage.Size = new Size(222, 22);
                menuItemContextBlockClosePackage.Text = "Close Associated Package";
                menuItemContextBlockClosePackage.Click += new EventHandler(OnContextBlockClosePackage);

                menuItemContextBlockOpenPackage.Name = "menuItemContextBlockOpenPackage";
                menuItemContextBlockOpenPackage.Size = new Size(222, 22);
                menuItemContextBlockOpenPackage.Text = "Open Associated Package";
                menuItemContextBlockOpenPackage.Click += new EventHandler(OnContextBlockOpenPackage);

                menuItemContextBlockSplitBlock.Name = "menuItemContextBlockSplitBlock";
                menuItemContextBlockSplitBlock.Size = new Size(222, 22);
                menuItemContextBlockSplitBlock.Text = "Make Clone(s)";
                menuItemContextBlockSplitBlock.Click += new EventHandler(OnContextBlockSplitBlock);

                menuContextBlock.ResumeLayout(false);
            }
            #endregion

            #region Connector Context Menu Initialisation
            {
                menuContextConnector = new ContextMenuStrip();
                menuItemContextConnectorSplitMulti = new ToolStripMenuItem();
                menuItemContextConnectorUnlink = new ToolStripMenuItem();
                menuContextConnector.SuspendLayout();

                menuContextConnector.Items.AddRange(new ToolStripItem[] { menuItemContextConnectorSplitMulti, menuItemContextConnectorUnlink });
                menuContextConnector.Name = "menuContextConnector";
                menuContextConnector.Size = new Size(223, 48);
                menuContextConnector.Opening += new CancelEventHandler(OnContextConnectorOpening);

                menuItemContextConnectorSplitMulti.Name = "menuItemContextSplitMulti";
                menuItemContextConnectorSplitMulti.Size = new Size(222, 22);
                menuItemContextConnectorSplitMulti.Text = "Split Multi-Connector";
                menuItemContextConnectorSplitMulti.Click += new EventHandler(OnContextConnectorSplitMulti);

                menuItemContextConnectorUnlink.Name = "menuItemContextUnlink";
                menuItemContextConnectorUnlink.Size = new Size(222, 22);
                menuItemContextConnectorUnlink.Text = "Remove STR# Entry";
                menuItemContextConnectorUnlink.Click += new EventHandler(OnContextConnectorUnlink);

                menuContextConnector.ResumeLayout(false);
            }
            #endregion

            #region Tooltip Initialisation
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
            #endregion

            #region Dialog Initialisation
            selectPathDialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true
            };

            selectFileDialog = new SaveFileDialog
            {
                Title = "Export as .package file",
                DefaultExt = "package",
                Filter = "DBPF Package|*.package"
            };
            #endregion

            #region Mouse Tracking Initialisation
            doubleClickMaxTime = TimeSpan.FromMilliseconds(SystemInformation.DoubleClickTime);

            mouseClickTimer = new Timer
            {
                Interval = SystemInformation.DoubleClickTime
            };
            mouseClickTimer.Tick += MouseClickTimer_Tick;
            #endregion

            DoubleBuffered = true;

            Reset(false);
        }

        public void Reset(bool flushEditor = true)
        {
            hideMissingBlocks = false;

            editBlock = null;
            DeselectBlocks(false);
            selectedBlockChain = null;
            hoverBlock = null;
            dropOntoBlocks.Clear();
            contextBlock = null;

            hoverConnector = null;
            dropOntoConnector = null;
            contextConnector = null;

            allBlocks.Clear();
            allConnectors.Clear();

            Invalidate();

            if (flushEditor)
            {
                owningForm.UpdateEditor(editBlock);
            }
        }

        private void DeselectBlocks(bool invalidate)
        {
            foreach (GraphBlock block in selectedBlocks)
            {
                block.IsSelected = false;
            }

            selectedBlocks.Clear();
            firstSelectedBlock = null;

            ChangeCursor();

            if (invalidate) Invalidate();
        }

        private void SelectSingleBlock(GraphBlock block)
        {
            DeselectBlocks(false);

            if (block != null)
            {
                firstSelectedBlock = block;
                firstSelectedBlock.IsSelected = true;

                selectedBlocks.Add(firstSelectedBlock);
            }

            Invalidate();
        }

        private void SelectBlock(GraphBlock block)
        {
            if (selectedBlocks.Count == 0)
            {
                SelectSingleBlock(block);
            }
            else
            {
                block.IsSelected = true;

                selectedBlocks.Add(block);

                ChangeCursor();

                Invalidate();
            }
        }

        private void DeselectEditBlock()
        {
            if (editBlock != null)
            {
                editBlock.IsEditing = false;
                owningForm.UpdateEditor(null);

                editBlock = null;

                Invalidate();
            }
        }

        private void SelectEditBlock(GraphBlock block)
        {
            if (block == editBlock) return;

            DeselectEditBlock();

            editBlock = block;
            editBlock.IsEditing = true;

            owningForm.UpdateEditor(editBlock);

            Invalidate();
        }
        #endregion

        #region Surface State Tracking
        public bool IsDirty
        {
            get
            {
                foreach (GraphBlock block in allBlocks)
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
            foreach (GraphBlock block in allBlocks)
            {
                if (block.PackagePath.Equals(packagePath))
                {
                    if (block.IsDirty || block.IsClone) return true;
                }
            }

            return false;
        }
        #endregion

        #region Surface Layout of Shapes
        public int MinWidth => NextFreeCol - ColumnGap / 2;

        public int MinHeight => NextFreeRow - RowGap / 2;

        public void ResizeToFit()
        {
            if (MinWidth > this.Width || MinHeight > this.Height)
            {
                this.Size = new Size(Math.Max(MinWidth, this.Width), Math.Max(MinHeight, this.Height));
            }
        }

        public int NextFreeCol
        {
            get
            {
                int highestX = 0;

                if (allBlocks.Count > 0)
                {
                    foreach (GraphBlock block in allBlocks)
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

                if (allBlocks.Count > 0)
                {
                    foreach (GraphBlock block in allBlocks)
                    {
                        if (hideMissingBlocks && block.IsMissing) continue;

                        if (block.Centre.Y > highestY) highestY = block.Centre.Y;
                    }

                    highestY = ((highestY + RowGap - 1) / RowGap) * RowGap;
                }

                return highestY + (RowGap / 2);
            }
        }

        private readonly SortedList<GraphBlock, GraphBlock> realignedBlocks = new SortedList<GraphBlock, GraphBlock>();

        public void RealignAll()
        {
            int freeCol = ColumnGap / 2;

            realignedBlocks.Clear();

            foreach (TypeTypeID typeId in SceneGraphPlusForm.UnderstoodTypeIds)
            {
                foreach (GraphBlock block in allBlocks)
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

        private void RealignChain(GraphBlock startBlock, ref int freeCol)
        {
            int startCol = freeCol;
            int startRow = owningForm.RowForType(startBlock.TypeId);

            /* if (startBlock.TypeId == Gzps.TYPE)
            {
                foreach (AbstractGraphConnector inConnector in startBlock.GetInConnectors())
                {
                    if (inConnector.StartBlock.TypeId == Aged.TYPE)
                    {
                        startRow += RowGap / 2;
                        break;
                    }
                }
            } */

            startBlock.Centre = new Point(startCol, startRow);

            realignedBlocks.Add(startBlock, startBlock);

            foreach (GraphConnector connector in startBlock.OutConnectors)
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
        #endregion

        #region Shape Arrays Manipulation
        /// <summary>
        /// Adds the block or connector to the surface,
        /// allowing it to be drawn and detected.
        /// </summary>
        public void AddShape(GraphShape shape)
        {
            if (shape is GraphBlock block)
            {
                allBlocks.Add(block);
            }
            else if (shape is GraphConnector connector)
            {
                allConnectors.Add(connector);
            }
            else
            {
                throw new Exception("Unknown IShape based class");
            }
        }

        /// <summary>
        /// Removes the block or connector from the surface.
        /// Does NOT disconnect a block from its associated connectors and
        /// does NOT disconnect a connector from its start and end blocks.
        /// </summary>
        public void RemoveShape(GraphShape shape)
        {
            if (shape is GraphBlock block)
            {
                if (block.Equals(editBlock))
                {
                    DeselectEditBlock();
                }

                allBlocks.Remove(block);
            }
            else if (shape is GraphConnector connector)
            {
                allConnectors.Remove(connector);
            }
            else
            {
                throw new Exception("Unknown IShape based class");
            }
        }

        /// <summary>
        /// Mark this unattached block for deletion.
        /// Disconnect block from children and discard associated connectors.
        /// </summary>
        private void MarkBlockForDeletion(GraphBlock block, bool ignoreInConnectors)
        {
            Trace.Assert(ignoreInConnectors || block.GetInConnectors().Count == 0, "Cannot delete block with in connectors");
            Trace.Assert(block.IsEditable, "Cannot delete a 'read-only' block");

            foreach (GraphConnector connector in block.GetOutConnectors())
            {
                connector.EndBlock.UnlinkFrom(connector);
                RemoveShape(connector);
            }

            block.MarkForDeletion();

            if (block.Equals(editBlock))
            {
                DeselectEditBlock();
            }
        }

        /// <summary>
        /// Deletes an unattached chain of blocks from the bottom to the top
        /// by repeatedly calling MarkBlockForDeletion.
        /// </summary>
        private void MarkChainForDeletion(GraphBlock startBlock)
        {
            Trace.Assert(startBlock.GetInConnectors().Count <= 1, "Cannot delete block with multiple in connectors");

            foreach (GraphConnector connector in startBlock.GetOutConnectors()) // GetOutConnectors() allows us to use UnlinkFrom() below
            {
                if (connector.EndBlock.GetInConnectors().Count == 1)
                {
                    MarkChainForDeletion(connector.EndBlock);

                    connector.EndBlock.UnlinkFrom(connector);
                }
            }

            MarkBlockForDeletion(startBlock, true);
        }

        private bool SeenIdenticalConnector(int i)
        {
            for (int j = i - 1; j > 0; --j)
            {
                if (allConnectors[i].Equals(allConnectors[j]))
                {
                    return true;
                }
            }

            return false;
        }

        private int CountIdenticalConnectors(int i)
        {
            int count = 1;

            for (int j = i + 1; j < allConnectors.Count; ++j)
            {
                if (allConnectors[i].Equals(allConnectors[j]))
                {
                    ++count;
                }
            }

            return count;
        }
        #endregion

        #region Block Chain Retrieval and Manipulation
        private HashSet<GraphBlock> GetBlockChain(GraphBlock startBlock, bool meshOnly, bool materialsOnly, bool rightOnly)
        {
            HashSet<GraphBlock> chain = GetOutBlockChain(startBlock, meshOnly, materialsOnly);

            if (rightOnly)
            {
                int startX = startBlock.Centre.X;

                List<GraphBlock> leftOfStart = new List<GraphBlock>();

                foreach (GraphBlock block in chain)
                {
                    if (block.Centre.X < startX)
                    {
                        leftOfStart.Add(block);
                    }
                }

                foreach (GraphBlock block in leftOfStart)
                {
                    chain.Remove(block);
                }
            }

            return chain;
        }

        private HashSet<GraphBlock> GetOutBlockChain(GraphBlock startBlock, bool meshOnly, bool materialsOnly)
        {
            HashSet<GraphBlock> chain = new HashSet<GraphBlock>
            {
                startBlock
            };

            foreach (GraphConnector connector in startBlock.OutConnectors)
            {
                bool wanted = !(meshOnly || materialsOnly);

                if (startBlock.TypeId == Mmat.TYPE || startBlock.TypeId == Gzps.TYPE || startBlock.TypeId == Xmol.TYPE || startBlock.TypeId == Xtol.TYPE)
                {
                    if (meshOnly)
                    {
                        wanted = (connector.EndBlock.TypeId == Cres.TYPE);
                    }
                    else if (materialsOnly)
                    {
                        wanted = (connector.EndBlock.TypeId == Txmt.TYPE);
                    }
                }

                if (wanted) chain.UnionWith(GetOutBlockChain(connector.EndBlock, false, false));
            }

            return chain;
        }

        private void HideBlockChain(GraphBlock startBlock)
        {
            startBlock.Hide();

            foreach (GraphConnector outConnector in startBlock.OutConnectors)
            {
                List<GraphConnector> visibleInConnectors = new List<GraphConnector>();

                foreach (GraphConnector inConnector in outConnector.EndBlock.GetInConnectors())
                {
                    if (!inConnector.StartBlock.IsHidden)
                    {
                        visibleInConnectors.Add(inConnector);
                    }
                }

                if (visibleInConnectors.Count == 0)
                {
                    HideBlockChain(outConnector.EndBlock);
                }
            }

            if (startBlock.Equals(editBlock))
            {
                DeselectEditBlock();
            }
        }
        #endregion

        #region Editor Based Updates
        public void ChangeEditingSgName(string sgName, bool prefixLowerCase)
        {
            if (editBlock != null)
            {
                ChangeSgName(editBlock, sgName, prefixLowerCase);

                owningForm.UpdateEditor(editBlock);
            }
        }

        private void ChangeSgName(GraphBlock block, string sgName, bool prefixLowerCase)
        {
            if (block != null)
            {
                block.SetSgFullName(sgName, prefixLowerCase);
                block.SetDirty();

                foreach (GraphConnector connector in block.GetInConnectors())
                {
                    connector.StartBlock.SetDirty();
                }

                if (block.TypeId == Txtr.TYPE)
                {
                    if (Xflr.TYPE == block.SoleParent?.TypeId)
                    {
                        // Special case - also update the associated _detail TXTR
                        GraphBlock detailTxtrBlock = block.SoleParent.OutConnectorByLabel("texturetname_detail").EndBlock;

                        detailTxtrBlock.SetSgFullName($"{sgName}_detail", prefixLowerCase);
                        detailTxtrBlock.SetDirty();
                    }
                }
                else if (block.TypeId == Cres.TYPE)
                {
                    foreach (GraphConnector connector in block.OutConnectors)
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
                    foreach (GraphConnector connector in editBlock.SoleParent.OutConnectors)
                    {
                        connector.EndBlock.UpdateSoundName(name);
                    }

                    foreach (GraphConnector connector in editBlock.GetInConnectors())
                    {
                        connector.StartBlock.SetDirty();
                    }
                }

                owningForm.UpdateEditor(editBlock);
            }
        }

        public void FixEditingIssues(bool fixAll)
        {
            if (fixAll)
            {
                bool fixedAny;

                do
                {
                    fixedAny = false;

                    foreach (GraphBlock block in allBlocks)
                    {
                        if (!block.Equals(editBlock))
                        {
                            if (block.HasFixableIssues)
                            {
                                FixIssues(block);

                                fixedAny = true;
                                break;
                            }
                        }
                    }
                } while (fixedAny);
            }

            if (editBlock != null)
            {
                FixIssues(editBlock);

                owningForm.UpdateEditor(editBlock);
            }
        }

        private void FixIssues(GraphBlock fixBlock)
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
            else if (!fixBlock.IsDefaultLangValid)
            {
                fixBlock.FixLanguageIssues();
            }
            else if (fixBlock.TypeId == Txmt.TYPE)
            {
                fixBlock.FixFileListIssues();
            }

            Invalidate();
        }

        public void FixEditingTgir(bool fixAll)
        {
            if (fixAll)
            {
                bool fixedAny;

                do
                {
                    fixedAny = false;

                    foreach (GraphBlock block in allBlocks)
                    {
                        if (!block.Equals(editBlock))
                        {
                            if (!block.IsTgirValid)
                            {
                                FixTgir(block);

                                fixedAny = true;
                                break;
                            }
                        }
                    }
                } while (fixedAny);
            }

            if (editBlock != null)
            {
                FixTgir(editBlock);

                owningForm.UpdateEditor(editBlock);
            }
        }

        private void FixTgir(GraphBlock fixBlock)
        {
            fixBlock.FixTgir();

            List<GraphBlock> relinkBlocks = new List<GraphBlock>();

            foreach (GraphBlock block in allBlocks)
            {
                if (block.IsMissing && block.Key != null)
                {
                    if (block.Key.Equals(fixBlock.Key))
                    {
                        relinkBlocks.Add(block);
                    }
                }
            }

            foreach (GraphBlock block in relinkBlocks)
            {
                // Clone the list of in connectors, so we can modify the original within the next loop
                List<GraphConnector> inConnectors = block.GetInConnectors();
                foreach (GraphConnector connector in inConnectors)
                {
                    connector.SetEndBlock(fixBlock, false);
                    block.UnlinkFrom(connector);
                }

                block.Discard();
            }

            Invalidate();
        }
        #endregion

        #region Block Context Menu Events
        private void OnContextBlockOpening(object sender, CancelEventArgs e)
        {
            if (contextBlock != null)
            {
                if (IsMultiSelect)
                {
                    if (!selectedBlocks.Contains(contextBlock))
                    {
                        DeselectBlocks(false);
                        SelectEditBlock(contextBlock);
                    }
                }

                if (NoneMissingOrClone())
                {
                    menuItemContextBlockTexture.Visible = !IsMultiSelect && (contextBlock.TypeId == Mmat.TYPE || contextBlock.TypeId == Txmt.TYPE || contextBlock.TypeId == Txtr.TYPE || contextBlock.TypeId == Lifo.TYPE);

                    menuItemContextBlockDelete.Visible = !IsMultiSelect && advancedMode;
                    menuItemContextBlockDelete.Enabled = (contextBlock.IsEditable && contextBlock.GetInConnectors().Count == 0 && contextBlock.OutConnectors.Count == 0);

                    menuItemContextBlockDeleteChain.Visible = !IsMultiSelect && advancedMode;
                    menuItemContextBlockDeleteChain.Enabled = false;
                    if (contextBlock.IsEditable && contextBlock.GetInConnectors().Count == 0)
                    {
                        menuItemContextBlockDeleteChain.Enabled = true;

                        foreach (GraphBlock block in GetBlockChain(contextBlock, false, false, false))
                        {
                            if (!block.IsEditable)
                            {
                                menuItemContextBlockDeleteChain.Enabled = false;
                                break;
                            }
                        }
                    }

                    menuItemContextBlockHide.Visible = !IsMultiSelect && advancedMode;
                    menuItemContextBlockHide.Enabled = (contextBlock.GetInConnectors().Count == 0);

                    menuItemContextBlockHideChain.Visible = !IsMultiSelect && advancedMode;
                    menuItemContextBlockHideChain.Enabled = (contextBlock.GetInConnectors().Count == 0);

                    menuItemContextBlockExtract.Visible = menuItemContextBlockExport.Visible = false;
                    if (contextBlock.TypeId == Cres.TYPE)
                    {
                        menuItemContextBlockExtract.Text = (!IsMultiSelect ? "Extract Mesh" : "Extract Mesh");
                        menuItemContextBlockExtract.Visible = true;

                        menuItemContextBlockExport.Text = (!IsMultiSelect ? "Export Mesh" : "Export Meshes");
                        menuItemContextBlockExport.Visible = true;
                    }
                    else if (contextBlock.TypeId == Mmat.TYPE || contextBlock.TypeId == Gzps.TYPE || contextBlock.TypeId == Xmol.TYPE || contextBlock.TypeId == Xtol.TYPE)
                    {
                        menuItemContextBlockExtract.Text = (!IsMultiSelect ? "Extract Recolour" : "Extract Recolours");
                        menuItemContextBlockExtract.Visible = true;

                        menuItemContextBlockExport.Text = (!IsMultiSelect ? "Export Recolour" : "Export Recolours");
                        menuItemContextBlockExport.Visible = true;
                    }

                    menuItemContextBlockFixTgir.Visible = CanFixTgi();
                    menuItemContextBlockFixFileList.Visible = (contextBlock.TypeId == Txmt.TYPE) && CanFixFilelist();
                    menuItemContextBlockFixLight.Visible = (contextBlock.TypeId == Lamb.TYPE || contextBlock.TypeId == Ldir.TYPE || contextBlock.TypeId == Lpnt.TYPE || contextBlock.TypeId == Lspt.TYPE) && CanFixLight();
                    menuItemContextBlockFixLanguages.Visible = (contextBlock.TypeId == Str.TYPE) && CanFixLanguages();

                    menuItemContextBlockCopySgName.Visible = !IsMultiSelect && (contextBlock.SoleRcolParent != null);
                    menuItemContextBlockCopySgName.Enabled = (contextBlock.SoleRcolParent?.SgBaseName != null && !(contextBlock.TypeId == Lamb.TYPE || contextBlock.TypeId == Ldir.TYPE || contextBlock.TypeId == Lpnt.TYPE || contextBlock.TypeId == Lspt.TYPE));

                    menuItemContextBlockClosePackage.Visible = true;
                    menuItemContextBlockClosePackage.Enabled = CanClosePackages();
                    menuItemContextBlockOpenPackage.Visible = false;

                    menuItemContextBlockSplitBlock.Visible = !IsMultiSelect && advancedMode;
                    menuItemContextBlockSplitBlock.Enabled = (contextBlock.GetInConnectors().Count > 1);
                }
                else
                {
                    foreach (ToolStripItem item in menuContextBlock.Items)
                    {
                        item.Visible = false;
                    }

                    e.Cancel = true;

                    if (!IsMultiSelect)
                    {
                        if (contextBlock.IsAvailable && !contextBlock.IsMaxis)
                        {
                            menuItemContextBlockOpenPackage.Visible = true;
                            menuItemContextBlockOpenPackage.Enabled = true;

                            e.Cancel = false;
                        }
                    }

                    if (AllMaxis())
                    {
                        if (contextBlock.TypeId == Cres.TYPE)
                        {
                            if (GameData.GetMaxisResource(contextBlock.TypeId, contextBlock.Key) != null)
                            {
                                menuItemContextBlockExtract.Text = (!IsMultiSelect ? "Extract Maxis Mesh" : "Extract Maxis Meshes");
                                menuItemContextBlockExtract.Visible = true;
                                menuItemContextBlockExtract.Enabled = true;

                                menuItemContextBlockExport.Text = (!IsMultiSelect ? "Export Maxis Mesh" : "Export Maxis Meshes");
                                menuItemContextBlockExport.Visible = true;
                                menuItemContextBlockExport.Enabled = true;

                                e.Cancel = false;
                            }
                        }
                        else if (contextBlock.TypeId == Mmat.TYPE || contextBlock.TypeId == Gzps.TYPE || contextBlock.TypeId == Xmol.TYPE || contextBlock.TypeId == Xtol.TYPE)
                        {
                            if (GameData.GetMaxisResource(contextBlock.TypeId, contextBlock.Key) != null)
                            {
                                menuItemContextBlockExtract.Text = (!IsMultiSelect ? "Extract Maxis Recolour" : "Extract Maxis Recolours");
                                menuItemContextBlockExtract.Visible = true;
                                menuItemContextBlockExtract.Enabled = true;

                                menuItemContextBlockExport.Text = (!IsMultiSelect ? "Export Maxis Recolour" : "Export Maxis Recolours");
                                menuItemContextBlockExport.Visible = true;
                                menuItemContextBlockExport.Enabled = true;

                                e.Cancel = false;
                            }
                        }
                    }

                    if (!IsMultiSelect)
                    {
                        if (advancedMode && contextBlock.GetInConnectors().Count > 1)
                        {
                            menuItemContextBlockSplitBlock.Visible = true;
                            menuItemContextBlockSplitBlock.Enabled = true;

                            e.Cancel = false;
                        }
                    }
                }
            }
            else
            {
                e.Cancel = true;
            }
        }

        private bool NoneMissingOrClone()
        {
            if (!IsMultiSelect)
            {
                return !contextBlock.IsMissingOrClone;
            }
            else
            {
                foreach (GraphBlock block in selectedBlocks)
                {
                    if (block.IsMissingOrClone) return false;
                }

                return true;
            }
        }

        private bool AllMaxis()
        {
            if (!IsMultiSelect)
            {
                return contextBlock.IsMaxis;
            }
            else
            {
                foreach (GraphBlock block in selectedBlocks)
                {
                    if (!block.IsMaxis) return false;
                }

                return true;
            }
        }

        private bool CanFixTgi()
        {
            if (!IsMultiSelect)
            {
                return !contextBlock.IsTgirValid;
            }
            else
            {
                foreach (GraphBlock block in selectedBlocks)
                {
                    if (!block.IsTgirValid) return true;
                }

                return false;
            }
        }

        private bool CanFixFilelist()
        {
            if (!IsMultiSelect)
            {
                return !contextBlock.IsDirty && !contextBlock.IsFileListValid;
            }
            else
            {
                foreach (GraphBlock block in selectedBlocks)
                {
                    if (!block.IsDirty && !block.IsFileListValid) return true;
                }

                return false;
            }
        }

        private bool CanFixLight()
        {
            if (!IsMultiSelect)
            {
                return !contextBlock.IsDirty && !contextBlock.IsLightValid;
            }
            else
            {
                foreach (GraphBlock block in selectedBlocks)
                {
                    if (!block.IsDirty && !block.IsLightValid) return true;
                }

                return false;
            }
        }

        private bool CanFixLanguages()
        {
            if (!IsMultiSelect)
            {
                return !contextBlock.IsDirty && !contextBlock.IsDefaultLangValid;
            }
            else
            {
                foreach (GraphBlock block in selectedBlocks)
                {
                    if (!block.IsDirty && !block.IsDefaultLangValid) return true;
                }

                return false;
            }
        }

        private bool CanClosePackages()
        {
            if (!IsMultiSelect)
            {
                return !HasPendingEdits(contextBlock.PackagePath);
            }
            else
            {
                foreach (GraphBlock block in selectedBlocks)
                {
                    if (HasPendingEdits(block.PackagePath)) return false;
                }

                return true;
            }
        }

        private void OnContextBlockTexture(object sender, EventArgs e)
        {
            Trace.Assert(contextBlock.TypeId == Mmat.TYPE || contextBlock.TypeId == Txmt.TYPE || contextBlock.TypeId == Txtr.TYPE || contextBlock.TypeId == Lifo.TYPE, "Expected MMAT, TXMT, TXTR or LIFO");

            owningForm.DisplayTexture(contextBlock);
        }

        private void OnContextBlockDelete(object sender, EventArgs e)
        {
            Trace.Assert(contextBlock.OutConnectors.Count == 0, "Cannot delete block with out connectors");
            MarkBlockForDeletion(contextBlock, false);

            Invalidate();
        }

        private void OnContextBlockDeleteChain(object sender, EventArgs e)
        {
            MarkChainForDeletion(contextBlock);

            Invalidate();
        }

        private void OnContextBlockHide(object sender, EventArgs e)
        {
            Trace.Assert(contextBlock.GetInConnectors().Count == 0, "Cannot hide block with in connectors");

            contextBlock.Hide();

            if (contextBlock.Equals(editBlock))
            {
                DeselectEditBlock();
            }

            Invalidate();
        }

        private void OnContextBlockHideChain(object sender, EventArgs e)
        {
            Trace.Assert(contextBlock.GetInConnectors().Count == 0, "Cannot hide chain with in connectors");

            HideBlockChain(contextBlock);

            Invalidate();
        }

        private void OnContextBlockExport(object sender, EventArgs e)
        {
            if (!IsMultiSelect)
            {
                if (contextBlock.TypeId == Cres.TYPE)
                {
                    selectFileDialog.Title = $"Export as .package file - {contextBlock.SgBaseName}";
                }
                else
                {
                    selectFileDialog.Title = $"Export as .package file - {contextBlock.BlockName}";
                }

                if (selectFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(selectFileDialog.FileName))
                    {
                        // TODO - SceneGraph Plus - exporting - or should we disallow this?
                        Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(selectFileDialog.FileName, Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs, Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);
                    }

                    ExportBlock(contextBlock, selectFileDialog.FileName);

                    Invalidate();
                }
            }
            else
            {
                try
                {
                    foreach (GraphBlock block in selectedBlocks)
                    {
                        if (block.TypeId == Cres.TYPE)
                        {
                            selectFileDialog.Title = $"Export as .package file - {block.SgBaseName}";
                        }
                        else
                        {
                            selectFileDialog.Title = $"Export as .package file - {block.BlockName}";
                        }

                        int a = allBlocks.Count;

                        if (selectFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            if (File.Exists(selectFileDialog.FileName))
                            {
                                // TODO - SceneGraph Plus - exporting - or should we disallow this?
                                Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(selectFileDialog.FileName, Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs, Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);
                            }

                            ExportBlock(block, selectFileDialog.FileName);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                finally
                {
                    DeselectBlocks(true);
                }
            }
        }

        private void OnContextBlockExtract(object sender, EventArgs e)
        {
            if (selectPathDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                IExporter extractor = new Extractor();

                extractor.Open(selectPathDialog.FileName);

                if (!IsMultiSelect)
                {
                    ExtractBlock(extractor, contextBlock);
                }
                else
                {
                    foreach (GraphBlock block in selectedBlocks)
                    {
                        ExtractBlock(extractor, block);
                    }
                }

                extractor.Close();

                if (IsMultiSelect)
                {
                    DeselectBlocks(true);
                }
            }
        }

        private void OnContextBlockFixTgir(object sender, EventArgs e)
        {
            if (!IsMultiSelect)
            {
                FixTgir(contextBlock);
            }
            else
            {
                foreach (GraphBlock block in selectedBlocks)
                {
                    if (!block.IsTgirValid) FixTgir(block);
                }
            }
        }

        private void OnContextBlockFixFileList(object sender, EventArgs e)
        {
            if (!IsMultiSelect)
            {
                FixIssues(contextBlock);
            }
            else
            {
                foreach (GraphBlock block in selectedBlocks)
                {
                    if (!block.IsDirty && !block.IsFileListValid) FixIssues(block);
                }
            }
        }

        private void OnContextBlockFixLight(object sender, EventArgs e)
        {
            if (!IsMultiSelect)
            {
                FixIssues(contextBlock);
            }
            else
            {
                foreach (GraphBlock block in selectedBlocks)
                {
                    if (!block.IsDirty && !block.IsLightValid) FixIssues(block);
                }
            }
        }

        private void OnContextBlockFixLanguages(object sender, EventArgs e)
        {
            if (!IsMultiSelect)
            {
                FixIssues(contextBlock);
            }
            else
            {
                foreach (GraphBlock block in selectedBlocks)
                {
                    if (!block.IsDirty && !block.IsDefaultLangValid) FixIssues(block);
                }
            }
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
            // TODO - SceneGraph Plus - splitting a GZPS -> TXMT is very dangerous as the 3IDR index may have been reused!
            int shiftMultiplier = 0;

            foreach (GraphConnector connector in contextBlock.GetInConnectors())
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

        private void OnContextBlockClosePackage(object sender, EventArgs e)
        {
            if (!IsMultiSelect)
            {
                owningForm.ClosePackage(contextBlock.PackagePath);
            }
            else
            {
                HashSet<string> allreadyClosed = new HashSet<string>();

                foreach (GraphBlock block in selectedBlocks)
                {
                    string packagePath = block.PackagePath;

                    if (!allreadyClosed.Contains(packagePath))
                    {
                        allreadyClosed.Add(packagePath);
                        owningForm.ClosePackage(packagePath);
                    }
                }

                DeselectBlocks(true);
            }
        }
        #endregion

        #region Connector Context Menu Events
        private void OnContextConnectorOpening(object sender, CancelEventArgs e)
        {
            if (contextConnector != null)
            {
                if (CountIdenticalConnectors(allConnectors.IndexOf(contextConnector)) > 1)
                {
                    menuItemContextConnectorSplitMulti.Visible = true;
                    menuItemContextConnectorUnlink.Visible = false;
                }
                else if (contextConnector.StartBlock.TypeId == Str.TYPE && (contextConnector.EndBlock.TypeId == Cres.TYPE || contextConnector.EndBlock.TypeId == Txmt.TYPE))
                {
                    menuItemContextConnectorUnlink.Visible = true;
                    menuItemContextConnectorSplitMulti.Visible = false;
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

            foreach (GraphConnector connector in new List<GraphConnector>(allConnectors)) // Make a copy, as we're changing the connectors list
            {
                if (connector == contextConnector) continue;

                if (connector.Equals(contextConnector))
                {
                    connector.SetEndBlock(connector.EndBlock.MakeClone(new Point((ColumnGap / 8) * shiftMultiplier, (RowGap / 8) * shiftMultiplier)), false);

                    ++shiftMultiplier;
                }
            }
        }

        private void OnContextConnectorUnlink(object sender, EventArgs e)
        {
            Trace.Assert(contextConnector.StartBlock.TypeId == Str.TYPE, "Expected start block to be a STR#");
            Trace.Assert(contextConnector.EndBlock.TypeId == Cres.TYPE || contextConnector.EndBlock.TypeId == Txmt.TYPE, "Expected end block to be a CRES or TXMT");

            GraphBlock startBlock = contextConnector.StartBlock;
            GraphBlock disconnectedEndBlock = contextConnector.EndBlock;

            disconnectedEndBlock.UnlinkFrom(contextConnector);

            startBlock.UnconnectTo(contextConnector);
            startBlock.SetDirty();

            contextConnector.Discard();

            if (disconnectedEndBlock.IsMissingOrClone)
            {
                if (disconnectedEndBlock.GetInConnectors().Count == 0) disconnectedEndBlock.Discard();
            }

            Invalidate();
        }
        #endregion

        #region Filters
        public void CloseFilters()
        {
            owningForm.CloseFilters();
        }

        public void ApplyFilters(BlockFilters filters)
        {
            // Do NOT merge these two loops!!!
            foreach (GraphBlock block in allBlocks)
            {
                block.Filter(false);
            }

            foreach (GraphBlock block in allBlocks)
            {
                if (block.TypeId == Aged.TYPE || block.TypeId == Gzps.TYPE || block.TypeId == Xmol.TYPE || block.TypeId == Xtol.TYPE)
                {
                    if (filters.Exclude(block.Text))
                    {
                        FilterChain(block);
                    }
                }
            }
        }

        private void FilterChain(GraphBlock startBlock)
        {
            startBlock.Filter(true);

            foreach (GraphConnector outConnector in startBlock.OutConnectors)
            {
                List<GraphConnector> visibleInConnectors = new List<GraphConnector>();

                foreach (GraphConnector inConnector in outConnector.EndBlock.GetInConnectors())
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
                DeselectEditBlock();
            }
        }
        #endregion

        #region Extraction
        private void ExportBlock(GraphBlock block, string filename)
        {
            IExporter exporter = new Exporter();

            exporter.Open(filename);

            if (block.IsMaxis)
            {
                if (block.TypeId == Cres.TYPE)
                {
                    ExtractMaxisMesh(exporter, block);
                }
                else
                {
                    ExtractMaxisRecolour(exporter, block);
                }

                exporter.Close();
            }
            else
            {
                if (block.TypeId == Cres.TYPE)
                {
                    ExtractCustomMesh(exporter, block);
                }
                else
                {
                    ExtractCustomRecolour(exporter, block);
                }

                exporter.Close();

                owningForm.AddPackage(filename);
            }
        }

        private void ExtractBlock(IExporter extractor, GraphBlock block)
        {
            if (block.IsMaxis)
            {
                if (block.TypeId == Cres.TYPE)
                {
                    ExtractMaxisMesh(extractor, block);
                }
                else
                {
                    ExtractMaxisRecolour(extractor, block);
                }
            }
            else
            {
                if (block.TypeId == Cres.TYPE)
                {
                    ExtractCustomMesh(extractor, block);
                }
                else
                {
                    ExtractCustomRecolour(extractor, block);
                }
            }
        }

        private void ExtractCustomMesh(IExporter exporter, GraphBlock block)
        {
            ExtractCustomMeshOrRecolour(exporter, block, true);
        }

        private void ExtractCustomRecolour(IExporter exporter, GraphBlock block)
        {
            ExtractCustomMeshOrRecolour(exporter, block, false);
        }

        private void ExtractCustomMeshOrRecolour(IExporter exporter, GraphBlock startBlock, bool meshOnly)
        {
            List<GraphBlock> exportedBlocks = new List<GraphBlock>();
            bool exporting = (exporter is Exporter);

            foreach (GraphBlock block in GetBlockChain(startBlock, meshOnly, !meshOnly, false))
            {
                if (!(block.IsMaxis || block.IsMissingOrClone))
                {
                    if (block.IsEditable)
                    {
                        Trace.Assert(block.IsEditable, "Cannot export a 'read-only' block");

                        // TODO - SceneGraph Plus - exporting - what do we do about a block with multiple in connectors (eg a TXMT/TXTR ref'ed from multiple places, eg shadows)

                        CacheableDbpfFile exportPackage = packageCache.GetOrOpen(block.PackagePath);
                        DBPFResource res = exportPackage.GetResourceByKey(block.Key);

                        UpdateRefsToChildren(exportPackage, res, block, owningForm.IsPrefixLowerCase);

                        if (startBlock.TypeId == Cres.TYPE && (block.TypeId == Cres.TYPE || block.TypeId == Shpe.TYPE || block.TypeId == Gmnd.TYPE || block.TypeId == Gmdc.TYPE))
                        {
                            exporter.Extract(res);
                            if (exporting) exportedBlocks.Add(block);
                        }
                        else if (startBlock.TypeId == Mmat.TYPE && (block.TypeId == Mmat.TYPE || block.TypeId == Txmt.TYPE || block.TypeId == Txtr.TYPE || block.TypeId == Lifo.TYPE))
                        {
                            exporter.Extract(res);
                            if (exporting) exportedBlocks.Add(block);
                        }
                        else if ((startBlock.TypeId == Gzps.TYPE || startBlock.TypeId == Xmol.TYPE || startBlock.TypeId == Xtol.TYPE) && (block.TypeId == Gzps.TYPE || block.TypeId == Xmol.TYPE || block.TypeId == Xtol.TYPE || block.TypeId == Txmt.TYPE || block.TypeId == Txtr.TYPE || block.TypeId == Lifo.TYPE))
                        {
                            if (block.TypeId == Gzps.TYPE || block.TypeId == Xmol.TYPE || block.TypeId == Xtol.TYPE)
                            {
                                // We also need the 3IDR paired with the GZPS/XMOL/XTOL
                                Idr binxIdr = (Idr)exportPackage.GetResourceByKey(new DBPFKey(Idr.TYPE, block.Key));
                                exporter.Extract(binxIdr);

                                // We also need the associated BINX, we'll start by assuming the GZPS/3IDR/BINX is a triple
                                Binx binx = (Binx)exportPackage.GetResourceByKey(new DBPFKey(Binx.TYPE, block.Key));
                                DBPFKey objectKey = binxIdr.GetItem(binx.ObjectIdx);

                                if (!block.Key.Equals(objectKey))
                                {
                                    // Rats! Not a triple.  We need to find the BINX.3IDR pair that references this block
                                    binx = null;

                                    foreach (DBPFEntry entry in exportPackage.GetEntriesByType(Binx.TYPE))
                                    {
                                        Binx thisBinx = (Binx)exportPackage.GetResourceByEntry(entry);
                                        binxIdr = (Idr)exportPackage.GetResourceByKey(new DBPFKey(Idr.TYPE, entry));

                                        if (binxIdr != null)
                                        {
                                            objectKey = binxIdr.GetItem(thisBinx.ObjectIdx);

                                            if (block.Key.Equals(objectKey))
                                            {
                                                // Found it!
                                                binx = thisBinx;
                                                break;
                                            }
                                        }
                                    }

                                    if (binx != null)
                                    {
                                        // Need this as well
                                        exporter.Extract(binxIdr);
                                    }
                                }

                                if (binx != null)
                                {
                                    exporter.Extract(binx);

                                    exporter.Extract(exportPackage.GetResourceByKey(binxIdr.GetItem(binx.StringSetIdx)));
                                }
                                else
                                {
                                    // Oh fuck!
                                }
                            }

                            exporter.Extract(res);

                            if (exporting) exportedBlocks.Add(block);
                        }
                    }
                }
            }

            if (exporting && exportedBlocks.Count > 0)
            {
                GraphBlock deletableBlock;

                do
                {
                    deletableBlock = null;

                    foreach (GraphBlock block in exportedBlocks)
                    {
                        if (block.GetInConnectors().Count <= 1)
                        {
                            deletableBlock = block;
                            break;
                        }
                    }

                    if (deletableBlock != null)
                    {
                        if (deletableBlock.GetInConnectors().Count == 1)
                        {
                            GraphConnector connector = deletableBlock.GetInConnectors()[0];
                            connector.EndBlock.UnlinkFrom(connector);
                        }

                        // TODO - SceneGraph Plus - exporting - and for GZPS et al, delete the associated BINX/3IDR/STR# resources
                        CacheableDbpfFile package = packageCache.GetOrOpen(deletableBlock.PackagePath);
                        package.Remove(deletableBlock.OriginalKey);
                        owningForm.RemoveResource(package, deletableBlock.OriginalKey);

                        exportedBlocks.Remove(deletableBlock);

                        MarkBlockForDeletion(deletableBlock, false);
                        deletableBlock.Discard();
                    }
                } while (exportedBlocks.Count > 0 && deletableBlock != null);
            }
        }

        private void ExtractMaxisMesh(IExporter exporter, GraphBlock block)
        {
            if (block.TypeId == Cres.TYPE)
            {
                DBPFKey cresKey = block.Key;

                string maxisPath = GameData.GetMaxisPackagePath(Cres.TYPE, cresKey, true);

                if (maxisPath != null)
                {
                    Cres cres = (Cres)GameData.GetMaxisResource(Cres.TYPE, cresKey, true);

                    exporter.Extract(maxisPath, cres);

                    foreach (DBPFKey shpeKey in cres.ShpeKeys)
                    {
                        maxisPath = GameData.GetMaxisPackagePath(Shpe.TYPE, shpeKey, true);

                        if (maxisPath != null)
                        {
                            Shpe shpe = (Shpe)GameData.GetMaxisResource(Shpe.TYPE, shpeKey, true);

                            exporter.Extract(maxisPath, shpe);

                            foreach (string gmndName in shpe.GmndNames)
                            {
                                Gmnd gmnd = (Gmnd)GameData.GetMaxisResource(Gmnd.TYPE, gmndName, true);

                                if (gmnd != null)
                                {
                                    maxisPath = GameData.GetMaxisPackagePath(Gmnd.TYPE, gmnd, true);

                                    exporter.Extract(maxisPath, gmnd);

                                    foreach (DBPFKey gmdcKey in gmnd.GmdcKeys)
                                    {
                                        maxisPath = GameData.GetMaxisPackagePath(Gmdc.TYPE, gmdcKey, true);

                                        if (maxisPath != null)
                                        {
                                            exporter.Extract(maxisPath, gmdcKey);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void ExtractMaxisRecolour(IExporter exporter, GraphBlock block)
        {
            if (block.TypeId == Gzps.TYPE || block.TypeId == Xmol.TYPE || block.TypeId == Xtol.TYPE)
            {
                DBPFKey gzpsKey = block.Key;

                string maxisPath = GameData.GetMaxisPackagePath(Gzps.TYPE, gzpsKey, true);

                if (maxisPath != null)
                {
                    Gzps gzps = (Gzps)GameData.GetMaxisResource(Gzps.TYPE, gzpsKey, true);

                    DBPFKey idrKey = new DBPFKey(Idr.TYPE, gzpsKey);
                    string idrPath = GameData.GetMaxisPackagePath(Idr.TYPE, idrKey, true);

                    if (idrPath != null)
                    {
                        Idr idr = (Idr)GameData.GetMaxisResource(Idr.TYPE, idrKey, true);

                        exporter.Extract(maxisPath, gzps);
                        exporter.Extract(idrPath, idr);

                        foreach (uint index in gzps.TxmtIndexes)
                        {
                            DBPFKey txmtKey = idr.GetItem(index);

                            maxisPath = GameData.GetMaxisPackagePath(Txmt.TYPE, txmtKey, true);

                            if (maxisPath != null)
                            {
                                Txmt txmt = (Txmt)GameData.GetMaxisResource(Txmt.TYPE, txmtKey, true);

                                exporter.Extract(maxisPath, txmt);

                                foreach (string txtrName in txmt.TxtrKeyedNames.Values)
                                {
                                    Txtr txtr = (Txtr)GameData.GetMaxisResource(Txtr.TYPE, txtrName, true);

                                    if (txtr != null)
                                    {
                                        maxisPath = GameData.GetMaxisPackagePath(Txtr.TYPE, txtr, true);

                                        exporter.Extract(maxisPath, txtr);

                                        foreach (string lifoRef in txtr.ImageData.GetLifoRefs())
                                        {
                                            Lifo lifo = (Lifo)GameData.GetMaxisResource(Lifo.TYPE, lifoRef, true);

                                            if (lifo != null)
                                            {
                                                maxisPath = GameData.GetMaxisPackagePath(Lifo.TYPE, lifo, true);

                                                exporter.Extract(maxisPath, lifo);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region Key Tracking
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey || e.KeyCode == Keys.LShiftKey || e.KeyCode == Keys.RShiftKey)
            {
                if (dropOntoBlocks.Count != 0)
                {
                    lblTooltip.Visible = picThumbnail.Visible = false;

                    foreach (GraphBlock dropOntoBlock in dropOntoBlocks)
                    {
                        dropOntoBlock.BorderVisible = false;
                    }

                    dropOntoBlocks.Clear();

                    this.Invalidate();
                }
            }
            else if (e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.LControlKey || e.KeyCode == Keys.RControlKey)
            {
                if (dropOntoBlocks.Count != 0)
                {
                    lblTooltip.Visible = picThumbnail.Visible = false;

                    foreach (GraphBlock dropOntoBlock in dropOntoBlocks)
                    {
                        dropOntoBlock.BorderVisible = false;
                    }

                    dropOntoBlocks.Clear();

                    this.Invalidate();
                }

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

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.M || e.KeyCode == Keys.Oemcomma) && ((e.Modifiers & Keys.Control) == Keys.Control))
            {
                // Ctrl-M - enable move of selected multiple blocks
                // Ctrl-Shift-M - enable move of selected multiple block chains
                // Ctrl-Shift-Comma - enable move of selected multiple block chains, right only

                if (IsMultiSelect)
                {
                    bool includeChain = ((e.Modifiers & Keys.Shift) == Keys.Shift);
                    selectedBlockChain = new HashSet<GraphBlock>();

                    foreach (GraphBlock block in selectedBlocks)
                    {
                        if (includeChain)
                        {
                            foreach (GraphBlock chainBlock in GetBlockChain(block, false, false, (e.KeyCode == Keys.Oemcomma)))
                            {
                                selectedBlockChain.Add(chainBlock);
                            }
                        }
                        else
                        {
                            selectedBlockChain.Add(block);
                        }
                    }

                    startPoint = previousPoint = MousePosition;
                    moving = true;
                }
            }
            else if (e.KeyCode == Keys.Escape)
            {
                MouseEventArgs mouseEventArgs = new MouseEventArgs(MouseButtons.Left, 1, startPoint.X, startPoint.Y, 0);

                // Fake a MouseMove back to the starting position
                OnMouseMove(mouseEventArgs);
                // Fake a MouseUp
                OnMouseUp(mouseEventArgs);
            }

            base.OnKeyDown(e);
        }
        #endregion

        #region Mouse Tracking
        private void MouseClickTimer_Tick(object sender, EventArgs e)
        {
            // Clear double click watcher and timer
            inDoubleClick = false;
            mouseClickTimer.Stop();
        }

        protected override void OnMouseDown(MouseEventArgs mouseEventArgs)
        {
            if (mouseEventArgs.Button == MouseButtons.Left)
            {
                DeselectBlocks(false);

                if (advancedMode)
                {
                    DateTime now = DateTime.Now;

                    if (inDoubleClick)
                    {
                        inDoubleClick = false;

                        // If double click is valid, respond
                        if (doubleClickArea.Contains(mouseEventArgs.Location) && ((now - mouseLastClick) < doubleClickMaxTime))
                        {
                            mouseClickTimer.Stop();
                            DeselectBlocks(true);

                            OnBlockDoubleClick(PointToScreen(mouseEventArgs.Location));
                        }

                        return;
                    }

                    // Double click was invalid, restart 
                    mouseClickTimer.Stop(); mouseClickTimer.Start();
                    mouseLastClick = now;
                    inDoubleClick = true;
                    doubleClickArea = new Rectangle(mouseEventArgs.Location.X - (SystemInformation.DoubleClickSize.Width / 2), mouseEventArgs.Location.Y - (SystemInformation.DoubleClickSize.Width / 2), SystemInformation.DoubleClickSize.Width, SystemInformation.DoubleClickSize.Height);
                }

                // Left-Click to drag, Shift-Left-Click to drag and drop onto a block, Ctrl-Left-Click to drag and drop onto a connector
                bool hitAnything = false;
                for (int i = allBlocks.Count - 1; i >= 0; i--)
                {
                    if (hideMissingBlocks && allBlocks[i].IsMissing) continue;

                    if (allBlocks[i].HitTest(mouseEventArgs.Location))
                    {
                        if (firstSelectedBlock == null)
                        {
                            firstSelectedBlock = allBlocks[i];
                        }

                        SelectBlock(allBlocks[i]);

                        hitAnything = true;
                        break;
                    }
                }

                if (!hitAnything)
                {
                    DeselectEditBlock();

                    banding = true;
                    bandingStart = bandingLast = mouseEventArgs.Location;
                }

                if (IsSingleSelect /* firstSelectedBlock != null */)
                {
                    moving = true;
                    startPoint = previousPoint = mouseEventArgs.Location;

                    if (firstSelectedBlock != editBlock && editBlock != null) editBlock.IsEditing = false;

                    SelectEditBlock(firstSelectedBlock);
                }
            }
            else if (mouseEventArgs.Button == MouseButtons.Right)
            {
                if (Form.ModifierKeys == (Keys.Shift | Keys.Control))
                {
                    bool hitAnything = false;
                    for (int i = allBlocks.Count - 1; i >= 0; i--)
                    {
                        if (hideMissingBlocks && allBlocks[i].IsMissing) continue;

                        if (allBlocks[i].HitTest(mouseEventArgs.Location))
                        {
                            DeselectBlocks(false);
                            DeselectEditBlock();
                            SelectSingleBlock(allBlocks[i]);

                            foreach (GraphBlock block in allBlocks)
                            {
                                if (hideMissingBlocks && block.IsMissing) continue;
                                if (block == firstSelectedBlock) continue;

                                if (block.TypeId == firstSelectedBlock.TypeId)
                                {
                                    SelectBlock(block);
                                }
                            }

                            Invalidate();
                            hitAnything = true;
                            break;
                        }
                    }

                    if (!hitAnything)
                    {
                        DeselectBlocks(false);
                        DeselectEditBlock();
                    }
                }
                else if (Form.ModifierKeys == Keys.Shift)
                {
                    bool hitAnything = false;
                    for (int i = allBlocks.Count - 1; i >= 0; i--)
                    {
                        if (hideMissingBlocks && allBlocks[i].IsMissing) continue;

                        if (allBlocks[i].HitTest(mouseEventArgs.Location))
                        {
                            if (firstSelectedBlock == null)
                            {
                                if (editBlock != null)
                                {
                                    SelectSingleBlock(editBlock);
                                }

                                SelectBlock(allBlocks[i]);

                                if (firstSelectedBlock != editBlock && editBlock != null) editBlock.IsEditing = false;

                                SelectEditBlock(firstSelectedBlock);
                            }
                            else
                            {
                                if (firstSelectedBlock.TypeId == allBlocks[i].TypeId)
                                {
                                    SelectBlock(allBlocks[i]);
                                }
                            }

                            hitAnything = true;
                            break;
                        }
                    }

                    if (!hitAnything)
                    {
                        DeselectBlocks(false);
                        DeselectEditBlock();
                    }
                }
                else if (Form.ModifierKeys == Keys.Control)
                {
                }
                else
                {
                    lblTooltip.Visible = picThumbnail.Visible = false;

                    contextBlock = null;
                    contextConnector = null;

                    for (int i = allBlocks.Count - 1; i >= 0; i--)
                    {
                        if (hideMissingBlocks && allBlocks[i].IsMissing) continue;

                        if (allBlocks[i].HitTest(mouseEventArgs.Location))
                        {
                            contextBlock = allBlocks[i];
                            break;
                        }
                    }

                    for (int i = allConnectors.Count - 1; i >= 0; i--)
                    {
                        if (allConnectors[i].HitTest(mouseEventArgs.Location))
                        {
                            contextConnector = allConnectors[i];
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
                    else
                    {
                        DeselectBlocks(false);
                        DeselectEditBlock();
                    }
                }
            }

            base.OnMouseDown(mouseEventArgs);
        }

        protected override void OnMouseUp(MouseEventArgs mouseEventArgs)
        {
            if (mouseEventArgs.Button == MouseButtons.Left)
            {
                if (moving)
                {
                    // Trace.Assert(IsSingleSelect, "Cannot move multiple blocks");

                    lblTooltip.Visible = picThumbnail.Visible = false;

                    if (Form.ModifierKeys == Keys.Shift)
                    {
                        if (dropOntoBlocks.Count != 0)
                        {
                            foreach (GraphBlock dropOntoBlock in dropOntoBlocks)
                            {
                                dropOntoBlock.BorderVisible = false;

                                // Clone the list of in connectors, so we can modify the original within the next loop
                                List<GraphConnector> inConnectors = dropOntoBlock.GetInConnectors();
                                foreach (GraphConnector connector in inConnectors)
                                {
                                    connector.SetEndBlock(firstSelectedBlock, true);
                                    dropOntoBlock.UnlinkFrom(connector);

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

                            GraphBlock disconnectedEndBlock = dropOntoConnector.EndBlock;

                            dropOntoConnector.SetEndBlock(firstSelectedBlock, true);
                            if (firstSelectedBlock.TypeId == Txmt.TYPE && dropOntoConnector.StartBlock.TypeId == Mmat.TYPE)
                            {
                                dropOntoConnector.StartBlock.BlockName = firstSelectedBlock.SgFullName;
                            }

                            dropOntoConnector.StartBlock.SetDirty();

                            if (disconnectedEndBlock.IsMissingOrClone)
                            {
                                if (disconnectedEndBlock.GetInConnectors().Count == 0) disconnectedEndBlock.Discard();
                            }

                            dropOntoConnector = null;

                            this.Invalidate();
                        }
                    }
                    else if (Form.ModifierKeys == (Keys.Shift | Keys.Control))
                    {
                        if (dropOntoBlocks.Count != 0)
                        {
                            foreach (GraphBlock dropOntoBlock in dropOntoBlocks)
                            {
                                dropOntoBlock.BorderVisible = false;

                                if (!dropOntoBlock.IsMissingOrClone)
                                {
                                    Trace.Assert(firstSelectedBlock.TypeId == Cres.TYPE || firstSelectedBlock.TypeId == Txmt.TYPE, "Can only ctrl-shift-drop a CRES or TXMT block!");
                                    Trace.Assert(dropOntoBlock.TypeId == Str.TYPE, "Can only ctrl-shift-drop onto a STR!");

                                    string sgFullName = firstSelectedBlock.SgFullName;
                                    if (sgFullName.EndsWith("_cres", StringComparison.OrdinalIgnoreCase)) sgFullName = sgFullName.Substring(0, sgFullName.Length - 5);
                                    if (sgFullName.EndsWith("_txmt", StringComparison.OrdinalIgnoreCase)) sgFullName = sgFullName.Substring(0, sgFullName.Length - 5);

                                    CacheableDbpfFile package = packageCache.GetOrAdd(dropOntoBlock.PackagePath);
                                    Str str = (Str)package.GetResourceByKey(dropOntoBlock.Key);

                                    int index = str.AppendLanguageItem(Languages.Default, new StrItem(Languages.Default, sgFullName, ""));

                                    package.Commit(str);
                                    dropOntoBlock.SetDirty();

                                    dropOntoBlock.ConnectTo(index, sgFullName, firstSelectedBlock);
                                }
                            }

                            dropOntoBlocks.Clear();

                            Invalidate();
                        }

                    }

                    if (dropToGrid)
                    {
                        // This may only work for ColumnGap = RowGap = 100
                        int gridX = (int)((ColumnGap / 2) * gridScale);
                        int gridY = (int)((RowGap / 2) * gridScale);

                        int offsetX = (gridX == ColumnGap) ? ColumnGap / 2 : 0;
                        int offsetY = (gridY == RowGap) ? RowGap / 2 : 0;

                        if (firstSelectedBlock == null && selectedBlockChain != null)
                        {
                            IEnumerator<GraphBlock> en = selectedBlockChain.GetEnumerator();
                            en.MoveNext();
                            firstSelectedBlock = en.Current;
                        }

                        Point newCentre;

                        if (gridScale <= 1.0f)
                        {
                            newCentre = new Point(((firstSelectedBlock.Centre.X + gridX / 2) / gridX) * gridX - offsetX, ((firstSelectedBlock.Centre.Y + gridY / 2) / gridY) * gridY - offsetY);
                        }
                        else
                        {
                            newCentre = new Point(((firstSelectedBlock.Centre.X + gridX) / gridX) * gridX - offsetX, ((firstSelectedBlock.Centre.Y + gridY) / gridY) * gridY - offsetY);
                        }

                        Point delta = new Point(newCentre.X - firstSelectedBlock.Centre.X, newCentre.Y - firstSelectedBlock.Centre.Y);

                        if (selectedBlockChain != null)
                        {
                            foreach (GraphBlock block in selectedBlockChain)
                            {
                                block.Move(delta);
                            }
                        }
                        else
                        {
                            firstSelectedBlock.Centre = newCentre;
                        }

                        this.Invalidate();
                    }

                    DeselectBlocks(true);
                    selectedBlockChain = null;
                    moving = false;
                    banding = false;
                }
                else if (banding)
                {
                    List<GraphBlock> bandedBlocks = new List<GraphBlock>();

                    foreach (GraphBlock block in allBlocks)
                    {
                        if (hideMissingBlocks && block.IsMissing) continue;

                        if (block.HitTest(bandingStart, bandingLast))
                        {
                            bandedBlocks.Add(block);
                        }
                    }

                    double shortestDistance = double.MaxValue;
                    GraphBlock firstBandedBlock = null;

                    foreach (GraphBlock block in bandedBlocks)
                    {
                        double distance = block.DistanceFrom(bandingStart);

                        if (distance < shortestDistance)
                        {
                            shortestDistance = distance;
                            firstBandedBlock = block;
                        }
                    }

                    SelectSingleBlock(firstBandedBlock);

                    foreach (GraphBlock block in bandedBlocks)
                    {
                        if (block != firstBandedBlock)
                        {
                            if (firstBandedBlock.TypeId == block.TypeId)
                            {
                                SelectBlock(block);
                            }
                        }
                    }

                    banding = false;
                    Invalidate();
                }
            }
            else if (mouseEventArgs.Button == MouseButtons.Right)
            {
            }

            base.OnMouseUp(mouseEventArgs);
        }

        protected override void OnMouseMove(MouseEventArgs mouseEventArgs)
        {
            if (moving)
            {
                // Trace.Assert(IsSingleSelect, "Cannot move multiple blocks");

                lblTooltip.Visible = picThumbnail.Visible = false;

                Point delta = new Point(mouseEventArgs.X - previousPoint.X, mouseEventArgs.Y - previousPoint.Y);

                if (selectedBlockChain == null && (Form.ModifierKeys & Keys.Alt) == Keys.Alt)
                {
                    if (advancedMode)
                    {
                        selectedBlockChain = GetBlockChain(firstSelectedBlock, false, false, (Form.ModifierKeys & Keys.Control) != Keys.Control);
                    }
                }

                if (selectedBlockChain != null)
                {
                    foreach (GraphBlock block in selectedBlockChain)
                    {
                        if (firstSelectedBlock == null) firstSelectedBlock = block;

                        block.Move(delta);
                    }
                }
                else
                {
                    firstSelectedBlock.Move(delta);
                }

                previousPoint = mouseEventArgs.Location;

                bool canDrop = (!firstSelectedBlock.IsClone) && (Form.ModifierKeys != Keys.Alt);
                if (!advancedMode && firstSelectedBlock.IsMissing) canDrop = false;

                if (firstSelectedBlock.TypeId == Str.TYPE) canDrop = false;

                if (canDrop)
                {
                    if (Form.ModifierKeys == Keys.Shift)
                    {
                        List<GraphBlock> currentDropOntoBlocks = new List<GraphBlock>();

                        for (int i = allBlocks.Count - 1; i >= 0; i--)
                        {
                            GraphBlock block = allBlocks[i];

                            if (hideMissingBlocks && block.IsMissing) continue;

                            if (block != firstSelectedBlock && block.GetInConnectors().Count > 0 && block.TypeId == firstSelectedBlock.TypeId && block.HitTest(firstSelectedBlock.Centre))
                            {
                                if (!dropOntoBlocks.Contains(block))
                                {
                                    block.BorderVisible = true;
                                }

                                currentDropOntoBlocks.Add(block);
                            }
                        }

                        foreach (GraphBlock dropOntoBlock in dropOntoBlocks)
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
                        GraphConnector currentDropOntoConnector = null;

                        for (int i = 0; i < allConnectors.Count; ++i)
                        {
                            GraphConnector connector = allConnectors[i];

                            if (connector.HitTest(firstSelectedBlock.Centre))
                            {
                                if (connector.EndBlock.TypeId == firstSelectedBlock.TypeId)
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
                    else if (Form.ModifierKeys == (Keys.Shift | Keys.Control))
                    {
                        if (firstSelectedBlock.TypeId == Cres.TYPE || firstSelectedBlock.TypeId == Txmt.TYPE)
                        {
                            List<GraphBlock> currentDropOntoBlocks = new List<GraphBlock>();

                            for (int i = allBlocks.Count - 1; i >= 0; i--)
                            {
                                GraphBlock block = allBlocks[i];

                                if (hideMissingBlocks && block.IsMissing) continue;

                                if (block.TypeId == Str.TYPE && block.HitTest(firstSelectedBlock.Centre))
                                {
                                    // "UnderstoodStrings"
                                    if ((firstSelectedBlock.TypeId == Cres.TYPE && block.InstanceId == DBPFData.STR_MODELS) ||
                                        (firstSelectedBlock.TypeId == Txmt.TYPE && block.InstanceId == DBPFData.STR_MATERIALS))
                                    {
                                        if (!dropOntoBlocks.Contains(block))
                                        {
                                            block.BorderVisible = true;
                                        }

                                        currentDropOntoBlocks.Add(block);
                                    }
                                }
                            }

                            foreach (GraphBlock dropOntoBlock in dropOntoBlocks)
                            {
                                if (!currentDropOntoBlocks.Contains(dropOntoBlock))
                                {
                                    dropOntoBlock.BorderVisible = false;
                                }
                            }

                            dropOntoBlocks.Clear();
                            dropOntoBlocks = currentDropOntoBlocks;
                        }
                    }
                }

                this.Invalidate();
            }
            else if (banding)
            {
                bandingLast = mouseEventArgs.Location;
                Invalidate();
            }
            else
            {
                GraphBlock currentHoverBlock = null;

                for (int i = allBlocks.Count - 1; i >= 0; i--)
                {
                    if (hideMissingBlocks && allBlocks[i].IsMissing) continue;

                    if (allBlocks[i].HitTest(mouseEventArgs.Location))
                    {
                        currentHoverBlock = allBlocks[i];
                        break;
                    }
                }

                if (currentHoverBlock != null)
                {
                    if (currentHoverBlock != hoverBlock)
                    {
                        if (hoverBlock != null) hoverBlock.BorderVisible = false;

                        hoverBlock = currentHoverBlock;

                        if (advancedMode)
                        {
                            if (hoverBlock.TypeId == Mmat.TYPE || hoverBlock.TypeId == Objd.TYPE || hoverBlock.TypeId == Gmnd.TYPE || hoverBlock.TypeId == Txmt.TYPE || hoverBlock.TypeId == Txtr.TYPE)
                            {
                                ChangeCursor(Cursors.Hand);
                            }
                            else
                            {
                                ChangeCursor();
                            }
                        }

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

                                toolTip = $"{toolTip}{hoverBlock.IssuesToolTip}";

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
                        if (advancedMode) ChangeCursor();

                        lblTooltip.Visible = picThumbnail.Visible = false;

                        hoverBlock.BorderVisible = false;

                        hoverBlock = null;

                        this.Invalidate();
                    }
                }

                GraphConnector currentHoverConnector = null;

                if (connectorsOver || currentHoverBlock == null)
                {
                    for (int i = 0; i < allConnectors.Count; ++i)
                    {
                        GraphConnector connector = allConnectors[i];

                        if (hideMissingBlocks && (connector.StartBlock.IsMissing || connector.EndBlock.IsMissing)) continue;

                        if (connector.HitTest(mouseEventArgs.Location))
                        {
                            currentHoverConnector = allConnectors[i];
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

                        foreach (GraphConnector connector in allConnectors)
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

            base.OnMouseMove(mouseEventArgs);
        }
        #endregion

        #region Double Click Processing
        private void OnBlockDoubleClick(Point mouseScreenLocation)
        {
            if (hoverBlock != null)
            {
                if (hoverBlock.IsMissingOrClone) return;

                if (hoverBlock.TypeId == Mmat.TYPE)
                {
                    CacheableDbpfFile mmatPackage = packageCache.GetOrAdd(hoverBlock.PackagePath);

                    Mmat mmat = (Mmat)mmatPackage.GetResourceByKey(hoverBlock.OriginalKey);
                    Trace.Assert(mmat != null, $"Double-Click: Missing resource for {hoverBlock.OriginalKey}");

                    string originalSubsetName = mmat.SubsetName;

                    List<GraphBlock> gmndBlocks = GetGmndBlocks(hoverBlock);
                    List<string> subsets = new List<string>();

                    if (gmndBlocks.Count > 0)
                    {
                        foreach (GraphBlock gmndBlock in gmndBlocks)
                        {
                            subsets.AddRange(((Gmnd)packageCache.GetOrAdd(gmndBlock.PackagePath).GetResourceByKey(gmndBlock.OriginalKey)).GetDesignModeEnabledSubsets());
                        }
                    }

                    if ((new MmatDialog().ShowDialog(mouseScreenLocation, mmat, subsets)) == DialogResult.OK)
                    {
                        if (mmat.IsDirty)
                        {
                            mmatPackage.Commit(mmat);
                            hoverBlock.SetDirty();

                            Invalidate();

                            if (!mmat.SubsetName.Equals(originalSubsetName))
                            {
                                GraphConnector subsetConnector = hoverBlock.OutConnectorByLabel(originalSubsetName);
                                Trace.Assert(subsetConnector.EndBlock.TypeId == Txmt.TYPE, "Expected subset to reference TXMT!");
                                subsetConnector.Label = mmat.SubsetName;
                            }

                            ValidateBlock(hoverBlock);
                        }

                        string suffix = "";

                        if (mmat.IsHiddenInCatalog)
                        {
                            suffix = " (Hide)";
                        }
                        else if (mmat.DefaultMaterial)
                        {
                            suffix = "(Def)";
                        }

                        hoverBlock.Text = $"MMAT{suffix}";
                    }
                }
                else if (hoverBlock.TypeId == Objd.TYPE)
                {
                    CacheableDbpfFile objdPackage = packageCache.GetOrAdd(hoverBlock.PackagePath);

                    Objd objd = (Objd)objdPackage.GetResourceByKey(hoverBlock.OriginalKey);
                    Trace.Assert(objd != null, $"Double-Click: Missing resource for {hoverBlock.OriginalKey}");

                    Ctss ctss = (Ctss)objdPackage.GetResourceByKey(new DBPFKey(Ctss.TYPE, objd.GroupID, (TypeInstanceID)objd.GetRawData(ObjdIndex.CatalogueStringsId), DBPFData.RESOURCE_NULL));

                    TypeGUID originalGuid = objd.Guid;
                    int defaultGraphic = objd.GetRawData(ObjdIndex.DefaultGraphic);
                    List<GraphBlock> gmndBlocks = GetGmndBlocks(hoverBlock, defaultGraphic);

                    string cresSgName = null;
                    Dictionary<string, SubsetData> subsets = new Dictionary<string, SubsetData>();

                    if (gmndBlocks.Count > 0)
                    {
                        GraphBlock cresBlock = hoverBlock.OutConnectorByLabel("Model Names").EndBlock.OutConnectorByIndex(defaultGraphic).EndBlock;
                        cresSgName = ((Cres)packageCache.GetOrAdd(cresBlock.PackagePath).GetResourceByKey(cresBlock.OriginalKey)).SgName;

                        foreach (GraphBlock gmndBlock in gmndBlocks)
                        {
                            GraphBlock shpeBlock = GetShpeBlock(gmndBlock);
                            Shpe shpe = (Shpe)packageCache.GetOrAdd(shpeBlock.PackagePath).GetResourceByKey(shpeBlock.OriginalKey);

                            GraphBlock gmdcBlock = GetGmdcBlock(gmndBlock);
                            Gmnd gmnd = (Gmnd)packageCache.GetOrAdd(gmndBlock.PackagePath).GetResourceByKey(gmndBlock.OriginalKey);

                            if (gmdcBlock != null)
                            {
                                foreach (string subset in ((Gmdc)packageCache.GetOrAdd(gmdcBlock.PackagePath).GetResourceByKey(gmdcBlock.OriginalKey)).Subsets)
                                {
                                    subsets.Add(subset, new SubsetData(subset, shpe.GetSubsetMaterial(subset), gmndBlock, gmnd));
                                }
                            }
                        }
                    }

                    bool hasMaterials = (hoverBlock.OutConnectorByIndex(1) != null);

                    if ((new ObjdDialog().ShowDialog(owningForm, mouseScreenLocation, objdPackage, objd, ctss, cresSgName, subsets, hasMaterials)) == DialogResult.OK)
                    {
                        if (objd.IsDirty)
                        {
                            objdPackage.Commit(objd);
                            hoverBlock.SetDirty();

                            if (!originalGuid.Equals(objd.Guid))
                            {
                                hoverBlock.SetGuid(objd.Guid);
                                owningForm.UpdateEditor(hoverBlock);

                                // Also need to update any linked MMATs
                                foreach (GraphConnector inConnector in hoverBlock.GetInConnectors())
                                {
                                    if (inConnector.StartBlock.TypeId == Mmat.TYPE)
                                    {
                                        inConnector.StartBlock.SetDirty();
                                    }
                                }
                            }
                        }

                        if (ctss != null && ctss.IsDirty)
                        {
                            objdPackage.Commit(ctss);
                            hoverBlock.SetDirty(); // Only way we can get the Save All button enabled
                        }

                        bool dirtyGmnds = false;

                        foreach (SubsetData subsetData in subsets.Values)
                        {
                            if (subsetData.OwningGmnd.IsDirty)
                            {
                                packageCache.GetOrAdd(subsetData.OwningGmndBlock.PackagePath).Commit(subsetData.OwningGmnd);
                                subsetData.OwningGmndBlock.SetDirty();

                                dirtyGmnds = true;
                            }
                        }

                        if (objd.IsDirty || (ctss != null && ctss.IsDirty) || dirtyGmnds)
                        {
                            Invalidate();
                        }

                        if (!hasMaterials)
                        {
                            Str materials = (Str)objdPackage.GetResourceByKey(new DBPFKey(Str.TYPE, objd.GroupID, DBPFData.STR_MATERIALS, DBPFData.RESOURCE_NULL));

                            if (materials != null)
                            {
                                owningForm.AddResource(objdPackage, materials);
                            }
                        }
                    }
                }
                else if (hoverBlock.TypeId == Gmnd.TYPE)
                {
                    CacheableDbpfFile gmndPackage = packageCache.GetOrAdd(hoverBlock.PackagePath);

                    Gmnd gmnd = (Gmnd)gmndPackage.GetResourceByKey(hoverBlock.OriginalKey);
                    Trace.Assert(gmnd != null, $"Double-Click: Missing resource for {hoverBlock.OriginalKey}");

                    Gmdc gmdc = null;

                    if (hoverBlock.OutConnectors.Count == 1)
                    {
                        GraphBlock gmdcBlock = hoverBlock.OutConnectors[0].EndBlock;
                        Trace.Assert(gmdcBlock.TypeId == Gmdc.TYPE, $"Double-Click: Expected a GMDC block");

                        CacheableDbpfFile gmdcPackage = packageCache.GetOrAdd(gmdcBlock.PackagePath);
                        gmdc = (Gmdc)gmdcPackage.GetResourceByKey(gmdcBlock.OriginalKey);
                    }

                    if ((new GmndDialog().ShowDialog(mouseScreenLocation, gmnd, gmdc)) == DialogResult.OK)
                    {
                        if (gmnd.IsDirty)
                        {
                            gmndPackage.Commit(gmnd);
                            hoverBlock.SetDirty();

                            Invalidate();
                        }
                    }
                }
                else if (hoverBlock.TypeId == Gzps.TYPE)
                {
                    // TODO - SceneGraph Plus - options dialogs - can this also be used for XMOLs and XTOLS?
                    CacheableDbpfFile gzpsPackage = packageCache.GetOrAdd(hoverBlock.PackagePath);

                    Gzps gzps = (Gzps)gzpsPackage.GetResourceByKey(hoverBlock.OriginalKey);
                    Trace.Assert(gzps != null, $"Double-Click: Missing resource for {hoverBlock.OriginalKey}");

                    Idr idrForGzps = (Idr)gzpsPackage.GetResourceByKey(new DBPFKey(Idr.TYPE, gzps));

                    Binx binx = null;
                    Idr idrForBinx = null;

                    Str str = null;
                    int strIndex = -1;

                    foreach (DBPFEntry entry in gzpsPackage.GetEntriesByType(Binx.TYPE))
                    {
                        binx = (Binx)gzpsPackage.GetResourceByEntry(entry);
                        Idr idr = (Idr)gzpsPackage.GetResourceByKey(new DBPFKey(Idr.TYPE, entry));

                        if (idr != null)
                        {
                            DBPFKey objectKey = idr.GetItem(binx.ObjectIdx);

                            if (gzps.Equals(objectKey))
                            {
                                // We've found the required binx/3idr pair
                                idrForBinx = idr;
                                break;
                            }
                        }
                    }

                    if (idrForBinx != null)
                    {
                        str = (Str)gzpsPackage.GetResourceByKey(idrForBinx.GetItem(binx.StringSetIdx));
                        strIndex = (int)binx.StringIndex;
                    }

                    bool removeLifos = false;

                    int numOverrides = (int)gzps.GetItem("numoverrides").UIntegerValue;
                    List<MaterialData> materials = new List<MaterialData>(numOverrides);

                    HashSet<uint> seenIdrRefs = new HashSet<uint>();

                    for (int i = 0; i < numOverrides; ++i)
                    {
                        string subset = gzps.GetItem($"override{i}subset").StringValue;
                        uint idrIndex = gzps.GetItem($"override{i}resourcekeyidx").UIntegerValue;

                        if (seenIdrRefs.Contains(idrIndex))
                        {
                            foreach (MaterialData materialData in materials)
                            {
                                if (materialData.idrIndex == idrIndex)
                                {
                                    materialData.subsets.Add(subset);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            MaterialData materialData = new MaterialData();
                            materials.Add(materialData);

                            materialData.subsets.Add(subset);
                            materialData.idrIndex = idrIndex;

                            seenIdrRefs.Add(idrIndex);

                            GraphBlock txmtBlock = hoverBlock.OutConnectorByLabel(materialData.SubsetDisplay)?.EndBlock;
                            GraphBlock txtrBlock = txmtBlock?.OutConnectorByLabel("stdMatBaseTextureName")?.EndBlock;

                            if (txmtBlock != null && !txmtBlock.IsMissing)
                            {
                                materialData.txmtPackage = packageCache.GetOrAdd(txmtBlock.PackagePath);
                                materialData.txmt = (Txmt)materialData.txmtPackage.GetResourceByKey(txmtBlock.Key);

                                if (txtrBlock != null && !txtrBlock.IsMissing)
                                {
                                    materialData.txtrPackage = packageCache.GetOrAdd(txtrBlock.PackagePath);
                                    materialData.txtr = (Txtr)materialData.txmtPackage.GetResourceByKey(txtrBlock.Key);

                                    // TODO - we need to set up lifos as well
                                    BuildLifoLists(txtrBlock, materialData.lifoConectors, materialData.lifos);
                                }
                            }
                        }
                    }

                    if ((new GzpsDialog().ShowDialog(owningForm, mouseScreenLocation, gzpsPackage, hoverBlock, gzps, idrForGzps, binx, idrForBinx, str, strIndex, materials, out removeLifos)) == DialogResult.OK)
                    {
                        // TODO - what goes in here? We must be looping the materials list
                        foreach (MaterialData material in materials)
                        {
                            if (material.txtr != null && material.txtr.IsDirty)
                            {
                                material.txtrPackage.Commit(material.txtr);
                                material.txtrBlock.SetDirty();

                                UpdateLifos(removeLifos, material.lifoConectors, material.lifos);
                            }

                        }

                        // TODO - what goes in here?
                    }

                    if (str != null && str.IsDirty)
                    {
                        gzpsPackage.Commit(str);
                        hoverBlock.SetDirty();
                    }
                }
                else if (hoverBlock.TypeId == Txmt.TYPE)
                {
                    CacheableDbpfFile txmtPackage = packageCache.GetOrAdd(hoverBlock.PackagePath);

                    Txmt txmt = (Txmt)txmtPackage.GetResourceByKey(hoverBlock.OriginalKey);
                    Trace.Assert(txmt != null, $"Double-Click: Missing resource for {hoverBlock.OriginalKey}");

                    GraphBlock gzpsBlock = null;
                    GraphBlock mmatBlock = null;
                    GraphBlock shpeBlock = null;
                    GraphBlock objdBlock = null;

                    GraphConnector shpeConnector = null;

                    TypeGUID guid;
                    TypeGroupID mmatGroup;
                    string cresSgName = null;
                    List<string> subsets = new List<string>();

                    foreach (GraphConnector inConnector in hoverBlock.GetInConnectors())
                    {
                        if (inConnector.StartBlock.TypeId == Gzps.TYPE)
                        {
                            gzpsBlock = inConnector.StartBlock;
                        }
                        else if (inConnector.StartBlock.TypeId == Mmat.TYPE)
                        {
                            mmatBlock = inConnector.StartBlock;
                        }
                        else if (inConnector.StartBlock.TypeId == Shpe.TYPE)
                        {
                            shpeBlock = inConnector.StartBlock;
                            shpeConnector = inConnector;
                        }
                    }

                    if (mmatBlock != null)
                    {
                        // TXMT -> MMAT -> OBJD
                        Mmat mmat = (Mmat)packageCache.GetOrAdd(mmatBlock.PackagePath).GetResourceByKey(mmatBlock.OriginalKey);

                        guid = mmat.ObjectGUID;
                        cresSgName = mmat.ModelName;
                        subsets.Add(mmat.SubsetName);

                        foreach (GraphConnector outConnector in mmatBlock.OutConnectors)
                        {
                            if (outConnector.EndBlock.TypeId == Objd.TYPE)
                            {
                                objdBlock = outConnector.EndBlock;
                                break;
                            }
                        }
                    }
                    else if (shpeBlock != null)
                    {
                        // TXMT -> SHPE -> CRES -> STR# -> OBJD
                        foreach (GraphConnector inConnector in shpeBlock.GetInConnectors())
                        {
                            if (inConnector.StartBlock.TypeId == Cres.TYPE)
                            {
                                foreach (GraphConnector cresInConnector in inConnector.StartBlock.GetInConnectors())
                                {
                                    if (cresInConnector.StartBlock.TypeId == Str.TYPE)
                                    {
                                        foreach (GraphConnector strInConnector in cresInConnector.StartBlock.GetInConnectors())
                                        {
                                            if (strInConnector.StartBlock.TypeId == Objd.TYPE)
                                            {
                                                objdBlock = strInConnector.StartBlock;

                                                break;
                                            }
                                        }

                                        break;
                                    }
                                }

                                break;
                            }
                        }
                    }

                    Objd objd = (Objd)txmtPackage.GetResourceByKey(objdBlock?.OriginalKey);

                    if (objd != null)
                    {
                        int defaultGraphic = objd.GetRawData(ObjdIndex.DefaultGraphic);
                        List<GraphBlock> gmndBlocks = GetGmndBlocks(shpeBlock);

                        guid = objd.Guid;
                        mmatGroup = objd.GroupID;

                        if (gmndBlocks != null)
                        {
                            GraphBlock cresBlock = objdBlock.OutConnectorByLabel("Model Names").EndBlock.OutConnectorByIndex(defaultGraphic).EndBlock;

                            if (cresBlock != null)
                            {
                                cresSgName = cresBlock.SgFullName;
                            }

                            foreach (GraphBlock gmndBlock in gmndBlocks)
                            {
                                CacheableDbpfFile gmndPackage = packageCache.GetOrAdd(gmndBlock.PackagePath);
                                Gmnd gmnd = (Gmnd)gmndPackage.GetResourceByKey(gmndBlock.OriginalKey);

                                if (gmnd != null)
                                {
                                    foreach (string subset in gmnd.GetDesignModeEnabledSubsets())
                                    {
                                        if (shpeConnector.Label.Equals(subset))
                                        {
                                            if (!subsets.Contains(subset)) subsets.Add(subset);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        guid = DBPFData.GUID_NULL;
                        mmatGroup = DBPFData.GROUP_NULL;
                    }

                    GraphBlock txtrBlock = hoverBlock.OutConnectorByLabel("stdMatBaseTextureName")?.EndBlock;
                    CacheableDbpfFile txtrPackage = null;
                    Txtr txtr = null;

                    bool removeLifos = false;
                    List<GraphConnector> lifoConectors = new List<GraphConnector>();
                    List<Lifo> lifos = new List<Lifo>();

                    if (txtrBlock != null)
                    {
                        txtrPackage = packageCache.GetOrAdd(txtrBlock.PackagePath);
                        txtr = (Txtr)txtrPackage.GetResourceByKey(txtrBlock.OriginalKey);

                        BuildLifoLists(txtrBlock, lifoConectors, lifos);
                    }

                    if ((new TxmtDialog().ShowDialog(owningForm, mouseScreenLocation, txmtPackage, hoverBlock, txmt, guid, mmatGroup, cresSgName, subsets, txtr, lifos, out removeLifos)) == DialogResult.OK)
                    {
                        if (txtr != null && txtr.IsDirty)
                        {
                            txtrPackage.Commit(txtr);
                            txtrBlock.SetDirty();

                            UpdateLifos(removeLifos, lifoConectors, lifos);
                        }

                        if (txmt.IsDirty)
                        {
                            txmtPackage.Commit(txmt);
                            hoverBlock.SetDirty();
                        }

                        if (txmt.IsDirty || (txtr != null && txtr.IsDirty))
                        {
                            Invalidate();
                        }

                        owningForm.UpdateTexture(hoverBlock);
                    }
                }
                else if (hoverBlock.TypeId == Txtr.TYPE)
                {
                    CacheableDbpfFile txtrPackage = packageCache.GetOrAdd(hoverBlock.PackagePath);

                    Txtr txtr = (Txtr)txtrPackage.GetResourceByKey(hoverBlock.OriginalKey);
                    Trace.Assert(txtr != null, $"Double-Click: Missing resource for {hoverBlock.OriginalKey}");

                    List<GraphConnector> lifoConectors = new List<GraphConnector>();
                    List<Lifo> lifos = new List<Lifo>();

                    foreach (GraphConnector outConnector in hoverBlock.OutConnectors)
                    {
                        if (outConnector.EndBlock.TypeId == Lifo.TYPE)
                        {
                            lifoConectors.Add(outConnector);

                            GraphBlock lifoBlock = outConnector.EndBlock;
                            Lifo lifo = null;

                            CacheableDbpfFile lifoPackage = packageCache.GetOrAdd(lifoBlock.PackagePath);

                            if (lifoPackage != null)
                            {
                                lifo = (Lifo)lifoPackage.GetResourceByKey(lifoBlock.Key);
                            }

                            lifos.Add(lifo);
                        }
                    }

                    bool removeLifos = false;
                    if ((new TxtrDialog().ShowDialog(owningForm, mouseScreenLocation, txtrPackage, txtr, hoverBlock.SgBaseName, lifos, out removeLifos)) == DialogResult.OK)
                    {
                        if (txtr.IsDirty)
                        {
                            txtrPackage.Commit(txtr);
                            hoverBlock.SetDirty();

                            Invalidate();

                            owningForm.UpdateTexture(hoverBlock);
                        }

                        if (removeLifos)
                        {
                            foreach (GraphConnector lifoConnector in lifoConectors)
                            {
                                GraphBlock lifoBlock = lifoConnector.EndBlock;

                                lifoBlock.UnlinkFrom(lifoConnector);

                                if (lifoBlock.GetInConnectors().Count == 0)
                                {
                                    CacheableDbpfFile package = packageCache.GetOrOpen(lifoBlock.PackagePath);
                                    package.Remove(lifoBlock.OriginalKey);
                                    owningForm.RemoveResource(package, lifoBlock.OriginalKey);

                                    MarkBlockForDeletion(lifoBlock, false);
                                }

                                lifoConnector.StartBlock.UnconnectTo(lifoConnector);
                                lifoConnector.Discard();
                            }
                        }
                        else
                        {
                            // The LIFOs were updated, so mark the blocks as dirty
                            for (int index = 0; index < lifoConectors.Count; ++index)
                            {
                                GraphConnector lifoConnector = lifoConectors[index];
                                Lifo lifo = lifos[index];

                                if (lifo != null && lifo.IsDirty)
                                {
                                    packageCache.GetOrAdd(lifoConnector.EndBlock.PackagePath).Commit(lifo);

                                    lifoConnector.EndBlock.SetDirty();
                                }
                            }
                        }
                    }
                }
            }
            else if (hoverConnector != null)
            {
                // Display the double-click context menu for this connector
            }
            else
            {
                // Must have been over the background
            }
        }

        private void BuildLifoLists(GraphBlock txtrBlock, List<GraphConnector> lifoConectors, List<Lifo> lifos)
        {
            if (txtrBlock != null)
            {
                foreach (GraphConnector outConnector in txtrBlock.OutConnectors)
                {
                    if (outConnector.EndBlock.TypeId == Lifo.TYPE)
                    {
                        lifoConectors.Add(outConnector);

                        GraphBlock lifoBlock = outConnector.EndBlock;
                        Lifo lifo = null;

                        CacheableDbpfFile lifoPackage = packageCache.GetOrAdd(lifoBlock.PackagePath);

                        if (lifoPackage != null)
                        {
                            lifo = (Lifo)lifoPackage.GetResourceByKey(lifoBlock.Key);
                        }

                        lifos.Add(lifo);
                    }
                }
            }
        }

        private void UpdateLifos(bool removeLifos, List<GraphConnector> lifoConectors, List<Lifo> lifos)
        {
            if (removeLifos)
            {
                foreach (GraphConnector lifoConnector in lifoConectors)
                {
                    GraphBlock lifoBlock = lifoConnector.EndBlock;

                    lifoBlock.UnlinkFrom(lifoConnector);

                    if (lifoBlock.GetInConnectors().Count == 0)
                    {
                        CacheableDbpfFile package = packageCache.GetOrOpen(lifoBlock.PackagePath);
                        package.Remove(lifoBlock.OriginalKey);
                        owningForm.RemoveResource(package, lifoBlock.OriginalKey);

                        MarkBlockForDeletion(lifoBlock, false);
                    }

                    lifoConnector.StartBlock.UnconnectTo(lifoConnector);
                    lifoConnector.Discard();
                }
            }
            else
            {
                // The LIFOs were updated, so mark the blocks as dirty
                for (int index = 0; index < lifoConectors.Count; ++index)
                {
                    GraphConnector lifoConnector = lifoConectors[index];
                    Lifo lifo = lifos[index];

                    if (lifo != null && lifo.IsDirty)
                    {
                        packageCache.GetOrAdd(lifoConnector.EndBlock.PackagePath).Commit(lifo);

                        lifoConnector.EndBlock.SetDirty();
                    }
                }
            }
        }
        #endregion

        #region Block Finding (chain traversal)
        [Obsolete("Use GetGmndBlocks instead")]
        private GraphBlock GetGmndBlock(GraphBlock startBlock)
        {
            return _GetGmndBlock(startBlock);
        }

        private GraphBlock _GetGmndBlock(GraphBlock startBlock)
        {
            if (startBlock == null) return null;

            if (startBlock.TypeId == Shpe.TYPE)
            {
                GraphBlock endBlock = startBlock.OutConnectorByIndex(0)?.EndBlock;

                if (endBlock == null) return null;

                Trace.Assert(endBlock.TypeId == Gmnd.TYPE, "Expected GMND linked from SHPE at index 0");

                if (startBlock.OutConnectorByIndex(1)?.EndBlock.TypeId == Gmnd.TYPE)
                {
                    // Multiple GMNDs referenced from the SHPE, can't cope with this
                    return null;
                }

                return endBlock;
            }
            else if (startBlock.TypeId == Cres.TYPE)
            {
                GraphBlock endBlock = startBlock.OutConnectorByIndex(0)?.EndBlock;

                if (endBlock == null) return null;

                Trace.Assert(endBlock.TypeId == Shpe.TYPE, "Expected SHPE linked from CRES at index 0");

                if (startBlock.OutConnectorByIndex(1)?.EndBlock.TypeId == Shpe.TYPE)
                {
                    // Multiple SHPEs referenced from the CRES, can't cope with this
                    return null;
                }

                return _GetGmndBlock(endBlock);
            }
            else if (startBlock.TypeId == Mmat.TYPE)
            {
                return _GetGmndBlock(startBlock.OutConnectorByLabel("model")?.EndBlock);
            }
            else if (startBlock.TypeId == Str.TYPE)
            {
                GraphBlock objdBlock = startBlock.SoleParent;

                if (objdBlock != null && !objdBlock.IsMissing)
                {
                    Objd objd = (Objd)packageCache.GetOrAdd(objdBlock.PackagePath).GetResourceByKey(objdBlock.OriginalKey);

                    if (objd != null)
                    {
                        return _GetGmndBlock(startBlock.SoleParent?.OutConnectorByLabel("Model Names")?.EndBlock?.OutConnectorByIndex(objd.GetRawData(ObjdIndex.DefaultGraphic))?.EndBlock);
                    }
                }
            }

            return null;
        }

        private List<GraphBlock> GetGmndBlocks(GraphBlock objdBlock, int defaultGraphic)
        {
            return GetGmndBlocks(objdBlock.OutConnectorByLabel("Model Names")?.EndBlock?.OutConnectorByIndex(defaultGraphic)?.EndBlock);
        }

        private List<GraphBlock> GetGmndBlocks(GraphBlock startBlock)
        {
            List<GraphBlock> gmndBlocks = new List<GraphBlock>();

            if (startBlock == null) return null;

            if (startBlock.TypeId == Shpe.TYPE)
            {
                foreach (GraphConnector connector in startBlock.OutConnectors)
                {
                    GraphBlock endBlock = connector.EndBlock;

                    if (endBlock == null) return null;

                    if (endBlock.TypeId == Gmnd.TYPE)
                    {
                        gmndBlocks.Add(endBlock);
                    }
                }

                return gmndBlocks;
            }
            else if (startBlock.TypeId == Cres.TYPE)
            {
                foreach (GraphConnector connector in startBlock.OutConnectors)
                {
                    GraphBlock endBlock = connector.EndBlock;

                    if (endBlock == null) return null;

                    if (endBlock.TypeId == Shpe.TYPE)
                    {
                        gmndBlocks.AddRange(GetGmndBlocks(endBlock));
                    }
                }

                return gmndBlocks;
            }
            else if (startBlock.TypeId == Mmat.TYPE)
            {
                return GetGmndBlocks(startBlock.OutConnectorByLabel("model")?.EndBlock);
            }
            else if (startBlock.TypeId == Str.TYPE)
            {
                GraphBlock objdBlock = startBlock.SoleParent;

                if (objdBlock != null && !objdBlock.IsMissing)
                {
                    Objd objd = (Objd)packageCache.GetOrAdd(objdBlock.PackagePath).GetResourceByKey(objdBlock.OriginalKey);

                    if (objd != null)
                    {
                        return GetGmndBlocks(startBlock.SoleParent, objd.GetRawData(ObjdIndex.DefaultGraphic));
                    }
                }
            }

            return null;
        }

        private GraphBlock GetGmdcBlock(GraphBlock startBlock)
        {
            if (startBlock == null) return null;

            if (startBlock.TypeId == Gmnd.TYPE)
            {
                GraphBlock endBlock = startBlock.OutConnectorByIndex(0)?.EndBlock;

                if (endBlock == null) return null;

                Trace.Assert(endBlock.TypeId == Gmdc.TYPE, "Expected GMDC linked from GMND at index 0");

                if (startBlock.OutConnectorByIndex(1)?.EndBlock.TypeId == Gmdc.TYPE)
                {
                    // Multiple GMDCs referenced from the GMND, can't cope with this
                    return null;
                }

                return endBlock;
            }

            // TODO - SceneGraph Plus - multiple GMNDs - use GetGmndBlocks instead
            return GetGmdcBlock(GetGmndBlock(startBlock));
        }

        private GraphBlock GetShpeBlock(GraphBlock startBlock)
        {
            if (startBlock == null) return null;

            if (startBlock.TypeId == Gmnd.TYPE)
            {
                GraphBlock shpeBlock = startBlock.SoleParent;

                if (shpeBlock == null) return null;

                Trace.Assert(shpeBlock.TypeId == Shpe.TYPE, "Expected SHPE as sole parent of GMND");

                return shpeBlock;
            }

            throw new NotImplementedException($"GetShpeBlock not implemented for starting from {DBPFData.TypeName(startBlock.TypeId)}");
        }
        #endregion

        #region Surface Painting
        private void ChangeCursor()
        {
            if (IsMultiSelect)
            {
                this.Cursor = CursorArrowPlus;
            }
            else
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void ChangeCursor(Cursor cursor)
        {
            if (!(Form.ModifierKeys == Keys.Shift)) this.Cursor = cursor;
        }

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
            firstSelectedBlock?.Draw(e.Graphics, false);

            if (banding)
            {
                if (!bandingStart.Equals(bandingLast))
                {
                    Pen pen = new Pen(Color.Black, 1);

                    int x = bandingStart.X;
                    if (bandingLast.X < x) x = bandingLast.X;

                    int y = bandingStart.Y;
                    if (bandingLast.Y < y) y = bandingLast.Y;

                    e.Graphics.DrawRectangle(pen, x, y, Math.Abs(bandingLast.X - bandingStart.X), Math.Abs(bandingStart.Y - bandingLast.Y));
                }
            }

            owningForm.UpdateForm();
        }

        private void PaintBlocks(Graphics g)
        {
            foreach (GraphBlock block in allBlocks)
            {
                //                if (!selectedBlocks.Contains(block)) // if we leave this condition in, blocks don't draw in the multi-select export loop
                block.Draw(g, hideMissingBlocks);
            }

            foreach (GraphBlock block in selectedBlocks)
            {
                if (block != firstSelectedBlock)
                    block.Draw(g, hideMissingBlocks);
            }
        }

        private void PaintConnectors(Graphics g)
        {
            for (int i = 0; i < allConnectors.Count; ++i)
            {
                if (SeenIdenticalConnector(i)) continue;

                allConnectors[i].Draw(g, hideMissingBlocks, CountIdenticalConnectors(i));
            }
        }
        #endregion

        #region Block Validation and Updating (post loading of all blocks)
        public void ValidateBlocks()
        {
            // Validate any blocks with special conditions
            foreach (GraphBlock block in allBlocks)
            {
                ValidateBlock(block);
            }
        }

        private void ValidateBlock(GraphBlock block)
        {
            // "UnderstoodTypes" - when adding a new resource type, need to update this block
            if (block.TypeId == Mmat.TYPE)
            {
                List<GraphBlock> gmndBlocks = GetGmndBlocks(block);

                if (gmndBlocks.Count > 0)
                {
                    Mmat mmat = (Mmat)packageCache.GetOrAdd(block.PackagePath).GetResourceByKey(block.Key);

                    if (mmat != null)
                    {
                        block.IsSubsetMmatValid = false;

                        foreach (GraphBlock gmndBlock in gmndBlocks)
                        {
                            Gmnd gmnd = (Gmnd)packageCache.GetOrAdd(gmndBlock.PackagePath).GetResourceByKey(gmndBlock.Key);

                            if (gmnd != null)
                            {
                                if (gmnd.GetDesignModeEnabledSubsets().Contains(mmat.SubsetName))
                                {
                                    block.IsSubsetMmatValid = true;
                                    break;
                                }
                            }
                        }
                    }

                }
            }
            else if (block.TypeId == Shpe.TYPE)
            {
                GraphBlock gmdcBlock = GetGmdcBlock(block);

                if (gmdcBlock != null)
                {
                    Shpe shpe = (Shpe)packageCache.GetOrAdd(block.PackagePath).GetResourceByKey(block.Key);
                    Gmdc gmdc = (Gmdc)packageCache.GetOrAdd(gmdcBlock.PackagePath).GetResourceByKey(gmdcBlock.Key);

                    if (shpe != null && gmdc != null)
                    {
                        block.IsSubsetShpeValid = true;

                        ReadOnlyCollection<string> gmdcSubsets = gmdc.Subsets;

                        foreach (string subset in shpe.Subsets)
                        {
                            if (!gmdcSubsets.Contains(subset))
                            {
                                block.IsSubsetShpeValid = false;
                                break;
                            }
                        }
                    }
                }
            }
            else if (block.TypeId == Gmnd.TYPE)
            {
                GraphBlock gmdcBlock = GetGmdcBlock(block);

                if (gmdcBlock != null)
                {
                    Gmnd gmnd = (Gmnd)packageCache.GetOrAdd(block.PackagePath).GetResourceByKey(block.Key);
                    Gmdc gmdc = (Gmdc)packageCache.GetOrAdd(gmdcBlock.PackagePath).GetResourceByKey(gmdcBlock.Key);

                    if (gmnd != null && gmdc != null)
                    {
                        block.IsSubsetGmndDesignableValid = true;
                        block.IsSubsetGmndMeshValid = true;
                        block.IsSubsetGmndSlavedValid = true;

                        ReadOnlyCollection<string> gmdcSubsets = gmdc.Subsets;

                        foreach (string subset in gmnd.GetDesignModeEnabledSubsets())
                        {
                            if (!gmdcSubsets.Contains(subset))
                            {
                                block.IsSubsetGmndDesignableValid = false;
                                break;
                            }
                        }

                        foreach (string subset in gmnd.GetMaterialsMeshNameSubsets())
                        {
                            if (!gmdcSubsets.Contains(subset))
                            {
                                block.IsSubsetGmndMeshValid = false;
                                break;
                            }
                        }

                        foreach (string subset in gmnd.GetDesignModeSlaveSubsets())
                        {
                            if (!gmdcSubsets.Contains(subset))
                            {
                                block.IsSubsetGmndSlavedValid = false;
                                break;
                            }
                            else
                            {
                                List<string> slaveSubsets = gmnd.GetDesignModeSlaveSubsetsSubset(subset);

                                foreach (string slaveSubset in slaveSubsets)
                                {
                                    if (!string.IsNullOrWhiteSpace(slaveSubset))
                                    {
                                        if (!gmdcSubsets.Contains(slaveSubset))
                                        {
                                            block.IsSubsetGmndSlavedValid = false;
                                            break;
                                        }
                                    }
                                }

                                if (!block.IsSubsetGmndSlavedValid) break;
                            }
                        }
                    }
                }
            }
            else if (block.TypeId == Str.TYPE)
            {
                if (block.InstanceId == DBPFData.STR_SUBSETS)
                {
                    GraphBlock gmdcBlock = GetGmdcBlock(block);

                    if (gmdcBlock != null)
                    {
                        Str str = (Str)packageCache.GetOrAdd(block.PackagePath).GetResourceByKey(block.Key);
                        Gmdc gmdc = (Gmdc)packageCache.GetOrAdd(gmdcBlock.PackagePath).GetResourceByKey(gmdcBlock.Key);

                        if (str != null && gmdc != null)
                        {
                            block.IsSubsetStrValid = true;

                            ReadOnlyCollection<string> gmdcSubsets = gmdc.Subsets;

                            foreach (StrItem item in str.LanguageItems(Languages.Default))
                            {
                                string subset = item.Title;

                                if (!(string.IsNullOrWhiteSpace(subset) || gmdcSubsets.Contains(subset)))
                                {
                                    block.IsSubsetStrValid = false;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void UpdateAvailableBlocks()
        {
            foreach (GraphBlock block in allBlocks)
            {
                block.IsAvailable = owningForm.IsAvailable(block.Key);
            }

            Invalidate();
        }
        #endregion

        #region Save
        public void SaveAll(bool autoBackup, bool alwaysSetNames, bool alwaysClearNames, bool prefixNames, bool prefixLowerCase)
        {
            Dictionary<string, List<GraphBlock>> dirtyBlocksByPackage = new Dictionary<string, List<GraphBlock>>();

            // Find all the dirty blocks by package
            foreach (GraphBlock block in allBlocks)
            {
                if (block.IsMissingOrClone) continue;

                if (block.IsDirty)
                {
                    if (!dirtyBlocksByPackage.ContainsKey(block.PackagePath))
                    {
                        dirtyBlocksByPackage.Add(block.PackagePath, new List<GraphBlock>());
                    }

                    dirtyBlocksByPackage[block.PackagePath].Add(block);
                }
            }

            // For each package that has dirty blocks ...
            foreach (KeyValuePair<string, List<GraphBlock>> kvPair in dirtyBlocksByPackage)
            {
                CacheableDbpfFile package = packageCache.GetOrAdd(kvPair.Key);

                // ... firstly delete any blocks ...
                foreach (GraphBlock block in kvPair.Value)
                {
                    if (block.IsDeleteMe)
                    {
                        package.Remove(block.OriginalKey);
                        owningForm.RemoveResource(package, block.OriginalKey);
                    }
                }

                // ... secondly update the dirty blocks' outbound refs ...
                foreach (GraphBlock block in kvPair.Value)
                {
                    if (!block.IsDeleted)
                    {
                        DBPFResource res = package.GetResourceByKey(block.OriginalKey);
                        Trace.Assert(res != null, $"Refs: Missing resource for {block.OriginalKey}");

                        if (res is Str str)
                        {
                            if (res.InstanceID != DBPFData.STR_SUBSETS)
                            {
                                foreach (StrItem item in str.LanguageItems(Languages.Default))
                                {
                                    item.Title = "";
                                }
                            }
                        }

                        UpdateRefsToChildren(package, res, block, prefixLowerCase);

                        package.Commit(res);
                    }
                }

                // ... thirdly update any block that references the dirty blocks where the dirty block's name has changed ...
                foreach (GraphBlock block in kvPair.Value)
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
                foreach (GraphBlock block in kvPair.Value)
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

                        // ... fifthly perform any block specific tidy up on save ...
                        if (res is Str str)
                        {
                            str.DefLanguageOnly();
                        }

                        package.Commit(res, true); // ALWAYS commit, as we removed the resource above
                    }
                }

                // ... lastly, mark the dirty blocks as clean
                foreach (GraphBlock block in kvPair.Value)
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
                    bool exitRetryLoop;

                    do
                    {
                        MessageBoxButtons buttons = (File.Exists(package.PackagePath) && File.Exists($"{package.PackagePath}.temp")) ? MessageBoxButtons.RetryCancel : MessageBoxButtons.OK;

                        DialogResult result = MsgBox.Show($"Error trying to update {package.PackageName}, file is probably open in SimPe!", "Package Update Error!", buttons);
                        exitRetryLoop = true;

                        if (result == DialogResult.Retry)
                        {
                            exitRetryLoop = package.RetryUpdateFromTemp();
                        }
                    } while (!exitRetryLoop);
                }

                packageCache.SetClean(package);
                package.Close();
            }

            Invalidate();
        }
        #endregion

        #region Block Updating (as part of save)
        private void UpdateName(DBPFResource res, GraphBlock block, bool alwaysSetNames, bool alwaysClearNames, bool prefixNames, bool prefixLowerCase)
        {
            // "UnderstoodTypes" - when adding a new resource type, need to update this block
            if (res is Str str)
            {
                str.SetKeyName(block.BlockName);
            }
            else if (res is Objd objd)
            {
                objd.SetKeyName(block.BlockName);
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
                List<GraphConnector> inConnectors = block.GetInConnectors();
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
                    string keyName = $"{block.SgBaseName}_{DBPFData.TypeName(block.TypeId).ToLower()}";
                    if (rcol.KeyName.StartsWith("##"))
                    {
                        keyName = $"##{(prefixLowerCase ? res.GroupID.ToString().ToLower() : res.GroupID.ToString())}!{keyName}";
                    }

                    rcol.SetKeyName(keyName);

                    if (res is Cres || res is Shpe || res is Gmnd)
                    {
                        UpdateOgnName(rcol, rcol.KeyName, alwaysSetNames, alwaysClearNames, prefixNames, prefixLowerCase);
                    }
                    else if (res is Txmt txmt)
                    {
                        UpdateMaterialName(txmt, block.SgBaseName, prefixNames, prefixLowerCase);
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

        private void UpdateMaterialName(Txmt txmt, string basename, bool prefixNames, bool prefixLowerCase)
        {
            // This will cause the game to crash if the Type is SimSkin (and possibly others)
            // txmt.MaterialDefinition.FileDescription = "";

            // Some "things" (eg accessories) require the prefix on the material name, so always add it
            if (true /*prefixNames*/)
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

        private void UpdateRefsToChildren(CacheableDbpfFile package, DBPFResource res, GraphBlock block, bool prefixLowerCase)
        {
            (res as Txmt)?.MaterialDefinition.ClearFiles();

            foreach (GraphConnector connector in block.OutConnectors)
            {
                Trace.Assert(connector.StartBlock == block, "Out connector is not for this block!");

                UpdateRefToChild(package, res, connector.EndBlock, connector.Index, connector.Label, prefixLowerCase);
            }
        }

        private void UpdateRefToChild(CacheableDbpfFile package, DBPFResource res, GraphBlock endBlock, int index, string label, bool prefixLowerCase)
        {
            // "UnderstoodTypes" - when adding a new resource type, need to update this block
            if (res is Str str)
            {
                Trace.Assert(endBlock.TypeId == Cres.TYPE || endBlock.TypeId == Txmt.TYPE || endBlock.TypeId == Hls.TYPE, "Expecting CRES, TXMT or HLS for EndBlock");

                List<StrItem> items = str.LanguageItems(Languages.Default);
                Trace.Assert(index < items.Count, "Index out of range");

                if (endBlock.TypeId == Hls.TYPE)
                {
                    Trace.Assert(str.InstanceID == DBPFData.STR_SOUNDS, "HLS expected for Sounds");

                    items[index].Title = endBlock.BlockName;
                }
                else
                {
                    if (endBlock.TypeId == Cres.TYPE)
                    {
                        Trace.Assert(str.InstanceID == DBPFData.STR_MODELS, "CRES expected for Model Names");
                    }
                    else if (endBlock.TypeId == Txmt.TYPE)
                    {
                        Trace.Assert(str.InstanceID == DBPFData.STR_MATERIALS, "TXMT expected for Material Names");
                    }

                    items[index].Title = MakeSgName(endBlock.Key.GroupID, endBlock.SgBaseName, prefixLowerCase);
                }
            }
            else if (res is Objd)
            {
                // STR# resources are in the same group as the OBJD, so can't be changed
            }
            else if (res is Mmat mmat)
            {
                Trace.Assert(endBlock.TypeId == Objd.TYPE || endBlock.TypeId == Cres.TYPE || endBlock.TypeId == Txmt.TYPE, "Expecting OBJD, CRES or TXMT for EndBlock");
                if (endBlock.TypeId == Objd.TYPE)
                {
                    mmat.GetItem("objectGUID").UIntegerValue = (uint)(endBlock.GUID);
                }
                else if (endBlock.TypeId == Cres.TYPE)
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

        private void UpdateRefsFromParents(DbpfFileCache packageCache, GraphBlock block, bool prefixLowerCase)
        {
            foreach (GraphConnector connector in block.GetInConnectors())
            {
                Trace.Assert(connector.EndBlock == block, "In connector is not for this block");

                if (!connector.StartBlock.IsDeleted)
                {
                    CacheableDbpfFile package = packageCache.GetOrAdd(connector.StartBlock.PackagePath);

                    DBPFResource parentRes = package.GetResourceByKey(connector.StartBlock.OriginalKey);
                    Trace.Assert(parentRes != null, "Can't locate parent resource");

                    UpdateRefToChild(package, parentRes, connector.EndBlock, connector.Index, connector.Label, prefixLowerCase);

                    package.Commit(parentRes);
                }
            }
        }

        private string MakeSgName(TypeGroupID groupId, string name, TypeTypeID typeId, bool prefixLowerCase)
        {
            return $"{MakeSgName(groupId, name, prefixLowerCase)}_{DBPFData.TypeName(typeId).ToLower()}";
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
        #endregion

        #region Close
        public bool ClosePackage(string packagePathToClose)
        {
            foreach (GraphBlock block in allBlocks)
            {
                if (block.PackagePath.Equals(packagePathToClose))
                {
                    block.Close();
                }
            }

            Invalidate();

            return true;
        }
        #endregion
    }
}
