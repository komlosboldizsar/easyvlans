using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model
{
    internal abstract class DataTable<TKnownItem> : TableObject
    {

        protected readonly TKnownItem _item;
        public DataTable(TKnownItem item)
        {
            _item = item;
            _objects.AddRange(getObjects());
        }
        protected abstract ScalarObject[] getObjects();
        protected readonly List<ScalarObject> _objects = new();
        protected override IEnumerable<ScalarObject> Objects => _objects;

        protected abstract class Variable<TOidGenerator> : ScalarObject
            where TOidGenerator : IOidGenerator, new()
        {
            protected readonly TKnownItem _item;
            public Variable(TKnownItem item)
                : base(new ObjectIdentifier(_oidGenerator.Generate(item)))
            {
                _item = item;
            }
            private static TOidGenerator _oidGenerator = new TOidGenerator();
        }

        protected interface IOidGenerator
        {
            string Generate(TKnownItem item);
        }

        protected abstract class OidGeneratorBaseBase : IOidGenerator
        {
            protected abstract string TableID { get; }
            protected abstract int GetItemIndex(TKnownItem item);
            protected abstract int PropertyIndex { get; }
            public string Generate(TKnownItem item) => $"{TableID}.{GetItemIndex(item)}.{PropertyIndex}";
        }

    }
}
