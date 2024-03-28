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
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks
{
    public class CMaterialDefinition : AbstractRcolBlock
    {
        public static readonly TypeBlockID TYPE = (TypeBlockID)0x49596978;
        public static string NAME = "cMaterialDefinition";

        string fldsc;
        public string FileDescription
        {
            get => fldsc;

            set { fldsc = value; _isDirty = true; }
        }

        string mattype;
        public string MatterialType => mattype;

        private string[] fileList;
        public ReadOnlyCollection<string> FileList => new ReadOnlyCollection<string>(fileList);

        private readonly List<MaterialDefinitionProperty> properties;

        public string GetProperty(string name)
        {
            name = name.Trim().ToLower();
            foreach (MaterialDefinitionProperty mdp in properties)
            {
                if (mdp.Name.Trim().ToLower() == name) return mdp.Value;
            }

            return null;
        }

        public void SetProperty(string name, string value)
        {
            name = name.Trim().ToLower();
            foreach (MaterialDefinitionProperty mdp in properties)
            {
                if (mdp.Name.Trim().ToLower() == name)
                {
                    mdp.Value = value;
                    break;
                }
            }
        }

        public bool AddProperty(string name, string value)
        {
            if (GetProperty(name) != null) return false;

            properties.Add(new MaterialDefinitionProperty(name, value));

            return true;
        }

        public override bool IsDirty
        {
            get
            {
                if (base.IsDirty) return true;

                foreach (MaterialDefinitionProperty mfd in properties)
                {
                    if (mfd.IsDirty) return true;
                }

                return false;
            }
        }

        public override void SetClean()
        {
            base.SetClean();

            foreach (MaterialDefinitionProperty mfd in properties)
            {
                mfd.SetClean();
            }
        }

        // Needed by reflection to create the class
        public CMaterialDefinition(Rcol parent) : base(parent)
        {
            BlockID = TYPE;
            BlockName = NAME;

            fldsc = "";
            mattype = "";

            properties = new List<MaterialDefinitionProperty>();
            fileList = new string[0];
        }

        public override void Unserialize(DbpfReader reader)
        {
#if DEBUG
            readStart = reader.Position;
#endif

            Version = reader.ReadUInt32();

            string blkName = reader.ReadString();
            TypeBlockID blkId = reader.ReadBlockId();

            NameResource.Unserialize(reader);
            NameResource.BlockName = blkName;
            NameResource.BlockID = blkId;

            fldsc = reader.ReadString();
            mattype = reader.ReadString();

            uint propCount = reader.ReadUInt32();
            for (int i = 0; i < propCount; i++)
            {
                properties.Add(new MaterialDefinitionProperty(reader));
            }

            if (Version == 8)
            {
                fileList = new string[0];
            }
            else
            {
                fileList = new string[reader.ReadUInt32()];
                for (int i = 0; i < fileList.Length; i++)
                {
                    fileList[i] = reader.ReadString();
                }
            }

#if DEBUG
            readEnd = reader.Position;
#endif
        }

        public override uint FileSize
        {
            get
            {
                long size = 4;

                size += DbpfWriter.Length(NameResource.BlockName) + 4 + NameResource.FileSize;

                size += DbpfWriter.Length(fldsc);
                size += DbpfWriter.Length(mattype);

                size += 4;
                foreach (MaterialDefinitionProperty mfd in properties)
                {
                    size += mfd.FileSize;
                }

                if (Version != 8)
                {
                    size += 4;
                    foreach (string s in fileList)
                    {
                        size += DbpfWriter.Length(s);
                    }
                }

                return (uint)size;
            }
        }

        public override void Serialize(DbpfWriter writer)
        {
#if DEBUG
            writeStart = writer.Position;
#endif

            writer.WriteUInt32(Version);

            writer.WriteString(NameResource.BlockName);
            writer.WriteBlockId(NameResource.BlockID);
            NameResource.Serialize(writer);

            writer.WriteString(fldsc);
            writer.WriteString(mattype);

            writer.WriteUInt32((uint)properties.Count);
            for (int i = 0; i < properties.Count; i++)
            {
                properties[i].Serialize(writer);
            }

            if (Version != 8)
            {
                writer.WriteUInt32((uint)fileList.Length);
                for (int i = 0; i < fileList.Length; i++)
                {
                    writer.WriteString(fileList[i]);
                }
            }

#if DEBUG
            writeEnd = writer.Position;

            Debug.Assert((writeEnd - writeStart) == FileSize);
            if (!IsDirty) Debug.Assert((writeEnd - writeStart) == (readEnd - readStart));
#endif
        }

        public override void Dispose()
        {
        }
    }

    internal class MaterialDefinitionProperty : IComparable, IEquatable<MaterialDefinitionProperty>
    {

        private string name;
        private string value;

        private bool _isDirty = false;
        internal bool IsDirty => _isDirty;
        internal void SetClean() => _isDirty = false;

        internal string Name => name;

        internal string Value
        {
            get { return this.value; }
            set { this.value = value; _isDirty = true; }
        }

        internal MaterialDefinitionProperty(string name, string value)
        {
            this.name = name;
            this.value = value;
            this._isDirty = true;
        }


        internal MaterialDefinitionProperty(DbpfReader reader)
        {
            Unserialize(reader);
        }

        private void Unserialize(DbpfReader reader)
        {
            name = reader.ReadString();
            value = reader.ReadString();
        }

        internal uint FileSize => (uint)(DbpfWriter.Length(name) + DbpfWriter.Length(value));

        internal void Serialize(DbpfWriter writer)
        {
            writer.WriteString(name);
            writer.WriteString(value);
        }

        public int CompareTo(object obj)
        {
            if (obj is MaterialDefinitionProperty that)
            {
                return this.name.CompareTo(that.name);
            }

            throw new NotImplementedException();
        }

        public bool Equals(MaterialDefinitionProperty that)
        {
            return this.name.Equals(that.name);
        }

        public override string ToString()
        {
            return $"{name}: {value}";
        }
    }
}
