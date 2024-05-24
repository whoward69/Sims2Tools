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

using Sims2Tools.DBPF.IO;
using System;
using System.Diagnostics;

namespace Sims2Tools.DBPF.Package
{
    // See also - https://modthesims.info/wiki.php?title=DBPF/Source_Code and https://modthesims.info/wiki.php?title=DBPF
    internal class DBPFHeader
    {
#pragma warning disable IDE0052 // Remove unread private members
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
#pragma warning restore IDE0052 // Remove unread private members

        private int HeaderSize => (minorVersion >= 1) ? 96 : 92;

        private readonly string packagePath;
        internal string PackagePath => packagePath;

        // private uint dateCreated, dateModified;

        private uint majorVersion, minorVersion;
        private uint indexMajorVersion, indexMinorVersion;

        public uint IndexMajorVersion => indexMajorVersion;
        public uint IndexMinorVersion => indexMinorVersion;


        private uint resourceIndexCount, resourceIndexOffset, resourceIndexSize;

        public uint ResourceIndexCount => resourceIndexCount;
        public uint ResourceIndexOffset => resourceIndexOffset;
        public uint ResourceIndexSize => resourceIndexSize;


        // private uint holeIndexCount, holeIndexOffset, holeIndexSize;

        // public uint HoleIndexCount => holeIndexCount;
        // public uint HoleIndexOffset => holeIndexOffset;
        // public uint HoleIndexSize => holeIndexSize;

        internal DBPFHeader(string packagePath)
        {
            this.packagePath = packagePath;

            majorVersion = 1;
            minorVersion = 1;

            indexMajorVersion = 7;
            indexMinorVersion = 2;

            resourceIndexCount = 0;
            resourceIndexOffset = 0;
            resourceIndexSize = 0;
        }

        internal DBPFHeader(string packagePath, DbpfReader reader) : this(packagePath)
        {
            Unserialize(reader);
        }

        // See https://modthesims.info/wiki.php?title=DBPF and also ~\fullsimpe\SimPe Packages\HeaderData.cs (method Load)
        private void Unserialize(DbpfReader reader)
        {
            Debug.Assert(reader.Position == 0);

            string magic = reader.ReadCString(4);
            if (magic != "DBPF")
            {
                throw new Exception("Not a DBPF file");
            }

            majorVersion = reader.ReadUInt32();
            minorVersion = reader.ReadUInt32();

            if (majorVersion > 1)
            {
                throw new Exception("Not a Sims 2 DBPF file");
            }

            reader.Skip(12);

            /* dateCreated */
            _ = reader.ReadUInt32();
            /* dateModified */
            _ = reader.ReadUInt32();

            indexMajorVersion = reader.ReadUInt32();

            resourceIndexCount = reader.ReadUInt32();
            resourceIndexOffset = reader.ReadUInt32();
            resourceIndexSize = reader.ReadUInt32();

            /* holeIndexCount */
            _ = reader.ReadUInt32();
            /* holeIndexOffset */
            _ = reader.ReadUInt32();
            /* holeIndexSize*/
            _ = reader.ReadUInt32();

            if (minorVersion >= 1)
            {
                indexMinorVersion = reader.ReadUInt32();
            }

            reader.Skip(32);

            Debug.Assert(reader.Position == HeaderSize);
        }

        internal void Serialize(DbpfWriter writer, DBPFResourceIndex resourceIndex)
        {
            Debug.Assert(writer.Position == 0);

            writer.WriteMagic(new char[] { 'D', 'B', 'P', 'F' });

            writer.WriteUInt32(majorVersion);
            writer.WriteUInt32(minorVersion);

            writer.WriteBytes(new byte[12]);

            writer.WriteUInt32(/*dateCreated*/ 0);
            writer.WriteUInt32(/*(uint) DateTime.Now.Ticks*/ 0);

            writer.WriteUInt32(indexMajorVersion);

            writer.WriteUInt32(resourceIndex.Count);
            writer.WriteUInt32((uint)(resourceIndex.Offset + HeaderSize));
            writer.WriteUInt32(resourceIndex.Size);

            writer.WriteUInt32(0);
            writer.WriteUInt32(0);
            writer.WriteUInt32(0);

            if (minorVersion >= 1)
            {
                writer.WriteUInt32(indexMinorVersion);
            }

            writer.WriteBytes(new byte[32]);

            Debug.Assert(writer.Position == HeaderSize);
        }
    }
}
