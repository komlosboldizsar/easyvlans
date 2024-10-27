using BToolbox.GUI.RecyclerTables;
using easyvlans.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace easyvlans.GUI
{
    public partial class MainForm
    {
        internal abstract class RowManagerBase<TItem> : RecyclerTableLayoutRowManager<TItem>
            where TItem : class
        {

            protected static void displayStatus(Label label, Status status, DateTime statusUpdateTime)
            {
                string labelText = statusStrings[status];
                if (status != Status.Empty)
                    labelText += $" ({statusUpdateTime:HH:mm:ss})";
                label.Text = labelText;
                label.ForeColor = statusColors[status];
            }

            private static readonly Dictionary<Status, string> statusStrings = new Dictionary<Status, string>()
            {
                { Status.Empty, "-" },
                { Status.Unknown, "unknown" },
                { Status.Querying, "in progress" },
                { Status.Successful, "ready" },
                { Status.Unsuccessful, "error" }
            };

            private static readonly Dictionary<Status, Color> statusColors = new Dictionary<Status, Color>()
            {
                { Status.Empty, SystemColors.ControlDark },
                { Status.Unknown, SystemColors.ControlDark },
                { Status.Querying, SystemColors.ControlText },
                { Status.Successful, SystemColors.ControlText },
                { Status.Unsuccessful, Color.Red }
            };

        }
    }
}
