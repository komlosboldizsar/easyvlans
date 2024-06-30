using BToolbox.XmlDeserializer.Context;
using BToolbox.XmlDeserializer.Exceptions;
using System.Xml;

namespace easyvlans.Model.SwitchOperationMethods
{
    internal sealed partial class SnmpMibDlinkDgs1210 : ISnmpMib
    {

        public const string MIB_NAME = "dlinkdgs1210";

        public class Deserializer : SnmpMibDeserializerBase
        {

            public override string ElementName => MIB_NAME;

            protected override object createCommonData(XmlNode xmlNode, DeserializationContext context)
            {
                Model model = getModel(xmlNode, context);
                return new CommonData()
                {
                    Model = model,
                    OidsForModel = new OidsForModel(model)
                };
            }

            private static Model getModel(XmlNode xmlNode, DeserializationContext context)
            {
                XmlNodeList xmlTagModel = xmlNode.SelectNodes(DATA_TAG_MODEL);
                if (xmlTagModel.Count == 0)
                    throw new DeserializationException("DGS-1210 model not defined.", xmlNode);
                if (xmlTagModel.Count > 1)
                    context.Report(DeserializationReportSeverity.Info, xmlNode, "Multiple model definitions found for DGS-1210 method, using the first one.");
                Model model = ModelRegister.GetByCode(xmlTagModel[0].InnerText);
                if (model == null)
                    throw new DeserializationException("Unknown DGS-1210 model defined.", xmlNode);
                return model;
            }

            private const string DATA_TAG_MODEL = "model";

            protected override IReadVlanMembershipMethod createReadConfigMethod(ISnmpConnection snmpConnection, object commonData)
                => new ReadVlanMembershipMethod(snmpConnection, commonData);

            protected override ISetPortToVlanMethod createSetPortToVlanMethod(ISnmpConnection snmpConnection, object commonData)
                => new SetPortToVlanMethod(snmpConnection, commonData);

            protected override IPersistChangesMethod createPersistChangesMethod(ISnmpConnection snmpConnection, object commonData)
                => new PersistChangesMethod(snmpConnection, commonData);

        }

        internal class MethodBase : SnmpMethodBase
        {

            public override string MibName => MIB_NAME;

            protected readonly Model _model;
            protected readonly OidsForModel _oidsForModel;

            public MethodBase(ISnmpConnection snmpConnection, object commonData)
                : base(snmpConnection)
            {
                _model = ((CommonData)commonData).Model;
                _oidsForModel = ((CommonData)commonData).OidsForModel;
            }

            protected override string CodeParameters => _model.Code;

            protected static (int, int) getByteBitIndex(int portIndex) => ((portIndex - 1) / 8, 7 - (portIndex - 1) % 8);

        }

        internal class CommonData
        {
            public Model Model { get; init; }
            public OidsForModel OidsForModel { get; init; }
        }

        private const int TXCONV_TRUTHVALUE_TRUE = 1;

    }
}
