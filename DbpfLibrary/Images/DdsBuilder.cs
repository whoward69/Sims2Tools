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

using Sims2Tools.DBPF.IO.TempFiles;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Sims2Tools.DBPF.Images
{
    public interface IDdsBuilder
    {
        DDSData[] BuildDDS(Image img, uint levels, DdsFormats dxtFormat, string extraParameters);
        DDSData[] BuildDDS(string imageInputFullName, uint levels, DdsFormats dxtFormat, string extraParameters);
    }

    public class NvidiaDdsBuilder : IDdsBuilder
    {
        private readonly string ddsUtilsPath;
        private readonly Sims2Tools.DBPF.Logger.IDBPFLogger logger;


        public NvidiaDdsBuilder(string ddsUtilsPath, Sims2Tools.DBPF.Logger.IDBPFLogger logger)
        {
            this.ddsUtilsPath = ddsUtilsPath;
            this.logger = logger;
        }

        public DDSData[] BuildDDS(Image img, uint levels, DdsFormats dxtFormat, string extraParameters)
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

        public DDSData[] BuildDDS(string imageInputFullName, uint levels, DdsFormats ddsFormat, string extraParameters)
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

            if (logger != null) logger.Info($"nvdxt {arguments}");

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
}
