using Lextm.SharpSnmpLib;

namespace easyvlans.Model.SwitchOperationMethods
{

    internal sealed class SnmpPersistChangesOldCiscoSysMethod : ISnmpPersistChangesMethod
    {

        public const string CODE = "oldciscosys";

        public class Factory : ISnmpPersistChangesMethod.IFactory
        {
            public string Code => CODE;
            public ISnmpPersistChangesMethod GetInstance(string @params, ISnmpSwitchOperationMethodCollection parent)
                => new SnmpPersistChangesOldCiscoSysMethod(@params, parent);
        }

        private ISnmpSwitchOperationMethodCollection _parent;

        public SnmpPersistChangesOldCiscoSysMethod(string @params, ISnmpSwitchOperationMethodCollection parent)
            => _parent = parent;

        public string Code => CODE;
        public string DetailedCode => $"{_parent.Code}[{CODE}]";

        async Task IPersistChangesMethod.DoAsync()
            => await _parent.SnmpConnection.SetAsync(new List<Variable>() {
                new Variable(new ObjectIdentifier(OID_WRITEMEM), new Integer32(TXCONV_WRITEMEM_WRITE))
            });

        private const string OID_WRITEMEM = "1.3.6.1.4.1.9.2.1.54";
        private const int TXCONV_WRITEMEM_WRITE = 1;

    }

}
