using Sims2Tools.DBPF.Utils;

namespace Sims2Tools.DBPF.SceneGraph
{
    public class ScenegraphHelper
    {
        #region Constant Repeat
        // TODO - needed?
        public const uint GMND = 0x7BA3838C;
        public const uint TXMT = 0x49596978;
        public const uint TXTR = 0x1C4A276C;
        public const uint LIFO = 0xED534136;
        public const uint ANIM = 0xFB00791E;
        public const uint SHPE = 0xFC6EB1F7;
        public const uint CRES = 0xE519C933;
        public const uint GMDC = 0xAC4F8687;

        public const uint MMAT = 0x4C697E5A;
        #endregion

        /// <summary>
        /// Returns a PackedFile Descriptor for the given filename
        /// </summary>
        /// <param name="flname"></param>
        /// <param name="type"></param>
        /// <param name="defgroup"></param>
        /// <returns></returns>
        public static PackedFileDescriptor BuildPfd(string flname, uint type, uint defgroup)
        {
            string name = Hashes.StripHashFromName(flname);
            PackedFileDescriptor pfd = new PackedFileDescriptor
            {
                Type = type,
                Group = Hashes.GetHashGroupFromName(flname, defgroup),
                Instance = Hashes.InstanceHash(name),
                SubType = Hashes.SubTypeHash(name)
                // TODO - needed? pfd.Filename = flname;
            };

            return pfd;
        }
    }
}
