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
using System;

namespace Sims2Tools.DBPF.SceneGraph.Geometry
{
    internal enum QuaternionParameterType : byte
    {
        /*/// <summary>
		/// Arguments represent an Euler Angle
		/// </summary>
		EulerAngles = 0x00,*/
        /// <summary>
        /// Arguments represent a (unit-)Axis/Angle Pair
        /// </summary>
        UnitAxisAngle = 0x01,
        /// <summary>
        /// Arguments represent the Imaginary koeef. of a Quaternion and the Real Part
        /// </summary>
        ImaginaryReal = 0x02
    }
    /// <summary>
    /// Zusammenfassung für Quaternion.
    /// </summary>
    [System.ComponentModel.TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class Quaternion : Vector4f
    {
        /// <summary>
        /// Creates new Quaternion i*x + j*y + k*z + w (Based an an coefficients/unit-Axis/Angle)
        /// </summary>
        /// <param name="p">How do you want to create the Quaternion</param>
        /// <param name="x">X-Imaginary Part/X-Axis</param>
        /// <param name="y">Y-Imaginary Part/Y-Axis</param>
        /// <param name="z">Z-Imaginary Part/Z-Axis</param>
        /// <param name="w">RealPart/Angle</param>
        internal Quaternion(QuaternionParameterType p, double x, double y, double z, double w) : base()
        {
            if (p == QuaternionParameterType.ImaginaryReal)
            {
                X = x; Y = y; Z = z; W = w;
            }
            else if (p == QuaternionParameterType.UnitAxisAngle)
            {
                this.SetFromAxisAngle(new Vector3f(x, y, z), w);
            }
        }

        /// <summary>
        /// Creates new Quaternion i*x + j*y + k*z + w (Based an an coefficients/unit-Axis/Angle)
        /// </summary>
        /// <param name="p">How do you want to create the Quaternion</param>
        /// <param name="v">The (unit) Axis for the Rotation/Imaginary part</param>
        /// <param name="a">The angle (in Radiants)/Real Part</param>
        internal Quaternion(QuaternionParameterType p, Vector3f v, double a) : base()
        {
            if (p == QuaternionParameterType.ImaginaryReal)
            {
                X = v.X; Y = v.Y; Z = v.Z; W = a;
            }
            else if (p == QuaternionParameterType.UnitAxisAngle)
            {
                this.SetFromAxisAngle(v, a);
            }
        }

        /// <summary>
        /// Creates a new Identity Quaternion
        /// </summary>
        public Quaternion() : base() { }

        /// <summary>
        /// Returns the Norm of the Quaternion
        /// </summary>
        public new double Norm
        {
            get
            {
                double n = Imaginary.Norm + Math.Pow(W, 2);
                return (double)n;
            }
        }

        /// <summary>
        /// Returns the Length of the Quaternion
        /// </summary>
        public new double Length
        {
            get
            {
                return (double)Math.Sqrt(Norm);
            }
        }

        /// <summary>
        /// Returns the Conjugate for this Quaternion
        /// </summary>
        public Quaternion Conjugate
        {
            get
            {
                return new Quaternion(QuaternionParameterType.ImaginaryReal, -1 * this.Imaginary, this.W);
            }
        }

        /// <summary>
        /// Multiplication
        /// </summary>
        /// <param name="q1">First Quaternion</param>
        /// <param name="q2">Second Quaternion</param>
        /// <returns>The resulting Quaternion</returns>
        public static Quaternion operator *(Quaternion q2, Quaternion q1)
        {
            return new Quaternion(
                QuaternionParameterType.ImaginaryReal,
                (q1.Imaginary | q2.Imaginary) + (q2.W * q1.Imaginary) + (q1.W * q2.Imaginary),
                q1.W * q2.W - (q1.Imaginary & q2.Imaginary));
        }

        /// <summary>
        /// Returns the Imaginary Part of the Quaternion
        /// </summary>
        public Vector3f Imaginary
        {
            get { return this; }
        }

        /// <summary>
        /// Returns an Identity Quaternion
        /// </summary>
        public static Quaternion Identity
        {
            get { return new Quaternion(QuaternionParameterType.ImaginaryReal, 0, 0, 0, 1); }
        }

        /// <summary>
        /// Returns an Angle in Degree
        /// </summary>
        /// <param name="rad">Angle in Radiants</param>
        /// <returns>Angle in Degree</returns>
        public static double RadToDeg(double rad)
        {
            return (double)((rad * 180.0) / Math.PI);
        }

        /// <summary>
        /// Makes sure this Quaternion is a Unit Quaternion (Length=1)
        /// </summary>
        public void MakeUnitQuaternion()
        {
            double l = Length;
            if (l != 0)
            {
                X /= l;
                Y /= l;
                Z /= l;
                W /= l;
            }
        }

        /// <summary>
        /// Returns the Rotation Angle (in Radiants)
        /// </summary>
        public double Angle
        {
            get
            {
                this.MakeUnitQuaternion();
                //if (W==0) return 0;
                return (double)(Math.Acos(W) * 2.0);
            }
        }

        /// <summary>
        /// Returns the rotation (unit-)Axis
        /// </summary>
        public Vector3f Axis
        {
            get
            {
                this.MakeUnitQuaternion();
                //if (W==0) return new Vector3f(0, 0, 1);
                double sina = Math.Sqrt(1 - Math.Pow(W, 2)); //(double)Math.Sin(Angle/2.0);

                if (sina == 0) return new Vector3f(0, 0, 0);
                return new Vector3f(X / sina, Y / sina, Z / sina);
            }
        }

        /// <summary>
        /// Set the Quaternion based on an Axis-Angle pair
        /// </summary>
        /// <param name="axis">The (unit-)Axis</param>
        /// <param name="a">The rotation Angle</param>
        public void SetFromAxisAngle(Vector3f axis, double a)
        {
            axis.MakeUnitVector();

            double sina = (double)Math.Sin(a / 2.0);
            X = axis.X * sina;
            Y = axis.Y * sina;
            Z = axis.Z * sina;

            W = (double)Math.Cos(a / 2.0);
            this.MakeUnitQuaternion();
        }

        public Vector3f GetEulerAngles()
        {
            Quaternion q = this.Clone();
            Vector3f v = q.GetEulerAnglesZYX();

            return v;
        }

        /// <summary>
        /// Get the Euler Angles represented by this Quaternion
        /// </summary>
        /// <returns></returns>
        /// X=Pitch
        /// Y=Yaw
        /// Z=Roll
        /// </remarks>
        public Vector3f GetEulerAnglesZYX()
        {
            Matrixd m = this.Matrix;
            Vector3f v = new Vector3f(0, 0, 0)
            {
                Y = Math.Asin(-m[2, 0])
            };

            if (v.Y < Math.PI / 2.0)
            {
                if (v.Y > Math.PI / -2.0)
                {
                    v.Z = (float)Math.Atan2(m[1, 0], m[0, 0]);
                    v.X = (float)Math.Atan2(m[2, 1], m[2, 2]);
                }
                else
                {
                    v.Z = (float)(-1 * Math.Atan2(-m[0, 1], m[0, 2]));
                }
            }
            else
            {
                v.Z = (float)Math.Atan2(-m[0, 1], m[0, 2]);
            }

            return v;
        }

        public override string ToString()
        {
            return base.ToString() + " (X=" + Axis.X.ToString("N2") + ", Y=" + Axis.Y.ToString("N2") + ", Z=" + Axis.Z.ToString("N2") + ", a=" + RadToDeg(Angle).ToString("N1") + "    euler=y:" + Quaternion.RadToDeg(GetEulerAngles().Y).ToString("N1") + "; p:" + Quaternion.RadToDeg(GetEulerAngles().X).ToString("N1") + "; r:" + Quaternion.RadToDeg(GetEulerAngles().Z).ToString("N1") + ")";
        }

        /// <summary>
        /// Create a clone of this Quaternion
        /// </summary>
        /// <returns></returns>
        public new Quaternion Clone()
        {
            Quaternion q = new Quaternion(QuaternionParameterType.ImaginaryReal, this.X, this.Y, this.Z, this.W);
            return q;
        }



        /// <summary>
        /// Returns the Matirx for this Quaternion. 		
        /// </summary>
        /// <remarks>
        /// Before the Matrix is generated, the Quaternion will get Normalized!!!
        /// </remarks>
        public Matrixd Matrix
        {
            get
            {
                this.MakeUnitQuaternion();

                Matrixd m = new Matrixd(4, 4);
                double sx = Math.Pow(X, 2);
                double sy = Math.Pow(Y, 2);
                double sz = Math.Pow(Z, 2);
                // double sw = Math.Pow(W, 2);
                m[0, 0] = 1 - 2 * (sy + sz); m[0, 1] = 2 * (X * Y - W * Z); m[0, 2] = 2 * (X * Z + W * Y); m[0, 3] = 0;
                m[1, 0] = 2 * (X * Y + W * Z); m[1, 1] = 1 - 2 * (sx + sz); m[1, 2] = 2 * (Y * Z - W * X); m[1, 3] = 0;
                m[2, 0] = 2 * (X * Z - W * Y); m[2, 1] = 2 * (Y * Z + W * X); m[2, 2] = 1 - 2 * (sx + sy); m[2, 3] = 0;
                m[3, 0] = 0; m[3, 1] = 0; m[3, 2] = 0; m[3, 3] = 1;

                return m;
            }
        }

        void LoadCorrection()
        {
            //W = -W;
            //X = -X;
            //Y = Z;
            //Z = -Z;
        }
        /// <summary>
        /// Unserializes a BinaryStream into the Attributes of this Instance
        /// </summary>
        /// <param name="reader">The Stream that contains the FileData</param>
        public override void Unserialize(DbpfReader reader)
        {
            base.Unserialize(reader);

            LoadCorrection();
        }
    }
}
