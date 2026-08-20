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

    public class HoodLoaderPackage
    {
        private readonly string hoodBaseFolder;

        public string HoodBaseFolder => hoodBaseFolder;

        public HoodLoaderPackage(string hoodBaseFolder)
        {
            this.hoodBaseFolder = hoodBaseFolder;
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

    public class WorkerCensusTask : IWorkerTask
    {
        private readonly CensusGridData gridData;
        private readonly DataRow row;

        public WorkerCensusTask(CensusGridData gridData, DataRow row)
        {
            this.gridData = gridData;
            this.row = row;
        }

        public void DoTask()
        {
            gridData.Rows.Add(row);
        }
    }
}
