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
using System;

namespace Sims2Tools.DBPF.SceneGraph.Geometry
{
    [System.ComponentModel.TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class Vector2f
    {
        protected double x, y;

        public double X
        {
            get
            {
                if (double.IsNaN(x))
                    return 0;
                return x;
            }
            // set { x = value; }
        }

        public double Y
        {
            get
            {
                if (double.IsNaN(y)) return 0;
                return y;
            }
            // set { y = value; }
        }

        public Vector2f()
        {
            x = 0; y = 0;
        }

        public Vector2f(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public virtual void Unserialize(DbpfReader reader)
        {
            x = reader.ReadSingle();
            y = reader.ReadSingle();
        }

        public virtual uint FileSize => (uint)(4 + 4);

        public virtual void Serialize(DbpfWriter writer)
        {
            writer.WriteSingle((float)x);
            writer.WriteSingle((float)y);
        }

        public override string ToString()
        {
            return x.ToString("N2") + "; " + y.ToString("N2");
        }

        public Vector2f Clone()
        {
            Vector2f v = new Vector2f(this.X, this.Y);
            return v;
        }
    }

    [System.ComponentModel.TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class Vector3f : Vector2f
    {
        protected double z;

        public double Z
        {
            get
            {
                if (double.IsNaN(z)) return 0;
                return z;
            }
            // set { z = value; }
        }

        public Vector3f() : base()
        {
            z = 0;
        }

        public Vector3f(double x, double y, double z) : base(x, y)
        {
            this.z = z;
        }

        public override void Unserialize(DbpfReader reader)
        {
            base.Unserialize(reader);

            z = reader.ReadSingle();
        }

        public override uint FileSize => (uint)(base.FileSize + 4);

        public override void Serialize(DbpfWriter writer)
        {
            base.Serialize(writer);

            writer.WriteSingle((float)z);
        }

        public override string ToString()
        {
            return base.ToString() + "; " + z.ToString("N2");
        }

        [System.ComponentModel.Browsable(false)]
        public double Norm
        {
            get
            {
                double n = Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2);
                return (double)n;
            }
        }

        public double Length
        {
            get
            {
                return (double)Math.Sqrt(Norm);
            }
        }

        public new Vector3f Clone()
        {
            Vector3f v = new Vector3f(this.X, this.Y, this.Z);
            return v;
        }
    }

    [System.ComponentModel.TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class Vector4f : Vector3f
    {
        protected double w;

        public double W
        {
            get
            {
                if (double.IsNaN(w)) return 0;
                return w;
            }
            // set { w = value; }
        }

        public Vector4f() : base()
        {
            w = 0;
        }

        public Vector4f(double x, double y, double z, double w) : base(x, y, z)
        {
            this.w = w;
        }

        public override void Unserialize(DbpfReader reader)
        {
            base.Unserialize(reader);

            w = reader.ReadSingle();
        }

        public override uint FileSize => (uint)(base.FileSize + 4);

        public override void Serialize(DbpfWriter writer)
        {
            base.Serialize(writer);

            writer.WriteSingle((float)w);
        }

        public new Vector4f Clone()
        {
            Vector4f v = new Vector4f(this.X, this.Y, this.Z, this.W);
            return v;
        }
    }
}
