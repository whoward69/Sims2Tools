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

namespace Sims2Tools.DBPF.SceneGraph.IDR
{
    public class IdrHelper
    {
        public static DBPFKey GetKey(string name, Cpf cpf, Idr idr)
        {
            DBPFKey key = null;

            if (idr != null)
            {
                CpfItem idx = cpf.GetItem($"{name}idx");

                if (idx != null)
                {
                    key = idr.GetItem(idx.UIntegerValue);
                }
            }

            if (key == null)
            {
                CpfItem restypeid = cpf.GetItem($"{name}restypeid");
                CpfItem groupid = cpf.GetItem($"{name}groupid");
                CpfItem id = cpf.GetItem($"{name}id");

                if (restypeid != null && groupid != null && id != null)
                {
                    key = new DBPFKey((TypeTypeID)restypeid.UIntegerValue, (TypeGroupID)groupid.UIntegerValue, (TypeInstanceID)id.UIntegerValue, DBPFData.RESOURCE_NULL);
                }
            }

            return key;
        }

        public static DBPFKey ObjectKey(Cpf cpf, Idr idr) => GetKey("object", cpf, idr);
        public static DBPFKey StringSetKey(Cpf cpf, Idr idr) => GetKey("stringset", cpf, idr);
        public static DBPFKey IconKey(Cpf cpf, Idr idr) => GetKey("icon", cpf, idr);
        public static DBPFKey BinKey(Cpf cpf, Idr idr) => GetKey("bin", cpf, idr);
    }
}
