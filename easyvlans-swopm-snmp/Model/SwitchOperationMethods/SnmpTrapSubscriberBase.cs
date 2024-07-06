using easyvlans.Helpers;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;

namespace easyvlans.Model.SwitchOperationMethods
{
    public abstract class SnmpTrapSubscriberBase : ITrapSubscriber
    {

        protected readonly ISnmpConnection _snmpConnection;

        public SnmpTrapSubscriberBase(ISnmpConnection snmpConnection)
            => _snmpConnection = snmpConnection;

        public abstract string MibName { get; }

        public abstract void TrapReceived(ISnmpMessage message, IEnumerable<Variable> variables);

    }
}
