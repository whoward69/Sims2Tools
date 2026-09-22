/*
 * Collection Manager - a utility for managing Sims 2 collections
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.Dialogs;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CollectionManager.Controls
{
    public partial class CollectionViewerForm : Form
    {
        private static readonly Sims2Tools.DBPF.Logger.IDBPFLogger logger = Sims2Tools.DBPF.Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly CollectionManagerForm managerForm = null;
        internal CollectionManagerForm ManagerForm => managerForm;

        private readonly CollectionViewer collectionViewer;
        public CollectionViewer CollectionViewer => collectionViewer;

        private bool beingDocked = false;

        public CollectionViewerForm(CollectionManagerForm managerForm, string collectionFilePath)
        {
            this.managerForm = managerForm;

            InitializeComponent();

            collectionViewer = new CollectionViewer()
            {
                Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right),
                Location = new Point(0, 0),
                Size = new Size(panelForm.Width, panelForm.Height),
                TabIndex = 0,

                CollectionFilePath = collectionFilePath,
            };

            panelForm.Controls.Add(collectionViewer);

            this.Text = $"{CollectionManagerApp.AppTitle} - {collectionViewer.TabName}";
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (!beingDocked && collectionViewer.IsDirty)
            {
                if (MsgBox.Show($"There are unsaved changes, do you really want to close this window?", "Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            managerForm?.NotifyClosed(this);
        }

        #region Window Menu
        private void OnWindow_Opening(object sender, EventArgs e)
        {
            menuItemViewerSave.Enabled = collectionViewer.IsDirty;
            menuItemViewerSaveAll.Enabled = managerForm.IsAnyDirty;
        }

        private void OnWindow_Dock(object sender, System.EventArgs e)
        {
            collectionViewer.CommitNeededChanges();

            managerForm?.NotifyDock(this);

            beingDocked = true;
            this.Close();
        }

        private void OnWindow_New(object sender, EventArgs e)
        {
            managerForm.NewCollection(false);
        }

        private void OnWindow_Save(object sender, EventArgs e)
        {
            managerForm.SaveCollection(collectionViewer);
        }

        private void OnWindow_SaveAs(object sender, EventArgs e)
        {
            managerForm.SaveAsCollection(collectionViewer);
        }

        private void OnWindow_SaveAll(object sender, EventArgs e)
        {
            managerForm.SaveAllCollection();
        }

        private void OnWindow_Close(object sender, System.EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Collection Menu
        private void OnCollection_ChangeIcon(object sender, EventArgs e)
        {
            managerForm.ChangeIcon(collectionViewer);
        }

        private void OnCollection_RenamePackage(object sender, EventArgs e)
        {
            if (managerForm.RenameCollection(collectionViewer))
            {
                this.Text = $"{CollectionManagerApp.AppTitle} - {collectionViewer.TabName}";
            }
        }

        private void OnCollection_Delete(object sender, EventArgs e)
        {
            if (managerForm.DeleteCollection(collectionViewer))
            {
                this.Close();
            }
        }
        #endregion
    }
}
