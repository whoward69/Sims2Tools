/*
 * Family Manager - a utility for manipulating family closets
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF;
using Sims2Tools.DBPF.SceneGraph.GZPS;
using Sims2Tools.DBPF.Utils;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace FamilyManager
{
    [System.ComponentModel.DesignerCategory("")]
    class FamilyManagerFamilyData : DataTable
    {
        public FamilyManagerFamilyData()
        {
            // Must match the order in the DataGridView control
            this.Columns.Add(new DataColumn("FirstName", typeof(string)));

            this.Columns.Add(new DataColumn("Gender", typeof(string)));
            this.Columns.Add(new DataColumn("GenderCode", typeof(string)));
            this.Columns.Add(new DataColumn("Age", typeof(string)));
            this.Columns.Add(new DataColumn("AgeCode", typeof(string)));
            this.Columns.Add(new DataColumn("DaysLeft", typeof(int)));

            this.Columns.Add(new DataColumn("Thumbnail", typeof(object)));
        }
    }

    [System.ComponentModel.DesignerCategory("")]
    class FamilyManagerClosetData : DataTable
    {
        public FamilyManagerClosetData()
        {
            // Must match the order in the DataGridView control
            this.Columns.Add(new DataColumn("Name", typeof(string)));

            this.Columns.Add(new DataColumn("Category", typeof(string)));
            this.Columns.Add(new DataColumn("Gender", typeof(string)));
            this.Columns.Add(new DataColumn("GenderCode", typeof(string)));
            this.Columns.Add(new DataColumn("Age", typeof(string)));
            this.Columns.Add(new DataColumn("AgeCode", typeof(string)));

            this.Columns.Add(new DataColumn("Data", typeof(object)));
            this.Columns.Add(new DataColumn("ThumbKey", typeof(object)));
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

    public class CharacterData
    {
        public TypeGUID guid;
        public string packagePath;

        public string givenName;  // CTSS[0]
        public string familyName; // CTSS[2]

        public Image thumbnail = null;
    }

    public class CasClothingData
    {
        public readonly DBPFKey resKey;
        public readonly string resPackagePath;

        public readonly string resName;
        public string resDesc = "";
        public readonly uint resCategory;
        public readonly uint resAge;
        public readonly uint resGender;
        public readonly string resHairtone;

        public readonly DBPFKey thumbKey;

        public CasClothingData(Gzps gzps, string packagePath)
        {
            this.resKey = new DBPFKey(gzps);
            this.resPackagePath = packagePath;

            resName = gzps.Name;

            resCategory = gzps.Category;
            resAge = gzps.Age;
            resGender = gzps.Gender;

            resHairtone = gzps.Hairtone;

            thumbKey = Hashes.CasThumbnailHash(resKey, resGender, resAge, "");
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

            thumbKey = row.Cells[$"{colNamePrefix}ThumbKey"].Value;
        }
    }

    public class ClosetDragData
    {
        public object sender;
        public List<ClosetData> items = new List<ClosetData>();
    }
}
