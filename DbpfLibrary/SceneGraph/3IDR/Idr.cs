using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.SceneGraph.GZPS;
using Sims2Tools.DBPF.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Sims2Tools.DBPF.SceneGraph.IDR
{
    public class Idr : SgResource
    {
		// See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
		public const uint TYPE = 0xAC506764;
		public const String NAME = "3IDR";

		public new string FileName
		{
			get => "3D ID Referencing File";
		}

		private DBPFKey[] items;

		public DBPFKey[] Items
		{
			get { return items; }
		}

		public Idr(DBPFEntry entry, IoBuffer reader) : base(entry)
		{
			Unserialize(reader);
		}

		protected void Unserialize(IoBuffer reader)
		{
			_ = reader.ReadUInt32();
			uint type = reader.ReadUInt32();

			items = new DBPFKey[reader.ReadUInt32()];

			for (int i = 0; i < items.Length; i++)
			{
				uint typeID = reader.ReadUInt32();
				uint groupID = reader.ReadUInt32();
				uint instanceID = reader.ReadUInt32();
				uint instanceID2 = (type == 0x02) ? reader.ReadUInt32() : 0x00000000;

				items[i] = new DBPFKey(typeID, groupID, instanceID, instanceID2);
			}
		}

		public override SgResourceList SgNeededResources()
		{
            SgResourceList needed = new SgResourceList
            {
                { Gzps.TYPE, GroupID, InstanceID2, InstanceID }
            };

            return needed;
		}
	}
}
