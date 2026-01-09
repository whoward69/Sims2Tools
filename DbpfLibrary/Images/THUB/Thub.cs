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

namespace Sims2Tools.DBPF.Images.THUB
{
    public class Thub : Img
    {

        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        // What about other image types (see link above)
        //   0x4D533EDD (THUB for XNGB in CANHObjectsThumbnails)

        public static readonly TypeTypeID[] TYPES = new TypeTypeID[]
        {
            (TypeTypeID) 0xAC2950C1, // object
            (TypeTypeID) 0xF03D464C, // awning
            (TypeTypeID) 0xCC48C51F, // chimney
            (TypeTypeID) 0x2C488BCA, // dormer
            (TypeTypeID) 0x2C30E040, // fence arch
            (TypeTypeID) 0xCC30CDF8, // fence or halfwall
            (TypeTypeID) 0x8C311262, // floor
            (TypeTypeID) 0x2C43CBD4, // foundation or pool
            (TypeTypeID) 0xCC44B5EC, // modular stair
            (TypeTypeID) 0xCC489E46, // roof
            (TypeTypeID) 0xEC3126C4, // terrain
            (TypeTypeID) 0x8C31125E  // wall
        };

        public enum ThubTypeIndex
        {
            Object,
            Awning,
            Chimney,
            Dormer,
            FenceArch,
            FenceOrHalfwall,
            Floor,
            FoundationOrPool,
            ModularStair,
            Roof,
            Terrain,
            Wall
        }

        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public new static readonly TypeTypeID TYPE = TYPES[(int)ThubTypeIndex.Object];
        public new const string NAME = "THUB";

        public Thub(DBPFEntry entry, DbpfReader reader) : base(entry, reader) { }
    }
}
