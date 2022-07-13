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

        private readonly IEnumerable<T> boundCollection;
        private readonly List<ItemProxy> proxyList = new();

        private readonly Func<T, string> toStringFunction;

        public bool ContainsNull { get; private init; }
        private readonly string nullLabel;

        public bool ContainsListCollection => false;

        public ComboBoxAdapter(IEnumerable<T> elements, Func<T, string> toStringFunction, bool containsNull = false, string nullLabel = "")
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

        public object Clone() => new ComboBoxAdapter<T>(boundCollection, toStringFunction, ContainsNull, nullLabel);

        internal record ItemProxy(T Value, string Label)
        {
            public override string ToString() => Label;
        }

    }

}
