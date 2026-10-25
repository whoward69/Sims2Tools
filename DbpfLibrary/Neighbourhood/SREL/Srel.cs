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
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.Utils;
using System;
using System.Xml;

namespace Sims2Tools.DBPF.Neighbourhood.SREL
{
    public enum KnownRelationship : int
    {
        Known,
        Contact,
        NotKnown
    }

    public enum FamilyRelationship : int
    {
        Spouse,
        Parent,
        Child,
        Sibling,
        Grandparent,
        Grandchild,
        AuntOrUncle,
        NieceOrNephew,
        Cousin,
        Family,
        None
    }

    public enum FriendRelationship : int
    {
        Friend,
        Buddy,
        None
    }

    public enum RomanceRelationship : int
    {
        Crush,
        Love,
        None
    }

    public enum CommitmentRelationship : int
    {
        Steady,
        Engaged,
        Married,
        None
    }

    public enum PetFriendRelationship : int
    {
        Friend,
        Buddy,
        None
    }

    public class RelationshipFlags : FlagBase
    {
        public RelationshipFlags(ushort flags) : base(flags) { }

        public bool IsKnown => GetBit((byte)RelationshipStateBits.Known);
        public bool IsContact => GetBit((byte)RelationshipStateBits.Contact);
        public bool IsFamilyMember => GetBit((byte)RelationshipStateBits.Family);

        public bool IsEnemy => GetBit((byte)RelationshipStateBits.Enemy);
        public bool IsFriend => GetBit((byte)RelationshipStateBits.Friends);
        public bool IsBuddy => GetBit((byte)RelationshipStateBits.Buddies);

        public bool HasCrush => GetBit((byte)RelationshipStateBits.Crush);
        public bool InLove => GetBit((byte)RelationshipStateBits.Love);

        public bool GoSteady => GetBit((byte)RelationshipStateBits.Steady);
        public bool IsEngaged => GetBit((byte)RelationshipStateBits.Engaged);
        public bool IsMarried => GetBit((byte)RelationshipStateBits.Married);


        public bool IsPetFriend => GetBit((byte)RelationshipStateBits.PetFriend);
        public bool IsPetBuddy => GetBit((byte)RelationshipStateBits.PetBuddies);

        public bool IsMaster => GetBit((byte)RelationshipStateBits.Master);
        public bool IsPackMember => GetBit((byte)RelationshipStateBits.Pack);
        public bool IsMine => GetBit((byte)RelationshipStateBits.Mine);


        public void SetEnemy(bool val)
        {
            SetBit((byte)RelationshipStateBits.Enemy, val);
            // TODO - Family Manager - Relationships Tab - should this also clear "friend" and "buddy" and "BFF"?
        }

        public void SetFriends(FriendRelationship rel)
        {
            SetBit((byte)RelationshipStateBits.Friends, false);
            SetBit((byte)RelationshipStateBits.Buddies, false);

            switch (rel)
            {
                case FriendRelationship.Friend:
                    SetBit((byte)RelationshipStateBits.Friends, true);
                    break;
                case FriendRelationship.Buddy:
                    SetBit((byte)RelationshipStateBits.Friends, true);
                    SetBit((byte)RelationshipStateBits.Buddies, true);
                    break;
            }
        }

        public void SetRomance(RomanceRelationship rel)
        {
            SetBit((byte)RelationshipStateBits.Crush, false);
            SetBit((byte)RelationshipStateBits.Love, false);

            switch (rel)
            {
                case RomanceRelationship.Crush:
                    SetBit((byte)RelationshipStateBits.Crush, true);
                    break;
                case RomanceRelationship.Love:
                    SetBit((byte)RelationshipStateBits.Crush, true);
                    SetBit((byte)RelationshipStateBits.Love, true);
                    break;
            }
        }

