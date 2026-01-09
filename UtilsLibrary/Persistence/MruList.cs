/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Parts of this code derived from http://csharphelper.com/blog/2018/06/build-an-mru-list-in-c/
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Sims2Tools.Utils.Persistence
{
    public class MruList
    {
        private readonly string AppRegKey;

        private readonly bool AllowFiles, AllowDirs;

        private readonly int NumFiles;
        private readonly List<FileInfo> FileInfos;

        private readonly ToolStripMenuItem MyMenu;
        private readonly ToolStripMenuItem[] MenuItems;

        public delegate void FileSelectedEventHandler(string filename);
        public event FileSelectedEventHandler FileSelected;

        public MruList(string appRegKey, ToolStripMenuItem menu, int num_files, bool allowFiles, bool allowDirs)
        {
            AppRegKey = appRegKey;
            MyMenu = menu;
            NumFiles = num_files;
            AllowFiles = allowFiles;
            AllowDirs = allowDirs;
            FileInfos = new List<FileInfo>();

            MenuItems = new ToolStripMenuItem[NumFiles + 1];
            for (int i = 0; i < NumFiles; i++)
            {
                MenuItems[i] = new ToolStripMenuItem
                {
                    Visible = false
                };
                MyMenu.DropDownItems.Add(MenuItems[i]);
            }

            LoadFiles();
            ShowFiles();
        }

        private void LoadFiles()
        {
            for (int i = 0; i < NumFiles; i++)
            {
                string fileName = (string)RegistryTools.GetSetting(AppRegKey, $"FilePath{i}", "");
                if (fileName != "")
                {
                    if ((AllowFiles && File.Exists(fileName)) || (AllowDirs && Directory.Exists(fileName)))
                    {
                        FileInfos.Add(new FileInfo(fileName));
                    }
                }
            }
        }

        private void ShowFiles()
        {
            MyMenu.Enabled = (FileInfos.Count > 0);

            for (int i = 0; i < FileInfos.Count; i++)
            {
                MenuItems[i].Text = string.Format("&{0} {1}", i + 1, FileInfos[i].Name);
                MenuItems[i].Visible = true;
                MenuItems[i].Tag = FileInfos[i];
                MenuItems[i].Click -= File_Click;
                MenuItems[i].Click += File_Click;
            }

            for (int i = FileInfos.Count; i < NumFiles; i++)
            {
                MenuItems[i].Visible = false;
                MenuItems[i].Click -= File_Click;
            }
        }

        private void SaveFiles()
        {
            for (int i = 0; i < NumFiles; i++)
            {
                RegistryTools.DeleteSetting(AppRegKey, $"FilePath{i}");
            }

            int index = 0;
            foreach (FileInfo file_info in FileInfos)
            {
                RegistryTools.SaveSetting(AppRegKey, $"FilePath{index}", file_info.FullName);
                index++;
            }
        }

        public void AddFile(string file_name)
        {
            if (file_name.Length > 0)
            {
                if (file_name.EndsWith(@"\"))
                    file_name = file_name.Substring(0, file_name.Length - 1);

                RemoveFileInfo(file_name);
                FileInfos.Insert(0, new FileInfo(file_name));
                if (FileInfos.Count > NumFiles) FileInfos.RemoveAt(NumFiles);

                ShowFiles();
                SaveFiles();
            }
        }

        private void RemoveFileInfo(string file_name)
        {
            if (file_name.Length > 0)
            {
                for (int i = FileInfos.Count - 1; i >= 0; i--)
                {
                    if (FileInfos[i].FullName == file_name)
                        FileInfos.RemoveAt(i);
                }
            }
        }

        public void RemoveFile(string file_name)
        {
            if (file_name.Length > 0)
            {
                RemoveFileInfo(file_name);

                ShowFiles();
                SaveFiles();
            }
        }

        private void File_Click(object sender, EventArgs e)
        {
            if (FileSelected != null)
            {
                ToolStripMenuItem menu_item = sender as ToolStripMenuItem;
                FileInfo file_info = menu_item.Tag as FileInfo;
                FileSelected(file_info.FullName);
            }
        }
    }
}
