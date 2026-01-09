/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Sims2Tools
{
    public partial class EpSpConfigDialog : Form
    {
        private readonly string installPath;

        public EpSpConfigDialog(string installPath)
        {
            InitializeComponent();

            selectPathDialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true
            };

            this.installPath = installPath;
        }

        private void OnConfigLoad(object sender, EventArgs e)
        {
            textBase.Text = ValidatePath(Sims2ToolsLib.Sims2BasePath);

            if (string.IsNullOrEmpty(textBase.Text))
            {
#pragma warning disable CS0612
                string basePath = SimpeData.PathSetting("Sims2Path");
#pragma warning restore CS0612

                if (!string.IsNullOrEmpty(ValidatePath(basePath)))
                {
                    textBase.Text = basePath;

#pragma warning disable CS0612
                    textEp1.Text = ValidatePath(SimpeData.PathSetting($"Sims2EP1Path"));
                    textEp2.Text = ValidatePath(SimpeData.PathSetting($"Sims2EP2Path"));
                    textEp3.Text = ValidatePath(SimpeData.PathSetting($"Sims2EP3Path"));
                    textEp4.Text = ValidatePath(SimpeData.PathSetting($"Sims2EP4Path"));
                    textEp5.Text = ValidatePath(SimpeData.PathSetting($"Sims2EP5Path"));
                    textEp6.Text = ValidatePath(SimpeData.PathSetting($"Sims2EP6Path"));
                    textEp7.Text = ValidatePath(SimpeData.PathSetting($"Sims2EP7Path"));
                    textEp8.Text = ValidatePath(SimpeData.PathSetting($"Sims2EP8Path"));
                    textEp9.Text = ValidatePath(SimpeData.PathSetting($"Sims2EP9Path"));

                    textSp1.Text = ValidatePath(SimpeData.PathSetting($"Sims2SP1Path"));
                    textSp2.Text = ValidatePath(SimpeData.PathSetting($"Sims2SP2Path"));
                    textSp3.Text = ValidatePath(SimpeData.PathSetting($"Sims2SCPath"));
                    textSp4.Text = ValidatePath(SimpeData.PathSetting($"Sims2SP4Path"));
                    textSp5.Text = ValidatePath(SimpeData.PathSetting($"Sims2SP5Path"));
                    textSp6.Text = ValidatePath(SimpeData.PathSetting($"Sims2SP6Path"));
                    textSp7.Text = ValidatePath(SimpeData.PathSetting($"Sims2SP7Path"));
                    textSp8.Text = ValidatePath(SimpeData.PathSetting($"Sims2SP8Path"));
#pragma warning restore CS0612
                }
                else
                {
                    if (installPath.EndsWith("Ultimate Collection"))
                    {
                        textBase.Text = ValidatePath($"{installPath}\\Double Deluxe\\Base");

                        textEp1.Text = ValidatePath($"{installPath}\\University Life\\EP1");
                        textEp2.Text = ValidatePath($"{installPath}\\Double Deluxe\\EP2");
                        textEp3.Text = ValidatePath($"{installPath}\\Best of Business\\EP3");
                        textEp4.Text = ValidatePath($"{installPath}\\Fun with Pets\\EP4");
                        textEp5.Text = ValidatePath($"{installPath}\\Seasons");
                        textEp6.Text = ValidatePath($"{installPath}\\Bon Voyage");
                        textEp7.Text = ValidatePath($"{installPath}\\Free Time");
                        textEp8.Text = ValidatePath($"{installPath}\\Apartment Life");
                        textEp9.Text = ValidatePath($"{installPath}\\Fun with Pets\\SP9");

                        textSp1.Text = ValidatePath($"{installPath}\\Fun with Pets\\SP1");
                        textSp2.Text = ValidatePath($"{installPath}\\Glamour Life Stuff");
                        textSp3.Text = ValidatePath($"");
                        textSp4.Text = ValidatePath($"{installPath}\\Double Deluxe\\SP4");
                        textSp5.Text = ValidatePath($"{installPath}\\Best of Business\\SP5");
                        textSp6.Text = ValidatePath($"{installPath}\\University Life\\SP6");
                        textSp7.Text = ValidatePath($"{installPath}\\Best of Business\\SP7");
                        textSp8.Text = ValidatePath($"{installPath}\\University Life\\SP8");
                    }
                    else if (installPath.EndsWith("Legacy Collection"))
                    {
                        textBase.Text = ValidatePath($"{installPath}\\Base");

                        textEp1.Text = ValidatePath($"{installPath}\\EP1");
                        textEp2.Text = ValidatePath($"{installPath}\\EP2");
                        textEp3.Text = ValidatePath($"{installPath}\\EP3");
                        textEp4.Text = ValidatePath($"{installPath}\\EP4");
                        textEp5.Text = ValidatePath($"{installPath}\\EP5");
                        textEp6.Text = ValidatePath($"{installPath}\\EP6");
                        textEp7.Text = ValidatePath($"{installPath}\\EP7");
                        textEp8.Text = ValidatePath($"{installPath}\\EP8");
                        textEp9.Text = ValidatePath($"{installPath}\\EP9");

                        textSp1.Text = ValidatePath($"{installPath}\\SP1");
                        textSp2.Text = ValidatePath($"{installPath}\\SP2");
                        textSp3.Text = ValidatePath($"");
                        textSp4.Text = ValidatePath($"{installPath}\\SP4");
                        textSp5.Text = ValidatePath($"{installPath}\\SP5");
                        textSp6.Text = ValidatePath($"{installPath}\\SP6");
                        textSp7.Text = ValidatePath($"{installPath}\\SP7");
                        textSp8.Text = ValidatePath($"");
                    }
                    else if (installPath.EndsWith("EA GAMES"))
                    {
                        textBase.Text = ValidatePath($"{installPath}\\The Sims 2");

                        if (string.IsNullOrEmpty(textBase.Text))
                        {
                            textBase.Text = ValidatePath($"{installPath}\\The Sims 2 Double Deluxe\\Base");

                            if (!string.IsNullOrEmpty(textBase.Text))
                            {
                                textEp2.Text = ValidatePath($"{installPath}\\The Sims 2 Double Deluxe\\EP2");
                                textSp4.Text = ValidatePath($"{installPath}\\The Sims 2 Double Deluxe\\SP4");
                            }
                        }

                        textEp1.Text = ValidatePath($"{installPath}\\The Sims 2 University");
                        if (string.IsNullOrEmpty(textEp2.Text)) textEp2.Text = ValidatePath($"{installPath}\\The Sims 2 Nightlife");
                        textEp3.Text = ValidatePath($"{installPath}\\The Sims 2 Open For Business");
                        textEp4.Text = ValidatePath($"{installPath}\\The Sims 2 Pets");
                        textEp5.Text = ValidatePath($"{installPath}\\The Sims 2 Seasons");
                        textEp6.Text = ValidatePath($"{installPath}\\The Sims 2 Bon Voyage");
                        textEp7.Text = ValidatePath($"{installPath}\\The Sims 2 Free Time");
                        textEp8.Text = ValidatePath($"{installPath}\\The Sims 2 Apartment Life");
                        textEp9.Text = ValidatePath($"{installPath}\\The Sims 2 Mansion and Garden Stuff");

                        textSp1.Text = ValidatePath($"{installPath}\\The Sims 2 Family Fun Stuff");
                        textSp2.Text = ValidatePath($"{installPath}\\The Sims 2 Glamour Life Stuff");
                        textSp3.Text = ValidatePath($"");
                        if (string.IsNullOrEmpty(textSp4.Text)) textSp4.Text = ValidatePath($"");
                        textSp5.Text = ValidatePath($"{installPath}\\The Sims 2 H&M® Fashion Stuff");
                        textSp6.Text = ValidatePath($"{installPath}\\The Sims 2 Teen Style Stuff");
                        textSp7.Text = ValidatePath($"{installPath}\\The Sims 2 Kitchen & Bath Interior Design Stuff");
                        textSp8.Text = ValidatePath($"{installPath}\\The Sims 2 IKEA® Home Stuff");
                    }
                }
            }
            else
            {
                textEp1.Text = ValidatePath(Sims2ToolsLib.Sims2EpPath(1));
                textEp2.Text = ValidatePath(Sims2ToolsLib.Sims2EpPath(2));
                textEp3.Text = ValidatePath(Sims2ToolsLib.Sims2EpPath(3));
                textEp4.Text = ValidatePath(Sims2ToolsLib.Sims2EpPath(4));
                textEp5.Text = ValidatePath(Sims2ToolsLib.Sims2EpPath(5));
                textEp6.Text = ValidatePath(Sims2ToolsLib.Sims2EpPath(6));
                textEp7.Text = ValidatePath(Sims2ToolsLib.Sims2EpPath(7));
                textEp8.Text = ValidatePath(Sims2ToolsLib.Sims2EpPath(8));
                textEp9.Text = ValidatePath(Sims2ToolsLib.Sims2EpPath(9));

                textSp1.Text = ValidatePath(Sims2ToolsLib.Sims2SpPath(1));
                textSp2.Text = ValidatePath(Sims2ToolsLib.Sims2SpPath(2));
                textSp3.Text = ValidatePath(Sims2ToolsLib.Sims2SpPath(3));
                textSp4.Text = ValidatePath(Sims2ToolsLib.Sims2SpPath(4));
                textSp5.Text = ValidatePath(Sims2ToolsLib.Sims2SpPath(5));
                textSp6.Text = ValidatePath(Sims2ToolsLib.Sims2SpPath(6));
                textSp7.Text = ValidatePath(Sims2ToolsLib.Sims2SpPath(7));
                textSp8.Text = ValidatePath(Sims2ToolsLib.Sims2SpPath(8));
            }

            UpdateFormState();
        }

        private string ValidatePath(string path)
        {
            return Directory.Exists(path) ? path : "";
        }

        private void UpdateFormState()
        {
            bool allOk = VerifyEpPath(textBase);

            allOk &= VerifyEpPath(textEp1);
            allOk &= VerifyEpPath(textEp2);
            allOk &= VerifyEpPath(textEp3);
            allOk &= VerifyEpPath(textEp4);
            allOk &= VerifyEpPath(textEp5);
            allOk &= VerifyEpPath(textEp6);
            allOk &= VerifyEpPath(textEp7);
            allOk &= VerifyEpPath(textEp8);
            allOk &= VerifyEpPath(textEp9);

            allOk &= VerifySpPath(textSp1);
            allOk &= VerifySpPath(textSp2);
            allOk &= VerifySpPath(textSp3);
            allOk &= VerifySpPath(textSp4);
            allOk &= VerifySpPath(textSp5);
            allOk &= VerifySpPath(textSp6);
            allOk &= VerifySpPath(textSp7);
            allOk &= VerifySpPath(textSp8);

            btnConfigOK.Enabled = allOk;
        }

        private bool VerifyEpPath(TextBox textBox)
        {
            bool ok = string.IsNullOrEmpty(textBox.Text) || File.Exists($"{textBox.Text}\\TSData\\Res\\Objects\\objects.package");
            textBox.BackColor = ok ? SystemColors.Window : Color.LightCoral;

            return ok;
        }

        private bool VerifySpPath(TextBox textBox)
        {
            bool ok = string.IsNullOrEmpty(textBox.Text) || File.Exists($"{textBox.Text}\\TSData\\Res\\Objects\\objects.package");
            textBox.BackColor = ok ? SystemColors.Window : Color.LightCoral;

            return ok;
        }

        private void OnSelectClicked(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                TextBox textBox = (TextBox)btn.Parent.Controls[btn.Parent.Controls.IndexOfKey($"text{btn.Name.Substring(3)}")];

                selectPathDialog.InitialDirectory = string.IsNullOrWhiteSpace(textBox.Text) ? installPath : textBox.Text;

                if (selectPathDialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    textBox.Text = selectPathDialog.FileName;
                }
            }
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            UpdateFormState();
        }

        private void OnConfigOkClicked(object sender, EventArgs e)
        {
            Sims2ToolsLib.Sims2BasePath = textBase.Text;

            Sims2ToolsLib.Sims2EpPath(1, textEp1.Text);
            Sims2ToolsLib.Sims2EpPath(2, textEp2.Text);
            Sims2ToolsLib.Sims2EpPath(3, textEp3.Text);
            Sims2ToolsLib.Sims2EpPath(4, textEp4.Text);
            Sims2ToolsLib.Sims2EpPath(5, textEp5.Text);
            Sims2ToolsLib.Sims2EpPath(6, textEp6.Text);
            Sims2ToolsLib.Sims2EpPath(7, textEp7.Text);
            Sims2ToolsLib.Sims2EpPath(8, textEp8.Text);
            Sims2ToolsLib.Sims2EpPath(9, textEp9.Text);

            Sims2ToolsLib.Sims2SpPath(1, textSp1.Text);
            Sims2ToolsLib.Sims2SpPath(2, textSp2.Text);
            Sims2ToolsLib.Sims2SpPath(3, textSp3.Text);
            Sims2ToolsLib.Sims2SpPath(4, textSp4.Text);
            Sims2ToolsLib.Sims2SpPath(5, textSp5.Text);
            Sims2ToolsLib.Sims2SpPath(6, textSp6.Text);
            Sims2ToolsLib.Sims2SpPath(7, textSp7.Text);
            Sims2ToolsLib.Sims2SpPath(8, textSp8.Text);

            this.Close();
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (Form.ModifierKeys == Keys.None && keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }

            return base.ProcessDialogKey(keyData);
        }
    }
}
