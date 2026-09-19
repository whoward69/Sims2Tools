/*
 * Collection Manager - a utility for managing Sims 2 collections
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System.Drawing;
using System.Windows.Forms;

namespace CollectionManager.Controls
{
    [System.ComponentModel.DesignerCategory("")]
    public class CollectionViewerTab : TabPage
    {
        private static readonly Sims2Tools.DBPF.Logger.IDBPFLogger logger = Sims2Tools.DBPF.Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly CollectionViewer collectionViewer;

        public CollectionViewer CollectionViewer => collectionViewer;

        public string CollectionFilePath
        {
            get => collectionViewer.CollectionFilePath;
            set { collectionViewer.CollectionFilePath = value; this.Text = collectionViewer.TabName; }
        }

        public CollectionViewerTab(string collectionFilePath)
        {
            collectionViewer = new CollectionViewer()
            {
                Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right),
                Location = new Point(3, 3),
                Size = new Size(450, 200),
                TabIndex = 0,

                CollectionFilePath = collectionFilePath
            };

            this.Location = new Point(4, 24);
            this.Padding = new Padding(3);
            this.Size = new Size(456, 206);
            this.TabIndex = 0;
            this.Text = collectionViewer.TabName;
            this.UseVisualStyleBackColor = true;

            this.Controls.Add(collectionViewer);
        }
    }
}
