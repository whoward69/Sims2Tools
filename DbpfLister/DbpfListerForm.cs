using Sims2Tools.DBPF;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph.BINX;
using Sims2Tools.DBPF.SceneGraph.GZPS;
using Sims2Tools.DBPF.SceneGraph.IDR;
using Sims2Tools.Utils.Persistence;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace DbpfLister
{
    public partial class DbpfListerForm : Form
    {
        public DbpfListerForm()
        {
            InitializeComponent();
            this.Text = DbpfListerApp.AppTitle;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            RegistryTools.LoadAppSettings(DbpfListerApp.RegistryKey, DbpfListerApp.AppVersionMajor, DbpfListerApp.AppVersionMinor);
            RegistryTools.LoadFormSettings(DbpfListerApp.RegistryKey, this);

        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            RegistryTools.SaveAppSettings(DbpfListerApp.RegistryKey, DbpfListerApp.AppVersionMajor, DbpfListerApp.AppVersionMinor);
            RegistryTools.SaveFormSettings(DbpfListerApp.RegistryKey, this);
        }

        private void OnGoClicked(object sender, EventArgs e)
        {
            btnGo.Enabled = false;
            textMessages.Text = "=== PROCESSING ===\r\n";

            // FindBinx("C:/Program Files/EA Games/The Sims 2 Ultimate Collection/Apartment Life", new DBPFKey(Gzps.TYPE, (TypeGroupID)0x2C17B74A, (TypeInstanceID)0xB519F9C9, DBPFData.RESOURCE_NULL));

            FindBinx("C:/Program Files/EA Games/The Sims 2 Ultimate Collection", new DBPFKey(Gzps.TYPE, (TypeGroupID)0x2C17B74A, (TypeInstanceID)0x14E56E42, DBPFData.RESOURCE_NULL));

            textMessages.AppendText("=== FINISHED ===");
            btnGo.Enabled = true;
        }

        private void FindBinx(string baseFolder, DBPFKey gzpsKey)
        {
            foreach (string packagePath in Directory.GetFiles(baseFolder, "*.package", SearchOption.AllDirectories))
            {
                using (DBPFFile package = new DBPFFile(packagePath))
                {
                    List<DBPFEntry> binxEntries = package.GetEntriesByType(Binx.TYPE);

                    if (binxEntries != null && binxEntries.Count > 0)
                    {
                        // textMessages.AppendText($"{packagePath.Substring(baseFolder.Length + 1)}\r\n");

                        foreach (DBPFEntry binxEntry in binxEntries)
                        {
                            Idr idr = (Idr)package.GetResourceByKey(new DBPFKey(Idr.TYPE, binxEntry.GroupID, binxEntry.InstanceID, binxEntry.ResourceID));

                            if (idr != null)
                            {
                                Binx binx = (Binx)package.GetResourceByEntry(binxEntry);

                                if (binx != null)
                                {
                                    if (gzpsKey.Equals(idr.GetItem(binx.ObjectIdx)))
                                    {
                                        textMessages.AppendText($"{packagePath.Substring(baseFolder.Length + 1)} - {binx}\r\n");

                                        package.Close();
                                        return;
                                    }
                                }
                            }
                        }
                    }

                    package.Close();
                }
            }
        }
    }
}
