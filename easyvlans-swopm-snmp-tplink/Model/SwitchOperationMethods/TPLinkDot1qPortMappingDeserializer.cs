using B.XmlDeserializer;
using B.XmlDeserializer.Attributes;
using B.XmlDeserializer.Context;
using System.Xml;

namespace easyvlans.Model.SwitchOperationMethods
{
    internal class TPLinkDot1qPortMappingDeserializer : ElementDeserializer<PortMapping, SnmpAccessVlanMembershipTPLinkDot1qVlanMethod.Parameters>
    {

        public override string ElementName => TAG_PORT_MAPPING;

        protected override PortMapping createElement(XmlNode elementNode, DeserializationContext context, object parent)
            => new(
                    /* LocalIndex */  (int)elementNode.AttributeAsInt(ATTR_LOCAL_INDEX, context).Mandatory().Min(1).Get().Value,
                    /* SnmpIndex */   (int)elementNode.AttributeAsInt(ATTR_SNMP_INDEX, context).Mandatory().Min(1).Get().Value,
                    /* SimpleId */    (int)elementNode.AttributeAsInt(ATTR_SIMPLE_ID, context).Mandatory().Min(1).Get().Value,
                    /* ThreePartId */ elementNode.AttributeAs(ATTR_THREE_PART_ID, context, TPLinkDot1qThreePartPortId.XmlAttributeConverter.Instance).Mandatory().Get().Value,
                    /* Count */       (int)elementNode.AttributeAsInt(ATTR_COUNT, context).Mandatory().Get().Value
                );

        public const string TAG_PORT_MAPPINGS = "port_mappings";
        public const string TAG_PORT_MAPPING = "port_mapping";
        public const string ATTR_LOCAL_INDEX = "local_index";
        public const string ATTR_SNMP_INDEX = "snmp_index";
        public const string ATTR_SIMPLE_ID = "simple_id";
        public const string ATTR_THREE_PART_ID = "three_part_id";
        public const string ATTR_COUNT = "count";

    }
}
