namespace easyvlans.Model.SwitchOperationMethods
{
    internal class TPLinkDot1qSnmpPort
    {

        public int ID { get; init; }
        public TPLinkDot1qSnmpPort(int id) => ID = id;

        public PortType Type { get; set; }
        public int PVID { get; set; }

        public enum PortType
        {
            Access = 0,
            Trunk = 1,
            General = 2
        }

    }
}
