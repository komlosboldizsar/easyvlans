using Lextm.SharpSnmpLib;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace easyvlans.Model.SwitchOperationMethods
{

    internal sealed class SnmpPersistChangesDlinkDgs121024axMethod : ISnmpPersistChangesMethod
    {

        public const string CODE = "dlinkdgs121024ax";

        public class Factory : ISnmpPersistChangesMethod.IFactory
        {
            public string Code => CODE;
            public ISnmpPersistChangesMethod GetInstance(ISnmpSwitchOperationMethodCollection parent)
                => new SnmpPersistChangesCiscoCopyMethod(@parent);
        }

        private ISnmpSwitchOperationMethodCollection _parent;
        public SnmpPersistChangesDlinkDgs121024axMethod(ISnmpSwitchOperationMethodCollection parent) => _parent = parent;
        public string Code => CODE;
        public string DetailedCode => $"{_parent.Code}[{CODE}]";

        async Task IPersistChangesMethod.DoAsync()
            => await _parent.SnmpConnection.SetAsync(new List<Variable>() {
                new Variable(new ObjectIdentifier(OID_COMPANYSYSTEM_SYSSAVE), new Integer32(1))
            });

        private const string OID_COMPANYSYSTEM_SYSSAVE = "1.3.6.1.4.1.171.10.76.10.1.10.0";

    }

}
