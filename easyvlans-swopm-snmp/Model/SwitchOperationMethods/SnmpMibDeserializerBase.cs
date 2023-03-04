using B.XmlDeserializer;
using B.XmlDeserializer.Attributes;
using B.XmlDeserializer.Context;
using B.XmlDeserializer.Relations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace easyvlans.Model.SwitchOperationMethods
{
    public abstract class SnmpMibDeserializerBase : ElementDeserializer<ISwitchOperationMethodCollection, Config>
    {

        protected override ISwitchOperationMethodCollection createElement(XmlNode xmlNode, DeserializationContext context, object parent)
        {
            if (parent is not ISnmpConnection snmpConnection)
                return null;
            string methodFilterString = xmlNode.AttributeAsString(ATTR_METHOD_FILTER, context).Get().Value;
            bool noMethodFilter = (methodFilterString == null);
            string[] methodFilters = noMethodFilter ? null : methodFilterString.Split(",");
            object commonData = createCommonData(xmlNode, context);
            return new MixedSwitchOperationMethodCollection()
            {
                ReadConfigMethod = (noMethodFilter || methodFilters.Contains(METHOD_FILTER__READ_CONFIG)) ? createReadConfigMethod(snmpConnection, commonData) : null,
                SetPortToVlanMethod = (noMethodFilter || methodFilters.Contains(METHOD_FILTER__SET_PORT_TO_VLAN)) ? createSetPortToVlanMethod(snmpConnection, commonData) : null,
                PersistChangesMethod = (noMethodFilter || methodFilters.Contains(METHOD_FILTER__PERSIST_CHANGES)) ? createPersistChangesMethod(snmpConnection, commonData) : null
            };
        }

        private const string ATTR_METHOD_FILTER = "method_filter";
        private const string METHOD_FILTER__READ_CONFIG = "read_config";
        private const string METHOD_FILTER__SET_PORT_TO_VLAN = "set_port_to_vlan";
        private const string METHOD_FILTER__PERSIST_CHANGES = "persist_changes";

        protected virtual object createCommonData(XmlNode xmlNode, DeserializationContext context)
            => null;

        protected virtual IReadConfigMethod createReadConfigMethod(ISnmpConnection snmpConnection, object commonData)
            => null;

        protected virtual ISetPortToVlanMethod createSetPortToVlanMethod(ISnmpConnection snmpConnection, object commonData)
            => null;

        protected virtual IPersistChangesMethod createPersistChangesMethod(ISnmpConnection snmpConnection, object commonData)
            => null;

    }
}
