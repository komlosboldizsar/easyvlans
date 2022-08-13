using B.XmlDeserializer;

namespace easyvlans.Model.SwitchOperationMethods
{
    public interface ISwitchOperationMethodCollection : IMethod
    {

        IReadConfigMethod ReadConfigMethod { get; }
        ISetPortToVlanMethod SetPortToVlanMethod { get; }
        IPersistChangesMethod PersistChangesMethod { get; }

        public interface IDeserializer : IDeserializer<ISwitchOperationMethodCollection, Config> { }

    }
}
