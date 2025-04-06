/*
 * DBPF Compare - a utility for comparing two DBPF packages
 *
 * William Howard - 2020-2025
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DbpfCompare.Controls
{
    public partial class LinkedTreeView : TreeView
    {
        private LinkedTreeView linkedTreeView = null;
        private bool syncing = false;

        public LinkedTreeView()
        {
            InitializeComponent();

            this.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.OnAfterExpandCollapse);
            this.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.OnAfterExpandCollapse);
        }

        public void AddLinkedTreeView(LinkedTreeView treeView, bool crossLink = true)
        {
            if (treeView == this) return;

            linkedTreeView = treeView;

            if (crossLink) linkedTreeView.AddLinkedTreeView(this, false);
        }

        private void OnAfterExpandCollapse(object sender, TreeViewEventArgs e)
        {
            if (syncing) return;

            SetLinkedExpandCollapse(linkedTreeView, e);
        }

        private void SetLinkedExpandCollapse(LinkedTreeView dest, TreeViewEventArgs e)
        {
            syncing = true;

            TreeNode destNode;

            if (e.Node.Parent == null)
            {
                // This is on the package node
                destNode = dest.Nodes[0];
            }
            else
            {
                // This is on a type node
                destNode = dest.Nodes[0].Nodes[e.Node.Index];
            }

            if (e.Action == TreeViewAction.Expand)
            {
                destNode.Expand();
                if (destNode.Tag != null) (destNode.Tag as DbpfCompareNodeTypeData).Expanded = true;
            }
            else
            {
                destNode.Collapse();
                if (destNode.Tag != null) (destNode.Tag as DbpfCompareNodeTypeData).Expanded = false;
            }

            syncing = false;
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (linkedTreeView != null && (m.Msg == User32.WM_VSCROLL || m.Msg == User32.WM_MOUSEWHEEL))
            {
                SetLinkedScrollPositions(this, linkedTreeView);

                Message copy = new Message
                {
                    HWnd = linkedTreeView.Handle,
                    LParam = m.LParam,
                    Msg = m.Msg,
                    Result = m.Result,
                    WParam = m.WParam
                };

                linkedTreeView.RecieveWndProc(ref copy);
            }
        }

        private void SetLinkedScrollPositions(LinkedTreeView source, LinkedTreeView dest)
        {
            int horizontal = User32.GetScrollPos(source.Handle, Orientation.Horizontal);
            int vertical = User32.GetScrollPos(source.Handle, Orientation.Vertical);

            User32.SetScrollPos(dest.Handle, Orientation.Horizontal, horizontal, true);
            User32.SetScrollPos(dest.Handle, Orientation.Vertical, vertical, true);
        }

        private void RecieveWndProc(ref Message m)
        {
            base.WndProc(ref m);
        }

        private class User32
        {
            public const int WM_VSCROLL = 0x115;
            public const int WM_MOUSEWHEEL = 0x020A;

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern int GetScrollPos(IntPtr hWnd, System.Windows.Forms.Orientation nBar);

            [DllImport("user32.dll")]
            public static extern int SetScrollPos(IntPtr hWnd, System.Windows.Forms.Orientation nBar, int nPos, bool bRedraw);
        }
    }
}
