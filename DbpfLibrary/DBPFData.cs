/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020
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
        public static uint GROUP_GLOBALS = 0x7FD46CD0;
        public static String NAME_GLOBALS = "Globals";

        public static uint GROUP_BEHAVIOR = 0x7FE59FD0;
        public static String NAME_BEHAVIOR = "Behaviour";

        public static uint GROUP_LOCAL = 0xFFFFFFFF;
        public static String NAME_LOCAL = "Local";

        public static uint INSTANCE_OBJD_DEFAULT = 0x41A7;

        private static readonly Dictionary<uint, String> TypeNames = new Dictionary<uint, string>();

        static DBPFData()
        {
            TypeNames.Add(Bcon.TYPE, Bcon.NAME);
            TypeNames.Add(Bhav.TYPE, Bhav.NAME);
            TypeNames.Add(Ctss.TYPE, Ctss.NAME);
            TypeNames.Add(Glob.TYPE, Glob.NAME);
            TypeNames.Add(Objd.TYPE, Objd.NAME);
            TypeNames.Add(Objf.TYPE, Objf.NAME);
            TypeNames.Add(Str.TYPE, Str.NAME);
            TypeNames.Add(Tprp.TYPE, Tprp.NAME);
            TypeNames.Add(Trcn.TYPE, Trcn.NAME);
            TypeNames.Add(Ttab.TYPE, Ttab.NAME);
            TypeNames.Add(Ttas.TYPE, Ttas.NAME);
            TypeNames.Add(Vers.TYPE, Vers.NAME);
        }

        public static Dictionary<uint, String>.KeyCollection Types
        {
            get => TypeNames.Keys;
        }

        public static String TypeName(uint type)
        {
            TypeNames.TryGetValue(type, out string typeName);

            return typeName;
        }
    }
}
