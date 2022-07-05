using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model
{
    internal class AccessVlanMembershipMethods
    {

        public static AccessVlanMembershipMethods Instance { get; } = new();

        private AccessVlanMembershipMethods()
        {
            knownMethodsDictionary = new();
            knownMethodsDictionary.Add(DefaultMethod.Name, DefaultMethod);
            foreach (IAccessVlanMembershipMethod method in knownMethods)
                knownMethodsDictionary.Add(method.Name, method);
        }

        public IAccessVlanMembershipMethod Get(string name)
        {
            if (name == null)
                return DefaultMethod;
            knownMethodsDictionary.TryGetValue(name, out IAccessVlanMembershipMethod method);
            return method;
        }

        public IAccessVlanMembershipMethod DefaultMethod { get; } = new AccessVlanMembershipQSwitchMibMethod();

        private Dictionary<string, IAccessVlanMembershipMethod> knownMethodsDictionary;

        private IAccessVlanMembershipMethod[] knownMethods = new IAccessVlanMembershipMethod[] { };
 
    }
}
