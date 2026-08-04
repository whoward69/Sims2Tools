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
using System.Drawing.Imaging;

namespace Sims2Tools.Controls
{
    public enum FloodFillStyle
    {
        Linear,
        Queue,
        Recursive
    };

    public delegate void FillFinishedEventHandler(object sender, FillFinishedEventArgs e);

    public class FillFinishedEventArgs : EventArgs
    {
        Exception m_exception = null;

        public FillFinishedEventArgs(Exception e)
        {
            m_exception = e;
        }

        public Exception exception
        {
            get
            {
                return m_exception;
            }
        }
    }

    public abstract class AbstractFloodFiller : object
    {
        protected Color m_fillcolorcolor = Color.Green;
        protected byte[] m_Tolerance = new byte[] { 11, 11, 11 };
        protected int m_TimeBenchmark = 0;
        protected FloodFillStyle m_FillStyle = FloodFillStyle.Linear;
        protected bool m_FillDiagonal = false;
        protected Bitmap m_Bmp = null;
        protected Point m_Pt = new Point();

        protected bool[,] PixelsChecked;
        protected Queue CheckQueue = new Queue();

        public AbstractFloodFiller()
        {
        }

        public Color FillColor
        {
            get
            {
                return m_fillcolorcolor;
            }
            set
            {
                m_fillcolorcolor = value;
            }
        }

        public byte[] Tolerance
        {
            get
            {
                return m_Tolerance;
            }
            set
            {
                m_Tolerance = value;
            }
        }

        public int TimeBenchmark
        {
            get
            {
                return m_TimeBenchmark;
            }
        }

        public bool FillDiagonal
        {
            get
            {
                return m_FillDiagonal;
            }
            set
            {
                m_FillDiagonal = value;
            }
        }

        public FloodFillStyle FillStyle
        {
            get
            {
                return m_FillStyle;
            }
            set
            {
                m_FillStyle = value;
            }
        }

        public Bitmap Bmp
        {
            get
            {
                return m_Bmp;
            }
            set
            {
                m_Bmp = value;
            }
        }

        public Point Pt
        {
            get
            {
                return m_Pt;
            }
            set
            {
                m_Pt = value;
            }
        }


        public void FloodFill()
        {
            Exception ex = null;

            try
            {
                FloodFill(m_Bmp, m_Pt);
            }
            catch (Exception e)
            {
                ex = e;
                OnFillFinished(new FillFinishedEventArgs(ex));
                return;

            }
            OnFillFinished(new FillFinishedEventArgs(ex));
        }

        //initializes the FloodFill operation
        public abstract void FloodFill(Bitmap bmp, Point pt);


        //**************
        //COLOR HELPER FUNCTIONS
        //**************
        public static byte GetR(int ARGB)
        {
            return LoByte((byte)LoWord(ARGB));

        }

        public static byte GetG(int ARGB)
        {
            return HiByte((short)LoWord(ARGB));

        }

        public static byte GetB(int ARGB)
        {
            return LoByte((byte)HiWord(ARGB));

        }

        public static byte GetA(int ARGB)
        {
            return HiByte((byte)HiWord(ARGB));

        }

        public static int RGBA(byte R, byte G, byte B, byte A)
        {
            return (int)(R + (G << 8) + (B << 16) + (A << 24));
        }

        public static int RGB(byte R, byte G, byte B)
        {
            return (int)(R + (G << 8) + (B << 16));
        }

        public static int BGRA(byte B, byte G, byte R, byte A)
        {
            return (int)(B + (G << 8) + (R << 16) + (A << 24));
        }

        public static short LoWord(int n)
        {
            return (short)(n & 0xffff);
        }

        public static short HiWord(int n)
        {
            return (short)((n >> 16) & 0xffff);
        }

        public static byte LoByte(short n)
        {
            return (byte)(n & 0xff);
        }

        public static byte HiByte(short n)
        {
            return (byte)((n >> 8) & 0xff);
        }


        //EVENTS/EVENT RAISERS
        //-------------

