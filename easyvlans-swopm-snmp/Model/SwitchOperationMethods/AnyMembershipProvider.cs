using Lextm.SharpSnmpLib.Pipeline;

namespace easyvlans.Model.SwitchOperationMethods
{
    public sealed class AnyMembershipProvider : IMembershipProvider
    {
        public bool AuthenticateRequest(ISnmpContext context) => true;
    }
}
