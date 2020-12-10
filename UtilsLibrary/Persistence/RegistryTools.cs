/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020
 *
 * Parts of this code derived from http://csharphelper.com/blog/2018/06/build-an-mru-list-in-c/
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Microsoft.Win32;
using System.Windows.Forms;

namespace Sims2Tools.Utils.Persistence
{
    public class RegistryTools
    {
        public static void LoadAppSettings(string AppRegKey, int versionMajor, int versionMinor)
        {
            int verMajor = (int)GetSetting(AppRegKey, "VersionMajor", 0);
            int verMinor = (int)GetSetting(AppRegKey, "VersionMinor", 0);

            if (versionMajor != verMajor || versionMinor != verMinor)
            {
            }
        }

        public static void SaveAppSettings(string AppRegKey, int versionMajor, int versionMinor)
        {
            SaveSetting(AppRegKey, "VersionMajor", versionMajor);
            SaveSetting(AppRegKey, "VersionMinor", versionMinor);
        }

        public static void LoadFormSettings(string AppRegKey, Form frm)
        {
            frm.SetBounds(
                (int)GetSetting(AppRegKey, "FormLeft", frm.Left),
                (int)GetSetting(AppRegKey, "FormTop", frm.Top),
                frm.Width,
                frm.Height);
            frm.WindowState = (FormWindowState)GetSetting(AppRegKey, "FormWindowState", frm.WindowState);
        }

        public static void SaveFormSettings(string AppRegKey, Form frm)
        {
            SaveSetting(AppRegKey, "FormWindowState", (int)frm.WindowState);
            if (frm.WindowState == FormWindowState.Normal)
            {
                // Save current location.
                SaveSetting(AppRegKey, "FormLeft", frm.Left);
                SaveSetting(AppRegKey, "FormTop", frm.Top);
            }
            else
            {
                // Save location when we're restored.
                SaveSetting(AppRegKey, "FormLeft", frm.RestoreBounds.Left);
                SaveSetting(AppRegKey, "FormTop", frm.RestoreBounds.Top);
            }
        }

        public static object GetSetting(string AppRegKey, string Key, object DefaultValue)
        {
            RegistryKey reg_key = Registry.CurrentUser.OpenSubKey("Software", true);
            RegistryKey sub_key = reg_key.CreateSubKey(AppRegKey);
            return sub_key.GetValue(Key, DefaultValue);
        }

        public static void SaveSetting(string AppRegKey, string Key, object Value)
        {
            RegistryKey reg_key = Registry.CurrentUser.OpenSubKey("Software", true);
            RegistryKey sub_key = reg_key.CreateSubKey(AppRegKey);
            sub_key.SetValue(Key, Value);
        }

        public static void DeleteSetting(string AppRegKey, string Key)
        {
            RegistryKey reg_key = Registry.CurrentUser.OpenSubKey("Software", true);
            RegistryKey sub_key = reg_key.CreateSubKey(AppRegKey);
            try
            {
                sub_key.DeleteValue(Key);
            }
            catch
            {
            }
        }
    }
}
