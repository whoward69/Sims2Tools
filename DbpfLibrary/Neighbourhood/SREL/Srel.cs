﻿/*
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
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.Utils;
using System;
using System.Xml;

namespace Sims2Tools.DBPF.Neighbourhood.SREL
{
    public class RelationshipFlags : FlagBase
    {
        public RelationshipFlags(ushort flags) : base(flags) { }

        public bool IsEnemy => GetBit((byte)RelationshipStateBits.Enemy);

        public bool IsFriend => GetBit((byte)RelationshipStateBits.Friends);

        public bool IsBuddie => GetBit((byte)RelationshipStateBits.Buddies);

        public bool HasCrush => GetBit((byte)RelationshipStateBits.Crush);

        public bool InLove => GetBit((byte)RelationshipStateBits.Love);

        public bool GoSteady => GetBit((byte)RelationshipStateBits.Steady);

        public bool IsEngaged => GetBit((byte)RelationshipStateBits.Engaged);

        public bool IsMarried => GetBit((byte)RelationshipStateBits.Married);

        public bool IsContact => GetBit((byte)RelationshipStateBits.Contact);

        public bool IsPackMember => GetBit((byte)RelationshipStateBits.Pack);

        public bool IsMaster => GetBit((byte)RelationshipStateBits.Master);

        public bool IsMine => GetBit((byte)RelationshipStateBits.Mine);

        public bool IsPetFriend => GetBit((byte)RelationshipStateBits.PetFriend);

        public bool IsPetBuddies => GetBit((byte)RelationshipStateBits.PetBuddies);

        public bool IsFamilyMember => GetBit((byte)RelationshipStateBits.Family);

        public bool IsKnown => GetBit((byte)RelationshipStateBits.Known);
    }

    public class UIFlags2 : FlagBase
    {
        public UIFlags2(ushort flags) : base(flags) { }

        public bool IsBFF
        {
            get { return GetBit((byte)UIFlags2Names.BestFriendForever); }
        }
    }

    public class Srel : DBPFResource
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0xCC364C2A;
        public const string NAME = "SREL";

        private int[] values = new int[4];

        public int Shortterm
        {
            get { return GetValue(0); }
        }

        readonly RelationshipFlags flags = new RelationshipFlags(1 << (byte)RelationshipStateBits.Known);
        public RelationshipFlags RelationState
        {
            get { return flags; }
        }

        public int Longterm
        {
            get { return GetValue(2); }
        }

        public RelationshipTypes FamilyRelation
        {
            get { return (RelationshipTypes)GetValue(3); }
        }

        readonly UIFlags2 flags2 = new UIFlags2(0);
        public UIFlags2 RelationState2
        {
            get { return flags2; }
        }


        protected int GetValue(int slot)
        {
            if (values.Length > slot) return values[slot];
            else return 0;
        }

        public Srel(DBPFEntry entry, DbpfReader reader) : base(entry)
        {
            Unserialize(reader);
        }

        protected void Unserialize(DbpfReader reader)
        {
            if (reader.Length <= 0) return;

            _ = reader.ReadUInt32();          //unknown
            uint stored = reader.ReadUInt32();
            values = new int[Math.Max(3, stored)];
            for (int i = 0; i < stored; i++) values[i] = reader.ReadInt32();

            //set some special Attributes
            flags.Value = (ushort)values[1];
            if (9 < values.Length) flags2.Value = (ushort)values[9];
        }

        public override XmlElement AddXml(XmlElement parent)
        {
            XmlElement element = XmlHelper.CreateElement(parent, NAME.ToLower());

            element.SetAttribute("simId", $"0x0000{InstanceID.Hex8String().Substring(0, 4)}");
            element.SetAttribute("towardsId", $"0x0000{InstanceID.Hex8String().Substring(4, 4)}");

            element.SetAttribute("str", Shortterm.ToString());
            element.SetAttribute("ltr", Longterm.ToString());

            if (RelationState.GoSteady) element.SetAttribute("steady", "true");
            if (RelationState.HasCrush) element.SetAttribute("crush", "true");
            if (RelationState.InLove) element.SetAttribute("love", "true");
            if (RelationState.IsBuddie) element.SetAttribute("buddie", "true"); // While it is tempting to change this to 'buddy' there are transforms that would disagree!
            if (RelationState.IsEnemy) element.SetAttribute("enemy", "true");
            if (RelationState.IsEngaged) element.SetAttribute("engaged", "true");
            if (RelationState.IsFamilyMember) element.SetAttribute("family", "true");
            if (RelationState.IsFriend) element.SetAttribute("friend", "true");
            if (RelationState.IsKnown) element.SetAttribute("known", "true");
            if (RelationState.IsMarried) element.SetAttribute("married", "true");
            if (RelationState.IsContact) element.SetAttribute("contact", "true");
            if (RelationState.IsPackMember) element.SetAttribute("pack", "true");
            if (RelationState.IsMaster) element.SetAttribute("master", "true");
            if (RelationState.IsMine) element.SetAttribute("mine", "true");
            if (RelationState.IsPetFriend) element.SetAttribute("petFriend", "true");
            if (RelationState.IsPetBuddies) element.SetAttribute("petBuddie", "true");
            if (RelationState2.IsBFF) element.SetAttribute("bff", "true");

            if (RelationState.IsFamilyMember) element.SetAttribute("relFamily", FamilyRelation.ToString());

            return element;
        }
    }
}
