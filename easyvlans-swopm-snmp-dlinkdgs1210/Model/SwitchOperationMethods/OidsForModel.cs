namespace easyvlans.Model.SwitchOperationMethods
{
    internal class OidsForModel
    {

        public readonly string OID_DOT1Q_VLAN;
        public readonly string OID_DOT1Q_VLAN_EGRESS_PORTS;
        public readonly string OID_DOT1Q_VLAN_UNTAGGED_PORTS;
        public readonly string OID_DOT1Q_PORT_PVID;
        public readonly string OID_COMPANYSYSTEM_SYSSAVE;

        public OidsForModel(Model model)
        {
            generateOid(ref OID_DOT1Q_VLAN, OID_TEMPLATE_DOT1Q_VLAN_ENTRY, model);
            generateOid(ref OID_DOT1Q_VLAN_EGRESS_PORTS, OID_TEMPLATE_DOT1Q_VLAN_EGRESS_PORTS, model);
            generateOid(ref OID_DOT1Q_VLAN_UNTAGGED_PORTS, OID_TEMPLATE_DOT1Q_VLAN_UNTAGGED_PORTS, model);
            generateOid(ref OID_DOT1Q_PORT_PVID, OID_TEMPLATE_DOT1Q_PORT_PVID, model);
            generateOid(ref OID_COMPANYSYSTEM_SYSSAVE, OID_TEMPLATE_COMPANYSYSTEM_SYSSAVE, model);
        }

        private static void generateOid(ref string outputMember, string template, Model model)
            => outputMember = string.Format(template, model.MibSubtreeIndex);


        private const string OID_TEMPLATE_COMPANY_DOT1Q_VLAN_GROUP = "1.3.6.1.4.1.171.10.76.{0}.7";
        private const string OID_TEMPLATE_DOT1Q_VLAN_ENTRY = $"{OID_TEMPLATE_COMPANY_DOT1Q_VLAN_GROUP}.6.1";
        private const string OID_TEMPLATE_DOT1Q_VLAN_EGRESS_PORTS = $"{OID_TEMPLATE_DOT1Q_VLAN_ENTRY}.2";
        private const string OID_TEMPLATE_DOT1Q_VLAN_UNTAGGED_PORTS = $"{OID_TEMPLATE_DOT1Q_VLAN_ENTRY}.4";
        private const string OID_TEMPLATE_DOT1Q_PORT_ENTRY = $"{OID_TEMPLATE_COMPANY_DOT1Q_VLAN_GROUP}.7.1";
        private const string OID_TEMPLATE_DOT1Q_PORT_PVID = $"{OID_TEMPLATE_DOT1Q_PORT_ENTRY}.1";
        private const string OID_TEMPLATE_COMPANYSYSTEM_SYSSAVE = "1.3.6.1.4.1.171.10.76.{0}.1.10.0";

    }
}
