/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2021
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System;
using System.Text;

namespace Sims2Tools.Win32
{
    internal enum CbtHookAction : int
    {
        HCBT_MOVESIZE = 0,
        HCBT_MINMAX = 1,
        HCBT_QS = 2,
        HCBT_CREATEWND = 3,
        HCBT_DESTROYWND = 4,
        HCBT_ACTIVATE = 5,
        HCBT_CLICKSKIPPED = 6,
        HCBT_KEYSKIPPED = 7,
        HCBT_SYSCOMMAND = 8,
        HCBT_SETFOCUS = 9
    }

    public class CbtEventArgs : EventArgs
    {
        public IntPtr wParam;
        public IntPtr lParam;
        public string className;
        public bool IsDialog;

        internal CbtEventArgs(IntPtr wParam, IntPtr lParam)
        {
            // cache the parameters
            this.wParam = wParam;
            this.lParam = lParam;

            // cache the window's class name
            StringBuilder sb = new StringBuilder
            {
                Capacity = 256
            };
            USER32.GetClassName(wParam, sb, 256);
            className = sb.ToString();
            IsDialog = (className == "#32770");
        }
    }

    public class CbtHook : WindowsHook
    {
        public delegate void CbtEventHandler(object sender, CbtEventArgs e);
        public event CbtEventHandler WindowCreate;
        public event CbtEventHandler WindowDestroye;
        public event CbtEventHandler WindowActivate;

        public CbtHook() : base(HookType.WH_CBT)
        {
            this.HookInvoke += new HookEventHandler(CbtHookInvoked);
        }

        public CbtHook(HookProc func) : base(HookType.WH_CBT, func)
        {
            this.HookInvoke += new HookEventHandler(CbtHookInvoked);
        }

        private void CbtHookInvoked(object sender, HookEventArgs e)
        {
            // handle hook events (only a few of available actions)
            switch ((CbtHookAction)e.code)
            {
                case CbtHookAction.HCBT_CREATEWND:
                    HandleCreateWndEvent(e.wParam, e.lParam);
                    break;
                case CbtHookAction.HCBT_DESTROYWND:
                    HandleDestroyWndEvent(e.wParam, e.lParam);
                    break;
                case CbtHookAction.HCBT_ACTIVATE:
                    HandleActivateEvent(e.wParam, e.lParam);
                    break;
            }
            return;
        }

        private void HandleCreateWndEvent(IntPtr wParam, IntPtr lParam)
        {
            if (WindowCreate != null)
            {
                CbtEventArgs e = new CbtEventArgs(wParam, lParam);
                WindowCreate(this, e);
            }
        }

        private void HandleDestroyWndEvent(IntPtr wParam, IntPtr lParam)
        {
            if (WindowDestroye != null)
            {
                CbtEventArgs e = new CbtEventArgs(wParam, lParam);
                WindowDestroye(this, e);
            }
        }

        private void HandleActivateEvent(IntPtr wParam, IntPtr lParam)
        {
            if (WindowActivate != null)
            {
                CbtEventArgs e = new CbtEventArgs(wParam, lParam);
                WindowActivate(this, e);
            }
        }
    }
}
