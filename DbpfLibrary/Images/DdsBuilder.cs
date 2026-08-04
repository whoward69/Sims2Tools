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

using Sims2Tools.DBPF.IO.TempFiles;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Sims2Tools.DBPF.Images.DdsBuilder
{
    public interface IDdsBuilder
    {
        DDSData[] BuildDDS(Image img, uint levels, DdsFormats dxtFormat, string extraParameters);
        DDSData[] BuildDDS(string imageInputFullName, uint levels, DdsFormats dxtFormat, string extraParameters);
    }

    public abstract class DdsBuilder : IDdsBuilder
    {
        private static IDdsBuilder ddsBuilder = null;
        private static string ddsUtilsPath = null;

        public static string DdsUtilsPath
        {
            get => ddsUtilsPath;
            set => ddsUtilsPath = value;
        }

        public static IDdsBuilder GetDdsBuilder(Sims2Tools.DBPF.Logger.IDBPFLogger logger)
        {
            if (ddsBuilder == null)
            {
                if (ddsUtilsPath != null)
                {
                    ddsBuilder = new NvidiaDdsBuilder(logger);
                }
            }

            return ddsBuilder;
        }

        public abstract DDSData[] BuildDDS(string imageInputFullName, uint levels, DdsFormats dxtFormat, string extraParameters);

        public abstract DDSData[] BuildDDS(Image img, uint levels, DdsFormats dxtFormat, string extraParameters);


        private class NvidiaDdsBuilder : DdsBuilder
        {
            private readonly Sims2Tools.DBPF.Logger.IDBPFLogger logger;

            internal NvidiaDdsBuilder(Sims2Tools.DBPF.Logger.IDBPFLogger logger)
            {
                this.logger = logger;
            }

            public override DDSData[] BuildDDS(Image img, uint levels, DdsFormats dxtFormat, string extraParameters)
            {
                string imageInputFullName = TempFile.GetTempFileName(".png");

                img.Save(imageInputFullName, ImageFormat.Png);

                try
                {
                    return BuildDDS(imageInputFullName, levels, dxtFormat, extraParameters);
                }
                finally
                {
                    File.Delete(imageInputFullName);
                }
            }

            public override DDSData[] BuildDDS(string imageInputFullName, uint levels, DdsFormats ddsFormat, string extraParameters)
            {
                string exePath = $"{ddsUtilsPath}\\nvdxt.exe";

                if (!File.Exists(exePath))
                {
                    return new DDSData[0];
                }

                string ddsOutputFullName = TempFile.GetTempFileName(".dds");

                string arguments = $"-file \"{imageInputFullName}\" -output \"{ddsOutputFullName}\"";

                if (ddsFormat == DdsFormats.DXT1Format)
                    arguments += " -dxt1c";
                else if (ddsFormat == DdsFormats.DXT3Format)
                    arguments += " -dxt3";
                else if (ddsFormat == DdsFormats.DXT5Format)
                    arguments += " -dxt5";
                else if (ddsFormat == DdsFormats.Raw8Bit || ddsFormat == DdsFormats.ExtRaw8Bit)
                    arguments += " -a8";
                else if (ddsFormat == DdsFormats.Raw24Bit || ddsFormat == DdsFormats.ExtRaw24Bit)
                    arguments += " -u888";
                else if (ddsFormat == DdsFormats.Raw32Bit)
                    arguments += " -u8888";
                else
                    throw new ArgumentException("Unsupported format");

                arguments += $" -nmips {levels}";

                if (!string.IsNullOrWhiteSpace(extraParameters))
                {
                    arguments += $" {extraParameters.Trim()}";
                }

                logger?.Info($"nvdxt {arguments}");

                try
                {
                    Process p = new Process();
                    p.StartInfo.FileName = exePath;
                    p.StartInfo.Arguments = arguments;

                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.CreateNoWindow = true;

                    p.Start();

                    p.WaitForExit();
                    p.Close();

                    return DdsLoader.ParseDDS(ddsOutputFullName);
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
                finally
                {
                    File.Delete(ddsOutputFullName);
                }
            }
        }


        private class BCnEncoderDdsBuilder : DdsBuilder
        {
            private readonly Sims2Tools.DBPF.Logger.IDBPFLogger logger;

            // BCnEncoder kinda requires ImageSharp, and that requires .Net8
            internal BCnEncoderDdsBuilder(Sims2Tools.DBPF.Logger.IDBPFLogger logger)
            {
                this.logger = logger;
            }

            public override DDSData[] BuildDDS(Image img, uint levels, DdsFormats dxtFormat, string extraParameters)
            {
                throw new NotImplementedException();
            }

            public override DDSData[] BuildDDS(string imageInputFullName, uint levels, DdsFormats ddsFormat, string extraParameters)
            {
                throw new NotImplementedException();
            }
        }
    }
}
