/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.SceneGraph.Geometry;
using Sims2Tools.DBPF.SceneGraph.RCOL;
using Sims2Tools.DBPF.Utils;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks
{
    public enum FrameType : byte
    {
        Translation = 0x10,
        Rotation = 0x0C,
        Unknown = 0xFF
    }

    public enum AnimationTokenType : byte
    {
        /// One short value (0=transform parameter)
        TwoByte = 0,
        /// Three short values (0=timecode, 1=transform parameter, 2=???)
        SixByte = 1,
        /// Four short values (0=timecode, 1=transform parameter, 2=???, 3=???)
        EightByte = 2
    }

    public class CAnimResourceConst : AbstractRcolBlock
    {
        public static readonly TypeBlockID TYPE = (TypeBlockID)0xFB00791E;
        public static string NAME = "cAnimResourceConst";

        #region Attributes
        byte[] unknowndata;
        internal byte[] Data
        {
            get { return unknowndata; }
            set { unknowndata = value; }
        }

        short unknown1;
        [DescriptionAttribute("The Time the Animation takes to play (probably in ms)")]
        public short TotalTime
        {
            get { return unknown1; }
            set { unknown1 = value; }
        }

        byte[] headerb;
        [DescriptionAttribute("Index 0 and 5 contain string Lengths.")]
        public byte[] HeaderBytes
        {
            get { return headerb; }
        }

        uint[] headeri;
        public uint[] HeaderInts
        {
            get { return headeri; }
        }

        float[] headerf;
        public float[] HeaderFloats
        {
            get { return headerf; }
        }

        string objname;
        public string ObjName
        {
            get { return objname; }
        }
        string objmod;
        public string ObjMod
        {
            get { return objmod; }
        }

        List<AnimationMeshBlock> animMeshBlocks;
        public List<AnimationMeshBlock> MeshBlocks
        {
            get { return animMeshBlocks; }
        }

        List<AnimBlock6> animBlock6s;
        #endregion

        // Needed by reflection to create the class
        public CAnimResourceConst(Rcol parent) : base(parent)
        {
            BlockID = TYPE;
            BlockName = NAME;
            Version = 0x09;

            headerb = new byte[6];
            headeri = new uint[4];
            headerf = new float[9];

            objname = "";
            objmod = "";

            animMeshBlocks = new List<AnimationMeshBlock>();
            animBlock6s = new List<AnimBlock6>();
        }

        public override void Unserialize(DbpfReader reader)
        {
#if DEBUG
            readStart = reader.Position;
#endif

            Version = reader.ReadUInt32();

            string blkName = reader.ReadString();
            TypeBlockID blkId = reader.ReadBlockId();

            NameResource.Unserialize(reader);
            NameResource.BlockName = blkName;
            NameResource.BlockID = blkId;

            int dataLen = reader.ReadInt32();
            UnserializeData(DbpfReader.FromStream(reader.MyStream, reader.Position + dataLen));

#if DEBUG
            readEnd = reader.Position;
#endif
        }

        public void UnserializeData(DbpfReader reader)
        {
            unknown1 = reader.ReadInt16();
            short animMeshBlocksCount = reader.ReadInt16();
            short animBlock6sCount = reader.ReadInt16();

            headerb = reader.ReadBytes(headerb.Length);
            for (int i = 0; i < headeri.Length; i++) headeri[i] = reader.ReadUInt32();
            for (int i = 0; i < headerf.Length; i++) headerf[i] = reader.ReadSingle();

            objname = Helper.ToString(reader.ReadBytes(headerb[5]));
            reader.ReadByte(); //read the terminating 0
            objmod = Helper.ToString(reader.ReadBytes(headerb[0]));
            reader.ReadByte(); //read the terminating 0

            int ct = headerb[0] + 1 + headerb[5] + 1;
            ReadAlign(reader, ct);

            //--- part1 ---
            animMeshBlocks = new List<AnimationMeshBlock>(animMeshBlocksCount);
            int len = 0;
            for (int i = 0; i < animMeshBlocksCount; i++)
            {
                AnimationMeshBlock animMeshBlock = new AnimationMeshBlock(this.Parent);
                animMeshBlock.UnserializeData(reader);
                animMeshBlocks.Add(animMeshBlock);
            }
            for (int i = 0; i < animMeshBlocksCount; i++) len += animMeshBlocks[i].UnserializeName(reader);
            ReadAlign(reader, len);

            //--- part2 ---
            len = 0;
            for (int i = 0; i < animMeshBlocksCount; i++) animMeshBlocks[i].UnserializePart2Data(reader);
            for (int i = 0; i < animMeshBlocksCount; i++) len += animMeshBlocks[i].UnserializePart2Name(reader);
            ReadAlign(reader, len);

#if !DEBUG
            try
            {
#endif
            //--- part3 ---
            for (int i = 0; i < animMeshBlocksCount; i++) animMeshBlocks[i].UnserializePart3Data(reader);
            for (int i = 0; i < animMeshBlocksCount; i++) animMeshBlocks[i].UnserializePart3AddonData(reader);

            //--- part4 ---
            for (int i = 0; i < animMeshBlocksCount; i++) animMeshBlocks[i].UnserializePart4Data(reader);

            //--- part5 ---
            for (int i = 0; i < animMeshBlocksCount; i++) animMeshBlocks[i].UnserializePart5Data(reader);

            //--- part6 ---
            animBlock6s = new List<AnimBlock6>(animBlock6sCount);
            len = 0;
            for (int i = 0; i < animBlock6sCount; i++)
            {
                AnimBlock6 animBlock6 = new AnimBlock6();
                animBlock6.UnserializeData(reader);
                animBlock6s.Add(animBlock6);
            }

            for (int i = 0; i < animBlock6sCount; i++) len += animBlock6s[i].UnserializeName(reader);
#if !DEBUG
            }
            catch { }
#endif

            unknowndata = reader.ReadBytes((int)(reader.Length - reader.Position));
        }

        public override uint FileSize
        {
            get
            {
                long size = 4;

                size += DbpfWriter.Length(NameResource.BlockName) + 4 + NameResource.FileSize;

                size += 4 + DataSize;

                return (uint)size;
            }
        }

        private int DataSize
        {
            get
            {
                long size = 2 + 2 + 2;

                size += headerb.Length;

                size += headeri.Length * 4;
                size += headerf.Length * 4;

                size += objname.Length + 1;
                size += objmod.Length + 1;
                size += CalcAlign(objname.Length + 1 + objmod.Length + 1);

                //--- part1 ---
                long len = 0;
                for (int i = 0; i < animMeshBlocks.Count; i++) size += animMeshBlocks[i].DataSize;
                for (int i = 0; i < animMeshBlocks.Count; i++) len += animMeshBlocks[i].NameSize;
                size += len;
                size += CalcAlign((int)len);

                //--- part2 ---
                len = 0;
                for (int i = 0; i < animMeshBlocks.Count; i++) size += animMeshBlocks[i].Part2DataSize;
                for (int i = 0; i < animMeshBlocks.Count; i++) len += animMeshBlocks[i].Part2NameSize;
                size += len;
                size += CalcAlign((int)len);

                //--- part3 ---
                for (int i = 0; i < animMeshBlocks.Count; i++) size += animMeshBlocks[i].Part3DataSize;
                for (int i = 0; i < animMeshBlocks.Count; i++) size += animMeshBlocks[i].Part3AddonDataSize;

                //--- part4 ---
                for (int i = 0; i < animMeshBlocks.Count; i++) size += animMeshBlocks[i].Part4DataSize;

                //--- part5 ---
                for (int i = 0; i < animMeshBlocks.Count; i++) size += animMeshBlocks[i].Part5DataSize;

                //--- part6 ---
                for (int i = 0; i < animBlock6s.Count; i++) size += animBlock6s[i].DataSize;
                for (int i = 0; i < animBlock6s.Count; i++) size += animBlock6s[i].NameSize;

                size += unknowndata.Length;

                return (int)size;
            }
        }

        public override void Serialize(DbpfWriter writer)
        {
#if DEBUG
            writeStart = writer.Position;
#endif

            writer.WriteUInt32(Version);

            writer.WriteString(NameResource.BlockName);
            writer.WriteBlockId(NameResource.BlockID);
            NameResource.Serialize(writer);

            writer.WriteInt32(DataSize);
            SerializeData(writer);

#if DEBUG
            writeEnd = writer.Position;

            Debug.Assert((writeEnd - writeStart) == FileSize);
            if (!IsDirty) Debug.Assert(((readEnd - readStart) == 0) || ((writeEnd - writeStart) == (readEnd - readStart)));
#endif
        }

        public void SerializeData(DbpfWriter writer)
        {
            writer.WriteInt16(unknown1);
            writer.WriteInt16((short)animMeshBlocks.Count);
            writer.WriteInt16((short)animBlock6s.Count);

            writer.WriteBytes(headerb);

            for (int i = 0; i < headeri.Length; i++) writer.WriteUInt32(headeri[i]);
            for (int i = 0; i < headerf.Length; i++) writer.WriteSingle(headerf[i]);

            byte[] bobjname = Helper.ToBytes(objname);
            byte[] bobjmod = Helper.ToBytes(objmod);
            headerb[0] = (byte)bobjmod.Length;
            headerb[5] = (byte)bobjname.Length;

            foreach (byte b in bobjname) writer.WriteByte(b);
            writer.WriteByte((byte)0);
            foreach (byte b in bobjmod) writer.WriteByte(b);
            writer.WriteByte((byte)0);

            int ct = headerb[0] + headerb[5];
            WriteAlign(writer, ct + 2);

            //--- part1 ---
            int len = 0;
            for (int i = 0; i < animMeshBlocks.Count; i++) animMeshBlocks[i].SerializeData(writer);
            for (int i = 0; i < animMeshBlocks.Count; i++) len += animMeshBlocks[i].SerializeName(writer);
            WriteAlign(writer, len);

            //--- part2 ---
            len = 0;
            for (int i = 0; i < animMeshBlocks.Count; i++) animMeshBlocks[i].SerializePart2Data(writer);
            for (int i = 0; i < animMeshBlocks.Count; i++) len += animMeshBlocks[i].SerializePart2Name(writer);
            WriteAlign(writer, len);

            //--- part3 ---
            for (int i = 0; i < animMeshBlocks.Count; i++) animMeshBlocks[i].SerializePart3Data(writer);
            for (int i = 0; i < animMeshBlocks.Count; i++) animMeshBlocks[i].SerializePart3AddonData(writer);

            //--- part4 ---
            for (int i = 0; i < animMeshBlocks.Count; i++) animMeshBlocks[i].SerializePart4Data(writer);

            //--- part5 ---
            for (int i = 0; i < animMeshBlocks.Count; i++) animMeshBlocks[i].SerializePart5Data(writer);

            //--- part6 ---
            for (int i = 0; i < animBlock6s.Count; i++) animBlock6s[i].SerializeData(writer);
            for (int i = 0; i < animBlock6s.Count; i++) animBlock6s[i].SerializeName(writer);

            writer.WriteBytes(unknowndata);
        }

        #region Alignment
        /// <summary>
        /// Calulates how many bytes we need to align the Stream
        /// </summary>
        /// <param name="ct">Number of bytes read/written</param>
        /// <returns>Number of bytes needed to align</returns>
        static int CalcAlign(int ct)
        {
            int add = 0;
            if (ct % 2 == 0) //even
            {
                add = (ct % 4);
            }
            else //uneven
            {
                add = ct % 2;
                if (((add + ct) % 4) == 0) add += 2;
            }

            return add;
        }

        static void ReadAlign(DbpfReader reader, int ct)
        {
            int add = CalcAlign(ct);
            for (int i = 0; i < add; i++) reader.ReadByte();
        }

        static void WriteAlign(DbpfWriter writer, int ct)
        {
            int add = CalcAlign(ct);
            for (int i = 0; i < add; i++) writer.WriteByte((byte)i);
        }
        #endregion

        public override void Dispose()
        {
        }
    }

    /// <summary>
    /// Assembles the Data Read from the ANIM Resource in a Frame
    /// </summary>
    public class AnimationFrame
    {
        List<AnimationAxisTransform> blocks;
        short tc;
        public AnimationFrame(short tc, FrameType tp)
        {
            this.tc = tc;
            this.tp = tp;
            blocks = new List<AnimationAxisTransform>
            {
                new AnimationAxisTransform(), // X block
                new AnimationAxisTransform(), // Y block
                new AnimationAxisTransform()  // Z block
            };
        }

        internal List<AnimationAxisTransform> Blocks
        {
            get { return blocks; }
        }

        public AnimationAxisTransform XBlock
        {
            get { return blocks[0]; }
            set { blocks[0] = value; }
        }

        public AnimationAxisTransform YBlock
        {
            get { return blocks[1]; }
            set { blocks[1] = value; }
        }

        public AnimationAxisTransform ZBlock
        {
            get { return blocks[2]; }
            set { blocks[2] = value; }
        }


        AnimationAxisTransform GetFrameAddonData(int part)
        {
            AnimationAxisTransform b = GetBlock((byte)(part % 3));

            if (b == null) return new AnimationAxisTransform(null, -1);
            return b;
        }

        public AnimationAxisTransform GetBlock(byte nr)
        {
            return blocks[nr];
        }

        [DescriptionAttribute("The X Value for this Transformation"), CategoryAttribute("Data"), DefaultValueAttribute(0)]
        public short X
        {
            get { return GetFrameAddonData(0).Parameter; }
            set { GetFrameAddonData(0).Parameter = value; }
        }

        [DescriptionAttribute("The Y Value for this Transformation"), CategoryAttribute("Data"), DefaultValueAttribute(0)]
        public short Y
        {
            get { return GetFrameAddonData(1).Parameter; }
            set { GetFrameAddonData(1).Parameter = value; }
        }

        [DescriptionAttribute("The Z Value for this Transformation"), CategoryAttribute("Data"), DefaultValueAttribute(0)]
        public short Z
        {
            get { return GetFrameAddonData(2).Parameter; }
            set { GetFrameAddonData(2).Parameter = value; }
        }

        [DescriptionAttribute("The X Value (as Floating Point) for this Transformation"), CategoryAttribute("Data"), DefaultValueAttribute(0)]
        public float Float_X
        {
            get { return GetFrameAddonData(0).ParameterFloat; }
            set { GetFrameAddonData(0).ParameterFloat = value; }
        }

        [DescriptionAttribute("The Y Value (as Floating Point) for this Transformation"), CategoryAttribute("Data"), DefaultValueAttribute(0)]
        public float Float_Y
        {
            get { return GetFrameAddonData(1).ParameterFloat; }
            set { GetFrameAddonData(1).ParameterFloat = value; }
        }

        [DescriptionAttribute("The Z Value (as Floating Point) for this Transformation"), CategoryAttribute("Data"), DefaultValueAttribute(0)]
        public float Float_Z
        {
            get { return GetFrameAddonData(2).ParameterFloat; }
            set { GetFrameAddonData(2).ParameterFloat = value; }
        }

        [DescriptionAttribute("The TimeCode the X Transformation should be finished"), CategoryAttribute("Data"), DefaultValueAttribute(0)]
        public short TimeCode
        {
            get
            {
                return tc;
            }
            set
            {
                if (tc != value)
                {
                    tc = value;
                    if (blocks[0] != null) blocks[0].TimeCode = value;
                    if (blocks[1] != null) blocks[1].TimeCode = value;
                    if (blocks[2] != null) blocks[2].TimeCode = value;
                }
            }
        }

        [DescriptionAttribute("True if Frames are interpolated linear fro this KeyFrame"), CategoryAttribute("Data"), DefaultValueAttribute(false)]
        public bool Linear
        {
            get
            {
                if (blocks[0] != null) return blocks[0].Linear;
                if (blocks[1] != null) return blocks[1].Linear;
                if (blocks[2] != null) return blocks[2].Linear;
                return false;
            }
            set
            {
                if (blocks[0] != null) blocks[0].Linear = value;
                if (blocks[1] != null) blocks[1].Linear = value;
                if (blocks[2] != null) blocks[2].Linear = value;
            }
        }




        public short Unknown1_X
        {
            get { return GetFrameAddonData(0).Unknown1; }
            set { GetFrameAddonData(0).Unknown1 = value; }
        }

        public short Unknown1_Y
        {
            get { return GetFrameAddonData(1).Unknown1; }
            set { GetFrameAddonData(1).Unknown1 = value; }
        }

        public short Unknown1_Z
        {
            get { return GetFrameAddonData(2).Unknown1; }
            set { GetFrameAddonData(2).Unknown1 = value; }
        }

        public short Unknown2_X
        {
            get { return GetFrameAddonData(0).Unknown2; }
            set { GetFrameAddonData(0).Unknown2 = value; }
        }

        public short Unknown2_Y
        {
            get { return GetFrameAddonData(1).Unknown2; }
            set { GetFrameAddonData(1).Unknown2 = value; }
        }

        public short Unknown2_Z
        {
            get { return GetFrameAddonData(2).Unknown2; }
            set { GetFrameAddonData(2).Unknown2 = value; }
        }

        public override string ToString()
        {
            return tc.ToString();
        }

        [DescriptionAttribute("Data interpreted as Vector"), CategoryAttribute("Information"), DefaultValueAttribute(0x11BA05F0)]
        public Vector3f Vector
        {
            get
            {
                double x = this.Float_X;
                double y = this.Float_Y;
                double z = this.Float_Z;

                return new Vector3f(this.Float_X, this.Float_Y, this.Float_Z);
            }
        }

        FrameType tp;
        [DescriptionAttribute("What kind of Transformation is performed. You can changes this in the Parent Node!"), CategoryAttribute("Information")]
        public FrameType Type
        {
            get
            {
                return tp;
            }
        }
    }
}
