using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.GUI.Helpers.DropDowns
{

    public class ComboBoxAdapter<T> : IComboBoxAdapter
        where T : class
    {

        private IEnumerable<T> boundCollection;
        private List<ItemProxy> proxyList = new List<ItemProxy>();

        public delegate string ToStringFunctionDelegate(T item);
        ToStringFunctionDelegate toStringFunction;

        public bool ContainsNull { get; private set; }
        private string nullLabel;

        public bool ContainsListCollection => false;

        public ComboBoxAdapter(IEnumerable<T> elements, ToStringFunctionDelegate toStringFunction, bool containsNull = false, string nullLabel = "")
        {
            this.boundCollection = elements; 
            this.toStringFunction = toStringFunction ?? (o => o.ToString());
            this.ContainsNull = containsNull;
            this.nullLabel = nullLabel;
            createProxyList();
        }

        public IList GetList() => proxyList;

        private void createProxyList()
        {
            if (ContainsNull)
                proxyList.Add(new ItemProxy(null, nullLabel));
            if (boundCollection == null)
                return;
            foreach (T item in boundCollection)
                proxyList.Add(new ItemProxy(item, toStringFunction(item)));
        }

        public object Clone()
            => new ComboBoxAdapter<T>(boundCollection, toStringFunction, ContainsNull, nullLabel);

        private class ItemProxy
        {

            public T Value { get; private set; }
            public string Label { get; private set; }

            public ItemProxy(T value, string label)
            {
                Value = value;
                Label = label;
            }

            public override string ToString() => Label;

        }

    }

}
