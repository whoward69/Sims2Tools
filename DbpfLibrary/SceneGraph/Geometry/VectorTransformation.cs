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

namespace Sims2Tools.DBPF.SceneGraph.Geometry

{
    public class VectorTransformation
    {
        public const double SMALL_NUMBER = 0.000001;

        public enum TransformOrder : byte
        {
            RotateThenTranslate = 0,
            TranslateThenRotate = 1
        };

        private readonly TransformOrder o;
        private readonly Vector3f trans;
        private readonly Quaternion quat;

        public TransformOrder Order
        {
            get { return o; }
        }

        public Vector3f Translation
        {
            get { return trans; }
        }

        public Quaternion Rotation
        {
            get { return quat; }
        }

        public VectorTransformation(TransformOrder o)
        {
            this.o = o;
            trans = new Vector3f();
            quat = Quaternion.Identity;
        }

        public virtual void Unserialize(DbpfReader reader)
        {
            if (o == TransformOrder.RotateThenTranslate)
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

        public virtual uint FileSize => quat.FileSize + trans.FileSize;

        public virtual void Serialize(DbpfWriter writer)
        {
            if (o == TransformOrder.RotateThenTranslate)
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

        public override string ToString()
        {
            return $"trans={trans}    rot={quat}";
        }
    }
}
