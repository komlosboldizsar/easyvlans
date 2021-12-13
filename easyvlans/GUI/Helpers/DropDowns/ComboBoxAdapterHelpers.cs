using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace easyvlans.GUI.Helpers.DropDowns
{

    public static class ComboBoxAdapterHelpers
    {

        public static void SelectByValue(this ComboBox comboBox, object value)
        {
            IComboBoxAdapter adapter = comboBox.DataSource as IComboBoxAdapter;
            if((value == null) && (adapter != null) && adapter.ContainsNull)
            {
                comboBox.SelectedIndex = 0;
                return;
            }
            comboBox.SelectedValue = value;
        }

        public static void CreateAdapterAsDataSource<T>(this ComboBox comboBox, IEnumerable<T> elements, ComboBoxAdapter<T>.ToStringFunctionDelegate toStringFunction, bool containsNull = false, string nullLabel = "")
            where T : class
        {
            ComboBoxAdapter<T> adapter = new ComboBoxAdapter<T>(elements, toStringFunction, containsNull, nullLabel);
            comboBox.SetAdapterAsDataSource(adapter);
        }

        public static void GetAdapterFromFactoryAsDataSource(this ComboBox comboBox, IComboBoxAdapterFactory factory)
            => comboBox.SetAdapterAsDataSource(factory.GetOne());

        public static void SetAdapterAsDataSource(this ComboBox comboBox, IComboBoxAdapter adapter)
        {
            comboBox.DataSource = adapter;
            comboBox.ValueMember = "Value";
            comboBox.DisplayMember = "Label";
        }

    }

}
