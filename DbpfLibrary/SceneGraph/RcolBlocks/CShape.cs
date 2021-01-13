using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.SceneGraph.RCOL;
using Sims2Tools.DBPF.SceneGraph.RcolBlocks.SubBlocks;
using Sims2Tools.DBPF.Utils;
using System;
using System.Collections;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks
{
    public class ShapePart
    {
        string type;
        public string Subset
        {
            get { return type; }
            set { type = value; }
        }

        string desc;
        public string FileName
        {
            get { return desc; }
            set { desc = value; }
        }

        byte[] data;
        public byte[] Data
        {
            get { return data; }
            set
            {
                if (value.Length == 9)
                {
                    data = value;
                }
                else if (value.Length > 9)
                {
                    data = new byte[9];
                    for (int i = 0; i < 9; i++) data[i] = value[i];
                }
                else
                {
                    data = new byte[9];
                    for (int i = 0; i < value.Length; i++) data[i] = value[i];
                }
            }
        }

        public ShapePart()
        {
            data = new byte[9];
            type = "";
            desc = "";
        }
        /// <summary>
        /// Unserializes a BinaryStream into the Attributes of this Instance
        /// </summary>
        /// <param name="reader">The Stream that contains the FileData</param>
        public void Unserialize(IoBuffer reader)
        {
            type = reader.ReadString();
            desc = reader.ReadString();
            data = reader.ReadBytes(9);
        }

        public override string ToString()
        {
            string name = type + ": " + desc;
            return name;
        }
    }

    /// <summary>
    /// A Shape Item
    /// </summary>
    public class ShapeItem
    {
        readonly CShape parent;

        int unknown1;
        public int Unknown1
        {
            get { return unknown1; }
            set { unknown1 = value; }
        }

        byte unknown2;
        public byte Unknown2
        {
            get { return unknown2; }
            set { unknown2 = value; }
        }

        int unknown3;
        public int Unknown3
        {
            get { return unknown3; }
            set { unknown3 = value; }
        }

        byte unknown4;
        public byte Unknown4
        {
            get { return unknown4; }
            set { unknown4 = value; }
        }

        string filename;
        public string FileName
        {
            get { return filename; }
            set { filename = value; }
        }

        public ShapeItem(CShape parent)
        {
            this.parent = parent;
            filename = "";
        }


        /// <summary>
        /// Unserializes a BinaryStream into the Attributes of this Instance
        /// </summary>
        /// <param name="reader">The Stream that contains the FileData</param>
        public void Unserialize(IoBuffer reader)
        {
            unknown1 = reader.ReadInt32();
            unknown2 = reader.ReadByte();
            if ((parent.Version == 0x07) || (parent.Version == 0x06))
            {
                filename = "";
                unknown3 = reader.ReadInt32();
                unknown4 = reader.ReadByte();
            }
            else
            {
                filename = reader.ReadString();
                unknown3 = 0;
                unknown4 = 0;
            }
        }

        public override string ToString()
        {
            string name = "0x" + Helper.Hex4String((uint)unknown1) + " - 0x" + Helper.Hex4String(unknown2);
            if ((parent.Version == 0x07) || (parent.Version == 0x06)) return name + " - 0x" + Helper.Hex4String((uint)unknown3) + " - 0x" + Helper.Hex4String(unknown4);
            else return name + ": " + filename;
        }

    }

    /// <summary>
    /// This is the actual FileWrapper
    /// </summary>
    /// <remarks>
    /// The wrapper is used to (un)serialize the Data of a file into it's Attributes. So Basically it reads 
    /// a BinaryStream and translates the data into some userdefine Attributes.
    /// </remarks>
    public class CShape : AbstractRcolBlock, IScenegraphBlock
    {
        public static uint TYPE = 0xFC6EB1F7;
        public static String NAME = "cShape";

        #region Attributes
        uint[] unknown;
        public uint[] Unknwon
        {
            get { return unknown; }
            set { unknown = value; }
        }

        ShapeItem[] items;
        public ShapeItem[] Items
        {
            get { return items; }
            set { items = value; }
        }

        ShapePart[] parts;
        public ShapePart[] Parts
        {
            get { return parts; }
            set { parts = value; }
        }


        ObjectGraphNode ogn;
        public ObjectGraphNode GraphNode
        {
            get { return ogn; }
            set { ogn = value; }
        }

        readonly ReferentNode refnode;
        public ReferentNode RefNode
        {
            get { return refnode; }
        }
        #endregion
        /*public Rcol Parent 
		{
			get { return parent; }
		}*/

        /// <summary>
        /// Constructor
        /// </summary>
        public CShape(Rcol parent) : base(parent)
        {
            sgres = new SGResource(null);
            refnode = new ReferentNode(null);
            ogn = new ObjectGraphNode(null);

            unknown = new uint[0];
            items = new ShapeItem[0];
            parts = new ShapePart[0];
            BlockID = 0xFC6EB1F7;
        }

        #region IRcolBlock Member

        /// <summary>
        /// Unserializes a BinaryStream into the Attributes of this Instance
        /// </summary>
        /// <param name="reader">The Stream that contains the FileData</param>
        public override void Unserialize(IoBuffer reader)
        {
            version = reader.ReadUInt32();
            string s = reader.ReadString();

            sgres.BlockID = reader.ReadUInt32();
            sgres.Unserialize(reader);

            s = reader.ReadString();
            refnode.BlockID = reader.ReadUInt32();
            refnode.Unserialize(reader);

            s = reader.ReadString();
            ogn.BlockID = reader.ReadUInt32();
            ogn.Unserialize(reader);

            if (version != 0x06) unknown = new uint[reader.ReadUInt32()];
            else unknown = new uint[0];
            for (int i = 0; i < unknown.Length; i++) unknown[i] = reader.ReadUInt32();

            items = new ShapeItem[reader.ReadUInt32()];
            for (int i = 0; i < items.Length; i++)
            {
                items[i] = new ShapeItem(this);
                items[i].Unserialize(reader);
            }

            parts = new ShapePart[reader.ReadUInt32()];
            for (int i = 0; i < parts.Length; i++)
            {
                parts[i] = new ShapePart();
                parts[i].Unserialize(reader);
            }
        }

        #endregion

        #region IScenegraphItem Member

        public void ReferencedItems(Hashtable refmap, uint parentgroup)
        {
            ArrayList list = new ArrayList();
            foreach (ShapePart part in Parts)
            {
                list.Add(ScenegraphHelper.BuildPfd(part.FileName.Trim() + "_txmt", ScenegraphHelper.TXMT, parentgroup));
            }
            refmap["Subsets"] = list;

            list = new ArrayList();
            foreach (ShapeItem item in Items)
            {
                list.Add(ScenegraphHelper.BuildPfd(item.FileName.Trim(), ScenegraphHelper.GMND, parentgroup));
            }
            refmap["Models"] = list;
        }

        #endregion

        public override void Dispose()
        {
        }
    }
}
