﻿/*
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

using Sims2Tools.DBPF.CPF;
using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.SceneGraph.CRES;
using Sims2Tools.DBPF.SceneGraph.GMDC;
using Sims2Tools.DBPF.SceneGraph.GMND;
using Sims2Tools.DBPF.SceneGraph.IDR;
using Sims2Tools.DBPF.SceneGraph.SHPE;
using Sims2Tools.DBPF.SceneGraph.TXMT;
using Sims2Tools.DBPF.SceneGraph.TXTR;
using Sims2Tools.DBPF.Utils;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Sims2Tools.DBPF.SceneGraph
{
    public interface ISgName
    {
        // Format is ##0xgggggggg!{filename}_tttt
        String SgName { get; }
    }

    public interface ISgHash
    {
        // Format is TTTT-0xGGGGGGGG-0xRRRRRRRR-0xIIIIIIII
        String SgHash { get; }
    }

    public static class SgHelper
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

#if DEBUG
        private static readonly List<TypeTypeID> ImmuneTypes = new List<TypeTypeID>(new TypeTypeID[] { Cres.TYPE, Gmdc.TYPE, Gmnd.TYPE, Shpe.TYPE, Txmt.TYPE, Txtr.TYPE });
#endif

        public static String SgHash(TypeTypeID typeID, TypeGroupID groupID, TypeResourceID resourceID, TypeInstanceID instanceID)
        {
#if DEBUG
            if (groupID == DBPFData.GROUP_LOCAL && !ImmuneTypes.Contains(typeID)) logger.Warn($"Local Group: {DBPFData.TypeName(typeID)}-{groupID}-{resourceID}-{instanceID}");
#endif

            return $"{DBPFData.TypeName(typeID)}-{groupID}-{resourceID}-{instanceID}";
        }

        public static String SgHash(DBPFKey key)
        {
            return SgHash(key.TypeID, key.GroupID, key.ResourceID, key.InstanceID);
        }

        private static String SgName(TypeTypeID typeID, String fileName, String prefix)
        {
            String type = DBPFData.TypeName(typeID).ToLower();

            String suffix = "";

            if (!fileName.ToLower().EndsWith($"_{type}"))
            {
                suffix = $"_{type}";
            }

            // All testing shows that the game is NOT case-sensitive when referencing scene graph resources
            return $"{prefix}{fileName}{suffix}".ToLower();
        }

        private static String SgName(TypeTypeID typeID, TypeGroupID groupID, String fileName)
        {
            String prefix = "";

            if (!fileName.StartsWith("##"))
            {
                prefix = $"##{groupID.ToString().ToLower()}!";
            }

#if DEBUG
            if (groupID == DBPFData.GROUP_LOCAL && !ImmuneTypes.Contains(typeID)) logger.Warn($"Local Group: {SgName(typeID, fileName, prefix)}");
#endif

            return SgName(typeID, fileName, prefix);
        }

        public static String SgName(TypeTypeID typeID, String fileName)
        {
            return SgName(typeID, fileName, "");
        }

        public static String SgName(DBPFNamedKey namedKey)
        {
            return SgName(namedKey.TypeID, namedKey.GroupID, namedKey.FileName);
        }

        public static DBPFKey TGIRFromQualifiedName(string fileName, TypeTypeID typeId, TypeGroupID defGroupId)
        {
            String name = Hashes.StripHashFromName(fileName);
            return new DBPFKey(typeId, Hashes.GetHashGroupFromName(fileName, defGroupId), Hashes.InstanceHash(name), Hashes.SubTypeHash(name));
        }
    }

    public interface ISgResource : IDBPFNamedKey, ISgName, ISgHash
    {
        SgResourceList SgNeededResources();
    }

    public abstract class SgResource : DBPFResource, ISgResource
    {
        private String sgHash = null;
        private String sgName = null;

        public String SgHash
        {
            get
            {
                if (sgHash == null) sgHash = SgHelper.SgHash(this);
                return sgHash;
            }
        }
        public String SgName
        {
            get
            {
                if (sgName == null) sgName = SgHelper.SgName(this);
                return sgName;
            }
        }

        public SgResource(DBPFEntry entry) : base(entry)
        {
        }

        public abstract SgResourceList SgNeededResources();

        public override void AddXml(XmlElement parent)
        {
            CreateComment(parent, $"{SgHash} - '{SgName}'");
        }
    }

    public interface ISgRefResource : ISgResource
    {
        List<uint> SgIdrIndexes();
    }

    public abstract class SgRefResource : SgResource, ISgRefResource
    {
        public SgRefResource(DBPFEntry entry) : base(entry)
        {
        }

        public abstract List<uint> SgIdrIndexes();
    }

    public abstract class SgCpf : Cpf, ISgResource
    {
        private readonly String sgHash;
        private readonly String sgName;

        public String SgHash => sgHash;
        public String SgName => sgName;

        public SgCpf(DBPFEntry entry, IoBuffer reader) : base(entry, reader)
        {
            sgHash = SgHelper.SgHash(this);
            sgName = SgHelper.SgName(this);
        }

        public abstract SgResourceList SgNeededResources();

        public override void AddXml(XmlElement parent)
        {
            CreateComment(parent, $"{SgHash} - '{SgName}'");
        }
    }

    public abstract class SgRefCpf : SgCpf, ISgRefResource
    {
        protected readonly SgResourceList sgResourceList = new SgResourceList(1);
        protected readonly List<uint> sgIdrIndexes = new List<uint>(1);

        public SgRefCpf(DBPFEntry entry, IoBuffer reader) : base(entry, reader)
        {
            sgResourceList.Add(SgHelper.SgHash(Idr.TYPE, this.GroupID, this.ResourceID, this.InstanceID));
        }

        public override SgResourceList SgNeededResources()
        {
            return sgResourceList;
        }

        public virtual List<uint> SgIdrIndexes()
        {
            return sgIdrIndexes;
        }
    }

    public class SgResourceList : List<String>
    {
        public SgResourceList(int size) : base(size)
        {
        }

        public SgResourceList() : this(0)
        {
        }

        public void Add(TypeTypeID typeID, string fileName)
        {
            base.Add(SgHelper.SgName(typeID, fileName));
        }

        public void Add(TypeTypeID typeID, TypeGroupID groupID, TypeResourceID resourceID, TypeInstanceID instanceID)
        {
            base.Add(SgHelper.SgHash(typeID, groupID, resourceID, instanceID));
        }
    }
}
