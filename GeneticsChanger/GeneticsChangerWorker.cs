/*
 * Genetics Changer - a utility for changing Sims 2 genetic items (skins, eyes, hairs)
 *                  - see http://www.picknmixmods.com/Sims2/Notes/GeneticsChanger/GeneticsChanger.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System.Data;
using System.Windows.Forms;

namespace GeneticsChanger
{
    public class WorkerPackage
    {
        private readonly string folder;
        private readonly bool updateFolders;
        private readonly bool updatePackages;
        private readonly bool updateResources;

        public string Folder => folder;
        public bool UpdateFolders => updateFolders;
        public bool UpdatePackages => updatePackages;
        public bool UpdateResources => updateResources;

        public WorkerPackage(string folder, bool updateFolder, bool updatePackages, bool updateResources)
        {
            this.folder = folder;
            this.updateFolders = updateFolder;
            this.updatePackages = updatePackages;
            this.updateResources = updateResources;
        }
    }

    public interface IWorkerTask
    {
        // This should only be called on the main UI thread
        void DoTask();
    }

    public class WorkerTreeTask : IWorkerTask
    {
        private readonly TreeNodeCollection nodes;
        private readonly string key;
        private readonly string text;
        private readonly ContextMenuStrip contextMenu;

        private TreeNode child;

        public TreeNode ChildNode => child;

        public WorkerTreeTask(TreeNodeCollection nodes, string key, string text, ContextMenuStrip contextMenu)
        {
            this.nodes = nodes;
            this.key = key;
            this.text = text;
            this.contextMenu = contextMenu;
        }

        public void DoTask()
        {
            child = nodes.Add(key, text);
            child.ContextMenuStrip = contextMenu;
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
