using B.XmlDeserializer.Context;
using Lextm.SharpSnmpLib;
using System.Xml;

namespace easyvlans.Model.SwitchOperationMethods
{

    internal sealed class SnmpPersistChangesHpBasicConfigMethod : ISnmpPersistChangesMethod
    {

        public const string CODE = "hpbasicconfig";

        public class Factory : ISnmpPersistChangesMethod.IFactory
        {
            public string Code => CODE;
            public ISnmpPersistChangesMethod GetInstance(XmlNode data, DeserializationContext deserializationContext, ISnmpSwitchOperationMethodCollection parent)
                => new SnmpPersistChangesHpBasicConfigMethod(data, deserializationContext, parent);
        }

        private ISnmpSwitchOperationMethodCollection _parent;

        public SnmpPersistChangesHpBasicConfigMethod(XmlNode data, DeserializationContext deserializationContext, ISnmpSwitchOperationMethodCollection parent)
            => _parent = parent;

        public string Code => CODE;
        public string DetailedCode => $"{_parent.Code}[{CODE}]";

        async Task IPersistChangesMethod.DoAsync()
            => await _parent.SnmpConnection.SetAsync(new List<Variable>() {
                new Variable(new ObjectIdentifier($"{OID_SAVECONFIG}"), new Integer32(TXCONV_SAVECONFIG_SAVECONFIG))
            });

        private const string OID_SAVECONFIG = "1.3.6.1.4.1.11.2.14.11.5.1.7.1.29.1.1";
        private const int TXCONV_SAVECONFIG_SAVECONFIG = 2;
    }

}
