using B.XmlDeserializer;
using B.XmlDeserializer.Attributes;
using B.XmlDeserializer.Context;
using B.XmlDeserializer.Relations;
using easyvlans.Model.SwitchOperationMethods;
using System.Xml;

namespace easyvlans.Model.Deserializers
{

    internal class SwitchDeserializer : ElementDeserializer<Switch, Config>
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
            List<ISwitchOperationMethodCollection> operationMethods = SwitchOperationMethodsDeserializer.Instance.ParseWithGivenParent(elementNode, context, out IRelationBuilder<Config> _, @switch);
            if (operationMethods.Count > 1)
                context.Report(DeserializationReportSeverity.Info, elementNode, "Multiple operation methods found for switch, using the first one.");
            @switch.OperationMethodCollection = operationMethods.FirstOrDefault();
            if (@switch.OperationMethodCollection == null)
                context.Report(DeserializationReportSeverity.Warning, elementNode, "No operation methods found for switch.");
            return @switch;
        }

        private const string ATTR_ID = "id";
        private const string ATTR_LABEL = "label";
        private const string ATTR_REMOTE_INDEX = "remote_index";

    }

}
