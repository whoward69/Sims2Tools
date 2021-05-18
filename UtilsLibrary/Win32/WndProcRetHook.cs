using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Sims2Tools.Win32
{
	public enum WndMessage : int
	{
		WM_INITDIALOG = 0x0110,
		WM_UNKNOWINIT = 0x0127
	}

	public class WndProcRetEventArgs : EventArgs
	{
		public IntPtr wParam;
		public IntPtr lParam;
		public CwPRetStruct cw;

		internal WndProcRetEventArgs(IntPtr wParam, IntPtr lParam)
		{
			this.wParam = wParam;
			this.lParam = lParam;
			cw = new CwPRetStruct();
			Marshal.PtrToStructure(lParam, cw);
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public class CwPRetStruct
	{
		public int lResult;
		public int lParam;
		public int wParam;
		public WndMessage message;
		public IntPtr hwnd;
	}

	public class WndProcRetHook : WindowsHook
	{
		public delegate void WndProcEventHandler(object sender, WndProcRetEventArgs e);

		private IntPtr hWndHooked;

		public event WndProcEventHandler WndProcRet;

		public WndProcRetHook(IntPtr hWndHooked) : base(HookType.WH_CALLWNDPROCRET)
		{
			this.hWndHooked = hWndHooked;
			this.HookInvoke += new HookEventHandler(WndProcRetHookInvoked);
		}

		public WndProcRetHook(IntPtr hWndHooked, HookProc func) : base(HookType.WH_CALLWNDPROCRET, func)
		{
			this.hWndHooked = hWndHooked;
			this.HookInvoke += new HookEventHandler(WndProcRetHookInvoked);
		}

		private void WndProcRetHookInvoked(object sender, HookEventArgs e)
		{
			WndProcRetEventArgs wpe = new WndProcRetEventArgs(e.wParam, e.lParam);
			if ((hWndHooked == IntPtr.Zero || wpe.cw.hwnd == hWndHooked) && WndProcRet != null)
				WndProcRet(this, wpe);
			return;
		}
	}
}
