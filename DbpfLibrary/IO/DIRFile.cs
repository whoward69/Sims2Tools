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

using Sims2Tools.DBPF.Utils;
using System.IO;

namespace Sims2Tools.DBPF.IO
{
    public class DIRFile
    {
        public static void Read(DBPFFile package, byte[] file)
        {
            var stream = new MemoryStream(file);
            var reader = IoBuffer.FromStream(stream);

            while (stream.Position < file.Length)
            {
                TypeTypeID TypeID = reader.ReadTypeId();
                TypeGroupID GroupID = reader.ReadGroupId();
                TypeInstanceID InstanceID = reader.ReadInstanceId();
                TypeResourceID ResourceID = (package.IndexMinorVersion >= 2) ? reader.ReadResourceId() : (TypeResourceID)0x00000000;

                package.GetEntryByTGIR(Hash.TGIRHash(InstanceID, ResourceID, TypeID, GroupID)).UncompressedSize = reader.ReadUInt32();
            }

            reader.Dispose();
            stream.Dispose();
        }
    }
}