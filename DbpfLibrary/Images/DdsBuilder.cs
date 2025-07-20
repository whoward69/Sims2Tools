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

        public DDSData[] BuildDDS(string imageInputFullName, uint levels, DdsFormats dxtFormat, string extraParameters)
        {
            string exePath = $"{ddsUtilsPath}\\nvdxt.exe";

            if (!File.Exists(exePath))
            {
                return new DDSData[0];
            }

            string ddsOutputFullName = TempFile.GetTempFileName(".dds");

            string arguments = $"-file \"{imageInputFullName}\" -output \"{ddsOutputFullName}\"";

            if (dxtFormat == DdsFormats.DXT1Format)
                arguments += " -dxt1c";
            else if (dxtFormat == DdsFormats.DXT3Format)
                arguments += " -dxt3";
            else if (dxtFormat == DdsFormats.DXT5Format)
                arguments += " -dxt5";
            else
                throw new ArgumentException("Expected DXT1, DXT3 or DXT5 format");

            arguments += $" -nmips {levels}";

            if (!string.IsNullOrWhiteSpace(extraParameters))
            {
                arguments += $" {extraParameters.Trim()}";
            }

            logger.Info($"nvdxt {arguments}");

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
