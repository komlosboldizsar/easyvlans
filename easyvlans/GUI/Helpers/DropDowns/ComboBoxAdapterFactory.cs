using System;
using System.Collections.Generic;

namespace easyvlans.GUI.Helpers.DropDowns
{

    public class ComboBoxAdapterFactory<T> : IComboBoxAdapterFactory
        where T : class
    {

        private IEnumerable<T> elements;
        private Func<T, string> toStringFunction;
        private bool containsNull;
        private string nullLabel;

        public ComboBoxAdapterFactory(IEnumerable<T> elements, Func<T, string> toStringFunction, bool containsNull = false, string nullLabel = "")
        {
            this.elements = elements;
            this.toStringFunction = toStringFunction;
            this.containsNull = containsNull;
            this.nullLabel = nullLabel;
        }

        public ComboBoxAdapter<T> GetOne() => new ComboBoxAdapter<T>(elements, toStringFunction, containsNull, nullLabel);
        IComboBoxAdapter IComboBoxAdapterFactory.GetOne() => GetOne();

    }

}
