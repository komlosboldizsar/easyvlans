using Lextm.SharpSnmpLib.Pipeline;

namespace easyvlans.Model.Remote.Snmp
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
