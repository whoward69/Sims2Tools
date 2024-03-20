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

using System;

namespace Sims2Tools.DBPF.SceneGraph.Geometry
{
    public class Matrixd
    {
        readonly double[][] m;
        /// <summary>
        /// Representation of a Matrix
        /// </summary>
        /// <param name="col">Number of Columns</param>
        /// <param name="row">Number of Rows</param>
        /// <remarks>Minimum is a 1x1 (rowxcol)Matrix</remarks>
        public Matrixd(int row, int col)
        {
            m = new double[row][];
            for (int i = 0; i < row; i++)
                m[i] = new double[col];
        }

        /// <summary>
        /// Create a new 3x1 Matrix
        /// </summary>
        /// <param name="v">the vecotor that should be represented as a Matrix</param>
        public Matrixd(Vector3f v) : this(3, 1)
        {
            this[0, 0] = v.X;
            this[1, 0] = v.Y;
            this[2, 0] = v.Z;
        }

        /// <summary>
        /// Create a new 4x1 Matrix
        /// </summary>
        /// <param name="v">the vecotor that should be represented as a Matrix</param>
        public Matrixd(Vector4f v) : this(4, 1)
        {
            this[0, 0] = v.X;
            this[1, 0] = v.Y;
            this[2, 0] = v.Z;
            this[3, 0] = v.W;
        }

        /// <summary>
        /// Returns the Vector stored in this matrix or null if not possible!
        /// </summary>
        /// <returns></returns>
        public Vector3f GetVector()
        {
            if ((Rows != 3 || Columns != 1) && ((Rows != 1 || Columns != 3))) return null;
            if (Rows == 3) return new Vector3f(m[0][0], m[1][0], m[2][0]);
            return new Vector3f(m[0][1], m[0][1], m[0][2]);
        }

        /// <summary>
        /// Returns the Vector stored in this matrix or null if not possible!
        /// </summary>
        /// <returns></returns>
        public Vector4f GetVector4()
        {
            if ((Rows != 4 || Columns != 1) && ((Rows != 1 || Columns != 4))) return null;
            if (Rows == 4) return new Vector4f(m[0][0], m[1][0], m[2][0], m[3][0]);
            return new Vector4f(m[0][1], m[0][1], m[0][2], m[0][3]);
        }

        /// <summary>
        /// Create the Transpose of this Matrix
        /// </summary>
        public Matrixd GetTranspose()
        {
            Matrixd res = new Matrixd(Columns, Rows);
            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Columns; c++)
                    res[c, r] = this[r, c];

            return res;
        }

        /// <summary>
        /// Create an identity Mareix
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static Matrixd GetIdentity(int row, int col)
        {
            Matrixd i = new Matrixd(row, col);

            for (int r = 0; r < row; r++)
                for (int c = 0; c < col; c++)
                {
                    if (r == c) i[r, c] = 1;
                    else i[r, c] = 0;
                }

            return i;
        }

        /// <summary>
        /// Number of stored Rows
        /// </summary>
        public int Rows
        {
            get { return m.Length; }
        }

        /// <summary>
        /// Numbner of stored Columns
        /// </summary>
        public int Columns
        {
            get
            {
                if (Rows == 0) return 0;
                return m[0].Length;
            }
        }

        /// <summary>
        /// Integer Indexer (row, column)
        /// </summary>
        public double this[int row, int col]
        {
            get { return m[row][col]; }
            set { m[row][col] = value; ; }
        }

        /// <summary>
        /// Returns the Trace of the Matrix
        /// </summary>
        /// <exception cref="GeometryException">Thrown if the matrix is not a square Matrix</exception>
        public double Trace
        {
            get
            {
                if (Rows != Columns) throw new GeometryException("Unable to get Trace for a non Square Matrix (" + this.ToString() + ")");
                double ret = 0;
                for (int i = 0; i < Rows; i++)
                    ret += this[i, i];

                return ret;
            }
        }

        /// <summary>
        /// Matirx Multiplication
        /// </summary>
        /// <param name="m1">First Matrix</param>
        /// <param name="m2">Second Matrix</param>
        /// <returns>The resulting Matrix</returns>
        /// <exception cref="GeometryException">Thrown if Number of Rows in m1 is not equal to Number of Columns in m2</exception>
        public static Matrixd operator *(Matrixd m1, Matrixd m2)
        {
            if (m1.Columns != m2.Rows) throw new GeometryException("Unable to multiplicate Matrices (" + m1.ToString() + " * " + m2.ToString() + ")");

            Matrixd m = new Matrixd(m1.Rows, m2.Columns);
            for (int r = 0; r < m.Rows; r++)
                for (int c = 0; c < m.Columns; c++)
                {
                    double res = 0;

                    for (int i = 0; i < m1.Columns; i++)
                        res += m1[r, i] * m2[i, c];

                    m[r, c] = res;
                }//for m
            return m;
        }

