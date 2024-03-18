/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2023
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigDialog));
            this.lblSims2Path = new System.Windows.Forms.Label();
            this.textSims2Path = new System.Windows.Forms.TextBox();
            this.btnSims2Select = new System.Windows.Forms.Button();
            this.lblSimPEPath = new System.Windows.Forms.Label();
            this.textSimPePath = new System.Windows.Forms.TextBox();
            this.btnSimPESelect = new System.Windows.Forms.Button();
            this.btnConfigOK = new System.Windows.Forms.Button();
            this.lblSimsHomePath = new System.Windows.Forms.Label();
            this.textSims2HomePath = new System.Windows.Forms.TextBox();
            this.btnSimsHomeSelect = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblSims2Path
            // 
            this.lblSims2Path.AutoSize = true;
            this.lblSims2Path.Location = new System.Drawing.Point(10, 19);
            this.lblSims2Path.Name = "lblSims2Path";
            this.lblSims2Path.Size = new System.Drawing.Size(111, 15);
            this.lblSims2Path.TabIndex = 0;
            this.lblSims2Path.Text = "Sims 2 Install Path:";
            // 
            // textSims2Path
            // 
            this.textSims2Path.Location = new System.Drawing.Point(127, 16);
            this.textSims2Path.Name = "textSims2Path";
            this.textSims2Path.Size = new System.Drawing.Size(530, 21);
            this.textSims2Path.TabIndex = 1;
            this.textSims2Path.TextChanged += new System.EventHandler(this.OnTextChanged);
            // 
            // btnSims2Select
            // 
            this.btnSims2Select.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSims2Select.Location = new System.Drawing.Point(665, 11);
            this.btnSims2Select.Name = "btnSims2Select";
            this.btnSims2Select.Size = new System.Drawing.Size(143, 30);
            this.btnSims2Select.TabIndex = 2;
            this.btnSims2Select.Text = "Select &Sims 2 Path...";
            this.btnSims2Select.UseVisualStyleBackColor = true;
            this.btnSims2Select.Click += new System.EventHandler(this.OnSelectSim2PathClicked);
            // 
            // lblSimPEPath
            // 
            this.lblSimPEPath.AutoSize = true;
            this.lblSimPEPath.Location = new System.Drawing.Point(10, 92);
            this.lblSimPEPath.Name = "lblSimPEPath";
            this.lblSimPEPath.Size = new System.Drawing.Size(111, 15);
            this.lblSimPEPath.TabIndex = 0;
            this.lblSimPEPath.Text = "SimPE Install Path:";
            // 
            // textSimPEPath
            // 
            this.textSimPePath.Location = new System.Drawing.Point(127, 89);
            this.textSimPePath.Name = "textSimPEPath";
            this.textSimPePath.Size = new System.Drawing.Size(530, 21);
            this.textSimPePath.TabIndex = 3;
            this.textSimPePath.TextChanged += new System.EventHandler(this.OnTextChanged);
            // 
            // btnSimPESelect
            // 
            this.btnSimPESelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSimPESelect.Location = new System.Drawing.Point(665, 84);
            this.btnSimPESelect.Name = "btnSimPESelect";
            this.btnSimPESelect.Size = new System.Drawing.Size(143, 30);
            this.btnSimPESelect.TabIndex = 4;
            this.btnSimPESelect.Text = "Select Sim&PE Path...";
            this.btnSimPESelect.UseVisualStyleBackColor = true;
            this.btnSimPESelect.Click += new System.EventHandler(this.OnSelectSimPEPathClicked);
            // 
            // btnConfigOK
            // 
            this.btnConfigOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnConfigOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfigOK.Location = new System.Drawing.Point(665, 127);
            this.btnConfigOK.Name = "btnConfigOK";
            this.btnConfigOK.Size = new System.Drawing.Size(143, 30);
            this.btnConfigOK.TabIndex = 5;
            this.btnConfigOK.Text = "OK";
            this.btnConfigOK.UseVisualStyleBackColor = true;
            this.btnConfigOK.Click += new System.EventHandler(this.OnConfigOkClicked);
            // 
            // lblSimsHomePath
            // 
            this.lblSimsHomePath.AutoSize = true;
            this.lblSimsHomePath.Location = new System.Drawing.Point(10, 56);
            this.lblSimsHomePath.Name = "lblSimsHomePath";
            this.lblSimsHomePath.Size = new System.Drawing.Size(113, 15);
            this.lblSimsHomePath.TabIndex = 6;
            this.lblSimsHomePath.Text = "Sims 2 Home Path:";
            // 
            // textSims2HomePath
            // 
            this.textSims2HomePath.Location = new System.Drawing.Point(127, 53);
            this.textSims2HomePath.Name = "textSims2HomePath";
            this.textSims2HomePath.Size = new System.Drawing.Size(530, 21);
            this.textSims2HomePath.TabIndex = 7;
            this.textSims2HomePath.TextChanged += new System.EventHandler(this.OnTextChanged);
            // 
            // btnSimsHomeSelect
            // 
            this.btnSimsHomeSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSimsHomeSelect.Location = new System.Drawing.Point(665, 48);
            this.btnSimsHomeSelect.Name = "btnSimsHomeSelect";
            this.btnSimsHomeSelect.Size = new System.Drawing.Size(143, 30);
            this.btnSimsHomeSelect.TabIndex = 8;
            this.btnSimsHomeSelect.Text = "Select Home Path...";
            this.btnSimsHomeSelect.UseVisualStyleBackColor = true;
            this.btnSimsHomeSelect.Click += new System.EventHandler(this.OnSelectSim2HomePathClicked);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(514, 127);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(143, 30);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // ConfigDialog
            // 
            this.AcceptButton = this.btnConfigOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(820, 169);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblSims2Path);
            this.Controls.Add(this.textSims2Path);
            this.Controls.Add(this.btnSims2Select);
            this.Controls.Add(this.lblSimPEPath);
            this.Controls.Add(this.textSimPePath);
            this.Controls.Add(this.btnSimPESelect);
            this.Controls.Add(this.btnConfigOK);
            this.Controls.Add(this.lblSimsHomePath);
            this.Controls.Add(this.textSims2HomePath);
            this.Controls.Add(this.btnSimsHomeSelect);
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

        private System.Windows.Forms.Label lblSims2Path;
        private System.Windows.Forms.TextBox textSims2Path;
        private System.Windows.Forms.Button btnSims2Select;
        private System.Windows.Forms.Label lblSimPEPath;
        private System.Windows.Forms.TextBox textSimPePath;
        private System.Windows.Forms.Button btnSimPESelect;
        private System.Windows.Forms.Button btnConfigOK;
        private CommonOpenFileDialog selectPathDialog;
        private System.Windows.Forms.Label lblSimsHomePath;
        private System.Windows.Forms.TextBox textSims2HomePath;
        private System.Windows.Forms.Button btnSimsHomeSelect;
        private System.Windows.Forms.Button btnCancel;
    }
}