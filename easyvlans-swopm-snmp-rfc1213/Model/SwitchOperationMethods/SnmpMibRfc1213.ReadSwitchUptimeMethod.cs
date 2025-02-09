using easyvlans.Helpers;
using Lextm.SharpSnmpLib;

namespace easyvlans.Model.SwitchOperationMethods
{
    internal sealed partial class SnmpMibRfc1213
    {
        internal class ReadSwitchBoottimeMethod : SnmpMethodBase, IReadSwitchBoottimeMethod
        {

            public ReadSwitchBoottimeMethod(ISnmpConnection snmpConnection) : base(snmpConnection) { }

            public override string MibName => MIB_NAME;

            public async Task DoAsync()
            {
                IEnumerable<Variable> response = await _snmpConnection.GetAsync(new string[] { OID_SYS_UP_TIME });
                Variable uptimeVariable = response.FirstOrDefault(v => v.Id.ToString() == OID_SYS_UP_TIME);
                if (uptimeVariable == null)
                    throw new Exception("'sysUpTime' object was not in the response.");
                TimeTicks uptimeTicks = default;
                if (!uptimeVariable.ToTimeTicks(t => uptimeTicks = t))
                    throw new Exception("'sysUpTime' object contained wrong type of value.");
                _snmpConnection.Switch.Boottime = DateTime.Now - uptimeTicks.ToTimeSpan();
            }

        }
    }
}
