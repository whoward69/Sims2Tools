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

using Sims2Tools.DBPF.BCON;
using Sims2Tools.DBPF.BHAV;
using Sims2Tools.DBPF.Cigen.CGN1;
using Sims2Tools.DBPF.CLST;
using Sims2Tools.DBPF.CTSS;
using Sims2Tools.DBPF.GLOB;
using Sims2Tools.DBPF.Groups.GROP;
using Sims2Tools.DBPF.Images.IMG;
using Sims2Tools.DBPF.Images.JPG;
using Sims2Tools.DBPF.Images.THUB;
using Sims2Tools.DBPF.Neighbourhood.BNFO;
using Sims2Tools.DBPF.Neighbourhood.FAMI;
using Sims2Tools.DBPF.Neighbourhood.FAMT;
using Sims2Tools.DBPF.Neighbourhood.IDNO;
using Sims2Tools.DBPF.Neighbourhood.LTXT;
using Sims2Tools.DBPF.Neighbourhood.NGBH;
using Sims2Tools.DBPF.Neighbourhood.SDNA;
using Sims2Tools.DBPF.Neighbourhood.SDSC;
using Sims2Tools.DBPF.Neighbourhood.SREL;
using Sims2Tools.DBPF.Neighbourhood.SWAF;
using Sims2Tools.DBPF.NREF;
using Sims2Tools.DBPF.OBJD;
using Sims2Tools.DBPF.OBJF;
using Sims2Tools.DBPF.SceneGraph.AGED;
using Sims2Tools.DBPF.SceneGraph.ANIM;
using Sims2Tools.DBPF.SceneGraph.BINX;
using Sims2Tools.DBPF.SceneGraph.CINE;
using Sims2Tools.DBPF.SceneGraph.COLL;
using Sims2Tools.DBPF.SceneGraph.CRES;
using Sims2Tools.DBPF.SceneGraph.GMDC;
using Sims2Tools.DBPF.SceneGraph.GMND;
using Sims2Tools.DBPF.SceneGraph.GZPS;
using Sims2Tools.DBPF.SceneGraph.IDR;
using Sims2Tools.DBPF.SceneGraph.LAMB;
using Sims2Tools.DBPF.SceneGraph.LDIR;
using Sims2Tools.DBPF.SceneGraph.LIFO;
using Sims2Tools.DBPF.SceneGraph.LPNT;
using Sims2Tools.DBPF.SceneGraph.LSPT;
using Sims2Tools.DBPF.SceneGraph.MMAT;
using Sims2Tools.DBPF.SceneGraph.SHPE;
using Sims2Tools.DBPF.SceneGraph.TXMT;
using Sims2Tools.DBPF.SceneGraph.TXTR;
using Sims2Tools.DBPF.SceneGraph.XFCH;
using Sims2Tools.DBPF.SceneGraph.XHTN;
using Sims2Tools.DBPF.SceneGraph.XMOL;
using Sims2Tools.DBPF.SceneGraph.XSTN;
using Sims2Tools.DBPF.SceneGraph.XTOL;
using Sims2Tools.DBPF.SLOT;
using Sims2Tools.DBPF.Sounds;
using Sims2Tools.DBPF.Sounds.HLS;
using Sims2Tools.DBPF.Sounds.TRKS;
using Sims2Tools.DBPF.STR;
using Sims2Tools.DBPF.TPRP;
using Sims2Tools.DBPF.TRCN;
using Sims2Tools.DBPF.TTAB;
using Sims2Tools.DBPF.TTAS;
using Sims2Tools.DBPF.UI;
using Sims2Tools.DBPF.VERS;
using Sims2Tools.DBPF.XFLR;
using Sims2Tools.DBPF.XFNC;
using Sims2Tools.DBPF.XOBJ;
using Sims2Tools.DBPF.XROF;
using Sims2Tools.DBPF.XWNT;
using System.Collections.Generic;

namespace Sims2Tools.DBPF
{
    public class DBPFData
    {
        public static TypeTypeID TYPE_NULL = (TypeTypeID)0x00000000; // Technically this is type UI

        public static TypeGroupID GROUP_GLOBALS = (TypeGroupID)0x7FD46CD0;
        public static string NAME_GLOBALS = "Globals";

        public static TypeGroupID GROUP_BEHAVIOR = (TypeGroupID)0x7FE59FD0;
        public static string NAME_BEHAVIOR = "Behaviour";

        public static TypeGroupID GROUP_LOCAL = (TypeGroupID)0xFFFFFFFF;
        public static string NAME_LOCAL = "Local";

        public static TypeGroupID GROUP_SG_MAXIS = (TypeGroupID)0x1C0532FA;
        public static TypeGroupID GROUP_SG_LOCAL = (TypeGroupID)0x1C050000;
        public static TypeGroupID GROUP_GZPS_MAXIS = (TypeGroupID)0x2C17B74A;
        public static TypeGroupID GROUP_COLLECTIONS = (TypeGroupID)0x0FFEFEFE;
        public static TypeGroupID GROUP_BONVOYAGE = (TypeGroupID)0x4F184AA9;

