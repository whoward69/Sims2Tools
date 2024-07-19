/*
 * SceneGraph Plus - a utility for repairing scene graphs
 *
 * William Howard - 2020-2024
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph.LIFO;
using Sims2Tools.DBPF.SceneGraph.TXTR;
using Sims2Tools.Dialogs;
using Sims2Tools.Utils.Persistence;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace SceneGraphPlus.Dialogs
{
    public partial class TextureDialog : Form
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Image texture = null;

        private string nextTexturePath = null;
        private DBPFKey nextTextureKey = null;
        private string nextTextureTitle = null;

        private string currentTexturePath;
        private DBPFKey currentTextureKey;
        private string currentTextureTitle;

        public TextureDialog()
        {
            InitializeComponent();

            pictHover.Visible = false;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            RegistryTools.LoadPopupSettings(SceneGraphPlusApp.RegistryKey, this);

            menuItemContextAutoZoom.Checked = ((int)RegistryTools.GetSetting(SceneGraphPlusApp.RegistryKey + @"\Texture", menuItemContextAutoZoom.Name, 1) != 0);

            menuItemContextAutoSize.Checked = ((int)RegistryTools.GetSetting(SceneGraphPlusApp.RegistryKey + @"\Texture", menuItemContextAutoSize.Name, 0) != 0);
            menuItemContext128.Checked = ((int)RegistryTools.GetSetting(SceneGraphPlusApp.RegistryKey + @"\Texture", menuItemContext128.Name, 0) != 0);
            menuItemContext256.Checked = ((int)RegistryTools.GetSetting(SceneGraphPlusApp.RegistryKey + @"\Texture", menuItemContext256.Name, 0) != 0);
            menuItemContext512.Checked = ((int)RegistryTools.GetSetting(SceneGraphPlusApp.RegistryKey + @"\Texture", menuItemContext512.Name, 1) != 0);

            SetWindowSize();
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            RegistryTools.SavePopupSettings(SceneGraphPlusApp.RegistryKey, this);

            RegistryTools.SaveSetting(SceneGraphPlusApp.RegistryKey + @"\Texture", menuItemContextAutoZoom.Name, menuItemContextAutoZoom.Checked ? 1 : 0);

            RegistryTools.SaveSetting(SceneGraphPlusApp.RegistryKey + @"\Texture", menuItemContextAutoSize.Name, menuItemContextAutoSize.Checked ? 1 : 0);
            RegistryTools.SaveSetting(SceneGraphPlusApp.RegistryKey + @"\Texture", menuItemContext128.Name, menuItemContext128.Checked ? 1 : 0);
            RegistryTools.SaveSetting(SceneGraphPlusApp.RegistryKey + @"\Texture", menuItemContext256.Name, menuItemContext256.Checked ? 1 : 0);
            RegistryTools.SaveSetting(SceneGraphPlusApp.RegistryKey + @"\Texture", menuItemContext512.Name, menuItemContext512.Checked ? 1 : 0);
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

        public void ClearTexture()
        {
            nextTexturePath = null;
            nextTextureKey = new DBPFKey(Txtr.TYPE, DBPFData.GROUP_LOCAL, DBPFData.INSTANCE_NULL, DBPFData.RESOURCE_NULL);
            nextTextureTitle = "";

            DisplayNextTexture();
        }

        public void SetTextureFromKey(string packagePath, DBPFKey key, string textureName)
        {
            nextTexturePath = packagePath;
            nextTextureKey = key;
            nextTextureTitle = textureName;

            DisplayNextTexture();
        }

        private void DisplayNextTexture()
        {
            if (textureWorker.IsBusy)
            {
                // Cancel any current load and start the next one
                Debug.Assert(textureWorker.WorkerSupportsCancellation == true);

                // Cancel the asynchronous operation.
                textureWorker.CancelAsync();
            }
            else
            {
                currentTexturePath = nextTexturePath;
                currentTextureKey = nextTextureKey;
                currentTextureTitle = nextTextureTitle;

                nextTextureKey = null;

                textureWorker.RunWorkerAsync();
            }
        }

        private void SetTexture(string name, Image texture)
        {
            this.texture = texture;

            if (texture != null)
            {
                this.Text = $"{name} {texture.Width}x{texture.Height}";
            }
            else
            {
                this.Text = "";
            }

            pictTexture.Image = texture;

            SetWindowSize();

            if (texture != null)
            {
                if (menuItemContextAutoSize.Checked)
                {
                    SetWindowSize(texture.Width, texture.Height);
                }
                else
                {
                    AdjustWindowSize();
                }
            }
        }

        private void OnTextureSizeClicked(object sender, EventArgs e)
        {
            if (sender == menuItemContext128)
            {
                menuItemContext256.Checked = false;
                menuItemContext512.Checked = false;
                menuItemContextAutoSize.Checked = !menuItemContext128.Checked;
            }
            else if (sender == menuItemContext256)
            {
                menuItemContext128.Checked = false;
                menuItemContext512.Checked = false;
                menuItemContextAutoSize.Checked = !menuItemContext256.Checked;
            }
            else if (sender == menuItemContext512)
            {
                menuItemContext128.Checked = false;
                menuItemContext256.Checked = false;
                menuItemContextAutoSize.Checked = !menuItemContext512.Checked;
            }
            else
            {
                menuItemContext128.Checked = false;
                menuItemContext256.Checked = false;
                menuItemContext512.Checked = !menuItemContextAutoSize.Checked;
            }

            SetWindowSize();

            if (!menuItemContextAutoSize.Checked)
            {
                AdjustWindowSize();
            }
        }

        private void SetWindowSize()
        {
            int width, height;

            if (menuItemContext128.Checked)
            {
                width = height = 128;
            }
            else if (menuItemContext256.Checked)
            {
                width = height = 256;
            }
            else if (menuItemContext512.Checked)
            {
                width = height = 512;
            }
            else
            {
                if (texture != null)
                {
                    width = texture.Width;
                    height = texture.Height;
                }
                else
                {
                    width = height = 512;
                }
            }

            SetWindowSize(width, height);
        }

        private void AdjustWindowSize()
        {
            if (texture != null)
            {
                if (texture.Width != texture.Height)
                {
                    if (texture.Width > texture.Height)
                    {
                        int height = texture.Height * (this.Height - 39) / texture.Width;
                        SetWindowSize(this.Width - 16, height);
                    }
                    else
                    {
                        int width = texture.Width * (this.Width - 16) / texture.Height;
                        SetWindowSize(width, this.Height - 39);
                    }
                }
            }
        }

        private void SetWindowSize(int width, int height)
        {
            this.Width = width + 16;
            this.Height = height + 39;
        }

        private void OnContextMenuOpening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            menuItemContextAutoSize.Enabled = !menuItemContextAutoSize.Checked;
            menuItemContext128.Enabled = !menuItemContext128.Checked;
            menuItemContext256.Enabled = !menuItemContext256.Checked;
            menuItemContext512.Enabled = !menuItemContext512.Checked;
        }

        private void OnMouseLeaveTexture(object sender, EventArgs e)
        {
            pictHover.Visible = false;
        }

        private void OnMouseMoveOverTexture(object sender, MouseEventArgs e)
        {
            if (menuItemContextAutoZoom.Checked)
            {
                ShowHoverImage(e.Location);
                MoveHoverImage(e.Location);
            }
        }

        private void OnTextureClicked_DEBUG(object sender, EventArgs e)
        {
#if DEBUG
            ShowHoverImage((e as MouseEventArgs).Location);
#endif
        }

        private void ShowHoverImage(Point mouse)
        {
            if (texture != null)
            {
                // Calculate squashed (< 1.0) or stretched (> 1.0) ratio of original texture into texture box
                float ratioX = pictTexture.Width / (float)texture.Width;
                float ratioY = pictTexture.Height / (float)texture.Height;
                float ratio = Math.Min(ratioX, ratioY);

                int snippetSize;

                if (ratio < 1.0)
                {
                    snippetSize = Math.Min(64, (int)(64 / ratio));
                }
                else
                {
                    snippetSize = Math.Max(16, (int)(64 / ratio / 2));
                }

                int snippetWidth = Math.Min(texture.Width, snippetSize);
                int snippetHeight = Math.Min(texture.Height, snippetSize);

                int snippetX = Math.Max((int)(mouse.X / ratioX), snippetWidth / 2) - (snippetWidth / 2);
                int snippetY = Math.Max((int)(mouse.Y / ratioY), snippetHeight / 2) - (snippetHeight / 2);

                snippetX = Math.Min(snippetX, texture.Width - snippetWidth);
                snippetY = Math.Min(snippetY, texture.Height - snippetHeight);

                Bitmap bm = new Bitmap(texture);
                pictHover.Image = bm.Clone(new Rectangle(snippetX, snippetY, snippetWidth, snippetHeight), bm.PixelFormat);

                pictHover.Visible = true;
            }
        }

        private void MoveHoverImage(Point hoverLocation)
        {
            hoverLocation.X += 16;
            hoverLocation.Y += 16;

            hoverLocation.X = Math.Min(hoverLocation.X, pictTexture.Width - 64);
            hoverLocation.Y = Math.Min(hoverLocation.Y, pictTexture.Height - 64);

            pictHover.Location = hoverLocation;
        }

        private void TextureWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            Image texture = null;

            if (currentTexturePath != null)
            {
                using (DBPFFile package = new DBPFFile(currentTexturePath))
                {
                    DBPFKey key = currentTextureKey;

                    if (key.TypeID == Txtr.TYPE)
                    {
                        Txtr txtr = (Txtr)package.GetResourceByKey(key);

                        if (!worker.CancellationPending)
                        {
                            worker.ReportProgress(50);

                            texture = txtr?.ImageData?.LargestTexture?.Texture;
                        }
                    }
                    else
                    {
                        Lifo lifo = (Lifo)package.GetResourceByKey(key);

                        if (!worker.CancellationPending)
                        {
                            worker.ReportProgress(50);

                            texture = lifo?.LevelInfo?.Texture;
                        }
                    }

                    package.Close();
                }
            }

            e.Result = texture;
        }

        private void TextureWorker_Progress(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage != 0)
            {
                // Display the progress
            }
        }

        private void TextureWorker_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                logger.Error(e.Error.Message);
                logger.Info(e.Error.StackTrace);

                MsgBox.Show("An error occured while loading the texture", "Error!", MessageBoxButtons.OK);
            }
            else
            {
                if (e.Cancelled == true)
                {
                    // Load cancelled by another image being requested
                }
                else
                {
                    // Load completed before another image was requested
                    SetTexture(currentTextureTitle, e.Result as Image);
                    Visible = true;
                }
            }

            if (nextTextureKey != null)
            {
                DisplayNextTexture();
            }
        }
    }
}
