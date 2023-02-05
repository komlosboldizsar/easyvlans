namespace easyvlans.Model.SwitchOperationMethods
{
    public class Dgs1210SnmpPort
    {
        public int ID { get; init; }
        public int PVID { get; set; }
        public Dgs1210SnmpPort(int id) => ID = id;
    }
}
