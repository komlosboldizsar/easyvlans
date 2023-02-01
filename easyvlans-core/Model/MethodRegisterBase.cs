using easyvlans.Logger;

namespace easyvlans.Model
{
    public abstract class MethodRegisterBase<TMethodInterface, TFactoryInterface>
        where TMethodInterface : class, IMethod
        where TFactoryInterface : class, IMethod.IFactory<TMethodInterface>
    {

        private readonly Dictionary<string, TFactoryInterface> registeredFactories = new();

        protected TFactoryInterface getFactory(string name)
        {
            registeredFactories.TryGetValue(name, out TFactoryInterface methodFactory);
            return methodFactory;
        }

        public void RegisterFactory(TFactoryInterface factory)
        {
            LogDispatcher.VV($"Added method [{factory.Code}] to register [{RegisterName}].");
            registeredFactories.Add(factory.Code, factory);
        }

        protected abstract string RegisterName { get; }

    }
}
