using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using System.Security.Permissions;
using fair_mark_desktop.Extensions;

namespace fair_mark_desktop
{

    static class Program
    {
        [DllImport("user32.dll")]
        static extern IntPtr SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        static extern IntPtr ShowWindow(IntPtr hWnd, int nCmdShow);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Path.Combine(Path.GetTempPath(), $"FCode\\url.txt").WriteToFile(args.FirstOrDefault());

            CheckRegistry();

            using (Mutex mutex = new Mutex(true, "fair-mark-desktop", out var createdNew))
            {
                if (createdNew)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new Form1());
                }
                else
                {
                    Process[] procs = Process.GetProcessesByName(Application.ProductName);
                    if (procs.Length > 1)
                    {
                        int index;
                        if ((int)procs[0].MainWindowHandle != 0) index = 0;
                        else index = 1;
                        SetForegroundWindow(procs[index].MainWindowHandle);
                        ShowWindow(procs[index].MainWindowHandle, 9);
                        return;
                    }
                }
            }
        }
        
        private static void CheckRegistry()
        {
            var keyTest = Registry.ClassesRoot;

            if (!keyTest.GetSubKeyNames().Contains("FCode"))
            {
                RegistryKey key = keyTest.CreateSubKey("FCode");
                key.SetValue("URL Protocol", "FCode Protocol");
                key.CreateSubKey(@"shell\open\command").SetValue("", $"\"{Application.ExecutablePath}\" %1");
            }

            var reg = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run\\");
            if (reg.GetValue("Fcode") == null)
            {
                reg.SetValue("Fcode", Application.ExecutablePath);
                reg.Close();
            }
        }
    }
}
