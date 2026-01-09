/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF.SceneGraph.TXMT;
using System;
using System.Drawing;

namespace Sims2Tools.Helpers
{
    public class ColourHelper
    {
        public static Color ColourFromTxmtProperty(Txmt txmt, string name)
        {
            string value = txmt.MaterialDefinition?.GetProperty(name);

            if (value != null)
            {
                string[] rgbs = value.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                try
                {
                    int r = (int)(Double.Parse(rgbs[0]) * 255);
                    int g = (int)(Double.Parse(rgbs[1]) * 255);
                    int b = (int)(Double.Parse(rgbs[2]) * 255);

                    return Color.FromArgb(r, g, b);
                }
                catch (Exception) { }

            }

            return Color.Blue; // Keep with Sims 2 using blue as a missing texture
        }

        public static void SetTxmtPropertyFromColour(Txmt txmt, string name, Color colour)
        {
            if (txmt.MaterialDefinition?.GetProperty(name) != null)
            {
                string value = $"{(colour.R / 255.0).ToString("0.00000")},{(colour.G / 255.0).ToString("0.00000")},{(colour.B / 255.0).ToString("0.00000")}";
                txmt.MaterialDefinition.SetProperty(name, value);
            }
        }
    }
}
