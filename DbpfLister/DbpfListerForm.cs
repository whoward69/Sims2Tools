using Sims2Tools;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.BHAV;
using Sims2Tools.DBPF.CPF;
using Sims2Tools.DBPF.CTSS;
using Sims2Tools.DBPF.Data;
using Sims2Tools.DBPF.Groups;
using Sims2Tools.DBPF.Groups.GROP;
using Sims2Tools.DBPF.OBJD;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph.AGED;
using Sims2Tools.DBPF.SceneGraph.BINX;
using Sims2Tools.DBPF.SceneGraph.CRES;
using Sims2Tools.DBPF.SceneGraph.IDR;
using Sims2Tools.DBPF.SceneGraph.LIFO;
using Sims2Tools.DBPF.SceneGraph.RCOL;
using Sims2Tools.DBPF.SceneGraph.SHPE;
using Sims2Tools.DBPF.SceneGraph.TXMT;
using Sims2Tools.DBPF.SceneGraph.TXTR;
using Sims2Tools.DBPF.SceneGraph.XMOL;
using Sims2Tools.DBPF.STR;
using Sims2Tools.DBPF.TTAB;
using Sims2Tools.DBPF.TTAS;
using Sims2Tools.DBPF.UI;
using Sims2Tools.DBPF.Utils;
using Sims2Tools.DBPF.XFLR;
using Sims2Tools.DBPF.XWNT;
using Sims2Tools.Files;
using Sims2Tools.Utils.Persistence;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

