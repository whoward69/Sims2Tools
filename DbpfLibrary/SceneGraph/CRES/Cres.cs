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

using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph.LPNT;
using Sims2Tools.DBPF.SceneGraph.RCOL;
using Sims2Tools.DBPF.SceneGraph.SHPE;
using System;
using System.Collections.Generic;

namespace Sims2Tools.DBPF.SceneGraph.CRES
{
    public class Cres : Rcol
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0xE519C933;
        public const String NAME = "CRES";

        public Cres(DBPFEntry entry, DbpfReader reader) : base(entry, reader)
        {
        }

        public List<DBPFKey> ShpeKeys
        {
            get
            {
                List<DBPFKey> shpeKeys = new List<DBPFKey>();

                for (int i = 0; i < ReferencedFiles.Length; ++i)
                {
                    if (ReferencedFiles[i].Type == Shpe.TYPE)
                    {
                        shpeKeys.Add(ReferencedFiles[i].DbpfKey);
                    }
                }

                return shpeKeys;
            }
        }

        public override SgResourceList SgNeededResources()
        {
            SgResourceList needed = new SgResourceList();

            for (int i = 0; i < ReferencedFiles.Length; ++i)
            {
                if (ReferencedFiles[i].Type == Shpe.TYPE || ReferencedFiles[i].Type == Lpnt.TYPE)
                {
                    needed.Add(ReferencedFiles[i].Type, ReferencedFiles[i].Group, ReferencedFiles[i].SubType, ReferencedFiles[i].Instance);
                }
            }

            return needed;
        }
    }
}
