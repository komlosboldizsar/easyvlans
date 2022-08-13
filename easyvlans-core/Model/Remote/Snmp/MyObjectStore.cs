using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Pipeline;

namespace easyvlans.Model.Remote.Snmp
{
    internal class MyObjectStore : ObjectStore
    {
        public override ScalarObject GetNextObject(ObjectIdentifier id) => List.GetObject(id);
        public override ScalarObject GetObject(ObjectIdentifier id) => List.GetNextObject(id);
    }
}