        public static TypeInstanceID INSTANCE_OBJD_DEFAULT = (TypeInstanceID)0x41A7;
        public static TypeInstanceID INSTANCE_NULL = (TypeInstanceID)0x00000000;
        public static TypeInstanceID INSTANCE_COLLECTIONS = (TypeInstanceID)0x0FFE0010;

        public static TypeInstanceID STR_MODELS = (TypeInstanceID)0x00000085;
        public static TypeInstanceID STR_MATERIALS = (TypeInstanceID)0x00000088;
        public static TypeInstanceID STR_BONES = (TypeInstanceID)0x00000090;
        public static TypeInstanceID STR_DIALOG = (TypeInstanceID)0x0000012D;
        public static TypeInstanceID STR_ACTIONS = (TypeInstanceID)0x0000012E;
        public static TypeInstanceID STR_CALL_TREE = (TypeInstanceID)0x0000012F;
        public static TypeInstanceID STR_SOUNDS = (TypeInstanceID)0x00004132;

        public static TypeResourceID RESOURCE_NULL = (TypeResourceID)0x00000000;

        public static TypeGUID GUID_NULL = (TypeGUID)0x00000000;

        private static readonly Dictionary<TypeTypeID, string> ModTypeNames = new Dictionary<TypeTypeID, string>();
        private static readonly Dictionary<TypeTypeID, string> ImgTypeNames = new Dictionary<TypeTypeID, string>();
        private static readonly Dictionary<TypeTypeID, string> RcolTypeNames = new Dictionary<TypeTypeID, string>();
        private static readonly Dictionary<TypeTypeID, string> SgTypeNames = new Dictionary<TypeTypeID, string>();
        private static readonly Dictionary<TypeTypeID, string> OtherTypeNames = new Dictionary<TypeTypeID, string>();
        private static readonly Dictionary<TypeTypeID, string> AllTypeNames = new Dictionary<TypeTypeID, string>();

