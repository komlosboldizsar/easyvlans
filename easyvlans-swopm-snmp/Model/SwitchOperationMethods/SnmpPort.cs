namespace easyvlans.Model.SwitchOperationMethods
{
    public class SnmpPort
    {
        public int ID { get; init; }
        public int PVID { get; set; }
        public int? VLAN { get; set; }
        public int owningVlans;
        public int lastOwningVlan;
        public SnmpPort(int id) => ID = id;
    }
}
