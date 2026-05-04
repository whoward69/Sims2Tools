/*
 * Family Manager - a utility for manipulating family closets
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System.Data;
using System.Windows.Forms;

namespace FamilyManager
{
    public class WorkerPackage
    {
    }

    public interface IWorkerTask
    {
        // This should only be called on the main UI thread
        void DoTask();
    }

    public class WorkerAddTreeNodeTask : IWorkerTask
    {
        private readonly TreeNodeCollection nodes;
        private readonly TreeNode child;

        public TreeNode ChildNode => child;

        public WorkerAddTreeNodeTask(TreeNodeCollection nodes, TreeNode child)
        {
            this.nodes = nodes;
            this.child = child;
        }

        public void DoTask()
        {
            nodes.Add(child);
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
