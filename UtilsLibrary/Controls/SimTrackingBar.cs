/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Sims2Tools.Controls
{
    public enum SimsTrackingBarStyle : uint
    {
        Simple,
        Flat,
        Increase,
        Decrease,
        Balance
    }

    [ToolboxBitmapAttribute(typeof(ProgressBar)), DefaultEvent("ChangedValue")]
    public class SimTrackingBar : UserControl
    {
        private static readonly Sims2Tools.DBPF.Logger.IDBPFLogger logger = Sims2Tools.DBPF.Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Container components = null;

        public SimTrackingBar()
        {

            SetStyle(
                ControlStyles.SupportsTransparentBackColor |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.ResizeRedraw |
                ControlStyles.DoubleBuffer
                , true);

            BackColor = Color.Transparent;

            barMinimum = 0;
            barMaximum = 100;
            barValue = 0;
            tw = 6;
            quality = true;

            usetokenbuffer = true;
            style = SimsTrackingBarStyle.Flat;
            gradStartColour = Color.White;
            gradEndColour = Color.White;
            backgroundColour = SystemColors.Window;
            borderColour = Color.FromArgb(100, Color.Black);
            unselectedColour = Color.Black;
            selectedColour = Color.YellowGreen;
            negativeBalanceColour = Color.Crimson;
            positiveBalanceColour = Color.YellowGreen;
            mGradient = LinearGradientMode.Vertical;

            InitializeComponent();

            this.MouseUp += new MouseEventHandler(this.OnMouseUp);
            this.MouseMove += new MouseEventHandler(this.OnMouseMove);
            this.MouseDown += new MouseEventHandler(this.OnMouseDown);

            this.OnResize(null);
            CompleteRedraw();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
            }

            base.Dispose(disposing);
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) ProgressBarUpdate(e);
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left) ProgressBarUpdate(e);
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) ProgressBarUpdate(e);
        }

        private void ProgressBarUpdate(MouseEventArgs e)
        {
            if (e != null)
            {
                int slider = Math.Max(0, Math.Min(SensitiveWidth, e.X));
                double fDelta = ((slider / (double)SensitiveWidth) * (Maximum - Minimum)) + Minimum;

                if (Minimum == 0 && Maximum == 10 && TokenCount == 10)
                {
                    // This is a special case for job levels
                    if (fDelta < 0.5)
                    {
                        fDelta = 0;
                    }
                    else
                    {
                        fDelta = (int)fDelta + 1;
                    }
                }

                int delta = (int)Math.Round(fDelta);

                // Probably unnecessary, but paranoia is good
                delta = Math.Max(Minimum, Math.Min(Maximum, delta));

                if (Form.ModifierKeys == Keys.Shift)
                {
                    int interval = ((Maximum - Minimum) / TokenCount);

                    if (delta < interval / 2)
                    {
                        delta = 0;
                    }
                    else
                    {
                        delta = Math.Max(Minimum, Math.Min(Maximum, ((delta / interval) + 1) * interval));
                    }
                }

                Value = delta;

                Update();
            }
        }


        #region Public Properties
        bool usetokenbuffer;
        SimsTrackingBarStyle style;
        int barValue, barMinimum, barMaximum;
        bool quality;
        Color unselectedColour, borderColour;
        Color selectedColour;
        Color negativeBalanceColour, positiveBalanceColour;
        Color backgroundColour;
        Color gradStartColour, gradEndColour;
        LinearGradientMode mGradient;


        public bool UseTokenBuffer
        {
            get { return usetokenbuffer; }
            set { usetokenbuffer = value; }
        }

        public SimsTrackingBarStyle Style
        {
            get { return style; }
            set
            {
                if (value != style)
                {
                    style = value;

                    if (style == SimsTrackingBarStyle.Simple)
                    {
                        gradEndColour = Color.Black;
                    }
                    else
                    {
                        gradEndColour = Color.White;
                    }
                    CompleteRedraw();
                    Invalidate();
                }
            }
        }

        public int Minimum
        {
            get { return barMinimum; }
            set
            {
                if (value != barMinimum)
                {
                    barMinimum = Math.Min(value, Maximum);
                    Refresh();
                    FireChangedEvent();
                }
            }
        }

        public int Maximum
        {
            get { return barMaximum; }
            set
            {
                if (value != barMaximum)
                {
                    barMaximum = Math.Max(Minimum, Math.Max(1, value));
                    Value = Math.Min(barMaximum, Value);
                    Refresh();
                    FireChangedEvent();
                }
            }
        }

        public int Value
        {
            get { return barValue; }
            set
            {
                if (value != barValue)
                {
                    barValue = Math.Max(Minimum, Math.Min(Maximum, value));
                    base.Refresh();
                    FireChangedEvent();
                }
            }
        }

        public bool Quality
        {
            get { return quality; }
            set
            {
                if (value != quality)
                {
                    quality = value;
                    Invalidate();
                }
            }
        }

        public Color UnselectedColor
        {
            get { return unselectedColour; }
            set
            {
                if (value != unselectedColour)
                {
                    unselectedColour = value;
                    this.Invalidate();
                }
            }
        }

        public Color SelectedColor
        {
            get { return selectedColour; }
            set
            {
                if (value != selectedColour)
                {
                    selectedColour = value;
                    CompleteRedraw();
                    Invalidate();
                }
            }
        }

        public Color PositiveBalanceColour
        {
            get { return positiveBalanceColour; }
            set
            {
                if (value != positiveBalanceColour)
                {
                    positiveBalanceColour = value;
                    this.Invalidate();
                }
            }
        }

        public Color NegativeBalanceColour
        {
            get { return negativeBalanceColour; }
            set
            {
                if (value != negativeBalanceColour)
                {
                    negativeBalanceColour = value;
                    this.Invalidate();
                }
            }
        }

        public Color BorderColor
        {
            get { return borderColour; }
            set
            {
                if (value != borderColour)
                {
                    borderColour = value;
                    this.Invalidate();
                }
            }
        }

        public Color ProgressBackColor
        {
            get { return backgroundColour; }
            set
            {
                if (value != backgroundColour)
                {
                    backgroundColour = value;
                    this.Invalidate();
                }
            }
        }

        public Color GradientStartColor
        {
            get { return gradStartColour; }
            set
            {
                if (value != gradStartColour)
                {
                    gradStartColour = value;
                    this.Invalidate();
                }
            }
        }

        public Color GradientEndColor
        {
            get { return gradEndColour; }
            set
            {
                if (value != gradEndColour)
                {
                    gradEndColour = value;
                    this.Invalidate();
                }
            }
        }

        public LinearGradientMode Gradient
        {
            get
            {
                return this.mGradient;
            }
            set
            {
                this.mGradient = value;
            }
        }
        #endregion

        #region Events
        public event EventHandler Changed;
        protected void FireChangedEvent()
        {
            Changed?.Invoke(this, new EventArgs());
        }
        #endregion

        #region Overrides
        public new void Invalidate()
        {
            if (DesignMode) CompleteRedraw();
            base.Invalidate();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            SetTokenCount(this.TokenCount, true);
            CompleteRedraw();

            base.OnSizeChanged(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            GraphicHelper.SetGraphicsMode(e.Graphics, true);

            if (Style == SimsTrackingBarStyle.Balance)
            {
                int halfWidth = Width / 2;

                double p = (double)Math.Abs(Value) / Maximum;
                int wd = (int)(((SensitiveWidth * p) + 1) / 2.0);

                if (Value > 0)
                {
                    Rectangle rectSelected = new Rectangle(halfWidth, 0, wd, Height);
                    Rectangle rectNeg = new Rectangle(0, 0, halfWidth, Height);
                    Rectangle rectPos = new Rectangle(halfWidth + wd, 0, halfWidth - wd, Height);

                    e.Graphics.DrawImage(cachedimg, rectNeg, rectNeg, GraphicsUnit.Pixel);
                    e.Graphics.DrawImage(cachedimgsel, rectSelected, rectSelected, GraphicsUnit.Pixel);
                    e.Graphics.DrawImage(cachedimg, rectPos, rectPos, GraphicsUnit.Pixel);
                }
                else if (Value < 0)
                {
                    Rectangle rectSelected = new Rectangle(halfWidth - wd, 0, wd, Height);
                    Rectangle rectNeg = new Rectangle(0, 0, halfWidth - wd, Height);
                    Rectangle rectPos = new Rectangle(halfWidth, 0, halfWidth, Height);

                    e.Graphics.DrawImage(cachedimg, rectNeg, rectNeg, GraphicsUnit.Pixel);
                    e.Graphics.DrawImage(cachedimgsel, rectSelected, rectSelected, GraphicsUnit.Pixel);
                    e.Graphics.DrawImage(cachedimg, rectPos, rectPos, GraphicsUnit.Pixel);
                }
                else
                {
                    Rectangle rectNeg = new Rectangle(0, 0, halfWidth, Height);
                    Rectangle rectPos = new Rectangle(halfWidth, 0, halfWidth, Height);

                    e.Graphics.DrawImage(cachedimg, rectNeg, rectNeg, GraphicsUnit.Pixel);
                    e.Graphics.DrawImage(cachedimg, rectPos, rectPos, GraphicsUnit.Pixel);
                }
            }
            else
            {
                double p = (double)(Value - Minimum) / (Maximum - Minimum);
                int wd = (int)(SensitiveWidth * p) + 1;
                if (p == 0) wd = 0;

                Rectangle rectSelected = new Rectangle(0, 0, wd, Height);
                Rectangle rectUnselected = new Rectangle(wd, 0, Width - wd, Height);

                e.Graphics.DrawImage(cachedimg, rectUnselected, rectUnselected, GraphicsUnit.Pixel);
                e.Graphics.DrawImage(cachedimgsel, rectSelected, rectSelected, GraphicsUnit.Pixel);
            }
        }

        #endregion

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.Name = "SimTrackBar";
            this.Size = new Size(150, 16);
        }
        #endregion

        #region Background Graphics
        private Bitmap cachedimgsel;
        private Bitmap cachedimg;

        public void CompleteRedraw()
        {
            if (Width <= 8) return;
            if (Height <= 8) return;
            cachedimg?.Dispose();
            cachedimgsel?.Dispose();

            try
            {
                cachedimg = new Bitmap(Width, Height);
                cachedimgsel = new Bitmap(Width, Height);
            }
            catch
            {
                cachedimg = new Bitmap(1, 1);
                cachedimgsel = new Bitmap(1, 1);
                return;
            }
            try
            {
                Graphics g = Graphics.FromImage(cachedimg);
                Graphics gsel = Graphics.FromImage(cachedimgsel);
                CompleteRedraw(g, gsel);
                g.Dispose();
                gsel.Dispose();
            }
            catch { }
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            if (needredraw && Visible) CompleteRedraw();
            base.OnVisibleChanged(e);
        }

        bool needredraw;
        void CompleteRedraw(Graphics g, Graphics gsel)
        {
            if (!Visible)
            {
                needredraw = true;
                return;
            }

            System.Diagnostics.Debug.WriteLine("Redraw " + Size + ", " + tw + ", " + tc + ", " + style);

            GraphicHelper.SetGraphicsMode(g, true);
            GraphicHelper.SetGraphicsMode(gsel, true);
            g.FillRectangle(new SolidBrush(base.BackColor), 0, 0, Width, Height);
            GraphicHelper.SetGraphicsMode(g, !quality);
            GraphicHelper.SetGraphicsMode(gsel, !quality);

            if (style == SimsTrackingBarStyle.Flat) UserDrawFlat(g, gsel);
            else if (style == SimsTrackingBarStyle.Simple) UserDrawSimple(g, gsel);
            else if (style == SimsTrackingBarStyle.Increase) UserDrawIncrease(g, gsel);
            else if (style == SimsTrackingBarStyle.Decrease) UserDrawDecrease(g, gsel);
            else if (style == SimsTrackingBarStyle.Balance) UserDrawBalance(g, gsel);
            needredraw = false;
        }

        class GraphicsId
        {
            private readonly int width, height;
            private readonly Color colour;

            public GraphicsId(int width, int height, Color colour)
            {
                this.width = width;
                this.height = height;
                this.colour = colour;
            }

            public Color Colour => colour;

            public override int GetHashCode()
            {
                return width;
            }

            public override bool Equals(object obj)
            {
                if (obj is GraphicsId other)
                {
                    return (this.width == other.width) && (this.height == other.height) && (this.colour == other.colour);
                }

                return base.Equals(obj);
            }
        }

        private static readonly Dictionary<GraphicsId, Image> tokenmap = new Dictionary<GraphicsId, Image>();
        private static readonly Dictionary<GraphicsId, Image> seltokenmap = new Dictionary<GraphicsId, Image>();

        protected void DrawTokens(Graphics g, Graphics gsel, int left, int top, int width, int height, Color colour)
        {
            if (!usetokenbuffer || Style == SimsTrackingBarStyle.Balance)
            {
                DoDrawTokens(g, gsel, left, top, width, height, colour);
                return;
            }

            GraphicsId sz = new GraphicsId(width, height, colour);
            UpdateTokenBuffer(width, height, sz);

            Image i = tokenmap[sz];
            Image si = seltokenmap[sz];

            g.DrawImageUnscaled(i, left, top);
            gsel.DrawImageUnscaled(si, left, top);
        }

        private void UpdateTokenBuffer(int width, int height, GraphicsId sz)
        {
            if (!tokenmap.ContainsKey(sz))
            {
                Bitmap b1 = new Bitmap(width + 1, height + 1);
                Graphics g1 = Graphics.FromImage(b1);
                Bitmap b2 = new Bitmap(width + 1, height + 1);
                Graphics g2 = Graphics.FromImage(b2);

                DoDrawTokens(g1, g2, 0, 0, width, height, sz.Colour);
                g1.Dispose(); g2.Dispose();

                tokenmap.Add(sz, b1);
                seltokenmap.Add(sz, b2);
            }
        }

        protected virtual void DoDrawTokens(Graphics g, Graphics gsel, int left, int top, int width, int height, Color colour)
        {
            int rad = 2;

            GraphicHelper.FillRoundRect(g, new SolidBrush(this.UnselectedColor), left, top, width, height, rad);
            GraphicHelper.FillRoundRect(gsel, new SolidBrush(colour), left, top, width, height, rad);

            //if ((this.TokenWidth>8 || this.Height>16)) 
            {
                GraphicHelper.SetGraphicsMode(g, true);
                GraphicHelper.SetGraphicsMode(gsel, true);
                LinearGradientBrush b
                    = new LinearGradientBrush(
                    new Rectangle(left, top, width, height),
                    Color.FromArgb(80, this.GradientStartColor),
                    Color.FromArgb(50, this.GradientEndColor),
                    LinearGradientMode.ForwardDiagonal
                    );

                GraphicHelper.FillRoundRect(g, b, left, top, width, height, rad);
                b.Dispose();

                CreateGlossyGradient(gsel, left, top, width, height, rad);

                GraphicHelper.SetGraphicsMode(g, !quality);
                GraphicHelper.SetGraphicsMode(gsel, !quality);
            }

            GraphicHelper.DrawRoundRect(g, new Pen(this.BorderColor), left, top, width, height, rad);
            GraphicHelper.DrawRoundRect(gsel, new Pen(this.BorderColor), left, top, width, height, rad);
        }

        protected virtual void DrawTokenline(Graphics g, Graphics gsel, int left, int top, int width, int height)
        {
            int rad = 2;
            GraphicHelper.FillRoundRect(gsel, new SolidBrush(this.SelectedColor), left + 1, top + 1, width - 1, height - 1, rad);
            CreateGlossyGradient(gsel, left, top, width, height, 3);
        }

        void CreateGlossyGradient(Graphics g, int left, int top, int width, int height, int rad)
        {
            LinearGradientBrush b
                = new LinearGradientBrush(
                new Rectangle(left, top, width, height),
                this.GradientStartColor,
                Color.Transparent,
                this.Gradient
                );

            Blend blend = new Blend
            {
                Factors = new float[] { 0.2f, 0.7f, 1f, 1f },
                Positions = new float[] { 0.0f, 0.1f, 0.5f, 1.0f }
            };

            b.Blend = blend;

            GraphicHelper.FillRoundRect(g, b, left, top + 1, width, height - 2, rad);
            b.Dispose();

            b = new LinearGradientBrush(
                new Rectangle(left, top, width, height),
                this.GradientEndColor,
                Color.Transparent,
                this.Gradient
                );

            //Create a Blend object and assign it to linGrBrush.
            blend = new Blend
            {
                Factors = new float[] { 1f, 1f, 0.7f, 0.5f },
                Positions = new float[] { 0.0f, 0.5f, 0.7f, 1.0f }
            };

            b.Blend = blend;

            GraphicHelper.FillRoundRect(g, b, left, top + 1, width, height - 1, rad);
            b.Dispose();
        }

        protected virtual void UserDrawBackground(Graphics g, Graphics gsel)
        {
            GraphicHelper.FillRoundRect(gsel, new SolidBrush(this.ProgressBackColor), 0, 0, Width - 2, Height - 1, 3);
            GraphicHelper.FillRoundRect(gsel, new SolidBrush(Color.FromArgb(150, this.BorderColor)), 0, 0, Width - 2, Height - 1, 3);
            GraphicHelper.FillRoundRect(gsel, new SolidBrush(this.ProgressBackColor), 1, 1, Width - 3, Height - 2, 3);

            GraphicHelper.DrawRoundRect(gsel, new Pen(Color.FromArgb(200, this.BorderColor)), 0, 0, Width - 2, Height - 1, 3);

            g.DrawImageUnscaled(this.cachedimgsel, 0, 0);
        }
        #endregion

        #region Styles

        protected virtual void UserDrawSimple(Graphics g, Graphics gsel)
        {
            UserDrawBackground(g, gsel);

            DrawTokenline(g, gsel, 2, 2, Width - 7, Height - 5);
        }

        protected virtual void UserDrawFlat(Graphics g, Graphics gsel)
        {
            for (int i = 0; i < TokenCount; i++)
            {
                int left = TokenOffset(i);

                DrawTokens(g, gsel, left, 0, TokenWidth, Height - 1, SelectedColor);
            }
        }

        protected virtual void UserDrawIncrease(Graphics g, Graphics gsel)
        {
            double minhg = (Height - 1) / 4.0;
            double step = ((Height - 1) - minhg) / (TokenCount - 1);
            for (int i = 0; i < TokenCount; i++)
            {
                int left = TokenOffset(i);
                int height = (int)Math.Floor(minhg + i * step);
                int top = (Height - 1) - height;

                DrawTokens(g, gsel, left, top, TokenWidth, height, SelectedColor);
            }
        }

        protected virtual void UserDrawDecrease(Graphics g, Graphics gsel)
        {
            double minhg = (Height - 1) / 4.0;
            double step = ((Height - 1) - minhg) / (this.TokenCount - 1);
            for (int i = 0; i < TokenCount; i++)
            {
                int left = TokenOffset(i);
                int height = (int)Math.Floor(minhg + (TokenCount - 1 - i) * step);
                int top = (Height - 1) - height;

                DrawTokens(g, gsel, left, top, TokenWidth, height, SelectedColor);
            }
        }

        protected virtual void UserDrawBalance(Graphics g, Graphics gsel)
        {
            double minhg = (Height - 1) / 4.0;
            int mid = (TokenCount / 2);
            double step = ((Height - 1) - minhg) / mid;

            for (int i = 0; i < TokenCount; i++)
            {
                Color colour;

                int left = TokenOffset(i);
                int height;
                if (i >= mid)
                {
                    colour = PositiveBalanceColour;
                    height = (int)Math.Floor(minhg + (i + 1 - mid) * step);
                }
                else
                {
                    colour = NegativeBalanceColour;
                    height = (int)Math.Floor(minhg + (mid - i) * step);
                }
                int top = (Height - 1) - height;

                DrawTokens(g, gsel, left, top, TokenWidth, height, colour);
            }
        }


        #endregion

        #region Properties
        public int SensitiveWidth
        {
            get
            {
                if (Style == SimsTrackingBarStyle.Simple) return Width;

                return TokenOffset(this.TokenCount - 1) + TokenWidth;
            }
        }
        public int TokenOffset(int nr)
        {
            return (int)Math.Floor(nr * (TokenWidth + Math.Floor(TokenMinSpacing)));
        }

        int tw;
        void SetTokenWidth(int val)
        {
            if (tw == val) return;
            tw = Math.Max(4, val);
            tc = Math.Max(2, (int)Math.Floor((double)((Width - 1) / (tw + 2))));
            CompleteRedraw();
            Invalidate();
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int TokenWidth
        {
            get { return tw; }
        }

        int tc;
        void SetTokenCount(int val, bool force)
        {
            if (tc == val && !force) return;
            tc = Math.Max(2, val);

            tw = Math.Max(4, ((Width - 1) / tc) - 2);
            CompleteRedraw();
            Invalidate();
        }
        public virtual int TokenCount
        {
            get
            {
                if (style == SimsTrackingBarStyle.Balance && (tc % 2) == 1) return tc + 1;
                return tc;
            }
            set
            {
                SetTokenCount(value, false);
            }
        }

        public double TokenMinSpacing
        {
            get { return ((Width - 1) - (TokenCount * TokenWidth)) / ((double)TokenCount - 1); }
        }
        #endregion
    }
}
