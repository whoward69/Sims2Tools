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
using System;
using System.Collections;

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

#if DEBUG
        /// <summary>
        /// returns the Euler Angles
        /// </summary>
        public Vector3f Euler
        {
            get
            {
                return this.GetEulerAngles();
            }
        }

        public bool IsComplex(double z)
        {
            return ((Math.Pow(X + Y + Z + W, 2) - 4.0 * (Norm - z)) < 0.0);
        }

        public double GetMovePlus(double z)
        {
            double d1 = ((-2.0 * (X + Y + Z + W)) + (2 * Math.Sqrt(Math.Pow(X + Y + Z + W, 2) - 4.0 * (Norm - z)))) / 8.0;
            double d2 = ((-2.0 * (X + Y + Z + W)) - (2 * Math.Sqrt(Math.Pow(X + Y + Z + W, 2) - 4.0 * (Norm - z)))) / 8.0;
            if (d1 < d2) return d2;
            else return d1;
        }

        public double GetMoveMinus(double z)
        {
            double d1 = ((-2.0 * (X + Y + Z + W)) + (2 * Math.Sqrt(Math.Pow(X + Y + Z + W, 2) - 4.0 * (Norm - z)))) / 8.0;
            double d2 = ((-2.0 * (X + Y + Z + W)) - (2 * Math.Sqrt(Math.Pow(X + Y + Z + W, 2) - 4.0 * (Norm - z)))) / 8.0;
            if (d1 > d2) return d2;
            else return d1;
        }
