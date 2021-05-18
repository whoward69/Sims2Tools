/*
 * Package Icon Handler - a shell extension for displaying icons for .package files
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 * 
 * Based on the SharpShell extensions - see https://www.codeproject.com/Articles/522665/NET-Shell-Extensions-Shell-Icon-Handlers
 *
 * William Howard - 2020-2021
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using SharpShell.Attributes;
using SharpShell.SharpIconHandler;
using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

namespace PackageIconHandler
{
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.ClassOfExtension, ".package")]
    public class PackageIconHandler : SharpIconHandler
    {
        protected override Icon GetIcon(bool smallIcon, uint iconSize)
        {
            Icon icon;

            FileInfo fi = new FileInfo(SelectedItemPath);
            String iconPath = $"{fi.FullName.Substring(0, fi.FullName.Length - fi.Extension.Length)}.ico";
            iconPath = iconPath.Replace(@"\Downloads\", @"\Thumbnails\Downloads\");

            try
            {
                icon = new Icon(iconPath);
            }
            catch (Exception)
            {
                icon = Properties.Resources.DefaultPlumbob;
            }

            return GetIconSpecificSize(icon, new Size((int)iconSize, (int)iconSize));
        }
    }
}