        /// <summary>
        /// Scalar Matirx Multiplication
        /// </summary>
        /// <param name="m1">First Matrix</param>
        /// <param name="d">a Scalar</param>
        /// <returns>The resulting Matrix</returns>
        public static Matrixd operator *(Matrixd m1, double d)
        {
            Matrixd m = new Matrixd(m1.Rows, m1.Columns);
            for (int r = 0; r < m.Rows; r++)
                for (int c = 0; c < m.Columns; c++)
                    m[r, c] = m1[r, c] * d;
            return m;
        }

        /// <summary>
        /// Scalar Matirx Multiplication
        /// </summary>
        /// <param name="m1">First Matrix</param>
        /// <param name="d">a Scalar</param>
        /// <returns>The resulting Matrix</returns>
        public static Matrixd operator *(double d, Matrixd m1)
        {
            return m1 * d;
        }

        /// <summary>
        /// Scalar Matirx Multiplication
        /// </summary>
        /// <param name="m1">First Matrix</param>
        /// <param name="d">a Scalar</param>
        /// <returns>The resulting Matrix</returns>
        /// <exception cref="GeometryException">Thrown if User did Attempt to divide By Zero</exception>
        public static Matrixd operator /(Matrixd m1, double d)
        {
            if (d == 0) throw new GeometryException("Unable to divide by Zero.");
            Matrixd m = new Matrixd(m1.Rows, m1.Columns);
            for (int r = 0; r < m.Rows; r++)
                for (int c = 0; c < m.Columns; c++)
                    m[r, c] = m1[r, c] / d;
            return m;
        }

        /// <summary>
        /// Scalar Matirx Multiplication
        /// </summary>
        /// <param name="m1">First Matrix</param>
        /// <param name="d">a Scalar</param>
        /// <returns>The resulting Matrix</returns>
        public static Matrixd operator /(double d, Matrixd m1)
        {
            return m1 / d;
        }

        /// <summary>
        /// Calculates the n-th Power
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        /// <remarks>Based on code by Rajitha Wimalasooriya (http://www.thecodeproject.com/csharp/rtwmatrix.asp)</remarks>
        /// <exception cref="GeometryException">Thrown if this is not a Square Matrix</exception>
        public static Matrixd operator ^(Matrixd m1, float val)
        {

            if (m1.Rows != m1.Columns)
                throw new GeometryException("Attempt to find the power of a non square matrix");
            //return null;

            Matrixd result = m1;

            for (int i = 0; i < val; i++)
                result *= m1;

            return result;
        }

        /// <summary>
        /// Adds two matrices
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        /// <remarks>Based on code by Rajitha Wimalasooriya (http://www.thecodeproject.com/csharp/rtwmatrix.asp)</remarks>
        /// <exception cref="GeometryException">Thrown if the MAtrices have diffrent Sizes</exception>
        public static Matrixd operator +(Matrixd m1, Matrixd m2)
        {

            if (m1.Rows != m2.Rows || m1.Columns != m2.Columns)
                throw new GeometryException("Attempt to add matrixes of different sizes.");
            //return null;

            Matrixd result = new Matrixd(m1.Rows, m1.Columns);

            for (int i = 0; i < m1.Rows; i++)
                for (int j = 0; j < m1.Columns; j++)
                    result[i, j] = m1[i, j] + m2[i, j];

            return result;
        }

        /// <summary>
        /// Substract two Matrices
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        /// <remarks>Based on code by Rajitha Wimalasooriya (http://www.thecodeproject.com/csharp/rtwmatrix.asp)</remarks>		
        /// <exception cref="GeometryException">Thrown if the MAtrices have diffrent Sizes</exception>
        public static Matrixd operator -(Matrixd m1, Matrixd m2)
        {

            if (m1.Rows != m2.Rows || m1.Columns != m2.Columns)
                throw new GeometryException("Attempt to subtract matrixes of different sizes.");
            //return null;

            Matrixd result = new Matrixd(m1.Rows, m1.Columns);

            for (int i = 0; i < m1.Rows; i++)
                for (int j = 0; j < m1.Columns; j++)
                    result[i, j] = m1[i, j] - m2[i, j];

            return result;
        }

