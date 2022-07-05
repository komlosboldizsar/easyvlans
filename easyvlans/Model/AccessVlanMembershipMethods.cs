using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model
{
    internal class AccessVlanMembershipMethods : MethodCollection<IAccessVlanMembershipMethod>
    {
        public static AccessVlanMembershipMethods Instance { get; } = new();
        private AccessVlanMembershipMethods() { }
        public override IAccessVlanMembershipMethod DefaultMethod { get; } = new AccessVlanMembershipQSwitchMibMethod();
        protected override IAccessVlanMembershipMethod[] knownMethods { get; } = new IAccessVlanMembershipMethod[] { };
    }
}