        static DBPFData()
        {
            ModTypeNames.Add(Bcon.TYPE, Bcon.NAME);
            ModTypeNames.Add(Bhav.TYPE, Bhav.NAME);
            ModTypeNames.Add(Ctss.TYPE, Ctss.NAME);
            ModTypeNames.Add(Glob.TYPE, Glob.NAME);
            ModTypeNames.Add(Objd.TYPE, Objd.NAME);
            ModTypeNames.Add(Objf.TYPE, Objf.NAME);
            ModTypeNames.Add(Nref.TYPE, Nref.NAME);
            ModTypeNames.Add(Slot.TYPE, Slot.NAME);
            ModTypeNames.Add(Str.TYPE, Str.NAME);
            ModTypeNames.Add(Tprp.TYPE, Tprp.NAME);
            ModTypeNames.Add(Trcn.TYPE, Trcn.NAME);
            ModTypeNames.Add(Ttab.TYPE, Ttab.NAME);
            ModTypeNames.Add(Ttas.TYPE, Ttas.NAME);
            ModTypeNames.Add(Ui.TYPE, Ui.NAME);
            ModTypeNames.Add(Vers.TYPE, Vers.NAME);

            ImgTypeNames.Add(Img.TYPE, Img.NAME);
            ImgTypeNames.Add(Jpg.TYPE, Jpg.NAME);
            ImgTypeNames.Add(Thub.TYPE, Thub.NAME);

            RcolTypeNames.Add(Anim.TYPE, Anim.NAME);
            RcolTypeNames.Add(Cine.TYPE, Cine.NAME);
            RcolTypeNames.Add(Cres.TYPE, Cres.NAME);
            RcolTypeNames.Add(Gmdc.TYPE, Gmdc.NAME);
            RcolTypeNames.Add(Gmnd.TYPE, Gmnd.NAME);
            RcolTypeNames.Add(Lamb.TYPE, Lamb.NAME);
            RcolTypeNames.Add(Ldir.TYPE, Ldir.NAME);
            RcolTypeNames.Add(Lifo.TYPE, Lifo.NAME);
            RcolTypeNames.Add(Lpnt.TYPE, Lpnt.NAME);
            RcolTypeNames.Add(Lspt.TYPE, Lspt.NAME);
            RcolTypeNames.Add(Shpe.TYPE, Shpe.NAME);
            RcolTypeNames.Add(Txmt.TYPE, Txmt.NAME);
            RcolTypeNames.Add(Txtr.TYPE, Txtr.NAME);

            SgTypeNames.Add(Aged.TYPE, Aged.NAME);
            SgTypeNames.Add(Binx.TYPE, Binx.NAME);
            SgTypeNames.Add(Coll.TYPE, Coll.NAME);
            SgTypeNames.Add(Gzps.TYPE, Gzps.NAME);
            SgTypeNames.Add(Idr.TYPE, Idr.NAME);
            SgTypeNames.Add(Mmat.TYPE, Mmat.NAME);
            SgTypeNames.Add(Xfch.TYPE, Xfch.NAME);
            SgTypeNames.Add(Xmol.TYPE, Xmol.NAME);
            SgTypeNames.Add(Xhtn.TYPE, Xhtn.NAME);
            SgTypeNames.Add(Xstn.TYPE, Xstn.NAME);
            SgTypeNames.Add(Xtol.TYPE, Xtol.NAME);

            OtherTypeNames.Add(Bnfo.TYPE, Bnfo.NAME);
            OtherTypeNames.Add(Clst.TYPE, Clst.NAME);
            OtherTypeNames.Add(Fami.TYPE, Fami.NAME);
            OtherTypeNames.Add(Famt.TYPE, Famt.NAME);
            OtherTypeNames.Add(Idno.TYPE, Idno.NAME);
            OtherTypeNames.Add(Ltxt.TYPE, Ltxt.NAME);
            OtherTypeNames.Add(Ngbh.TYPE, Ngbh.NAME);
            OtherTypeNames.Add(Sdna.TYPE, Sdna.NAME);
            OtherTypeNames.Add(Sdsc.TYPE, Sdsc.NAME);
            OtherTypeNames.Add(Srel.TYPE, Srel.NAME);
            OtherTypeNames.Add(Swaf.TYPE, Swaf.NAME);
            OtherTypeNames.Add(Xflr.TYPE, Xflr.NAME);
            OtherTypeNames.Add(Xfnc.TYPE, Xfnc.NAME);
            OtherTypeNames.Add(Xobj.TYPE, Xobj.NAME);
            OtherTypeNames.Add(Xrof.TYPE, Xrof.NAME);
            OtherTypeNames.Add(Xwnt.TYPE, Xwnt.NAME);

            OtherTypeNames.Add(Hls.TYPE, Hls.NAME);
            OtherTypeNames.Add(Trks.TYPE, Trks.NAME);
            OtherTypeNames.Add(Audio.TYPE, Audio.NAME);

            OtherTypeNames.Add(Cgn1.TYPE, Cgn1.NAME);
            OtherTypeNames.Add(Grop.TYPE, Grop.NAME);

            foreach (KeyValuePair<TypeTypeID, string> kvPair in ModTypeNames) { AllTypeNames.Add(kvPair.Key, kvPair.Value); }
            foreach (KeyValuePair<TypeTypeID, string> kvPair in ImgTypeNames) { AllTypeNames.Add(kvPair.Key, kvPair.Value); }
            foreach (KeyValuePair<TypeTypeID, string> kvPair in RcolTypeNames) { SgTypeNames.Add(kvPair.Key, kvPair.Value); }
            foreach (KeyValuePair<TypeTypeID, string> kvPair in SgTypeNames) { AllTypeNames.Add(kvPair.Key, kvPair.Value); }
            foreach (KeyValuePair<TypeTypeID, string> kvPair in OtherTypeNames) { AllTypeNames.Add(kvPair.Key, kvPair.Value); }
        }

        public static Dictionary<TypeTypeID, string>.KeyCollection AllTypes
        {
            get => AllTypeNames.Keys;
        }

        public static Dictionary<TypeTypeID, string>.KeyCollection ModTypes
        {
            get => ModTypeNames.Keys;
        }

        public static Dictionary<TypeTypeID, string>.KeyCollection ImgTypes
        {
            get => ImgTypeNames.Keys;
        }

        public static Dictionary<TypeTypeID, string>.KeyCollection SgTypes
        {
            get => SgTypeNames.Keys;
        }

        public static string TypeName(TypeTypeID type)
        {
            AllTypeNames.TryGetValue(type, out string typeName);

            return typeName ?? type.ToString();
        }

        public static bool IsKnownType(TypeTypeID type)
        {
            return AllTypeNames.ContainsKey(type);
        }

        public static bool IsKnownModType(TypeTypeID type)
        {
            return ModTypeNames.ContainsKey(type);
        }

        public static bool IsKnownImgType(TypeTypeID type)
        {
            return ImgTypeNames.ContainsKey(type);
        }

        public static bool IsKnownRcolType(TypeTypeID type)
        {
            return RcolTypeNames.ContainsKey(type);
        }

        public static bool IsKnownSgType(TypeTypeID type)
        {
            return SgTypeNames.ContainsKey(type);
        }

        public static TypeTypeID TypeID(string name)
        {
            foreach (KeyValuePair<TypeTypeID, string> kvPair in AllTypeNames)
            {
                if (kvPair.Value.Equals(name.ToUpper()))
                {
                    return kvPair.Key;
                }
            }

            return (TypeTypeID)0x00000000;
        }
    }
}
