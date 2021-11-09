﻿using System;
using System.Windows.Forms;

namespace LogWatcher.Controls
{
    [System.ComponentModel.DesignerCategory("")]
    public class LogTab : TabPage
    {
        private readonly LogViewer logViewer;
        private readonly String logFilePath;

        public String LogFilePath => logFilePath;

        public LogTab(String logFilePath)
        {
            this.logFilePath = logFilePath;

            logViewer = new LogViewer
            {
                Dock = System.Windows.Forms.DockStyle.Fill,
                Location = new System.Drawing.Point(3, 3),
                Size = new System.Drawing.Size(919, 461),
                TabIndex = 0,

                LogFilePath = LogFilePath
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
    }
}