namespace DbpfLister
{
    public partial class DbpfListerForm : Form
    {
        public DbpfListerForm()
        {
            InitializeComponent();
            this.Text = DbpfListerApp.AppTitle;

            pictBox.Visible = false;
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

            FindWallmasks("C:\\Program Files\\EA Games\\The Sims 2 Ultimate Collection");

            // FindHighestShpeSubsets("C:\\Program Files\\EA Games\\The Sims 2 Ultimate Collection");

            // ProcessODS("C:\\Users\\whowa\\Desktop\\Saline Body Shapes\\ShapeTemplates\\template.ods");

            // ProcessObjects();
            // ProcessWants();

            // FindAgedNonLsOne("C:\\Program Files\\EA Games\\The Sims 2 Ultimate Collection");

            // FindCresNonShpeRefs("C:\\Program Files\\EA Games\\The Sims 2 Ultimate Collection");

            /*
            FindNodeVersions("C:\\Program Files\\EA Games\\The Sims 2 Ultimate Collection\\Double Deluxe\\Base\\TSData\\Res\\Objects\\objects.package");
            FindNodeVersions("C:\\Program Files\\EA Games\\The Sims 2 Ultimate Collection\\University Life\\EP1\\TSData\\Res\\Objects\\objects.package");
            FindNodeVersions("C:\\Program Files\\EA Games\\The Sims 2 Ultimate Collection\\Double Deluxe\\EP2\\TSData\\Res\\Objects\\objects.package");
            FindNodeVersions("C:\\Program Files\\EA Games\\The Sims 2 Ultimate Collection\\Best of Business\\EP3\\TSData\\Res\\Objects\\objects.package");
            FindNodeVersions("C:\\Program Files\\EA Games\\The Sims 2 Ultimate Collection\\Fun with Pets\\SP1\\TSData\\Res\\Objects\\objects.package");
            FindNodeVersions("C:\\Program Files\\EA Games\\The Sims 2 Ultimate Collection\\Glamour Life Stuff\\TSData\\Res\\Objects\\objects.package");
            FindNodeVersions("C:\\Program Files\\EA Games\\The Sims 2 Ultimate Collection\\Fun with Pets\\EP4\\TSData\\Res\\Objects\\objects.package");
            FindNodeVersions("C:\\Program Files\\EA Games\\The Sims 2 Ultimate Collection\\Seasons\\TSData\\Res\\Objects\\objects.package");
            FindNodeVersions("C:\\Program Files\\EA Games\\The Sims 2 Ultimate Collection\\Double Deluxe\\SP4\\TSData\\Res\\Objects\\objects.package");
            FindNodeVersions("C:\\Program Files\\EA Games\\The Sims 2 Ultimate Collection\\Best of Business\\SP5\\TSData\\Res\\Objects\\objects.package");
            FindNodeVersions("C:\\Program Files\\EA Games\\The Sims 2 Ultimate Collection\\Bon Voyage\\TSData\\Res\\Objects\\objects.package");
            FindNodeVersions("C:\\Program Files\\EA Games\\The Sims 2 Ultimate Collection\\University Life\\SP6\\TSData\\Res\\Objects\\objects.package");
            FindNodeVersions("C:\\Program Files\\EA Games\\The Sims 2 Ultimate Collection\\Free Time\\TSData\\Res\\Objects\\objects.package");
            FindNodeVersions("C:\\Program Files\\EA Games\\The Sims 2 Ultimate Collection\\Best of Business\\SP7\\TSData\\Res\\Objects\\objects.package");
            FindNodeVersions("C:\\Program Files\\EA Games\\The Sims 2 Ultimate Collection\\University Life\\SP8\\TSData\\Res\\Objects\\objects.package");
            FindNodeVersions("C:\\Program Files\\EA Games\\The Sims 2 Ultimate Collection\\Apartment Life\\TSData\\Res\\Objects\\objects.package");
            FindNodeVersions("C:\\Program Files\\EA Games\\The Sims 2 Ultimate Collection\\Fun with Pets\\SP9\\TSData\\Res\\Objects\\objects.package");
            */

            // FindLifo("C:\\Users\\whowa\\Documents\\EA Games\\The Sims™ 2 Ultimate Collection\\Downloads");

            // FindTxtr("C:\\Users\\whowa\\Desktop\\SG Test Stuff\\Multi-SHPE\\WH_SimTurner.package");
            // FindTxtr("C:\\Users\\whowa\\Desktop\\SG Test Stuff\\Candyshop Panties\\1_WH_Materials_CandyShop_Panties.package");

            // FindMultiGmnd("C:\\Program Files\\EA Games\\The Sims 2 Ultimate Collection\\Fun with Pets\\SP9\\TSData\\Res\\Objects");

            // FindUi("C:\\Program Files\\EA Games\\The Sims 2 Ultimate Collection\\Fun with Pets\\SP9\\TSData\\Res\\UI\\ui.package");

            // FindXflr("C:\\Program Files\\EA Games\\The Sims 2 Ultimate Collection");

            // FindXmolNonMeshoverlay("C:\\Program Files\\EA Games\\The Sims 2 Ultimate Collection");

            // DumpAllMaxisResources(Cres.TYPE, "C:/Program Files/EA Games/The Sims 2 Ultimate Collection");
            // DumpAllMaxisResources(Shpe.TYPE, "C:/Program Files/EA Games/The Sims 2 Ultimate Collection");
            // DumpAllMaxisResources(Gmnd.TYPE, "C:/Program Files/EA Games/The Sims 2 Ultimate Collection");
            // DumpAllMaxisResources(Gmdc.TYPE, "C:/Program Files/EA Games/The Sims 2 Ultimate Collection");
            // DumpAllMaxisResources(Txmt.TYPE, "C:/Program Files/EA Games/The Sims 2 Ultimate Collection");
            // DumpAllMaxisResources(Txtr.TYPE, "C:/Program Files/EA Games/The Sims 2 Ultimate Collection");

            // GropTesting();

            // ExportStrings("C:\\Users\\whowa\\Desktop\\DrivingLicence_FR", MetaData.Languages.French);
            // ImportStrings("C:\\Users\\whowa\\Desktop\\DrivingLicence", "C:\\Users\\whowa\\Desktop\\DrivingLicence_FR", MetaData.Languages.French);
            // ExportStrings("C:\\Users\\whowa\\Desktop\\DrivingLicence", MetaData.Languages.French);

            // FindBinx("C:/Program Files/EA Games/The Sims 2 Ultimate Collection/Apartment Life", new DBPFKey(Gzps.TYPE, (TypeGroupID)0x2C17B74A, (TypeInstanceID)0xB519F9C9, DBPFData.RESOURCE_NULL));
            // FindBinx("C:/Program Files/EA Games/The Sims 2 Ultimate Collection", new DBPFKey(Gzps.TYPE, (TypeGroupID)0x2C17B74A, (TypeInstanceID)0x14E56E42, DBPFData.RESOURCE_NULL));

            // FindTTAB("C:\\Program Files\\EA Games\\The Sims 2 Ultimate Collection\\Fun with Pets\\SP9\\TSData\\Res\\Objects\\objects.package");

            textMessages.AppendText("=== FINISHED ===");
            btnGo.Enabled = true;
        }

