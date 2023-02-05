using Lextm.SharpSnmpLib;

namespace easyvlans.Model.SwitchOperationMethods
{
    public static class ISnmpConnectionExtensions
    {

        public static Task SetAsync(this ISnmpConnection connection, Variable variable)
            => connection.SetAsync(new List<Variable>() { variable });

        public static Task SetAsync(this ISnmpConnection connection, ObjectIdentifier oid, ISnmpData data)
            => connection.SetAsync(new List<Variable>() { new Variable(oid, data) });

        public static Task SetAsync(this ISnmpConnection connection, string oidString, ISnmpData data)
            => connection.SetAsync(new List<Variable>() { new Variable(new ObjectIdentifier(oidString), data) });

    }
}
