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

using Sims2Tools.DBPF.BCON;
using Sims2Tools.DBPF.BHAV;
using Sims2Tools.DBPF.CTSS;
using Sims2Tools.DBPF.GLOB;
using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.OBJD;
using Sims2Tools.DBPF.OBJF;
using Sims2Tools.DBPF.SceneGraph.BINX;
using Sims2Tools.DBPF.SceneGraph.COLL;
using Sims2Tools.DBPF.SceneGraph.CRES;
using Sims2Tools.DBPF.SceneGraph.GMDC;
using Sims2Tools.DBPF.SceneGraph.GMND;
using Sims2Tools.DBPF.SceneGraph.GZPS;
using Sims2Tools.DBPF.SceneGraph.IDR;
using Sims2Tools.DBPF.SceneGraph.LAMB;
using Sims2Tools.DBPF.SceneGraph.LDIR;
using Sims2Tools.DBPF.SceneGraph.LPNT;
using Sims2Tools.DBPF.SceneGraph.LSPT;
using Sims2Tools.DBPF.SceneGraph.MMAT;
using Sims2Tools.DBPF.SceneGraph.SHPE;
using Sims2Tools.DBPF.SceneGraph.TXMT;
using Sims2Tools.DBPF.SceneGraph.TXTR;
using Sims2Tools.DBPF.SceneGraph.XFCH;
using Sims2Tools.DBPF.SceneGraph.XHTN;
using Sims2Tools.DBPF.SceneGraph.XMOL;
using Sims2Tools.DBPF.SceneGraph.XSTN;
using Sims2Tools.DBPF.SceneGraph.XTOL;
using Sims2Tools.DBPF.STR;
using Sims2Tools.DBPF.TPRP;
using Sims2Tools.DBPF.TRCN;
using Sims2Tools.DBPF.TTAB;
using Sims2Tools.DBPF.TTAS;
using Sims2Tools.DBPF.Utils;
using Sims2Tools.DBPF.VERS;
using System;
using System.Collections.Generic;
using System.IO;

namespace Sims2Tools.DBPF
{
    // See also - https://modthesims.info/wiki.php?title=DBPF/Source_Code and https://modthesims.info/wiki.php?title=DBPF
    public class DBPFFile : IDisposable
    {
        public string fname = "";
        public DIRFile DIR = null;

        public int DateCreated;
        public int DateModified;

        public uint IndexMajorVersion;
        public uint IndexMinorVersion;

        public uint NumEntries;
        private IoBuffer m_Reader;

        private readonly Dictionary<int, DBPFEntry> m_EntryByID = new Dictionary<int, DBPFEntry>();
        private Dictionary<int, DBPFEntry> m_EntryByFullID = new Dictionary<int, DBPFEntry>();
        private readonly Dictionary<TypeTypeID, List<DBPFEntry>> m_EntriesByType = new Dictionary<TypeTypeID, List<DBPFEntry>>();

        public DBPFFile(string file)
        {
            var stream = File.OpenRead(file);
            fname = file;
            Read(stream);
        }

