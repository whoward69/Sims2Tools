/*
 * SceneGraph Plus - a utility for repairing scene graphs
 *
 * William Howard - 2020-2025
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

#region Usings
using Microsoft.WindowsAPICodePack.Dialogs;
using SceneGraphPlus.Data;
using SceneGraphPlus.Dialogs.Options;
using SceneGraphPlus.Shapes;
using Sims2Tools;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.CPF;
using Sims2Tools.DBPF.CTSS;
using Sims2Tools.DBPF.OBJD;
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

        #region Surface Dynamic Controls (context menus, dialogs, tooltip and thumbnail)
        private readonly Label lblTooltip = new Label();
        private readonly PictureBox picThumbnail = new PictureBox();

        private readonly CommonOpenFileDialog selectPathDialog;
        private readonly SaveFileDialog selectFileDialog;

        private readonly ContextMenuStrip menuContextBlock;
        private readonly ToolStripMenuItem menuItemContextTexture;
        private readonly ToolStripMenuItem menuItemContextDelete;
        private readonly ToolStripMenuItem menuItemContextDeleteChain;
        private readonly ToolStripMenuItem menuItemContextExtract;
        private readonly ToolStripMenuItem menuItemContextExport;
        private readonly ToolStripMenuItem menuItemContextHide;
        private readonly ToolStripMenuItem menuItemContextHideChain;
        private readonly ToolStripMenuItem menuItemContextFixTgir;
        private readonly ToolStripMenuItem menuItemContextFixFileList;
        private readonly ToolStripMenuItem menuItemContextFixLight;
        private readonly ToolStripMenuItem menuItemContextFixLanguages;
        private readonly ToolStripMenuItem menuItemContextCopySgName;
        private readonly ToolStripMenuItem menuItemContextClosePackage;
        private readonly ToolStripMenuItem menuItemContextOpenPackage;
        private readonly ToolStripMenuItem menuItemContextSplitBlock;

        private readonly ContextMenuStrip menuContextConnector;
        private readonly ToolStripMenuItem menuItemContextSplitMulti;
        private readonly ToolStripMenuItem menuItemContextUnlink;
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
        private GraphBlock selectedBlock = null;
        private HashSet<GraphBlock> selectedBlockChain = null;
        private GraphBlock hoverBlock = null;
        private GraphBlock editBlock = null;
        private List<GraphBlock> dropOntoBlocks = new List<GraphBlock>();
        private GraphBlock contextBlock = null;

        private GraphConnector hoverConnector = null;
        private GraphConnector dropOntoConnector = null;
        private GraphConnector contextConnector = null;
        #endregion

        #region Surface Mouse Tracking
        private bool moving;
        private Point previousPoint = Point.Empty;

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

            #region Block Context Menu Initialisation
            {
                menuContextBlock = new ContextMenuStrip();
                menuItemContextTexture = new ToolStripMenuItem();
                menuItemContextDelete = new ToolStripMenuItem();
                menuItemContextDeleteChain = new ToolStripMenuItem();
                menuItemContextHide = new ToolStripMenuItem();
                menuItemContextHideChain = new ToolStripMenuItem();
                menuItemContextExtract = new ToolStripMenuItem();
                menuItemContextExport = new ToolStripMenuItem();
                menuItemContextFixTgir = new ToolStripMenuItem();
                menuItemContextFixFileList = new ToolStripMenuItem();
                menuItemContextFixLight = new ToolStripMenuItem();
                menuItemContextFixLanguages = new ToolStripMenuItem();
                menuItemContextCopySgName = new ToolStripMenuItem();
                menuItemContextClosePackage = new ToolStripMenuItem();
                menuItemContextOpenPackage = new ToolStripMenuItem();
                menuItemContextSplitBlock = new ToolStripMenuItem();

                menuContextBlock.SuspendLayout();

                menuContextBlock.Items.AddRange(new ToolStripItem[] {
                    menuItemContextTexture,
                    menuItemContextHide, menuItemContextHideChain,
                    menuItemContextExtract, menuItemContextExport,
                    menuItemContextFixTgir, menuItemContextFixFileList, menuItemContextFixLight, menuItemContextFixLanguages,
                    menuItemContextCopySgName,
                    menuItemContextClosePackage, menuItemContextOpenPackage,
                    menuItemContextSplitBlock,
                    menuItemContextDelete, menuItemContextDeleteChain
                });
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
                menuItemContextExtract.ToolTipText = "Extract in SimPe package.xml format";
                menuItemContextExtract.Click += new EventHandler(OnContextBlockExtract);

                menuItemContextExport.Name = "menuItemContextExport";
                menuItemContextExport.Size = new Size(222, 22);
                menuItemContextExport.Text = "Export";
                menuItemContextExport.ToolTipText = "Export as a .package file";
                menuItemContextExport.Click += new EventHandler(OnContextBlockExport);

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

                menuItemContextFixLanguages.Name = "menuItemContextFixLanguages";
                menuItemContextFixLanguages.Size = new Size(222, 22);
                menuItemContextFixLanguages.Text = "Fix Languages";
                menuItemContextFixLanguages.Click += new EventHandler(OnContextBlockFixLanguages);

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
            #endregion

            #region Connector Context Menu Initialisation
            {
                menuContextConnector = new ContextMenuStrip();
                menuItemContextSplitMulti = new ToolStripMenuItem();
                menuItemContextUnlink = new ToolStripMenuItem();
                menuContextConnector.SuspendLayout();

                menuContextConnector.Items.AddRange(new ToolStripItem[] { menuItemContextSplitMulti, menuItemContextUnlink });
                menuContextConnector.Name = "menuContextConnector";
                menuContextConnector.Size = new Size(223, 48);
                menuContextConnector.Opening += new CancelEventHandler(OnContextConnectorOpening);

                menuItemContextSplitMulti.Name = "menuItemContextSplitMulti";
                menuItemContextSplitMulti.Size = new Size(222, 22);
                menuItemContextSplitMulti.Text = "Split Multi-Connector";
                menuItemContextSplitMulti.Click += new EventHandler(OnContextConnectorSplitMulti);

                menuItemContextUnlink.Name = "menuItemContextUnlink";
                menuItemContextUnlink.Size = new Size(222, 22);
                menuItemContextUnlink.Text = "Remove STR# Entry";
                menuItemContextUnlink.Click += new EventHandler(OnContextConnectorUnlink);

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
            selectedBlock = null;
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
                    editBlock = null;

                    owningForm.UpdateEditor(editBlock);
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
        private void MarkBlockForDeletion(GraphBlock block)
        {
            Trace.Assert(block.GetInConnectors().Count == 0, "Cannot delete block with in connectors");
            Trace.Assert(block.IsEditable, "Cannot delete a 'read-only' block");

            foreach (GraphConnector connector in block.GetOutConnectors())
            {
                connector.EndBlock.UnlinkFrom(connector);
                RemoveShape(connector);
            }

            block.MarkForDeletion();

            if (block.Equals(editBlock))
            {
                editBlock = null;
                owningForm.UpdateEditor(editBlock);
            }
        }

        /// <summary>
        /// Deletes an unattached chain of blocks from the bottom to the top
        /// by repeatedly calling MarkBlockForDeletion.
        /// </summary>
        private void MarkChainForDeletion(GraphBlock startBlock)
        {
            Trace.Assert(contextBlock.GetInConnectors().Count == 0, "Cannot delete chain with in connectors");
            Trace.Assert(contextBlock.IsEditable, "Cannot delete a 'read-only' block");

            foreach (GraphConnector connector in startBlock.GetOutConnectors()) // GetOutConnectors() allows us to use DisconnectFrom() below
            {
                if (connector.EndBlock.GetInConnectors().Count == 1)
                {
                    MarkChainForDeletion(connector.EndBlock);

                    connector.EndBlock.UnlinkFrom(connector);
                }
            }

            MarkBlockForDeletion(startBlock);
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

                if (startBlock.TypeId == Mmat.TYPE)
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
                editBlock = null;
                owningForm.UpdateEditor(editBlock);
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
                if (!contextBlock.IsMissingOrClone)
                {
                    menuItemContextTexture.Visible = (contextBlock.TypeId == Mmat.TYPE || contextBlock.TypeId == Txmt.TYPE || contextBlock.TypeId == Txtr.TYPE || contextBlock.TypeId == Lifo.TYPE);

                    menuItemContextDelete.Visible = advancedMode;
                    menuItemContextDelete.Enabled = (contextBlock.IsEditable && contextBlock.GetInConnectors().Count == 0 && contextBlock.OutConnectors.Count == 0);

                    menuItemContextDeleteChain.Visible = advancedMode;
                    menuItemContextDeleteChain.Enabled = false;
                    if (contextBlock.IsEditable && contextBlock.GetInConnectors().Count == 0)
                    {
                        menuItemContextDeleteChain.Enabled = true;

                        foreach (GraphBlock block in GetBlockChain(contextBlock, false, false, false))
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

                    menuItemContextExtract.Visible = menuItemContextExport.Visible = false;
                    if (contextBlock.TypeId == Cres.TYPE)
                    {
                        menuItemContextExtract.Text = "Extract Mesh";
                        menuItemContextExtract.Visible = true;

                        menuItemContextExport.Text = "Export Mesh";
                        menuItemContextExport.Visible = true;
                    }
                    else if (contextBlock.TypeId == Gzps.TYPE || contextBlock.TypeId == Mmat.TYPE)
                    {
                        menuItemContextExtract.Text = "Extract Recolour";
                        menuItemContextExtract.Visible = true;

                        menuItemContextExport.Text = "Export Recolour";
                        menuItemContextExport.Visible = true;
                    }

                    menuItemContextFixTgir.Visible = !contextBlock.IsTgirValid;
                    menuItemContextFixFileList.Visible = (contextBlock.TypeId == Txmt.TYPE) && !contextBlock.IsDirty && !contextBlock.IsFileListValid;
                    menuItemContextFixLight.Visible = (contextBlock.TypeId == Lamb.TYPE || contextBlock.TypeId == Ldir.TYPE || contextBlock.TypeId == Lpnt.TYPE || contextBlock.TypeId == Lspt.TYPE) && !contextBlock.IsDirty && !contextBlock.IsLightValid;
                    menuItemContextFixLanguages.Visible = (contextBlock.TypeId == Str.TYPE) && !contextBlock.IsDirty && !contextBlock.IsDefaultLangValid;

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

                    if (contextBlock.IsMaxis)
                    {
                        if (contextBlock.TypeId == Cres.TYPE)
                        {
                            if (GameData.GetMaxisResource(contextBlock.TypeId, contextBlock.Key) != null)
                            {
                                menuItemContextExtract.Text = "Extract Maxis Mesh";
                                menuItemContextExtract.Visible = true;
                                menuItemContextExtract.Enabled = true;

                                menuItemContextExport.Text = "Export Maxis Mesh";
                                menuItemContextExport.Visible = true;
                                menuItemContextExport.Enabled = true;

                                e.Cancel = false;
                            }
                        }
                        else if (contextBlock.TypeId == Gzps.TYPE)
                        {
                            if (GameData.GetMaxisResource(contextBlock.TypeId, contextBlock.Key) != null)
                            {
                                menuItemContextExtract.Text = "Extract Maxis Recolour";
                                menuItemContextExtract.Visible = true;
                                menuItemContextExtract.Enabled = true;

                                menuItemContextExport.Text = "Export Maxis Recolour";
                                menuItemContextExport.Visible = true;
                                menuItemContextExport.Enabled = true;

                                e.Cancel = false;
                            }
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
            Trace.Assert(contextBlock.TypeId == Mmat.TYPE || contextBlock.TypeId == Txmt.TYPE || contextBlock.TypeId == Txtr.TYPE || contextBlock.TypeId == Lifo.TYPE, "Expected MMAT, TXMT, TXTR or LIFO");

            owningForm.DisplayTexture(contextBlock);
        }

        private void OnContextBlockDelete(object sender, EventArgs e)
        {
            Trace.Assert(contextBlock.OutConnectors.Count == 0, "Cannot delete block with out connectors");
            MarkBlockForDeletion(contextBlock);

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
                editBlock = null;
                owningForm.UpdateEditor(editBlock);
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
            if (selectFileDialog.ShowDialog() == DialogResult.OK)
            {
                IExporter exporter = new Exporter();

                exporter.Open(selectFileDialog.FileName);

                if (contextBlock.IsMaxis)
                {
                    if (contextBlock.TypeId == Cres.TYPE)
                    {
                        ExtractMaxisMesh(exporter);
                    }
                    else if (contextBlock.TypeId == Gzps.TYPE)
                    {
                        ExtractMaxisRecolour(exporter);
                    }

                    exporter.Close();
                }
                else
                {
                    if (contextBlock.TypeId == Cres.TYPE)
                    {
                        ExtractCustomMesh(exporter);
                    }
                    else
                    {
                        ExtractCustomRecolour(exporter);
                    }


                    exporter.Close();

                    owningForm.AddPackage(selectFileDialog.FileName);

                    Invalidate();
                }
            }
        }

        private void OnContextBlockExtract(object sender, EventArgs e)
        {
            if (selectPathDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                IExporter extractor = new Extractor();

                extractor.Open(selectPathDialog.FileName);

                if (contextBlock.IsMaxis)
                {
                    if (contextBlock.TypeId == Cres.TYPE)
                    {
                        ExtractMaxisMesh(extractor);
                    }
                    else if (contextBlock.TypeId == Gzps.TYPE)
                    {
                        ExtractMaxisRecolour(extractor);
                    }
                }
                else
                {
                    if (contextBlock.TypeId == Cres.TYPE)
                    {
                        ExtractCustomMesh(extractor);
                    }
                    else
                    {
                        ExtractCustomRecolour(extractor);
                    }
                }

                extractor.Close();
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

        private void OnContextBlockFixLanguages(object sender, EventArgs e)
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
            owningForm.ClosePackage(contextBlock.PackagePath);
        }
        #endregion

        #region Connector Context Menu Events
        private void OnContextConnectorOpening(object sender, CancelEventArgs e)
        {
            if (contextConnector != null)
            {
                if (CountIdenticalConnectors(allConnectors.IndexOf(contextConnector)) > 1)
                {
                    menuItemContextSplitMulti.Visible = true;
                    menuItemContextUnlink.Visible = false;
                }
                else if (contextConnector.StartBlock.TypeId == Str.TYPE && (contextConnector.EndBlock.TypeId == Cres.TYPE || contextConnector.EndBlock.TypeId == Txmt.TYPE))
                {
                    menuItemContextUnlink.Visible = true;
                    menuItemContextSplitMulti.Visible = false;
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
                if (block.TypeId == Gzps.TYPE || block.TypeId == Aged.TYPE || block.TypeId == Xmol.TYPE || block.TypeId == Xtol.TYPE)
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
                editBlock = null;
                owningForm.UpdateEditor(editBlock);
            }
        }
        #endregion

        #region Extraction
        private void ExtractCustomMesh(IExporter exporter)
        {
            ExtractCustomMeshOrRecolour(exporter, true);
        }

        private void ExtractCustomRecolour(IExporter exporter)
        {
            ExtractCustomMeshOrRecolour(exporter, false);
        }

        private void ExtractCustomMeshOrRecolour(IExporter exporter, bool meshOnly)
        {
            List<GraphBlock> exportedBlocks = new List<GraphBlock>();
            bool exporting = (exporter is Exporter);

            foreach (GraphBlock block in GetBlockChain(contextBlock, meshOnly, !meshOnly, false))
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

                        if (contextBlock.TypeId == Cres.TYPE && (block.TypeId == Cres.TYPE || block.TypeId == Shpe.TYPE || block.TypeId == Gmnd.TYPE || block.TypeId == Gmdc.TYPE))
                        {
                            exporter.Extract(res);
                            if (exporting) exportedBlocks.Add(block);
                        }
                        else if (contextBlock.TypeId == Mmat.TYPE && (block.TypeId == Mmat.TYPE || block.TypeId == Txmt.TYPE || block.TypeId == Txtr.TYPE || block.TypeId == Lifo.TYPE))
                        {
                            exporter.Extract(res);
                            if (exporting) exportedBlocks.Add(block);
                        }
                        else if (contextBlock.TypeId == Gzps.TYPE && (block.TypeId == Gzps.TYPE || block.TypeId == Txmt.TYPE || block.TypeId == Txtr.TYPE || block.TypeId == Lifo.TYPE))
                        {
                            if (block.TypeId == Gzps.TYPE)
                            {
                                DBPFResource idrRes = exportPackage.GetResourceByKey(new DBPFKey(Idr.TYPE, block.Key));
                                exporter.Extract(idrRes);

                                DBPFResource binxRes = exportPackage.GetResourceByKey(new DBPFKey(Binx.TYPE, block.Key));
                                exporter.Extract(binxRes);
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

                        // TODO - SceneGraph Plus - exporting - and for GZPS et al, delete the associated BINX/3IDR resources
                        CacheableDbpfFile package = packageCache.GetOrOpen(deletableBlock.PackagePath);
                        package.Remove(deletableBlock.OriginalKey);
                        owningForm.RemoveResource(package, deletableBlock.OriginalKey);

                        exportedBlocks.Remove(deletableBlock);

                        MarkBlockForDeletion(deletableBlock);
                        deletableBlock.Discard();
                    }
                } while (exportedBlocks.Count > 0 && deletableBlock != null);
            }
        }

        private void ExtractMaxisMesh(IExporter exporter)
        {
            if (contextBlock.TypeId == Cres.TYPE)
            {
                DBPFKey cresKey = contextBlock.Key;

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

        private void ExtractMaxisRecolour(IExporter exporter)
        {
            if (contextBlock.TypeId == Gzps.TYPE)
            {
                DBPFKey gzpsKey = contextBlock.Key;

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
        #endregion

        #region Mouse Tracking
        private void MouseClickTimer_Tick(object sender, EventArgs e)
        {
            // Clear double click watcher and timer
            inDoubleClick = false;
            mouseClickTimer.Stop();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (advancedMode)
                {
                    DateTime now = DateTime.Now;

                    if (inDoubleClick)
                    {
                        inDoubleClick = false;

                        // If double click is valid, respond
                        if (doubleClickArea.Contains(e.Location) && ((now - mouseLastClick) < doubleClickMaxTime))
                        {
                            mouseClickTimer.Stop();
                            selectedBlock = null;

                            OnBlockDoubleClick(PointToScreen(e.Location));
                        }

                        return;
                    }

                    // Double click was invalid, restart 
                    mouseClickTimer.Stop(); mouseClickTimer.Start();
                    mouseLastClick = now;
                    inDoubleClick = true;
                    doubleClickArea = new Rectangle(e.Location.X - (SystemInformation.DoubleClickSize.Width / 2), e.Location.Y - (SystemInformation.DoubleClickSize.Width / 2), SystemInformation.DoubleClickSize.Width, SystemInformation.DoubleClickSize.Height);
                }

                // Left-Click to drag, Shift-Left-Click to drag and drop onto a block, Ctrl-Left-Click to drag and drop onto a connector
                for (int i = allBlocks.Count - 1; i >= 0; i--)
                {
                    if (hideMissingBlocks && allBlocks[i].IsMissing) continue;

                    if (allBlocks[i].HitTest(e.Location))
                    {
                        selectedBlock = allBlocks[i];
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

                for (int i = allBlocks.Count - 1; i >= 0; i--)
                {
                    if (hideMissingBlocks && allBlocks[i].IsMissing) continue;

                    if (allBlocks[i].HitTest(e.Location))
                    {
                        contextBlock = allBlocks[i];
                        break;
                    }
                }

                for (int i = allConnectors.Count - 1; i >= 0; i--)
                {
                    if (allConnectors[i].HitTest(e.Location))
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
                            foreach (GraphBlock dropOntoBlock in dropOntoBlocks)
                            {
                                dropOntoBlock.BorderVisible = false;

                                // Clone the list of in connectors, so we can modify the original within the next loop
                                List<GraphConnector> inConnectors = dropOntoBlock.GetInConnectors();
                                foreach (GraphConnector connector in inConnectors)
                                {
                                    connector.SetEndBlock(selectedBlock, true);
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

                            dropOntoConnector.SetEndBlock(selectedBlock, true);
                            if (selectedBlock.TypeId == Txmt.TYPE && dropOntoConnector.StartBlock.TypeId == Mmat.TYPE)
                            {
                                dropOntoConnector.StartBlock.BlockName = selectedBlock.SgFullName;
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
                                    Trace.Assert(selectedBlock.TypeId == Cres.TYPE || selectedBlock.TypeId == Txmt.TYPE, "Can only ctrl-shift-drop a CRES or TXMT block!");
                                    Trace.Assert(dropOntoBlock.TypeId == Str.TYPE, "Can only ctrl-shift-drop onto a STR!");

                                    string sgFullName = selectedBlock.SgFullName;
                                    if (sgFullName.EndsWith("_cres", StringComparison.OrdinalIgnoreCase)) sgFullName = sgFullName.Substring(0, sgFullName.Length - 5);
                                    if (sgFullName.EndsWith("_txmt", StringComparison.OrdinalIgnoreCase)) sgFullName = sgFullName.Substring(0, sgFullName.Length - 5);

                                    CacheableDbpfFile package = packageCache.GetOrAdd(dropOntoBlock.PackagePath);
                                    Str str = (Str)package.GetResourceByKey(dropOntoBlock.Key);

                                    int index = str.AppendLanguageItem(Languages.Default, new StrItem(Languages.Default, sgFullName, ""));

                                    package.Commit(str);
                                    dropOntoBlock.SetDirty();

                                    dropOntoBlock.ConnectTo(index, sgFullName, selectedBlock);
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
                            foreach (GraphBlock block in selectedBlockChain)
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

                if ((Form.ModifierKeys & Keys.Alt) == Keys.Alt)
                {
                    if (advancedMode)
                    {
                        if (selectedBlockChain == null)
                        {
                            selectedBlockChain = GetBlockChain(selectedBlock, false, false, (Form.ModifierKeys & Keys.Control) != Keys.Control);
                        }

                        foreach (GraphBlock block in selectedBlockChain)
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
                if (!advancedMode && selectedBlock.IsMissing) canDrop = false;

                if (selectedBlock.TypeId == Str.TYPE) canDrop = false;

                if (canDrop)
                {
                    if (Form.ModifierKeys == Keys.Shift)
                    {
                        List<GraphBlock> currentDropOntoBlocks = new List<GraphBlock>();

                        for (int i = allBlocks.Count - 1; i >= 0; i--)
                        {
                            GraphBlock block = allBlocks[i];

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
                    else if (Form.ModifierKeys == (Keys.Shift | Keys.Control))
                    {
                        if (selectedBlock.TypeId == Cres.TYPE || selectedBlock.TypeId == Txmt.TYPE)
                        {
                            List<GraphBlock> currentDropOntoBlocks = new List<GraphBlock>();

                            for (int i = allBlocks.Count - 1; i >= 0; i--)
                            {
                                GraphBlock block = allBlocks[i];

                                if (hideMissingBlocks && block.IsMissing) continue;

                                if (block.TypeId == Str.TYPE && block.HitTest(selectedBlock.Centre))
                                {
                                    // "UnderstoodStrings"
                                    if ((selectedBlock.TypeId == Cres.TYPE && block.InstanceId == DBPFData.STR_MODELS) ||
                                        (selectedBlock.TypeId == Txmt.TYPE && block.InstanceId == DBPFData.STR_MATERIALS))
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
            else
            {
                GraphBlock currentHoverBlock = null;

                for (int i = allBlocks.Count - 1; i >= 0; i--)
                {
                    if (hideMissingBlocks && allBlocks[i].IsMissing) continue;

                    if (allBlocks[i].HitTest(e.Location))
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
                                this.Cursor = Cursors.Hand;
                            }
                            else
                            {
                                this.Cursor = Cursors.Default;
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
                        if (advancedMode) this.Cursor = Cursors.Default;

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

                        if (connector.HitTest(e.Location))
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

            base.OnMouseMove(e);
        }
        #endregion

        #region Double Click Tracking
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

                        hoverBlock.Text = $"MMAT{(mmat.DefaultMaterial ? " (Def)" : "")}";
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
                else if (hoverBlock.TypeId == Txmt.TYPE)
                {
                    CacheableDbpfFile txmtPackage = packageCache.GetOrAdd(hoverBlock.PackagePath);

                    Txmt txmt = (Txmt)txmtPackage.GetResourceByKey(hoverBlock.OriginalKey);
                    Trace.Assert(txmt != null, $"Double-Click: Missing resource for {hoverBlock.OriginalKey}");

                    GraphBlock mmatBlock = null;
                    GraphBlock gzpsBlock = null;
                    GraphBlock shpeBlock = null;
                    GraphBlock objdBlock = null;

                    GraphConnector shpeConnector = null;

                    TypeGUID guid;
                    TypeGroupID mmatGroup;
                    string cresSgName = null;
                    List<string> subsets = new List<string>();
                    CacheableDbpfFile gzpsPackage = null;
                    Gzps gzps = null;

                    foreach (GraphConnector inConnector in hoverBlock.GetInConnectors())
                    {
                        if (inConnector.StartBlock.TypeId == Mmat.TYPE)
                        {
                            mmatBlock = inConnector.StartBlock;
                        }
                        else if (inConnector.StartBlock.TypeId == Gzps.TYPE)
                        {
                            gzpsBlock = inConnector.StartBlock;
                        }
                        else if (inConnector.StartBlock.TypeId == Shpe.TYPE)
                        {
                            shpeBlock = inConnector.StartBlock;
                            shpeConnector = inConnector;
                        }
                    }

                    if (gzpsBlock != null)
                    {
                        // TXMT -> GZPS/3IDR
                        gzpsPackage = packageCache.GetOrAdd(gzpsBlock.PackagePath);
                        gzps = (Gzps)gzpsPackage.GetResourceByKey(gzpsBlock.OriginalKey);
                        Idr idr = (Idr)packageCache.GetOrAdd(gzpsBlock.PackagePath).GetResourceByKey(new DBPFKey(Idr.TYPE, gzpsBlock.OriginalKey));

                        if (idr == null)
                        {
                            gzps = null;
                        }
                    }
                    else if (mmatBlock != null)
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

                    List<GraphConnector> lifoConectors = new List<GraphConnector>();
                    List<Lifo> lifos = new List<Lifo>();

                    if (txtrBlock != null)
                    {
                        txtrPackage = packageCache.GetOrAdd(txtrBlock.PackagePath);
                        txtr = (Txtr)txtrPackage.GetResourceByKey(txtrBlock.OriginalKey);


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

                    bool removeLifos = false;
                    if ((new TxmtDialog().ShowDialog(owningForm, mouseScreenLocation, txmtPackage, hoverBlock, txmt, guid, mmatGroup, cresSgName, subsets, gzpsPackage, gzps, txtr, lifos, out removeLifos)) == DialogResult.OK)
                    {
                        if (txtr != null && txtr.IsDirty)
                        {
                            txtrPackage.Commit(txtr);
                            txtrBlock.SetDirty();

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

                                        MarkBlockForDeletion(lifoBlock);
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

                                    MarkBlockForDeletion(lifoBlock);
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
            foreach (GraphBlock block in allBlocks)
            {
                if (block != selectedBlock) block.Draw(g, hideMissingBlocks);
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
                                    if (!gmdcSubsets.Contains(slaveSubset))
                                    {
                                        block.IsSubsetGmndSlavedValid = false;
                                        break;
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
