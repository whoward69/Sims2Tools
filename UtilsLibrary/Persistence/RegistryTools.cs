/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Microsoft.Win32;
using Sims2Tools.Cache;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Sims2Tools.Utils.Persistence
{
    public class RegistryTools
    {
        public static bool IsUpdateCheckDue(string AppRegKey)
        {
            long nextUpdateCheck = (long)GetSetting(AppRegKey, "NextUpdateCheck", 1L);

            return (nextUpdateCheck > 0 && DateTime.Now.ToFileTimeUtc() > nextUpdateCheck);
        }

        public static void SetNextUpdateCheck(string AppRegKey)
        {
            DateTime nextCheck = DateTime.Now;

            int updateCheckFrequency = (int)GetSetting(AppRegKey, "UpdateCheckFrequency", 7);

            if (updateCheckFrequency < 0) updateCheckFrequency = 7;

            if (updateCheckFrequency > 0)
            {
                /* Normalise to 6am */
                nextCheck = nextCheck.AddHours(-(nextCheck.Hour - 6));
                nextCheck = nextCheck.AddMinutes(-(nextCheck.Minute));
                nextCheck = nextCheck.AddSeconds(-(nextCheck.Second));

                nextCheck = nextCheck.AddDays(updateCheckFrequency);
                SaveSetting(AppRegKey, "NextUpdateCheck", nextCheck.ToFileTimeUtc(), RegistryValueKind.QWord);
#if DEBUG
                SaveSetting(AppRegKey, "NextUpdateCheckString", $"{nextCheck.ToShortDateString()} {nextCheck.ToShortTimeString()}");
#endif
            }
            else
            {
                SaveSetting(AppRegKey, "NextUpdateCheck", 0, RegistryValueKind.QWord);
#if DEBUG
                SaveSetting(AppRegKey, "NextUpdateCheckString", $"Never");
#endif
            }
        }

        public static void SetUpdateCheckFrequency(string AppRegKey, int days)
        {
            SaveSetting(AppRegKey, "UpdateCheckFrequency", days);

            SetNextUpdateCheck(AppRegKey);
        }

        public static int GetUpdateCheckFrequency(string AppRegKey)
        {
            return (int)GetSetting(AppRegKey, "UpdateCheckFrequency", 7);
        }

        public delegate void VersionChangeCallback(int prevVersionMajor, int prevVersionMinor);

        public static void LoadAppSettings(string AppRegKey, int versionMajor, int versionMinor, VersionChangeCallback versionChange = null)
        {
            int verMajor = (int)GetSetting(AppRegKey, "VersionMajor", 0);
            int verMinor = (int)GetSetting(AppRegKey, "VersionMinor", 0);

            if (versionMajor != verMajor || versionMinor != verMinor)
            {
                versionChange?.Invoke(verMajor, verMinor);

                DeleteSetting(AppRegKey, "FormWidth");
                DeleteSetting(AppRegKey, "FormHeight");
            }

            // We need to do this here so the Updater has the correct info to work from
            SaveAppSettings(AppRegKey, versionMajor, versionMinor);
        }

        public static void SaveAppSettings(string AppRegKey, int versionMajor, int versionMinor)
        {
            SaveSetting(AppRegKey, "VersionMajor", versionMajor);
            SaveSetting(AppRegKey, "VersionMinor", versionMinor);
        }

        public static void LoadFormSettings(string AppRegKey, Form frm)
        {
            Rectangle formArea = new Rectangle((int)GetSetting(AppRegKey, "FormLeft", frm.Left), (int)GetSetting(AppRegKey, "FormTop", frm.Top),
                                               (int)GetSetting(AppRegKey, "FormWidth", frm.Width), (int)GetSetting(AppRegKey, "FormHeight", frm.Height));

            if (Screen.AllScreens.Any(s => s.WorkingArea.IntersectsWith(formArea)))
            {
                frm.SetBounds(formArea.Left, formArea.Top, formArea.Width, formArea.Height);
            }

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
                SaveSetting(AppRegKey, "FormWidth", frm.Width);
                SaveSetting(AppRegKey, "FormHeight", frm.Height);
            }
            else
            {
                // Save location when we're restored.
                SaveSetting(AppRegKey, "FormLeft", frm.RestoreBounds.Left);
                SaveSetting(AppRegKey, "FormTop", frm.RestoreBounds.Top);
                SaveSetting(AppRegKey, "FormWidth", frm.RestoreBounds.Width);
                SaveSetting(AppRegKey, "FormHeight", frm.RestoreBounds.Height);
            }
        }

        public static void LoadPopupSettings(string AppRegKey, Form frm)
        {
            string frmKey = AppRegKey + $"\\Popup\\{frm.Name}";

            Rectangle formArea = new Rectangle((int)GetSetting(frmKey, "FormLeft", frm.Left), (int)GetSetting(frmKey, "FormTop", frm.Top),
                                               (int)GetSetting(frmKey, "FormWidth", frm.Width), (int)GetSetting(frmKey, "FormHeight", frm.Height));

            if (Screen.AllScreens.Any(s => s.WorkingArea.IntersectsWith(formArea)))
            {
                frm.SetBounds(formArea.Left, formArea.Top, formArea.Width, formArea.Height);
            }

            frm.WindowState = (FormWindowState)GetSetting(frmKey, "FormWindowState", frm.WindowState);
        }

        public static void SavePopupSettings(string AppRegKey, Form frm)
        {
            string frmKey = AppRegKey + $"\\Popup\\{frm.Name}";

            SaveSetting(frmKey, "FormWindowState", (int)frm.WindowState);
            if (frm.WindowState == FormWindowState.Normal)
            {
                // Save current location.
                SaveSetting(frmKey, "FormLeft", frm.Left);
                SaveSetting(frmKey, "FormTop", frm.Top);
                SaveSetting(frmKey, "FormWidth", frm.Width);
                SaveSetting(frmKey, "FormHeight", frm.Height);
            }
            else
            {
                // Save location when we're restored.
                SaveSetting(frmKey, "FormLeft", frm.RestoreBounds.Left);
                SaveSetting(frmKey, "FormTop", frm.RestoreBounds.Top);
                SaveSetting(frmKey, "FormWidth", frm.RestoreBounds.Width);
                SaveSetting(frmKey, "FormHeight", frm.RestoreBounds.Height);
            }
        }

        public static void SetAllAdvancedMode()
        {
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey("Software", true);
            RegistryKey myKey = regKey.CreateSubKey(Sims2ToolsLib.RegistryKey);

            foreach (string appKeyName in myKey.GetSubKeyNames())
            {
                RegistryKey appKey = myKey.CreateSubKey(appKeyName);

                if (appKey.OpenSubKey("Mode") != null)
                {
                    RegistryKey modeKey = appKey.CreateSubKey("Mode");
                    modeKey.SetValue("menuItemAdvanced", 1);
                }
            }
        }

        public static bool IsSet(string AppRegKey, string Key)
        {
            RegistryKey reg_key = Registry.CurrentUser.OpenSubKey("Software", true);

            RegistryKey sub_key = reg_key.OpenSubKey(AppRegKey);
            if (sub_key == null) return false;

            return (sub_key.GetValue(Key) != null);
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

        public static object GetPopupSetting(string AppRegKey, Form frm, string Key, object DefaultValue)
        {
            return GetSetting(AppRegKey + $"\\Popup\\{frm.Name}", Key, DefaultValue);
        }

        public static void SavePopupSetting(string AppRegKey, Form frm, string Key, object Value)
        {
            SaveSetting(AppRegKey + $"\\Popup\\{frm.Name}", Key, Value);
        }

        private static void SaveSetting(string AppRegKey, string Key, object Value, RegistryValueKind valueKind)
        {
            RegistryKey reg_key = Registry.CurrentUser.OpenSubKey("Software", true);
            RegistryKey sub_key = reg_key.CreateSubKey(AppRegKey);
            sub_key.SetValue(Key, Value, valueKind);
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

        public static string GetPath(string AppRegKey, string Key)
        {
            string path = GetSetting(AppRegKey, Key, "") as string;

            // Better to do this here rather than in the setter as a user can edit the registry directly
            return path.EndsWith(@"\") ? path.Substring(0, path.Length - 1) : path;
        }

        public static void SetPath(string AppRegKey, string Key, string Value, bool InvalidateCache = true)
        {
            if (Value == null)
            {
                DeleteSetting(AppRegKey, Key);
            }
            else
            {
                SaveSetting(AppRegKey, Key, Value);
            }

            if (InvalidateCache) GameDataCache.Invalidate();
        }
    }
}
