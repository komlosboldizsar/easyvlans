using Lextm.SharpSnmpLib;

namespace easyvlans.Model.SwitchOperationMethods
{
    public interface ISnmpConnection
    {
        Task<List<Variable>> WalkAsync(string objectIdentifierStr);
        Task SetAsync(List<Variable> variables);
    }
}
