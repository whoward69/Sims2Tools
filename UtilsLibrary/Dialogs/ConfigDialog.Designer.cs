/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2025
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */
 
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Sims2Tools
{
    partial class ConfigDialog
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigDialog));
            this.lblSims2ExePath = new System.Windows.Forms.Label();
            this.textSims2ExePath = new System.Windows.Forms.TextBox();
            this.btnSims2ExeSelect = new System.Windows.Forms.Button();
            this.lblSims2InstallPath = new System.Windows.Forms.Label();
            this.textSims2InstallPath = new System.Windows.Forms.TextBox();
            this.btnSims2InstallSelect = new System.Windows.Forms.Button();
            this.btnConfigOK = new System.Windows.Forms.Button();
            this.lblSims2HomePath = new System.Windows.Forms.Label();
            this.textSims2HomePath = new System.Windows.Forms.TextBox();
            this.btnSims2HomeSelect = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.ckbAllAdvancedMode = new System.Windows.Forms.CheckBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.lblHelpExePath = new System.Windows.Forms.Label();
            this.lblHelpHomePath = new System.Windows.Forms.Label();
            this.lblHelpInstallPath = new System.Windows.Forms.Label();
            this.btnSims2EpSpSelect = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblSims2ExePath
            // 
            this.lblSims2ExePath.AutoSize = true;
            this.lblSims2ExePath.Location = new System.Drawing.Point(10, 19);
            this.lblSims2ExePath.Name = "lblSims2ExePath";
            this.lblSims2ExePath.Size = new System.Drawing.Size(100, 15);
            this.lblSims2ExePath.TabIndex = 0;
            this.lblSims2ExePath.Text = "Sims 2 Exe Path:";
            this.toolTip.SetToolTip(this.lblSims2ExePath, "Path to the folder that contains the TSBin sub-folder that contains the .exe file" +
        " used to start The Sims 2");
            // 
            // textSims2ExePath
            // 
            this.textSims2ExePath.Location = new System.Drawing.Point(140, 16);
            this.textSims2ExePath.Name = "textSims2ExePath";
            this.textSims2ExePath.Size = new System.Drawing.Size(515, 21);
            this.textSims2ExePath.TabIndex = 1;
            this.toolTip.SetToolTip(this.textSims2ExePath, "Path to the folder that contains the TSBin sub-folder that contains the .exe file" +
        " used to start The Sims 2");
            this.textSims2ExePath.TextChanged += new System.EventHandler(this.OnTextChanged);
            // 
            // btnSims2ExeSelect
            // 
            this.btnSims2ExeSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSims2ExeSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSims2ExeSelect.Location = new System.Drawing.Point(665, 11);
            this.btnSims2ExeSelect.Name = "btnSims2ExeSelect";
            this.btnSims2ExeSelect.Size = new System.Drawing.Size(143, 30);
            this.btnSims2ExeSelect.TabIndex = 2;
            this.btnSims2ExeSelect.Text = "Select &Exe Path...";
            this.btnSims2ExeSelect.UseVisualStyleBackColor = true;
            this.btnSims2ExeSelect.Click += new System.EventHandler(this.OnSelectSims2ExePathClicked);
            // 
            // lblSims2InstallPath
            // 
            this.lblSims2InstallPath.AutoSize = true;
            this.lblSims2InstallPath.Location = new System.Drawing.Point(10, 93);
            this.lblSims2InstallPath.Name = "lblSims2InstallPath";
            this.lblSims2InstallPath.Size = new System.Drawing.Size(111, 15);
            this.lblSims2InstallPath.TabIndex = 0;
            this.lblSims2InstallPath.Text = "Sims 2 Install Path:";
            this.toolTip.SetToolTip(this.lblSims2InstallPath, "Path to the folder containing the sub-folders for the base game, EPs and SPs");
            // 
            // textSims2InstallPath
            // 
            this.textSims2InstallPath.Location = new System.Drawing.Point(140, 90);
            this.textSims2InstallPath.Name = "textSims2InstallPath";
            this.textSims2InstallPath.Size = new System.Drawing.Size(515, 21);
            this.textSims2InstallPath.TabIndex = 3;
            this.toolTip.SetToolTip(this.textSims2InstallPath, "Path to the folder containing the sub-folders for the base game, EPs and SPs");
            this.textSims2InstallPath.TextChanged += new System.EventHandler(this.OnTextChanged);
            // 
            // btnSims2InstallSelect
            // 
            this.btnSims2InstallSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSims2InstallSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSims2InstallSelect.Location = new System.Drawing.Point(665, 85);
            this.btnSims2InstallSelect.Name = "btnSims2InstallSelect";
            this.btnSims2InstallSelect.Size = new System.Drawing.Size(143, 30);
            this.btnSims2InstallSelect.TabIndex = 4;
            this.btnSims2InstallSelect.Text = "Select &Install Path...";
            this.btnSims2InstallSelect.UseVisualStyleBackColor = true;
            this.btnSims2InstallSelect.Click += new System.EventHandler(this.OnSelectSims2InstallPathClicked);
            // 
            // btnConfigOK
            // 
            this.btnConfigOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfigOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnConfigOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfigOK.Location = new System.Drawing.Point(665, 171);
            this.btnConfigOK.Name = "btnConfigOK";
            this.btnConfigOK.Size = new System.Drawing.Size(143, 30);
            this.btnConfigOK.TabIndex = 5;
            this.btnConfigOK.Text = "OK";
            this.btnConfigOK.UseVisualStyleBackColor = true;
            this.btnConfigOK.Click += new System.EventHandler(this.OnConfigOkClicked);
            // 
            // lblSims2HomePath
            // 
            this.lblSims2HomePath.AutoSize = true;
            this.lblSims2HomePath.Location = new System.Drawing.Point(10, 56);
            this.lblSims2HomePath.Name = "lblSims2HomePath";
            this.lblSims2HomePath.Size = new System.Drawing.Size(113, 15);
            this.lblSims2HomePath.TabIndex = 6;
            this.lblSims2HomePath.Text = "Sims 2 Home Path:";
            this.toolTip.SetToolTip(this.lblSims2HomePath, "Path to folder containing the Downloads sub-folder");
            // 
            // textSims2HomePath
            // 
            this.textSims2HomePath.Location = new System.Drawing.Point(140, 53);
            this.textSims2HomePath.Name = "textSims2HomePath";
            this.textSims2HomePath.Size = new System.Drawing.Size(515, 21);
            this.textSims2HomePath.TabIndex = 7;
            this.toolTip.SetToolTip(this.textSims2HomePath, "Path to folder containing the Downloads sub-folder");
            this.textSims2HomePath.TextChanged += new System.EventHandler(this.OnTextChanged);
            // 
            // btnSims2HomeSelect
            // 
            this.btnSims2HomeSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSims2HomeSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSims2HomeSelect.Location = new System.Drawing.Point(665, 48);
            this.btnSims2HomeSelect.Name = "btnSims2HomeSelect";
            this.btnSims2HomeSelect.Size = new System.Drawing.Size(143, 30);
            this.btnSims2HomeSelect.TabIndex = 8;
            this.btnSims2HomeSelect.Text = "Select &Home Path...";
            this.btnSims2HomeSelect.UseVisualStyleBackColor = true;
            this.btnSims2HomeSelect.Click += new System.EventHandler(this.OnSelectSims2HomePathClicked);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(514, 171);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(143, 30);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // ckbAllAdvancedMode
            // 
            this.ckbAllAdvancedMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ckbAllAdvancedMode.AutoSize = true;
            this.ckbAllAdvancedMode.Location = new System.Drawing.Point(140, 178);
            this.ckbAllAdvancedMode.Name = "ckbAllAdvancedMode";
            this.ckbAllAdvancedMode.Size = new System.Drawing.Size(160, 19);
            this.ckbAllAdvancedMode.TabIndex = 10;
            this.ckbAllAdvancedMode.Text = "All Apps Advanced Mode";
            this.ckbAllAdvancedMode.UseVisualStyleBackColor = true;
            // 
            // lblHelpExePath
            // 
            this.lblHelpExePath.AutoSize = true;
            this.lblHelpExePath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHelpExePath.Location = new System.Drawing.Point(120, 19);
            this.lblHelpExePath.Name = "lblHelpExePath";
            this.lblHelpExePath.Size = new System.Drawing.Size(15, 15);
            this.lblHelpExePath.TabIndex = 12;
            this.lblHelpExePath.Text = "?";
            this.toolTip.SetToolTip(this.lblHelpExePath, "Path to the folder that contains the TSBin sub-folder that contains the .exe file" +
        " used to start The Sims 2");
            // 
            // lblHelpHomePath
            // 
            this.lblHelpHomePath.AutoSize = true;
            this.lblHelpHomePath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHelpHomePath.Location = new System.Drawing.Point(120, 56);
            this.lblHelpHomePath.Name = "lblHelpHomePath";
            this.lblHelpHomePath.Size = new System.Drawing.Size(15, 15);
            this.lblHelpHomePath.TabIndex = 13;
            this.lblHelpHomePath.Text = "?";
            this.toolTip.SetToolTip(this.lblHelpHomePath, "Path to folder containing the Downloads sub-folder");
            // 
            // lblHelpInstallPath
            // 
            this.lblHelpInstallPath.AutoSize = true;
            this.lblHelpInstallPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHelpInstallPath.Location = new System.Drawing.Point(120, 93);
            this.lblHelpInstallPath.Name = "lblHelpInstallPath";
            this.lblHelpInstallPath.Size = new System.Drawing.Size(15, 15);
            this.lblHelpInstallPath.TabIndex = 14;
            this.lblHelpInstallPath.Text = "?";
            this.toolTip.SetToolTip(this.lblHelpInstallPath, "Path to the folder containing the sub-folders for the base game, EPs and SPs");
            // 
            // btnSims2EpSpSelect
            // 
            this.btnSims2EpSpSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSims2EpSpSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSims2EpSpSelect.Location = new System.Drawing.Point(665, 122);
            this.btnSims2EpSpSelect.Name = "btnSims2EpSpSelect";
            this.btnSims2EpSpSelect.Size = new System.Drawing.Size(143, 30);
            this.btnSims2EpSpSelect.TabIndex = 15;
            this.btnSims2EpSpSelect.Text = "Select EP && SP Paths...";
            this.btnSims2EpSpSelect.UseVisualStyleBackColor = true;
            this.btnSims2EpSpSelect.Click += new System.EventHandler(this.OnSelectSims2EpSpPathClicked);
            // 
            // ConfigDialog
            // 
            this.AcceptButton = this.btnConfigOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(820, 213);
            this.Controls.Add(this.btnSims2EpSpSelect);
            this.Controls.Add(this.lblHelpInstallPath);
            this.Controls.Add(this.lblHelpHomePath);
            this.Controls.Add(this.lblHelpExePath);
            this.Controls.Add(this.ckbAllAdvancedMode);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblSims2ExePath);
            this.Controls.Add(this.textSims2ExePath);
            this.Controls.Add(this.btnSims2ExeSelect);
            this.Controls.Add(this.lblSims2InstallPath);
            this.Controls.Add(this.textSims2InstallPath);
            this.Controls.Add(this.btnSims2InstallSelect);
            this.Controls.Add(this.btnConfigOK);
            this.Controls.Add(this.lblSims2HomePath);
            this.Controls.Add(this.textSims2HomePath);
            this.Controls.Add(this.btnSims2HomeSelect);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configuration";
            this.Load += new System.EventHandler(this.OnConfigLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSims2ExePath;
        private System.Windows.Forms.TextBox textSims2ExePath;
        private System.Windows.Forms.Button btnSims2ExeSelect;
        private System.Windows.Forms.Label lblSims2InstallPath;
        private System.Windows.Forms.TextBox textSims2InstallPath;
        private System.Windows.Forms.Button btnSims2InstallSelect;
        private System.Windows.Forms.Button btnConfigOK;
        private CommonOpenFileDialog selectPathDialog;
        private System.Windows.Forms.Label lblSims2HomePath;
        private System.Windows.Forms.TextBox textSims2HomePath;
        private System.Windows.Forms.Button btnSims2HomeSelect;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox ckbAllAdvancedMode;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label lblHelpExePath;
        private System.Windows.Forms.Label lblHelpHomePath;
        private System.Windows.Forms.Label lblHelpInstallPath;
        private System.Windows.Forms.Button btnSims2EpSpSelect;
    }
}