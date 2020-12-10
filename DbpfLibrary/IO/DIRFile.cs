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

using Sims2Tools.DBPF.Utils;
using System.IO;

namespace Sims2Tools.DBPF.IO
{
    public class DIRFile
    {
        public static void Read(DBPFFile package, byte[] file)
        {
            var stream = new MemoryStream(file);
            var reader = IoBuffer.FromStream(stream, ByteOrder.LITTLE_ENDIAN);

            while (stream.Position < file.Length)
            {
                var TypeID = reader.ReadUInt32();
                var GroupID = reader.ReadUInt32();
                var InstanceID = reader.ReadUInt32();
                uint InstanceID2 = 0x00000000;
                if (package.IndexMinorVersion >= 2)
                    InstanceID2 = reader.ReadUInt32();
                var idEntry2 = Hash.TGIRHash(InstanceID, InstanceID2, TypeID, GroupID);
                package.GetEntryByFullID(idEntry2).uncompressedSize = reader.ReadUInt32();
            }

            reader.Dispose();
            stream.Dispose();
        }
    }
}