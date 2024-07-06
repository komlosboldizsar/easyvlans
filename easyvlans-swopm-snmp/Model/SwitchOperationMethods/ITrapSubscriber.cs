using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;

namespace easyvlans.Model.SwitchOperationMethods
{
    public interface ITrapSubscriber
    {
        void TrapReceived(ISnmpMessage message, IEnumerable<Variable> variables);
    }
}
