namespace easyvlans.Model.SwitchOperationMethods
{
    internal interface ISnmpPersistChangesMethod : ISnmpMethod, IPersistChangesMethod
    {
        public interface IFactory : IFactory<ISnmpPersistChangesMethod> { }
    }
}
