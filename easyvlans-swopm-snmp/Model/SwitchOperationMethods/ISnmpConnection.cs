using BToolbox.SNMP;
using Lextm.SharpSnmpLib;

namespace easyvlans.Model.SwitchOperationMethods
{
    public interface ISnmpConnection
    {
        string Version { get; }
        Switch Switch { get; }
        Task<IList<Variable>> GetAsync(IEnumerable<string> objectIdentifierStrs);
        Task<List<Variable>> WalkAsync(string objectIdentifierStr);
        Task SetAsync(List<Variable> variables);
        void SubscribeForTrap(ITrapSubscriber subscriber, GenericCode v1GenericCode, int? v1SpecificCode, string v1EnterpriseFilter, ObjectIdentifier v2TrapOid);
        void SubscribeForTrap(ITrapSubscriber subscriber, TrapDescriptor descriptor);
    }
}
