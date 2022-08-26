/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2022
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF.Images.IMG;
using Sims2Tools.DBPF.IO;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sims2Tools.DBPF.Cigen.CGN1
{
    // Determined by reverse engineering the cigen.package file!
    // This could all be horribly wrong!

    public class Cgn1Dictionary : IEnumerable<Cgn1Item>
    {
        private Dictionary<DBPFKey, Cgn1List> theDictionary;

        public Cgn1Dictionary()
        {
            theDictionary = new Dictionary<DBPFKey, Cgn1List>();
        }

        public Cgn1Dictionary(int capacity)
        {
            theDictionary = new Dictionary<DBPFKey, Cgn1List>(capacity);
        }

        public int Count
        {
            get
            {
                int count = 0;

                foreach (Cgn1List list in theDictionary.Values)
                {
                    count += list.Count;
                }

                return count;
            }
        }

        public bool ContainsKey(DBPFKey key) => theDictionary.ContainsKey(key);

        public List<DBPFKey> GetImageKeys(DBPFKey key)
        {
            List<DBPFKey> imageKeys = new List<DBPFKey>(1);

            if (theDictionary.ContainsKey(key))
            {
                foreach (Cgn1Item item in theDictionary[key].AsList())
                {
                    imageKeys.Add(item.ImageKey);
                }
            }

            return imageKeys;
        }

        public void Add(DBPFKey key, Cgn1Item item)
        {
            if (!theDictionary.TryGetValue(key, out Cgn1List list))
            {
                list = new Cgn1List();
                theDictionary.Add(key, list);
            }

            list.Add(item);
        }

        public bool Remove(DBPFKey key)
        {
            return theDictionary.Remove(key);
        }

        public IEnumerator<Cgn1Item> GetEnumerator()
        {
            return new Cgn1DictionaryEnum(theDictionary);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }
    }


    public class Cgn1DictionaryEnum : IEnumerator<Cgn1Item>
    {
        private Dictionary<DBPFKey, Cgn1List> theDictionary;
        private Dictionary<DBPFKey, Cgn1List>.Enumerator dictionaryEnum;
        private Cgn1ListEnum listEnum;

        public Cgn1DictionaryEnum(Dictionary<DBPFKey, Cgn1List> theDictionary)
        {
            this.theDictionary = theDictionary;
            Reset();
        }

        public bool MoveNext()
        {
            if (listEnum.MoveNext())
            {
                return true;
            }

            if (dictionaryEnum.MoveNext())
            {
                listEnum = (Cgn1ListEnum)dictionaryEnum.Current.Value.GetEnumerator();
                return listEnum.MoveNext();
            }

            return false;
        }

        public void Reset()
        {
            dictionaryEnum = theDictionary.GetEnumerator();
            if (dictionaryEnum.MoveNext())
            {
                listEnum = (Cgn1ListEnum)dictionaryEnum.Current.Value.GetEnumerator();
            }
            else
            {
                listEnum = (Cgn1ListEnum)(new Cgn1List()).GetEnumerator();
            }
        }

        object IEnumerator.Current => Current;

        public Cgn1Item Current => listEnum.Current;

        public void Dispose()
        {
        }
    }


    public class Cgn1List : IEnumerable<Cgn1Item>
    {
        private List<Cgn1Item> theList;

        public Cgn1List()
        {
            theList = new List<Cgn1Item>();
        }

        public int Count => theList.Count;

        public List<Cgn1Item> AsList()
        {
            return theList;
        }

        public void Add(Cgn1Item item)
        {
            theList.Add(item);
        }

        public IEnumerator<Cgn1Item> GetEnumerator()
        {
            return new Cgn1ListEnum(theList);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }
    }

    public class Cgn1ListEnum : IEnumerator<Cgn1Item>
    {
        private List<Cgn1Item> theList;
        private List<Cgn1Item>.Enumerator listEnum;

        public Cgn1ListEnum(List<Cgn1Item> theList)
        {
            this.theList = theList;
            Reset();
        }

        public bool MoveNext()
        {
            return listEnum.MoveNext();
        }

        public void Reset()
        {
            listEnum = theList.GetEnumerator();
        }

        object IEnumerator.Current => Current;

        public Cgn1Item Current => listEnum.Current;

        public void Dispose()
        {
        }
    }


    public class Cgn1Item
    {
        public static uint FixedFileSize = (2 + 2 + 16 + 16 + 68);

        private DBPFKey ownerKey;
        private DBPFKey imageKey;

        private ushort unknown1, unknown2;
        private byte[] unknownData;

        public DBPFKey OwnerKey => ownerKey;
        public DBPFKey ImageKey => imageKey;

        public Cgn1Item(DbpfReader reader)
        {
            this.Unserialize(reader);
        }

        protected void Unserialize(DbpfReader reader)
        {
            // Unknown 4 bytes, that look like two 16-bit values, first is usually 0x0012, second looks like it could be the length of the image data
            unknown1 = reader.ReadUInt16();
            unknown2 = reader.ReadUInt16();

            // TGIR of the owning resource, have seen AGED (skins) and GZPS (clothing) types, but probably more
            ownerKey = new DBPFKey(reader.ReadTypeId(), reader.ReadGroupId(), reader.ReadInstanceId(), reader.ReadResourceId());

            // We're expecting an IMG resource now
            TypeTypeID nextType = reader.ReadTypeId();
            Debug.Assert(nextType.Equals(Img.TYPE));

            // Oddly, the group for the IMG is given as 0x6F001872 (which could be a hash of "cigen"), but the actual group is 0xFFFFFFFF
            _ = reader.ReadGroupId();
            imageKey = new DBPFKey(nextType, DBPFData.GROUP_LOCAL, reader.ReadInstanceId(), reader.ReadResourceId());

            // Unknown 68 bytes, mostly 0x00s, could be anything!
            unknownData = reader.ReadBytes(17 * 4);
        }

        public uint FileSize => FixedFileSize;

        public void Serialize(DbpfWriter writer)
        {
            writer.WriteUInt16(unknown1);
            writer.WriteUInt16(unknown2);

            writer.WriteTypeId(ownerKey.TypeID);
            writer.WriteGroupId(ownerKey.GroupID);
            writer.WriteInstanceId(ownerKey.InstanceID);
            writer.WriteResourceId(ownerKey.ResourceID);

            writer.WriteTypeId(imageKey.TypeID);
            writer.WriteGroupId((TypeGroupID)0x6F001872);
            writer.WriteInstanceId(imageKey.InstanceID);
            writer.WriteResourceId(imageKey.ResourceID);

            writer.WriteBytes(unknownData);
        }
    }
}
