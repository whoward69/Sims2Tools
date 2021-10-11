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
using System.Collections;

namespace Sims2Tools.DBPF.SceneGraph.Geometry

{
    public class VectorTransformation
    {
        public const double SMALL_NUMBER = 0.000001;
        /// <summary>
        /// What Order should the Transformation be applied
        /// </summary>
        public enum TransformOrder : byte
        {
            /// <summary>
            /// Rotate then Translate
            /// </summary>
            RotateTranslate = 0,
            /// <summary>
            /// Translate then Rotate (rigid Body)
            /// </summary>
            TranslateRotate = 1
        };


        TransformOrder o;
        /// <summary>
        /// Returns / Sets the current Order
        /// </summary>
        public TransformOrder Order
        {
            get { return o; }
            set { o = value; }
        }
        Vector3f trans;
        /// <summary>
        /// The Translation
        /// </summary>
        public Vector3f Translation
        {
            get { return trans; }
            set { trans = value; }
        }

        Quaternion quat;
        /// <summary>
        /// The Rotation
        /// </summary>
        public Quaternion Rotation
        {
            get { return quat; }
            set { quat = value; }
        }


        /// <summary>
        /// Create a new Instance
        /// </summary>
        /// <param name="o">The order of the Transform</param>
        public VectorTransformation(TransformOrder o)
        {
            this.o = o;
            trans = new Vector3f();
            quat = Quaternion.Identity;
        }

        /// <summary>
        /// Create a new Instance
        /// </summary>
        /// <remarks>Order is implicit set to <see cref="TransformOrder.TranslateRotate"/></remarks>
        public VectorTransformation() : this(TransformOrder.TranslateRotate)
        {
        }

        public override string ToString()
        {
            return $"trans={trans}    rot={quat}";
        }

        /// <summary>
        /// Unserializes a BinaryStream into the Attributes of this Instance
        /// </summary>
        /// <param name="reader">The Stream that contains the FileData</param>
        public virtual void Unserialize(IoBuffer reader)
        {
            if (o == TransformOrder.RotateTranslate)
            {
                quat.Unserialize(reader);
                trans.Unserialize(reader);
            }
            else
            {
                trans.Unserialize(reader);
                quat.Unserialize(reader);
            }
        }

        /// <summary>
        /// Serializes a the Attributes stored in this Instance to the BinaryStream
        /// </summary>
        /// <param name="writer">The Stream the Data should be stored to</param>
        /// <remarks>
        /// Be sure that the Position of the stream is Proper on 
        /// return (i.e. must point to the first Byte after your actual File)
        /// </remarks>
        public virtual void Serialize(System.IO.BinaryWriter writer)
        {
            if (o == TransformOrder.RotateTranslate)
            {
                quat.Serialize(writer);
                trans.Serialize(writer);
            }
            else
            {
                trans.Serialize(writer);
                quat.Serialize(writer);
            }
        }

        /// <summary>
        /// Applies the Transformation to the passed Vertex
        /// </summary>
        /// <param name="v">The Vertex you want to Transform</param>
        /// <returns>Transformed Vertex</returns>
        public Vector3f Transform(Vector3f v)
        {
            if (o == TransformOrder.RotateTranslate)
            {
                v = quat.Rotate(v);
                return v + trans;
            }
            else
            {
                v += trans;
                return quat.Rotate(v);
            }
        }

        /// <summary>
        /// Create a Clone of this Transformation Set
        /// </summary>
        /// <returns></returns>
        public VectorTransformation Clone()
        {
            VectorTransformation v = new VectorTransformation(this.Order)
            {
                Rotation = Rotation.Clone(),
                Translation = Translation.Clone()
            };

            return v;
        }

#if DEBUG
        string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
#endif
    }

    /// <summary>
    /// Typesave ArrayList for VectorTransformation Objects
    /// </summary>
    public class VectorTransformations : ArrayList
    {
        /// <summary>
        /// Integer Indexer
        /// </summary>
        public new VectorTransformation this[int index]
        {
            get { return ((VectorTransformation)base[index]); }
            set { base[index] = value; }
        }

        /// <summary>
        /// unsigned Integer Indexer
        /// </summary>
        public VectorTransformation this[uint index]
        {
            get { return ((VectorTransformation)base[(int)index]); }
            set { base[(int)index] = value; }
        }

        /// <summary>
        /// add a new Element
        /// </summary>
        /// <param name="item">The object you want to add</param>
        /// <returns>The index it was added on</returns>
        public int Add(VectorTransformation item)
        {
            return base.Add(item);
        }

        /// <summary>
        /// insert a new Element
        /// </summary>
        /// <param name="index">The Index where the Element should be stored</param>
        /// <param name="item">The object that should be inserted</param>
        public void Insert(int index, VectorTransformation item)
        {
            base.Insert(index, item);
        }

        /// <summary>
        /// remove an Element
        /// </summary>
        /// <param name="item">The object that should be removed</param>
        public void Remove(VectorTransformation item)
        {
            base.Remove(item);
        }

        /// <summary>
        /// Checks wether or not the object is already stored in the List
        /// </summary>
        /// <param name="item">The Object you are looking for</param>
        /// <returns>true, if it was found</returns>
        public bool Contains(VectorTransformation item)
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
            VectorTransformations list = new VectorTransformations();
            foreach (VectorTransformation item in this) list.Add(item);

            return list;
        }
    }

}
