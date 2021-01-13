using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.SceneGraph.RCOL;
using System;
using System.Collections;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks
{
    public class CMaterialDefinition : AbstractRcolBlock, IScenegraphBlock
    {
        public static uint TYPE = 0x49596978;
        public static String NAME = "cMaterialDefinition";

        #region Attributes


        string fldsc;
        public string FileDescription
        {
            get { return fldsc; }
            set { fldsc = value; }
        }

        string mattype;
        public string MatterialType
        {
            get { return mattype; }
            set { mattype = value; }
        }

        MaterialDefinitionProperty[] properties;
        public MaterialDefinitionProperty[] Properties
        {
            get { return properties; }
            set { properties = value; }
        }

        string[] listing;
        public string[] Listing
        {
            get { return listing; }
            set { listing = value; }
        }
        #endregion



        /// <summary>
        /// Constructor
        /// </summary>
        public CMaterialDefinition(Rcol parent) : base(parent)
        {
            properties = new MaterialDefinitionProperty[0];
            listing = new String[0];
            sgres = new SGResource(null);
            BlockID = 0x49596978;
            fldsc = "";
            mattype = "";
        }

        /// <summary>
        /// Returns the Property Item 
        /// </summary>
        /// <param name="name">The Property name</param>
        /// <returns>The Item or an Item with no Name if the property dies not exits</returns>
        public MaterialDefinitionProperty FindProperty(string name)
        {
            name = name.Trim().ToLower();
            foreach (MaterialDefinitionProperty mdp in properties)
            {
                if (mdp.Name.Trim().ToLower() == name) return mdp;
            }

            return new MaterialDefinitionProperty();
        }

        public MaterialDefinitionProperty GetProperty(string name)
        {
            return FindProperty(name);
            /*name = name.ToLower();
			foreach (MaterialDefinitionProperty mdp in properties)
			{
				if (name==mdp.Name.ToLower()) return mdp;
			}
			return new MaterialDefinitionProperty();*/
        }

        #region IRcolBlock Member

        /// <summary>
        /// Unserializes a BinaryStream into the Attributes of this Instance
        /// </summary>
        /// <param name="reader">The Stream that contains the FileData</param>
        public override void Unserialize(IoBuffer reader)
        {
            version = reader.ReadUInt32();
            /*byte len = reader.ReadByte();
			fldsc = Helper.ToString(reader.ReadBytes(len));*/
            fldsc = reader.ReadString();
            uint myid = reader.ReadUInt32();
            sgres.Unserialize(reader);
            sgres.BlockID = myid;

            /*len = reader.ReadByte();
			fldsc = Helper.ToString(reader.ReadBytes(len));*/
            fldsc = reader.ReadString();
            /*len = reader.ReadByte();
			mattype = Helper.ToString(reader.ReadBytes(len));*/
            mattype = reader.ReadString();

            properties = new MaterialDefinitionProperty[reader.ReadUInt32()];
            for (int i = 0; i < properties.Length; i++)
            {
                properties[i] = new MaterialDefinitionProperty();
                properties[i].Unserialize(reader);
            }

            if (version == 8)
            {
                listing = new String[0];
            }
            else
            {
                listing = new String[reader.ReadUInt32()];
                for (int i = 0; i < listing.Length; i++)
                {
                    /*len = reader.ReadByte();
					listing[i] = Helper.ToString(reader.ReadBytes(len));*/
                    listing[i] = reader.ReadString();
                }
            }
        }

        #endregion

        #region IScenegraphBlock Member

        public void ReferencedItems(Hashtable refmap, uint parentgroup)
        {
            ArrayList list = new ArrayList();
            foreach (string name in Listing)
            {
                list.Add(ScenegraphHelper.BuildPfd(name + "_txtr", ScenegraphHelper.TXTR, parentgroup));
            }
            refmap["TXTR"] = list;

            string refname = this.GetProperty("stdMatBaseTextureName").Value;
            if (refname.Trim() != "")
            {
                list = new ArrayList();
                list.Add(ScenegraphHelper.BuildPfd(refname + "_txtr", ScenegraphHelper.TXTR, parentgroup));
                refmap["stdMatBaseTextureName"] = list;
            }

            refname = this.GetProperty("stdMatNormalMapTextureName").Value;
            if (refname.Trim() != "")
            {
                list = new ArrayList();
                list.Add(ScenegraphHelper.BuildPfd(refname + "_txtr", ScenegraphHelper.TXTR, parentgroup));
                refmap["stdMatNormalMapTextureName"] = list;
            }

            refname = this.GetProperty("stdMatEnvCubeTextureName").Value;
            if (refname.Trim() != "")
            {
                list = new ArrayList();
                list.Add(ScenegraphHelper.BuildPfd(refname + "_txtr", ScenegraphHelper.TXTR, parentgroup));
                refmap["stdMatEnvCubeTextureName"] = list;
            }


            //for characters
            int count = 0;
            try
            {
                string s = this.GetProperty("numTexturesToComposite").Value;
                if (s != "") count = Convert.ToInt32(this.GetProperty("numTexturesToComposite").Value);
            }
            catch { }
            list = new ArrayList();
            refmap["baseTexture"] = list;
            for (int i = 0; i < count; i++)
            {
                refname = this.GetProperty("baseTexture" + i.ToString()).Value.Trim();
                if (refname != "")
                {
                    if (!refname.EndsWith("_txtr")) refname += "_txtr";
                    list.Add(ScenegraphHelper.BuildPfd(refname, ScenegraphHelper.TXTR, parentgroup));
                }
            }
        }

        #endregion

        /// <summary>
        /// Sort the Properties in Alphabetic Order
        /// </summary>
        public void Sort()
        {
            for (int i = 0; i < this.properties.Length - 1; i++)
                for (int j = i + 1; j < this.properties.Length; j++)
                {
                    if (properties[i].Name.CompareTo(properties[j].Name) > 0)
                    {
                        MaterialDefinitionProperty dum = properties[i];
                        properties[i] = properties[j];
                        properties[j] = dum;
                    }
                }
        }

        public override void Dispose()
        {
        }
    }

    public class MaterialDefinitionProperty
    {
        #region Attributes
        string name;
        string val;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Value
        {
            get { return val; }
            set { val = value; }
        }
        #endregion

        public MaterialDefinitionProperty()
        {
            name = "";
            val = "";
        }

        /// <summary>
        /// Unserializes a BinaryStream into the Attributes of this Instance
        /// </summary>
        /// <param name="reader">The Stream that contains the FileData</param>
        public void Unserialize(IoBuffer reader)
        {

            name = reader.ReadString();
            val = reader.ReadString();
        }
    }
}
