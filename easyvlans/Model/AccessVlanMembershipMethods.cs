using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model
{
    internal sealed class AccessVlanMembershipMethods : MethodCollection<IAccessVlanMembershipMethod, AccessVlanMembershipQSwitchMibMethod>
    {
        public static AccessVlanMembershipMethods Instance { get; } = new();
        private AccessVlanMembershipMethods() { }
        protected override void registerMethods()
        {
            registerMethod<AccessVlanMembershipDlinkDgs121024axMethod>();
        }
    }
}