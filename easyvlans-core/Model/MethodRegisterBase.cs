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

        public void RegisterFactory(TFactoryInterface factory) => registeredFactories.Add(factory.Code, factory);

    }
}