        //raised when a fill operation is finished
        public event FillFinishedEventHandler FillFinished;
        protected void OnFillFinished(FillFinishedEventArgs args)
        {
            if (FillFinished != null)
                FillFinished.BeginInvoke(this, args, null, null);
        }

    }

    public class FloodFiller : AbstractFloodFiller
    {

        private int m_fillcolor = 255 << 8;

        /// <summary>
        /// Default constructor - initializes all fields to default values
        /// </summary>
        public FloodFiller()
        {
        }


        ///<summary>initializes the FloodFill operation</summary>
        public override void FloodFill(Bitmap bmp, Point pt)
        {
            //int ctr=0;//timeGetTime();

            //Debug.WriteLine("*******Flood Fill******");

            //get the color's int value, and convert it from RGBA to BGRA format (as GDI+ uses BGRA)
            m_fillcolor = ColorTranslator.ToWin32(m_fillcolorcolor);
            m_fillcolor = BGRA(GetB(m_fillcolor), GetG(m_fillcolor), GetR(m_fillcolor), GetA(m_fillcolor));

            //get the bits
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            System.IntPtr Scan0 = bmpData.Scan0;

            unsafe
            {
                //resolve pointer
                byte* scan0 = (byte*)(void*)Scan0;
                //get the starting color
                //[loc += Y offset + X offset]
                int loc = CoordsToIndex(pt.X, pt.Y, bmpData.Stride);//((bmpData.Stride*(pt.Y-1))+(pt.X*4));
                int color = *((int*)(scan0 + loc));

                //create the array of bools that indicates whether each pixel
                //has been checked.  (Should be bitfield, but C# doesn't support bitfields.)
                PixelsChecked = new bool[bmpData.Width + 1, bmpData.Height + 1];

                //do the first call to the loop
                switch (m_FillStyle)
                {
                    case FloodFillStyle.Linear:
                        if (m_FillDiagonal)
                        {
                            LinearFloodFill8(scan0, pt.X, pt.Y, new Size(bmpData.Width, bmpData.Height), bmpData.Stride, (byte*)&color);
                        }
                        else
                        {
                            LinearFloodFill4(scan0, pt.X, pt.Y, new Size(bmpData.Width, bmpData.Height), bmpData.Stride, (byte*)&color);
                        }
                        break;
                    case FloodFillStyle.Queue:
                        QueueFloodFill(scan0, pt.X, pt.Y, new Size(bmpData.Width, bmpData.Height), bmpData.Stride, (byte*)&color);
                        break;
                    case FloodFillStyle.Recursive:
                        if (m_FillDiagonal)
                        {
                            RecursiveFloodFill8(scan0, pt.X, pt.Y, new Size(bmpData.Width, bmpData.Height), bmpData.Stride, (byte*)&color);
                        }
                        else
                        {
                            RecursiveFloodFill4(scan0, pt.X, pt.Y, new Size(bmpData.Width, bmpData.Height), bmpData.Stride, (byte*)&color);
                        }
                        break;
                }
            }

            bmp.UnlockBits(bmpData);

            //m_TimeBenchmark=timeGetTime()-ctr;

        }

        //***********
        //LINEAR ALGORITHM
        //***********

