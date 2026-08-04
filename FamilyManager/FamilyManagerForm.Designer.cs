/*
 * Family Manager - a utility for manipulating family closets
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Microsoft.WindowsAPICodePack.Dialogs;

namespace FamilyManager
{
    partial class FamilyManagerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FamilyManagerForm));
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSaveAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemConfiguration = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMode = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAdvanced = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemAutoBackup = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemUseCodes = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparatorSplitFiles = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemShowSplitFiles = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemHighlightSplitFiles = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemIncludeNPCs = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemOnlyNPCs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemTeensHaveAdultJobs = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemYAsHaveAdultJobs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemIntDisplay = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemIntDisplayBarAndBox = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemIntDisplayBarOnly = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemIntDisplayBoxOnly = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLanguage = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCaching = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemCachingUpdateCustomCareers = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemCachingUpdateMaxisClothes = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemCachingUpdateCustomClothes = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemCachingUpdateMaxisJewellery = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemCachingUpdateCustomJewellery = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparatorCaching = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemCachingRemoveLocal = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemCachingRemoveThumbnails = new System.Windows.Forms.ToolStripMenuItem();
            this.splitTopBottom = new System.Windows.Forms.SplitContainer();
            this.splitTopLeftRight = new System.Windows.Forms.SplitContainer();
            this.treeHoods = new System.Windows.Forms.TreeView();
            this.lblLotName = new System.Windows.Forms.Label();
            this.lblFamilyName = new System.Windows.Forms.Label();
            this.imageFamily = new System.Windows.Forms.PictureBox();
            this.gridFamilyMembers = new System.Windows.Forms.DataGridView();
            this.colFirstName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSplitFile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGender = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGenderCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAge = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAgeCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDaysLeft = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGenderHex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAgeHex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colThumbnail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuContextMembers = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuContextMemberChangeSimName = new System.Windows.Forms.ToolStripMenuItem();
            this.menuContextMemberChangeFamilyName = new System.Windows.Forms.ToolStripMenuItem();
            this.menuContextMemberChangeDays = new System.Windows.Forms.ToolStripMenuItem();
            this.menuContextMemberFilterAll = new System.Windows.Forms.ToolStripMenuItem();
            this.menuContextMemberFilterSelected = new System.Windows.Forms.ToolStripMenuItem();
            this.menuContextMemberFilterThis = new System.Windows.Forms.ToolStripMenuItem();
            this.menuContextMemberSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuContextMemberMergeSplitFiles = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPages = new System.Windows.Forms.TabControl();
            this.tabFamily = new System.Windows.Forms.TabPage();
            this.panelFamily = new System.Windows.Forms.Panel();
            this.ckbFamilyNameSelected = new System.Windows.Forms.CheckBox();
            this.ckbFamilyNameSame = new System.Windows.Forms.CheckBox();
            this.ckbFamilyNameAll = new System.Windows.Forms.CheckBox();
            this.textAddressDesc = new System.Windows.Forms.TextBox();
            this.ckbMoneyLock = new System.Windows.Forms.CheckBox();
            this.textBusinessMoney = new System.Windows.Forms.TextBox();
            this.lblBusinessMoney = new System.Windows.Forms.Label();
            this.imageHouse = new System.Windows.Forms.PictureBox();
            this.textFamilyName = new System.Windows.Forms.TextBox();
            this.lblFamName = new System.Windows.Forms.Label();
            this.lblAddress = new System.Windows.Forms.Label();
            this.textFamilyWriteUp = new System.Windows.Forms.TextBox();
            this.textAddressName = new System.Windows.Forms.TextBox();
            this.lblWriteUp = new System.Windows.Forms.Label();
            this.textFamilyMoney = new System.Windows.Forms.TextBox();
            this.lblMoney = new System.Windows.Forms.Label();
            this.tabCloset = new System.Windows.Forms.TabPage();
            this.splitClosetLeftRight = new System.Windows.Forms.SplitContainer();
            this.gridSuitcase = new System.Windows.Forms.DataGridView();
            this.colSuitcaseVisible = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSuitcaseName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSuitcaseCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSuitcaseGender = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSuitcaseGenderCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSuitcaseAge = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSuitcaseAgeCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSuitcaseData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSuitcaseGenderHex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSuitcaseAgeHex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSuitcaseThumbKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSuitcaseLocalThumbKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuContextSuitcase = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuContextSuitcaseCopyToCloset = new System.Windows.Forms.ToolStripMenuItem();
            this.menuContextSuitcaseMoveToCloset = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.menuContextSuitcaseDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSuitcaseEmpty = new System.Windows.Forms.Button();
            this.btnSuitcaseSave = new System.Windows.Forms.Button();
            this.btnSuitcaseLoad = new System.Windows.Forms.Button();
            this.btnSuitcaseCopy = new System.Windows.Forms.Button();
            this.btnSuitcaseMove = new System.Windows.Forms.Button();
            this.lblClosetCachesNeeded = new System.Windows.Forms.Label();
            this.gridFamilyCloset = new System.Windows.Forms.DataGridView();
            this.colClosetVisible = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClosetName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClosetCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClosetGender = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClosetGenderCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClosetAge = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClosetAgeCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClosetData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClosetGenderHex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClosetAgeHex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClosetThumbKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClosetLocalThumbKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuContextCloset = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuContextClosetCopyToSuitcase = new System.Windows.Forms.ToolStripMenuItem();
            this.menuContextClosetMoveToSuitcase = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.menuContextClosetFilterAll = new System.Windows.Forms.ToolStripMenuItem();
            this.menuContextClosetFilterSelected = new System.Windows.Forms.ToolStripMenuItem();
            this.menuContextClosetFilterUnwearable = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuContextClosetDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.btnClosetCopy = new System.Windows.Forms.Button();
            this.btnClosetMove = new System.Windows.Forms.Button();
            this.btnClosetDelete = new System.Windows.Forms.Button();
            this.btnClosetShowAll = new System.Windows.Forms.Button();
            this.tabSafe = new System.Windows.Forms.TabPage();
            this.splitSafeLeftRight = new System.Windows.Forms.SplitContainer();
            this.gridJewelbox = new System.Windows.Forms.DataGridView();
            this.colJewelboxVisible = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colJewelboxName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colJewelboxCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colJewelboxGender = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colJewelboxGenderCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colJewelboxAge = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colJewelboxAgeCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colJewelboxData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colJewelboxGenderHex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colJewelboxAgeHex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colJewelboxThumbKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colJewelboxLocalThumbKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuContextJewelbox = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuContextJewelboxCopyToSafe = new System.Windows.Forms.ToolStripMenuItem();
            this.menuContextJewelboxMoveToSafe = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.menuContextJewelboxDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.btnJewelboxEmpty = new System.Windows.Forms.Button();
            this.btnJewelboxSave = new System.Windows.Forms.Button();
            this.btnJewelboxLoad = new System.Windows.Forms.Button();
            this.btnJewelboxCopy = new System.Windows.Forms.Button();
            this.btnJewelboxMove = new System.Windows.Forms.Button();
            this.lblSafeCachesNeeded = new System.Windows.Forms.Label();
            this.gridFamilySafe = new System.Windows.Forms.DataGridView();
            this.colSafeVisible = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSafeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSafeCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSafeGender = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSafeGenderCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSafeAge = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSafeAgeCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSafeData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSafeGenderHex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSafeAgeHex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSafeThumbKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSafeLocalThumbKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuContextSafe = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuContextSafeCopyToJewelbox = new System.Windows.Forms.ToolStripMenuItem();
            this.menuContextSafeMoveToJewelbox = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.menuContextSafeFilterAll = new System.Windows.Forms.ToolStripMenuItem();
            this.menuContextSafeFilterSelected = new System.Windows.Forms.ToolStripMenuItem();
            this.menuContextSafeFilterUnwearable = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.menuContextSafeDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSafeCopy = new System.Windows.Forms.Button();
            this.btnSafeMove = new System.Windows.Forms.Button();
            this.btnSafeDelete = new System.Windows.Forms.Button();
            this.btnSafeShowAll = new System.Windows.Forms.Button();
            this.tabCareer = new System.Windows.Forms.TabPage();
            this.imageSim = new System.Windows.Forms.PictureBox();
            this.grpJob = new System.Windows.Forms.GroupBox();
            this.lblJobPTOSummary = new System.Windows.Forms.Label();
            this.textJobRetiredGUID = new Sims2Tools.Controls.GuidTextBox();
            this.textJobRetiredLevel = new Sims2Tools.Controls.UIntTextBox();
            this.trackJobRetiredLevel = new Sims2Tools.Controls.SimTrackingBar();
            this.lblJobRetiredLevel = new System.Windows.Forms.Label();
            this.lblJobRetiredType = new System.Windows.Forms.Label();
            this.comboJobRetiredType = new System.Windows.Forms.ComboBox();
            this.textJobGUID = new Sims2Tools.Controls.GuidTextBox();
            this.textJobLevel = new Sims2Tools.Controls.UIntTextBox();
            this.textJobPerformance = new Sims2Tools.Controls.IntTextBox();
            this.textJobPTO = new Sims2Tools.Controls.UIntTextBox();
            this.textJobPension = new Sims2Tools.Controls.UIntTextBox();
            this.lblJobPension = new System.Windows.Forms.Label();
            this.trackJobPerformance = new Sims2Tools.Controls.SimTrackingBar();
            this.trackJobLevel = new Sims2Tools.Controls.SimTrackingBar();
            this.lblJobPTO = new System.Windows.Forms.Label();
            this.lblJobPerformance = new System.Windows.Forms.Label();
            this.lblJobLevel = new System.Windows.Forms.Label();
            this.lblJobType = new System.Windows.Forms.Label();
            this.comboJobType = new System.Windows.Forms.ComboBox();
            this.grpUniversity = new System.Windows.Forms.GroupBox();
            this.textUniGrade = new Sims2Tools.Controls.DoubleTextBox();
            this.textUniTimeLeft = new Sims2Tools.Controls.UIntTextBox();
            this.textUniInfluence = new Sims2Tools.Controls.UIntTextBox();
            this.textUniEffort = new Sims2Tools.Controls.UIntTextBox();
            this.textMajorGUID = new Sims2Tools.Controls.GuidTextBox();
            this.lblUniStudying = new System.Windows.Forms.Label();
            this.comboUniResult = new System.Windows.Forms.ComboBox();
            this.lblUniResult = new System.Windows.Forms.Label();
            this.ckbUniStudying = new System.Windows.Forms.CheckBox();
            this.trackUniTimeLeft = new Sims2Tools.Controls.SimTrackingBar();
            this.trackUniEffort = new Sims2Tools.Controls.SimTrackingBar();
            this.lblUniProbation = new System.Windows.Forms.Label();
            this.ckbUniProbation = new System.Windows.Forms.CheckBox();
            this.trackUniGrade = new Sims2Tools.Controls.SimTrackingBar();
            this.lblUniInfluence = new System.Windows.Forms.Label();
            this.comboUniSemester = new System.Windows.Forms.ComboBox();
            this.lblUniTimeLeft = new System.Windows.Forms.Label();
            this.lblUniGrade = new System.Windows.Forms.Label();
            this.lblUniSemester = new System.Windows.Forms.Label();
            this.comboUniMajor = new System.Windows.Forms.ComboBox();
            this.lblUniEffort = new System.Windows.Forms.Label();
            this.lblUniMajor = new System.Windows.Forms.Label();
            this.grpSchool = new System.Windows.Forms.GroupBox();
            this.textSchoolGUID = new Sims2Tools.Controls.GuidTextBox();
            this.comboSchoolGrade = new System.Windows.Forms.ComboBox();
            this.lblSchoolGrade = new System.Windows.Forms.Label();
            this.lblSchoolType = new System.Windows.Forms.Label();
            this.comboSchoolType = new System.Windows.Forms.ComboBox();
            this.tabSkills = new System.Windows.Forms.TabPage();
            this.grpSkillsPet = new System.Windows.Forms.GroupBox();
            this.trackSkillPetUseToilet = new Sims2Tools.Controls.SkillTracker();
            this.trackSkillPetStay = new Sims2Tools.Controls.SkillTracker();
            this.trackSkillPetSpeak = new Sims2Tools.Controls.SkillTracker();
            this.trackSkillPetSitUp = new Sims2Tools.Controls.SkillTracker();
            this.trackSkillPetShake = new Sims2Tools.Controls.SkillTracker();
            this.trackSkillPetRollOver = new Sims2Tools.Controls.SkillTracker();
            this.trackSkillPetPlayDead = new Sims2Tools.Controls.SkillTracker();
            this.trackSkillPetComeHere = new Sims2Tools.Controls.SkillTracker();
            this.lblSkillPetUseToilet = new System.Windows.Forms.Label();
            this.lblSkillPetStay = new System.Windows.Forms.Label();
            this.lblSkillPetSpeak = new System.Windows.Forms.Label();
            this.lblSkillPetSitUp = new System.Windows.Forms.Label();
            this.lblSkillPetShake = new System.Windows.Forms.Label();
            this.lblSkillPetRollOver = new System.Windows.Forms.Label();
            this.lblSkillPetPlayDead = new System.Windows.Forms.Label();
            this.lblSkillPetComeHere = new System.Windows.Forms.Label();
            this.grpSkillsLife = new System.Windows.Forms.GroupBox();
            this.trackSkillLifePhysiology = new Sims2Tools.Controls.SkillTracker();
            this.trackSkillLifeParenting = new Sims2Tools.Controls.SkillTracker();
            this.trackSkillLifeHappiness = new Sims2Tools.Controls.SkillTracker();
            this.trackSkillLifeFireSafety = new Sims2Tools.Controls.SkillTracker();
            this.trackSkillLifeCounselling = new Sims2Tools.Controls.SkillTracker();
            this.trackSkillLifeAngerMgmt = new Sims2Tools.Controls.SkillTracker();
            this.lblSkillLifePhysiology = new System.Windows.Forms.Label();
            this.lblSkillLifeParenting = new System.Windows.Forms.Label();
            this.lblSkillLifeHappiness = new System.Windows.Forms.Label();
            this.lblSkillLifeFireSafety = new System.Windows.Forms.Label();
            this.lblSkillLifeCounselling = new System.Windows.Forms.Label();
            this.lblSkillLifeAngerMgmt = new System.Windows.Forms.Label();
            this.grpSkillsToddler = new System.Windows.Forms.GroupBox();
            this.trackSkillToddlerWalk = new Sims2Tools.Controls.SkillTracker();
            this.lblSkillToddlerWalk = new System.Windows.Forms.Label();
            this.trackSkillToddlerTalk = new Sims2Tools.Controls.SkillTracker();
            this.lblSkillToddlerTalk = new System.Windows.Forms.Label();
            this.trackSkillToddlerRhyming = new Sims2Tools.Controls.SkillTracker();
            this.lblSkillToddlerRhyming = new System.Windows.Forms.Label();
            this.trackSkillToddlerPotty = new Sims2Tools.Controls.SkillTracker();
            this.lblSkillToddlerPotty = new System.Windows.Forms.Label();
            this.grpSkillsHidden = new System.Windows.Forms.GroupBox();
            this.trackSkillHiddenTaiChi = new Sims2Tools.Controls.SkillTracker();
            this.trackSkillHiddenStudy = new Sims2Tools.Controls.SkillTracker();
            this.trackSkillHiddenPool = new Sims2Tools.Controls.SkillTracker();
            this.trackSkillHiddenMeditate = new Sims2Tools.Controls.SkillTracker();
            this.trackSkillHiddenDance = new Sims2Tools.Controls.SkillTracker();
            this.lblSkillHiddenTaiChi = new System.Windows.Forms.Label();
            this.lblSkillHiddenStudy = new System.Windows.Forms.Label();
            this.lblSkillHiddenPool = new System.Windows.Forms.Label();
            this.lblSkillHiddenMeditate = new System.Windows.Forms.Label();
            this.lblSkillHiddenDance = new System.Windows.Forms.Label();
            this.grpSkillsGeneral = new System.Windows.Forms.GroupBox();
            this.trackSkillRomance = new Sims2Tools.Controls.SkillTracker();
            this.trackSkillMechanical = new Sims2Tools.Controls.SkillTracker();
            this.trackSkillLogic = new Sims2Tools.Controls.SkillTracker();
            this.trackSkillCreativity = new Sims2Tools.Controls.SkillTracker();
            this.trackSkillCooking = new Sims2Tools.Controls.SkillTracker();
            this.trackSkillCleaning = new Sims2Tools.Controls.SkillTracker();
            this.trackSkillCharisma = new Sims2Tools.Controls.SkillTracker();
            this.trackSkillBody = new Sims2Tools.Controls.SkillTracker();
            this.lblSkillRomance = new System.Windows.Forms.Label();
            this.lblSkillMechanical = new System.Windows.Forms.Label();
            this.lblSkillLogic = new System.Windows.Forms.Label();
            this.lblSkillCreativity = new System.Windows.Forms.Label();
            this.lblSkillCooking = new System.Windows.Forms.Label();
            this.lblSkillCleaning = new System.Windows.Forms.Label();
            this.lblSkillCharisma = new System.Windows.Forms.Label();
            this.lblSkillBody = new System.Windows.Forms.Label();
            this.tabInterests = new System.Windows.Forms.TabPage();
            this.grpBadges = new System.Windows.Forms.GroupBox();
            this.trackBadgeStocking = new Sims2Tools.Controls.InterestTracker();
            this.trackBadgePottery = new Sims2Tools.Controls.InterestTracker();
            this.trackBadgeSewing = new Sims2Tools.Controls.InterestTracker();
            this.lblBadgeToyMaking = new System.Windows.Forms.Label();
            this.trackBadgeSales = new Sims2Tools.Controls.InterestTracker();
            this.trackBadgeFlorist = new Sims2Tools.Controls.InterestTracker();
            this.trackBadgeRobotery = new Sims2Tools.Controls.InterestTracker();
            this.trackBadgeToyMaking = new Sims2Tools.Controls.InterestTracker();
            this.lblBadgeStocking = new System.Windows.Forms.Label();
            this.trackBadgeFishing = new Sims2Tools.Controls.InterestTracker();
            this.lblBadgeSewing = new System.Windows.Forms.Label();
            this.trackBadgeGardening = new Sims2Tools.Controls.InterestTracker();
            this.lblBadgeSales = new System.Windows.Forms.Label();
            this.trackBadgeCashier = new Sims2Tools.Controls.InterestTracker();
            this.trackBadgeCosmetics = new Sims2Tools.Controls.InterestTracker();
            this.lblBadgeRobotery = new System.Windows.Forms.Label();
            this.lblBadgePottery = new System.Windows.Forms.Label();
            this.lblBadgeFlorist = new System.Windows.Forms.Label();
            this.lblBadgeFishing = new System.Windows.Forms.Label();
            this.lblBadgeGardening = new System.Windows.Forms.Label();
            this.lblBadgeCosmetics = new System.Windows.Forms.Label();
            this.lblBadgeCashier = new System.Windows.Forms.Label();
            this.grpHobbies = new System.Windows.Forms.GroupBox();
            this.trackHobbySport = new Sims2Tools.Controls.InterestTracker();
            this.trackHobbySecret = new Sims2Tools.Controls.InterestTracker();
            this.trackHobbyScience = new Sims2Tools.Controls.InterestTracker();
            this.trackHobbyMusic = new Sims2Tools.Controls.InterestTracker();
            this.trackHobbyGames = new Sims2Tools.Controls.InterestTracker();
            this.trackHobbyFitness = new Sims2Tools.Controls.InterestTracker();
            this.trackHobbyFilm = new Sims2Tools.Controls.InterestTracker();
            this.trackHobbyArts = new Sims2Tools.Controls.InterestTracker();
            this.trackHobbyNature = new Sims2Tools.Controls.InterestTracker();
            this.trackHobbyCuisine = new Sims2Tools.Controls.InterestTracker();
            this.trackHobbyTinker = new Sims2Tools.Controls.InterestTracker();
            this.comboHobbyOneTrue = new System.Windows.Forms.ComboBox();
            this.lblHobbyOneTrue = new System.Windows.Forms.Label();
            this.lblHobbyScience = new System.Windows.Forms.Label();
            this.lblHobbyMusic = new System.Windows.Forms.Label();
            this.lblHobbyFilm = new System.Windows.Forms.Label();
            this.lblHobbySport = new System.Windows.Forms.Label();
            this.lblHobbyArts = new System.Windows.Forms.Label();
            this.lblHobbySecret = new System.Windows.Forms.Label();
            this.lblHobbyGames = new System.Windows.Forms.Label();
            this.lblHobbyFitness = new System.Windows.Forms.Label();
            this.lblHobbyNature = new System.Windows.Forms.Label();
            this.lblHobbyTinker = new System.Windows.Forms.Label();
            this.lblHobbyCuisine = new System.Windows.Forms.Label();
            this.grpInterests = new System.Windows.Forms.GroupBox();
            this.trackIntWork = new Sims2Tools.Controls.InterestTracker();
            this.trackIntWeather = new Sims2Tools.Controls.InterestTracker();
            this.trackIntTravel = new Sims2Tools.Controls.InterestTracker();
            this.trackIntToys = new Sims2Tools.Controls.InterestTracker();
            this.trackIntSports = new Sims2Tools.Controls.InterestTracker();
            this.trackIntSciFi = new Sims2Tools.Controls.InterestTracker();
            this.trackIntSchool = new Sims2Tools.Controls.InterestTracker();
            this.trackIntParanormal = new Sims2Tools.Controls.InterestTracker();
            this.trackIntPolitics = new Sims2Tools.Controls.InterestTracker();
            this.trackIntMoney = new Sims2Tools.Controls.InterestTracker();
            this.trackIntHealth = new Sims2Tools.Controls.InterestTracker();
            this.trackIntFood = new Sims2Tools.Controls.InterestTracker();
            this.lblIntAnimals = new System.Windows.Forms.Label();
            this.trackIntFashion = new Sims2Tools.Controls.InterestTracker();
            this.lblIntWork = new System.Windows.Forms.Label();
            this.trackIntEnvironment = new Sims2Tools.Controls.InterestTracker();
            this.lblIntWeather = new System.Windows.Forms.Label();
            this.trackIntEntertainment = new Sims2Tools.Controls.InterestTracker();
            this.lblIntTravel = new System.Windows.Forms.Label();
            this.trackIntCulture = new Sims2Tools.Controls.InterestTracker();
            this.lblIntToys = new System.Windows.Forms.Label();
            this.trackIntAnimals = new Sims2Tools.Controls.InterestTracker();
            this.trackIntCrime = new Sims2Tools.Controls.InterestTracker();
            this.lblIntSports = new System.Windows.Forms.Label();
            this.lblIntSciFi = new System.Windows.Forms.Label();
            this.lblIntSchool = new System.Windows.Forms.Label();
            this.lblIntPolitics = new System.Windows.Forms.Label();
            this.lblIntParanormal = new System.Windows.Forms.Label();
            this.lblIntMoney = new System.Windows.Forms.Label();
            this.lblIntHealth = new System.Windows.Forms.Label();
            this.lblIntFood = new System.Windows.Forms.Label();
            this.lblIntFashion = new System.Windows.Forms.Label();
            this.lblIntEnvironment = new System.Windows.Forms.Label();
            this.lblIntEntertainment = new System.Windows.Forms.Label();
            this.lblIntCulture = new System.Windows.Forms.Label();
            this.lblIntCrime = new System.Windows.Forms.Label();
            this.thumbBox = new System.Windows.Forms.PictureBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.saveAsFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openSuitcaseFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveSuitcaseFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.saveJewelboxFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openJewelboxFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.trackSkillHiddenBreakDance = new Sims2Tools.Controls.SkillTracker();
            this.lblSkillHiddenBreakDance = new System.Windows.Forms.Label();
            this.trackSkillHiddenFireDance = new Sims2Tools.Controls.SkillTracker();
            this.lblSkillHiddenFireDance = new System.Windows.Forms.Label();
            this.menuMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitTopBottom)).BeginInit();
            this.splitTopBottom.Panel1.SuspendLayout();
            this.splitTopBottom.Panel2.SuspendLayout();
            this.splitTopBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitTopLeftRight)).BeginInit();
            this.splitTopLeftRight.Panel1.SuspendLayout();
            this.splitTopLeftRight.Panel2.SuspendLayout();
            this.splitTopLeftRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageFamily)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridFamilyMembers)).BeginInit();
            this.menuContextMembers.SuspendLayout();
            this.tabPages.SuspendLayout();
            this.tabFamily.SuspendLayout();
            this.panelFamily.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageHouse)).BeginInit();
            this.tabCloset.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitClosetLeftRight)).BeginInit();
            this.splitClosetLeftRight.Panel1.SuspendLayout();
            this.splitClosetLeftRight.Panel2.SuspendLayout();
            this.splitClosetLeftRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridSuitcase)).BeginInit();
            this.menuContextSuitcase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridFamilyCloset)).BeginInit();
            this.menuContextCloset.SuspendLayout();
            this.tabSafe.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitSafeLeftRight)).BeginInit();
            this.splitSafeLeftRight.Panel1.SuspendLayout();
            this.splitSafeLeftRight.Panel2.SuspendLayout();
            this.splitSafeLeftRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridJewelbox)).BeginInit();
            this.menuContextJewelbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridFamilySafe)).BeginInit();
            this.menuContextSafe.SuspendLayout();
            this.tabCareer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageSim)).BeginInit();
            this.grpJob.SuspendLayout();
            this.grpUniversity.SuspendLayout();
            this.grpSchool.SuspendLayout();
            this.tabSkills.SuspendLayout();
            this.grpSkillsPet.SuspendLayout();
            this.grpSkillsLife.SuspendLayout();
            this.grpSkillsToddler.SuspendLayout();
            this.grpSkillsHidden.SuspendLayout();
            this.grpSkillsGeneral.SuspendLayout();
            this.tabInterests.SuspendLayout();
            this.grpBadges.SuspendLayout();
            this.grpHobbies.SuspendLayout();
            this.grpInterests.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.thumbBox)).BeginInit();
            this.SuspendLayout();
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuHelp,
            this.menuMode,
            this.menuOptions,
            this.menuLanguage,
            this.menuCaching});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuMain.Size = new System.Drawing.Size(1284, 24);
            this.menuMain.TabIndex = 0;
            this.menuMain.Text = "menuStrip";
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemSaveAll,
            this.toolStripSeparator2,
            this.menuItemConfiguration,
            this.menuItemSeparator2,
            this.menuItemExit});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(37, 20);
            this.menuFile.Text = "&File";
            // 
            // menuItemSaveAll
            // 
            this.menuItemSaveAll.Name = "menuItemSaveAll";
            this.menuItemSaveAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.menuItemSaveAll.Size = new System.Drawing.Size(157, 22);
            this.menuItemSaveAll.Text = "&Save All";
            this.menuItemSaveAll.Click += new System.EventHandler(this.OnSaveClicked);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(154, 6);
            // 
            // menuItemConfiguration
            // 
            this.menuItemConfiguration.Name = "menuItemConfiguration";
            this.menuItemConfiguration.Size = new System.Drawing.Size(157, 22);
            this.menuItemConfiguration.Text = "Configuration...";
            this.menuItemConfiguration.Click += new System.EventHandler(this.OnConfigurationClicked);
            // 
            // menuItemSeparator2
            // 
            this.menuItemSeparator2.Name = "menuItemSeparator2";
            this.menuItemSeparator2.Size = new System.Drawing.Size(154, 6);
            // 
            // menuItemExit
            // 
            this.menuItemExit.Name = "menuItemExit";
            this.menuItemExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.menuItemExit.Size = new System.Drawing.Size(157, 22);
            this.menuItemExit.Text = "E&xit";
            this.menuItemExit.Click += new System.EventHandler(this.OnExitClicked);
            // 
            // menuHelp
            // 
            this.menuHelp.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.menuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemAbout});
            this.menuHelp.Name = "menuHelp";
            this.menuHelp.Size = new System.Drawing.Size(44, 20);
            this.menuHelp.Text = "&Help";
            // 
            // menuItemAbout
            // 
            this.menuItemAbout.Name = "menuItemAbout";
            this.menuItemAbout.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.menuItemAbout.Size = new System.Drawing.Size(126, 22);
            this.menuItemAbout.Text = "&About";
            this.menuItemAbout.Click += new System.EventHandler(this.OnHelpClicked);
            // 
            // menuMode
            // 
            this.menuMode.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemAdvanced,
            this.toolStripSeparator4,
            this.menuItemAutoBackup});
            this.menuMode.Name = "menuMode";
            this.menuMode.Size = new System.Drawing.Size(50, 20);
            this.menuMode.Text = "&Mode";
            this.menuMode.DropDownOpening += new System.EventHandler(this.OnModeOpening);
            // 
            // menuItemAdvanced
            // 
            this.menuItemAdvanced.CheckOnClick = true;
            this.menuItemAdvanced.Name = "menuItemAdvanced";
            this.menuItemAdvanced.Size = new System.Drawing.Size(144, 22);
            this.menuItemAdvanced.Text = "Advanced";
            this.menuItemAdvanced.Click += new System.EventHandler(this.OnAdvancedModeChanged);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(141, 6);
            // 
            // menuItemAutoBackup
            // 
            this.menuItemAutoBackup.CheckOnClick = true;
            this.menuItemAutoBackup.Name = "menuItemAutoBackup";
            this.menuItemAutoBackup.Size = new System.Drawing.Size(144, 22);
            this.menuItemAutoBackup.Text = "Auto-&Backup";
            // 
            // menuOptions
            // 
            this.menuOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemUseCodes,
            this.toolStripSeparatorSplitFiles,
            this.menuItemShowSplitFiles,
            this.menuItemHighlightSplitFiles,
            this.toolStripSeparator1,
            this.menuItemIncludeNPCs,
            this.menuItemOnlyNPCs,
            this.toolStripSeparator13,
            this.menuItemTeensHaveAdultJobs,
            this.menuItemYAsHaveAdultJobs,
            this.toolStripSeparator5,
            this.menuItemIntDisplay});
            this.menuOptions.Name = "menuOptions";
            this.menuOptions.Size = new System.Drawing.Size(61, 20);
            this.menuOptions.Text = "&Options";
            this.menuOptions.DropDownOpening += new System.EventHandler(this.OnOptionsOpening);
            // 
            // menuItemUseCodes
            // 
            this.menuItemUseCodes.CheckOnClick = true;
            this.menuItemUseCodes.Name = "menuItemUseCodes";
            this.menuItemUseCodes.Size = new System.Drawing.Size(230, 22);
            this.menuItemUseCodes.Text = "Use Gender/Age Codes";
            this.menuItemUseCodes.Click += new System.EventHandler(this.OnUseCodesClicked);
            // 
            // toolStripSeparatorSplitFiles
            // 
            this.toolStripSeparatorSplitFiles.Name = "toolStripSeparatorSplitFiles";
            this.toolStripSeparatorSplitFiles.Size = new System.Drawing.Size(227, 6);
            // 
            // menuItemShowSplitFiles
            // 
            this.menuItemShowSplitFiles.CheckOnClick = true;
            this.menuItemShowSplitFiles.Name = "menuItemShowSplitFiles";
            this.menuItemShowSplitFiles.Size = new System.Drawing.Size(230, 22);
            this.menuItemShowSplitFiles.Text = "Show Split Character Files";
            this.menuItemShowSplitFiles.Click += new System.EventHandler(this.OnShowSplitFilesClicked);
            // 
            // menuItemHighlightSplitFiles
            // 
            this.menuItemHighlightSplitFiles.CheckOnClick = true;
            this.menuItemHighlightSplitFiles.Name = "menuItemHighlightSplitFiles";
            this.menuItemHighlightSplitFiles.Size = new System.Drawing.Size(230, 22);
            this.menuItemHighlightSplitFiles.Text = "Highlight Split Character Files";
            this.menuItemHighlightSplitFiles.Click += new System.EventHandler(this.OnHighlightSplitFilesClicked);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(227, 6);
            // 
            // menuItemIncludeNPCs
            // 
            this.menuItemIncludeNPCs.CheckOnClick = true;
            this.menuItemIncludeNPCs.Name = "menuItemIncludeNPCs";
            this.menuItemIncludeNPCs.Size = new System.Drawing.Size(230, 22);
            this.menuItemIncludeNPCs.Text = "Include NPCs";
            this.menuItemIncludeNPCs.Click += new System.EventHandler(this.OnIncludeNPCsClicked);
            // 
            // menuItemOnlyNPCs
            // 
            this.menuItemOnlyNPCs.CheckOnClick = true;
            this.menuItemOnlyNPCs.Name = "menuItemOnlyNPCs";
            this.menuItemOnlyNPCs.Size = new System.Drawing.Size(230, 22);
            this.menuItemOnlyNPCs.Text = "Only NPCs";
            this.menuItemOnlyNPCs.Click += new System.EventHandler(this.OnIncludeNPCsClicked);
            // 
            // toolStripSeparator13
            // 
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            this.toolStripSeparator13.Size = new System.Drawing.Size(227, 6);
            // 
            // menuItemTeensHaveAdultJobs
            // 
            this.menuItemTeensHaveAdultJobs.CheckOnClick = true;
            this.menuItemTeensHaveAdultJobs.Name = "menuItemTeensHaveAdultJobs";
            this.menuItemTeensHaveAdultJobs.Size = new System.Drawing.Size(230, 22);
            this.menuItemTeensHaveAdultJobs.Text = "Teens Can Have Adult Jobs";
            this.menuItemTeensHaveAdultJobs.Click += new System.EventHandler(this.OnTeensHaveAdultJobsClicked);
            // 
            // menuItemYAsHaveAdultJobs
            // 
            this.menuItemYAsHaveAdultJobs.CheckOnClick = true;
            this.menuItemYAsHaveAdultJobs.Name = "menuItemYAsHaveAdultJobs";
            this.menuItemYAsHaveAdultJobs.Size = new System.Drawing.Size(230, 22);
            this.menuItemYAsHaveAdultJobs.Text = "YAs Can Have Adult Jobs";
            this.menuItemYAsHaveAdultJobs.Click += new System.EventHandler(this.OnYAsHaveAdultJobsClicked);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(227, 6);
            // 
            // menuItemIntDisplay
            // 
            this.menuItemIntDisplay.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemIntDisplayBarAndBox,
            this.menuItemIntDisplayBarOnly,
            this.menuItemIntDisplayBoxOnly});
            this.menuItemIntDisplay.Name = "menuItemIntDisplay";
            this.menuItemIntDisplay.Size = new System.Drawing.Size(230, 22);
            this.menuItemIntDisplay.Text = "Interests Display As...";
            this.menuItemIntDisplay.DropDownOpening += new System.EventHandler(this.OnInterestsDisplayOpening);
            // 
            // menuItemIntDisplayBarAndBox
            // 
            this.menuItemIntDisplayBarAndBox.Name = "menuItemIntDisplayBarAndBox";
            this.menuItemIntDisplayBarAndBox.Size = new System.Drawing.Size(136, 22);
            this.menuItemIntDisplayBarAndBox.Text = "Bar and Box";
            this.menuItemIntDisplayBarAndBox.Click += new System.EventHandler(this.OnInterestsDisplayClicked);
            // 
            // menuItemIntDisplayBarOnly
            // 
            this.menuItemIntDisplayBarOnly.Name = "menuItemIntDisplayBarOnly";
            this.menuItemIntDisplayBarOnly.Size = new System.Drawing.Size(136, 22);
            this.menuItemIntDisplayBarOnly.Text = "Bar Only";
            this.menuItemIntDisplayBarOnly.Click += new System.EventHandler(this.OnInterestsDisplayClicked);
            // 
            // menuItemIntDisplayBoxOnly
            // 
            this.menuItemIntDisplayBoxOnly.Name = "menuItemIntDisplayBoxOnly";
            this.menuItemIntDisplayBoxOnly.Size = new System.Drawing.Size(136, 22);
            this.menuItemIntDisplayBoxOnly.Text = "Box Only";
            this.menuItemIntDisplayBoxOnly.Click += new System.EventHandler(this.OnInterestsDisplayClicked);
            // 
            // menuLanguage
            // 
            this.menuLanguage.Name = "menuLanguage";
            this.menuLanguage.Size = new System.Drawing.Size(71, 20);
            this.menuLanguage.Text = "&Language";
            // 
            // menuCaching
            // 
            this.menuCaching.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemCachingUpdateCustomCareers,
            this.toolStripSeparator6,
            this.menuItemCachingUpdateMaxisClothes,
            this.menuItemCachingUpdateCustomClothes,
            this.toolStripSeparator7,
            this.menuItemCachingUpdateMaxisJewellery,
            this.menuItemCachingUpdateCustomJewellery,
            this.toolStripSeparatorCaching,
            this.menuItemCachingRemoveLocal,
            this.menuItemCachingRemoveThumbnails});
            this.menuCaching.Name = "menuCaching";
            this.menuCaching.Size = new System.Drawing.Size(63, 20);
            this.menuCaching.Text = "&Caching";
            this.menuCaching.DropDownOpening += new System.EventHandler(this.OnCachingOpening);
            // 
            // menuItemCachingUpdateCustomCareers
            // 
            this.menuItemCachingUpdateCustomCareers.Name = "menuItemCachingUpdateCustomCareers";
            this.menuItemCachingUpdateCustomCareers.Size = new System.Drawing.Size(243, 22);
            this.menuItemCachingUpdateCustomCareers.Text = "Update Custom Career Cache";
            this.menuItemCachingUpdateCustomCareers.Click += new System.EventHandler(this.OnCachingUpdateCustomCareers);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(240, 6);
            // 
            // menuItemCachingUpdateMaxisClothes
            // 
            this.menuItemCachingUpdateMaxisClothes.Name = "menuItemCachingUpdateMaxisClothes";
            this.menuItemCachingUpdateMaxisClothes.Size = new System.Drawing.Size(243, 22);
            this.menuItemCachingUpdateMaxisClothes.Text = "Update Maxis Clothing Cache";
            this.menuItemCachingUpdateMaxisClothes.Click += new System.EventHandler(this.OnCachingUpdateMaxisOutfits);
            // 
            // menuItemCachingUpdateCustomClothes
            // 
            this.menuItemCachingUpdateCustomClothes.Name = "menuItemCachingUpdateCustomClothes";
            this.menuItemCachingUpdateCustomClothes.Size = new System.Drawing.Size(243, 22);
            this.menuItemCachingUpdateCustomClothes.Text = "Update Custom Clothing Cache";
            this.menuItemCachingUpdateCustomClothes.Click += new System.EventHandler(this.OnCachingUpdateCustomOutfits);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(240, 6);
            // 
            // menuItemCachingUpdateMaxisJewellery
            // 
            this.menuItemCachingUpdateMaxisJewellery.Name = "menuItemCachingUpdateMaxisJewellery";
            this.menuItemCachingUpdateMaxisJewellery.Size = new System.Drawing.Size(243, 22);
            this.menuItemCachingUpdateMaxisJewellery.Text = "Update Maxis Jewellery Cache";
            this.menuItemCachingUpdateMaxisJewellery.Click += new System.EventHandler(this.OnCachingUpdateMaxisOutfits);
            // 
            // menuItemCachingUpdateCustomJewellery
            // 
            this.menuItemCachingUpdateCustomJewellery.Name = "menuItemCachingUpdateCustomJewellery";
            this.menuItemCachingUpdateCustomJewellery.Size = new System.Drawing.Size(243, 22);
            this.menuItemCachingUpdateCustomJewellery.Text = "Update Custom Jewellery Cache";
            this.menuItemCachingUpdateCustomJewellery.Click += new System.EventHandler(this.OnCachingUpdateCustomOutfits);
            // 
            // toolStripSeparatorCaching
            // 
            this.toolStripSeparatorCaching.Name = "toolStripSeparatorCaching";
            this.toolStripSeparatorCaching.Size = new System.Drawing.Size(240, 6);
            // 
            // menuItemCachingRemoveLocal
            // 
            this.menuItemCachingRemoveLocal.Name = "menuItemCachingRemoveLocal";
            this.menuItemCachingRemoveLocal.Size = new System.Drawing.Size(243, 22);
            this.menuItemCachingRemoveLocal.Text = "Remove Local Caches";
            this.menuItemCachingRemoveLocal.Click += new System.EventHandler(this.OnCachingRemoveLocal);
            // 
            // menuItemCachingRemoveThumbnails
            // 
            this.menuItemCachingRemoveThumbnails.Name = "menuItemCachingRemoveThumbnails";
            this.menuItemCachingRemoveThumbnails.Size = new System.Drawing.Size(243, 22);
            this.menuItemCachingRemoveThumbnails.Text = "Remove Thumbnails Cache";
            this.menuItemCachingRemoveThumbnails.Click += new System.EventHandler(this.OnCachingRemoveThumbnails);
            // 
            // splitTopBottom
            // 
            this.splitTopBottom.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitTopBottom.Location = new System.Drawing.Point(0, 24);
            this.splitTopBottom.Name = "splitTopBottom";
            this.splitTopBottom.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitTopBottom.Panel1
            // 
            this.splitTopBottom.Panel1.Controls.Add(this.splitTopLeftRight);
            this.splitTopBottom.Panel1MinSize = 200;
            // 
            // splitTopBottom.Panel2
            // 
            this.splitTopBottom.Panel2.Controls.Add(this.tabPages);
            this.splitTopBottom.Panel2MinSize = 200;
            this.splitTopBottom.Size = new System.Drawing.Size(1284, 625);
            this.splitTopBottom.SplitterDistance = 312;
            this.splitTopBottom.TabIndex = 1;
            // 
            // splitTopLeftRight
            // 
            this.splitTopLeftRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitTopLeftRight.Location = new System.Drawing.Point(0, 0);
            this.splitTopLeftRight.Name = "splitTopLeftRight";
            // 
            // splitTopLeftRight.Panel1
            // 
            this.splitTopLeftRight.Panel1.Controls.Add(this.treeHoods);
            this.splitTopLeftRight.Panel1MinSize = 300;
            // 
            // splitTopLeftRight.Panel2
            // 
            this.splitTopLeftRight.Panel2.Controls.Add(this.lblLotName);
            this.splitTopLeftRight.Panel2.Controls.Add(this.lblFamilyName);
            this.splitTopLeftRight.Panel2.Controls.Add(this.imageFamily);
            this.splitTopLeftRight.Panel2.Controls.Add(this.gridFamilyMembers);
            this.splitTopLeftRight.Panel2MinSize = 300;
            this.splitTopLeftRight.Size = new System.Drawing.Size(1284, 312);
            this.splitTopLeftRight.SplitterDistance = 500;
            this.splitTopLeftRight.TabIndex = 0;
            this.splitTopLeftRight.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.OnSplitterMoved);
            // 
            // treeHoods
            // 
            this.treeHoods.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeHoods.BackColor = System.Drawing.SystemColors.Window;
            this.treeHoods.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.treeHoods.HideSelection = false;
            this.treeHoods.Location = new System.Drawing.Point(4, 0);
            this.treeHoods.Name = "treeHoods";
            this.treeHoods.Size = new System.Drawing.Size(497, 312);
            this.treeHoods.TabIndex = 0;
            this.treeHoods.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.OnTreeHoodsBeforeExpand);
            this.treeHoods.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.OnTreeHoods_DrawNode);
            this.treeHoods.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.OnTreeHoodsClicked);
            // 
            // lblLotName
            // 
            this.lblLotName.AutoSize = true;
            this.lblLotName.Location = new System.Drawing.Point(3, 25);
            this.lblLotName.Name = "lblLotName";
            this.lblLotName.Size = new System.Drawing.Size(61, 15);
            this.lblLotName.TabIndex = 3;
            this.lblLotName.Text = "Lot Name";
            // 
            // lblFamilyName
            // 
            this.lblFamilyName.AutoSize = true;
            this.lblFamilyName.Location = new System.Drawing.Point(3, 3);
            this.lblFamilyName.Name = "lblFamilyName";
            this.lblFamilyName.Size = new System.Drawing.Size(80, 15);
            this.lblFamilyName.TabIndex = 2;
            this.lblFamilyName.Text = "Family Name";
            // 
            // imageFamily
            // 
            this.imageFamily.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.imageFamily.Location = new System.Drawing.Point(584, 28);
            this.imageFamily.Name = "imageFamily";
            this.imageFamily.Size = new System.Drawing.Size(192, 192);
            this.imageFamily.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imageFamily.TabIndex = 1;
            this.imageFamily.TabStop = false;
            // 
            // gridFamilyMembers
            // 
            this.gridFamilyMembers.AllowUserToAddRows = false;
            this.gridFamilyMembers.AllowUserToDeleteRows = false;
            this.gridFamilyMembers.AllowUserToOrderColumns = true;
            this.gridFamilyMembers.AllowUserToResizeRows = false;
            this.gridFamilyMembers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridFamilyMembers.BackgroundColor = System.Drawing.SystemColors.Window;
            this.gridFamilyMembers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridFamilyMembers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colFirstName,
            this.colSplitFile,
            this.colGender,
            this.colGenderCode,
            this.colAge,
            this.colAgeCode,
            this.colDaysLeft,
            this.colGenderHex,
            this.colAgeHex,
            this.colThumbnail,
            this.colData});
            this.gridFamilyMembers.ContextMenuStrip = this.menuContextMembers;
            this.gridFamilyMembers.Location = new System.Drawing.Point(0, 50);
            this.gridFamilyMembers.Name = "gridFamilyMembers";
            this.gridFamilyMembers.ReadOnly = true;
            this.gridFamilyMembers.RowHeadersVisible = false;
            this.gridFamilyMembers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridFamilyMembers.Size = new System.Drawing.Size(580, 262);
            this.gridFamilyMembers.TabIndex = 0;
            this.gridFamilyMembers.MultiSelectChanged += new System.EventHandler(this.OnMemberGridSelectionChanged);
            this.gridFamilyMembers.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellMouseEnter);
            this.gridFamilyMembers.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellMouseLeave);
            this.gridFamilyMembers.CellToolTipTextNeeded += new System.Windows.Forms.DataGridViewCellToolTipTextNeededEventHandler(this.OnToolTipTextNeeded);
            this.gridFamilyMembers.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.OnDataBindingComplete);
            this.gridFamilyMembers.SelectionChanged += new System.EventHandler(this.OnMemberGridSelectionChanged);
            // 
            // colFirstName
            // 
            this.colFirstName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colFirstName.DataPropertyName = "FirstName";
            this.colFirstName.HeaderText = "Name";
            this.colFirstName.Name = "colFirstName";
            this.colFirstName.ReadOnly = true;
            // 
            // colSplitFile
            // 
            this.colSplitFile.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colSplitFile.DataPropertyName = "SplitFile";
            this.colSplitFile.HeaderText = "Split";
            this.colSplitFile.Name = "colSplitFile";
            this.colSplitFile.ReadOnly = true;
            this.colSplitFile.ToolTipText = "Character file is split";
            this.colSplitFile.Width = 56;
            // 
            // colGender
            // 
            this.colGender.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colGender.DataPropertyName = "Gender";
            this.colGender.FillWeight = 75F;
            this.colGender.HeaderText = "Gender";
            this.colGender.Name = "colGender";
            this.colGender.ReadOnly = true;
            this.colGender.Width = 73;
            // 
            // colGenderCode
            // 
            this.colGenderCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colGenderCode.DataPropertyName = "GenderCode";
            this.colGenderCode.HeaderText = "⚥";
            this.colGenderCode.Name = "colGenderCode";
            this.colGenderCode.ReadOnly = true;
            this.colGenderCode.Visible = false;
            // 
            // colAge
            // 
            this.colAge.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colAge.DataPropertyName = "Age";
            this.colAge.FillWeight = 55F;
            this.colAge.HeaderText = "Age";
            this.colAge.Name = "colAge";
            this.colAge.ReadOnly = true;
            this.colAge.Width = 53;
            // 
            // colAgeCode
            // 
            this.colAgeCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colAgeCode.DataPropertyName = "AgeCode";
            this.colAgeCode.HeaderText = "Age";
            this.colAgeCode.Name = "colAgeCode";
            this.colAgeCode.ReadOnly = true;
            this.colAgeCode.Visible = false;
            // 
            // colDaysLeft
            // 
            this.colDaysLeft.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colDaysLeft.DataPropertyName = "DaysLeft";
            this.colDaysLeft.FillWeight = 75F;
            this.colDaysLeft.HeaderText = "Left";
            this.colDaysLeft.Name = "colDaysLeft";
            this.colDaysLeft.ReadOnly = true;
            // 
            // colGenderHex
            // 
            this.colGenderHex.DataPropertyName = "GenderHex";
            this.colGenderHex.HeaderText = "Gender Hex";
            this.colGenderHex.Name = "colGenderHex";
            this.colGenderHex.ReadOnly = true;
            this.colGenderHex.Visible = false;
            // 
            // colAgeHex
            // 
            this.colAgeHex.DataPropertyName = "AgeHex";
            this.colAgeHex.HeaderText = "Age Hex";
            this.colAgeHex.Name = "colAgeHex";
            this.colAgeHex.ReadOnly = true;
            this.colAgeHex.Visible = false;
            // 
            // colThumbnail
            // 
            this.colThumbnail.DataPropertyName = "Thumbnail";
            this.colThumbnail.HeaderText = "Thumbnail";
            this.colThumbnail.Name = "colThumbnail";
            this.colThumbnail.ReadOnly = true;
            this.colThumbnail.Visible = false;
            // 
            // colData
            // 
            this.colData.DataPropertyName = "Data";
            this.colData.HeaderText = "Data";
            this.colData.Name = "colData";
            this.colData.ReadOnly = true;
            this.colData.Visible = false;
            // 
            // menuContextMembers
            // 
            this.menuContextMembers.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuContextMemberChangeSimName,
            this.menuContextMemberChangeFamilyName,
            this.menuContextMemberChangeDays,
            this.menuContextMemberFilterAll,
            this.menuContextMemberFilterSelected,
            this.menuContextMemberFilterThis,
            this.menuContextMemberSeparator1,
            this.menuContextMemberMergeSplitFiles});
            this.menuContextMembers.Name = "menuContextMembers";
            this.menuContextMembers.Size = new System.Drawing.Size(223, 164);
            this.menuContextMembers.Opening += new System.ComponentModel.CancelEventHandler(this.OnContextMembersOpening);
            // 
            // menuContextMemberChangeSimName
            // 
            this.menuContextMemberChangeSimName.Name = "menuContextMemberChangeSimName";
            this.menuContextMemberChangeSimName.Size = new System.Drawing.Size(222, 22);
            this.menuContextMemberChangeSimName.Text = "Change This Sim\'s &Name";
            this.menuContextMemberChangeSimName.Click += new System.EventHandler(this.OnChangeSimNameClicked);
            // 
            // menuContextMemberChangeFamilyName
            // 
            this.menuContextMemberChangeFamilyName.Name = "menuContextMemberChangeFamilyName";
            this.menuContextMemberChangeFamilyName.Size = new System.Drawing.Size(222, 22);
            this.menuContextMemberChangeFamilyName.Text = "Change &Family Name";
            this.menuContextMemberChangeFamilyName.Click += new System.EventHandler(this.OnChangeFamilyNameClicked);
            // 
            // menuContextMemberChangeDays
            // 
            this.menuContextMemberChangeDays.Name = "menuContextMemberChangeDays";
            this.menuContextMemberChangeDays.Size = new System.Drawing.Size(222, 22);
            this.menuContextMemberChangeDays.Text = "Add/Remove &Days";
            this.menuContextMemberChangeDays.Click += new System.EventHandler(this.OnChangeDaysClicked);
            // 
            // menuContextMemberFilterAll
            // 
            this.menuContextMemberFilterAll.Name = "menuContextMemberFilterAll";
            this.menuContextMemberFilterAll.Size = new System.Drawing.Size(222, 22);
            this.menuContextMemberFilterAll.Text = "Show &All";
            this.menuContextMemberFilterAll.Click += new System.EventHandler(this.OnShowAllClicked);
            // 
            // menuContextMemberFilterSelected
            // 
            this.menuContextMemberFilterSelected.Name = "menuContextMemberFilterSelected";
            this.menuContextMemberFilterSelected.Size = new System.Drawing.Size(222, 22);
            this.menuContextMemberFilterSelected.Text = "Show only for &Selected Sims";
            this.menuContextMemberFilterSelected.Click += new System.EventHandler(this.OnShowSelectedSimsClicked);
            // 
            // menuContextMemberFilterThis
            // 
            this.menuContextMemberFilterThis.Name = "menuContextMemberFilterThis";
            this.menuContextMemberFilterThis.Size = new System.Drawing.Size(222, 22);
            this.menuContextMemberFilterThis.Text = "Show only for &This Sim";
            this.menuContextMemberFilterThis.Click += new System.EventHandler(this.OnShowThisSimClicked);
            // 
            // menuContextMemberSeparator1
            // 
            this.menuContextMemberSeparator1.Name = "menuContextMemberSeparator1";
            this.menuContextMemberSeparator1.Size = new System.Drawing.Size(219, 6);
            // 
            // menuContextMemberMergeSplitFiles
            // 
            this.menuContextMemberMergeSplitFiles.Name = "menuContextMemberMergeSplitFiles";
            this.menuContextMemberMergeSplitFiles.Size = new System.Drawing.Size(222, 22);
            this.menuContextMemberMergeSplitFiles.Text = "Merge Split Files";
            this.menuContextMemberMergeSplitFiles.Click += new System.EventHandler(this.OnMergeSplitFilesClicked);
            // 
            // tabPages
            // 
            this.tabPages.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabPages.Controls.Add(this.tabFamily);
            this.tabPages.Controls.Add(this.tabCloset);
            this.tabPages.Controls.Add(this.tabSafe);
            this.tabPages.Controls.Add(this.tabCareer);
            this.tabPages.Controls.Add(this.tabSkills);
            this.tabPages.Controls.Add(this.tabInterests);
            this.tabPages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabPages.Location = new System.Drawing.Point(0, 0);
            this.tabPages.Margin = new System.Windows.Forms.Padding(0);
            this.tabPages.Name = "tabPages";
            this.tabPages.Padding = new System.Drawing.Point(0, 0);
            this.tabPages.SelectedIndex = 0;
            this.tabPages.Size = new System.Drawing.Size(1284, 309);
            this.tabPages.TabIndex = 4;
            this.tabPages.SelectedIndexChanged += new System.EventHandler(this.OnTabPageChanged);
            // 
            // tabFamily
            // 
            this.tabFamily.Controls.Add(this.panelFamily);
            this.tabFamily.Location = new System.Drawing.Point(4, 4);
            this.tabFamily.Margin = new System.Windows.Forms.Padding(0);
            this.tabFamily.Name = "tabFamily";
            this.tabFamily.Size = new System.Drawing.Size(1276, 281);
            this.tabFamily.TabIndex = 1;
            this.tabFamily.Text = "Household";
            this.tabFamily.UseVisualStyleBackColor = true;
            // 
            // panelFamily
            // 
            this.panelFamily.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelFamily.Controls.Add(this.ckbFamilyNameSelected);
            this.panelFamily.Controls.Add(this.ckbFamilyNameSame);
            this.panelFamily.Controls.Add(this.ckbFamilyNameAll);
            this.panelFamily.Controls.Add(this.textAddressDesc);
            this.panelFamily.Controls.Add(this.ckbMoneyLock);
            this.panelFamily.Controls.Add(this.textBusinessMoney);
            this.panelFamily.Controls.Add(this.lblBusinessMoney);
            this.panelFamily.Controls.Add(this.imageHouse);
            this.panelFamily.Controls.Add(this.textFamilyName);
            this.panelFamily.Controls.Add(this.lblFamName);
            this.panelFamily.Controls.Add(this.lblAddress);
            this.panelFamily.Controls.Add(this.textFamilyWriteUp);
            this.panelFamily.Controls.Add(this.textAddressName);
            this.panelFamily.Controls.Add(this.lblWriteUp);
            this.panelFamily.Controls.Add(this.textFamilyMoney);
            this.panelFamily.Controls.Add(this.lblMoney);
            this.panelFamily.Location = new System.Drawing.Point(-1, 0);
            this.panelFamily.Name = "panelFamily";
            this.panelFamily.Size = new System.Drawing.Size(1277, 243);
            this.panelFamily.TabIndex = 13;
            // 
            // ckbFamilyNameSelected
            // 
            this.ckbFamilyNameSelected.AutoSize = true;
            this.ckbFamilyNameSelected.Location = new System.Drawing.Point(466, 9);
            this.ckbFamilyNameSelected.Name = "ckbFamilyNameSelected";
            this.ckbFamilyNameSelected.Size = new System.Drawing.Size(74, 19);
            this.ckbFamilyNameSelected.TabIndex = 14;
            this.ckbFamilyNameSelected.Text = "Selected";
            this.ckbFamilyNameSelected.UseVisualStyleBackColor = true;
            this.ckbFamilyNameSelected.CheckedChanged += new System.EventHandler(this.OnFamilyNameChecked);
            // 
            // ckbFamilyNameSame
            // 
            this.ckbFamilyNameSame.AutoSize = true;
            this.ckbFamilyNameSame.Checked = true;
            this.ckbFamilyNameSame.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbFamilyNameSame.Location = new System.Drawing.Point(401, 9);
            this.ckbFamilyNameSame.Name = "ckbFamilyNameSame";
            this.ckbFamilyNameSame.Size = new System.Drawing.Size(59, 19);
            this.ckbFamilyNameSame.TabIndex = 13;
            this.ckbFamilyNameSame.Text = "Same";
            this.ckbFamilyNameSame.UseVisualStyleBackColor = true;
            this.ckbFamilyNameSame.CheckedChanged += new System.EventHandler(this.OnFamilyNameChecked);
            // 
            // ckbFamilyNameAll
            // 
            this.ckbFamilyNameAll.AutoSize = true;
            this.ckbFamilyNameAll.Location = new System.Drawing.Point(546, 9);
            this.ckbFamilyNameAll.Name = "ckbFamilyNameAll";
            this.ckbFamilyNameAll.Size = new System.Drawing.Size(39, 19);
            this.ckbFamilyNameAll.TabIndex = 15;
            this.ckbFamilyNameAll.Text = "All";
            this.ckbFamilyNameAll.UseVisualStyleBackColor = true;
            this.ckbFamilyNameAll.CheckedChanged += new System.EventHandler(this.OnFamilyNameChecked);
            // 
            // textAddressDesc
            // 
            this.textAddressDesc.Location = new System.Drawing.Point(79, 61);
            this.textAddressDesc.Name = "textAddressDesc";
            this.textAddressDesc.Size = new System.Drawing.Size(319, 21);
            this.textAddressDesc.TabIndex = 18;
            this.textAddressDesc.TextChanged += new System.EventHandler(this.OnTextChanged);
            this.textAddressDesc.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnKeyUp);
            this.textAddressDesc.Leave += new System.EventHandler(this.OnFamilyControlLeave);
            // 
            // ckbMoneyLock
            // 
            this.ckbMoneyLock.AutoSize = true;
            this.ckbMoneyLock.Checked = true;
            this.ckbMoneyLock.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbMoneyLock.Location = new System.Drawing.Point(353, 182);
            this.ckbMoneyLock.Name = "ckbMoneyLock";
            this.ckbMoneyLock.Size = new System.Drawing.Size(52, 19);
            this.ckbMoneyLock.TabIndex = 25;
            this.ckbMoneyLock.Text = "Lock";
            this.ckbMoneyLock.UseVisualStyleBackColor = true;
            this.ckbMoneyLock.CheckedChanged += new System.EventHandler(this.OnMoneyLockChanged);
            // 
            // textBusinessMoney
            // 
            this.textBusinessMoney.Enabled = false;
            this.textBusinessMoney.Location = new System.Drawing.Point(267, 180);
            this.textBusinessMoney.Name = "textBusinessMoney";
            this.textBusinessMoney.Size = new System.Drawing.Size(75, 21);
            this.textBusinessMoney.TabIndex = 24;
            this.textBusinessMoney.TextChanged += new System.EventHandler(this.OnTextChanged);
            this.textBusinessMoney.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnKeyUp);
            this.textBusinessMoney.Leave += new System.EventHandler(this.OnFamilyControlLeave);
            this.textBusinessMoney.Validating += new System.ComponentModel.CancelEventHandler(this.OnValidating_Money);
            this.textBusinessMoney.Validated += new System.EventHandler(this.OnValidated_Ok);
            // 
            // lblBusinessMoney
            // 
            this.lblBusinessMoney.AutoSize = true;
            this.lblBusinessMoney.Location = new System.Drawing.Point(161, 183);
            this.lblBusinessMoney.Name = "lblBusinessMoney";
            this.lblBusinessMoney.Size = new System.Drawing.Size(100, 15);
            this.lblBusinessMoney.TabIndex = 23;
            this.lblBusinessMoney.Text = "Business Money:";
            // 
            // imageHouse
            // 
            this.imageHouse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.imageHouse.Location = new System.Drawing.Point(1085, 7);
            this.imageHouse.Name = "imageHouse";
            this.imageHouse.Size = new System.Drawing.Size(192, 192);
            this.imageHouse.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imageHouse.TabIndex = 4;
            this.imageHouse.TabStop = false;
            // 
            // textFamilyName
            // 
            this.textFamilyName.Location = new System.Drawing.Point(79, 7);
            this.textFamilyName.Name = "textFamilyName";
            this.textFamilyName.Size = new System.Drawing.Size(319, 21);
            this.textFamilyName.TabIndex = 12;
            this.textFamilyName.TextChanged += new System.EventHandler(this.OnTextChanged);
            this.textFamilyName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnKeyUp);
            this.textFamilyName.Leave += new System.EventHandler(this.OnFamilyControlLeave);
            this.textFamilyName.Validating += new System.ComponentModel.CancelEventHandler(this.OnValidating_NotEmpty);
            this.textFamilyName.Validated += new System.EventHandler(this.OnValidated_Ok);
            // 
            // lblFamName
            // 
            this.lblFamName.AutoSize = true;
            this.lblFamName.Location = new System.Drawing.Point(3, 7);
            this.lblFamName.Name = "lblFamName";
            this.lblFamName.Size = new System.Drawing.Size(70, 15);
            this.lblFamName.TabIndex = 11;
            this.lblFamName.Text = "Household:";
            // 
            // lblAddress
            // 
            this.lblAddress.AutoSize = true;
            this.lblAddress.Location = new System.Drawing.Point(19, 37);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(54, 15);
            this.lblAddress.TabIndex = 16;
            this.lblAddress.Text = "Address:";
            // 
            // textFamilyWriteUp
            // 
            this.textFamilyWriteUp.Location = new System.Drawing.Point(79, 88);
            this.textFamilyWriteUp.Multiline = true;
            this.textFamilyWriteUp.Name = "textFamilyWriteUp";
            this.textFamilyWriteUp.Size = new System.Drawing.Size(319, 89);
            this.textFamilyWriteUp.TabIndex = 20;
            this.textFamilyWriteUp.TextChanged += new System.EventHandler(this.OnTextChanged);
            this.textFamilyWriteUp.Leave += new System.EventHandler(this.OnFamilyControlLeave);
            // 
            // textAddressName
            // 
            this.textAddressName.Location = new System.Drawing.Point(79, 34);
            this.textAddressName.Name = "textAddressName";
            this.textAddressName.Size = new System.Drawing.Size(319, 21);
            this.textAddressName.TabIndex = 17;
            this.textAddressName.TextChanged += new System.EventHandler(this.OnTextChanged);
            this.textAddressName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnKeyUp);
            this.textAddressName.Leave += new System.EventHandler(this.OnFamilyControlLeave);
            this.textAddressName.Validating += new System.ComponentModel.CancelEventHandler(this.OnValidating_NotEmpty);
            this.textAddressName.Validated += new System.EventHandler(this.OnValidated_Ok);
            // 
            // lblWriteUp
            // 
            this.lblWriteUp.AutoSize = true;
            this.lblWriteUp.Location = new System.Drawing.Point(16, 91);
            this.lblWriteUp.Name = "lblWriteUp";
            this.lblWriteUp.Size = new System.Drawing.Size(57, 15);
            this.lblWriteUp.TabIndex = 19;
            this.lblWriteUp.Text = "Write Up:";
            // 
            // textFamilyMoney
            // 
            this.textFamilyMoney.Location = new System.Drawing.Point(79, 180);
            this.textFamilyMoney.Name = "textFamilyMoney";
            this.textFamilyMoney.Size = new System.Drawing.Size(75, 21);
            this.textFamilyMoney.TabIndex = 22;
            this.textFamilyMoney.TextChanged += new System.EventHandler(this.OnTextChanged);
            this.textFamilyMoney.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnKeyUp);
            this.textFamilyMoney.Leave += new System.EventHandler(this.OnFamilyControlLeave);
            this.textFamilyMoney.Validating += new System.ComponentModel.CancelEventHandler(this.OnValidating_Money);
            this.textFamilyMoney.Validated += new System.EventHandler(this.OnValidated_Ok);
            // 
            // lblMoney
            // 
            this.lblMoney.AutoSize = true;
            this.lblMoney.Location = new System.Drawing.Point(26, 186);
            this.lblMoney.Name = "lblMoney";
            this.lblMoney.Size = new System.Drawing.Size(47, 15);
            this.lblMoney.TabIndex = 21;
            this.lblMoney.Text = "Money:";
            // 
            // tabCloset
            // 
            this.tabCloset.Controls.Add(this.splitClosetLeftRight);
            this.tabCloset.Location = new System.Drawing.Point(4, 4);
            this.tabCloset.Margin = new System.Windows.Forms.Padding(0);
            this.tabCloset.Name = "tabCloset";
            this.tabCloset.Size = new System.Drawing.Size(1276, 283);
            this.tabCloset.TabIndex = 0;
            this.tabCloset.Text = "Closet";
            this.tabCloset.UseVisualStyleBackColor = true;
            // 
            // splitClosetLeftRight
            // 
            this.splitClosetLeftRight.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitClosetLeftRight.Location = new System.Drawing.Point(-3, -3);
            this.splitClosetLeftRight.Name = "splitClosetLeftRight";
            // 
            // splitClosetLeftRight.Panel1
            // 
            this.splitClosetLeftRight.Panel1.Controls.Add(this.gridSuitcase);
            this.splitClosetLeftRight.Panel1.Controls.Add(this.btnSuitcaseEmpty);
            this.splitClosetLeftRight.Panel1.Controls.Add(this.btnSuitcaseSave);
            this.splitClosetLeftRight.Panel1.Controls.Add(this.btnSuitcaseLoad);
            this.splitClosetLeftRight.Panel1.Controls.Add(this.btnSuitcaseCopy);
            this.splitClosetLeftRight.Panel1.Controls.Add(this.btnSuitcaseMove);
            this.splitClosetLeftRight.Panel1MinSize = 200;
            // 
            // splitClosetLeftRight.Panel2
            // 
            this.splitClosetLeftRight.Panel2.Controls.Add(this.lblClosetCachesNeeded);
            this.splitClosetLeftRight.Panel2.Controls.Add(this.gridFamilyCloset);
            this.splitClosetLeftRight.Panel2.Controls.Add(this.btnClosetCopy);
            this.splitClosetLeftRight.Panel2.Controls.Add(this.btnClosetMove);
            this.splitClosetLeftRight.Panel2.Controls.Add(this.btnClosetDelete);
            this.splitClosetLeftRight.Panel2.Controls.Add(this.btnClosetShowAll);
            this.splitClosetLeftRight.Panel2MinSize = 300;
            this.splitClosetLeftRight.Size = new System.Drawing.Size(1280, 280);
            this.splitClosetLeftRight.SplitterDistance = 500;
            this.splitClosetLeftRight.TabIndex = 0;
            this.splitClosetLeftRight.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.OnSplitterMoved);
            // 
            // gridSuitcase
            // 
            this.gridSuitcase.AllowDrop = true;
            this.gridSuitcase.AllowUserToAddRows = false;
            this.gridSuitcase.AllowUserToDeleteRows = false;
            this.gridSuitcase.AllowUserToOrderColumns = true;
            this.gridSuitcase.AllowUserToResizeRows = false;
            this.gridSuitcase.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridSuitcase.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridSuitcase.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridSuitcase.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridSuitcase.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSuitcaseVisible,
            this.colSuitcaseName,
            this.colSuitcaseCategory,
            this.colSuitcaseGender,
            this.colSuitcaseGenderCode,
            this.colSuitcaseAge,
            this.colSuitcaseAgeCode,
            this.colSuitcaseData,
            this.colSuitcaseGenderHex,
            this.colSuitcaseAgeHex,
            this.colSuitcaseThumbKey,
            this.colSuitcaseLocalThumbKey});
            this.gridSuitcase.ContextMenuStrip = this.menuContextSuitcase;
            this.gridSuitcase.Location = new System.Drawing.Point(3, 3);
            this.gridSuitcase.Name = "gridSuitcase";
            this.gridSuitcase.ReadOnly = true;
            this.gridSuitcase.RowHeadersVisible = false;
            this.gridSuitcase.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridSuitcase.Size = new System.Drawing.Size(497, 245);
            this.gridSuitcase.TabIndex = 2;
            this.gridSuitcase.MultiSelectChanged += new System.EventHandler(this.OnOutfitGridSelectionChanged);
            this.gridSuitcase.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellMouseEnter);
            this.gridSuitcase.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellMouseLeave);
            this.gridSuitcase.CellToolTipTextNeeded += new System.Windows.Forms.DataGridViewCellToolTipTextNeededEventHandler(this.OnToolTipTextNeeded);
            this.gridSuitcase.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.OnDataBindingComplete);
            this.gridSuitcase.SelectionChanged += new System.EventHandler(this.OnOutfitGridSelectionChanged);
            this.gridSuitcase.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnGridDragDrop);
            this.gridSuitcase.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnGridDragEnter);
            this.gridSuitcase.DragOver += new System.Windows.Forms.DragEventHandler(this.OnGridDragOver);
            this.gridSuitcase.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnGridMouseDown);
            // 
            // colSuitcaseVisible
            // 
            this.colSuitcaseVisible.DataPropertyName = "Visible";
            this.colSuitcaseVisible.HeaderText = "Visible";
            this.colSuitcaseVisible.Name = "colSuitcaseVisible";
            this.colSuitcaseVisible.ReadOnly = true;
            this.colSuitcaseVisible.Visible = false;
            // 
            // colSuitcaseName
            // 
            this.colSuitcaseName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colSuitcaseName.DataPropertyName = "Name";
            this.colSuitcaseName.FillWeight = 300F;
            this.colSuitcaseName.HeaderText = "Suitcase";
            this.colSuitcaseName.Name = "colSuitcaseName";
            this.colSuitcaseName.ReadOnly = true;
            // 
            // colSuitcaseCategory
            // 
            this.colSuitcaseCategory.DataPropertyName = "Category";
            this.colSuitcaseCategory.HeaderText = "Category";
            this.colSuitcaseCategory.Name = "colSuitcaseCategory";
            this.colSuitcaseCategory.ReadOnly = true;
            // 
            // colSuitcaseGender
            // 
            this.colSuitcaseGender.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colSuitcaseGender.DataPropertyName = "Gender";
            this.colSuitcaseGender.FillWeight = 75F;
            this.colSuitcaseGender.HeaderText = "Gender";
            this.colSuitcaseGender.Name = "colSuitcaseGender";
            this.colSuitcaseGender.ReadOnly = true;
            this.colSuitcaseGender.Width = 73;
            // 
            // colSuitcaseGenderCode
            // 
            this.colSuitcaseGenderCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colSuitcaseGenderCode.DataPropertyName = "GenderCode";
            this.colSuitcaseGenderCode.HeaderText = "⚥";
            this.colSuitcaseGenderCode.Name = "colSuitcaseGenderCode";
            this.colSuitcaseGenderCode.ReadOnly = true;
            this.colSuitcaseGenderCode.Visible = false;
            // 
            // colSuitcaseAge
            // 
            this.colSuitcaseAge.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colSuitcaseAge.DataPropertyName = "Age";
            this.colSuitcaseAge.FillWeight = 55F;
            this.colSuitcaseAge.HeaderText = "Age";
            this.colSuitcaseAge.Name = "colSuitcaseAge";
            this.colSuitcaseAge.ReadOnly = true;
            this.colSuitcaseAge.Width = 53;
            // 
            // colSuitcaseAgeCode
            // 
            this.colSuitcaseAgeCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colSuitcaseAgeCode.DataPropertyName = "AgeCode";
            this.colSuitcaseAgeCode.HeaderText = "Age";
            this.colSuitcaseAgeCode.Name = "colSuitcaseAgeCode";
            this.colSuitcaseAgeCode.ReadOnly = true;
            this.colSuitcaseAgeCode.Visible = false;
            // 
            // colSuitcaseData
            // 
            this.colSuitcaseData.DataPropertyName = "Data";
            this.colSuitcaseData.HeaderText = "Data";
            this.colSuitcaseData.Name = "colSuitcaseData";
            this.colSuitcaseData.ReadOnly = true;
            this.colSuitcaseData.Visible = false;
            // 
            // colSuitcaseGenderHex
            // 
            this.colSuitcaseGenderHex.DataPropertyName = "GenderHex";
            this.colSuitcaseGenderHex.HeaderText = "Gender Hex";
            this.colSuitcaseGenderHex.Name = "colSuitcaseGenderHex";
            this.colSuitcaseGenderHex.ReadOnly = true;
            this.colSuitcaseGenderHex.Visible = false;
            // 
            // colSuitcaseAgeHex
            // 
            this.colSuitcaseAgeHex.DataPropertyName = "AgeHex";
            this.colSuitcaseAgeHex.HeaderText = "Age Hex";
            this.colSuitcaseAgeHex.Name = "colSuitcaseAgeHex";
            this.colSuitcaseAgeHex.ReadOnly = true;
            this.colSuitcaseAgeHex.Visible = false;
            // 
            // colSuitcaseThumbKey
            // 
            this.colSuitcaseThumbKey.DataPropertyName = "ThumbKey";
            this.colSuitcaseThumbKey.HeaderText = "ThumbKey";
            this.colSuitcaseThumbKey.Name = "colSuitcaseThumbKey";
            this.colSuitcaseThumbKey.ReadOnly = true;
            this.colSuitcaseThumbKey.Visible = false;
            // 
            // colSuitcaseLocalThumbKey
            // 
            this.colSuitcaseLocalThumbKey.DataPropertyName = "LocalThumbKey";
            this.colSuitcaseLocalThumbKey.HeaderText = "LocalThumbKey";
            this.colSuitcaseLocalThumbKey.Name = "colSuitcaseLocalThumbKey";
            this.colSuitcaseLocalThumbKey.ReadOnly = true;
            this.colSuitcaseLocalThumbKey.Visible = false;
            // 
            // menuContextSuitcase
            // 
            this.menuContextSuitcase.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuContextSuitcaseCopyToCloset,
            this.menuContextSuitcaseMoveToCloset,
            this.toolStripSeparator9,
            this.menuContextSuitcaseDelete});
            this.menuContextSuitcase.Name = "menuContextSuitcase";
            this.menuContextSuitcase.Size = new System.Drawing.Size(155, 76);
            this.menuContextSuitcase.Opening += new System.ComponentModel.CancelEventHandler(this.OnContextSuitcaseOpening);
            // 
            // menuContextSuitcaseCopyToCloset
            // 
            this.menuContextSuitcaseCopyToCloset.Name = "menuContextSuitcaseCopyToCloset";
            this.menuContextSuitcaseCopyToCloset.Size = new System.Drawing.Size(154, 22);
            this.menuContextSuitcaseCopyToCloset.Text = "&Copy to Closet";
            this.menuContextSuitcaseCopyToCloset.Click += new System.EventHandler(this.OnCopyToClosetClicked);
            // 
            // menuContextSuitcaseMoveToCloset
            // 
            this.menuContextSuitcaseMoveToCloset.Name = "menuContextSuitcaseMoveToCloset";
            this.menuContextSuitcaseMoveToCloset.Size = new System.Drawing.Size(154, 22);
            this.menuContextSuitcaseMoveToCloset.Text = "&Move to Closet";
            this.menuContextSuitcaseMoveToCloset.Click += new System.EventHandler(this.OnMoveToClosetClicked);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(151, 6);
            // 
            // menuContextSuitcaseDelete
            // 
            this.menuContextSuitcaseDelete.Name = "menuContextSuitcaseDelete";
            this.menuContextSuitcaseDelete.Size = new System.Drawing.Size(154, 22);
            this.menuContextSuitcaseDelete.Text = "Delete Selected";
            this.menuContextSuitcaseDelete.Click += new System.EventHandler(this.OnDeleteFromSuitcaseClicked);
            // 
            // btnSuitcaseEmpty
            // 
            this.btnSuitcaseEmpty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSuitcaseEmpty.Location = new System.Drawing.Point(3, 251);
            this.btnSuitcaseEmpty.Name = "btnSuitcaseEmpty";
            this.btnSuitcaseEmpty.Size = new System.Drawing.Size(70, 26);
            this.btnSuitcaseEmpty.TabIndex = 29;
            this.btnSuitcaseEmpty.Text = "Empty";
            this.btnSuitcaseEmpty.UseVisualStyleBackColor = true;
            this.btnSuitcaseEmpty.Click += new System.EventHandler(this.OnEmptySuitcaseClicked);
            // 
            // btnSuitcaseSave
            // 
            this.btnSuitcaseSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSuitcaseSave.Location = new System.Drawing.Point(79, 251);
            this.btnSuitcaseSave.Name = "btnSuitcaseSave";
            this.btnSuitcaseSave.Size = new System.Drawing.Size(70, 26);
            this.btnSuitcaseSave.TabIndex = 32;
            this.btnSuitcaseSave.Text = "Save";
            this.btnSuitcaseSave.UseVisualStyleBackColor = true;
            this.btnSuitcaseSave.Click += new System.EventHandler(this.OnSaveSuitcaseClicked);
            // 
            // btnSuitcaseLoad
            // 
            this.btnSuitcaseLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSuitcaseLoad.Location = new System.Drawing.Point(155, 251);
            this.btnSuitcaseLoad.Name = "btnSuitcaseLoad";
            this.btnSuitcaseLoad.Size = new System.Drawing.Size(70, 26);
            this.btnSuitcaseLoad.TabIndex = 33;
            this.btnSuitcaseLoad.Text = "Load";
            this.btnSuitcaseLoad.UseVisualStyleBackColor = true;
            this.btnSuitcaseLoad.Click += new System.EventHandler(this.OnLoadSuitcaseClicked);
            // 
            // btnSuitcaseCopy
            // 
            this.btnSuitcaseCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSuitcaseCopy.Location = new System.Drawing.Point(231, 251);
            this.btnSuitcaseCopy.Name = "btnSuitcaseCopy";
            this.btnSuitcaseCopy.Size = new System.Drawing.Size(70, 26);
            this.btnSuitcaseCopy.TabIndex = 30;
            this.btnSuitcaseCopy.Text = "Copy -->";
            this.btnSuitcaseCopy.UseVisualStyleBackColor = true;
            this.btnSuitcaseCopy.Click += new System.EventHandler(this.OnCopyToClosetClicked);
            // 
            // btnSuitcaseMove
            // 
            this.btnSuitcaseMove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSuitcaseMove.Location = new System.Drawing.Point(307, 251);
            this.btnSuitcaseMove.Name = "btnSuitcaseMove";
            this.btnSuitcaseMove.Size = new System.Drawing.Size(70, 26);
            this.btnSuitcaseMove.TabIndex = 31;
            this.btnSuitcaseMove.Text = "Move -->";
            this.btnSuitcaseMove.UseVisualStyleBackColor = true;
            this.btnSuitcaseMove.Click += new System.EventHandler(this.OnMoveToClosetClicked);
            // 
            // lblClosetCachesNeeded
            // 
            this.lblClosetCachesNeeded.AutoSize = true;
            this.lblClosetCachesNeeded.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblClosetCachesNeeded.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClosetCachesNeeded.ForeColor = System.Drawing.Color.Red;
            this.lblClosetCachesNeeded.Location = new System.Drawing.Point(0, 3);
            this.lblClosetCachesNeeded.Name = "lblClosetCachesNeeded";
            this.lblClosetCachesNeeded.Size = new System.Drawing.Size(501, 22);
            this.lblClosetCachesNeeded.TabIndex = 32;
            this.lblClosetCachesNeeded.Text = "You need to create the clothing caches before using the family closet!";
            // 
            // gridFamilyCloset
            // 
            this.gridFamilyCloset.AllowDrop = true;
            this.gridFamilyCloset.AllowUserToAddRows = false;
            this.gridFamilyCloset.AllowUserToDeleteRows = false;
            this.gridFamilyCloset.AllowUserToOrderColumns = true;
            this.gridFamilyCloset.AllowUserToResizeRows = false;
            this.gridFamilyCloset.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridFamilyCloset.BackgroundColor = System.Drawing.SystemColors.Window;
            this.gridFamilyCloset.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridFamilyCloset.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridFamilyCloset.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colClosetVisible,
            this.colClosetName,
            this.colClosetCategory,
            this.colClosetGender,
            this.colClosetGenderCode,
            this.colClosetAge,
            this.colClosetAgeCode,
            this.colClosetData,
            this.colClosetGenderHex,
            this.colClosetAgeHex,
            this.colClosetThumbKey,
            this.colClosetLocalThumbKey});
            this.gridFamilyCloset.ContextMenuStrip = this.menuContextCloset;
            this.gridFamilyCloset.Location = new System.Drawing.Point(0, 3);
            this.gridFamilyCloset.Name = "gridFamilyCloset";
            this.gridFamilyCloset.ReadOnly = true;
            this.gridFamilyCloset.RowHeadersVisible = false;
            this.gridFamilyCloset.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridFamilyCloset.Size = new System.Drawing.Size(773, 247);
            this.gridFamilyCloset.TabIndex = 1;
            this.gridFamilyCloset.MultiSelectChanged += new System.EventHandler(this.OnOutfitGridSelectionChanged);
            this.gridFamilyCloset.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellMouseEnter);
            this.gridFamilyCloset.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellMouseLeave);
            this.gridFamilyCloset.CellToolTipTextNeeded += new System.Windows.Forms.DataGridViewCellToolTipTextNeededEventHandler(this.OnToolTipTextNeeded);
            this.gridFamilyCloset.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.OnDataBindingComplete);
            this.gridFamilyCloset.SelectionChanged += new System.EventHandler(this.OnOutfitGridSelectionChanged);
            this.gridFamilyCloset.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnGridDragDrop);
            this.gridFamilyCloset.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnGridDragEnter);
            this.gridFamilyCloset.DragOver += new System.Windows.Forms.DragEventHandler(this.OnGridDragOver);
            this.gridFamilyCloset.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnGridMouseDown);
            // 
            // colClosetVisible
            // 
            this.colClosetVisible.DataPropertyName = "Visible";
            this.colClosetVisible.HeaderText = "Visible";
            this.colClosetVisible.Name = "colClosetVisible";
            this.colClosetVisible.ReadOnly = true;
            this.colClosetVisible.Visible = false;
            // 
            // colClosetName
            // 
            this.colClosetName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colClosetName.DataPropertyName = "Name";
            this.colClosetName.FillWeight = 300F;
            this.colClosetName.HeaderText = "Family Closet";
            this.colClosetName.Name = "colClosetName";
            this.colClosetName.ReadOnly = true;
            // 
            // colClosetCategory
            // 
            this.colClosetCategory.DataPropertyName = "Category";
            this.colClosetCategory.HeaderText = "Category";
            this.colClosetCategory.Name = "colClosetCategory";
            this.colClosetCategory.ReadOnly = true;
            // 
            // colClosetGender
            // 
            this.colClosetGender.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colClosetGender.DataPropertyName = "Gender";
            this.colClosetGender.FillWeight = 75F;
            this.colClosetGender.HeaderText = "Gender";
            this.colClosetGender.Name = "colClosetGender";
            this.colClosetGender.ReadOnly = true;
            this.colClosetGender.Width = 73;
            // 
            // colClosetGenderCode
            // 
            this.colClosetGenderCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colClosetGenderCode.DataPropertyName = "GenderCode";
            this.colClosetGenderCode.HeaderText = "⚥";
            this.colClosetGenderCode.Name = "colClosetGenderCode";
            this.colClosetGenderCode.ReadOnly = true;
            this.colClosetGenderCode.Visible = false;
            // 
            // colClosetAge
            // 
            this.colClosetAge.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colClosetAge.DataPropertyName = "Age";
            this.colClosetAge.FillWeight = 55F;
            this.colClosetAge.HeaderText = "Age";
            this.colClosetAge.Name = "colClosetAge";
            this.colClosetAge.ReadOnly = true;
            this.colClosetAge.Width = 53;
            // 
            // colClosetAgeCode
            // 
            this.colClosetAgeCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colClosetAgeCode.DataPropertyName = "AgeCode";
            this.colClosetAgeCode.HeaderText = "Age";
            this.colClosetAgeCode.Name = "colClosetAgeCode";
            this.colClosetAgeCode.ReadOnly = true;
            this.colClosetAgeCode.Visible = false;
            // 
            // colClosetData
            // 
            this.colClosetData.DataPropertyName = "Data";
            this.colClosetData.HeaderText = "Closet Data";
            this.colClosetData.Name = "colClosetData";
            this.colClosetData.ReadOnly = true;
            this.colClosetData.Visible = false;
            // 
            // colClosetGenderHex
            // 
            this.colClosetGenderHex.DataPropertyName = "GenderHex";
            this.colClosetGenderHex.HeaderText = "Gender Hex";
            this.colClosetGenderHex.Name = "colClosetGenderHex";
            this.colClosetGenderHex.ReadOnly = true;
            this.colClosetGenderHex.Visible = false;
            // 
            // colClosetAgeHex
            // 
            this.colClosetAgeHex.DataPropertyName = "AgeHex";
            this.colClosetAgeHex.HeaderText = "Age Hex";
            this.colClosetAgeHex.Name = "colClosetAgeHex";
            this.colClosetAgeHex.ReadOnly = true;
            this.colClosetAgeHex.Visible = false;
            // 
            // colClosetThumbKey
            // 
            this.colClosetThumbKey.DataPropertyName = "ThumbKey";
            this.colClosetThumbKey.HeaderText = "ThumbKey";
            this.colClosetThumbKey.Name = "colClosetThumbKey";
            this.colClosetThumbKey.ReadOnly = true;
            this.colClosetThumbKey.Visible = false;
            // 
            // colClosetLocalThumbKey
            // 
            this.colClosetLocalThumbKey.DataPropertyName = "LocalThumbKey";
            this.colClosetLocalThumbKey.HeaderText = "LocalThumbKey";
            this.colClosetLocalThumbKey.Name = "colClosetLocalThumbKey";
            this.colClosetLocalThumbKey.ReadOnly = true;
            this.colClosetLocalThumbKey.Visible = false;
            // 
            // menuContextCloset
            // 
            this.menuContextCloset.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuContextClosetCopyToSuitcase,
            this.menuContextClosetMoveToSuitcase,
            this.toolStripSeparator10,
            this.menuContextClosetFilterAll,
            this.menuContextClosetFilterSelected,
            this.menuContextClosetFilterUnwearable,
            this.toolStripSeparator3,
            this.menuContextClosetDelete});
            this.menuContextCloset.Name = "menuContextCloset";
            this.menuContextCloset.Size = new System.Drawing.Size(223, 148);
            this.menuContextCloset.Opening += new System.ComponentModel.CancelEventHandler(this.OnContextClosetOpening);
            // 
            // menuContextClosetCopyToSuitcase
            // 
            this.menuContextClosetCopyToSuitcase.Name = "menuContextClosetCopyToSuitcase";
            this.menuContextClosetCopyToSuitcase.Size = new System.Drawing.Size(222, 22);
            this.menuContextClosetCopyToSuitcase.Text = "&Copy to Suitcase";
            this.menuContextClosetCopyToSuitcase.Click += new System.EventHandler(this.OnCopyToSuitcaseClicked);
            // 
            // menuContextClosetMoveToSuitcase
            // 
            this.menuContextClosetMoveToSuitcase.Name = "menuContextClosetMoveToSuitcase";
            this.menuContextClosetMoveToSuitcase.Size = new System.Drawing.Size(222, 22);
            this.menuContextClosetMoveToSuitcase.Text = "&Move to Suitcase";
            this.menuContextClosetMoveToSuitcase.Click += new System.EventHandler(this.OnMoveToSuitcaseClicked);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(219, 6);
            // 
            // menuContextClosetFilterAll
            // 
            this.menuContextClosetFilterAll.Name = "menuContextClosetFilterAll";
            this.menuContextClosetFilterAll.Size = new System.Drawing.Size(222, 22);
            this.menuContextClosetFilterAll.Text = "Show &All";
            this.menuContextClosetFilterAll.Click += new System.EventHandler(this.OnShowAllClicked);
            // 
            // menuContextClosetFilterSelected
            // 
            this.menuContextClosetFilterSelected.Name = "menuContextClosetFilterSelected";
            this.menuContextClosetFilterSelected.Size = new System.Drawing.Size(222, 22);
            this.menuContextClosetFilterSelected.Text = "Show only for &Selected Sims";
            this.menuContextClosetFilterSelected.Click += new System.EventHandler(this.OnShowSelectedSimsClicked);
            // 
            // menuContextClosetFilterUnwearable
            // 
            this.menuContextClosetFilterUnwearable.Name = "menuContextClosetFilterUnwearable";
            this.menuContextClosetFilterUnwearable.Size = new System.Drawing.Size(222, 22);
            this.menuContextClosetFilterUnwearable.Text = "Show only &Unwearable";
            this.menuContextClosetFilterUnwearable.Click += new System.EventHandler(this.OnShowUnwearableClicked);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(219, 6);
            // 
            // menuContextClosetDelete
            // 
            this.menuContextClosetDelete.Name = "menuContextClosetDelete";
            this.menuContextClosetDelete.Size = new System.Drawing.Size(222, 22);
            this.menuContextClosetDelete.Text = "Delete Selected";
            this.menuContextClosetDelete.Click += new System.EventHandler(this.OnDeleteFromClosetClicked);
            // 
            // btnClosetCopy
            // 
            this.btnClosetCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClosetCopy.Location = new System.Drawing.Point(0, 253);
            this.btnClosetCopy.Name = "btnClosetCopy";
            this.btnClosetCopy.Size = new System.Drawing.Size(88, 26);
            this.btnClosetCopy.TabIndex = 26;
            this.btnClosetCopy.Text = "<-- Copy";
            this.btnClosetCopy.UseVisualStyleBackColor = true;
            this.btnClosetCopy.Click += new System.EventHandler(this.OnCopyToSuitcaseClicked);
            // 
            // btnClosetMove
            // 
            this.btnClosetMove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClosetMove.Location = new System.Drawing.Point(94, 253);
            this.btnClosetMove.Name = "btnClosetMove";
            this.btnClosetMove.Size = new System.Drawing.Size(88, 26);
            this.btnClosetMove.TabIndex = 27;
            this.btnClosetMove.Text = "<-- Move";
            this.btnClosetMove.UseVisualStyleBackColor = true;
            this.btnClosetMove.Click += new System.EventHandler(this.OnMoveToSuitcaseClicked);
            // 
            // btnClosetDelete
            // 
            this.btnClosetDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClosetDelete.Location = new System.Drawing.Point(188, 253);
            this.btnClosetDelete.Name = "btnClosetDelete";
            this.btnClosetDelete.Size = new System.Drawing.Size(88, 26);
            this.btnClosetDelete.TabIndex = 28;
            this.btnClosetDelete.Text = "Delete";
            this.btnClosetDelete.UseVisualStyleBackColor = true;
            this.btnClosetDelete.Click += new System.EventHandler(this.OnDeleteFromClosetClicked);
            // 
            // btnClosetShowAll
            // 
            this.btnClosetShowAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClosetShowAll.Location = new System.Drawing.Point(292, 253);
            this.btnClosetShowAll.Name = "btnClosetShowAll";
            this.btnClosetShowAll.Size = new System.Drawing.Size(88, 25);
            this.btnClosetShowAll.TabIndex = 29;
            this.btnClosetShowAll.Text = "Show All";
            this.btnClosetShowAll.UseVisualStyleBackColor = true;
            this.btnClosetShowAll.Click += new System.EventHandler(this.OnShowAllClicked);
            // 
            // tabSafe
            // 
            this.tabSafe.Controls.Add(this.splitSafeLeftRight);
            this.tabSafe.Location = new System.Drawing.Point(4, 4);
            this.tabSafe.Margin = new System.Windows.Forms.Padding(0);
            this.tabSafe.Name = "tabSafe";
            this.tabSafe.Size = new System.Drawing.Size(1276, 283);
            this.tabSafe.TabIndex = 2;
            this.tabSafe.Text = "Safe";
            this.tabSafe.UseVisualStyleBackColor = true;
            // 
            // splitSafeLeftRight
            // 
            this.splitSafeLeftRight.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitSafeLeftRight.Location = new System.Drawing.Point(-3, -3);
            this.splitSafeLeftRight.Name = "splitSafeLeftRight";
            // 
            // splitSafeLeftRight.Panel1
            // 
            this.splitSafeLeftRight.Panel1.Controls.Add(this.gridJewelbox);
            this.splitSafeLeftRight.Panel1.Controls.Add(this.btnJewelboxEmpty);
            this.splitSafeLeftRight.Panel1.Controls.Add(this.btnJewelboxSave);
            this.splitSafeLeftRight.Panel1.Controls.Add(this.btnJewelboxLoad);
            this.splitSafeLeftRight.Panel1.Controls.Add(this.btnJewelboxCopy);
            this.splitSafeLeftRight.Panel1.Controls.Add(this.btnJewelboxMove);
            this.splitSafeLeftRight.Panel1MinSize = 200;
            // 
            // splitSafeLeftRight.Panel2
            // 
            this.splitSafeLeftRight.Panel2.Controls.Add(this.lblSafeCachesNeeded);
            this.splitSafeLeftRight.Panel2.Controls.Add(this.gridFamilySafe);
            this.splitSafeLeftRight.Panel2.Controls.Add(this.btnSafeCopy);
            this.splitSafeLeftRight.Panel2.Controls.Add(this.btnSafeMove);
            this.splitSafeLeftRight.Panel2.Controls.Add(this.btnSafeDelete);
            this.splitSafeLeftRight.Panel2.Controls.Add(this.btnSafeShowAll);
            this.splitSafeLeftRight.Panel2MinSize = 300;
            this.splitSafeLeftRight.Size = new System.Drawing.Size(1280, 280);
            this.splitSafeLeftRight.SplitterDistance = 500;
            this.splitSafeLeftRight.TabIndex = 1;
            this.splitSafeLeftRight.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.OnSplitterMoved);
            // 
            // gridJewelbox
            // 
            this.gridJewelbox.AllowDrop = true;
            this.gridJewelbox.AllowUserToAddRows = false;
            this.gridJewelbox.AllowUserToDeleteRows = false;
            this.gridJewelbox.AllowUserToOrderColumns = true;
            this.gridJewelbox.AllowUserToResizeRows = false;
            this.gridJewelbox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridJewelbox.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridJewelbox.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.gridJewelbox.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridJewelbox.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colJewelboxVisible,
            this.colJewelboxName,
            this.colJewelboxCategory,
            this.colJewelboxGender,
            this.colJewelboxGenderCode,
            this.colJewelboxAge,
            this.colJewelboxAgeCode,
            this.colJewelboxData,
            this.colJewelboxGenderHex,
            this.colJewelboxAgeHex,
            this.colJewelboxThumbKey,
            this.colJewelboxLocalThumbKey});
            this.gridJewelbox.ContextMenuStrip = this.menuContextJewelbox;
            this.gridJewelbox.Location = new System.Drawing.Point(3, 3);
            this.gridJewelbox.Name = "gridJewelbox";
            this.gridJewelbox.ReadOnly = true;
            this.gridJewelbox.RowHeadersVisible = false;
            this.gridJewelbox.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridJewelbox.Size = new System.Drawing.Size(497, 245);
            this.gridJewelbox.TabIndex = 2;
            this.gridJewelbox.MultiSelectChanged += new System.EventHandler(this.OnOutfitGridSelectionChanged);
            this.gridJewelbox.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellMouseEnter);
            this.gridJewelbox.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellMouseLeave);
            this.gridJewelbox.CellToolTipTextNeeded += new System.Windows.Forms.DataGridViewCellToolTipTextNeededEventHandler(this.OnToolTipTextNeeded);
            this.gridJewelbox.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.OnDataBindingComplete);
            this.gridJewelbox.SelectionChanged += new System.EventHandler(this.OnOutfitGridSelectionChanged);
            this.gridJewelbox.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnGridDragDrop);
            this.gridJewelbox.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnGridDragEnter);
            this.gridJewelbox.DragOver += new System.Windows.Forms.DragEventHandler(this.OnGridDragOver);
            this.gridJewelbox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnGridMouseDown);
            // 
            // colJewelboxVisible
            // 
            this.colJewelboxVisible.DataPropertyName = "Visible";
            this.colJewelboxVisible.HeaderText = "Visible";
            this.colJewelboxVisible.Name = "colJewelboxVisible";
            this.colJewelboxVisible.ReadOnly = true;
            this.colJewelboxVisible.Visible = false;
            // 
            // colJewelboxName
            // 
            this.colJewelboxName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colJewelboxName.DataPropertyName = "Name";
            this.colJewelboxName.FillWeight = 300F;
            this.colJewelboxName.HeaderText = "Jewellery Box";
            this.colJewelboxName.Name = "colJewelboxName";
            this.colJewelboxName.ReadOnly = true;
            // 
            // colJewelboxCategory
            // 
            this.colJewelboxCategory.DataPropertyName = "Category";
            this.colJewelboxCategory.HeaderText = "Category";
            this.colJewelboxCategory.Name = "colJewelboxCategory";
            this.colJewelboxCategory.ReadOnly = true;
            // 
            // colJewelboxGender
            // 
            this.colJewelboxGender.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colJewelboxGender.DataPropertyName = "Gender";
            this.colJewelboxGender.FillWeight = 75F;
            this.colJewelboxGender.HeaderText = "Gender";
            this.colJewelboxGender.Name = "colJewelboxGender";
            this.colJewelboxGender.ReadOnly = true;
            this.colJewelboxGender.Width = 73;
            // 
            // colJewelboxGenderCode
            // 
            this.colJewelboxGenderCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colJewelboxGenderCode.DataPropertyName = "GenderCode";
            this.colJewelboxGenderCode.HeaderText = "⚥";
            this.colJewelboxGenderCode.Name = "colJewelboxGenderCode";
            this.colJewelboxGenderCode.ReadOnly = true;
            this.colJewelboxGenderCode.Visible = false;
            // 
            // colJewelboxAge
            // 
            this.colJewelboxAge.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colJewelboxAge.DataPropertyName = "Age";
            this.colJewelboxAge.FillWeight = 55F;
            this.colJewelboxAge.HeaderText = "Age";
            this.colJewelboxAge.Name = "colJewelboxAge";
            this.colJewelboxAge.ReadOnly = true;
            this.colJewelboxAge.Width = 53;
            // 
            // colJewelboxAgeCode
            // 
            this.colJewelboxAgeCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colJewelboxAgeCode.DataPropertyName = "AgeCode";
            this.colJewelboxAgeCode.HeaderText = "Age";
            this.colJewelboxAgeCode.Name = "colJewelboxAgeCode";
            this.colJewelboxAgeCode.ReadOnly = true;
            this.colJewelboxAgeCode.Visible = false;
            // 
            // colJewelboxData
            // 
            this.colJewelboxData.DataPropertyName = "Data";
            this.colJewelboxData.HeaderText = "Data";
            this.colJewelboxData.Name = "colJewelboxData";
            this.colJewelboxData.ReadOnly = true;
            this.colJewelboxData.Visible = false;
            // 
            // colJewelboxGenderHex
            // 
            this.colJewelboxGenderHex.DataPropertyName = "GenderHex";
            this.colJewelboxGenderHex.HeaderText = "Gender Hex";
            this.colJewelboxGenderHex.Name = "colJewelboxGenderHex";
            this.colJewelboxGenderHex.ReadOnly = true;
            this.colJewelboxGenderHex.Visible = false;
            // 
            // colJewelboxAgeHex
            // 
            this.colJewelboxAgeHex.DataPropertyName = "AgeHex";
            this.colJewelboxAgeHex.HeaderText = "Age Hex";
            this.colJewelboxAgeHex.Name = "colJewelboxAgeHex";
            this.colJewelboxAgeHex.ReadOnly = true;
            this.colJewelboxAgeHex.Visible = false;
            // 
            // colJewelboxThumbKey
            // 
            this.colJewelboxThumbKey.DataPropertyName = "ThumbKey";
            this.colJewelboxThumbKey.HeaderText = "ThumbKey";
            this.colJewelboxThumbKey.Name = "colJewelboxThumbKey";
            this.colJewelboxThumbKey.ReadOnly = true;
            this.colJewelboxThumbKey.Visible = false;
            // 
            // colJewelboxLocalThumbKey
            // 
            this.colJewelboxLocalThumbKey.DataPropertyName = "LocalThumbKey";
            this.colJewelboxLocalThumbKey.HeaderText = "LocalThumbKey";
            this.colJewelboxLocalThumbKey.Name = "colJewelboxLocalThumbKey";
            this.colJewelboxLocalThumbKey.ReadOnly = true;
            this.colJewelboxLocalThumbKey.Visible = false;
            // 
            // menuContextJewelbox
            // 
            this.menuContextJewelbox.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuContextJewelboxCopyToSafe,
            this.menuContextJewelboxMoveToSafe,
            this.toolStripSeparator12,
            this.menuContextJewelboxDelete});
            this.menuContextJewelbox.Name = "menuContextJewelbox";
            this.menuContextJewelbox.Size = new System.Drawing.Size(155, 76);
            this.menuContextJewelbox.Opening += new System.ComponentModel.CancelEventHandler(this.OnContextJewelboxOpening);
            // 
            // menuContextJewelboxCopyToSafe
            // 
            this.menuContextJewelboxCopyToSafe.Name = "menuContextJewelboxCopyToSafe";
            this.menuContextJewelboxCopyToSafe.Size = new System.Drawing.Size(154, 22);
            this.menuContextJewelboxCopyToSafe.Text = "&Copy to Safe";
            this.menuContextJewelboxCopyToSafe.Click += new System.EventHandler(this.OnCopyToSafeClicked);
            // 
            // menuContextJewelboxMoveToSafe
            // 
            this.menuContextJewelboxMoveToSafe.Name = "menuContextJewelboxMoveToSafe";
            this.menuContextJewelboxMoveToSafe.Size = new System.Drawing.Size(154, 22);
            this.menuContextJewelboxMoveToSafe.Text = "&Move to Safe";
            this.menuContextJewelboxMoveToSafe.Click += new System.EventHandler(this.OnMoveToSafeClicked);
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size(151, 6);
            // 
            // menuContextJewelboxDelete
            // 
            this.menuContextJewelboxDelete.Name = "menuContextJewelboxDelete";
            this.menuContextJewelboxDelete.Size = new System.Drawing.Size(154, 22);
            this.menuContextJewelboxDelete.Text = "Delete Selected";
            this.menuContextJewelboxDelete.Click += new System.EventHandler(this.OnDeleteFromJewelboxClicked);
            // 
            // btnJewelboxEmpty
            // 
            this.btnJewelboxEmpty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnJewelboxEmpty.Location = new System.Drawing.Point(3, 251);
            this.btnJewelboxEmpty.Name = "btnJewelboxEmpty";
            this.btnJewelboxEmpty.Size = new System.Drawing.Size(70, 26);
            this.btnJewelboxEmpty.TabIndex = 29;
            this.btnJewelboxEmpty.Text = "Empty";
            this.btnJewelboxEmpty.UseVisualStyleBackColor = true;
            this.btnJewelboxEmpty.Click += new System.EventHandler(this.OnEmptyJewelboxClicked);
            // 
            // btnJewelboxSave
            // 
            this.btnJewelboxSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnJewelboxSave.Location = new System.Drawing.Point(79, 251);
            this.btnJewelboxSave.Name = "btnJewelboxSave";
            this.btnJewelboxSave.Size = new System.Drawing.Size(70, 26);
            this.btnJewelboxSave.TabIndex = 32;
            this.btnJewelboxSave.Text = "Save";
            this.btnJewelboxSave.UseVisualStyleBackColor = true;
            this.btnJewelboxSave.Click += new System.EventHandler(this.OnSaveJewelboxClicked);
            // 
            // btnJewelboxLoad
            // 
            this.btnJewelboxLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnJewelboxLoad.Location = new System.Drawing.Point(155, 251);
            this.btnJewelboxLoad.Name = "btnJewelboxLoad";
            this.btnJewelboxLoad.Size = new System.Drawing.Size(70, 26);
            this.btnJewelboxLoad.TabIndex = 33;
            this.btnJewelboxLoad.Text = "Load";
            this.btnJewelboxLoad.UseVisualStyleBackColor = true;
            this.btnJewelboxLoad.Click += new System.EventHandler(this.OnLoadJewelboxClicked);
            // 
            // btnJewelboxCopy
            // 
            this.btnJewelboxCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnJewelboxCopy.Location = new System.Drawing.Point(231, 251);
            this.btnJewelboxCopy.Name = "btnJewelboxCopy";
            this.btnJewelboxCopy.Size = new System.Drawing.Size(70, 26);
            this.btnJewelboxCopy.TabIndex = 30;
            this.btnJewelboxCopy.Text = "Copy -->";
            this.btnJewelboxCopy.UseVisualStyleBackColor = true;
            this.btnJewelboxCopy.Click += new System.EventHandler(this.OnCopyToSafeClicked);
            // 
            // btnJewelboxMove
            // 
            this.btnJewelboxMove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnJewelboxMove.Location = new System.Drawing.Point(307, 251);
            this.btnJewelboxMove.Name = "btnJewelboxMove";
            this.btnJewelboxMove.Size = new System.Drawing.Size(70, 26);
            this.btnJewelboxMove.TabIndex = 31;
            this.btnJewelboxMove.Text = "Move -->";
            this.btnJewelboxMove.UseVisualStyleBackColor = true;
            this.btnJewelboxMove.Click += new System.EventHandler(this.OnMoveToSafeClicked);
            // 
            // lblSafeCachesNeeded
            // 
            this.lblSafeCachesNeeded.AutoSize = true;
            this.lblSafeCachesNeeded.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSafeCachesNeeded.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSafeCachesNeeded.ForeColor = System.Drawing.Color.Red;
            this.lblSafeCachesNeeded.Location = new System.Drawing.Point(0, 3);
            this.lblSafeCachesNeeded.Name = "lblSafeCachesNeeded";
            this.lblSafeCachesNeeded.Size = new System.Drawing.Size(494, 22);
            this.lblSafeCachesNeeded.TabIndex = 32;
            this.lblSafeCachesNeeded.Text = "You need to create the jewellery caches before using the family safe!";
            // 
            // gridFamilySafe
            // 
            this.gridFamilySafe.AllowDrop = true;
            this.gridFamilySafe.AllowUserToAddRows = false;
            this.gridFamilySafe.AllowUserToDeleteRows = false;
            this.gridFamilySafe.AllowUserToOrderColumns = true;
            this.gridFamilySafe.AllowUserToResizeRows = false;
            this.gridFamilySafe.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridFamilySafe.BackgroundColor = System.Drawing.SystemColors.Window;
            this.gridFamilySafe.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.gridFamilySafe.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridFamilySafe.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSafeVisible,
            this.colSafeName,
            this.colSafeCategory,
            this.colSafeGender,
            this.colSafeGenderCode,
            this.colSafeAge,
            this.colSafeAgeCode,
            this.colSafeData,
            this.colSafeGenderHex,
            this.colSafeAgeHex,
            this.colSafeThumbKey,
            this.colSafeLocalThumbKey});
            this.gridFamilySafe.ContextMenuStrip = this.menuContextSafe;
            this.gridFamilySafe.Location = new System.Drawing.Point(0, 3);
            this.gridFamilySafe.Name = "gridFamilySafe";
            this.gridFamilySafe.ReadOnly = true;
            this.gridFamilySafe.RowHeadersVisible = false;
            this.gridFamilySafe.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridFamilySafe.Size = new System.Drawing.Size(773, 245);
            this.gridFamilySafe.TabIndex = 1;
            this.gridFamilySafe.MultiSelectChanged += new System.EventHandler(this.OnOutfitGridSelectionChanged);
            this.gridFamilySafe.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellMouseEnter);
            this.gridFamilySafe.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellMouseLeave);
            this.gridFamilySafe.CellToolTipTextNeeded += new System.Windows.Forms.DataGridViewCellToolTipTextNeededEventHandler(this.OnToolTipTextNeeded);
            this.gridFamilySafe.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.OnDataBindingComplete);
            this.gridFamilySafe.SelectionChanged += new System.EventHandler(this.OnOutfitGridSelectionChanged);
            this.gridFamilySafe.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnGridDragDrop);
            this.gridFamilySafe.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnGridDragEnter);
            this.gridFamilySafe.DragOver += new System.Windows.Forms.DragEventHandler(this.OnGridDragOver);
            this.gridFamilySafe.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnGridMouseDown);
            // 
            // colSafeVisible
            // 
            this.colSafeVisible.DataPropertyName = "Visible";
            this.colSafeVisible.HeaderText = "Visible";
            this.colSafeVisible.Name = "colSafeVisible";
            this.colSafeVisible.ReadOnly = true;
            this.colSafeVisible.Visible = false;
            // 
            // colSafeName
            // 
            this.colSafeName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colSafeName.DataPropertyName = "Name";
            this.colSafeName.FillWeight = 300F;
            this.colSafeName.HeaderText = "Family Safe";
            this.colSafeName.Name = "colSafeName";
            this.colSafeName.ReadOnly = true;
            // 
            // colSafeCategory
            // 
            this.colSafeCategory.DataPropertyName = "Category";
            this.colSafeCategory.HeaderText = "Category";
            this.colSafeCategory.Name = "colSafeCategory";
            this.colSafeCategory.ReadOnly = true;
            // 
            // colSafeGender
            // 
            this.colSafeGender.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colSafeGender.DataPropertyName = "Gender";
            this.colSafeGender.FillWeight = 75F;
            this.colSafeGender.HeaderText = "Gender";
            this.colSafeGender.Name = "colSafeGender";
            this.colSafeGender.ReadOnly = true;
            this.colSafeGender.Width = 73;
            // 
            // colSafeGenderCode
            // 
            this.colSafeGenderCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colSafeGenderCode.DataPropertyName = "GenderCode";
            this.colSafeGenderCode.HeaderText = "⚥";
            this.colSafeGenderCode.Name = "colSafeGenderCode";
            this.colSafeGenderCode.ReadOnly = true;
            this.colSafeGenderCode.Visible = false;
            // 
            // colSafeAge
            // 
            this.colSafeAge.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colSafeAge.DataPropertyName = "Age";
            this.colSafeAge.FillWeight = 55F;
            this.colSafeAge.HeaderText = "Age";
            this.colSafeAge.Name = "colSafeAge";
            this.colSafeAge.ReadOnly = true;
            this.colSafeAge.Width = 53;
            // 
            // colSafeAgeCode
            // 
            this.colSafeAgeCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colSafeAgeCode.DataPropertyName = "AgeCode";
            this.colSafeAgeCode.HeaderText = "Age";
            this.colSafeAgeCode.Name = "colSafeAgeCode";
            this.colSafeAgeCode.ReadOnly = true;
            this.colSafeAgeCode.Visible = false;
            // 
            // colSafeData
            // 
            this.colSafeData.DataPropertyName = "Data";
            this.colSafeData.HeaderText = "Closet Data";
            this.colSafeData.Name = "colSafeData";
            this.colSafeData.ReadOnly = true;
            this.colSafeData.Visible = false;
            // 
            // colSafeGenderHex
            // 
            this.colSafeGenderHex.DataPropertyName = "GenderHex";
            this.colSafeGenderHex.HeaderText = "Gender Hex";
            this.colSafeGenderHex.Name = "colSafeGenderHex";
            this.colSafeGenderHex.ReadOnly = true;
            this.colSafeGenderHex.Visible = false;
            // 
            // colSafeAgeHex
            // 
            this.colSafeAgeHex.DataPropertyName = "AgeHex";
            this.colSafeAgeHex.HeaderText = "Age Hex";
            this.colSafeAgeHex.Name = "colSafeAgeHex";
            this.colSafeAgeHex.ReadOnly = true;
            this.colSafeAgeHex.Visible = false;
            // 
            // colSafeThumbKey
            // 
            this.colSafeThumbKey.DataPropertyName = "ThumbKey";
            this.colSafeThumbKey.HeaderText = "ThumbKey";
            this.colSafeThumbKey.Name = "colSafeThumbKey";
            this.colSafeThumbKey.ReadOnly = true;
            this.colSafeThumbKey.Visible = false;
            // 
            // colSafeLocalThumbKey
            // 
            this.colSafeLocalThumbKey.DataPropertyName = "LocalThumbKey";
            this.colSafeLocalThumbKey.HeaderText = "LocalThumbKey";
            this.colSafeLocalThumbKey.Name = "colSafeLocalThumbKey";
            this.colSafeLocalThumbKey.ReadOnly = true;
            this.colSafeLocalThumbKey.Visible = false;
            // 
            // menuContextSafe
            // 
            this.menuContextSafe.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuContextSafeCopyToJewelbox,
            this.menuContextSafeMoveToJewelbox,
            this.toolStripSeparator8,
            this.menuContextSafeFilterAll,
            this.menuContextSafeFilterSelected,
            this.menuContextSafeFilterUnwearable,
            this.toolStripSeparator11,
            this.menuContextSafeDelete});
            this.menuContextSafe.Name = "menuContextSafe";
            this.menuContextSafe.Size = new System.Drawing.Size(223, 148);
            this.menuContextSafe.Opening += new System.ComponentModel.CancelEventHandler(this.OnContextSafeOpening);
            // 
            // menuContextSafeCopyToJewelbox
            // 
            this.menuContextSafeCopyToJewelbox.Name = "menuContextSafeCopyToJewelbox";
            this.menuContextSafeCopyToJewelbox.Size = new System.Drawing.Size(222, 22);
            this.menuContextSafeCopyToJewelbox.Text = "&Copy to Jewellery Box";
            this.menuContextSafeCopyToJewelbox.Click += new System.EventHandler(this.OnCopyToJewelboxClicked);
            // 
            // menuContextSafeMoveToJewelbox
            // 
            this.menuContextSafeMoveToJewelbox.Name = "menuContextSafeMoveToJewelbox";
            this.menuContextSafeMoveToJewelbox.Size = new System.Drawing.Size(222, 22);
            this.menuContextSafeMoveToJewelbox.Text = "&Move to Jewellery Box";
            this.menuContextSafeMoveToJewelbox.Click += new System.EventHandler(this.OnMoveToJewelboxClicked);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(219, 6);
            // 
            // menuContextSafeFilterAll
            // 
            this.menuContextSafeFilterAll.Name = "menuContextSafeFilterAll";
            this.menuContextSafeFilterAll.Size = new System.Drawing.Size(222, 22);
            this.menuContextSafeFilterAll.Text = "Show &All";
            this.menuContextSafeFilterAll.Click += new System.EventHandler(this.OnShowAllClicked);
            // 
            // menuContextSafeFilterSelected
            // 
            this.menuContextSafeFilterSelected.Name = "menuContextSafeFilterSelected";
            this.menuContextSafeFilterSelected.Size = new System.Drawing.Size(222, 22);
            this.menuContextSafeFilterSelected.Text = "Show only for &Selected Sims";
            this.menuContextSafeFilterSelected.Click += new System.EventHandler(this.OnShowSelectedSimsClicked);
            // 
            // menuContextSafeFilterUnwearable
            // 
            this.menuContextSafeFilterUnwearable.Name = "menuContextSafeFilterUnwearable";
            this.menuContextSafeFilterUnwearable.Size = new System.Drawing.Size(222, 22);
            this.menuContextSafeFilterUnwearable.Text = "Show only &Unwearable";
            this.menuContextSafeFilterUnwearable.Click += new System.EventHandler(this.OnShowUnwearableClicked);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(219, 6);
            // 
            // menuContextSafeDelete
            // 
            this.menuContextSafeDelete.Name = "menuContextSafeDelete";
            this.menuContextSafeDelete.Size = new System.Drawing.Size(222, 22);
            this.menuContextSafeDelete.Text = "Delete Selected";
            this.menuContextSafeDelete.Click += new System.EventHandler(this.OnDeleteFromSafeClicked);
            // 
            // btnSafeCopy
            // 
            this.btnSafeCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSafeCopy.Location = new System.Drawing.Point(0, 251);
            this.btnSafeCopy.Name = "btnSafeCopy";
            this.btnSafeCopy.Size = new System.Drawing.Size(88, 26);
            this.btnSafeCopy.TabIndex = 26;
            this.btnSafeCopy.Text = "<-- Copy";
            this.btnSafeCopy.UseVisualStyleBackColor = true;
            this.btnSafeCopy.Click += new System.EventHandler(this.OnCopyToJewelboxClicked);
            // 
            // btnSafeMove
            // 
            this.btnSafeMove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSafeMove.Location = new System.Drawing.Point(94, 251);
            this.btnSafeMove.Name = "btnSafeMove";
            this.btnSafeMove.Size = new System.Drawing.Size(88, 26);
            this.btnSafeMove.TabIndex = 27;
            this.btnSafeMove.Text = "<-- Move";
            this.btnSafeMove.UseVisualStyleBackColor = true;
            this.btnSafeMove.Click += new System.EventHandler(this.OnMoveToJewelboxClicked);
            // 
            // btnSafeDelete
            // 
            this.btnSafeDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSafeDelete.Location = new System.Drawing.Point(188, 251);
            this.btnSafeDelete.Name = "btnSafeDelete";
            this.btnSafeDelete.Size = new System.Drawing.Size(88, 26);
            this.btnSafeDelete.TabIndex = 28;
            this.btnSafeDelete.Text = "Delete";
            this.btnSafeDelete.UseVisualStyleBackColor = true;
            this.btnSafeDelete.Click += new System.EventHandler(this.OnDeleteFromSafeClicked);
            // 
            // btnSafeShowAll
            // 
            this.btnSafeShowAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSafeShowAll.Location = new System.Drawing.Point(292, 251);
            this.btnSafeShowAll.Name = "btnSafeShowAll";
            this.btnSafeShowAll.Size = new System.Drawing.Size(88, 26);
            this.btnSafeShowAll.TabIndex = 29;
            this.btnSafeShowAll.Text = "Show All";
            this.btnSafeShowAll.UseVisualStyleBackColor = true;
            this.btnSafeShowAll.Click += new System.EventHandler(this.OnShowAllClicked);
            // 
            // tabCareer
            // 
            this.tabCareer.Controls.Add(this.imageSim);
            this.tabCareer.Controls.Add(this.grpJob);
            this.tabCareer.Controls.Add(this.grpUniversity);
            this.tabCareer.Controls.Add(this.grpSchool);
            this.tabCareer.Location = new System.Drawing.Point(4, 4);
            this.tabCareer.Margin = new System.Windows.Forms.Padding(0);
            this.tabCareer.Name = "tabCareer";
            this.tabCareer.Size = new System.Drawing.Size(1276, 283);
            this.tabCareer.TabIndex = 3;
            this.tabCareer.Text = "Career";
            this.tabCareer.UseVisualStyleBackColor = true;
            // 
            // imageSim
            // 
            this.imageSim.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.imageSim.Location = new System.Drawing.Point(1085, 15);
            this.imageSim.Name = "imageSim";
            this.imageSim.Size = new System.Drawing.Size(192, 192);
            this.imageSim.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imageSim.TabIndex = 3;
            this.imageSim.TabStop = false;
            // 
            // grpJob
            // 
            this.grpJob.Controls.Add(this.lblJobPTOSummary);
            this.grpJob.Controls.Add(this.textJobRetiredGUID);
            this.grpJob.Controls.Add(this.textJobRetiredLevel);
            this.grpJob.Controls.Add(this.trackJobRetiredLevel);
            this.grpJob.Controls.Add(this.lblJobRetiredLevel);
            this.grpJob.Controls.Add(this.lblJobRetiredType);
            this.grpJob.Controls.Add(this.comboJobRetiredType);
            this.grpJob.Controls.Add(this.textJobGUID);
            this.grpJob.Controls.Add(this.textJobLevel);
            this.grpJob.Controls.Add(this.textJobPerformance);
            this.grpJob.Controls.Add(this.textJobPTO);
            this.grpJob.Controls.Add(this.textJobPension);
            this.grpJob.Controls.Add(this.lblJobPension);
            this.grpJob.Controls.Add(this.trackJobPerformance);
            this.grpJob.Controls.Add(this.trackJobLevel);
            this.grpJob.Controls.Add(this.lblJobPTO);
            this.grpJob.Controls.Add(this.lblJobPerformance);
            this.grpJob.Controls.Add(this.lblJobLevel);
            this.grpJob.Controls.Add(this.lblJobType);
            this.grpJob.Controls.Add(this.comboJobType);
            this.grpJob.Location = new System.Drawing.Point(715, 8);
            this.grpJob.Name = "grpJob";
            this.grpJob.Size = new System.Drawing.Size(360, 265);
            this.grpJob.TabIndex = 2;
            this.grpJob.TabStop = false;
            this.grpJob.Text = "Job";
            // 
            // lblJobPTOSummary
            // 
            this.lblJobPTOSummary.AutoSize = true;
            this.lblJobPTOSummary.Location = new System.Drawing.Point(176, 127);
            this.lblJobPTOSummary.Name = "lblJobPTOSummary";
            this.lblJobPTOSummary.Size = new System.Drawing.Size(67, 15);
            this.lblJobPTOSummary.TabIndex = 29;
            this.lblJobPTOSummary.Text = "(1.70 days)";
            // 
            // textJobRetiredGUID
            // 
            this.textJobRetiredGUID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textJobRetiredGUID.Location = new System.Drawing.Point(266, 191);
            this.textJobRetiredGUID.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.textJobRetiredGUID.Name = "textJobRetiredGUID";
            this.textJobRetiredGUID.Size = new System.Drawing.Size(85, 21);
            this.textJobRetiredGUID.TabIndex = 28;
            this.textJobRetiredGUID.Value = ((uint)(2408550287u));
            this.textJobRetiredGUID.TextChanged += new System.EventHandler(this.OnJobRetiredGuidChanged);
            // 
            // textJobRetiredLevel
            // 
            this.textJobRetiredLevel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textJobRetiredLevel.Location = new System.Drawing.Point(266, 224);
            this.textJobRetiredLevel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.textJobRetiredLevel.Maximum = ((uint)(10u));
            this.textJobRetiredLevel.Minimum = ((uint)(0u));
            this.textJobRetiredLevel.Name = "textJobRetiredLevel";
            this.textJobRetiredLevel.Size = new System.Drawing.Size(50, 21);
            this.textJobRetiredLevel.TabIndex = 27;
            this.textJobRetiredLevel.Value = ((uint)(6u));
            this.textJobRetiredLevel.TextChanged += new System.EventHandler(this.OnJobRetiredLevelValueChanged);
            // 
            // trackJobRetiredLevel
            // 
            this.trackJobRetiredLevel.BackColor = System.Drawing.Color.Transparent;
            this.trackJobRetiredLevel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.trackJobRetiredLevel.Gradient = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.trackJobRetiredLevel.GradientEndColor = System.Drawing.Color.White;
            this.trackJobRetiredLevel.GradientStartColor = System.Drawing.Color.White;
            this.trackJobRetiredLevel.Location = new System.Drawing.Point(90, 225);
            this.trackJobRetiredLevel.Maximum = 10;
            this.trackJobRetiredLevel.Minimum = 0;
            this.trackJobRetiredLevel.Name = "trackJobRetiredLevel";
            this.trackJobRetiredLevel.NegativeBalanceColour = System.Drawing.Color.Crimson;
            this.trackJobRetiredLevel.PositiveBalanceColour = System.Drawing.Color.YellowGreen;
            this.trackJobRetiredLevel.ProgressBackColor = System.Drawing.SystemColors.Window;
            this.trackJobRetiredLevel.Quality = true;
            this.trackJobRetiredLevel.SelectedColor = System.Drawing.Color.YellowGreen;
            this.trackJobRetiredLevel.Size = new System.Drawing.Size(170, 20);
            this.trackJobRetiredLevel.Style = Sims2Tools.Controls.SimsTrackingBarStyle.Increase;
            this.trackJobRetiredLevel.TabIndex = 23;
            this.trackJobRetiredLevel.TokenCount = 10;
            this.trackJobRetiredLevel.UnselectedColor = System.Drawing.Color.Black;
            this.trackJobRetiredLevel.UseTokenBuffer = true;
            this.trackJobRetiredLevel.Value = 6;
            this.trackJobRetiredLevel.Changed += new System.EventHandler(this.OnJobRetiredLevelSliderChanged);
            // 
            // lblJobRetiredLevel
            // 
            this.lblJobRetiredLevel.AutoSize = true;
            this.lblJobRetiredLevel.Location = new System.Drawing.Point(2, 230);
            this.lblJobRetiredLevel.Name = "lblJobRetiredLevel";
            this.lblJobRetiredLevel.Size = new System.Drawing.Size(82, 15);
            this.lblJobRetiredLevel.TabIndex = 26;
            this.lblJobRetiredLevel.Text = "Retired Level:";
            // 
            // lblJobRetiredType
            // 
            this.lblJobRetiredType.AutoSize = true;
            this.lblJobRetiredType.Location = new System.Drawing.Point(5, 193);
            this.lblJobRetiredType.Name = "lblJobRetiredType";
            this.lblJobRetiredType.Size = new System.Drawing.Size(79, 15);
            this.lblJobRetiredType.TabIndex = 25;
            this.lblJobRetiredType.Text = "Retired Type:";
            // 
            // comboJobRetiredType
            // 
            this.comboJobRetiredType.FormattingEnabled = true;
            this.comboJobRetiredType.Location = new System.Drawing.Point(89, 190);
            this.comboJobRetiredType.Name = "comboJobRetiredType";
            this.comboJobRetiredType.Size = new System.Drawing.Size(170, 23);
            this.comboJobRetiredType.TabIndex = 24;
            this.comboJobRetiredType.Text = "Unknown";
            this.comboJobRetiredType.SelectedIndexChanged += new System.EventHandler(this.OnJobRetiredTypeChanged);
            // 
            // textJobGUID
            // 
            this.textJobGUID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textJobGUID.Location = new System.Drawing.Point(266, 26);
            this.textJobGUID.Name = "textJobGUID";
            this.textJobGUID.Size = new System.Drawing.Size(85, 21);
            this.textJobGUID.TabIndex = 22;
            this.textJobGUID.Value = ((uint)(2408550287u));
            this.textJobGUID.TextChanged += new System.EventHandler(this.OnJobGuidChanged);
            // 
            // textJobLevel
            // 
            this.textJobLevel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textJobLevel.Location = new System.Drawing.Point(266, 59);
            this.textJobLevel.Maximum = ((uint)(10u));
            this.textJobLevel.Minimum = ((uint)(0u));
            this.textJobLevel.Name = "textJobLevel";
            this.textJobLevel.Size = new System.Drawing.Size(50, 21);
            this.textJobLevel.TabIndex = 21;
            this.textJobLevel.Value = ((uint)(6u));
            this.textJobLevel.TextChanged += new System.EventHandler(this.OnJobLevelValueChanged);
            // 
            // textJobPerformance
            // 
            this.textJobPerformance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textJobPerformance.Location = new System.Drawing.Point(266, 91);
            this.textJobPerformance.Maximum = 100;
            this.textJobPerformance.Minimum = -100;
            this.textJobPerformance.Name = "textJobPerformance";
            this.textJobPerformance.Size = new System.Drawing.Size(50, 21);
            this.textJobPerformance.TabIndex = 20;
            this.textJobPerformance.Value = 52;
            this.textJobPerformance.TextChanged += new System.EventHandler(this.OnJobPerformanceValueChanged);
            // 
            // textJobPTO
            // 
            this.textJobPTO.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textJobPTO.Location = new System.Drawing.Point(90, 124);
            this.textJobPTO.Maximum = ((uint)(32767u));
            this.textJobPTO.Minimum = ((uint)(0u));
            this.textJobPTO.Name = "textJobPTO";
            this.textJobPTO.Size = new System.Drawing.Size(80, 21);
            this.textJobPTO.TabIndex = 19;
            this.textJobPTO.Value = ((uint)(0u));
            this.textJobPTO.TextChanged += new System.EventHandler(this.OnJobPtoValueChanged);
            // 
            // textJobPension
            // 
            this.textJobPension.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textJobPension.Location = new System.Drawing.Point(90, 157);
            this.textJobPension.Maximum = ((uint)(32000u));
            this.textJobPension.Minimum = ((uint)(0u));
            this.textJobPension.Name = "textJobPension";
            this.textJobPension.Size = new System.Drawing.Size(80, 21);
            this.textJobPension.TabIndex = 18;
            this.textJobPension.Value = ((uint)(0u));
            this.textJobPension.TextChanged += new System.EventHandler(this.OnJobPensionValueChanged);
            // 
            // lblJobPension
            // 
            this.lblJobPension.AutoSize = true;
            this.lblJobPension.Location = new System.Drawing.Point(29, 160);
            this.lblJobPension.Name = "lblJobPension";
            this.lblJobPension.Size = new System.Drawing.Size(55, 15);
            this.lblJobPension.TabIndex = 16;
            this.lblJobPension.Text = "Pension:";
            // 
            // trackJobPerformance
            // 
            this.trackJobPerformance.BackColor = System.Drawing.Color.Transparent;
            this.trackJobPerformance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.trackJobPerformance.Gradient = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.trackJobPerformance.GradientEndColor = System.Drawing.Color.White;
            this.trackJobPerformance.GradientStartColor = System.Drawing.Color.White;
            this.trackJobPerformance.Location = new System.Drawing.Point(90, 92);
            this.trackJobPerformance.Maximum = 100;
            this.trackJobPerformance.Minimum = -100;
            this.trackJobPerformance.Name = "trackJobPerformance";
            this.trackJobPerformance.NegativeBalanceColour = System.Drawing.Color.Crimson;
            this.trackJobPerformance.PositiveBalanceColour = System.Drawing.Color.YellowGreen;
            this.trackJobPerformance.ProgressBackColor = System.Drawing.SystemColors.Window;
            this.trackJobPerformance.Quality = true;
            this.trackJobPerformance.SelectedColor = System.Drawing.Color.YellowGreen;
            this.trackJobPerformance.Size = new System.Drawing.Size(170, 20);
            this.trackJobPerformance.Style = Sims2Tools.Controls.SimsTrackingBarStyle.Balance;
            this.trackJobPerformance.TabIndex = 5;
            this.trackJobPerformance.TokenCount = 10;
            this.trackJobPerformance.UnselectedColor = System.Drawing.Color.Black;
            this.trackJobPerformance.UseTokenBuffer = true;
            this.trackJobPerformance.Value = 52;
            this.trackJobPerformance.Changed += new System.EventHandler(this.OnJobPerformanceSliderChanged);
            // 
            // trackJobLevel
            // 
            this.trackJobLevel.BackColor = System.Drawing.Color.Transparent;
            this.trackJobLevel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.trackJobLevel.Gradient = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.trackJobLevel.GradientEndColor = System.Drawing.Color.White;
            this.trackJobLevel.GradientStartColor = System.Drawing.Color.White;
            this.trackJobLevel.Location = new System.Drawing.Point(90, 60);
            this.trackJobLevel.Maximum = 10;
            this.trackJobLevel.Minimum = 0;
            this.trackJobLevel.Name = "trackJobLevel";
            this.trackJobLevel.NegativeBalanceColour = System.Drawing.Color.Crimson;
            this.trackJobLevel.PositiveBalanceColour = System.Drawing.Color.YellowGreen;
            this.trackJobLevel.ProgressBackColor = System.Drawing.SystemColors.Window;
            this.trackJobLevel.Quality = true;
            this.trackJobLevel.SelectedColor = System.Drawing.Color.YellowGreen;
            this.trackJobLevel.Size = new System.Drawing.Size(170, 20);
            this.trackJobLevel.Style = Sims2Tools.Controls.SimsTrackingBarStyle.Increase;
            this.trackJobLevel.TabIndex = 5;
            this.trackJobLevel.TokenCount = 10;
            this.trackJobLevel.UnselectedColor = System.Drawing.Color.Black;
            this.trackJobLevel.UseTokenBuffer = true;
            this.trackJobLevel.Value = 6;
            this.trackJobLevel.Changed += new System.EventHandler(this.OnJobLevelSliderChanged);
            // 
            // lblJobPTO
            // 
            this.lblJobPTO.AutoSize = true;
            this.lblJobPTO.Location = new System.Drawing.Point(50, 127);
            this.lblJobPTO.Name = "lblJobPTO";
            this.lblJobPTO.Size = new System.Drawing.Size(34, 15);
            this.lblJobPTO.TabIndex = 10;
            this.lblJobPTO.Text = "PTO:";
            // 
            // lblJobPerformance
            // 
            this.lblJobPerformance.AutoSize = true;
            this.lblJobPerformance.Location = new System.Drawing.Point(3, 97);
            this.lblJobPerformance.Name = "lblJobPerformance";
            this.lblJobPerformance.Size = new System.Drawing.Size(81, 15);
            this.lblJobPerformance.TabIndex = 9;
            this.lblJobPerformance.Text = "Performance:";
            // 
            // lblJobLevel
            // 
            this.lblJobLevel.AutoSize = true;
            this.lblJobLevel.Location = new System.Drawing.Point(45, 65);
            this.lblJobLevel.Name = "lblJobLevel";
            this.lblJobLevel.Size = new System.Drawing.Size(39, 15);
            this.lblJobLevel.TabIndex = 8;
            this.lblJobLevel.Text = "Level:";
            // 
            // lblJobType
            // 
            this.lblJobType.AutoSize = true;
            this.lblJobType.Location = new System.Drawing.Point(48, 28);
            this.lblJobType.Name = "lblJobType";
            this.lblJobType.Size = new System.Drawing.Size(36, 15);
            this.lblJobType.TabIndex = 6;
            this.lblJobType.Text = "Type:";
            // 
            // comboJobType
            // 
            this.comboJobType.FormattingEnabled = true;
            this.comboJobType.Location = new System.Drawing.Point(90, 25);
            this.comboJobType.Name = "comboJobType";
            this.comboJobType.Size = new System.Drawing.Size(170, 23);
            this.comboJobType.TabIndex = 5;
            this.comboJobType.Text = "Unknown";
            this.comboJobType.SelectedIndexChanged += new System.EventHandler(this.OnJobTypeChanged);
            // 
            // grpUniversity
            // 
            this.grpUniversity.Controls.Add(this.textUniGrade);
            this.grpUniversity.Controls.Add(this.textUniTimeLeft);
            this.grpUniversity.Controls.Add(this.textUniInfluence);
            this.grpUniversity.Controls.Add(this.textUniEffort);
            this.grpUniversity.Controls.Add(this.textMajorGUID);
            this.grpUniversity.Controls.Add(this.lblUniStudying);
            this.grpUniversity.Controls.Add(this.comboUniResult);
            this.grpUniversity.Controls.Add(this.lblUniResult);
            this.grpUniversity.Controls.Add(this.ckbUniStudying);
            this.grpUniversity.Controls.Add(this.trackUniTimeLeft);
            this.grpUniversity.Controls.Add(this.trackUniEffort);
            this.grpUniversity.Controls.Add(this.lblUniProbation);
            this.grpUniversity.Controls.Add(this.ckbUniProbation);
            this.grpUniversity.Controls.Add(this.trackUniGrade);
            this.grpUniversity.Controls.Add(this.lblUniInfluence);
            this.grpUniversity.Controls.Add(this.comboUniSemester);
            this.grpUniversity.Controls.Add(this.lblUniTimeLeft);
            this.grpUniversity.Controls.Add(this.lblUniGrade);
            this.grpUniversity.Controls.Add(this.lblUniSemester);
            this.grpUniversity.Controls.Add(this.comboUniMajor);
            this.grpUniversity.Controls.Add(this.lblUniEffort);
            this.grpUniversity.Controls.Add(this.lblUniMajor);
            this.grpUniversity.Location = new System.Drawing.Point(350, 8);
            this.grpUniversity.Name = "grpUniversity";
            this.grpUniversity.Size = new System.Drawing.Size(350, 265);
            this.grpUniversity.TabIndex = 1;
            this.grpUniversity.TabStop = false;
            this.grpUniversity.Text = "University";
            // 
            // textUniGrade
            // 
            this.textUniGrade.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textUniGrade.Format = "N1";
            this.textUniGrade.Location = new System.Drawing.Point(256, 95);
            this.textUniGrade.Maximum = 4D;
            this.textUniGrade.Minimum = 0D;
            this.textUniGrade.Name = "textUniGrade";
            this.textUniGrade.Size = new System.Drawing.Size(50, 21);
            this.textUniGrade.TabIndex = 23;
            this.textUniGrade.Value = 3.4D;
            this.textUniGrade.TextChanged += new System.EventHandler(this.OnUniGpaValueChanged);
            // 
            // textUniTimeLeft
            // 
            this.textUniTimeLeft.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textUniTimeLeft.Location = new System.Drawing.Point(256, 186);
            this.textUniTimeLeft.Maximum = ((uint)(72u));
            this.textUniTimeLeft.Minimum = ((uint)(0u));
            this.textUniTimeLeft.Name = "textUniTimeLeft";
            this.textUniTimeLeft.Size = new System.Drawing.Size(50, 21);
            this.textUniTimeLeft.TabIndex = 37;
            this.textUniTimeLeft.Value = ((uint)(48u));
            this.textUniTimeLeft.TextChanged += new System.EventHandler(this.OnUniTimeLeftValueChanged);
            // 
            // textUniInfluence
            // 
            this.textUniInfluence.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textUniInfluence.Location = new System.Drawing.Point(80, 224);
            this.textUniInfluence.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.textUniInfluence.Maximum = ((uint)(32000u));
            this.textUniInfluence.Minimum = ((uint)(0u));
            this.textUniInfluence.Name = "textUniInfluence";
            this.textUniInfluence.Size = new System.Drawing.Size(80, 21);
            this.textUniInfluence.TabIndex = 23;
            this.textUniInfluence.Value = ((uint)(3782u));
            this.textUniInfluence.TextChanged += new System.EventHandler(this.OnUniInfluenceValueChanged);
            // 
            // textUniEffort
            // 
            this.textUniEffort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textUniEffort.Location = new System.Drawing.Point(256, 126);
            this.textUniEffort.Maximum = ((uint)(1000u));
            this.textUniEffort.Minimum = ((uint)(0u));
            this.textUniEffort.Name = "textUniEffort";
            this.textUniEffort.Size = new System.Drawing.Size(50, 21);
            this.textUniEffort.TabIndex = 23;
            this.textUniEffort.Value = ((uint)(753u));
            this.textUniEffort.TextChanged += new System.EventHandler(this.OnUniEffortValueChanged);
            // 
            // textMajorGUID
            // 
            this.textMajorGUID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textMajorGUID.Location = new System.Drawing.Point(256, 26);
            this.textMajorGUID.Name = "textMajorGUID";
            this.textMajorGUID.Size = new System.Drawing.Size(85, 21);
            this.textMajorGUID.TabIndex = 24;
            this.textMajorGUID.Value = ((uint)(2408550287u));
            this.textMajorGUID.TextChanged += new System.EventHandler(this.OnUniMajorGuidChanged);
            // 
            // lblUniStudying
            // 
            this.lblUniStudying.AutoSize = true;
            this.lblUniStudying.Location = new System.Drawing.Point(142, 158);
            this.lblUniStudying.Name = "lblUniStudying";
            this.lblUniStudying.Size = new System.Drawing.Size(87, 15);
            this.lblUniStudying.TabIndex = 36;
            this.lblUniStudying.Text = "Studying Hard:";
            // 
            // comboUniResult
            // 
            this.comboUniResult.FormattingEnabled = true;
            this.comboUniResult.Items.AddRange(new object[] {
            "Didn\'t Go",
            "Graduated",
            "Dropped Out",
            "Expelled"});
            this.comboUniResult.Location = new System.Drawing.Point(80, 60);
            this.comboUniResult.Name = "comboUniResult";
            this.comboUniResult.Size = new System.Drawing.Size(170, 23);
            this.comboUniResult.TabIndex = 32;
            this.comboUniResult.SelectedIndexChanged += new System.EventHandler(this.OnUniOutcomeChanged);
            // 
            // lblUniResult
            // 
            this.lblUniResult.AutoSize = true;
            this.lblUniResult.Location = new System.Drawing.Point(29, 63);
            this.lblUniResult.Name = "lblUniResult";
            this.lblUniResult.Size = new System.Drawing.Size(45, 15);
            this.lblUniResult.TabIndex = 31;
            this.lblUniResult.Text = "Result:";
            // 
            // ckbUniStudying
            // 
            this.ckbUniStudying.AutoSize = true;
            this.ckbUniStudying.Location = new System.Drawing.Point(235, 158);
            this.ckbUniStudying.Name = "ckbUniStudying";
            this.ckbUniStudying.Size = new System.Drawing.Size(15, 14);
            this.ckbUniStudying.TabIndex = 35;
            this.ckbUniStudying.UseVisualStyleBackColor = true;
            this.ckbUniStudying.Click += new System.EventHandler(this.OnUniGoodCompletedChanged);
            // 
            // trackUniTimeLeft
            // 
            this.trackUniTimeLeft.BackColor = System.Drawing.Color.Transparent;
            this.trackUniTimeLeft.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.trackUniTimeLeft.Gradient = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.trackUniTimeLeft.GradientEndColor = System.Drawing.Color.White;
            this.trackUniTimeLeft.GradientStartColor = System.Drawing.Color.White;
            this.trackUniTimeLeft.Location = new System.Drawing.Point(80, 191);
            this.trackUniTimeLeft.Maximum = 72;
            this.trackUniTimeLeft.Minimum = 0;
            this.trackUniTimeLeft.Name = "trackUniTimeLeft";
            this.trackUniTimeLeft.NegativeBalanceColour = System.Drawing.Color.Crimson;
            this.trackUniTimeLeft.PositiveBalanceColour = System.Drawing.Color.YellowGreen;
            this.trackUniTimeLeft.ProgressBackColor = System.Drawing.SystemColors.Window;
            this.trackUniTimeLeft.Quality = true;
            this.trackUniTimeLeft.SelectedColor = System.Drawing.Color.YellowGreen;
            this.trackUniTimeLeft.Size = new System.Drawing.Size(160, 16);
            this.trackUniTimeLeft.Style = Sims2Tools.Controls.SimsTrackingBarStyle.Flat;
            this.trackUniTimeLeft.TabIndex = 5;
            this.trackUniTimeLeft.TokenCount = 18;
            this.trackUniTimeLeft.UnselectedColor = System.Drawing.Color.Black;
            this.trackUniTimeLeft.UseTokenBuffer = true;
            this.trackUniTimeLeft.Value = 48;
            this.trackUniTimeLeft.Changed += new System.EventHandler(this.OnUniTimeLeftSliderChanged);
            // 
            // trackUniEffort
            // 
            this.trackUniEffort.BackColor = System.Drawing.Color.Transparent;
            this.trackUniEffort.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.trackUniEffort.Gradient = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.trackUniEffort.GradientEndColor = System.Drawing.Color.White;
            this.trackUniEffort.GradientStartColor = System.Drawing.Color.White;
            this.trackUniEffort.Location = new System.Drawing.Point(80, 127);
            this.trackUniEffort.Maximum = 1000;
            this.trackUniEffort.Minimum = 0;
            this.trackUniEffort.Name = "trackUniEffort";
            this.trackUniEffort.NegativeBalanceColour = System.Drawing.Color.Crimson;
            this.trackUniEffort.PositiveBalanceColour = System.Drawing.Color.YellowGreen;
            this.trackUniEffort.ProgressBackColor = System.Drawing.SystemColors.Window;
            this.trackUniEffort.Quality = true;
            this.trackUniEffort.SelectedColor = System.Drawing.Color.YellowGreen;
            this.trackUniEffort.Size = new System.Drawing.Size(170, 20);
            this.trackUniEffort.Style = Sims2Tools.Controls.SimsTrackingBarStyle.Increase;
            this.trackUniEffort.TabIndex = 16;
            this.trackUniEffort.TokenCount = 10;
            this.trackUniEffort.UnselectedColor = System.Drawing.Color.Black;
            this.trackUniEffort.UseTokenBuffer = true;
            this.trackUniEffort.Value = 753;
            this.trackUniEffort.Changed += new System.EventHandler(this.OnUniEffortSliderChanged);
            // 
            // lblUniProbation
            // 
            this.lblUniProbation.AutoSize = true;
            this.lblUniProbation.Location = new System.Drawing.Point(11, 158);
            this.lblUniProbation.Name = "lblUniProbation";
            this.lblUniProbation.Size = new System.Drawing.Size(63, 15);
            this.lblUniProbation.TabIndex = 34;
            this.lblUniProbation.Text = "Probation:";
            // 
            // ckbUniProbation
            // 
            this.ckbUniProbation.AutoSize = true;
            this.ckbUniProbation.Location = new System.Drawing.Point(80, 159);
            this.ckbUniProbation.Name = "ckbUniProbation";
            this.ckbUniProbation.Size = new System.Drawing.Size(15, 14);
            this.ckbUniProbation.TabIndex = 33;
            this.ckbUniProbation.UseVisualStyleBackColor = true;
            this.ckbUniProbation.Click += new System.EventHandler(this.OnUniProbationChanged);
            // 
            // trackUniGrade
            // 
            this.trackUniGrade.BackColor = System.Drawing.Color.Transparent;
            this.trackUniGrade.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.trackUniGrade.Gradient = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.trackUniGrade.GradientEndColor = System.Drawing.Color.White;
            this.trackUniGrade.GradientStartColor = System.Drawing.Color.White;
            this.trackUniGrade.Location = new System.Drawing.Point(80, 95);
            this.trackUniGrade.Maximum = 40;
            this.trackUniGrade.Minimum = 0;
            this.trackUniGrade.Name = "trackUniGrade";
            this.trackUniGrade.NegativeBalanceColour = System.Drawing.Color.Crimson;
            this.trackUniGrade.PositiveBalanceColour = System.Drawing.Color.YellowGreen;
            this.trackUniGrade.ProgressBackColor = System.Drawing.SystemColors.Window;
            this.trackUniGrade.Quality = true;
            this.trackUniGrade.SelectedColor = System.Drawing.Color.YellowGreen;
            this.trackUniGrade.Size = new System.Drawing.Size(170, 20);
            this.trackUniGrade.Style = Sims2Tools.Controls.SimsTrackingBarStyle.Increase;
            this.trackUniGrade.TabIndex = 5;
            this.trackUniGrade.TokenCount = 4;
            this.trackUniGrade.UnselectedColor = System.Drawing.Color.Black;
            this.trackUniGrade.UseTokenBuffer = true;
            this.trackUniGrade.Value = 34;
            this.trackUniGrade.Changed += new System.EventHandler(this.OnUniGpaSliderChanged);
            // 
            // lblUniInfluence
            // 
            this.lblUniInfluence.AutoSize = true;
            this.lblUniInfluence.Location = new System.Drawing.Point(14, 227);
            this.lblUniInfluence.Name = "lblUniInfluence";
            this.lblUniInfluence.Size = new System.Drawing.Size(60, 15);
            this.lblUniInfluence.TabIndex = 29;
            this.lblUniInfluence.Text = "Influence:";
            // 
            // comboUniSemester
            // 
            this.comboUniSemester.FormattingEnabled = true;
            this.comboUniSemester.Location = new System.Drawing.Point(80, 60);
            this.comboUniSemester.Name = "comboUniSemester";
            this.comboUniSemester.Size = new System.Drawing.Size(170, 23);
            this.comboUniSemester.TabIndex = 28;
            this.comboUniSemester.Text = "4 - Sophomore 2 ";
            this.comboUniSemester.SelectedIndexChanged += new System.EventHandler(this.OnUniSemesterChanged);
            // 
            // lblUniTimeLeft
            // 
            this.lblUniTimeLeft.AutoSize = true;
            this.lblUniTimeLeft.Location = new System.Drawing.Point(13, 192);
            this.lblUniTimeLeft.Name = "lblUniTimeLeft";
            this.lblUniTimeLeft.Size = new System.Drawing.Size(61, 15);
            this.lblUniTimeLeft.TabIndex = 25;
            this.lblUniTimeLeft.Text = "Time Left:";
            // 
            // lblUniGrade
            // 
            this.lblUniGrade.AutoSize = true;
            this.lblUniGrade.Location = new System.Drawing.Point(40, 100);
            this.lblUniGrade.Name = "lblUniGrade";
            this.lblUniGrade.Size = new System.Drawing.Size(34, 15);
            this.lblUniGrade.TabIndex = 21;
            this.lblUniGrade.Text = "GPA:";
            // 
            // lblUniSemester
            // 
            this.lblUniSemester.AutoSize = true;
            this.lblUniSemester.Location = new System.Drawing.Point(11, 63);
            this.lblUniSemester.Name = "lblUniSemester";
            this.lblUniSemester.Size = new System.Drawing.Size(63, 15);
            this.lblUniSemester.TabIndex = 19;
            this.lblUniSemester.Text = "Semester:";
            // 
            // comboUniMajor
            // 
            this.comboUniMajor.FormattingEnabled = true;
            this.comboUniMajor.Location = new System.Drawing.Point(80, 25);
            this.comboUniMajor.Name = "comboUniMajor";
            this.comboUniMajor.Size = new System.Drawing.Size(170, 23);
            this.comboUniMajor.TabIndex = 14;
            this.comboUniMajor.Text = "Undeclared";
            this.comboUniMajor.SelectedIndexChanged += new System.EventHandler(this.OnUniMajorTypeChanged);
            // 
            // lblUniEffort
            // 
            this.lblUniEffort.AutoSize = true;
            this.lblUniEffort.Location = new System.Drawing.Point(36, 132);
            this.lblUniEffort.Name = "lblUniEffort";
            this.lblUniEffort.Size = new System.Drawing.Size(38, 15);
            this.lblUniEffort.TabIndex = 17;
            this.lblUniEffort.Text = "Effort:";
            // 
            // lblUniMajor
            // 
            this.lblUniMajor.AutoSize = true;
            this.lblUniMajor.Location = new System.Drawing.Point(32, 28);
            this.lblUniMajor.Name = "lblUniMajor";
            this.lblUniMajor.Size = new System.Drawing.Size(42, 15);
            this.lblUniMajor.TabIndex = 15;
            this.lblUniMajor.Text = "Major:";
            // 
            // grpSchool
            // 
            this.grpSchool.Controls.Add(this.textSchoolGUID);
            this.grpSchool.Controls.Add(this.comboSchoolGrade);
            this.grpSchool.Controls.Add(this.lblSchoolGrade);
            this.grpSchool.Controls.Add(this.lblSchoolType);
            this.grpSchool.Controls.Add(this.comboSchoolType);
            this.grpSchool.Location = new System.Drawing.Point(5, 8);
            this.grpSchool.Name = "grpSchool";
            this.grpSchool.Size = new System.Drawing.Size(330, 265);
            this.grpSchool.TabIndex = 0;
            this.grpSchool.TabStop = false;
            this.grpSchool.Text = "School";
            // 
            // textSchoolGUID
            // 
            this.textSchoolGUID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textSchoolGUID.Location = new System.Drawing.Point(236, 26);
            this.textSchoolGUID.Name = "textSchoolGUID";
            this.textSchoolGUID.Size = new System.Drawing.Size(85, 21);
            this.textSchoolGUID.TabIndex = 23;
            this.textSchoolGUID.Value = ((uint)(2408550287u));
            this.textSchoolGUID.TextChanged += new System.EventHandler(this.OnSchoolGuidChanged);
            // 
            // comboSchoolGrade
            // 
            this.comboSchoolGrade.FormattingEnabled = true;
            this.comboSchoolGrade.Location = new System.Drawing.Point(60, 60);
            this.comboSchoolGrade.Name = "comboSchoolGrade";
            this.comboSchoolGrade.Size = new System.Drawing.Size(80, 23);
            this.comboSchoolGrade.TabIndex = 3;
            this.comboSchoolGrade.Text = "C+";
            this.comboSchoolGrade.SelectedIndexChanged += new System.EventHandler(this.OnSchoolGradeChanged);
            // 
            // lblSchoolGrade
            // 
            this.lblSchoolGrade.AutoSize = true;
            this.lblSchoolGrade.Location = new System.Drawing.Point(10, 63);
            this.lblSchoolGrade.Name = "lblSchoolGrade";
            this.lblSchoolGrade.Size = new System.Drawing.Size(44, 15);
            this.lblSchoolGrade.TabIndex = 2;
            this.lblSchoolGrade.Text = "Grade:";
            // 
            // lblSchoolType
            // 
            this.lblSchoolType.AutoSize = true;
            this.lblSchoolType.Location = new System.Drawing.Point(18, 28);
            this.lblSchoolType.Name = "lblSchoolType";
            this.lblSchoolType.Size = new System.Drawing.Size(36, 15);
            this.lblSchoolType.TabIndex = 1;
            this.lblSchoolType.Text = "Type:";
            // 
            // comboSchoolType
            // 
            this.comboSchoolType.FormattingEnabled = true;
            this.comboSchoolType.Location = new System.Drawing.Point(60, 25);
            this.comboSchoolType.Name = "comboSchoolType";
            this.comboSchoolType.Size = new System.Drawing.Size(170, 23);
            this.comboSchoolType.TabIndex = 0;
            this.comboSchoolType.Text = "Unknown";
            this.comboSchoolType.SelectedIndexChanged += new System.EventHandler(this.OnSchoolTypeChanged);
            // 
            // tabSkills
            // 
            this.tabSkills.Controls.Add(this.grpSkillsPet);
            this.tabSkills.Controls.Add(this.grpSkillsLife);
            this.tabSkills.Controls.Add(this.grpSkillsToddler);
            this.tabSkills.Controls.Add(this.grpSkillsHidden);
            this.tabSkills.Controls.Add(this.grpSkillsGeneral);
            this.tabSkills.Location = new System.Drawing.Point(4, 4);
            this.tabSkills.Margin = new System.Windows.Forms.Padding(0);
            this.tabSkills.Name = "tabSkills";
            this.tabSkills.Size = new System.Drawing.Size(1276, 281);
            this.tabSkills.TabIndex = 4;
            this.tabSkills.Text = "Skills";
            this.tabSkills.UseVisualStyleBackColor = true;
            // 
            // grpSkillsPet
            // 
            this.grpSkillsPet.Controls.Add(this.trackSkillPetUseToilet);
            this.grpSkillsPet.Controls.Add(this.trackSkillPetStay);
            this.grpSkillsPet.Controls.Add(this.trackSkillPetSpeak);
            this.grpSkillsPet.Controls.Add(this.trackSkillPetSitUp);
            this.grpSkillsPet.Controls.Add(this.trackSkillPetShake);
            this.grpSkillsPet.Controls.Add(this.trackSkillPetRollOver);
            this.grpSkillsPet.Controls.Add(this.trackSkillPetPlayDead);
            this.grpSkillsPet.Controls.Add(this.trackSkillPetComeHere);
            this.grpSkillsPet.Controls.Add(this.lblSkillPetUseToilet);
            this.grpSkillsPet.Controls.Add(this.lblSkillPetStay);
            this.grpSkillsPet.Controls.Add(this.lblSkillPetSpeak);
            this.grpSkillsPet.Controls.Add(this.lblSkillPetSitUp);
            this.grpSkillsPet.Controls.Add(this.lblSkillPetShake);
            this.grpSkillsPet.Controls.Add(this.lblSkillPetRollOver);
            this.grpSkillsPet.Controls.Add(this.lblSkillPetPlayDead);
            this.grpSkillsPet.Controls.Add(this.lblSkillPetComeHere);
            this.grpSkillsPet.Location = new System.Drawing.Point(960, 8);
            this.grpSkillsPet.Name = "grpSkillsPet";
            this.grpSkillsPet.Size = new System.Drawing.Size(230, 265);
            this.grpSkillsPet.TabIndex = 103;
            this.grpSkillsPet.TabStop = false;
            this.grpSkillsPet.Text = "Pet Skills";
            // 
            // trackSkillPetUseToilet
            // 
            this.trackSkillPetUseToilet.Location = new System.Drawing.Point(82, 230);
            this.trackSkillPetUseToilet.Margin = new System.Windows.Forms.Padding(0);
            this.trackSkillPetUseToilet.Maximum = 1000;
            this.trackSkillPetUseToilet.Name = "trackSkillPetUseToilet";
            this.trackSkillPetUseToilet.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.NONE;
            this.trackSkillPetUseToilet.Size = new System.Drawing.Size(138, 21);
            this.trackSkillPetUseToilet.TabIndex = 102;
            this.trackSkillPetUseToilet.Tag = "Romance";
            this.trackSkillPetUseToilet.TokenGuid = ((uint)(1907296382u));
            this.trackSkillPetUseToilet.TokenProp = ((uint)(0u));
            this.trackSkillPetUseToilet.Value = ((ushort)(0));
            this.trackSkillPetUseToilet.Changed += new System.EventHandler(this.OnPetSkillChanged);
            // 
            // trackSkillPetStay
            // 
            this.trackSkillPetStay.Location = new System.Drawing.Point(82, 200);
            this.trackSkillPetStay.Margin = new System.Windows.Forms.Padding(0);
            this.trackSkillPetStay.Maximum = 1000;
            this.trackSkillPetStay.Name = "trackSkillPetStay";
            this.trackSkillPetStay.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.NONE;
            this.trackSkillPetStay.Size = new System.Drawing.Size(138, 21);
            this.trackSkillPetStay.TabIndex = 101;
            this.trackSkillPetStay.Tag = "Mechanical";
            this.trackSkillPetStay.TokenGuid = ((uint)(2974183285u));
            this.trackSkillPetStay.TokenProp = ((uint)(0u));
            this.trackSkillPetStay.Value = ((ushort)(0));
            this.trackSkillPetStay.Changed += new System.EventHandler(this.OnPetSkillChanged);
            // 
            // trackSkillPetSpeak
            // 
            this.trackSkillPetSpeak.Location = new System.Drawing.Point(82, 170);
            this.trackSkillPetSpeak.Margin = new System.Windows.Forms.Padding(0);
            this.trackSkillPetSpeak.Maximum = 1000;
            this.trackSkillPetSpeak.Name = "trackSkillPetSpeak";
            this.trackSkillPetSpeak.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.NONE;
            this.trackSkillPetSpeak.Size = new System.Drawing.Size(138, 21);
            this.trackSkillPetSpeak.TabIndex = 100;
            this.trackSkillPetSpeak.Tag = "Logic";
            this.trackSkillPetSpeak.TokenGuid = ((uint)(825507393u));
            this.trackSkillPetSpeak.TokenProp = ((uint)(0u));
            this.trackSkillPetSpeak.Value = ((ushort)(0));
            this.trackSkillPetSpeak.Changed += new System.EventHandler(this.OnPetSkillChanged);
            // 
            // trackSkillPetSitUp
            // 
            this.trackSkillPetSitUp.Location = new System.Drawing.Point(82, 140);
            this.trackSkillPetSitUp.Margin = new System.Windows.Forms.Padding(0);
            this.trackSkillPetSitUp.Maximum = 1000;
            this.trackSkillPetSitUp.Name = "trackSkillPetSitUp";
            this.trackSkillPetSitUp.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.NONE;
            this.trackSkillPetSitUp.Size = new System.Drawing.Size(138, 21);
            this.trackSkillPetSitUp.TabIndex = 99;
            this.trackSkillPetSitUp.Tag = "Creativity";
            this.trackSkillPetSitUp.TokenGuid = ((uint)(1906766110u));
            this.trackSkillPetSitUp.TokenProp = ((uint)(0u));
            this.trackSkillPetSitUp.Value = ((ushort)(0));
            this.trackSkillPetSitUp.Changed += new System.EventHandler(this.OnPetSkillChanged);
            // 
            // trackSkillPetShake
            // 
            this.trackSkillPetShake.Location = new System.Drawing.Point(82, 110);
            this.trackSkillPetShake.Margin = new System.Windows.Forms.Padding(0);
            this.trackSkillPetShake.Maximum = 1000;
            this.trackSkillPetShake.Name = "trackSkillPetShake";
            this.trackSkillPetShake.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.NONE;
            this.trackSkillPetShake.Size = new System.Drawing.Size(138, 21);
            this.trackSkillPetShake.TabIndex = 98;
            this.trackSkillPetShake.Tag = "Cooking";
            this.trackSkillPetShake.TokenGuid = ((uint)(4046217132u));
            this.trackSkillPetShake.TokenProp = ((uint)(0u));
            this.trackSkillPetShake.Value = ((ushort)(0));
            this.trackSkillPetShake.Changed += new System.EventHandler(this.OnPetSkillChanged);
            // 
            // trackSkillPetRollOver
            // 
            this.trackSkillPetRollOver.Location = new System.Drawing.Point(82, 80);
            this.trackSkillPetRollOver.Margin = new System.Windows.Forms.Padding(0);
            this.trackSkillPetRollOver.Maximum = 1000;
            this.trackSkillPetRollOver.Name = "trackSkillPetRollOver";
            this.trackSkillPetRollOver.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.NONE;
            this.trackSkillPetRollOver.Size = new System.Drawing.Size(138, 21);
            this.trackSkillPetRollOver.TabIndex = 97;
            this.trackSkillPetRollOver.Tag = "Cleaning";
            this.trackSkillPetRollOver.TokenGuid = ((uint)(2980508008u));
            this.trackSkillPetRollOver.TokenProp = ((uint)(0u));
            this.trackSkillPetRollOver.Value = ((ushort)(0));
            this.trackSkillPetRollOver.Changed += new System.EventHandler(this.OnPetSkillChanged);
            // 
            // trackSkillPetPlayDead
            // 
            this.trackSkillPetPlayDead.Location = new System.Drawing.Point(82, 50);
            this.trackSkillPetPlayDead.Margin = new System.Windows.Forms.Padding(0);
            this.trackSkillPetPlayDead.Maximum = 1000;
            this.trackSkillPetPlayDead.Name = "trackSkillPetPlayDead";
            this.trackSkillPetPlayDead.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.NONE;
            this.trackSkillPetPlayDead.Size = new System.Drawing.Size(138, 21);
            this.trackSkillPetPlayDead.TabIndex = 96;
            this.trackSkillPetPlayDead.Tag = "Charisma";
            this.trackSkillPetPlayDead.TokenGuid = ((uint)(1362464868u));
            this.trackSkillPetPlayDead.TokenProp = ((uint)(0u));
            this.trackSkillPetPlayDead.Value = ((ushort)(0));
            this.trackSkillPetPlayDead.Changed += new System.EventHandler(this.OnPetSkillChanged);
            // 
            // trackSkillPetComeHere
            // 
            this.trackSkillPetComeHere.Location = new System.Drawing.Point(82, 20);
            this.trackSkillPetComeHere.Margin = new System.Windows.Forms.Padding(0);
            this.trackSkillPetComeHere.Maximum = 1000;
            this.trackSkillPetComeHere.Name = "trackSkillPetComeHere";
            this.trackSkillPetComeHere.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.NONE;
            this.trackSkillPetComeHere.Size = new System.Drawing.Size(138, 21);
            this.trackSkillPetComeHere.TabIndex = 95;
            this.trackSkillPetComeHere.Tag = "Body";
            this.trackSkillPetComeHere.TokenGuid = ((uint)(3510116837u));
            this.trackSkillPetComeHere.TokenProp = ((uint)(0u));
            this.trackSkillPetComeHere.Value = ((ushort)(0));
            this.trackSkillPetComeHere.Changed += new System.EventHandler(this.OnPetSkillChanged);
            // 
            // lblSkillPetUseToilet
            // 
            this.lblSkillPetUseToilet.AutoSize = true;
            this.lblSkillPetUseToilet.Location = new System.Drawing.Point(13, 235);
            this.lblSkillPetUseToilet.Name = "lblSkillPetUseToilet";
            this.lblSkillPetUseToilet.Size = new System.Drawing.Size(65, 15);
            this.lblSkillPetUseToilet.TabIndex = 52;
            this.lblSkillPetUseToilet.Text = "Use Toilet:";
            // 
            // lblSkillPetStay
            // 
            this.lblSkillPetStay.AutoSize = true;
            this.lblSkillPetStay.Location = new System.Drawing.Point(45, 205);
            this.lblSkillPetStay.Name = "lblSkillPetStay";
            this.lblSkillPetStay.Size = new System.Drawing.Size(33, 15);
            this.lblSkillPetStay.TabIndex = 51;
            this.lblSkillPetStay.Text = "Stay:";
            // 
            // lblSkillPetSpeak
            // 
            this.lblSkillPetSpeak.AutoSize = true;
            this.lblSkillPetSpeak.Location = new System.Drawing.Point(33, 175);
            this.lblSkillPetSpeak.Name = "lblSkillPetSpeak";
            this.lblSkillPetSpeak.Size = new System.Drawing.Size(45, 15);
            this.lblSkillPetSpeak.TabIndex = 50;
            this.lblSkillPetSpeak.Tag = "";
            this.lblSkillPetSpeak.Text = "Speak:";
            // 
            // lblSkillPetSitUp
            // 
            this.lblSkillPetSitUp.AutoSize = true;
            this.lblSkillPetSitUp.Location = new System.Drawing.Point(35, 145);
            this.lblSkillPetSitUp.Name = "lblSkillPetSitUp";
            this.lblSkillPetSitUp.Size = new System.Drawing.Size(43, 15);
            this.lblSkillPetSitUp.TabIndex = 49;
            this.lblSkillPetSitUp.Tag = "";
            this.lblSkillPetSitUp.Text = "Sit Up:";
            // 
            // lblSkillPetShake
            // 
            this.lblSkillPetShake.AutoSize = true;
            this.lblSkillPetShake.Location = new System.Drawing.Point(33, 115);
            this.lblSkillPetShake.Name = "lblSkillPetShake";
            this.lblSkillPetShake.Size = new System.Drawing.Size(45, 15);
            this.lblSkillPetShake.TabIndex = 48;
            this.lblSkillPetShake.Text = "Shake:";
            // 
            // lblSkillPetRollOver
            // 
            this.lblSkillPetRollOver.AutoSize = true;
            this.lblSkillPetRollOver.Location = new System.Drawing.Point(18, 85);
            this.lblSkillPetRollOver.Name = "lblSkillPetRollOver";
            this.lblSkillPetRollOver.Size = new System.Drawing.Size(60, 15);
            this.lblSkillPetRollOver.TabIndex = 47;
            this.lblSkillPetRollOver.Text = "Roll Over:";
            // 
            // lblSkillPetPlayDead
            // 
            this.lblSkillPetPlayDead.AutoSize = true;
            this.lblSkillPetPlayDead.Location = new System.Drawing.Point(12, 55);
            this.lblSkillPetPlayDead.Name = "lblSkillPetPlayDead";
            this.lblSkillPetPlayDead.Size = new System.Drawing.Size(66, 15);
            this.lblSkillPetPlayDead.TabIndex = 46;
            this.lblSkillPetPlayDead.Text = "Play Dead:";
            // 
            // lblSkillPetComeHere
            // 
            this.lblSkillPetComeHere.AutoSize = true;
            this.lblSkillPetComeHere.Location = new System.Drawing.Point(7, 25);
            this.lblSkillPetComeHere.Name = "lblSkillPetComeHere";
            this.lblSkillPetComeHere.Size = new System.Drawing.Size(73, 15);
            this.lblSkillPetComeHere.TabIndex = 0;
            this.lblSkillPetComeHere.Text = "Come Here:";
            // 
            // grpSkillsLife
            // 
            this.grpSkillsLife.Controls.Add(this.trackSkillLifePhysiology);
            this.grpSkillsLife.Controls.Add(this.trackSkillLifeParenting);
            this.grpSkillsLife.Controls.Add(this.trackSkillLifeHappiness);
            this.grpSkillsLife.Controls.Add(this.trackSkillLifeFireSafety);
            this.grpSkillsLife.Controls.Add(this.trackSkillLifeCounselling);
            this.grpSkillsLife.Controls.Add(this.trackSkillLifeAngerMgmt);
            this.grpSkillsLife.Controls.Add(this.lblSkillLifePhysiology);
            this.grpSkillsLife.Controls.Add(this.lblSkillLifeParenting);
            this.grpSkillsLife.Controls.Add(this.lblSkillLifeHappiness);
            this.grpSkillsLife.Controls.Add(this.lblSkillLifeFireSafety);
            this.grpSkillsLife.Controls.Add(this.lblSkillLifeCounselling);
            this.grpSkillsLife.Controls.Add(this.lblSkillLifeAngerMgmt);
            this.grpSkillsLife.Location = new System.Drawing.Point(715, 8);
            this.grpSkillsLife.Name = "grpSkillsLife";
            this.grpSkillsLife.Size = new System.Drawing.Size(235, 265);
            this.grpSkillsLife.TabIndex = 83;
            this.grpSkillsLife.TabStop = false;
            this.grpSkillsLife.Text = "Life Skills";
            // 
            // trackSkillLifePhysiology
            // 
            this.trackSkillLifePhysiology.Location = new System.Drawing.Point(87, 170);
            this.trackSkillLifePhysiology.Margin = new System.Windows.Forms.Padding(0);
            this.trackSkillLifePhysiology.Maximum = 1000;
            this.trackSkillLifePhysiology.Name = "trackSkillLifePhysiology";
            this.trackSkillLifePhysiology.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.NONE;
            this.trackSkillLifePhysiology.Size = new System.Drawing.Size(138, 21);
            this.trackSkillLifePhysiology.TabIndex = 106;
            this.trackSkillLifePhysiology.Tag = "Physiology";
            this.trackSkillLifePhysiology.TokenGuid = ((uint)(1423445459u));
            this.trackSkillLifePhysiology.TokenProp = ((uint)(1u));
            this.trackSkillLifePhysiology.Value = ((ushort)(0));
            this.trackSkillLifePhysiology.Changed += new System.EventHandler(this.OnLifeSkillChanged);
            // 
            // trackSkillLifeParenting
            // 
            this.trackSkillLifeParenting.Location = new System.Drawing.Point(87, 140);
            this.trackSkillLifeParenting.Margin = new System.Windows.Forms.Padding(0);
            this.trackSkillLifeParenting.Maximum = 1000;
            this.trackSkillLifeParenting.Name = "trackSkillLifeParenting";
            this.trackSkillLifeParenting.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.NONE;
            this.trackSkillLifeParenting.Size = new System.Drawing.Size(138, 21);
            this.trackSkillLifeParenting.TabIndex = 105;
            this.trackSkillLifeParenting.Tag = "Parenting";
            this.trackSkillLifeParenting.TokenGuid = ((uint)(3019036469u));
            this.trackSkillLifeParenting.TokenProp = ((uint)(1u));
            this.trackSkillLifeParenting.Value = ((ushort)(0));
            this.trackSkillLifeParenting.Changed += new System.EventHandler(this.OnLifeSkillChanged);
            // 
            // trackSkillLifeHappiness
            // 
            this.trackSkillLifeHappiness.Location = new System.Drawing.Point(87, 110);
            this.trackSkillLifeHappiness.Margin = new System.Windows.Forms.Padding(0);
            this.trackSkillLifeHappiness.Maximum = 1000;
            this.trackSkillLifeHappiness.Name = "trackSkillLifeHappiness";
            this.trackSkillLifeHappiness.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.NONE;
            this.trackSkillLifeHappiness.Size = new System.Drawing.Size(138, 21);
            this.trackSkillLifeHappiness.TabIndex = 104;
            this.trackSkillLifeHappiness.Tag = "Lifelong Happiness";
            this.trackSkillLifeHappiness.TokenGuid = ((uint)(1423445446u));
            this.trackSkillLifeHappiness.TokenProp = ((uint)(1u));
            this.trackSkillLifeHappiness.Value = ((ushort)(0));
            this.trackSkillLifeHappiness.Changed += new System.EventHandler(this.OnLifeSkillChanged);
            // 
            // trackSkillLifeFireSafety
            // 
            this.trackSkillLifeFireSafety.Location = new System.Drawing.Point(87, 80);
            this.trackSkillLifeFireSafety.Margin = new System.Windows.Forms.Padding(0);
            this.trackSkillLifeFireSafety.Maximum = 1000;
            this.trackSkillLifeFireSafety.Name = "trackSkillLifeFireSafety";
            this.trackSkillLifeFireSafety.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.NONE;
            this.trackSkillLifeFireSafety.Size = new System.Drawing.Size(138, 21);
            this.trackSkillLifeFireSafety.TabIndex = 103;
            this.trackSkillLifeFireSafety.Tag = "Fire Prevention";
            this.trackSkillLifeFireSafety.TokenGuid = ((uint)(1960316328u));
            this.trackSkillLifeFireSafety.TokenProp = ((uint)(1u));
            this.trackSkillLifeFireSafety.Value = ((ushort)(0));
            this.trackSkillLifeFireSafety.Changed += new System.EventHandler(this.OnLifeSkillChanged);
            // 
            // trackSkillLifeCounselling
            // 
            this.trackSkillLifeCounselling.Location = new System.Drawing.Point(87, 50);
            this.trackSkillLifeCounselling.Margin = new System.Windows.Forms.Padding(0);
            this.trackSkillLifeCounselling.Maximum = 1000;
            this.trackSkillLifeCounselling.Name = "trackSkillLifeCounselling";
            this.trackSkillLifeCounselling.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.NONE;
            this.trackSkillLifeCounselling.Size = new System.Drawing.Size(138, 21);
            this.trackSkillLifeCounselling.TabIndex = 102;
            this.trackSkillLifeCounselling.Tag = "Couple Counselling";
            this.trackSkillLifeCounselling.TokenGuid = ((uint)(886266039u));
            this.trackSkillLifeCounselling.TokenProp = ((uint)(1u));
            this.trackSkillLifeCounselling.Value = ((ushort)(0));
            this.trackSkillLifeCounselling.Changed += new System.EventHandler(this.OnLifeSkillChanged);
            // 
            // trackSkillLifeAngerMgmt
            // 
            this.trackSkillLifeAngerMgmt.Location = new System.Drawing.Point(87, 20);
            this.trackSkillLifeAngerMgmt.Margin = new System.Windows.Forms.Padding(0);
            this.trackSkillLifeAngerMgmt.Maximum = 1000;
            this.trackSkillLifeAngerMgmt.Name = "trackSkillLifeAngerMgmt";
            this.trackSkillLifeAngerMgmt.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.NONE;
            this.trackSkillLifeAngerMgmt.Size = new System.Drawing.Size(138, 21);
            this.trackSkillLifeAngerMgmt.TabIndex = 101;
            this.trackSkillLifeAngerMgmt.Tag = "Anger Management";
            this.trackSkillLifeAngerMgmt.TokenGuid = ((uint)(2497187258u));
            this.trackSkillLifeAngerMgmt.TokenProp = ((uint)(1u));
            this.trackSkillLifeAngerMgmt.Value = ((ushort)(0));
            this.trackSkillLifeAngerMgmt.Changed += new System.EventHandler(this.OnLifeSkillChanged);
            // 
            // lblSkillLifePhysiology
            // 
            this.lblSkillLifePhysiology.AutoSize = true;
            this.lblSkillLifePhysiology.Location = new System.Drawing.Point(17, 175);
            this.lblSkillLifePhysiology.Name = "lblSkillLifePhysiology";
            this.lblSkillLifePhysiology.Size = new System.Drawing.Size(68, 15);
            this.lblSkillLifePhysiology.TabIndex = 54;
            this.lblSkillLifePhysiology.Text = "Physiology:";
            // 
            // lblSkillLifeParenting
            // 
            this.lblSkillLifeParenting.AutoSize = true;
            this.lblSkillLifeParenting.Location = new System.Drawing.Point(22, 145);
            this.lblSkillLifeParenting.Name = "lblSkillLifeParenting";
            this.lblSkillLifeParenting.Size = new System.Drawing.Size(63, 15);
            this.lblSkillLifeParenting.TabIndex = 51;
            this.lblSkillLifeParenting.Text = "Parenting:";
            // 
            // lblSkillLifeHappiness
            // 
            this.lblSkillLifeHappiness.AutoSize = true;
            this.lblSkillLifeHappiness.Location = new System.Drawing.Point(16, 115);
            this.lblSkillLifeHappiness.Name = "lblSkillLifeHappiness";
            this.lblSkillLifeHappiness.Size = new System.Drawing.Size(69, 15);
            this.lblSkillLifeHappiness.TabIndex = 48;
            this.lblSkillLifeHappiness.Text = "Happiness:";
            // 
            // lblSkillLifeFireSafety
            // 
            this.lblSkillLifeFireSafety.AutoSize = true;
            this.lblSkillLifeFireSafety.Location = new System.Drawing.Point(18, 85);
            this.lblSkillLifeFireSafety.Name = "lblSkillLifeFireSafety";
            this.lblSkillLifeFireSafety.Size = new System.Drawing.Size(67, 15);
            this.lblSkillLifeFireSafety.TabIndex = 47;
            this.lblSkillLifeFireSafety.Text = "Fire Safety:";
            // 
            // lblSkillLifeCounselling
            // 
            this.lblSkillLifeCounselling.AutoSize = true;
            this.lblSkillLifeCounselling.Location = new System.Drawing.Point(10, 55);
            this.lblSkillLifeCounselling.Name = "lblSkillLifeCounselling";
            this.lblSkillLifeCounselling.Size = new System.Drawing.Size(75, 15);
            this.lblSkillLifeCounselling.TabIndex = 46;
            this.lblSkillLifeCounselling.Text = "Counselling:";
            // 
            // lblSkillLifeAngerMgmt
            // 
            this.lblSkillLifeAngerMgmt.AutoSize = true;
            this.lblSkillLifeAngerMgmt.Location = new System.Drawing.Point(8, 25);
            this.lblSkillLifeAngerMgmt.Name = "lblSkillLifeAngerMgmt";
            this.lblSkillLifeAngerMgmt.Size = new System.Drawing.Size(77, 15);
            this.lblSkillLifeAngerMgmt.TabIndex = 0;
            this.lblSkillLifeAngerMgmt.Text = "Anger Mgmt:";
            // 
            // grpSkillsToddler
            // 
            this.grpSkillsToddler.Controls.Add(this.trackSkillToddlerWalk);
            this.grpSkillsToddler.Controls.Add(this.lblSkillToddlerWalk);
            this.grpSkillsToddler.Controls.Add(this.trackSkillToddlerTalk);
            this.grpSkillsToddler.Controls.Add(this.lblSkillToddlerTalk);
            this.grpSkillsToddler.Controls.Add(this.trackSkillToddlerRhyming);
            this.grpSkillsToddler.Controls.Add(this.lblSkillToddlerRhyming);
            this.grpSkillsToddler.Controls.Add(this.trackSkillToddlerPotty);
            this.grpSkillsToddler.Controls.Add(this.lblSkillToddlerPotty);
            this.grpSkillsToddler.Location = new System.Drawing.Point(240, 8);
            this.grpSkillsToddler.Name = "grpSkillsToddler";
            this.grpSkillsToddler.Size = new System.Drawing.Size(220, 265);
            this.grpSkillsToddler.TabIndex = 83;
            this.grpSkillsToddler.TabStop = false;
            this.grpSkillsToddler.Text = "Toddler Skills";
            // 
            // trackSkillToddlerWalk
            // 
            this.trackSkillToddlerWalk.Location = new System.Drawing.Point(72, 110);
            this.trackSkillToddlerWalk.Margin = new System.Windows.Forms.Padding(0);
            this.trackSkillToddlerWalk.Maximum = 1000;
            this.trackSkillToddlerWalk.Name = "trackSkillToddlerWalk";
            this.trackSkillToddlerWalk.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.NONE;
            this.trackSkillToddlerWalk.Size = new System.Drawing.Size(138, 21);
            this.trackSkillToddlerWalk.TabIndex = 106;
            this.trackSkillToddlerWalk.Tag = "Learn To Walk";
            this.trackSkillToddlerWalk.TokenGuid = ((uint)(1306463762u));
            this.trackSkillToddlerWalk.TokenProp = ((uint)(2u));
            this.trackSkillToddlerWalk.Value = ((ushort)(0));
            this.trackSkillToddlerWalk.Changed += new System.EventHandler(this.OnToddlerSkillChanged);
            // 
            // lblSkillToddlerWalk
            // 
            this.lblSkillToddlerWalk.AutoSize = true;
            this.lblSkillToddlerWalk.Location = new System.Drawing.Point(33, 115);
            this.lblSkillToddlerWalk.Name = "lblSkillToddlerWalk";
            this.lblSkillToddlerWalk.Size = new System.Drawing.Size(37, 15);
            this.lblSkillToddlerWalk.TabIndex = 48;
            this.lblSkillToddlerWalk.Text = "Walk:";
            // 
            // trackSkillToddlerTalk
            // 
            this.trackSkillToddlerTalk.Location = new System.Drawing.Point(72, 80);
            this.trackSkillToddlerTalk.Margin = new System.Windows.Forms.Padding(0);
            this.trackSkillToddlerTalk.Maximum = 600;
            this.trackSkillToddlerTalk.Name = "trackSkillToddlerTalk";
            this.trackSkillToddlerTalk.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.NONE;
            this.trackSkillToddlerTalk.Size = new System.Drawing.Size(138, 21);
            this.trackSkillToddlerTalk.TabIndex = 105;
            this.trackSkillToddlerTalk.Tag = "Learn To Talk";
            this.trackSkillToddlerTalk.TokenGuid = ((uint)(1306463762u));
            this.trackSkillToddlerTalk.TokenProp = ((uint)(3u));
            this.trackSkillToddlerTalk.Value = ((ushort)(0));
            this.trackSkillToddlerTalk.Changed += new System.EventHandler(this.OnToddlerSkillChanged);
            // 
            // lblSkillToddlerTalk
            // 
            this.lblSkillToddlerTalk.AutoSize = true;
            this.lblSkillToddlerTalk.Location = new System.Drawing.Point(37, 85);
            this.lblSkillToddlerTalk.Name = "lblSkillToddlerTalk";
            this.lblSkillToddlerTalk.Size = new System.Drawing.Size(33, 15);
            this.lblSkillToddlerTalk.TabIndex = 47;
            this.lblSkillToddlerTalk.Text = "Talk:";
            // 
            // trackSkillToddlerRhyming
            // 
            this.trackSkillToddlerRhyming.Location = new System.Drawing.Point(72, 50);
            this.trackSkillToddlerRhyming.Margin = new System.Windows.Forms.Padding(0);
            this.trackSkillToddlerRhyming.Maximum = 600;
            this.trackSkillToddlerRhyming.Name = "trackSkillToddlerRhyming";
            this.trackSkillToddlerRhyming.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.NONE;
            this.trackSkillToddlerRhyming.Size = new System.Drawing.Size(138, 21);
            this.trackSkillToddlerRhyming.TabIndex = 104;
            this.trackSkillToddlerRhyming.Tag = "Learn To Rhyme";
            this.trackSkillToddlerRhyming.TokenGuid = ((uint)(1306463762u));
            this.trackSkillToddlerRhyming.TokenProp = ((uint)(8u));
            this.trackSkillToddlerRhyming.Value = ((ushort)(0));
            this.trackSkillToddlerRhyming.Changed += new System.EventHandler(this.OnToddlerSkillChanged);
            // 
            // lblSkillToddlerRhyming
            // 
            this.lblSkillToddlerRhyming.AutoSize = true;
            this.lblSkillToddlerRhyming.Location = new System.Drawing.Point(11, 55);
            this.lblSkillToddlerRhyming.Name = "lblSkillToddlerRhyming";
            this.lblSkillToddlerRhyming.Size = new System.Drawing.Size(59, 15);
            this.lblSkillToddlerRhyming.TabIndex = 46;
            this.lblSkillToddlerRhyming.Text = "Rhyming:";
            // 
            // trackSkillToddlerPotty
            // 
            this.trackSkillToddlerPotty.Location = new System.Drawing.Point(72, 20);
            this.trackSkillToddlerPotty.Margin = new System.Windows.Forms.Padding(0);
            this.trackSkillToddlerPotty.Maximum = 150;
            this.trackSkillToddlerPotty.Name = "trackSkillToddlerPotty";
            this.trackSkillToddlerPotty.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.NONE;
            this.trackSkillToddlerPotty.Size = new System.Drawing.Size(138, 21);
            this.trackSkillToddlerPotty.TabIndex = 103;
            this.trackSkillToddlerPotty.Tag = "Potty Training";
            this.trackSkillToddlerPotty.TokenGuid = ((uint)(1306463762u));
            this.trackSkillToddlerPotty.TokenProp = ((uint)(4u));
            this.trackSkillToddlerPotty.Value = ((ushort)(0));
            this.trackSkillToddlerPotty.Changed += new System.EventHandler(this.OnToddlerSkillChanged);
            // 
            // lblSkillToddlerPotty
            // 
            this.lblSkillToddlerPotty.AutoSize = true;
            this.lblSkillToddlerPotty.Location = new System.Drawing.Point(34, 25);
            this.lblSkillToddlerPotty.Name = "lblSkillToddlerPotty";
            this.lblSkillToddlerPotty.Size = new System.Drawing.Size(36, 15);
            this.lblSkillToddlerPotty.TabIndex = 0;
            this.lblSkillToddlerPotty.Text = "Potty:";
            // 
            // grpSkillsHidden
            // 
            this.grpSkillsHidden.Controls.Add(this.trackSkillHiddenFireDance);
            this.grpSkillsHidden.Controls.Add(this.lblSkillHiddenFireDance);
            this.grpSkillsHidden.Controls.Add(this.trackSkillHiddenBreakDance);
            this.grpSkillsHidden.Controls.Add(this.lblSkillHiddenBreakDance);
            this.grpSkillsHidden.Controls.Add(this.trackSkillHiddenTaiChi);
            this.grpSkillsHidden.Controls.Add(this.trackSkillHiddenStudy);
            this.grpSkillsHidden.Controls.Add(this.trackSkillHiddenPool);
            this.grpSkillsHidden.Controls.Add(this.trackSkillHiddenMeditate);
            this.grpSkillsHidden.Controls.Add(this.trackSkillHiddenDance);
            this.grpSkillsHidden.Controls.Add(this.lblSkillHiddenTaiChi);
            this.grpSkillsHidden.Controls.Add(this.lblSkillHiddenStudy);
            this.grpSkillsHidden.Controls.Add(this.lblSkillHiddenPool);
            this.grpSkillsHidden.Controls.Add(this.lblSkillHiddenMeditate);
            this.grpSkillsHidden.Controls.Add(this.lblSkillHiddenDance);
            this.grpSkillsHidden.Location = new System.Drawing.Point(470, 8);
            this.grpSkillsHidden.Name = "grpSkillsHidden";
            this.grpSkillsHidden.Size = new System.Drawing.Size(235, 265);
            this.grpSkillsHidden.TabIndex = 82;
            this.grpSkillsHidden.TabStop = false;
            this.grpSkillsHidden.Text = "Hidden Skills";
            // 
            // trackSkillHiddenTaiChi
            // 
            this.trackSkillHiddenTaiChi.Location = new System.Drawing.Point(87, 200);
            this.trackSkillHiddenTaiChi.Margin = new System.Windows.Forms.Padding(0);
            this.trackSkillHiddenTaiChi.Maximum = 72;
            this.trackSkillHiddenTaiChi.Name = "trackSkillHiddenTaiChi";
            this.trackSkillHiddenTaiChi.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.NONE;
            this.trackSkillHiddenTaiChi.Size = new System.Drawing.Size(138, 21);
            this.trackSkillHiddenTaiChi.TabIndex = 104;
            this.trackSkillHiddenTaiChi.Tag = "Tai Chi";
            this.trackSkillHiddenTaiChi.TokenGuid = ((uint)(1932666764u));
            this.trackSkillHiddenTaiChi.TokenProp = ((uint)(1u));
            this.trackSkillHiddenTaiChi.Value = ((ushort)(0));
            this.trackSkillHiddenTaiChi.Changed += new System.EventHandler(this.OnHiddenSkillChanged);
            // 
            // trackSkillHiddenStudy
            // 
            this.trackSkillHiddenStudy.Location = new System.Drawing.Point(87, 170);
            this.trackSkillHiddenStudy.Margin = new System.Windows.Forms.Padding(0);
            this.trackSkillHiddenStudy.Maximum = 50;
            this.trackSkillHiddenStudy.Name = "trackSkillHiddenStudy";
            this.trackSkillHiddenStudy.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.NONE;
            this.trackSkillHiddenStudy.Size = new System.Drawing.Size(138, 21);
            this.trackSkillHiddenStudy.TabIndex = 103;
            this.trackSkillHiddenStudy.Tag = "Study";
            this.trackSkillHiddenStudy.TokenGuid = ((uint)(1300958403u));
            this.trackSkillHiddenStudy.TokenProp = ((uint)(4u));
            this.trackSkillHiddenStudy.Value = ((ushort)(0));
            this.trackSkillHiddenStudy.Changed += new System.EventHandler(this.OnHiddenSkillChanged);
            // 
            // trackSkillHiddenPool
            // 
            this.trackSkillHiddenPool.Location = new System.Drawing.Point(87, 140);
            this.trackSkillHiddenPool.Margin = new System.Windows.Forms.Padding(0);
            this.trackSkillHiddenPool.Maximum = 100;
            this.trackSkillHiddenPool.Name = "trackSkillHiddenPool";
            this.trackSkillHiddenPool.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.NONE;
            this.trackSkillHiddenPool.Size = new System.Drawing.Size(138, 21);
            this.trackSkillHiddenPool.TabIndex = 102;
            this.trackSkillHiddenPool.Tag = "Play Pool";
            this.trackSkillHiddenPool.TokenGuid = ((uint)(1300958403u));
            this.trackSkillHiddenPool.TokenProp = ((uint)(5u));
            this.trackSkillHiddenPool.Value = ((ushort)(0));
            this.trackSkillHiddenPool.Changed += new System.EventHandler(this.OnHiddenSkillChanged);
            // 
            // trackSkillHiddenMeditate
            // 
            this.trackSkillHiddenMeditate.Location = new System.Drawing.Point(87, 110);
            this.trackSkillHiddenMeditate.Margin = new System.Windows.Forms.Padding(0);
            this.trackSkillHiddenMeditate.Maximum = 500;
            this.trackSkillHiddenMeditate.Name = "trackSkillHiddenMeditate";
            this.trackSkillHiddenMeditate.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.NONE;
            this.trackSkillHiddenMeditate.Size = new System.Drawing.Size(138, 21);
            this.trackSkillHiddenMeditate.TabIndex = 101;
            this.trackSkillHiddenMeditate.Tag = "Meditate";
            this.trackSkillHiddenMeditate.TokenGuid = ((uint)(1300958403u));
            this.trackSkillHiddenMeditate.TokenProp = ((uint)(3u));
            this.trackSkillHiddenMeditate.Value = ((ushort)(0));
            this.trackSkillHiddenMeditate.Changed += new System.EventHandler(this.OnHiddenSkillChanged);
            // 
            // trackSkillHiddenDance
            // 
            this.trackSkillHiddenDance.Location = new System.Drawing.Point(87, 50);
            this.trackSkillHiddenDance.Margin = new System.Windows.Forms.Padding(0);
            this.trackSkillHiddenDance.Maximum = 750;
            this.trackSkillHiddenDance.Name = "trackSkillHiddenDance";
            this.trackSkillHiddenDance.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.NONE;
            this.trackSkillHiddenDance.Size = new System.Drawing.Size(138, 21);
            this.trackSkillHiddenDance.TabIndex = 100;
            this.trackSkillHiddenDance.Tag = "Dance";
            this.trackSkillHiddenDance.TokenGuid = ((uint)(1877468243u));
            this.trackSkillHiddenDance.TokenProp = ((uint)(1u));
            this.trackSkillHiddenDance.Value = ((ushort)(0));
            this.trackSkillHiddenDance.Changed += new System.EventHandler(this.OnHiddenSkillChanged);
            // 
            // lblSkillHiddenTaiChi
            // 
            this.lblSkillHiddenTaiChi.AutoSize = true;
            this.lblSkillHiddenTaiChi.Location = new System.Drawing.Point(37, 205);
            this.lblSkillHiddenTaiChi.Name = "lblSkillHiddenTaiChi";
            this.lblSkillHiddenTaiChi.Size = new System.Drawing.Size(48, 15);
            this.lblSkillHiddenTaiChi.TabIndex = 51;
            this.lblSkillHiddenTaiChi.Text = "Tai Chi:";
            // 
            // lblSkillHiddenStudy
            // 
            this.lblSkillHiddenStudy.AutoSize = true;
            this.lblSkillHiddenStudy.Location = new System.Drawing.Point(45, 175);
            this.lblSkillHiddenStudy.Name = "lblSkillHiddenStudy";
            this.lblSkillHiddenStudy.Size = new System.Drawing.Size(40, 15);
            this.lblSkillHiddenStudy.TabIndex = 48;
            this.lblSkillHiddenStudy.Text = "Study:";
            // 
            // lblSkillHiddenPool
            // 
            this.lblSkillHiddenPool.AutoSize = true;
            this.lblSkillHiddenPool.Location = new System.Drawing.Point(24, 145);
            this.lblSkillHiddenPool.Name = "lblSkillHiddenPool";
            this.lblSkillHiddenPool.Size = new System.Drawing.Size(61, 15);
            this.lblSkillHiddenPool.TabIndex = 47;
            this.lblSkillHiddenPool.Text = "Play Pool:";
            // 
            // lblSkillHiddenMeditate
            // 
            this.lblSkillHiddenMeditate.AutoSize = true;
            this.lblSkillHiddenMeditate.Location = new System.Drawing.Point(27, 115);
            this.lblSkillHiddenMeditate.Name = "lblSkillHiddenMeditate";
            this.lblSkillHiddenMeditate.Size = new System.Drawing.Size(58, 15);
            this.lblSkillHiddenMeditate.TabIndex = 46;
            this.lblSkillHiddenMeditate.Text = "Meditate:";
            // 
            // lblSkillHiddenDance
            // 
            this.lblSkillHiddenDance.AutoSize = true;
            this.lblSkillHiddenDance.Location = new System.Drawing.Point(39, 55);
            this.lblSkillHiddenDance.Name = "lblSkillHiddenDance";
            this.lblSkillHiddenDance.Size = new System.Drawing.Size(46, 15);
            this.lblSkillHiddenDance.TabIndex = 0;
            this.lblSkillHiddenDance.Text = "Dance:";
            // 
            // grpSkillsGeneral
            // 
            this.grpSkillsGeneral.Controls.Add(this.trackSkillRomance);
            this.grpSkillsGeneral.Controls.Add(this.trackSkillMechanical);
            this.grpSkillsGeneral.Controls.Add(this.trackSkillLogic);
            this.grpSkillsGeneral.Controls.Add(this.trackSkillCreativity);
            this.grpSkillsGeneral.Controls.Add(this.trackSkillCooking);
            this.grpSkillsGeneral.Controls.Add(this.trackSkillCleaning);
            this.grpSkillsGeneral.Controls.Add(this.trackSkillCharisma);
            this.grpSkillsGeneral.Controls.Add(this.trackSkillBody);
            this.grpSkillsGeneral.Controls.Add(this.lblSkillRomance);
            this.grpSkillsGeneral.Controls.Add(this.lblSkillMechanical);
            this.grpSkillsGeneral.Controls.Add(this.lblSkillLogic);
            this.grpSkillsGeneral.Controls.Add(this.lblSkillCreativity);
            this.grpSkillsGeneral.Controls.Add(this.lblSkillCooking);
            this.grpSkillsGeneral.Controls.Add(this.lblSkillCleaning);
            this.grpSkillsGeneral.Controls.Add(this.lblSkillCharisma);
            this.grpSkillsGeneral.Controls.Add(this.lblSkillBody);
            this.grpSkillsGeneral.Location = new System.Drawing.Point(0, 8);
            this.grpSkillsGeneral.Name = "grpSkillsGeneral";
            this.grpSkillsGeneral.Size = new System.Drawing.Size(230, 265);
            this.grpSkillsGeneral.TabIndex = 81;
            this.grpSkillsGeneral.TabStop = false;
            this.grpSkillsGeneral.Text = "General Skills";
            // 
            // trackSkillRomance
            // 
            this.trackSkillRomance.Location = new System.Drawing.Point(82, 230);
            this.trackSkillRomance.Margin = new System.Windows.Forms.Padding(0);
            this.trackSkillRomance.Maximum = 1000;
            this.trackSkillRomance.Name = "trackSkillRomance";
            this.trackSkillRomance.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.RomanceSkill;
            this.trackSkillRomance.Size = new System.Drawing.Size(138, 21);
            this.trackSkillRomance.TabIndex = 102;
            this.trackSkillRomance.Tag = "Romance";
            this.trackSkillRomance.TokenGuid = ((uint)(0u));
            this.trackSkillRomance.TokenProp = ((uint)(0u));
            this.trackSkillRomance.Value = ((ushort)(0));
            this.trackSkillRomance.Changed += new System.EventHandler(this.OnGeneralSkillChanged);
            // 
            // trackSkillMechanical
            // 
            this.trackSkillMechanical.Location = new System.Drawing.Point(82, 200);
            this.trackSkillMechanical.Margin = new System.Windows.Forms.Padding(0);
            this.trackSkillMechanical.Maximum = 1000;
            this.trackSkillMechanical.Name = "trackSkillMechanical";
            this.trackSkillMechanical.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.MechanicalSkill;
            this.trackSkillMechanical.Size = new System.Drawing.Size(138, 21);
            this.trackSkillMechanical.TabIndex = 101;
            this.trackSkillMechanical.Tag = "Mechanical";
            this.trackSkillMechanical.TokenGuid = ((uint)(0u));
            this.trackSkillMechanical.TokenProp = ((uint)(0u));
            this.trackSkillMechanical.Value = ((ushort)(0));
            this.trackSkillMechanical.Changed += new System.EventHandler(this.OnGeneralSkillChanged);
            // 
            // trackSkillLogic
            // 
            this.trackSkillLogic.Location = new System.Drawing.Point(82, 170);
            this.trackSkillLogic.Margin = new System.Windows.Forms.Padding(0);
            this.trackSkillLogic.Maximum = 1000;
            this.trackSkillLogic.Name = "trackSkillLogic";
            this.trackSkillLogic.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.LogicSkill;
            this.trackSkillLogic.Size = new System.Drawing.Size(138, 21);
            this.trackSkillLogic.TabIndex = 100;
            this.trackSkillLogic.Tag = "Logic";
            this.trackSkillLogic.TokenGuid = ((uint)(0u));
            this.trackSkillLogic.TokenProp = ((uint)(0u));
            this.trackSkillLogic.Value = ((ushort)(0));
            this.trackSkillLogic.Changed += new System.EventHandler(this.OnGeneralSkillChanged);
            // 
            // trackSkillCreativity
            // 
            this.trackSkillCreativity.Location = new System.Drawing.Point(82, 140);
            this.trackSkillCreativity.Margin = new System.Windows.Forms.Padding(0);
            this.trackSkillCreativity.Maximum = 1000;
            this.trackSkillCreativity.Name = "trackSkillCreativity";
            this.trackSkillCreativity.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.CreativitySkill;
            this.trackSkillCreativity.Size = new System.Drawing.Size(138, 21);
            this.trackSkillCreativity.TabIndex = 99;
            this.trackSkillCreativity.Tag = "Creativity";
            this.trackSkillCreativity.TokenGuid = ((uint)(0u));
            this.trackSkillCreativity.TokenProp = ((uint)(0u));
            this.trackSkillCreativity.Value = ((ushort)(0));
            this.trackSkillCreativity.Changed += new System.EventHandler(this.OnGeneralSkillChanged);
            // 
            // trackSkillCooking
            // 
            this.trackSkillCooking.Location = new System.Drawing.Point(82, 110);
            this.trackSkillCooking.Margin = new System.Windows.Forms.Padding(0);
            this.trackSkillCooking.Maximum = 1000;
            this.trackSkillCooking.Name = "trackSkillCooking";
            this.trackSkillCooking.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.CookingSkill;
            this.trackSkillCooking.Size = new System.Drawing.Size(138, 21);
            this.trackSkillCooking.TabIndex = 98;
            this.trackSkillCooking.Tag = "Cooking";
            this.trackSkillCooking.TokenGuid = ((uint)(0u));
            this.trackSkillCooking.TokenProp = ((uint)(0u));
            this.trackSkillCooking.Value = ((ushort)(0));
            this.trackSkillCooking.Changed += new System.EventHandler(this.OnGeneralSkillChanged);
            // 
            // trackSkillCleaning
            // 
            this.trackSkillCleaning.Location = new System.Drawing.Point(82, 80);
            this.trackSkillCleaning.Margin = new System.Windows.Forms.Padding(0);
            this.trackSkillCleaning.Maximum = 1000;
            this.trackSkillCleaning.Name = "trackSkillCleaning";
            this.trackSkillCleaning.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.CleaningSkill;
            this.trackSkillCleaning.Size = new System.Drawing.Size(138, 21);
            this.trackSkillCleaning.TabIndex = 97;
            this.trackSkillCleaning.Tag = "Cleaning";
            this.trackSkillCleaning.TokenGuid = ((uint)(0u));
            this.trackSkillCleaning.TokenProp = ((uint)(0u));
            this.trackSkillCleaning.Value = ((ushort)(0));
            this.trackSkillCleaning.Changed += new System.EventHandler(this.OnGeneralSkillChanged);
            // 
            // trackSkillCharisma
            // 
            this.trackSkillCharisma.Location = new System.Drawing.Point(82, 50);
            this.trackSkillCharisma.Margin = new System.Windows.Forms.Padding(0);
            this.trackSkillCharisma.Maximum = 1000;
            this.trackSkillCharisma.Name = "trackSkillCharisma";
            this.trackSkillCharisma.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.CharismaSkill;
            this.trackSkillCharisma.Size = new System.Drawing.Size(138, 21);
            this.trackSkillCharisma.TabIndex = 96;
            this.trackSkillCharisma.Tag = "Charisma";
            this.trackSkillCharisma.TokenGuid = ((uint)(0u));
            this.trackSkillCharisma.TokenProp = ((uint)(0u));
            this.trackSkillCharisma.Value = ((ushort)(0));
            this.trackSkillCharisma.Changed += new System.EventHandler(this.OnGeneralSkillChanged);
            // 
            // trackSkillBody
            // 
            this.trackSkillBody.Location = new System.Drawing.Point(82, 20);
            this.trackSkillBody.Margin = new System.Windows.Forms.Padding(0);
            this.trackSkillBody.Maximum = 1000;
            this.trackSkillBody.Name = "trackSkillBody";
            this.trackSkillBody.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.BodySkill;
            this.trackSkillBody.Size = new System.Drawing.Size(138, 21);
            this.trackSkillBody.TabIndex = 95;
            this.trackSkillBody.Tag = "Body";
            this.trackSkillBody.TokenGuid = ((uint)(0u));
            this.trackSkillBody.TokenProp = ((uint)(0u));
            this.trackSkillBody.Value = ((ushort)(0));
            this.trackSkillBody.Changed += new System.EventHandler(this.OnGeneralSkillChanged);
            // 
            // lblSkillRomance
            // 
            this.lblSkillRomance.AutoSize = true;
            this.lblSkillRomance.Location = new System.Drawing.Point(16, 235);
            this.lblSkillRomance.Name = "lblSkillRomance";
            this.lblSkillRomance.Size = new System.Drawing.Size(64, 15);
            this.lblSkillRomance.TabIndex = 52;
            this.lblSkillRomance.Text = "Romance:";
            // 
            // lblSkillMechanical
            // 
            this.lblSkillMechanical.AutoSize = true;
            this.lblSkillMechanical.Location = new System.Drawing.Point(6, 205);
            this.lblSkillMechanical.Name = "lblSkillMechanical";
            this.lblSkillMechanical.Size = new System.Drawing.Size(74, 15);
            this.lblSkillMechanical.TabIndex = 51;
            this.lblSkillMechanical.Text = "Mechanical:";
            // 
            // lblSkillLogic
            // 
            this.lblSkillLogic.AutoSize = true;
            this.lblSkillLogic.Location = new System.Drawing.Point(40, 175);
            this.lblSkillLogic.Name = "lblSkillLogic";
            this.lblSkillLogic.Size = new System.Drawing.Size(40, 15);
            this.lblSkillLogic.TabIndex = 50;
            this.lblSkillLogic.Tag = "";
            this.lblSkillLogic.Text = "Logic:";
            // 
            // lblSkillCreativity
            // 
            this.lblSkillCreativity.AutoSize = true;
            this.lblSkillCreativity.Location = new System.Drawing.Point(22, 145);
            this.lblSkillCreativity.Name = "lblSkillCreativity";
            this.lblSkillCreativity.Size = new System.Drawing.Size(58, 15);
            this.lblSkillCreativity.TabIndex = 49;
            this.lblSkillCreativity.Tag = "";
            this.lblSkillCreativity.Text = "Creativity:";
            // 
            // lblSkillCooking
            // 
            this.lblSkillCooking.AutoSize = true;
            this.lblSkillCooking.Location = new System.Drawing.Point(25, 115);
            this.lblSkillCooking.Name = "lblSkillCooking";
            this.lblSkillCooking.Size = new System.Drawing.Size(55, 15);
            this.lblSkillCooking.TabIndex = 48;
            this.lblSkillCooking.Text = "Cooking:";
            // 
            // lblSkillCleaning
            // 
            this.lblSkillCleaning.AutoSize = true;
            this.lblSkillCleaning.Location = new System.Drawing.Point(21, 85);
            this.lblSkillCleaning.Name = "lblSkillCleaning";
            this.lblSkillCleaning.Size = new System.Drawing.Size(59, 15);
            this.lblSkillCleaning.TabIndex = 47;
            this.lblSkillCleaning.Text = "Cleaning:";
            // 
            // lblSkillCharisma
            // 
            this.lblSkillCharisma.AutoSize = true;
            this.lblSkillCharisma.Location = new System.Drawing.Point(17, 55);
            this.lblSkillCharisma.Name = "lblSkillCharisma";
            this.lblSkillCharisma.Size = new System.Drawing.Size(63, 15);
            this.lblSkillCharisma.TabIndex = 46;
            this.lblSkillCharisma.Text = "Charisma:";
            // 
            // lblSkillBody
            // 
            this.lblSkillBody.AutoSize = true;
            this.lblSkillBody.Location = new System.Drawing.Point(43, 25);
            this.lblSkillBody.Name = "lblSkillBody";
            this.lblSkillBody.Size = new System.Drawing.Size(37, 15);
            this.lblSkillBody.TabIndex = 0;
            this.lblSkillBody.Text = "Body:";
            // 
            // tabInterests
            // 
            this.tabInterests.Controls.Add(this.grpBadges);
            this.tabInterests.Controls.Add(this.grpHobbies);
            this.tabInterests.Controls.Add(this.grpInterests);
            this.tabInterests.Location = new System.Drawing.Point(4, 4);
            this.tabInterests.Margin = new System.Windows.Forms.Padding(0);
            this.tabInterests.Name = "tabInterests";
            this.tabInterests.Size = new System.Drawing.Size(1276, 283);
            this.tabInterests.TabIndex = 5;
            this.tabInterests.Text = "Interests";
            this.tabInterests.UseVisualStyleBackColor = true;
            // 
            // grpBadges
            // 
            this.grpBadges.Controls.Add(this.trackBadgeStocking);
            this.grpBadges.Controls.Add(this.trackBadgePottery);
            this.grpBadges.Controls.Add(this.trackBadgeSewing);
            this.grpBadges.Controls.Add(this.lblBadgeToyMaking);
            this.grpBadges.Controls.Add(this.trackBadgeSales);
            this.grpBadges.Controls.Add(this.trackBadgeFlorist);
            this.grpBadges.Controls.Add(this.trackBadgeRobotery);
            this.grpBadges.Controls.Add(this.trackBadgeToyMaking);
            this.grpBadges.Controls.Add(this.lblBadgeStocking);
            this.grpBadges.Controls.Add(this.trackBadgeFishing);
            this.grpBadges.Controls.Add(this.lblBadgeSewing);
            this.grpBadges.Controls.Add(this.trackBadgeGardening);
            this.grpBadges.Controls.Add(this.lblBadgeSales);
            this.grpBadges.Controls.Add(this.trackBadgeCashier);
            this.grpBadges.Controls.Add(this.trackBadgeCosmetics);
            this.grpBadges.Controls.Add(this.lblBadgeRobotery);
            this.grpBadges.Controls.Add(this.lblBadgePottery);
            this.grpBadges.Controls.Add(this.lblBadgeFlorist);
            this.grpBadges.Controls.Add(this.lblBadgeFishing);
            this.grpBadges.Controls.Add(this.lblBadgeGardening);
            this.grpBadges.Controls.Add(this.lblBadgeCosmetics);
            this.grpBadges.Controls.Add(this.lblBadgeCashier);
            this.grpBadges.Location = new System.Drawing.Point(840, 8);
            this.grpBadges.Name = "grpBadges";
            this.grpBadges.Size = new System.Drawing.Size(430, 265);
            this.grpBadges.TabIndex = 82;
            this.grpBadges.TabStop = false;
            this.grpBadges.Text = "Badges";
            // 
            // trackBadgeStocking
            // 
            this.trackBadgeStocking.Location = new System.Drawing.Point(282, 110);
            this.trackBadgeStocking.Margin = new System.Windows.Forms.Padding(0);
            this.trackBadgeStocking.Name = "trackBadgeStocking";
            this.trackBadgeStocking.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.eSport;
            this.trackBadgeStocking.Size = new System.Drawing.Size(138, 21);
            this.trackBadgeStocking.TabIndex = 97;
            this.trackBadgeStocking.Tag = "Restocking";
            this.trackBadgeStocking.TokenGuid = ((uint)(274717778u));
            this.trackBadgeStocking.Value = ((ushort)(0));
            this.trackBadgeStocking.Changed += new System.EventHandler(this.OnBadgeChanged);
            // 
            // trackBadgePottery
            // 
            this.trackBadgePottery.Location = new System.Drawing.Point(76, 170);
            this.trackBadgePottery.Margin = new System.Windows.Forms.Padding(0);
            this.trackBadgePottery.Name = "trackBadgePottery";
            this.trackBadgePottery.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.eMusic;
            this.trackBadgePottery.Size = new System.Drawing.Size(138, 21);
            this.trackBadgePottery.TabIndex = 98;
            this.trackBadgePottery.Tag = "Pottery";
            this.trackBadgePottery.TokenGuid = ((uint)(4091132122u));
            this.trackBadgePottery.Value = ((ushort)(0));
            this.trackBadgePottery.Changed += new System.EventHandler(this.OnBadgeChanged);
            // 
            // trackBadgeSewing
            // 
            this.trackBadgeSewing.Location = new System.Drawing.Point(282, 80);
            this.trackBadgeSewing.Margin = new System.Windows.Forms.Padding(0);
            this.trackBadgeSewing.Name = "trackBadgeSewing";
            this.trackBadgeSewing.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.eUnused;
            this.trackBadgeSewing.Size = new System.Drawing.Size(138, 21);
            this.trackBadgeSewing.TabIndex = 96;
            this.trackBadgeSewing.Tag = "Sewing";
            this.trackBadgeSewing.TokenGuid = ((uint)(3554355751u));
            this.trackBadgeSewing.Value = ((ushort)(0));
            this.trackBadgeSewing.Changed += new System.EventHandler(this.OnBadgeChanged);
            // 
            // lblBadgeToyMaking
            // 
            this.lblBadgeToyMaking.AutoSize = true;
            this.lblBadgeToyMaking.Location = new System.Drawing.Point(245, 145);
            this.lblBadgeToyMaking.Name = "lblBadgeToyMaking";
            this.lblBadgeToyMaking.Size = new System.Drawing.Size(35, 15);
            this.lblBadgeToyMaking.TabIndex = 79;
            this.lblBadgeToyMaking.Tag = "";
            this.lblBadgeToyMaking.Text = "Toys:";
            // 
            // trackBadgeSales
            // 
            this.trackBadgeSales.Location = new System.Drawing.Point(282, 50);
            this.trackBadgeSales.Margin = new System.Windows.Forms.Padding(0);
            this.trackBadgeSales.Name = "trackBadgeSales";
            this.trackBadgeSales.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.eScience;
            this.trackBadgeSales.Size = new System.Drawing.Size(138, 21);
            this.trackBadgeSales.TabIndex = 95;
            this.trackBadgeSales.Tag = "Sales";
            this.trackBadgeSales.TokenGuid = ((uint)(2959072283u));
            this.trackBadgeSales.Value = ((ushort)(0));
            this.trackBadgeSales.Changed += new System.EventHandler(this.OnBadgeChanged);
            // 
            // trackBadgeFlorist
            // 
            this.trackBadgeFlorist.Location = new System.Drawing.Point(76, 110);
            this.trackBadgeFlorist.Margin = new System.Windows.Forms.Padding(0);
            this.trackBadgeFlorist.Name = "trackBadgeFlorist";
            this.trackBadgeFlorist.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.eGames;
            this.trackBadgeFlorist.Size = new System.Drawing.Size(138, 21);
            this.trackBadgeFlorist.TabIndex = 97;
            this.trackBadgeFlorist.Tag = "Flower Arranging";
            this.trackBadgeFlorist.TokenGuid = ((uint)(2422201269u));
            this.trackBadgeFlorist.Value = ((ushort)(0));
            this.trackBadgeFlorist.Changed += new System.EventHandler(this.OnBadgeChanged);
            // 
            // trackBadgeRobotery
            // 
            this.trackBadgeRobotery.Location = new System.Drawing.Point(282, 20);
            this.trackBadgeRobotery.Margin = new System.Windows.Forms.Padding(0);
            this.trackBadgeRobotery.Name = "trackBadgeRobotery";
            this.trackBadgeRobotery.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.eNature;
            this.trackBadgeRobotery.Size = new System.Drawing.Size(138, 21);
            this.trackBadgeRobotery.TabIndex = 94;
            this.trackBadgeRobotery.Tag = "Robot Making";
            this.trackBadgeRobotery.TokenGuid = ((uint)(1348459462u));
            this.trackBadgeRobotery.Value = ((ushort)(0));
            this.trackBadgeRobotery.Changed += new System.EventHandler(this.OnBadgeChanged);
            // 
            // trackBadgeToyMaking
            // 
            this.trackBadgeToyMaking.Location = new System.Drawing.Point(282, 140);
            this.trackBadgeToyMaking.Margin = new System.Windows.Forms.Padding(0);
            this.trackBadgeToyMaking.Name = "trackBadgeToyMaking";
            this.trackBadgeToyMaking.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.eTinkering;
            this.trackBadgeToyMaking.Size = new System.Drawing.Size(138, 21);
            this.trackBadgeToyMaking.TabIndex = 93;
            this.trackBadgeToyMaking.Tag = "Toy Making";
            this.trackBadgeToyMaking.TokenGuid = ((uint)(3495943057u));
            this.trackBadgeToyMaking.Value = ((ushort)(0));
            this.trackBadgeToyMaking.Changed += new System.EventHandler(this.OnBadgeChanged);
            // 
            // lblBadgeStocking
            // 
            this.lblBadgeStocking.AutoSize = true;
            this.lblBadgeStocking.Location = new System.Drawing.Point(223, 115);
            this.lblBadgeStocking.Name = "lblBadgeStocking";
            this.lblBadgeStocking.Size = new System.Drawing.Size(57, 15);
            this.lblBadgeStocking.TabIndex = 78;
            this.lblBadgeStocking.Text = "Stocking:";
            // 
            // trackBadgeFishing
            // 
            this.trackBadgeFishing.Location = new System.Drawing.Point(76, 80);
            this.trackBadgeFishing.Margin = new System.Windows.Forms.Padding(0);
            this.trackBadgeFishing.Name = "trackBadgeFishing";
            this.trackBadgeFishing.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.eFitness;
            this.trackBadgeFishing.Size = new System.Drawing.Size(138, 21);
            this.trackBadgeFishing.TabIndex = 96;
            this.trackBadgeFishing.Tag = "Fishing";
            this.trackBadgeFishing.TokenGuid = ((uint)(304705879u));
            this.trackBadgeFishing.Value = ((ushort)(0));
            this.trackBadgeFishing.Changed += new System.EventHandler(this.OnBadgeChanged);
            // 
            // lblBadgeSewing
            // 
            this.lblBadgeSewing.AutoSize = true;
            this.lblBadgeSewing.Location = new System.Drawing.Point(229, 85);
            this.lblBadgeSewing.Name = "lblBadgeSewing";
            this.lblBadgeSewing.Size = new System.Drawing.Size(51, 15);
            this.lblBadgeSewing.TabIndex = 77;
            this.lblBadgeSewing.Text = "Sewing:";
            // 
            // trackBadgeGardening
            // 
            this.trackBadgeGardening.Location = new System.Drawing.Point(76, 140);
            this.trackBadgeGardening.Margin = new System.Windows.Forms.Padding(0);
            this.trackBadgeGardening.Name = "trackBadgeGardening";
            this.trackBadgeGardening.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.eFilmLit;
            this.trackBadgeGardening.Size = new System.Drawing.Size(138, 21);
            this.trackBadgeGardening.TabIndex = 95;
            this.trackBadgeGardening.Tag = "Gardening";
            this.trackBadgeGardening.TokenGuid = ((uint)(303409345u));
            this.trackBadgeGardening.Value = ((ushort)(0));
            this.trackBadgeGardening.Changed += new System.EventHandler(this.OnBadgeChanged);
            // 
            // lblBadgeSales
            // 
            this.lblBadgeSales.AutoSize = true;
            this.lblBadgeSales.Location = new System.Drawing.Point(239, 55);
            this.lblBadgeSales.Name = "lblBadgeSales";
            this.lblBadgeSales.Size = new System.Drawing.Size(41, 15);
            this.lblBadgeSales.TabIndex = 76;
            this.lblBadgeSales.Text = "Sales:";
            // 
            // trackBadgeCashier
            // 
            this.trackBadgeCashier.Location = new System.Drawing.Point(76, 20);
            this.trackBadgeCashier.Margin = new System.Windows.Forms.Padding(0);
            this.trackBadgeCashier.Name = "trackBadgeCashier";
            this.trackBadgeCashier.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.eArts;
            this.trackBadgeCashier.Size = new System.Drawing.Size(138, 21);
            this.trackBadgeCashier.TabIndex = 94;
            this.trackBadgeCashier.Tag = "Cash Register";
            this.trackBadgeCashier.TokenGuid = ((uint)(1348459560u));
            this.trackBadgeCashier.Value = ((ushort)(0));
            this.trackBadgeCashier.Changed += new System.EventHandler(this.OnBadgeChanged);
            // 
            // trackBadgeCosmetics
            // 
            this.trackBadgeCosmetics.Location = new System.Drawing.Point(76, 50);
            this.trackBadgeCosmetics.Margin = new System.Windows.Forms.Padding(0);
            this.trackBadgeCosmetics.Name = "trackBadgeCosmetics";
            this.trackBadgeCosmetics.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.eCuisine;
            this.trackBadgeCosmetics.Size = new System.Drawing.Size(138, 21);
            this.trackBadgeCosmetics.TabIndex = 93;
            this.trackBadgeCosmetics.Tag = "Cosmetology";
            this.trackBadgeCosmetics.TokenGuid = ((uint)(1885330575u));
            this.trackBadgeCosmetics.Value = ((ushort)(0));
            this.trackBadgeCosmetics.Changed += new System.EventHandler(this.OnBadgeChanged);
            // 
            // lblBadgeRobotery
            // 
            this.lblBadgeRobotery.AutoSize = true;
            this.lblBadgeRobotery.Location = new System.Drawing.Point(222, 25);
            this.lblBadgeRobotery.Name = "lblBadgeRobotery";
            this.lblBadgeRobotery.Size = new System.Drawing.Size(58, 15);
            this.lblBadgeRobotery.TabIndex = 75;
            this.lblBadgeRobotery.Text = "Robotics:";
            // 
            // lblBadgePottery
            // 
            this.lblBadgePottery.AutoSize = true;
            this.lblBadgePottery.Location = new System.Drawing.Point(27, 175);
            this.lblBadgePottery.Name = "lblBadgePottery";
            this.lblBadgePottery.Size = new System.Drawing.Size(47, 15);
            this.lblBadgePottery.TabIndex = 53;
            this.lblBadgePottery.Tag = "";
            this.lblBadgePottery.Text = "Pottery:";
            // 
            // lblBadgeFlorist
            // 
            this.lblBadgeFlorist.AutoSize = true;
            this.lblBadgeFlorist.Location = new System.Drawing.Point(31, 115);
            this.lblBadgeFlorist.Name = "lblBadgeFlorist";
            this.lblBadgeFlorist.Size = new System.Drawing.Size(43, 15);
            this.lblBadgeFlorist.TabIndex = 52;
            this.lblBadgeFlorist.Tag = "";
            this.lblBadgeFlorist.Text = "Florist:";
            // 
            // lblBadgeFishing
            // 
            this.lblBadgeFishing.AutoSize = true;
            this.lblBadgeFishing.Location = new System.Drawing.Point(24, 85);
            this.lblBadgeFishing.Name = "lblBadgeFishing";
            this.lblBadgeFishing.Size = new System.Drawing.Size(50, 15);
            this.lblBadgeFishing.TabIndex = 51;
            this.lblBadgeFishing.Text = "Fishing:";
            // 
            // lblBadgeGardening
            // 
            this.lblBadgeGardening.AutoSize = true;
            this.lblBadgeGardening.Location = new System.Drawing.Point(6, 145);
            this.lblBadgeGardening.Name = "lblBadgeGardening";
            this.lblBadgeGardening.Size = new System.Drawing.Size(68, 15);
            this.lblBadgeGardening.TabIndex = 50;
            this.lblBadgeGardening.Text = "Gardening:";
            // 
            // lblBadgeCosmetics
            // 
            this.lblBadgeCosmetics.AutoSize = true;
            this.lblBadgeCosmetics.Location = new System.Drawing.Point(7, 55);
            this.lblBadgeCosmetics.Name = "lblBadgeCosmetics";
            this.lblBadgeCosmetics.Size = new System.Drawing.Size(67, 15);
            this.lblBadgeCosmetics.TabIndex = 49;
            this.lblBadgeCosmetics.Text = "Cosmetics:";
            // 
            // lblBadgeCashier
            // 
            this.lblBadgeCashier.AutoSize = true;
            this.lblBadgeCashier.Location = new System.Drawing.Point(22, 25);
            this.lblBadgeCashier.Name = "lblBadgeCashier";
            this.lblBadgeCashier.Size = new System.Drawing.Size(52, 15);
            this.lblBadgeCashier.TabIndex = 48;
            this.lblBadgeCashier.Text = "Cashier:";
            // 
            // grpHobbies
            // 
            this.grpHobbies.Controls.Add(this.trackHobbySport);
            this.grpHobbies.Controls.Add(this.trackHobbySecret);
            this.grpHobbies.Controls.Add(this.trackHobbyScience);
            this.grpHobbies.Controls.Add(this.trackHobbyMusic);
            this.grpHobbies.Controls.Add(this.trackHobbyGames);
            this.grpHobbies.Controls.Add(this.trackHobbyFitness);
            this.grpHobbies.Controls.Add(this.trackHobbyFilm);
            this.grpHobbies.Controls.Add(this.trackHobbyArts);
            this.grpHobbies.Controls.Add(this.trackHobbyNature);
            this.grpHobbies.Controls.Add(this.trackHobbyCuisine);
            this.grpHobbies.Controls.Add(this.trackHobbyTinker);
            this.grpHobbies.Controls.Add(this.comboHobbyOneTrue);
            this.grpHobbies.Controls.Add(this.lblHobbyOneTrue);
            this.grpHobbies.Controls.Add(this.lblHobbyScience);
            this.grpHobbies.Controls.Add(this.lblHobbyMusic);
            this.grpHobbies.Controls.Add(this.lblHobbyFilm);
            this.grpHobbies.Controls.Add(this.lblHobbySport);
            this.grpHobbies.Controls.Add(this.lblHobbyArts);
            this.grpHobbies.Controls.Add(this.lblHobbySecret);
            this.grpHobbies.Controls.Add(this.lblHobbyGames);
            this.grpHobbies.Controls.Add(this.lblHobbyFitness);
            this.grpHobbies.Controls.Add(this.lblHobbyNature);
            this.grpHobbies.Controls.Add(this.lblHobbyTinker);
            this.grpHobbies.Controls.Add(this.lblHobbyCuisine);
            this.grpHobbies.Location = new System.Drawing.Point(420, 8);
            this.grpHobbies.Name = "grpHobbies";
            this.grpHobbies.Size = new System.Drawing.Size(410, 265);
            this.grpHobbies.TabIndex = 81;
            this.grpHobbies.TabStop = false;
            this.grpHobbies.Text = "Hobbies";
            // 
            // trackHobbySport
            // 
            this.trackHobbySport.Location = new System.Drawing.Point(262, 110);
            this.trackHobbySport.Margin = new System.Windows.Forms.Padding(0);
            this.trackHobbySport.Name = "trackHobbySport";
            this.trackHobbySport.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.eSport;
            this.trackHobbySport.Size = new System.Drawing.Size(138, 21);
            this.trackHobbySport.TabIndex = 92;
            this.trackHobbySport.Tag = "Sports";
            this.trackHobbySport.TokenGuid = ((uint)(0u));
            this.trackHobbySport.Value = ((ushort)(0));
            this.trackHobbySport.Changed += new System.EventHandler(this.OnHobbyChanged);
            // 
            // trackHobbySecret
            // 
            this.trackHobbySecret.Location = new System.Drawing.Point(262, 80);
            this.trackHobbySecret.Margin = new System.Windows.Forms.Padding(0);
            this.trackHobbySecret.Name = "trackHobbySecret";
            this.trackHobbySecret.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.eUnused;
            this.trackHobbySecret.Size = new System.Drawing.Size(138, 21);
            this.trackHobbySecret.TabIndex = 91;
            this.trackHobbySecret.Tag = "Secret (Unused)";
            this.trackHobbySecret.TokenGuid = ((uint)(0u));
            this.trackHobbySecret.Value = ((ushort)(0));
            this.trackHobbySecret.Changed += new System.EventHandler(this.OnHobbyChanged);
            // 
            // trackHobbyScience
            // 
            this.trackHobbyScience.Location = new System.Drawing.Point(262, 50);
            this.trackHobbyScience.Margin = new System.Windows.Forms.Padding(0);
            this.trackHobbyScience.Name = "trackHobbyScience";
            this.trackHobbyScience.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.eScience;
            this.trackHobbyScience.Size = new System.Drawing.Size(138, 21);
            this.trackHobbyScience.TabIndex = 90;
            this.trackHobbyScience.Tag = "Science";
            this.trackHobbyScience.TokenGuid = ((uint)(0u));
            this.trackHobbyScience.Value = ((ushort)(0));
            this.trackHobbyScience.Changed += new System.EventHandler(this.OnHobbyChanged);
            // 
            // trackHobbyMusic
            // 
            this.trackHobbyMusic.Location = new System.Drawing.Point(60, 170);
            this.trackHobbyMusic.Margin = new System.Windows.Forms.Padding(0);
            this.trackHobbyMusic.Name = "trackHobbyMusic";
            this.trackHobbyMusic.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.eMusic;
            this.trackHobbyMusic.Size = new System.Drawing.Size(138, 21);
            this.trackHobbyMusic.TabIndex = 89;
            this.trackHobbyMusic.Tag = "Music and Dance";
            this.trackHobbyMusic.TokenGuid = ((uint)(0u));
            this.trackHobbyMusic.Value = ((ushort)(0));
            this.trackHobbyMusic.Changed += new System.EventHandler(this.OnHobbyChanged);
            // 
            // trackHobbyGames
            // 
            this.trackHobbyGames.Location = new System.Drawing.Point(60, 140);
            this.trackHobbyGames.Margin = new System.Windows.Forms.Padding(0);
            this.trackHobbyGames.Name = "trackHobbyGames";
            this.trackHobbyGames.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.eGames;
            this.trackHobbyGames.Size = new System.Drawing.Size(138, 21);
            this.trackHobbyGames.TabIndex = 88;
            this.trackHobbyGames.Tag = "Games";
            this.trackHobbyGames.TokenGuid = ((uint)(0u));
            this.trackHobbyGames.Value = ((ushort)(0));
            this.trackHobbyGames.Changed += new System.EventHandler(this.OnHobbyChanged);
            // 
            // trackHobbyFitness
            // 
            this.trackHobbyFitness.Location = new System.Drawing.Point(60, 110);
            this.trackHobbyFitness.Margin = new System.Windows.Forms.Padding(0);
            this.trackHobbyFitness.Name = "trackHobbyFitness";
            this.trackHobbyFitness.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.eFitness;
            this.trackHobbyFitness.Size = new System.Drawing.Size(138, 21);
            this.trackHobbyFitness.TabIndex = 87;
            this.trackHobbyFitness.Tag = "Fitness";
            this.trackHobbyFitness.TokenGuid = ((uint)(0u));
            this.trackHobbyFitness.Value = ((ushort)(0));
            this.trackHobbyFitness.Changed += new System.EventHandler(this.OnHobbyChanged);
            // 
            // trackHobbyFilm
            // 
            this.trackHobbyFilm.Location = new System.Drawing.Point(60, 80);
            this.trackHobbyFilm.Margin = new System.Windows.Forms.Padding(0);
            this.trackHobbyFilm.Name = "trackHobbyFilm";
            this.trackHobbyFilm.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.eFilmLit;
            this.trackHobbyFilm.Size = new System.Drawing.Size(138, 21);
            this.trackHobbyFilm.TabIndex = 86;
            this.trackHobbyFilm.Tag = "Film and Literature";
            this.trackHobbyFilm.TokenGuid = ((uint)(0u));
            this.trackHobbyFilm.Value = ((ushort)(0));
            this.trackHobbyFilm.Changed += new System.EventHandler(this.OnHobbyChanged);
            // 
            // trackHobbyArts
            // 
            this.trackHobbyArts.Location = new System.Drawing.Point(60, 20);
            this.trackHobbyArts.Margin = new System.Windows.Forms.Padding(0);
            this.trackHobbyArts.Name = "trackHobbyArts";
            this.trackHobbyArts.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.eArts;
            this.trackHobbyArts.Size = new System.Drawing.Size(138, 21);
            this.trackHobbyArts.TabIndex = 85;
            this.trackHobbyArts.Tag = "Arts and Crafts";
            this.trackHobbyArts.TokenGuid = ((uint)(0u));
            this.trackHobbyArts.Value = ((ushort)(0));
            this.trackHobbyArts.Changed += new System.EventHandler(this.OnHobbyChanged);
            // 
            // trackHobbyNature
            // 
            this.trackHobbyNature.Location = new System.Drawing.Point(262, 20);
            this.trackHobbyNature.Margin = new System.Windows.Forms.Padding(0);
            this.trackHobbyNature.Name = "trackHobbyNature";
            this.trackHobbyNature.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.eNature;
            this.trackHobbyNature.Size = new System.Drawing.Size(138, 21);
            this.trackHobbyNature.TabIndex = 84;
            this.trackHobbyNature.Tag = "Nature";
            this.trackHobbyNature.TokenGuid = ((uint)(0u));
            this.trackHobbyNature.Value = ((ushort)(0));
            this.trackHobbyNature.Changed += new System.EventHandler(this.OnHobbyChanged);
            // 
            // trackHobbyCuisine
            // 
            this.trackHobbyCuisine.Location = new System.Drawing.Point(60, 50);
            this.trackHobbyCuisine.Margin = new System.Windows.Forms.Padding(0);
            this.trackHobbyCuisine.Name = "trackHobbyCuisine";
            this.trackHobbyCuisine.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.eCuisine;
            this.trackHobbyCuisine.Size = new System.Drawing.Size(138, 21);
            this.trackHobbyCuisine.TabIndex = 83;
            this.trackHobbyCuisine.Tag = "Cuisine";
            this.trackHobbyCuisine.TokenGuid = ((uint)(0u));
            this.trackHobbyCuisine.Value = ((ushort)(0));
            this.trackHobbyCuisine.Changed += new System.EventHandler(this.OnHobbyChanged);
            // 
            // trackHobbyTinker
            // 
            this.trackHobbyTinker.Location = new System.Drawing.Point(262, 140);
            this.trackHobbyTinker.Margin = new System.Windows.Forms.Padding(0);
            this.trackHobbyTinker.Name = "trackHobbyTinker";
            this.trackHobbyTinker.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.eTinkering;
            this.trackHobbyTinker.Size = new System.Drawing.Size(138, 21);
            this.trackHobbyTinker.TabIndex = 82;
            this.trackHobbyTinker.Tag = "Tinkering";
            this.trackHobbyTinker.TokenGuid = ((uint)(0u));
            this.trackHobbyTinker.Value = ((ushort)(0));
            this.trackHobbyTinker.Changed += new System.EventHandler(this.OnHobbyChanged);
            // 
            // comboHobbyOneTrue
            // 
            this.comboHobbyOneTrue.FormattingEnabled = true;
            this.comboHobbyOneTrue.Location = new System.Drawing.Point(60, 210);
            this.comboHobbyOneTrue.Name = "comboHobbyOneTrue";
            this.comboHobbyOneTrue.Size = new System.Drawing.Size(138, 23);
            this.comboHobbyOneTrue.TabIndex = 81;
            this.comboHobbyOneTrue.Text = "Arts & Crafts";
            this.comboHobbyOneTrue.SelectedIndexChanged += new System.EventHandler(this.OnOneTrueHobbyChanged);
            // 
            // lblHobbyOneTrue
            // 
            this.lblHobbyOneTrue.AutoSize = true;
            this.lblHobbyOneTrue.Location = new System.Drawing.Point(23, 213);
            this.lblHobbyOneTrue.Name = "lblHobbyOneTrue";
            this.lblHobbyOneTrue.Size = new System.Drawing.Size(35, 15);
            this.lblHobbyOneTrue.TabIndex = 80;
            this.lblHobbyOneTrue.Text = "OTH:";
            // 
            // lblHobbyScience
            // 
            this.lblHobbyScience.AutoSize = true;
            this.lblHobbyScience.Location = new System.Drawing.Point(206, 55);
            this.lblHobbyScience.Name = "lblHobbyScience";
            this.lblHobbyScience.Size = new System.Drawing.Size(54, 15);
            this.lblHobbyScience.TabIndex = 79;
            this.lblHobbyScience.Text = "Science:";
            // 
            // lblHobbyMusic
            // 
            this.lblHobbyMusic.AutoSize = true;
            this.lblHobbyMusic.Location = new System.Drawing.Point(15, 175);
            this.lblHobbyMusic.Name = "lblHobbyMusic";
            this.lblHobbyMusic.Size = new System.Drawing.Size(43, 15);
            this.lblHobbyMusic.TabIndex = 78;
            this.lblHobbyMusic.Text = "Music:";
            // 
            // lblHobbyFilm
            // 
            this.lblHobbyFilm.AutoSize = true;
            this.lblHobbyFilm.Location = new System.Drawing.Point(24, 85);
            this.lblHobbyFilm.Name = "lblHobbyFilm";
            this.lblHobbyFilm.Size = new System.Drawing.Size(34, 15);
            this.lblHobbyFilm.TabIndex = 77;
            this.lblHobbyFilm.Text = "Film:";
            // 
            // lblHobbySport
            // 
            this.lblHobbySport.AutoSize = true;
            this.lblHobbySport.Location = new System.Drawing.Point(215, 115);
            this.lblHobbySport.Name = "lblHobbySport";
            this.lblHobbySport.Size = new System.Drawing.Size(45, 15);
            this.lblHobbySport.TabIndex = 76;
            this.lblHobbySport.Text = "Sports:";
            // 
            // lblHobbyArts
            // 
            this.lblHobbyArts.AutoSize = true;
            this.lblHobbyArts.Location = new System.Drawing.Point(28, 25);
            this.lblHobbyArts.Name = "lblHobbyArts";
            this.lblHobbyArts.Size = new System.Drawing.Size(30, 15);
            this.lblHobbyArts.TabIndex = 75;
            this.lblHobbyArts.Text = "Arts:";
            // 
            // lblHobbySecret
            // 
            this.lblHobbySecret.AutoSize = true;
            this.lblHobbySecret.Location = new System.Drawing.Point(215, 85);
            this.lblHobbySecret.Name = "lblHobbySecret";
            this.lblHobbySecret.Size = new System.Drawing.Size(45, 15);
            this.lblHobbySecret.TabIndex = 53;
            this.lblHobbySecret.Text = "Secret:";
            // 
            // lblHobbyGames
            // 
            this.lblHobbyGames.AutoSize = true;
            this.lblHobbyGames.Location = new System.Drawing.Point(8, 145);
            this.lblHobbyGames.Name = "lblHobbyGames";
            this.lblHobbyGames.Size = new System.Drawing.Size(50, 15);
            this.lblHobbyGames.TabIndex = 52;
            this.lblHobbyGames.Text = "Games:";
            // 
            // lblHobbyFitness
            // 
            this.lblHobbyFitness.AutoSize = true;
            this.lblHobbyFitness.Location = new System.Drawing.Point(9, 115);
            this.lblHobbyFitness.Name = "lblHobbyFitness";
            this.lblHobbyFitness.Size = new System.Drawing.Size(49, 15);
            this.lblHobbyFitness.TabIndex = 51;
            this.lblHobbyFitness.Text = "Fitness:";
            // 
            // lblHobbyNature
            // 
            this.lblHobbyNature.AutoSize = true;
            this.lblHobbyNature.Location = new System.Drawing.Point(213, 25);
            this.lblHobbyNature.Name = "lblHobbyNature";
            this.lblHobbyNature.Size = new System.Drawing.Size(47, 15);
            this.lblHobbyNature.TabIndex = 50;
            this.lblHobbyNature.Text = "Nature:";
            // 
            // lblHobbyTinker
            // 
            this.lblHobbyTinker.AutoSize = true;
            this.lblHobbyTinker.Location = new System.Drawing.Point(216, 145);
            this.lblHobbyTinker.Name = "lblHobbyTinker";
            this.lblHobbyTinker.Size = new System.Drawing.Size(44, 15);
            this.lblHobbyTinker.TabIndex = 49;
            this.lblHobbyTinker.Text = "Tinker:";
            // 
            // lblHobbyCuisine
            // 
            this.lblHobbyCuisine.AutoSize = true;
            this.lblHobbyCuisine.Location = new System.Drawing.Point(7, 55);
            this.lblHobbyCuisine.Name = "lblHobbyCuisine";
            this.lblHobbyCuisine.Size = new System.Drawing.Size(51, 15);
            this.lblHobbyCuisine.TabIndex = 48;
            this.lblHobbyCuisine.Text = "Cuisine:";
            // 
            // grpInterests
            // 
            this.grpInterests.Controls.Add(this.trackIntWork);
            this.grpInterests.Controls.Add(this.trackIntWeather);
            this.grpInterests.Controls.Add(this.trackIntTravel);
            this.grpInterests.Controls.Add(this.trackIntToys);
            this.grpInterests.Controls.Add(this.trackIntSports);
            this.grpInterests.Controls.Add(this.trackIntSciFi);
            this.grpInterests.Controls.Add(this.trackIntSchool);
            this.grpInterests.Controls.Add(this.trackIntParanormal);
            this.grpInterests.Controls.Add(this.trackIntPolitics);
            this.grpInterests.Controls.Add(this.trackIntMoney);
            this.grpInterests.Controls.Add(this.trackIntHealth);
            this.grpInterests.Controls.Add(this.trackIntFood);
            this.grpInterests.Controls.Add(this.lblIntAnimals);
            this.grpInterests.Controls.Add(this.trackIntFashion);
            this.grpInterests.Controls.Add(this.lblIntWork);
            this.grpInterests.Controls.Add(this.trackIntEnvironment);
            this.grpInterests.Controls.Add(this.lblIntWeather);
            this.grpInterests.Controls.Add(this.trackIntEntertainment);
            this.grpInterests.Controls.Add(this.lblIntTravel);
            this.grpInterests.Controls.Add(this.trackIntCulture);
            this.grpInterests.Controls.Add(this.lblIntToys);
            this.grpInterests.Controls.Add(this.trackIntAnimals);
            this.grpInterests.Controls.Add(this.trackIntCrime);
            this.grpInterests.Controls.Add(this.lblIntSports);
            this.grpInterests.Controls.Add(this.lblIntSciFi);
            this.grpInterests.Controls.Add(this.lblIntSchool);
            this.grpInterests.Controls.Add(this.lblIntPolitics);
            this.grpInterests.Controls.Add(this.lblIntParanormal);
            this.grpInterests.Controls.Add(this.lblIntMoney);
            this.grpInterests.Controls.Add(this.lblIntHealth);
            this.grpInterests.Controls.Add(this.lblIntFood);
            this.grpInterests.Controls.Add(this.lblIntFashion);
            this.grpInterests.Controls.Add(this.lblIntEnvironment);
            this.grpInterests.Controls.Add(this.lblIntEntertainment);
            this.grpInterests.Controls.Add(this.lblIntCulture);
            this.grpInterests.Controls.Add(this.lblIntCrime);
            this.grpInterests.Location = new System.Drawing.Point(0, 8);
            this.grpInterests.Name = "grpInterests";
            this.grpInterests.Size = new System.Drawing.Size(410, 265);
            this.grpInterests.TabIndex = 0;
            this.grpInterests.TabStop = false;
            this.grpInterests.Text = "Interests";
            // 
            // trackIntWork
            // 
            this.trackIntWork.Location = new System.Drawing.Point(262, 236);
            this.trackIntWork.Margin = new System.Windows.Forms.Padding(0);
            this.trackIntWork.Name = "trackIntWork";
            this.trackIntWork.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.iWork;
            this.trackIntWork.Size = new System.Drawing.Size(138, 21);
            this.trackIntWork.TabIndex = 111;
            this.trackIntWork.Tag = "Work";
            this.trackIntWork.TokenGuid = ((uint)(0u));
            this.trackIntWork.Value = ((ushort)(0));
            this.trackIntWork.Changed += new System.EventHandler(this.OnInterestChanged);
            // 
            // trackIntWeather
            // 
            this.trackIntWeather.Location = new System.Drawing.Point(262, 209);
            this.trackIntWeather.Margin = new System.Windows.Forms.Padding(0);
            this.trackIntWeather.Name = "trackIntWeather";
            this.trackIntWeather.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.iWeather;
            this.trackIntWeather.Size = new System.Drawing.Size(138, 21);
            this.trackIntWeather.TabIndex = 110;
            this.trackIntWeather.Tag = "Weather";
            this.trackIntWeather.TokenGuid = ((uint)(0u));
            this.trackIntWeather.Value = ((ushort)(0));
            this.trackIntWeather.Changed += new System.EventHandler(this.OnInterestChanged);
            // 
            // trackIntTravel
            // 
            this.trackIntTravel.Location = new System.Drawing.Point(262, 182);
            this.trackIntTravel.Margin = new System.Windows.Forms.Padding(0);
            this.trackIntTravel.Name = "trackIntTravel";
            this.trackIntTravel.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.iTravel;
            this.trackIntTravel.Size = new System.Drawing.Size(138, 21);
            this.trackIntTravel.TabIndex = 109;
            this.trackIntTravel.Tag = "Travel";
            this.trackIntTravel.TokenGuid = ((uint)(0u));
            this.trackIntTravel.Value = ((ushort)(0));
            this.trackIntTravel.Changed += new System.EventHandler(this.OnInterestChanged);
            // 
            // trackIntToys
            // 
            this.trackIntToys.Location = new System.Drawing.Point(262, 153);
            this.trackIntToys.Margin = new System.Windows.Forms.Padding(0);
            this.trackIntToys.Name = "trackIntToys";
            this.trackIntToys.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.iToys;
            this.trackIntToys.Size = new System.Drawing.Size(138, 21);
            this.trackIntToys.TabIndex = 108;
            this.trackIntToys.Tag = "Toys";
            this.trackIntToys.TokenGuid = ((uint)(0u));
            this.trackIntToys.Value = ((ushort)(0));
            this.trackIntToys.Changed += new System.EventHandler(this.OnInterestChanged);
            // 
            // trackIntSports
            // 
            this.trackIntSports.Location = new System.Drawing.Point(262, 128);
            this.trackIntSports.Margin = new System.Windows.Forms.Padding(0);
            this.trackIntSports.Name = "trackIntSports";
            this.trackIntSports.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.iSports;
            this.trackIntSports.Size = new System.Drawing.Size(138, 21);
            this.trackIntSports.TabIndex = 107;
            this.trackIntSports.Tag = "Sports";
            this.trackIntSports.TokenGuid = ((uint)(0u));
            this.trackIntSports.Value = ((ushort)(0));
            this.trackIntSports.Changed += new System.EventHandler(this.OnInterestChanged);
            // 
            // trackIntSciFi
            // 
            this.trackIntSciFi.Location = new System.Drawing.Point(262, 101);
            this.trackIntSciFi.Margin = new System.Windows.Forms.Padding(0);
            this.trackIntSciFi.Name = "trackIntSciFi";
            this.trackIntSciFi.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.iSciFi;
            this.trackIntSciFi.Size = new System.Drawing.Size(138, 21);
            this.trackIntSciFi.TabIndex = 106;
            this.trackIntSciFi.Tag = "Science Fiction";
            this.trackIntSciFi.TokenGuid = ((uint)(0u));
            this.trackIntSciFi.Value = ((ushort)(0));
            this.trackIntSciFi.Changed += new System.EventHandler(this.OnInterestChanged);
            // 
            // trackIntSchool
            // 
            this.trackIntSchool.Location = new System.Drawing.Point(262, 74);
            this.trackIntSchool.Margin = new System.Windows.Forms.Padding(0);
            this.trackIntSchool.Name = "trackIntSchool";
            this.trackIntSchool.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.iSchool;
            this.trackIntSchool.Size = new System.Drawing.Size(138, 21);
            this.trackIntSchool.TabIndex = 105;
            this.trackIntSchool.Tag = "School";
            this.trackIntSchool.TokenGuid = ((uint)(0u));
            this.trackIntSchool.Value = ((ushort)(0));
            this.trackIntSchool.Changed += new System.EventHandler(this.OnInterestChanged);
            // 
            // trackIntParanormal
            // 
            this.trackIntParanormal.Location = new System.Drawing.Point(262, 20);
            this.trackIntParanormal.Margin = new System.Windows.Forms.Padding(0);
            this.trackIntParanormal.Name = "trackIntParanormal";
            this.trackIntParanormal.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.iParanormal;
            this.trackIntParanormal.Size = new System.Drawing.Size(138, 21);
            this.trackIntParanormal.TabIndex = 104;
            this.trackIntParanormal.Tag = "Paranormal";
            this.trackIntParanormal.TokenGuid = ((uint)(0u));
            this.trackIntParanormal.Value = ((ushort)(0));
            this.trackIntParanormal.Changed += new System.EventHandler(this.OnInterestChanged);
            // 
            // trackIntPolitics
            // 
            this.trackIntPolitics.Location = new System.Drawing.Point(262, 47);
            this.trackIntPolitics.Margin = new System.Windows.Forms.Padding(0);
            this.trackIntPolitics.Name = "trackIntPolitics";
            this.trackIntPolitics.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.iPolitics;
            this.trackIntPolitics.Size = new System.Drawing.Size(138, 21);
            this.trackIntPolitics.TabIndex = 103;
            this.trackIntPolitics.Tag = "Politics";
            this.trackIntPolitics.TokenGuid = ((uint)(0u));
            this.trackIntPolitics.Value = ((ushort)(0));
            this.trackIntPolitics.Changed += new System.EventHandler(this.OnInterestChanged);
            // 
            // trackIntMoney
            // 
            this.trackIntMoney.Location = new System.Drawing.Point(62, 236);
            this.trackIntMoney.Margin = new System.Windows.Forms.Padding(0);
            this.trackIntMoney.Name = "trackIntMoney";
            this.trackIntMoney.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.iMoney;
            this.trackIntMoney.Size = new System.Drawing.Size(138, 21);
            this.trackIntMoney.TabIndex = 102;
            this.trackIntMoney.Tag = "Money";
            this.trackIntMoney.TokenGuid = ((uint)(0u));
            this.trackIntMoney.Value = ((ushort)(0));
            this.trackIntMoney.Changed += new System.EventHandler(this.OnInterestChanged);
            // 
            // trackIntHealth
            // 
            this.trackIntHealth.Location = new System.Drawing.Point(62, 209);
            this.trackIntHealth.Margin = new System.Windows.Forms.Padding(0);
            this.trackIntHealth.Name = "trackIntHealth";
            this.trackIntHealth.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.iHealth;
            this.trackIntHealth.Size = new System.Drawing.Size(138, 21);
            this.trackIntHealth.TabIndex = 101;
            this.trackIntHealth.Tag = "Health";
            this.trackIntHealth.TokenGuid = ((uint)(0u));
            this.trackIntHealth.Value = ((ushort)(0));
            this.trackIntHealth.Changed += new System.EventHandler(this.OnInterestChanged);
            // 
            // trackIntFood
            // 
            this.trackIntFood.Location = new System.Drawing.Point(62, 182);
            this.trackIntFood.Margin = new System.Windows.Forms.Padding(0);
            this.trackIntFood.Name = "trackIntFood";
            this.trackIntFood.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.iFood;
            this.trackIntFood.Size = new System.Drawing.Size(138, 21);
            this.trackIntFood.TabIndex = 100;
            this.trackIntFood.Tag = "Food";
            this.trackIntFood.TokenGuid = ((uint)(0u));
            this.trackIntFood.Value = ((ushort)(0));
            this.trackIntFood.Changed += new System.EventHandler(this.OnInterestChanged);
            // 
            // lblIntAnimals
            // 
            this.lblIntAnimals.AutoSize = true;
            this.lblIntAnimals.Location = new System.Drawing.Point(6, 25);
            this.lblIntAnimals.Name = "lblIntAnimals";
            this.lblIntAnimals.Size = new System.Drawing.Size(54, 15);
            this.lblIntAnimals.TabIndex = 99;
            this.lblIntAnimals.Text = "Animals:";
            // 
            // trackIntFashion
            // 
            this.trackIntFashion.Location = new System.Drawing.Point(62, 153);
            this.trackIntFashion.Margin = new System.Windows.Forms.Padding(0);
            this.trackIntFashion.Name = "trackIntFashion";
            this.trackIntFashion.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.iFashion;
            this.trackIntFashion.Size = new System.Drawing.Size(138, 21);
            this.trackIntFashion.TabIndex = 98;
            this.trackIntFashion.Tag = "Fashion";
            this.trackIntFashion.TokenGuid = ((uint)(0u));
            this.trackIntFashion.Value = ((ushort)(0));
            this.trackIntFashion.Changed += new System.EventHandler(this.OnInterestChanged);
            // 
            // lblIntWork
            // 
            this.lblIntWork.AutoSize = true;
            this.lblIntWork.Location = new System.Drawing.Point(222, 241);
            this.lblIntWork.Name = "lblIntWork";
            this.lblIntWork.Size = new System.Drawing.Size(38, 15);
            this.lblIntWork.TabIndex = 80;
            this.lblIntWork.Text = "Work:";
            // 
            // trackIntEnvironment
            // 
            this.trackIntEnvironment.Location = new System.Drawing.Point(62, 128);
            this.trackIntEnvironment.Margin = new System.Windows.Forms.Padding(0);
            this.trackIntEnvironment.Name = "trackIntEnvironment";
            this.trackIntEnvironment.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.iEnvironment;
            this.trackIntEnvironment.Size = new System.Drawing.Size(138, 21);
            this.trackIntEnvironment.TabIndex = 97;
            this.trackIntEnvironment.Tag = "Environment";
            this.trackIntEnvironment.TokenGuid = ((uint)(0u));
            this.trackIntEnvironment.Value = ((ushort)(0));
            this.trackIntEnvironment.Changed += new System.EventHandler(this.OnInterestChanged);
            // 
            // lblIntWeather
            // 
            this.lblIntWeather.AutoSize = true;
            this.lblIntWeather.Location = new System.Drawing.Point(204, 214);
            this.lblIntWeather.Name = "lblIntWeather";
            this.lblIntWeather.Size = new System.Drawing.Size(56, 15);
            this.lblIntWeather.TabIndex = 79;
            this.lblIntWeather.Text = "Weather:";
            // 
            // trackIntEntertainment
            // 
            this.trackIntEntertainment.Location = new System.Drawing.Point(62, 101);
            this.trackIntEntertainment.Margin = new System.Windows.Forms.Padding(0);
            this.trackIntEntertainment.Name = "trackIntEntertainment";
            this.trackIntEntertainment.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.iEntertainment;
            this.trackIntEntertainment.Size = new System.Drawing.Size(138, 21);
            this.trackIntEntertainment.TabIndex = 96;
            this.trackIntEntertainment.Tag = "Entertainment";
            this.trackIntEntertainment.TokenGuid = ((uint)(0u));
            this.trackIntEntertainment.Value = ((ushort)(0));
            this.trackIntEntertainment.Changed += new System.EventHandler(this.OnInterestChanged);
            // 
            // lblIntTravel
            // 
            this.lblIntTravel.AutoSize = true;
            this.lblIntTravel.Location = new System.Drawing.Point(217, 187);
            this.lblIntTravel.Name = "lblIntTravel";
            this.lblIntTravel.Size = new System.Drawing.Size(43, 15);
            this.lblIntTravel.TabIndex = 78;
            this.lblIntTravel.Text = "Travel:";
            // 
            // trackIntCulture
            // 
            this.trackIntCulture.Location = new System.Drawing.Point(62, 74);
            this.trackIntCulture.Margin = new System.Windows.Forms.Padding(0);
            this.trackIntCulture.Name = "trackIntCulture";
            this.trackIntCulture.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.iCulture;
            this.trackIntCulture.Size = new System.Drawing.Size(138, 21);
            this.trackIntCulture.TabIndex = 95;
            this.trackIntCulture.Tag = "Culture";
            this.trackIntCulture.TokenGuid = ((uint)(0u));
            this.trackIntCulture.Value = ((ushort)(0));
            this.trackIntCulture.Changed += new System.EventHandler(this.OnInterestChanged);
            // 
            // lblIntToys
            // 
            this.lblIntToys.AutoSize = true;
            this.lblIntToys.Location = new System.Drawing.Point(225, 160);
            this.lblIntToys.Name = "lblIntToys";
            this.lblIntToys.Size = new System.Drawing.Size(35, 15);
            this.lblIntToys.TabIndex = 77;
            this.lblIntToys.Text = "Toys:";
            // 
            // trackIntAnimals
            // 
            this.trackIntAnimals.Location = new System.Drawing.Point(62, 20);
            this.trackIntAnimals.Margin = new System.Windows.Forms.Padding(0);
            this.trackIntAnimals.Name = "trackIntAnimals";
            this.trackIntAnimals.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.iAnimals;
            this.trackIntAnimals.Size = new System.Drawing.Size(138, 21);
            this.trackIntAnimals.TabIndex = 94;
            this.trackIntAnimals.Tag = "Animals";
            this.trackIntAnimals.TokenGuid = ((uint)(0u));
            this.trackIntAnimals.Value = ((ushort)(0));
            this.trackIntAnimals.Changed += new System.EventHandler(this.OnInterestChanged);
            // 
            // trackIntCrime
            // 
            this.trackIntCrime.Location = new System.Drawing.Point(62, 47);
            this.trackIntCrime.Margin = new System.Windows.Forms.Padding(0);
            this.trackIntCrime.Name = "trackIntCrime";
            this.trackIntCrime.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.iCrime;
            this.trackIntCrime.Size = new System.Drawing.Size(138, 21);
            this.trackIntCrime.TabIndex = 93;
            this.trackIntCrime.Tag = "Crime";
            this.trackIntCrime.TokenGuid = ((uint)(0u));
            this.trackIntCrime.Value = ((ushort)(0));
            this.trackIntCrime.Changed += new System.EventHandler(this.OnInterestChanged);
            // 
            // lblIntSports
            // 
            this.lblIntSports.AutoSize = true;
            this.lblIntSports.Location = new System.Drawing.Point(215, 133);
            this.lblIntSports.Name = "lblIntSports";
            this.lblIntSports.Size = new System.Drawing.Size(45, 15);
            this.lblIntSports.TabIndex = 76;
            this.lblIntSports.Text = "Sports:";
            // 
            // lblIntSciFi
            // 
            this.lblIntSciFi.AutoSize = true;
            this.lblIntSciFi.Location = new System.Drawing.Point(219, 106);
            this.lblIntSciFi.Name = "lblIntSciFi";
            this.lblIntSciFi.Size = new System.Drawing.Size(41, 15);
            this.lblIntSciFi.TabIndex = 75;
            this.lblIntSciFi.Tag = "";
            this.lblIntSciFi.Text = "Sci-Fi:";
            // 
            // lblIntSchool
            // 
            this.lblIntSchool.AutoSize = true;
            this.lblIntSchool.Location = new System.Drawing.Point(212, 79);
            this.lblIntSchool.Name = "lblIntSchool";
            this.lblIntSchool.Size = new System.Drawing.Size(48, 15);
            this.lblIntSchool.TabIndex = 74;
            this.lblIntSchool.Text = "School:";
            // 
            // lblIntPolitics
            // 
            this.lblIntPolitics.AutoSize = true;
            this.lblIntPolitics.Location = new System.Drawing.Point(211, 52);
            this.lblIntPolitics.Name = "lblIntPolitics";
            this.lblIntPolitics.Size = new System.Drawing.Size(49, 15);
            this.lblIntPolitics.TabIndex = 73;
            this.lblIntPolitics.Text = "Politics:";
            // 
            // lblIntParanormal
            // 
            this.lblIntParanormal.AutoSize = true;
            this.lblIntParanormal.Location = new System.Drawing.Point(224, 25);
            this.lblIntParanormal.Name = "lblIntParanormal";
            this.lblIntParanormal.Size = new System.Drawing.Size(36, 15);
            this.lblIntParanormal.TabIndex = 54;
            this.lblIntParanormal.Text = "Para:";
            // 
            // lblIntMoney
            // 
            this.lblIntMoney.AutoSize = true;
            this.lblIntMoney.Location = new System.Drawing.Point(13, 241);
            this.lblIntMoney.Name = "lblIntMoney";
            this.lblIntMoney.Size = new System.Drawing.Size(47, 15);
            this.lblIntMoney.TabIndex = 53;
            this.lblIntMoney.Text = "Money:";
            // 
            // lblIntHealth
            // 
            this.lblIntHealth.AutoSize = true;
            this.lblIntHealth.Location = new System.Drawing.Point(14, 214);
            this.lblIntHealth.Name = "lblIntHealth";
            this.lblIntHealth.Size = new System.Drawing.Size(46, 15);
            this.lblIntHealth.TabIndex = 52;
            this.lblIntHealth.Text = "Health:";
            // 
            // lblIntFood
            // 
            this.lblIntFood.AutoSize = true;
            this.lblIntFood.Location = new System.Drawing.Point(22, 187);
            this.lblIntFood.Name = "lblIntFood";
            this.lblIntFood.Size = new System.Drawing.Size(38, 15);
            this.lblIntFood.TabIndex = 51;
            this.lblIntFood.Tag = "";
            this.lblIntFood.Text = "Food:";
            // 
            // lblIntFashion
            // 
            this.lblIntFashion.AutoSize = true;
            this.lblIntFashion.Location = new System.Drawing.Point(6, 160);
            this.lblIntFashion.Name = "lblIntFashion";
            this.lblIntFashion.Size = new System.Drawing.Size(54, 15);
            this.lblIntFashion.TabIndex = 50;
            this.lblIntFashion.Text = "Fashion:";
            // 
            // lblIntEnvironment
            // 
            this.lblIntEnvironment.AutoSize = true;
            this.lblIntEnvironment.Location = new System.Drawing.Point(9, 133);
            this.lblIntEnvironment.Name = "lblIntEnvironment";
            this.lblIntEnvironment.Size = new System.Drawing.Size(51, 15);
            this.lblIntEnvironment.TabIndex = 49;
            this.lblIntEnvironment.Text = "Environ:";
            // 
            // lblIntEntertainment
            // 
            this.lblIntEntertainment.AutoSize = true;
            this.lblIntEntertainment.Location = new System.Drawing.Point(32, 106);
            this.lblIntEntertainment.Name = "lblIntEntertainment";
            this.lblIntEntertainment.Size = new System.Drawing.Size(28, 15);
            this.lblIntEntertainment.TabIndex = 48;
            this.lblIntEntertainment.Text = "Ent:";
            // 
            // lblIntCulture
            // 
            this.lblIntCulture.AutoSize = true;
            this.lblIntCulture.Location = new System.Drawing.Point(11, 79);
            this.lblIntCulture.Name = "lblIntCulture";
            this.lblIntCulture.Size = new System.Drawing.Size(49, 15);
            this.lblIntCulture.TabIndex = 47;
            this.lblIntCulture.Text = "Culture:";
            // 
            // lblIntCrime
            // 
            this.lblIntCrime.AutoSize = true;
            this.lblIntCrime.Location = new System.Drawing.Point(17, 52);
            this.lblIntCrime.Name = "lblIntCrime";
            this.lblIntCrime.Size = new System.Drawing.Size(43, 15);
            this.lblIntCrime.TabIndex = 46;
            this.lblIntCrime.Text = "Crime:";
            // 
            // thumbBox
            // 
            this.thumbBox.BackColor = System.Drawing.Color.WhiteSmoke;
            this.thumbBox.Location = new System.Drawing.Point(10, 57);
            this.thumbBox.Name = "thumbBox";
            this.thumbBox.Size = new System.Drawing.Size(128, 128);
            this.thumbBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.thumbBox.TabIndex = 25;
            this.thumbBox.TabStop = false;
            this.thumbBox.Visible = false;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(1192, 632);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(88, 26);
            this.btnSave.TabIndex = 23;
            this.btnSave.Text = "&Save All";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.OnSaveClicked);
            // 
            // saveAsFileDialog
            // 
            this.saveAsFileDialog.Filter = "DBPF Package|*.package";
            this.saveAsFileDialog.Title = "Save as replacements";
            // 
            // openSuitcaseFileDialog
            // 
            this.openSuitcaseFileDialog.DefaultExt = "fms";
            this.openSuitcaseFileDialog.DereferenceLinks = false;
            this.openSuitcaseFileDialog.Filter = "Family Manager Suitcase files|*.fms|All files|*.*";
            this.openSuitcaseFileDialog.Title = "Load Suitcase Items";
            // 
            // saveSuitcaseFileDialog
            // 
            this.saveSuitcaseFileDialog.DefaultExt = "fms";
            this.saveSuitcaseFileDialog.Filter = "Family Manager Suitcase files|*.fms|All files|*.*";
            this.saveSuitcaseFileDialog.Title = "Save Suitcase Items";
            // 
            // saveJewelboxFileDialog
            // 
            this.saveJewelboxFileDialog.DefaultExt = "fmj";
            this.saveJewelboxFileDialog.Filter = "Family Manager Jewel Box files|*.fmj|All files|*.*";
            this.saveJewelboxFileDialog.Title = "Save Jewellery Items";
            // 
            // openJewelboxFileDialog
            // 
            this.openJewelboxFileDialog.DefaultExt = "j";
            this.openJewelboxFileDialog.DereferenceLinks = false;
            this.openJewelboxFileDialog.Filter = "Family Manager Jewel Box files|*.fmj|All files|*.*";
            this.openJewelboxFileDialog.Title = "Load Jewellery Items";
            // 
            // trackSkillHiddenBreakDance
            // 
            this.trackSkillHiddenBreakDance.Location = new System.Drawing.Point(87, 20);
            this.trackSkillHiddenBreakDance.Margin = new System.Windows.Forms.Padding(0);
            this.trackSkillHiddenBreakDance.Maximum = 300;
            this.trackSkillHiddenBreakDance.Name = "trackSkillHiddenBreakDance";
            this.trackSkillHiddenBreakDance.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.NONE;
            this.trackSkillHiddenBreakDance.Size = new System.Drawing.Size(138, 21);
            this.trackSkillHiddenBreakDance.TabIndex = 106;
            this.trackSkillHiddenBreakDance.Tag = "Break Dance";
            this.trackSkillHiddenBreakDance.TokenGuid = ((uint)(1424141221u));
            this.trackSkillHiddenBreakDance.TokenProp = ((uint)(1u));
            this.trackSkillHiddenBreakDance.Value = ((ushort)(0));
            this.trackSkillHiddenBreakDance.Changed += new System.EventHandler(this.OnHiddenSkillChanged);
            // 
            // lblSkillHiddenBreakDance
            // 
            this.lblSkillHiddenBreakDance.AutoSize = true;
            this.lblSkillHiddenBreakDance.Location = new System.Drawing.Point(4, 25);
            this.lblSkillHiddenBreakDance.Name = "lblSkillHiddenBreakDance";
            this.lblSkillHiddenBreakDance.Size = new System.Drawing.Size(81, 15);
            this.lblSkillHiddenBreakDance.TabIndex = 105;
            this.lblSkillHiddenBreakDance.Text = "Break Dance:";
            // 
            // trackSkillHiddenFireDance
            // 
            this.trackSkillHiddenFireDance.Location = new System.Drawing.Point(87, 80);
            this.trackSkillHiddenFireDance.Margin = new System.Windows.Forms.Padding(0);
            this.trackSkillHiddenFireDance.Maximum = 300;
            this.trackSkillHiddenFireDance.Name = "trackSkillHiddenFireDance";
            this.trackSkillHiddenFireDance.SdscIndex = Sims2Tools.DBPF.Neighbourhood.SDSC.SdscIndex.NONE;
            this.trackSkillHiddenFireDance.Size = new System.Drawing.Size(138, 21);
            this.trackSkillHiddenFireDance.TabIndex = 108;
            this.trackSkillHiddenFireDance.Tag = "Fire Dance";
            this.trackSkillHiddenFireDance.TokenGuid = ((uint)(860225760u));
            this.trackSkillHiddenFireDance.TokenProp = ((uint)(1u));
            this.trackSkillHiddenFireDance.Value = ((ushort)(0));
            this.trackSkillHiddenFireDance.Changed += new System.EventHandler(this.OnHiddenSkillChanged);
            // 
            // lblSkillHiddenFireDance
            // 
            this.lblSkillHiddenFireDance.AutoSize = true;
            this.lblSkillHiddenFireDance.Location = new System.Drawing.Point(15, 85);
            this.lblSkillHiddenFireDance.Name = "lblSkillHiddenFireDance";
            this.lblSkillHiddenFireDance.Size = new System.Drawing.Size(70, 15);
            this.lblSkillHiddenFireDance.TabIndex = 107;
            this.lblSkillHiddenFireDance.Text = "Fire Dance:";
            // 
            // FamilyManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1284, 661);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.menuMain);
            this.Controls.Add(this.thumbBox);
            this.Controls.Add(this.splitTopBottom);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuMain;
            this.MinimumSize = new System.Drawing.Size(1300, 700);
            this.Name = "FamilyManagerForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.OnLoad);
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.splitTopBottom.Panel1.ResumeLayout(false);
            this.splitTopBottom.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitTopBottom)).EndInit();
            this.splitTopBottom.ResumeLayout(false);
            this.splitTopLeftRight.Panel1.ResumeLayout(false);
            this.splitTopLeftRight.Panel2.ResumeLayout(false);
            this.splitTopLeftRight.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitTopLeftRight)).EndInit();
            this.splitTopLeftRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imageFamily)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridFamilyMembers)).EndInit();
            this.menuContextMembers.ResumeLayout(false);
            this.tabPages.ResumeLayout(false);
            this.tabFamily.ResumeLayout(false);
            this.panelFamily.ResumeLayout(false);
            this.panelFamily.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageHouse)).EndInit();
            this.tabCloset.ResumeLayout(false);
            this.splitClosetLeftRight.Panel1.ResumeLayout(false);
            this.splitClosetLeftRight.Panel2.ResumeLayout(false);
            this.splitClosetLeftRight.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitClosetLeftRight)).EndInit();
            this.splitClosetLeftRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridSuitcase)).EndInit();
            this.menuContextSuitcase.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridFamilyCloset)).EndInit();
            this.menuContextCloset.ResumeLayout(false);
            this.tabSafe.ResumeLayout(false);
            this.splitSafeLeftRight.Panel1.ResumeLayout(false);
            this.splitSafeLeftRight.Panel2.ResumeLayout(false);
            this.splitSafeLeftRight.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitSafeLeftRight)).EndInit();
            this.splitSafeLeftRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridJewelbox)).EndInit();
            this.menuContextJewelbox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridFamilySafe)).EndInit();
            this.menuContextSafe.ResumeLayout(false);
            this.tabCareer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imageSim)).EndInit();
            this.grpJob.ResumeLayout(false);
            this.grpJob.PerformLayout();
            this.grpUniversity.ResumeLayout(false);
            this.grpUniversity.PerformLayout();
            this.grpSchool.ResumeLayout(false);
            this.grpSchool.PerformLayout();
            this.tabSkills.ResumeLayout(false);
            this.grpSkillsPet.ResumeLayout(false);
            this.grpSkillsPet.PerformLayout();
            this.grpSkillsLife.ResumeLayout(false);
            this.grpSkillsLife.PerformLayout();
            this.grpSkillsToddler.ResumeLayout(false);
            this.grpSkillsToddler.PerformLayout();
            this.grpSkillsHidden.ResumeLayout(false);
            this.grpSkillsHidden.PerformLayout();
            this.grpSkillsGeneral.ResumeLayout(false);
            this.grpSkillsGeneral.PerformLayout();
            this.tabInterests.ResumeLayout(false);
            this.grpBadges.ResumeLayout(false);
            this.grpBadges.PerformLayout();
            this.grpHobbies.ResumeLayout(false);
            this.grpHobbies.PerformLayout();
            this.grpInterests.ResumeLayout(false);
            this.grpInterests.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.thumbBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuItemConfiguration;
        private System.Windows.Forms.ToolStripSeparator menuItemSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuItemExit;
        private System.Windows.Forms.ToolStripMenuItem menuHelp;
        private System.Windows.Forms.ToolStripMenuItem menuItemAbout;
        private System.Windows.Forms.ToolStripMenuItem menuOptions;
        private System.Windows.Forms.ToolStripMenuItem menuMode;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem menuItemAutoBackup;
        private CommonOpenFileDialog selectPathDialog;
        private System.Windows.Forms.SplitContainer splitTopBottom;
        private System.Windows.Forms.SplitContainer splitTopLeftRight;
        private System.Windows.Forms.SplitContainer splitClosetLeftRight;
        private System.Windows.Forms.TreeView treeHoods;
        private System.Windows.Forms.DataGridView gridFamilyCloset;
        private System.Windows.Forms.ContextMenuStrip menuContextCloset;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripMenuItem menuContextClosetMoveToSuitcase;
        private System.Windows.Forms.ToolStripMenuItem menuContextClosetFilterAll;
        private System.Windows.Forms.ToolStripMenuItem menuContextClosetFilterSelected;
        private System.Windows.Forms.ToolStripMenuItem menuContextClosetFilterUnwearable;
        private System.Windows.Forms.ToolStripMenuItem menuContextClosetCopyToSuitcase;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.SaveFileDialog saveAsFileDialog;
        private System.Windows.Forms.PictureBox thumbBox;
        private System.Windows.Forms.ToolStripMenuItem menuItemSaveAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuItemAdvanced;
        private System.Windows.Forms.DataGridView gridFamilyMembers;
        private System.Windows.Forms.ContextMenuStrip menuContextMembers;
        private System.Windows.Forms.ToolStripMenuItem menuContextMemberFilterSelected;
        private System.Windows.Forms.Label lblLotName;
        private System.Windows.Forms.Label lblFamilyName;
        private System.Windows.Forms.PictureBox imageFamily;
        private System.Windows.Forms.Button btnClosetCopy;
        private System.Windows.Forms.Button btnClosetMove;
        private System.Windows.Forms.Button btnClosetDelete;
        private System.Windows.Forms.DataGridView gridSuitcase;
        private System.Windows.Forms.ContextMenuStrip menuContextSuitcase;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem menuContextSuitcaseMoveToCloset;
        private System.Windows.Forms.ToolStripMenuItem menuContextSuitcaseDelete;
        private System.Windows.Forms.ToolStripMenuItem menuContextSuitcaseCopyToCloset;
        private System.Windows.Forms.Button btnSuitcaseEmpty;
        private System.Windows.Forms.Button btnSuitcaseCopy;
        private System.Windows.Forms.TabControl tabPages;
        private System.Windows.Forms.TabPage tabCloset;
        private System.Windows.Forms.ToolStripMenuItem menuItemUseCodes;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSuitcaseVisible;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSuitcaseName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSuitcaseCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSuitcaseGender;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSuitcaseGenderCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSuitcaseAge;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSuitcaseAgeCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSuitcaseGenderHex;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSuitcaseAgeHex;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSuitcaseData;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSuitcaseThumbKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSuitcaseLocalThumbKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClosetVisible;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClosetName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClosetCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClosetGender;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClosetGenderCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClosetAge;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClosetAgeCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClosetGenderHex;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClosetAgeHex;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClosetData;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClosetThumbKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClosetLocalThumbKey;
        private System.Windows.Forms.ToolStripMenuItem menuCaching;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorCaching;
        private System.Windows.Forms.ToolStripMenuItem menuItemCachingRemoveLocal;
        private System.Windows.Forms.ToolStripMenuItem menuItemCachingUpdateMaxisClothes;
        private System.Windows.Forms.ToolStripMenuItem menuItemCachingUpdateCustomClothes;
        private System.Windows.Forms.ToolStripMenuItem menuItemCachingUpdateMaxisJewellery;
        private System.Windows.Forms.ToolStripMenuItem menuItemCachingUpdateCustomJewellery;
        private System.Windows.Forms.Button btnClosetShowAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem menuContextClosetDelete;
        private System.Windows.Forms.Button btnSuitcaseMove;
        private System.Windows.Forms.ToolStripMenuItem menuContextMemberFilterAll;
        private System.Windows.Forms.ToolStripMenuItem menuContextMemberFilterThis;
        private System.Windows.Forms.TabPage tabFamily;
        private System.Windows.Forms.Label lblMoney;
        private System.Windows.Forms.TextBox textFamilyMoney;
        private System.Windows.Forms.TextBox textAddressName;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.PictureBox imageHouse;
        private System.Windows.Forms.TextBox textFamilyWriteUp;
        private System.Windows.Forms.Label lblWriteUp;
        private System.Windows.Forms.TextBox textFamilyName;
        private System.Windows.Forms.Label lblFamName;
        private System.Windows.Forms.Panel panelFamily;
        private System.Windows.Forms.ToolStripMenuItem menuLanguage;
        private System.Windows.Forms.TextBox textBusinessMoney;
        private System.Windows.Forms.Label lblBusinessMoney;
        private System.Windows.Forms.CheckBox ckbMoneyLock;
        private System.Windows.Forms.TextBox textAddressDesc;
        private System.Windows.Forms.Label lblClosetCachesNeeded;
        private System.Windows.Forms.ToolStripMenuItem menuItemCachingRemoveThumbnails;
        private System.Windows.Forms.ToolStripMenuItem menuContextMemberChangeFamilyName;
        private System.Windows.Forms.ToolStripMenuItem menuContextMemberChangeDays;
        private System.Windows.Forms.CheckBox ckbFamilyNameSelected;
        private System.Windows.Forms.CheckBox ckbFamilyNameSame;
        private System.Windows.Forms.CheckBox ckbFamilyNameAll;
        private System.Windows.Forms.Button btnSuitcaseLoad;
        private System.Windows.Forms.Button btnSuitcaseSave;
        private System.Windows.Forms.OpenFileDialog openSuitcaseFileDialog;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorSplitFiles;
        private System.Windows.Forms.ToolStripMenuItem menuItemShowSplitFiles;
        private System.Windows.Forms.ToolStripMenuItem menuItemHighlightSplitFiles;
        private System.Windows.Forms.TabPage tabSafe;
        private System.Windows.Forms.SplitContainer splitSafeLeftRight;
        private System.Windows.Forms.Button btnJewelboxLoad;
        private System.Windows.Forms.Button btnJewelboxSave;
        private System.Windows.Forms.Button btnJewelboxMove;
        private System.Windows.Forms.Button btnJewelboxCopy;
        private System.Windows.Forms.DataGridView gridJewelbox;
        private System.Windows.Forms.Button btnJewelboxEmpty;
        private System.Windows.Forms.Label lblSafeCachesNeeded;
        private System.Windows.Forms.Button btnSafeShowAll;
        private System.Windows.Forms.DataGridView gridFamilySafe;
        private System.Windows.Forms.Button btnSafeCopy;
        private System.Windows.Forms.Button btnSafeMove;
        private System.Windows.Forms.Button btnSafeDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJewelboxVisible;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJewelboxName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJewelboxCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJewelboxGender;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJewelboxGenderCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJewelboxAge;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJewelboxAgeCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJewelboxData;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJewelboxGenderHex;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJewelboxAgeHex;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJewelboxThumbKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJewelboxLocalThumbKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSafeVisible;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSafeName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSafeCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSafeGender;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSafeGenderCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSafeAge;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSafeAgeCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSafeData;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSafeGenderHex;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSafeAgeHex;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSafeThumbKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSafeLocalThumbKey;
        private System.Windows.Forms.SaveFileDialog saveSuitcaseFileDialog;
        private System.Windows.Forms.SaveFileDialog saveJewelboxFileDialog;
        private System.Windows.Forms.OpenFileDialog openJewelboxFileDialog;
        private System.Windows.Forms.ContextMenuStrip menuContextSafe;
        private System.Windows.Forms.ToolStripMenuItem menuContextSafeCopyToJewelbox;
        private System.Windows.Forms.ToolStripMenuItem menuContextSafeMoveToJewelbox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem menuContextSafeFilterAll;
        private System.Windows.Forms.ToolStripMenuItem menuContextSafeFilterSelected;
        private System.Windows.Forms.ToolStripMenuItem menuContextSafeFilterUnwearable;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripMenuItem menuContextSafeDelete;
        private System.Windows.Forms.ContextMenuStrip menuContextJewelbox;
        private System.Windows.Forms.ToolStripMenuItem menuContextJewelboxCopyToSafe;
        private System.Windows.Forms.ToolStripMenuItem menuContextJewelboxMoveToSafe;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
        private System.Windows.Forms.ToolStripMenuItem menuContextJewelboxDelete;
        private System.Windows.Forms.ToolStripMenuItem menuContextMemberChangeSimName;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ToolStripSeparator menuContextMemberSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuContextMemberMergeSplitFiles;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuItemIncludeNPCs;
        private System.Windows.Forms.ToolStripMenuItem menuItemOnlyNPCs;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFirstName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSplitFile;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGender;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGenderCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAge;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAgeCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDaysLeft;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGenderHex;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAgeHex;
        private System.Windows.Forms.DataGridViewTextBoxColumn colThumbnail;
        private System.Windows.Forms.DataGridViewTextBoxColumn colData;
        private System.Windows.Forms.TabPage tabCareer;
        private System.Windows.Forms.TabPage tabSkills;
        private System.Windows.Forms.GroupBox grpJob;
        private System.Windows.Forms.GroupBox grpUniversity;
        private System.Windows.Forms.GroupBox grpSchool;
        private System.Windows.Forms.ComboBox comboSchoolGrade;
        private System.Windows.Forms.Label lblSchoolGrade;
        private System.Windows.Forms.Label lblSchoolType;
        private System.Windows.Forms.ComboBox comboSchoolType;
        private System.Windows.Forms.Label lblJobType;
        private System.Windows.Forms.ComboBox comboJobType;
        private System.Windows.Forms.Label lblJobPTO;
        private System.Windows.Forms.Label lblJobPerformance;
        private System.Windows.Forms.Label lblJobLevel;
        private System.Windows.Forms.Label lblUniSemester;
        private System.Windows.Forms.ComboBox comboUniMajor;
        private System.Windows.Forms.Label lblUniEffort;
        private System.Windows.Forms.Label lblUniMajor;
        private System.Windows.Forms.Label lblUniGrade;
        private System.Windows.Forms.Label lblUniTimeLeft;
        private System.Windows.Forms.ComboBox comboUniSemester;
        private System.Windows.Forms.Label lblUniInfluence;
        private Sims2Tools.Controls.SimTrackingBar trackJobLevel;
        private Sims2Tools.Controls.SimTrackingBar trackJobPerformance;
        private Sims2Tools.Controls.SimTrackingBar trackUniEffort;
        private Sims2Tools.Controls.SimTrackingBar trackUniGrade;
        private Sims2Tools.Controls.SimTrackingBar trackUniTimeLeft;
        private System.Windows.Forms.Label lblJobPension;
        private System.Windows.Forms.GroupBox grpSkillsGeneral;
        private System.Windows.Forms.Label lblSkillRomance;
        private System.Windows.Forms.Label lblSkillMechanical;
        private System.Windows.Forms.Label lblSkillLogic;
        private System.Windows.Forms.Label lblSkillCreativity;
        private System.Windows.Forms.Label lblSkillCooking;
        private System.Windows.Forms.Label lblSkillCleaning;
        private System.Windows.Forms.Label lblSkillCharisma;
        private System.Windows.Forms.Label lblSkillBody;
        private System.Windows.Forms.TabPage tabInterests;
        private System.Windows.Forms.GroupBox grpHobbies;
        private System.Windows.Forms.ComboBox comboHobbyOneTrue;
        private System.Windows.Forms.Label lblHobbyOneTrue;
        private System.Windows.Forms.Label lblHobbyScience;
        private System.Windows.Forms.Label lblHobbyMusic;
        private System.Windows.Forms.Label lblHobbyFilm;
        private System.Windows.Forms.Label lblHobbySport;
        private System.Windows.Forms.Label lblHobbyArts;
        private System.Windows.Forms.Label lblHobbySecret;
        private System.Windows.Forms.Label lblHobbyGames;
        private System.Windows.Forms.Label lblHobbyFitness;
        private System.Windows.Forms.Label lblHobbyNature;
        private System.Windows.Forms.Label lblHobbyTinker;
        private System.Windows.Forms.Label lblHobbyCuisine;
        private System.Windows.Forms.GroupBox grpInterests;
        private System.Windows.Forms.Label lblIntWork;
        private System.Windows.Forms.Label lblIntWeather;
        private System.Windows.Forms.Label lblIntTravel;
        private System.Windows.Forms.Label lblIntToys;
        private System.Windows.Forms.Label lblIntSports;
        private System.Windows.Forms.Label lblIntSciFi;
        private System.Windows.Forms.Label lblIntSchool;
        private System.Windows.Forms.Label lblIntPolitics;
        private System.Windows.Forms.Label lblIntParanormal;
        private System.Windows.Forms.Label lblIntMoney;
        private System.Windows.Forms.Label lblIntHealth;
        private System.Windows.Forms.Label lblIntFood;
        private System.Windows.Forms.Label lblIntFashion;
        private System.Windows.Forms.Label lblIntEnvironment;
        private System.Windows.Forms.Label lblIntEntertainment;
        private System.Windows.Forms.Label lblIntCulture;
        private System.Windows.Forms.Label lblIntCrime;
        private System.Windows.Forms.GroupBox grpSkillsLife;
        private System.Windows.Forms.Label lblSkillLifeParenting;
        private System.Windows.Forms.Label lblSkillLifeHappiness;
        private System.Windows.Forms.Label lblSkillLifeFireSafety;
        private System.Windows.Forms.Label lblSkillLifeCounselling;
        private System.Windows.Forms.Label lblSkillLifeAngerMgmt;
        private System.Windows.Forms.GroupBox grpSkillsToddler;
        private System.Windows.Forms.Label lblSkillToddlerWalk;
        private System.Windows.Forms.Label lblSkillToddlerTalk;
        private System.Windows.Forms.Label lblSkillToddlerRhyming;
        private System.Windows.Forms.Label lblSkillToddlerPotty;
        private System.Windows.Forms.GroupBox grpSkillsHidden;
        private System.Windows.Forms.Label lblSkillHiddenStudy;
        private System.Windows.Forms.Label lblSkillHiddenPool;
        private System.Windows.Forms.Label lblSkillHiddenMeditate;
        private System.Windows.Forms.Label lblSkillHiddenDance;
        private System.Windows.Forms.Label lblSkillLifePhysiology;
        private System.Windows.Forms.GroupBox grpBadges;
        private System.Windows.Forms.Label lblBadgeCashier;
        private System.Windows.Forms.Label lblBadgeCosmetics;
        private System.Windows.Forms.Label lblBadgeGardening;
        private System.Windows.Forms.Label lblBadgeFishing;
        private System.Windows.Forms.Label lblBadgeFlorist;
        private System.Windows.Forms.Label lblBadgePottery;
        private System.Windows.Forms.Label lblBadgeRobotery;
        private System.Windows.Forms.Label lblBadgeSales;
        private System.Windows.Forms.Label lblBadgeSewing;
        private System.Windows.Forms.Label lblBadgeStocking;
        private System.Windows.Forms.Label lblBadgeToyMaking;
        private System.Windows.Forms.PictureBox imageSim;
        private System.Windows.Forms.ToolStripMenuItem menuItemCachingUpdateCustomCareers;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ComboBox comboUniResult;
        private System.Windows.Forms.Label lblUniResult;
        private System.Windows.Forms.Label lblUniStudying;
        private System.Windows.Forms.CheckBox ckbUniStudying;
        private System.Windows.Forms.Label lblUniProbation;
        private System.Windows.Forms.CheckBox ckbUniProbation;
        private Sims2Tools.Controls.UIntTextBox textJobPension;
        private Sims2Tools.Controls.UIntTextBox textJobPTO;
        private Sims2Tools.Controls.UIntTextBox textJobLevel;
        private Sims2Tools.Controls.IntTextBox textJobPerformance;
        private Sims2Tools.Controls.GuidTextBox textJobGUID;
        private Sims2Tools.Controls.GuidTextBox textSchoolGUID;
        private Sims2Tools.Controls.GuidTextBox textMajorGUID;
        private Sims2Tools.Controls.UIntTextBox textUniInfluence;
        private Sims2Tools.Controls.UIntTextBox textUniTimeLeft;
        private Sims2Tools.Controls.UIntTextBox textUniEffort;
        private Sims2Tools.Controls.DoubleTextBox textUniGrade;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
        private System.Windows.Forms.ToolStripMenuItem menuItemTeensHaveAdultJobs;
        private System.Windows.Forms.ToolStripMenuItem menuItemYAsHaveAdultJobs;
        private Sims2Tools.Controls.InterestTracker trackHobbyTinker;
        private Sims2Tools.Controls.InterestTracker trackHobbyNature;
        private Sims2Tools.Controls.InterestTracker trackHobbyCuisine;
        private Sims2Tools.Controls.InterestTracker trackHobbyArts;
        private Sims2Tools.Controls.InterestTracker trackHobbyMusic;
        private Sims2Tools.Controls.InterestTracker trackHobbyGames;
        private Sims2Tools.Controls.InterestTracker trackHobbyFitness;
        private Sims2Tools.Controls.InterestTracker trackHobbyFilm;
        private Sims2Tools.Controls.InterestTracker trackHobbySport;
        private Sims2Tools.Controls.InterestTracker trackHobbySecret;
        private Sims2Tools.Controls.InterestTracker trackHobbyScience;
        private Sims2Tools.Controls.InterestTracker trackIntFashion;
        private Sims2Tools.Controls.InterestTracker trackIntEnvironment;
        private Sims2Tools.Controls.InterestTracker trackIntEntertainment;
        private Sims2Tools.Controls.InterestTracker trackIntCulture;
        private Sims2Tools.Controls.InterestTracker trackIntAnimals;
        private Sims2Tools.Controls.InterestTracker trackIntCrime;
        private System.Windows.Forms.Label lblIntAnimals;
        private Sims2Tools.Controls.InterestTracker trackIntMoney;
        private Sims2Tools.Controls.InterestTracker trackIntHealth;
        private Sims2Tools.Controls.InterestTracker trackIntFood;
        private Sims2Tools.Controls.InterestTracker trackBadgeStocking;
        private Sims2Tools.Controls.InterestTracker trackBadgePottery;
        private Sims2Tools.Controls.InterestTracker trackBadgeSewing;
        private Sims2Tools.Controls.InterestTracker trackBadgeSales;
        private Sims2Tools.Controls.InterestTracker trackBadgeFlorist;
        private Sims2Tools.Controls.InterestTracker trackBadgeRobotery;
        private Sims2Tools.Controls.InterestTracker trackBadgeToyMaking;
        private Sims2Tools.Controls.InterestTracker trackBadgeFishing;
        private Sims2Tools.Controls.InterestTracker trackBadgeGardening;
        private Sims2Tools.Controls.InterestTracker trackBadgeCashier;
        private Sims2Tools.Controls.InterestTracker trackBadgeCosmetics;
        private Sims2Tools.Controls.InterestTracker trackIntWork;
        private Sims2Tools.Controls.InterestTracker trackIntWeather;
        private Sims2Tools.Controls.InterestTracker trackIntTravel;
        private Sims2Tools.Controls.InterestTracker trackIntToys;
        private Sims2Tools.Controls.InterestTracker trackIntSports;
        private Sims2Tools.Controls.InterestTracker trackIntSciFi;
        private Sims2Tools.Controls.InterestTracker trackIntSchool;
        private Sims2Tools.Controls.InterestTracker trackIntParanormal;
        private Sims2Tools.Controls.InterestTracker trackIntPolitics;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem menuItemIntDisplay;
        private System.Windows.Forms.ToolStripMenuItem menuItemIntDisplayBarAndBox;
        private System.Windows.Forms.ToolStripMenuItem menuItemIntDisplayBarOnly;
        private System.Windows.Forms.ToolStripMenuItem menuItemIntDisplayBoxOnly;
        private System.Windows.Forms.Label lblSkillHiddenTaiChi;
        private Sims2Tools.Controls.SkillTracker trackSkillBody;
        private Sims2Tools.Controls.SkillTracker trackSkillRomance;
        private Sims2Tools.Controls.SkillTracker trackSkillMechanical;
        private Sims2Tools.Controls.SkillTracker trackSkillLogic;
        private Sims2Tools.Controls.SkillTracker trackSkillCreativity;
        private Sims2Tools.Controls.SkillTracker trackSkillCooking;
        private Sims2Tools.Controls.SkillTracker trackSkillCleaning;
        private Sims2Tools.Controls.SkillTracker trackSkillCharisma;
        private Sims2Tools.Controls.SkillTracker trackSkillToddlerWalk;
        private Sims2Tools.Controls.SkillTracker trackSkillToddlerTalk;
        private Sims2Tools.Controls.SkillTracker trackSkillToddlerRhyming;
        private Sims2Tools.Controls.SkillTracker trackSkillToddlerPotty;
        private Sims2Tools.Controls.SkillTracker trackSkillLifePhysiology;
        private Sims2Tools.Controls.SkillTracker trackSkillLifeParenting;
        private Sims2Tools.Controls.SkillTracker trackSkillLifeHappiness;
        private Sims2Tools.Controls.SkillTracker trackSkillLifeFireSafety;
        private Sims2Tools.Controls.SkillTracker trackSkillLifeCounselling;
        private Sims2Tools.Controls.SkillTracker trackSkillLifeAngerMgmt;
        private Sims2Tools.Controls.SkillTracker trackSkillHiddenTaiChi;
        private Sims2Tools.Controls.SkillTracker trackSkillHiddenStudy;
        private Sims2Tools.Controls.SkillTracker trackSkillHiddenPool;
        private Sims2Tools.Controls.SkillTracker trackSkillHiddenMeditate;
        private Sims2Tools.Controls.SkillTracker trackSkillHiddenDance;
        private Sims2Tools.Controls.GuidTextBox textJobRetiredGUID;
        private Sims2Tools.Controls.UIntTextBox textJobRetiredLevel;
        private Sims2Tools.Controls.SimTrackingBar trackJobRetiredLevel;
        private System.Windows.Forms.Label lblJobRetiredLevel;
        private System.Windows.Forms.Label lblJobRetiredType;
        private System.Windows.Forms.ComboBox comboJobRetiredType;
        private System.Windows.Forms.Label lblJobPTOSummary;
        private System.Windows.Forms.GroupBox grpSkillsPet;
        private Sims2Tools.Controls.SkillTracker trackSkillPetUseToilet;
        private Sims2Tools.Controls.SkillTracker trackSkillPetStay;
        private Sims2Tools.Controls.SkillTracker trackSkillPetSpeak;
        private Sims2Tools.Controls.SkillTracker trackSkillPetSitUp;
        private Sims2Tools.Controls.SkillTracker trackSkillPetShake;
        private Sims2Tools.Controls.SkillTracker trackSkillPetRollOver;
        private Sims2Tools.Controls.SkillTracker trackSkillPetPlayDead;
        private Sims2Tools.Controls.SkillTracker trackSkillPetComeHere;
        private System.Windows.Forms.Label lblSkillPetUseToilet;
        private System.Windows.Forms.Label lblSkillPetStay;
        private System.Windows.Forms.Label lblSkillPetSpeak;
        private System.Windows.Forms.Label lblSkillPetSitUp;
        private System.Windows.Forms.Label lblSkillPetShake;
        private System.Windows.Forms.Label lblSkillPetRollOver;
        private System.Windows.Forms.Label lblSkillPetPlayDead;
        private System.Windows.Forms.Label lblSkillPetComeHere;
        private Sims2Tools.Controls.SkillTracker trackSkillHiddenFireDance;
        private System.Windows.Forms.Label lblSkillHiddenFireDance;
        private Sims2Tools.Controls.SkillTracker trackSkillHiddenBreakDance;
        private System.Windows.Forms.Label lblSkillHiddenBreakDance;
    }
}