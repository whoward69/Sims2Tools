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
using Sims2Tools.DBPF.Package;
using System;
using System.Xml;

namespace Sims2Tools.DBPF
{
    abstract public class DBPFResource : DBPFNamedKey
    {
        protected bool _isDirty = false;

        public void SetKeyNameDirty(string keyName)
        {
            base.KeyName = keyName;
            _isDirty = true;
        }


        public new void ChangeGroupID(TypeGroupID groupID)
        {
            base.ChangeGroupID(groupID);

            _isDirty = true;
        }

        protected DBPFResource(DBPFEntry entry) : base(entry, "")
        {
        }

        #region Serialize
        public virtual bool IsDirty => _isDirty;
        public virtual void SetClean() => _isDirty = false;

        public virtual uint FileSize => throw new NotImplementedException();

        public virtual void Serialize(DbpfWriter writer)
        {
            throw new NotImplementedException();
        }
        #endregion


        #region XML Support
        public abstract XmlElement AddXml(XmlElement parent);
        #endregion
    }
}
