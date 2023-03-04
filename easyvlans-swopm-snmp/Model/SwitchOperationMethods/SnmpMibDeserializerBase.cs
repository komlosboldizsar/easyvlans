using B.XmlDeserializer;
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
            object commonData = createCommonData(xmlNode, context);
            return new MixedSwitchOperationMethodCollection()
            {
                ReadConfigMethod = createReadConfigMethod(snmpConnection, commonData),
                SetPortToVlanMethod = createSetPortToVlanMethod(snmpConnection, commonData),
                PersistChangesMethod = createPersistChangesMethod(snmpConnection, commonData)
            };
        }

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
