/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2025
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.SceneGraph.RCOL;
using Sims2Tools.DBPF.SceneGraph.RcolBlocks.SubBlocks;
using System.Xml;

namespace Sims2Tools.DBPF.SceneGraph
{
    public interface IRcolBlock : System.IDisposable
    {
        string BlockName
        {
            get;
            set;
        }

        TypeBlockID BlockID
        {
            get;
            set;
        }

        SGResource NameResource
        {
            get;
        }

        bool IsDirty
        {
            get;
        }

        void SetClean();

        void Unserialize(DbpfReader reader);

        uint FileSize { get; }

        void Serialize(DbpfWriter writer);

        IRcolBlock Duplicate(Rcol parent);

        XmlElement AddXml(XmlElement parent);
    }
}
