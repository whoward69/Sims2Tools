/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph.RCOL;
using Sims2Tools.DBPF.SceneGraph.RcolBlocks;
using System;
using System.Xml;

namespace Sims2Tools.DBPF.SceneGraph.ANIM
{
    public class Anim : Rcol
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0xFB00791E;
        public const string NAME = "ANIM";

#if !DEBUG
        private static readonly Logger.IDBPFLogger logger = Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
#endif

        private CAnimResourceConst cAnimData = null;
        public CAnimResourceConst AnimData => cAnimData;

        public override bool IsDirty => base.IsDirty || cAnimData.IsDirty;

        public override void SetClean()
        {
            cAnimData.SetClean();
            base.SetClean();
        }

        public Anim(DBPFEntry entry, DbpfReader reader) : base(entry, reader)
        {
            FindAnimDataBlock();
        }

        private void FindAnimDataBlock()
        {
            foreach (IRcolBlock block in Blocks)
            {
                if (block.BlockID == CAnimResourceConst.TYPE)
                {
                    if (cAnimData == null)
                    {
                        cAnimData = block as CAnimResourceConst;
                    }
                    else
                    {
#if DEBUG
                        throw new Exception($"2nd cAnimData found in {this}");
#else
                        logger.Warn($"2nd cAnimData found in {this}");
#endif
                    }
                }
            }
        }

        public override SgResourceList SgNeededResources()
        {
            return new SgResourceList();
        }

        #region IDBPFScriptable
        public override bool Assignment(string item, ScriptValue sv)
        {
            return base.Assignment(item, sv);
        }

        public override ScriptValue Value(string item)
        {
            if (item.Equals("XXX"))
            {
                throw new NotImplementedException();
            }

            return base.Value(item);
        }
        #endregion

        public override XmlElement AddXml(XmlElement parent)
        {
            return AddXml(parent, NAME);
        }
    }
}
