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
using fair_mark_desktop.Service;

namespace fair_mark_desktop
{

    static class Program
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        public static Form1 MainForm = null;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            
            var service = new DesktopService();
            File.WriteAllText(Path.Combine(service.GetPath(), $"url.txt"), args.FirstOrDefault());

            var KeyTest = Registry.ClassesRoot;

            if (!KeyTest.GetSubKeyNames().Contains("FCode"))
            {
                RegistryKey key = KeyTest.CreateSubKey("FCode");
                key.SetValue("URL Protocol", "FCode Protocol");
                key.CreateSubKey(@"shell\open\command").SetValue("", $"\"{Application.ExecutablePath}\" %1");
            }

            bool createdNew = true;
            using (Mutex mutex = new Mutex(true, "fair-mark-desktop", out createdNew))
            {
                if (createdNew)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    if (MainForm == null)
                        MainForm = new Form1(args);
                    Application.Run(MainForm);
                }
                else
                {
                    Process current = Process.GetCurrentProcess();
                    foreach (Process process in Process.GetProcessesByName(current.ProcessName))
                    {
                        if (process.Id != current.Id)
                        {
                            SetForegroundWindow(process.MainWindowHandle);
                            break;
                        }
                    }
                }
            }

        }
    }
}
