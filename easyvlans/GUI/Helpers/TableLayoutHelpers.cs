using System;
using System.Windows.Forms;

namespace easyvlans.GUI.Helpers
{

    public static class TableLayoutHelpers
    {

        public abstract class Cloner<TItemStyle>
            where TItemStyle : TableLayoutStyle
        {

            protected TableLayoutPanel tableLayout;
            protected int sourceIndex;
            protected TItemStyle sourceStyle;

            public Cloner(TableLayoutPanel tableLayout, int sourceIndex)
            {
                this.tableLayout = tableLayout;
                this.sourceIndex = sourceIndex;
                if (getItemCount() <= sourceIndex)
                    throw new ArgumentException();
                sourceStyle = getSourceItemStyle();
            }

            public void DoCloning(int destinationIndex = DESTINATION_INDEX_LAST, string[] excludeProperties = null)
            {
                int oldItemCount = getItemCount();
                if (destinationIndex < 0)
                    destinationIndex = oldItemCount + destinationIndex + 1;
                setItemCount(oldItemCount + 1);
                cloneSourceItemStyleTo(destinationIndex);
                int newSourceIndex = sourceIndex;
                if (destinationIndex <= sourceIndex)
                    newSourceIndex++;
                int orthogonalIndex = 0;
                int orthogonalItemCount = getOrthogonalItemCount();
                while (orthogonalIndex < orthogonalItemCount)
                {
                    Control originalControl = getControl(newSourceIndex, orthogonalIndex);
                    if (originalControl != null)
                    {
                        int orthogonalSpan = getOrthogonalSpan(originalControl);
                        Control clonedControl = originalControl.CloneTypeOnly();
                        addControl(clonedControl, destinationIndex, orthogonalIndex);
                        setOrthogonalSpan(clonedControl, orthogonalSpan);
                        clonedControl.ClonePropertiesFrom(originalControl, excludeProperties);
                        orthogonalIndex += orthogonalSpan;
                    }
                    else
                    {
                        orthogonalIndex++;
                    }
                }
            }

            public const int DESTINATION_INDEX_LAST = -1;
            public static readonly string[] EXCLUDE_VISIBILITY = Cloning.EXCLUDE_VISIBILITY;

            protected abstract int getItemCount();
            protected abstract void setItemCount(int value);
            protected abstract int getOrthogonalItemCount();
            protected abstract TItemStyle getSourceItemStyle();
            protected abstract void cloneSourceItemStyleTo(int destinationIndex);
            protected abstract Control getControl(int itemIndex, int orthogonalIndex);
            protected abstract void addControl(Control control, int itemIndex, int orthogonalIndex);
            protected abstract int getOrthogonalSpan(Control control);
            protected abstract void setOrthogonalSpan(Control control, int value);

        }

        public class RowCloner : Cloner<RowStyle>
        {
            public RowCloner(TableLayoutPanel tableLayout, int sourceIndex) : base(tableLayout, sourceIndex) { }
            protected override int getItemCount() => tableLayout.RowCount;
            protected override void setItemCount(int value) => tableLayout.RowCount = value;
            protected override int getOrthogonalItemCount() => tableLayout.ColumnCount;
            protected override RowStyle getSourceItemStyle() => tableLayout.RowStyles[sourceIndex];
            protected override void cloneSourceItemStyleTo(int destinationIndex) => tableLayout.RowStyles.Insert(destinationIndex, new RowStyle(sourceStyle.SizeType, sourceStyle.Height));
            protected override Control getControl(int itemIndex, int orthogonalIndex) => tableLayout.GetControlFromPosition(orthogonalIndex, itemIndex);
            protected override void addControl(Control control, int itemIndex, int orthogonalIndex) => tableLayout.Controls.Add(control, orthogonalIndex, itemIndex);
            protected override int getOrthogonalSpan(Control control) => tableLayout.GetColumnSpan(control);
            protected override void setOrthogonalSpan(Control control, int value) => tableLayout.SetColumnSpan(control, value);
        }

        public class ColumnCloner : Cloner<ColumnStyle>
        {
            public ColumnCloner(TableLayoutPanel tableLayout, int sourceIndex) : base(tableLayout, sourceIndex) { }
            protected override int getItemCount() => tableLayout.ColumnCount;
            protected override void setItemCount(int value) => tableLayout.ColumnCount = value;
            protected override int getOrthogonalItemCount() => tableLayout.RowCount;
            protected override ColumnStyle getSourceItemStyle() => tableLayout.ColumnStyles[sourceIndex];
            protected override void cloneSourceItemStyleTo(int destinationIndex) => tableLayout.ColumnStyles.Insert(destinationIndex, new ColumnStyle(sourceStyle.SizeType, sourceStyle.Width));
            protected override Control getControl(int itemIndex, int orthogonalIndex) => tableLayout.GetControlFromPosition(itemIndex, orthogonalIndex);
            protected override void addControl(Control control, int itemIndex, int orthogonalIndex) => tableLayout.Controls.Add(control, itemIndex, orthogonalIndex);
            protected override int getOrthogonalSpan(Control control) => tableLayout.GetRowSpan(control);
            protected override void setOrthogonalSpan(Control control, int value) => tableLayout.SetRowSpan(control, value);
        }

        public const int ROW_INDEX_LAST = RowCloner.DESTINATION_INDEX_LAST;

        public static void CloneRow(this TableLayoutPanel tableLayout, int sourceIndex, int destinationIndex = ROW_INDEX_LAST, string[] excludeProperties = null)
            => (new RowCloner(tableLayout, sourceIndex)).DoCloning(destinationIndex, excludeProperties);

        public const int COLUMN_INDEX_LAST = ColumnCloner.DESTINATION_INDEX_LAST;

        public static void CloneColumn(this TableLayoutPanel tableLayout, int sourceIndex, int destinationIndex = COLUMN_INDEX_LAST, string[] excludeProperties = null)
            => (new ColumnCloner(tableLayout, sourceIndex)).DoCloning(destinationIndex, excludeProperties);

    }

}
