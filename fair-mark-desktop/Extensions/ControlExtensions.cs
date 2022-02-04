using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fair_mark_desktop.Extensions
{
    public static class ControlExtensions
    {
        public static void Execute(this Control source, Action action)
        {
            source.Invoke((MethodInvoker)(() => action()));
        }
    }
}