        /// <summary>
        /// Create the Inverse of a Matrix
        /// </summary>
        /// <param name="m1">The Matrix you want to Invert</param>
        /// <returns>The inverted matrix</returns>
        public static Matrixd operator !(Matrixd m1)
        {
            return m1.GetInverse();
        }

        /// <summary>
        /// Returns the Inverse of this Quaternion
        /// </summary>
        /// <returns>The Inverted Matrix</returns>
        /// <remarks>Based on code by Rajitha Wimalasooriya (http://www.thecodeproject.com/csharp/rtwmatrix.asp)</remarks>
        /// <exception cref="GeometryException">Thrown if the Matrix is Singular (<see cref="Determinant"/>==0)</exception>		
        public Matrixd GetInverse()
        {
            if (Determinant() == 0)
                throw new GeometryException("Attempt to invert a singular matrix.");

            //inverse of a 2x2 matrix
            if (Rows == 2 && Columns == 2)
            {
                Matrixd tempMtx = new Matrixd(2, 2);

                tempMtx[0, 0] = this[1, 1];
                tempMtx[0, 1] = -this[0, 1];
                tempMtx[1, 0] = -this[1, 0];
                tempMtx[1, 1] = this[0, 0];

                return tempMtx / Determinant();
            }

            return Adjoint() / Determinant();
        }

        public override string ToString()
        {
            return Rows + "x" + Columns + "-Matrix";
        }