        public void Read(Stream stream)
        {
            m_EntryByFullID = new Dictionary<int, DBPFEntry>();

            var io = IoBuffer.FromStream(stream, ByteOrder.LITTLE_ENDIAN);
            this.m_Reader = io;

            var magic = io.ReadCString(4);
            if (magic != "DBPF")
            {
                throw new Exception("Not a DBPF file");
            }

            var majorVersion = io.ReadUInt32();
            var minorVersion = io.ReadUInt32();
            var version = majorVersion + (minorVersion / 10.0);

            io.Skip(12);
            if (version <= 1.2)
            {
                this.DateCreated = io.ReadInt32();
                this.DateModified = io.ReadInt32();
            }

            if (version < 2.0)
            {
                IndexMajorVersion = io.ReadUInt32();
            }

            NumEntries = io.ReadUInt32();
            uint indexOffset = 0;
            if (version < 2.0)
            {
                indexOffset = io.ReadUInt32();
            }

            _ = io.ReadUInt32();

            if (version < 2.0)
            {
                _ = io.ReadUInt32();
                _ = io.ReadUInt32();
                _ = io.ReadUInt32();
                IndexMinorVersion = io.ReadUInt32();
            }
            else if (version == 2.0)
            {
                IndexMinorVersion = io.ReadUInt32();
                indexOffset = io.ReadUInt32();
                io.Skip(4);
            }

            io.Skip(32);

            io.Seek(SeekOrigin.Begin, indexOffset);
            for (int i = 0; i < NumEntries; i++)
            {
                TypeTypeID typeID = io.ReadTypeId();
                TypeGroupID groupID = io.ReadGroupId();
                TypeInstanceID instanceID = io.ReadInstanceId();
                TypeResourceID resourceID = (IndexMinorVersion >= 2) ? io.ReadResourceId() : (TypeResourceID)0x00000000;

                var entry = new DBPFEntry(typeID, groupID, instanceID, resourceID)
                {
                    FileOffset = io.ReadUInt32(),
                    FileSize = io.ReadUInt32()
                };

                var id = Hash.TGIHash(entry.InstanceID, entry.TypeID, entry.GroupID);
                var fullID = Hash.TGIRHash(entry.InstanceID, entry.ResourceID, entry.TypeID, entry.GroupID);

                m_EntryByID[id] = entry;
                m_EntryByFullID[fullID] = entry;

                if (!m_EntriesByType.ContainsKey(entry.TypeID))
                    m_EntriesByType.Add(entry.TypeID, new List<DBPFEntry>());
                m_EntriesByType[entry.TypeID].Add(entry);
            }

            var dirEntry = GetItemByFullID(Hash.TGIRHash((TypeInstanceID)0x286B1F03, (TypeResourceID)0x00000000, (TypeTypeID)0xE86B1EEF, (TypeGroupID)0xE86B1EEF));
            if (dirEntry != null)
            {
                DIRFile.Read(this, dirEntry);
            }
        }

        public IoBuffer GetIoBuffer(DBPFEntry entry)
        {
            if (entry.UncompressedSize != 0)
            {
                byte[] data = GetItem(entry);

                return IoBuffer.FromStream(new MemoryStream(data), ByteOrder.LITTLE_ENDIAN);
            }

            m_Reader.Seek(SeekOrigin.Begin, entry.FileOffset);
            return m_Reader;
        }

        public byte[] GetItem(DBPFEntry entry)
        {
            m_Reader.Seek(SeekOrigin.Begin, entry.FileOffset);

            if (entry.UncompressedSize != 0)
            {
                return DIREntry.Decompress(m_Reader.ReadBytes((int)entry.FileSize), entry.UncompressedSize);
            }

            return m_Reader.ReadBytes((int)entry.FileSize);
        }

        public byte[] GetItemByFullID(int tgir)
        {
            if (m_EntryByFullID.ContainsKey(tgir))
                return GetItem(m_EntryByFullID[tgir]);
            else
                return null;
        }

        public DBPFEntry GetEntryByFullID(int tgir)
        {
            if (m_EntryByFullID.ContainsKey(tgir))
                return m_EntryByFullID[tgir];
            else
                return null;
        }

        public List<DBPFEntry> GetEntriesByType(TypeTypeID Type)
        {

            var result = new List<DBPFEntry>();

            if (m_EntriesByType.ContainsKey(Type))
            {
                var entries = m_EntriesByType[Type];
                for (int i = 0; i < entries.Count; i++)
                {
                    result.Add(entries[i]);
                }
            }

            return result;
        }

        public String GetFilenameByEntry(DBPFEntry entry)
        {
            return Helper.ToString(this.GetIoBuffer(entry).ReadBytes(Math.Min((int)entry.FileSize, 0x40)));
        }

