/*
 * Family Manager - a utility for manipulating family closets
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF;
using Sims2Tools.DBPF.Neighbourhood.FAMI;
using Sims2Tools.DBPF.SceneGraph.BINX;
using Sims2Tools.DBPF.SceneGraph.IDR;
using Sims2Tools.DbpfCache;
using System;
using System.Xml;

namespace FamilyManager
{
    public class FamilyDbpfData : IEquatable<FamilyDbpfData>
    {
        private static readonly Sims2Tools.DBPF.Logger.IDBPFLogger logger = Sims2Tools.DBPF.Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static DbpfFileCache cache;
        public static void SetCache(DbpfFileCache cache)
        {
            FamilyDbpfData.cache = cache;
        }

        private readonly string packagePath;
        private readonly Fami fami;

        public string PackagePath => packagePath;

        public FamilyDbpfData(string packagePath, Fami fami)
        {
            this.packagePath = packagePath;
            this.fami = fami;
        }

        public bool Equals(FamilyDbpfData other)
        {
            return this.packagePath.Equals(other.packagePath) && this.fami.Equals(other.fami);
        }

        public override string ToString()
        {
            return fami.ToString();
        }
    }

    public class OutfitDbpfData : IEquatable<OutfitDbpfData>
    {
        private static readonly Sims2Tools.DBPF.Logger.IDBPFLogger logger = Sims2Tools.DBPF.Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static DbpfFileCache cache;
        public static void SetCache(DbpfFileCache cache)
        {
            OutfitDbpfData.cache = cache;
        }

        private string packagePath;

        private DBPFKey cpfKey; // GZPS (clothing) or XMOL (jewellery)

        private Idr idr = null;
        private Binx binx = null;

        public string PackagePath => packagePath;
        public Idr OutfitIdr => idr;
        public Binx OutfitBinx => binx;

        public DBPFKey CpfKey => cpfKey;

        public static OutfitDbpfData Create(CacheableDbpfFile package, Idr idr)
        {
            return new OutfitDbpfData(package, idr);
        }

        private OutfitDbpfData(CacheableDbpfFile package, Idr idr)
        {
            this.packagePath = package.PackagePath;
            this.idr = idr;

            this.binx = (Binx)package.GetResourceByKey(new DBPFKey(Binx.TYPE, idr));

            this.cpfKey = idr.GetItem(2);
        }

        public OutfitDbpfData(XmlReader reader)
        {
            ReadXml(reader);
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("idr");
            writer.WriteAttributeString("version", "1.0");

            writer.WriteElementString("path", packagePath);
            writer.WriteElementString("key", idr.TGRIString);

            writer.WriteEndElement();
        }

        public void ReadXml(XmlReader reader)
        {
            DBPFKey idrKey = null;

            bool wantPath = false;
            bool wantKey = false;

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name.Equals("path"))
                    {
                        wantPath = true;
                    }
                    else if (reader.Name.Equals("key"))
                    {
                        wantKey = true;
                    }
                }
                else if (reader.NodeType == XmlNodeType.Text)
                {
                    if (wantPath)
                    {
                        packagePath = reader.Value;
                        wantPath = false;
                    }
                    else if (wantKey)
                    {
                        idrKey = new DBPFKey(reader.Value);
                        wantKey = false;
                    }
                }
                else if (reader.NodeType == XmlNodeType.EndElement)
                {
                    if (reader.Name.Equals("idr"))
                    {
                        if (idrKey != null)
                        {
                            using (CacheableDbpfFile package = cache.OpenForReadOnly(packagePath))
                            {
                                idr = (Idr)package.GetResourceByKey(idrKey);
                                cpfKey = idr?.GetItem(2);

                                binx = (Binx)package.GetResourceByKey(new DBPFKey(Binx.TYPE, idrKey));

                                package.Close();
                            }
                        }

                        return;
                    }
                }
            }
        }

        public bool Equals(OutfitDbpfData other)
        {
            return this.packagePath.Equals(other.packagePath) && this.idr.Equals(other.idr);
        }

        public override string ToString()
        {
            return idr.ToString();
        }
    }
}
