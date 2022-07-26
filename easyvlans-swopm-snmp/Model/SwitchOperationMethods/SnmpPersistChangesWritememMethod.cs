using Lextm.SharpSnmpLib;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace easyvlans.Model.SwitchOperationMethods
{

    internal sealed class SnmpPersistChangesWritememMethod : ISnmpPersistChangesMethod
    {

        public const string CODE = "writemem";

        public class Factory : ISnmpPersistChangesMethod.IFactory
        {
            public string Code => CODE;
            public ISnmpPersistChangesMethod GetInstance(ISnmpSwitchOperationMethodCollection parent)
                => new SnmpPersistChangesWritememMethod(parent);
        }

        private ISnmpSwitchOperationMethodCollection _parent;
        public SnmpPersistChangesWritememMethod(ISnmpSwitchOperationMethodCollection parent) => _parent = parent;
        public string Code => CODE;
        public string DetailedCode => $"{_parent.Code}[{CODE}]";

        async Task IPersistChangesMethod.DoAsync()
            => await _parent.SnmpConnection.SetAsync(new List<Variable>() {
                new Variable(new ObjectIdentifier(OID_WRITEMEM), new Integer32(1))
            });

        private const string OID_WRITEMEM = "1.3.6.1.4.1.9.2.1.54";

    }

}
