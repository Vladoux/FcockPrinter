using System;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using fair_mark_desktop.Extensions;

namespace fair_mark_desktop
{

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            using (var mutex = new Mutex(true, "fair-mark-desktop", out var createdNew))
            {
                if (createdNew)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new Form1(args));
                }
                else
                {
                    Path.Combine(Path.GetTempPath(), $"FCode\\url.txt").WriteToFile(args.FirstOrDefault());
                }
            }
        }
    }
}
