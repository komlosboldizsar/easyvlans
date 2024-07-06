namespace easyvlans.Model.SwitchOperationMethods
{
    public class SnmpV1SwitchOperationMethodCollectionDeserializer : SnmpSwitchOperationMethodCollectionDeserializerBase
    {
        public override string ElementName => "snmpv1";
        protected override ISnmpConnection createConnection(Switch @switch, string ip, int port, string communityStrings, int? trapPort, string trapCommunityString, bool trapVersionStrict)
            => new SnmpV1Connection(@switch, ip, port, communityStrings, trapPort, trapCommunityString, trapVersionStrict);
    }
}
