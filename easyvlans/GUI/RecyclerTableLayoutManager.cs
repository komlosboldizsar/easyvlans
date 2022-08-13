using easyvlans.GUI.Helpers;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace easyvlans.GUI
{

    internal partial class RecyclerTableLayoutManager<TItem, TRowManager>
        where TItem : class
        where TRowManager : RecyclerTableLayoutRowManager<TItem>, new()
    {

        private readonly Form containerForm;
        private readonly TableLayoutPanel table;
        private readonly int headerRows;
        private readonly List<TRowManager> rowManagers = new();

        public RecyclerTableLayoutManager(MainForm containerForm, TableLayoutPanel table, int headerRows)
        {
            this.containerForm = containerForm;
            this.table = table;
            this.headerRows = headerRows;
        }

        public void CreateRow(int dataRowIndex)
        {
            Control[] rowControls = createRowControls(dataRowIndex);
            increaseContainerFormHeight(dataRowIndex);
            TRowManager rowManager = new();
            rowManager.Init(table);
            rowManager.SetMembers(rowControls);
            rowManager.SubscribeControlEvents();
            rowManagers.Add(rowManager);
        }

        private Control[] createRowControls(int dataRowIndex)
        {
            if (dataRowIndex > 0)
                table.CloneRow(headerRows);
            Control[] controls = new Control[table.ColumnCount];
            for (int i = 0; i < table.ColumnCount; i++)
                controls[i] = table.GetControlFromPosition(i, headerRows + dataRowIndex);
            return controls;
        }

        private void increaseContainerFormHeight(int dataRowIndex)
        {
            float rowHeight = table.RowStyles[headerRows + dataRowIndex].Height;
            containerForm.Size = new Size(containerForm.Size.Width, containerForm.Size.Height + (int)rowHeight);
        }

        public void CreateAllRows(int count)
        {
            table.SuspendLayout();
            for (int i = 0; i < count; i++)
                CreateRow(i);
            table.ResumeLayout(true);
            rowManagers.ForEach(r => r.Shown = false);
        }

        public void BindItems(IEnumerable<TItem> items)
        {
            int itemCount = items.Count();
            int rowCount = rowManagers.Count;
            int maxCount = (itemCount > rowCount) ? itemCount : rowCount;
            IEnumerator<TItem> itemsEnumerator = items.GetEnumerator();
            bool itemsEnumeratorValid = itemsEnumerator.MoveNext();
            table.SuspendLayout();
            for (int i = 0; i < maxCount; i++)
            {
                if (i >= rowCount)
                    CreateRow(i);
                rowManagers[i].Item = itemsEnumeratorValid ? itemsEnumerator.Current : null;
                itemsEnumeratorValid = itemsEnumerator.MoveNext();
            }
            table.ResumeLayout(true);
        }

    }

}
