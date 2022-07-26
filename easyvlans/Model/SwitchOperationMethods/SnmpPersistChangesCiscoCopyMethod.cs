using Lextm.SharpSnmpLib;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace easyvlans.Model.SwitchOperationMethods
{

    internal sealed class SnmpPersistChangesCiscoCopyMethod : ISnmpPersistChangesMethod
    {

        public const string CODE = "ciscocopy";

        public class Factory : ISnmpPersistChangesMethod.IFactory
        {
            public string Code => CODE;
            public ISnmpPersistChangesMethod GetInstance(ISnmpSwitchOperationMethodCollection parent)
                => new SnmpPersistChangesCiscoCopyMethod(parent);
        }

        private ISnmpSwitchOperationMethodCollection _parent;
        public SnmpPersistChangesCiscoCopyMethod(ISnmpSwitchOperationMethodCollection parent) => _parent = parent;
        public string Code => CODE;
        public string DetailedCode => $"{_parent.Code}[{CODE}]";

        async Task IPersistChangesMethod.DoAsync()
        {
            int randomRowId = randomGenerator.Next(1, 512);
            await _parent.SnmpConnection.SetAsync(new List<Variable>() {
                new Variable(new ObjectIdentifier($"{OID_CC_COPY_SOURCE_FILE_TYPE}.{randomRowId}"), new Integer32(4)),
                new Variable(new ObjectIdentifier($"{OID_CC_COPY_DESTINATION_FILE_TYPE}.{randomRowId}"), new Integer32(3)),
                new Variable(new ObjectIdentifier($"{OID_CC_COPY_ENTRY_ROW_STATUS}.{randomRowId}"), new Integer32(1))
            });
        }

        private static readonly Random randomGenerator = new();

        private const string OID_CC_COPY_SOURCE_FILE_TYPE = "1.3.6.1.4.1.9.9.96.1.1.1.1.3";
        private const string OID_CC_COPY_DESTINATION_FILE_TYPE = "1.3.6.1.4.1.9.9.96.1.1.1.1.4";
        private const string OID_CC_COPY_ENTRY_ROW_STATUS = "1.3.6.1.4.1.9.9.96.1.1.1.1.14";

    }

}
