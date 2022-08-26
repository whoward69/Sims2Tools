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

using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.SceneGraph.RCOL;
using Sims2Tools.DBPF.Utils;

namespace Sims2Tools.DBPF.SceneGraph
{
    public class ObjectGraphNodeItem
    {
        byte enabled;
        byte depend;
        uint index;

        public byte Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public byte Dependant
        {
            get { return depend; }
            set { depend = value; }
        }

        public uint Index
        {
            get { return index; }
            set { index = value; }
        }

        public void Unserialize(DbpfReader reader)
        {
            enabled = reader.ReadByte();
            depend = reader.ReadByte();
            index = reader.ReadUInt32();
        }

        public override string ToString()
        {
            return index.ToString() + ": " + Helper.Hex2PrefixString(enabled) + ", " + Helper.Hex2PrefixString(depend);
        }

    }

    public class ObjectGraphNode : AbstractRcolBlock
    {
        ObjectGraphNodeItem[] items;
        public ObjectGraphNodeItem[] Items
        {
            get { return items; }
            set { items = value; }
        }

        string filename;
        public string FileName
        {
            get { return filename ?? BlockName; }
            set { filename = value; }
        }

        public ObjectGraphNode(Rcol parent) : base(parent)
        {
            items = new ObjectGraphNodeItem[0];
            version = 4;
        }

        public override void Unserialize(DbpfReader reader)
        {
            version = reader.ReadUInt32();

            items = new ObjectGraphNodeItem[reader.ReadUInt32()];
            for (int i = 0; i < items.Length; i++)
            {
                items[i] = new ObjectGraphNodeItem();
                items[i].Unserialize(reader);
            }

            if (version == 0x04) filename = reader.ReadString();
            else filename = "cObjectGraphNode";
        }

        public override string ToString()
        {
            return filename;
        }

        public override void Dispose()
        {
        }
    }
}
