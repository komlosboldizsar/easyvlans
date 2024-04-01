using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Pipeline;

namespace BToolbox.SNMP
{
    public abstract class MyTableObject : TableObject
    {
        public override ScalarObject MatchGet(ObjectIdentifier id) => Objects.GetObject(id);
        public override ScalarObject MatchGetNext(ObjectIdentifier id) => Objects.GetNextObject(id);
    }
}
