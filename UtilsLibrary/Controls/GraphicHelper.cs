/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * See https://github.com/ukushu/TextProgressBar
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Sims2Tools.Controls
{
    public class GraphicHelper
    {
        #region RoundRect Routines
        public static void DrawRoundRect(Graphics g, Pen p, Rectangle rect, int radius)
        {
            DrawRoundRect(g, p, rect.X, rect.Y, rect.Width, rect.Height, radius);
        }

        public static void FillRoundRect(Graphics g, Brush b, Rectangle rect, int radius)
        {
            FillRoundRect(g, b, rect.X, rect.Y, rect.Width, rect.Height, radius);
        }

        public static void DrawRoundRect(Graphics g, Pen p, int x, int y, int width, int height, int radius)
        {
            g.DrawPath(p, RoundRectPath(x, y, width, height, radius));
        }

        public static void FillRoundRect(Graphics g, Brush b, int x, int y, int width, int height, int radius)
        {
            g.FillPath(b, RoundRectPath(x, y, width, height, radius));
        }

        public static GraphicsPath GethRoundRectPath(Rectangle rect, int radius)
        {
            return RoundRectPath(rect.X, rect.Y, rect.Width, rect.Height, radius);
        }

        public static GraphicsPath GethRoundRectPath(int x, int y, int width, int height, int radius)
        {
            return RoundRectPath(x, y, width, height, radius);
        }

        static GraphicsPath RoundRectPath(int x, int y, int width, int height, int radius)
        {
            GraphicsPath gp = new GraphicsPath();
            if (radius > 1)
            {
                gp.AddLine(x + radius, y, x + width - radius, y);
                gp.AddArc(x + width - radius, y, radius, radius, 270, 90);
                gp.AddLine(x + width, y + radius, x + width, y + height - radius);
                gp.AddArc(x + width - radius, y + height - radius, radius, radius, 0, 90);
                gp.AddLine(x + width - radius, y + height, x + radius, y + height);
                gp.AddArc(x, y + height - radius, radius, radius, 90, 90);
                gp.AddLine(x, y + height - radius, x, y + radius);
                gp.AddArc(x, y, radius, radius, 180, 90);
                gp.CloseFigure();
            }
            else
            {
                gp.AddRectangle(new Rectangle(x, y, width, height));
            }

            return gp;
        }
        #endregion

        public static ColorMap[] CloseColors(Color cl, double tolerance, Color target)
        {
            int sub = (int)Math.Floor(0xff * tolerance);
            int minr = Math.Max(0, Math.Min(0xff, cl.R - sub));
            int maxr = Math.Max(0, Math.Min(0xff, cl.R + sub));

            int ming = Math.Max(0, Math.Min(0xff, cl.G - sub));
            int maxg = Math.Max(0, Math.Min(0xff, cl.G + sub));

            int minb = Math.Max(0, Math.Min(0xff, cl.B - sub));
            int maxb = Math.Max(0, Math.Min(0xff, cl.B + sub));

            ArrayList cmap = new ArrayList();

            for (int r = minr; r < maxr; r++)
                for (int g = ming; g < maxg; g++)
                    for (int b = minb; b < maxb; b++)
                    {
                        ColorMap c = new ColorMap();
                        c.NewColor = target;
                        c.OldColor = Color.FromArgb(r, g, b);
                        cmap.Add(c);
                    }

            ColorMap[] res = new ColorMap[cmap.Count];
            cmap.CopyTo(res);

            return res;
        }

        public static ArrayList CloseColors(Color cl, double tolerance)
        {
            int sub = (int)Math.Floor(0xff * tolerance);
            int minr = Math.Max(0, Math.Min(0xff, cl.R - sub));
            int maxr = Math.Max(0, Math.Min(0xff, cl.R + sub));

            int ming = Math.Max(0, Math.Min(0xff, cl.G - sub));
            int maxg = Math.Max(0, Math.Min(0xff, cl.G + sub));

            int minb = Math.Max(0, Math.Min(0xff, cl.B - sub));
            int maxb = Math.Max(0, Math.Min(0xff, cl.B + sub));

            ArrayList cmap = new ArrayList();

            for (int r = minr; r < maxr; r++)
                for (int g = ming; g < maxg; g++)
                    for (int b = minb; b < maxb; b++)
                    {
                        cmap.Add(Color.FromArgb(r, g, b));
                    }

            return cmap;
        }


        public static Image MakeTransparent(Image img, Color cl, bool quality)
        {
            return MakeTransparent(img, cl, 0.05f, quality);
        }

        public static Image MakeTransparent(Image img, Color cl, double tolerance, bool quality)
        {
            Bitmap bm = new Bitmap(img.Width, img.Height);

            ColorMap[] colorMap = CloseColors(cl, tolerance, Color.Transparent);

            ImageAttributes attr = new ImageAttributes();
            attr.SetRemapTable(colorMap);

            Graphics g = Graphics.FromImage(bm);
            SetGraphicsMode(g, !quality);
            Rectangle rect = new Rectangle(0, 0, img.Width, img.Height);
            g.DrawImage(img, rect, rect.Left, rect.Top, rect.Width, rect.Height, GraphicsUnit.Pixel, attr);
            g.Dispose();

            return bm;
        }
        [DllImport("gdi32")]
        public static extern int ExtFloodFill(IntPtr hDC, int x, int y, int crColor, int wFillType);

        [DllImport("gdi32")]
        static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [DllImport("gdi32")]
        static extern int DeleteObject(IntPtr hObject);

        [DllImport("gdi32")]
        static extern IntPtr CreateSolidBrush(int crColor);

        public static void FloodFill(Image img, Point pos, Color backColor, Color limitColor)
        {
            Graphics g = Graphics.FromImage(img);
            FloodFill(g, pos, backColor, limitColor);
            g.Dispose();
        }
        public static void FloodFill(Graphics g, Point pos, Color backColor, Color limitColor)
        {
            IntPtr p = g.GetHdc();
            IntPtr hb = CreateSolidBrush(ColorTranslator.ToWin32(backColor));
            SelectObject(p, hb);
            ExtFloodFill(p, pos.X, pos.Y, ColorTranslator.ToWin32(limitColor), 1);
            DeleteObject(hb);
            g.ReleaseHdc(p);
        }

        public static Image KnockoutImage(Image img, Point pos, Color fillcl)
        {
            return KnockoutImage(img, pos, fillcl, true);
        }

        public static Image KnockoutImage(Image img, Point pos, Color fillcl, bool save)
        {
            Bitmap bm = null;
            if (!save)
                bm = new Bitmap(img.Width, img.Height);
            else
                bm = new Bitmap(img.Width + 2, img.Height + 2);

            Graphics g = Graphics.FromImage(bm);
            if (save)
            {
                g.FillRectangle(new SolidBrush(((Bitmap)img).GetPixel(pos.X, pos.Y)), 0, 0, bm.Width, bm.Height);
                g.DrawImage(img, new Rectangle(1, 1, img.Width, img.Height), new Rectangle(0, 0, img.Width, img.Height), GraphicsUnit.Pixel);
            }
            else g.DrawImageUnscaled(img, 0, 0);

            g.Dispose();

            FloodFiller ff = new FloodFiller();
            ff.FillColor = fillcl;
            ff.FloodFill(bm, pos);
            ((Bitmap)img).MakeTransparent(fillcl);

            return bm;
        }

        public static Image ScaleImage(Image img, Size sz, bool quality)
        {
            return ScaleImage(img, sz.Width, sz.Height, quality);
        }

        public static Image ScaleImage(Image img, int width, int height, bool quality)
        {
            if (img == null) return img;

            Bitmap bm = new Bitmap(width, height);

            Graphics g = Graphics.FromImage(bm);
            SetGraphicsMode(g, !quality);
            g.DrawImage(img, new Rectangle(0, 0, width, height), new Rectangle(0, 0, img.Width, img.Height), GraphicsUnit.Pixel);
            g.Dispose();

            return bm;
        }

        public static Color InterpolateColors(Color src, Color dst, double percentage)
        {
            int r1 = src.R;
            int g1 = src.G;
            int b1 = src.B;
            int r2 = dst.R;
            int g2 = dst.G;
            int b2 = dst.B;
            byte r = Convert.ToByte((double)(r1 + ((r2 - r1) * percentage)));
            byte g = Convert.ToByte((double)(g1 + ((g2 - g1) * percentage)));
            byte b = Convert.ToByte((double)(b1 + ((b2 - b1) * percentage)));
            return Color.FromArgb(r, g, b);
        }

        public static void SetGraphicsMode(Graphics g, bool fast)
        {
            if (fast)
            {
                g.SmoothingMode = SmoothingMode.HighSpeed;
                g.CompositingQuality = CompositingQuality.HighSpeed;
                g.InterpolationMode = InterpolationMode.Default;
            }
            else
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            }
        }
    }
}
