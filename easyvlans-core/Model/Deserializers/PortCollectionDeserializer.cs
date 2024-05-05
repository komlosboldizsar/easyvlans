using BToolbox.XmlDeserializer;
using BToolbox.XmlDeserializer.Attributes;
using BToolbox.XmlDeserializer.Context;
using System.Xml;

namespace easyvlans.Model.Deserializers
{
    internal class PortCollectionDeserializer :
        HeterogenousCollectionDeserializerBase<PortCollection, IPortOrPortCollection, Config>
    {

        private bool _root;

        public PortCollectionDeserializer(bool root = false) : base(root ? ConfigTagNames.PORTS : ConfigTagNames.PORTPAGE)
        {
            _root = root;
            Register(portsDeserializer);
            Register(portCollectionDeserializer ?? this);
        }

        private static readonly PortDeserializer portsDeserializer = new();
        private static readonly PortCollectionDeserializer portCollectionDeserializer = new();

        protected override PortCollection createCollection(XmlNode collectionNode, DeserializationContext context, object parent)
        {
            if (_root)
                return new();
            PortCollection parentCollection = parent as PortCollection;
            return new()
            {
                Title = collectionNode.AttributeAsString(ATTR_TITLE, context).Mandatory().Get().Value,
                IsDefault = collectionNode.AttributeAsBool(ATTR_DEFAULT, context).Get().Value,
                Parent = parentCollection,
                Level = parentCollection?.Level + 1 ?? 0,
                RememberLastSelectedSubCollection = collectionNode.AttributeAsBool(ATTR_REMEMBER_LAST_SUBPAGE, context).Default(true).Get().Value,
            };
        }

        protected override void addElementToCollection(PortCollection portCollection, IPortOrPortCollection portOrPortCollection)
            => portCollection.Add(portOrPortCollection);

        private const string ATTR_TITLE = "title";
        private const string ATTR_DEFAULT = "default";
        private const string ATTR_REMEMBER_LAST_SUBPAGE = "remember_last_subpage";

    }
}
