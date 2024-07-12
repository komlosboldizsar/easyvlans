using Lextm.SharpSnmpLib;

namespace easyvlans.Model.SwitchOperationMethods;

public class TrapDescriptor
{

    public readonly GenericCode V1GenericCode;
    public readonly int? V1SpecificCode;
    public readonly string V1EnterpriseFilter;
    public readonly ObjectIdentifier V2TrapOid;

    public TrapDescriptor(GenericCode v1GenericCode, int? v1SpecificCode, string v1EnterpriseFilter, ObjectIdentifier v2TrapOid)
    {
        if ((V1SpecificCode != null) && (V1GenericCode != GenericCode.EnterpriseSpecific))
            throw new ArgumentException("Specific code can only be defined when generic code is 'enterprise specific'.", nameof(v1SpecificCode));
        V1GenericCode = v1GenericCode;
        V1SpecificCode = v1SpecificCode;
        V1EnterpriseFilter = v1EnterpriseFilter;
        V2TrapOid = v2TrapOid;
    }

    public TrapDescriptor(GenericCode v1GenericCode, int? v1SpecificCode, string v1EnterpriseFilter, string v2TrapOidStr)
        : this(v1GenericCode, v1SpecificCode, v1EnterpriseFilter, new ObjectIdentifier(v2TrapOidStr)) { }

    public TrapDescriptor(GenericCode v1GenericCode, string v1EnterpriseFilter, ObjectIdentifier v2TrapOid)
        : this(v1GenericCode, null, v1EnterpriseFilter, v2TrapOid) { }

    public TrapDescriptor(GenericCode v1GenericCode, string v1EnterpriseFilter, string v2TrapOidStr)
        : this(v1GenericCode, null, v1EnterpriseFilter, new ObjectIdentifier(v2TrapOidStr)) { }

}