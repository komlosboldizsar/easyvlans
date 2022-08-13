using B.XmlDeserializer;
using B.XmlDeserializer.Attributes;
using B.XmlDeserializer.Context;
using System.Xml;

namespace easyvlans.Model.Deserializers
{
    internal class PortPageDeserializer : 
        SimpleCollectionDeserializerBase<PortPage, Port, Config>
    {

        public PortPageDeserializer() : base(ConfigTagNames.PORTPAGE, portDeserializer) { }
        private static readonly PortDeserializer portDeserializer = new();

        protected override PortPage createCollection(XmlNode collectionNode, DeserializationContext context, object parent)
            => new()
            {
                Title = collectionNode.AttributeAsString(ATTR_TITLE, context).Mandatory().Get().Value,
                IsDefault = collectionNode.AttributeAsBool(ATTR_DEFAULT, context).Get().Value
            };

        protected override void addElementToCollection(PortPage portPage, Port port)
            => portPage.Add(port);

        private const string ATTR_TITLE = "title";
        private const string ATTR_DEFAULT = "default";

    }
}
