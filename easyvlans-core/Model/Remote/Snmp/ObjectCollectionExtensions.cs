using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model.Remote.Snmp
{
    internal static class ObjectCollectionExtensions
    {

        private static ScalarObject GetObjectByRule(IEnumerable<ISnmpObject> collection, Func<ISnmpObject, ScalarObject> rule)
            => collection.Select(rule)
                .Where(r => r != null)
                .OrderBy(r => r.Variable.Id)
                .FirstOrDefault();

        public static ScalarObject GetNextObject(this IEnumerable<ISnmpObject> collection, ObjectIdentifier id)
            => GetObjectByRule(collection, o => o.MatchGetNext(id));

        public static ScalarObject GetObject(this IEnumerable<ISnmpObject> collection, ObjectIdentifier id)
            => GetObjectByRule(collection, o => o.MatchGet(id));

    }
}
