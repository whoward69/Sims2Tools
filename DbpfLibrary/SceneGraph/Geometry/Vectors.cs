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
using System;
using System.Collections;

namespace Sims2Tools.DBPF.SceneGraph.Geometry
{
    [System.ComponentModel.TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class Vector2f
    {
        double x, y;

        /// <summary>
        /// The X Coordinate of teh Vector
        /// </summary>
        public double X
        {
            get
            {
                if (double.IsNaN(x))
                    return 0;
                return x;
            }
            set { x = value; }
        }
        /// <summary>
        /// The Y Coordinate of teh Vector
        /// </summary>
        public double Y
        {
            get
            {
                if (double.IsNaN(y)) return 0;
                return y;
            }
            set { y = value; }
        }


        /// <summary>
        /// Creates a new Vector Instance (0-Vector)
        /// </summary>
        public Vector2f()
        {
            x = 0; y = 0;
        }

        /// <summary>
        /// Creates new Vector Instance
        /// </summary>
        /// <param name="x">X-Coordinate</param>
        /// <param name="y">Y-Coordinate</param>
        public Vector2f(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        protected double EpsilonCorrect(double v)
        {
            if (Math.Abs(v) < 0.00001) return 0;
            return v;
        }

        /// <summary>
        /// Unserializes a BinaryStream into the Attributes of this Instance
        /// </summary>
        /// <param name="reader">The Stream that contains the FileData</param>
        public virtual void Unserialize(IoBuffer reader)
        {
            x = (reader.ReadSingle());
            y = (reader.ReadSingle());
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
            writer.Write((float)x);
            writer.Write((float)y);
        }

        /// <summary>
        /// This output is used in the ListBox View
        /// </summary>
        /// <returns>A String Describing the Data</returns>
        public override string ToString()
        {
            return x.ToString("N2") + "; " + y.ToString("N2");
        }

        /// <summary>
        /// Create a clone of this Vector
        /// </summary>
        /// <returns></returns>
        public Vector2f Clone()
        {
            Vector2f v = new Vector2f(this.X, this.Y);
            return v;
        }

        public static Vector2f Zero
        {
            get { return new Vector2f(0, 0); }
        }
    }

    /// <summary>
    /// Contains the a 3D Vector (when (un)serialized, it will be interpreted as SingleFloat!)
    /// </summary>
    [System.ComponentModel.TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class Vector3f : Vector2f
    {
        double z;

        /// <summary>
        /// The Z Coordinate of teh Vector
        /// </summary>
        public double Z
        {
            get
            {
                if (double.IsNaN(z)) return 0;
                return z;
            }
            set { z = value; }
        }

        /// <summary>
        /// Creates a new Vector Instance (0-Vector)
        /// </summary>
        public Vector3f() : base()
        {
            z = 0;
        }

        /// <summary>
        /// Creates new Vector Instance
        /// </summary>
        /// <param name="x">X-Coordinate</param>
        /// <param name="y">Y-Coordinate</param>
        /// <param name="z">Z-Coordinate</param>
        public Vector3f(double x, double y, double z) : base(x, y)
        {
            this.z = z;
        }

        /// <summary>
        /// Unserializes a BinaryStream into the Attributes of this Instance
        /// </summary>
        /// <param name="reader">The Stream that contains the FileData</param>
        public override void Unserialize(IoBuffer reader)
        {
            base.Unserialize(reader);
            z = (reader.ReadSingle());
        }

        /// <summary>
        /// Serializes a the Attributes stored in this Instance to the BinaryStream
        /// </summary>
        /// <param name="writer">The Stream the Data should be stored to</param>
        /// <remarks>
        /// Be sure that the Position of the stream is Proper on 
        /// return (i.e. must point to the first Byte after your actual File)
        /// </remarks>
        public override void Serialize(System.IO.BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.Write((float)z);
        }

        /// <summary>
        /// This output is used in the ListBox View
        /// </summary>
        /// <returns>A String Describing the Data</returns>
        public override string ToString()
        {
            return base.ToString() + "; " + z.ToString("N2");
        }

        /// <summary>
        /// Returns the UnitVector for this Vector
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        public Vector3f UnitVector
        {
            get
            {
                Vector3f uv = new Vector3f();

                double l = Length;
                if (l != 0)
                {
                    uv.X = X / l;
                    uv.Y = Y / l;
                    uv.Z = Z / l;
                }
                return uv;
            }
        }

        /// <summary>
        /// Makes sure this Vector is a Unit Vector (Length=1)
        /// </summary>
        public void MakeUnitVector()
        {
            Vector3f uv = UnitVector;
            X = uv.X;
            Y = uv.Y;
            Z = uv.Z;
        }

        /// <summary>
        /// Returns the Norm of the Vector
        /// </summary>
        public double Norm
        {
            get
            {
                double n = Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2);
                return (double)n;
            }
        }

        /// <summary>
        /// Returns the Length of the Vector
        /// </summary>
        public double Length
        {
            get
            {
                return (double)Math.Sqrt(Norm);
            }
        }

        /// <summary>
        /// Create the Inverse of a Vector
        /// </summary>
        public Vector3f GetInverse()
        {
            return !this;
        }

        /// <summary>
        /// Create the Inverse of a Vector
        /// </summary>
        /// <param name="v">The Vector you want to Invert</param>
        /// <returns>The inverted Vector</returns>
        public static Vector3f operator !(Vector3f v)
        {
            return v * (double)(-1.0);
        }

        /// <summary>
        /// Vector addition
        /// </summary>
        /// <param name="v1">First Vector</param>
        /// <param name="v2">Second Vector</param>
        /// <returns>The resulting Vector</returns>
        public static Vector3f operator +(Vector3f v1, Vector3f v2)
        {
            return new Vector3f(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        /// <summary>
        /// Vector substraction
        /// </summary>
        /// <param name="v1">First Vector</param>
        /// <param name="v2">Second Vector</param>
        /// <returns>The resulting Vector</returns>
        public static Vector3f operator -(Vector3f v1, Vector3f v2)
        {
            return new Vector3f(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        /// <summary>
        /// Scalar Product
        /// </summary>
        /// <param name="v1">First Vector</param>
        /// <param name="v2">Second Vector</param>
        /// <returns>The resulting Vector</returns>
        public static double operator *(Vector3f v1, Vector3f v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        }

        /// <summary>
        /// Scalar Product
        /// </summary>
        /// <param name="v1">First Vector</param>
        /// <param name="v2">Second Vector</param>
        /// <returns>The resulting Vector</returns>
        public static double operator &(Vector3f v1, Vector3f v2)
        {
            return v1 * v2;
        }

        /// <summary>
        /// Scalar Multiplication
        /// </summary>
        /// <param name="v1">First Vector</param>
        /// <param name="d">Scalar Factor</param>
        /// <returns>The resulting Vector</returns>
        public static Vector3f operator *(Vector3f v1, double d)
        {
            return new Vector3f(v1.X * d, v1.Y * d, v1.Z * d);
        }

        /// <summary>
        /// Scalar Multiplication
        /// </summary>
        /// <param name="v1">First Vector</param>
        /// <param name="d">Scalar Factor</param>
        /// <returns>The resulting Vector</returns>
        public static Vector3f operator *(double d, Vector3f v1)
        {
            return v1 * d;
        }

        /// <summary>
        /// Scalar Division
        /// </summary>
        /// <param name="v1">First Vector</param>
        /// <param name="d">Scalar Factor</param>
        /// <returns>The resulting Vector</returns>
        public static Vector3f operator /(Vector3f v1, double d)
        {
            return new Vector3f(v1.X / d, v1.Y / d, v1.Z / d);
        }

        /// <summary>
        /// Scalar Division
        /// </summary>
        /// <param name="v1">First Vector</param>
        /// <param name="d">Scalar Factor</param>
        /// <returns>The resulting Vector</returns>
        public static Vector3f operator /(double d, Vector3f v1)
        {
            return v1 / d;
        }

        /// <summary>
        /// Cross Product
        /// </summary>
        /// <param name="v1">First Vector</param>
        /// <param name="v2">Second Vector</param>
        /// <returns>The resulting Vector</returns>
        public static Vector3f operator |(Vector3f v1, Vector3f v2)
        {
            return new Vector3f(
                v1.Y * v2.Z - v1.Z * v2.Y,
                v1.Z * v2.X - v1.X * v2.Z,
                v1.X * v2.Y - v1.Y * v2.X
                );
        }

        /// <summary>
        /// Compare
        /// </summary>
        /// <param name="v1">First Vector</param>
        /// <param name="v2">Second Vector</param>
        /// <returns>The resulting Vector</returns>
        public static bool operator ==(Vector3f v1, Vector3f v2)
        {
            if (((object)v1) == null || ((object)v2) == null) return (((object)v1) == null && ((object)v2) == null);
            return (v1.X == v2.X) && (v1.Y == v2.Y) && (v1.Z == v2.Z);
        }

        /// <summary>
        /// Returns a HashCode to identify this Instance
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Returns true if the passed Objects equals this one
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }


        /// <summary>
        /// Compare
        /// </summary>
        /// <param name="v1">First Vector</param>
        /// <param name="v2">Second Vector</param>
        /// <returns>The resulting Vector</returns>
        public static bool operator !=(Vector3f v1, Vector3f v2)
        {
            return (v1.X != v2.X) || (v1.Y != v2.Y) || (v1.Z != v2.Z);
        }

        /// <summary>
        /// Returns a Component of this Vector (0=x, 1=y, 2=z)
        /// </summary>
        /// <param name="index">Index of the component</param>
        /// <returns>the value stored in that Component</returns>
        public virtual double GetComponent(int index)
        {
            if (index == 0) return X;
            if (index == 1) return Y;
            if (index == 2) return Z;

            return 0;
        }

        /// <summary>
        /// Set a Component of this Vector (0=x, 1=y, 2=z)
        /// </summary>
        /// <param name="index">Index of the component</param>
        /// <param name="val">The new Value</param>
        public virtual void SetComponent(int index, double val)
        {
            if (index == 0) X = val;
            if (index == 1) Y = val;
            if (index == 2) Z = val;
        }

        /// <summary>
        /// Integer Indexer
        /// </summary>
        public double this[int index]
        {
            get { return GetComponent(index); }
            set { SetComponent(index, value); }
        }

        /// <summary>
        /// Create a clone of this Vector
        /// </summary>
        /// <returns></returns>
        public new Vector3f Clone()
        {
            Vector3f v = new Vector3f(this.X, this.Y, this.Z);
            return v;
        }

        /*public static implicit operator Ambertation.Geometry.Vector3(Vector3f v)
		{
			return new Ambertation.Geometry.Vector3(v.X, v.Y, v.Z);
		}*/

        public Vector3f(string[] datarr)
        {
            this.X = double.Parse(datarr[0]);
            this.Y = double.Parse(datarr[1]);
            this.Z = double.Parse(datarr[2]);
        }

        public Vector3f(string data)
        {
            string[] datarr = data.Split(" ".ToCharArray());
            this.X = double.Parse(datarr[0]);
            this.Y = double.Parse(datarr[1]);
            this.Z = double.Parse(datarr[2]);
        }

        public Vector3f(double[] data)
        {
            this.X = data[0];
            this.Y = data[1];
            this.Z = data[2];
        }

        public string ToString2()
        {
            return X.ToString("N6") + " " + Y.ToString("N6") + " " + Z.ToString("N6");
        }


        public static new Vector3f Zero
        {
            get { return new Vector3f(0, 0, 0); }
        }
    }

    /// <summary>
    /// Contains the a 3D Vector
    /// </summary>
    public class Vector3i
    {
        int x, y, z;

        /// <summary>
        /// The X Coordinate of the Vector
        /// </summary>
        public int X
        {
            get { return x; }
            set { x = value; }
        }
        /// <summary>
        /// The Y Coordinate of the Vector
        /// </summary>
        public int Y
        {
            get { return y; }
            set { y = value; }
        }
        /// <summary>
        /// The Z Coordinate of the Vector
        /// </summary>
        public int Z
        {
            get { return z; }
            set { z = value; }
        }

        /// <summary>
        /// Creates a new Vector Instance (0-Vector)
        /// </summary>
        public Vector3i()
        {
            x = 0; y = 0; z = 0;
        }

        /// <summary>
        /// Creates new Vector Instance
        /// </summary>
        /// <param name="x">X-Coordinate</param>
        /// <param name="y">Y-Coordinate</param>
        /// <param name="z">Z-Coordinate</param>
        public Vector3i(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// Unserializes a BinaryStream into the Attributes of this Instance
        /// </summary>
        /// <param name="reader">The Stream that contains the FileData</param>
        public virtual void Unserialize(System.IO.BinaryReader reader)
        {
            x = reader.ReadInt32();
            y = reader.ReadInt32();
            z = reader.ReadInt32();
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
            writer.Write(x);
            writer.Write(y);
            writer.Write(z);
        }
    }

    /// <summary>
    /// Contains the a 4D Vector (when (un)serialized, it will be interpreted as SingleFloat!)
    /// </summary>
    [System.ComponentModel.TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class Vector4f : Vector3f
    {
        double w;
        /// <summary>
        /// The 4th Component of an Vector (often used as focal Point)
        /// </summary>
        public double W
        {
            get
            {
                if (double.IsNaN(w)) return 0;
                return w;
            }
            set { w = value; }
        }

        /// <summary>
        /// Creates a new Vector Instance (0-Vector)
        /// </summary>
        public Vector4f() : base()
        {
            w = 0;
        }

        /// <summary>
        /// Creates new Vector Instance
        /// </summary>
        /// <param name="x">X-Coordinate</param>
        /// <param name="y">Y-Coordinate</param>
        /// <param name="z">Z-Coordinate</param>
        public Vector4f(double x, double y, double z) : this(x, y, z, 0)
        {
        }

        /// <summary>
        /// Creates new Vector Instance
        /// </summary>
        /// <param name="x">X-Coordinate</param>
        /// <param name="y">Y-Coordinate</param>
        /// <param name="z">Z-Coordinate</param>
        /// <param name="w">4th-Coordinate (often the focal Point)</param>
        public Vector4f(double x, double y, double z, double w) : base(x, y, z)
        {
            this.w = w;
        }

        /// <summary>
        /// Unserializes a BinaryStream into the Attributes of this Instance
        /// </summary>
        /// <param name="reader">The Stream that contains the FileData</param>
        public override void Unserialize(IoBuffer reader)
        {
            base.Unserialize(reader);
            w = (reader.ReadSingle());

        }

        /// <summary>
        /// This output is used in the ListBox View
        /// </summary>
        /// <returns>A String Describing the Data</returns>
        public override string ToString()
        {
            return base.ToString() + ", " + w.ToString("N2");
        }

        /// <summary>
        /// Returns a Component of this Vector (0=x, 1=y, 2=z, 3=w)
        /// </summary>
        /// <param name="index">Index of the component</param>
        /// <returns>the value stored in that Component</returns>
        public override double GetComponent(int index)
        {
            if (index == 3) return W;
            return base.GetComponent(index);
        }

        /// <summary>
        /// Set a Component of this Vector (0=x, 1=y, 2=z, 3=w)
        /// </summary>
        /// <param name="index">Index of the component</param>
        /// <param name="val">The new Value</param>
        public override void SetComponent(int index, double val)
        {
            base.SetComponent(index, val);
            if (index == 3) W = val;
        }

        /// <summary>
        /// Create a clone of this Vector
        /// </summary>
        /// <returns></returns>
        public new Vector4f Clone()
        {
            Vector4f v = new Vector4f(this.X, this.Y, this.Z, this.W);
            return v;
        }
    }

    /// <summary>
    /// Typesave ArrayList for Vector3i Objects
    /// </summary>
    public class Vectors3i : ArrayList
    {
        /// <summary>
        /// Integer Indexer
        /// </summary>
        public new Vector3i this[int index]
        {
            get { return ((Vector3i)base[index]); }
            set { base[index] = value; }
        }

        /// <summary>
        /// unsigned Integer Indexer
        /// </summary>
        public Vector3i this[uint index]
        {
            get { return ((Vector3i)base[(int)index]); }
            set { base[(int)index] = value; }
        }

        /// <summary>
        /// add a new Element
        /// </summary>
        /// <param name="item">The object you want to add</param>
        /// <returns>The index it was added on</returns>
        public int Add(Vector3i item)
        {
            return base.Add(item);
        }

        /// <summary>
        /// insert a new Element
        /// </summary>
        /// <param name="index">The Index where the Element should be stored</param>
        /// <param name="item">The object that should be inserted</param>
        public void Insert(int index, Vector3i item)
        {
            base.Insert(index, item);
        }

        /// <summary>
        /// remove an Element
        /// </summary>
        /// <param name="item">The object that should be removed</param>
        public void Remove(Vector3i item)
        {
            base.Remove(item);
        }

        /// <summary>
        /// Checks wether or not the object is already stored in the List
        /// </summary>
        /// <param name="item">The Object you are looking for</param>
        /// <returns>true, if it was found</returns>
        public bool Contains(Vector3i item)
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
            Vectors3i list = new Vectors3i();
            foreach (Vector3i item in this) list.Add(item);

            return list;
        }
    }

    /// <summary>
    /// Typesave ArrayList for Vector3f Objects
    /// </summary>
    public class Vectors3f : ArrayList
    {
        /// <summary>
        /// Integer Indexer
        /// </summary>
        public new Vector3f this[int index]
        {
            get { return ((Vector3f)base[index]); }
            set { base[index] = value; }
        }

        /// <summary>
        /// unsigned Integer Indexer
        /// </summary>
        public Vector3f this[uint index]
        {
            get { return ((Vector3f)base[(int)index]); }
            set { base[(int)index] = value; }
        }

        /// <summary>
        /// add a new Element
        /// </summary>
        /// <param name="item">The object you want to add</param>
        /// <returns>The index it was added on</returns>
        public int Add(Vector3f item)
        {
            return base.Add(item);
        }

        /// <summary>
        /// insert a new Element
        /// </summary>
        /// <param name="index">The Index where the Element should be stored</param>
        /// <param name="item">The object that should be inserted</param>
        public void Insert(int index, Vector3f item)
        {
            base.Insert(index, item);
        }

        /// <summary>
        /// remove an Element
        /// </summary>
        /// <param name="item">The object that should be removed</param>
        public void Remove(Vector3f item)
        {
            base.Remove(item);
        }

        /// <summary>
        /// Checks wether or not the object is already stored in the List
        /// </summary>
        /// <param name="item">The Object you are looking for</param>
        /// <returns>true, if it was found</returns>
        public bool Contains(Vector3f item)
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
            Vectors3f list = new Vectors3f();
            foreach (Vector3f item in this) list.Add(item);

            return list;
        }
    }

    /// <summary>
    /// Typesave ArrayList for Vector2f Objects
    /// </summary>
    public class Vectors2f : ArrayList
    {
        /// <summary>
        /// Integer Indexer
        /// </summary>
        public new Vector2f this[int index]
        {
            get { return ((Vector2f)base[index]); }
            set { base[index] = value; }
        }

        /// <summary>
        /// unsigned Integer Indexer
        /// </summary>
        public Vector2f this[uint index]
        {
            get { return ((Vector2f)base[(int)index]); }
            set { base[(int)index] = value; }
        }

        /// <summary>
        /// add a new Element
        /// </summary>
        /// <param name="item">The object you want to add</param>
        /// <returns>The index it was added on</returns>
        public int Add(Vector2f item)
        {
            return base.Add(item);
        }

        /// <summary>
        /// insert a new Element
        /// </summary>
        /// <param name="index">The Index where the Element should be stored</param>
        /// <param name="item">The object that should be inserted</param>
        public void Insert(int index, Vector2f item)
        {
            base.Insert(index, item);
        }

        /// <summary>
        /// remove an Element
        /// </summary>
        /// <param name="item">The object that should be removed</param>
        public void Remove(Vector2f item)
        {
            base.Remove(item);
        }

        /// <summary>
        /// Checks wether or not the object is already stored in the List
        /// </summary>
        /// <param name="item">The Object you are looking for</param>
        /// <returns>true, if it was found</returns>
        public bool Contains(Vector2f item)
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
            Vectors2f list = new Vectors2f();
            foreach (Vector2f item in this) list.Add(item);

            return list;
        }
    }

    /// <summary>
    /// Typesave ArrayList for Vector4f Objects
    /// </summary>
    public class Vectors4f : ArrayList
    {
        /// <summary>
        /// Integer Indexer
        /// </summary>
        public new Vector4f this[int index]
        {
            get { return ((Vector4f)base[index]); }
            set { base[index] = value; }
        }

        /// <summary>
        /// unsigned Integer Indexer
        /// </summary>
        public Vector4f this[uint index]
        {
            get { return ((Vector4f)base[(int)index]); }
            set { base[(int)index] = value; }
        }

        /// <summary>
        /// add a new Element
        /// </summary>
        /// <param name="item">The object you want to add</param>
        /// <returns>The index it was added on</returns>
        public int Add(Vector4f item)
        {
            return base.Add(item);
        }

        /// <summary>
        /// insert a new Element
        /// </summary>
        /// <param name="index">The Index where the Element should be stored</param>
        /// <param name="item">The object that should be inserted</param>
        public void Insert(int index, Vector4f item)
        {
            base.Insert(index, item);
        }

        /// <summary>
        /// remove an Element
        /// </summary>
        /// <param name="item">The object that should be removed</param>
        public void Remove(Vector4f item)
        {
            base.Remove(item);
        }

        /// <summary>
        /// Checks wether or not the object is already stored in the List
        /// </summary>
        /// <param name="item">The Object you are looking for</param>
        /// <returns>true, if it was found</returns>
        public bool Contains(Vector4f item)
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
            Vectors4f list = new Vectors4f();
            foreach (Vector4f item in this) list.Add(item);

            return list;
        }
    }

}
