/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2024
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.Utils;
using System.Xml;

namespace Sims2Tools.DBPF.Groups.GROP
{
    public class GropItem
    {
        private string filename;

        uint unknown1;
        TypeGroupID localGroupID;
        TypeGroupID[] refGroupIDs;

        public string FileName => filename.Trim();

        public TypeGroupID LocalGroupID => localGroupID;
        public TypeGroupID[] RefGroupIDs => refGroupIDs;

        public GropItem(DbpfReader reader)
        {
            this.Unserialize(reader);
        }

        protected void Unserialize(DbpfReader reader)
        {
            filename = Helper.ToString(reader.ReadBytes(reader.ReadInt32()));

            unknown1 = reader.ReadUInt32();

            localGroupID = reader.ReadGroupId();

            refGroupIDs = new TypeGroupID[reader.ReadUInt32()];

            for (int i = 0; i < refGroupIDs.Length; i++)
            {
                refGroupIDs[i] = reader.ReadGroupId();
            }
        }

        public XmlElement AddXml(XmlElement parent)
        {
            XmlElement element = parent.OwnerDocument.CreateElement("entry");
            parent.AppendChild(element);

            element.SetAttribute("fileName", FileName);
            element.SetAttribute("unknown1", unknown1.ToString());
            element.SetAttribute("localGroupID", LocalGroupID.ToString());

            XmlElement refGroups = parent.OwnerDocument.CreateElement("refGroups");
            element.AppendChild(refGroups);

            for (int i = 0; i < refGroupIDs.Length; i++)
            {
                XmlElement refGroup = parent.OwnerDocument.CreateElement("refGroup");
                refGroups.AppendChild(refGroup);

                refGroup.SetAttribute("groupID", refGroupIDs[i].ToString());
            }

            return element;
        }

        public override string ToString()
        {
            string str = $"{FileName} => {Helper.Hex8PrefixString(unknown1)}:{LocalGroupID} (";

            for (int i = 0; i < refGroupIDs.Length; i++)
            {
                if (i != 0) str += ", ";
                str += refGroupIDs[i];
            }

            return $"{str})";
        }
    }
}
