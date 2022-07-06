using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model
{
    internal class AccessVlanMembershipMethods : MethodCollection<IAccessVlanMembershipMethod, AccessVlanMembershipQSwitchMibMethod>
    {
        public static AccessVlanMembershipMethods Instance { get; } = new();
        private AccessVlanMembershipMethods() { }
        protected override IAccessVlanMembershipMethod[] knownMethods { get; } = new IAccessVlanMembershipMethod[]
        {
            new AccessVlanMembershipDLinkPrivateMibMethod()
        };
    }
}