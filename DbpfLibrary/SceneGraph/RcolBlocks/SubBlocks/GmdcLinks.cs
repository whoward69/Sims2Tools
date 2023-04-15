/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2023
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF.IO;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks.SubBlocks
{
    public class GmdcLink : GmdcLinkBlock
    {


        List<int> items1;
        /// <summary>
        /// This returns the List of all used <see cref="GmdcElement"/> Items. The Values are Indices
        /// for the <see cref="CGeometryDataContainer.Elements"/> Property.
        /// </summary>
        public List<int> ReferencedElement
        {
            get { return items1; }
            set { items1 = value; }
        }

        int unknown1;
        /// <summary>
        /// The Number of Elements that are Referenced by this Link
        /// </summary>
        public int ReferencedSize
        {
            get { return unknown1; }
            set { unknown1 = value; }
        }

        int unknown2;
        /// <summary>
        /// How many <see cref="GmdcElement"/> Items are referenced by this Link
        /// </summary>
        public int ActiveElements
        {
            get { return unknown2; }
            set { unknown2 = value; }
        }

        readonly List<int>[] refs;
        /// <summary>
        /// This Array Contains three <see cref="IntArrayList"/> Items. Each Item has to be interporeted as 
        /// Element Index Alias.
        /// The <see cref="GmdcGroup"/> is referencing the Vertices that form a Face by an Index. If one of 
        /// this Lists is set, it means, that whenever you pares an Index, read the value stored at that Index 
        /// in one of this Lists. The Value read from there is then thge actual <see cref="GmdcElement"/> Index.
        /// 
        /// The first List store here is an Alias Map for the first referenced <see cref="GmdcElement"/> in the
        /// <see cref="ReferencedElement"/> Property.
        /// </summary>
        public List<int>[] AliasValues
        {
            get { return refs; }
        }


        /// <summary>
        /// Constructor
        /// </summary>
        public GmdcLink(CGeometryDataContainer parent) : base(parent)
        {
            items1 = new List<int>();
            refs = new List<int>[3];
            for (int i = 0; i < refs.Length; i++) refs[i] = new List<int>();
        }



        /// <summary>
        /// Unserializes a BinaryStream into the Attributes of this Instance
        /// </summary>
        /// <param name="reader">The Stream that contains the FileData</param>
        public void Unserialize(DbpfReader reader)
        {
            ReadBlock(reader, items1);

            unknown1 = reader.ReadInt32();
            unknown2 = reader.ReadInt32();

            for (int i = 0; i < refs.Length; i++) ReadBlock(reader, refs[i]);
        }

        /// <summary>
        /// This output is used in the ListBox View
        /// </summary>
        /// <returns>A String Describing the Data</returns>
        public override string ToString()
        {
            string s = items1.Count.ToString();
            for (int i = 0; i < refs.Length; i++) s += ", " + refs[i].Count;
            return s;
        }

        /// <summary>
        /// Returns the first Element Referenced from this Link that implements 
        /// the passed <see cref="ElementIdentity"/>.
        /// </summary>
        /// <param name="id">Identity you are looking for</param>
        /// <returns>null or the First mathcing Element</returns>
        public GmdcElement FindElementType(ElementIdentity id)
        {
            foreach (int i in this.ReferencedElement)
            {
                if (parent.Elements[i].Identity == id) return parent.Elements[i];
            }

            return null;
        }

        /// <summary>
        /// Returns the nr (as it can be used in GetValue() Method) of the passed Element in this Link Section
        /// </summary>
        /// <param name="e">Element you are looking for</param>
        /// <returns>
        /// -1 if the Element is not referenced from this Link or the index of that Element in the 
        /// ReferenceElement Member
        /// </returns>
        public int GetElementNr(GmdcElement e)
        {
            if (e == null) return -1;
            for (int i = 0; i < this.items1.Count; i++)
                if (parent.Elements[items1[i]] == e) return i;

            return -1;
        }

        /// <summary>
        /// Returns a specific Value
        /// </summary>
        /// <param name="nr">The Number of the referenced Element (index to the ReferencedElement Member)</param>
        /// <param name="index">The index of the value you want to read from thet Element</param>
        /// <returns>The stored Value or null on error</returns>
        /// <remarks>To retrieve the correct number for an Element, see the GetElementNr() Method</remarks>
        public GmdcElementValueBase GetValue(int nr, int index)
        {
            try
            {
                //if (nr>=this.items1.Length) return null;
                int enr = this.items1[nr];

                //if (enr>=this.parent.Elements.Length) return null;
                GmdcElement e = this.parent.Elements[enr];

                //Higher Number
                if (nr >= refs.Length)
                {
                    //if (index>=e.Values.Length) return null;
                    return e.Values[index];
                }

                //Do we have aliases?
                if (refs[nr].Count == 0) //no
                {
                    //if (index>=e.Values.Length) return null;
                    return e.Values[index];
                }
                else //yes
                {
                    //if (index>=this.refs.Length) return null;
                    index = refs[nr][index];
                    //if (index>=e.Values.Length) return null;
                    return e.Values[index];
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Returns a specific Value
        /// </summary>
        /// <param name="nr">The Number of the referenced Element (index to the ReferencedElement Member)</param>
        /// <param name="index">The index of the value you want to read from thet Element</param>
        /// <returns>-1 or an Element Index</returns>
        /// <remarks>To retrieve the correct number for an Element, see the GetElementNr() Method</remarks>
        public int GetRealIndex(int nr, int index)
        {
            try
            {
                int enr = this.items1[nr];

                GmdcElement e = this.parent.Elements[enr];

                //Higher Number
                if (nr >= refs.Length) return index;

                //Do we have aliases?
                if (refs[nr].Count == 0) return index;
                else return refs[nr][index];
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// Returns the value for <see cref="ReferencedSize"/> for the current Settings
        /// </summary>
        /// <returns>The suggested value for <see cref="ReferencedSize"/></returns>
        public int GetReferencedSize()
        {
            int minct = int.MaxValue;
            //add all populated Element Lists
            for (int k = 0; k < this.items1.Count; k++)
            {
                int id = items1[k];
                if (parent.Elements[id].Values.Length > 0)
                    minct = Math.Min(minct, parent.Elements[id].Values.Count);
            } // for k
            if (minct == int.MaxValue) minct = 0;

            int res = minct;

            //If we have AliasLists, the we need that Number as Reference Count!
            minct = int.MaxValue;
            for (int i = 0; i < AliasValues.Length; i++) if (AliasValues[i].Count > 0) minct = Math.Min(minct, AliasValues[i].Count);
            if (minct != int.MaxValue) res = minct;

            return res;
        }

        /// <summary>
        /// Makes sure that the Aliaslists are not used!
        /// </summary>
        public void Flatten()
        {
            GmdcElement vn = new GmdcElement(this.parent);
            GmdcElement vt = new GmdcElement(this.parent);

            GmdcElement ovn = this.FindElementType(ElementIdentity.Normal);
            GmdcElement ovt = this.FindElementType(ElementIdentity.UVCoordinate);

            //contains a List of all additional Elements assigned to this Link, which 
            //are related to the Vertex Element (like BoneWeights)
            GmdcElements ovelements = new GmdcElements();
            GmdcElements velements = new GmdcElements();
            ovelements.Add(this.FindElementType(ElementIdentity.Vertex));
            velements.Add(new GmdcElement(this.Parent));

            int nv = this.GetElementNr(ovelements[0]);
            int nvn = this.GetElementNr(ovn);
            int nvt = this.GetElementNr(ovt);

            //add all other Elements
            foreach (int i in this.ReferencedElement)
            {
                if (ovelements.Contains(parent.Elements[i]) || parent.Elements[i] == ovn || parent.Elements[i] == ovt) continue;
                ovelements.Add(parent.Elements[i]);
                velements.Add(new GmdcElement(this.Parent));
            }

            for (int i = 0; i < this.ReferencedSize; i++)
            {
                for (int j = 0; j < velements.Length; j++) velements[j].Values.Add(ovelements[j].Values[this.GetRealIndex(nv, i)]);

                if (ovn != null) vn.Values.Add(ovn.Values[this.GetRealIndex(nvn, i)]);
                if (ovt != null) vt.Values.Add(ovt.Values[this.GetRealIndex(nvt, i)]);
            }

            for (int i = 0; i < velements.Length; i++)
            {
                ovelements[i].Values = velements[i].Values;
                ovelements[i].Number = velements[i].Number;
            }
            if (ovn != null)
            {
                ovn.Values = vn.Values;
                ovn.Number = ReferencedSize;
            }
            if (ovt != null)
            {
                ovt.Values = vt.Values;
                ovt.Number = ReferencedSize;
            }

            for (int i = 0; i < this.AliasValues.Length; i++) this.AliasValues[i].Clear();
        }
    }

    /// <summary>
    /// Typesave ArrayList for GmdcLink Objects
    /// </summary>
    public class GmdcLinks : ArrayList
    {
        /// <summary>
        /// Integer Indexer
        /// </summary>
        public new GmdcLink this[int index]
        {
            get { return ((GmdcLink)base[index]); }
            set { base[index] = value; }
        }

        /// <summary>
        /// unsigned Integer Indexer
        /// </summary>
        public GmdcLink this[uint index]
        {
            get { return ((GmdcLink)base[(int)index]); }
            set { base[(int)index] = value; }
        }

        /// <summary>
        /// add a new Element
        /// </summary>
        /// <param name="item">The object you want to add</param>
        /// <returns>The index it was added on</returns>
        public int Add(GmdcLink item)
        {
            return base.Add(item);
        }

        /// <summary>
        /// insert a new Element
        /// </summary>
        /// <param name="index">The Index where the Element should be stored</param>
        /// <param name="item">The object that should be inserted</param>
        public void Insert(int index, GmdcLink item)
        {
            base.Insert(index, item);
        }

        /// <summary>
        /// remove an Element
        /// </summary>
        /// <param name="item">The object that should be removed</param>
        public void Remove(GmdcLink item)
        {
            base.Remove(item);
        }

        /// <summary>
        /// Checks wether or not the object is already stored in the List
        /// </summary>
        /// <param name="item">The Object you are looking for</param>
        /// <returns>true, if it was found</returns>
        public bool Contains(GmdcLink item)
        {
            return base.Contains(item);
        }

        /// <summary>
        /// Number of stored Elements
        /// </summary>
        public int Length
        {
            get { return this.Count; }
        }

        /// <summary>
        /// Create a clone of this Object
        /// </summary>
        /// <returns>The clone</returns>
        public override object Clone()
        {
            GmdcLinks list = new GmdcLinks();
            foreach (GmdcLink item in this) list.Add(item);

            return list;
        }
    }

}
