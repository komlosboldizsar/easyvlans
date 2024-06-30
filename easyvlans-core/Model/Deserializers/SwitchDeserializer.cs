using BToolbox.XmlDeserializer;
using BToolbox.XmlDeserializer.Attributes;
using BToolbox.XmlDeserializer.Context;
using BToolbox.XmlDeserializer.Exceptions;
using BToolbox.XmlDeserializer.Relations;
using easyvlans.Logger;
using easyvlans.Model.SwitchOperationMethods;
using System.Xml;

namespace easyvlans.Model.Deserializers
{

    public class SwitchDeserializer : ElementDeserializer<Switch, Config>
    {

        public override string ElementName => ConfigTagNames.SWITCH;

        protected override Switch createElement(XmlNode elementNode, DeserializationContext context, object parent)
        {
            Switch @switch = new()
            {
                ID = elementNode.AttributeAsString(ATTR_ID, context).Mandatory().NotEmpty().Get().Value,
                Label = elementNode.AttributeAsString(ATTR_LABEL, context).Mandatory().NotEmpty().Get().Value,
                RemoteIndex = elementNode.AttributeAsInt(ATTR_REMOTE_INDEX, context).Min(1).Get().Value
            };
            List<ISwitchOperationMethodCollection> operationMethods = operationMethodsDeserializer.ParseWithGivenParent(elementNode, context, out IRelationBuilder<Config> _, @switch);
            @switch.OperationMethodCollection = MixedSwitchOperationMethodCollection.Create(operationMethods, out MixedSwitchOperationMethodCollection.MethodCounts methodCounts);
            reportMethodCount(elementNode, context, methodCounts.ReadInterfaceStatusMethodCount, "read interface status");
            reportMethodCount(elementNode, context, methodCounts.ReadVlanMembershipMethodCount, "read VLAN membership");
            reportMethodCount(elementNode, context, methodCounts.SetPortToVlanMethodCount, "set VLAN membership");
            reportMethodCount(elementNode, context, methodCounts.PersistChangesMethodCount, "persist changes", true);
            return @switch;
        }

        private static void reportMethodCount(XmlNode elementNode, DeserializationContext context, int methodCount, string methodPurpose, bool suppressWarning = false)
        {
            if ((methodCount == 0) && !suppressWarning)
                context.Report(DeserializationReportSeverity.Warning, elementNode, $"No method to {methodPurpose} found for switch.");
            if (methodCount > 1)
                context.Report(DeserializationReportSeverity.Info, elementNode, $"Multiple methods to {methodPurpose} found for switch, using the first one.");
        }

        protected override ISlaveRelationBuilder<Switch, Config> createSlaveRelationBuilder() => new RelationBuilder();

        internal class RelationBuilder : ISlaveRelationBuilder<Switch, Config>
        {

            public void BuildRelations(XmlNode switchNode, Switch @switch, Config config, DeserializationContext context)
            {
                RelationBuilderHelpers.HandleExceptions(assignConfig, switchNode, @switch, config, context);
                RelationBuilderHelpers.HandleExceptions(findVlans, switchNode, @switch, config, context);
            }

            private void assignConfig(XmlNode switchNode, Switch @switch, Config config, DeserializationContext context)
            {
                @switch.Config = config;
            }

            private void findVlans(XmlNode portNode, Switch @switch, Config config, DeserializationContext context)
            {
                XmlAttributeData<string> filterStringData = portNode.AttributeAsString(ATTR_VLANS, context).Get();
                Action<DeserializationException> invalidRelationHandler = (ex) => context.ReportInvalidRelation(ex, filterStringData.Attribute, @switch);
                @switch.Vlans = VlansetFilter.FilterVlans(filterStringData.Value, config.Vlans, config.Vlansets, invalidRelationHandler);
            }

        }

        private const string ATTR_ID = "id";
        private const string ATTR_LABEL = "label";
        private const string ATTR_REMOTE_INDEX = "remote_index";
        private const string ATTR_VLANS = "vlans";

        private static readonly HeterogenousListDeserializer<ISwitchOperationMethodCollection, Config> operationMethodsDeserializer = new(ConfigTagNames.SWITCH);

        public static void RegisterOperationMethodsDeserializer(IDeserializer<ISwitchOperationMethodCollection, Config> deserializer)
        {
            operationMethodsDeserializer.Register(deserializer);
            LogDispatcher.VV($"Registered XML deserializer for switch operation methods: [{deserializer.ElementName}]");
        }

    }

}