        unsafe void LinearFloodFill4(byte* scan0, int x, int y, Size bmpsize, int stride, byte* startcolor)
        {

            //offset the pointer to the point passed in
            int* p = (int*)(scan0 + (CoordsToIndex(x, y, stride)));


            //FIND LEFT EDGE OF COLOR AREA
            int LFillLoc = x; //the location to check/fill on the left
            int* ptr = p; //the pointer to the current location
            while (true)
            {
                ptr[0] = m_fillcolor;    //fill with the color
                PixelsChecked[LFillLoc, y] = true;
                LFillLoc--;              //de-increment counter
                ptr -= 1;                    //de-increment pointer
                if (LFillLoc <= 0 || !CheckPixel((byte*)ptr, startcolor) || (PixelsChecked[LFillLoc, y]))
                    break;               //exit loop if we're at edge of bitmap or color area

            }
            LFillLoc++;

            //FIND RIGHT EDGE OF COLOR AREA
            int RFillLoc = x; //the location to check/fill on the left
            ptr = p;
            while (true)
            {
                ptr[0] = m_fillcolor; //fill with the color
                PixelsChecked[RFillLoc, y] = true;
                RFillLoc++;          //increment counter
                ptr += 1;                //increment pointer
                if (RFillLoc >= bmpsize.Width || !CheckPixel((byte*)ptr, startcolor) || (PixelsChecked[RFillLoc, y]))
                    break;           //exit loop if we're at edge of bitmap or color area

            }
            RFillLoc--;


            //START THE LOOP UPWARDS AND DOWNWARDS			
            ptr = (int*)(scan0 + CoordsToIndex(LFillLoc, y, stride));
            for (int i = LFillLoc; i <= RFillLoc; i++)
            {
                //START LOOP UPWARDS
                //if we're not above the top of the bitmap and the pixel above this one is within the color tolerance
                if (y > 0 && CheckPixel((byte*)(scan0 + CoordsToIndex(i, y - 1, stride)), startcolor) && (!(PixelsChecked[i, y - 1])))
                    LinearFloodFill4(scan0, i, y - 1, bmpsize, stride, startcolor);
                //START LOOP DOWNWARDS
                if (y < (bmpsize.Height - 1) && CheckPixel((byte*)(scan0 + CoordsToIndex(i, y + 1, stride)), startcolor) && (!(PixelsChecked[i, y + 1])))
                    LinearFloodFill4(scan0, i, y + 1, bmpsize, stride, startcolor);
                ptr += 1;
            }

        }

        unsafe void LinearFloodFill8(byte* scan0, int x, int y, Size bmpsize, int stride, byte* startcolor)
        {

            //offset the pointer to the point passed in
            int* p = (int*)(scan0 + (CoordsToIndex(x, y, stride)));


            //FIND LEFT EDGE OF COLOR AREA
            int LFillLoc = x; //the location to check/fill on the left
            int* ptr = p; //the pointer to the current location
            while (true)
            {
                ptr[0] = m_fillcolor;    //fill with the color
                PixelsChecked[LFillLoc, y] = true;
                LFillLoc--;              //de-increment counter
                ptr -= 1;                    //de-increment pointer
                if (LFillLoc <= 0 || !CheckPixel((byte*)ptr, startcolor) || (PixelsChecked[LFillLoc, y]))
                    break;               //exit loop if we're at edge of bitmap or color area

            }
            LFillLoc++;

            //FIND RIGHT EDGE OF COLOR AREA
            int RFillLoc = x; //the location to check/fill on the left
            ptr = p;
            while (true)
            {
                ptr[0] = m_fillcolor; //fill with the color
                PixelsChecked[RFillLoc, y] = true;
                RFillLoc++;          //increment counter
                ptr += 1;                //increment pointer
                if (RFillLoc >= bmpsize.Width || !CheckPixel((byte*)ptr, startcolor) || (PixelsChecked[RFillLoc, y]))
                    break;           //exit loop if we're at edge of bitmap or color area

            }
            RFillLoc--;


            //START THE LOOP UPWARDS AND DOWNWARDS			
            ptr = (int*)(scan0 + CoordsToIndex(LFillLoc, y, stride));
            for (int i = LFillLoc; i <= RFillLoc; i++)
            {
                //START LOOP UPWARDS
                //if we're not above the top of the bitmap and the pixel above this one is within the color tolerance
                //START LOOP DOWNWARDS
                if (y > 0)
                {
                    //UP
                    if (CheckPixel((byte*)(scan0 + CoordsToIndex(i, y - 1, stride)), startcolor) && (!(PixelsChecked[i, y - 1])))
                        LinearFloodFill8(scan0, i, y - 1, bmpsize, stride, startcolor);
                    //UP-LEFT
                    if (x > 0 && CheckPixel((byte*)(scan0 + CoordsToIndex(i - 1, y - 1, stride)), startcolor) && (!(PixelsChecked[i - 1, y - 1])))
                        LinearFloodFill8(scan0, i - 1, y - 1, bmpsize, stride, startcolor);
                    //UP-RIGHT
                    if (x < (bmpsize.Width - 1) && CheckPixel((byte*)(scan0 + CoordsToIndex(i + 1, y - 1, stride)), startcolor) && (!(PixelsChecked[i + 1, y - 1])))
                        LinearFloodFill8(scan0, i + 1, y - 1, bmpsize, stride, startcolor);
                }

                if (y < (bmpsize.Height - 1))
                {
                    //DOWN
                    if (CheckPixel((byte*)(scan0 + CoordsToIndex(i, y + 1, stride)), startcolor) && (!(PixelsChecked[i, y + 1])))
                        LinearFloodFill8(scan0, i, y + 1, bmpsize, stride, startcolor);
                    //DOWN-LEFT
                    if (x > 0 && CheckPixel((byte*)(scan0 + CoordsToIndex(i - 1, y + 1, stride)), startcolor) && (!(PixelsChecked[i - 1, y + 1])))
                        LinearFloodFill8(scan0, i - 1, y + 1, bmpsize, stride, startcolor);
                    //UP-RIGHT
                    if (x < (bmpsize.Width - 1) && CheckPixel((byte*)(scan0 + CoordsToIndex(i + 1, y + 1, stride)), startcolor) && (!(PixelsChecked[i + 1, y + 1])))
                        LinearFloodFill8(scan0, i + 1, y + 1, bmpsize, stride, startcolor);

                }

                ptr += 1;
            }

        }

