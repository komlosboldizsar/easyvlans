using Lextm.SharpSnmpLib;
using System.Xml;

namespace easyvlans.Model.SwitchOperationMethods
{

    internal sealed class SnmpPersistChangesCiscoConfigCopyMethod : ISnmpPersistChangesMethod
    {

        public const string CODE = "ciscoconfigcopy";

        public class Factory : ISnmpPersistChangesMethod.IFactory
        {
            public string Code => CODE;
            public ISnmpPersistChangesMethod GetInstance(XmlNode data, ISnmpSwitchOperationMethodCollection parent)
                => new SnmpPersistChangesCiscoConfigCopyMethod(data, parent);
        }

        private ISnmpSwitchOperationMethodCollection _parent;

        public SnmpPersistChangesCiscoConfigCopyMethod(XmlNode data, ISnmpSwitchOperationMethodCollection parent)
            => _parent = parent;

        public string Code => CODE;
        public string DetailedCode => $"{_parent.Code}[{CODE}]";

        async Task IPersistChangesMethod.DoAsync()
        {
            int randomRowId = randomGenerator.Next(1, 512);
            await _parent.SnmpConnection.SetAsync(new List<Variable>() {
                new Variable(new ObjectIdentifier($"{OID_CC_COPY_ENTRY}.{COLUMN_SOURCE_FILE_TYPE}.{randomRowId}"), new Integer32(TXCONV_CONFIGFILETYPE_RUNNING)),
                new Variable(new ObjectIdentifier($"{OID_CC_COPY_ENTRY}.{COLUMN_DEST_FILE_TYPE}.{randomRowId}"), new Integer32(TXCONV_CONFIGFILETYPE_STARTUP)),
                new Variable(new ObjectIdentifier($"{OID_CC_COPY_ENTRY}.{COLUMN_ENTRY_ROW_STATUS}.{randomRowId}"), new Integer32(TXCONV_ROWSTATUS_ACTIVE))
            });
        }

        private static readonly Random randomGenerator = new();

        private const string OID_CC_COPY_ENTRY = "1.3.6.1.4.1.9.9.96.1.1.1.1";
        private const int COLUMN_SOURCE_FILE_TYPE = 3;
        private const int COLUMN_DEST_FILE_TYPE = 4;
        private const int COLUMN_ENTRY_ROW_STATUS = 14;
        private const int TXCONV_CONFIGFILETYPE_STARTUP = 3;
        private const int TXCONV_CONFIGFILETYPE_RUNNING = 4;
        private const int TXCONV_ROWSTATUS_ACTIVE = 1;

    }

}
