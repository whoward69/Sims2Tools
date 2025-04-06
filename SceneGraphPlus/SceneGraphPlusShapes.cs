﻿/*
 * SceneGraph Plus - a utility for repairing scene graphs
 *
 * William Howard - 2020-2025
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using SceneGraphPlus.Cache;
using SceneGraphPlus.Surface;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.SceneGraph.GZPS;
using Sims2Tools.DBPF.STR;
using Sims2Tools.DBPF.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SceneGraphPlus.Shapes
{
    // See - https://stackoverflow.com/questions/38747027/how-to-drag-and-move-shapes-in-c-sharp
    public abstract class AbstractGraphShape
    {
        protected DrawingSurface surface;
        private bool _dirty = false;

        public virtual bool IsDirty => _dirty;

        public virtual void SetDirty()
        {
            _dirty = true;
        }

        public virtual void SetClean()
        {
            _dirty = false;
        }

        private bool borderVisible = false;

        public string strData; // Any old shit that needs to be stored for later use.

        protected AbstractGraphShape(DrawingSurface surface)
        {
            this.surface = surface;

            surface.Add(this);
        }

        public bool BorderVisible
        {
            get => borderVisible;
            set => borderVisible = value;
        }

        public abstract string ToolTip
        {
            get;
        }

        public void Discard()
        {
            surface.Remove(this);
        }


        public abstract Point Centre { get; set; }
        public abstract GraphicsPath GetPath();
        public abstract bool HitTest(Point p);
        public abstract void Move(Point delta);
    }

    public abstract class AbstractGraphConnector : AbstractGraphShape, IEquatable<AbstractGraphConnector>
    {
        public static readonly Color ConnectorColour = Color.FromName(Properties.Settings.Default.ConnectorColour);
        public static readonly Color ConnectorHighlightColour = Color.FromName(Properties.Settings.Default.ConnectorHighlightColour);
        public static readonly Color MultiConnectorColour = Color.FromName(Properties.Settings.Default.MultiConnectorColour);

        public static readonly int ConnectorWidth = Properties.Settings.Default.ConnectorWidth;
        public static readonly int MultiConnectorWidth = Properties.Settings.Default.MultiConnectorWidth;
        public static readonly int ConnectorDetectWidth = Properties.Settings.Default.ConnectorDetectWidth;

        private int lineWidth = ConnectorWidth;
        private Color lineColour = ConnectorColour;

        private int index;
        private string label;

        protected AbstractGraphBlock startBlock = null;
        protected AbstractGraphBlock endBlock = null;

        protected AbstractGraphConnector(DrawingSurface surface, AbstractGraphBlock startBlock, AbstractGraphBlock endBlock) : base(surface)
        {
            this.startBlock = startBlock;
            this.endBlock = endBlock;
        }

        public override Point Centre
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public int LineWidth
        {
            get => (BorderVisible ? 4 : lineWidth);
            set => lineWidth = value;
        }

        public Color LineColour
        {
            get => (BorderVisible ? ConnectorHighlightColour : lineColour);
            set => lineColour = value;
        }

        public int Index
        {
            get => index;
            set => index = value;
        }

        public string Label
        {
            get => label;
            set => label = value;
        }

        public AbstractGraphBlock StartBlock => startBlock;

        public AbstractGraphBlock EndBlock => endBlock;

        public void SetEndBlock(AbstractGraphBlock block, bool makesDirty)
        {
            endBlock?.DisconnectFrom(this);

            endBlock = block;
            endBlock.ConnectedFrom(this);

            if (makesDirty) SetDirty();
        }

        public Point StartPoint => StartBlock.BestStartPoint(EndBlock);
        public Point EndPoint => ((EndBlock == null) ? Cursor.Position : EndBlock.BestEndPoint(StartBlock));

        public abstract void Draw(Graphics g, bool hideMissingBlocks, int count);

        public bool Equals(AbstractGraphConnector that)
        {
            return (this.StartBlock.Equals(that.StartBlock) && this.EndBlock.Equals(that.EndBlock));
        }

        public override string ToolTip
        {
            get => Label ?? (StartBlock.Key.TypeID == Str.TYPE ? $"{Helper.Hex4PrefixString(Index)} ({Index})" : null);
        }

        public override string ToString()
        {
            return $"{StartBlock} -{(ToolTip != null ? $"({ToolTip})" : "")}-> {EndBlock}";
        }
    }

    public class BezierConnector : AbstractGraphConnector
    {
        // See - https://learn.microsoft.com/en-us/dotnet/api/system.drawing.graphics.drawbezier?view=net-8.0&viewFallbackFrom=dotnet-plat-ext-7.0
        public BezierConnector(DrawingSurface surface, AbstractGraphBlock startBlock, AbstractGraphBlock endBlock) : base(surface, startBlock, endBlock) { }

        public override GraphicsPath GetPath()
        {
            Point start = StartPoint;
            Point end = EndPoint;

            GraphicsPath path = new GraphicsPath();

            if (StartBlock.IsTopConnectionPoint(start))
            {
                if (EndBlock.IsBottomConnectionPoint(end))
                {
                    path.AddBezier(new Point(start.X, start.Y - 5), new Point(start.X, start.Y + (end.Y - start.Y) / 2), new Point(end.X, start.Y + (end.Y - start.Y) / 2), new Point(end.X, end.Y + 5));
                }
                else
                {
                    int halfWidth = Math.Max(1, Math.Abs(start.X - end.X) - 5);
                    int halfHeight = Math.Max(1, (start.Y - end.Y - 5));

                    if (EndBlock.IsLeftConnectionPoint(end))
                    {
                        path.AddArc(start.X, start.Y - 5 - halfHeight, 2 * halfWidth, 2 * halfHeight, 180, 90);
                    }
                    else
                    {
                        path.AddArc(start.X - 2 * halfWidth, start.Y - 5 - halfHeight, 2 * halfWidth, 2 * halfHeight, 270, 90);
                    }
                }
            }
            else if (StartBlock.IsBottomConnectionPoint(start))
            {
                if (EndBlock.IsTopConnectionPoint(end))
                {
                    path.AddBezier(new Point(start.X, start.Y + 5), new Point(start.X, start.Y + (end.Y - start.Y) / 2), new Point(end.X, start.Y + (end.Y - start.Y) / 2), new Point(end.X, end.Y - 5));
                }
                else
                {
                    int halfWidth = Math.Max(1, Math.Abs(end.X - start.X) - 5);
                    int halfHeight = Math.Max(1, (end.Y - start.Y - 5));

                    if (EndBlock.IsLeftConnectionPoint(end))
                    {
                        path.AddArc(start.X, start.Y + 5 - halfHeight, 2 * halfWidth, 2 * halfHeight, 90, 90);
                    }
                    else
                    {
                        path.AddArc(start.X - 2 * halfWidth, start.Y + 5 - halfHeight, 2 * halfWidth, 2 * halfHeight, 0, 90);
                    }
                }
            }
            else if (StartBlock.IsLeftConnectionPoint(start))
            {
                path.AddBezier(new Point(start.X - 5, start.Y), new Point(start.X + (end.X - start.X) / 2, start.Y), new Point(start.X + (end.X - start.X) / 2, end.Y), new Point(end.X + 5, end.Y));
            }
            else
            {
                path.AddBezier(new Point(start.X + 5, start.Y), new Point(start.X + (end.X - start.X) / 2, start.Y), new Point(start.X + (end.X - start.X) / 2, end.Y), new Point(end.X - 5, end.Y));
            }

            return path;
        }

        public override void Draw(Graphics g, bool hideMissingBlocks, int count)
        {
            if (StartBlock.IsDeleted || StartBlock.IsDeleteMe || EndBlock.IsDeleted || EndBlock.IsDeleteMe) return;
            if (StartBlock.IsHidden || EndBlock.IsHidden) return;

            if (hideMissingBlocks)
            {
                if (StartBlock.IsMissing || EndBlock.IsMissing) return;
            }

            Color pathColour = LineColour;
            int pathWidth = LineWidth;

            if (count > 1)
            {
                if (!BorderVisible)
                {
                    pathColour = MultiConnectorColour;
                    pathWidth = MultiConnectorWidth;
                }
            }

            using (GraphicsPath path = GetPath())
            {
                using (Pen pen = new Pen(pathColour, pathWidth))
                {
                    g.DrawPath(pen, path);
                }
            }

            Rectangle startRect;
            Point[] endPolyPoints;

            using (Brush brush = new SolidBrush(pathColour))
            {
                Point start = StartPoint;
                Point end = EndPoint;

                if (StartBlock.IsTopConnectionPoint(start))
                {
                    startRect = new Rectangle(start.X - 5, start.Y - 10, 10, 10);
                }
                else if (StartBlock.IsBottomConnectionPoint(start))
                {
                    startRect = new Rectangle(start.X - 5, start.Y, 10, 10);
                }
                else if (StartBlock.IsLeftConnectionPoint(start))
                {
                    startRect = new Rectangle(start.X - 10, start.Y - 5, 10, 10);
                }
                else
                {
                    startRect = new Rectangle(start.X, start.Y - 5, 10, 10);
                }

                g.FillEllipse(brush, startRect);

                if (EndBlock.IsTopConnectionPoint(end))
                {
                    endPolyPoints = new Point[]
                    {
                        new Point(end.X - 5, end.Y - 10),
                        new Point(end.X + 5, end.Y - 10),
                        new Point(end.X, end.Y)
                    };
                }
                else if (EndBlock.IsBottomConnectionPoint(end))
                {
                    endPolyPoints = new Point[]
                    {
                        new Point(end.X - 5, end.Y + 10),
                        new Point(end.X + 5, end.Y + 10),
                        new Point(end.X, end.Y)
                    };
                }
                else if (EndBlock.IsLeftConnectionPoint(end))
                {
                    endPolyPoints = new Point[]
                    {
                        new Point(end.X - 10, end.Y + 5),
                        new Point(end.X - 10, end.Y - 5),
                        new Point(end.X, end.Y)
                    };
                }
                else
                {
                    endPolyPoints = new Point[]
                    {
                        new Point(end.X + 10, end.Y + 5),
                        new Point(end.X + 10, end.Y - 5),
                        new Point(end.X, end.Y)
                    };
                }

                g.FillPolygon(brush, endPolyPoints);
            }
        }

        public override bool HitTest(Point p)
        {
            if (StartBlock.IsDeleted || StartBlock.IsDeleteMe || EndBlock.IsDeleted || EndBlock.IsDeleteMe) return false;
            if (StartBlock.IsHidden || EndBlock.IsHidden) return false;

            return GetPath().IsOutlineVisible(p, new Pen(Color.Black, ConnectorDetectWidth));
        }

        public override void Move(Point delta)
        {
            // Just draw to the current mouse position
        }
    }

    public abstract class AbstractGraphBlock : AbstractGraphShape, IEquatable<AbstractGraphBlock>, IComparable<AbstractGraphBlock>, IDisposable
    {
        public static readonly Color BadTgirColour = Color.FromName(Properties.Settings.Default.BadTgirColour);
        public static readonly Color MaxisBlockColour = Color.FromName(Properties.Settings.Default.MaxisBlockColour);
        public static readonly Color MissingBlockColour = Color.FromName(Properties.Settings.Default.MissingBlockColour);
        public static readonly Color CloneBlockColour = Color.FromName(Properties.Settings.Default.CloneBlockColour);

        public static readonly Color AvailableEdgeColour = Color.FromName(Properties.Settings.Default.AvailableEdgeColour);

        public static readonly int BlockWidth = Properties.Settings.Default.BlockWidth;
        public static readonly int BlockHeight = Properties.Settings.Default.BlockHeight;

        private BlockRef blockRef = null;
        private string blockName = null;

        private bool missing = false;
        private bool available = false;
        private bool editable = true; // This should be called "readonly" but hey, that's a reserved word!
        private bool editing = false;
        private bool deleteMe = false;
        private bool deleted = false;
        private bool hidden = false;
        private bool filtered = false;

        private bool fileListValid = true;
        private bool lightValid = true;

        protected AbstractGraphBlock clonedFrom = null;

        public override bool IsDirty => (IsClone ? clonedFrom.IsDirty : (base.IsDirty || deleteMe));

        public override void SetDirty()
        {
            if (IsClone)
            {
                clonedFrom.SetDirty();
            }
            else
            {
                base.SetDirty();
            }
        }

        public override void SetClean()
        {
            Trace.Assert(!IsClone, "Can't mark a clone as clean!");

            blockRef.SetClean();

            base.SetClean();

            deleteMe = false;
        }

        public bool IsEditable
        {
            get => editable;
            set => editable = value;
        }

        public bool IsDeleteMe => deleteMe;
        public bool IsDeleted => deleted;

        public void Delete()
        {
            deleteMe = true;
            deleted = true;
        }

        public bool IsHidden => hidden || filtered;

        public void Hide()
        {
            hidden = true;
        }

        public void Filter(bool value)
        {
            filtered = value;
        }

        protected BlockRef BlockRef => (IsClone ? clonedFrom.BlockRef : blockRef);

        public string BlockName
        {
            get => (IsClone ? clonedFrom.BlockName : blockName);
            set
            {
                if (IsClone)
                {
                    BlockName = value;
                }
                else
                {
                    blockName = value;
                }
            }
        }

        private string text = null;

        private Point centre;
        private Size dimensions = new Size(BlockWidth, BlockHeight);

        private Color fillColour = Color.HotPink;
        private readonly Color textColour = Color.Black;

        private readonly List<AbstractGraphConnector> outConnectors = new List<AbstractGraphConnector>(); // Connectors this is the StartBlock of
        private readonly List<AbstractGraphConnector> inConnectors = new List<AbstractGraphConnector>(); // Connectors this is the EndBlock of

        public AbstractGraphBlock SoleParent => (inConnectors.Count == 1) ? inConnectors[0].StartBlock : null;
        public AbstractGraphBlock SoleRcolParent
        {
            get
            {
                AbstractGraphBlock rcolBlock = null;

                foreach (AbstractGraphConnector connector in inConnectors)
                {
                    if (DBPFData.IsKnownRcolType(connector.StartBlock.TypeId))
                    {
                        if (rcolBlock != null) return null;

                        rcolBlock = connector.StartBlock;
                    }
                }

                return rcolBlock;
            }
        }

        protected AbstractGraphBlock(DrawingSurface surface) : base(surface)
        {
        }

        protected AbstractGraphBlock(DrawingSurface surface, BlockRef blockRef) : base(surface)
        {
            this.blockRef = blockRef;
        }

        public abstract AbstractGraphBlock MakeClone(Point delta);

        public void ReplaceBlockRef(BlockRef blockRef)
        {
            Trace.Assert(!IsClone, "Cannot replace a clone's BlockRef");

            // Use this.blockRef here and not BlockRef as we want the non-clone redirected value

            if (!(this.blockRef.Key != null && this.blockRef.Key.Equals(blockRef.Key)))
            {
                Trace.Assert((this.blockRef.Key == null && blockRef.SgFullName != null) || (this.blockRef.SgFullName == null && this.blockRef.Key != null), "Bad BlockRef replacement");
            }

            this.blockRef = blockRef;
        }

        public void Close()
        {
            missing = true;
        }

        public bool IsOriginalTgirValid => BlockRef.IsOriginalTgirValid;
        public bool IsTgirValid => BlockRef.IsTgirValid;

        public void UpdateSgName(string sgName, bool prefixLowerCase)
        {
            BlockRef.SetSgFullName(sgName, prefixLowerCase);
        }

        public void UpdateSoundName(string name)
        {
            blockRef.SetSoundKey(name);

            SetDirty();
        }

        public bool IsFileListValid
        {
            get => (IsClone ? clonedFrom.IsFileListValid : fileListValid);
            set
            {
                if (IsClone)
                {
                    clonedFrom.IsFileListValid = value;
                }
                else
                {
                    fileListValid = value;
                }
            }
        }

        public bool IsLightValid
        {
            get => (IsClone ? clonedFrom.IsLightValid : lightValid);
            set
            {
                if (IsClone)
                {
                    clonedFrom.IsLightValid = value;
                }
                else
                {
                    lightValid = value;
                }
            }
        }

        public DBPFKey OriginalKey => BlockRef.OriginalKey;
        public DBPFKey Key => BlockRef.Key;

        public string PackagePath => BlockRef.PackagePath;
        public string KeyName => BlockRef.Key?.ToString();
        public string SgOriginalName => BlockRef.SgOriginalName;
        public string SgFullName
        {
            get => BlockRef.SgFullName;
        }
        public void SetSgFullName(string value, bool prefixLowerCase)
        {
            BlockRef.SetSgFullName(value, prefixLowerCase);
            BlockRef.FixTgir();
            SetDirty();
        }

        public string SgBaseName => BlockRef.MinimiseSgName(SgFullName);

        public bool HasIssues => !IsDirty && !(IsFileListValid && IsLightValid);

        public void FixFileListIssues()
        {
            IsFileListValid = true;

            SetDirty();
        }


        public void FixTgir()
        {
            BlockRef.FixTgir();
            FixFileListIssues();
            SetDirty();

            foreach (AbstractGraphConnector inConnector in inConnectors)
            {
                inConnector.StartBlock.SetDirty();
            }
        }

        public TypeTypeID TypeId => BlockRef.TypeId;

        public bool IsMissing
        {
            get => missing;
            set => missing = value;
        }

        public bool IsMaxis
        {
            get => (BlockRef.Key.GroupID == DBPFData.GROUP_SG_MAXIS || (BlockRef.Key.TypeID == Gzps.TYPE && BlockRef.Key.GroupID == DBPFData.GROUP_GZPS_MAXIS));
        }

        public bool IsClone
        {
            get => (clonedFrom != null);
        }

        public bool IsMissingOrClone => IsMissing || IsClone;

        public bool IsAvailable
        {
            get => (IsMissing && available);
            set => available = value;
        }

        public bool IsEditing
        {
            get => editing;
            set => editing = value;
        }

        public string Text
        {
            get => $"{(IsClone ? clonedFrom.Text : text)}{(HasIssues ? "!" : "")}";
            set
            {
                if (IsClone)
                {
                    clonedFrom.Text = value;
                }
                else
                {
                    text = value;
                }
            }
        }

        public override string ToolTip
        {
            get
            {
                string name = BlockName ?? SgFullName;

                return $"{name}{(name != null ? "\r\n" : "")}{((Key != null) ? Key.ToString() : SgFullName)} {(IsMissing ? "from" : "in")} {BlockRef.PackageName}";
            }
        }

        public override Point Centre
        {
            get => centre;
            set => centre = value;
        }

        public Size Dimensions
        {
            get => dimensions;
            set => dimensions = value;
        }

        public Color FillColour
        {
            get => IsMissing ? (IsMaxis ? MaxisBlockColour : MissingBlockColour) : (IsClone ? CloneBlockColour : fillColour);
            set => fillColour = value;
        }

        public Color TextColor
        {
            get => (BlockRef.IsTgirValid ? textColour : Color.White);
        }

        public List<AbstractGraphConnector> GetInConnectors()
        {
            List<AbstractGraphConnector> connectors = new List<AbstractGraphConnector>();

            foreach (AbstractGraphConnector connector in inConnectors)
            {
                connectors.Add(connector);
            }

            return connectors;
        }

        public List<AbstractGraphConnector> OutConnectors => outConnectors;

        public List<AbstractGraphConnector> GetOutConnectors()
        {
            List<AbstractGraphConnector> connectors = new List<AbstractGraphConnector>();

            foreach (AbstractGraphConnector connector in outConnectors)
            {
                connectors.Add(connector);
            }

            return connectors;
        }

        public AbstractGraphConnector OutConnectorByLabel(string label)
        {
            foreach (AbstractGraphConnector connector in outConnectors)
            {
                if (connector.Label.Equals(label))
                {
                    return connector;
                }
            }

            return null;
        }

        public AbstractGraphConnector ConnectTo(int index, string label, AbstractGraphBlock endBlock)
        {
            AbstractGraphConnector connector = new BezierConnector(surface, this, endBlock) { Index = index, Label = label };
            outConnectors.Add(connector);

            endBlock.ConnectedFrom(connector);

            return connector;
        }

        public void ConnectedFrom(AbstractGraphConnector connector)
        {
            inConnectors.Add(connector);
        }

        public void DisconnectFrom(AbstractGraphConnector connector)
        {
            inConnectors.Remove(connector);
        }

        public Point TopConnectionPoint => new Point(Centre.X, Centre.Y - Dimensions.Height / 2);
        public Point BottomConnectionPoint => new Point(Centre.X, Centre.Y + Dimensions.Height / 2);
        public Point LeftConnectionPoint => new Point(Centre.X - Dimensions.Width / 2, Centre.Y);
        public Point RightConnectionPoint => new Point(Centre.X + Dimensions.Width / 2, Centre.Y);

        public bool IsTopConnectionPoint(Point that) => (TopConnectionPoint == that);
        public bool IsBottomConnectionPoint(Point that) => (BottomConnectionPoint == that);
        public bool IsLeftConnectionPoint(Point that) => (LeftConnectionPoint == that);
        public bool IsRightConnectionPoint(Point that) => (RightConnectionPoint == that);

        public bool IsAbove(AbstractGraphBlock that)
        {
            return DistanceAbove(that) >= 0;
        }

        public bool IsBelow(AbstractGraphBlock that)
        {
            return !IsAbove(that);
        }

        public bool IsLeft(AbstractGraphBlock that)
        {
            return DistanceLeft(that) >= 0;
        }

        public bool IsRight(AbstractGraphBlock that)
        {
            return !IsLeft(that);
        }

        public int DistanceAbove(AbstractGraphBlock that)
        {
            return (that.Centre.Y - this.Centre.Y);
        }

        public int DistanceBelow(AbstractGraphBlock that)
        {
            return (this.Centre.Y - that.Centre.Y);
        }

        public int DistanceLeft(AbstractGraphBlock that)
        {
            return (that.Centre.X - this.Centre.X);
        }

        public int DistanceRight(AbstractGraphBlock that)
        {
            return (this.Centre.X - that.Centre.X);
        }

        public Point BestStartPoint(AbstractGraphBlock that)
        {
            if (IsAbove(that))
            {
                int distAbove = DistanceAbove(that);
                int distLeft = DistanceLeft(that) - (Dimensions.Width / 2) - 10;
                int distRight = DistanceRight(that) - (Dimensions.Width / 2) - 10;

                if (distAbove > distLeft && distAbove > distRight) return BottomConnectionPoint;

                return IsLeft(that) ? RightConnectionPoint : LeftConnectionPoint;
            }
            else
            {
                int distBelow = DistanceBelow(that);
                int distLeft = DistanceLeft(that) - (Dimensions.Width / 2) - 10;
                int distRight = DistanceRight(that) - (Dimensions.Width / 2) - 10;

                if (distBelow > distLeft && distBelow > distRight) return TopConnectionPoint;

                return IsLeft(that) ? RightConnectionPoint : LeftConnectionPoint;
            }
        }

        public Point BestEndPoint(AbstractGraphBlock that)
        {
            if (IsLeft(that))
            {
                int distAbove = DistanceAbove(that) - (Dimensions.Height / 2) - 10;
                int distBelow = DistanceBelow(that) - (Dimensions.Height / 2) - 10;
                int distLeft = DistanceLeft(that);

                if (distLeft > distAbove && distLeft > distBelow) return RightConnectionPoint;

                return IsAbove(that) ? BottomConnectionPoint : TopConnectionPoint;
            }
            else
            {
                int distAbove = DistanceAbove(that) - (Dimensions.Height / 2) - 10;
                int distBelow = DistanceBelow(that) - (Dimensions.Height / 2) - 10;
                int distRight = DistanceRight(that);

                if (distRight > distAbove && distRight > distBelow) return LeftConnectionPoint;

                return IsAbove(that) ? BottomConnectionPoint : TopConnectionPoint;
            }
        }

        public override bool HitTest(Point p)
        {
            if (IsDeleted || IsDeleteMe) return false;
            if (IsHidden) return false;

            bool result = false;
            using (GraphicsPath path = GetPath())
                result = path.IsVisible(p);
            return result;
        }

        public override void Move(Point delta)
        {
            Centre = new Point(Centre.X + delta.X, Centre.Y + delta.Y);
        }

        public virtual void Draw(Graphics g, bool hideMissingBlocks)
        {
            if (IsDeleted) return;
            if (IsHidden) return;

            if (hideMissingBlocks)
            {
                if (IsMissing) return;
            }

            GraphicsPath path = GetPath();

            g.FillPath(new SolidBrush(FillColour), path);

            if (BorderVisible)
            {
                g.DrawPath(new Pen(Color.Black, 2) { DashStyle = DashStyle.Dash }, path);
            }
            else if (IsEditing)
            {
                g.DrawPath(new Pen(Color.Black, 2), path);
            }
            else if (IsAvailable)
            {
                g.DrawPath(new Pen(AvailableEdgeColour, 2), path);
            }

            Font textFont = SystemFonts.DefaultFont;

            if (IsDirty)
            {
                textFont = new Font(textFont, FontStyle.Bold);
            }
            else if (!(fileListValid && lightValid))
            {
                textFont = new Font(textFont, FontStyle.Underline);
            }

            string text = Text;

            if (text.Contains("\n"))
            {
                int pos = text.IndexOf("\n");
                string text1 = text.Substring(0, pos);
                string text2 = text.Substring(pos + 1);

                Size text1Size = TextRenderer.MeasureText(text1, textFont);
                g.DrawString(text1, textFont, new SolidBrush(TextColor), new Point(Centre.X - text1Size.Width / 2, Centre.Y - 1 - text1Size.Height));

                Size text2Size = TextRenderer.MeasureText(text2, textFont);
                g.DrawString(text2, textFont, new SolidBrush(TextColor), new Point(Centre.X - text2Size.Width / 2, Centre.Y + 1));
            }
            else
            {
                Size textSize = TextRenderer.MeasureText(text, textFont);
                g.DrawString(text, textFont, new SolidBrush(TextColor), new Point(Centre.X - textSize.Width / 2, Centre.Y - textSize.Height / 2));
            }
        }

        public bool Equals(AbstractGraphBlock that)
        {
            if (that == null) return false;

            if (this.IsClone && that.IsClone)
            {
                return this == that;
            }
            else
            {
                return !this.IsClone && !that.IsClone && this.BlockRef.Equals(that.BlockRef);
            }
        }

        public int CompareTo(AbstractGraphBlock that)
        {
            if (this.IsClone) return 1;
            if (that.IsClone) return -1;

            return this.BlockRef.CompareTo(that.BlockRef);
        }

        public override string ToString()
        {
            return Text;
        }

        public new void Discard()
        {
            while (outConnectors.Count > 0)
            {
                AbstractGraphConnector outConnector = outConnectors[0];
                outConnectors.RemoveAt(0);

                outConnector.EndBlock.DisconnectFrom(outConnector);
                outConnector.Discard();
            }

            while (inConnectors.Count > 0)
            {
                AbstractGraphConnector inConnector = inConnectors[0];
                inConnectors.RemoveAt(0);

                DisconnectFrom(inConnector);
                inConnector.Discard();
            }

            base.Discard();
        }

        public void Dispose()
        {
            while (outConnectors.Count > 0)
            {
                outConnectors.RemoveAt(0);
            }

            while (inConnectors.Count > 0)
            {
                inConnectors.RemoveAt(0);
            }

            surface = null;
        }
    }

    public class RoundedBlock : AbstractGraphBlock
    {
        private int radius = 10;

        public int Radius
        {
            get => radius;
            set => radius = value;
        }

        private RoundedBlock(DrawingSurface surface, AbstractGraphBlock clonedFrom) : base(surface)
        {
            this.clonedFrom = clonedFrom;
            this.Text = clonedFrom.Text;
        }

        public RoundedBlock(DrawingSurface surface, BlockRef blockRef) : base(surface, blockRef) { }


        public override AbstractGraphBlock MakeClone(Point delta)
        {
            AbstractGraphBlock clone = new RoundedBlock(surface, this);

            foreach (AbstractGraphConnector connector in OutConnectors)
            {
                clone.ConnectTo(connector.Index, connector.Label, connector.EndBlock);
            }

            delta.Offset(this.Centre);
            clone.Move(delta);

            return clone;
        }


        public override GraphicsPath GetPath()
        {
            Rectangle bounds = new Rectangle() { X = Centre.X - Dimensions.Width / 2, Y = Centre.Y - Dimensions.Height / 2, Width = Dimensions.Width, Height = Dimensions.Height };
            Rectangle arc = new Rectangle() { X = Centre.X - Dimensions.Width / 2, Y = Centre.Y - Dimensions.Height / 2, Width = Radius * 2, Height = Radius * 2 };
            GraphicsPath path = new GraphicsPath();

            if (Radius == 0)
            {
                path.AddRectangle(new Rectangle() { X = Centre.X - Dimensions.Width / 2, Y = Centre.Y - Dimensions.Height / 2, Width = Dimensions.Width, Height = Dimensions.Height });
            }
            else
            {
                path.AddArc(arc, 180, 90);

                arc.X = bounds.Right - Radius * 2;
                path.AddArc(arc, 270, 90);

                arc.Y = bounds.Bottom - Radius * 2;
                path.AddArc(arc, 0, 90);

                arc.X = bounds.Left;
                path.AddArc(arc, 90, 90);

                path.CloseFigure();
            }

            return path;
        }
    }
}