        public void SetCommitment(CommitmentRelationship rel)
        {
            SetBit((byte)RelationshipStateBits.Steady, false);
            SetBit((byte)RelationshipStateBits.Engaged, false);
            SetBit((byte)RelationshipStateBits.Married, false);

            switch (rel)
            {
                case CommitmentRelationship.Steady:
                    SetBit((byte)RelationshipStateBits.Steady, true);
                    break;
                case CommitmentRelationship.Engaged:
                    SetBit((byte)RelationshipStateBits.Engaged, true);
                    break;
                case CommitmentRelationship.Married:
                    SetBit((byte)RelationshipStateBits.Married, true);
                    break;
            }
        }
    }

    public class UIFlags2 : FlagBase
    {
        public UIFlags2(ushort flags) : base(flags) { }

        public bool IsBFF => GetBit((byte)RelationshipState2Bits.BestFriendForever);
    }

    public class Srel : DBPFResource
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0xCC364C2A;
        public const string NAME = "SREL";

        private int[] values = new int[4];

        public TypeInstanceID SimID => (TypeInstanceID)((this.InstanceID.AsUInt() & 0xFFFF0000) >> 16);
        public TypeInstanceID TowardsID => (TypeInstanceID)(this.InstanceID.AsUInt() & 0x0000FFFF);

        public int STR => GetValue(0);
        public int LTR => GetValue(2);

        private readonly RelationshipFlags relationshipFlags = new RelationshipFlags(1 << (byte)RelationshipStateBits.Known);
        public RelationshipFlags RelationshipFlags => relationshipFlags;
        readonly UIFlags2 relationshipFlags2 = new UIFlags2(0);
        public UIFlags2 RelationshipFlags2 => relationshipFlags2;

        [Obsolete("This property has typos, use FamilyRelationship instead", false)]
        private RelationshipTypes FamilyRelation => (RelationshipTypes)GetValue(3);

        public FamilyRelationship FamilyRelationship
        {
            get
            {
#pragma warning disable CS0618 // Type or member is obsolete
                switch (GetValue(3))
                {
                    case (int)RelationshipTypes.Parent:
                        return FamilyRelationship.Parent;
                    case (int)RelationshipTypes.Child:
                        return FamilyRelationship.Child;
                    case (int)RelationshipTypes.Sibling:
                        return FamilyRelationship.Sibling;
                    case (int)RelationshipTypes.Gradparent:
                        return FamilyRelationship.Grandparent;
                    case (int)RelationshipTypes.Grandchild:
                        return FamilyRelationship.Grandchild;
                    case (int)RelationshipTypes.Nice_Nephew:
                        return FamilyRelationship.NieceOrNephew;
                    case (int)RelationshipTypes.Aunt:
                        return FamilyRelationship.AuntOrUncle;
                    case (int)RelationshipTypes.Cousin:
                        return FamilyRelationship.Cousin;
                    case (int)RelationshipTypes.Spouses:
                        return FamilyRelationship.Spouse;
                }
#pragma warning restore CS0618 // Type or member is obsolete

                return RelationshipFlags.IsFamilyMember ? FamilyRelationship.Family : FamilyRelationship.None;
            }
        }

        public KnownRelationship KnownRelationship
        {
            get
            {
                if (RelationshipFlags.IsContact) return KnownRelationship.Contact;
                if (RelationshipFlags.IsKnown) return KnownRelationship.Known;

                return KnownRelationship.NotKnown;
            }
        }

        public FriendRelationship FriendRelationship
        {
            get
            {
                if (RelationshipFlags.IsBuddy) return FriendRelationship.Buddy;
                if (RelationshipFlags.IsFriend) return FriendRelationship.Friend;

                if (RelationshipFlags.IsPetBuddy) return FriendRelationship.Buddy;
                if (RelationshipFlags.IsPetFriend) return FriendRelationship.Friend;

                return FriendRelationship.None;
            }
        }

