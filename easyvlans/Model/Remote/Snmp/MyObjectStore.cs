using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model.Remote.Snmp
{
    internal class MyObjectStore : ObjectStore
    {
        public override ScalarObject GetNextObject(ObjectIdentifier id) => List.GetObject(id);
        public override ScalarObject GetObject(ObjectIdentifier id) => List.GetNextObject(id);
    }
}
