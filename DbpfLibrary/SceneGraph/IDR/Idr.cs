/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2021
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.Package;
using System;

namespace Sims2Tools.DBPF.SceneGraph.IDR
{
    public class Idr : SgResource
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0xAC506764;
        public const String NAME = "3IDR";

        public override string FileName
        {
            get => "3D ID Referencing File";
        }

        private DBPFKey[] items;

        public DBPFKey[] Items
        {
            get { return items; }
        }

        public Idr(DBPFEntry entry, DbpfReader reader) : base(entry)
        {
            Unserialize(reader);
        }

        protected void Unserialize(DbpfReader reader)
        {
            _ = reader.ReadUInt32();
            uint type = reader.ReadUInt32();

            items = new DBPFKey[reader.ReadUInt32()];

            for (int i = 0; i < items.Length; i++)
            {
                TypeTypeID typeID = reader.ReadTypeId();
                TypeGroupID groupID = reader.ReadGroupId();
                TypeInstanceID instanceID = reader.ReadInstanceId();
                TypeResourceID resourceID = (type == 0x02) ? reader.ReadResourceId() : (TypeResourceID)0x00000000;

                items[i] = new DBPFKey(typeID, groupID, instanceID, resourceID);
            }
        }

        public override SgResourceList SgNeededResources()
        {
            return new SgResourceList();
        }
    }
}
