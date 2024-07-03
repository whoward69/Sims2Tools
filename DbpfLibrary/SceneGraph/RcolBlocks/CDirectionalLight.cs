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
using Sims2Tools.DBPF.SceneGraph.RCOL;
using Sims2Tools.DBPF.SceneGraph.RcolBlocks.SubBlocks;
using System.Diagnostics;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks
{
    public class CDirectionalLight : AbstractLightRcolBlock
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeBlockID TYPE = (TypeBlockID)0xC9C81BA3;
        public const string NAME = "cDirectionalLight";

        private readonly StandardLightBase slb = new StandardLightBase();
        private readonly LightT lt = new LightT();
        private readonly ReferentNode rn = new ReferentNode();
        private string unknown2;
        private float unknown3;
        private float unknown4;
        private float red;
        private float green;
        private float blue;

        public override string Name => unknown2;

        public StandardLightBase StandardLightBase => slb;

        public override LightT LightT => lt;

        public ReferentNode ReferentNode => rn;

        public float Val1 => unknown3;

        public float Val2 => unknown4;

        public float Red => red;

        public float Green => green;
        public float Blue => blue;

        public override bool IsDirty => base.IsDirty || slb.IsDirty || lt.IsDirty || rn.IsDirty;

        public override void SetClean()
        {
            base.SetClean();

            slb.SetClean();
            lt.SetClean();
            rn.SetClean();
        }

        // Needed by reflection to create the class
        public CDirectionalLight(Rcol parent) : base(parent)
        {
            Version = 1;
            BlockID = TYPE;
            BlockName = NAME;

            slb.Parent = parent;
            lt.Parent = parent;
            rn.Parent = parent;

            unknown2 = "";
        }

        public override void Unserialize(DbpfReader reader)
        {
#if DEBUG
            readStart = reader.Position;
#endif

            Version = reader.ReadUInt32();

            slb.BlockName = reader.ReadString();
            slb.BlockID = reader.ReadBlockId();
            slb.Unserialize(reader);

            NameResource.BlockName = reader.ReadString();
            NameResource.BlockID = reader.ReadBlockId();
            NameResource.Unserialize(reader);

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

#if DEBUG
            readEnd = reader.Position;
#endif
        }

        public override uint FileSize => GetFileSize();

        private uint GetFileSize()
        {
            long size = 4;

            size += DbpfWriter.Length(slb.BlockName) + 4 + slb.FileSize;
            size += DbpfWriter.Length(NameResource.BlockName) + 4 + NameResource.FileSize;
            size += DbpfWriter.Length(lt.BlockName) + 4 + lt.FileSize;
            size += DbpfWriter.Length(rn.BlockName) + 4 + rn.FileSize;
            size += DbpfWriter.Length(ogn.BlockName) + 4 + ogn.FileSize;

            size += DbpfWriter.Length(unknown2) + 4 + 4;

            size += 4 + 4 + 4;

            return (uint)size;
        }

        public override void Serialize(DbpfWriter writer)
        {
#if DEBUG
            writeStart = writer.Position;
#endif

            writer.WriteUInt32(Version);

            writer.WriteString(slb.BlockName);
            writer.WriteBlockId(slb.BlockID);
            slb.Serialize(writer);

            writer.WriteString(NameResource.BlockName);
            writer.WriteBlockId(NameResource.BlockID);
            NameResource.Serialize(writer);

            writer.WriteString(lt.BlockName);
            writer.WriteBlockId(lt.BlockID);
            lt.Serialize(writer);

            writer.WriteString(rn.BlockName);
            writer.WriteBlockId(rn.BlockID);
            rn.Serialize(writer);

            writer.WriteString(ogn.BlockName);
            writer.WriteBlockId(ogn.BlockID);
            ogn.Serialize(writer);

            writer.WriteString(unknown2);
            writer.WriteSingle(unknown3);
            writer.WriteSingle(unknown4);

            writer.WriteSingle(red);
            writer.WriteSingle(green);
            writer.WriteSingle(blue);

#if DEBUG
            writeEnd = writer.Position;

            Debug.Assert((writeEnd - writeStart) == GetFileSize());
            if (!IsDirty) Debug.Assert((writeEnd - writeStart) == (readEnd - readStart));
#endif
        }

        public override void Dispose()
        {
        }
    }
}
