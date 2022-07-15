using Lextm.SharpSnmpLib.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model
{
    internal static class ObjectStoreExtensions
    {
        public static void AddRange(this ObjectStore objectStore, IEnumerable<ISnmpObject> objects)
        {
            foreach (ISnmpObject @object in objects)
                objectStore.Add(@object);
        }
    }
}
