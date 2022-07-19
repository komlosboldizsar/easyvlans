using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model.Remote.Snmp
{

    internal abstract class DataTable<TKnownItem> : TableObject
    {

        public readonly TKnownItem Item;
        protected readonly List<ScalarObject> _objects = new();
        protected override IEnumerable<ScalarObject> Objects => _objects;

        public DataTable(TKnownItem item)
        {
            Item = item;
            _objects.AddRange(VariableFactories.Select(vf => vf.CreateVariable(this)));
        }

        protected abstract IVariableFactory[] VariableFactories { get; }
        protected abstract string TableOid { get; }
        protected virtual int EntryOidIndex { get; } = 1;
        protected virtual int ItemIndex
        {
            get
            {
                if (Item is not IRemoteable remoteable)
                    throw new NotImplementedException($"{nameof(DataTable<object>)}.{nameof(ItemIndex)}.get() default implementation is only usable for subclasses/implementations of {nameof(IRemoteable)} interface.");
                return (int)remoteable.RemoteIndex;
            }
        }

        protected class UniversalVariable : ScalarObject
        {

            protected readonly VariableDataProvider _dataProvider;

            public UniversalVariable(string oid, VariableDataProvider dataProvider)
                : base(new ObjectIdentifier(oid))
                => _dataProvider = dataProvider;

            public override ISnmpData Data
            {
                get => _dataProvider.Get();
                set => _dataProvider.Set(value);
            }

        }

        protected abstract class VariableDataProvider
        {
            public TKnownItem Item { get; init; }
            public virtual ISnmpData Get() => throw new AccessFailureException();
            public virtual void Set(ISnmpData data) => throw new AccessFailureException();
        }

        protected interface IVariableFactory
        {
            public ScalarObject CreateVariable(DataTable<TKnownItem> table);
        }

        protected class VariableFactory<TDataProvider> : IVariableFactory
            where TDataProvider : VariableDataProvider, new()
        {

            int _propertyIndex;

            public VariableFactory(int propertyIndex) => _propertyIndex = propertyIndex;

            public ScalarObject CreateVariable(DataTable<TKnownItem> table)
            {
                VariableDataProvider dataProvider = new TDataProvider() { Item = table.Item };
                return new UniversalVariable($"{table.TableOid}.{table.EntryOidIndex}.{_propertyIndex}.{table.ItemIndex}", dataProvider);
            }

        }

    }
}
