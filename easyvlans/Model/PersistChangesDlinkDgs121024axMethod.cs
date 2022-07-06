using Lextm.SharpSnmpLib;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace easyvlans.Model
{

    internal class PersistChangesDlinkDgs121024axMethod : IPersistChangesMethod
    {

        public string Name => "dlinkdgs121024ax";

        public PersistChangesDlinkDgs121024axMethod() { }
        public PersistChangesDlinkDgs121024axMethod(Switch @switch) => _switch = @switch;
        public IPersistChangesMethod GetInstance(Switch @switch) => new PersistChangesDlinkDgs121024axMethod(@switch);
        private Switch _switch;

        public async Task Do()
        {
            await _switch.SnmpSetAsync(new List<Variable>() {
                new Variable(new ObjectIdentifier(OID_COMPANYSYSTEM_SYSSAVE), new Integer32(1))
            });
        }

        private const string OID_COMPANYSYSTEM_SYSSAVE = "1.3.6.1.4.1.171.10.76.10.1.10.0";

    }

}
