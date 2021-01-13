using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.SceneGraph.Geometry;
using Sims2Tools.DBPF.SceneGraph.RCOL;
using Sims2Tools.DBPF.Utils;
using System;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks
{
    public class ExtensionItem
    {
        //Known Types
        public enum ItemTypes : byte
        {
            Value = 0x02,
            Float = 0x03,
            Translation = 0x05,
            String = 0x06,
            Array = 0x07,
            Rotation = 0x08,
            Binary = 0x09
        }

        #region Attributes
        ItemTypes typecode;
        public ItemTypes Typecode
        {
            get { return typecode; }
            set { typecode = value; }
        }

        string varname;
        public string Name
        {
            get
            {
                if (varname == null) return "";
                return varname;
            }
            set { varname = value; }
        }

        int val;
        public int Value
        {
            get { return val; }
            set { val = value; }
        }

        float single;
        public float Single
        {
            get { return single; }
            set { single = value; }
        }

        Vector3f translation;
        public Vector3f Translation
        {
            get { return translation; }
            set { translation = value; }
        }

        string str;
        public string String
        {
            get { return str; }
            set { str = value; }
        }

        ExtensionItem[] ei;
        public ExtensionItem[] Items
        {
            get { return ei; }
            set { ei = value; }
        }

        Quaternion rotation;
        public Quaternion Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        byte[] data;
        public byte[] Data
        {
            get { return data; }
            set { data = value; }
        }

        #endregion

        public ExtensionItem()
        {
            varname = "";
            translation = new Vector3f();
            single = 0;
            ei = new ExtensionItem[0];
            rotation = new Quaternion();
            data = new byte[0];
            str = "";
        }

        /// <summary>
        /// Unserializes a BinaryStream into the Attributes of this Instance
        /// </summary>
        /// <param name="reader">The Stream that contains the FileData</param>
        public void Unserialize(IoBuffer reader)
        {
            typecode = (ItemTypes)reader.ReadByte();
            varname = reader.ReadString();

            switch (typecode)
            {
                case ItemTypes.Value:
                    {
                        val = reader.ReadInt32();
                        break;
                    }
                case ItemTypes.Float:
                    {
                        single = reader.ReadSingle();
                        break;
                    }
                case ItemTypes.Translation:
                    {
                        translation.Unserialize(reader);
                        break;
                    }
                case ItemTypes.String:
                    {
                        str = reader.ReadString();
                        break;
                    }
                case ItemTypes.Array:
                    {
                        ei = new ExtensionItem[reader.ReadUInt32()];
                        for (int i = 0; i < ei.Length; i++)
                        {
                            ei[i] = new ExtensionItem();
                            ei[i].Unserialize(reader);
                        }
                        break;
                    }
                case ItemTypes.Rotation:
                    {
                        rotation.Unserialize(reader);
                        break;
                    }
                case ItemTypes.Binary:
                    {
                        int len = reader.ReadInt32();
                        data = reader.ReadBytes(len);
                        break;
                    }
                default:
                    {
                        throw new Exception("Unknown Extension Item 0x" + Helper.Hex2String((byte)typecode) + "\n\nPosition: 0x" + Helper.Hex8String((uint)reader.Position));
                    }
            }
        }
    }

    /// <summary>
    /// This is the actual FileWrapper
    /// </summary>
    /// <remarks>
    /// The wrapper is used to (un)serialize the Data of a file into it's Attributes. So Basically it reads 
    /// a BinaryStream and translates the data into some userdefine Attributes.
    /// </remarks>
    public class Extension : AbstractRcolBlock
    {
        #region Attributes

        byte typecode;
        public byte TypeCode
        {
            get { return typecode; }
        }

        string varname;
        public string VarName
        {
            get
            {
                if (varname == null) return "";
                return varname;
            }
        }

        ExtensionItem[] items;
        public ExtensionItem[] Items
        {
            get { return items; }
        }

        readonly byte[] data;

        //int unknown1;
        //int unknown2;
        #endregion


        /// <summary>
        /// Constructor
        /// </summary>
        public Extension(Rcol parent) : base(parent)
        {
            items = new ExtensionItem[0];
            version = 0x03;
            typecode = 0x07;
            data = new byte[0];
            varname = "";
        }

        #region IRcolBlock Member

        /// <summary>
        /// Unserializes a BinaryStream into the Attributes of this Instance
        /// </summary>
        /// <param name="reader">The Stream that contains the FileData</param>
        public override void Unserialize(IoBuffer reader) { Unserialize(reader, 0); }

        /// <summary>
        /// Unserializes a BinaryStream into the Attributes of this Instance
        /// </summary>
        /// <param name="reader">The Stream that contains the FileData</param>
        public void Unserialize(IoBuffer reader, uint ver)
        {
            version = reader.ReadUInt32();
            typecode = reader.ReadByte();

            if ((typecode < 0x07))
            {
                int sz = 16;
                if ((typecode != 0x03) || (ver == 4)) sz += 15;
                if ((typecode <= 0x03) && (version == 3))
                {
                    if (ver == 5) sz = 31;
                    else sz = 15;
                }
                if ((typecode <= 0x03) && ver == 4) sz = 31;

                items = new ExtensionItem[1];
                ExtensionItem ei = new ExtensionItem();
                ei.Typecode = ExtensionItem.ItemTypes.Binary;
                ei.Data = reader.ReadBytes(sz);
                items[0] = ei;
            }
            else
            {
                varname = reader.ReadString();

                items = new ExtensionItem[reader.ReadUInt32()];
                for (int i = 0; i < items.Length; i++)
                {
                    items[i] = new ExtensionItem();
                    items[i].Unserialize(reader);
                }
            }

        }

        #endregion

        /// <summary>
        /// You can use this to setop the Controls on a TabPage befor it is dispplayed
        /// </summary>
        public override void Dispose()
        {
        }
    }
}
