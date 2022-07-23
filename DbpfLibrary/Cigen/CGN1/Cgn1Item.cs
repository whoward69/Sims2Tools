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

using Sims2Tools.DBPF.Images.IMG;
using Sims2Tools.DBPF.IO;
using System.Diagnostics;

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
            // Unknown 4 bytes, that look like two 16-bit values, first is usually 0x0012, second looks like it could be the length of the image data
            _ = reader.ReadUInt16();
            _ = reader.ReadUInt16();

            // TGIR of the owning resource, have seen AGED (skins) and GZPS (clothing) types, but probably more
            ownerKey = new DBPFKey(reader.ReadTypeId(), reader.ReadGroupId(), reader.ReadInstanceId(), reader.ReadResourceId());

            // We're expecting an IMG resource now
            TypeTypeID nextType = reader.ReadTypeId();
            Debug.Assert(nextType.Equals(Img.TYPE));

            // Oddly, the group for the IMG is given as 0x6F001872 (which could be a hash of "cigen"), but the actual group is 0xFFFFFFFF
            _ = reader.ReadGroupId();
            imageKey = new DBPFKey(nextType, DBPFData.GROUP_LOCAL, reader.ReadInstanceId(), reader.ReadResourceId());

            // Unknown 68 bytes, mostly 0x00s, could be anything!
            _ = reader.ReadBytes(17 * 4);
        }
    }
}
