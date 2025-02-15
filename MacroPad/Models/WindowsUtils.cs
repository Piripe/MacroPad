using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using WindowsShortcutFactory;

namespace MacroPad.Models
{
    [SupportedOSPlatform("windows")]
    internal static class WindowsUtils
    {
        public struct StartupStatus
        {
            public bool OnStartup { get; set; }
            public bool Minimized { get; set; }
        }
        public static StartupStatus GetAppStartupStatus()
        {
            StartupStatus status = new StartupStatus();
            var processPath = Environment.ProcessPath;
            if (processPath == null)
            {
                Debug.Fail("Could not get process path, startup shortcut not detected.");
                return status;
            }

            var processName = Path.GetFileNameWithoutExtension(processPath);
            var startupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            var shortcutAddress = Path.Combine(startupFolder, processName + ".lnk");

            if (File.Exists(shortcutAddress))
            {
                status.OnStartup = true;
                using (var shortcut = WindowsShortcut.Load(shortcutAddress))
                {
                    if (shortcut.Arguments?.Contains("-m") ?? false) status.Minimized = true;
                }
            }

            return status;
        }
        public static void SetAppRunOnLaunch(bool runOnLaunch, bool runMinimized)
        {
            var processPath = Environment.ProcessPath;
            if (processPath == null)
            {
                Debug.Fail("Could not get process path, startup shortcut not created.");
                return;
            }

            var processName = Path.GetFileNameWithoutExtension(processPath);
            var startupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            var shortcutAddress = Path.Combine(startupFolder, processName + ".lnk");

            if (File.Exists(shortcutAddress) || !runOnLaunch)
            {
                File.Delete(shortcutAddress);
            }

            if (!runOnLaunch)
            {
                return;
            }

            using WindowsShortcut shortcut = new() { Path = processPath, WorkingDirectory = Path.GetDirectoryName(processPath), Arguments = runMinimized?"-m":null };
            shortcut.Save(shortcutAddress);
        }
    }
}