#endif

        /// <summary>
        /// Create the Inverse of a Quaternion
        /// </summary>
        /// <param name="q">The Quaternion you want to Invert</param>
        /// <returns>The inverted Quaternion</returns>
        public static Quaternion operator !(Quaternion q)
        {
            return q.GetInverse();
        }

        /// <summary>
        /// Returns the Inverse of this Quaternion
        /// </summary>
        /// <returns>Inverted Quaternion</returns>
        public new Quaternion GetInverse()
        {
            return Conjugate * (double)(1.0 / this.Norm);
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
        /// Scalar, dot or inner Product
        /// </summary>
        /// <param name="q1">First Quaternion</param>
        /// <param name="q2">Second Quaternion</param>
        /// <returns>The resulting Quaternion</returns>
        public static double operator &(Quaternion q1, Quaternion q2)
        {
            return q1.W * q2.W + (q1.Imaginary & q2.Imaginary);
        }

        /// <summary>
        /// Cross Product
        /// </summary>
        /// <param name="q1">First Quaternion</param>
        /// <param name="q2">Second Quaternion</param>
        /// <returns>The resulting Quaternion</returns>
        public static Quaternion operator |(Quaternion q1, Quaternion q2)
        {
            return new Quaternion(QuaternionParameterType.ImaginaryReal, q2.Imaginary | q1.Imaginary, 0);
        }

        /// <summary>
        /// Multiplication
        /// </summary>
        /// <param name="q1">First Quaternion</param>
        /// <param name="d">a Scalar Value</param>
        /// <returns>The resulting Quaternion</returns>
        public static Quaternion operator *(Quaternion q1, double d)
        {
            return new Quaternion(QuaternionParameterType.ImaginaryReal, q1.Imaginary * d, q1.W * d);
        }

        /// <summary>
        /// Multiplication
        /// </summary>
        /// <param name="q1">First Quaternion</param>
        /// <param name="d">a Scalar Value</param>
        /// <returns>The resulting Quaternion</returns>
        public static Quaternion operator *(double d, Quaternion q1)
        {
            return q1 * d;
        }

        /// <summary>
        /// Addition
        /// </summary>
        /// <param name="q1">First Quaternion</param>
        /// <param name="q2">Second Quaternion</param>
        /// <returns>The resulting Quaternion</returns>
        public static Quaternion operator +(Quaternion q1, Quaternion q2)
        {
            return new Quaternion(QuaternionParameterType.ImaginaryReal, q1.Imaginary + q2.Imaginary, q1.W + q2.W);
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
        /// Returns an Empty Quaternion
        /// </summary>
        public static new Quaternion Zero
        {
            get { return new Quaternion(QuaternionParameterType.ImaginaryReal, 0, 0, 0, 0); }
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
        /// Returns an Angle in Radiants
        /// </summary>
        /// <param name="deg">Angle in Degree</param>
        /// <returns>Angle in Radiants</returns>
        public static double DegToRad(double deg)
        {
            return (double)((deg * Math.PI) / 180.0);
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

        protected double MakeRobustAngle(double rad)
        {
            return rad;
            /*double sgn = 1; if (rad!=0) sgn = rad/Math.Abs(rad);
			rad = Math.Abs(RadToDeg(rad));
			if (rad>10)
				rad = Math.Round(rad);

			return DegToRad(sgn*rad);*/
        }

        protected void MakeRobust()
        {
        }

        protected void DoMakeRobust()
        {

            /*const float percision = 100000;			

			X = (float)((int)(X*percision))/percision;
			Y = (float)((int)(Y*percision))/percision;
			Z = (float)((int)(Z*percision))/percision;
			W = (float)((int)(W*percision))/percision;*/

            //this is a ingularity
            /*if (IsNear(W, 0.7, 0.09))
			{
				if (IsNear(Z, 0.7, 0.09))
				{
					double sgnz = 1; if (Z!=0) sgnz = Z/Math.Abs(Z);				
				
					X = 0;
					Y = 0;
					Z = Z - (Math.Pow(X, 2) + Math.Pow(Y, 2) ) *sgnz;
				} 
				else  if (IsNear(Y, 0.7, 0.09))
				{
					double sgny = 1; if (Y!=0) sgny = Y/Math.Abs(Y);				
				
					X = 0;
					Y = Y - (Math.Pow(X, 2) + Math.Pow(Z, 2) ) *sgny;
					Z = 0;
				}
				else  if (IsNear(X, 0.7, 0.09))
				{
					double sgnx = 1; if (X!=0) sgnx = X/Math.Abs(X);				
									
					X = X - (Math.Pow(Y, 2) + Math.Pow(Z, 2) ) *sgnx;
					Y = 0;
					Z = 0;
				}
			}*/

            /*if (IsNear(X, 1, 0.05)) { Y=0; Z=0; W=0;}
			if (IsNear(Y, 1, 0.05)) { X=0; Z=0; W=0;}
			if (IsNear(Z, 1, 0.05)) { Y=0; X=0; W=0;}*/
            //if (IsNear(W, 1, 0.05)) { Y=0; Z=0; X=0;}
        }

        /// <summary>
        /// Returns the Rotation Angle (in Radiants)
        /// </summary>
        public double Angle
        {
            get
            {
                MakeRobust();
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
                MakeRobust();
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
            MakeRobust();
            this.MakeUnitQuaternion();
        }

        public Vector3f GetEulerAngles()
        {
            Quaternion q = this.Clone();
            q.DoMakeRobust();
            Vector3f v = q.GetEulerAnglesZYX();

            v.X = MakeRobustAngle(v.X);
            v.Y = MakeRobustAngle(v.Y);
            v.Z = MakeRobustAngle(v.Z);
            return v;
        }

        protected double Clip1(double d)
        {
            if (d < -1) return -1;
            if (d > 1) return 1;
            return d;
        }

        /// <summary>
        /// Get the Euler Angles represented by this Quaternion
        /// </summary>
        /// <returns></returns>
        /// X=Pitch
        /// Y=Yaw
        /// Z=Roll
        /// </remarks>
        public Vector3f GetEulerAnglesYXZ()
        {
            Matrixd m = this.Matrix;
            Vector3f v = new Vector3f(0, 0, 0)
            {
                X = Math.Asin(-Clip1(m[1, 2]))
            };

            if (v.X < Math.PI / 2.0)
            {
                if (v.X > Math.PI / -2.0)
                {
                    v.Y = (float)Math.Atan2(Clip1(m[0, 2]), Clip1(m[2, 2]));
                    v.Z = (float)Math.Atan2(Clip1(m[1, 0]), Clip1(m[1, 1]));
                }
                else
                {
                    v.Y = (float)(-1 * Math.Atan2(-Clip1(m[0, 1]), Clip1(m[0, 0])));
                }
            }
            else
            {
                v.Y = (float)Math.Atan2(-Clip1(m[0, 1]), Clip1(m[0, 0]));
            }

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
        public Vector3f GetEulerAnglesZXY()
        {
            Matrixd m = this.Matrix;
            Vector3f v = new Vector3f(0, 0, 0)
            {
                X = Math.Asin(m[2, 1])
            };

            if (v.X < Math.PI / 2.0)
            {
                if (v.X > Math.PI / -2.0)
                {
                    v.Z = (float)Math.Atan2(-m[0, 1], m[1, 1]);
                    v.Y = (float)Math.Atan2(-m[2, 0], m[2, 2]);
                }
                else
                {
                    v.Z = (float)(-1 * Math.Atan2(-m[0, 2], m[0, 0]));
                }
            }
            else
            {
                v.Z = (float)Math.Atan2(m[0, 2], m[0, 0]);
            }

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

        public static Quaternion FromRotationMatrix(Matrixd r)
        {
            double x = 0, y = 0, z = 0, w = 0;

            double t = r.Trace;
            if (t > 0)
            {
                w = Math.Sqrt(t) / 2;
                x = (r[2, 1] - r[1, 2]) / (4 * w);
                y = (r[0, 2] - r[2, 0]) / (4 * w);
                z = (r[1, 0] - r[0, 1]) / (4 * w);
            }
            else
            {
                if (r[0, 0] >= r[1, 1] && r[0, 0] >= r[2, 2])
                {
                    x = Math.Sqrt(r[0, 0] - r[1, 1] - r[2, 2] + 1) / 2;
                    w = (r[1, 2] - r[2, 1]) / (4 * x);
                    y = (r[0, 1] - r[1, 0]) / (4 * x);
                    z = (r[2, 0] - r[0, 2]) / (4 * x);
                }
                else if (r[1, 1] >= r[0, 0] && r[1, 1] >= r[2, 2])
                {
                    y = Math.Sqrt(r[1, 1] - r[0, 0] - r[2, 2] + 1) / 2;
                    w = (r[2, 0] - r[0, 2]) / (4 * y);
                    x = (r[0, 1] - r[1, 0]) / (4 * y);
                    z = (r[1, 2] - r[2, 1]) / (4 * y);
                }
                else if (r[2, 2] >= r[0, 0] && r[2, 2] >= r[1, 1])
                {
                    z = Math.Sqrt(r[2, 2] - r[0, 0] - r[1, 1] + 1) / 2;
                    w = (r[0, 1] - r[1, 0]) / (4 * z);
                    x = (r[2, 0] - r[0, 2]) / (4 * z);
                    y = (r[1, 2] - r[2, 1]) / (4 * z);
                }
            }

            Quaternion ret = Quaternion.FromImaginaryReal(x, y, z, w);
            ret.MakeRobust();
            ret.MakeUnitQuaternion();
            return ret;
        }


        /// <summary>
        /// Set the quaternion based on the passed Euler Angles
        /// </summary>
        /// <param name="ea">The Euler Angles</param>
        /// <remarks>
        /// X=Pitch
        /// Y=Yaw
        /// Z=Roll
        /// </remarks>
        public static Quaternion FromEulerAngles(Vector3f ea)
        {
            return FromRotationMatrix(Matrixd.RotateZ(ea.Z) * Matrixd.RotateY(ea.Y) * Matrixd.RotateX(ea.X));
        }

        /// <summary>
        /// Set the quaternion based on the passed Euler Angles
        /// </summary>
        public static Quaternion FromEulerAngles(double yaw, double pitch, double roll)
        {
            return FromEulerAngles(new Vector3f(pitch, yaw, roll));
        }

        public static Quaternion FromAxisAngle(Vector3f v, double angle)
        {
            v.MakeUnitVector();
            return new Quaternion(QuaternionParameterType.UnitAxisAngle, v.X, v.Y, v.Z, angle);
        }

        public static Quaternion FromAxisAngle(double x, double y, double z, double angle)
        {
            return new Quaternion(QuaternionParameterType.UnitAxisAngle, x, y, z, angle);
        }

        public static Quaternion FromImaginaryReal(Vector3f v, double w)
        {
            return new Quaternion(QuaternionParameterType.ImaginaryReal, v.X, v.Y, v.Z, w);
        }

        public static Quaternion FromImaginaryReal(Vector4f v)
        {
            return new Quaternion(QuaternionParameterType.ImaginaryReal, v.X, v.Y, v.Z, v.W);
        }

        public static Quaternion FromImaginaryReal(double x, double y, double z, double w)
        {
            return new Quaternion(QuaternionParameterType.ImaginaryReal, x, y, z, w);
        }


        public string ToLinedString()
        {
            string s = "";
            s += "X: " + X.ToString() + "\n";
            s += "Y: " + Y.ToString() + "\n";
            s += "Z: " + Z.ToString() + "\n";
            s += "W: " + W.ToString() + "\n";
            s += "-----\n";
            s += "X: " + Axis.X.ToString() + "\n";
            s += "Y: " + Axis.Y.ToString() + "\n";
            s += "Z: " + Axis.Z.ToString() + "\n";
            s += "A: " + RadToDeg(Angle) + "\n";
            s += "-----\n";
            s += "Y: " + RadToDeg(GetEulerAngles().Y) + "\n";
            s += "P: " + RadToDeg(GetEulerAngles().X) + "\n";
            s += "R: " + RadToDeg(GetEulerAngles().Z) + "\n";
            return s;
        }


        public override string ToString()
        {
            return base.ToString() + " (X=" + Axis.X.ToString("N2") + ", Y=" + Axis.Y.ToString("N2") + ", Z=" + Axis.Z.ToString("N2") + ", a=" + RadToDeg(Angle).ToString("N1") + "    euler=y:" + Quaternion.RadToDeg(GetEulerAngles().Y).ToString("N1") + "; p:" + Quaternion.RadToDeg(GetEulerAngles().X).ToString("N1") + "; r:" + Quaternion.RadToDeg(GetEulerAngles().Z).ToString("N1") + ")";
        }

        /// <summary>
        /// Rotate the passed Vector by this Quaternion
        /// </summary>
        /// <param name="v">Vector you want to rotate</param>
        /// <returns>rotated Vector</returns>
        /// <remarks>Make sure the Quaternion is normalized before you rotate a Vector!</remarks>
        public Vector3f Rotate(Vector3f v)
        {

            Quaternion vq = new Quaternion(QuaternionParameterType.ImaginaryReal, v.X, v.Y, v.Z, 0);
            vq = this * vq * this.Conjugate;
            return new Vector3f(vq.X, vq.Y, vq.Z);

            //SimPe.Geometry.Vector4f v4 = new SimPe.Geometry.Vector4f(v.X, v.Y, v.Z, 0);
            //v4 = Matrix*v4;
            //return new SimPe.Geometry.Vector3f(v4.X, v4.Y, v4.Z);
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
                MakeRobust();
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

    /// <summary>
    /// Typesave ArrayList for Quaternion Objects
    /// </summary>
    public class Quaternions : ArrayList
    {
        /// <summary>
        /// Integer Indexer
        /// </summary>
        public new Quaternion this[int index]
        {
            get { return ((Quaternion)base[index]); }
            set { base[index] = value; }
        }

        /// <summary>
        /// unsigned Integer Indexer
        /// </summary>
        public Quaternion this[uint index]
        {
            get { return ((Quaternion)base[(int)index]); }
            set { base[(int)index] = value; }
        }

        /// <summary>
        /// add a new Element
        /// </summary>
        /// <param name="item">The object you want to add</param>
        /// <returns>The index it was added on</returns>
        public int Add(Quaternion item)
        {
            return base.Add(item);
        }

        /// <summary>
        /// insert a new Element
        /// </summary>
        /// <param name="index">The Index where the Element should be stored</param>
        /// <param name="item">The object that should be inserted</param>
        public void Insert(int index, Quaternion item)
        {
            base.Insert(index, item);
        }

        /// <summary>
        /// remove an Element
        /// </summary>
        /// <param name="item">The object that should be removed</param>
        public void Remove(Quaternion item)
        {
            base.Remove(item);
        }

        /// <summary>
        /// Checks wether or not the object is already stored in the List
        /// </summary>
        /// <param name="item">The Object you are looking for</param>
        /// <returns>true, if it was found</returns>
        public bool Contains(Quaternion item)
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
            Quaternions list = new Quaternions();
            foreach (Quaternion item in this) list.Add(item);

            return list;
        }

    }

}
