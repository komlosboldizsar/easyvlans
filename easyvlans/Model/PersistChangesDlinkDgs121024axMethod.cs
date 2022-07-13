using Lextm.SharpSnmpLib;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace easyvlans.Model
{

    internal sealed class PersistChangesDlinkDgs121024axMethod : MethodBase, IPersistChangesMethod
    {

        public string Name => "dlinkdgs121024ax";

        public async Task Do()
            => await Switch.SnmpSetAsync(new List<Variable>() {
                new Variable(new ObjectIdentifier(OID_COMPANYSYSTEM_SYSSAVE), new Integer32(1))
            });

        private const string OID_COMPANYSYSTEM_SYSSAVE = "1.3.6.1.4.1.171.10.76.10.1.10.0";

    }

}
