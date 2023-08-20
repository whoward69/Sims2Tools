/*
 * Hood Exporter - a utility for exporting a Sims 2 'hood as XML
 *               - see http://www.picknmixmods.com/Sims2/Notes/HoodExporter/HoodExporter.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2023
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Microsoft.WindowsAPICodePack.Dialogs;
using Sims2Tools;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.CTSS;
using Sims2Tools.DBPF.Data;
using Sims2Tools.DBPF.Images.IMG;
using Sims2Tools.DBPF.Images.JPG;
using Sims2Tools.DBPF.Neighbourhood.BNFO;
using Sims2Tools.DBPF.Neighbourhood.FAMI;
using Sims2Tools.DBPF.Neighbourhood.FAMT;
using Sims2Tools.DBPF.Neighbourhood.IDNO;
using Sims2Tools.DBPF.Neighbourhood.LTXT;
using Sims2Tools.DBPF.Neighbourhood.NGBH;
using Sims2Tools.DBPF.Neighbourhood.SDNA;
using Sims2Tools.DBPF.Neighbourhood.SDSC;
using Sims2Tools.DBPF.Neighbourhood.SREL;
using Sims2Tools.DBPF.Neighbourhood.SWAF;
using Sims2Tools.DBPF.OBJD;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.STR;
using Sims2Tools.DBPF.Utils;
using Sims2Tools.Dialogs;
using Sims2Tools.Updates;
using Sims2Tools.Utils.Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace HoodExporter
{
    public partial class HoodExporterForm : Form
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Regex LotRegex = new Regex(@"_Lot([0-9]+)\.package$");

        private MruList MyMruList;
        private Updater MyUpdater;

        private const MetaData.Languages defLid = MetaData.Languages.English;
        private MetaData.Languages prefLid = defLid;

        private readonly TypeTypeID[] baseTypes = new TypeTypeID[] { Idno.TYPE, Ctss.TYPE, Ngbh.TYPE, Str.TYPE };
        private readonly TypeTypeID[] simTypes = new TypeTypeID[] { Sdsc.TYPE, Sdna.TYPE, Srel.TYPE, Swaf.TYPE };
        private readonly TypeTypeID[] lotTypes = new TypeTypeID[] { Ltxt.TYPE, Bnfo.TYPE };
        private readonly TypeTypeID[] familyTypes = new TypeTypeID[] { Fami.TYPE, Famt.TYPE };

        private readonly List<TypeTypeID> rufioTypes = new List<TypeTypeID>();
        private readonly List<TypeTypeID> mainTypes = new List<TypeTypeID>();
        private readonly List<TypeTypeID> suburbTypes = new List<TypeTypeID>();
        private readonly List<TypeTypeID> universityTypes = new List<TypeTypeID>();
        private readonly List<TypeTypeID> downtownTypes = new List<TypeTypeID>();
        private readonly List<TypeTypeID> vacationTypes = new List<TypeTypeID>();

        private readonly List<TokenData> tokenDataList = new List<TokenData>();

        private SimDataCache simCache;

        private String xsltPath = "";
        private bool isRufio = false;
        private bool reuseExport = false;

        private String hoodCode;
        private String exportPath;

        // MUST total 100!
        private const int processSimImagesPercent = 15;
        private const int processLotImagesPercent = 5;
        private const int processFamiyImagesPercent = 5;
        private const int processSuburbHoodsPercent = 10;
        private const int processUniversityHoodsPercent = 5;
        private const int processDowntownHoodsPercent = 5;
        private const int processVactionHoodsPercent = 5;
        private const int processMainHoodPercent = 40;
        private const int transformPercent = 5;
        private const int outputPercent = 5;

        private int startingPercent;

        private readonly Dictionary<TypeGUID, String> npcsByGuid = new Dictionary<TypeGUID, string>();

        public HoodExporterForm()
        {
            logger.Info(HoodExporterApp.AppProduct);

            InitializeComponent();
            this.Text = HoodExporterApp.AppName;

            selectPathDialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true
            };

            foreach (TypeTypeID type in baseTypes)
            {
                rufioTypes.Add(type);
                mainTypes.Add(type);
                suburbTypes.Add(type);
                universityTypes.Add(type);
                downtownTypes.Add(type);
                vacationTypes.Add(type);
            }

            foreach (TypeTypeID type in lotTypes)
            {
                rufioTypes.Add(type);
            }

            foreach (TypeTypeID type in familyTypes)
            {
                rufioTypes.Add(type);
            }

            foreach (TypeTypeID type in simTypes)
            {
                rufioTypes.Add(type);
            }

            String lastLid = (String)RegistryTools.GetSetting(HoodExporterApp.RegistryKey + @"\Options", menuLanguage.Name, Helper.Hex2PrefixString((int)MetaData.Languages.English));
            foreach (String lid in GameData.languagesByCode.Keys)
            {
                if (GameData.languagesByCode.TryGetValue(lid, out String lang))
                {
                    ToolStripMenuItem item = new ToolStripMenuItem();
                    menuLanguage.DropDownItems.Add(item);
                    item.Tag = lid;
                    item.Text = lang;
                    item.CheckOnClick = true;
                    item.Checked = lastLid.Equals(lid);
                    item.Click += new System.EventHandler(this.OnLangClicked);
                    item.Size = new System.Drawing.Size(180, 22);
                }
            }

            ParseXml("Resources/XML/npcs.xml", "npc", npcsByGuid);
            ParseTokens("Resources/XML/tokens.xml", tokenDataList);

            foreach (String xslt in Directory.GetFiles("Resources/XSL"))
            {
                FileInfo fi = new FileInfo(xslt);

                ToolStripMenuItem menuItemXslt = new ToolStripMenuItem
                {
                    Name = fi.Name,
                    Text = fi.Name,
                    Tag = fi.FullName,
                    Checked = false,
                    CheckOnClick = true,
                    CheckState = System.Windows.Forms.CheckState.Unchecked,
                    Size = new System.Drawing.Size(180, 22)
                };
                menuItemXslt.Click += new System.EventHandler(this.OnTransformClicked);
                menuTransform.DropDownItems.Add(menuItemXslt);

                if (Properties.Settings.Default.DefaultXslt.Equals(fi.Name))
                {
                    OnTransformClicked(menuItemXslt, null);
                }
            }
        }

        private void ParseXml(String xml, String element, Dictionary<TypeGUID, String> byValue)
        {
            XmlReader reader = XmlReader.Create(xml);

            TypeGUID value = DBPFData.GUID_NULL;
            String name = null;

            reader.MoveToContent();
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name.Equals("value"))
                    {
                        reader.Read();
                        value = (TypeGUID)UInt32.Parse(reader.Value.Substring(2), NumberStyles.HexNumber);
                    }
                    else if (reader.Name.Equals("name"))
                    {
                        reader.Read();
                        name = reader.Value;
                    }
                }
                else if (reader.NodeType == XmlNodeType.EndElement && reader.Name.Equals(element))
                {
                    byValue.Add(value, name);
                }
            }
        }

        private void ParseTokens(String xml, List<TokenData> tokenList)
        {
            XmlReader reader = XmlReader.Create(xml);

            reader.MoveToContent();
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name.Equals("token"))
                    {
                        String strGuid = reader.GetAttribute("guid");
                        String strProp = reader.GetAttribute("property");
                        String strEle = reader.GetAttribute("element");
                        String strAttr = reader.GetAttribute("attribute");

                        if (strGuid != null && strProp != null && strEle != null && strAttr != null)
                        {
                            if (strGuid.StartsWith("0x")) strGuid = strGuid.Substring(2);

                            try
                            {
                                TypeGUID guid = (TypeGUID)UInt32.Parse(strGuid, NumberStyles.HexNumber);
                                int prop = Int16.Parse(strProp, NumberStyles.Integer);

                                tokenList.Add(new TokenData(guid, prop, strEle, strAttr));
                            }
                            catch (Exception) { }
                        }
                    }
                }
            }
        }

        private void HoodWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs args)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            DirectoryInfo hoodDirInfo = new DirectoryInfo(textHoodPath.Text);
            String hoodDirPath = hoodDirInfo.FullName;

#if !DEBUG
            try
#endif
            {
                FileInfo[] files = hoodDirInfo.GetFiles("*_Neighborhood.package", SearchOption.TopDirectoryOnly);

                if (files.Length != 1)
                {
                    throw new Exception("Missing main neighborhood package file");
                }

                String hoodPackagePath = files[0].FullName;
                String hoodPackageName = files[0].Name;
                hoodCode = hoodPackageName.Substring(0, hoodPackageName.IndexOf("_"));

                isRufio = xsltPath.ToLower().Contains("rufio") && File.Exists(xsltPath);
                String exportSubDir = isRufio ? "Rufio" : hoodCode;

                exportPath = (new DirectoryInfo($"{textSavePath.Text}/{exportSubDir}")).FullName;

                if (exportPath.StartsWith(hoodDirPath))
                {
                    throw new Exception("Can't output under the neighborhood directory");
                }

                if (reuseExport && File.Exists(xsltPath) && !isRufio && File.Exists($"{exportPath}/{hoodCode}.xml"))
                {
                    // Reuse the previously exported .xml file
                    XmlDocument doc = new XmlDocument();
                    doc.Load($"{exportPath}/{hoodCode}.xml");
                    worker.ReportProgress(50);

                    // Run the transform
                    HoodExporterTransformer transformer = new HoodExporterTransformer(exportPath);
                    transformer.Transform((XmlElement)doc.DocumentElement.SelectSingleNode("/hood"), xsltPath);
                    worker.ReportProgress(100);
                }
                else
                {
                    // Create the .xml file
                    Directory.CreateDirectory(exportPath);

                    simCache = new SimDataCache();

                    startingPercent = 0;

                    XmlDocument doc = new XmlDocument();

                    XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                    XmlElement root = doc.DocumentElement;
                    doc.InsertBefore(xmlDeclaration, root);

                    DateTime now = DateTime.Now;
                    doc.AppendChild(doc.CreateComment($"{now.ToShortDateString()} {now.ToShortTimeString()}"));

                    XmlElement eleHood = doc.CreateElement(string.Empty, "hood", string.Empty);
                    doc.AppendChild(eleHood);

                    eleHood.SetAttribute("path", hoodDirPath);

                    if (!ProcessCharacterFiles(worker, processSimImagesPercent, hoodDirPath, exportPath) ||
                        !ProcessLotFiles(worker, processLotImagesPercent, hoodDirPath, exportPath) ||
                        !ProcessMainHood(worker, processMainHoodPercent, hoodDirInfo, hoodPackagePath, eleHood, "main", isRufio ? rufioTypes : mainTypes) ||
                        !ProcessSubHoods(worker, processSuburbHoodsPercent, hoodDirInfo, $"{hoodCode}_Suburb*.package", eleHood, "suburb", suburbTypes) ||
                        !ProcessSubHoods(worker, processUniversityHoodsPercent, hoodDirInfo, $"{hoodCode}_University*.package", eleHood, "university", universityTypes) ||
                        !ProcessSubHoods(worker, processDowntownHoodsPercent, hoodDirInfo, $"{hoodCode}_Downtown*.package", eleHood, "downtown", downtownTypes) ||
                        !ProcessSubHoods(worker, processVactionHoodsPercent, hoodDirInfo, $"{hoodCode}_Vacation*.package", eleHood, "vacation", vacationTypes) ||
                        !ProcessFamilyThumbnails(worker, processFamiyImagesPercent, hoodDirPath, exportPath)
                        )
                    {
                        args.Cancel = true;
                        return;
                    }

                    // Run any transform
                    if (File.Exists(xsltPath))
                    {
                        HoodExporterTransformer transformer = new HoodExporterTransformer(exportPath);
                        XmlElement eleXform = transformer.Transform(eleHood, xsltPath);

                        if (eleXform != null) eleHood = eleXform;
                    }

                    startingPercent += transformPercent;
                    worker.ReportProgress(startingPercent);

                    // Save the output
                    if (!isRufio)
                    {
                        using (StreamWriter writer = new StreamWriter($"{exportPath}/{hoodCode}.xml", false))
                        {
                            if (menuItemPrettyPrint.Checked)
                            {
                                writer.WriteLine(XDocument.Parse(eleHood.OwnerDocument.OuterXml).ToString());
                            }
                            else
                            {
                                writer.WriteLine(eleHood.OwnerDocument.OuterXml);
                            }
                            writer.Close();
                        }
                    }

                    startingPercent += outputPercent;
                    worker.ReportProgress(startingPercent);

                }
            }
#if !DEBUG
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Info(ex.StackTrace);

                if (MsgBox.Show($"An error occured while processing\n{hoodDirPath}\n\nReason: {ex.Message}", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                {
                    args.Cancel = true;
                    return;
                }
            }
#endif
        }

        private bool ProcessMainHood(BackgroundWorker worker, int percent, DirectoryInfo _/*hoodDirInfo*/, String mainPackagePath, XmlElement parent, String kind, List<TypeTypeID> types)
        {
            XmlElement eleMain = parent.OwnerDocument.CreateElement(kind);
            parent.AppendChild(eleMain);

            if (!ProcessHood(worker, percent, mainPackagePath, eleMain, types))
            {
                return false;
            }

            return true;
        }

        private bool ProcessSubHoods(BackgroundWorker worker, int percent, DirectoryInfo hoodDirInfo, String subhoodPackageFilter, XmlElement parent, String kind, List<TypeTypeID> types)
        {
            int savedStartingPercent = startingPercent;

            FileInfo[] subHoods = hoodDirInfo.GetFiles(subhoodPackageFilter, SearchOption.TopDirectoryOnly);
            foreach (FileInfo subhood in subHoods)
            {
                if (worker.CancellationPending)
                {
                    return false;
                }

                String subhoodPackagePath = subhood.FullName;

                XmlElement eleSubhood = parent.OwnerDocument.CreateElement(kind);
                parent.AppendChild(eleSubhood);

                if (!ProcessHood(worker, percent / subHoods.Length, subhoodPackagePath, eleSubhood, types))
                {
                    return false;
                }
            }

            startingPercent = savedStartingPercent + percent;
            worker.ReportProgress(startingPercent);
            return true;
        }

        private bool ProcessHood(BackgroundWorker worker, int percent, String packagePath, XmlElement parent, List<TypeTypeID> types)
        {
            parent.SetAttribute("package", (new FileInfo(packagePath)).Name);

            using (DBPFFile package = new DBPFFile(packagePath))
            {
                uint total = package.ResourceCount;
                uint done = 0;

                foreach (TypeTypeID type in types)
                {
                    List<DBPFEntry> resources = package.GetEntriesByType(type);
                    SortedDictionary<DBPFKey, DBPFEntry> sortedResources = new SortedDictionary<DBPFKey, DBPFEntry>();

                    foreach (DBPFEntry entry in resources)
                    {
                        try
                        {
                            sortedResources.Add(entry, entry);
                        }
                        catch (Exception)
                        {
                            MsgBox.Show($"The resource {entry} is duplicated - second occurrence has been ignored", "Duplicate Resource Found");
                        }
                    }

                    foreach (DBPFEntry entry in sortedResources.Values)
                    {
                        if (worker.CancellationPending)
                        {
                            return false;
                        }

                        DBPFResource resource = package.GetResourceByEntry(entry);

                        if (resource != null)
                        {
                            if (resource is Str)
                            {
                                (resource as Str).PrefLid = prefLid;
                            }

                            XmlElement element;
                            if (resource is Ngbh ngbh)
                            {
                                element = ngbh.AddXml(parent, menuItemLots.Checked, menuItemFamilies.Checked, menuItemSims.Checked);
                                simCache.AddTokens(ngbh, tokenDataList);
                            }
                            else
                            {
                                element = resource.AddXml(parent);
                            }

                            if (resource is Sdsc sdsc)
                            {
                                SimData simDataById = simCache.GetSimData(sdsc.InstanceID);
                                SimData simDataByGuid = simCache.GetSimData(sdsc.SimGuid);
                                if (simDataByGuid != null)
                                {

                                    XmlElement eleGivenName = parent.OwnerDocument.CreateElement("givenName");
                                    element.AppendChild(eleGivenName);
                                    XmlCDataSection cdata = parent.OwnerDocument.CreateCDataSection(simDataByGuid.GivenName);
                                    eleGivenName.AppendChild(cdata);

                                    XmlElement eleFamilyName = parent.OwnerDocument.CreateElement("familyName");
                                    element.AppendChild(eleFamilyName);
                                    cdata = parent.OwnerDocument.CreateCDataSection(simDataByGuid.FamilyName);
                                    eleFamilyName.AppendChild(cdata);

                                    XmlElement eleDescription = parent.OwnerDocument.CreateElement("description");
                                    element.AppendChild(eleDescription);
                                    cdata = parent.OwnerDocument.CreateCDataSection(simDataByGuid.Description);
                                    eleDescription.AppendChild(cdata);

                                    element.SetAttribute("characterFile", simDataByGuid.CharacterFile);
                                }
                                else if (npcsByGuid.ContainsKey(sdsc.SimGuid))
                                {
                                    if (npcsByGuid.TryGetValue(sdsc.SimGuid, out String npcName))
                                    {
                                        XmlElement eleGivenName = parent.OwnerDocument.CreateElement("givenName");
                                        element.AppendChild(eleGivenName);
                                        XmlCDataSection cdata = parent.OwnerDocument.CreateCDataSection(npcName);
                                        eleGivenName.AppendChild(cdata);
                                    }
                                }

                                foreach (XmlElement child in element.ChildNodes)
                                {
                                    if (child.Name.Equals("base"))
                                    {
                                        foreach (XmlElement basechild in child.ChildNodes)
                                        {
                                            if (basechild.Name.Equals("aspiration"))
                                            {
                                                uint aspPri = UInt32.Parse(basechild.GetAttribute("aspiration").Substring(2), NumberStyles.HexNumber);
                                                uint aspSec = 0;

                                                if (simDataById != null)
                                                {
                                                    aspSec = simDataById.SecondaryAspiration;
                                                    aspPri -= aspSec;
                                                }

                                                basechild.SetAttribute("aspirationPrimary", ((Sims2Tools.DBPF.Neighbourhood.AspirationTypes)aspPri).ToString());
                                                basechild.SetAttribute("aspirationSecondary", ((Sims2Tools.DBPF.Neighbourhood.AspirationTypes)aspSec).ToString());
                                            }
                                        }
                                    }
                                }

                                TokenDataCache simTokenCache = simDataById.TokenCache;
                                foreach (TokenData tokenData in tokenDataList)
                                {
                                    uint tokenValue = simTokenCache.GetValue(tokenData);

                                    XmlElement tokenElementParent = element;
                                    String tokenElementName = tokenData.ElementName;
                                    while (tokenElementName.Contains("/"))
                                    {
                                        String interElementName = tokenElementName.Substring(0, tokenElementName.IndexOf("/"));
                                        XmlElement interElement = null;
                                        foreach (XmlElement child in tokenElementParent.ChildNodes)
                                        {
                                            if (child.Name.Equals(interElementName))
                                            {
                                                interElement = child;
                                                break;
                                            }
                                        }

                                        if (interElement == null)
                                        {
                                            interElement = tokenElementParent.OwnerDocument.CreateElement(interElementName);
                                            tokenElementParent.AppendChild(interElement);
                                        }

                                        tokenElementParent = interElement;
                                        tokenElementName = tokenElementName.Substring(tokenElementName.IndexOf("/") + 1);
                                    }

                                    XmlElement tokenElement = null;
                                    foreach (XmlElement child in tokenElementParent.ChildNodes)
                                    {
                                        if (child.Name.Equals(tokenElementName))
                                        {
                                            tokenElement = child;
                                            break;
                                        }
                                    }

                                    if (tokenElement == null)
                                    {
                                        tokenElement = tokenElementParent.OwnerDocument.CreateElement(tokenElementName);
                                        tokenElementParent.AppendChild(tokenElement);
                                    }

                                    tokenElement.SetAttribute(tokenData.AttributeName, tokenValue.ToString());
                                }

                                if (isRufio)
                                {
                                    foreach (String simImage in Directory.GetFiles($"{exportPath}/SimImage", $"{sdsc.SimGuid}*"))
                                    {
                                        FileInfo fi = new FileInfo(simImage);
                                        String lifeStage = fi.Name.Substring(fi.Name.IndexOf("_") + 1);
                                        lifeStage = lifeStage.Substring(0, lifeStage.Length - 4);

                                        String suffix = lifeStage.Equals(sdsc.SimBase.LifeSection.ToString()) ? "" : $"_{lifeStage}";
                                        String newName = Path.Combine(fi.DirectoryName, $"{hoodCode}_{sdsc.InstanceID.AsUInt()}{suffix}{fi.Extension}");

                                        if (File.Exists(newName))
                                        {
                                            Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(newName, Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs, Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);
                                        }

                                        fi.MoveTo(newName);
                                    }
                                }
                            }

                            worker.ReportProgress(startingPercent + (int)((++done / (float)total) * (percent * 1.0)));
                        }
                    }
                }

                package.Close();
            }

            startingPercent += percent;
            worker.ReportProgress(startingPercent);
            return true;
        }

        private bool ProcessCharacterFiles(BackgroundWorker worker, int percent, String hoodDir, String outputPath)
        {
            DirectoryInfo charactersDir = new DirectoryInfo($"{hoodDir}/Characters");

            String extn = menuItemSaveAsPng.Checked ? "png" : "jpg";
            String simsSubDir = isRufio ? "SimImage" : "Sims";
            String simsPath = $"{outputPath}/{simsSubDir}";

            if (menuItemSimImages.Checked) Directory.CreateDirectory(simsPath);

            FileInfo[] characterFiles = charactersDir.GetFiles("*.package");
            int total = characterFiles.Length;
            int done = 0;

            foreach (FileInfo characterFile in characterFiles)
            {
                if (worker.CancellationPending)
                {
                    return false;
                }

                using (DBPFFile package = new DBPFFile(characterFile.FullName))
                {
                    List<DBPFEntry> objds = package.GetEntriesByType(Objd.TYPE);

                    if (objds.Count == 1)
                    {
                        List<DBPFEntry> ctsss = package.GetEntriesByType(Ctss.TYPE);

                        if (ctsss.Count == 1)
                        {
                            Objd objd = (Objd)package.GetResourceByEntry(objds[0]);
                            Ctss ctss = (Ctss)package.GetResourceByEntry(ctsss[0]);

                            StrItemList strs = ctss.LanguageItems(prefLid);
                            if (strs.Count == 0) strs = ctss.LanguageItems(MetaData.Languages.English);

                            if (strs.Count >= 2)
                            {
                                simCache.Add(objd.Guid, new SimData(objd.Guid, strs[0].Title, strs[2].Title, strs[1].Title, characterFile.Name));

                                if (menuItemSimImages.Checked)
                                {
                                    List<DBPFEntry> imgs = package.GetEntriesByType(Img.TYPE);

                                    foreach (DBPFEntry entry in package.GetEntriesByType(Jpg.TYPE))
                                    {
                                        imgs.Add(entry);
                                    }

                                    if (imgs.Count > 0)
                                    {
                                        int count = 0;

                                        foreach (DBPFEntry entry in imgs)
                                        {
                                            if (worker.CancellationPending)
                                            {
                                                return false;
                                            }

                                            int lifeStage = (int)entry.InstanceID.AsUInt();
                                            if (lifeStage >= 0x01 && lifeStage <= 0x40)
                                            {
                                                Img img = (Img)package.GetResourceByEntry(entry);

                                                String suffix;
                                                switch (lifeStage)
                                                {
                                                    case 0x20: suffix = "Baby"; break;
                                                    case 0x01: suffix = "Toddler"; break;
                                                    case 0x02: suffix = "Child"; break;
                                                    case 0x04: suffix = "Teen"; break;
                                                    case 0x40: suffix = "YoungAdult"; break;
                                                    case 0x08: suffix = "Adult"; break;
                                                    case 0x10: suffix = "Elder"; break;
                                                    default: suffix = "Unknown"; break;
                                                }

                                                String imageName = $"{objd.Guid}_{suffix}";

                                                using (Stream stream = new FileStream($"{simsPath}/{imageName}.{extn}", FileMode.OpenOrCreate, FileAccess.Write))
                                                {
                                                    if (menuItemSaveAsPng.Checked)
                                                    {
                                                        img.Image.Save(stream, ImageFormat.Png);
                                                    }
                                                    else
                                                    {
                                                        img.Image.Save(stream, ImageFormat.Jpeg);
                                                    }

                                                    stream.Close();
                                                    ++count;
                                                }
                                            }
                                        }

                                        if (count == 0)
                                        {
                                            logger.Warn($"{characterFile.Name} has no usable images");
                                        }
                                    }
                                    else
                                    {
                                        logger.Warn($"{characterFile.Name} has no images");
                                    }
                                }
                            }
                            else
                            {
                                logger.Warn($"{characterFile.Name} has an invalid CTSS entry");
                            }
                        }
                        else
                        {
                            logger.Warn($"{characterFile.Name} has no CTSS entry");
                        }
                    }
                    else
                    {
                        logger.Warn($"{characterFile.Name} has no OBJD entry");
                    }

                    package.Close();
                }

                worker.ReportProgress(startingPercent + (int)((++done / (float)total) * (percent * 1.0)));
            }

            startingPercent += percent;
            worker.ReportProgress(startingPercent);
            return true;
        }

        private bool ProcessLotFiles(BackgroundWorker worker, int percent, String hoodDir, String outputPath)
        {
            if (menuItemLotImages.Checked)
            {
                DirectoryInfo lotsDir = new DirectoryInfo($"{hoodDir}/Lots");

                String extn = menuItemSaveAsPng.Checked ? "png" : "jpg";
                String lotsSubDir = isRufio ? "LotImage" : "Lots";
                String lotsPath = $"{outputPath}/{lotsSubDir}";

                Directory.CreateDirectory(lotsPath);

                FileInfo[] lotFiles = lotsDir.GetFiles("*.package");
                int total = lotFiles.Length;
                int done = 0;

                foreach (FileInfo lotFile in lotFiles)
                {
                    if (worker.CancellationPending)
                    {
                        return false;
                    }

                    using (DBPFFile package = new DBPFFile(lotFile.FullName))
                    {
                        // if (menuItemLotImages.Checked)
                        {
                            List<DBPFEntry> imgs = package.GetEntriesByType(Img.TYPE);

                            foreach (DBPFEntry entry in package.GetEntriesByType(Jpg.TYPE))
                            {
                                imgs.Add(entry);
                            }

                            int count = 0;

                            foreach (DBPFEntry entry in imgs)
                            {
                                if (worker.CancellationPending)
                                {
                                    return false;
                                }

                                if (entry.InstanceID == (TypeInstanceID)0x35CA0002)
                                {
                                    Match m = LotRegex.Match(lotFile.Name);
                                    if (m.Success)
                                    {
                                        uint uid = UInt32.Parse(m.Groups[1].Value);

                                        Img img = (Img)package.GetResourceByEntry(entry);
                                        String imageName = isRufio ? $"{hoodCode}_{uid}" : Helper.Hex8PrefixString(uid);

                                        using (Stream stream = new FileStream($"{lotsPath}/{imageName}.{extn}", FileMode.OpenOrCreate, FileAccess.Write))
                                        {
                                            if (menuItemSaveAsPng.Checked)
                                            {
                                                img.Image.Save(stream, ImageFormat.Png);
                                            }
                                            else
                                            {
                                                img.Image.Save(stream, ImageFormat.Jpeg);
                                            }

                                            stream.Close();
                                            ++count;
                                        }
                                    }
                                }
                            }

                            if (count == 0)
                            {
                                logger.Warn($"{lotFile.Name} has no usable image");
                            }
                        }

                        package.Close();
                    }

                    worker.ReportProgress(startingPercent + (int)((++done / (float)total) * (percent * 1.0)));
                }
            }

            startingPercent += percent;
            worker.ReportProgress(startingPercent);
            return true;
        }

        private bool ProcessFamilyThumbnails(BackgroundWorker worker, int percent, String hoodDir, String outputPath)
        {
            if (menuItemFamilyImages.Checked)
            {
                DirectoryInfo thumbnailsDir = new DirectoryInfo($"{hoodDir}/Thumbnails");

                String extn = menuItemSaveAsPng.Checked ? "png" : "jpg";
                String familiesSubDir = isRufio ? "FamilyImage" : "Families";
                String familiesPath = $"{outputPath}/{familiesSubDir}";

                Directory.CreateDirectory(familiesPath);

                foreach (FileInfo thumbnailFile in thumbnailsDir.GetFiles("*_FamilyThumbnails.package"))
                {
                    using (DBPFFile package = new DBPFFile(thumbnailFile.FullName))
                    {
                        List<DBPFEntry> imgs = package.GetEntriesByType(Img.TYPE);

                        foreach (DBPFEntry entry in package.GetEntriesByType(Jpg.TYPE))
                        {
                            imgs.Add(entry);
                        }

                        uint total = package.ResourceCount;
                        uint done = 0;

                        foreach (DBPFEntry entry in imgs)
                        {
                            if (worker.CancellationPending)
                            {
                                return false;
                            }

                            Img img = (Img)package.GetResourceByEntry(entry);
                            String imageName = isRufio ? $"{hoodCode}_{entry.InstanceID.AsUInt()}" : entry.InstanceID.ToString();

                            using (Stream stream = new FileStream($"{familiesPath}/{imageName}.{extn}", FileMode.OpenOrCreate, FileAccess.Write))
                            {
                                if (menuItemSaveAsPng.Checked)
                                {
                                    img.Image.Save(stream, ImageFormat.Png);
                                }
                                else
                                {
                                    img.Image.Save(stream, ImageFormat.Jpeg);
                                }

                                stream.Close();
                            }

                            worker.ReportProgress(startingPercent + (int)((++done / (float)total) * (percent * 1.0)));
                        }

                        package.Close();
                    }
                }
            }

            startingPercent += percent;
            worker.ReportProgress(startingPercent);
            return true;
        }

        private void HoodWorker_Progress(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage != 0)
            {
                progressBar.Value = e.ProgressPercentage;
            }
        }

        private void HoodWorker_Completed(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            lblProgress.Visible = false;
            progressBar.Visible = false;

            if (e.Error != null)
            {
                MyMruList.RemoveFile(textHoodPath.Text);
                textHoodPath.Text = "";

                logger.Error(e.Error.Message);
                logger.Info(e.Error.StackTrace);

                MsgBox.Show("An error occured while scanning", "Error!", MessageBoxButtons.OK);
            }
            else
            {
                MyMruList.AddFile(textHoodPath.Text);

                if (e.Cancelled == true)
                {
                    lblProgress.Text = $"ERROR!";
                    lblProgress.Visible = true;
                }
                else
                {
                    lblProgress.Text = $"SUCCESS!";
                    lblProgress.Visible = true;
                }
            }

            btnGO.Text = "&EXPORT";
        }

        private void OnGoClicked(object sender, System.EventArgs e)
        {
            if (hoodWorker.IsBusy)
            {
                // This is the Cancel action
                Debug.Assert(hoodWorker.WorkerSupportsCancellation == true);

                // Cancel the asynchronous operation.
                hoodWorker.CancelAsync();
            }
            else
            {
                reuseExport = ((Control.ModifierKeys & Keys.Shift) == Keys.Shift);

                // This is the Export action
                btnGO.Text = "Cancel";

                lblProgress.Text = "Progress:";
                lblProgress.Visible = true;
                progressBar.Visible = true;
                progressBar.Value = 0;

                hoodWorker.RunWorkerAsync();
            }
        }

        private void MyMruList_FileSelected(String folder)
        {
            textHoodPath.Text = folder;
        }

        private void OnLoad(object sender, System.EventArgs e)
        {
            RegistryTools.LoadAppSettings(HoodExporterApp.RegistryKey, HoodExporterApp.AppVersionMajor, HoodExporterApp.AppVersionMinor);
            RegistryTools.LoadFormSettings(HoodExporterApp.RegistryKey, this);
            textHoodPath.Text = RegistryTools.GetSetting(HoodExporterApp.RegistryKey, textHoodPath.Name, "") as String;
            textSavePath.Text = RegistryTools.GetSetting(HoodExporterApp.RegistryKey, textSavePath.Name, "") as String;

            MyMruList = new MruList(HoodExporterApp.RegistryKey, selectRecentHoodsToolStripMenuItem, Properties.Settings.Default.MruSize, false, true);
            MyMruList.FileSelected += MyMruList_FileSelected;

            menuItemLots.Checked = ((int)RegistryTools.GetSetting(HoodExporterApp.RegistryKey + @"\Resources", menuItemLots.Name, 1) != 0); OnLotsClicked(menuItemLots, null);
            menuItemSims.Checked = ((int)RegistryTools.GetSetting(HoodExporterApp.RegistryKey + @"\Resources", menuItemSims.Name, 1) != 0); OnSimsClicked(menuItemSims, null);
            menuItemFamilies.Checked = ((int)RegistryTools.GetSetting(HoodExporterApp.RegistryKey + @"\Resources", menuItemFamilies.Name, 1) != 0); OnFamiliesClicked(menuItemFamilies, null);

            menuItemPrettyPrint.Checked = ((int)RegistryTools.GetSetting(HoodExporterApp.RegistryKey + @"\Options", menuItemPrettyPrint.Name, 1) != 0);

            menuItemSaveAsJpg.Checked = ((int)RegistryTools.GetSetting(HoodExporterApp.RegistryKey + @"\Options", menuItemSaveAsJpg.Name, 0) != 0);
            menuItemSaveAsPng.Checked = ((int)RegistryTools.GetSetting(HoodExporterApp.RegistryKey + @"\Options", menuItemSaveAsPng.Name, 1) != 0);

            menuItemSimImages.Checked = ((int)RegistryTools.GetSetting(HoodExporterApp.RegistryKey + @"\Options", menuItemSimImages.Name, 1) != 0);
            menuItemLotImages.Checked = ((int)RegistryTools.GetSetting(HoodExporterApp.RegistryKey + @"\Options", menuItemLotImages.Name, 1) != 0);
            menuItemFamilyImages.Checked = ((int)RegistryTools.GetSetting(HoodExporterApp.RegistryKey + @"\Options", menuItemFamilyImages.Name, 1) != 0);

            MyUpdater = new Updater(HoodExporterApp.RegistryKey, menuHelp);
            MyUpdater.CheckForUpdates();
        }

        private void OnExitClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            RegistryTools.SaveAppSettings(HoodExporterApp.RegistryKey, HoodExporterApp.AppVersionMajor, HoodExporterApp.AppVersionMinor);
            RegistryTools.SaveFormSettings(HoodExporterApp.RegistryKey, this);
            RegistryTools.SaveSetting(HoodExporterApp.RegistryKey, textHoodPath.Name, textHoodPath.Text);
            RegistryTools.SaveSetting(HoodExporterApp.RegistryKey, textSavePath.Name, textSavePath.Text);

            RegistryTools.SaveSetting(HoodExporterApp.RegistryKey + @"\Options", menuItemPrettyPrint.Name, menuItemPrettyPrint.Checked ? 1 : 0);

            RegistryTools.SaveSetting(HoodExporterApp.RegistryKey + @"\Options", menuItemSaveAsPng.Name, menuItemSaveAsPng.Checked ? 1 : 0);
            RegistryTools.SaveSetting(HoodExporterApp.RegistryKey + @"\Options", menuItemSaveAsJpg.Name, menuItemSaveAsJpg.Checked ? 1 : 0);

            RegistryTools.SaveSetting(HoodExporterApp.RegistryKey + @"\Options", menuItemSimImages.Name, menuItemSimImages.Checked ? 1 : 0);
            RegistryTools.SaveSetting(HoodExporterApp.RegistryKey + @"\Options", menuItemLotImages.Name, menuItemLotImages.Checked ? 1 : 0);
            RegistryTools.SaveSetting(HoodExporterApp.RegistryKey + @"\Options", menuItemFamilyImages.Name, menuItemFamilyImages.Checked ? 1 : 0);

            HoodExporterTransformer.Shutdown();
        }

        private void OnSelectHoodPathClicked(object sender, EventArgs e)
        {
            selectPathDialog.InitialDirectory = textHoodPath.Text;

            if (selectPathDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                textHoodPath.Text = selectPathDialog.FileName;
            }
        }

        private void OnSelectSavePathClicked(object sender, EventArgs e)
        {
            selectPathDialog.InitialDirectory = textSavePath.Text;

            if (selectPathDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                textSavePath.Text = selectPathDialog.FileName;
            }
        }

        private void OnPathsChanged(object sender, EventArgs e)
        {
            UpdateForm();
        }

        private void UpdateForm()
        {
            btnGO.Enabled = (textHoodPath.Text.Length > 0 && textSavePath.Text.Length > 0);
            lblProgress.Visible = false;
        }

        private void OnHelpClicked(object sender, EventArgs e)
        {
            new Sims2ToolsAboutDialog(HoodExporterApp.AppProduct).ShowDialog();
        }

        private void OnConfigClicked(object sender, EventArgs e)
        {
            Form config = new Sims2ToolsConfigDialog();

            if (config.ShowDialog() == DialogResult.OK)
            {
            }
        }

        private void OnLotsClicked(object sender, EventArgs e)
        {
            bool enabled = ((ToolStripMenuItem)sender).Checked;

            if (enabled)
            {
                foreach (TypeTypeID type in lotTypes)
                {
                    if (!mainTypes.Contains(type)) mainTypes.Add(type);
                    if (!suburbTypes.Contains(type)) suburbTypes.Add(type);
                    if (!universityTypes.Contains(type)) universityTypes.Add(type);
                    if (!downtownTypes.Contains(type)) downtownTypes.Add(type);
                    if (!vacationTypes.Contains(type)) vacationTypes.Add(type);
                }
            }
            else
            {
                foreach (TypeTypeID type in lotTypes)
                {
                    mainTypes.Remove(type);
                    suburbTypes.Remove(type);
                    universityTypes.Remove(type);
                    downtownTypes.Remove(type);
                    vacationTypes.Remove(type);
                }
            }

            RegistryTools.SaveSetting(HoodExporterApp.RegistryKey + @"\Resources", menuItemLots.Name, enabled ? 1 : 0);
        }

        private void OnSimsClicked(object sender, EventArgs e)
        {
            bool enabled = ((ToolStripMenuItem)sender).Checked;

            if (enabled)
            {
                foreach (TypeTypeID type in simTypes)
                {
                    if (!mainTypes.Contains(type)) mainTypes.Add(type);
                }
            }
            else
            {
                foreach (TypeTypeID type in simTypes)
                {
                    mainTypes.Remove(type);
                }
            }

            RegistryTools.SaveSetting(HoodExporterApp.RegistryKey + @"\Resources", menuItemSims.Name, enabled ? 1 : 0);
        }

        private void OnFamiliesClicked(object sender, EventArgs e)
        {
            bool enabled = ((ToolStripMenuItem)sender).Checked;

            if (enabled)
            {
                foreach (TypeTypeID type in familyTypes)
                {
                    if (!mainTypes.Contains(type)) mainTypes.Add(type);
                }
            }
            else
            {
                foreach (TypeTypeID type in familyTypes)
                {
                    mainTypes.Remove(type);
                }
            }

            RegistryTools.SaveSetting(HoodExporterApp.RegistryKey + @"\Resources", menuItemFamilies.Name, enabled ? 1 : 0);
        }

        private void OnSaveAsPngClicked(object sender, EventArgs e)
        {
            menuItemSaveAsJpg.Checked = !menuItemSaveAsPng.Checked;
        }

        private void OnSaveAsJpgClicked(object sender, EventArgs e)
        {
            menuItemSaveAsPng.Checked = !menuItemSaveAsJpg.Checked;
        }

        private void OnLangClicked(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;

            try
            {
                prefLid = (MetaData.Languages)Convert.ToInt16(menuItem.Tag as String, 16);

                RegistryTools.SaveSetting(HoodExporterApp.RegistryKey + @"\Options", menuLanguage.Name, menuItem.Tag);

                foreach (ToolStripMenuItem otherItem in menuLanguage.DropDownItems)
                {
                    if (otherItem != menuItem)
                    {
                        otherItem.Checked = false;
                    }
                }
            }
            catch (Exception) { }
        }

        private void OnTransformClicked(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;

            foreach (ToolStripItem item in menuTransform.DropDownItems)
            {
                if (item is ToolStripMenuItem menu)
                {
                    menu.Checked = (menu == menuItem);
                }
            }

            xsltPath = menuItem.Tag as String;

            if (menuItem == menuItemXsltNone)
            {
                this.Text = $"{HoodExporterApp.AppName}";
            }
            else
            {
                this.Text = $"{HoodExporterApp.AppName} - {menuItem.Text}";
            }
        }
    }
}
