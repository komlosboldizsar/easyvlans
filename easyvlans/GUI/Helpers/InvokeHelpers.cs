using System;
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
