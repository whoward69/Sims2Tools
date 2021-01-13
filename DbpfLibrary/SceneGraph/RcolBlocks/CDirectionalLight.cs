using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.SceneGraph.RCOL;
using Sims2Tools.DBPF.SceneGraph.RcolBlocks.SubBlocks;
using System;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks
{
    public class CDirectionalLight : AbstractRcolBlock
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public const uint TYPE = 0xC9C81BA3;
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
            BlockID = 0xC9C81BA3;

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
            slb.BlockID = reader.ReadUInt32();
            slb.Unserialize(reader);

            sgres.BlockName = reader.ReadString();
            sgres.BlockID = reader.ReadUInt32();
            sgres.Unserialize(reader);

            lt.BlockName = reader.ReadString();
            lt.BlockID = reader.ReadUInt32();
            lt.Unserialize(reader);

            rn.BlockName = reader.ReadString();
            rn.BlockID = reader.ReadUInt32();
            rn.Unserialize(reader);

            ogn.BlockName = reader.ReadString();
            ogn.BlockID = reader.ReadUInt32();
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
