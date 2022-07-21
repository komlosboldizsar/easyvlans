using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace easyvlans.GUI.Helpers
{
    internal static class InvokeHelpers
    {
        public static void InvokeIfRequired(this Control control, Action action)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(action);
                return;
            }
            action();
        }
    }
}
