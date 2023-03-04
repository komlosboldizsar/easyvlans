using Lextm.SharpSnmpLib;

namespace easyvlans.Model.SwitchOperationMethods
{
    public interface ISnmpConnection
    {
        string Version { get; }
        Switch Switch { get; }
        Task<List<Variable>> WalkAsync(string objectIdentifierStr);
        Task SetAsync(List<Variable> variables);
    }
}
