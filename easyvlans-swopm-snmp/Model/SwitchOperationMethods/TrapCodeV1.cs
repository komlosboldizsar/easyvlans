using Lextm.SharpSnmpLib;

namespace easyvlans.Model.SwitchOperationMethods
{
    internal record TrapCodeV1(GenericCode Generic, int? Specific)
    {

        public static EqualityComparer EQUALITY_COMPARER = new();

        public class EqualityComparer : IEqualityComparer<TrapCodeV1>
        {

            bool IEqualityComparer<TrapCodeV1>.Equals(TrapCodeV1 x, TrapCodeV1 y)
            {
                if ((x.Generic == GenericCode.EnterpriseSpecific) && (y.Generic == GenericCode.EnterpriseSpecific))
                    return (x.Specific != null) && (x.Specific == y.Specific);
                return (x.Generic == y.Generic);
            }

            int IEqualityComparer<TrapCodeV1>.GetHashCode(TrapCodeV1 obj)
                => (int)obj.Generic + (obj.Specific ?? 0) * 100;

        }

    }
}
