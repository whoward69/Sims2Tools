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
using Sims2Tools.DBPF.SceneGraph.RCOL;
using System;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks
{
    public class CMaterialDefinition : AbstractRcolBlock
    {
        public static readonly TypeBlockID TYPE = (TypeBlockID)0x49596978;
        public static String NAME = "cMaterialDefinition";

        string fldsc;
        public string FileDescription
        {
            get { return fldsc; }
            set { fldsc = value; }
        }

        string mattype;
        public string MatterialType
        {
            get { return mattype; }
            set { mattype = value; }
        }

        MaterialDefinitionProperty[] properties;
        public MaterialDefinitionProperty[] Properties
        {
            get { return properties; }
            set { properties = value; }
        }

        string[] listing;
        public string[] Listing
        {
            get { return listing; }
            set { listing = value; }
        }

        public CMaterialDefinition(Rcol parent) : base(parent)
        {
            properties = new MaterialDefinitionProperty[0];
            listing = new String[0];
            sgres = new SGResource(null);
            BlockID = TYPE;
            fldsc = "";
            mattype = "";
        }

        public MaterialDefinitionProperty GetProperty(string name)
        {
            name = name.Trim().ToLower();
            foreach (MaterialDefinitionProperty mdp in properties)
            {
                if (mdp.Name.Trim().ToLower() == name) return mdp;
            }

            return null;
        }

        public override void Unserialize(DbpfReader reader)
        {
            version = reader.ReadUInt32();

            fldsc = reader.ReadString();

            TypeBlockID myid = reader.ReadBlockId();
            sgres.Unserialize(reader);
            sgres.BlockID = myid;

            fldsc = reader.ReadString();
            mattype = reader.ReadString();

            properties = new MaterialDefinitionProperty[reader.ReadUInt32()];
            for (int i = 0; i < properties.Length; i++)
            {
                properties[i] = new MaterialDefinitionProperty();
                properties[i].Unserialize(reader);
            }

            if (version == 8)
            {
                listing = new String[0];
            }
            else
            {
                listing = new String[reader.ReadUInt32()];
                for (int i = 0; i < listing.Length; i++)
                {
                    listing[i] = reader.ReadString();
                }
            }
        }

        public override void Dispose()
        {
        }
    }

    public class MaterialDefinitionProperty
    {

        string name;
        string val;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Value
        {
            get { return val; }
            set { val = value; }
        }


        public MaterialDefinitionProperty()
        {
            name = "";
            val = "";
        }

        public void Unserialize(DbpfReader reader)
        {

            name = reader.ReadString();
            val = reader.ReadString();
        }
    }
}
