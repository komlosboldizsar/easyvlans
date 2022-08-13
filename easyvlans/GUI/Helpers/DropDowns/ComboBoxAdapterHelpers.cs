using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace easyvlans.GUI.Helpers.DropDowns
{
    public static class ComboBoxAdapterHelpers
    {

        public static void SelectByValue(this ComboBox comboBox, object value)
        {
            if ((value == null) && (comboBox.DataSource is IComboBoxAdapter adapter) && adapter.ContainsNull)
            {
                comboBox.SelectedIndex = 0;
                return;
            }
            comboBox.SelectedValue = value;
        }

        public static void CreateAdapterAsDataSource<T>(this ComboBox comboBox, IEnumerable<T> elements, Func<T, string> toStringFunction, bool containsNull = false, string nullLabel = "")
            where T : class
            => comboBox.SetAdapterAsDataSource(new ComboBoxAdapter<T>(elements, toStringFunction, containsNull, nullLabel));

        public static void GetAdapterFromFactoryAsDataSource(this ComboBox comboBox, IComboBoxAdapterFactory factory)
            => comboBox.SetAdapterAsDataSource(factory.GetOne());

        public static void SetAdapterAsDataSource(this ComboBox comboBox, IComboBoxAdapter adapter)
        {
            comboBox.DataSource = adapter;
            comboBox.ValueMember = nameof(ComboBoxAdapter<object>.ItemProxy.Value);
            comboBox.DisplayMember = nameof(ComboBoxAdapter<object>.ItemProxy.Label);
        }

    }
}
