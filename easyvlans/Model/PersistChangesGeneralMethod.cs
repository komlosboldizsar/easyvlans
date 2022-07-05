using Lextm.SharpSnmpLib;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace easyvlans.Model
{

    internal class PersistChangesWritememMethod : IPersistChangesMethod
    {

        public string Name => "writemem";

        public async Task Do(Switch @switch)
        {
            await @switch.SnmpSetAsync(new List<Variable>() {
                new Variable(new ObjectIdentifier(OID_WRITEMEM), new Integer32(1))
            });
        }

        private const string OID_WRITEMEM = "1.3.6.1.4.1.9.2.1.54";

    }

}
