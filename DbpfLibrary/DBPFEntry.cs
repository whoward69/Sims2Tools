/*
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

namespace Sims2Tools.DBPF
{
    public class DBPFEntry : DBPFKey
    {
        private uint fileOffset;
        private uint fileSize;
        private uint dataSize;

        private uint uncompressedSize = 0;

        public uint FileOffset
        {
            get { return fileOffset; }
            set { fileOffset = value; }
        }

        public uint FileSize
        {
            get { return fileSize; }
            set { dataSize = fileSize = value; }
        }

        public uint DataSize
        {
            get { return dataSize; }
        }

        public uint UncompressedSize
        {
            get { return uncompressedSize; }
            set { dataSize = uncompressedSize = value; }
        }

        public DBPFEntry(TypeTypeID typeID, TypeGroupID groupID, TypeInstanceID instanceID, TypeResourceID resourceID) : base(typeID, groupID, instanceID, resourceID)
        {
        }
    }
}
