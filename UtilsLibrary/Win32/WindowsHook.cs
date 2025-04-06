/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2025
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System;
using System.Runtime.InteropServices;

namespace Sims2Tools.Win32
{
    public class HookEventArgs : EventArgs
    {
        public int code;
        public IntPtr wParam;
        public IntPtr lParam;

        internal HookEventArgs(int code, IntPtr wParam, IntPtr lParam)
        {
            this.code = code;
            this.wParam = wParam;
            this.lParam = lParam;
        }
    }

    public enum HookType : int
    {
        WH_JOURNALRECORD = 0,
        WH_JOURNALPLAYBACK = 1,
        WH_KEYBOARD = 2,
        WH_GETMESSAGE = 3,
        WH_CALLWNDPROC = 4,
        WH_CBT = 5,
        WH_SYSMSGFILTER = 6,
        WH_MOUSE = 7,
        WH_HARDWARE = 8,
        WH_DEBUG = 9,
        WH_SHELL = 10,
        WH_FOREGROUNDIDLE = 11,
        WH_CALLWNDPROCRET = 12,
        WH_KEYBOARD_LL = 13,
        WH_MOUSE_LL = 14
    }

    public class WindowsHook
    {
        public delegate int HookProc(int code, IntPtr wParam, IntPtr lParam);

        internal IntPtr hHook = IntPtr.Zero;
        internal HookProc filterFunc = null;
        internal HookType hookType;

        public delegate void HookEventHandler(object sender, HookEventArgs e);
        public event HookEventHandler HookInvoke;

        internal void OnHookInvoke(HookEventArgs e)
        {
            HookInvoke?.Invoke(this, e);
        }

        public WindowsHook(HookType hook)
        {
            hookType = hook;
            filterFunc = new HookProc(this.CoreHookProc);
        }

        public WindowsHook(HookType hook, HookProc func)
        {
            hookType = hook;
            filterFunc = func;
        }

        internal int CoreHookProc(int code, IntPtr wParam, IntPtr lParam)
        {
            if (code < 0)
                return CallNextHookEx(hHook, code, wParam, lParam);

            // let clients determine what to do
            HookEventArgs e = new HookEventArgs(code, wParam, lParam);
            OnHookInvoke(e);

            // yield to the next hook in the chain
            return CallNextHookEx(hHook, code, wParam, lParam);
        }

        public void Install()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            hHook = SetWindowsHookEx(hookType, filterFunc, IntPtr.Zero, AppDomain.GetCurrentThreadId());
#pragma warning restore CS0618 // Type or member is obsolete
        }

        public void Uninstall()
        {
            if (hHook != IntPtr.Zero)
            {
                UnhookWindowsHookEx(hHook);
                hHook = IntPtr.Zero;
            }
        }

        [DllImport("user32.dll")]
        internal static extern IntPtr SetWindowsHookEx(HookType code, HookProc func, IntPtr hInstance, int threadID);

        [DllImport("user32.dll")]
        internal static extern int UnhookWindowsHookEx(IntPtr hhook);

        [DllImport("user32.dll")]
        internal static extern int CallNextHookEx(IntPtr hhook, int code, IntPtr wParam, IntPtr lParam);
    }
}
