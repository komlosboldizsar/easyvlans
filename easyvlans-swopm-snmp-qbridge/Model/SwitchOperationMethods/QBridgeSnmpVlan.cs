namespace easyvlans.Model
{
    public class QBridgeSnmpVlan
    {
        public int ID { get; init; }
        public byte[] EgressPorts { get; set; }
        public byte[] UntaggedPorts { get; set; }
        public Vlan UserVlan { get; set; }
        public QBridgeSnmpVlan(int id) => ID = id;
    }
}