        private void OnCopyClicked(object sender, EventArgs e)
        {
            Clipboard.SetText(textMessages.Text);
        }

        private void FindTxtr(string packagePath)
        {
            try
            {
                using (DBPFFile package = new DBPFFile(packagePath))
                {
                    foreach (DBPFEntry entry in package.GetEntriesByType(Txtr.TYPE))
                    {
                        try
                        {
                            Txtr txtr = (Txtr)package.GetResourceByEntry(entry);

                            Image image = txtr.ImageData?.LargestTexture?.Texture;

                            if (image != null)
                            {
                                int maxWidth = 512;
                                int maxHeight = maxWidth * image.Width / image.Height;

                                pictBox.Width = Math.Min(maxWidth, image.Width);
                                pictBox.Height = Math.Min(maxHeight, image.Height);

                                pictBox.Image = image;

                                pictBox.Visible = true;

                                break;
                            }

                            textMessages.AppendText($"{entry} - {packagePath}\r\n");
                        }
                        catch (Exception e2)
                        {
                            textMessages.AppendText($"{e2.Message} - {packagePath}\r\n");
                        }
                    }

                    package.Close();
                }
            }
            catch (Exception e1)
            {
                textMessages.AppendText($"{e1.Message} - {packagePath}\r\n");
            }
        }

