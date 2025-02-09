using BToolbox.XmlDeserializer;
using BToolbox.XmlDeserializer.Attributes;
using BToolbox.XmlDeserializer.Context;
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
            string trapFilterString = xmlNode.AttributeAsString(ATTR_TRAP_FILTER, context).Get().Value;
            string[] trapFilters = (trapFilterString == null) ? null : trapFilterString.Split(",");
            subscribeTraps(snmpConnection, trapFilters, commonData);
            return new MixedSwitchOperationMethodCollection()
            {
                ReadSwitchBoottimeMethod = (noMethodFilter || methodFilters.Contains(MethodCodes.METHOD__READ_SWITCH_BOOTTIME)) ? createReadSwitchBoottimeMethod(snmpConnection, commonData) : null,
                ReadInterfaceStatusMethod = (noMethodFilter || methodFilters.Contains(MethodCodes.METHOD__READ_INTERFACE_STATUS)) ? createReadInterfaceStatusMethod(snmpConnection, commonData) : null,
                ReadVlanMembershipMethod = (noMethodFilter || methodFilters.Contains(MethodCodes.METHOD__READ_VLAN_MEMBERSHIP)) ? createReadConfigMethod(snmpConnection, commonData) : null,
                SetPortToVlanMethod = (noMethodFilter || methodFilters.Contains(MethodCodes.METHOD__SET_PORT_TO_VLAN)) ? createSetPortToVlanMethod(snmpConnection, commonData) : null,
                PersistChangesMethod = (noMethodFilter || methodFilters.Contains(MethodCodes.METHOD__PERSIST_CHANGES)) ? createPersistChangesMethod(snmpConnection, commonData) : null
            };
        }

        private const string ATTR_METHOD_FILTER = "method_filter";
        private const string ATTR_TRAP_FILTER = "trap_filter";

        protected virtual object createCommonData(XmlNode xmlNode, DeserializationContext context)
            => null;

        protected virtual IReadSwitchBoottimeMethod createReadSwitchBoottimeMethod(ISnmpConnection snmpConnection, object commonData)
            => null;

        protected virtual IReadInterfaceStatusMethod createReadInterfaceStatusMethod(ISnmpConnection snmpConnection, object commonData)
            => null;

        protected virtual IReadVlanMembershipMethod createReadConfigMethod(ISnmpConnection snmpConnection, object commonData)
            => null;

        protected virtual ISetPortToVlanMethod createSetPortToVlanMethod(ISnmpConnection snmpConnection, object commonData)
            => null;

        protected virtual IPersistChangesMethod createPersistChangesMethod(ISnmpConnection snmpConnection, object commonData)
            => null;

        protected virtual void subscribeTraps(ISnmpConnection snmpConnection, string[] trapFilter, object commonData) { }

    }
}
