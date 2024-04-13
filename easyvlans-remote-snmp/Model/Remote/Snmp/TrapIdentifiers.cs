namespace easyvlans.Model.Remote.Snmp
{
    internal static class TrapIdentifiers
    {

        public static string EnterpriseBase => "99";

        public static string CODE_Started => "started";
        public static int SPECIFICCODE_Started => 1001;

        public static string CODE_PortVlanMembershipChanged => "portVlanMembershipChanged";
        public static int SPECIFICCODE_PortVlanMembershipChanged => 1011;

        public static string CODE_SwitchPortsWithPendingChangeCountChanged => "switchPortsSiwthPendingChangeCountChanged";
        public static int SPECIFICCODE_SwitchPortsWithPendingChangeCountChanged => 1021;

        public static string CODE_SwitchReadVlanConfigStatusChanged => "switchReadVlanConfigStatusChanged";
        public static int SPECIFICCODE_SwitchReadVlanConfigStatusChanged => 1022;

    }
}
