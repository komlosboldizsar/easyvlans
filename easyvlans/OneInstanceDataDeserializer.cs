using BToolbox.XmlDeserializer;
using BToolbox.XmlDeserializer.Attributes;
using BToolbox.XmlDeserializer.Context;
using BToolbox.XmlDeserializer.Relations;
using easyvlans.Model;
using System.Xml;

namespace easyvlans
{
    internal class OneInstanceDataDeserializer : IDeserializer<OneInstanceData, Config>
    {

        public string ElementName => "one_instance";
        private const string ATTR_ID = "id";
        private const string ATTR_START_VISIBLE = "start_visible";

        public OneInstanceData Parse(XmlNode xmlNode, DeserializationContext context, out IRelationBuilder<Config> relationBuilder, object parent = null)
        {
            relationBuilder = null;
            return new()
            {
                ID = xmlNode.AttributeAsString(ATTR_ID, context).NotEmpty().Get().Value,
                StartVisible = xmlNode.AttributeAsBool(ATTR_START_VISIBLE, context).Default(true).Get().Value
            };
        }

    }
}