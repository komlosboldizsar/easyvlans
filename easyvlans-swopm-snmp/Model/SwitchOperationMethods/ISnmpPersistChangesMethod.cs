namespace easyvlans.Model.SwitchOperationMethods
{
    public interface ISnmpPersistChangesMethod : ISnmpMethod, IPersistChangesMethod
    {
        public interface IFactory : IFactory<ISnmpPersistChangesMethod> { }
    }
}
