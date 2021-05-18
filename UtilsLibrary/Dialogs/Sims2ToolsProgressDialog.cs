/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2021
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.Controls;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Sims2Tools
{
    // See - https://www.codeproject.com/Articles/160219/ProgressForm-A-simple-form-linked-to-a-BackgroundW
    public partial class Sims2ToolsProgressDialog : Form
    {
        public delegate void DoWorkEventHandler(Sims2ToolsProgressDialog sender, DoWorkEventArgs e);
        public event DoWorkEventHandler DoWork;
        public event DoWorkEventHandler DoData;

        public object Argument { get; set; }

        public RunWorkerCompletedEventArgs Result { get; private set; }

        public bool CancellationPending
        {
            get => backgroundWorker.CancellationPending;
        }

        public string DefaultStatusText { get; set; } = "Please wait...";

        public string CancellingText { get; set; } = "Cancelling operation...";

        public ProgressBarDisplayMode VisualMode
        {
            get => progressBar.VisualMode;
            set => progressBar.VisualMode = value;
        }

        public Sims2ToolsProgressDialog(object argument = null)
        {
            InitializeComponent();

            Argument = argument;
        }

        public void SetData(object data)
        {
            DoData?.Invoke(this, new DoWorkEventArgs(data));
        }

        public void SetProgress(int percent, string status = null)
        {
            backgroundWorker.ReportProgress(percent, status);
        }

        private void OnLoad(object sender, EventArgs e)
        {
            Result = null;

            btnProgressCancel.Enabled = true;
            progressBar.Value = progressBar.Minimum;
            progressBar.CustomText = DefaultStatusText;

            backgroundWorker.RunWorkerAsync(Argument);
        }

        private void OnCancelClicked(object sender, EventArgs e)
        {
            if (backgroundWorker.IsBusy)
            {
                backgroundWorker.CancelAsync();
                btnProgressCancel.Enabled = false;
                progressBar.CustomText = CancellingText;
            }
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            DoWork?.Invoke(this, e);
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;

            if (e.UserState != null && !backgroundWorker.CancellationPending)
                progressBar.CustomText = e.UserState.ToString();
        }

        private void BackgroundWorker_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            Result = e;

            if (e.Error != null)
                DialogResult = DialogResult.Abort;
            else if (e.Cancelled)
                DialogResult = DialogResult.Cancel;
            else
                DialogResult = DialogResult.OK;

            Close();
        }
    }
}
