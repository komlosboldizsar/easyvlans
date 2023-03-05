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

    }
}