        public DBPFResource GetResourceByEntry(DBPFEntry entry)
        {
            if (entry == null) return null;

            DBPFResource res = null;

            if (entry.TypeID == Bcon.TYPE)
            {
                res = new Bcon(entry, this.GetIoBuffer(entry));
            }
            else if (entry.TypeID == Bhav.TYPE)
            {
                res = new Bhav(entry, this.GetIoBuffer(entry));
            }
            else if (entry.TypeID == Ctss.TYPE)
            {
                res = new Ctss(entry, this.GetIoBuffer(entry));
            }
            else if (entry.TypeID == Glob.TYPE)
            {
                res = new Glob(entry, this.GetIoBuffer(entry));
            }
            else if (entry.TypeID == Objd.TYPE)
            {
                res = new Objd(entry, this.GetIoBuffer(entry));
            }
            else if (entry.TypeID == Objf.TYPE)
            {
                res = new Objf(entry, this.GetIoBuffer(entry));
            }
            else if (entry.TypeID == Str.TYPE)
            {
                res = new Str(entry, this.GetIoBuffer(entry));
            }
            else if (entry.TypeID == Tprp.TYPE)
            {
                res = new Tprp(entry, this.GetIoBuffer(entry));
            }
            else if (entry.TypeID == Trcn.TYPE)
            {
                res = new Trcn(entry, this.GetIoBuffer(entry));
            }
            else if (entry.TypeID == Ttab.TYPE)
            {
                res = new Ttab(entry, this.GetIoBuffer(entry));
            }
            else if (entry.TypeID == Ttas.TYPE)
            {
                res = new Ttas(entry, this.GetIoBuffer(entry));
            }
            else if (entry.TypeID == Vers.TYPE)
            {
                res = new Vers(entry, this.GetIoBuffer(entry));
            }
            // SceneGraph resources
            else if (entry.TypeID == Binx.TYPE)
            {
                res = new Binx(entry, this.GetIoBuffer(entry));
            }
            else if (entry.TypeID == Coll.TYPE)
            {
                res = new Coll(entry, this.GetIoBuffer(entry));
            }
            else if (entry.TypeID == Cres.TYPE)
            {
                res = new Cres(entry, this.GetIoBuffer(entry));
            }
            else if (entry.TypeID == Gmdc.TYPE)
            {
                res = new Gmdc(entry, this.GetIoBuffer(entry));
            }
            else if (entry.TypeID == Gmnd.TYPE)
            {
                res = new Gmnd(entry, this.GetIoBuffer(entry));
            }
            else if (entry.TypeID == Gzps.TYPE)
            {
                res = new Gzps(entry, this.GetIoBuffer(entry));
            }
            else if (entry.TypeID == Idr.TYPE)
            {
                res = new Idr(entry, this.GetIoBuffer(entry));
            }
            else if (entry.TypeID == Lamb.TYPE)
            {
                res = new Lamb(entry, this.GetIoBuffer(entry));
            }
            else if (entry.TypeID == Ldir.TYPE)
            {
                res = new Ldir(entry, this.GetIoBuffer(entry));
            }
            else if (entry.TypeID == Lpnt.TYPE)
            {
                res = new Lpnt(entry, this.GetIoBuffer(entry));
            }
            else if (entry.TypeID == Lspt.TYPE)
            {
                res = new Lspt(entry, this.GetIoBuffer(entry));
            }
            else if (entry.TypeID == Mmat.TYPE)
            {
                res = new Mmat(entry, this.GetIoBuffer(entry));
            }
            else if (entry.TypeID == Shpe.TYPE)
            {
                res = new Shpe(entry, this.GetIoBuffer(entry));
            }
            else if (entry.TypeID == Txmt.TYPE)
            {
                res = new Txmt(entry, this.GetIoBuffer(entry));
            }
            else if (entry.TypeID == Txtr.TYPE)
            {
                res = new Txtr(entry, this.GetIoBuffer(entry));
            }
            else if (entry.TypeID == Xfch.TYPE)
            {
                res = new Xfch(entry, this.GetIoBuffer(entry));
            }
            else if (entry.TypeID == Xmol.TYPE)
            {
                res = new Xmol(entry, this.GetIoBuffer(entry));
            }
            else if (entry.TypeID == Xhtn.TYPE)
            {
                res = new Xhtn(entry, this.GetIoBuffer(entry));
            }
            else if (entry.TypeID == Xstn.TYPE)
            {
                res = new Xstn(entry, this.GetIoBuffer(entry));
            }
            else if (entry.TypeID == Xtol.TYPE)
            {
                res = new Xtol(entry, this.GetIoBuffer(entry));
            }

            return res;
        }

        public void Close()
        {
            m_Reader.Close();
        }

        public void Dispose()
        {
            m_Reader.Dispose();
        }
    }
}
