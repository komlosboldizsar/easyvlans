namespace easyvlans.Model.SwitchOperationMethods
{
    public class CiscoVlanMemebershipSnmpPort
    {
        public int ID { get; init; }
        public int VLAN { get; set; }
        public int TYPE { get; set; }
        public CiscoVlanMemebershipSnmpPort(int id) => ID = id;
    }
}
