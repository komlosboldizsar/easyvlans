using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model
{
    public abstract class MethodRegisterBase<TMethodInterface, TFactoryInterface>
        where TMethodInterface : class, IMethod
        where TFactoryInterface : class, IMethod.IFactory<TMethodInterface>
    {

        protected MethodRegisterBase() => registerKnownFactories();

        private readonly Dictionary<string, TFactoryInterface> registeredFactories = new();

        protected TFactoryInterface getFactory(string name)
        {
            registeredFactories.TryGetValue(name, out TFactoryInterface methodFactory);
            return methodFactory;
        }

        public void RegisterFactory(TFactoryInterface factory) => registeredFactories.Add(factory.Code, factory);

        private void registerKnownFactories()
        {
            foreach (TFactoryInterface factory in KnownFactories)
                RegisterFactory(factory);
        }

        protected virtual TFactoryInterface[] KnownFactories { get; } = Array.Empty<TFactoryInterface>();

    }
}
