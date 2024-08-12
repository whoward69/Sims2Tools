using Sims2Tools;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.BHAV;
using Sims2Tools.DBPF.CPF;
using Sims2Tools.DBPF.CTSS;
using Sims2Tools.DBPF.Data;
using Sims2Tools.DBPF.Groups;
using Sims2Tools.DBPF.Groups.GROP;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph.AGED;
using Sims2Tools.DBPF.SceneGraph.BINX;
using Sims2Tools.DBPF.SceneGraph.CRES;
using Sims2Tools.DBPF.SceneGraph.IDR;
using Sims2Tools.DBPF.SceneGraph.LIFO;
using Sims2Tools.DBPF.SceneGraph.RCOL;
using Sims2Tools.DBPF.SceneGraph.SHPE;
using Sims2Tools.DBPF.SceneGraph.TXTR;
using Sims2Tools.DBPF.SceneGraph.XMOL;
using Sims2Tools.DBPF.STR;
using Sims2Tools.DBPF.TTAB;
using Sims2Tools.DBPF.TTAS;
using Sims2Tools.DBPF.UI;
using Sims2Tools.DBPF.Utils;
using Sims2Tools.DBPF.XFLR;
using Sims2Tools.Files;
using Sims2Tools.Utils.Persistence;
using System;
using System.Collections.Generic;
using System.Drawing;
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

            FindAgedNonLsOne("C:\\Program Files\\EA Games\\The Sims 2 Ultimate Collection");

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
    }
}
