/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2022
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF.IO;

namespace Sims2Tools.DBPF.Cigen.CGN1
{
    // Determined by reverse engineering the cigen.package file!
    // This could all be horribly wrong!

    public class Cgn1Item
    {
        private DBPFKey ownerKey;
        private DBPFKey imageKey;

        public DBPFKey OwnerKey => ownerKey;
        public DBPFKey ImageKey => imageKey;

        public Cgn1Item(DbpfReader reader)
        {
            this.Unserialize(reader);
        }

        protected void Unserialize(DbpfReader reader)
        {
            _ = reader.ReadUInt16();
            _ = reader.ReadUInt16();

            ownerKey = new DBPFKey(reader.ReadTypeId(), reader.ReadGroupId(), reader.ReadInstanceId(), reader.ReadResourceId());
            imageKey = new DBPFKey(reader.ReadTypeId(), reader.ReadGroupId(), reader.ReadInstanceId(), reader.ReadResourceId());

            _ = reader.ReadBytes(17 * 4);
        }
    }
}
