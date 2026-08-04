/*
 * Family Manager - a utility for manipulating family closets
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

#region Usings
using FamilyManager.Caching;
using Microsoft.WindowsAPICodePack.Dialogs;
using Sims2Tools;
using Sims2Tools.Cache;
using Sims2Tools.Controls;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.CTSS;
using Sims2Tools.DBPF.Data;
using Sims2Tools.DBPF.Neighbourhood;
using Sims2Tools.DBPF.Neighbourhood.FAMI;
using Sims2Tools.DBPF.Neighbourhood.SDSC;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph.BINX;
using Sims2Tools.DBPF.SceneGraph.COLL;
using Sims2Tools.DBPF.SceneGraph.GZPS;
using Sims2Tools.DBPF.SceneGraph.IDR;
using Sims2Tools.DBPF.SceneGraph.XMOL;
using Sims2Tools.DBPF.STR;
using Sims2Tools.DBPF.Utils;
using Sims2Tools.DbpfCache;
using Sims2Tools.Dialogs;
using Sims2Tools.Helpers;
using Sims2Tools.Updates;
using Sims2Tools.Utils.NamedValue;
using Sims2Tools.Utils.Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
#endregion

namespace FamilyManager
{
    public partial class FamilyManagerForm : Form
    {
        private static readonly Sims2Tools.DBPF.Logger.IDBPFLogger logger = Sims2Tools.DBPF.Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const MetaData.Languages defLid = MetaData.Languages.Default;
        private static MetaData.Languages prefLid = defLid;

        private readonly DbpfFileCache packageCache = new DbpfFileCache();

        private Updater MyUpdater;

        private static readonly Color colourSplitFileHighlight = Color.FromName(Properties.Settings.Default.SplitFileHighlight);
        private static readonly Color colourThumbnailBackground = Color.FromName(Properties.Settings.Default.ThumbnailBackground);
        private static readonly Color colourValidationError = Color.FromName(Properties.Settings.Default.ValidationError);

        private bool cachesLoaded = false;
        private readonly ClothingThumbnailsCache clothingThumbnailsCache = new ClothingThumbnailsCache();

        private readonly FamilyGridData dataFamilyMembers = new FamilyGridData();

        private readonly OutfitGridData dataFamilyCloset = new OutfitGridData();
        private readonly OutfitGridData dataSuitcase = new OutfitGridData();

        private readonly OutfitGridData dataFamilySafe = new OutfitGridData();
        private readonly OutfitGridData dataJewelbox = new OutfitGridData();

        private HoodTreeNode lastHoodNode = null;
        private FamilyTreeNode lastFamilyNode = null;

        private FamilyData currentFamily = null;

        private readonly CharacterCache characterCache = new CharacterCache();
        private readonly Dictionary<uint, TypeInstanceID> sdscInstanceBySimGuid = new Dictionary<uint, TypeInstanceID>();

        private readonly CareerCache careerCache;

        private readonly OutfitCache clothingCache;
        private readonly OutfitCache jewelleryCache;

        private readonly Filter filters = new Filter();

        InterestTrackerStyle interestsTrackersStyle = InterestTrackerStyle.BarAndBox;


        public bool IsAdvancedMode => Sims2ToolsLib.AllAdvancedMode || menuItemAdvanced.Checked;

        #region Constructor and TidyUp
        public FamilyManagerForm()
        {
            logger.Info(FamilyManagerApp.AppProduct);

            InitializeComponent();
            SetTitle();

            trackSkillToddlerWalk.Maximum = Properties.Settings.Default.MaxSkillWalk;
            trackSkillToddlerTalk.Maximum = Properties.Settings.Default.MaxSkillTalk;
            trackSkillToddlerRhyming.Maximum = Properties.Settings.Default.MaxSkillRhyming;
            trackSkillToddlerPotty.Maximum = Properties.Settings.Default.MaxSkillPotty;

            trackSkillHiddenBreakDance.Maximum = Properties.Settings.Default.MaxSkillBreakDance;
            trackSkillHiddenDance.Maximum = Properties.Settings.Default.MaxSkillDance;
            trackSkillHiddenFireDance.Maximum = Properties.Settings.Default.MaxSkillFireDance;
            trackSkillHiddenMeditate.Maximum = Properties.Settings.Default.MaxSkillMeditate;
            trackSkillHiddenPool.Maximum = Properties.Settings.Default.MaxSkillPool;
            trackSkillHiddenStudy.Maximum = Properties.Settings.Default.MaxSkillStudy;
            trackSkillHiddenTaiChi.Maximum = Properties.Settings.Default.MaxSkillTaiChi;

            FamilyDbpfData.SetCache(packageCache);
            CharacterCache.SetCache(packageCache);
            OutfitDbpfData.SetCache(packageCache);

            clothingCache = new OutfitCache(DataCache.CacheClothesPath, DataCache.MaxisClothingFilename, DataCache.CustomClothingFilename);
            jewelleryCache = new OutfitCache(DataCache.CacheJewelleryPath, DataCache.MaxisJewelleryFilename, DataCache.CustomJewelleryFilename);

            careerCache = new CareerCache(DataCache.CacheCareersPath, DataCache.CustomCareerFilename, DataCache.CustomCareerOverrideFilename);
            textUniTimeLeft.Maximum = careerCache.SemesterLength;
            trackUniTimeLeft.Maximum = (int)textUniTimeLeft.Maximum;

            selectPathDialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true
            };

            gridFamilyMembers.DataSource = dataFamilyMembers;

            gridFamilyCloset.DataSource = dataFamilyCloset;
            gridSuitcase.DataSource = dataSuitcase;

            gridFamilySafe.DataSource = dataFamilySafe;
            gridJewelbox.DataSource = dataJewelbox;

            thumbBox.BackColor = colourThumbnailBackground;
        }

        public void TidyUp()
        {
            clothingThumbnailsCache.Close();
        }
        #endregion

        #region Career (Schools/Majors/Jobs) Combo Box Loading
        private void LoadCareers()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { LoadCareers(); });
                return;
            }

            // This will be run on the main (UI) thread 
            LoadSchools();
            LoadMajors();

            CareerTypes wantedJobs = lastJobs;
            lastJobs = CareerTypes.Unknown;
            switch (wantedJobs)
            {
                case CareerTypes.TeenJob:
                    LoadTeenJobs();
                    break;
                case CareerTypes.AdultJob:
                    LoadAdultJobs();
                    break;
                case CareerTypes.ElderJob:
                    LoadElderJobs();
                    break;
                case CareerTypes.PetJob:
                    LoadPetJobs();
                    break;
            }

            LoadRetiredJobs();
        }

        private void LoadSchools()
        {
            SortedDictionary<string, uint> schools = new SortedDictionary<string, uint>(new CareerNameComparer())
            {
                { "Public", (uint)SchoolTypes.PublicSchool },
                { "Private", (uint)SchoolTypes.PrivateSchool }
            };

            LoadCustomCareers(schools, CareerTypes.School);

            comboSchoolType.Items.Clear();
            comboSchoolType.Items.Add(new UintNamedValue("Unknown", (uint)SchoolTypes.NoSchool));

            foreach (string school in schools.Keys)
            {
                comboSchoolType.Items.Add(new UintNamedValue(school, schools[school]));
            }

            ControlHelper.SetDropDownWidth(comboSchoolType);
        }

        private void LoadSchoolGrades()
        {
            comboSchoolGrade.Items.Clear();
            comboSchoolGrade.Items.Add(new UintNamedValue("Unknown", (uint)Grades.Unknown));
            comboSchoolGrade.Items.Add(new UintNamedValue("A+", (uint)Grades.APlus));
            comboSchoolGrade.Items.Add(new UintNamedValue("A", (uint)Grades.A));
            comboSchoolGrade.Items.Add(new UintNamedValue("A-", (uint)Grades.AMinus));
            comboSchoolGrade.Items.Add(new UintNamedValue("B+", (uint)Grades.BPlus));
            comboSchoolGrade.Items.Add(new UintNamedValue("B", (uint)Grades.B));
            comboSchoolGrade.Items.Add(new UintNamedValue("B-", (uint)Grades.BMinus));
            comboSchoolGrade.Items.Add(new UintNamedValue("C+", (uint)Grades.CPlus));
            comboSchoolGrade.Items.Add(new UintNamedValue("C", (uint)Grades.C));
            comboSchoolGrade.Items.Add(new UintNamedValue("C-", (uint)Grades.CMinus));
            comboSchoolGrade.Items.Add(new UintNamedValue("D+", (uint)Grades.DPlus));
            comboSchoolGrade.Items.Add(new UintNamedValue("D", (uint)Grades.D));
            comboSchoolGrade.Items.Add(new UintNamedValue("D-", (uint)Grades.DMinus));
            comboSchoolGrade.Items.Add(new UintNamedValue("F", (uint)Grades.F));
        }

        private void LoadMajors()
        {
            SortedDictionary<string, uint> majors = new SortedDictionary<string, uint>(new CareerNameComparer())
            {
                { "Art", (uint)Majors.Art },
                { "Biology", (uint)Majors.Biology },
                { "Drama", (uint)Majors.Drama },
                { "Economics", (uint)Majors.Economics },
                { "History", (uint)Majors.History },
                { "Literature", (uint)Majors.Literature },
                { "Mathematics", (uint)Majors.Mathematics },
                { "Philosophy", (uint)Majors.Philosophy },
                { "Physics", (uint)Majors.Physics },
                { "Political Science", (uint)Majors.PoliticalScience },
                { "Psychology", (uint)Majors.Psychology }
            };

            LoadCustomCareers(majors, CareerTypes.Major);

            comboUniMajor.Items.Clear();
            comboUniMajor.Items.Add(new UintNamedValue("Unknown", (uint)Majors.Unknown));
            comboUniMajor.Items.Add(new UintNamedValue("Undeclared", (uint)Majors.Undeclared));

            foreach (string major in majors.Keys)
            {
                comboUniMajor.Items.Add(new UintNamedValue(major, majors[major]));
            }

            ControlHelper.SetDropDownWidth(comboUniMajor);
        }

        private void LoadSemesters()
        {
            comboUniSemester.Items.Clear();
            comboUniSemester.Items.Add(new UintNamedValue("Unknown", 0));
            comboUniSemester.Items.Add(new UintNamedValue("1 - Freshman 1", 1));
            comboUniSemester.Items.Add(new UintNamedValue("2 - Freshman 2", 2));
            comboUniSemester.Items.Add(new UintNamedValue("3 - Sophomore 1", 3));
            comboUniSemester.Items.Add(new UintNamedValue("4 - Sophomore 2", 4));
            comboUniSemester.Items.Add(new UintNamedValue("5 - Junior 1", 5));
            comboUniSemester.Items.Add(new UintNamedValue("6 - Junior 2", 6));
            comboUniSemester.Items.Add(new UintNamedValue("7 - Senior 1", 7));
            comboUniSemester.Items.Add(new UintNamedValue("8 - Senior 2", 8));

            ControlHelper.SetDropDownWidth(comboUniSemester);
        }

        private CareerTypes lastJobs = CareerTypes.Unknown;

        private void LoadTeenJobs()
        {
            if (lastJobs == CareerTypes.TeenJob) return;
            lastJobs = CareerTypes.TeenJob;

            SortedDictionary<string, uint> jobs = new SortedDictionary<string, uint>(new CareerNameComparer());

            LoadMaxisCareers(jobs, CareerTypes.TeenOrElderJob);
            LoadCustomCareers(jobs, CareerTypes.TeenOrElderJob);

            // Teens (for example, with InTeen) can have adult jobs
            if (menuItemTeensHaveAdultJobs.Checked)
            {
                LoadMaxisCareers(jobs, CareerTypes.AdultJob);
                LoadCustomCareers(jobs, CareerTypes.AdultJob);
            }

            comboJobType.Items.Clear();
            comboJobType.Items.Add(new UintNamedValue("Unemployed", (uint)Careers.Unemployed));
            comboJobType.Items.Add(new UintNamedValue("Unknown", (uint)Careers.Unknown));

            foreach (string job in jobs.Keys)
            {
                comboJobType.Items.Add(new UintNamedValue(job, jobs[job]));
            }

            ControlHelper.SetDropDownWidth(comboJobType);
        }

        private void LoadAdultJobs()
        {
            if (lastJobs == CareerTypes.AdultJob) return;
            lastJobs = CareerTypes.AdultJob;

            SortedDictionary<string, uint> jobs = new SortedDictionary<string, uint>(new CareerNameComparer());

            LoadMaxisCareers(jobs, CareerTypes.AdultJob);
            LoadCustomCareers(jobs, CareerTypes.AdultJob);

            comboJobType.Items.Clear();
            comboJobType.Items.Add(new UintNamedValue("Unemployed", (uint)Careers.Unemployed));
            comboJobType.Items.Add(new UintNamedValue("Unknown", (uint)Careers.Unknown));

            foreach (string job in jobs.Keys)
            {
                comboJobType.Items.Add(new UintNamedValue(job, jobs[job]));
            }

            ControlHelper.SetDropDownWidth(comboJobType);
        }

        private void LoadElderJobs()
        {
            if (lastJobs == CareerTypes.ElderJob) return;
            lastJobs = CareerTypes.ElderJob;

            SortedDictionary<string, uint> jobs = new SortedDictionary<string, uint>(new CareerNameComparer());

            LoadMaxisCareers(jobs, CareerTypes.TeenOrElderJob);
            LoadCustomCareers(jobs, CareerTypes.TeenOrElderJob);

            // Elders that haven't retired can still have adult jobs
            LoadMaxisCareers(jobs, CareerTypes.AdultJob);
            LoadCustomCareers(jobs, CareerTypes.AdultJob);

            comboJobType.Items.Clear();
            comboJobType.Items.Add(new UintNamedValue("Unemployed", (uint)Careers.Unemployed));
            comboJobType.Items.Add(new UintNamedValue("Unknown", (uint)Careers.Unknown));

            foreach (string job in jobs.Keys)
            {
                comboJobType.Items.Add(new UintNamedValue(job, jobs[job]));
            }

            ControlHelper.SetDropDownWidth(comboJobType);
        }

        private void LoadRetiredJobs()
        {
            SortedDictionary<string, uint> jobs = new SortedDictionary<string, uint>(new CareerNameComparer());

            LoadMaxisCareers(jobs, CareerTypes.AdultJob);
            LoadCustomCareers(jobs, CareerTypes.AdultJob);

            comboJobRetiredType.Items.Clear();
            comboJobRetiredType.Items.Add(new UintNamedValue("Unemployed", (uint)Careers.Unemployed));

            foreach (string job in jobs.Keys)
            {
                comboJobRetiredType.Items.Add(new UintNamedValue(job, jobs[job]));
            }

            ControlHelper.SetDropDownWidth(comboJobRetiredType);
        }

        private void LoadPetJobs()
        {
            if (lastJobs == CareerTypes.PetJob) return;
            lastJobs = CareerTypes.PetJob;

            SortedDictionary<string, uint> jobs = new SortedDictionary<string, uint>(new CareerNameComparer())
            {
                { "Security", (uint)Careers.PetSecurity },
                { "Service", (uint)Careers.PetService },
                { "Show Biz", (uint)Careers.PetShowBiz },
            };

            LoadCustomCareers(jobs, CareerTypes.PetJob);

            comboJobType.Items.Clear();
            comboJobType.Items.Add(new UintNamedValue("Unemployed", (uint)Careers.Unemployed));
            comboJobType.Items.Add(new UintNamedValue("Unknown", (uint)Careers.Unknown));

            foreach (string job in jobs.Keys)
            {
                comboJobType.Items.Add(new UintNamedValue(job, jobs[job]));
            }

            ControlHelper.SetDropDownWidth(comboJobType);
        }

        private readonly Dictionary<string, uint> adultJobs = new Dictionary<string, uint>()
        {
            { "Adventurer", (uint)Careers.Adventurer },
            { "Architecture", (uint)Careers.Architecture },
            { "Athletic", (uint)Careers.Athletic },
            { "Artist", (uint)Careers.Artist },
            { "Business", (uint)Careers.Business },
            { "Criminal", (uint)Careers.Criminal },
            { "Culinary", (uint)Careers.Culinary },
            { "Dance", (uint)Careers.Dance },
            { "Education", (uint)Careers.Education },
            { "Entertainment", (uint)Careers.Entertainment },
            { "Gamer", (uint)Careers.Gamer },
            { "Intelligence", (uint)Careers.Intelligence },
            { "Journalism", (uint)Careers.Journalism },
            { "Law", (uint)Careers.Law },
            { "Law Enforcement", (uint) Careers.LawEnforcement },
            { "Medicine", (uint) Careers.Medicine },
            { "Military", (uint)Careers.Military },
            { "Music", (uint)Careers.Music },
            { "Natural Scientist", (uint)Careers.NaturalScientist },
            { "Ocenography", (uint)Careers.Ocenography },
            { "Paranormal", (uint)Careers.Paranormal },
            { "Politics", (uint)Careers.Politics },
            { "Science", (uint)Careers.Science },
            { "Show Biz", (uint)Careers.ShowBiz },
            { "Slacker", (uint)Careers.Slacker }
        };

        private readonly Dictionary<string, uint> teenElderJobs = new Dictionary<string, uint>()
        {
            { "Adventurer", (uint)Careers.TeenElderAdventurer },
            { "Architecture", (uint)Careers.TeenElderArchitecture },
            { "Athletic", (uint)Careers.TeenElderAthletic },
            { "Business", (uint)Careers.TeenElderBusiness },
            { "Criminal", (uint)Careers.TeenElderCriminal },
            { "Culinary", (uint)Careers.TeenElderCulinary },
            { "Dance", (uint)Careers.TeenElderDance },
            { "Education", (uint)Careers.TeenElderEducation },
            { "Entertainment", (uint)Careers.TeenElderEntertainment },
            { "Gamer", (uint)Careers.TeenElderGamer },
            { "Intelligence", (uint)Careers.TeenElderIntelligence },
            { "Journalism", (uint)Careers.TeenElderJournalism },
            { "Law", (uint) Careers.TeenElderLaw },
            { "Law Enforcement", (uint) Careers.TeenElderLawEnforcement },
            { "Medicine", (uint) Careers.TeenElderMedicine },
            { "Military", (uint)Careers.TeenElderMilitary },
            { "Music", (uint)Careers.TeenElderMusic },
            { "Ocenography", (uint)Careers.TeenElderOcenography },
            { "Politics", (uint)Careers.TeenElderPolitics },
            { "Science", (uint)Careers.TeenElderScience },
            { "Slacker", (uint)Careers.TeenElderSlacker }
        };

        private void LoadMaxisCareers(SortedDictionary<string, uint> careers, CareerTypes type)
        {
            Dictionary<string, uint> jobs = ((type == CareerTypes.AdultJob) ? adultJobs : teenElderJobs);

            string suffix = (type == CareerTypes.AdultJob) ? "" : " (T/E)";

            foreach (string key in jobs.Keys)
            {
                string name = $"{key}{suffix}";
                careers.Remove(name);
                careers.Add(name, jobs[key]);
            }
        }

        private void LoadCustomCareers(SortedDictionary<string, uint> careers, CareerTypes type)
        {
            string suffix = (type == CareerTypes.TeenJob || type == CareerTypes.ElderJob || type == CareerTypes.TeenOrElderJob) ? " (T/E)" : "";

            foreach (CareerData career in careerCache)
            {
                if (career.CareerType == type)
                {
                    careers.Remove($"{career.Name}{suffix}");
                    careers.Add($"*{career.Name}{suffix}", career.Guid.AsUInt());
                }
            }
        }
        #endregion

        #region Interests (Interests/Hobbies/Badges) Combo Box Loading
        private void LoadOneTrueHobbies()
        {
            SortedDictionary<string, uint> hobbies = new SortedDictionary<string, uint>(new CareerNameComparer())
            {
                { "Cuisine", 0x00CC },
                { "Arts & Crafts", 0x00CD },
                { "Film", 0x00CE },
                { "Sport", 0x00CF },
                { "Games", 0x00D0 },
                { "Nature", 0x00D1 },
                { "Tinkering", 0x00D2 },
                { "Fitness", 0x00D3 },
                { "Science", 0x00D4 },
                { "Music", 0x00D5 },
                { "Secret", 0x00D6 }
            };

            comboHobbyOneTrue.Items.Clear();

            foreach (string hobby in hobbies.Keys)
            {
                comboHobbyOneTrue.Items.Add(new UintNamedValue(hobby, hobbies[hobby]));
            }

            ControlHelper.SetDropDownWidth(comboHobbyOneTrue);
        }
        #endregion

        #region Form Management
        private void OnLoad(object sender, EventArgs e)
        {
            RegistryTools.LoadAppSettings(FamilyManagerApp.RegistryKey, FamilyManagerApp.AppVersionMajor, FamilyManagerApp.AppVersionMinor);
            RegistryTools.LoadFormSettings(FamilyManagerApp.RegistryKey, this);
            splitTopBottom.SplitterDistance = (int)RegistryTools.GetSetting(FamilyManagerApp.RegistryKey, "splitterTB", splitTopBottom.SplitterDistance);
            splitTopLeftRight.SplitterDistance = (int)RegistryTools.GetSetting(FamilyManagerApp.RegistryKey, "splitterLR", splitTopLeftRight.SplitterDistance);

            // See also OnSplitterMoved
            splitClosetLeftRight.SplitterDistance = splitTopLeftRight.SplitterDistance;
            splitSafeLeftRight.SplitterDistance = splitTopLeftRight.SplitterDistance;

            menuItemUseCodes.Checked = ((int)RegistryTools.GetSetting(FamilyManagerApp.RegistryKey + @"\Options", menuItemUseCodes.Name, 0) != 0); OnUseCodesClicked(menuItemUseCodes, null);
            menuItemShowSplitFiles.Checked = ((int)RegistryTools.GetSetting(FamilyManagerApp.RegistryKey + @"\Options", menuItemShowSplitFiles.Name, 0) != 0); OnShowSplitFilesClicked(menuItemShowSplitFiles, null);
            menuItemHighlightSplitFiles.Checked = ((int)RegistryTools.GetSetting(FamilyManagerApp.RegistryKey + @"\Options", menuItemHighlightSplitFiles.Name, 0) != 0); OnHighlightSplitFilesClicked(menuItemHighlightSplitFiles, null);
            menuItemIncludeNPCs.Checked = ((int)RegistryTools.GetSetting(FamilyManagerApp.RegistryKey + @"\Options", menuItemIncludeNPCs.Name, 0) != 0);
            menuItemOnlyNPCs.Checked = ((int)RegistryTools.GetSetting(FamilyManagerApp.RegistryKey + @"\Options", menuItemOnlyNPCs.Name, 0) != 0);
            menuItemTeensHaveAdultJobs.Checked = ((int)RegistryTools.GetSetting(FamilyManagerApp.RegistryKey + @"\Options", menuItemTeensHaveAdultJobs.Name, 0) != 0);
            menuItemYAsHaveAdultJobs.Checked = ((int)RegistryTools.GetSetting(FamilyManagerApp.RegistryKey + @"\Options", menuItemYAsHaveAdultJobs.Name, 0) != 0);

            UpdateInterestTrackers((InterestTrackerStyle)(int)RegistryTools.GetSetting(FamilyManagerApp.RegistryKey + @"\Options", menuItemIntDisplay.Name, (int)InterestTrackerStyle.BarAndBox));

            menuItemAdvanced.Checked = ((int)RegistryTools.GetSetting(FamilyManagerApp.RegistryKey + @"\Mode", menuItemAdvanced.Name, 0) != 0); OnAdvancedModeChanged(menuItemAdvanced, null);
            menuItemAutoBackup.Checked = ((int)RegistryTools.GetSetting(FamilyManagerApp.RegistryKey + @"\Mode", menuItemAutoBackup.Name, 1) != 0);

            string lastLid = (string)RegistryTools.GetSetting(FamilyManagerApp.RegistryKey + @"\Options", menuLanguage.Name, Helper.Hex2PrefixString((int)defLid));
            foreach (string lid in GameData.LanguagesByCode.Keys)
            {
                if (GameData.LanguagesByCode.TryGetValue(lid, out string lang))
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
            prefLid = (MetaData.Languages)Convert.ToInt16(lastLid, 16);

            UpdateFormState();

            MyUpdater = new Updater(FamilyManagerApp.RegistryKey, menuHelp);
            MyUpdater.CheckForUpdates();

            DataCache.InvalidateHoods();

            DoWork_FillHoodTree(null, DBPFData.INSTANCE_NULL);

            LoadSchools();
            LoadSchoolGrades();

            LoadMajors();
            LoadSemesters();

            LoadAdultJobs();

            LoadOneTrueHobbies();
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            UpdateCurrentFamily();

            if (packageCache.IsDirty)
            {
                if (MsgBox.Show($"There are unsaved changes, do you really want to exit?", "Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }

            if (Form.ModifierKeys == (Keys.Control | Keys.Shift))
            {
                RegistryTools.RemoveAppSettings(FamilyManagerApp.RegistryKey);
                DataCache.RemoveAll();
            }
            else
            {
                RegistryTools.SaveAppSettings(FamilyManagerApp.RegistryKey, FamilyManagerApp.AppVersionMajor, FamilyManagerApp.AppVersionMinor);
                RegistryTools.SaveFormSettings(FamilyManagerApp.RegistryKey, this);
                RegistryTools.SaveSetting(FamilyManagerApp.RegistryKey, "splitterTB", splitTopBottom.SplitterDistance);
                RegistryTools.SaveSetting(FamilyManagerApp.RegistryKey, "splitterLR", splitTopLeftRight.SplitterDistance);

                RegistryTools.SaveSetting(FamilyManagerApp.RegistryKey + @"\Options", menuItemUseCodes.Name, menuItemUseCodes.Checked ? 1 : 0);
                RegistryTools.SaveSetting(FamilyManagerApp.RegistryKey + @"\Options", menuItemShowSplitFiles.Name, menuItemShowSplitFiles.Checked ? 1 : 0);
                RegistryTools.SaveSetting(FamilyManagerApp.RegistryKey + @"\Options", menuItemHighlightSplitFiles.Name, menuItemHighlightSplitFiles.Checked ? 1 : 0);
                RegistryTools.SaveSetting(FamilyManagerApp.RegistryKey + @"\Options", menuItemIncludeNPCs.Name, menuItemIncludeNPCs.Checked ? 1 : 0);
                RegistryTools.SaveSetting(FamilyManagerApp.RegistryKey + @"\Options", menuItemOnlyNPCs.Name, menuItemOnlyNPCs.Checked ? 1 : 0);
                RegistryTools.SaveSetting(FamilyManagerApp.RegistryKey + @"\Options", menuItemTeensHaveAdultJobs.Name, menuItemTeensHaveAdultJobs.Checked ? 1 : 0);
                RegistryTools.SaveSetting(FamilyManagerApp.RegistryKey + @"\Options", menuItemYAsHaveAdultJobs.Name, menuItemYAsHaveAdultJobs.Checked ? 1 : 0);
                RegistryTools.SaveSetting(FamilyManagerApp.RegistryKey + @"\Options", menuItemIntDisplay.Name, (int)interestsTrackersStyle);

                RegistryTools.SaveSetting(FamilyManagerApp.RegistryKey + @"\Mode", menuItemAdvanced.Name, IsAdvancedMode ? 1 : 0);
                RegistryTools.SaveSetting(FamilyManagerApp.RegistryKey + @"\Mode", menuItemAutoBackup.Name, menuItemAutoBackup.Checked ? 1 : 0);
            }

            TidyUp();
        }

        private void SetTitle(string hood = null)
        {
            if (hood == null)
            {
                this.Text = $"{FamilyManagerApp.AppTitle}";
            }
            else
            {
                this.Text = $"{FamilyManagerApp.AppTitle} - {hood}";
            }
        }

        private void OnExitClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnHelpClicked(object sender, EventArgs e)
        {
            new AboutDialog(FamilyManagerApp.AppProduct).ShowDialog();
        }

        private void OnSplitterMoved(object sender, SplitterEventArgs e)
        {
            if (sender == splitTopLeftRight)
            {
                splitClosetLeftRight.SplitterDistance = splitTopLeftRight.SplitterDistance;
                splitSafeLeftRight.SplitterDistance = splitTopLeftRight.SplitterDistance;
            }
            else if (sender == splitClosetLeftRight)
            {
                splitTopLeftRight.SplitterDistance = splitClosetLeftRight.SplitterDistance;
                splitSafeLeftRight.SplitterDistance = splitClosetLeftRight.SplitterDistance;
            }
            else if (sender == splitSafeLeftRight)
            {
                splitTopLeftRight.SplitterDistance = splitSafeLeftRight.SplitterDistance;
                splitClosetLeftRight.SplitterDistance = splitSafeLeftRight.SplitterDistance;
            }
        }
        #endregion

        #region Worker
        private string lastPackageFile;

        private void DoWork_FillHoodTree(string hood, TypeInstanceID familyId)
        {
            if (Directory.Exists($"{Sims2ToolsLib.Sims2HomePath}\\Neighborhoods"))
            {
                dataFamilyMembers.Clear();

                dataFamilyCloset.Clear();
                dataSuitcase.Clear();

                dataFamilySafe.Clear();
                dataJewelbox.Clear();

                gridFamilyMembers.Enabled = false;
                treeHoods.Nodes.Clear();

                ClearFamilyTabValues();

                lastHoodNode = null;
                lastFamilyNode = null;

                ProgressDialog progressDialog = new ProgressDialog(new WorkerPackage());
                progressDialog.DoWork += new ProgressDialog.DoWorkEventHandler(DoAsyncWork_ProcessHoods);
                progressDialog.DoData += new ProgressDialog.DoWorkEventHandler(DoAsyncData_ProcessHoods);

                DialogResult result = progressDialog.ShowDialog();

                if (result == DialogResult.Abort)
                {
                    logger.Error(progressDialog.Result.Error.Message);
                    logger.Info(progressDialog.Result.Error.StackTrace);

                    MsgBox.Show($"An error occured while processing\n{lastPackageFile}", "Error!", MessageBoxButtons.OK);
                }
                else
                {
                    if (result == DialogResult.Cancel)
                    {
                        treeHoods.Nodes.Clear();
                    }
                    else
                    {
                        if (familyId == DBPFData.INSTANCE_NULL)
                        {
                            OnTreeHoodsClicked(treeHoods, new TreeNodeMouseClickEventArgs(treeHoods.Nodes[0], MouseButtons.Left, 1, 0, 0));
                            // treeHoods.Nodes[0]?.Nodes[0]?.Expand();
                            treeHoods.Nodes[0]?.Expand();
                        }
                        else
                        {
                            foreach (TreeNode hnode in treeHoods.Nodes[0].Nodes)
                            {
                                if (hnode is HoodTreeNode hoodNode && hoodNode.HoodSubFolder.Equals(hood))
                                {
                                    foreach (TreeNode fnode in hoodNode.Nodes)
                                    {
                                        if (fnode is FamilyTreeNode familyNode && familyNode.FamilyId == familyId)
                                        {
                                            treeHoods.SelectedNode = familyNode;
                                            hoodNode.Expand();
                                            treeHoods.Nodes[0].Expand();

                                            DoWork_FillHoodOrFamilyGrid(familyNode);

                                            break;
                                        }
                                    }

                                    break;
                                }
                            }
                        }
                    }

                    UpdateFormState();
                }
            }
        }

        private void DoWork_FillHoodOrFamilyGrid(TreeNode selectedNode)
        {
            if (selectedNode is TopTreeNode)
            {
                logger.Info("Selected Top:");

                if (!cachesLoaded)
                {
                    careerCache.LoadCareers();
                    clothingCache.LoadOutfits(Gzps.TYPE);
                    jewelleryCache.LoadOutfits(Xmol.TYPE);
                    cachesLoaded = true;

                    LoadCareers();
                }

                UpdateCurrentFamily();
                ClearFamily();

                ClearHood();
            }
            else if (selectedNode is HoodTreeNode)
            {
                // SelectHood(selectedNode as HoodTreeNode);

                UpdateCurrentFamily();
                ClearFamily();
            }
            else if (selectedNode is FamilyTreeNode familyNode)
            {
                HoodTreeNode hoodNode = familyNode.Parent as HoodTreeNode;
                SelectHood(hoodNode);

                logger.Info($"Selected Family: {familyNode.Text}");

                if (!familyNode.Equals(lastFamilyNode))
                {
                    lastFamilyNode = familyNode;
                    lastPackageFile = hoodNode.PackagePath;

                    filters.ShowAll();

                    DoWork_FillFamilyGrid(hoodNode, familyNode);
                    DoWork_FillClosetOrSafeGrid(hoodNode, familyNode);
                }
            }

            UpdateFormState();
        }

        private void DoWork_FillFamilyGrid(HoodTreeNode hoodNode, FamilyTreeNode familyNode)
        {
            Stopwatch s = new Stopwatch();
            s.Start();

            UpdateCurrentFamily();

            currentFamily = new FamilyData(packageCache, hoodNode, familyNode);

            lblFamilyName.Text = textFamilyName.Text = currentFamily.FamilyName;
            textFamilyWriteUp.Text = currentFamily.FamilyWriteUp;
            textFamilyMoney.Text = currentFamily.FamilyMoney;
            textBusinessMoney.Text = currentFamily.BusinessMoney;
            imageFamily.Image = currentFamily.FamilyImage;

            textFamilyName.Enabled = textFamilyWriteUp.Enabled = (currentFamily.FamilyName != null);
            textAddressName.Enabled = textAddressDesc.Enabled = (currentFamily.LotAddress != null);

            lblLotName.Text = currentFamily.LotAddress ?? "The Sim Bin";
            textAddressName.Text = currentFamily.LotAddress;
            textAddressDesc.Text = currentFamily.LotDescription;
            imageHouse.Image = currentFamily.LotImage;

            dataFamilyMembers.Clear();
            gridFamilyMembers.Enabled = true;


            using (CacheableDbpfFile hoodPackage = packageCache.OpenForReadOnly(hoodNode.PackagePath))
            {
                foreach (uint memberGuid in currentFamily.FamilyMembers)
                {
                    Sdsc sdsc = (Sdsc)hoodPackage.GetResourceByKey(new DBPFKey(Sdsc.TYPE, DBPFData.GROUP_LOCAL, sdscInstanceBySimGuid[memberGuid], DBPFData.RESOURCE_NULL));

                    if (characterCache.TryGetValue(sdsc.SimGuid, out CharacterData data))
                    {
                        data.SetSdscDetails(hoodNode.PackagePath, sdsc.InstanceID);

                        uint genderCode = GenderHelper.CpfGenderCode(sdsc.Gender);
                        uint ageCode = AgeHelper.CpfAgeCode(sdsc.LifeSection);

                        DataRow memberRow = dataFamilyMembers.NewRow();

                        memberRow["Data"] = data;

                        memberRow["FirstName"] = $"{data.GivenName(prefLid)} {data.FamilyName(prefLid)}";
                        memberRow["SplitFile"] = data.IsSplit ? "Y" : "N";

                        memberRow["Gender"] = sdsc.Gender.ToString();
                        memberRow["GenderCode"] = sdsc.Gender.ToString().Substring(0, 1);
                        memberRow["Age"] = sdsc.LifeSection.ToString();
                        memberRow["AgeCode"] = BuildAgeCodeString(ageCode);

                        memberRow["GenderHex"] = genderCode;
                        memberRow["AgeHex"] = ageCode;

                        memberRow["DaysLeft"] = sdsc.AgeDaysLeft;

                        if (ageCode != 0x0000)
                        {
                            memberRow["Thumbnail"] = data.Thumbnail(ageCode);
                        }

                        dataFamilyMembers.Rows.Add(memberRow);
                    }
                }

                hoodPackage.Close();
            }

            logger.Info($"Family loaded in {(s.ElapsedMilliseconds / 1000.0)}s");
            s.Stop();
        }

        private void DoWork_FillClosetOrSafeGrid(HoodTreeNode hoodNode, FamilyTreeNode familyNode)
        {
            if (IsClosetTabActive)
            {
                DoWork_FillFamilyClosetGrid(hoodNode, familyNode);
            }
            else if (IsSafeTabActive)
            {
                DoWork_FillFamilySafeGrid(hoodNode, familyNode);
            }

            FilterActiveContainer();
        }

        private void DoWork_FillFamilyClosetGrid(HoodTreeNode hoodNode, FamilyTreeNode familyNode)
        {
            Stopwatch s = new Stopwatch();
            s.Start();

            dataFamilyCloset.Clear();

            using (CacheableDbpfFile package = packageCache.OpenForReadOnly(hoodNode.PackagePath))
            {
                foreach (DBPFEntry entry in package.GetEntriesByType(Idr.TYPE))
                {
                    Idr idr = (Idr)package.GetResourceByEntry(entry);

                    if (idr.InstanceID.AsUInt() > 0x00007FFF && idr.ItemCount == 3)
                    {
                        DBPFKey collKey = idr.GetItem(1);

                        if (collKey.TypeID == Coll.TYPE && collKey.InstanceID == familyNode.FamilyId)
                        {
                            DBPFKey cpfKey = idr.GetItem(2);

                            if (cpfKey.TypeID == Gzps.TYPE)
                            {
                                DataRow closetRow = dataFamilyCloset.NewRow();

                                closetRow["Visible"] = "Yes";
                                closetRow["Data"] = OutfitDbpfData.Create(package, idr);

                                if (clothingCache.ContainsKey(cpfKey))
                                {
                                    CasOutfitData data = clothingCache.GetData(cpfKey);

                                    closetRow["Name"] = data.ResName;
                                    closetRow["Category"] = BuildCategoryString(data.ResCategory);
                                    closetRow["Gender"] = BuildGenderString(data.ResGender);
                                    closetRow["GenderCode"] = BuildGenderCodeString(data.ResGender);
                                    closetRow["Age"] = BuildAgeString(data.ResAge);
                                    closetRow["AgeCode"] = BuildAgeCodeString(data.ResAge);

                                    closetRow["GenderHex"] = data.ResGender;
                                    closetRow["AgeHex"] = data.ResAge;

                                    closetRow["ThumbKey"] = data.ThumbKey;
                                    closetRow["LocalThumbKey"] = data.LocalThumbKeyZ;
                                }
                                else
                                {
                                    closetRow["Name"] = cpfKey.ToString();
                                }

                                dataFamilyCloset.Rows.Add(closetRow);
                            }
                        }
                    }
                }

                package.Close();
            }

            logger.Info($"Closet loaded in {(s.ElapsedMilliseconds / 1000.0)}s");
            s.Stop();
        }

        private void DoWork_FillFamilySafeGrid(HoodTreeNode hoodNode, FamilyTreeNode familyNode)
        {
            Stopwatch s = new Stopwatch();
            s.Start();

            dataFamilySafe.Clear();

            using (CacheableDbpfFile package = packageCache.OpenForReadOnly(hoodNode.PackagePath))
            {
                foreach (DBPFEntry entry in package.GetEntriesByType(Idr.TYPE))
                {
                    Idr idr = (Idr)package.GetResourceByEntry(entry);

                    if (idr.InstanceID.AsUInt() > 0x00007FFF && idr.ItemCount == 3)
                    {
                        DBPFKey collKey = idr.GetItem(1);

                        if (collKey.TypeID == Coll.TYPE && collKey.InstanceID == familyNode.FamilyId)
                        {
                            DBPFKey cpfKey = idr.GetItem(2);

                            if (cpfKey.TypeID == Xmol.TYPE)
                            {
                                DataRow safeRow = dataFamilySafe.NewRow();

                                safeRow["Visible"] = "Yes";
                                safeRow["Data"] = OutfitDbpfData.Create(package, idr);

                                if (jewelleryCache.ContainsKey(cpfKey))
                                {
                                    CasOutfitData data = jewelleryCache.GetData(cpfKey);

                                    safeRow["Name"] = data.ResName;
                                    safeRow["Category"] = BuildCategoryString(data.ResCategory);
                                    safeRow["Gender"] = BuildGenderString(data.ResGender);
                                    safeRow["GenderCode"] = BuildGenderCodeString(data.ResGender);
                                    safeRow["Age"] = BuildAgeString(data.ResAge);
                                    safeRow["AgeCode"] = BuildAgeCodeString(data.ResAge);

                                    safeRow["GenderHex"] = data.ResGender;
                                    safeRow["AgeHex"] = data.ResAge;

                                    safeRow["ThumbKey"] = data.ThumbKey;
                                    safeRow["LocalThumbKey"] = data.LocalThumbKeyZ;
                                }
                                else
                                {
                                    safeRow["Name"] = cpfKey.ToString();
                                }

                                dataFamilySafe.Rows.Add(safeRow);
                            }
                        }
                    }
                }

                package.Close();
            }

            logger.Info($"Jewellery loaded in {(s.ElapsedMilliseconds / 1000.0)}s");
            s.Stop();
        }

        private void DoAsyncWork_ProcessHoods(ProgressDialog sender, DoWorkEventArgs args)
        {
            WorkerPackage workPackage = args.Argument as WorkerPackage; // As passed to the Sims2ToolsProgressDialog constructor

            sender.VisualMode = ProgressBarDisplayMode.CustomText;

#if !DEBUG
            try
#endif
            {
                //sender.SetProgress(0, "Loading Hood Tree");

                WorkerAddTreeNodeTask task = new WorkerAddTreeNodeTask(treeHoods.Nodes, new TopTreeNode("Hoods"));
                sender.SetData(task);

                if (!PopulateHoods(sender, task.ChildNode))
                {
                    args.Cancel = true;
                    return;
                }
            }
#if !DEBUG
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Info(ex.StackTrace);

                if (MsgBox.Show($"An error occured while processing\n{lastPackageFile}\n\nReason: {ex.Message}", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                {
                    throw ex;
                }
            }
#endif
        }

        private void DoAsyncData_ProcessHoods(ProgressDialog sender, DoWorkEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { DoAsyncData_ProcessHoods(sender, e); });
                return;
            }

            // This will be run on the main (UI) thread 
            IWorkerTask task = e.Argument as IWorkerTask;
            task.DoTask();
        }

        private void DoAsyncWork_ProcessFamilies(ProgressDialog sender, DoWorkEventArgs args)
        {
            HoodTreeNode hoodNode = args.Argument as HoodTreeNode; // As passed to the Sims2ToolsProgressDialog constructor

            sender.VisualMode = ProgressBarDisplayMode.CustomText;

#if !DEBUG
            try
#endif
            {
                if (!PopulateHoodFamilies(sender, hoodNode))
                {
                    args.Cancel = true;
                    return;
                }
            }
#if !DEBUG
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Info(ex.StackTrace);

                if (MsgBox.Show($"An error occured while processing\n{lastPackageFile}\n\nReason: {ex.Message}", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                {
                    throw ex;
                }
            }
#endif
        }

        private void DoAsyncData_ProcessFamilies(ProgressDialog sender, DoWorkEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { DoAsyncData_ProcessFamilies(sender, e); });
                return;
            }

            // This will be run on the main (UI) thread 
            IWorkerTask task = e.Argument as IWorkerTask;
            task.DoTask();
        }
        #endregion

        #region Hood Worker Helpers
        private bool PopulateHoods(ProgressDialog sender, TreeNode parent)
        {
            string[] subHoodDirs = Directory.GetDirectories($"{Sims2ToolsLib.Sims2HomePath}\\Neighborhoods", "*", SearchOption.TopDirectoryOnly);

            double percent = 0.0f;
            double delta = 100.0f / subHoodDirs.Length;

            foreach (string subHoodDir in subHoodDirs)
            {
                if (sender.CancellationPending)
                {
                    return false;
                }

                DirectoryInfo di = new DirectoryInfo(subHoodDir);

                if (!di.Name.Equals("Tutorial"))
                {
                    foreach (string packagePath in Directory.GetFiles(subHoodDir, $"{di.Name}_Neighborhood.package", SearchOption.TopDirectoryOnly))
                    {
                        if (sender.CancellationPending)
                        {
                            return false;
                        }

                        using (CacheableDbpfFile package = packageCache.OpenForReadOnly(packagePath))
                        {
                            Ctss ctss = (Ctss)package.GetResourceByKey(new DBPFKey(Ctss.TYPE, DBPFData.GROUP_LOCAL, (TypeInstanceID)0x00000001, DBPFData.RESOURCE_NULL));

                            if (ctss != null)
                            {
                                string hoodName = GetString(ctss, 0);
                                HoodTreeNode hoodNode = new HoodTreeNode(packagePath, di.Name, hoodName);
                                WorkerAddTreeNodeTask task = new WorkerAddTreeNodeTask(parent.Nodes, hoodNode);
                                sender.SetProgress((int)percent, $"Loading {hoodName} ({di.Name})");
                                sender.SetData(task);
                                sender.SetData(new WorkerAddTreeNodeTask(hoodNode.Nodes, new TreeNode("Placeholder"))); // Force the + by the side of the hood node
                            }

                            package.Close();
                        }
                    }
                }

                percent += delta;
            }

            return true;
        }

        private bool PopulateHoodFamilies(ProgressDialog sender, HoodTreeNode hoodNode)
        {
            using (CacheableDbpfFile package = packageCache.OpenForReadOnly(hoodNode.PackagePath))
            {
                SortedDictionary<string, List<TypeInstanceID>> hoodFamilies = new SortedDictionary<string, List<TypeInstanceID>>();

                List<DBPFEntry> famiEntries = package.GetEntriesByType(Fami.TYPE);

                double percent = 0.0f;
                double delta = 100.0f / famiEntries.Count;

                foreach (DBPFEntry famiEntry in famiEntries)
                {
                    if (sender.CancellationPending)
                    {
                        return false;
                    }

                    uint inst = famiEntry.InstanceID.AsUInt();

                    if (menuItemOnlyNPCs.Checked)
                    {
                        if (inst > 0x0000 && inst < (uint)FamiCodes.Lowest) continue;
                    }
                    else if (!menuItemIncludeNPCs.Checked)
                    {
                        if (inst == 0x0000 || inst >= (uint)FamiCodes.Lowest) continue;
                    }

                    Fami fami = (Fami)package.GetResourceByEntry(famiEntry);
                    Str str = (Str)package.GetResourceByKey(new DBPFKey(Str.TYPE, fami));
                    string familyName;

                    if (inst == 0x0000 || inst >= (uint)FamiCodes.Lowest)
                    {
                        familyName = $"{(FamiCodes)inst} (NPCs)";
                    }
                    else
                    {
                        familyName = GetString(str, 0);
                    }

                    if (!hoodFamilies.ContainsKey(familyName))
                    {
                        hoodFamilies.Add(familyName, new List<TypeInstanceID>());
                        sender.SetProgress((int)percent, $"Loading {familyName}");
                    }

                    hoodFamilies[familyName].Add(fami.InstanceID);

                    percent += delta;
                }

                foreach (string familyName in hoodFamilies.Keys)
                {
                    foreach (TypeInstanceID familyInstance in hoodFamilies[familyName])
                    {
                        FamilyTreeNode familyNode = new FamilyTreeNode(familyInstance, familyName, FamilyData.FamilyLocation(packageCache, hoodNode, familyInstance));
                        sender.SetData(new WorkerAddTreeNodeTask(hoodNode.Nodes, familyNode));
                    }
                }

                package.Close();
            }

            return true;
        }

        private void ClearHood()
        {
            lastHoodNode = null;
        }

        private void SelectHood(HoodTreeNode hoodNode)
        {
            if (!hoodNode.Equals(lastHoodNode))
            {
                logger.Info($"Selected Hood: {hoodNode.Text}");
                lastHoodNode = hoodNode;

                ProgressDialog progressDialog = new ProgressDialog(new WorkerPackage());
                progressDialog.DoWork += new ProgressDialog.DoWorkEventHandler(DoAsyncWork_LoadCharacterCache);

                DialogResult result = progressDialog.ShowDialog();

                if (result == DialogResult.Abort)
                {
                    logger.Error(progressDialog.Result.Error.Message);
                    logger.Info(progressDialog.Result.Error.StackTrace);

                    MsgBox.Show($"An error occured while processing\n{characterCache.ErrorPackagePath}", "Error!", MessageBoxButtons.OK);
                }
                else
                {
                    if (result == DialogResult.Cancel)
                    {
                        // Load Character Cache cancelled
                    }
                    else
                    {
                        // Load Character Cache completed
                    }
                }

                // Do NOT remove these without recoding the transfer drag/drop and copy/paste code!
                dataSuitcase.Clear();
                dataJewelbox.Clear();
            }
        }

        #endregion

        #region Family Worker Helpers
        private void ClearFamily()
        {
            ClearFamilyTabValues();

            lblFamilyName.Text = "";
            lblLotName.Text = "";
            dataFamilyMembers.Clear();
            imageFamily.Image = null;

            lastHoodNode = null;

            lastFamilyNode = null;
        }

        private void UpdateCurrentFamily()
        {
            bool cacheState = packageCache.IsDirty;
            string oldFamilyName = currentFamily?.FamilyName;

            if (currentFamily != null)
            {
                if (!currentFamily.FamilyName.Equals(textFamilyName.Text))
                {
                    lastFamilyNode.Text = textFamilyName.Text; // Update the hood tree node for this family
                    lblFamilyName.Text = textFamilyName.Text; // Update the family name above the member list

                    UpdateCurrentFamilyMembers(); // Do this before changing the family name
                }

                currentFamily.FamilyName = textFamilyName.Text;
                currentFamily.FamilyWriteUp = textFamilyWriteUp.Text;

                // TODO - Family Manager - family tab - there MAY be other STR# (with the LOTD resource) that need updating with the new name/desc
                if (currentFamily.LotAddress != null)
                {
                    lblLotName.Text = textAddressName.Text;
                    currentFamily.LotAddress = textAddressName.Text;
                    currentFamily.LotDescription = textAddressDesc.Text;
                }

                if (ckbMoneyLock.Checked && !currentFamily.FamilyMoney.Equals(textFamilyMoney.Text))
                {
                    textBusinessMoney.Text = textFamilyMoney.Text;
                }

                currentFamily.FamilyMoney = textFamilyMoney.Text;
                currentFamily.BusinessMoney = textBusinessMoney.Text;
            }

            if (packageCache.IsDirty && !cacheState)
            {
                logger.Debug($"Package cache state changed to dirty for family {oldFamilyName}");
            }
        }

        private void UpdateCurrentFamilyMembers()
        {
            if (ckbFamilyNameAll.Checked || ckbFamilyNameSame.Checked || ckbFamilyNameSelected.Checked)
            {
                if (!currentFamily.FamilyName.Equals(textFamilyName.Text))
                {
                    if (ckbFamilyNameSelected.Checked)
                    {
                        foreach (DataGridViewRow row in gridFamilyMembers.SelectedRows)
                        {
                            ChangeMemberFamilyName(row, textFamilyName.Text);
                        }
                    }
                    else
                    {
                        foreach (DataGridViewRow row in gridFamilyMembers.Rows)
                        {
                            if (ckbFamilyNameSame.Checked)
                            {
                                CharacterData data = (row.Cells["colData"].Value as CharacterData);

                                if (data.FamilyName(prefLid).Equals(currentFamily.FamilyName))
                                {
                                    ChangeMemberFamilyName(row, textFamilyName.Text);
                                }
                            }
                            else
                            {
                                ChangeMemberFamilyName(row, textFamilyName.Text);
                            }
                        }
                    }
                }
            }
        }

        private void DoAsyncWork_LoadCharacterCache(ProgressDialog sender, DoWorkEventArgs args)
        {
            sender.VisualMode = ProgressBarDisplayMode.CustomText;
            sender.SetProgress(0, "Loading Hood Characters");

            characterCache.Load(sender, lastHoodNode);

            sender.SetProgress(0, "Caching SDSC References");
            sdscInstanceBySimGuid.Clear();

            using (CacheableDbpfFile hoodPackage = packageCache.OpenForReadOnly(lastHoodNode.PackagePath))
            {
                foreach (DBPFEntry entry in hoodPackage.GetEntriesByType(Sdsc.TYPE))
                {
                    Sdsc sdsc = (Sdsc)hoodPackage.GetResourceByEntry(entry);

                    sdscInstanceBySimGuid.Add(sdsc.SimGuid.AsUInt(), entry.InstanceID);
                }

                hoodPackage.Close();
            }
        }
        #endregion

        #region Member Worker Helpers
        private CharacterData currentMemberData;
        private bool ignoreCareerChanges = false;
        private bool ignoreInterestsChanges = false;
        private bool ignoreSkillsChanges = false;

        private void ClearCareerTab()
        {
            currentMemberData = null;

            imageSim.Image = null;

            ignoreCareerChanges = true;

            grpSchool.Enabled = false;
            comboSchoolType.SelectedIndex = -1;
            textSchoolGUID.Value = GuidTextBox.NO_VALUE;
            comboSchoolGrade.SelectedIndex = -1;

            grpUniversity.Enabled = false;
            grpUniversity.Text = "University";

            lblUniSemester.Visible = comboUniSemester.Visible = false;
            lblUniGrade.Visible = trackUniGrade.Visible = textUniGrade.Visible = false;
            lblUniEffort.Visible = trackUniEffort.Visible = textUniEffort.Visible = false;
            lblUniTimeLeft.Visible = trackUniTimeLeft.Visible = textUniTimeLeft.Visible = false;
            lblUniInfluence.Visible = textUniInfluence.Visible = false;
            lblUniProbation.Visible = ckbUniProbation.Visible = false;
            lblUniStudying.Visible = ckbUniStudying.Visible = false;

            comboUniMajor.SelectedIndex = -1;
            comboUniSemester.SelectedIndex = -1;
            trackUniGrade.Value = 0;
            textUniGrade.Value = UIntTextBox.NO_VALUE;
            trackUniEffort.Value = 0;
            textUniEffort.Value = UIntTextBox.NO_VALUE;
            trackUniTimeLeft.Value = 0;
            textUniTimeLeft.Value = UIntTextBox.NO_VALUE;
            textUniInfluence.Value = UIntTextBox.NO_VALUE;

            grpJob.Enabled = false;
            comboJobType.SelectedIndex = -1;
            textJobGUID.Value = GuidTextBox.NO_VALUE;
            trackJobLevel.Value = 0;
            textJobLevel.Value = UIntTextBox.NO_VALUE;
            trackJobPerformance.Value = 0;
            textJobPerformance.Value = IntTextBox.NO_VALUE;
            textJobPTO.Value = UIntTextBox.NO_VALUE;
            lblJobPTOSummary.Visible = false;
            textJobPension.Value = UIntTextBox.NO_VALUE;
            comboJobRetiredType.SelectedIndex = -1;
            textJobRetiredGUID.Value = GuidTextBox.NO_VALUE;
            trackJobRetiredLevel.Value = 0;
            textJobRetiredLevel.Value = UIntTextBox.NO_VALUE;

            ignoreCareerChanges = false;
        }

        private void DoWork_FillCareerTab()
        {
            if (gridFamilyMembers.SelectedRows.Count == 1)
            {
                currentMemberData = (gridFamilyMembers.SelectedRows[0].Cells["colData"].Value as CharacterData);

                imageSim.Image = currentMemberData.Thumbnail(currentMemberData.AgeCode);

                ignoreCareerChanges = true;

                { // School
                    grpSchool.Enabled = currentMemberData.IsChildOrOlder;

                    if (grpSchool.Enabled)
                    {
                        SetCombo(comboSchoolType, currentMemberData.SchoolGuid.AsUInt());
                        textSchoolGUID.Value = currentMemberData.SchoolGuid.AsUInt();

                        lblSchoolGrade.Visible = comboSchoolGrade.Visible = true;
                        SetCombo(comboSchoolGrade, currentMemberData.SchoolGrade);
                    }
                    else
                    {
                        comboSchoolType.SelectedIndex = -1;
                        textSchoolGUID.Value = GuidTextBox.NO_VALUE;

                        lblSchoolGrade.Visible = comboSchoolGrade.Visible = false;
                    }
                }

                { // University
                    grpUniversity.Enabled = currentMemberData.IsYoungAdultOrOlder;

                    if (!grpUniversity.Enabled)
                    {
                        comboUniMajor.SelectedIndex = 0;
                        textMajorGUID.Value = GuidTextBox.NO_VALUE;
                    }
                    else
                    {
                        SetCombo(comboUniMajor, currentMemberData.UniMajorGuid.AsUInt());
                        textMajorGUID.Value = currentMemberData.UniMajorGuid.AsUInt();
                    }

                    lblUniResult.Visible = comboUniResult.Visible = false;
                    lblUniSemester.Visible = comboUniSemester.Visible = false;
                    lblUniGrade.Visible = trackUniGrade.Visible = textUniGrade.Visible = false;
                    lblUniEffort.Visible = trackUniEffort.Visible = textUniEffort.Visible = false;
                    lblUniTimeLeft.Visible = trackUniTimeLeft.Visible = textUniTimeLeft.Visible = false;
                    lblUniInfluence.Visible = textUniInfluence.Visible = false;
                    lblUniProbation.Visible = ckbUniProbation.Visible = false;
                    lblUniStudying.Visible = ckbUniStudying.Visible = false;

                    if (currentMemberData.OnCampus)
                    {
                        grpUniversity.Text = "University (On Campus)";
                        lblUniResult.Visible = comboUniResult.Visible = false;

                        lblUniSemester.Visible = comboUniSemester.Visible = true;
                        SetCombo(comboUniSemester, currentMemberData.UniSemester);

                        lblUniGrade.Visible = trackUniGrade.Visible = textUniGrade.Visible = true;
                        trackUniGrade.Value = currentMemberData.UniCurrentGPA;
                        textUniGrade.Value = (currentMemberData.UniCurrentGPA / 10.0f);

                        lblUniEffort.Visible = trackUniEffort.Visible = textUniEffort.Visible = true;
                        trackUniEffort.Value = currentMemberData.UniEffort;
                        textUniEffort.Value = currentMemberData.UniEffort;

                        lblUniTimeLeft.Visible = trackUniTimeLeft.Visible = textUniTimeLeft.Visible = true;
                        trackUniTimeLeft.Value = (int)Math.Min(careerCache.SemesterLength, currentMemberData.UniTimeLeft);
                        textUniTimeLeft.Value = Math.Min(careerCache.SemesterLength, currentMemberData.UniTimeLeft);

                        lblUniInfluence.Visible = textUniInfluence.Visible = true;
                        textUniInfluence.Value = currentMemberData.UniInfluence;

                        lblUniProbation.Visible = ckbUniProbation.Visible = true;
                        ckbUniProbation.Checked = currentMemberData.UniProbation;

                        lblUniStudying.Visible = ckbUniStudying.Visible = true;
                        ckbUniStudying.Checked = currentMemberData.UniStudying;
                    }
                    else
                    {
                        grpUniversity.Text = "University";

                        lblUniResult.Visible = comboUniResult.Visible = currentMemberData.IsAdultOrOlder;

                        if (currentMemberData.Graduated)
                        {
                            comboUniResult.SelectedIndex = 1;
                        }
                        else if (currentMemberData.DroppedOut)
                        {
                            comboUniResult.SelectedIndex = 2;
                        }
                        else if (currentMemberData.Expelled)
                        {
                            comboUniResult.SelectedIndex = 3;
                        }
                        else
                        {
                            comboUniResult.SelectedIndex = 0;
                        }

                        lblUniMajor.Enabled = comboUniMajor.Enabled = textMajorGUID.Enabled = (comboUniResult.SelectedIndex != 0);
                    }
                }

                { // Job
                    EnableJobGroup(currentMemberData.IsTeen || currentMemberData.IsAdultOrOlder || (currentMemberData.IsYoungAdult && menuItemYAsHaveAdultJobs.Checked));
                }

                ignoreCareerChanges = false;
            }
            else
            {
                ClearCareerTab();
            }
        }

        private void ClearSkillsTab()
        {
            currentMemberData = null;

            ignoreSkillsChanges = true;

            foreach (Control control in grpSkillsGeneral.Controls)
            {
                if (control is SkillTracker tracker)
                {
                    tracker.Value = SkillTracker.NO_VALUE;
                }
            }

            foreach (Control control in grpSkillsToddler.Controls)
            {
                if (control is SkillTracker tracker)
                {
                    tracker.Value = SkillTracker.NO_VALUE;
                }
            }

            foreach (Control control in grpSkillsHidden.Controls)
            {
                if (control is SkillTracker tracker)
                {
                    tracker.Value = SkillTracker.NO_VALUE;
                }
            }

            foreach (Control control in grpSkillsLife.Controls)
            {
                if (control is SkillTracker tracker)
                {
                    tracker.Value = SkillTracker.NO_VALUE;
                }
            }

            foreach (Control control in grpSkillsPet.Controls)
            {
                if (control is SkillTracker tracker)
                {
                    tracker.Value = SkillTracker.NO_VALUE;
                }
            }

            ignoreSkillsChanges = false;
        }

        private void DoWork_FillSkillsTab()
        {
            if (gridFamilyMembers.SelectedRows.Count == 1)
            {
                currentMemberData = (gridFamilyMembers.SelectedRows[0].Cells["colData"].Value as CharacterData);

                ignoreSkillsChanges = true;

                { // General Skills
                    grpSkillsGeneral.Enabled = currentMemberData.IsToddlerOrOlder;

                    if (grpSkillsGeneral.Enabled)
                    {
                        foreach (Control control in grpSkillsGeneral.Controls)
                        {
                            if (control is SkillTracker tracker)
                            {
                                tracker.Value = currentMemberData.GetSkillValue(tracker.SdscIndex, tracker.Maximum);
                            }
                        }
                    }
                    else
                    {
                        foreach (Control control in grpSkillsGeneral.Controls)
                        {
                            if (control is SkillTracker tracker)
                            {
                                tracker.Value = SkillTracker.NO_VALUE;
                            }
                        }
                    }
                }

                { // Toddler Skills
                    grpSkillsToddler.Enabled = currentMemberData.IsToddlerOrOlder;

                    if (grpSkillsToddler.Enabled)
                    {
                        foreach (Control control in grpSkillsToddler.Controls)
                        {
                            if (control is SkillTracker tracker)
                            {
                                tracker.Value = currentMemberData.GetToddlerSkillValue((TypeGUID)tracker.TokenGuid, (int)tracker.TokenProp, tracker.Maximum);
                            }
                        }
                    }
                    else
                    {
                        foreach (Control control in grpSkillsToddler.Controls)
                        {
                            if (control is SkillTracker tracker)
                            {
                                tracker.Value = SkillTracker.NO_VALUE;
                            }
                        }
                    }
                }

                { // Hidden Skills
                    grpSkillsHidden.Enabled = currentMemberData.IsChildOrOlder;

                    if (grpSkillsHidden.Enabled)
                    {
                        foreach (Control control in grpSkillsHidden.Controls)
                        {
                            if (control is SkillTracker tracker)
                            {
                                tracker.Value = currentMemberData.GetHiddenSkillValue((TypeGUID)tracker.TokenGuid, (int)tracker.TokenProp, tracker.Maximum);
                            }
                        }
                    }
                    else
                    {
                        foreach (Control control in grpSkillsHidden.Controls)
                        {
                            if (control is SkillTracker tracker)
                            {
                                tracker.Value = SkillTracker.NO_VALUE;
                            }
                        }
                    }
                }

                { // Life Skills
                    grpSkillsLife.Enabled = currentMemberData.IsChildOrOlder;

                    if (grpSkillsLife.Enabled)
                    {
                        foreach (Control control in grpSkillsLife.Controls)
                        {
                            if (control is SkillTracker tracker)
                            {
                                tracker.Value = currentMemberData.GetLifeSkillValue((TypeGUID)tracker.TokenGuid, tracker.Maximum);
                            }
                        }
                    }
                    else
                    {
                        foreach (Control control in grpSkillsLife.Controls)
                        {
                            if (control is SkillTracker tracker)
                            {
                                tracker.Value = SkillTracker.NO_VALUE;
                            }
                        }
                    }
                }

                { // Pet Skills
                    grpSkillsPet.Enabled = currentMemberData.IsPet;

                    if (grpSkillsPet.Enabled)
                    {
                        foreach (Control control in grpSkillsPet.Controls)
                        {
                            if (control is SkillTracker tracker)
                            {
                                tracker.Value = (ushort)currentMemberData.GetPetSkillValue((TypeGUID)tracker.TokenGuid);
                            }
                        }

                        trackSkillPetUseToilet.Enabled = currentMemberData.IsCat;
                        if (!trackSkillPetUseToilet.Enabled)
                        {
                            trackSkillPetUseToilet.Value = SkillTracker.NO_VALUE;
                        }
                    }
                    else
                    {
                        foreach (Control control in grpSkillsPet.Controls)
                        {
                            if (control is SkillTracker tracker)
                            {
                                tracker.Value = SkillTracker.NO_VALUE;
                            }
                        }
                    }
                }

                ignoreSkillsChanges = false;
            }
            else
            {
                ClearSkillsTab();
            }
        }

        private void ClearInterestsTab()
        {
            currentMemberData = null;

            ignoreInterestsChanges = true;

            foreach (Control control in grpInterests.Controls)
            {
                if (control is InterestTracker tracker)
                {
                    tracker.Value = InterestTracker.NO_VALUE;
                }
            }

            foreach (Control control in grpHobbies.Controls)
            {
                if (control is InterestTracker tracker)
                {
                    tracker.Value = InterestTracker.NO_VALUE;
                }
            }

            comboHobbyOneTrue.SelectedIndex = -1;

            foreach (Control control in grpBadges.Controls)
            {
                if (control is InterestTracker tracker)
                {
                    tracker.Value = InterestTracker.NO_VALUE;
                }
            }

            ignoreInterestsChanges = false;
        }

        private void DoWork_FillInterestsTab()
        {
            if (gridFamilyMembers.SelectedRows.Count == 1)
            {
                currentMemberData = (gridFamilyMembers.SelectedRows[0].Cells["colData"].Value as CharacterData);

                ignoreInterestsChanges = true;

                { // Interests
                    foreach (Control control in grpInterests.Controls)
                    {
                        if (control is InterestTracker tracker)
                        {
                            tracker.Value = currentMemberData.GetInterestValue(tracker.SdscIndex);
                        }
                    }
                }

                { // Hobbies - Requires FreeTime
                    grpHobbies.Enabled = currentMemberData.HasHobbies;

                    if (currentMemberData.HasHobbies)
                    {
                        foreach (Control control in grpHobbies.Controls)
                        {
                            if (control is InterestTracker tracker)
                            {
                                tracker.Value = currentMemberData.GetHobbyValue(tracker.SdscIndex);
                            }
                        }

                        SetCombo(comboHobbyOneTrue, currentMemberData.OneTrueHobby);
                    }
                    else
                    {
                        foreach (Control control in grpHobbies.Controls)
                        {
                            if (control is InterestTracker tracker)
                            {
                                tracker.Value = InterestTracker.NO_VALUE;
                            }
                        }

                        comboHobbyOneTrue.SelectedIndex = -1;
                    }
                }

                { // Badges - Requires OfB (Seasons and FreeTime)
                    grpBadges.Enabled = currentMemberData.HasBadges;

                    foreach (InterestTracker tracker in new InterestTracker[] { trackBadgeCashier, trackBadgeCosmetics, trackBadgeFlorist, trackBadgeRobotery, trackBadgeSales, trackBadgeStocking, trackBadgeToyMaking })
                    {
                        tracker.Value = currentMemberData.GetBadgeValue(tracker.TokenGuid);
                    }

                    foreach (InterestTracker tracker in new InterestTracker[] { trackBadgeFishing, trackBadgeGardening })
                    {
                        if (currentMemberData.HasSeasonsBadges)
                        {
                            tracker.Enabled = true;
                            tracker.Value = currentMemberData.GetBadgeValue(tracker.TokenGuid);
                        }
                        else
                        {
                            tracker.Enabled = false;
                            tracker.Value = InterestTracker.NO_VALUE;
                        }
                    }

                    foreach (InterestTracker tracker in new InterestTracker[] { trackBadgePottery, trackBadgeSewing })
                    {
                        if (currentMemberData.HasFreeTimeBadges)
                        {
                            tracker.Enabled = true;
                            tracker.Value = currentMemberData.GetBadgeValue(tracker.TokenGuid);
                        }
                        else
                        {
                            tracker.Enabled = false;
                            tracker.Value = InterestTracker.NO_VALUE;
                        }
                    }
                }

                ignoreInterestsChanges = false;
            }
            else
            {
                ClearInterestsTab();
            }
        }

        private void EnableJobGroup(bool enabled)
        {
            grpJob.Enabled = enabled;

            if (!enabled)
            {
                comboJobType.SelectedIndex = -1;
                textJobGUID.Value = GuidTextBox.NO_VALUE;

                lblJobLevel.Visible = trackJobLevel.Visible = textJobLevel.Visible = false;
                lblJobPerformance.Visible = trackJobPerformance.Visible = textJobPerformance.Visible = false;
                lblJobPTO.Visible = textJobPTO.Visible = lblJobPTOSummary.Visible = false;

                lblJobPension.Visible = textJobPension.Visible = false;
                lblJobRetiredType.Visible = comboJobRetiredType.Visible = textJobRetiredGUID.Visible = false;
                lblJobRetiredLevel.Visible = trackJobRetiredLevel.Visible = textJobRetiredLevel.Visible = false;
            }
            else
            {
                lblJobLevel.Visible = trackJobLevel.Visible = textJobLevel.Visible = true;
                lblJobPerformance.Visible = trackJobPerformance.Visible = textJobPerformance.Visible = true;
                lblJobPTO.Visible = textJobPTO.Visible = lblJobPTOSummary.Visible = true;

                lblJobPension.Visible = textJobPension.Visible = currentMemberData.IsElder;
                lblJobRetiredType.Visible = comboJobRetiredType.Visible = textJobRetiredGUID.Visible = currentMemberData.IsElder;
                lblJobRetiredLevel.Visible = trackJobRetiredLevel.Visible = textJobRetiredLevel.Visible = currentMemberData.IsElder;

                if (currentMemberData.IsTeen)
                {
                    LoadTeenJobs();
                }
                else if (currentMemberData.IsElder)
                {
                    LoadElderJobs();
                }
                else if (currentMemberData.IsPet)
                {
                    LoadPetJobs();
                }
                else
                {
                    LoadAdultJobs();
                }

                SetCombo(comboJobType, currentMemberData.JobGuid.AsUInt());
                textJobGUID.Value = currentMemberData.JobGuid.AsUInt();
                trackJobLevel.Value = currentMemberData.JobLevel;
                textJobLevel.Value = currentMemberData.JobLevel;
                trackJobPerformance.Value = currentMemberData.JobPerformance;
                textJobPerformance.Value = currentMemberData.JobPerformance;
                textJobPTO.Value = currentMemberData.JobPTO;
                lblJobPTOSummary.Text = $"({(textJobPTO.Value == 0 ? 0 : Math.Max(0, (textJobPTO.Value - 1) / 100))} days)";
                textJobPension.Value = currentMemberData.JobPension;
                SetCombo(comboJobRetiredType, currentMemberData.JobRetiredGuid.AsUInt());
                textJobRetiredGUID.Value = currentMemberData.JobRetiredGuid.AsUInt();
                trackJobRetiredLevel.Value = currentMemberData.JobRetiredLevel;
                textJobRetiredLevel.Value = currentMemberData.JobRetiredLevel;
            }
        }

        private void SetCombo(ComboBox combo, uint value)
        {
            bool found = false;

            for (int i = 1; i < combo.Items.Count; ++i)
            {
                if ((combo.Items[i] as UintNamedValue).Value == value)
                {
                    combo.SelectedIndex = i;
                    found = true;
                    break;
                }
            }

            if (!found) combo.SelectedIndex = 0;
        }
        #endregion

        #region Form State
        private bool updatingFormState = false;

        private void UpdateFormState()
        {
            if (updatingFormState) return;

            updatingFormState = true;

            UpdateSaveState();

            btnClosetCopy.Enabled = btnClosetMove.Enabled = btnClosetDelete.Enabled = (gridFamilyCloset.SelectedRows.Count > 0);

            btnSuitcaseEmpty.Enabled = btnSuitcaseSave.Enabled = (gridSuitcase.Rows.Count > 0);
            btnSuitcaseLoad.Enabled = !btnSuitcaseSave.Enabled;
            btnSuitcaseCopy.Enabled = btnSuitcaseMove.Enabled = (gridSuitcase.SelectedRows.Count > 0);

            btnClosetShowAll.Enabled = !filters.IsAll;

            btnSafeCopy.Enabled = btnSafeMove.Enabled = btnSafeDelete.Enabled = (gridFamilySafe.SelectedRows.Count > 0);

            btnJewelboxEmpty.Enabled = btnJewelboxSave.Enabled = (gridJewelbox.Rows.Count > 0);
            btnJewelboxLoad.Enabled = !btnJewelboxSave.Enabled;
            btnJewelboxCopy.Enabled = btnJewelboxMove.Enabled = (gridJewelbox.SelectedRows.Count > 0);

            btnSafeShowAll.Enabled = !filters.IsAll;

            panelFamily.Enabled = (currentFamily != null);

            if (currentFamily == null)
            {
                tabPages.SelectedIndex = 0;

                tabPages.TabPages.Remove(tabCloset);
                tabPages.TabPages.Remove(tabSafe);

                tabPages.TabPages.Remove(tabCareer);
                tabPages.TabPages.Remove(tabSkills);
                tabPages.TabPages.Remove(tabInterests);
            }
            else
            {
                if (currentFamily.IsNPCFamily)
                {
                    tabPages.SelectedIndex = 0;

                    tabPages.TabPages.Remove(tabCloset);
                    tabPages.TabPages.Remove(tabSafe);
                }
                else
                {
                    if (!tabPages.TabPages.Contains(tabCloset)) tabPages.TabPages.Insert(1, tabCloset);
                    if (!tabPages.TabPages.Contains(tabSafe)) tabPages.TabPages.Insert(2, tabSafe);
                }

                if (!tabPages.TabPages.Contains(tabCareer)) tabPages.TabPages.Add(tabCareer);
                if (!tabPages.TabPages.Contains(tabSkills)) tabPages.TabPages.Add(tabSkills);
                if (!tabPages.TabPages.Contains(tabInterests)) tabPages.TabPages.Add(tabInterests);

                panelFamily.Enabled = !currentFamily.IsNPCFamily;

                foreach (DataGridViewRow row in gridFamilyMembers.Rows)
                {
                    string splitFile = row.Cells["colSplitFile"].Value as string;

                    if (IsAdvancedMode && menuItemHighlightSplitFiles.Checked && "Y".Equals(splitFile, StringComparison.OrdinalIgnoreCase))
                    {
                        row.DefaultCellStyle.BackColor = colourSplitFileHighlight;
                    }
                    else
                    {
                        row.DefaultCellStyle.BackColor = Color.Empty;
                    }
                }
            }

            gridFamilyMembers.Columns["colSplitFile"].Visible = (IsAdvancedMode && menuItemShowSplitFiles.Checked);

            UpdateClosetTabState();
            UpdateSafeTabState();

            updatingFormState = false;
        }

        private void UpdateClosetTabState()
        {
            gridFamilyCloset.Enabled = clothingCache.CachesExist();
            lblClosetCachesNeeded.Visible = !gridFamilyCloset.Enabled;
        }

        private void UpdateSafeTabState()
        {
            gridFamilySafe.Enabled = jewelleryCache.CachesExist();
            lblSafeCachesNeeded.Visible = !gridFamilySafe.Enabled;
        }

        private void UpdateCareerTabState()
        {
            LoadCareers();

            textUniTimeLeft.Maximum = careerCache.SemesterLength;
            trackUniTimeLeft.Maximum = (int)textUniTimeLeft.Maximum;
        }

        private void UpdateSaveState()
        {
            menuItemSaveAll.Enabled = btnSave.Enabled = packageCache.IsDirty;
        }
        #endregion

        #region File Menu Actions
        private void OnConfigurationClicked(object sender, EventArgs e)
        {
            Form config = new ConfigDialog(true);

            if (config.ShowDialog() == DialogResult.OK)
            {
                // Perform any reload necessary after changing the objects.package location
            }
        }
        #endregion

        #region Mode Menu Actions
        private void OnModeOpening(object sender, EventArgs e)
        {
            menuItemAdvanced.Enabled = !Sims2ToolsLib.AllAdvancedMode;
            if (Sims2ToolsLib.AllAdvancedMode) menuItemAdvanced.Checked = true;
        }

        private void OnAdvancedModeChanged(object sender, EventArgs e)
        {
            UpdateFormState();
        }
        #endregion

        #region Options Menu Actions
        private void OnOptionsOpening(object sender, EventArgs e)
        {
            menuItemShowSplitFiles.Visible = menuItemHighlightSplitFiles.Visible = toolStripSeparatorSplitFiles.Visible = IsAdvancedMode;
        }

        private void OnUseCodesClicked(object sender, EventArgs e)
        {
            gridFamilyMembers.Columns["colAgeCode"].Visible = gridFamilyMembers.Columns["colGenderCode"].Visible = menuItemUseCodes.Checked;
            gridFamilyMembers.Columns["colAge"].Visible = gridFamilyMembers.Columns["colGender"].Visible = !menuItemUseCodes.Checked;

            gridFamilyCloset.Columns["colClosetAgeCode"].Visible = gridFamilyCloset.Columns["colClosetGenderCode"].Visible = menuItemUseCodes.Checked;
            gridFamilyCloset.Columns["colClosetAge"].Visible = gridFamilyCloset.Columns["colClosetGender"].Visible = !menuItemUseCodes.Checked;

            gridSuitcase.Columns["colSuitcaseAgeCode"].Visible = gridSuitcase.Columns["colSuitcaseGenderCode"].Visible = menuItemUseCodes.Checked;
            gridSuitcase.Columns["colSuitcaseAge"].Visible = gridSuitcase.Columns["colSuitcaseGender"].Visible = !menuItemUseCodes.Checked;

            gridFamilySafe.Columns["colSafeAgeCode"].Visible = gridFamilySafe.Columns["colSafeGenderCode"].Visible = menuItemUseCodes.Checked;
            gridFamilySafe.Columns["colSafeAge"].Visible = gridFamilySafe.Columns["colSafeGender"].Visible = !menuItemUseCodes.Checked;

            gridJewelbox.Columns["colJewelboxAgeCode"].Visible = gridJewelbox.Columns["colJewelboxGenderCode"].Visible = menuItemUseCodes.Checked;
            gridJewelbox.Columns["colJewelboxAge"].Visible = gridJewelbox.Columns["colJewelboxGender"].Visible = !menuItemUseCodes.Checked;
        }

        private void OnShowSplitFilesClicked(object sender, EventArgs e)
        {
            gridFamilyMembers.Columns["colSplitFile"].Visible = (IsAdvancedMode && menuItemShowSplitFiles.Checked);
        }

        private void OnHighlightSplitFilesClicked(object sender, EventArgs e)
        {
            UpdateFormState();
        }

        private void OnIncludeNPCsClicked(object sender, EventArgs e)
        {
            if (sender == menuItemIncludeNPCs)
            {
                if (menuItemIncludeNPCs.Checked) menuItemOnlyNPCs.Checked = false;
            }
            else if (sender == menuItemOnlyNPCs)
            {
                if (menuItemOnlyNPCs.Checked) menuItemIncludeNPCs.Checked = false;
            }

            DoWork_FillHoodTree(null, DBPFData.INSTANCE_NULL);
        }

        private void OnTeensHaveAdultJobsClicked(object sender, EventArgs e)
        {
            if (IsCareerTabActive && (lastJobs == CareerTypes.TeenJob))
            {
                ignoreCareerChanges = true;
                lastJobs = CareerTypes.Unknown;
                LoadTeenJobs();
                SetCombo(comboJobType, textJobGUID.Value);
                ignoreCareerChanges = false;
            }
        }

        private void OnYAsHaveAdultJobsClicked(object sender, EventArgs e)
        {
            if (IsCareerTabActive && currentMemberData.IsYoungAdult)
            {
                ignoreCareerChanges = true;
                EnableJobGroup(menuItemYAsHaveAdultJobs.Checked);
                ignoreCareerChanges = false;
            }
        }
        private void OnInterestsDisplayOpening(object sender, EventArgs e)
        {
            menuItemIntDisplayBarAndBox.Checked = (interestsTrackersStyle == InterestTrackerStyle.BarAndBox);
            menuItemIntDisplayBarOnly.Checked = (interestsTrackersStyle == InterestTrackerStyle.BarOnly);
            menuItemIntDisplayBoxOnly.Checked = (interestsTrackersStyle == InterestTrackerStyle.BoxOnly);
        }

        private void OnInterestsDisplayClicked(object sender, EventArgs e)
        {
            if (sender == menuItemIntDisplayBarAndBox)
            {
                UpdateInterestTrackers(InterestTrackerStyle.BarAndBox);
            }
            else if (sender == menuItemIntDisplayBarOnly)
            {
                UpdateInterestTrackers(InterestTrackerStyle.BarOnly);
            }
            else if (sender == menuItemIntDisplayBoxOnly)
            {
                UpdateInterestTrackers(InterestTrackerStyle.BoxOnly);
            }
        }

        private void UpdateInterestTrackers(InterestTrackerStyle newTrackerStyle)
        {
            if (newTrackerStyle != interestsTrackersStyle)
            {
                interestsTrackersStyle = newTrackerStyle;

                foreach (Control control in grpInterests.Controls)
                {
                    if (control is InterestTracker tracker)
                    {
                        tracker.Style = interestsTrackersStyle;
                    }
                }

                foreach (Control control in grpHobbies.Controls)
                {
                    if (control is InterestTracker tracker)
                    {
                        tracker.Style = interestsTrackersStyle;
                    }
                }

                foreach (Control control in grpBadges.Controls)
                {
                    if (control is InterestTracker tracker)
                    {
                        tracker.Style = interestsTrackersStyle;
                    }
                }
            }
        }
        #endregion

        #region Language Menu Actions
        private void OnLangClicked(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;

            try
            {
                MetaData.Languages newPrefLid = (MetaData.Languages)Convert.ToInt16(menuItem.Tag as string, 16);

                if (newPrefLid != prefLid)
                {
                    RegistryTools.SaveSetting(FamilyManagerApp.RegistryKey + @"\Options", menuLanguage.Name, menuItem.Tag);

                    foreach (ToolStripMenuItem otherItem in menuLanguage.DropDownItems)
                    {
                        if (otherItem != menuItem)
                        {
                            otherItem.Checked = false;
                        }
                    }

                    UpdateCurrentFamily(); // Do this before changing the preferred language

                    prefLid = newPrefLid;

                    DoWork_FillHoodTree(lastHoodNode?.HoodSubFolder, (lastFamilyNode == null) ? DBPFData.INSTANCE_NULL : lastFamilyNode.FamilyId);
                }
            }
            catch (Exception) { }
        }

        public static bool IsDefLang => (prefLid == defLid);

        public static string GetString(Str str, int index)
        {
            string value = GetString(str, index, prefLid);

            if (value == null && prefLid != defLid)
            {
                value = GetString(str, index, defLid);
            }

            return value;
        }

        private static string GetString(Str str, int index, MetaData.Languages lid)
        {
            string value = null;

            List<StrItem> langItems = str.LanguageItems(lid);
            if (langItems != null && index < langItems.Count)
            {
                value = langItems[index].Title;
            }

            return value;
        }

        public static void SetString(Str str, int index, string value)
        {
            if (GetString(str, index, prefLid) != null)
            {
                SetString(str, index, prefLid, value);
            }
            else
            {
                SetString(str, index, defLid, value);
            }
        }

        private static void SetString(Str str, int index, MetaData.Languages lid, string value)
        {
            List<StrItem> langItems = str.LanguageItems(lid);
            if (langItems != null && index < langItems.Count)
            {
                langItems[index].Title = value;
            }
        }
        #endregion

        #region Cache Menu Actions
        private void OnCachingOpening(object sender, EventArgs e)
        {
            menuItemCachingUpdateCustomCareers.Text = DataCache.CacheExists(DataCache.CacheCareersPath, DataCache.CustomCareerFilename) ? "Update Custom Careers Cache" : "Create Custom Careers Cache";

            menuItemCachingUpdateMaxisClothes.Text = DataCache.CacheExists(DataCache.CacheClothesPath, DataCache.MaxisClothingFilename) ? "Update Maxis Clothing Cache" : "Create Maxis Clothing Cache";
            menuItemCachingUpdateCustomClothes.Text = DataCache.CacheExists(DataCache.CacheClothesPath, DataCache.CustomClothingFilename) ? "Update Custom Clothing Cache" : "Create Custom Clothing Cache";

            menuItemCachingUpdateMaxisJewellery.Text = DataCache.CacheExists(DataCache.CacheJewelleryPath, DataCache.MaxisJewelleryFilename) ? "Update Maxis Jewellery Cache" : "Create Maxis Jewellery Cache";
            menuItemCachingUpdateCustomJewellery.Text = DataCache.CacheExists(DataCache.CacheJewelleryPath, DataCache.CustomJewelleryFilename) ? "Update Custom Jewellery Cache" : "Create Custom Jewellery Cache";

            menuItemCachingRemoveLocal.Visible = menuItemCachingRemoveThumbnails.Visible = toolStripSeparatorCaching.Visible = IsAdvancedMode;
        }

        private void OnCachingUpdateMaxisOutfits(object sender, EventArgs e)
        {
            TypeTypeID typeId = (sender == menuItemCachingUpdateMaxisClothes ? Gzps.TYPE : Xmol.TYPE);

            ProgressDialog progressDialog = new ProgressDialog(typeId);
            progressDialog.DoWork += new ProgressDialog.DoWorkEventHandler(DoAsyncWork_UpdateMaxisOutfits);

            DialogResult result = progressDialog.ShowDialog();

            if (result == DialogResult.Abort)
            {
                logger.Error(progressDialog.Result.Error.Message);
                logger.Info(progressDialog.Result.Error.StackTrace);

                MsgBox.Show($"An error occured while processing\n{((typeId == Gzps.TYPE) ? clothingCache : jewelleryCache).ErrorPackagePath}", "Error!", MessageBoxButtons.OK);
            }
            else
            {
                if (result == DialogResult.Cancel)
                {
                    // Update Maxis Outfits cancelled
                }
                else
                {
                    // Update Maxis Outfits completed
                    UpdateClosetTabState();
                    UpdateSafeTabState();
                }
            }
        }

        private void DoAsyncWork_UpdateMaxisOutfits(ProgressDialog sender, DoWorkEventArgs args)
        {
            TypeTypeID typeId = (TypeTypeID)args.Argument;

            sender.VisualMode = ProgressBarDisplayMode.CustomText;
            sender.SetProgress(0, $"Loading Maxis {(typeId == Gzps.TYPE ? "Clothes" : "Jewellery")}");

            if (typeId == Gzps.TYPE)
            {
                clothingCache.ReloadMaxisOutfits(sender, typeId);
            }
            else
            {
                jewelleryCache.ReloadMaxisOutfits(sender, typeId);
            }
        }

        private void OnCachingUpdateCustomOutfits(object sender, EventArgs e)
        {
            TypeTypeID typeId = (sender == menuItemCachingUpdateCustomClothes ? Gzps.TYPE : Xmol.TYPE);

            ProgressDialog progressDialog = new ProgressDialog(typeId);
            progressDialog.DoWork += new ProgressDialog.DoWorkEventHandler(DoAsyncWork_UpdateCustomOutfits);

            DialogResult result = progressDialog.ShowDialog();

            if (result == DialogResult.Abort)
            {
                logger.Error(progressDialog.Result.Error.Message);
                logger.Info(progressDialog.Result.Error.StackTrace);

                MsgBox.Show($"An error occured while processing\n{((typeId == Gzps.TYPE) ? clothingCache : jewelleryCache).ErrorPackagePath}", "Error!", MessageBoxButtons.OK);
            }
            else
            {
                if (result == DialogResult.Cancel)
                {
                    // Update Custom Outfits cancelled
                }
                else
                {
                    // Update Custom Outfits completed
                    UpdateClosetTabState();
                    UpdateSafeTabState();
                }
            }
        }

        private void DoAsyncWork_UpdateCustomOutfits(ProgressDialog sender, DoWorkEventArgs args)
        {
            TypeTypeID typeId = (TypeTypeID)args.Argument;

            sender.VisualMode = ProgressBarDisplayMode.CustomText;
            sender.SetProgress(0, $"Loading Custom {(typeId == Gzps.TYPE ? "Clothes" : "Jewellery")}");

            if (typeId == Gzps.TYPE)
            {
                clothingCache.ReloadCustomOutfits(sender, typeId);
            }
            else
            {
                jewelleryCache.ReloadCustomOutfits(sender, typeId);
            }
        }


        private void OnCachingUpdateCustomCareers(object sender, EventArgs e)
        {
            ProgressDialog progressDialog = new ProgressDialog();
            progressDialog.DoWork += new ProgressDialog.DoWorkEventHandler(DoAsyncWork_UpdateCustomCareers);

            DialogResult result = progressDialog.ShowDialog();

            if (result == DialogResult.Abort)
            {
                logger.Error(progressDialog.Result.Error.Message);
                logger.Info(progressDialog.Result.Error.StackTrace);

                MsgBox.Show($"An error occured while processing\n{careerCache.ErrorPackagePath}", "Error!", MessageBoxButtons.OK);
            }
            else
            {
                if (result == DialogResult.Cancel)
                {
                    // Update Custom Careers cancelled
                }
                else
                {
                    // Update Custom Careers completed
                    if (currentMemberData != null) UpdateCareerTabState();
                }
            }
        }

        private void DoAsyncWork_UpdateCustomCareers(ProgressDialog sender, DoWorkEventArgs args)
        {
            sender.VisualMode = ProgressBarDisplayMode.CustomText;
            sender.SetProgress(0, $"Loading Custom Careers");

            careerCache.ReloadCustomCareers(sender);
            LoadCareers();
        }

        private void OnCachingRemoveLocal(object sender, EventArgs e)
        {
            DataCache.RemoveAll();
            UpdateClosetTabState();
            UpdateSafeTabState();
            UpdateCareerTabState();
        }

        private void OnCachingRemoveThumbnails(object sender, EventArgs e)
        {
            clothingThumbnailsCache.RemoveCaches();
        }
        #endregion

        #region Tabs
        private bool IsFamilyTabActive => IsTabActive(0);
        private bool IsClosetTabActive => IsTabActive(1);
        private bool IsSafeTabActive => IsTabActive(2);
        private bool IsCareerTabActive => IsTabActive(3);
        private bool IsSkillsTabActive => IsTabActive(4);
        private bool IsInterestsTabActive => IsTabActive(5);

        private bool IsTabActive(int index)
        {
            if (index == 0)
            {
                return (tabPages.SelectedIndex == index);
            }
            else if (index == 1 || index == 2)
            {
                return tabPages.Contains(tabCloset) && (tabPages.SelectedIndex == index);
            }
            else
            {
                if (!tabPages.Contains(tabCloset)) index -= 2;

                return (tabPages.SelectedIndex == index);
            }
        }

        private void OnTabPageChanged(object sender, EventArgs e)
        {
            if (IsClosetTabActive)
            {
                if (gridFamilyCloset.Rows.Count == 0)
                {
                    if (lastFamilyNode != null)
                    {
                        DoWork_FillFamilyClosetGrid(lastHoodNode, lastFamilyNode);
                    }
                }
            }
            else if (IsSafeTabActive)
            {
                if (gridFamilySafe.Rows.Count == 0)
                {
                    if (lastFamilyNode != null)
                    {
                        DoWork_FillFamilySafeGrid(lastHoodNode, lastFamilyNode);
                    }
                }
            }
            else if (IsCareerTabActive)
            {
                DoWork_FillCareerTab();
            }
            else if (IsSkillsTabActive)
            {
                DoWork_FillSkillsTab();
            }
            else if (IsInterestsTabActive)
            {
                DoWork_FillInterestsTab();
            }
        }
        #endregion

        #region Family Tab Changes
        bool ignoreFamilyChanges = false;

        private void ClearFamilyTabValues()
        {
            ignoreFamilyChanges = true;

            textFamilyName.Text = textFamilyWriteUp.Text = null;
            textFamilyMoney.Text = textBusinessMoney.Text = null;
            textAddressName.Text = textAddressDesc.Text = null;

            imageHouse.Image = null;

            currentFamily = null;

            ignoreFamilyChanges = false;
        }

        private void OnFamilyControlLeave(object sender, EventArgs e)
        {
            if (ignoreFamilyChanges) return;

            UpdateCurrentFamily();
            UpdateSaveState();
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            if (ignoreFamilyChanges) return;
        }

        private void OnMoneyLockChanged(object sender, EventArgs e)
        {
            textBusinessMoney.Enabled = !ckbMoneyLock.Checked;

            if (textBusinessMoney.Enabled && !textFamilyMoney.Text.Equals(textBusinessMoney.Text))
            {
                textBusinessMoney.Text = textFamilyMoney.Text;
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                UpdateCurrentFamily();
                UpdateSaveState();
            }
        }

        bool ignoreCkb = false;
        private void OnFamilyNameChecked(object sender, EventArgs e)
        {
            if (ignoreCkb) return;

            if (sender is CheckBox ckb)
            {
                ignoreCkb = true;

                bool ticked = ckb.Checked;

                ckbFamilyNameAll.Checked = ckbFamilyNameSame.Checked = ckbFamilyNameSelected.Checked = false;

                ckb.Checked = ticked;

                ignoreCkb = false;
            }
        }
        #endregion

        #region Career Tab Changes
        private void OnSchoolTypeChanged(object sender, EventArgs e)
        {
            if (ignoreCareerChanges) return;

            if (textSchoolGUID.Value != (comboSchoolType.SelectedItem as UintNamedValue).Value)
            {
                ignoreCareerChanges = true;
                textSchoolGUID.Value = (comboSchoolType.SelectedItem as UintNamedValue).Value;
                currentMemberData.SchoolGuid = (TypeGUID)(comboSchoolType.SelectedItem as UintNamedValue).Value;
                ignoreCareerChanges = false;

                UpdateSaveState();
            }
        }

        private void OnSchoolGuidChanged(object sender, EventArgs e)
        {
            if (ignoreCareerChanges) return;

            if (currentMemberData.SchoolGuid.AsUInt() != textSchoolGUID.Value)
            {
                ignoreCareerChanges = true;
                SetCombo(comboSchoolType, textSchoolGUID.Value);
                currentMemberData.SchoolGuid = (TypeGUID)(comboSchoolType.SelectedItem as UintNamedValue).Value;
                ignoreCareerChanges = false;

                UpdateSaveState();
            }
        }

        private void OnSchoolGradeChanged(object sender, EventArgs e)
        {
            if (ignoreCareerChanges) return;

            if (currentMemberData.SchoolGrade != (comboSchoolGrade.SelectedItem as UintNamedValue).Value)
            {
                currentMemberData.SchoolGrade = (comboSchoolGrade.SelectedItem as UintNamedValue).Value;

                UpdateSaveState();
            }
        }

        private void OnUniMajorTypeChanged(object sender, EventArgs e)
        {
            if (ignoreCareerChanges) return;

            if (textMajorGUID.Value != (comboUniMajor.SelectedItem as UintNamedValue).Value)
            {
                ignoreCareerChanges = true;
                textMajorGUID.Value = (comboUniMajor.SelectedItem as UintNamedValue).Value;
                currentMemberData.UniMajorGuid = (TypeGUID)(comboUniMajor.SelectedItem as UintNamedValue).Value;
                ignoreCareerChanges = false;

                UpdateSaveState();
            }
        }

        private void OnUniMajorGuidChanged(object sender, EventArgs e)
        {
            if (ignoreCareerChanges) return;

            if (currentMemberData.UniMajorGuid.AsUInt() != textMajorGUID.Value)
            {
                ignoreCareerChanges = true;
                SetCombo(comboUniMajor, textMajorGUID.Value);
                currentMemberData.UniMajorGuid = (TypeGUID)(comboUniMajor.SelectedItem as UintNamedValue).Value;
                ignoreCareerChanges = false;

                UpdateSaveState();
            }
        }

        private void OnUniOutcomeChanged(object sender, EventArgs e)
        {
            if (ignoreCareerChanges) return;

            lblUniMajor.Enabled = comboUniMajor.Enabled = textMajorGUID.Enabled = (comboUniResult.SelectedIndex != 0);

            ushort flags = (ushort)(currentMemberData.UniInfoFlags & 0xCFBF);

            if (comboUniResult.SelectedIndex == 0)
            {
                // Didn't Go
                comboUniMajor.SelectedIndex = 1; // Undeclared
            }
            else if (comboUniResult.SelectedIndex == 1)
            {
                // Graduated
                flags |= 0x0040;
            }
            else if (comboUniResult.SelectedIndex == 2)
            {
                // Dropped Out
                flags |= 0x1000;
            }
            else if (comboUniResult.SelectedIndex == 3)
            {
                // Expelled
                flags |= 0x2000;
            }

            currentMemberData.UniInfoFlags = flags;

            UpdateSaveState();
        }

        private void OnUniSemesterChanged(object sender, EventArgs e)
        {
            if (ignoreCareerChanges) return;

            if (currentMemberData.UniSemester != (ushort)(comboUniSemester.SelectedItem as UintNamedValue).Value)
            {
                currentMemberData.UniSemester = (ushort)(comboUniSemester.SelectedItem as UintNamedValue).Value;
                currentMemberData.UniInfoFlags &= 0xFFF0;

                switch (currentMemberData.UniSemester)
                {
                    case 1:
                    case 2:
                        currentMemberData.UniInfoFlags |= 0x0001;
                        break;
                    case 3:
                    case 4:
                        currentMemberData.UniInfoFlags |= 0x0002;
                        break;
                    case 5:
                    case 6:
                        currentMemberData.UniInfoFlags |= 0x0004;
                        break;
                    case 7:
                    case 8:
                        currentMemberData.UniInfoFlags |= 0x0008;
                        break;
                }

                UpdateSaveState();
            }
        }

        private void OnUniGpaSliderChanged(object sender, EventArgs e)
        {
            if (ignoreCareerChanges) return;

            textUniGrade.Value = trackUniGrade.Value / 10.0f;
        }

        private void OnUniGpaValueChanged(object sender, EventArgs e)
        {
            if (ignoreCareerChanges) return;

            ushort newGPA = (ushort)(textUniGrade.Value * 10.0f);

            if (currentMemberData.UniCurrentGPA != newGPA)
            {
                ignoreCareerChanges = true;
                currentMemberData.UniCurrentGPA = newGPA;
                trackUniGrade.Value = newGPA;
                ignoreCareerChanges = false;

                UpdateSaveState();
            }
        }

        private void OnUniEffortSliderChanged(object sender, EventArgs e)
        {
            if (ignoreCareerChanges) return;

            textUniEffort.Value = (uint)trackUniEffort.Value;
        }

        private void OnUniEffortValueChanged(object sender, EventArgs e)
        {
            if (ignoreCareerChanges) return;

            if (currentMemberData.UniEffort != (ushort)textUniEffort.Value)
            {
                ignoreCareerChanges = true;
                currentMemberData.UniEffort = (ushort)textUniEffort.Value;
                trackUniEffort.Value = (int)textUniEffort.Value;
                ignoreCareerChanges = false;

                UpdateSaveState();
            }
        }

        private void OnUniProbationChanged(object sender, EventArgs e)
        {
            if (ignoreCareerChanges) return;

            currentMemberData.UniInfoFlags &= 0xFFDF;

            if (ckbUniProbation.Checked) currentMemberData.UniInfoFlags |= 0x0020;

            UpdateSaveState();
        }

        private void OnUniGoodCompletedChanged(object sender, EventArgs e)
        {
            if (ignoreCareerChanges) return;

            currentMemberData.UniInfoFlags &= 0xFFEF;

            if (ckbUniProbation.Checked) currentMemberData.UniInfoFlags |= 0x0010;

            UpdateSaveState();
        }

        private void OnUniTimeLeftSliderChanged(object sender, EventArgs e)
        {
            if (ignoreCareerChanges) return;

            textUniTimeLeft.Value = (uint)trackUniTimeLeft.Value;
        }

        private void OnUniTimeLeftValueChanged(object sender, EventArgs e)
        {
            if (ignoreCareerChanges) return;

            if (currentMemberData.UniTimeLeft != (ushort)textUniTimeLeft.Value)
            {
                ignoreCareerChanges = true;
                currentMemberData.UniTimeLeft = (ushort)textUniTimeLeft.Value;
                trackUniTimeLeft.Value = (int)textUniTimeLeft.Value;
                ignoreCareerChanges = false;

                UpdateSaveState();
            }
        }

        private void OnUniInfluenceValueChanged(object sender, EventArgs e)
        {
            if (ignoreCareerChanges) return;

            if (currentMemberData.UniInfluence != (ushort)textUniInfluence.Value)
            {
                currentMemberData.UniInfluence = (ushort)textUniInfluence.Value;

                UpdateSaveState();
            }
        }

        private void OnJobTypeChanged(object sender, EventArgs e)
        {
            if (ignoreCareerChanges) return;

            if (currentMemberData.JobGuid != (TypeGUID)(comboJobType.SelectedItem as UintNamedValue).Value)
            {
                ignoreCareerChanges = true;

                textJobGUID.Value = (comboJobType.SelectedItem as UintNamedValue).Value;
                currentMemberData.JobGuid = (TypeGUID)(comboJobType.SelectedItem as UintNamedValue).Value;

                ignoreCareerChanges = false;

                if (!currentMemberData.IsUnemployed)
                {
                    if (trackJobLevel.Value == 0)
                    {
                        trackJobLevel.Value = 1;
                    }
                }
                else
                {
                    trackJobLevel.Value = 0;
                }

                UpdateSaveState();
            }
        }

        private void OnJobGuidChanged(object sender, EventArgs e)
        {
            if (ignoreCareerChanges) return;

            if (currentMemberData.JobGuid.AsUInt() != textJobGUID.Value)
            {
                ignoreCareerChanges = true;

                SetCombo(comboJobType, textJobGUID.Value);
                currentMemberData.JobGuid = (TypeGUID)(comboJobType.SelectedItem as UintNamedValue).Value;

                ignoreCareerChanges = false;

                if (!currentMemberData.IsUnemployed)
                {
                    if (trackJobLevel.Value == 0)
                    {
                        trackJobLevel.Value = 1;
                    }
                }
                else
                {
                    trackJobLevel.Value = 0;
                }

                UpdateSaveState();
            }
        }

        private void OnJobLevelSliderChanged(object sender, EventArgs e)
        {
            if (ignoreCareerChanges) return;

            textJobLevel.Value = (uint)trackJobLevel.Value;
        }

        private void OnJobLevelValueChanged(object sender, EventArgs e)
        {
            if (ignoreCareerChanges) return;

            if (currentMemberData.JobLevel != (ushort)textJobLevel.Value)
            {
                ignoreCareerChanges = true;
                currentMemberData.JobLevel = (ushort)textJobLevel.Value;
                trackJobLevel.Value = (int)textJobLevel.Value;
                ignoreCareerChanges = false;

                UpdateSaveState();
            }
        }

        private void OnJobPerformanceSliderChanged(object sender, EventArgs e)
        {
            if (ignoreCareerChanges) return;

            textJobPerformance.Value = trackJobPerformance.Value;
        }

        private void OnJobPerformanceValueChanged(object sender, EventArgs e)
        {
            if (ignoreCareerChanges) return;

            if (currentMemberData.JobPerformance != (ushort)textJobPerformance.Value)
            {
                ignoreCareerChanges = true;
                currentMemberData.JobPerformance = (ushort)textJobPerformance.Value;
                trackJobPerformance.Value = (int)textJobPerformance.Value;
                ignoreCareerChanges = false;

                UpdateSaveState();
            }
        }

        private void OnJobPtoValueChanged(object sender, EventArgs e)
        {
            if (ignoreCareerChanges) return;

            if (currentMemberData.JobPTO != (ushort)textJobPTO.Value)
            {
                currentMemberData.JobPTO = (ushort)textJobPTO.Value;
                lblJobPTOSummary.Text = $"({(textJobPTO.Value == 0 ? 0 : Math.Max(0, (textJobPTO.Value - 1) / 100))} days)";

                UpdateSaveState();
            }
        }

        private void OnJobPensionValueChanged(object sender, EventArgs e)
        {
            if (ignoreCareerChanges) return;

            if (currentMemberData.JobPension != (ushort)textJobPension.Value)
            {
                currentMemberData.JobPension = (ushort)textJobPension.Value;

                UpdateSaveState();
            }
        }

        private void OnJobRetiredTypeChanged(object sender, EventArgs e)
        {
            if (ignoreCareerChanges) return;

            if (currentMemberData.JobRetiredGuid != (TypeGUID)(comboJobRetiredType.SelectedItem as UintNamedValue).Value)
            {
                ignoreCareerChanges = true;

                textJobRetiredGUID.Value = (comboJobRetiredType.SelectedItem as UintNamedValue).Value;
                currentMemberData.JobRetiredGuid = (TypeGUID)(comboJobRetiredType.SelectedItem as UintNamedValue).Value;

                ignoreCareerChanges = false;

                FixRetiredValues();

                UpdateSaveState();
            }
        }

        private void OnJobRetiredGuidChanged(object sender, EventArgs e)
        {
            if (ignoreCareerChanges) return;

            if (currentMemberData.JobRetiredGuid.AsUInt() != textJobRetiredGUID.Value)
            {
                ignoreCareerChanges = true;

                SetCombo(comboJobRetiredType, textJobRetiredGUID.Value);
                currentMemberData.JobRetiredGuid = (TypeGUID)(comboJobRetiredType.SelectedItem as UintNamedValue).Value;

                ignoreCareerChanges = false;

                FixRetiredValues();

                UpdateSaveState();
            }
        }

        private void FixRetiredValues()
        {
            if (currentMemberData.IsRetiredUnemployed)
            {
                trackJobRetiredLevel.Value = 0;
                textJobPension.Value = 0;
            }
            else
            {
                if (trackJobRetiredLevel.Value == 0)
                {
                    trackJobRetiredLevel.Value = 1;
                }

                if (textJobPension.Value == 0)
                {
                    textJobPension.Value = 1;
                }
            }
        }

        private void OnJobRetiredLevelSliderChanged(object sender, EventArgs e)
        {
            if (ignoreCareerChanges) return;

            textJobRetiredLevel.Value = (uint)trackJobRetiredLevel.Value;
        }

        private void OnJobRetiredLevelValueChanged(object sender, EventArgs e)
        {
            if (ignoreCareerChanges) return;

            if (currentMemberData.JobRetiredLevel != (ushort)textJobRetiredLevel.Value)
            {
                ignoreCareerChanges = true;
                currentMemberData.JobRetiredLevel = (ushort)textJobRetiredLevel.Value;
                trackJobRetiredLevel.Value = (int)textJobRetiredLevel.Value;
                ignoreCareerChanges = false;

                UpdateSaveState();
            }
        }
        #endregion

        #region Validation
        private void OnValidated_Ok(object sender, EventArgs e)
        {
            (sender as Control).BackColor = SystemColors.Window;
        }

        private void OnValidating_NotEmpty(object sender, CancelEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    e.Cancel = true;

                    textBox.BackColor = colourValidationError;
                }
            }
        }

        private void OnValidating_Money(object sender, CancelEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (!(Int32.TryParse(textBox.Text, out int cash) && cash >= 0))
                {
                    e.Cancel = true;

                    textBox.BackColor = colourValidationError;
                }
            }
        }
        #endregion

        #region Member Context Menu
        private void OnContextMembersOpening(object sender, CancelEventArgs e)
        {
            if (gridFamilyMembers.Rows.Count < 1)
            {
                e.Cancel = true;
                return;
            }

            if (IsClosetTabActive || IsSafeTabActive)
            {
                menuContextMemberChangeSimName.Visible = menuContextMemberChangeFamilyName.Visible = false;
                menuContextMemberChangeDays.Visible = false;

                menuContextMemberSeparator1.Visible = menuContextMemberMergeSplitFiles.Visible = false;

                menuContextMemberFilterAll.Visible = true;
                menuContextMemberFilterAll.Enabled = !filters.IsAll;

                menuContextMemberFilterSelected.Visible = false;
                menuContextMemberFilterThis.Visible = true;

                if (!(mouseLocation == null || mouseLocation.RowIndex == -1))
                {
                    // Mouse has to be over a selected row
                    foreach (DataGridViewRow selectedRow in gridFamilyMembers.SelectedRows)
                    {
                        if (mouseLocation.RowIndex == selectedRow.Index)
                        {
                            menuContextMemberFilterSelected.Visible = true;
                            menuContextMemberFilterThis.Visible = false;
                            break;
                        }
                    }
                }
            }
            else
            {
                // Just assume it's the family tab
                menuContextMemberChangeSimName.Visible = menuContextMemberChangeFamilyName.Visible = true;
                menuContextMemberChangeDays.Visible = true;

                menuContextMemberFilterAll.Visible = false;
                menuContextMemberFilterSelected.Visible = false;
                menuContextMemberFilterThis.Visible = false;

                menuContextMemberChangeFamilyName.Enabled = (gridFamilyMembers.SelectedRows.Count > 0);
                menuContextMemberChangeSimName.Enabled = false;

                menuContextMemberSeparator1.Visible = menuContextMemberMergeSplitFiles.Visible = false;

                if (!(mouseLocation == null || mouseLocation.RowIndex == -1))
                {
                    menuContextMemberChangeSimName.Enabled = true;

                    if (IsAdvancedMode)
                    {
                        if (!packageCache.IsDirty) // Doing this after doing some edits is not the best idea the user had!
                        {
                            string splitFile = gridFamilyMembers.Rows[mouseLocation.RowIndex].Cells["colSplitFile"].Value as string;

                            if ("Y".Equals(splitFile, StringComparison.OrdinalIgnoreCase))
                            {
                                menuContextMemberSeparator1.Visible = menuContextMemberMergeSplitFiles.Visible = true;
                            }
                        }
                    }
                }
            }
        }

        private void OnChangeSimNameClicked(object sender, EventArgs e)
        {
            if (!(mouseLocation == null || mouseLocation.RowIndex == -1))
            {
                DataGridViewRow row = gridFamilyMembers.Rows[mouseLocation.RowIndex];
                CharacterData data = (row.Cells["colData"].Value as CharacterData);

                TextAndTextEntryDialog dialog = new TextAndTextEntryDialog("Change Sim's Name", "New Given Name", data.GivenName(prefLid), "New Family Name", data.FamilyName(prefLid));

                if (dialog.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.TextEntry1) && !string.IsNullOrWhiteSpace(dialog.TextEntry2))
                {
                    ChangeMemberName(row, dialog.TextEntry1, dialog.TextEntry2);

                    UpdateFormState();
                }
            }
        }

        private void OnChangeFamilyNameClicked(object sender, EventArgs e)
        {
            if (!(mouseLocation == null || mouseLocation.RowIndex == -1))
            {
                int rowIndex = mouseLocation.RowIndex;

                foreach (DataGridViewRow selectedRow in gridFamilyMembers.SelectedRows)
                {
                    if (mouseLocation.RowIndex == selectedRow.Index)
                    {
                        rowIndex = -1;
                        break;
                    }
                }

                TextEntryDialog dialog = new TextEntryDialog("Change Family Name", "New Family Name", textFamilyName.Text);

                if (dialog.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.TextEntry))
                {
                    if (rowIndex == -1)
                    {
                        foreach (DataGridViewRow row in gridFamilyMembers.SelectedRows)
                        {
                            ChangeMemberFamilyName(row, dialog.TextEntry);
                        }
                    }
                    else
                    {
                        ChangeMemberFamilyName(gridFamilyMembers.Rows[rowIndex], dialog.TextEntry);
                    }

                    UpdateFormState();
                }
            }
        }

        private void ChangeMemberName(DataGridViewRow row, string newGivenName, string newFamilyName)
        {
            CharacterData data = (row.Cells["colData"].Value as CharacterData);
            data?.SetGivenName(prefLid, newGivenName);
            data?.SetFamilyName(prefLid, newFamilyName);
            row.Cells["colFirstName"].Value = $"{data.GivenName(prefLid)} {data.FamilyName(prefLid)}";
        }

        private void ChangeMemberFamilyName(DataGridViewRow row, string newFamilyName)
        {
            CharacterData data = (row.Cells["colData"].Value as CharacterData);
            data?.SetFamilyName(prefLid, newFamilyName);
            row.Cells["colFirstName"].Value = $"{data.GivenName(prefLid)} {data.FamilyName(prefLid)}";
        }

        private void OnChangeDaysClicked(object sender, EventArgs e)
        {
            if (!(mouseLocation == null || mouseLocation.RowIndex == -1))
            {
                int rowIndex = mouseLocation.RowIndex;

                foreach (DataGridViewRow selectedRow in gridFamilyMembers.SelectedRows)
                {
                    if (mouseLocation.RowIndex == selectedRow.Index)
                    {
                        rowIndex = -1;
                        break;
                    }
                }

                TextEntryDialog dialog = new TextEntryDialog("Change Days Remaining", "Days Adjustment (+/-)", "");

                if (dialog.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.TextEntry))
                {
                    bool valid = false;
                    string dayAdjust = dialog.TextEntry;
                    int pos = dayAdjust.IndexOf(":");

                    short daysLow;
                    short daysHigh = 0;

                    if (pos == -1)
                    {
                        if (Int16.TryParse(dayAdjust, out daysLow))
                        {
                            daysHigh = daysLow;
                            valid = (daysLow != 0);
                        }
                    }
                    else
                    {
                        if (Int16.TryParse(dayAdjust.Substring(0, pos), out daysLow) && Int16.TryParse(dayAdjust.Substring(pos + 1), out daysHigh))
                        {
                            valid = (daysLow != daysHigh || daysLow != 0);
                        }
                    }

                    if (valid)
                    {
                        if (rowIndex == -1)
                        {
                            foreach (DataGridViewRow row in gridFamilyMembers.SelectedRows)
                            {
                                ChangeMemberDays(row, daysLow, daysHigh);
                            }
                        }
                        else
                        {
                            ChangeMemberDays(gridFamilyMembers.Rows[rowIndex], daysLow, daysHigh);
                        }

                        UpdateFormState();
                    }
                }
            }
        }

        private void ChangeMemberDays(DataGridViewRow row, int daysLow, int daysHigh)
        {
            int days = daysLow;

            if (daysLow != daysHigh)
            {
                if (daysLow > daysHigh)
                {
                    days = daysLow;
                    daysLow = daysHigh;
                    daysHigh = days;
                }

                days = (new Random()).Next(daysLow, daysHigh + 1);
            }

            CharacterData data = (row.Cells["colData"].Value as CharacterData);
            data?.ChangeDaysLeft(days);
            row.Cells["colDaysLeft"].Value = data.DaysLeft;
        }

        private bool confirmBackupBeforeSplit = true;
        private void OnMergeSplitFilesClicked(object sender, EventArgs e)
        {
            if (!(mouseLocation == null || mouseLocation.RowIndex == -1))
            {
                DataGridViewRow row = gridFamilyMembers.Rows[mouseLocation.RowIndex];
                CharacterData characterData = (row.Cells["colData"].Value as CharacterData);

                if (characterData.IsSplit && "Y".Equals(row.Cells["colSplitFile"].Value as string, StringComparison.OrdinalIgnoreCase))
                {
                    thumbBox.Visible = false;

                    if (confirmBackupBeforeSplit)
                    {
                        if (MsgBox.Show("Please confirm that you have a recent backup of your hood(s)\nAnd understand the split character file merging process", "Confirm Recent Hood Backups", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) == DialogResult.Yes)
                        {
                            confirmBackupBeforeSplit = false;
                        }
                        else
                        {
                            return;
                        }
                    }

                    characterData.FixSplit(packageCache);
                }
                else
                {
                    logger.Warn($"{characterData.PackageName} does not appear to be split!");
                }
            }

            DoWork_FillFamilyGrid(lastHoodNode, lastFamilyNode);
            UpdateFormState();
        }
        #endregion

        #region Closet Context Menu
        private void OnContextClosetOpening(object sender, CancelEventArgs e)
        {
            if (gridFamilyCloset.Rows.Count < 1)
            {
                e.Cancel = true;
                return;
            }

            menuContextClosetFilterAll.Enabled = !filters.IsAll;
            menuContextClosetFilterSelected.Enabled = (gridFamilyMembers.SelectedRows.Count > 0);
            menuContextClosetFilterUnwearable.Enabled = !filters.IsInverted;

            menuContextClosetCopyToSuitcase.Enabled = menuContextClosetMoveToSuitcase.Enabled = false;
            menuContextClosetDelete.Enabled = false;

            if (!(mouseLocation == null || mouseLocation.RowIndex == -1))
            {
                // Mouse has to be over a selected row
                foreach (DataGridViewRow selectedRow in gridFamilyCloset.SelectedRows)
                {
                    if (mouseLocation.RowIndex == selectedRow.Index)
                    {
                        menuContextClosetCopyToSuitcase.Enabled = menuContextClosetMoveToSuitcase.Enabled = true;
                        menuContextClosetDelete.Enabled = true;
                        break;
                    }
                }
            }
        }
        #endregion

        #region Suitcase Context Menu
        private void OnContextSuitcaseOpening(object sender, CancelEventArgs e)
        {
            if (gridSuitcase.Rows.Count < 1)
            {
                e.Cancel = true;
                return;
            }

            menuContextSuitcaseCopyToCloset.Enabled = menuContextSuitcaseMoveToCloset.Enabled = false;
            menuContextSuitcaseDelete.Enabled = false;

            if (!(mouseLocation == null || mouseLocation.RowIndex == -1))
            {
                // Mouse has to be over a selected row
                foreach (DataGridViewRow selectedRow in gridSuitcase.SelectedRows)
                {
                    if (mouseLocation.RowIndex == selectedRow.Index)
                    {
                        menuContextSuitcaseCopyToCloset.Enabled = menuContextSuitcaseMoveToCloset.Enabled = true;
                        menuContextSuitcaseDelete.Enabled = true;
                        break;
                    }
                }
            }
        }
        #endregion

        #region Safe Context Menu
        private void OnContextSafeOpening(object sender, CancelEventArgs e)
        {
            if (gridFamilySafe.Rows.Count < 1)
            {
                e.Cancel = true;
                return;
            }

            menuContextSafeFilterAll.Enabled = !filters.IsAll;
            menuContextSafeFilterSelected.Enabled = (gridFamilyMembers.SelectedRows.Count > 0);
            menuContextSafeFilterUnwearable.Enabled = !filters.IsInverted;

            menuContextSafeCopyToJewelbox.Enabled = menuContextSafeMoveToJewelbox.Enabled = false;
            menuContextSafeDelete.Enabled = false;

            if (!(mouseLocation == null || mouseLocation.RowIndex == -1))
            {
                // Mouse has to be over a selected row
                foreach (DataGridViewRow selectedRow in gridFamilySafe.SelectedRows)
                {
                    if (mouseLocation.RowIndex == selectedRow.Index)
                    {
                        menuContextSafeCopyToJewelbox.Enabled = menuContextSafeMoveToJewelbox.Enabled = true;
                        menuContextSafeDelete.Enabled = true;
                        break;
                    }
                }
            }
        }
        #endregion

        #region Jewelbox Context Menu
        private void OnContextJewelboxOpening(object sender, CancelEventArgs e)
        {
            if (gridJewelbox.Rows.Count < 1)
            {
                e.Cancel = true;
                return;
            }

            menuContextJewelboxCopyToSafe.Enabled = menuContextJewelboxMoveToSafe.Enabled = false;
            menuContextJewelboxDelete.Enabled = false;

            if (!(mouseLocation == null || mouseLocation.RowIndex == -1))
            {
                // Mouse has to be over a selected row
                foreach (DataGridViewRow selectedRow in gridJewelbox.SelectedRows)
                {
                    if (mouseLocation.RowIndex == selectedRow.Index)
                    {
                        menuContextJewelboxCopyToSafe.Enabled = menuContextJewelboxMoveToSafe.Enabled = true;
                        menuContextJewelboxDelete.Enabled = true;
                        break;
                    }
                }
            }
        }
        #endregion

        #region Closet/Suitcase Buttons/Context Menu Items
        private void OnCopyToClosetClicked(object sender, EventArgs e)
        {
            PasteIntoContainer(gridFamilyCloset, BuildTransferList(gridSuitcase));
            UpdateFormState();
        }

        private void OnMoveToClosetClicked(object sender, EventArgs e)
        {
            PasteIntoContainer(gridFamilyCloset, BuildTransferList(gridSuitcase));
            DeleteSelectedFromTransfer(gridSuitcase);
            UpdateFormState();
        }

        private void OnDeleteFromClosetClicked(object sender, EventArgs e)
        {
            DeleteSelectedFromContainer(gridFamilyCloset);
            UpdateFormState();
        }

        private void OnCopyToSuitcaseClicked(object sender, EventArgs e)
        {
            PasteIntoTransfer(gridSuitcase, BuildTransferList(gridFamilyCloset));
            UpdateFormState();
        }

        private void OnMoveToSuitcaseClicked(object sender, EventArgs e)
        {
            PasteIntoTransfer(gridSuitcase, BuildTransferList(gridFamilyCloset));
            DeleteSelectedFromContainer(gridFamilyCloset);
            UpdateFormState();
        }

        private void OnDeleteFromSuitcaseClicked(object sender, EventArgs e)
        {
            DeleteSelectedFromTransfer(gridSuitcase);
            UpdateFormState();
        }

        private void OnEmptySuitcaseClicked(object sender, EventArgs e)
        {
            dataSuitcase.Clear();
            UpdateFormState();
        }
        #endregion

        #region Safe/Jewelbox Buttons/Context Menu Items
        private void OnCopyToSafeClicked(object sender, EventArgs e)
        {
            PasteIntoContainer(gridFamilySafe, BuildTransferList(gridJewelbox));
            UpdateFormState();
        }

        private void OnMoveToSafeClicked(object sender, EventArgs e)
        {
            PasteIntoContainer(gridFamilySafe, BuildTransferList(gridJewelbox));
            DeleteSelectedFromTransfer(gridJewelbox);
            UpdateFormState();
        }

        private void OnDeleteFromSafeClicked(object sender, EventArgs e)
        {
            DeleteSelectedFromContainer(gridFamilySafe);
            UpdateFormState();
        }

        private void OnCopyToJewelboxClicked(object sender, EventArgs e)
        {
            PasteIntoTransfer(gridJewelbox, BuildTransferList(gridFamilySafe));
            UpdateFormState();
        }

        private void OnMoveToJewelboxClicked(object sender, EventArgs e)
        {
            PasteIntoTransfer(gridJewelbox, BuildTransferList(gridFamilySafe));
            DeleteSelectedFromContainer(gridFamilySafe);
            UpdateFormState();
        }

        private void OnDeleteFromJewelboxClicked(object sender, EventArgs e)
        {
            DeleteSelectedFromTransfer(gridJewelbox);
            UpdateFormState();
        }

        private void OnEmptyJewelboxClicked(object sender, EventArgs e)
        {
            dataJewelbox.Clear();
            UpdateFormState();
        }
        #endregion

        #region Actions on the "container" grid (closet and safe)
        private bool IsContainerGrid(DataGridView grid)
        {
            return (grid == gridFamilyCloset || grid == gridFamilySafe);
        }

        private void DeleteSelectedFromContainer(DataGridView container)
        {
            int selectedIndex = -1;
            string colPrefix = GetColPrefix(container);

            SortedList<int, int> selectedRowIndexes = new SortedList<int, int>();

            foreach (DataGridViewRow row in container.SelectedRows)
            {
                if (selectedIndex == -1)
                {
                    selectedIndex = row.Index + 1;
                }

                selectedRowIndexes.Add(row.Index, row.Index);

                OutfitDbpfData closetData = row.Cells[$"{colPrefix}Data"].Value as OutfitDbpfData;

                using (CacheableDbpfFile package = packageCache.OpenForUpdate(closetData.PackagePath))
                {
                    package.Remove(closetData.OutfitIdr);
                    package.Remove(new DBPFKey(Binx.TYPE, closetData.OutfitIdr));

                    package.Close();
                }
            }

            while (selectedRowIndexes.Keys.Contains(selectedIndex))
            {
                ++selectedIndex;
            }

            selectedIndex -= (selectedRowIndexes.IndexOfKey(selectedIndex - 1) + 1);

            DoWork_FillClosetOrSafeGrid(lastHoodNode, lastFamilyNode);

            container.ClearSelection();

            if (selectedIndex < 0)
            {
                selectedIndex = 0;
            }
            else if (selectedIndex >= container.Rows.Count)
            {
                selectedIndex = container.Rows.Count - 1;
            }

            // container.Rows[selectedIndex].Selected = true;
            if (selectedIndex >= 0 && selectedIndex < container.Rows.Count)
            {
                container.FirstDisplayedScrollingRowIndex = selectedIndex;
            }
        }

        private void PasteIntoContainer(DataGridView container, ClosetTransferData transferData)
        {
            if (transferData == null) return;

            foreach (ClosetData item in transferData.items)
            {
                PasteItemIntoContainer(container, item);
            }

            DoWork_FillClosetOrSafeGrid(lastHoodNode, lastFamilyNode);
        }

        private void PasteItemIntoContainer(DataGridView container, ClosetData item)
        {
            if (IsDuplicateEntry(container, GetColPrefix(container), item)) return;

            using (CacheableDbpfFile package = packageCache.OpenForUpdate(item.dbpfData.PackagePath))
            {
                // If originally from the current family's closet
                if (item.dbpfData.OutfitIdr.GetItem(1).InstanceID == lastFamilyNode.FamilyId)
                {
                    // Just put it back
                    package.Commit(item.dbpfData.OutfitIdr, true);
                    package.Commit(item.dbpfData.OutfitBinx, true);
                }
                else
                {
                    // Otherwise
                    DBPFKey newIdrKey = new DBPFKey(Idr.TYPE, DBPFData.GROUP_LOCAL, TypeInstanceID.RandomID, DBPFData.RESOURCE_NULL);

                    //   Find an unused instance for the new 3IDR
                    while (newIdrKey.InstanceID.AsUInt() <= 0x00007FFF || package.GetEntryByKey(newIdrKey) != null)
                    {
                        newIdrKey.ChangeIR(TypeInstanceID.RandomID, newIdrKey.ResourceID);
                    }

                    //   Clone the existing 3IDR and change its instance id
                    Idr newIdr = item.dbpfData.OutfitIdr.Duplicate(newIdrKey);

                    //   Change the clone's [1].InstanceId to the familyId
                    DBPFKey collKey = newIdr.GetItem(1);
                    collKey.ChangeIR(lastFamilyNode.FamilyId, collKey.ResourceID);

                    //   Clone the existing BINX for the new 3IDR
                    Binx newBinx = item.dbpfData.OutfitBinx.Duplicate(new DBPFKey(Binx.TYPE, newIdrKey));

                    //   Commit the clones
                    package.Commit(newIdr, true);
                    package.Commit(newBinx, true);
                }

                package.Close();
            }
        }
        #endregion

        #region Actions on the "transfer" grid (suitcase & jewelbox)
        private bool IsTransferGrid(DataGridView grid)
        {
            return (grid == gridSuitcase || grid == gridJewelbox);
        }

        private ClosetTransferData BuildTransferList(DataGridView grid, bool all = false)
        {
            string colPrefix = GetColPrefix(grid);

            ClosetTransferData transferData = new ClosetTransferData(null);

            if (all)
            {
                foreach (DataGridViewRow row in grid.Rows)
                {
                    if (IsCompleteOutfitRow(grid, row)) transferData.items.Add(new ClosetData(colPrefix, row));
                }
            }
            else
            {
                List<DataGridViewRow> rows = new List<DataGridViewRow>();

                foreach (DataGridViewRow row in grid.SelectedRows)
                {
                    rows.Add(row);
                }

                foreach (DataGridViewRow row in rows)
                {
                    if (IsCompleteOutfitRow(grid, row))
                    {
                        transferData.items.Add(new ClosetData(colPrefix, row));
                    }
                    else
                    {
                        row.Selected = false;
                    }
                }
            }

            return transferData;
        }

        private void DeleteSelectedFromTransfer(DataGridView transfer)
        {
            OutfitGridData data = GetDataForGrid(transfer);

            SortedSet<int> rowsToRemove = new SortedSet<int>();

            foreach (DataGridViewRow row in transfer.SelectedRows)
            {
                rowsToRemove.Add(row.Index);
            }

            foreach (int index in rowsToRemove.Reverse())
            {
                data.Rows.RemoveAt(index);
            }
        }

        private void PasteIntoTransfer(DataGridView transfer, ClosetTransferData transferData)
        {
            if (transferData == null) return;

            foreach (ClosetData item in transferData.items)
            {
                PasteItemIntoTransfer(transfer, item);
            }
        }

        private void PasteItemIntoTransfer(DataGridView transfer, ClosetData item)
        {
            if (IsDuplicateEntry(transfer, GetColPrefix(transfer), item)) return;

            OutfitGridData data = GetDataForGrid(transfer);

            DataRow transferRow = data.NewRow();

            transferRow["Visible"] = "Yes";
            transferRow["Data"] = item.dbpfData;

            transferRow["Name"] = item.name;
            transferRow["Category"] = item.category;
            transferRow["Gender"] = item.gender;
            transferRow["GenderCode"] = item.genderCode;
            transferRow["Age"] = item.age;
            transferRow["AgeCode"] = item.ageCode;

            transferRow["GenderHex"] = item.genderHex;
            transferRow["AgeHex"] = item.ageHex;

            transferRow["ThumbKey"] = item.thumbKey;
            transferRow["LocalThumbKey"] = item.localThumbKey;

            data.Rows.Add(transferRow);
        }

        private bool IsDuplicateEntry(DataGridView grid, string colNamePrefix, ClosetData item)
        {
            DBPFKey itemCpfKey = item.dbpfData.OutfitIdr.GetItem(2);

            foreach (DataGridViewRow row in grid.Rows)
            {
                if (itemCpfKey == (row.Cells[$"{colNamePrefix}Data"].Value as OutfitDbpfData).OutfitIdr.GetItem(2))
                {
                    return true;
                }
            }

            return false;
        }
        #endregion

        #region Save to / Load from XML file
        private void OnSaveSuitcaseClicked(object sender, EventArgs e)
        {
            if (saveSuitcaseFileDialog.ShowDialog() == DialogResult.OK)
            {
                XmlWriterSettings settings = new XmlWriterSettings
                {
                    Indent = true
                };

                XmlWriter writer = XmlWriter.Create(saveSuitcaseFileDialog.FileName, settings);

                BuildTransferList(gridSuitcase, true).WriteXml(writer, "suitcase");

                writer.Flush();
                writer.Close();
            }
        }

        private void OnLoadSuitcaseClicked(object sender, EventArgs e)
        {
            if (openSuitcaseFileDialog.ShowDialog() == DialogResult.OK)
            {
                dataSuitcase.Clear();

                ClosetTransferData transferData = null;

                XmlReaderSettings settings = new XmlReaderSettings
                {
                    IgnoreComments = true,
                    IgnoreProcessingInstructions = true,
                    IgnoreWhitespace = true
                };

                XmlReader reader = XmlReader.Create(openSuitcaseFileDialog.FileName, settings);
                reader.MoveToContent();

                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name.Equals("suitcase"))
                    {
                        transferData = new ClosetTransferData(null);

                        transferData.ReadXml(reader);
                    }
                }

                reader.Close();

                PasteIntoTransfer(gridSuitcase, transferData);
                UpdateFormState();
            }
        }

        private void OnSaveJewelboxClicked(object sender, EventArgs e)
        {
            if (saveJewelboxFileDialog.ShowDialog() == DialogResult.OK)
            {
                XmlWriterSettings settings = new XmlWriterSettings
                {
                    Indent = true
                };

                XmlWriter writer = XmlWriter.Create(saveJewelboxFileDialog.FileName, settings);

                BuildTransferList(gridJewelbox, true).WriteXml(writer, "jewelbox");

                writer.Flush();
                writer.Close();
            }
        }

        private void OnLoadJewelboxClicked(object sender, EventArgs e)
        {
            if (openJewelboxFileDialog.ShowDialog() == DialogResult.OK)
            {
                dataJewelbox.Clear();

                ClosetTransferData transferData = null;

                XmlReaderSettings settings = new XmlReaderSettings
                {
                    IgnoreComments = true,
                    IgnoreProcessingInstructions = true,
                    IgnoreWhitespace = true
                };

                XmlReader reader = XmlReader.Create(openJewelboxFileDialog.FileName, settings);
                reader.MoveToContent();

                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name.Equals("jewelbox"))
                    {
                        transferData = new ClosetTransferData(null);

                        transferData.ReadXml(reader);
                    }
                }

                reader.Close();

                PasteIntoTransfer(gridJewelbox, transferData);
                UpdateFormState();
            }
        }
        #endregion

        #region Tooltips and Thumbnails
        private bool IsMemberGrid(DataGridView grid)
        {
            return (grid == gridFamilyMembers);
        }

        private void OnToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridView grid = sender as DataGridView;
                int index = e.RowIndex;

                if (index < grid.Rows.Count)
                {
                    DataGridViewRow row = (grid).Rows[index];

                    if (IsContainerGrid(grid))
                    {
                        if (row.Cells[e.ColumnIndex].OwningColumn.Name.EndsWith("Name"))
                        {
                            e.ToolTipText = GetTooltip(row, GetColPrefix(grid));
                        }
                    }
                    else if (IsMemberGrid(grid))
                    {
                        if (row.Cells[e.ColumnIndex].OwningColumn.Name.Equals("colFirstName"))
                        {
                            if (row.Cells["colData"].Value is CharacterData data)
                            {
                                e.ToolTipText = data.PackageName;
                            }
                        }
                    }
                }
            }
        }

        private string GetTooltip(DataGridViewRow row, string colNamePrefix)
        {
            CasOutfitData casData = null;

            if (row.Cells[$"{colNamePrefix}Data"].Value is OutfitDbpfData data)
            {
                DBPFKey cpfKey = data.CpfKey;

                if (clothingCache.ContainsKey(cpfKey))
                {
                    casData = clothingCache.GetData(cpfKey);
                }
                else if (jewelleryCache.ContainsKey(cpfKey))
                {
                    casData = jewelleryCache.GetData(cpfKey);
                }
            }

            return casData?.ResPackagePath;
        }

        private Image GetThumbnail(DataGridViewRow row, string colNamePrefix)
        {
            if (row.Cells[$"{colNamePrefix}LocalThumbKey"]?.Value is DBPFKey)
            {
                OutfitDbpfData data = row.Cells[$"{colNamePrefix}Data"]?.Value as OutfitDbpfData;

                return jewelleryCache.GetData(data.CpfKey).GetLocalThumbnail();
            }
            else
            {
                DBPFKey thumbKey = row.Cells[$"{colNamePrefix}ThumbKey"]?.Value as DBPFKey;
                DBPFKey cpfKey = (row.Cells[$"{colNamePrefix}Data"].Value as OutfitDbpfData)?.CpfKey;

                return clothingThumbnailsCache.GetThumbnail(thumbKey, cpfKey);
            }
        }

        #endregion

        #region Hood Tree Management
        private void OnTreeHoods_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            if (e.Node == null) return;

            // if treeview's HideSelection property is "True", 
            // this will always returns "False" on unfocused treeview
            bool selected = (e.State & TreeNodeStates.Selected) == TreeNodeStates.Selected;
            bool unfocused = !e.Node.TreeView.Focused;

            // we need to do owner drawing only on a selected node
            // and when the treeview is unfocused, else let the OS do it for us
            if (selected && unfocused)
            {
                Font font = e.Node.NodeFont ?? e.Node.TreeView.Font;
                e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
                TextRenderer.DrawText(e.Graphics, e.Node.Text, font, e.Bounds, SystemColors.HighlightText, TextFormatFlags.GlyphOverhangPadding);
            }
            else
            {
                e.DrawDefault = true;
            }
        }

        private void OnTreeHoodsBeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Nodes.Count == 1 && e.Node.Nodes[0].Text.Equals("Placeholder"))
            {
                e.Node.Nodes.Clear();

                ProgressDialog progressDialog = new ProgressDialog(e.Node);
                progressDialog.DoWork += new ProgressDialog.DoWorkEventHandler(DoAsyncWork_ProcessFamilies);
                progressDialog.DoData += new ProgressDialog.DoWorkEventHandler(DoAsyncData_ProcessFamilies);

                DialogResult result = progressDialog.ShowDialog();

                if (result == DialogResult.Abort)
                {
                    logger.Error(progressDialog.Result.Error.Message);
                    logger.Info(progressDialog.Result.Error.StackTrace);

                    MsgBox.Show($"An error occured while processing\n{lastPackageFile}", "Error!", MessageBoxButtons.OK);
                }
                else
                {
                    if (result == DialogResult.Cancel)
                    {
                        e.Cancel = true;
                    }
                    else
                    {
                    }
                }
            }
        }

        private void OnTreeHoodsClicked(object sender, TreeNodeMouseClickEventArgs e)
        {
            treeHoods.SelectedNode = e.Node;
            DoWork_FillHoodOrFamilyGrid(e.Node);
        }

        private bool OnTreeHoods_ExpandNode(TreeNodeCollection nodes, string key)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Name.Equals(key))
                {
                    node.Expand();
                    treeHoods.SelectedNode = node;
                    OnTreeHoodsClicked(treeHoods, new TreeNodeMouseClickEventArgs(node, MouseButtons.Left, 1, 0, 0));
                    return true;
                }

                if (OnTreeHoods_ExpandNode(node.Nodes, key)) return true;
            }

            return false;
        }
        #endregion

        #region Grid Management
        private void OnDataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if ((sender as DataGridView).SortedColumn != null)
            {
                UpdateFormState();
            }
        }

        private void OnMemberGridSelectionChanged(object sender, EventArgs e)
        {
            if (IsCareerTabActive)
            {
                DoWork_FillCareerTab();
            }
            else if (IsSkillsTabActive)
            {
                DoWork_FillSkillsTab();
            }
            else if (IsInterestsTabActive)
            {
                DoWork_FillInterestsTab();
            }

            UpdateFormState();
        }

        private void OnOutfitGridSelectionChanged(object sender, EventArgs e)
        {
            UpdateFormState();
        }

        private bool IsCompleteOutfitRow(DataGridView grid, DataGridViewRow row)
        {
            return (row.Cells[$"{GetColPrefix(grid)}Name"].Value is string name &&
                    !(name.StartsWith("GZPS-") || name.StartsWith("XMOL-")));
        }
        #endregion

        #region Grid Row Fill
        private string BuildCategoryString(uint categoryCode)
        {
            string category = "";

            if ((categoryCode & 0x1B7F) == 0x1B7F) return "All (inc Naked)";
            if ((categoryCode & 0x137F) == 0x137F) return "All";

            if ((categoryCode & 0x0007) == 0x0007) category += ", Everyday";
            if ((categoryCode & 0x0008) == 0x0008) category += ", Swim";
            if ((categoryCode & 0x0010) == 0x0010) category += ", PJs";
            if ((categoryCode & 0x0020) == 0x0020) category += ", Formal";
            if ((categoryCode & 0x0040) == 0x0040) category += ", Undies";
            if ((categoryCode & 0x0100) == 0x0100) category += ", Maternity";
            if ((categoryCode & 0x0200) == 0x0200) category += ", Gym";
            if ((categoryCode & 0x1000) == 0x1000) category += ", Outer";

            if ((categoryCode & 0x0800) == 0x0800) category += ", Naked";

            return (category.Length > 2) ? category.Substring(2) : category;
        }

        private string BuildGenderString(uint genderCode)
        {
            if (genderCode == 1)
            {
                return "Female";
            }
            else if (genderCode == 2)
            {
                return "Male";
            }

            return "Unisex";
        }

        private string BuildGenderCodeString(uint genderCode)
        {
            return BuildGenderString(genderCode).Substring(0, 1);
        }

        private string BuildAgeString(uint ageCode)
        {
            string age = "";

            if ((ageCode & 0x20) == 0x20) age += ", Baby";
            if ((ageCode & 0x01) == 0x01) age += ", Toddler";
            if ((ageCode & 0x02) == 0x02) age += ", Child";
            if ((ageCode & 0x04) == 0x04) age += ", Teen";
            if ((ageCode & 0x40) == 0x40) age += ", Young Adult";
            if ((ageCode & 0x08) == 0x08) age += ", Adult";
            if ((ageCode & 0x10) == 0x10) age += ", Elder";

            return (age.Length > 2) ? age.Substring(2) : age;
        }

        private string BuildAgeCodeString(uint ageCode)
        {
            string age = "";

            if ((ageCode & 0x20) == 0x20) age += ",B";
            if ((ageCode & 0x01) == 0x01) age += ",P";
            if ((ageCode & 0x02) == 0x02) age += ",C";
            if ((ageCode & 0x04) == 0x04) age += ",T";
            if ((ageCode & 0x40) == 0x40) age += ",YA";
            if ((ageCode & 0x08) == 0x08) age += ",A";
            if ((ageCode & 0x10) == 0x10) age += ",E";

            return (age.Length > 1) ? age.Substring(1) : age;
        }
        #endregion

        #region Mouse Management
        private DataGridViewCellEventArgs mouseLocation = null;

        private void OnCellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            mouseLocation = e;
            Point MousePosition = Cursor.Position;

            DataGridView grid = sender as DataGridView;
            Image thumbnail = null;

            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.RowIndex < grid.RowCount && e.ColumnIndex < grid.ColumnCount)
            {
                DataGridViewRow row = grid.Rows[e.RowIndex];
                string colName = row.Cells[e.ColumnIndex].OwningColumn.Name;

                if (grid == gridFamilyMembers)
                {
                    if (colName.Equals("colFirstName"))
                    {
                        thumbnail = row.Cells["colThumbnail"].Value as Image;
                    }
                }
                else
                {
                    if (colName.EndsWith("Name"))
                    {
                        thumbnail = GetThumbnail(row, GetColPrefix(sender as DataGridView));
                    }
                }
            }

            if (thumbnail != null)
            {
                thumbBox.Image = thumbnail;

                int fudge = 20; // A fudge factor so the thumbnail doesn't sit on the bottom of the app's window
                int thumbY = (MousePosition.Y - this.Location.Y);
                if ((thumbY + thumbBox.Size.Height + fudge) > (this.Size.Height - splitTopBottom.Location.Y)) thumbY = this.Size.Height - splitTopBottom.Location.Y - thumbBox.Size.Height - fudge;
                thumbBox.Location = new System.Drawing.Point(MousePosition.X - this.Location.X, thumbY);

                thumbBox.Visible = true;
            }
        }

        private void OnCellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            thumbBox.Visible = false;
        }
        #endregion

        #region Drag And Drop
        private void OnGridDragEnter(object sender, DragEventArgs e)
        {
            object data = e.Data.GetData(typeof(ClosetTransferData));

            if (data is ClosetTransferData closetData)
            {
                e.Effect = (closetData.Grid != sender) ? e.AllowedEffect : DragDropEffects.None;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void OnGridDragOver(object sender, DragEventArgs e)
        {
        }

        private void OnGridDragDrop(object sender, DragEventArgs e)
        {
            DataGridView grid = sender as DataGridView;

            bool reloadFamilyClosetOrSafe = false;

            ClosetTransferData draggedTransferData = (ClosetTransferData)e.Data.GetData(typeof(ClosetTransferData));

            if (draggedTransferData != null && draggedTransferData.Grid != grid)
            {
                foreach (ClosetData item in draggedTransferData.items)
                {
                    if (IsTransferGrid(grid))
                    {
                        PasteItemIntoTransfer(grid, item);
                    }
                    else if (IsContainerGrid(grid))
                    {
                        PasteItemIntoContainer(grid, item);

                        reloadFamilyClosetOrSafe = true;
                    }

                    if (e.Effect == DragDropEffects.Move)
                    {
                        if (IsContainerGrid(draggedTransferData.Grid))
                        {
                            using (CacheableDbpfFile package = packageCache.OpenForUpdate(item.dbpfData.PackagePath))
                            {
                                package.Remove(item.dbpfData.OutfitIdr);
                                package.Remove(new DBPFKey(Binx.TYPE, item.dbpfData.OutfitIdr));

                                package.Close();
                            }

                            reloadFamilyClosetOrSafe = true;
                        }
                        else if (IsTransferGrid(draggedTransferData.Grid))
                        {
                            string colPrefix = GetColPrefix(draggedTransferData.Grid);

                            foreach (DataGridViewRow row in draggedTransferData.Grid.Rows)
                            {
                                if ((row.Cells[$"{colPrefix}Data"].Value as OutfitDbpfData).OutfitIdr == item.dbpfData.OutfitIdr)
                                {
                                    draggedTransferData.Grid.Rows.Remove(row);
                                    break;
                                }
                            }
                        }
                    }
                }

                if (reloadFamilyClosetOrSafe) DoWork_FillClosetOrSafeGrid(lastHoodNode, lastFamilyNode);
            }
        }

        private void OnGridMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && mouseLocation != null && mouseLocation.RowIndex != -1)
            {
                DataGridView grid = sender as DataGridView;
                string colNamePrefix = GetColPrefix(sender as DataGridView);

                if (grid.CurrentRow != null)
                {
                    ClosetTransferData transferData = new ClosetTransferData(sender as DataGridView);

                    if (grid.CurrentRow.Selected)
                    {
                        foreach (DataGridViewRow selectedRow in grid.SelectedRows)
                        {
                            if (IsCompleteOutfitRow(grid, selectedRow)) transferData.items.Add(new ClosetData(colNamePrefix, selectedRow));
                        }
                    }
                    else
                    {
                        if (IsCompleteOutfitRow(grid, grid.CurrentRow)) transferData.items.Add(new ClosetData(colNamePrefix, grid.CurrentRow));
                    }

                    if (transferData.items.Count > 0)
                    {
                        thumbBox.Visible = false;
                        grid.DoDragDrop(transferData, (Form.ModifierKeys == Keys.Control) ? DragDropEffects.Copy : DragDropEffects.Move);
                    }
                }
            }
        }

        private string GetColPrefix(DataGridView grid)
        {
            if (grid == gridFamilyCloset) return "colCloset";
            if (grid == gridSuitcase) return "colSuitcase";
            if (grid == gridFamilySafe) return "colSafe";
            if (grid == gridJewelbox) return "colJewelbox";

            throw new NotImplementedException();
        }

        private OutfitGridData GetDataForGrid(DataGridView grid)
        {
            if (grid == gridFamilyCloset) return dataFamilyCloset;
            if (grid == gridSuitcase) return dataSuitcase;
            if (grid == gridFamilySafe) return dataFamilySafe;
            if (grid == gridJewelbox) return dataJewelbox;

            throw new NotImplementedException();
        }
        #endregion

        #region Filters
        private void OnShowAllClicked(object sender, EventArgs e)
        {
            filters.ShowAll();

            FilterActiveContainer();
        }

        private void OnShowSelectedSimsClicked(object sender, EventArgs e)
        {
            filters.Clear();

            foreach (DataGridViewRow row in gridFamilyMembers.SelectedRows)
            {
                filters.IncludeMember(row);
            }

            FilterActiveContainer();
        }

        private void OnShowThisSimClicked(object sender, EventArgs e)
        {
            if (!(mouseLocation == null || mouseLocation.RowIndex == -1))
            {
                filters.Clear();
                filters.IncludeMember(gridFamilyMembers.Rows[mouseLocation.RowIndex]);

                FilterActiveContainer();
            }
        }

        private void OnShowUnwearableClicked(object sender, EventArgs e)
        {
            filters.Clear();

            foreach (DataGridViewRow row in gridFamilyMembers.Rows)
            {
                filters.IncludeMember(row);
            }

            filters.SetInverted();

            FilterActiveContainer();
        }

        private void FilterActiveContainer()
        {
            OutfitGridData containerData = (IsSafeTabActive) ? dataFamilySafe : dataFamilyCloset;

            foreach (DataRow row in containerData.Rows)
            {
                row["Visible"] = filters.Visible(row);
            }
        }
        #endregion

        #region Save Button
        private void OnSaveClicked(object sender, EventArgs e)
        {
            Save();

            UpdateFormState();
        }

        private void Save()
        {
            UpdateCurrentFamily();

            foreach (CacheableDbpfFile dbpfPackage in packageCache)
            {
                try
                {
                    dbpfPackage.Update(menuItemAutoBackup.Checked);
                }
                catch (Exception)
                {
                    MsgBox.Show($"Error trying to update {dbpfPackage.PackageName}", "Package Update Error!");
                }

                dbpfPackage.Close();
            }

            packageCache.Clear();
        }
        #endregion

        private void OnSimTrackingBarChanged(object sender, EventArgs e)
        {
            SimTrackingBar trackBar = sender as SimTrackingBar;

            toolTip.SetToolTip(trackBar, $"{trackBar.Tag}: {trackBar.Value} out of {trackBar.Maximum}");
        }

        private void OnInterestChanged(object sender, EventArgs e)
        {
            SimTrackingBar trackBar = sender as SimTrackingBar;
            InterestTracker tracker = trackBar.Parent as InterestTracker;

            toolTip.SetToolTip(trackBar, $"{trackBar.Tag}: {trackBar.Value} out of {trackBar.Maximum}");

            if (ignoreInterestsChanges) return;

            currentMemberData.SetInterestValue(tracker.SdscIndex, tracker.Value);
            UpdateSaveState();
        }

        private void OnHobbyChanged(object sender, EventArgs e)
        {
            SimTrackingBar trackBar = sender as SimTrackingBar;
            InterestTracker tracker = trackBar.Parent as InterestTracker;

            toolTip.SetToolTip(trackBar, $"{trackBar.Tag}: {trackBar.Value} out of {trackBar.Maximum}");

            if (ignoreInterestsChanges) return;

            currentMemberData.SetHobbyValue(tracker.SdscIndex, tracker.Value);
            UpdateSaveState();
        }

        private void OnOneTrueHobbyChanged(object sender, EventArgs e)
        {
            if (ignoreInterestsChanges) return;

            currentMemberData.OneTrueHobby = (ushort)((comboHobbyOneTrue.SelectedItem as UintNamedValue).Value);
            UpdateSaveState();
        }

        private void OnBadgeChanged(object sender, EventArgs e)
        {
            SimTrackingBar trackBar = sender as SimTrackingBar;
            InterestTracker tracker = trackBar.Parent as InterestTracker;

            toolTip.SetToolTip(trackBar, $"{tracker.Tag}: {trackBar.Value} out of {trackBar.Maximum}");

            if (ignoreInterestsChanges) return;

            currentMemberData.SetBadgeValue((TypeGUID)tracker.TokenGuid, tracker.Value);
            UpdateSaveState();
        }

        private void OnGeneralSkillChanged(object sender, EventArgs e)
        {
            SimTrackingBar trackBar = sender as SimTrackingBar;
            SkillTracker tracker = trackBar.Parent as SkillTracker;

            toolTip.SetToolTip(trackBar, $"{tracker.Tag}: {trackBar.Value} out of {trackBar.Maximum}");

            if (ignoreSkillsChanges) return;

            currentMemberData.SetSkillValue(tracker.SdscIndex, tracker.Value);
            UpdateSaveState();
        }

        private void OnToddlerSkillChanged(object sender, EventArgs e)
        {
            SimTrackingBar trackBar = sender as SimTrackingBar;
            SkillTracker tracker = trackBar.Parent as SkillTracker;

            toolTip.SetToolTip(trackBar, $"{tracker.Tag}: {trackBar.Value} out of {trackBar.Maximum}");

            if (ignoreSkillsChanges) return;

            currentMemberData.SetToddlerSkillValue((TypeGUID)tracker.TokenGuid, (int)tracker.TokenProp, tracker.Value, (tracker.Value == tracker.Maximum));
            UpdateSaveState();
        }

        private void OnHiddenSkillChanged(object sender, EventArgs e)
        {
            SimTrackingBar trackBar = sender as SimTrackingBar;
            SkillTracker tracker = trackBar.Parent as SkillTracker;

            toolTip.SetToolTip(trackBar, $"{tracker.Tag}: {trackBar.Value} out of {trackBar.Maximum}");

            if (ignoreSkillsChanges) return;

            currentMemberData.SetHiddenSkillValue((TypeGUID)tracker.TokenGuid, (int)tracker.TokenProp, tracker.Value);
            UpdateSaveState();
        }

        private void OnLifeSkillChanged(object sender, EventArgs e)
        {
            SimTrackingBar trackBar = sender as SimTrackingBar;
            SkillTracker tracker = trackBar.Parent as SkillTracker;

            toolTip.SetToolTip(trackBar, $"{tracker.Tag}: {trackBar.Value} out of {trackBar.Maximum}");

            if (ignoreSkillsChanges) return;

            currentMemberData.SetLifeSkillValue((TypeGUID)tracker.TokenGuid, tracker.Value);
            UpdateSaveState();
        }

        private void OnPetSkillChanged(object sender, EventArgs e)
        {
            SimTrackingBar trackBar = sender as SimTrackingBar;
            SkillTracker tracker = trackBar.Parent as SkillTracker;

            toolTip.SetToolTip(trackBar, $"{tracker.Tag}: {trackBar.Value} out of {trackBar.Maximum}");

            if (ignoreSkillsChanges) return;

            currentMemberData.SetPetSkillValue((TypeGUID)tracker.TokenGuid, tracker.Value);
            UpdateSaveState();
        }
    }
}