        //*********
        //RECURSIVE ALGORITHM
        //*********

        public unsafe void RecursiveFloodFill8(byte* scan0, int x, int y, Size bmpsize, int stride, byte* startcolor)
        {
            //don't go over the edge
            if ((x < 0) || (x >= bmpsize.Width)) return;
            if ((y < 0) || (y >= bmpsize.Height)) return;
            int* p = (int*)(scan0 + (CoordsToIndex(x, y, stride)));
            if (!(PixelsChecked[x, y]) && CheckPixel((byte*)p, startcolor))
            {
                p[0] = m_fillcolor;      //fill with the color
                PixelsChecked[x, y] = true;

                //branch in all 8 directions
                RecursiveFloodFill8(scan0, x + 1, y, bmpsize, stride, startcolor);
                RecursiveFloodFill8(scan0, x, y + 1, bmpsize, stride, startcolor);
                RecursiveFloodFill8(scan0, x - 1, y, bmpsize, stride, startcolor);
                RecursiveFloodFill8(scan0, x, y - 1, bmpsize, stride, startcolor);
                RecursiveFloodFill8(scan0, x + 1, y + 1, bmpsize, stride, startcolor);
                RecursiveFloodFill8(scan0, x - 1, y + 1, bmpsize, stride, startcolor);
                RecursiveFloodFill8(scan0, x - 1, y - 1, bmpsize, stride, startcolor);
                RecursiveFloodFill8(scan0, x + 1, y - 1, bmpsize, stride, startcolor);
            }
        }

        public unsafe void RecursiveFloodFill4(byte* scan0, int x, int y, Size bmpsize, int stride, byte* startcolor)
        {
            //don't go over the edge
            if ((x < 0) || (x >= bmpsize.Width)) return;
            if ((y < 0) || (y >= bmpsize.Height)) return;

            //calculate pointer offset
            int* p = (int*)(scan0 + (CoordsToIndex(x, y, stride)));

            //if the pixel is within the color tolerance, fill it and branch out
            if (!(PixelsChecked[x, y]) && CheckPixel((byte*)p, startcolor))
            {
                p[0] = m_fillcolor;      //fill with the color
                PixelsChecked[x, y] = true;

                RecursiveFloodFill4(scan0, x + 1, y, bmpsize, stride, startcolor);
                RecursiveFloodFill4(scan0, x, y + 1, bmpsize, stride, startcolor);
                RecursiveFloodFill4(scan0, x - 1, y, bmpsize, stride, startcolor);
                RecursiveFloodFill4(scan0, x, y - 1, bmpsize, stride, startcolor);
            }
        }

