/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2022
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF.Cigen.CGN1;
using Sims2Tools.DBPF.Package;
using System;

namespace Sims2Tools.DBPF.Cigen
{
    public class CigenFile : IDisposable
    {
        private readonly string cigenPath;
        private readonly DBPFFile cigenPackage = null;

        private readonly Cgn1 cigenIndex;

        public CigenFile(string cigenPath)
        {
            this.cigenPath = cigenPath;
            cigenPackage = new DBPFFile(cigenPath);

            cigenIndex = (Cgn1)cigenPackage?.GetResourceByKey(new DBPFKey(Cgn1.TYPE, DBPFData.GROUP_LOCAL, (TypeInstanceID)0x00000001, (TypeResourceID)0x00000000));
        }

        public DBPFKey GetImageKey(DBPFKey ownerKey)
        {
            return cigenIndex?.GetImageKey(ownerKey);
        }

        public void Close()
        {
            cigenPackage?.Close();
        }

        public void Dispose()
        {
            Close();
            cigenPackage?.Dispose();
        }
    }
}
