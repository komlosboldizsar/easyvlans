using B.XmlDeserializer.Context;
using System.Xml;

namespace easyvlans.Model.SwitchOperationMethods
{
    internal static class Dgs1210Helpers
    {

        public static void GenerateOid(ref string outputMember, string template, Dgs1210Model model)
            => outputMember = string.Format(template, model.MibSubtreeIndex);

        public static Dgs1210Model GetModel(XmlNode data, DeserializationContext deserializationContext)
        {
            XmlNodeList xmlTagModel = data.SelectNodes(DATA_TAG_MODEL);
            if (xmlTagModel.Count == 0)
                throw new Exception(); // TODO: error: not instantiable
            if (xmlTagModel.Count > 1)
                deserializationContext.Report(DeserializationReportSeverity.Info, data, "Multiple model definitions found for DGS-1210 method, using the first one.");
            Dgs1210Model model = Dgs1210ModelRegister.GetByCode(xmlTagModel[0].InnerText);
            if (model == null)
                throw new Exception(); // TODO: error: not instantiable
            return model;
        }

        private const string DATA_TAG_MODEL = "model";

    }
}
