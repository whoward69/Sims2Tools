namespace Sims2Tools.Controls
{
    partial class SecretLotButton
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
            this.iconSecretLot = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.iconSecretLot)).BeginInit();
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
            // iconSecretLot
            // 
            this.iconSecretLot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.iconSecretLot.BackColor = System.Drawing.Color.Transparent;
            this.iconSecretLot.BackgroundImage = global::Sims2Tools.Properties.Resources.Vacation_SecretLot;
            this.iconSecretLot.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.iconSecretLot.Location = new System.Drawing.Point(0, 35);
            this.iconSecretLot.Name = "iconSecretLot";
            this.iconSecretLot.Size = new System.Drawing.Size(25, 25);
            this.iconSecretLot.TabIndex = 3;
            this.iconSecretLot.TabStop = false;
            this.iconSecretLot.Click += new System.EventHandler(this.OnSecretLotIconClicked);
            // 
            // SecretLotButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.iconSecretLot);
            this.Controls.Add(this.button);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "SecretLotButton";
            this.Size = new System.Drawing.Size(60, 60);
            ((System.ComponentModel.ISupportInitialize)(this.iconSecretLot)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button;
        private System.Windows.Forms.PictureBox iconSecretLot;
    }
}
