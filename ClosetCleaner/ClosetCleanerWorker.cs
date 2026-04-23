/*
 * Closet Cleaner - a utility for moving objects in the Buy/Build Mode catalogues
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System.Data;
using System.Windows.Forms;

namespace ClosetCleaner
{
    public class WorkerPackage
    {
        private readonly TreeNode selectedNode;
        private readonly bool updateHoodTree;
        private readonly bool updateHoodOrFamilyPanel;
        private readonly bool updateResources;

        public TreeNode SelectedNode => selectedNode;
        public bool UpdateHoodTree => updateHoodTree;
        public bool UpdateHoodOrFamilyPanel => updateHoodOrFamilyPanel;
        public bool UpdateResources => updateResources;

        public WorkerPackage(TreeNode selectedNode, bool updateHoodTree, bool updateHoodOrFamilyPanel, bool updateResources)
        {
            this.selectedNode = selectedNode;
            this.updateHoodTree = updateHoodTree;
            this.updateHoodOrFamilyPanel = updateHoodOrFamilyPanel;
            this.updateResources = updateResources;
        }
    }

    public interface IWorkerTask
    {
        // This should only be called on the main UI thread
        void DoTask();
    }

    public class WorkerAddTreeNodeTask : IWorkerTask
    {
        private readonly TreeNodeCollection nodes;
        private readonly string key;
        private readonly string text;
        private readonly string tag;

        private TreeNode child;

        public TreeNode ChildNode => child;

        public WorkerAddTreeNodeTask(TreeNodeCollection nodes, string key, string text, string tag)
        {
            this.nodes = nodes;
            this.key = key;
            this.text = text;
            this.tag = tag;
        }

        public void DoTask()
        {
            child = nodes.Add(key, text, tag);
            child.Tag = tag;
        }
    }

    public class WorkerRenameTreeNodeTask : IWorkerTask
    {
        private readonly TreeNode node;
        private readonly string text;

        public WorkerRenameTreeNodeTask(TreeNode node, string text)
        {
            this.node = node;
            this.text = text;
        }

        public void DoTask()
        {
            node.Text = text;
        }
    }

    public class WorkerGridTask : IWorkerTask
    {
        private readonly DataTable table;
        private readonly DataRow row;

        public WorkerGridTask(DataTable table, DataRow row)
        {
            this.table = table;
            this.row = row;
        }

        public void DoTask()
        {
            table.Rows.Add(row);
        }
    }
}
