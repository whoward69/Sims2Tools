/*
 * Log Watcher - a utility for monitoring Sims 2 ObjectError logs
 *
 * William Howard - 2020-2025
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System.Windows.Forms;

namespace LogWatcher.Controls
{
    [System.ComponentModel.DesignerCategory("")]
    public class LogTab : TabPage, ISearcher
    {
        private readonly LogViewer logViewer;

        private readonly ISearcher searcher;

        public string LogFilePath
        {
            get => logViewer.LogFilePath;
            set { logViewer.LogFilePath = value; this.Text = logViewer.TabName; }
        }

        public bool IncPropIndex
        {
            get => logViewer.IncPropIndex;
            set => logViewer.IncPropIndex = value;
        }

        public LogTab(ISearcher searcher, string logFilePath, bool incPropIndex)
        {
            this.searcher = searcher;

            logViewer = new LogViewer()
            {
                Searcher = searcher,

                Dock = System.Windows.Forms.DockStyle.Fill,
                Location = new System.Drawing.Point(3, 3),
                Size = new System.Drawing.Size(919, 461),
                TabIndex = 0,

                LogFilePath = logFilePath,
                IncPropIndex = incPropIndex
            };

            this.Location = new System.Drawing.Point(4, 24);
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Size = new System.Drawing.Size(925, 467);
            this.TabIndex = 0;
            this.Text = logViewer.TabName;
            this.UseVisualStyleBackColor = true;

            this.Controls.Add(logViewer);
        }

        public void Reload()
        {
            logViewer.Reload();
        }

        public void FindFirst(string text)
        {
            logViewer.FindFirst(text);
        }

        public void FindNext(string text)
        {
            logViewer.FindNext(text);
        }

        void ISearcher.Reset(bool enabled)
        {
            searcher.Reset(enabled);
        }
    }
}
