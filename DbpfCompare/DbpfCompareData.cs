/*
 * DBPF Compare - a utility for comparing two DBPF packages
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF;
using System.Drawing;
using System.Windows.Forms;

namespace DbpfCompare
{
    internal enum DbpfNodeState
    {
        Same = 0,
        LeftMissing,
        RightMissing,
        Different,
        CopyLeftToRight,
        ToBeDeleted
    }

    internal class DbpfCompareNodeTypeData
    {
        private readonly TypeTypeID typeId;
        private readonly ContextMenuStrip contextMenu;


        private TreeNode leftNode = null;
        private TreeNode rightNode = null;

        private bool expanded = true;

        internal bool Expanded
        {
            get => expanded;
            set => expanded = value;
        }

        internal DbpfCompareNodeTypeData(TypeTypeID typeId, ContextMenuStrip contextMenu)
        {
            this.typeId = typeId;
            this.contextMenu = contextMenu;
        }

        internal bool IsDirty
        {
            get
            {
                foreach (TreeNode node in rightNode.Nodes)
                {
                    if ((node.Tag as DbpfCompareNodeResourceData).IsDirty) return true;
                }

                return false;
            }
        }

        internal bool AnyMissing
        {
            get
            {
                foreach (TreeNode node in leftNode.Nodes)
                {
                    if ((node.Tag as DbpfCompareNodeResourceData).IsRightMissing) return true;
                }

                return false;
            }
        }

        internal int NodeCount => (leftNode.Nodes.Count + rightNode.Nodes.Count);

        internal void Clear()
        {
            leftNode?.Nodes.Clear();
            rightNode?.Nodes.Clear();
        }

        internal void EnsureVisible()
        {
            leftNode?.EnsureVisible();
            rightNode?.EnsureVisible();
        }

        internal Color ForeColor
        {
            set
            {
                leftNode.ForeColor = value;
                rightNode.ForeColor = value;
            }
        }

        internal TreeNode GetLeftNode()
        {
            if (leftNode == null)
            {
                leftNode = new TreeNode
                {
                    Text = DBPFData.TypeName(typeId),
                    Tag = this,
                    ContextMenuStrip = contextMenu
                };
            }

            return leftNode;
        }

        internal TreeNode GetRightNode()
        {
            if (rightNode == null)
            {
                rightNode = new TreeNode
                {
                    Text = DBPFData.TypeName(typeId),
                    Tag = this
                };
            }

            return rightNode;
        }

        internal void AddLeft(TreeNode node)
        {
            leftNode.Nodes.Add(node);
        }

        internal void AddRight(TreeNode node)
        {
            rightNode.Nodes.Add(node);
        }
    }

    public class DbpfCompareNodeResourceData
    {
        private readonly DBPFKey key;
        private readonly ContextMenuStrip contextMenu;
        private DbpfNodeState state = DbpfNodeState.Same;

        private TreeNode leftNode = null;
        private TreeNode rightNode = null;

        internal DBPFKey Key => key;

        internal bool IsToBeCopied => (state == DbpfNodeState.CopyLeftToRight);
        internal bool IsToBeDeleted => (state == DbpfNodeState.ToBeDeleted);
        internal bool IsDirty => (IsToBeCopied || IsToBeDeleted);

        internal bool IsSame => (state == DbpfNodeState.Same || state == DbpfNodeState.CopyLeftToRight);
        internal bool IsLeftMissing => (state == DbpfNodeState.LeftMissing);
        internal bool IsRightMissing => (state == DbpfNodeState.RightMissing);
        internal bool IsDifferent => (state == DbpfNodeState.Different);

        internal TypeTypeID TypeID => key.TypeID;
        internal string TypeName => DBPFData.TypeName(key.TypeID);

        internal DbpfCompareNodeResourceData(DBPFKey key, ContextMenuStrip contextMenu, DbpfNodeState state = DbpfNodeState.Same)
        {
            this.key = key;
            this.contextMenu = contextMenu;
            this.state = state;
        }

        internal void SetSame() => state = DbpfNodeState.Same;
        internal void SetLeftMissing() => state = DbpfNodeState.LeftMissing;
        internal void SetRightMissing() => state = DbpfNodeState.RightMissing;
        internal void SetDifferent() => state = DbpfNodeState.Different;

        internal void SetCopyLeftToRight()
        {
            state = DbpfNodeState.CopyLeftToRight;

            leftNode.ForeColor = DbpfCompareForm.colourSame;

            rightNode.Text = RightString();
            rightNode.ForeColor = DbpfCompareForm.colourSame;
        }

        internal void SetToBeDeleted()
        {
            state = DbpfNodeState.ToBeDeleted;
        }

        internal void EnsureVisible()
        {
            leftNode?.EnsureVisible();
            rightNode?.EnsureVisible();
        }

        internal TreeNode LeftNode()
        {
            if (leftNode == null)
            {
                leftNode = new TreeNode()
                {
                    Text = LeftString(),
                    ForeColor = IsDifferent ? DbpfCompareForm.colourDiffers : (IsRightMissing ? DbpfCompareForm.colourMissing : DbpfCompareForm.colourSame),
                    Tag = this,
                    ContextMenuStrip = contextMenu
                };
            }

            return leftNode;
        }

        internal TreeNode RightNode()
        {
            if (rightNode == null)
            {
                rightNode = new TreeNode()
                {
                    Text = RightString(),
                    ForeColor = IsDifferent ? DbpfCompareForm.colourDiffers : (IsLeftMissing ? DbpfCompareForm.colourMissing : DbpfCompareForm.colourSame),
                    Tag = this,
                    ContextMenuStrip = contextMenu
                };
            }

            return rightNode;
        }

        private string LeftString() => IsLeftMissing ? "" : key.ToString();
        private string RightString() => IsRightMissing ? "" : key.ToString();
    }
}
