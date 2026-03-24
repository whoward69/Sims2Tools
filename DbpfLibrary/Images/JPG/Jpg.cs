/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF.Images.IMG;
using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.Package;

namespace Sims2Tools.DBPF.Images.JPG
{
    public class Jpg : Img
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        // What about other image types (see link above)
        //   0x424D505F

        public static readonly TypeTypeID[] TYPES = new TypeTypeID[]
        {
            (TypeTypeID) 0x8C3CE95A, // Normal
            (TypeTypeID) 0x0C7E9A76  // CasThumbnails
        };

        public enum JpgTypeIndex
        {
            Normal,
            CasThumbnail
        }

        public new static readonly TypeTypeID TYPE = TYPES[(int)JpgTypeIndex.Normal];
        public new const string NAME = "JPG";

        public Jpg(DBPFEntry entry, DbpfReader reader) : base(entry, reader) { }
    }
}
