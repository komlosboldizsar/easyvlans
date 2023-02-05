using Lextm.SharpSnmpLib;
using System.Xml;

namespace easyvlans.Model.SwitchOperationMethods
{

    internal abstract class SnmpPersistChangesDlinkDgs1210MethodBase : ISnmpPersistChangesMethod, IDgs1210Method
    {

        private ISnmpSwitchOperationMethodCollection _parent;

        public SnmpPersistChangesDlinkDgs1210MethodBase(XmlNode data, ISnmpSwitchOperationMethodCollection parent)
        {
            _parent = parent;
            Dgs1210Helpers.GenerateOid(ref OID_COMPANYSYSTEM_SYSSAVE, OID_TEMPLATE_COMPANYSYSTEM_SYSSAVE, this);
        }

        public abstract string Code { get; }
        public string DetailedCode => $"{_parent.Code}[{Code}]";
        public abstract int MibSubtreeIndex { get; }

        async Task IPersistChangesMethod.DoAsync()
            => await _parent.SnmpConnection.SetAsync(new List<Variable>() {
                new Variable(new ObjectIdentifier(OID_COMPANYSYSTEM_SYSSAVE), new Integer32(TXCONV_TRUTHVALUE_TRUE))
            });

        private const string OID_TEMPLATE_COMPANYSYSTEM_SYSSAVE = "1.3.6.1.4.1.171.10.76.{0}.1.10.0";
        private readonly string OID_COMPANYSYSTEM_SYSSAVE;
        private const int TXCONV_TRUTHVALUE_TRUE = 1;

    }

}
