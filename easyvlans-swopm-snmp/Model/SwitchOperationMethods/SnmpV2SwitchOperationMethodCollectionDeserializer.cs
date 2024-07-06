namespace easyvlans.Model.SwitchOperationMethods
{
    public class SnmpV2SwitchOperationMethodCollectionDeserializer : SnmpSwitchOperationMethodCollectionDeserializerBase
    {
        public override string ElementName => "snmpv2";
        protected override ISnmpConnection createConnection(Switch @switch, string ip, int port, string communityStrings, int? trapPort, string trapCommunityString, bool trapVersionStrict)
            => new SnmpV2Connection(@switch, ip, port, communityStrings, trapPort, trapCommunityString, trapVersionStrict);
    }
}
