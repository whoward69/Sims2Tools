/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2023
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

namespace Sims2Tools
{
    partial class Sims2ToolsProgressDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Sims2ToolsProgressDialog));
            this.btnProgressCancel = new System.Windows.Forms.Button();
            this.progressBar = new Sims2Tools.Controls.TextProgressBar();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // btnProgressCancel
            // 
            this.btnProgressCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnProgressCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProgressCancel.Location = new System.Drawing.Point(232, 93);
            this.btnProgressCancel.Name = "btnProgressCancel";
            this.btnProgressCancel.Size = new System.Drawing.Size(143, 30);
            this.btnProgressCancel.TabIndex = 1;
            this.btnProgressCancel.Text = "Cancel";
            this.btnProgressCancel.UseVisualStyleBackColor = true;
            this.btnProgressCancel.Click += new System.EventHandler(this.OnCancelClicked);
            // 
            // progressBar
            // 
            this.progressBar.CustomText = "";
            this.progressBar.Location = new System.Drawing.Point(44, 28);
            this.progressBar.Name = "progressBar";
            this.progressBar.ProgressColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.progressBar.Size = new System.Drawing.Size(519, 26);
            this.progressBar.TabIndex = 2;
            this.progressBar.TextColor = System.Drawing.Color.Black;
            this.progressBar.TextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.progressBar.VisualMode = Sims2Tools.Controls.ProgressBarDisplayMode.NoText;
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.WorkerReportsProgress = true;
            this.backgroundWorker.WorkerSupportsCancellation = true;
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker_DoWork);
            this.backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.BackgroundWorker_ProgressChanged);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorker_Completed);
            // 
            // Sims2ToolsProgressDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(606, 135);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.btnProgressCancel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Sims2ToolsProgressDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Progress";
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnProgressCancel;
        private Controls.TextProgressBar progressBar;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
    }
}