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

namespace fair_mark_desktop
{

    static class Program
    {
        [DllImport("user32.dll")]
        static extern IntPtr SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        static extern IntPtr ShowWindow(IntPtr hWnd, int nCmdShow);

        public static Form1 MainForm = null;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            
            File.WriteAllText(Path.Combine(Path.GetTempPath(), $"url.txt"), args.FirstOrDefault());

            var KeyTest = Registry.ClassesRoot;

            if (!KeyTest.GetSubKeyNames().Contains("FCode"))
            {
                RegistryKey key = KeyTest.CreateSubKey("FCode");
                key.SetValue("URL Protocol", "FCode Protocol");
                key.CreateSubKey(@"shell\open\command").SetValue("", $"\"{Application.ExecutablePath}\" %1");
            }

            RegistryKey reg;
            reg = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run\\");
            if (reg.GetValue("Fcode") == null)
            {
                reg.SetValue("Fcode", Application.ExecutablePath);
                reg.Close();
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
                    //Process current = Process.GetCurrentProcess();
                    //foreach (Process process in Process.GetProcessesByName(current.ProcessName))
                    //{
                    //    if (process.Id != current.Id)
                    //    {

                    //        SetForegroundWindow(process.MainWindowHandle);

                    //        break;
                    //    }
                    //}
                    

                    // At start-up - Get the number of instances of this app
                    Process[] procs = Process.GetProcessesByName(Application.ProductName);
                    if (procs.Length > 1)
                    {
                        // the previously running instance will be at either index 0 or 1
                        int index;
                        if ((int)procs[0].MainWindowHandle != 0) index = 0;
                        else index = 1;
                        SetForegroundWindow(procs[index].MainWindowHandle);
                        // 9 = SW_RESTORE (winuser.h)
                        ShowWindow(procs[index].MainWindowHandle, 9);
                        return; // exit, terminate this instance
                    }
                }
            }

        }
    }
}
