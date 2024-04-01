using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Pipeline;

namespace BToolbox.SNMP
{
    public class MyObjectStore : ObjectStore
    {
        public override ScalarObject GetObject(ObjectIdentifier id) => List.GetObject(id);
        public override ScalarObject GetNextObject(ObjectIdentifier id) => List.GetNextObject(id);
        public virtual void Remove(ISnmpObject objectToRemove) => List.Remove(objectToRemove);
    }
}
