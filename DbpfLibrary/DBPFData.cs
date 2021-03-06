﻿/*
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

using Sims2Tools.DBPF.BCON;
using Sims2Tools.DBPF.BHAV;
using Sims2Tools.DBPF.CTSS;
using Sims2Tools.DBPF.GLOB;
using Sims2Tools.DBPF.OBJD;
using Sims2Tools.DBPF.OBJF;
using Sims2Tools.DBPF.SceneGraph.BINX;
using Sims2Tools.DBPF.SceneGraph.COLL;
using Sims2Tools.DBPF.SceneGraph.CRES;
using Sims2Tools.DBPF.SceneGraph.GMDC;
using Sims2Tools.DBPF.SceneGraph.GMND;
using Sims2Tools.DBPF.SceneGraph.GZPS;
using Sims2Tools.DBPF.SceneGraph.IDR;
using Sims2Tools.DBPF.SceneGraph.LAMB;
using Sims2Tools.DBPF.SceneGraph.LDIR;
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
using Sims2Tools.DBPF.STR;
using Sims2Tools.DBPF.TPRP;
using Sims2Tools.DBPF.TRCN;
using Sims2Tools.DBPF.TTAB;
using Sims2Tools.DBPF.TTAS;
using Sims2Tools.DBPF.VERS;
using System;
using System.Collections.Generic;

namespace Sims2Tools.DBPF
{
    public class DBPFData
    {
        public static TypeGroupID GROUP_GLOBALS = (TypeGroupID)0x7FD46CD0;
        public static String NAME_GLOBALS = "Globals";

        public static TypeGroupID GROUP_BEHAVIOR = (TypeGroupID)0x7FE59FD0;
        public static String NAME_BEHAVIOR = "Behaviour";

        public static TypeGroupID GROUP_LOCAL = (TypeGroupID)0xFFFFFFFF;
        public static String NAME_LOCAL = "Local";

        public static TypeGroupID GROUP_SG = (TypeGroupID)0x1C050000;

        public static TypeInstanceID INSTANCE_OBJD_DEFAULT = (TypeInstanceID)0x41A7;

        private static readonly Dictionary<TypeTypeID, String> ModTypeNames = new Dictionary<TypeTypeID, string>();
        private static readonly Dictionary<TypeTypeID, String> SgTypeNames = new Dictionary<TypeTypeID, string>();
        private static readonly Dictionary<TypeTypeID, String> AllTypeNames = new Dictionary<TypeTypeID, string>();

        static DBPFData()
        {
            ModTypeNames.Add(Bcon.TYPE, Bcon.NAME);
            ModTypeNames.Add(Bhav.TYPE, Bhav.NAME);
            ModTypeNames.Add(Ctss.TYPE, Ctss.NAME);
            ModTypeNames.Add(Glob.TYPE, Glob.NAME);
            ModTypeNames.Add(Objd.TYPE, Objd.NAME);
            ModTypeNames.Add(Objf.TYPE, Objf.NAME);
            ModTypeNames.Add(Str.TYPE, Str.NAME);
            ModTypeNames.Add(Tprp.TYPE, Tprp.NAME);
            ModTypeNames.Add(Trcn.TYPE, Trcn.NAME);
            ModTypeNames.Add(Ttab.TYPE, Ttab.NAME);
            ModTypeNames.Add(Ttas.TYPE, Ttas.NAME);
            ModTypeNames.Add(Vers.TYPE, Vers.NAME);

            // SgTypeNames.Add(Anim.TYPE, Anim.NAME);
            SgTypeNames.Add(Binx.TYPE, Binx.NAME);
            // SgTypeNames.Add(Cine.TYPE, Cine.NAME);
            SgTypeNames.Add(Coll.TYPE, Coll.NAME);
            SgTypeNames.Add(Cres.TYPE, Cres.NAME);
            SgTypeNames.Add(Gmdc.TYPE, Gmdc.NAME);
            SgTypeNames.Add(Gmnd.TYPE, Gmnd.NAME);
            SgTypeNames.Add(Gzps.TYPE, Gzps.NAME);
            SgTypeNames.Add(Idr.TYPE, Idr.NAME);
            SgTypeNames.Add(Lamb.TYPE, Lamb.NAME);
            SgTypeNames.Add(Ldir.TYPE, Ldir.NAME);
            // SgTypeNames.Add(Lifo.TYPE, Lifo.NAME);
            SgTypeNames.Add(Lpnt.TYPE, Lpnt.NAME);
            SgTypeNames.Add(Lspt.TYPE, Lspt.NAME);
            SgTypeNames.Add(Mmat.TYPE, Mmat.NAME);
            SgTypeNames.Add(Shpe.TYPE, Shpe.NAME);
            SgTypeNames.Add(Txmt.TYPE, Txmt.NAME);
            SgTypeNames.Add(Txtr.TYPE, Txtr.NAME);
            SgTypeNames.Add(Xfch.TYPE, Xfch.NAME);
            SgTypeNames.Add(Xmol.TYPE, Xmol.NAME);
            SgTypeNames.Add(Xhtn.TYPE, Xhtn.NAME);
            SgTypeNames.Add(Xstn.TYPE, Xstn.NAME);
            SgTypeNames.Add(Xtol.TYPE, Xtol.NAME);

            foreach (KeyValuePair<TypeTypeID, String> kvPair in ModTypeNames) { AllTypeNames.Add(kvPair.Key, kvPair.Value); }
            foreach (KeyValuePair<TypeTypeID, String> kvPair in SgTypeNames) { AllTypeNames.Add(kvPair.Key, kvPair.Value); }
        }

        public static Dictionary<TypeTypeID, String>.KeyCollection AllTypes
        {
            get => AllTypeNames.Keys;
        }

        public static Dictionary<TypeTypeID, String>.KeyCollection ModTypes
        {
            get => ModTypeNames.Keys;
        }

        public static Dictionary<TypeTypeID, String>.KeyCollection SgTypes
        {
            get => SgTypeNames.Keys;
        }

        public static String TypeName(TypeTypeID type)
        {
            AllTypeNames.TryGetValue(type, out string typeName);

            return typeName ?? type.ToString();
        }

        public static bool IsKnownType(TypeTypeID type)
        {
            return AllTypeNames.ContainsKey(type);
        }

        public static TypeTypeID TypeID(String name)
        {
            foreach (KeyValuePair<TypeTypeID, String> kvPair in AllTypeNames)
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
