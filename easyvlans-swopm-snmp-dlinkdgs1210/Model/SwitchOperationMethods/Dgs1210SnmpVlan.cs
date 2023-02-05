namespace easyvlans.Model
{
    public class Dgs1210SnmpVlan
    {
        public int ID { get; init; }
        public byte[] EgressPorts { get; set; }
        public byte[] UntaggedPorts { get; set; }
        public Vlan UserVlan { get; set; }
        public Dgs1210SnmpVlan(int id) => ID = id;
    }
}
