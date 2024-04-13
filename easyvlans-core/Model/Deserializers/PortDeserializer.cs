using BToolbox.XmlDeserializer;
using BToolbox.XmlDeserializer.Attributes;
using BToolbox.XmlDeserializer.Context;
using BToolbox.XmlDeserializer.Exceptions;
using BToolbox.XmlDeserializer.Relations;
using System.Xml;

namespace easyvlans.Model.Deserializers
{
    internal class PortDeserializer : ElementDeserializer<Port, Config>
    {

        public override string ElementName => ConfigTagNames.PORT;

        protected override Port createElement(XmlNode elementNode, DeserializationContext context, object parent) => new()
        {
            Label = elementNode.AttributeAsString(ATTR_LABEL, context).Mandatory().NotEmpty().Get().Value,
            Index = (int)elementNode.AttributeAsInt(ATTR_INDEX, context).Mandatory().Min(1).Get().Value,
            RemoteIndex = elementNode.AttributeAsInt(ATTR_REMOTE_INDEX, context).Min(1).Get().Value,
            Page = parent as PortPage
        };

        protected override ISlaveRelationBuilder<Port, Config> createSlaveRelationBuilder() => new RelationBuilder();

        internal class RelationBuilder : ISlaveRelationBuilder<Port, Config>
        {

            public void BuildRelations(XmlNode portNode, Port port, Config config, DeserializationContext context)
            {
                RelationBuilderHelpers.HandleExceptions(findSwitch, portNode, port, config, context);
                RelationBuilderHelpers.HandleExceptions(findDefaultVlan, portNode, port, config, context);
                RelationBuilderHelpers.HandleExceptions(findVlans, portNode, port, config, context);
            }

            private void findSwitch(XmlNode portNode, Port port, Config config, DeserializationContext context)
            {
                XmlAttributeData<string> switchData = portNode.AttributeAsString(ATTR_SWITCH, context).Mandatory().NotEmpty().Get();
                if (!config.Switches.TryGetValue(switchData.Value, out Switch @switch))
                    RelatedObjectNotFoundException.Throw(switchData, typeof(Switch));
                port.Switch = @switch;
                @switch.AssociatePort(port);
            }

            private void findDefaultVlan(XmlNode portNode, Port port, Config config, DeserializationContext context)
            {
                XmlAttributeData<int?> defaultVlanData = portNode.AttributeAsInt(ATTR_DEFAULT_VLAN, context).Get();
                if (defaultVlanData.Value == null)
                    return;
                if (!config.Vlans.TryGetValue((int)defaultVlanData.Value, out Vlan defaultVlan))
                    RelatedObjectNotFoundException.Throw(defaultVlanData, typeof(Vlan));
                port.DefaultVlan = defaultVlan;
            }

            private void findVlans(XmlNode portNode, Port port, Config config, DeserializationContext context)
            {
                XmlAttributeData<string> filterStringData = portNode.AttributeAsString(ATTR_VLANS, context).Get();
                Action<DeserializationException> invalidRelationHandler = (ex) => context.ReportInvalidRelation(ex, filterStringData.Attribute, port);
                port.Vlans = VlansetFilter.FilterVlans(filterStringData.Value, config.Vlans, config.Vlansets, invalidRelationHandler);
            }

        }

        private const string ATTR_LABEL = "label";
        private const string ATTR_SWITCH = "switch";
        private const string ATTR_INDEX = "index";
        private const string ATTR_DEFAULT_VLAN = "default_vlan";
        private const string ATTR_VLANS = "vlans";
        private const string ATTR_REMOTE_INDEX = "remote_index";

    }


}
