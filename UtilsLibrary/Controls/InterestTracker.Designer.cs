/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

namespace Sims2Tools.Controls
{
    partial class InterestTracker
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox = new Sims2Tools.Controls.DoubleTextBox();
            this.trackBar = new Sims2Tools.Controls.SimTrackingBar();
            this.SuspendLayout();
            // 
            // textBox
            // 
            this.textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox.Format = "N2";
            this.textBox.Location = new System.Drawing.Point(103, 0);
            this.textBox.Maximum = 1000F;
            this.textBox.Minimum = 0F;
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(35, 20);
            this.textBox.TabIndex = 1;
            this.textBox.Value = 0F;
            this.textBox.TextChanged += new System.EventHandler(this.OnTextBoxChanged);
            // 
            // trackBar
            // 
            this.trackBar.BackColor = System.Drawing.Color.Transparent;
            this.trackBar.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.trackBar.Gradient = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.trackBar.GradientEndColor = System.Drawing.Color.White;
            this.trackBar.GradientStartColor = System.Drawing.Color.White;
            this.trackBar.Location = new System.Drawing.Point(0, 1);
            this.trackBar.Maximum = 1000;
            this.trackBar.Minimum = 0;
            this.trackBar.Name = "trackBar";
            this.trackBar.ProgressBackColor = System.Drawing.SystemColors.Window;
            this.trackBar.Quality = true;
            this.trackBar.SelectedColor = System.Drawing.Color.YellowGreen;
            this.trackBar.Size = new System.Drawing.Size(100, 20);
            this.trackBar.Style = Sims2Tools.Controls.SimsTrackingBarStyle.Increase;
            this.trackBar.TabIndex = 0;
            this.trackBar.TokenCount = 10;
            this.trackBar.UnselectedColor = System.Drawing.Color.Black;
            this.trackBar.UseTokenBuffer = true;
            this.trackBar.Value = 0;
            this.trackBar.Changed += new System.EventHandler(this.OnTrackBarChanged);
            // 
            // InterestTracker
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.trackBar);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "InterestTracker";
            this.Size = new System.Drawing.Size(138, 21);
            this.ResumeLayout(false);

        }

        #endregion

        private SimTrackingBar trackBar;
        private Sims2Tools.Controls.DoubleTextBox textBox;
    }
}
