using easyvlans.GUI;
using easyvlans.GUI.Helpers;
using easyvlans.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace easyvlans.GUI
{
    public partial class MainForm
    {

        private abstract class RowControls<TRowControls, TItem>
            where TRowControls : RowControls<TRowControls, TItem>, new()
            where TItem : class
        {

            protected static MainForm mainForm;
            protected static TableLayoutPanel table;
            protected static float originalRowHeight;
            private static List<TRowControls> rowControls = new();
            protected const int HEADER_ROWS = 1;

            public static void Init(MainForm mainForm, TableLayoutPanel table)
            {
                RowControls<TRowControls, TItem>.mainForm = mainForm;
                RowControls<TRowControls, TItem>.table = table;
                originalRowHeight = table.RowStyles[HEADER_ROWS].Height;
            }

            public static void Create(int itemIndex)
            {
                TRowControls rowControls = new TRowControls()
                {
                    _rowStyle = (itemIndex > 0) ? new RowStyle(SizeType.Absolute, originalRowHeight) : table.RowStyles[HEADER_ROWS]
                };
                rowControls.createControls(itemIndex);
                rowControls.Shown = false;
                RowControls<TRowControls, TItem>.rowControls.Add(rowControls);
                mainForm.Size = new Size(mainForm.Size.Width, mainForm.Size.Height + (int)originalRowHeight);
            }

            public static void CreateAll(int count)
            {
                table.SuspendLayout();
                for (int i = 0; i < count; i++)
                    Create(i);
                table.ResumeLayout(true);
            }

            public static void Bind(IEnumerable<TItem> items)
            {
                int itemCount = items.Count();
                int rowCount = rowControls.Count();
                int maxCount = (itemCount > rowCount) ? itemCount : rowCount;
                IEnumerator<TItem> itemsEnumerator = items.GetEnumerator();
                bool itemsEnumeratorValid = itemsEnumerator.MoveNext();
                table.SuspendLayout();
                for (int i = 0; i < maxCount; i++)
                {
                    if (i >= rowCount)
                        Create(i);
                    rowControls[i].Item = itemsEnumeratorValid ? itemsEnumerator.Current : null;
                    itemsEnumeratorValid = itemsEnumerator.MoveNext();
                }
                table.ResumeLayout(true);
            }

            protected RowControls() { }

            protected RowStyle _rowStyle;

            protected bool Shown
            {
                set => _rowStyle.Height = value ? originalRowHeight : 0;
            }

            protected TItem _item;
            public TItem Item
            {
                get => _item;
                set
                {
                    if (value == _item)
                        return;
                    if (_item != null)
                        debindItem();
                    _item = value;
                    if (_item != null)
                    {
                        bindItem();
                        Shown = true;
                    }
                    else
                    {
                        Shown = false;
                    }
                }
            }

            protected abstract void createControls(int itemIndex);
            protected abstract void debindItem();
            protected abstract void bindItem();

            protected static T cloneOrOriginal<T>(T originalControl, int itemIndex) where T : Control
                => (itemIndex == 0) ? originalControl : originalControl.Clone();

            protected static void displayStatus(Label label, Status status, DateTime statusUpdateTime)
            {
                string labelText = statusStrings[status];
                if (status != Status.Empty)
                    labelText += string.Format(" ({0:HH\\:mm\\:ss})", statusUpdateTime);
                label.Text = labelText;
                label.ForeColor = statusColors[status];
            }

            private static Dictionary<Status, string> statusStrings = new Dictionary<Status, string>()
            {
                { Status.Empty, "-" },
                { Status.Unknown, "unknown" },
                { Status.Querying, "in progress" },
                { Status.Successful, "ready" },
                { Status.Unsuccessful, "error" }
            };

            private static Dictionary<Status, Color> statusColors = new Dictionary<Status, Color>()
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
