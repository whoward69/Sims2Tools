/*
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

using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.SceneGraph.RCOL;
using Sims2Tools.DBPF.SceneGraph.RcolBlocks.SubBlocks;
using System;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks
{
    public class CDirectionalLight : AbstractRcolBlock
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeBlockID TYPE = (TypeBlockID)0xC9C81BA3;
        public const String NAME = "cDirectionalLight";

        #region Attributes

        StandardLightBase slb;
        public StandardLightBase StandardLightBase
        {
            get { return slb; }
            set { slb = value; }
        }

        LightT lt;
        public LightT LightT
        {
            get { return lt; }
            set { lt = value; }
        }

        ReferentNode rn;
        public ReferentNode ReferentNode
        {
            get { return rn; }
            set { rn = value; }
        }

        ObjectGraphNode ogn;
        public ObjectGraphNode ObjectGraphNode
        {
            get { return ogn; }
            set { ogn = value; }
        }


        string unknown2;
        public string Name
        {
            get { return unknown2; }
            set { unknown2 = value; }
        }

        float unknown3;
        public float Val1
        {
            get { return unknown3; }
            set { unknown3 = value; }
        }

        float unknown4;
        public float Val2
        {
            get { return unknown4; }
            set { unknown4 = value; }
        }

        float red;
        public float Red
        {
            get { return red; }
            set { red = value; }
        }

        float green;
        public float Green
        {
            get { return green; }
            set { green = value; }
        }

        float blue;
        public float Blue
        {
            get { return blue; }
            set { blue = value; }
        }


        #endregion


        /// <summary>
        /// Constructor
        /// </summary>
        public CDirectionalLight(Rcol parent) : base(parent)
        {
            version = 1;
            BlockID = TYPE;

            slb = new StandardLightBase(null);
            sgres = new SGResource(null);
            lt = new LightT(null);
            rn = new ReferentNode(null);
            ogn = new ObjectGraphNode(null);

            unknown2 = "";
        }

        #region IRcolBlock Member

        /// <summary>
        /// Unserializes a BinaryStream into the Attributes of this Instance
        /// </summary>
        /// <param name="reader">The Stream that contains the FileData</param>
        public override void Unserialize(IoBuffer reader)
        {
            version = reader.ReadUInt32();

            slb.BlockName = reader.ReadString();
            slb.BlockID = reader.ReadBlockId();
            slb.Unserialize(reader);

            sgres.BlockName = reader.ReadString();
            sgres.BlockID = reader.ReadBlockId();
            sgres.Unserialize(reader);

            lt.BlockName = reader.ReadString();
            lt.BlockID = reader.ReadBlockId();
            lt.Unserialize(reader);

            rn.BlockName = reader.ReadString();
            rn.BlockID = reader.ReadBlockId();
            rn.Unserialize(reader);

            ogn.BlockName = reader.ReadString();
            ogn.BlockID = reader.ReadBlockId();
            ogn.Unserialize(reader);

            unknown2 = reader.ReadString();
            unknown3 = reader.ReadSingle();
            unknown4 = reader.ReadSingle();
            red = reader.ReadSingle();
            green = reader.ReadSingle();
            blue = reader.ReadSingle();
        }
        #endregion

        public override void Dispose()
        {
        }
    }
}
