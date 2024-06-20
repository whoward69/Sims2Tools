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

using Sims2Tools.DBPF.BCON;
using Sims2Tools.DBPF.BHAV;
using Sims2Tools.DBPF.Cigen.CGN1;
using Sims2Tools.DBPF.CTSS;
using Sims2Tools.DBPF.GLOB;
using Sims2Tools.DBPF.Groups.GROP;
using Sims2Tools.DBPF.Images.IMG;
using Sims2Tools.DBPF.Images.JPG;
using Sims2Tools.DBPF.Images.THUB;
using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.Logger;
using Sims2Tools.DBPF.Neighbourhood.BNFO;
using Sims2Tools.DBPF.Neighbourhood.FAMI;
using Sims2Tools.DBPF.Neighbourhood.FAMT;
using Sims2Tools.DBPF.Neighbourhood.IDNO;
using Sims2Tools.DBPF.Neighbourhood.LTXT;
using Sims2Tools.DBPF.Neighbourhood.NGBH;
using Sims2Tools.DBPF.Neighbourhood.SDNA;
using Sims2Tools.DBPF.Neighbourhood.SDSC;
using Sims2Tools.DBPF.Neighbourhood.SREL;
using Sims2Tools.DBPF.Neighbourhood.SWAF;
using Sims2Tools.DBPF.NREF;
using Sims2Tools.DBPF.OBJD;
using Sims2Tools.DBPF.OBJF;
using Sims2Tools.DBPF.SceneGraph;
using Sims2Tools.DBPF.SceneGraph.BINX;
using Sims2Tools.DBPF.SceneGraph.COLL;
using Sims2Tools.DBPF.SceneGraph.CRES;
using Sims2Tools.DBPF.SceneGraph.GMDC;
using Sims2Tools.DBPF.SceneGraph.GMND;
using Sims2Tools.DBPF.SceneGraph.GZPS;
using Sims2Tools.DBPF.SceneGraph.IDR;
using Sims2Tools.DBPF.SceneGraph.LAMB;
using Sims2Tools.DBPF.SceneGraph.LDIR;
using Sims2Tools.DBPF.SceneGraph.LIFO;
using Sims2Tools.DBPF.SceneGraph.LPNT;
using Sims2Tools.DBPF.SceneGraph.LSPT;
using Sims2Tools.DBPF.SceneGraph.MMAT;
using Sims2Tools.DBPF.SceneGraph.RCOL;
using Sims2Tools.DBPF.SceneGraph.SHPE;
using Sims2Tools.DBPF.SceneGraph.TXMT;
using Sims2Tools.DBPF.SceneGraph.TXTR;
using Sims2Tools.DBPF.SceneGraph.XFCH;
using Sims2Tools.DBPF.SceneGraph.XHTN;
using Sims2Tools.DBPF.SceneGraph.XMOL;
using Sims2Tools.DBPF.SceneGraph.XSTN;
using Sims2Tools.DBPF.SceneGraph.XTOL;
using Sims2Tools.DBPF.SLOT;
using Sims2Tools.DBPF.STR;
using Sims2Tools.DBPF.TPRP;
using Sims2Tools.DBPF.TRCN;
using Sims2Tools.DBPF.TTAB;
using Sims2Tools.DBPF.TTAS;
using Sims2Tools.DBPF.UI;
using Sims2Tools.DBPF.Utils;
using Sims2Tools.DBPF.VERS;
using Sims2Tools.DBPF.XFLR;
using Sims2Tools.DBPF.XFNC;
using Sims2Tools.DBPF.XOBJ;
using Sims2Tools.DBPF.XROF;
using Sims2Tools.DBPF.XWNT;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Sims2Tools.DBPF.Package
{
    // See also - https://modthesims.info/wiki.php?title=DBPF/Source_Code and https://modthesims.info/wiki.php?title=DBPF
    public class DBPFFile : IDisposable
    {
        private static readonly IDBPFLogger logger = new DBPFLogger(log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType));

        private static readonly Encoding encoding = new UTF8Encoding(false, true);
        public static Encoding Encoding => encoding;

        private readonly string packagePath = null;
        private readonly string packageDir = null;
        private readonly string packageName = null;
        private readonly string packageNameNoExtn = null;

        private DBPFHeader header;
        private DBPFResourceIndex resourceIndex;
        private DBPFResourceCache resourceCache;

        private DbpfReader m_Reader;

        public string PackagePath => packagePath;
        public string PackageDir => packageDir;
        public string PackageName => packageName;
        public string PackageNameNoExtn => packageNameNoExtn;

        public uint ResourceCount => header.ResourceIndexCount;

        public bool IsDirty => resourceIndex.IsDirty;

        public void SetClean() => resourceIndex.SetClean();

        public DBPFFile(string packagePath) : this(logger, packagePath)
        {
        }

        private DBPFFile(IDBPFLogger logger, string packagePath)
        {
            this.packagePath = packagePath;

            FileInfo fi = new FileInfo(packagePath);
            this.packageDir = fi.DirectoryName;
            this.packageName = fi.Name;
            this.packageNameNoExtn = packageName.Substring(0, packageName.LastIndexOf('.'));

            if (File.Exists(packagePath))
            {
                Read(File.OpenRead(packagePath));
            }
            else
            {
                header = new DBPFHeader(logger, PackagePath);
                resourceCache = new DBPFResourceCache(logger);
                resourceIndex = new DBPFResourceIndex(logger, header, resourceCache, null);
            }
        }

        protected void Read(Stream stream)
        {
            Read(stream, stream.Length);
        }

        protected void Read(Stream stream, long length)
        {
            this.m_Reader = DbpfReader.FromStream(stream, length);

            header = new DBPFHeader(logger, PackagePath, m_Reader);

            resourceCache = new DBPFResourceCache(logger);
            resourceIndex = new DBPFResourceIndex(logger, header, resourceCache, m_Reader);
        }

        protected void Write(Stream stream)
        {
            using (DbpfWriter writer = DbpfWriter.FromStream(stream))
            {
                header.Serialize(writer, resourceIndex);

                resourceIndex.Serialize(writer);

                foreach (DBPFEntry entry in resourceIndex.GetAllEntries(true))
                {
                    if (resourceIndex.IsDuplicate(entry))
                    {
                        m_Reader.Seek(SeekOrigin.Begin, entry.FileOffset);
                        writer.WriteBytes(m_Reader.ReadBytes((int)entry.FileSize));
                    }
                    else if (resourceCache.IsResource(entry))
                    {
                        long bytesBefore = writer.Position;
                        resourceCache.GetResourceByKey(entry).Serialize(writer);
                        Trace.Assert(entry.FileSize == (writer.Position - bytesBefore), $"Serialize data != FileSize for {entry}");
                    }
                    else if (resourceCache.IsItem(entry))
                    {
                        writer.WriteBytes(resourceCache.GetItemByKey(entry));
                    }
                    else
                    {
                        m_Reader.Seek(SeekOrigin.Begin, entry.FileOffset);
                        writer.WriteBytes(m_Reader.ReadBytes((int)entry.FileSize));
                    }
                }
            }
        }

        public void Commit(DBPFResource resource, bool ignoreDirty = false) => resourceIndex.Commit(resource, ignoreDirty);

        public void Commit(DBPFKey key, byte[] item) => resourceIndex.Commit(key, item);

        public void UnCommit(DBPFKey key) => resourceIndex.UnCommit(key);

        public bool Remove(DBPFKey key) => resourceIndex.Remove(key);

        public string Update(string subFolder) => Update(false, subFolder);

        public string Update(bool autoBackup) => Update(autoBackup, null);

        private string Update(bool autoBackup, string subFolder)
        {
            string originalName = packagePath;
            string updateName;
            string backupName = NextBackupName();

            if (subFolder != null)
            {
                updateName = $"{packageDir}\\{subFolder}\\{packageName}";

                Directory.CreateDirectory($"{packageDir}\\{subFolder}");
            }
            else
            {
                updateName = $"{packagePath}.temp";
            }

            using (Stream stream = File.OpenWrite(updateName))
            {
                Write(stream);

                stream.Close();

                this.Close();

                if (autoBackup && File.Exists(originalName))
                {
                    File.Copy(originalName, backupName, true);
                }

                if (subFolder == null)
                {
                    try
                    {
                        File.Delete(originalName);

                        File.Copy(updateName, originalName, true);
                        File.Delete(updateName);
                    }
                    catch (Exception)
                    {
                        // SimPe propbably has the file open!
                        backupName = null;
                    }

                    Read(File.OpenRead(originalName));
                }
            }

            return (subFolder != null) ? updateName : backupName;
        }

        private string NextBackupName()
        {
            int lastVersion = 0;

            if (Directory.Exists(packageDir))
            {
                int packageLen = packagePath.Length;

                foreach (string baseFile in Directory.GetFiles($"{packageDir}", $"{packageName}.*", SearchOption.TopDirectoryOnly))
                {
                    if (baseFile.EndsWith(".bak"))
                    {
                        string versionPart = baseFile.Substring(packageLen, baseFile.Length - packageLen - 4);

                        if (versionPart.Length == 0)
                        {
                            lastVersion = 1;
                        }
                        else if (versionPart.StartsWith(".V"))
                        {
                            if (int.TryParse(versionPart.Substring(2), out int thisVersion))
                            {
                                if (thisVersion > lastVersion)
                                {
                                    lastVersion = thisVersion;
                                }
                            }
                        }
                    }
                }
            }

            lastVersion += 1;

            string version = (lastVersion <= 1) ? "" : $".V{lastVersion}";

            return $"{packageDir}/{packageName}{version}.bak";
        }

        private DbpfReader GetDbpfReader(DBPFEntry entry)
        {
            return DbpfReader.FromStream(new MemoryStream(GetItemByEntry(entry)));
        }

        private byte[] GetItemByEntry(DBPFEntry entry)
        {
            uint uncompressedSize = entry.UncompressedSize;

            m_Reader.Seek(SeekOrigin.Begin, entry.FileOffset);

            // NOTE: Just because there is no CLST resource (or no entry in the CLST) does NOT mean the data is uncompressed!
            if (uncompressedSize == 0 && entry.FileSize > 9)
            {
                byte[] header = m_Reader.ReadBytes(9);

                m_Reader.Seek(SeekOrigin.Begin, entry.FileOffset);

                // Header bytes 0 thru 3 are a 32-bit value giving the size of the compressed data (little-endian)
                int headerCompressedSize = (((header[3] * 256 + header[2]) * 256 + header[1]) * 256 + header[0]);
                // Header bytes 4 thru 5 are a 16-bit signature value 
                ushort headerSignature = (ushort)(header[5] * 256 + header[4]);
                // Header bytes 6 thru 8 are a 24-bit value giving the size of the decompressed data (big-endian)
                int headerUncompressedSize = ((header[6] * 256 + header[7]) * 256 + header[8]);

                if (headerCompressedSize == entry.FileSize &&
                    headerSignature == 0xFB10 &&
                    headerUncompressedSize > (entry.FileSize - 9))
                {
                    logger.Debug($"Resource {entry} appears to be compressed when marked as not! Missing CLST resource?");

                    uncompressedSize = (uint)headerUncompressedSize;
                }
            }

            if (uncompressedSize != 0)
            {
                try
                {
                    return Decompressor.Decompress(m_Reader.ReadBytes((int)entry.FileSize), uncompressedSize);
                }
                catch (Exception)
                {
                    // This is a fall-back that should never happen as the decompressor does the necessary checks
                    logger.Warn($"Failed to decompress {entry}");

                    m_Reader.Seek(SeekOrigin.Begin, entry.FileOffset);
                }
            }

            return m_Reader.ReadBytes((int)entry.FileSize);
        }

        public byte[] GetItemByKey(DBPFKey key)
        {
            DBPFEntry entry = GetEntryByKey(key);

            return (entry != null) ? GetItemByEntry(entry) : null;
        }

        public byte[] GetOriginalItemByEntry(DBPFEntry entry)
        {
            return GetItemByEntry(entry);
        }

        public DBPFEntry GetEntryByTGIR(int tgir)
        {
            return resourceIndex.GetEntryByTGIR(tgir);
        }

        public DBPFEntry GetEntryByKey(DBPFKey key)
        {
            return resourceIndex.GetEntryByKey(key);
        }

        public List<DBPFEntry> GetAllEntries() // Do NOT change to ReadOnlyCollection, this copy List can be changed if needed
        {
            return resourceIndex.GetAllEntries(false);
        }

        public List<DBPFEntry> GetEntriesByType(TypeTypeID type) // Do NOT change to ReadOnlyCollection, this copy List can be changed if needed
        {
            return resourceIndex.GetEntriesByType(type);
        }

        public string GetFilenameByEntry(DBPFEntry entry)
        {
            if (entry.TypeID == Ui.TYPE)
            {
                return "";
            }
            else
            {
                if (DBPFData.IsKnownRcolType(entry.TypeID))
                {
                    GenericRcol rcol;

                    if (resourceCache.IsCached(entry))
                    {
                        rcol = (GenericRcol)resourceCache.GetResourceByKey(entry);
                    }
                    else
                    {
                        rcol = new GenericRcol(entry, this.GetDbpfReader(entry));
                    }

                    return rcol.KeyName;
                }
                else
                {
                    return Helper.ToString(this.GetDbpfReader(entry).ReadBytes(Math.Min((int)entry.FileSize, 0x40)));
                }
            }
        }

        public DBPFEntry GetEntryByName(TypeTypeID typeId, string sgName)
        {
            if (sgName == null) return null;

            DBPFEntry entry = GetEntryByKey(SgHelper.KeyFromQualifiedName(sgName, typeId, DBPFData.GROUP_SG_MAXIS));

            if (entry == null)
            {
                string suffix = $"_{DBPFData.TypeName(typeId).ToLower()}";

                if (sgName.EndsWith(suffix))
                {
                    sgName = sgName.Substring(0, sgName.Length - suffix.Length);
                }
                else
                {
                    sgName = $"{sgName}{suffix}";
                }

                entry = GetEntryByKey(SgHelper.KeyFromQualifiedName(sgName, typeId, DBPFData.GROUP_SG_MAXIS));
            }

            return entry;
        }

        public DBPFResource GetResourceByName(TypeTypeID typeId, string sgName)
        {
            return GetResourceByEntry(GetEntryByName(typeId, sgName));
        }

        public DBPFResource GetResourceByTGIR(int tgir)
        {
            return GetResourceByEntry(GetEntryByTGIR(tgir));
        }

        public DBPFResource GetResourceByKey(DBPFKey key)
        {
            return GetResourceByEntry(GetEntryByKey(key));
        }

        public DBPFResource GetResourceByEntry(DBPFEntry entry)
        {
            if (entry == null) return null;

            if (resourceCache.IsCached(entry))
            {
                return resourceCache.GetResourceByKey(entry);
            }

            DBPFResource res = null;
            DbpfReader reader = this.GetDbpfReader(entry);

            // If these wern't constructors ...
            // ... we could use delegates.
            // But they are, so we can't!
            if (entry.TypeID == Bcon.TYPE)
            {
                res = new Bcon(entry, reader);
            }
            else if (entry.TypeID == Bhav.TYPE)
            {
                res = new Bhav(entry, reader);
            }
            else if (entry.TypeID == Ctss.TYPE)
            {
                res = new Ctss(entry, reader);
            }
            else if (entry.TypeID == Glob.TYPE)
            {
                res = new Glob(entry, reader);
            }
            else if (entry.TypeID == Objd.TYPE)
            {
                res = new Objd(entry, reader);
            }
            else if (entry.TypeID == Objf.TYPE)
            {
                res = new Objf(entry, reader);
            }
            else if (entry.TypeID == Nref.TYPE)
            {
                res = new Nref(entry, reader);
            }
            else if (entry.TypeID == Slot.TYPE)
            {
                res = new Slot(entry, reader);
            }
            else if (entry.TypeID == Str.TYPE)
            {
                res = new Str(entry, reader);
            }
            else if (entry.TypeID == Tprp.TYPE)
            {
                res = new Tprp(entry, reader);
            }
            else if (entry.TypeID == Trcn.TYPE)
            {
                res = new Trcn(entry, reader);
            }
            else if (entry.TypeID == Ttab.TYPE)
            {
                res = new Ttab(entry, reader);
            }
            else if (entry.TypeID == Ttas.TYPE)
            {
                res = new Ttas(entry, reader);
            }
            else if (entry.TypeID == Vers.TYPE)
            {
                res = new Vers(entry, reader);
            }
            else if (entry.TypeID == Xflr.TYPE)
            {
                res = new Xflr(entry, reader);
            }
            else if (entry.TypeID == Xfnc.TYPE)
            {
                res = new Xfnc(entry, reader);
            }
            else if (entry.TypeID == Xobj.TYPE)
            {
                res = new Xobj(entry, reader);
            }
            else if (entry.TypeID == Xrof.TYPE)
            {
                res = new Xrof(entry, reader);
            }
            else if (entry.TypeID == Xwnt.TYPE)
            {
                res = new Xwnt(entry, reader);
            }
            //
            // Image resources
            //
            else if (entry.TypeID == Img.TYPE)
            {
                res = new Img(entry, reader);
            }
            else if (entry.TypeID == Jpg.TYPE)
            {
                res = new Jpg(entry, reader);
            }
            else if (entry.TypeID == Thub.TYPE)
            {
                res = new Thub(entry, reader);
            }
            else if (entry.TypeID == Thub.TYPES[(int)Thub.ThubTypeIndex.Awning] ||
                     entry.TypeID == Thub.TYPES[(int)Thub.ThubTypeIndex.Chimney] ||
                     entry.TypeID == Thub.TYPES[(int)Thub.ThubTypeIndex.Dormer] ||
                     entry.TypeID == Thub.TYPES[(int)Thub.ThubTypeIndex.FenceArch] ||
                     entry.TypeID == Thub.TYPES[(int)Thub.ThubTypeIndex.FenceOrHalfwall] ||
                     entry.TypeID == Thub.TYPES[(int)Thub.ThubTypeIndex.Floor] ||
                     entry.TypeID == Thub.TYPES[(int)Thub.ThubTypeIndex.FoundationOrPool] ||
                     entry.TypeID == Thub.TYPES[(int)Thub.ThubTypeIndex.ModularStair] ||
                     entry.TypeID == Thub.TYPES[(int)Thub.ThubTypeIndex.Roof] ||
                     entry.TypeID == Thub.TYPES[(int)Thub.ThubTypeIndex.Terrain] ||
                     entry.TypeID == Thub.TYPES[(int)Thub.ThubTypeIndex.Wall])
            {
                res = new Thub(entry, reader);
            }
            //
            // Neighbourhood resources
            //
            else if (entry.TypeID == Bnfo.TYPE)
            {
                res = new Bnfo(entry, reader);
            }
            else if (entry.TypeID == Fami.TYPE)
            {
                res = new Fami(entry, reader);
            }
            else if (entry.TypeID == Famt.TYPE)
            {
                res = new Famt(entry, reader);
            }
            else if (entry.TypeID == Idno.TYPE)
            {
                res = new Idno(entry, reader);
            }
            else if (entry.TypeID == Ltxt.TYPE)
            {
                res = new Ltxt(entry, reader);
            }
            else if (entry.TypeID == Ngbh.TYPE)
            {
                res = new Ngbh(entry, reader);
            }
            else if (entry.TypeID == Sdna.TYPE)
            {
                res = new Sdna(entry, reader);
            }
            else if (entry.TypeID == Sdsc.TYPE)
            {
                res = new Sdsc(entry, reader);
            }
            else if (entry.TypeID == Srel.TYPE)
            {
                res = new Srel(entry, reader);
            }
            else if (entry.TypeID == Swaf.TYPE)
            {
                res = new Swaf(entry, reader);
            }
            //
            // SceneGraph resources
            //
            else if (entry.TypeID == Binx.TYPE)
            {
                res = new Binx(entry, reader);
            }
            else if (entry.TypeID == Coll.TYPE)
            {
                res = new Coll(entry, reader);
            }
            else if (entry.TypeID == Cres.TYPE)
            {
                res = new Cres(entry, reader);
            }
            else if (entry.TypeID == Gmdc.TYPE)
            {
                res = new Gmdc(entry, reader);
            }
            else if (entry.TypeID == Gmnd.TYPE)
            {
                res = new Gmnd(entry, reader);
            }
            else if (entry.TypeID == Gzps.TYPE)
            {
                res = new Gzps(entry, reader);
            }
            else if (entry.TypeID == Idr.TYPE)
            {
                res = new Idr(entry, reader);
            }
            else if (entry.TypeID == Lamb.TYPE)
            {
                res = new Lamb(entry, reader);
            }
            else if (entry.TypeID == Ldir.TYPE)
            {
                res = new Ldir(entry, reader);
            }
            else if (entry.TypeID == Lifo.TYPE)
            {
                res = new Lifo(entry, reader);
            }
            else if (entry.TypeID == Lpnt.TYPE)
            {
                res = new Lpnt(entry, reader);
            }
            else if (entry.TypeID == Lspt.TYPE)
            {
                res = new Lspt(entry, reader);
            }
            else if (entry.TypeID == Mmat.TYPE)
            {
                res = new Mmat(entry, reader);
            }
            else if (entry.TypeID == Shpe.TYPE)
            {
                res = new Shpe(entry, reader);
            }
            else if (entry.TypeID == Txmt.TYPE)
            {
                res = new Txmt(entry, reader);
            }
            else if (entry.TypeID == Txtr.TYPE)
            {
                res = new Txtr(entry, reader);
            }
            else if (entry.TypeID == Xfch.TYPE)
            {
                res = new Xfch(entry, reader);
            }
            else if (entry.TypeID == Xmol.TYPE)
            {
                res = new Xmol(entry, reader);
            }
            else if (entry.TypeID == Xhtn.TYPE)
            {
                res = new Xhtn(entry, reader);
            }
            else if (entry.TypeID == Xstn.TYPE)
            {
                res = new Xstn(entry, reader);
            }
            else if (entry.TypeID == Xtol.TYPE)
            {
                res = new Xtol(entry, reader);
            }
            //
            // Cigen Resources
            //
            else if (entry.TypeID == Cgn1.TYPE)
            {
                res = new Cgn1(entry, reader);
            }
            //
            // Groups Resources
            //
            else if (entry.TypeID == Grop.TYPE)
            {
                res = new Grop(entry, reader);
            }
            //
            // UI Resources
            //
            else if (entry.TypeID == Ui.TYPE)
            {
                res = new Ui(entry, reader);
            }

            return res;
        }

        public void Close()
        {
            m_Reader?.Close();
        }

        public void Dispose()
        {
            Close();
            m_Reader?.Dispose();
        }
    }
}
