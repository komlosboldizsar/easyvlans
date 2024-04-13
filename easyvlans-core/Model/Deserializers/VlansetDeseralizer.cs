using BToolbox.XmlDeserializer;
using BToolbox.XmlDeserializer.Attributes;
using BToolbox.XmlDeserializer.Context;
using BToolbox.XmlDeserializer.Exceptions;
using BToolbox.XmlDeserializer.Relations;
using System.Xml;

namespace easyvlans.Model.Deserializers
{
    internal class VlansetDeserializer : ElementDeserializer<Vlanset, Config>
    {

        public override string ElementName => ConfigTagNames.VLANSET;

        protected override Vlanset createElement(XmlNode elementNode, DeserializationContext context, object parent) => new()
        {
            ID = elementNode.AttributeAsString(ATTR_ID, context).Mandatory().NotEmpty().Get().Value
        };

        protected override ISlaveRelationBuilder<Vlanset, Config> createSlaveRelationBuilder() => new RelationBuilder();

        internal class RelationBuilder : ISlaveRelationBuilder<Vlanset, Config>
        {

            public void BuildRelations(XmlNode vlansetNode, Vlanset vlanset, Config config, DeserializationContext context)
            {
                RelationBuilderHelpers.HandleExceptions(findVlans, vlansetNode, vlanset, config, context);
            }

            private void findVlans(XmlNode vlansetNode, Vlanset vlanset, Config config, DeserializationContext context)
            {
                XmlAttributeData<string> filterStringData = vlansetNode.AttributeAsString(ATTR_VLANS, context).Mandatory().Get();
                Action<DeserializationException> invalidRelationHandler = (ex) => context.ReportInvalidRelation(ex, filterStringData.Attribute, vlanset);
                vlanset.AddRange(VlansetFilter.FilterVlans(filterStringData.Value, config.Vlans, null, invalidRelationHandler));
            }

        }

        private const string ATTR_ID = "id";
        private const string ATTR_VLANS = "vlans";

    }
}
