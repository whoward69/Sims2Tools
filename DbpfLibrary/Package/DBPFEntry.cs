/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2023
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

namespace Sims2Tools.DBPF.Package
{
    public class DBPFEntry : DBPFKey
    {
#if DEBUG
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
#endif

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
            set
            {
#if DEBUG
                // Some apps leave an entry in the CLST resource where the compressed size is the same as the data size, ie the data is NOT compressed. But we'll let the decompressor take care of it.
                if (dataSize != value)
                {
                    // logger.Debug($"Uncompressed size is the same as the data size for {this}");
                }
#endif

                dataSize = uncompressedSize = value;
            }
        }

        public DBPFEntry(TypeTypeID typeID, TypeGroupID groupID, TypeInstanceID instanceID, TypeResourceID resourceID) : base(typeID, groupID, instanceID, resourceID)
        {
        }

        public DBPFEntry(IDBPFKey key) : base(key)
        {
        }
    }
}
