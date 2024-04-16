using Sims2Tools.DBPF;
using Sims2Tools.DBPF.CTSS;
using Sims2Tools.DBPF.Data;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph.BINX;
using Sims2Tools.DBPF.SceneGraph.IDR;
using Sims2Tools.DBPF.STR;
using Sims2Tools.DBPF.TTAS;
using Sims2Tools.Files;
using Sims2Tools.Utils.Persistence;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
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

            // ExportStrings("C:\\Users\\whowa\\Desktop\\DrivingLicence_FR", MetaData.Languages.French);
            // ImportStrings("C:\\Users\\whowa\\Desktop\\DrivingLicence", "C:\\Users\\whowa\\Desktop\\DrivingLicence_FR", MetaData.Languages.French);
            ExportStrings("C:\\Users\\whowa\\Desktop\\DrivingLicence", MetaData.Languages.French);

            // FindBinx("C:/Program Files/EA Games/The Sims 2 Ultimate Collection/Apartment Life", new DBPFKey(Gzps.TYPE, (TypeGroupID)0x2C17B74A, (TypeInstanceID)0xB519F9C9, DBPFData.RESOURCE_NULL));
            // FindBinx("C:/Program Files/EA Games/The Sims 2 Ultimate Collection", new DBPFKey(Gzps.TYPE, (TypeGroupID)0x2C17B74A, (TypeInstanceID)0x14E56E42, DBPFData.RESOURCE_NULL));

            textMessages.AppendText("=== FINISHED ===");
            btnGo.Enabled = true;
        }

        private void ImportStrings(string targetFolder, string importFolder, MetaData.Languages importLang)
        {
            Regex re = new Regex("(.*)_(STR|TTAs|CTSS)_(0x[0-9A-F]{8})_(0x[0-9A-F]{8})_([0-9]+)\\.txt");

            Dictionary<string, DBPFFile> updatedPackages = new Dictionary<string, DBPFFile>();
            bool errors = false;

            foreach (string txtPath in Directory.GetFiles(importFolder, "*.txt", SearchOption.AllDirectories))
            {
                textMessages.AppendText($"{txtPath}\r\n");

                FileInfo fi = new FileInfo(txtPath);

                Match m = re.Match(fi.Name);

                if (m.Success)
                {
                    string packagePath = $"{targetFolder}/{m.Groups[1]}.package";

                    if (File.Exists(packagePath))
                    {
                        try
                        {
                            TypeTypeID typeId = (m.Groups[2].Value.Equals("CTSS") ? Ctss.TYPE : (m.Groups[2].Value.Equals("TTAs") ? Ttas.TYPE : Str.TYPE));
                            TypeGroupID groupId = (TypeGroupID)Convert.ToUInt32(m.Groups[3].Value, 16);
                            TypeInstanceID instId = (TypeInstanceID)Convert.ToUInt32(m.Groups[4].Value, 16);
                            MetaData.Languages lang = (MetaData.Languages)Convert.ToSByte(m.Groups[5].Value);

                            if (lang == importLang)
                            {
                                DBPFFile package;

                                if (updatedPackages.ContainsKey(packagePath))
                                {
                                    package = updatedPackages[packagePath];
                                }
                                else
                                {
                                    package = new DBPFFile(packagePath);

                                    updatedPackages.Add(packagePath, package);
                                }

                                DBPFKey key = new DBPFKey(typeId, groupId, instId, DBPFData.RESOURCE_NULL);
                                Str str = (Str)package.GetResourceByKey(key);

                                if (str != null)
                                {
                                    int defCount = str.LanguageItems(MetaData.Languages.Default).Count;

                                    if (!str.HasLanguage(importLang))
                                    {
                                        str.AddLanguage(importLang);
                                    }

                                    List<StrItem> langItems = str.LanguageItems(importLang);

                                    PjseStringFileReader reader = new PjseStringFileReader(txtPath);

                                    string line = reader.ReadString();
                                    int item = 0;

                                    while (line != null)
                                    {
                                        if (item >= langItems.Count)
                                        {
                                            langItems.Add(new StrItem(importLang, line, ""));
                                        }
                                        else
                                        {
                                            langItems[item].Title = line;
                                            langItems[item].Description = "";
                                        }

                                        line = reader.ReadString();
                                        ++item;
                                    }

                                    reader.Close();

                                    package.Commit(str);

                                    if (langItems.Count > defCount)
                                    {
                                        textMessages.AppendText($"WARNING: More translated strings than for default lang!\r\n");
                                    }
                                    else if (langItems.Count < defCount)
                                    {
                                        textMessages.AppendText($"WARNING: Fewer translated strings than for default lang!\r\n");
                                    }
                                }
                                else
                                {
                                    textMessages.AppendText($"ERROR: Can't find {key}\r\n");
                                    errors = true;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            textMessages.AppendText($"ERROR: {e.Message}\r\n");
                            errors = true;
                        }
                    }
                    else
                    {
                        textMessages.AppendText($"ERROR: Can't find {packagePath}\r\n");
                        errors = true;
                    }
                }
            }

            foreach (DBPFFile package in updatedPackages.Values)
            {
                if (!errors)
                {
                    package.Update(true);
                }

                package.Close();
            }
        }

        private void ExportStrings(string exportFolder, MetaData.Languages lang)
        {
            foreach (string packagePath in Directory.GetFiles(exportFolder, "*.package", SearchOption.AllDirectories))
            {
                using (DBPFFile package = new DBPFFile(packagePath))
                {
                    textMessages.AppendText($"{package.PackageNameNoExtn}\r\n");

                    foreach (TypeTypeID typeId in new List<TypeTypeID> { Str.TYPE, Ctss.TYPE, Ttas.TYPE })
                    {
                        foreach (DBPFEntry entry in package.GetEntriesByType(typeId))
                        {
                            Str str = (Str)package.GetResourceByEntry(entry);

                            List<StrItem> langStrings = str.LanguageItems(lang);

                            if (langStrings.Count > 0)
                            {
                                textMessages.AppendText($"  {DBPFData.TypeName(typeId)}_{entry.GroupID}_{entry.InstanceID}_{(byte)lang}\r\n");

                                using (StreamWriter writer = File.CreateText($"{package.PackageDir}/{package.PackageNameNoExtn}_{DBPFData.TypeName(typeId)}_{entry.GroupID}_{entry.InstanceID}_{(byte)lang}.txt"))
                                {
                                    writer.WriteLine("<-Comment->");
                                    writer.WriteLine("PJSE String file - single language export");

                                    foreach (StrItem strItem in langStrings)
                                    {
                                        writer.WriteLine("<-String->");
                                        if (!string.IsNullOrEmpty(strItem.Title)) writer.WriteLine(strItem.Title);
                                        writer.WriteLine("<-Desc->");
                                        if (!string.IsNullOrEmpty(strItem.Description)) writer.WriteLine(strItem.Description);
                                    }

                                    int diff = str.LanguageItems(MetaData.Languages.Default).Count - langStrings.Count;

                                    if (diff > 0)
                                    {
                                        textMessages.AppendText("WARNING: Not enough translated strings\r\n");
                                    }
                                    else if (diff < 0)
                                    {
                                        textMessages.AppendText("WARNING: Too many translated strings\r\n");
                                    }

                                    writer.Close();
                                }
                            }
                        }
                    }

                    package.Close();
                }
            }
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
