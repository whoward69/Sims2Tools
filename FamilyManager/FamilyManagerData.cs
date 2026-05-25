/*
 * Family Manager - a utility for manipulating family closets
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.Images.IMG;
using Sims2Tools.DBPF.Images.JPG;
using Sims2Tools.DBPF.Neighbourhood.FAMI;
using Sims2Tools.DBPF.Neighbourhood.LTXT;
using Sims2Tools.DBPF.STR;
using Sims2Tools.DbpfCache;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace FamilyManager
{
    [System.ComponentModel.DesignerCategory("")]
    class FamilyGridData : DataTable
    {
        public FamilyGridData()
        {
            // Must match the order in the DataGridView control
            this.Columns.Add(new DataColumn("FirstName", typeof(string)));

            this.Columns.Add(new DataColumn("Gender", typeof(string)));
            this.Columns.Add(new DataColumn("GenderCode", typeof(string)));
            this.Columns.Add(new DataColumn("Age", typeof(string)));
            this.Columns.Add(new DataColumn("AgeCode", typeof(string)));
            this.Columns.Add(new DataColumn("DaysLeft", typeof(int)));

            this.Columns.Add(new DataColumn("GenderHex", typeof(uint)));
            this.Columns.Add(new DataColumn("AgeHex", typeof(uint)));

            this.Columns.Add(new DataColumn("Thumbnail", typeof(object)));
        }
    }

    [System.ComponentModel.DesignerCategory("")]
    class ClothesGridData : DataTable
    {
        public ClothesGridData()
        {
            // Must match the order in the DataGridView control
            this.Columns.Add(new DataColumn("Visible", typeof(string)));

            this.Columns.Add(new DataColumn("Name", typeof(string)));

            this.Columns.Add(new DataColumn("Category", typeof(string)));
            this.Columns.Add(new DataColumn("Gender", typeof(string)));
            this.Columns.Add(new DataColumn("GenderCode", typeof(string)));
            this.Columns.Add(new DataColumn("Age", typeof(string)));
            this.Columns.Add(new DataColumn("AgeCode", typeof(string)));

            this.Columns.Add(new DataColumn("GenderHex", typeof(uint)));
            this.Columns.Add(new DataColumn("AgeHex", typeof(uint)));

            this.Columns.Add(new DataColumn("Data", typeof(object)));
            this.Columns.Add(new DataColumn("ThumbKey", typeof(object)));

            this.DefaultView.RowFilter = "Visible = 'Yes'";
        }

        public void Append(DataRow row)
        {
            this.Rows.Add(row);
        }
    }

    public class TopTreeNode : TreeNode
    {
        public TopTreeNode(string name) : base(name)
        {
        }
    }

    public class HoodTreeNode : TreeNode
    {
        private readonly string packagePath;
        private readonly string hoodSubFolder;

        public string PackagePath => packagePath;
        public string HoodSubFolder => hoodSubFolder;

        public HoodTreeNode(string packagePath, string hoodSubFolder, string hoodName) : base(hoodName)
        {
            this.packagePath = packagePath;
            this.hoodSubFolder = hoodSubFolder;
        }
    }

    public class FamilyTreeNode : TreeNode
    {
        private readonly TypeInstanceID familyId;

        public TypeInstanceID FamilyId => familyId;

        public FamilyTreeNode(TypeInstanceID familyId, string familyName) : base(familyName)
        {
            this.familyId = familyId;
        }
    }

    public class FamilyData
    {
        private readonly DbpfFileCache packageCache;

        private readonly string famiPackagePath;
        private readonly Fami fami;
        private readonly Str famiStr;

        private readonly string ltxtPackagePath;
        private readonly Ltxt ltxt;
        private readonly Str ltxtStr;

        private readonly string lotdPackagePath;
        private readonly Lotd lotd;

        private string familyName = null;
        private string familyWriteUp = null;
        private readonly HashSet<uint> familyMembers = new HashSet<uint>();
        private int familyMoney = 0;
        private readonly Image familyImage = null;

        private int businessMoney = 0;

        private string lotAddress = null;
        private string lotDescription = null; // Only seems to be used for community lot descriptions
        private readonly Image lotImage = null;

        public string FamilyName
        {
            get => familyName;
            set
            {
                if (!familyName.Equals(value))
                {
                    FamilyManagerForm.SetString(famiStr, 0, value);

                    using (CacheableDbpfFile package = packageCache.OpenForUpdate(famiPackagePath))
                    {
                        package.Commit(famiStr);

                        package.Close();
                    }

                    familyName = value;
                }
            }
        }

        public string FamilyWriteUp
        {
            get => familyWriteUp;
            set
            {
                if (familyWriteUp == null || !familyWriteUp.Equals(value))
                {
                    FamilyManagerForm.SetString(famiStr, 1, value);

                    using (CacheableDbpfFile package = packageCache.OpenForUpdate(famiPackagePath))
                    {
                        package.Commit(famiStr);

                        package.Close();
                    }

                    familyWriteUp = value;
                }
            }
        }

        public string FamilyMoney
        {
            get => familyMoney.ToString();
            set
            {
                if (!FamilyMoney.Equals(value))
                {
                    if (Int32.TryParse(value, out int cash) && cash != familyMoney)
                    {
                        fami.Money = cash;

                        using (CacheableDbpfFile package = packageCache.OpenForUpdate(famiPackagePath))
                        {
                            package.Commit(fami);

                            package.Close();
                        }

                        familyMoney = cash;
                    }
                }
            }
        }

        public string BusinessMoney
        {
            get => businessMoney.ToString();
            set
            {
                if (!BusinessMoney.Equals(value))
                {
                    if (Int32.TryParse(value, out int cash) && cash != businessMoney)
                    {
                        fami.BusinessMoney = cash;

                        using (CacheableDbpfFile package = packageCache.OpenForUpdate(famiPackagePath))
                        {
                            package.Commit(fami);

                            package.Close();
                        }

                        businessMoney = cash;
                    }
                }
            }
        }

        public Image FamilyImage => familyImage;

        public string LotAddress
        {
            get => lotAddress;
            set
            {
                if (lotAddress == null || !lotAddress.Equals(value))
                {
                    if (FamilyManagerForm.IsDefLang)
                    {
                        ltxt.LotName = value;

                        lotd.LotName = value;

                        using (CacheableDbpfFile package = packageCache.OpenForUpdate(lotdPackagePath))
                        {
                            package.Commit(lotd);

                            package.Close();
                        }
                    }

                    FamilyManagerForm.SetString(ltxtStr, 0, value);

                    using (CacheableDbpfFile package = packageCache.OpenForUpdate(ltxtPackagePath))
                    {
                        package.Commit(ltxt);
                        package.Commit(ltxtStr);

                        package.Close();
                    }

                    lotAddress = value;
                }
            }
        }

        public string LotDescription
        {
            get => lotDescription;
            set
            {
                if (lotDescription == null || !lotDescription.Equals(value))
                {
                    if (FamilyManagerForm.IsDefLang)
                    {
                        ltxt.LotDesc = value;

                        lotd.LotDesc = value;

                        using (CacheableDbpfFile package = packageCache.OpenForUpdate(lotdPackagePath))
                        {
                            package.Commit(lotd);

                            package.Close();
                        }
                    }

                    FamilyManagerForm.SetString(ltxtStr, 1, value);

                    using (CacheableDbpfFile package = packageCache.OpenForUpdate(ltxtPackagePath))
                    {
                        package.Commit(ltxtStr);

                        package.Close();
                    }

                    lotDescription = value;
                }
            }
        }

        public Image LotImage => lotImage;

        public FamilyData(DbpfFileCache packageCache, HoodTreeNode hoodNode, FamilyTreeNode familyNode)
        {
            this.packageCache = packageCache;
            famiPackagePath = hoodNode.PackagePath;
            ltxtPackagePath = hoodNode.PackagePath;

            using (CacheableDbpfFile familyPackage = packageCache.OpenForReadOnly(famiPackagePath))
            {
                CacheableDbpfFile subhoodPackage = familyPackage;

                fami = (Fami)familyPackage.GetResourceByKey(new DBPFKey(Fami.TYPE, DBPFData.GROUP_LOCAL, familyNode.FamilyId, DBPFData.RESOURCE_NULL));

                if (fami != null)
                {
                    familyName = familyNode.Text;

                    famiStr = (Str)familyPackage.GetResourceByKey(new DBPFKey(Str.TYPE, fami));

                    if (famiStr != null)
                    {
                        familyName = FamilyManagerForm.GetString(famiStr, 0);
                        familyWriteUp = FamilyManagerForm.GetString(famiStr, 1);
                    }

                    DBPFKey ltxtKey = new DBPFKey(Ltxt.TYPE, DBPFData.GROUP_LOCAL, fami.LotInstance, DBPFData.RESOURCE_NULL);
                    ltxt = (Ltxt)familyPackage.GetResourceByKey(ltxtKey);

                    if (ltxt == null)
                    {
                        string mainHoodPackage = $"{Sims2ToolsLib.Sims2HomePath}\\Neighborhoods\\{hoodNode.HoodSubFolder}_Neighborhood.package";

                        foreach (string subhood in Directory.GetFiles($"{Sims2ToolsLib.Sims2HomePath}\\Neighborhoods\\{hoodNode.HoodSubFolder}", $"{hoodNode.HoodSubFolder}_*.package", SearchOption.TopDirectoryOnly))
                        {
                            if (!mainHoodPackage.Equals(subhood))
                            {
                                CacheableDbpfFile package = packageCache.OpenForReadOnly(subhood); // Don't use using() here, as we need to keep the .package file open for the STR# below

                                ltxt = (Ltxt)package.GetResourceByKey(ltxtKey);

                                if (ltxt != null)
                                {
                                    subhoodPackage = package;
                                    ltxtPackagePath = subhood;

                                    // Leave the package open, we'll close it later
                                    break;
                                }

                                package.Close();
                            }
                        }
                    }

                    if (ltxt != null)
                    {
                        ltxtStr = (Str)subhoodPackage.GetResourceByKey(new DBPFKey(Str.TYPE, DBPFData.GROUP_LOCAL, (TypeInstanceID)(ltxt.InstanceID.AsUInt() + 0x8000), DBPFData.RESOURCE_NULL));
                        lotAddress = FamilyManagerForm.GetString(ltxtStr, 0);
                        lotDescription = FamilyManagerForm.GetString(ltxtStr, 1);

                        lotdPackagePath = $"{Sims2ToolsLib.Sims2HomePath}\\Neighborhoods\\{hoodNode.HoodSubFolder}\\Lots\\{hoodNode.HoodSubFolder}_Lot{ltxt.InstanceID.AsUInt()}.package";
                        using (CacheableDbpfFile lotPackage = packageCache.OpenForReadOnly(lotdPackagePath))
                        {
                            lotd = (Lotd)lotPackage.GetResourceByKey(new DBPFKey(Lotd.TYPE, DBPFData.GROUP_LOCAL, DBPFData.INSTANCE_NULL, DBPFData.RESOURCE_NULL));
                            Img img = (Img)lotPackage.GetResourceByKey(new DBPFKey(Img.TYPE, DBPFData.GROUP_LOCAL, (TypeInstanceID)0x35CA0002, DBPFData.RESOURCE_NULL));

                            if (img != null)
                            {
                                lotImage = img.Image;
                            }

                            lotPackage.Close();
                        }
                    }

                    familyMoney = fami.Money;
                    businessMoney = fami.BusinessMoney;

                    familyMembers = new HashSet<uint>(fami.Members);
                }

                if (subhoodPackage != familyPackage) subhoodPackage.Close();
                familyPackage.Close();
            }

            string thumbnailPath = $"{Sims2ToolsLib.Sims2HomePath}\\Neighborhoods\\{hoodNode.HoodSubFolder}\\Thumbnails\\{hoodNode.HoodSubFolder}_FamilyThumbnails.package";

            using (CacheableDbpfFile thumbnailPackage = packageCache.OpenForReadOnly(thumbnailPath))
            {
                Jpg jpg = (Jpg)thumbnailPackage.GetResourceByKey(new DBPFKey(Jpg.TYPE, DBPFData.GROUP_LOCAL, familyNode.FamilyId, DBPFData.RESOURCE_NULL));

                if (jpg != null)
                {
                    familyImage = jpg.Image;
                }

                thumbnailPackage.Close();
            }
        }

        public bool IsMember(uint member)
        {
            return familyMembers.Contains(member);
        }
    }


    public class ClosetData
    {
        public ClosetDbpfData dbpfData;

        public string name;

        public string category;
        public string gender;
        public string genderCode;
        public string age;
        public string ageCode;

        public uint genderHex;
        public uint ageHex;

        public object thumbKey;

        public ClosetData(string colNamePrefix, DataGridViewRow row)
        {
            dbpfData = row.Cells[$"{colNamePrefix}Data"].Value as ClosetDbpfData;

            name = row.Cells[$"{colNamePrefix}Name"].Value as string;

            category = row.Cells[$"{colNamePrefix}Category"].Value as string;
            gender = row.Cells[$"{colNamePrefix}Gender"].Value as string;
            genderCode = row.Cells[$"{colNamePrefix}GenderCode"].Value as string;
            age = row.Cells[$"{colNamePrefix}Age"].Value as string;
            ageCode = row.Cells[$"{colNamePrefix}AgeCode"].Value as string;

            genderHex = (uint)(row.Cells[$"{colNamePrefix}GenderHex"].Value);
            ageHex = (uint)(row.Cells[$"{colNamePrefix}AgeHex"].Value);

            thumbKey = row.Cells[$"{colNamePrefix}ThumbKey"].Value;
        }
    }

    public class ClosetTransferData
    {
        public object sender;
        public List<ClosetData> items = new List<ClosetData>();
    }
}
