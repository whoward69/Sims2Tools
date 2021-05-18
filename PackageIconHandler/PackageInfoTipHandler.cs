/*
 * Package Icon Handler - a shell extension for displaying icons for .package files
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 * 
 * Based on the SharpShell extensions - see https://www.codeproject.com/Articles/527058/NET-Shell-Extensions-Shell-Info-Tip-Handlers
 *
 * William Howard - 2020-2021
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using SharpShell.Attributes;
using SharpShell.SharpInfoTipHandler;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace PackageIconHandler
{
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.ClassOfExtension, ".package")]
    public class PackageInfoTipHandler : SharpInfoTipHandler
    {
        protected override string GetInfo(RequestedInfoType infoType, bool singleLine)
        {
            if (infoType == RequestedInfoType.InfoTip)
            {
                FileInfo fi = new FileInfo(SelectedItemPath);
                String txtPath = $"{fi.FullName.Substring(0, fi.FullName.Length - fi.Extension.Length)}.txt";
                txtPath = txtPath.Replace(@"\Downloads\", @"\Thumbnails\Downloads\");

                try
                {
                    if (singleLine)
                    {
                        return File.ReadAllLines(txtPath)[0];
                    }
                    else
                    {
                        return File.ReadAllText(txtPath);
                    }
                }
                catch (Exception)
                {
                }
            }

            return Path.GetFileName(SelectedItemPath);
        }
    }
}
