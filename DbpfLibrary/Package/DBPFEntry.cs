/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2024
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

namespace Sims2Tools.DBPF.Package
{
    public class DBPFEntry : DBPFKey // Adding IEquatable<DBPFEntry> here is problematic as it overrides DBPFKey.Equals() and we don't really want all the hassle that comes by doing that!
    {
#if DEBUG
        // private static readonly Logger.IDBPFLogger logger = Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
#endif

        private uint fileOffset;
        private uint fileSize;
        private uint dataSize;

        private uint uncompressedSize = 0;

        public uint FileOffset
        {
            get => fileOffset;
            set => fileOffset = value;
        }

        public uint FileSize
        {
            get => fileSize;
            set => dataSize = fileSize = value;
        }

        public uint DataSize => dataSize;

        public uint UncompressedSize
        {
            get => uncompressedSize;

            set
            {
#if DEBUG
                // Some apps leave an entry in the CLST resource where the compressed size is the same as the data size, ie the data is NOT compressed
                // But we'll let the decompressor take care of it.
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

        public bool IsEquivalent(DBPFEntry that)
        {
            if (that == null) return false;

            if (this.TypeID == that.TypeID && this.GroupID == that.GroupID &&
                this.ResourceID == that.ResourceID && this.InstanceID == that.InstanceID)
            {
                return (this.FileOffset == that.FileOffset && this.FileSize == that.FileSize);
            }

            return false;
        }
    }
}
