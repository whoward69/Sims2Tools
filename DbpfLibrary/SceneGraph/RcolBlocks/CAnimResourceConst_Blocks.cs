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
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks
{
    public abstract class AnimBlockBase
    {
        protected string MinStrLength(string input, int length)
        {
            while (input.Length < length) input = "0" + input;
            return input;
        }
    }

    public abstract class AnimBlock : AnimBlockBase
    {
        protected string name = "";

        public virtual string Name
        {
            get => name;
            set => name = value;
        }

        internal AnimBlock()
        {
        }

        internal int UnserializeName(DbpfReader reader)
        {
            name = reader.ReadPChar();

            return name.Length + 1;
        }

        internal long NameSize
        {
            get
            {
                long size = DbpfWriter.PLength(name);

                return size;
            }
        }

        internal int SerializeName(DbpfWriter writer)
        {
            writer.WritePChar(name);

            return name.Length + 1;
        }

        public override string ToString() => name;
    }

    // Part 1 - Mesh
    public class AnimationMeshBlock : AnimBlock
    {
        #region Attributes
        Rcol parent;
        public Rcol Parent
        {
            get { return parent; }
        }

        public CAnimResourceConst Animation
        {
            get
            {
                return (CAnimResourceConst)parent.Blocks[0];
            }
        }

        List<AnimationFrameBlock> animFrameBlocks;
        public List<AnimationFrameBlock> FrameBlocks
        {
            get { return animFrameBlocks; }
            set { animFrameBlocks = value; }
        }

        [DescriptionAttribute("Number of loaded AnimationFrameBlock Items"), CategoryAttribute("Information")]
        public int FrameBlocksCount
        {
            get { return animFrameBlocks.Count; }
        }

        List<AnimBlock4> ab4;
        public List<AnimBlock4> Part4
        {
            get { return ab4; }
        }

        [DescriptionAttribute("Number of loaded AnimBlock4 Items"), CategoryAttribute("Information")]
        public int Part4Count
        {
            get { return ab4.Count; }
        }

        uint[] datai;
        [DescriptionAttribute("Reserved"), CategoryAttribute("Reserved"), DefaultValueAttribute(0x11BA05F0)]
        public uint Unknown1
        {
            get { return datai[0]; }
            set { datai[0] = value; }
        }
        [DescriptionAttribute("Reserved"), CategoryAttribute("Reserved"), DefaultValueAttribute(0x11BA05F0)]
        public uint Unknown2
        {
            get { return datai[1]; }
            set { datai[1] = value; }
        }
        [DescriptionAttribute("Reserved"), CategoryAttribute("Reserved"), DefaultValueAttribute(0x11BA05F0)]
        public uint Unknown3
        {
            get { return datai[2]; }
            set { datai[2] = value; }
        }
        [DescriptionAttribute("Reserved"), CategoryAttribute("Reserved"), DefaultValueAttribute(0x11BA05F0)]
        public uint Unknown4
        {
            get { return datai[3]; }
            set { datai[3] = value; }
        }
        [DescriptionAttribute("Reserved"), CategoryAttribute("Reserved"), DefaultValueAttribute(0x11BA05F0)]
        public uint Unknown5
        {
            get { return datai[4]; }
            set { datai[4] = value; }
        }

        short[] datas;
        public short SUnknown1
        {
            get { return datas[0]; }
            set { datas[0] = value; }
        }

        [DescriptionAttribute("Number of assigned AnimationFrameBlock Items")]
        public short AnimatedBoneCount
        {
            get { return datas[1]; }
        }

        [DescriptionAttribute("Lower 6 Bits(?) are reserved for the Number of assigned AnimBlock4 Items")]
        public short SUnknown3
        {
            get { return datas[2]; }
            set { datas[2] = value; }
        }
        public short SUnknown4
        {
            get { return datas[3]; }
            set { datas[3] = value; }
        }
        #endregion

        internal AnimationMeshBlock(Rcol parent)
        {
            datai = new uint[6];
            datas = new short[4];
            animFrameBlocks = new List<AnimationFrameBlock>();
            ab4 = new List<AnimBlock4>();
            this.parent = parent;
        }

        /// <summary>
        /// Unserializes a BinaryStream into the Attributes of this Instance
        /// </summary>
        /// <param name="reader">The Stream that contains the FileData</param>
        internal void UnserializeData(DbpfReader reader)
        {
            datai[0] = reader.ReadUInt32();
            datai[1] = reader.ReadUInt32();

            datas[0] = reader.ReadInt16();
            datas[1] = reader.ReadInt16();  //number of ab2 Items
            datas[2] = reader.ReadInt16();  //number of ab4 Items (and some unknown Bits)
            datas[3] = reader.ReadInt16();

            datai[2] = reader.ReadUInt32();
            datai[3] = reader.ReadUInt32();
            datai[4] = reader.ReadUInt32();
        }


        internal long DataSize
        {
            get
            {
                long size = 4 + 4;

                size += 2 + 2 + 2 + 2;

                size += 4 + 4 + 4;

                return size;
            }
        }

        internal void SerializeData(DbpfWriter writer)
        {
            this.SetPart2Count(this.FrameBlocksCount);
            this.SetPart4Count(this.Part4Count);

            writer.WriteUInt32(datai[0]);
            writer.WriteUInt32(datai[1]);

            writer.WriteInt16(datas[0]);
            writer.WriteInt16(datas[1]);
            writer.WriteInt16(datas[2]);
            writer.WriteInt16(datas[3]);

            writer.WriteUInt32(datai[2]);
            writer.WriteUInt32(datai[3]);
            writer.WriteUInt32(datai[4]);
        }

        /// <summary>
        /// Unserializes a BinaryStream into the Attributes of this Instance
        /// </summary>
        /// <param name="reader">The Stream that contains the FileData</param>
        internal void UnserializePart2Data(DbpfReader reader)
        {
            animFrameBlocks = new List<AnimationFrameBlock>(GetPart2Count());
            for (int i = 0; i < GetPart2Count(); i++)
            {
                AnimationFrameBlock animFrameBlock = new AnimationFrameBlock(this);
                animFrameBlock.UnserializeData(reader);
                animFrameBlocks.Add(animFrameBlock);
            }
        }

        internal long Part2DataSize
        {
            get
            {
                long size = 0;

                for (int i = 0; i < animFrameBlocks.Count; i++) size += animFrameBlocks[i].DataSize;

                return size;
            }
        }

        internal void SerializePart2Data(DbpfWriter writer)
        {
            for (int i = 0; i < animFrameBlocks.Count; i++) animFrameBlocks[i].SerializeData(writer);
        }

        /// <summary>
        /// Unserializes a BinaryStream into the Attributes of this Instance
        /// </summary>
        /// <param name="reader">The Stream that contains the FileData</param>
        internal int UnserializePart2Name(DbpfReader reader)
        {
            int len = 0;
            for (int i = 0; i < animFrameBlocks.Count; i++) len += animFrameBlocks[i].UnserializeName(reader);
            return len;
        }

        internal long Part2NameSize
        {
            get
            {
                long size = 0;

                for (int i = 0; i < animFrameBlocks.Count; i++) size += animFrameBlocks[i].NameSize;

                return size;
            }
        }

        internal int SerializePart2Name(DbpfWriter writer)
        {
            int len = 0;
            for (int i = 0; i < animFrameBlocks.Count; i++) len += animFrameBlocks[i].SerializeName(writer);
            return len;
        }

        internal void UnserializePart3Data(DbpfReader reader)
        {
            for (int i = 0; i < animFrameBlocks.Count; i++) animFrameBlocks[i].UnserializePart3Data(reader);
        }

        internal long Part3DataSize
        {
            get
            {
                long size = 0;

                for (int i = 0; i < animFrameBlocks.Count; i++) size += animFrameBlocks[i].Part3DataSize;

                return size;
            }
        }

        internal void SerializePart3Data(DbpfWriter writer)
        {
            for (int i = 0; i < animFrameBlocks.Count; i++) animFrameBlocks[i].SerializePart3Data(writer);
        }

        internal void UnserializePart3AddonData(DbpfReader reader)
        {
            for (int i = 0; i < animFrameBlocks.Count; i++) animFrameBlocks[i].UnserializePart3AddonData(reader);
        }

        internal long Part3AddonDataSize
        {
            get
            {
                long size = 0;

                for (int i = 0; i < animFrameBlocks.Count; i++) size += animFrameBlocks[i].Part3AddonDataSize;

                return size;
            }
        }

        internal void SerializePart3AddonData(DbpfWriter writer)
        {
            for (int i = 0; i < animFrameBlocks.Count; i++) animFrameBlocks[i].SerializePart3AddonData(writer);
        }

        /// <summary>
        /// Unserializes a BinaryStream into the Attributes of this Instance
        /// </summary>
        /// <param name="reader">The Stream that contains the FileData</param>
        internal void UnserializePart4Data(DbpfReader reader)
        {
            ab4 = new List<AnimBlock4>(GetPart4Count());
            for (int i = 0; i < GetPart4Count(); i++)
            {
                AnimBlock4 animBlock4 = new AnimBlock4();
                animBlock4.UnserializeData(reader);
                ab4.Add(animBlock4);
            }
        }

        internal long Part4DataSize
        {
            get
            {
                long size = 0;

                for (int i = 0; i < ab4.Count; i++) size += ab4[i].DataSize;

                return size;
            }
        }

        internal void SerializePart4Data(DbpfWriter writer)
        {
            for (int i = 0; i < ab4.Count; i++) ab4[i].SerializeData(writer);
        }

        internal void UnserializePart5Data(DbpfReader reader)
        {
            for (int i = 0; i < ab4.Count; i++) ab4[i].UnserializePart5Data(reader);
        }

        internal long Part5DataSize
        {
            get
            {
                long size = 0;

                for (int i = 0; i < ab4.Count; i++) size += ab4[i].Part5DataSize;

                return size;
            }
        }

        internal void SerializePart5Data(DbpfWriter writer)
        {
            for (int i = 0; i < ab4.Count; i++) ab4[i].SerializePart5Data(writer);
        }

        /// <summary>
        /// Returns the Number of Items for Part 2 assigned to this Object
        /// </summary>
        /// <returns>Number of Items</returns>
        int GetPart2Count()
        {
            return (datas[1]);
        }

        /// <summary>
        /// Set the count for Part 5 Items
        /// </summary>
        /// <param name="ct">The New Count</param>
        void SetPart2Count(int ct)
        {
            datas[1] = (short)ct;
        }

        /// <summary>
        /// Returns the Number of Items for Part 4 assigned to this Object
        /// </summary>
        /// <returns>Number of Items</returns>
        int GetPart4Count()
        {
            return (datas[2] & 0x3f);
        }

        /// <summary>
        /// Set the count for Part 5 Items
        /// </summary>
        /// <param name="ct">The New Count</param>
        void SetPart4Count(int ct)
        {
            if (ct > 0x3f) ct = 0x3f;
            ct = ct & 0x3f;

            datas[2] = (short)((int)datas[2] & 0x0000FFC0);
            datas[2] = (short)((ushort)datas[2] | (ushort)ct);
        }

        /// <summary>
        /// Returns the first transformation for the given name and type
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns>null or the matching Block</returns>
        public AnimationFrameBlock GetJointTransformation(string name, FrameType type)
        {
            foreach (AnimationFrameBlock ab in this.FrameBlocks)
                if (ab.Name == name && ab.TransformationType == type && ab.AxisTransformBlocksCount == 3)
                    return ab;

            return null;
        }
    }

    // Part 2 - Frame
    public class AnimationFrameBlock : AnimBlock, ICloneable
    {
        #region Attributes
        AnimationMeshBlock parent;
        public AnimationMeshBlock Parent
        {
            get { return parent; }
        }

        List<AnimationAxisTransformBlock> animAxisTransformBlocks;
        public List<AnimationAxisTransformBlock> AxisTransformBlocks
        {
            get { return animAxisTransformBlocks; }
        }
        [DescriptionAttribute("Number of loaded AnimationAxisTransformBlock Items"), CategoryAttribute("Information")]
        public int AxisTransformBlocksCount
        {
            get { return animAxisTransformBlocks.Count; }
        }

        internal int MaxAxisFrameCount
        {
            get
            {
                int ct = 0;
                foreach (AnimationAxisTransformBlock ab in animAxisTransformBlocks)
                    ct = Math.Max(ct, ab.Count);

                return ct;
            }
        }

        [DescriptionAttribute("Number of loaded Frames"), CategoryAttribute("Information")]
        public int FrameCount
        {
            get { return Frames.Count; }
        }

        public void InterpolateMissingBlocks(List<AnimationFrame> frames, short maxtime)
        {
            if (frames.Count == 0) return;

            for (int i = 0; i < frames.Count; i++)
                for (int j = 0; j < frames[i].Blocks.Count; j++)
                    if (frames[i].Blocks[j] == null) Interpolate(frames, i, j, maxtime);

        }

        public List<AnimationFrame> InterpolateMissingFrames()
        {
            List<AnimationFrame> frames = this.UnlockedFrames;
            if (frames.Count != 0)
                InterpolateMissingBlocks(frames, frames[frames.Count - 1].TimeCode);

            return frames;
        }

        public List<AnimationFrame> GetFrames(bool exludelocked)
        {
            List<short> tclist = new List<short>();
            Dictionary<short, AnimationFrame> ht = new Dictionary<short, AnimationFrame>();

            //get a List of all TimeCodes
            for (int i = 0; i < MaxAxisFrameCount; i++)
            {
                foreach (AnimationAxisTransformBlock ab in animAxisTransformBlocks)
                {
                    List<int> tcs = ab.GetTimeCodes(true, true);

                    if (ab.Locked && exludelocked && ab.Count <= 1) tcs.Clear();
                    foreach (int rtc in tcs)
                    {
                        short tc = (short)rtc;
                        if (!tclist.Contains((short)tc))
                        {
                            tclist.Add(tc);
                            ht[tc] = new AnimationFrame(tc, this.TransformationType);
                        }
                    }
                }
            }

            tclist.Sort();
            for (int part = 0; part < animAxisTransformBlocks.Count; part++)
            {
                AnimationAxisTransformBlock ab = animAxisTransformBlocks[part];
                if (ab.Locked && exludelocked && ab.Count <= 1) continue;

                for (int i = 0; i < ab.Count; i++)
                {
                    short tc = ab.GetTimeCode(i);
                    AnimationFrame af = ht[tc];
                    if (af != null)
                    {
                        if (part == 0) af.XBlock = ab.AxisTransforms[i];
                        else if (part == 1) af.YBlock = ab.AxisTransforms[i];
                        else if (part == 2) af.ZBlock = ab.AxisTransforms[i];
                    }
                }
            }

            //build ordered List
            List<AnimationFrame> afs = new List<AnimationFrame>(tclist.Count);
            foreach (short tc in tclist)
            {
                afs.Add(ht[tc]);
            }

            return afs;
        }

        [DescriptionAttribute("Available Frames"), CategoryAttribute("Information"), Browsable(false)]
        public List<AnimationFrame> Frames
        {
            get
            {
                return GetFrames(false);
            }
        }

        [DescriptionAttribute("Available Frames"), CategoryAttribute("Information"), Browsable(false)]
        public List<AnimationFrame> UnlockedFrames
        {
            get
            {
                return GetFrames(false); //should be true normaly, but that seems not to work!
            }
        }

        uint[] datai;
        [DescriptionAttribute("Reserved"), CategoryAttribute("Reserved"), DefaultValueAttribute(0x11BA05F0)]
        public uint Unknown1
        {
            get { return datai[0]; }
            set { datai[0] = value; }
        }
        [DescriptionAttribute("Reserved"), CategoryAttribute("Reserved"), DefaultValueAttribute(0x11BA05F0)]
        public uint Unknown2
        {
            get { return datai[1]; }
            set { datai[1] = value; }
        }

        [DescriptionAttribute("CRC32 over the Name."), CategoryAttribute("Information"), ReadOnly(true)]
        public uint NameChecksum
        {
            get { return datai[2]; }
            set { datai[2] = value; }
        }

        [DescriptionAttribute("Reserved"), CategoryAttribute("Reserved"), DefaultValueAttribute(0x11BA05F0)]
        public uint Unknown4
        {
            get { return datai[3]; }
            set { datai[3] = value; }
        }

        [DescriptionAttribute("What kind of Transformation is performed."), CategoryAttribute("Information")]
        public FrameType TransformationType
        {
            get
            {
                uint i = Unknown5 & 0x01F00000;
                i = i >> 20;
                return (FrameType)((byte)i);
            }
            set
            {
                uint i = (uint)value;
                i = i << 20;
                i = i & 0x01F00000;
                Unknown5 = (uint)((Unknown5 & 0xFE0FFFFF) | i);
            }
        }

        [DescriptionAttribute("The duration of this animation Block."), CategoryAttribute("Information"), ReadOnly(true)]
        public short Duration
        {
            get
            {
                uint i = Unknown5 & 0x00007FFF;
                return (short)i;
            }
            set
            {
                uint i = (uint)value;
                i = i & 0x00007FFF;
                i = i | 0x00008000;
                Unknown5 = (uint)((Unknown5 & 0xFFFF0000) | i);
            }
        }

        public override string Name
        {
            get { return base.Name; }
            set
            {
                base.Name = value;
                this.NameChecksum = Hashes.AnimationHash(base.Name);
            }
        }


        [DescriptionAttribute("Highest 3 Bits (Bit 31-29) contain the Number of assigned AnimationAxisTransformBlock Items, Bits 16-23 describe the Transformation Type (0=Translation, C=Rotation). Bits 0-15 Decode the Time this Animation Runs.")]
        public uint Unknown5
        {
            get { return datai[4]; }
            set { datai[4] = value; }
        }

        [DescriptionAttribute("Bits 24-28 of Unknown5")]
        public byte Unknown5Bits
        {
            get
            {
                uint i = Unknown5 & 0x1E000000;
                i = i >> 25;
                return (byte)i;
            }

            set
            {
                uint i = ((uint)value << 24) & 0x1E000000;
                Unknown5 = (Unknown5 & 0xE1FFFFFF) | i;
            }
        }

        [DescriptionAttribute("Highest 3 Bits contain the Number of assigned AnimationAxisTransformBlock Items")]
        public string Unknown5Binary
        {
            get
            {
                string s = Convert.ToString(Unknown5, 2);
                s = MinStrLength(s, 32);
                int p = s.Length - 4;
                while (p >= 0)
                {
                    s = s.Insert(p, " ");
                    p -= 4;
                }
                return s.Trim();
            }

        }

        [DescriptionAttribute("Highest 3 Bits contain the Number of assigned AnimationAxisTransformBlock Items")]
        public string Unknown5Hex
        {
            get { return Helper.Hex8PrefixString(Unknown5); }

        }
        [DescriptionAttribute("Reserved"), CategoryAttribute("Reserved"), DefaultValueAttribute(0x11BA05F0)]
        public uint Unknown6
        {
            get { return datai[5]; }
            set { datai[5] = value; }
        }
        #endregion

        public AnimationFrameBlock CloneBase(bool fullclone)
        {
            AnimationFrameBlock ab = new AnimationFrameBlock(this.parent);

            ab.datai = (uint[])this.datai.Clone();
            ab.name = this.name;
            if (fullclone)
            {
                ab.animAxisTransformBlocks = new List<AnimationAxisTransformBlock>(this.AxisTransformBlocksCount);

                for (int i = 0; i < ab.AxisTransformBlocksCount; i++)
                {
                    ab.AxisTransformBlocks[i] = this.AxisTransformBlocks[i].CloneBase(); // TODO - DBPF Library - ANIM - check this
                }
            }

            return ab;
        }

        /// <summary>
        /// Creat an additional Part3 Item
        /// </summary>
        public void AddNewAxis()
        {
            animAxisTransformBlocks.Add(new AnimationAxisTransformBlock(this));
        }

        public void CreateBaseAxisSet()
        {
            CreateBaseAxisSet(AnimationTokenType.SixByte);
        }

        public void CreateBaseAxisSet(AnimationTokenType t)
        {
            animAxisTransformBlocks = new List<AnimationAxisTransformBlock>(3);
            for (int i = 0; i < AxisTransformBlocksCount; i++)
            {
                animAxisTransformBlocks.Add(new AnimationAxisTransformBlock(this) { Type = t });
            }
        }

        /// <summary>
        /// Change the TokenType of all AxisSets to t
        /// </summary>
        /// <param name="t">The new TokenType</param>
        public void ChangeTokenType(AnimationTokenType t)
        {
            for (int i = 0; i < AxisTransformBlocksCount; i++)
                animAxisTransformBlocks[i].Type = t;
        }

        /// <summary>
        /// Change the TokenType of all AxisSets to t, if the currently are set to current. Otherwise do not change
        /// </summary>
        /// <param name="current">The current TokenType</param>
        /// <param name="t">The new TokenType</param>
        public void ChangeTokenType(AnimationTokenType current, AnimationTokenType t)
        {
            for (int i = 0; i < AxisTransformBlocksCount; i++)
                if (animAxisTransformBlocks[i].Type == current)
                    animAxisTransformBlocks[i].Type = t;
        }

        public void SortByTimeCode()
        {
            for (int i = 0; i < AxisTransformBlocksCount; i++)
                animAxisTransformBlocks[i].Sort();
        }

        public void ClearFrames()
        {
            ClearFrames(true, true);
        }

        public void ClearFrames(bool clearlinear, bool clearnonlinear)
        {
            for (int i = 0; i < AxisTransformBlocksCount; i++)
                animAxisTransformBlocks[i].Clear(clearlinear, clearnonlinear);
        }

        /// <summary>
        /// Return the value matching the id
        /// </summary>
        /// <param name="id">Id of the Component you want to write</param>
        /// <param name="x">returned when id=0</param>
        /// <param name="y">returned when id=1</param>
        /// <param name="z">returned when id=2</param>
        /// <returns>0, x, y, or z</returns>
        public static short GetAxisValue(int id, short x, short y, short z)
        {
            if (id == 0) return x;
            else if (id == 1) return y;
            else if (id == 2) return z;

            return 0;
        }

        public AnimationFrame GetFrameAtTimeCode(short tc)
        {
            List<AnimationFrame> frames = this.Frames;
            foreach (AnimationFrame f in frames)
            {
                if (f.TimeCode == tc) return f;
            }

            return null;
        }

        public AnimationFrame AddFrame(short tc, short x, short y, short z, bool linear)
        {
            AnimationFrame af = new AnimationFrame(tc, TransformationType);
            //af.Blocks = new AnimationAxisTransform[AxisCount];
            for (int i = 0; i < AxisTransformBlocksCount; i++)
            {
                AnimationAxisTransformBlock b = AxisTransformBlocks[i];
                AnimationAxisTransform aat = b.Add(tc, GetAxisValue(i, x, y, z), 0, 0, linear);

                if (i < 4) af.Blocks[i] = aat;
            }

            return af;
        }

        public void AddFrame(short tc, float x, float y, float z, bool linear)
        {
            for (int i = 0; i < AxisTransformBlocksCount; i++)
            {
                AnimationAxisTransformBlock b = AxisTransformBlocks[i];
                b.Add(tc, GetAxisValue(i, b.FromCompressedFloat(x), b.FromCompressedFloat(y), b.FromCompressedFloat(z)), 0, 0, linear);
            }
        }

        public void AddFrame(short tc, Vector3f v, bool linear)
        {
            AddFrame(tc, (float)v.X, (float)v.Y, (float)v.Z, linear);
        }

        public AnimationFrameBlock(AnimationMeshBlock parent)
        {
            this.parent = parent;
            datai = new uint[6];
            datai[0] = 297403888;
            datai[1] = 297403888;
            datai[3] = 297403888;
            datai[5] = 297403888;
            this.Unknown5Bits = 15;
            animAxisTransformBlocks = new List<AnimationAxisTransformBlock>();
            this.TransformationType = FrameType.Unknown;
        }

        /// <summary>
        /// Unserializes a BinaryStream into the Attributes of this Instance
        /// </summary>
        /// <param name="reader">The Stream that contains the FileData</param>
        internal void UnserializeData(DbpfReader reader)
        {
            datai[0] = reader.ReadUInt32();
            datai[1] = reader.ReadUInt32();
            datai[2] = reader.ReadUInt32(); // unknown Data
            datai[3] = reader.ReadUInt32();
            datai[4] = reader.ReadUInt32(); // contains the part3 count and unknown data
            datai[5] = reader.ReadUInt32();
        }

        /// <summary>
        /// Returns the Higest available TimeCode
        /// </summary>
        /// <returns></returns>
        public short GetDuration()
        {
            short tc = 0;
            foreach (AnimationAxisTransformBlock ab in AxisTransformBlocks) tc = Math.Max(tc, ab.LastTimeCode);

            return tc;
        }

        internal long DataSize
        {
            get
            {
                long size = datai.Length * 4;

                return size;
            }
        }

        internal void SerializeData(DbpfWriter writer)
        {
            this.SetPart3Count(animAxisTransformBlocks.Count);

            writer.WriteUInt32(datai[0]);
            writer.WriteUInt32(datai[1]);
            writer.WriteUInt32(datai[2]);
            writer.WriteUInt32(datai[3]);
            writer.WriteUInt32(datai[4]);
            writer.WriteUInt32(datai[5]);
        }

        internal void UnserializePart3Data(DbpfReader reader)
        {
            animAxisTransformBlocks = new List<AnimationAxisTransformBlock>(GetPart3Count());
            for (int i = 0; i < GetPart3Count(); i++)
            {
                AnimationAxisTransformBlock animAxisTransBlock = new AnimationAxisTransformBlock(this);
                animAxisTransBlock.UnserializeData(reader);
                animAxisTransformBlocks.Add(animAxisTransBlock);
            }
        }

        internal long Part3DataSize
        {
            get
            {
                long size = 0;

                for (int i = 0; i < animAxisTransformBlocks.Count; i++) size += animAxisTransformBlocks[i].DataSize;

                return size;
            }
        }

        internal void SerializePart3Data(DbpfWriter writer)
        {
            for (int i = 0; i < animAxisTransformBlocks.Count; i++) animAxisTransformBlocks[i].SerializeData(writer);
        }

        internal void UnserializePart3AddonData(DbpfReader reader)
        {
            for (int i = 0; i < animAxisTransformBlocks.Count; i++) animAxisTransformBlocks[i].UnserializeAddonData(reader);
        }

        internal long Part3AddonDataSize
        {
            get
            {
                long size = 0;

                for (int i = 0; i < animAxisTransformBlocks.Count; i++) size += animAxisTransformBlocks[i].AddonDataSize;

                return size;
            }
        }

        internal void SerializePart3AddonData(DbpfWriter writer)
        {
            for (int i = 0; i < animAxisTransformBlocks.Count; i++) animAxisTransformBlocks[i].SerializeAddonData(writer);
        }

        /// <summary>
        /// Returns the Number of Items for Part 3 assigned to this Object
        /// </summary>
        /// <returns>Number of Items</returns>
        int GetPart3Count()
        {
            //using highest 3-Bits xxx0 0000 0000 0000 0000 0000 0000 0000
            return ((int)datai[4] >> 0x1D) & 0x7;
        }

        /// <summary>
        /// Set the count for Part 5 Items
        /// </summary>
        /// <param name="ct">The New Count</param>
        void SetPart3Count(int ct)
        {
            if (ct > 7) ct = 7;
            ct = ct & 0x00000007;
            ct = ct << 0x1D;
            datai[4] = datai[4] & 0x1FFFFFFF;

            datai[4] = (uint)((ulong)datai[4] | (uint)ct);
        }

        #region ICloneable Member
        object System.ICloneable.Clone()
        {
            return CloneBase(true);
        }
        #endregion

        public AnimationAxisTransform InterpolateFrame(AnimationAxisTransform first, AnimationAxisTransform last, short timecode)
        {
            AnimationAxisTransform b = new AnimationAxisTransform(null, -1);

            b.TimeCode = timecode;
            b.Linear = first.Linear || last.Linear;

            if (first.TimeCode == last.TimeCode)
            {
                b.Parameter = first.Parameter;
            }
            else
            {
                //swap first and last?
                if (first.TimeCode > last.TimeCode)
                {
                    AnimationAxisTransform d = first;
                    first = last;
                    last = d;
                }

                float pos = (float)(b.TimeCode - first.TimeCode) / (float)(last.TimeCode - first.TimeCode);
                short val = (short)(((last.Parameter - first.Parameter) * pos) + first.Parameter);

                b.Parameter = val;
            }

            if (this.AxisTransformBlocksCount > 0) b.SetParent(this.AxisTransformBlocks[0]);

            return b;
        }

        void Interpolate(List<AnimationFrame> frames, int index, int blid, short maxtime)
        {
            int last = index - 1;
            int next = index + 1;

            while (last >= 0)
            {
                if (frames[last].Blocks[blid] != null) break;
                last--;
            }

            while (next < frames.Count)
            {
                if (frames[next].Blocks[blid] != null) break;
                next++;
            }

            AnimationAxisTransform lb = new AnimationAxisTransform(null, -1);
            lb.TimeCode = Math.Min((short)0, frames[index].TimeCode);
            lb.Linear = frames[index].Linear;

            //if (last<0 && next<frames.Length) last=next; //if the first Frame is missing, use the Position of the next Frame
            if (last >= 0)
            {
                lb.TimeCode = frames[last].TimeCode;
                lb.Parameter = frames[last].Blocks[blid].Parameter;
                if (frames[last].Blocks[blid].Parent != null)
                    if (lb.TimeCode == 0 && frames[last].Blocks[blid].Parent.Locked)
                        lb.Parameter = 0;
            }

            AnimationAxisTransform nb = new AnimationAxisTransform(null, -1);
            nb.TimeCode = Math.Max(maxtime, frames[index].TimeCode);
            nb.Parameter = lb.Parameter;
            nb.Linear = frames[index].Linear;


            if (next < frames.Count)
            {
                nb.TimeCode = frames[next].TimeCode;
                nb.Parameter = frames[next].Blocks[blid].Parameter;
            }


            frames[index].Blocks[blid] = InterpolateFrame(lb, nb, frames[index].TimeCode);
        }

        public override string ToString()
        {
            string s = this.Name + " (";
            if (this.TransformationType == FrameType.Translation) s += "trn";
            else s += "rot";
            //s += ", "+this.FrameCount.ToString();
            for (int i = 0; i < animAxisTransformBlocks.Count; i++)
                s += ", " + animAxisTransformBlocks[i].Count.ToString();
            s += ")";
            return s;
        }

    }

    // Part 3 - Transform
    public class AnimationAxisTransformBlock : AnimBlockBase, ICloneable, IEnumerable
    {
        #region Attributes
        AnimationFrameBlock parent;
        [Browsable(false)]
        public AnimationFrameBlock Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        uint[] datai;
        [DescriptionAttribute("Lower 16 Bits contain the count, Bit 16-17 contain the type of the assigned AddonData. Bit 18 seems to Lock the Animation"), Category("Information")]
        public uint Unknown1
        {
            get { return datai[0]; }
            set { datai[0] = value; }
        }

        [DescriptionAttribute("Setting this Bit seems to Lock the Animation. However I am not sure about this!"), Category("Information")]
        public bool Locked
        {
            get { return ((Unknown1 >> 0x12) & 1) == 1; }
            set
            {
                uint i = 0;
                if (value)
                {
                    i = 1;
                    i = i << 0x12;
                }

                Unknown1 = (Unknown1 & 0xFFFBFFFF) | i;
            }
        }

        [DescriptionAttribute("Unknown Parts of Unknown1.")]
        public uint Unknown1Bits
        {
            get
            {
                return Unknown1 >> 0x12;
            }
            set
            {
                Unknown1 = (uint)((Unknown1 & 0x0003FFFF) | ((value << 0x12) & 0xFFFC0000));
            }
        }

        public string Unknown1Binary
        {
            get
            {
                string s = Convert.ToString(Unknown1Bits, 2);
                s = MinStrLength(s, 14);
                int p = s.Length - 4;
                while (p >= 0)
                {
                    s = s.Insert(p, " ");
                    p -= 4;
                }
                return s.Trim();
            }
        }

        public string Unknown1Hex
        {
            get { return Helper.Hex8PrefixString(Unknown1Bits); }

        }

        [DescriptionAttribute("Reserved"), CategoryAttribute("Reserved"), DefaultValueAttribute(0x11BA05F0)]
        public uint Unknown2
        {
            get { return datai[1]; }
            set { datai[1] = value; }
        }

        List<AnimationAxisTransform> animAxisTransforms;

        public List<AnimationAxisTransform> AxisTransforms => animAxisTransforms;

        byte type;
        [DescriptionAttribute("Propbably some sort of Type Identifier"), CategoryAttribute("Information")]
        public AnimationTokenType Type
        {
            get { return (AnimationTokenType)type; }
            set { type = (byte)value; }
        }

        [DescriptionAttribute("The First TimeCode for this Transformation Element"), CategoryAttribute("Information")]
        public short FirstTimeCode
        {
            get
            {

                short tc = short.MaxValue;
                for (int i = 0; i < this.Count; i++)
                    tc = Math.Min(tc, GetTimeCode(i));

                if (tc == short.MaxValue) tc = 0;

                return tc;
            }
        }

        [DescriptionAttribute("The Last TimeCode for this Transformation Element"), CategoryAttribute("Information")]
        public short LastTimeCode
        {
            get
            {
                short tc = 0;
                for (int i = 0; i < this.Count; i++)
                    tc = Math.Max(tc, GetTimeCode(i));

                return tc;
            }
        }

        [DescriptionAttribute("Size (in Bytes) of one Addon Token"), CategoryAttribute("Information")]
        public byte TokenSize
        {
            get
            {
                byte size = 0;

                if (type == 0) size = 1;
                else if (type == 1) size = 3;
                else size = 4;

                return size;
            }
        }

        [DescriptionAttribute("Remaining Information stored in Unknown1"), CategoryAttribute("Information")]
        public uint AddonTokenUnknown
        {
            get
            {
                return Unknown1 >> 0x13;
            }
        }

        [DescriptionAttribute("Number of Tokens stored in the Addon Data"), CategoryAttribute("Information")]
        public int Count
        {
            get { return animAxisTransforms.Count; }
        }
        #endregion

        /// <summary>
        /// Returns the TimeCode for the indexth Frame
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public short GetTimeCode(int index)
        {
            if (index < 0 || index >= Count) return 0;
            return animAxisTransforms[index].TimeCode;
        }

        public AnimationAxisTransformBlock CloneBase()
        {
            AnimationAxisTransformBlock ab = new AnimationAxisTransformBlock(null);
            ab.datai = (uint[])this.datai.Clone();
            foreach (AnimationAxisTransform aat in animAxisTransforms)
                ab.Add(aat.CloneBase());

            ab.type = this.type;

            return ab;
        }

        public AnimationAxisTransformBlock(AnimationFrameBlock parent)
        {
            animAxisTransforms = new List<AnimationAxisTransform>();
            datai = new uint[2];
            this.parent = parent;

            this.Type = AnimationTokenType.SixByte;
            this.Unknown1Bits = 0;
        }


        internal void UnserializeData(DbpfReader reader)
        {
            datai[0] = reader.ReadUInt32();
            datai[1] = reader.ReadUInt32();
        }

        internal long DataSize
        {
            get
            {
                long size = 4 + 4;

                return size;
            }
        }

        internal void SerializeData(DbpfWriter writer)
        {
            SetCount(animAxisTransforms.Count * this.TokenSize);

            writer.WriteUInt32(datai[0]);
            writer.WriteUInt32(datai[1]);
        }

        internal void UnserializeAddonData(DbpfReader reader)
        {
            int ct = GetCount() / this.TokenSize;


            for (int i = 0; i < ct; i++)
            {
                AnimationAxisTransform aat = new AnimationAxisTransform(this, i);
                aat.UnserializeData(reader);
                animAxisTransforms.Add(aat);
            }
        }

        internal long AddonDataSize
        {
            get
            {
                long size = 0;

                for (int i = 0; i < this.Count; i++)
                    size += animAxisTransforms[i].DataSize;

                return size;
            }
        }

        internal void SerializeAddonData(DbpfWriter writer)
        {
            for (int i = 0; i < this.Count; i++)
                animAxisTransforms[i].SerializeData(writer);
        }

        public override string ToString()
        {
            string n = this.Type.ToString();
            if (n.Length > 4) n = n.Substring(0, n.Length - 4);
            string s = n + ": ";

            s += this.TokenSize.ToString() + " " + this.Unknown1Bits.ToString();
            s += " (" + Count.ToString();
            if (this.Locked) s += ", locked";
            s += ")";
            return s;
        }

        /// <summary>
        /// Return the number of Additional Word Values
        /// </summary>
        /// <returns>number of additional words to read</returns>
        int GetCount()
        {
            short dum = (short)((int)datai[0] >> 0x10);
            short count = (short)(datai[0] & 0xffff);
            type = (byte)(dum & 3);
            int size = TokenSize;

            return (count * size);
        }

        /// <summary>
        /// Set the count for Part 5 Items
        /// </summary>
        /// <param name="ct">The New Count</param>
        void SetCount(int ct)
        {
            int size = TokenSize;
            int count = ct / size;

            count = (type << 0x10) | count;

            datai[0] = (uint)(((ulong)datai[0] & 0xFFFC0000) | ((ulong)count & 0x0003FFFF));
        }

        /// <summary>
        /// Returns a List of available TimeCodes
        /// </summary>
        /// <param name="linear">true if you want to have TimeCodes for Linear KeyFrames</param>
        /// <param name="nonlinear">true if you want TimeCodes for Non Linear KeyFrames</param>		
        /// <returns>List of TimeCodes</returns>
        public List<int> GetTimeCodes(bool linear, bool nonlinear)
        {
            List<int> list = new List<int>();

            foreach (AnimationAxisTransform aat in animAxisTransforms)
                if ((aat.Linear && linear) || (!aat.Linear && nonlinear))
                    list.Add(aat.TimeCode);

            return list;
        }

        #region ICloneable Member
        object System.ICloneable.Clone()
        {
            return CloneBase();
        }
        #endregion

        #region Collection Members	
        /// <summary>
        /// Sorts the stored Frames by TimeCode
        /// </summary>
        public void Sort()
        {
            animAxisTransforms.Sort();
        }

        /// <summary>
        /// Returns the Last Frame
        /// </summary>
        /// <returns></returns>
        public AnimationAxisTransform GetLast()
        {
            if (Count == 0) return null;
            return animAxisTransforms[Count - 1];
        }

        /// <summary>
        /// Returns the First Frame
        /// </summary>
        /// <returns></returns>
        public AnimationAxisTransform GetFirst()
        {
            if (Count == 0) return null;
            return animAxisTransforms[0];
        }

        public AnimationAxisTransform BuildAnimationAxisTransform(short timecode, short param, short u1, short u2, bool islinear, int index)
        {
            AnimationAxisTransform aat = new AnimationAxisTransform(this, index);
            aat.TimeCode = timecode;
            aat.Linear = islinear;
            aat.Parameter = param;
            aat.Unknown1 = u1;
            aat.Unknown2 = u2;

            return aat;
        }

        /// <summary>
        /// Add a new <see cref="AnimationAxisTransform"/> based on a Cloned Object
        /// </summary>
        /// <param name="aat">The Item you want to Add</param>		
        /// <exception cref="AxisTransformException">
        /// Thrown, if the Item you triy to add, is a Child of another <see cref="AnimationAxisTransformBlock"/>, 
        /// or is already included in the current Listing. Before add a Frame, you have to create a Clone!
        /// </exception>
        public void Add(AnimationAxisTransform aat)
        {
            if ((aat.Parent != this && aat.Parent != null) || aat.Index != -1)
                throw new AxisTransformException("Can't add the passed AnimationAxisTransform!");
            aat.SetIndex(animAxisTransforms.Count);
            aat.SetParent(this);
            animAxisTransforms.Add(aat);
        }
        public bool ContainsTimeCode(short timecode)
        {
            foreach (AnimationAxisTransform a in animAxisTransforms)
                if (a.TimeCode == timecode)
                    return true;

            return false;
        }

        /// <summary>
        /// Add a new <see cref="AnimationAxisTransform"/> Item
        /// </summary>
        /// <param name="timecode"></param>
        /// <param name="param"></param>
        /// <param name="u1"></param>
        /// <param name="u2"></param>
        /// <param name="islinear"></param>
        /// <remarks>The Data does not get added when the timecode already exists, null will be returned in that case</remarks>
        public AnimationAxisTransform Add(short timecode, short param, short u1, short u2, bool islinear)
        {
            AnimationAxisTransform aat = BuildAnimationAxisTransform(timecode, param, u1, u2, islinear, animAxisTransforms.Count);
            if (ContainsTimeCode(timecode)) return null;

            animAxisTransforms.Add(aat);
            return aat;
        }

        /// <summary>
        /// Insert a new <see cref="AnimationAxisTransform"/> based on a Cloned Object
        /// </summary>
        /// <param name="index">The index within the List</param>
        /// <param name="aat">The Item you want to Add</param>	
        /// <exception cref="AxisTransformException">
        /// Thrown, if the Item you triy to add, is a Child of another <see cref="AnimationAxisTransformBlock"/>, 
        /// or is already included in the current Listing. Before add a Frame, you have to create a Clone!
        /// </exception>
        public void Insert(int index, AnimationAxisTransform aat)
        {
            if ((aat.Parent != this && aat.Parent != null) || aat.Index != -1)
                throw new AxisTransformException("Can't add the passed AnimationAxisTransform!");

            aat.SetIndex(index);
            aat.SetParent(this);
            animAxisTransforms.Insert(index, aat);
            ReIndex(index + 1);
        }

        /// <summary>
        /// Insert a new <see cref="AnimationAxisTransform"/> Item
        /// </summary>
        /// <param name="index">The index within the List</param>
        /// <param name="timecode"></param>
        /// <param name="param"></param>
        /// <param name="u1"></param>
        /// <param name="u2"></param>
        /// <param name="islinear"></param>
        public void Insert(int index, short timecode, short param, short u1, short u2, bool islinear)
        {
            animAxisTransforms.Insert(index, BuildAnimationAxisTransform(timecode, param, u1, u2, islinear, index));
            ReIndex(index + 1);
        }

        /// <summary>
        /// Remove all Data Stored here
        /// </summary>
        /// <param name="clearlinear">true if you want to clear linear transformations</param>
        /// <param name="clearnonlinear">true if you want to clear non Linear KeyFrames</param>
        public void Clear(bool clearlinear, bool clearnonlinear)
        {
            if (clearlinear && clearnonlinear) this.Clear();
            else
            {
                List<AnimationAxisTransform> list = new List<AnimationAxisTransform>();

                foreach (AnimationAxisTransform aat in animAxisTransforms)
                {
                    if (aat.Linear && !clearlinear) list.Add(aat);

                    if (!aat.Linear && !clearnonlinear) list.Add(aat);
                }

                animAxisTransforms.Clear();
                animAxisTransforms = list;
                ReIndex();
            }
        }

        /// <summary>
        /// Clear al stored Transformations
        /// </summary>
        public void Clear()
        {
            for (int i = animAxisTransforms.Count - 1; i >= 0; i--)
                animAxisTransforms[i].Dispose();

            animAxisTransforms.Clear();
        }

        /// <summary>
        /// Remove the passed Item from the Parent
        /// </summary>
        /// <param name="aat"></param>
        public void Remove(AnimationAxisTransform aat)
        {
            int ct = animAxisTransforms.Count;
            animAxisTransforms.Remove(aat);
            ReIndex();

            if (ct != animAxisTransforms.Count)
            {
                aat.SetParent(null);
                aat.SetIndex(-1);
            }
        }

        /// <summary>
        /// Make sure the indices stored in the items Elements are in sync
        /// </summary>
        protected void ReIndex()
        {
            ReIndex(0);
        }

        /// <summary>
        /// Make sure the indices stored in the items Elements are in sync
        /// </summary>
        /// <param name="start">the first Block you want to check</param>
        protected void ReIndex(int start)
        {
            start = Math.Max(0, start);
            for (int i = start; i < animAxisTransforms.Count; i++) animAxisTransforms[i].SetIndex(i);
        }
        #endregion

        #region IEnumerable Member
        public IEnumerator GetEnumerator()
        {
            return animAxisTransforms.GetEnumerator();
        }
        #endregion

        #region Float Converters
        public float GetScale()
        {
            FrameType ft = FrameType.Translation;
            if (parent != null)
                ft = parent.TransformationType;

            return GetScale(ft);
        }

        public float GetCompressedFloat(short val)
        {
            return GetCompressedFloat(val, GetScale());
        }

        public short FromCompressedFloat(float val)
        {
            return FromCompressedFloat(val, GetScale());
        }

        public float GetScale(FrameType ft)
        {
            return GetScale(Locked, ft);
        }

        public static float GetScale(bool locked, FrameType ft)
        {
            float scale = SCALE;
            if (!locked) scale = scale * 16f;
            if (ft == FrameType.Rotation)
                scale = SCALEROT;


            return scale;
        }

        public float GetCompressedFloat(short val, FrameType ft)
        {
            return GetCompressedFloat(val, GetScale(ft));
        }

        public short FromCompressedFloat(float val, FrameType ft)
        {
            return FromCompressedFloat(val, GetScale(ft));
        }

        #region statics
        //public const float SCALE = 6.25f/1000f;//10/(float)short.MaxValue;
        public const float SCALE = 1.0f / 1000f;
        public const float SCALEROT = (float)(((1f / 180f) * Math.PI) / 64f);

        public static float GetCompressedFloat(short v, float scale)
        {
            //if (scale==SCALEROT) 
            return ((float)v * scale);
            //return ((float)((v - 7.33333) * 0.003));
            //
        }

        public static short FromCompressedFloat(float v, float scale)
        {
            return (short)(v / scale);
        }
        #endregion
        #endregion
    }

    // Part 4 - ???
    public class AnimBlock4 : AnimBlockBase
    {
        #region Attributes		
        List<AnimBlock5> ab5;
        public List<AnimBlock5> Part5
        {
            get { return ab5; }
        }
        [DescriptionAttribute("Number of loaded AnimBlock4 Items"), CategoryAttribute("Information")]
        public int Part5Count
        {
            get { return ab5.Count; }
        }

        uint[] datai;
        [DescriptionAttribute("Reserved"), CategoryAttribute("Reserved"), DefaultValueAttribute(0x11BA05F0)]
        public uint Unknown1
        {
            get { return datai[0]; }
            set { datai[0] = value; }
        }
        [DescriptionAttribute("Reserved"), CategoryAttribute("Reserved"), DefaultValueAttribute(0x11BA05F0)]
        public uint Unknown2
        {
            get { return datai[1]; }
            set { datai[1] = value; }
        }
        [DescriptionAttribute("Reserved"), CategoryAttribute("Reserved"), DefaultValueAttribute(0x11BA05F0)]
        public uint Unknown3
        {
            get { return datai[2]; }
            set { datai[2] = value; }
        }

        byte[] data;
        [DescriptionAttribute("On Index 2 the Number of assigned AnimBlock5 Items is stored")]
        public byte[] AddonData
        {
            get { return data; }
        }
        #endregion

        internal AnimBlock4()
        {
            datai = new uint[3];
            data = new byte[0x3A];
            ab5 = new List<AnimBlock5>();
        }

        internal void UnserializeData(DbpfReader reader)
        {
            long pos = reader.Position;
            if (reader.Length - pos < 4 + 4 + data.Length + 4) return;

            datai[0] = reader.ReadUInt32();
            datai[1] = reader.ReadUInt32();

            data = reader.ReadBytes(data.Length);

            datai[2] = reader.ReadUInt32();

            if (datai[2] != datai[1])
            {
                reader.Seek(System.IO.SeekOrigin.Begin, pos);
                return;
            }
        }

        internal long DataSize
        {
            get
            {
                long size = 4 + 4;

                size += data.Length;

                size += 4;

                return size;
            }
        }

        internal void SerializeData(DbpfWriter writer)
        {
            this.SetPart5Count(ab5.Count);

            writer.WriteUInt32(datai[0]);
            writer.WriteUInt32(datai[1]);

            writer.WriteBytes(data);

            writer.WriteUInt32(datai[2]);
        }

        internal void UnserializePart5Data(DbpfReader reader)
        {
            ab5 = new List<AnimBlock5>(GetPart5Count());
            for (int i = 0; i < GetPart5Count(); i++)
            {
                AnimBlock5 animBlock5 = new AnimBlock5();
                animBlock5.UnserializeData(reader);
                ab5.Add(animBlock5);
            }
        }

        internal long Part5DataSize
        {
            get
            {
                long size = 0;

                for (int i = 0; i < ab5.Count; i++) size += ab5[i].DataSize;

                return size;
            }
        }

        internal void SerializePart5Data(DbpfWriter writer)
        {
            for (int i = 0; i < ab5.Count; i++) ab5[i].SerializeData(writer);
        }

        /// <summary>
        /// Returns the Number of Items for Part 5 assigned to this Object
        /// </summary>
        /// <returns>Number of Items</returns>
        int GetPart5Count()
        {
            return (data[2]);
        }

        /// <summary>
        /// Set the count for Part 5 Items
        /// </summary>
        /// <param name="ct">The New Count</param>
        void SetPart5Count(int ct)
        {
            if (ct > 0xff) ct = 0xff;
            data[2] = (byte)(ct & 0xff);
        }

        public override string ToString()
        {
            return "AnimBlock4: " + this.Part5Count.ToString() + " " + this.AddonData.Length.ToString();
        }
    }

    // Part 5 - ???
    public class AnimBlock5 : AnimBlockBase
    {
        #region Attributes
        uint[] datai;
        [DescriptionAttribute("Reserved"), CategoryAttribute("Reserved"), DefaultValueAttribute(0x11BA05F0)]
        public uint Unknown1
        {
            get { return datai[0]; }
            set { datai[0] = value; }
        }

        public uint Unknown2
        {
            get { return datai[1]; }
            set { datai[1] = value; }
        }

        public string Unknown2Binary
        {
            get
            {
                string s = Convert.ToString(Unknown2, 2);
                s = MinStrLength(s, 14);
                int p = s.Length - 4;
                while (p >= 0)
                {
                    s = s.Insert(p, " ");
                    p -= 4;
                }
                return s.Trim();
            }

        }

        public string Unknown2Hex
        {
            get { return Helper.Hex8PrefixString(Unknown2); }

        }

        byte[] data;
        public byte[] AddonData
        {
            get { return data; }
        }
        #endregion

        internal AnimBlock5()
        {
            datai = new uint[2];
            data = new byte[0x23];
        }

        internal void UnserializeData(DbpfReader reader)
        {
            datai[0] = reader.ReadUInt32();
            datai[1] = reader.ReadUInt32();

            data = reader.ReadBytes(data.Length);
        }

        internal long DataSize
        {
            get
            {
                long size = 4 + 4;

                size += data.Length;

                return size;
            }
        }

        internal void SerializeData(DbpfWriter writer)
        {
            writer.WriteUInt32(datai[0]);
            writer.WriteUInt32(datai[1]);

            writer.WriteBytes(data);
        }

        public override string ToString()
        {
            return Helper.Hex8PrefixString(Unknown2) + " " + this.AddonData.Length.ToString();
        }
    }

    // Part 6 - ???
    public class AnimBlock6 : AnimBlock
    {
        #region Attributes
        uint[] datai;
        [DescriptionAttribute("Reserved"), CategoryAttribute("Reserved"), DefaultValueAttribute(0x11BA05F0)]
        public uint Unknown1
        {
            get { return datai[0]; }
            set { datai[0] = value; }
        }
        [DescriptionAttribute("Reserved"), CategoryAttribute("Reserved"), DefaultValueAttribute(0x11BA05F0)]
        public uint Unknown2
        {
            get { return datai[1]; }
            set { datai[1] = value; }
        }

        short[] datas;
        public short SUnknown1
        {
            get { return datas[0]; }
            set { datas[0] = value; }
        }
        public short SUnknown2
        {
            get { return datas[1]; }
            set { datas[1] = value; }
        }
        public short SUnknown3
        {
            get { return datas[2]; }
            set { datas[2] = value; }
        }
        #endregion

        internal AnimBlock6()
        {
            datai = new uint[2];
            datas = new short[3];
        }

        internal void UnserializeData(DbpfReader reader)
        {
            datai[0] = reader.ReadUInt32();

            datas[0] = reader.ReadInt16();
            datas[1] = reader.ReadInt16();
            datas[2] = reader.ReadInt16();

            datai[1] = reader.ReadUInt32();
        }

        internal long DataSize
        {
            get
            {
                long size = 4;

                size += 2 + 2 + 2;

                size += 4;

                return size;
            }
        }

        internal void SerializeData(DbpfWriter writer)
        {
            writer.WriteUInt32(datai[0]);

            writer.WriteInt16(datas[0]);
            writer.WriteInt16(datas[1]);
            writer.WriteInt16(datas[2]);

            writer.WriteUInt32(datai[1]);
        }
    }

    public class AnimationAxisTransform : IDisposable, ICloneable, IComparable
    {
        #region Attributes
        int index;
        public int Index
        {
            get { return index; }
        }

        AnimationAxisTransformBlock parent;
        public AnimationAxisTransformBlock Parent
        {
            get { return parent; }
        }

        ushort tc;
        public short TimeCode
        {
            get
            {
                return (short)(tc & 0x7fff);
            }
            set
            {
                tc = (ushort)((tc & 0x8000) | (ushort)(value & 0x7fff));
            }
        }

        /// <summary>
        /// Use this KeyFrame as a Linear Pole?
        /// </summary>
        [Description("Use this KeyFrame as a Linear Pole."), Category("Information")]
        public bool Linear
        {
            get
            {
                return ((tc & 0x8000) == 0x8000);
            }
            set
            {
                tc = (ushort)(tc & 0x7fff);
                if (value) tc = (ushort)(tc | 0x8000);
            }
        }

        public bool ParentLocked
        {
            get
            {
                if (parent == null) return true;
                return parent.Locked;
            }
            set
            {
                parent.Locked = value;
            }
        }

        short param;
        public short Parameter
        {
            get { return param; }
            set { param = value; }
        }

        public float ParameterFloat
        {
            get { return this.GetCompressedFloat(Parameter); }
            set { Parameter = this.FromCompressedFloat(value); }
        }

        short u1;
        public short Unknown1
        {
            get { return u1; }
            set { u1 = value; }
        }

        short u2;
        public short Unknown2
        {
            get { return u2; }
            set { u2 = value; }
        }
        #endregion

        /// <summary>
        /// Create a new Instance. 
        /// </summary>
        /// <param name="parent">The parent Block</param>
        /// <remarks>
        /// Instances are only valid in the context of a <see cref="AnimationAxisTransformBlock"/>!
        /// </remarks>
        internal AnimationAxisTransform(AnimationAxisTransformBlock parent, int index)
        {
            SetIndex(index);
            SetParent(parent);

            Reset();
        }

        public AnimationAxisTransform() : this(null, -1) { }

        public AnimationAxisTransform(AnimationAxisTransformBlock parent) : this(parent, -1)
        {
        }

        public AnimationAxisTransform CloneBase()
        {
            AnimationAxisTransform aat = new AnimationAxisTransform(null, -1);
            aat.Linear = this.Linear;
            aat.TimeCode = this.TimeCode;
            aat.Parameter = this.Parameter;
            aat.Unknown1 = this.Unknown1;
            aat.Unknown2 = this.Unknown2;

            return aat;
        }

        internal void SetIndex(int index)
        {
            this.index = index;
        }

        internal void SetParent(AnimationAxisTransformBlock parent)
        {
            this.parent = parent;
        }

        /// <summary>
        /// Reset the stored Values
        /// </summary>
        public void Reset()
        {
            tc = 0;
            param = 0;
            u1 = 0;
            u2 = 0;
        }

        internal void UnserializeData(DbpfReader reader)
        {
            Reset();
            short[] datas = new short[parent.TokenSize];
            for (int i = 0; i < datas.Length; i++) datas[i] = reader.ReadInt16();

            if (parent.Type == AnimationTokenType.TwoByte)
            {
                param = datas[0];
            }
            else if (parent.Type == AnimationTokenType.SixByte)
            {
                tc = (ushort)datas[0];
                param = datas[1];
                u1 = datas[2];
            }
            else
            {
                tc = (ushort)datas[0];
                param = datas[1];
                u1 = datas[2];
                u2 = datas[3];
            }
        }

        internal long DataSize
        {
            get
            {
                long size = parent.TokenSize * 2;

                return size;
            }
        }

        internal void SerializeData(DbpfWriter writer)
        {
            short[] datas = new short[parent.TokenSize];

            if (parent.Type == AnimationTokenType.TwoByte)
            {
                datas[0] = param;
            }
            else if (parent.Type == AnimationTokenType.SixByte)
            {
                datas[0] = (short)tc;
                datas[1] = param;
                datas[2] = u1;
            }
            else
            {
                datas[0] = (short)tc;
                datas[1] = param;
                datas[2] = u1;
                datas[3] = u2;
            }

            for (int i = 0; i < datas.Length; i++) writer.WriteInt16(datas[i]);
        }


        #region IDisposable Member
        public void Dispose()
        {
            if (parent != null)
            {
                parent = null;
            }
        }
        #endregion

        #region ICloneable Member
        public object Clone()
        {
            return this.CloneBase();
        }
        #endregion

        public override string ToString()
        {
            string s = TimeCode.ToString() + ": " + Parameter.ToString();
            if (parent == null)
            {
                s += ", " + Unknown1.ToString() + "; " + Unknown2.ToString();
            }
            else
            {
                if (parent.Type == AnimationTokenType.SixByte) s += "; " + Unknown1.ToString();
                if (parent.Type == AnimationTokenType.EightByte) s += "; " + Unknown1.ToString() + "; " + Unknown2.ToString();
            }
            if (Linear) s += " (linear)";
            if (ParentLocked) s += " (locked)";
            return s;
        }

        #region IComparable Member
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            if (!(obj is AnimationAxisTransform)) return -1;

            AnimationAxisTransform aat = (AnimationAxisTransform)obj;
            return this.TimeCode.CompareTo(aat.TimeCode);
        }
        #endregion

        #region Float Converters
        public float GetCompressedFloat(short val)
        {
            if (parent != null) return parent.GetCompressedFloat(val);
            return AnimationAxisTransformBlock.GetCompressedFloat(val, AnimationAxisTransformBlock.SCALE);
        }

        public short FromCompressedFloat(float val)
        {
            if (parent != null) return parent.FromCompressedFloat(val);
            return AnimationAxisTransformBlock.FromCompressedFloat(val, AnimationAxisTransformBlock.SCALE);
        }

        #endregion
    }

    public class AxisTransformException : Exception
    {
        public AxisTransformException(string message) : base(message) { }
    }
}
