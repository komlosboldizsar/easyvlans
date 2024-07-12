using easyvlans.Model.SwitchOperationMethods;
using Lextm.SharpSnmpLib;

namespace easyvlans.Model;

public static class GenericTrapDescriptors
{

    private const string SNMP_TRAPS_OID = "1.3.6.1.6.3.1.1.5";

    public static readonly TrapDescriptor COLD_START = new(GenericCode.ColdStart, null, $"{SNMP_TRAPS_OID}.1");
    public static readonly TrapDescriptor WARM_START = new(GenericCode.WarmStart, null, $"{SNMP_TRAPS_OID}.2");
    public static readonly TrapDescriptor LINK_DOWN = new(GenericCode.LinkDown, null, $"{SNMP_TRAPS_OID}.3");
    public static readonly TrapDescriptor LINK_UP = new(GenericCode.LinkUp, null, $"{SNMP_TRAPS_OID}.4");
    public static readonly TrapDescriptor AUTHENTICATION_FAILURE = new(GenericCode.AuthenticationFailure, null, $"{SNMP_TRAPS_OID}.5");
    public static readonly TrapDescriptor EGP_NEIGHBOR_LOSS = new(GenericCode.EgpNeighborLoss, null, $"{SNMP_TRAPS_OID}.6");

}