        //********
        //QUEUE ALGORITHM
        //********

        public unsafe void QueueFloodFill(byte* scan0, int x, int y, Size bmpsize, int stride, byte* startcolor)
        {
            CheckQueue = new Queue();

            if (!m_FillDiagonal)
            {
                //start the loop
                QueueFloodFill4(scan0, x, y, bmpsize, stride, startcolor);
                //call next item on queue
                while (CheckQueue.Count > 0)
                {
                    Point pt = (Point)CheckQueue.Dequeue();
                    QueueFloodFill4(scan0, pt.X, pt.Y, bmpsize, stride, startcolor);
                }
            }
            else
            {
                //start the loop
                QueueFloodFill8(scan0, x, y, bmpsize, stride, startcolor);
                //call next item on queue
                while (CheckQueue.Count > 0)
                {
                    Point pt = (Point)CheckQueue.Dequeue();
                    QueueFloodFill8(scan0, pt.X, pt.Y, bmpsize, stride, startcolor);
                }
            }
        }

        public unsafe void QueueFloodFill8(byte* scan0, int x, int y, Size bmpsize, int stride, byte* startcolor)
        {
            //don't go over the edge
            if ((x < 0) || (x >= bmpsize.Width)) return;
            if ((y < 0) || (y >= bmpsize.Height)) return;
            int* p = (int*)(scan0 + (CoordsToIndex(x, y, stride)));
            if (!(PixelsChecked[x, y]) && CheckPixel((byte*)p, startcolor))
            {
                p[0] = m_fillcolor;      //fill with the color
                PixelsChecked[x, y] = true;

                CheckQueue.Enqueue(new Point(x + 1, y));
                CheckQueue.Enqueue(new Point(x, y + 1));
                CheckQueue.Enqueue(new Point(x - 1, y));
                CheckQueue.Enqueue(new Point(x, y - 1));

                CheckQueue.Enqueue(new Point(x + 1, y + 1));
                CheckQueue.Enqueue(new Point(x - 1, y - 1));
                CheckQueue.Enqueue(new Point(x - 1, y + 1));
                CheckQueue.Enqueue(new Point(x + 1, y - 1));
            }
        }

        public unsafe void QueueFloodFill4(byte* scan0, int x, int y, Size bmpsize, int stride, byte* startcolor)
        {
            //don't go over the edge
            if ((x < 0) || (x >= bmpsize.Width)) return;
            if ((y < 0) || (y >= bmpsize.Height)) return;

            //calculate pointer offset
            int* p = (int*)(scan0 + (CoordsToIndex(x, y, stride)));

            //if the pixel is within the color tolerance, fill it and branch out
            if (!(PixelsChecked[x, y]) && CheckPixel((byte*)p, startcolor))
            {
                p[0] = m_fillcolor;      //fill with the color
                PixelsChecked[x, y] = true;

                CheckQueue.Enqueue(new Point(x + 1, y));
                CheckQueue.Enqueue(new Point(x, y + 1));
                CheckQueue.Enqueue(new Point(x - 1, y));
                CheckQueue.Enqueue(new Point(x, y - 1));
            }
        }

        //*********
        //HELPER FUNCTIONS
        //*********

        ///<summary>Sees if a pixel is within the color tolerance range.</summary>
        //px - a pointer to the pixel to check
        //startcolor - a pointer to the color of the pixel we started at
        unsafe bool CheckPixel(byte* px, byte* startcolor)
        {
            bool ret = true;
            for (byte i = 0; i < 3; i++)
                ret &= (px[i] >= (startcolor[i] - m_Tolerance[i])) && px[i] <= (startcolor[i] + m_Tolerance[i]);
            return ret;
        }

        ///<summary>Given X and Y coordinates and the bitmap's stride, returns a pointer offset</summary>
        int CoordsToIndex(int x, int y, int stride)
        {
            return (stride * y) + (x * 4);
        }
    }
}
