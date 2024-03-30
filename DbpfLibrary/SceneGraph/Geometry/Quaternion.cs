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

namespace Sims2Tools.DBPF.SceneGraph.Geometry
{
    internal enum QuaternionParameterType : byte
    {
        ImaginaryReal = 0x02
    }

    [System.ComponentModel.TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class Quaternion : Vector4f
    {
        internal Quaternion(QuaternionParameterType p, double x, double y, double z, double w) : base()
        {
            if (p == QuaternionParameterType.ImaginaryReal)
            {
                this.x = x;
                this.y = y;
                this.z = z;
                this.w = w;
            }
        }

        public Quaternion() : base() { }

        public static Quaternion Identity
        {
            get { return new Quaternion(QuaternionParameterType.ImaginaryReal, 0, 0, 0, 1); }
        }

        public new Quaternion Clone()
        {
            Quaternion q = new Quaternion(QuaternionParameterType.ImaginaryReal, this.X, this.Y, this.Z, this.W);
            return q;
        }
    }
}
