using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model.Remote.Snmp
{
    internal abstract class MyTableObject : TableObject
    {
        public override ScalarObject MatchGetNext(ObjectIdentifier id) => Objects.GetObject(id);
        public override ScalarObject MatchGet(ObjectIdentifier id) => Objects.GetNextObject(id);
    }
}
