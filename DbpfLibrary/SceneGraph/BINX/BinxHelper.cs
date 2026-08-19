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

using Sims2Tools.DBPF.CPF;
using Sims2Tools.DBPF.SceneGraph.IDR;

namespace Sims2Tools.DBPF.SceneGraph.BINX
{
    public class BinxHelper
    {
        public static DBPFKey GetKey(string name, Binx binx, Idr idr)
        {
            DBPFKey key = null;

            if (idr != null)
            {
                CpfItem idx = binx.GetItem($"{name}idx");

                if (idx != null)
                {
                    key = idr.GetItem(idx.UIntegerValue);
                }
            }

            if (key == null)
            {
                CpfItem restypeid = binx.GetItem($"{name}restypeid");
                CpfItem groupid = binx.GetItem($"{name}groupid");
                CpfItem id = binx.GetItem($"{name}id");

                if (restypeid != null && groupid != null && id != null)
                {
                    key = new DBPFKey((TypeTypeID)restypeid.UIntegerValue, (TypeGroupID)groupid.UIntegerValue, (TypeInstanceID)id.UIntegerValue, DBPFData.RESOURCE_NULL);
                }
            }

            return key;
        }

        public static DBPFKey ObjectKey(Binx binx, Idr idr) => GetKey("object", binx, idr);
        public static DBPFKey StringSetKey(Binx binx, Idr idr) => GetKey("stringset", binx, idr);
        public static DBPFKey IconKey(Binx binx, Idr idr) => GetKey("icon", binx, idr);
        public static DBPFKey BinKey(Binx binx, Idr idr) => GetKey("bin", binx, idr);
    }
}
