using Lextm.SharpSnmpLib;
using System.Xml;

namespace easyvlans.Model.SwitchOperationMethods
{

    internal class SnmpPersistChangesDlinkDgs1210Method : ISnmpPersistChangesMethod
    {

        public const string CODE = "dlinkdgs1210";

        public class Factory : ISnmpPersistChangesMethod.IFactory
        {
            public string Code => CODE;
            public ISnmpPersistChangesMethod GetInstance(XmlNode data, ISnmpSwitchOperationMethodCollection parent)
                => new SnmpPersistChangesDlinkDgs1210Method(data, parent);
        }

        private ISnmpSwitchOperationMethodCollection _parent;

        public SnmpPersistChangesDlinkDgs1210Method(XmlNode data, ISnmpSwitchOperationMethodCollection parent)
        {
            _parent = parent;
            Dgs1210Model model = Dgs1210Helpers.GetModel(data);
            Dgs1210Helpers.GenerateOid(ref OID_COMPANYSYSTEM_SYSSAVE, OID_TEMPLATE_COMPANYSYSTEM_SYSSAVE, model);
        }

        public string Code => CODE;
        public string DetailedCode => $"{_parent.Code}[{Code}]";

        async Task IPersistChangesMethod.DoAsync()
            => await _parent.SnmpConnection.SetAsync(new List<Variable>() {
                new Variable(new ObjectIdentifier(OID_COMPANYSYSTEM_SYSSAVE), new Integer32(TXCONV_TRUTHVALUE_TRUE))
            });

        private const string OID_TEMPLATE_COMPANYSYSTEM_SYSSAVE = "1.3.6.1.4.1.171.10.76.{0}.1.10.0";
        private readonly string OID_COMPANYSYSTEM_SYSSAVE;
        private const int TXCONV_TRUTHVALUE_TRUE = 1;

    }

}
