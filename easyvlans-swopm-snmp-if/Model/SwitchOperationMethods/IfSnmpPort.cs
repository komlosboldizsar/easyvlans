using Lextm.SharpSnmpLib;

namespace easyvlans.Model.SwitchOperationMethods
{
    public class IfSnmpPort
    {
        public int ID { get; init; }
        public int AdminStatus { get; set; }
        public int OperStatus { get; set; }
        public uint? LastChange { get; set; }
        public IfSnmpPort(int id) => ID = id;
        public long InterfaceSpeed { get; set; }
    }
}