        private void GropTesting()
        {
            if (Sims2ToolsLib.IsSims2HomePathSet)
            {
                string groupsPath = $"{Sims2ToolsLib.Sims2HomePath}\\Groups.cache";

                GroupsFile groupsCache = new GroupsFile(groupsPath);

                foreach (GropItem item in groupsCache.GetGroups("%UserDataDir%/downloads/"))
                {
                    for (int i = 0; i < item.RefGroupIDs.Length; ++i)
                    {
                        TypeGroupID group = item.RefGroupIDs[i];

                        if ((group.AsUInt() & 0xFF000000) == 0x7F000000)
                        {
                            if (group == DBPFData.GROUP_GLOBALS)
                            {
                                textMessages.AppendText($"Global\t{DBPFData.GROUP_GLOBALS}\t{item.FileName}\r\n");
                            }
                            else if (group == DBPFData.GROUP_BEHAVIOR)
                            {
                                textMessages.AppendText($"Behaviour:\t{DBPFData.GROUP_BEHAVIOR}\t{item.FileName}\r\n");
                            }
                            else if (GameData.SemiGlobalsByGroup.TryGetValue(group.Hex8String(), out string semiName))
                            {
                                textMessages.AppendText($"Semi-global\t{semiName}\t{item.FileName}\r\n");
                            }
                            else if (GameData.GlobalObjectsByGroup.TryGetValue(group.Hex8String(), out string objectName))
                            {
                                textMessages.AppendText($"Game object\t{objectName}\t{item.FileName}\r\n");
                            }
                        }
                    }
                }
            }
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

        private void DumpAllMaxisResources(TypeTypeID typeID, string baseFolder)
        {
            string suffix = $"_{DBPFData.TypeName(typeID).ToLower()}";
            Regex re = new Regex($"#0x[0-9a-f]+!(0x[0-9a-f]+_?)?age[0-9]_[0-9]{suffix}");

            textMessages.AppendText($"Processing {DBPFData.TypeName(typeID)}\r\n");

            using (StreamWriter writer = new StreamWriter($"C:/Users/whowa/Desktop/mesh{suffix}.csv"))
            {
                writer.WriteLine("Type ID,Group ID,Instance ID,Resource ID,Name,Relative Path");

                foreach (string packagePath in Directory.GetFiles(baseFolder, "*.package", SearchOption.AllDirectories))
                {
                    if (
                        packagePath.Contains("\\Characters\\") ||
                        packagePath.Contains("\\Lots\\") ||
                        packagePath.Contains("\\LotCatalog\\") ||
                        packagePath.Contains("\\GlobalLots\\") ||
                        packagePath.Contains("\\LotTemplates\\")
                       ) continue;

                    if (
                        packagePath.Contains("\\FaceInfo.package") ||
                        packagePath.Contains("\\effects.package")
                       ) continue;

                    using (DBPFFile package = new DBPFFile(packagePath))
                    {
                        foreach (DBPFEntry entry in package.GetEntriesByType(typeID))
                        {
                            Rcol rcol = (Rcol)package.GetResourceByEntry(entry);

                            string name = rcol.KeyName;

                            if (re.IsMatch(name.ToLower())) continue;

                            if (name.EndsWith(suffix, StringComparison.OrdinalIgnoreCase)) name = name.Substring(0, name.Length - 5);

                            writer.WriteLine($"{rcol.TypeID},{rcol.GroupID},{rcol.InstanceID},{rcol.ResourceID},{name},{packagePath.Substring(baseFolder.Length + 1)}");
                        }

                        package.Close();
                    }
                }

                writer.Close();
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

        private void FindCresNonShpeRefs(string baseFolder)
        {
            foreach (string packagePath in Directory.GetFiles(baseFolder, "*.package", SearchOption.AllDirectories))
            {
                try
                {
                    bool breakOut = false;

                    using (DBPFFile package = new DBPFFile(packagePath))
                    {
                        foreach (DBPFEntry entry in package.GetEntriesByType(Cres.TYPE))
                        {
                            try
                            {
                                Cres cres = (Cres)package.GetResourceByEntry(entry);

                                foreach (DBPFKey refKey in cres.ReferencedFiles)
                                {
                                    if (refKey.TypeID != Cres.TYPE && refKey.TypeID != Shpe.TYPE
                                        // && refKey.TypeID != Lpnt.TYPE && refKey.TypeID != Lamb.TYPE && refKey.TypeID != Lspt.TYPE && refKey.TypeID != Ldir.TYPE
                                        )
                                    {
                                        textMessages.AppendText($"{entry}\t{refKey}\t{packagePath}\r\n");

                                        // breakOut = true;

                                        if (breakOut) break;
                                    }
                                }
                            }
                            catch (Exception e2)
                            {
                                textMessages.AppendText($"{e2.Message} - {packagePath}\r\n");
                            }

                            if (breakOut) break;
                        }

                        package.Close();
                    }
                }
                catch (Exception e1)
                {
                    textMessages.AppendText($"{e1.Message} - {packagePath}\r\n");
                }
            }
        }

        private void FindXflr(string baseFolder)
        {
            foreach (string packagePath in Directory.GetFiles(baseFolder, "*.package", SearchOption.AllDirectories))
            {
                try
                {
                    using (DBPFFile package = new DBPFFile(packagePath))
                    {
                        foreach (DBPFEntry entry in package.GetEntriesByType(Xflr.TYPE))
                        {
                            try
                            {
                                Xflr xflr = (Xflr)package.GetResourceByEntry(entry);

                                textMessages.AppendText($"{entry} - {packagePath}\r\n");
                            }
                            catch (Exception e2)
                            {
                                textMessages.AppendText($"{e2.Message} - {packagePath}\r\n");
                            }
                        }

                        package.Close();
                    }
                }
                catch (Exception e1)
                {
                    textMessages.AppendText($"{e1.Message} - {packagePath}\r\n");
                }
            }
        }

        private void FindLifo(string baseFolder)
        {
            foreach (string packagePath in Directory.GetFiles(baseFolder, "*.package", SearchOption.AllDirectories))
            {
                try
                {
                    using (DBPFFile package = new DBPFFile(packagePath))
                    {
                        foreach (DBPFEntry entry in package.GetEntriesByType(Lifo.TYPE))
                        {
                            try
                            {
                                Lifo lifo = (Lifo)package.GetResourceByEntry(entry);

                                textMessages.AppendText($"{entry} - {packagePath}\r\n");
                            }
                            catch (Exception e2)
                            {
                                textMessages.AppendText($"{e2.Message} - {packagePath}\r\n");
                            }
                        }

                        package.Close();
                    }
                }
                catch (Exception e1)
                {
                    textMessages.AppendText($"{e1.Message} - {packagePath}\r\n");
                }
            }
        }

        private void FindMultiGmnd(string baseFolder)
        {
            foreach (string packagePath in Directory.GetFiles(baseFolder, "*.package", SearchOption.AllDirectories))
            {
                try
                {
                    using (DBPFFile package = new DBPFFile(packagePath))
                    {
                        foreach (DBPFEntry entry in package.GetEntriesByType(Shpe.TYPE))
                        {
                            try
                            {
                                Shpe shpe = (Shpe)package.GetResourceByEntry(entry);

                                if (shpe.Shape.Items.Count != 1)
                                {
                                    if (!shpe.Shape.Items[1].FileName.ToLower().Contains("lod"))
                                    {
                                        textMessages.AppendText($"{shpe.Shape.Items.Count} - {entry} - {packagePath}\r\n");
                                    }
                                }
                            }
                            catch (Exception e2)
                            {
                                textMessages.AppendText($"{e2.Message} - {packagePath}\r\n");
                            }
                        }

                        package.Close();
                    }
                }
                catch (Exception e1)
                {
                    textMessages.AppendText($"{e1.Message} - {packagePath}\r\n");
                }
            }
        }

        private void FindWallmasks(string baseFolder)
        {
            foreach (string packagePath in Directory.GetFiles(baseFolder, "*.package", SearchOption.AllDirectories))
            {
                try
                {
                    using (DBPFFile package = new DBPFFile(packagePath))
                    {
                        foreach (DBPFEntry entry in package.GetEntriesByType(Txmt.TYPE))
                        {
                            try
                            {
                                Txmt txmt = (Txmt)package.GetResourceByEntry(entry);

                                if (txmt.SgName.ToLower().Contains("wallmask"))
                                {
                                    textMessages.AppendText($"{txmt.SgName}\r\n");
                                }
                            }
                            catch (Exception e2)
                            {
                                textMessages.AppendText($"{e2.Message} - {packagePath}\r\n");
                            }
                        }

                        package.Close();
                    }
                }
                catch (Exception e1)
                {
                    textMessages.AppendText($"{e1.Message} - {packagePath}\r\n");
                }
            }
        }

        private void FindHighestShpeSubsets(string baseFolder)
        {
            int most = 0;
            string res = "";

            foreach (string packagePath in Directory.GetFiles(baseFolder, "*.package", SearchOption.AllDirectories))
            {
                try
                {
                    using (DBPFFile package = new DBPFFile(packagePath))
                    {
                        foreach (DBPFEntry entry in package.GetEntriesByType(Shpe.TYPE))
                        {
                            try
                            {
                                Shpe shpe = (Shpe)package.GetResourceByEntry(entry);

                                if (shpe.Shape.Parts.Count != most)
                                {
                                    most = shpe.Shape.Parts.Count;
                                    res = shpe.SgName;
                                }
                            }
                            catch (Exception e2)
                            {
                                textMessages.AppendText($"{e2.Message} - {packagePath}\r\n");
                            }
                        }

                        package.Close();
                    }
                }
                catch (Exception e1)
                {
                    textMessages.AppendText($"{e1.Message} - {packagePath}\r\n");
                }
            }

            textMessages.AppendText($"Most subsets is {most} in {res}\r\n");
        }

        private void FindXmolNonMeshoverlay(string baseFolder)
        {
            foreach (string packagePath in Directory.GetFiles(baseFolder, "*.package", SearchOption.AllDirectories))
            {
                try
                {
                    using (DBPFFile package = new DBPFFile(packagePath))
                    {
                        foreach (DBPFEntry entry in package.GetEntriesByType(Xmol.TYPE))
                        {
                            try
                            {
                                Xmol xmol = (Xmol)package.GetResourceByEntry(entry);

                                if (!"meshoverlay".Equals(xmol.GetItem("type")?.StringValue.ToLower()))
                                {
                                    textMessages.AppendText($"{entry} - {packagePath}\r\n");
                                }
                            }
                            catch (Exception e2)
                            {
                                textMessages.AppendText($"{e2.Message} - {packagePath}\r\n");
                            }
                        }

                        package.Close();
                    }
                }
                catch (Exception e1)
                {
                    textMessages.AppendText($"{e1.Message} - {packagePath}\r\n");
                }
            }
        }

        private void FindAgedNonLsOne(string baseFolder)
        {
            foreach (string packagePath in Directory.GetFiles(baseFolder, "*.package", SearchOption.AllDirectories))
            {
                try
                {
                    using (DBPFFile package = new DBPFFile(packagePath))
                    {
                        foreach (DBPFEntry entry in package.GetEntriesByType(Aged.TYPE))
                        {
                            try
                            {
                                Aged aged = (Aged)package.GetResourceByEntry(entry);

                                CpfItem item = aged.GetItem("listcnt");

                                if (item != null)
                                {
                                    int cnt = (int)item.UIntegerValue;

                                    for (int i = 0; i < cnt; ++i)
                                    {
                                        string lsValue = aged.GetItem($"ls{i}")?.StringValue;

                                        if (!string.IsNullOrEmpty(lsValue) && !lsValue.Equals("1"))
                                        {
                                            textMessages.AppendText($"{entry} - {packagePath}\r\n");
                                        }
                                    }
                                }
                            }
                            catch (Exception e2)
                            {
                                textMessages.AppendText($"{e2.Message} - {packagePath}\r\n");
                            }
                        }

                        package.Close();
                    }
                }
                catch (Exception e1)
                {
                    textMessages.AppendText($"{e1.Message} - {packagePath}\r\n");
                }
            }
        }

        private void FindNodeVersions(string packagePath)
        {
            textMessages.AppendText($"--- {packagePath}\r\n");

            SortedDictionary<int, SortedList<int, int>> nodeVersionsByPrimitive = new SortedDictionary<int, SortedList<int, int>>();

            using (DBPFFile package = new DBPFFile(packagePath))
            {
                foreach (DBPFEntry entry in package.GetEntriesByType(Bhav.TYPE))
                {
                    Bhav bhav = (Bhav)package.GetResourceByEntry(entry);

                    foreach (Instruction instruction in bhav.Instructions)
                    {
                        if (instruction.OpCode > 0x00FF) continue;

                        if (!nodeVersionsByPrimitive.ContainsKey(instruction.OpCode))
                        {
                            nodeVersionsByPrimitive.Add(instruction.OpCode, new SortedList<int, int>());
                        }

                        SortedList<int, int> nodeVersions = nodeVersionsByPrimitive[instruction.OpCode];

                        if (!nodeVersions.ContainsKey(instruction.NodeVersion))
                        {
                            nodeVersions.Add(instruction.NodeVersion, 0);
                        }

                        nodeVersions[instruction.NodeVersion] += 1;
                    }
                }

                package.Close();
            }

            foreach (int primitive in nodeVersionsByPrimitive.Keys)
            {
                if (nodeVersionsByPrimitive[primitive].Count == 1) continue;

                textMessages.AppendText($"{Helper.Hex4PrefixString(primitive)}:");

                SortedList<int, int> nodeVersions = nodeVersionsByPrimitive[primitive];

                bool addComma = false;
                foreach (int version in nodeVersions.Keys)
                {
                    textMessages.AppendText($"{(addComma ? "," : "")} {version} ({nodeVersions[version]})");

                    addComma = true;
                }

                textMessages.AppendText("\r\n");
            }
        }

        private void FindTTAB(string packagePath)
        {
            using (DBPFFile package = new DBPFFile(packagePath))
            {
                foreach (DBPFEntry entry in package.GetEntriesByType(Ttab.TYPE))
                {
                    Console.WriteLine(entry);
                    try
                    {
                        Ttab ttab = (Ttab)package.GetResourceByEntry(entry);

                        foreach (TtabItem item in ttab.GetItems())
                        {
                            if (item.Autonomy != 0x64 && item.Autonomy != 0x32)
                            {
                                textMessages.AppendText($"{entry}::{item.StringIndex} Autonomy {item.Autonomy} ({Helper.Hex8PrefixString(item.Autonomy)})\r\n");
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        textMessages.AppendText($"EXCEPTION: {entry} - {e.Message}\r\n");
                    }
                }

                package.Close();
            }
        }

        private void FindUi(string packagePath)
        {
            using (DBPFFile package = new DBPFFile(packagePath))
            {
                foreach (DBPFEntry entry in package.GetEntriesByType(Ui.TYPE))
                {
                    // Console.WriteLine(entry);
                    try
                    {
                        Ui ui = (Ui)package.GetResourceByEntry(entry);
                    }
                    catch (Exception e)
                    {
                        textMessages.AppendText($"EXCEPTION: {entry} - {e.Message}\r\n");
                    }
                }

                package.Close();
            }
        }

        private void ProcessODS(string fullPath)
        {
            List<List<string>> rows = null;
            List<string> row = null;

            int rowRepeat = 0;

            int cellRepeat = 0;
            string cellText = "";

            using (ZipArchive zip = ZipFile.Open(fullPath, ZipArchiveMode.Read))
            {
                ZipArchiveEntry content = zip.GetEntry("content.xml");

                if (content != null)
                {
                    XmlReader reader = XmlReader.Create(content.Open());

                    reader.MoveToContent();
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            if (reader.Name.Equals("table:table"))
                            {
                                rows = new List<List<string>>();
                            }
                            else if (reader.Name.Equals("table:table-row"))
                            {
                                // Process the previous row when we encounter the next one in the table, this stops us processing the last row (which repeats many, many times)
                                if (row != null)
                                {
                                    while (rowRepeat-- > 0)
                                    {
                                        rows.Add(row);
                                    }
                                }

                                string repeat = reader.GetAttribute("table:number-rows-repeated");
                                rowRepeat = (repeat == null) ? 1 : Int32.Parse(repeat);

                                row = new List<string>();

                                cellRepeat = 0;
                            }
                            else if (reader.Name.Equals("table:table-cell"))
                            {
                                // Process the previous cell when we encounter the next one in the row, this stops us processing the last cell (which repeats many, many times)
                                while (cellRepeat-- > 0)
                                {
                                    row.Add(cellText);
                                }

                                string repeat = reader.GetAttribute("table:number-columns-repeated");
                                cellRepeat = (repeat == null) ? 1 : Int32.Parse(repeat);
                                cellText = "";
                            }
                            else if (reader.Name.Equals("text:p"))
                            {
                                cellText = reader.ReadElementContentAsString();
                            }
                        }
                        else if (reader.NodeType == XmlNodeType.EndElement)
                        {
                            if (reader.Name.Equals("table:table"))
                            {
                                // Only process the first table in the first spreadsheet
                                return;
                            }
                        }
                    }
                }
            }
        }

        private void ProcessObjects()
        {
            string csvFile = "C:\\Users\\whowa\\Desktop\\MaxisObjects.csv";

            File.Delete(csvFile);

            StreamWriter sw = new StreamWriter(csvFile);

            DirectoryInfo installPath = new DirectoryInfo($"{Sims2ToolsLib.Sims2Path}\\..\\..");

            Dictionary<TypeGUID, string> allObjects = new Dictionary<TypeGUID, string>();

            // Process the main object file first
            string mainPackagePath = $"{Sims2ToolsLib.Sims2Path}{GameData.objectsSubPath}";
            using (DBPFFile package = new DBPFFile(mainPackagePath))
            {
                string relPath = mainPackagePath.Substring(installPath.FullName.Length);

                foreach (DBPFEntry entry in package.GetEntriesByType(Objd.TYPE))
                {
                    try
                    {
                        Objd objd = (Objd)package.GetResourceByEntry(entry);

                        if (!allObjects.ContainsKey(objd.Guid))
                        {
                            allObjects.Add(objd.Guid, objd.KeyName);

                            sw.Write($"{objd.Guid},{CsvEscapeQuote(objd.KeyName)},{objd.GroupID},{relPath}");

                            try
                            {
                                Ctss ctss = (Ctss)package.GetResourceByKey(new DBPFKey(Ctss.TYPE, objd.GroupID, (TypeInstanceID)objd.GetRawData(ObjdIndex.CatalogueStringsId), DBPFData.RESOURCE_NULL));
                                if (ctss != null) sw.Write($",{CsvEscapeQuote(ctss.LanguageItems(MetaData.Languages.Default)[0].Title)},{CsvEscapeQuote(ctss.LanguageItems(MetaData.Languages.Default)[1].Title)}");
                            }
                            catch (Exception)
                            {
                            }

                            sw.WriteLine();
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            foreach (string packagePath in Directory.GetFiles(installPath.FullName, "*.package", SearchOption.AllDirectories))
            {
                if (!packagePath.Equals(mainPackagePath))
                {
                    if (!(packagePath.Contains("FaceInfo") || packagePath.Contains("effects") || packagePath.Contains("Tutorial")))
                    {
                        using (DBPFFile package = new DBPFFile(packagePath))
                        {
                            string relPath = packagePath.Substring(installPath.FullName.Length);

                            foreach (DBPFEntry entry in package.GetEntriesByType(Objd.TYPE))
                            {
                                try
                                {
                                    Objd objd = (Objd)package.GetResourceByEntry(entry);

                                    if (!allObjects.ContainsKey(objd.Guid))
                                    {
                                        allObjects.Add(objd.Guid, objd.KeyName);

                                        sw.Write($"{objd.Guid},{CsvEscapeQuote(objd.KeyName)},{objd.GroupID},{relPath}");

                                        try
                                        {
                                            Ctss ctss = (Ctss)package.GetResourceByKey(new DBPFKey(Ctss.TYPE, objd.GroupID, (TypeInstanceID)objd.GetRawData(ObjdIndex.CatalogueStringsId), DBPFData.RESOURCE_NULL));
                                            if (ctss != null) sw.Write($",{CsvEscapeQuote(ctss.LanguageItems(MetaData.Languages.Default)[0].Title)},{CsvEscapeQuote(ctss.LanguageItems(MetaData.Languages.Default)[1].Title)}");
                                        }
                                        catch (Exception)
                                        {
                                        }

                                        sw.WriteLine();
                                    }
                                }
                                catch (Exception)
                                {
                                }
                            }
                        }
                    }
                }
            }

            sw.Close();
        }

        private string CsvEscapeQuote(string s)
        {
            return $"\"{s.Replace("\"", "\"\"")}\"";
        }

        private void ProcessWants()
        {
            DirectoryInfo installPath = new DirectoryInfo($"{Sims2ToolsLib.Sims2Path}\\..\\..");

            // Process the main wants file first
            string mainPackagePath = $"{Sims2ToolsLib.Sims2Path}{GameData.wantsSubDir}\\Wants.package";
            using (DBPFFile package = new DBPFFile(mainPackagePath))
            {
                string relPath = mainPackagePath.Substring(installPath.FullName.Length);

                string textPackagePath = new DirectoryInfo($"{mainPackagePath}\\..\\..\\Text\\Wants.package").FullName;

                using (DBPFFile textPackage = new DBPFFile(textPackagePath))
                {
                    foreach (DBPFEntry entry in package.GetEntriesByType(Xwnt.TYPE))
                    {
                        try
                        {
                            Xwnt xwnt = (Xwnt)package.GetResourceByEntry(entry);

                            string title = null;

                            TypeInstanceID strInst = (TypeInstanceID)xwnt.GetItem("stringSet").UIntegerValue;
                            Str str = (Str)textPackage.GetResourceByKey(new DBPFKey(Str.TYPE, DBPFData.GROUP_LOCAL, strInst, DBPFData.RESOURCE_NULL));

                            if (str != null)
                            {
                                title = str.LanguageItems(MetaData.Languages.Default)[2]?.Title;

                                if (string.IsNullOrEmpty(title))
                                {
                                    title = str.LanguageItems(MetaData.Languages.Default)[0]?.Title;
                                }
                            }

                            if (string.IsNullOrEmpty(title))
                            {
                                title = $"[{xwnt.GetItem("nodeText")?.StringValue}]";

                                if (string.IsNullOrEmpty(title))
                                {
                                    title = "[unknown}";
                                }
                            }

                            textMessages.AppendText($"{entry.InstanceID}\t{title}\t{xwnt.GetItem("folder")?.StringValue}\t{xwnt.GetItem("objectType")?.StringValue}\t{xwnt.GetItem("score")?.IntegerValue}\t{xwnt.GetItem("influence")?.IntegerValue}\r\n");
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
        }
    }
}
