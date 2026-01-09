/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Sims2Tools.Win32
{
    public struct RECT
    {
        public int left, top, right, bottom;
    }

    public sealed class API
    {
        private API() { }

        public static Rectangle TrueScreenRect
        {
            get
            {
                // get the biggest screen area
                Rectangle rectScreen = Screen.PrimaryScreen.WorkingArea;
                int left = rectScreen.Left;
                int top = rectScreen.Top;
                int right = rectScreen.Right;
                int bottom = rectScreen.Bottom;
                foreach (Screen screen in Screen.AllScreens)
                {
                    left = Math.Min(left, screen.WorkingArea.Left);
                    right = Math.Max(right, screen.WorkingArea.Right);
                    top = Math.Min(top, screen.WorkingArea.Top);
                    bottom = Math.Max(bottom, screen.WorkingArea.Bottom);
                }
                return new Rectangle(left, top, right - left, bottom - top);
            }
        }
    }

    public sealed class USER32
    {
        private USER32() { }

        [DllImport("user32", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        internal static extern int GetWindowRect(IntPtr hWnd, ref RECT rect);

        [DllImport("user32", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        internal static extern int MoveWindow(IntPtr hWnd, int x, int y, int w, int h, int repaint);

        [DllImport("user32", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr GetActiveWindow();

        [DllImport("user32", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        internal static extern int GetClassName(IntPtr hwnd, StringBuilder lpClassName, int nMaxCount);
    }
}