        public RomanceRelationship RomanceRelationship
        {
            get
            {
                if (RelationshipFlags.InLove) return RomanceRelationship.Love;
                if (RelationshipFlags.HasCrush) return RomanceRelationship.Crush;

                return RomanceRelationship.None;
            }
        }

        public CommitmentRelationship CommitmentRelationship
        {
            get
            {
                if (RelationshipFlags.IsMarried) return CommitmentRelationship.Married;
                if (RelationshipFlags.IsEngaged) return CommitmentRelationship.Engaged;
                if (RelationshipFlags.GoSteady) return CommitmentRelationship.Steady;

                return CommitmentRelationship.None;
            }
        }

        public PetFriendRelationship PetFriendRelationship
        {
            get
            {
                if (RelationshipFlags.IsPetBuddy) return PetFriendRelationship.Buddy;
                if (RelationshipFlags.IsPetFriend) return PetFriendRelationship.Friend;

                return PetFriendRelationship.None;
            }
        }

        public Srel(DBPFEntry entry, DbpfReader reader) : base(entry)
        {
            Unserialize(reader);
        }

        protected int GetValue(int slot)
        {
            return (values.Length > slot) ? values[slot] : 0;
        }

        protected void Unserialize(DbpfReader reader)
        {
            if (reader.Length <= 0) return;

            _ = reader.ReadUInt32();          //unknown
            uint stored = reader.ReadUInt32();
            values = new int[Math.Max(3, stored)];
            for (int i = 0; i < stored; i++) values[i] = reader.ReadInt32();

            //set some special Attributes
            relationshipFlags.Value = (ushort)values[1];
            if (9 < values.Length) relationshipFlags2.Value = (ushort)values[9];
        }

        public override XmlElement AddXml(XmlElement parent)
        {
            XmlElement element = XmlHelper.CreateElement(parent, NAME.ToLower());

            element.SetAttribute("simId", $"0x0000{InstanceID.Hex8String().Substring(0, 4)}");
            element.SetAttribute("towardsId", $"0x0000{InstanceID.Hex8String().Substring(4, 4)}");

            element.SetAttribute("str", STR.ToString());
            element.SetAttribute("ltr", LTR.ToString());

            if (RelationshipFlags.GoSteady) element.SetAttribute("steady", "true");
            if (RelationshipFlags.HasCrush) element.SetAttribute("crush", "true");
            if (RelationshipFlags.InLove) element.SetAttribute("love", "true");
            if (RelationshipFlags.IsBuddy) element.SetAttribute("buddie", "true"); // While it is tempting to change this to 'buddy' there are transforms that would disagree!
            if (RelationshipFlags.IsEnemy) element.SetAttribute("enemy", "true");
            if (RelationshipFlags.IsEngaged) element.SetAttribute("engaged", "true");
            if (RelationshipFlags.IsFamilyMember) element.SetAttribute("family", "true");
            if (RelationshipFlags.IsFriend) element.SetAttribute("friend", "true");
            if (RelationshipFlags.IsKnown) element.SetAttribute("known", "true");
            if (RelationshipFlags.IsMarried) element.SetAttribute("married", "true");
            if (RelationshipFlags.IsContact) element.SetAttribute("contact", "true");
            if (RelationshipFlags.IsPackMember) element.SetAttribute("pack", "true");
            if (RelationshipFlags.IsMaster) element.SetAttribute("master", "true");
            if (RelationshipFlags.IsMine) element.SetAttribute("mine", "true");
            if (RelationshipFlags.IsPetFriend) element.SetAttribute("petFriend", "true");
            if (RelationshipFlags.IsPetBuddy) element.SetAttribute("petBuddie", "true");
            if (RelationshipFlags2.IsBFF) element.SetAttribute("bff", "true");

#pragma warning disable CS0618 // Various XSLTs rely on the typos!
            if (RelationshipFlags.IsFamilyMember) element.SetAttribute("relFamily", FamilyRelation.ToString());
#pragma warning restore CS0618

            return element;
        }
    }
}
