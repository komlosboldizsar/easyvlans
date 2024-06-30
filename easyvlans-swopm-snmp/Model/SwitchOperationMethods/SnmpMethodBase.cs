using easyvlans.Helpers;
using Lextm.SharpSnmpLib;

namespace easyvlans.Model.SwitchOperationMethods
{
    public abstract class SnmpMethodBase : ISwitchOperationMethod
    {

        protected readonly ISnmpConnection _snmpConnection;

        public SnmpMethodBase(ISnmpConnection snmpConnection)
            => _snmpConnection = snmpConnection;

        public abstract string MibName { get; }

        public string DetailedCode
        {
            get
            {
                string codeParameters = CodeParameters;
                if (codeParameters != null)
                    codeParameters = ":" + codeParameters;
                return $"snmp[{_snmpConnection.Version}][{MibName}{codeParameters}]";
            }
        }

        protected virtual string CodeParameters => null;

        protected async Task WalkAndProcess<T>(string objectIdentifierStr, Dictionary<int, T> collection, Func<int, T> collectionCreateNew, Action<T, Variable> processAction)
        {
            foreach (Variable row in await _snmpConnection.WalkAsync(objectIdentifierStr))
            {
                SnmpVariableHelpers.IdParts idParts = row.GetIdParts();
                T element = collection.GetAnyway(idParts.RowId, collectionCreateNew);
                if (idParts.NodeId == objectIdentifierStr)
                    processAction(element, row);
            }
        }

    }
}
