using Lextm.SharpSnmpLib;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace easyvlans.Model
{

    internal class PersistChangesCiscoCopyMethod : IPersistChangesMethod
    {

        public string Name => "ciscocopy";

        public PersistChangesCiscoCopyMethod() { }
        public PersistChangesCiscoCopyMethod(Switch @switch) => _switch = @switch;
        public IPersistChangesMethod GetInstance(Switch @switch) => new PersistChangesCiscoCopyMethod(@switch);
        private Switch _switch;

        public async Task Do()
        {
            int randomRowId = _randomGenerator.Next(1, 512);
            await _switch.SnmpSetAsync(new List<Variable>() {
                new Variable(new ObjectIdentifier($"{OID_CC_COPY_SOURCE_FILE_TYPE}.{randomRowId}"), new Integer32(4)),
                new Variable(new ObjectIdentifier($"{OID_CC_COPY_DESTINATION_FILE_TYPE}.{randomRowId}"), new Integer32(3)),
                new Variable(new ObjectIdentifier($"{OID_CC_COPY_ENTRY_ROW_STATUS}.{randomRowId}"), new Integer32(1))
            });
        }

        private Random _randomGenerator = new Random();

        private const string OID_CC_COPY_SOURCE_FILE_TYPE = "1.3.6.1.4.1.9.9.96.1.1.1.1.3";
        private const string OID_CC_COPY_DESTINATION_FILE_TYPE = "1.3.6.1.4.1.9.9.96.1.1.1.1.4";
        private const string OID_CC_COPY_ENTRY_ROW_STATUS = "1.3.6.1.4.1.9.9.96.1.1.1.1.14";

    }

}
