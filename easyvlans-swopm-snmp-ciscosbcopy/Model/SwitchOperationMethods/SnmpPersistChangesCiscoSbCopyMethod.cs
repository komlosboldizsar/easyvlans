using Lextm.SharpSnmpLib;
using System.Xml;

namespace easyvlans.Model.SwitchOperationMethods
{

    internal sealed class SnmpPersistChangesCiscoSbCopyMethod : ISnmpPersistChangesMethod
    {

        public const string CODE = "ciscosbcopy";

        public class Factory : ISnmpPersistChangesMethod.IFactory
        {
            public string Code => CODE;
            public ISnmpPersistChangesMethod GetInstance(XmlNode data, ISnmpSwitchOperationMethodCollection parent)
                => new SnmpPersistChangesCiscoSbCopyMethod(data, parent);
        }

        private ISnmpSwitchOperationMethodCollection _parent;

        public SnmpPersistChangesCiscoSbCopyMethod(XmlNode data, ISnmpSwitchOperationMethodCollection parent)
            => _parent = parent;

        public string Code => CODE;
        public string DetailedCode => $"{_parent.Code}[{CODE}]";

        async Task IPersistChangesMethod.DoAsync()
        {
            int randomRowId = randomGenerator.Next(1, 512);
            await _parent.SnmpConnection.SetAsync(new List<Variable>() {
                new Variable(new ObjectIdentifier($"{OID_RL_COPY_ENTRY}.{COLUMN_SOURCE_FILE_TYPE}.{randomRowId}"), new Integer32(2)),
                new Variable(new ObjectIdentifier($"{OID_RL_COPY_ENTRY}.{COLUMN_DESTINATION_FILE_TYPE}.{randomRowId}"), new Integer32(3)),
                new Variable(new ObjectIdentifier($"{OID_RL_COPY_ENTRY}.{COLUMN_SOURCE_LOCATION}.{randomRowId}"), new Integer32(1)),
                new Variable(new ObjectIdentifier($"{OID_RL_COPY_ENTRY}.{COLUMN_DESTIONATION_LOCATION}.{randomRowId}"), new Integer32(1)),
                new Variable(new ObjectIdentifier($"{OID_RL_COPY_ENTRY}.{COLUMN_ENTRY_ROW_STATUS}.{randomRowId}"), new Integer32(4)),
            });
        }

        private static readonly Random randomGenerator = new();

        private const string OID_RL_COPY_ENTRY = "1.3.6.1.4.1.9.6.1.101.87.2.1";
        private const int COLUMN_SOURCE_FILE_TYPE = 7;
        private const int COLUMN_DESTINATION_FILE_TYPE = 12;
        private const int COLUMN_SOURCE_LOCATION = 3;
        private const int COLUMN_DESTIONATION_LOCATION = 8;
        private const int COLUMN_ENTRY_ROW_STATUS = 17;

    }

}
