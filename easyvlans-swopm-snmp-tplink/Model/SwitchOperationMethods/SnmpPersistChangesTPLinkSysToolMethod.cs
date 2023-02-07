using B.XmlDeserializer.Context;
using Lextm.SharpSnmpLib;
using System.Xml;

namespace easyvlans.Model.SwitchOperationMethods
{

    internal sealed class SnmpPersistChangesTPLinkSysToolMethod : ISnmpPersistChangesMethod
    {

        public const string CODE = "tplinksystool";

        public class Factory : ISnmpPersistChangesMethod.IFactory
        {
            public string Code => CODE;
            public ISnmpPersistChangesMethod GetInstance(XmlNode data, DeserializationContext deserializationContext, ISnmpSwitchOperationMethodCollection parent)
                => new SnmpPersistChangesTPLinkSysToolMethod(data, deserializationContext, parent);
        }

        private ISnmpSwitchOperationMethodCollection _parent;

        public SnmpPersistChangesTPLinkSysToolMethod(XmlNode data, DeserializationContext deserializationContext, ISnmpSwitchOperationMethodCollection parent)
            => _parent = parent;

        public string Code => CODE;
        public string DetailedCode => $"{_parent.Code}[{CODE}]";

        async Task IPersistChangesMethod.DoAsync()
            => await _parent.SnmpConnection.SetAsync(new List<Variable>() {
                new Variable(new ObjectIdentifier($"{OID_TSYSTOOLCONFIGSAVE}"), new Integer32(TXCONV_TSYSTOOLCONFIGSAVE_COMMIT))
            });

        private const string OID_TSYSTOOLCONFIGSAVE = "1.3.6.1.4.1.11863.6.3.1.3";
        private const int TXCONV_TSYSTOOLCONFIGSAVE_COMMIT = 1;
    }

}
