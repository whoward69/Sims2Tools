/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2025
 *
 * See https://github.com/ukushu/TextProgressBar
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Sims2Tools.Controls
{
    public enum ProgressBarDisplayMode
    {
        NoText,
        Percentage,
        CurrProgress,
        CustomText,
        TextAndPercentage,
        TextAndCurrProgress
    }

    public partial class TextProgressBar : ProgressBar
    {
        public TextProgressBar()
        {
            InitializeComponent();

            Value = Minimum;
            FixComponentBlinking();
        }


        [Description("Font of the text on ProgressBar"), Category("Additional Options")]
        public Font TextFont { get; set; } = new Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);

        private SolidBrush _textColourBrush = (SolidBrush)Brushes.Black;
        [Category("Additional Options")]
        public Color TextColor
        {
            get => _textColourBrush.Color;
            set
            {
                _textColourBrush.Dispose();
                _textColourBrush = new SolidBrush(value);
            }
        }

        private SolidBrush _progressColourBrush = (SolidBrush)Brushes.LightGreen;
        [Category("Additional Options"), Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public Color ProgressColor
        {
            get => _progressColourBrush.Color;
            set
            {
                _progressColourBrush.Dispose();
                _progressColourBrush = new SolidBrush(value);
            }
        }

        private ProgressBarDisplayMode _visualMode = ProgressBarDisplayMode.CurrProgress;
        [Category("Additional Options"), Browsable(true)]
        public ProgressBarDisplayMode VisualMode
        {
            get => _visualMode;
            set => _visualMode = value;
        }

        private string _text = string.Empty;

        [Description("If it's empty, % will be shown"), Category("Additional Options"), Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public string CustomText
        {
            get => _text;
            set => _text = value;
        }

        private string TextToDraw
        {
            get
            {
                string text = CustomText;

                switch (VisualMode)
                {
                    case (ProgressBarDisplayMode.Percentage):
                        text = PercentageStr;
                        break;
                    case (ProgressBarDisplayMode.CurrProgress):
                        text = CurrProgressStr;
                        break;
                    case (ProgressBarDisplayMode.TextAndCurrProgress):
                        text = $"{CustomText}: {CurrProgressStr}";
                        break;
                    case (ProgressBarDisplayMode.TextAndPercentage):
                        text = $"{CustomText}: {PercentageStr}";
                        break;
                }

                return text;
            }
        }

        private string PercentageStr
        {
            get => $"{(int)((float)Value - Minimum) / ((float)Maximum - Minimum) * 100} %";
        }

        private string CurrProgressStr
        {
            get => $"{Value}/{Maximum}";
        }

        private void FixComponentBlinking()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            DrawProgressBar(g);

            DrawStringIfNeeded(g);
        }

        private void DrawProgressBar(Graphics g)
        {
            Rectangle rect = ClientRectangle;

            ProgressBarRenderer.DrawHorizontalBar(g, rect);

            rect.Inflate(-3, -3);

            if (Value > 0)
            {
                Rectangle clip = new Rectangle(rect.X, rect.Y, (int)Math.Round(((float)Value / Maximum) * rect.Width), rect.Height);

                g.FillRectangle(_progressColourBrush, clip);
            }
        }

        private void DrawStringIfNeeded(Graphics g)
        {
            if (VisualMode != ProgressBarDisplayMode.NoText)
            {

                string text = TextToDraw;

                SizeF len = g.MeasureString(text, TextFont);

                Point location = new Point(((Width / 2) - (int)len.Width / 2), ((Height / 2) - (int)len.Height / 2));

                g.DrawString(text, TextFont, _textColourBrush, location);
            }
        }

        public new void Dispose()
        {
            _textColourBrush.Dispose();
            _progressColourBrush.Dispose();
            base.Dispose();
        }
    }
}
