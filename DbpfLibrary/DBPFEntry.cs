﻿/*
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

namespace Sims2Tools.DBPF
{
    public class DBPFEntry
    {
        public uint TypeID;
        public uint GroupID;
        public uint InstanceID;
        public uint InstanceID2;

        public uint FileOffset;
        public uint FileSize;
        public uint uncompressedSize = 0;
    }
}