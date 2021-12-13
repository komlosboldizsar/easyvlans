using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.GUI.Helpers.DropDowns
{

    public class ComboBoxAdapterFactory<T> : IComboBoxAdapterFactory
        where T : class
    {

        private IEnumerable<T> elements;
        private ComboBoxAdapter<T>.ToStringFunctionDelegate toStringFunction;
        private bool containsNull;
        private string nullLabel;

        public ComboBoxAdapterFactory(IEnumerable<T> elements, ComboBoxAdapter<T>.ToStringFunctionDelegate toStringFunction, bool containsNull = false, string nullLabel = "")
        {
            this.elements = elements;
            this.toStringFunction = toStringFunction;
            this.containsNull = containsNull;
            this.nullLabel = nullLabel;
        }

        public ComboBoxAdapter<T> GetOneT()
            => new ComboBoxAdapter<T>(elements, toStringFunction, containsNull, nullLabel);

        public IComboBoxAdapter GetOne()
            => GetOneT();

    }

}
