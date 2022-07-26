using easyvlans.Logger;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace easyvlans.Model.SwitchOperationMethods
{
    internal abstract class SnmpV1V2SwitchOperationMethodCollectionBase : SnmpSwitchOperationMethodCollectionBase
    {

        public abstract class FactoryBase : ISwitchOperationMethodCollection.IFactory
        {

            public abstract string Code { get; }

            public ISwitchOperationMethodCollection GetInstance(XmlNode configNode, Switch @switch)
            {
                // TODO: error reporting
                string ip = configNode.Attributes[ATTRIBUTE_IP]?.Value;
                if (string.IsNullOrWhiteSpace(ip))
                    throw new ConfigParsingException($"IP address of switch (XML attribute: {ATTRIBUTE_IP}) can't be empty at TODO. <TODO> tag!");
                if (!REGEXP_IP_ADDRESS.IsMatch(ip))
                    throw new ConfigParsingException($"IP address of switch (XML attribute: {ATTRIBUTE_IP}) is invalid at TODO. <TODO> tag!");
                string portStr = configNode.Attributes[ATTRIBUTE_PORT]?.Value;
                if (string.IsNullOrWhiteSpace(portStr))
                    throw new ConfigParsingException($"Port of switch (XML attribute: {ATTRIBUTE_PORT}) can't be empty at TODO. <TODO> tag!");
                if (!int.TryParse(portStr, out int port))
                    throw new ConfigParsingException($"Port of switch (XML attribute: {ATTRIBUTE_PORT}) is invalid at TODO. <TODO> tag!");
                string communityString = configNode.Attributes[ATTRIBUTE_COMMUNITY_STRING]?.Value;
                if (string.IsNullOrWhiteSpace(communityString))
                    throw new ConfigParsingException($"Community string of switch (XML attribute: {ATTRIBUTE_COMMUNITY_STRING}) can't be empty at TODO. <TODO> tag!");
                string accessVlanMembershipMethod = configNode.Attributes[ATTRIBUTE_METHOD_ACCESS_VLAN_MEMBERSHIP]?.Value;
                string persistChangesMethod = configNode.Attributes[ATTRIBUTE_METHOD_PERSIST_CHANGES]?.Value;
                return createInstance(@switch, ip, port, communityString, accessVlanMembershipMethod, persistChangesMethod);
            }

            protected abstract ISwitchOperationMethodCollection createInstance(Switch @switch, string ip, int port, string communityStrings, string accessVlanMembershipMethodName, string persistChangesMethodName);

            private const string ATTRIBUTE_IP = "ip";
            private const string ATTRIBUTE_PORT = "port";
            private const string ATTRIBUTE_COMMUNITY_STRING = "community_string";
            private const string ATTRIBUTE_METHOD_ACCESS_VLAN_MEMBERSHIP = "method_access_vlan_membership";
            private const string ATTRIBUTE_METHOD_PERSIST_CHANGES = "method_persist_changes";

            private readonly Regex REGEXP_IP_ADDRESS = new(@"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        }

        public SnmpV1V2SwitchOperationMethodCollectionBase(Switch @switch, string accessVlanMembershipMethodName, string persistChangesMethodName)
            : base(@switch, accessVlanMembershipMethodName, persistChangesMethodName) { }

    }
}
