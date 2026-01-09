/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */
 
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Sims2Tools
{
    partial class EpSpConfigDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EpSpConfigDialog));
            this.lblBase = new System.Windows.Forms.Label();
            this.textBase = new System.Windows.Forms.TextBox();
            this.btnBase = new System.Windows.Forms.Button();
            this.lblEp2 = new System.Windows.Forms.Label();
            this.textEp2 = new System.Windows.Forms.TextBox();
            this.btnEp2 = new System.Windows.Forms.Button();
            this.btnConfigOK = new System.Windows.Forms.Button();
            this.lblEp1 = new System.Windows.Forms.Label();
            this.textEp1 = new System.Windows.Forms.TextBox();
            this.btnEp1 = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblEp3 = new System.Windows.Forms.Label();
            this.textEp3 = new System.Windows.Forms.TextBox();
            this.btnEp3 = new System.Windows.Forms.Button();
            this.lblEp4 = new System.Windows.Forms.Label();
            this.textEp4 = new System.Windows.Forms.TextBox();
            this.btnEp4 = new System.Windows.Forms.Button();
            this.lblEp5 = new System.Windows.Forms.Label();
            this.textEp5 = new System.Windows.Forms.TextBox();
            this.btnEp5 = new System.Windows.Forms.Button();
            this.lblEp6 = new System.Windows.Forms.Label();
            this.textEp6 = new System.Windows.Forms.TextBox();
            this.btnEp6 = new System.Windows.Forms.Button();
            this.lblEp7 = new System.Windows.Forms.Label();
            this.textEp7 = new System.Windows.Forms.TextBox();
            this.btnEp7 = new System.Windows.Forms.Button();
            this.lblEp8 = new System.Windows.Forms.Label();
            this.textEp8 = new System.Windows.Forms.TextBox();
            this.btnEp8 = new System.Windows.Forms.Button();
            this.lblEp9 = new System.Windows.Forms.Label();
            this.textEp9 = new System.Windows.Forms.TextBox();
            this.btnEp9 = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabSP = new System.Windows.Forms.TabPage();
            this.lblSp1 = new System.Windows.Forms.Label();
            this.btnSp1 = new System.Windows.Forms.Button();
            this.textSp1 = new System.Windows.Forms.TextBox();
            this.btnSp2 = new System.Windows.Forms.Button();
            this.lblSp8 = new System.Windows.Forms.Label();
            this.textSp2 = new System.Windows.Forms.TextBox();
            this.textSp8 = new System.Windows.Forms.TextBox();
            this.lblSp2 = new System.Windows.Forms.Label();
            this.btnSp8 = new System.Windows.Forms.Button();
            this.btnSp3 = new System.Windows.Forms.Button();
            this.lblSp7 = new System.Windows.Forms.Label();
            this.textSp3 = new System.Windows.Forms.TextBox();
            this.textSp7 = new System.Windows.Forms.TextBox();
            this.lblSp3 = new System.Windows.Forms.Label();
            this.btnSp7 = new System.Windows.Forms.Button();
            this.btnSp4 = new System.Windows.Forms.Button();
            this.lblSp6 = new System.Windows.Forms.Label();
            this.textSp4 = new System.Windows.Forms.TextBox();
            this.textSp6 = new System.Windows.Forms.TextBox();
            this.lblSp4 = new System.Windows.Forms.Label();
            this.btnSp6 = new System.Windows.Forms.Button();
            this.btnSp5 = new System.Windows.Forms.Button();
            this.lblSp5 = new System.Windows.Forms.Label();
            this.textSp5 = new System.Windows.Forms.TextBox();
            this.tabEP = new System.Windows.Forms.TabPage();
            this.tabControl.SuspendLayout();
            this.tabSP.SuspendLayout();
            this.tabEP.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblBase
            // 
            this.lblBase.AutoSize = true;
            this.lblBase.Location = new System.Drawing.Point(10, 14);
            this.lblBase.Name = "lblBase";
            this.lblBase.Size = new System.Drawing.Size(75, 15);
            this.lblBase.TabIndex = 0;
            this.lblBase.Text = "Base Game:";
            // 
            // textBase
            // 
            this.textBase.Location = new System.Drawing.Point(124, 11);
            this.textBase.Name = "textBase";
            this.textBase.Size = new System.Drawing.Size(621, 21);
            this.textBase.TabIndex = 1;
            this.textBase.TextChanged += new System.EventHandler(this.OnTextChanged);
            // 
            // btnBase
            // 
            this.btnBase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBase.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBase.Location = new System.Drawing.Point(750, 10);
            this.btnBase.Name = "btnBase";
            this.btnBase.Size = new System.Drawing.Size(60, 23);
            this.btnBase.TabIndex = 2;
            this.btnBase.Text = "Select...";
            this.btnBase.UseVisualStyleBackColor = true;
            this.btnBase.Click += new System.EventHandler(this.OnSelectClicked);
            // 
            // lblEp2
            // 
            this.lblEp2.AutoSize = true;
            this.lblEp2.Location = new System.Drawing.Point(6, 32);
            this.lblEp2.Name = "lblEp2";
            this.lblEp2.Size = new System.Drawing.Size(55, 15);
            this.lblEp2.TabIndex = 0;
            this.lblEp2.Text = "Nightlife:";
            // 
            // textEp2
            // 
            this.textEp2.Location = new System.Drawing.Point(120, 30);
            this.textEp2.Name = "textEp2";
            this.textEp2.Size = new System.Drawing.Size(620, 21);
            this.textEp2.TabIndex = 3;
            this.textEp2.TextChanged += new System.EventHandler(this.OnTextChanged);
            // 
            // btnEp2
            // 
            this.btnEp2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEp2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEp2.Location = new System.Drawing.Point(746, 29);
            this.btnEp2.Name = "btnEp2";
            this.btnEp2.Size = new System.Drawing.Size(60, 23);
            this.btnEp2.TabIndex = 4;
            this.btnEp2.Text = "Select...";
            this.btnEp2.UseVisualStyleBackColor = true;
            this.btnEp2.Click += new System.EventHandler(this.OnSelectClicked);
            // 
            // btnConfigOK
            // 
            this.btnConfigOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfigOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnConfigOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfigOK.Location = new System.Drawing.Point(735, 313);
            this.btnConfigOK.Name = "btnConfigOK";
            this.btnConfigOK.Size = new System.Drawing.Size(81, 30);
            this.btnConfigOK.TabIndex = 5;
            this.btnConfigOK.Text = "OK";
            this.btnConfigOK.UseVisualStyleBackColor = true;
            this.btnConfigOK.Click += new System.EventHandler(this.OnConfigOkClicked);
            // 
            // lblEp1
            // 
            this.lblEp1.AutoSize = true;
            this.lblEp1.Location = new System.Drawing.Point(6, 4);
            this.lblEp1.Name = "lblEp1";
            this.lblEp1.Size = new System.Drawing.Size(62, 15);
            this.lblEp1.TabIndex = 6;
            this.lblEp1.Text = "University:";
            // 
            // textEp1
            // 
            this.textEp1.Location = new System.Drawing.Point(120, 2);
            this.textEp1.Name = "textEp1";
            this.textEp1.Size = new System.Drawing.Size(620, 21);
            this.textEp1.TabIndex = 7;
            this.textEp1.TextChanged += new System.EventHandler(this.OnTextChanged);
            // 
            // btnEp1
            // 
            this.btnEp1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEp1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEp1.Location = new System.Drawing.Point(746, 1);
            this.btnEp1.Name = "btnEp1";
            this.btnEp1.Size = new System.Drawing.Size(60, 23);
            this.btnEp1.TabIndex = 8;
            this.btnEp1.Text = "Select...";
            this.btnEp1.UseVisualStyleBackColor = true;
            this.btnEp1.Click += new System.EventHandler(this.OnSelectClicked);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(655, 313);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(74, 30);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblEp3
            // 
            this.lblEp3.AutoSize = true;
            this.lblEp3.Location = new System.Drawing.Point(6, 60);
            this.lblEp3.Name = "lblEp3";
            this.lblEp3.Size = new System.Drawing.Size(110, 15);
            this.lblEp3.TabIndex = 10;
            this.lblEp3.Text = "Open for Business:";
            // 
            // textEp3
            // 
            this.textEp3.Location = new System.Drawing.Point(120, 58);
            this.textEp3.Name = "textEp3";
            this.textEp3.Size = new System.Drawing.Size(620, 21);
            this.textEp3.TabIndex = 11;
            this.textEp3.TextChanged += new System.EventHandler(this.OnTextChanged);
            // 
            // btnEp3
            // 
            this.btnEp3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEp3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEp3.Location = new System.Drawing.Point(746, 57);
            this.btnEp3.Name = "btnEp3";
            this.btnEp3.Size = new System.Drawing.Size(60, 23);
            this.btnEp3.TabIndex = 12;
            this.btnEp3.Text = "Select...";
            this.btnEp3.UseVisualStyleBackColor = true;
            this.btnEp3.Click += new System.EventHandler(this.OnSelectClicked);
            // 
            // lblEp4
            // 
            this.lblEp4.AutoSize = true;
            this.lblEp4.Location = new System.Drawing.Point(6, 88);
            this.lblEp4.Name = "lblEp4";
            this.lblEp4.Size = new System.Drawing.Size(34, 15);
            this.lblEp4.TabIndex = 13;
            this.lblEp4.Text = "Pets:";
            // 
            // textEp4
            // 
            this.textEp4.Location = new System.Drawing.Point(120, 86);
            this.textEp4.Name = "textEp4";
            this.textEp4.Size = new System.Drawing.Size(620, 21);
            this.textEp4.TabIndex = 14;
            this.textEp4.TextChanged += new System.EventHandler(this.OnTextChanged);
            // 
            // btnEp4
            // 
            this.btnEp4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEp4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEp4.Location = new System.Drawing.Point(746, 85);
            this.btnEp4.Name = "btnEp4";
            this.btnEp4.Size = new System.Drawing.Size(60, 23);
            this.btnEp4.TabIndex = 15;
            this.btnEp4.Text = "Select...";
            this.btnEp4.UseVisualStyleBackColor = true;
            this.btnEp4.Click += new System.EventHandler(this.OnSelectClicked);
            // 
            // lblEp5
            // 
            this.lblEp5.AutoSize = true;
            this.lblEp5.Location = new System.Drawing.Point(6, 116);
            this.lblEp5.Name = "lblEp5";
            this.lblEp5.Size = new System.Drawing.Size(58, 15);
            this.lblEp5.TabIndex = 16;
            this.lblEp5.Text = "Seasons:";
            // 
            // textEp5
            // 
            this.textEp5.Location = new System.Drawing.Point(120, 114);
            this.textEp5.Name = "textEp5";
            this.textEp5.Size = new System.Drawing.Size(620, 21);
            this.textEp5.TabIndex = 17;
            this.textEp5.TextChanged += new System.EventHandler(this.OnTextChanged);
            // 
            // btnEp5
            // 
            this.btnEp5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEp5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEp5.Location = new System.Drawing.Point(746, 113);
            this.btnEp5.Name = "btnEp5";
            this.btnEp5.Size = new System.Drawing.Size(60, 23);
            this.btnEp5.TabIndex = 18;
            this.btnEp5.Text = "Select...";
            this.btnEp5.UseVisualStyleBackColor = true;
            this.btnEp5.Click += new System.EventHandler(this.OnSelectClicked);
            // 
            // lblEp6
            // 
            this.lblEp6.AutoSize = true;
            this.lblEp6.Location = new System.Drawing.Point(6, 144);
            this.lblEp6.Name = "lblEp6";
            this.lblEp6.Size = new System.Drawing.Size(75, 15);
            this.lblEp6.TabIndex = 19;
            this.lblEp6.Text = "Bon Voyage:";
            // 
            // textEp6
            // 
            this.textEp6.Location = new System.Drawing.Point(120, 142);
            this.textEp6.Name = "textEp6";
            this.textEp6.Size = new System.Drawing.Size(620, 21);
            this.textEp6.TabIndex = 20;
            this.textEp6.TextChanged += new System.EventHandler(this.OnTextChanged);
            // 
            // btnEp6
            // 
            this.btnEp6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEp6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEp6.Location = new System.Drawing.Point(746, 141);
            this.btnEp6.Name = "btnEp6";
            this.btnEp6.Size = new System.Drawing.Size(60, 23);
            this.btnEp6.TabIndex = 21;
            this.btnEp6.Text = "Select...";
            this.btnEp6.UseVisualStyleBackColor = true;
            this.btnEp6.Click += new System.EventHandler(this.OnSelectClicked);
            // 
            // lblEp7
            // 
            this.lblEp7.AutoSize = true;
            this.lblEp7.Location = new System.Drawing.Point(6, 172);
            this.lblEp7.Name = "lblEp7";
            this.lblEp7.Size = new System.Drawing.Size(66, 15);
            this.lblEp7.TabIndex = 22;
            this.lblEp7.Text = "Free Time:";
            // 
            // textEp7
            // 
            this.textEp7.Location = new System.Drawing.Point(120, 170);
            this.textEp7.Name = "textEp7";
            this.textEp7.Size = new System.Drawing.Size(620, 21);
            this.textEp7.TabIndex = 23;
            this.textEp7.TextChanged += new System.EventHandler(this.OnTextChanged);
            // 
            // btnEp7
            // 
            this.btnEp7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEp7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEp7.Location = new System.Drawing.Point(746, 169);
            this.btnEp7.Name = "btnEp7";
            this.btnEp7.Size = new System.Drawing.Size(60, 23);
            this.btnEp7.TabIndex = 24;
            this.btnEp7.Text = "Select...";
            this.btnEp7.UseVisualStyleBackColor = true;
            this.btnEp7.Click += new System.EventHandler(this.OnSelectClicked);
            // 
            // lblEp8
            // 
            this.lblEp8.AutoSize = true;
            this.lblEp8.Location = new System.Drawing.Point(6, 200);
            this.lblEp8.Name = "lblEp8";
            this.lblEp8.Size = new System.Drawing.Size(89, 15);
            this.lblEp8.TabIndex = 25;
            this.lblEp8.Text = "Apartment Life:";
            // 
            // textEp8
            // 
            this.textEp8.Location = new System.Drawing.Point(120, 198);
            this.textEp8.Name = "textEp8";
            this.textEp8.Size = new System.Drawing.Size(620, 21);
            this.textEp8.TabIndex = 26;
            this.textEp8.TextChanged += new System.EventHandler(this.OnTextChanged);
            // 
            // btnEp8
            // 
            this.btnEp8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEp8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEp8.Location = new System.Drawing.Point(746, 197);
            this.btnEp8.Name = "btnEp8";
            this.btnEp8.Size = new System.Drawing.Size(60, 23);
            this.btnEp8.TabIndex = 27;
            this.btnEp8.Text = "Select...";
            this.btnEp8.UseVisualStyleBackColor = true;
            this.btnEp8.Click += new System.EventHandler(this.OnSelectClicked);
            // 
            // lblEp9
            // 
            this.lblEp9.AutoSize = true;
            this.lblEp9.Location = new System.Drawing.Point(6, 228);
            this.lblEp9.Name = "lblEp9";
            this.lblEp9.Size = new System.Drawing.Size(113, 15);
            this.lblEp9.TabIndex = 28;
            this.lblEp9.Text = "Mansion && Garden:";
            // 
            // textEp9
            // 
            this.textEp9.Location = new System.Drawing.Point(120, 226);
            this.textEp9.Name = "textEp9";
            this.textEp9.Size = new System.Drawing.Size(620, 21);
            this.textEp9.TabIndex = 29;
            this.textEp9.TextChanged += new System.EventHandler(this.OnTextChanged);
            // 
            // btnEp9
            // 
            this.btnEp9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEp9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEp9.Location = new System.Drawing.Point(746, 225);
            this.btnEp9.Name = "btnEp9";
            this.btnEp9.Size = new System.Drawing.Size(60, 23);
            this.btnEp9.TabIndex = 30;
            this.btnEp9.Text = "Select...";
            this.btnEp9.UseVisualStyleBackColor = true;
            this.btnEp9.Click += new System.EventHandler(this.OnSelectClicked);
            // 
            // tabControl
            // 
            this.tabControl.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tabControl.Controls.Add(this.tabEP);
            this.tabControl.Controls.Add(this.tabSP);
            this.tabControl.Location = new System.Drawing.Point(0, 34);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(820, 298);
            this.tabControl.TabIndex = 31;
            // 
            // tabSP
            // 
            this.tabSP.Controls.Add(this.lblSp1);
            this.tabSP.Controls.Add(this.btnSp1);
            this.tabSP.Controls.Add(this.textSp1);
            this.tabSP.Controls.Add(this.btnSp2);
            this.tabSP.Controls.Add(this.lblSp8);
            this.tabSP.Controls.Add(this.textSp2);
            this.tabSP.Controls.Add(this.textSp8);
            this.tabSP.Controls.Add(this.lblSp2);
            this.tabSP.Controls.Add(this.btnSp8);
            this.tabSP.Controls.Add(this.btnSp3);
            this.tabSP.Controls.Add(this.lblSp7);
            this.tabSP.Controls.Add(this.textSp3);
            this.tabSP.Controls.Add(this.textSp7);
            this.tabSP.Controls.Add(this.lblSp3);
            this.tabSP.Controls.Add(this.btnSp7);
            this.tabSP.Controls.Add(this.btnSp4);
            this.tabSP.Controls.Add(this.lblSp6);
            this.tabSP.Controls.Add(this.textSp4);
            this.tabSP.Controls.Add(this.textSp6);
            this.tabSP.Controls.Add(this.lblSp4);
            this.tabSP.Controls.Add(this.btnSp6);
            this.tabSP.Controls.Add(this.btnSp5);
            this.tabSP.Controls.Add(this.lblSp5);
            this.tabSP.Controls.Add(this.textSp5);
            this.tabSP.Location = new System.Drawing.Point(4, 4);
            this.tabSP.Name = "tabSP";
            this.tabSP.Padding = new System.Windows.Forms.Padding(3);
            this.tabSP.Size = new System.Drawing.Size(812, 270);
            this.tabSP.TabIndex = 1;
            this.tabSP.Text = "Stuff Packs";
            this.tabSP.UseVisualStyleBackColor = true;
            // 
            // lblSp1
            // 
            this.lblSp1.AutoSize = true;
            this.lblSp1.Location = new System.Drawing.Point(6, 4);
            this.lblSp1.Name = "lblSp1";
            this.lblSp1.Size = new System.Drawing.Size(70, 15);
            this.lblSp1.TabIndex = 31;
            this.lblSp1.Text = "Family Fun:";
            // 
            // btnSp1
            // 
            this.btnSp1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSp1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSp1.Location = new System.Drawing.Point(746, 1);
            this.btnSp1.Name = "btnSp1";
            this.btnSp1.Size = new System.Drawing.Size(60, 23);
            this.btnSp1.TabIndex = 33;
            this.btnSp1.Text = "Select...";
            this.btnSp1.UseVisualStyleBackColor = true;
            this.btnSp1.Click += new System.EventHandler(this.OnSelectClicked);
            // 
            // textSp1
            // 
            this.textSp1.Location = new System.Drawing.Point(120, 2);
            this.textSp1.Name = "textSp1";
            this.textSp1.Size = new System.Drawing.Size(620, 21);
            this.textSp1.TabIndex = 32;
            this.textSp1.TextChanged += new System.EventHandler(this.OnTextChanged);
            // 
            // btnSp2
            // 
            this.btnSp2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSp2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSp2.Location = new System.Drawing.Point(746, 29);
            this.btnSp2.Name = "btnSp2";
            this.btnSp2.Size = new System.Drawing.Size(60, 23);
            this.btnSp2.TabIndex = 30;
            this.btnSp2.Text = "Select...";
            this.btnSp2.UseVisualStyleBackColor = true;
            this.btnSp2.Click += new System.EventHandler(this.OnSelectClicked);
            // 
            // lblSp8
            // 
            this.lblSp8.AutoSize = true;
            this.lblSp8.Location = new System.Drawing.Point(6, 200);
            this.lblSp8.Name = "lblSp8";
            this.lblSp8.Size = new System.Drawing.Size(73, 15);
            this.lblSp8.TabIndex = 49;
            this.lblSp8.Text = "IKEA Home:";
            // 
            // textSp2
            // 
            this.textSp2.Location = new System.Drawing.Point(120, 30);
            this.textSp2.Name = "textSp2";
            this.textSp2.Size = new System.Drawing.Size(620, 21);
            this.textSp2.TabIndex = 29;
            this.textSp2.TextChanged += new System.EventHandler(this.OnTextChanged);
            // 
            // textSp8
            // 
            this.textSp8.Location = new System.Drawing.Point(120, 198);
            this.textSp8.Name = "textSp8";
            this.textSp8.Size = new System.Drawing.Size(620, 21);
            this.textSp8.TabIndex = 50;
            this.textSp8.TextChanged += new System.EventHandler(this.OnTextChanged);
            // 
            // lblSp2
            // 
            this.lblSp2.AutoSize = true;
            this.lblSp2.Location = new System.Drawing.Point(6, 32);
            this.lblSp2.Name = "lblSp2";
            this.lblSp2.Size = new System.Drawing.Size(81, 15);
            this.lblSp2.TabIndex = 28;
            this.lblSp2.Text = "Glamour Life:";
            // 
            // btnSp8
            // 
            this.btnSp8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSp8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSp8.Location = new System.Drawing.Point(746, 197);
            this.btnSp8.Name = "btnSp8";
            this.btnSp8.Size = new System.Drawing.Size(60, 23);
            this.btnSp8.TabIndex = 51;
            this.btnSp8.Text = "Select...";
            this.btnSp8.UseVisualStyleBackColor = true;
            this.btnSp8.Click += new System.EventHandler(this.OnSelectClicked);
            // 
            // btnSp3
            // 
            this.btnSp3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSp3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSp3.Location = new System.Drawing.Point(746, 57);
            this.btnSp3.Name = "btnSp3";
            this.btnSp3.Size = new System.Drawing.Size(60, 23);
            this.btnSp3.TabIndex = 36;
            this.btnSp3.Text = "Select...";
            this.btnSp3.UseVisualStyleBackColor = true;
            this.btnSp3.Click += new System.EventHandler(this.OnSelectClicked);
            // 
            // lblSp7
            // 
            this.lblSp7.AutoSize = true;
            this.lblSp7.Location = new System.Drawing.Point(6, 172);
            this.lblSp7.Name = "lblSp7";
            this.lblSp7.Size = new System.Drawing.Size(90, 15);
            this.lblSp7.TabIndex = 46;
            this.lblSp7.Text = "Kitchen && Bath:";
            // 
            // textSp3
            // 
            this.textSp3.Location = new System.Drawing.Point(120, 58);
            this.textSp3.Name = "textSp3";
            this.textSp3.Size = new System.Drawing.Size(620, 21);
            this.textSp3.TabIndex = 35;
            this.textSp3.TextChanged += new System.EventHandler(this.OnTextChanged);
            // 
            // textSp7
            // 
            this.textSp7.Location = new System.Drawing.Point(120, 170);
            this.textSp7.Name = "textSp7";
            this.textSp7.Size = new System.Drawing.Size(620, 21);
            this.textSp7.TabIndex = 47;
            this.textSp7.TextChanged += new System.EventHandler(this.OnTextChanged);
            // 
            // lblSp3
            // 
            this.lblSp3.AutoSize = true;
            this.lblSp3.Location = new System.Drawing.Point(6, 60);
            this.lblSp3.Name = "lblSp3";
            this.lblSp3.Size = new System.Drawing.Size(89, 15);
            this.lblSp3.TabIndex = 34;
            this.lblSp3.Text = "Happy Holiday:";
            // 
            // btnSp7
            // 
            this.btnSp7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSp7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSp7.Location = new System.Drawing.Point(746, 169);
            this.btnSp7.Name = "btnSp7";
            this.btnSp7.Size = new System.Drawing.Size(60, 23);
            this.btnSp7.TabIndex = 48;
            this.btnSp7.Text = "Select...";
            this.btnSp7.UseVisualStyleBackColor = true;
            this.btnSp7.Click += new System.EventHandler(this.OnSelectClicked);
            // 
            // btnSp4
            // 
            this.btnSp4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSp4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSp4.Location = new System.Drawing.Point(746, 85);
            this.btnSp4.Name = "btnSp4";
            this.btnSp4.Size = new System.Drawing.Size(60, 23);
            this.btnSp4.TabIndex = 39;
            this.btnSp4.Text = "Select...";
            this.btnSp4.UseVisualStyleBackColor = true;
            this.btnSp4.Click += new System.EventHandler(this.OnSelectClicked);
            // 
            // lblSp6
            // 
            this.lblSp6.AutoSize = true;
            this.lblSp6.Location = new System.Drawing.Point(6, 144);
            this.lblSp6.Name = "lblSp6";
            this.lblSp6.Size = new System.Drawing.Size(67, 15);
            this.lblSp6.TabIndex = 43;
            this.lblSp6.Text = "Teen Style:";
            // 
            // textSp4
            // 
            this.textSp4.Location = new System.Drawing.Point(120, 86);
            this.textSp4.Name = "textSp4";
            this.textSp4.Size = new System.Drawing.Size(620, 21);
            this.textSp4.TabIndex = 38;
            this.textSp4.TextChanged += new System.EventHandler(this.OnTextChanged);
            // 
            // textSp6
            // 
            this.textSp6.Location = new System.Drawing.Point(120, 142);
            this.textSp6.Name = "textSp6";
            this.textSp6.Size = new System.Drawing.Size(620, 21);
            this.textSp6.TabIndex = 44;
            this.textSp6.TextChanged += new System.EventHandler(this.OnTextChanged);
            // 
            // lblSp4
            // 
            this.lblSp4.AutoSize = true;
            this.lblSp4.Location = new System.Drawing.Point(6, 88);
            this.lblSp4.Name = "lblSp4";
            this.lblSp4.Size = new System.Drawing.Size(73, 15);
            this.lblSp4.TabIndex = 37;
            this.lblSp4.Text = "Celebration!";
            // 
            // btnSp6
            // 
            this.btnSp6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSp6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSp6.Location = new System.Drawing.Point(746, 141);
            this.btnSp6.Name = "btnSp6";
            this.btnSp6.Size = new System.Drawing.Size(60, 23);
            this.btnSp6.TabIndex = 45;
            this.btnSp6.Text = "Select...";
            this.btnSp6.UseVisualStyleBackColor = true;
            this.btnSp6.Click += new System.EventHandler(this.OnSelectClicked);
            // 
            // btnSp5
            // 
            this.btnSp5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSp5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSp5.Location = new System.Drawing.Point(746, 113);
            this.btnSp5.Name = "btnSp5";
            this.btnSp5.Size = new System.Drawing.Size(60, 23);
            this.btnSp5.TabIndex = 42;
            this.btnSp5.Text = "Select...";
            this.btnSp5.UseVisualStyleBackColor = true;
            this.btnSp5.Click += new System.EventHandler(this.OnSelectClicked);
            // 
            // lblSp5
            // 
            this.lblSp5.AutoSize = true;
            this.lblSp5.Location = new System.Drawing.Point(6, 116);
            this.lblSp5.Name = "lblSp5";
            this.lblSp5.Size = new System.Drawing.Size(85, 15);
            this.lblSp5.TabIndex = 40;
            this.lblSp5.Text = "H&&M Fashion:";
            // 
            // textSp5
            // 
            this.textSp5.Location = new System.Drawing.Point(120, 114);
            this.textSp5.Name = "textSp5";
            this.textSp5.Size = new System.Drawing.Size(620, 21);
            this.textSp5.TabIndex = 41;
            this.textSp5.TextChanged += new System.EventHandler(this.OnTextChanged);
            // 
            // tabEP
            // 
            this.tabEP.Controls.Add(this.lblEp1);
            this.tabEP.Controls.Add(this.lblEp9);
            this.tabEP.Controls.Add(this.btnEp1);
            this.tabEP.Controls.Add(this.textEp9);
            this.tabEP.Controls.Add(this.textEp1);
            this.tabEP.Controls.Add(this.btnEp9);
            this.tabEP.Controls.Add(this.btnEp2);
            this.tabEP.Controls.Add(this.lblEp8);
            this.tabEP.Controls.Add(this.textEp2);
            this.tabEP.Controls.Add(this.textEp8);
            this.tabEP.Controls.Add(this.lblEp2);
            this.tabEP.Controls.Add(this.btnEp8);
            this.tabEP.Controls.Add(this.btnEp3);
            this.tabEP.Controls.Add(this.lblEp7);
            this.tabEP.Controls.Add(this.textEp3);
            this.tabEP.Controls.Add(this.textEp7);
            this.tabEP.Controls.Add(this.lblEp3);
            this.tabEP.Controls.Add(this.btnEp7);
            this.tabEP.Controls.Add(this.btnEp4);
            this.tabEP.Controls.Add(this.lblEp6);
            this.tabEP.Controls.Add(this.textEp4);
            this.tabEP.Controls.Add(this.textEp6);
            this.tabEP.Controls.Add(this.lblEp4);
            this.tabEP.Controls.Add(this.btnEp6);
            this.tabEP.Controls.Add(this.btnEp5);
            this.tabEP.Controls.Add(this.lblEp5);
            this.tabEP.Controls.Add(this.textEp5);
            this.tabEP.Location = new System.Drawing.Point(4, 4);
            this.tabEP.Name = "tabEP";
            this.tabEP.Padding = new System.Windows.Forms.Padding(3);
            this.tabEP.Size = new System.Drawing.Size(812, 270);
            this.tabEP.TabIndex = 0;
            this.tabEP.Text = "Expansion Packs";
            this.tabEP.UseVisualStyleBackColor = true;
            // 
            // EpSpConfigDialog
            // 
            this.AcceptButton = this.btnConfigOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(820, 344);
            this.Controls.Add(this.btnConfigOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblBase);
            this.Controls.Add(this.textBase);
            this.Controls.Add(this.btnBase);
            this.Controls.Add(this.tabControl);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EpSpConfigDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "EP & SP Configuration";
            this.Load += new System.EventHandler(this.OnConfigLoad);
            this.tabControl.ResumeLayout(false);
            this.tabSP.ResumeLayout(false);
            this.tabSP.PerformLayout();
            this.tabEP.ResumeLayout(false);
            this.tabEP.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblBase;
        private System.Windows.Forms.TextBox textBase;
        private System.Windows.Forms.Button btnBase;
        private System.Windows.Forms.Label lblEp2;
        private System.Windows.Forms.TextBox textEp2;
        private System.Windows.Forms.Button btnEp2;
        private System.Windows.Forms.Button btnConfigOK;
        private CommonOpenFileDialog selectPathDialog;
        private System.Windows.Forms.Label lblEp1;
        private System.Windows.Forms.TextBox textEp1;
        private System.Windows.Forms.Button btnEp1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblEp3;
        private System.Windows.Forms.TextBox textEp3;
        private System.Windows.Forms.Button btnEp3;
        private System.Windows.Forms.Label lblEp4;
        private System.Windows.Forms.TextBox textEp4;
        private System.Windows.Forms.Button btnEp4;
        private System.Windows.Forms.Label lblEp5;
        private System.Windows.Forms.TextBox textEp5;
        private System.Windows.Forms.Button btnEp5;
        private System.Windows.Forms.Label lblEp6;
        private System.Windows.Forms.TextBox textEp6;
        private System.Windows.Forms.Button btnEp6;
        private System.Windows.Forms.Label lblEp7;
        private System.Windows.Forms.TextBox textEp7;
        private System.Windows.Forms.Button btnEp7;
        private System.Windows.Forms.Label lblEp8;
        private System.Windows.Forms.TextBox textEp8;
        private System.Windows.Forms.Button btnEp8;
        private System.Windows.Forms.Label lblEp9;
        private System.Windows.Forms.TextBox textEp9;
        private System.Windows.Forms.Button btnEp9;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabEP;
        private System.Windows.Forms.TabPage tabSP;
        private System.Windows.Forms.Label lblSp1;
        private System.Windows.Forms.Button btnSp1;
        private System.Windows.Forms.TextBox textSp1;
        private System.Windows.Forms.Button btnSp2;
        private System.Windows.Forms.Label lblSp8;
        private System.Windows.Forms.TextBox textSp2;
        private System.Windows.Forms.TextBox textSp8;
        private System.Windows.Forms.Label lblSp2;
        private System.Windows.Forms.Button btnSp8;
        private System.Windows.Forms.Button btnSp3;
        private System.Windows.Forms.Label lblSp7;
        private System.Windows.Forms.TextBox textSp3;
        private System.Windows.Forms.TextBox textSp7;
        private System.Windows.Forms.Label lblSp3;
        private System.Windows.Forms.Button btnSp7;
        private System.Windows.Forms.Button btnSp4;
        private System.Windows.Forms.Label lblSp6;
        private System.Windows.Forms.TextBox textSp4;
        private System.Windows.Forms.TextBox textSp6;
        private System.Windows.Forms.Label lblSp4;
        private System.Windows.Forms.Button btnSp6;
        private System.Windows.Forms.Button btnSp5;
        private System.Windows.Forms.Label lblSp5;
        private System.Windows.Forms.TextBox textSp5;
    }
}