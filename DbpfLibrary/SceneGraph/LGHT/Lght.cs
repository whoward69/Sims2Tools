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
using Sims2Tools.DBPF.SceneGraph.RCOL;
using Sims2Tools.DBPF.SceneGraph.RcolBlocks;
using System;

namespace Sims2Tools.DBPF.SceneGraph.LGHT
{
    public abstract class Lght : Rcol
    {
#if !DEBUG
        private static readonly Logger.IDBPFLogger logger = Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
#endif

        private readonly AbstractLightRcolBlock cBaseLight = null;
        public AbstractLightRcolBlock BaseLight => cBaseLight;

        public Lght(DBPFEntry entry, DbpfReader reader) : base(entry, reader)
        {
            foreach (IRcolBlock block in Blocks)
            {
                if (block.BlockID == CAmbientLight.TYPE || block.BlockID == CDirectionalLight.TYPE || block.BlockID == CPointLight.TYPE || block.BlockID == CSpotLight.TYPE)
                {
                    if (cBaseLight == null)
                    {
                        cBaseLight = block as CDirectionalLight;
                        ogn = cBaseLight.ObjectGraphNode;
                    }
                    else
                    {
#if DEBUG
                        throw new Exception($"2nd cTypeLight found in {this}");
#else
                        logger.Warn($"2nd cTypeLight found in {this}");
#endif
                    }
                }
            }
        }

        public override SgResourceList SgNeededResources()
        {
            return new SgResourceList();
        }

        public bool IsLightValid(string cresName)
        {
            if (!string.IsNullOrEmpty(OgnName)) return false;

            if (!KeyName.Equals(BaseLight.LightT.NameResource.FileName, StringComparison.OrdinalIgnoreCase)) return false;

            if (string.IsNullOrEmpty(BaseLight.Name)) return false;

            if (cresName != null)
            {
                if (!KeyName.Equals($"{cresName}_{BaseLight.Name}_lght", StringComparison.OrdinalIgnoreCase)) return false;
            }

            return true;
        }
    }
}
