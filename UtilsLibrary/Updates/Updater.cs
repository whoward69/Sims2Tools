/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2025
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.Dialogs;
using Sims2Tools.Utils.Persistence;
using System;
using System.Windows.Forms;
using System.Xml;

namespace Sims2Tools.Updates
{
    public class Updater
    {
        private static readonly DBPF.Logger.IDBPFLogger logger = DBPF.Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly string AppRegKey;

        private readonly ToolStripMenuItem MyMenu;

        private readonly ToolStripMenuItem menuItemUpdatesNow;
        private readonly ToolStripMenuItem menuItemUpdatesDaily;
        private readonly ToolStripMenuItem menuItemUpdatesWeekly;
        private readonly ToolStripMenuItem menuItemUpdatesMonthly;
        private readonly ToolStripMenuItem menuItemUpdatesNever;

        public bool Enabled
        {
            get => menuItemUpdatesNow.Enabled;
            set => menuItemUpdatesNow.Enabled = value;
        }

        public Updater(string appRegKey, ToolStripMenuItem parentMenu)
        {
            AppRegKey = appRegKey;

            this.menuItemUpdatesNow = new ToolStripMenuItem()
            {
                Size = new System.Drawing.Size(180, 22),
                Text = "&Check Now",
            };
            menuItemUpdatesNow.Click += new System.EventHandler(this.OnUpdateNow);

            this.menuItemUpdatesDaily = new System.Windows.Forms.ToolStripMenuItem()
            {
                Size = new System.Drawing.Size(180, 22),
                Text = "Check Every &Day",
                Tag = 1
            };
            menuItemUpdatesDaily.Click += new System.EventHandler(this.OnUpdateFrequency);

            this.menuItemUpdatesWeekly = new System.Windows.Forms.ToolStripMenuItem()
            {
                Size = new System.Drawing.Size(180, 22),
                Text = "Check Every &Week",
                Tag = 7
            };
            menuItemUpdatesWeekly.Click += new System.EventHandler(this.OnUpdateFrequency);

            this.menuItemUpdatesMonthly = new System.Windows.Forms.ToolStripMenuItem()
            {
                Size = new System.Drawing.Size(180, 22),
                Text = "Check Every &Month",
                Tag = 30
            };
            menuItemUpdatesMonthly.Click += new System.EventHandler(this.OnUpdateFrequency);

            this.menuItemUpdatesNever = new System.Windows.Forms.ToolStripMenuItem()
            {
                Size = new System.Drawing.Size(180, 22),
                Text = "Check &Never",
                Tag = 0
            };
            menuItemUpdatesNever.Click += new System.EventHandler(this.OnUpdateFrequency);

            MyMenu = new ToolStripMenuItem()
            {
                Size = new System.Drawing.Size(180, 22),
                Text = "&Updates"
            };
            MyMenu.DropDownOpening += new System.EventHandler(this.OnUpdatesOpening);

            MyMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            menuItemUpdatesNow,
            new ToolStripSeparator() { Size = new System.Drawing.Size(177, 6) },
            menuItemUpdatesDaily,
            menuItemUpdatesWeekly,
            menuItemUpdatesMonthly,
            menuItemUpdatesNever
            });

            parentMenu.DropDownItems.Add(new ToolStripSeparator()
            {
                Size = new System.Drawing.Size(177, 6)
            });
            parentMenu.DropDownItems.Add(MyMenu);
        }

        private void OnUpdatesOpening(object sender, EventArgs e)
        {
            int checkFrequency = RegistryTools.GetUpdateCheckFrequency(AppRegKey);

            menuItemUpdatesDaily.Checked = (checkFrequency == 1);
            menuItemUpdatesWeekly.Checked = (checkFrequency == 7);
            menuItemUpdatesMonthly.Checked = (checkFrequency == 30);
            menuItemUpdatesNever.Checked = (checkFrequency == 0);

            if (!menuItemUpdatesDaily.Checked && !menuItemUpdatesWeekly.Checked && !menuItemUpdatesMonthly.Checked && !menuItemUpdatesNever.Checked)
            {
                menuItemUpdatesWeekly.Checked = true;
                RegistryTools.SetUpdateCheckFrequency(AppRegKey, 7);
            }
        }

        private void OnUpdateFrequency(object sender, EventArgs e)
        {
            RegistryTools.SetUpdateCheckFrequency(AppRegKey, (int)(sender as ToolStripMenuItem).Tag);
        }

        private void OnUpdateNow(object sender, EventArgs e)
        {
            CheckForUpdatesNow();
        }

        public void CheckForUpdates()
        {
            if (RegistryTools.IsUpdateCheckDue(AppRegKey))
            {
                CheckForUpdatesNow();
            }
        }

        private void CheckForUpdatesNow()
        {
            try
            {
                DateTime now = DateTime.Now;

                XmlTextReader reader = new XmlTextReader($"https://www.picknmixmods.com/Sims2/Notes/Sims2Tools.html?now={now.Ticks}");

                string eleName = AppRegKey.Substring(AppRegKey.LastIndexOf(@"\") + 1);

                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name.Equals(eleName))
                        {
                            int majorLatest = Convert.ToInt32(reader.GetAttribute("MajorLatest"));
                            int minorLatest = Convert.ToInt32(reader.GetAttribute("MinorLatest"));

                            int majorCurrent = (int)RegistryTools.GetSetting(AppRegKey, "VersionMajor", 0);
                            int minorCurrent = (int)RegistryTools.GetSetting(AppRegKey, "VersionMinor", 0);

                            if ((majorLatest > majorCurrent) ||
                                (majorLatest == majorCurrent && minorLatest > minorCurrent))
                            {
                                if (MsgBox.Show($"A later version of this utility is available.\n\nCurrent version {majorCurrent}.{minorCurrent}, latest version {majorLatest}.{minorLatest}", "Update Available", MessageBoxButtons.OK) == DialogResult.OK)
                                {
                                    RegistryTools.SetNextUpdateCheck(AppRegKey);
                                }
                            }

                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Info(ex.StackTrace);
            }
        }
    }
}