        /// <summary>
        /// Calculate a new HashCode describing the instance
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            double result = 0;

            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Columns; j++)
                    result += this[i, j];

            return (int)result;
        }

        /// <summary>
        /// Does the passed Object contain Equal Data?
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(Matrixd)) return false;

            Matrixd m1 = (Matrixd)obj;
            if (this.Rows != m1.Rows || this.Columns != m1.Columns)
                return false;

            for (int i = 0; i < this.Rows; i++)
                for (int j = 0; j < this.Columns; j++)
                    if (this[i, j] != m1[i, j])
                        return false;

            return true;
        }



        /// <summary>
        /// SMatirx Multiplication
        /// </summary>
        /// <param name="m1">First Matrix</param>
        /// <param name="v">Vector</param>
        /// <returns>The resulting Vector</returns>
        public static Vector3f operator *(Matrixd m1, Vector3f v)
        {
            Matrixd m2 = new Matrixd(v);
            m2 = m1 * m2;
            return m2.GetVector();
        }

        /// <summary>
        /// SMatirx Multiplication
        /// </summary>
        /// <param name="m1">First Matrix</param>
        /// <param name="v">Vector</param>
        /// <returns>The resulting Vector</returns>
        public static Vector4f operator *(Matrixd m1, Vector4f v)
        {
            Matrixd m2 = new Matrixd(v);
            m2 = m1 * m2;
            return m2.GetVector4();
        }

        /// <summary>
        /// Are tow Matrixces equal
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public static bool operator ==(Matrixd m1, Matrixd m2)
        {
            return Equals(m1, m2);
        }

        /// <summary>
        /// Are two Matrices inequal?
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public static bool operator !=(Matrixd m1, Matrixd m2)
        {
            return !(m1 == m2);
        }

        /// <summary>
        /// Calculate the determinant of a Matrix
        /// </summary>
        /// <returns>The determinant</returns>
        /// <exception cref="GeometryException">Thrown, if the Matrix is not a Square Matrix</exception>
        /// <remarks>Based on code by Rajitha Wimalasooriya (http://www.thecodeproject.com/csharp/rtwmatrix.asp)</remarks>
        public double Determinant()
        {
            if (this.Rows != this.Columns)
                throw new GeometryException("You can only compute the Determinant of a Square Matrix. (" + ToString() + ")");

            double d = 0;

            //get the determinent of a 2x2 matrix
            if (this.Rows == 2 && this.Columns == 2)
            {
                d = (this[0, 0] * this[1, 1]) - (this[0, 1] * this[1, 0]);
                return d;
            }

            //get the determinent of a 3x3 matrix
            if (this.Rows == 3 && this.Columns == 3)
            {
                d = (this[0, 0] * this[1, 1] * this[2, 2])
                    + (this[0, 1] * this[1, 2] * this[2, 0])
                    + (this[0, 2] * this[1, 0] * this[2, 1])
                    - (this[0, 2] * this[1, 1] * this[2, 0])
                    - (this[0, 1] * this[1, 0] * this[2, 2])
                    - (this[0, 0] * this[1, 2] * this[2, 1]);
                return d;
            }

#pragma warning disable IDE0059 // Unnecessary assignment of a value
            Matrixd tempMtx = new Matrixd(this.Rows - 1, this.Columns - 1);
#pragma warning restore IDE0059 // Unnecessary assignment of a value

            //find the determinent with respect to the first row
            for (int j = 0; j < this.Columns; j++)
            {
                tempMtx = this.Minor(0, j);

                //recursively add the determinents
                d += (int)Math.Pow(-1, j) * this[0, j] * tempMtx.Determinant();

            }

            return d;
        }

        /// <summary>
        /// Calculate the Adjoint of a Matrix
        /// </summary>
        /// <returns>The adjoint</returns>
        /// <exception cref="GeometryException">Thrown if <see cref="Rows"/> or <see cref="Columns"/> is less than 2.</exception>
        /// <remarks>Based on code by Rajitha Wimalasooriya (http://www.thecodeproject.com/csharp/rtwmatrix.asp)</remarks>
        public Matrixd Adjoint()
        {

            if (this.Rows < 2 || this.Columns < 2)
                throw new GeometryException("Adjoint matrix is not available. (" + ToString() + ")");

#pragma warning disable IDE0059 // Unnecessary assignment of a value
            Matrixd tempMtx = new Matrixd(this.Rows - 1, this.Columns - 1);
#pragma warning restore IDE0059 // Unnecessary assignment of a value
            Matrixd adjMtx = new Matrixd(this.Columns, this.Rows);

            for (int i = 0; i < this.Rows; i++)
                for (int j = 0; j < this.Columns; j++)
                {
                    tempMtx = this.Minor(i, j);

                    //put the determinent of the minor in the transposed position
                    adjMtx[j, i] = (int)Math.Pow(-1, i + j) * tempMtx.Determinant();
                }

            return adjMtx;
        }

        /// <summary>
        /// Create the Minor/cofactor Matrix
        /// </summary>
        /// <param name="row">row that should be removed<</param>
        /// <param name="column">column that should be removed</param>
        /// <exception cref="GeometryException">Thrown if <see cref="Rows"/> or <see cref="Columns"/> is less than 2.</exception>
        /// <returns>The Minor Matrix for the given row/column</returns>
        /// <remarks>Based on code by Rajitha Wimalasooriya (http://www.thecodeproject.com/csharp/rtwmatrix.asp)</remarks>
        public Matrixd Minor(int row, int column)
        {

            if (this.Rows < 2 || this.Columns < 2)
                throw new GeometryException("Minor not available. (" + ToString() + ")");

            Matrixd minom2 = new Matrixd(this.Rows - 1, this.Columns - 1);

            //find the minor with respect to the first element
            for (int k = 0; k < minom2.Rows; k++)
            {
                int i;
                if (k >= row)
                    i = k + 1;
                else
                    i = k;

                for (int l = 0; l < minom2.Columns; l++)
                {
                    int j;

                    if (l >= column)
                        j = l + 1;
                    else
                        j = l;

                    minom2[k, l] = this[i, j];
                }
            }

            return minom2;
        }

        /// <summary>
        /// Returns true, if this is the identity matrix
        /// </summary>
        /// <remarks>Based on code by Rajitha Wimalasooriya (http://www.thecodeproject.com/csharp/rtwmatrix.asp)</remarks>
        public bool Identity
        {
            get
            {
                if (Rows != Columns)
                    return false;

                for (int i = 0; i < Rows; i++)
                    for (int j = 0; j < Columns; j++)
                    {
                        if (i == j)
                        {
                            if (this[i, j] != 1.0f) return false;
                        }
                        else
                        {
                            if (this[i, j] != 0.0f) return false;
                        }
                    }

                return true;
            }
        }

        /// <summary>
        /// True if this Matrix is invertibale
        /// </summary>
        public bool Invertable
        {
            get { return (this.Determinant() != 0); }
        }

        /// <summary>
        /// True if the Matrix is Orthogonal
        /// </summary>
        public bool Orthogonal
        {
            get
            {
                if (Rows != Columns) return false;

                Matrixd t = this * this.GetTranspose();
                if (!t.Identity) return false;

                t = this.GetTranspose() * this;
                if (!t.Identity) return false;

                return true;
            }
        }

        /// <summary>
        /// Create  Translation Matrixd
        /// </summary>
        /// <param name="v">The Translation Vector</param>
        /// <returns>a new Translation Matrixd</returns>
        public static Matrixd Translation(Vector3f v)
        {
            return Translation(v.X, v.Y, v.Z);
        }

        /// <summary>
        /// Create a Translation Matrixd
        /// </summary>
        /// <param name="x">Translation to x</param>
        /// <param name="y">Translation to y</param>
        /// <param name="z">Translation to z</param>
        /// <returns>a new Translation Matrixd</returns>
        public static Matrixd Translation(double x, double y, double z)
        {
            Matrixd m = new Matrixd(4, 4);
            m[0, 0] = 1; m[0, 1] = 0; m[0, 2] = 0; m[0, 3] = x;
            m[1, 0] = 0; m[1, 1] = 1; m[1, 2] = 0; m[1, 3] = y;
            m[2, 0] = 0; m[2, 1] = 0; m[2, 2] = 1; m[2, 3] = z;
            m[3, 0] = 0; m[3, 1] = 0; m[3, 2] = 0; m[3, 3] = 1;

            return m;
        }

        /// <summary>
        /// Create a UniformScale Matrixd
        /// </summary>
        /// <param name="x">Scales</param>
        /// <returns>a new Translation Matrixd</returns>
        public static Matrixd Scale(double s)
        {
            return Scale(s, s, s);
        }

        /// <summary>
        /// Create a Scale Matrixd
        /// </summary>
        /// <param name="x">Scale in x</param>
        /// <param name="y">Scale in y</param>
        /// <param name="z">Scale in z</param>
        /// <returns>a new Translation Matrixd</returns>
        public static Matrixd Scale(double x, double y, double z)
        {
            Matrixd m = new Matrixd(4, 4);
            m[0, 0] = x; m[0, 1] = 0; m[0, 2] = 0; m[0, 3] = 0;
            m[1, 0] = 0; m[1, 1] = y; m[1, 2] = 0; m[1, 3] = 0;
            m[2, 0] = 0; m[2, 1] = 0; m[2, 2] = z; m[2, 3] = 0;
            m[3, 0] = 0; m[3, 1] = 0; m[3, 2] = 0; m[3, 3] = 1;

            return m;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="yaw">Y-Component of a Rotation Vector</param>
        /// <param name="pitch">X-Component of a Rotation Vector</param>
        /// <param name="roll">Z-Component of a Rotation Vector</param>
        /// <returns></returns>
        public static Matrixd RotateYawPitchRoll(double yaw, double pitch, double roll)
        {
            return RotateY(yaw) * RotateX(pitch) * RotateZ(roll);
        }

        /// <summary>
        /// Rotation round the X-Axis
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Matrixd RotateX(double angle)
        {
            Matrixd m = Matrixd.GetIdentity(4, 4);

            m[1, 1] = Math.Cos(angle);
            m[1, 2] = -Math.Sin(angle);

            m[2, 1] = Math.Sin(angle);
            m[2, 2] = Math.Cos(angle);

            return m;
        }

        /// <summary>
        /// Rotation round the X-Axis
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Matrixd RotateY(double angle)
        {
            Matrixd m = Matrixd.GetIdentity(4, 4);

            m[0, 0] = Math.Cos(angle);
            m[0, 2] = Math.Sin(angle);

            m[2, 0] = -Math.Sin(angle);
            m[2, 2] = Math.Cos(angle);

            return m;
        }

        /// <summary>
        /// Rotation round the X-Axis
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Matrixd RotateZ(double angle)
        {
            Matrixd m = Matrixd.GetIdentity(4, 4);

            m[0, 0] = Math.Cos(angle);
            m[0, 1] = -Math.Sin(angle);

            m[1, 0] = Math.Sin(angle);
            m[1, 1] = Math.Cos(angle);

            return m;
        }



        public string ToString(bool full)
        {
            if (!full) return this.ToString();

            string s = "";
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                {
                    s += this[r, c].ToString("N3") + " | ";
                }
                s += "\n";
            }

            return s;
        }

        public Matrixd To33Matrix()
        {
            Matrixd m = Matrixd.GetIdentity(3, 3);

            for (int r = 0; r < Math.Min(3, this.Rows); r++)
                for (int c = 0; c < Math.Min(3, this.Columns); c++)
                    m[r, c] = this[r, c];

            return m;
        }
    }
}
