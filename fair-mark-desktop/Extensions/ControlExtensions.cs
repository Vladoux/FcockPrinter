using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace fair_mark_desktop.Extensions
{
    public static class ControlExtensions
    {
        public static void Execute(this Control source, Action action)
        {
            source.Invoke((MethodInvoker)(() => action()));
        }

        public static void CreateContextMenu(this MaterialCheckbox source, Dictionary<string, EventHandler> actions)
        {
            var menu = new MaterialContextMenuStrip();

            foreach (var action in actions)
            {
                var item = new MaterialToolStripMenuItem
                {
                    Text = action.Key,
                    AutoSize = true
                };
                item.Click += action.Value;
                menu.Items.Add(item);
            }
            source.ContextMenuStrip = menu;
        }

        public static MaterialCheckbox GetItemBoxFromContext(this MaterialToolStripMenuItem source)
        {
            return (MaterialCheckbox)((MaterialContextMenuStrip)(source.GetCurrentParent())).SourceControl;
        }
    }
}
