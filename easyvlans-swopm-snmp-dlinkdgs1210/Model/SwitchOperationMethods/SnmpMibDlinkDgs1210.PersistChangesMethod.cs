using B.XmlDeserializer.Context;
using Lextm.SharpSnmpLib;
using System.Xml;

namespace easyvlans.Model.SwitchOperationMethods
{
    internal sealed partial class SnmpMibDlinkDgs1210
    {
        internal class PersistChangesMethod : MethodBase, IPersistChangesMethod
        {

            public PersistChangesMethod(ISnmpConnection snmpConnection, object commonData)
                : base(snmpConnection, commonData) { }

            async Task IPersistChangesMethod.DoAsync()
                => await _snmpConnection.SetAsync(new List<Variable>() {
                    new Variable(new ObjectIdentifier(_oidsForModel.OID_COMPANYSYSTEM_SYSSAVE), new Integer32(TXCONV_TRUTHVALUE_TRUE))
                });

        }
    }
}
