namespace easyvlans.Model.SwitchOperationMethods
{
    public class QBridgeSnmpPort
    {
        public int ID { get; init; }
        public int PVID { get; set; }
        public QBridgeSnmpPort(int id) => ID = id;
    }
}
