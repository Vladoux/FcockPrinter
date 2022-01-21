using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fair_mark_desktop
{
    [RunInstaller(true)]
    public partial class InstallerClass : System.Configuration.Install.Installer
    {
        public InstallerClass()
        {
            InitializeComponent();

        }
        //public override void Install(System.Collections.IDictionary stateSaver)
        //{
        //    base.Install(stateSaver);
        //    var str = string.Empty;
        //    str += $"{Application.ExecutablePath}";
            
        //    System.Windows.Forms.MessageBox.Show($"ЛЕЗГИНЫ ПИДОРАСЫ\n{str}");
        //}
    }
}
