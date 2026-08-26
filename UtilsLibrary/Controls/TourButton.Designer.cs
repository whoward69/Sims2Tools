namespace Sims2Tools.Controls
{
    partial class TourButton
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
            this.button = new System.Windows.Forms.Button();
            this.iconVacation = new System.Windows.Forms.PictureBox();
            this.iconTourGuide = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.iconVacation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconTourGuide)).BeginInit();
            this.SuspendLayout();
            // 
            // button
            // 
            this.button.BackColor = System.Drawing.Color.LightGray;
            this.button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button.Location = new System.Drawing.Point(0, 0);
            this.button.Margin = new System.Windows.Forms.Padding(0);
            this.button.Name = "button";
            this.button.Size = new System.Drawing.Size(60, 60);
            this.button.TabIndex = 0;
            this.button.UseVisualStyleBackColor = false;
            // 
            // iconVacation
            // 
            this.iconVacation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.iconVacation.BackColor = System.Drawing.Color.Transparent;
            this.iconVacation.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.iconVacation.Location = new System.Drawing.Point(35, 35);
            this.iconVacation.Name = "iconVacation";
            this.iconVacation.Size = new System.Drawing.Size(22, 22);
            this.iconVacation.TabIndex = 3;
            this.iconVacation.TabStop = false;
            this.iconVacation.Click += new System.EventHandler(this.OnVacationIconClicked);
            // 
            // iconTourGuide
            // 
            this.iconTourGuide.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.iconTourGuide.BackColor = System.Drawing.Color.Transparent;
            this.iconTourGuide.BackgroundImage = global::Sims2Tools.Properties.Resources.Vacation_TourGuide;
            this.iconTourGuide.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.iconTourGuide.Location = new System.Drawing.Point(3, 35);
            this.iconTourGuide.Name = "iconTourGuide";
            this.iconTourGuide.Size = new System.Drawing.Size(22, 22);
            this.iconTourGuide.TabIndex = 2;
            this.iconTourGuide.TabStop = false;
            this.iconTourGuide.Click += new System.EventHandler(this.OnTourGuideIconClicked);
            // 
            // TourButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.iconVacation);
            this.Controls.Add(this.iconTourGuide);
            this.Controls.Add(this.button);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "TourButton";
            this.Size = new System.Drawing.Size(60, 60);
            ((System.ComponentModel.ISupportInitialize)(this.iconVacation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconTourGuide)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button;
        private System.Windows.Forms.PictureBox iconTourGuide;
        private System.Windows.Forms.PictureBox iconVacation;
    }
}
