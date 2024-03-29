﻿using B.XmlDeserializer.Context;
using easyvlans.Logger;
using System.Xml;

namespace easyvlans.Model.SwitchOperationMethods
{

    internal sealed partial class SnmpMibQBridge : ISnmpMib
    {

        public const string MIB_NAME = "qbridge";

        public class Deserializer : SnmpMibDeserializerBase
        {

            public override string ElementName => MIB_NAME;

            protected override object createCommonData(XmlNode xmlNode, DeserializationContext context)
                => new CommonData()
                {
                    NoPvid = (xmlNode.SelectNodes(DATA_TAG_NO_PVID).Count > 0)
                };

            public const string DATA_TAG_NO_PVID = "nopvid";

            protected override IReadConfigMethod createReadConfigMethod(ISnmpConnection snmpConnection, object commonData)
                => new ReadConfigMethod(snmpConnection, commonData);

            protected override ISetPortToVlanMethod createSetPortToVlanMethod(ISnmpConnection snmpConnection, object commonData)
                => new SetPortToVlanMethod(snmpConnection, commonData);

        }

        internal class MethodBase : SnmpMethodBase
        {

            public override string MibName => MIB_NAME;

            protected readonly CommonData _commonData;

            public MethodBase(ISnmpConnection snmpConnection, object commonData)
                : base(snmpConnection)
                => _commonData = (CommonData)commonData;

            protected static (int, int) getByteBitIndex(int portIndex) => ((portIndex - 1) / 8, 7 - (portIndex - 1) % 8);

        }

        internal class CommonData
        {
            public bool NoPvid { get; init; }
        }

        private const string OID_DOT1Q_VLAN_STATIC_TABLE = "1.3.6.1.2.1.17.7.1.4.3";
        private const string OID_DOT1Q_VLAN_STATIC_EGRESS_PORTS = $"{OID_DOT1Q_VLAN_STATIC_TABLE}.1.2";
        private const string OID_DOT1Q_VLAN_STATIC_UNTAGGED_PORTS = $"{OID_DOT1Q_VLAN_STATIC_TABLE}.1.4";
        private const string OID_DOT1Q_PORT_VLAN_TABLE = "1.3.6.1.2.1.17.7.1.4.5";
        private const string OID_DOT1Q_PVID = $"{OID_DOT1Q_PORT_VLAN_TABLE}.1.1";

    }

}
