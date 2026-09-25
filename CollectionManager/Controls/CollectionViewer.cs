/*
 * Collection Manager - a utility for managing Sims 2 collections
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools;
using Sims2Tools.Cache.Objects;
using Sims2Tools.Cache.Outfits;
using Sims2Tools.Cache.Thumbnails;
using Sims2Tools.Clipboard;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.CPF;
using Sims2Tools.DBPF.Data;
using Sims2Tools.DBPF.Images.IMG;
using Sims2Tools.DBPF.OBJD;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph.BINX;
using Sims2Tools.DBPF.SceneGraph.COLL;
using Sims2Tools.DBPF.SceneGraph.GZPS;
using Sims2Tools.DBPF.SceneGraph.IDR;
using Sims2Tools.DBPF.STR;
using Sims2Tools.DBPF.Utils;
using Sims2Tools.DBPF.XOBJ;
using Sims2Tools.DbpfCache;
using Sims2Tools.Dialogs;
using Sims2Tools.DragDrop;
using Sims2Tools.Helpers;
using Sims2Tools.Utils.NamedValue;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
// Do NOT include System.Windows here
using System.Windows.Forms;

namespace CollectionManager.Controls
{
    public partial class CollectionViewer : UserControl
    {
        private static readonly Sims2Tools.DBPF.Logger.IDBPFLogger logger = Sims2Tools.DBPF.Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly Color colourDragBackground = Color.FromName(Properties.Settings.Default.DragBackground);
        private static readonly Color colourThumbnailBackground = Color.FromName(Properties.Settings.Default.ThumbnailBackground);

        #region Caches
        private static DbpfFileCache packageCache;

        private static ObjectsCache objectsCache;
        private static ObjectThumbnailsCache objectThumbnailsCache;

        private static OutfitCache clothingCache;
        private static ClothingThumbnailsCache clothingThumbnailsCache;

        public static void SetPackageCache(DbpfFileCache pkgCache)
        {
            packageCache = pkgCache;
        }

        public static void SetObjectsCache(ObjectsCache objCache, ObjectThumbnailsCache objThumbnailsCache)
        {
            objectsCache = objCache;
            objectThumbnailsCache = objThumbnailsCache;
        }

        public static void SetClothingCache(OutfitCache clothesCache, ClothingThumbnailsCache clothesThumbnailsCache)
        {
            clothingCache = clothesCache;
            clothingThumbnailsCache = clothesThumbnailsCache;
        }
        #endregion

        private string collectionFilePath;
        private DBPFKey collectionKey;

        private bool _needsUpdate_CollName = false;
        private bool _needsUpdate_SortValue = false;

        private bool _isDirty = false;
        public bool IsDirty => _isDirty;
        public void SetClean()
        {
            packageCache.SetClean(collectionFilePath);

            _isDirty = false;
        }

        private readonly CollectionDataTable dataCollItems = new CollectionDataTable();

        private readonly OpenFileDialog selectFileDialog;

        private bool isPreHashed = false;
        private bool needsFullReindex = false;

        public string TabName => new FileInfo(collectionFilePath).Name;
        public int CollectionSort
        {
            get => Int16.Parse(textCollSort.Text);
            set => textCollSort.Text = value.ToString();
        }

        public string CollectionFilePath
        {
            get => collectionFilePath;
            set
            {
                collectionFilePath = value;

                if (value != null)
                {
                    Reload();
                }
            }
        }

        public bool IsObjectCollection
        {
            get
            {
                if (comboCollType.SelectedIndex >= 0)
                {
                    return (comboCollType.SelectedItem as StringNamedValue).Value.EndsWith("collection");
                }

                return false;
            }
        }
        public bool IsClothingCollection
        {
            get
            {
                if (comboCollType.SelectedIndex >= 0)
                {
                    return (comboCollType.SelectedItem as StringNamedValue).Value.Equals("clothing");
                }

                return false;
            }
        }


        #region Constructor
        public CollectionViewer()
        {
            InitializeComponent();
            pictCollIcon.AllowDrop = true; // Why isn't this exposed in the designer?

            timerThumbnail.Interval = Properties.Settings.Default.ThumbnailTimerDelayMilliSec;

            if (Sims2ToolsLib.IsRunningOnWindows)
            {
                gridCollItems.MouseDown += new MouseEventHandler(this.OnMouseDown_CollItemsGrid);
            }

            gridCollItems.DataSource = dataCollItems;

            comboCollType.Items.Add(new StringNamedValue("Residential Lots", "collection"));
            comboCollType.Items.Add(new StringNamedValue("Community Lots", "communitylotcollection"));
            comboCollType.Items.Add(new StringNamedValue("Any Lots", "lotcollection"));
            comboCollType.Items.Add(new StringNamedValue("Clothing", "clothing"));

            thumbBox.BackColor = colourThumbnailBackground;

            selectFileDialog = new OpenFileDialog
            {
                Filter = "Image Files(*.bmp;*.jpg;*.png;*.gif;*.tif)|*.bmp;*.jpg;*.jpeg;*.png;*.gif;*.tif;*.tiff|All files(*.*)|*.*"
            };
        }
        #endregion

        #region Form Management
        private CollectionManagerForm GetMainForm()
        {
            Control mainForm = Parent;

            while (!(mainForm is Form))
            {
                mainForm = mainForm.Parent;
            }

            if (mainForm is CollectionViewerForm collForm)
            {
                mainForm = collForm.ManagerForm;
            }

            return mainForm as CollectionManagerForm;
        }

        // This is needed as User Controls seem to be unable to send the correct size to their child controls - go figure!
        private void OnResize(object sender, EventArgs e)
        {
            panelViewer.Size = new Size(this.Size.Width, this.Size.Height);
        }

        private void UpdateSaveState()
        {
            btnSave.Enabled = IsDirty;
        }
        #endregion

        #region Collection Loading
        private bool ignoreChanges = false;
        public void Reload()
        {
            ignoreChanges = true;

            CommitNeededChanges();

            try
            {
                Coll coll = null;
                Str str = null;
                Img img = null;

                string packageName;

                using (CacheableDbpfFile package = packageCache.OpenForReadOnly(collectionFilePath))
                {
                    packageName = package.PackageName;

                    collectionKey = GetCollKey(package);

                    if (collectionKey != null)
                    {
                        coll = GetCollResource(package, collectionKey, out Idr idrForColl);
                        str = GetStrResource(package, coll, idrForColl);
                        img = GetImgResource(package, coll, idrForColl);
                    }

                    _isDirty = package.IsDirty;

                    package.Close();
                }

                if (collectionKey != null)
                {
                    if (str == null || img == null)
                    {
                        if (RepairPrehashes())
                        {
                            MsgBox.Show($"{packageName} has been repaired.\r\n(It was probably renamed manually.)", "Collection Repaired", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            using (CacheableDbpfFile package = packageCache.OpenForReadOnly(collectionFilePath))
                            {
                                coll = GetCollResource(package, collectionKey, out Idr idrForColl);
                                str = GetStrResource(package, coll, idrForColl);
                                img = GetImgResource(package, coll, idrForColl);

                                package.Close();
                            }

                            _isDirty = true;
                        }
                        else
                        {
                            MsgBox.Show($"{packageName} appears to be broken.\r\nIt was probably renamed manually.", "Collection Load Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }

                    // Collection name and sort value
                    textCollName.Text = str?.LanguageItems(MetaData.Languages.Default)?[coll.GetItem("stringindex").IntegerValue].Title;
                    textCollSort.Text = coll.SortIndex.ToString();

                    // Collection icon
                    pictCollIcon.BackgroundImage = img?.Image;

                    using (CacheableDbpfFile package = packageCache.OpenForReadOnly(collectionFilePath))
                    {
                        // Collection items
                        ReloadCollItems(package);

                        package.Close();
                    }

                    // Collection type - do this AFTER the collection items
                    string collType = coll.GetItem("type").StringValue;

                    if (!string.IsNullOrWhiteSpace(collType))
                    {
                        if (gridCollItems.Rows.Count > 0)
                        {
                            // Collection has items, so restrict what its type can be changed to
                            comboCollType.Items.Clear();

                            if (collType.Equals("clothing"))
                            {
                                comboCollType.Items.Add(new StringNamedValue("Clothing", "clothing"));
                            }
                            else
                            {
                                comboCollType.Items.Add(new StringNamedValue("Residential Lots", "collection"));
                                comboCollType.Items.Add(new StringNamedValue("Community Lots", "communitylotcollection"));
                                comboCollType.Items.Add(new StringNamedValue("Any Lots", "lotcollection"));
                            }
                        }

                        foreach (object o in comboCollType.Items)
                        {
                            if ((o as StringNamedValue).Value.Equals(collType))
                            {
                                comboCollType.SelectedItem = o;
                                break;
                            }
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                logger.Error(ex.Message);
                logger.Info(ex.StackTrace);
            }

            UpdateSaveState();

            ignoreChanges = false;
        }

        private void ReloadCollItems(CacheableDbpfFile package)
        {
            dataCollItems.Clear();

            foreach (DBPFEntry binxEntry in package.GetEntriesByType(Binx.TYPE))
            {
                Binx binx = (Binx)package.GetResourceByEntry(binxEntry);
                Idr idrForBinx = (Idr)package.GetResourceByKey(new DBPFKey(Idr.TYPE, binxEntry));

                int sort = binx.SortIndex;

                DBPFKey itemKey = IdrHelper.ObjectKey(binx, idrForBinx);
                if (itemKey != null)
                {
                    if (itemKey.TypeID == (TypeTypeID)0x00000000)
                    {
                        if (itemKey.GroupID == (TypeGroupID)0x00000000)
                        {
                            // Bring this weirdness into the fold
                            itemKey = new DBPFKey(Coll.COLLITEM_OBJD, itemKey);
                        }
                    }

                    if (itemKey.TypeID == Coll.COLLITEM_OBJD)
                    {
                        TypeGUID guid = (TypeGUID)itemKey.InstanceID.AsUInt();

                        if (objectsCache.ContainsKey(guid))
                        {
                            ObjectData data = objectsCache.GetData(guid);

                            dataCollItems.Append(itemKey, binxEntry, "Object", $"{data.ResTitle ?? data.ResName}", sort, data.ResKey, data);
                        }
                        else
                        {
                            dataCollItems.Append(itemKey, binxEntry, "Object", $"GUID: {guid}", sort, null, null);
                        }
                    }
                    else if (itemKey.TypeID == Coll.COLLITEM_XOBJ)
                    {
                        uint guidHash = itemKey.InstanceID.AsUInt();
                        string objectName;
                        ObjectData data = null;

                        if (objectsCache.ContainsKey(guidHash))
                        {
                            data = objectsCache.GetData(guidHash);

                            objectName = data.ResTitle ?? data.ResName;
                        }
                        else
                        {
                            objectName = $"Hash of GUID: {Helper.Hex8PrefixString(guidHash)}";
                        }

                        if (itemKey.GroupID == (TypeGroupID)0x00000001)
                        {
                            dataCollItems.Append(itemKey, binxEntry, "Floor Covering", objectName, sort, data?.ResKey, data);
                        }
                        else if (itemKey.GroupID == (TypeGroupID)0x00000002)
                        {
                            dataCollItems.Append(itemKey, binxEntry, "Wall Covering", objectName, sort, data?.ResKey, data);
                        }
                        else
                        {
#if DEBUG
                            dataCollItems.Append(itemKey, binxEntry, "UNKNOWN", $"{itemKey}", int.MinValue, null, null);
#endif
                            logger.Warn($"Unknown sub-type ({itemKey.GroupID}) in {binx as DBPFKey}");
                        }
                    }
                    else if (itemKey.TypeID == Coll.COLLITEM_GZPS)
                    {
                        DBPFKey gzpsKey = itemKey;

                        if (clothingCache.ContainsKey(itemKey))
                        {
                            CasOutfitData data = clothingCache.GetData(itemKey);

                            dataCollItems.Append(itemKey, binxEntry, "Clothing", $"{data.ResName}", sort, gzpsKey, null);
                        }
                        else
                        {
                            dataCollItems.Append(itemKey, binxEntry, "Clothing", $"{itemKey}", sort, gzpsKey, null);
                        }
                    }
                    else
                    {
#if DEBUG
                        dataCollItems.Append(itemKey, binxEntry, "UNKNOWN", $"{itemKey}", int.MinValue, null, null);
#endif
                        logger.Warn($"Unknown type ({itemKey.TypeID}) in {binx as DBPFKey}");
                    }
                }
            }

            dataCollItems.Finished(out needsFullReindex);
        }

        private DBPFKey GetCollKey(CacheableDbpfFile package)
        {
            List<DBPFEntry> collEntries = package.GetEntriesByType(Coll.TYPE);

            if (collEntries.Count == 0)
            {
                MsgBox.Show($"{package.PackageName} does not contain a COLL resource", "Collection Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (collEntries.Count == 1)
            {
                return new DBPFKey(collEntries[0]);
            }
            else
            {
                MsgBox.Show($"{package.PackageName} contains multiple COLL resources", "Collection Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return null;
        }

        private Coll GetCollResource(CacheableDbpfFile package, DBPFKey collKey, out Idr idrForColl)
        {

            Coll coll = (Coll)package.GetResourceByKey(collKey);
            idrForColl = (Idr)package.GetResourceByKey(new DBPFKey(Idr.TYPE, coll));

            return coll;
        }

        private Str GetStrResource(CacheableDbpfFile package, Coll coll, Idr idrForColl)
        {
            DBPFKey strKey = IdrHelper.StringSetKey(coll, idrForColl);
            Str str = (Str)package.GetResourceByKey(strKey);

            if (str == null && strKey.GroupID.Equals(Hashes.GroupIDHash(package.PackageNameNoExtn)))
            {
                isPreHashed = true;

                strKey = new DBPFKey(strKey.TypeID, DBPFData.GROUP_LOCAL, strKey.InstanceID, strKey.ResourceID);
                str = (Str)package.GetResourceByKey(strKey);
            }

            return str;
        }

        private Img GetImgResource(CacheableDbpfFile package, Coll coll, Idr idrForColl)
        {
            DBPFKey imgKey = IdrHelper.IconKey(coll, idrForColl);
            Img img = (Img)package.GetResourceByKey(imgKey);

            if (img == null && imgKey.GroupID.Equals(Hashes.GroupIDHash(package.PackageNameNoExtn)))
            {
                isPreHashed = true;

                imgKey = new DBPFKey(imgKey.TypeID, DBPFData.GROUP_LOCAL, imgKey.InstanceID, imgKey.ResourceID);
                img = (Img)package.GetResourceByKey(imgKey);
            }

            return img;
        }
        #endregion

        #region Mouse Management
        private int mouseRowIndex = -1;
        private Point lastMouseAt;

        private void OnCellMouseEnter_GridCollItems(object sender, DataGridViewCellEventArgs e)
        {
            mouseRowIndex = e.RowIndex;

            lastMouseAt = Cursor.Position;
            timerThumbnail.Start();
        }

        private void OnCellMouseLeave_GridCollItems(object sender, DataGridViewCellEventArgs e)
        {
            thumbBox.Visible = false;
            timerThumbnail.Stop();
        }

        private void OnTimerTick_Thumbnail(object sender, EventArgs e)
        {
            timerThumbnail.Stop();

            if (dragLabel != null) return;

            Point mousePosition = Cursor.Position;

            Image thumbnail = null;

            Point p = gridCollItems.PointToClient(new Point(mousePosition.X, mousePosition.Y));
            mouseRowIndex = gridCollItems.HitTest(p.X, p.Y).RowIndex;

            if (mouseRowIndex >= 0 && mouseRowIndex < gridCollItems.RowCount)
            {
                DataGridViewRow row = gridCollItems.Rows[mouseRowIndex];

                if (row.Cells["colKey"].Value is DBPFKey key)
                {
                    if (key.TypeID == Gzps.TYPE)
                    {
                        if (clothingCache.ContainsKey(key))
                        {
                            CasOutfitData data = clothingCache.GetData(key);

                            thumbnail = clothingThumbnailsCache.GetThumbnail(data.ThumbKey, key);

                            thumbBox.Size = new Size(128, 128);
                        }
                    }
                    else
                    {
                        ObjectData data = row.Cells["colData"].Value as ObjectData;

                        thumbnail = objectThumbnailsCache.GetObjectThumbnail(data.ThumbKey);

                        thumbBox.Size = new Size(96, 96);
                    }
                }
            }

            if (thumbnail != null)
            {
                thumbBox.Image = thumbnail;

                Point panelLocationOnScreen = panelViewer.PointToScreen(panelViewer.Location);

                Control myPanel = Parent;

                int fudge = 20; // A fudge factor so the thumbnail doesn't sit on the bottom of the app's window
                int thumbY = (mousePosition.Y - panelLocationOnScreen.Y);
                if ((thumbY + thumbBox.Size.Height + fudge) > myPanel.Size.Height)
                {
                    thumbY = myPanel.Size.Height - thumbBox.Size.Height - fudge;
                }
                thumbBox.Location = new Point(mousePosition.X - panelLocationOnScreen.X + 10, thumbY);

                thumbBox.Visible = true;
            }
        }
        #endregion

        #region Controls Changed
        public void CommitNeededChanges()
        {
            if (_needsUpdate_CollName || _needsUpdate_SortValue)
            {
                using (CacheableDbpfFile package = packageCache.OpenForUpdate(collectionFilePath))
                {
                    Coll coll = GetCollResource(package, collectionKey, out Idr idrForColl);

                    if (_needsUpdate_CollName)
                    {
                        Str str = GetStrResource(package, coll, idrForColl);

                        List<StrItem> items = str.LanguageItems(MetaData.Languages.Default);
                        int index = coll.GetItem("stringindex").IntegerValue;

                        if (items != null && items.Count > index)
                        {
                            items[index].Title = textCollName.Text;
                            _needsUpdate_CollName = false;
                            _isDirty = true;

                            package.Commit(str);
                        }
                    }

                    if (_needsUpdate_SortValue)
                    {
                        coll.GetOrAddItem("sortindex", MetaData.DataTypes.dtInteger).IntegerValue = CollectionSort;
                        _needsUpdate_SortValue = false;
                        _isDirty = true;

                        package.Commit(coll);
                    }

                    package.Close();
                }
            }
        }

        private void OnCollNameChanged(object sender, EventArgs e)
        {
            if (ignoreChanges) return;

            _isDirty = true;
            _needsUpdate_CollName = true;

            UpdateSaveState();
        }

        private void OnCollSortValueChanged(object sender, EventArgs e)
        {
            if (ignoreChanges) return;

            _isDirty = true;
            _needsUpdate_SortValue = true;

            UpdateSaveState();
        }

        private void OnCollTypeChanged(object sender, EventArgs e)
        {
            if (ignoreChanges) return;

            using (CacheableDbpfFile package = packageCache.OpenForUpdate(collectionFilePath))
            {
                Coll coll = GetCollResource(package, collectionKey, out Idr _);

                string collType = (comboCollType.SelectedItem as StringNamedValue).Value;
                coll.GetOrAddItem("type", MetaData.DataTypes.dtString).StringValue = collType;

                if (collType.Equals("lotcollection") || collType.Equals("clothing"))
                {
                    coll.GetItem("flags").UIntegerValue = 0x00000012;
                }
                else
                {
                    coll.GetItem("flags").UIntegerValue = 0x00000002;
                }

                _isDirty = true;

                package.Commit(coll);
                package.Close();
            }

            UpdateSaveState();
        }
        #endregion

        #region Icon Update
        private void OnIconContext_ChangeIcon(object sender, EventArgs e)
        {
            ChangeIcon();
        }

        #endregion

        #region Menu Helpers
        public void SaveAsCollection(bool autoBackup)
        {
            if (saveAsFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (!string.IsNullOrWhiteSpace(saveAsFileDialog.FileName))
                {
                    string packageFile = saveAsFileDialog.FileName;

                    if (packageFile != null && File.Exists(packageFile))
                    {
                        if (autoBackup)
                        {
                            if (File.Exists($"{packageFile}.bak"))
                            {
                                try
                                {
                                    Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile($"{packageFile}.bak", Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs, Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);
                                }
                                catch (Exception)
                                {
                                    MsgBox.Show($"Error trying to remove {packageFile}.bak, you should delete this file manually.", "Package Save As Error!");
                                    return;
                                }
                            }

                            try
                            {
                                File.Move(packageFile, $"{packageFile}.bak");
                            }
                            catch (Exception)
                            {
                                MsgBox.Show($"Error trying to backup {packageFile}, possibly open in SimPe.", "Package Save As Error!");
                                return;
                            }
                        }
                        else
                        {
                            try
                            {
                                Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(packageFile, Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs, Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);
                            }
                            catch (Exception)
                            {
                                MsgBox.Show($"Error trying to remove {packageFile}, possibly open in SimPe.", "Package Save As Error!");
                                return;
                            }
                        }
                    }

                    CommitNeededChanges();
                    ResolvePrehashes(false);

                    using (CacheableDbpfFile package = packageCache.OpenForReadOnly(CollectionFilePath))
                    {
                        if (package.SaveAs(packageFile) == null)
                        {
                            MsgBox.Show($"Error trying to save {package.PackageName}, file is probably open in SimPe!\n\nChanges are in the associated .temp file.", "Package Save Error!");
                        }

                        // Do NOT use SetClean() here, as it doesn't decache the open package file
                        _isDirty = false;
                        packageCache.SetClean(package);

                        package.Close();
                        collectionFilePath = packageFile;
                    }

                    UpdateSaveState();

                    {
                        // Update the tab/window name
                        Control parentCtrl = Parent;

                        while (parentCtrl != null)
                        {
                            if (parentCtrl is CollectionViewerTab)
                            {
                                parentCtrl.Text = TabName;
                                break;
                            }
                            else if (parentCtrl is CollectionViewerForm)
                            {
                                parentCtrl.Text = $"{CollectionManagerApp.AppTitle} - {TabName}";
                                break;
                            }

                            parentCtrl = parentCtrl.Parent;
                        }
                    }

                    return;
                }
            }

            return;
        }

        public void SaveCollection(bool autoBackup)
        {
            CommitNeededChanges();

            using (CacheableDbpfFile package = packageCache.OpenForReadOnly(CollectionFilePath))
            {
                if (package.IsDirty)
                {
                    if (package.Update(autoBackup) == null)
                    {
                        MsgBox.Show($"Error trying to update {package.PackageName}, file is probably open in SimPe!\n\nChanges are in the associated .temp file.", "Package Update Error!");
                    }

                    // Do NOT use SetClean() here, as it doesn't decache the open package file
                    _isDirty = false;
                    packageCache.SetClean(package);
                }

                package.Close();
            }

            UpdateSaveState();
        }

        public void ChangeIcon()
        {
            selectFileDialog.InitialDirectory = $"{Sims2ToolsLib.Sims2CollectionsPath}\\Icons";
            selectFileDialog.FileName = "*.png";

            if (selectFileDialog.ShowDialog() == DialogResult.OK)
            {
                UpdateIcon(selectFileDialog.FileName);
            }
        }

        public void UpdateIcon(string imagePath)
        {
            try
            {
                using (Image image = Image.FromFile(imagePath))
                {
                    if (image.Width > Properties.Settings.Default.MaxIconWidth)
                    {
                        MsgBox.Show("The selected image is too wide", "Icon Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    else if (image.Height > Properties.Settings.Default.MaxIconHeight)
                    {
                        MsgBox.Show("The selected image is too tall", "Icon Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    Image iconImage = image;

                    if (iconImage.Width != 28 || iconImage.Height != 22)
                    {
                        iconImage = ImageHelper.ResizeImage(iconImage, 28, 22);
                    }

                    Image collIcon = new Bitmap(iconImage);

                    pictCollIcon.BackgroundImage = collIcon;

                    using (CacheableDbpfFile package = packageCache.OpenForUpdate(collectionFilePath))
                    {
                        Coll coll = GetCollResource(package, collectionKey, out Idr idrForColl);

                        Img img = GetImgResource(package, coll, idrForColl);

                        img.Image = collIcon;
                        _isDirty = true;

                        package.Commit(img);
                        package.Close();
                    }
                }
            }
            catch (Exception)
            {
            }

            UpdateSaveState();
        }

        public bool RenameCollection()
        {
            if (IsDirty)
            {
                MsgBox.Show("Cannot rename a collection with unsaved changes.", "Collection Rename Error");

                return false;
            }

            FileEntryDialog rename = new FileEntryDialog("Collection Package Rename", "Please enter a new name for the collection package", new FileInfo(collectionFilePath).Name, ".package");

            if (rename.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(rename.FileEntry))
            {
                return RenameCollectionTo(rename.FileEntry);
            }

            return false;
        }

        public bool RenameCollectionTo(string newName)
        {
            if (!newName.EndsWith(".package", StringComparison.CurrentCultureIgnoreCase))
            {
                newName += ".package";
            }

            FileInfo fiOld = new FileInfo(CollectionFilePath);
            FileInfo fiNew = new FileInfo($"{fiOld.DirectoryName}/{newName}");

            if (!fiOld.Name.Equals(newName))
            {
                if (fiNew.Exists)
                {
                    MsgBox.Show("File already exists", "Error!", MessageBoxButtons.OK);
                }
                else
                {
                    try
                    {
                        ResolvePrehashes(true);

                        Microsoft.VisualBasic.FileIO.FileSystem.RenameFile(fiOld.FullName, fiNew.Name);
                        collectionFilePath = fiNew.FullName;

                        return true;
                    }
                    catch (Exception) { }
                }
            }

            return false;
        }

        private bool RepairPrehashes()
        {
            bool repairable = false;
            uint preHashGroup;

            using (CacheableDbpfFile package = packageCache.OpenForReadOnly(collectionFilePath))
            {
                Coll coll = GetCollResource(package, collectionKey, out Idr idrForColl);
                DBPFKey strKey = IdrHelper.StringSetKey(coll, idrForColl);
                DBPFKey imgKey = IdrHelper.IconKey(coll, idrForColl);

                repairable = (strKey.GroupID != DBPFData.GROUP_LOCAL && strKey.GroupID == imgKey.GroupID);
                preHashGroup = strKey.GroupID.AsUInt();

                package.Close();
            }

            if (repairable)
            {
                using (CacheableDbpfFile package = packageCache.OpenForUpdate(collectionFilePath))
                {
                    Coll coll = GetCollResource(package, collectionKey, out Idr idrForColl);

                    // For each entry ending "groupid" in the COLL resource, if the value is the pre - hashed value, change it to 0xFFFFFFFF(4294967295)
                    {
                        PatchPreHashGroups(coll, preHashGroup);

                        package.Commit(coll);
                    }

                    // For each entry in each 3IDR resource, if the group id is the pre-hashed value, change it to 0xFFFFFFFF
                    {
                        if (idrForColl != null)
                        {
                            for (uint index = 0; index < idrForColl.ItemCount; ++index)
                            {
                                DBPFKey key = idrForColl.GetItem(index);

                                if (key.GroupID.AsUInt() == preHashGroup)
                                {
                                    idrForColl.SetItem(index, new DBPFKey(key.TypeID, DBPFData.GROUP_NULL, key.InstanceID, key.ResourceID));
                                }
                            }

                            package.Commit(idrForColl);
                        }
                    }

                    // For each entry ending "groupid" in each BINX resource, if the value is the pre - hashed value, change it to 0xFFFFFFFF(4294967295)
                    {
                        foreach (DBPFEntry entry in package.GetEntriesByType(Binx.TYPE))
                        {
                            Binx binx = (Binx)package.GetResourceByEntry(entry);
                            PatchPreHashGroups(binx, preHashGroup);

                            package.Commit(binx);
                        }
                    }

                    package.Close();
                }
            }

            return repairable;
        }

        private void ResolvePrehashes(bool doUpdate)
        {
            if (isPreHashed)
            {
                using (CacheableDbpfFile package = packageCache.OpenForReadOnly(collectionFilePath))
                {
                    uint preHashGroup = Hashes.GroupIDHash(package.PackageNameNoExtn).AsUInt();

                    Coll coll = GetCollResource(package, collectionKey, out Idr idrForColl);

                    // For each entry ending "groupid" in the COLL resource, if the value is the pre - hashed value, change it to 0xFFFFFFFF(4294967295)
                    {
                        PatchPreHashGroups(coll, preHashGroup);

                        package.Commit(coll);
                    }

                    // For each entry in each 3IDR resource, if the group id is the pre-hashed value, change it to 0xFFFFFFFF
                    {
                        if (idrForColl != null)
                        {
                            for (uint index = 0; index < idrForColl.ItemCount; ++index)
                            {
                                DBPFKey key = idrForColl.GetItem(index);

                                if (key.GroupID.AsUInt() == preHashGroup)
                                {
                                    idrForColl.SetItem(index, new DBPFKey(key.TypeID, DBPFData.GROUP_NULL, key.InstanceID, key.ResourceID));
                                }
                            }

                            package.Commit(idrForColl);
                        }
                    }

                    // For each entry ending "groupid" in each BINX resource, if the value is the pre - hashed value, change it to 0xFFFFFFFF(4294967295)
                    {
                        foreach (DBPFEntry entry in package.GetEntriesByType(Binx.TYPE))
                        {
                            Binx binx = (Binx)package.GetResourceByEntry(entry);
                            PatchPreHashGroups(binx, preHashGroup);

                            package.Commit(binx);
                        }
                    }

                    if (doUpdate && package.IsDirty) package.Update(false);

                    package.Close();
                }

                isPreHashed = false;
            }
        }

        private void PatchPreHashGroups(Cpf cpf, uint preHashGroup)
        {
            foreach (string itemName in cpf.GetItemNames())
            {
                if (itemName.EndsWith("groupid"))
                {
                    CpfItem item = cpf.GetItem(itemName);

                    if (item.UIntegerValue == preHashGroup)
                    {
                        item.UIntegerValue = DBPFData.GROUP_LOCAL.AsUInt();
                    }
                }
            }
        }

        public bool DeleteCollection(bool confirmDelete)
        {
            if (!confirmDelete || MsgBox.Show($"Delete {collectionFilePath}?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(collectionFilePath, Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs, Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);

                    return true;
                }
                catch (Exception)
                {
                    MsgBox.Show($"Error trying to remove {collectionFilePath}, you should delete this file manually.", "Package Delete Error!");
                }
            }

            return false;
        }
        #endregion

        #region Items Context Menu Actions
        private void OnCollItemsContext_Opened(object sender, EventArgs e)
        {
            thumbBox.Visible = false;
        }

        private void OnCollItems_Opening(object sender, CancelEventArgs e)
        {
            if (gridCollItems.Rows.Count < 1)
            {
                menuItemCollContextClipboardAfter.Text = "Paste From Clipboard";
                menuItemCollContextClipboardAfter.Enabled = ClipboardIsUsable();

                menuItemCollContextClipboardBefore.Visible = false;

                menuItemCollContextClipboardCopyTo.Enabled = false;

                menuItemCollContextBefore.Enabled = menuItemCollContextAfter.Enabled = false;
                menuItemCollContextDelete.Enabled = false;
            }
            else
            {
                // Bail early if the mouse is not over any row
                if (mouseRowIndex == -1)
                {
                    e.Cancel = true;
                    return;
                }

                // Figure out what was right-clicked over
                bool overSelected = false;
                bool anySelected = false;
                foreach (DataGridViewRow mouseRow in gridCollItems.SelectedRows)
                {
                    if (mouseRowIndex == mouseRow.Index)
                    {
                        overSelected = true;
                    }

                    anySelected = true;
                }

                // Some rows have to be selected
                {
                    menuItemCollContextClipboardCopyTo.Enabled = anySelected;
                }

                // Mouse has to be over any row
                {
                    menuItemCollContextClipboardAfter.Text = "Paste From Clipboard AFTER";
                    menuItemCollContextClipboardAfter.Enabled = ClipboardIsUsable();

                    menuItemCollContextClipboardBefore.Visible = true;
                    menuItemCollContextClipboardBefore.Enabled = menuItemCollContextClipboardAfter.Enabled;
                }

                // Mouse has to be over a selected row
                {
                    menuItemCollContextClipboardCopyTo.Enabled = overSelected;

                    menuItemCollContextDelete.Enabled = overSelected;
                }

                // Mouse has to NOT be over a selected row
                {
                    menuItemCollContextAfter.Enabled = menuItemCollContextBefore.Enabled = !overSelected && anySelected;
                }
            }
        }

        private bool ClipboardIsUsable()
        {
            logger.Debug("Clipboard: Looking for stuff");
            return ClipboardContainsCollItems() || ClipboardContainsPackageFiles();
        }

        private bool ClipboardContainsPackageFiles()
        {
            try
            {
                if (ClipboardHelper.ContainsFileList)
                {
                    logger.Debug("Clipboard: Found a file list");
                    foreach (string path in ClipboardHelper.FileList)
                    {
                        if (path.EndsWith(".package"))
                        {
                            logger.Debug("Clipboard: Found (at least one) .package file");
                            return true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                logger.Debug(e.Message);
            }

            return false;
        }

        private bool ClipboardContainsCollItems()
        {
            try
            {
                return ClipboardHelper.ContainsCollItems(collectionKey);
            }
            catch (Exception e)
            {
                logger.Debug(e.Message);
            }

            return false;
        }

        private void OnCollItemsContext_Delete(object sender, EventArgs e)
        {
            int rowIndex = mouseRowIndex;

            while (rowIndex > 0 && gridCollItems.Rows[rowIndex].Selected)
            {
                --rowIndex;
            }

            List<DataGridViewRow> selectedRows = new List<DataGridViewRow>();

            foreach (DataGridViewRow row in gridCollItems.SelectedRows)
            {
                selectedRows.Add(row);
            }

            gridCollItems.ClearSelection();

            using (CacheableDbpfFile collPackage = packageCache.OpenForUpdate(collectionFilePath))
            {
                foreach (DataGridViewRow row in selectedRows)
                {
                    DBPFKey binxKey = row.Cells["colBinxKey"].Value as DBPFKey;
                    DBPFKey idrKey = new DBPFKey(Idr.TYPE, binxKey);

                    dataCollItems.RemoveByBinxKey(binxKey);

                    collPackage.Remove(binxKey);
                    collPackage.Remove(idrKey);

                    _isDirty = true;
                }

                DBPFKey inViewItemKey = (gridCollItems.Rows[rowIndex].Cells["colItemKey"].Value as DBPFKey);

                ReloadCollItems(collPackage); // Need this to remove the deleted row(s)
                FullReindexRows(collPackage);
                ReloadCollItems(collPackage);

                BringIntoView(inViewItemKey);

                collPackage.Close();
            }

            UpdateSaveState();
        }
        #endregion

        #region Reorder Rows (helpers)
        private int moveRowLow, moveRowHigh;
        private readonly List<DataGridViewRow> dragRows = new List<DataGridViewRow>();

        private void GetSelectedRows()
        {
            moveRowLow = gridCollItems.Rows.Count;
            moveRowHigh = -1;

            foreach (DataGridViewRow row in gridCollItems.SelectedRows)
            {
                if (row.Index < moveRowLow) moveRowLow = row.Index;
                if (row.Index > moveRowHigh) moveRowHigh = row.Index;
            }
        }

        private void PreDragRows()
        {
            dragRows.Clear();

            foreach (DataGridViewRow row in gridCollItems.SelectedRows)
            {
                dragRows.Add(row);
            }

            HighlightDragRows();
        }

        private void HighlightDragRows()
        {
            gridCollItems.ClearSelection();

            foreach (DataGridViewRow row in dragRows)
            {
                row.DefaultCellStyle.BackColor = colourDragBackground;
            }
        }

        private void UnhighlightDragRows()
        {
            foreach (DataGridViewRow row in dragRows)
            {
                row.DefaultCellStyle.BackColor = Color.Empty;
            }
        }

        private void PostMoveRows()
        {
            gridCollItems.Refresh();
            gridCollItems.ClearSelection();

            UpdateSaveState();
        }

        private void MoveSelectedRowsBefore(int targetRowIndex)
        {
            if (targetRowIndex >= moveRowLow && targetRowIndex <= moveRowHigh)
            {
                // Can't move into the selected row(s)
            }
            else
            {
                int movedRowCount = moveRowHigh - moveRowLow + 1;

                if (targetRowIndex < moveRowLow)
                {
                    // Moving upwards, move to BEFORE the target row
                    for (int i = 0; i < movedRowCount; ++i)
                    {
                        MoveRow(moveRowLow + i, targetRowIndex + i);
                    }

                    ReindexRows(targetRowIndex, moveRowHigh);
                }
                else
                {
                    // Moving downwards, move to BEFORE the target row
                    for (int i = 0; i < movedRowCount; ++i)
                    {
                        MoveRow(moveRowLow, targetRowIndex - 1);
                    }

                    ReindexRows(moveRowLow, targetRowIndex);
                }

                PostMoveRows();
            }
        }

        private void MoveSelectedRowsAfter(int targetRowIndex)
        {
            if (targetRowIndex >= moveRowLow && targetRowIndex <= moveRowHigh)
            {
                // Can't move into the selected row(s)
            }
            else
            {
                int movedRowCount = moveRowHigh - moveRowLow + 1;

                if (targetRowIndex < moveRowLow)
                {
                    // Moving upwards, move to AFTER the target row
                    for (int i = 0; i < movedRowCount; ++i)
                    {
                        MoveRow(moveRowLow + i, targetRowIndex + i + 1);
                    }

                    ReindexRows(targetRowIndex, moveRowHigh);
                }
                else
                {
                    // Moving downwards, move to AFTER the drop row
                    for (int i = 0; i < movedRowCount; ++i)
                    {
                        MoveRow(moveRowLow, targetRowIndex);
                    }

                    ReindexRows(moveRowLow, targetRowIndex);
                }

                PostMoveRows();
            }
        }

        private void MoveDraggedRows(int dropRowIndex)
        {
            if (dropRowIndex >= moveRowLow && dropRowIndex <= moveRowHigh)
            {
                // Can't drop onto the selected row(s)
                UnhighlightDragRows();
            }
            else
            {
                int movedRowCount = moveRowHigh - moveRowLow + 1;

                if (dropRowIndex < dragRowIndex)
                {
                    // Dragging UPWARDS
                    int offset = GetMainForm().GetMouseDropOffset(true);

                    for (int i = 0; i < movedRowCount; ++i)
                    {
                        MoveRow(moveRowLow + i, dropRowIndex + i + offset);
                    }

                    ReindexRows(dropRowIndex, moveRowHigh);
                }
                else
                {
                    // Dragging DOWNWARDS
                    int offset = GetMainForm().GetMouseDropOffset(false);

                    for (int i = 0; i < movedRowCount; ++i)
                    {
                        MoveRow(moveRowLow, dropRowIndex - 1 + offset);
                    }

                    ReindexRows(moveRowLow, dropRowIndex);
                }

                PostMoveRows();
            }
        }
        #endregion

        #region Reorder Rows (by mouse move)
        private void OnCollItemsContext_MoveBefore(object sender, EventArgs e)
        {
            DBPFKey inViewItemKey = (gridCollItems.Rows[mouseRowIndex].Cells["colItemKey"].Value as DBPFKey);

            GetSelectedRows();
            MoveSelectedRowsBefore(mouseRowIndex);

            BringIntoView(inViewItemKey);
        }

        private void OnCollItemsContext_MoveAfter(object sender, EventArgs e)
        {
            DBPFKey inViewItemKey = (gridCollItems.Rows[mouseRowIndex].Cells["colItemKey"].Value as DBPFKey);

            GetSelectedRows();
            MoveSelectedRowsAfter(mouseRowIndex);

            BringIntoView(inViewItemKey);
        }
        #endregion

        #region Reorder Rows (by drag and drop)
        // TODO - Collection Manager - items drag-and-drop - check all this logic
        private bool isLeftMouseDown = false;
        private Point leftButtonDownAt;
        private int dragRowIndex = -1;
        private Label dragLabel = null;

        private void OnCellMouseDown_GridCollItems(object sender, DataGridViewCellMouseEventArgs e)
        {
            isLeftMouseDown = ((e.Button & MouseButtons.Left) == MouseButtons.Left);
            if (isLeftMouseDown) leftButtonDownAt = e.Location;
        }

        private void OnCellMouseMove_GridCollItems(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Do the thumnail tracking stuff first
            {
                if (Math.Abs(Cursor.Position.X - lastMouseAt.X) > System.Windows.SystemParameters.MinimumHorizontalDragDistance || 
                    Math.Abs(Cursor.Position.Y - lastMouseAt.Y) > System.Windows.SystemParameters.MinimumVerticalDragDistance)
                {
                    timerThumbnail.Stop();
                    timerThumbnail.Start();
                    lastMouseAt = Cursor.Position;
                }
            }

            // Bail early if not over an item
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;

            // Is this the start of a drag operation?
            if (isLeftMouseDown && dragLabel == null)
            {
                int xDelta = Math.Abs(e.Location.X - leftButtonDownAt.X);
                int yDelta = Math.Abs(e.Location.Y - leftButtonDownAt.Y);

                if (xDelta >= System.Windows.SystemParameters.MinimumHorizontalDragDistance || yDelta >= System.Windows.SystemParameters.MinimumVerticalDragDistance)
                {
                    dragRowIndex = e.RowIndex;

                    GetSelectedRows();
                    PreDragRows();

                    dragLabel = new Label
                    {
                        Parent = gridCollItems,
                        AutoSize = true,
                        Text = gridCollItems[2, e.RowIndex].Value.ToString()
                    };

                    if (moveRowLow != moveRowHigh)
                    {
                        int others = moveRowHigh - moveRowLow;
                        dragLabel.Text += $" + {others} other{(others == 1 ? "" : "s")}";
                    }

                    thumbBox.Visible = false;
                }
            }

            if (dragLabel != null)
            {
                Rectangle r = gridCollItems.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                dragLabel.Location = new Point(r.Left + e.Location.X, r.Bottom);

                // TODO - Collection Manager - do we really need this?
                // HighlightDragRows();
            }
        }

        private void OnCellMouseUp_GridCollItems(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Dragging to reorder items within the collection
            if (dragLabel != null)
            {
                if (e.RowIndex >= 0)
                {
                    if (dragRowIndex >= 0)
                    {
                        DBPFKey inViewItemKey = (gridCollItems.Rows[e.RowIndex].Cells["colItemKey"].Value as DBPFKey);

                        MoveDraggedRows(e.RowIndex);

                        BringIntoView(inViewItemKey);
                    }
                }

                dragLabel?.Dispose();
                dragLabel = null;
            }

            isLeftMouseDown = false;
        }

        private void MoveRow(int fromIndex, int toIndex)
        {
            DataRow moveRow = dataCollItems.Rows[fromIndex];
            DataRow newRow = dataCollItems.NewRow();
            newRow.ItemArray = moveRow.ItemArray; // clone the values, as once removed they are deleted

            dataCollItems.Rows.Remove(moveRow);
            dataCollItems.Rows.InsertAt(newRow, toIndex);
        }

        private void FullReindexRows(CacheableDbpfFile collPackage)
        {
            if (needsFullReindex)
            {
                int sortValue = 0;

                foreach (DataGridViewRow row in gridCollItems.Rows)
                {
                    DBPFKey binxKey = row.Cells["colBinxKey"].Value as DBPFKey;
                    Binx binx = (Binx)collPackage.GetResourceByKey(binxKey);

                    binx.GetOrAddItem("sortindex", MetaData.DataTypes.dtInteger).IntegerValue = sortValue++;
                    _isDirty = true;

                    collPackage.Commit(binx);
                }

                needsFullReindex = false;
            }
        }

        private void ReindexRows(int fromIndex, int toIndex)
        {
            using (CacheableDbpfFile collPackage = packageCache.OpenForUpdate(collectionFilePath))
            {
                FullReindexRows(collPackage);

                for (int index = fromIndex; index <= toIndex; ++index)
                {
                    DataGridViewRow row = gridCollItems.Rows[index];
                    DataGridViewCell sortCell = row.Cells["colSort"];
                    int sortValue = (int)sortCell.Value;

                    if (index != sortValue)
                    {
                        DBPFKey binxKey = row.Cells["colBinxKey"].Value as DBPFKey;
                        Binx binx = (Binx)collPackage.GetResourceByKey(binxKey);

                        binx.GetOrAddItem("sortindex", MetaData.DataTypes.dtInteger).IntegerValue = index;
                        _isDirty = true;

                        collPackage.Commit(binx);
                    }
                }

                ReloadCollItems(collPackage);

                collPackage.Close();
            }

            UpdateSaveState();
        }

        private void ReindexRows(int fromIndex, int toIndex, int sortValue)
        {
            using (CacheableDbpfFile collPackage = packageCache.OpenForUpdate(collectionFilePath))
            {
                FullReindexRows(collPackage);

                for (int index = fromIndex; index <= toIndex; ++index)
                {
                    DataGridViewRow row = gridCollItems.Rows[index];
                    DataGridViewCell sortCell = row.Cells["colSort"];

                    DBPFKey binxKey = row.Cells["colBinxKey"].Value as DBPFKey;
                    Binx binx = (Binx)collPackage.GetResourceByKey(binxKey);

                    binx.GetOrAddItem("sortindex", MetaData.DataTypes.dtInteger).IntegerValue = sortValue++;
                    _isDirty = true;

                    collPackage.Commit(binx);
                }

                ReloadCollItems(collPackage);

                collPackage.Close();
            }

            UpdateSaveState();
        }
        #endregion

        #region Copy To Clipboard
        private void OnIconContext_ClipboardCopyTo(object sender, EventArgs e)
        {
            logger.Debug("Clipboard: Placing icon");
            Clipboard.SetImage(pictCollIcon.BackgroundImage);
        }

        private void OnCollItemsContext_ClipboardCopyTo(object sender, EventArgs e)
        {
            ClipboardCollListItems collListItems = new ClipboardCollListItems(collectionKey);

            foreach (DataGridViewRow row in gridCollItems.SelectedRows)
            {
                DBPFKey key = (DBPFKey)row.Cells["colItemKey"].Value;

                if (key != null)
                {
                    collListItems.AddItem(row.Index, key);
                }
            }

            logger.Debug("Clipboard: Placing items");
            collListItems.PlaceOnClipboard(false);
        }
        #endregion

        #region Paste From Clipboard
        private void OnCollItemsContext_ClipboardPasteBefore(object sender, EventArgs e)
        {
            DBPFKey inViewItemKey = (gridCollItems.Rows[mouseRowIndex].Cells["colItemKey"].Value as DBPFKey);

            if (ClipboardContainsPackageFiles())
            {
                AddItems(ClipboardHelper.FileList, 0);

                BringIntoView(inViewItemKey);
            }
            else if (ClipboardContainsCollItems())
            {
                AddItems(ClipboardHelper.CollListItems, 0);

                BringIntoView(inViewItemKey);
            }
        }

        private void OnCollItemsContext_ClipboardPasteAfter(object sender, EventArgs e)
        {
            DBPFKey inViewItemKey = (gridCollItems.Rows[mouseRowIndex].Cells["colItemKey"].Value as DBPFKey);

            if (ClipboardContainsPackageFiles())
            {
                AddItems(ClipboardHelper.FileList, 1);

                BringIntoView(inViewItemKey);
            }
            else if (ClipboardContainsCollItems())
            {
                AddItems(ClipboardHelper.CollListItems, 1);

                BringIntoView(inViewItemKey);
            }
        }
        #endregion

        #region Drag And Drop (Icons)
        private void OnDragEnter_Icon(object sender, DragEventArgs e)
        {
            Regex reImageName = new Regex(@"\.(png|jpg|jpeg|bmp|gif|tif|tiff)$");

            DataObject data = e.Data as DataObject;

            if (data.ContainsFileDropList())
            {
                string[] rawFiles = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (rawFiles != null && rawFiles.Length == 1)
                {
                    if (!reImageName.Match(Path.GetFileName(rawFiles[0])).Success)
                    {
                        return;
                    }

                    using (Image img = Image.FromFile(rawFiles[0]))
                    {
                        if (img.Width > Properties.Settings.Default.MaxIconWidth || img.Height > Properties.Settings.Default.MaxIconHeight)
                        {
                            return;
                        }
                    }

                    e.Effect = DragDropEffects.Copy;
                }
            }
        }

        private void OnDragDrop_Icon(object sender, DragEventArgs e)
        {
            DataObject data = e.Data as DataObject;

            if (data.ContainsFileDropList())
            {
                string[] rawFiles = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (rawFiles != null && rawFiles.Length == 1)
                {
                    UpdateIcon(rawFiles[0]);
                }
            }
        }
        #endregion

        #region Drag Drop (Collection Items)
        // TODO - Collection Manager - why do we use both MouseDown and CellMouseDown events?
        private void OnMouseDown_CollItemsGrid(object sender, MouseEventArgs e)
        {
            // TODO - Collection Manager - items drag-and-drop
            return;

            if (e.Button == MouseButtons.Left)
            {
                DropCollListItems dropListItems = new DropCollListItems(collectionKey);

                foreach (DataGridViewRow selectedRow in gridCollItems.SelectedRows)
                {
                    DBPFKey itemKey = selectedRow.Cells["colItemKey"].Value as DBPFKey;

                    dropListItems.AddItem(selectedRow.Index, itemKey);
                }

                DoDragDrop(dropListItems.GetDragData(), DragDropEffects.Copy);
            }
        }

        private void OnDragEnter_GridCollItems(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DragDropHelper.DragItemListLabel, false))
            {
                DropCollListItems dropListItems = new DropCollListItems(e.Data);

                DBPFKey collKey = dropListItems.CollKey;

                if (collKey.GroupID == DBPFData.GROUP_NULL && collKey.InstanceID == DBPFData.INSTANCE_NULL)
                {
                    Sim2ToolsAppCodes appCode = (Sim2ToolsAppCodes)collKey.ResourceID.AsUInt();

                    if (appCode == Sim2ToolsAppCodes.BSOKEditor || appCode == Sim2ToolsAppCodes.OutfitOrganiser)
                    {
                        if (IsObjectCollection)
                        {
                            logger.Debug($"Can't drag items from {appCode} into an object collection");
                            e.Effect = DragDropEffects.None;
                        }
                        else
                        {
                            logger.Debug($"Dragging items from {appCode}");
                            e.Effect = DragDropEffects.Copy;
                        }
                    }
                    else if (appCode == Sim2ToolsAppCodes.ObjectRelocator)
                    {
                        if (IsClothingCollection)
                        {
                            logger.Debug($"Can't drag items from {appCode} into a clothing collection");
                            e.Effect = DragDropEffects.None;
                        }
                        else
                        {
                            logger.Debug($"Dragging items from {appCode}");
                            e.Effect = DragDropEffects.Copy;
                        }
                    }
                    else
                    {
                        logger.Debug($"Dragging items from {appCode}");
                        e.Effect = DragDropEffects.Copy;
                    }
                }
                else
                {
                    logger.Debug($"Dragging items from {collKey}");

                    if (collectionKey.Equals(dropListItems.CollKey))
                    {
                        // Trying to drag-and-drop WITHIN the same collection, this is not allowed (as the drop is a copy operation that would duplicate entries)
                        e.Effect = DragDropEffects.None;
                    }
                    else
                    {
                        e.Effect = DragDropEffects.Copy;
                    }
                }

                return;
            }
            else
            {
                DataObject data = e.Data as DataObject;

                if (data.ContainsFileDropList())
                {
                    logger.Debug($"DragDrop: Dragging .package files");
                    string[] fileList = (string[])e.Data.GetData(DataFormats.FileDrop);

                    if (fileList != null)
                    {
                        bool allOk = true;

                        foreach (string filePath in fileList)
                        {
                            if (!Path.GetFileName(filePath).EndsWith(".package"))
                            {
                                allOk = false;
                                break;
                            }
                        }

                        e.Effect = allOk ? DragDropEffects.Copy : DragDropEffects.None;
                    }
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            }
        }

        private void OnDragDrop_GridCollItems(object sender, DragEventArgs e)
        {
            IDataObject dataObject = e.Data;

            Point p = gridCollItems.PointToClient(new Point(e.X, e.Y));
            mouseRowIndex = gridCollItems.HitTest(p.X, p.Y).RowIndex;
            logger.Debug($"Drop row is {mouseRowIndex}");

            DBPFKey inViewItemKey = (gridCollItems.Rows[mouseRowIndex].Cells["colItemKey"].Value as DBPFKey);

            if (DragDropHelper.ContainsDragItemList(dataObject))
            {
                DropCollListItems dropListItems = new DropCollListItems(dataObject);

                AddItems(dropListItems, GetMainForm().GetMouseDropOffset(false));

                BringIntoView(inViewItemKey);
            }
            else
            {
                DataObject data = e.Data as DataObject;

                if (data.ContainsFileDropList())
                {
                    logger.Debug($"DragDrop: Dropping .package files");
                    string[] fileList = (string[])e.Data.GetData(DataFormats.FileDrop);

                    if (fileList != null)
                    {
                        AddItems(fileList, GetMainForm().GetMouseDropOffset(false));

                        BringIntoView(inViewItemKey);
                    }
                }
            }
        }

        private void AddItems(AbstractCollListItems collListItems, int targetOffset)
        {
            bool added = false;

            DBPFKey collKey = collListItems.CollKey;

            if (collKey.GroupID == DBPFData.GROUP_NULL && collKey.InstanceID == DBPFData.INSTANCE_NULL)
            {
                Sim2ToolsAppCodes appCode = (Sim2ToolsAppCodes)collKey.ResourceID.AsUInt();

                if (appCode == Sim2ToolsAppCodes.BSOKEditor || appCode == Sim2ToolsAppCodes.OutfitOrganiser)
                {
                    if (IsObjectCollection)
                    {
                        logger.Debug($"Can't add items from {appCode} into an object collection");
                        return;
                    }
                    else
                    {
                        logger.Debug($"Adding items from {appCode}");
                    }
                }
                else if (appCode == Sim2ToolsAppCodes.ObjectRelocator)
                {
                    if (IsClothingCollection)
                    {
                        logger.Debug($"Can't add items from {appCode} into a clothing collection");
                        return;
                    }
                    else
                    {
                        logger.Debug($"Adding items from {appCode}");
                    }
                }
                else
                {
                    logger.Debug($"Adding items from {appCode}");
                }
            }
            else
            {
                logger.Debug($"Adding items from {collKey}");
            }

            List<DBPFKey> items = collListItems.CollItems;

            using (CacheableDbpfFile collPackage = packageCache.OpenForUpdate(collectionFilePath))
            {
                FullReindexRows(collPackage);

                uint instanceID = GetNextIdrInstance(collPackage);

                int startRowIndex = (mouseRowIndex == -1) ? 0 : (mouseRowIndex + targetOffset);
                // TODO - Collection Manager - already fixed a bug in this line
                // int sortindex = startRowIndex + 1;
                int sortindex = startRowIndex;

                if (!IsClothingCollection && !IsObjectCollection)
                {
                    // Define the collection by the first key
                    if (items[0].TypeID == Coll.COLLITEM_GZPS)
                    {
                        comboCollType.SelectedIndex = 3;
                    }
                    else
                    {
                        comboCollType.SelectedIndex = 0;
                    }
                }

                foreach (DBPFKey item in items)
                {
                    if ((IsClothingCollection && item.TypeID == Coll.COLLITEM_GZPS) ||
                        (IsObjectCollection && (item.TypeID == Coll.COLLITEM_OBJD || item.TypeID == Coll.COLLITEM_XOBJ)))
                    {
                        if (AddCollItem(collPackage, collectionKey, instanceID, sortindex, item))
                        {
                            added = true;
                        }

                        ++sortindex;
                        ++instanceID;
                    }
                }

                if (added)
                {
                    // TODO - Collection Manager - already fixed a bug in this line with "from index"
                    // ReindexRows(startRowIndex + targetOffset, gridCollItems.Rows.Count - 1, sortindex);
                    ReindexRows(startRowIndex, gridCollItems.Rows.Count - 1, sortindex);
                }

                collPackage.Close();
            }

            if (added)
            {
                Reload();
            }
        }

        private void AddItems(StringCollection filelist, int targetOffset)
        {
            AddItems(filelist.Cast<string>().ToList(), targetOffset);
        }

        private void AddItems(string[] filelist, int targetOffset)
        {
            AddItems(filelist.Cast<string>().ToList(), targetOffset);
        }

        private void AddItems(List<string> filelist, int targetOffset)
        {
            bool added = false;

            using (CacheableDbpfFile collPackage = packageCache.OpenForUpdate(collectionFilePath))
            {
                FullReindexRows(collPackage);

                uint instanceID = GetNextIdrInstance(collPackage);

                int startRowIndex = (mouseRowIndex == -1) ? 0 : (mouseRowIndex + targetOffset);
                logger.Debug($"Start row index = {startRowIndex}");
                int sortindex = startRowIndex;

                foreach (string filepath in filelist)
                {
                    using (DBPFFile package = new DBPFFile(filepath))
                    {
                        if (IsClothingCollection)
                        {
                            added = AddClothingItems(package, collPackage, collectionKey, ref instanceID, ref sortindex);
                        }
                        else if (IsObjectCollection)
                        {
                            added = AddObjectItems(package, collPackage, collectionKey, ref instanceID, ref sortindex);
                        }
                        else
                        {
                            if (AddObjectItems(package, collPackage, collectionKey, ref instanceID, ref sortindex))
                            {
                                comboCollType.SelectedIndex = 0;
                                added = true;
                            }
                            else if (AddClothingItems(package, collPackage, collectionKey, ref instanceID, ref sortindex))
                            {
                                comboCollType.SelectedIndex = 3;
                                added = true;
                            }
                        }

                        package.Close();
                    }
                }

                if (added)
                {
                    // TODO - Collection Manager - already fixed a bug in this line with "from index"
                    // ReindexRows(startRowIndex + targetOffset, gridCollItems.Rows.Count - 1, sortindex);
                    ReindexRows(startRowIndex, gridCollItems.Rows.Count - 1, sortindex);
                }

                collPackage.Close();
            }

            if (added)
            {
                Reload();
            }
        }

        private bool AddClothingItems(DBPFFile package, CacheableDbpfFile collPackage, DBPFKey collKey, ref uint instanceID, ref int sortindex)
        {
            bool added = false;

            foreach (DBPFEntry entry in package.GetEntriesByType(Gzps.TYPE))
            {
                if (AddCollItem(collPackage, collKey, instanceID, sortindex, entry))
                {
                    added = true;
                }

                ++sortindex;
                ++instanceID;
            }

            return added;
        }

        private bool AddObjectItems(DBPFFile package, CacheableDbpfFile collPackage, DBPFKey collKey, ref uint instanceID, ref int sortindex)
        {
            bool added = false;

            foreach (DBPFEntry entry in package.GetEntriesByType(Objd.TYPE))
            {
                Objd objd = (Objd)package.GetResourceByEntry(entry);

                if (AddCollItem(collPackage, collKey, instanceID, sortindex,
                                 new DBPFKey(Coll.COLLITEM_OBJD, (TypeGroupID)0x00000000, (TypeInstanceID)objd.Guid.AsUInt(), DBPFData.RESOURCE_NULL)))
                {
                    added = true;
                }

                ++sortindex;
                ++instanceID;
            }

            foreach (DBPFEntry entry in package.GetEntriesByType(Xobj.TYPE))
            {
                Xobj xobj = (Xobj)package.GetResourceByEntry(entry);

                TypeGUID guid = (TypeGUID)xobj.GetItem("guid").UIntegerValue;
                string type = xobj.GetItem("type").StringValue;

                if (type.Equals("wall"))
                {
                    if (AddCollItem(collPackage, collKey, instanceID, sortindex,
                                     new DBPFKey(Coll.COLLITEM_XOBJ, (TypeGroupID)0x00000002, (TypeInstanceID)Hashes.CollectionHash(guid), DBPFData.RESOURCE_NULL)))
                    {
                        added = true;
                    }

                    ++sortindex;
                    ++instanceID;
                }
                else if (type.Equals("floor"))
                {
                    if (AddCollItem(collPackage, collKey, instanceID, sortindex,
                                     new DBPFKey(Coll.COLLITEM_XOBJ, (TypeGroupID)0x00000001, (TypeInstanceID)Hashes.CollectionHash(guid), DBPFData.RESOURCE_NULL)))
                    {
                        added = true;
                    }

                    ++sortindex;
                    ++instanceID;
                }
            }

            return added;
        }

        private bool AddCollItem(CacheableDbpfFile collPackage, DBPFKey collKey, uint instanceID, int sortindex, DBPFKey itemKey)
        {
            if (dataCollItems.Contains(itemKey)) return false;

            logger.Debug($"Adding Item: {itemKey}");

            DBPFKey binxKey = new DBPFKey(Binx.TYPE, collKey.GroupID, (TypeInstanceID)instanceID, DBPFData.RESOURCE_NULL);
            Binx binx = new Binx(binxKey);
            binx.AddItem(new CpfItem("iconidx", (uint)0));
            binx.AddItem(new CpfItem("stringsetidx", (uint)0));
            binx.AddItem(new CpfItem("stringindex", (uint)0));
            binx.AddItem(new CpfItem("binidx", (uint)1));
            binx.AddItem(new CpfItem("objectidx", (uint)2));
            binx.AddItem(new CpfItem("creatorid", "00000000-0000-0000-0000-000000000000"));
            binx.AddItem(new CpfItem("sortindex", sortindex));

            Idr idrForBinx = new Idr(new DBPFKey(Idr.TYPE, binxKey), 3);
            idrForBinx.AppendItem(new DBPFKey((TypeTypeID)0x00000000, (TypeGroupID)0x00000000, (TypeInstanceID)0x00000000, (TypeResourceID)0x00000000));
            idrForBinx.AppendItem(collKey);
            idrForBinx.AppendItem(itemKey);

            _isDirty = true;

            collPackage.Commit(binx, true);
            collPackage.Commit(idrForBinx, true);

            return true;
        }

        private uint GetNextIdrInstance(CacheableDbpfFile collPackage)
        {
            uint instanceID = 0;

            foreach (DBPFEntry entry in collPackage.GetEntriesByType(Binx.TYPE))
            {
                if (entry.InstanceID.AsUInt() > instanceID) instanceID = entry.InstanceID.AsUInt();
            }

            foreach (DBPFEntry entry in collPackage.GetEntriesByType(Idr.TYPE))
            {
                if (entry.InstanceID.AsUInt() > instanceID) instanceID = entry.InstanceID.AsUInt();
            }

            return (instanceID + 1);
        }
        #endregion

        #region Save Button
        private void OnSaveClicked(object sender, EventArgs e)
        {
            if (Form.ModifierKeys == Keys.Control)
            {
                GetMainForm().SaveAsCollection(this);
            }
            else if (Form.ModifierKeys == Keys.Shift)
            {
                GetMainForm().SaveAllCollection();
            }
            else
            {
                // This is a bugger as we need the state of the main form's Auto-Backup menu item!
                GetMainForm().SaveCollection(this);
            }
        }
        #endregion

        private void BringIntoView(DBPFKey itemKey)
        {
            if (itemKey != null)
            {
                foreach (DataGridViewRow row in gridCollItems.Rows)
                {
                    if (itemKey.Equals(row.Cells["colItemKey"].Value as DBPFKey))
                    {
                        EnsureVisibleRow(row.Index);
                        break;
                    }
                }
            }
        }

        private void EnsureVisibleRow(int rowToShow)
        {
            if (rowToShow >= 0 && rowToShow < gridCollItems.RowCount)
            {
                int countVisible = gridCollItems.DisplayedRowCount(false);
                int firstVisible = gridCollItems.FirstDisplayedScrollingRowIndex;

                if (rowToShow < firstVisible || rowToShow >= firstVisible + countVisible)
                {
                    gridCollItems.FirstDisplayedScrollingRowIndex = Math.Max(0, rowToShow - (countVisible / 2));
                }
            }
        }
    }
}
