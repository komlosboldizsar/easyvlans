using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Pipeline;

namespace BToolbox.SNMP
{

    public abstract class ObjectDataTable<TModel> : MyTableObject
    {

        public TModel Model { get; private set; }
        public SnmpAgent SnmpAgent { get; private set; }
        protected readonly List<ScalarObject> _objects = new();
        protected override IEnumerable<ScalarObject> Objects => _objects;
        private readonly List<TrapGenerator> _trapGenerators = new();

        internal Dictionary<IVariableFactory, UniversalScalarObject> VariableFactoryProducts { get; private set; } = new();
        internal UniversalScalarObject IndexerVariableFactoryProduct { get; private set; } = null;

        public void Init(TModel model, SnmpAgent snmpAgent)
        {
            Model = model;
            SnmpAgent = snmpAgent;
            foreach (IVariableFactory variableFactory in VariableFactories)
            {
                UniversalScalarObject variable = variableFactory.CreateVariable(this);
                _objects.Add(variable);
                if (variableFactory == IndexerVariableFactory)
                    IndexerVariableFactoryProduct = variable;
                VariableFactoryProducts.Add(variableFactory, variable);
            }
            foreach (ITrapGeneratorFactory trapGeneratorFactory in TrapGeneratorFactories)
            {
                TrapGenerator trapGenerator = trapGeneratorFactory.CreateTrapGenerator(this);
                trapGenerator.Init();
                trapGenerator.Subscribe();
                _trapGenerators.Add(trapGenerator);
            }
        }

        public void End()
        {
            _trapGenerators.ForEach(tg => tg.Unsubscribe());
        }

        protected abstract IVariableFactory[] VariableFactories { get; }
        protected abstract IVariableFactory IndexerVariableFactory { get; }
        protected abstract ITrapGeneratorFactory[] TrapGeneratorFactories { get; }
        protected abstract string TableOid { get; }
        protected virtual int EntryOidIndex { get; } = 1;
        protected abstract int ItemIndex { get; }

        public class UniversalScalarObject : ScalarObject
        {

            public readonly VariableDataProvider DataProvider;
            private ObjectIdentifier _oid;
            private ObjectIdentifier _oidWithoutIndexer;

            public UniversalScalarObject(string oid, VariableDataProvider dataProvider)
                : base(new ObjectIdentifier(oid))
            {
                DataProvider = dataProvider;
                _oid = new ObjectIdentifier(oid);
                _oidWithoutIndexer = _oid.Shorten();
            }

            public override ISnmpData Data
            {
                get => DataProvider.Get();
                set => DataProvider.Set(value);
            }

            public Variable VariableWithoutIndexer => new(_oidWithoutIndexer, Data);

        }

        public abstract class VariableDataProvider
        {
            public TModel Model { get; init; }
            public virtual ISnmpData Get() => throw new AccessFailureException();
            public virtual void Set(ISnmpData data) => throw new AccessFailureException();
        }

        public interface IVariableFactory
        {
            public UniversalScalarObject CreateVariable(ObjectDataTable<TModel> table);
        }

        public class VariableFactory<TDataProvider> : IVariableFactory
            where TDataProvider : VariableDataProvider, new()
        {

            int _propertyIndex;

            public VariableFactory(int propertyIndex) => _propertyIndex = propertyIndex;

            public UniversalScalarObject CreateVariable(ObjectDataTable<TModel> table)
            {
                VariableDataProvider dataProvider = new TDataProvider() { Model = table.Model };
                return new UniversalScalarObject($"{table.TableOid}.{table.EntryOidIndex}.{_propertyIndex}.{table.ItemIndex}", dataProvider);
            }

        }

        public abstract class TrapGenerator
        {

            public abstract string Code { get; }
            public abstract string EnterpriseBase { get; }
            public abstract int SpecificCode { get; }
            public abstract IEnumerable<IVariableFactory> PayloadVariableFactories { get; }
            public abstract void Subscribe();
            public abstract void Unsubscribe();
            internal virtual bool AutoAddIndexer { get; } = true;
            internal virtual bool AutoIndexerWithoutIndex { get; } = true;
            internal virtual bool VariablesWithoutIndex { get; } = true;

            public ObjectDataTable<TModel> Table { get; init; }
            public List<UniversalScalarObject> PayloadObjects { get; set; }
            private TrapEnterprise _enterprise;

            public void Init()
                => _enterprise = new(EnterpriseBase, SpecificCode);

            protected void SendTrap()
            {
                List<Variable> payloadVariables = new();
                bool withoutIndex = AutoAddIndexer ? AutoIndexerWithoutIndex : VariablesWithoutIndex;
                foreach (UniversalScalarObject payloadObject in PayloadObjects)
                {
                    payloadVariables.Add(withoutIndex ? payloadObject.VariableWithoutIndexer : payloadObject.Variable);
                    withoutIndex = VariablesWithoutIndex;
                }
                Table.SnmpAgent.SendTraps(Code, _enterprise, payloadVariables);
            }

        }

        public interface ITrapGeneratorFactory
        {
            public TrapGenerator CreateTrapGenerator(ObjectDataTable<TModel> table);
        }

        public class TrapGeneratorFactory<TTrapGenerator> : ITrapGeneratorFactory
            where TTrapGenerator : TrapGenerator, new()
        {
            public TrapGenerator CreateTrapGenerator(ObjectDataTable<TModel> table)
            {
                TrapGenerator trapGenerator = new TTrapGenerator()
                {
                    Table = table
                };
                List<UniversalScalarObject> payloadObjects = new();
                if (trapGenerator.AutoAddIndexer)
                    payloadObjects.Add(table.IndexerVariableFactoryProduct);
                foreach (IVariableFactory payloadVariableFactory in trapGenerator.PayloadVariableFactories)
                    if (table.VariableFactoryProducts.TryGetValue(payloadVariableFactory, out UniversalScalarObject payloadObject))
                        payloadObjects.Add(payloadObject);
                trapGenerator.PayloadObjects = payloadObjects;
                return trapGenerator;
            }
        }

    }
}
