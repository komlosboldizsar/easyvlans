using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Pipeline;

namespace BToolbox.SNMP.Traps
{

    public class SimpleTrapGenerator
    {

        private SnmpAgent _snmpAgent;
        private string _code;
        private TrapEnterprise _enterprise;

        public SimpleTrapGenerator(SnmpAgent snmpAgent, string code, TrapEnterprise enterprise)
        {
            _snmpAgent = snmpAgent;
            _code = code;
            _enterprise = enterprise;
        }

        public SimpleTrapGenerator(SnmpAgent snmpAgent, string code, string enterpriseBase, int specificCode)
            : this(snmpAgent, code, new TrapEnterprise(enterpriseBase, specificCode))
        { }

        protected void SendTrap(List<ScalarObject> payloadObjects)
        {
            List<Variable> payloadVariables = new();
            payloadVariables = payloadObjects.Select(po => po.Variable).ToList();
            _snmpAgent.SendTraps(_code, _enterprise, payloadVariables);
        }

    }

}
