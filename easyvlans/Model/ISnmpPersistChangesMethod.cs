namespace easyvlans.Model
{
    internal interface ISnmpPersistChangesMethod : ISnmpMethod, IPersistChangesMethod
    {
        public interface IFactory : IFactory<ISnmpPersistChangesMethod> { }
    }
}
