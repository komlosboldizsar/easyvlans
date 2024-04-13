using BToolbox.XmlDeserializer;
using BToolbox.XmlDeserializer.Attributes;
using BToolbox.XmlDeserializer.Context;
using System.Xml;

namespace easyvlans.Model.Deserializers
{

    internal class VlanDeserializer : ElementDeserializer<Vlan, Config>
    {

        public override string ElementName => ConfigTagNames.VLAN;

        protected override Vlan createElement(XmlNode elementNode, DeserializationContext context, object parent) => new()
        {
            ID = (int)elementNode.AttributeAsInt(ATTR_ID, context).Mandatory().Min(1).Max(4095).Get().Value,
            Name = elementNode.AttributeAsString(ATTR_NAME, context).Mandatory().NotEmpty().Get().Value
        };

        private const string ATTR_ID = "id";
        private const string ATTR_NAME = "name";

    }

}
