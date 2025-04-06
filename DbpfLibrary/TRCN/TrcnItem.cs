/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2025
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.Utils;

namespace Sims2Tools.DBPF.TRCN
{
    public class TrcnItem
    {
        //private uint used;
        //private uint constId;
        private string constName;
        //private string constDesc = "";
        //private ushort defValue;
        //private ushort minValue;
        //private ushort maxValue;

        public string ConstName
        {
            get => this.constName;
        }

        public TrcnItem(DbpfReader reader, uint version)
        {
            this.Unserialize(reader, version);
        }

        protected void Unserialize(DbpfReader reader, uint version)
        {
            /*this.used =*/
            reader.ReadUInt32();
            /*this.constId =*/
            reader.ReadUInt32();
            this.constName = Helper.ToString(reader.ReadBytes(reader.ReadByte()));

            if (version > 83U)
            {
                /*this.constDesc = Helper.ToString(*/
                reader.ReadBytes(reader.ReadByte())/*)*/;
                /*this.defValue =*/
                reader.ReadByte();
            }
            else
            {
                /*this.constDesc = "";*/
                /*this.defValue =*/
                reader.ReadUInt16();
            }

            /*this.minValue =*/
            reader.ReadUInt16();
            /*this.maxValue = */
            reader.ReadUInt16();
        }

        public override string ToString() => this.constName;

        public static implicit operator string(TrcnItem i) => i.constName;
    }
}
