using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace easyvlans.Model
{
    public class ConfigParser
    {

        private const string FILE_CONFIG = "config.xml";

        private const string TAG_ROOT = "easyvlans";

        private const string TAG_SWITCHES = "switches";
        private const string TAG_SWITCH = "switch";
        private const string ATTRIBUTE_SWITCH_ID = "id";
        private const string ATTRIBUTE_SWITCH_LABEL = "label";
        private const string ATTRIBUTE_SWITCH_IP = "ip";
        private const string ATTRIBUTE_SWITCH_PORT = "port";
        private const string ATTRIBUTE_SWITCH_COMMUNITY_READ = "community_read";
        private const string ATTRIBUTE_SWITCH_COMMUNITY_WRITE = "community_write";
        private const string ATTRIBUTE_SWITCH_ACCESS_VLAN_MEMBERSHIP = "method_access_vlan_membership";
        private const string ATTRIBUTE_SWITCH_METHOD_PERSIST = "method_persist";

        private const string TAG_VLANS = "vlans";
        private const string TAG_VLAN = "vlan";
        private const string ATTRIBUTE_VLAN_ID = "id";
        private const string ATTRIBUTE_VLAN_NAME = "name";

        private const string TAG_PORTS = "ports";
        private const string TAG_PORT = "port";
        private const string ATTRIBUTE_PORT_LABEL = "label";
        private const string ATTRIBUTE_PORT_SWITCH = "switch";
        private const string ATTRIBUTE_PORT_INDEX = "index";
        private const string ATTRIBUTE_PORT_VLANS = "vlans";

        public Config LoadConfig()
        {
            if (File.Exists(FILE_CONFIG))
            {
                try
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(FILE_CONFIG);
                    XmlNode root = doc.DocumentElement;
                    if (root.LocalName != TAG_ROOT)
                        throw new ConfigParsingException("Root tag of configuration XML is invalid!");
                    Dictionary<string, Switch> switches = null;
                    Dictionary<int, UserVlan> vlans = null;
                    List<UserPort> ports = null;
                    foreach (XmlNode node in root.ChildNodes)
                    {
                        switch (node.LocalName)
                        {
                            case TAG_SWITCHES:
                                switches = loadSwitches(node);
                                break;
                            case TAG_VLANS:
                                vlans = loadVlans(node);
                                break;
                            case TAG_PORTS:
                                ports = loadPorts(node, switches, vlans);
                                break;
                        }
                    }
                    if (switches == null)
                        throw new ConfigParsingException("Couldn't load switches from configuration XML!");
                    if (switches == null)
                        throw new ConfigParsingException("Couldn't load VLANs from configuration XML!");
                    if (switches == null)
                        throw new ConfigParsingException("Couldn't load ports from configuration XML!");
                    return new Config(switches, vlans, ports);
                }
                catch (ConfigParsingException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new ConfigParsingException("An exception was thrown while parsing configuration XML!", ex);
                }
            }
            else
            {
                throw new ConfigParsingException("Couldn't find config.xml!");
            }
        }

        private Dictionary<string, Switch> loadSwitches(XmlNode parentNode)
        {
            Dictionary<string, Switch> switches = new Dictionary<string, Switch>();
            int tagIndex = 1;
            foreach (XmlNode node in parentNode.ChildNodes)
            {
                if (node.LocalName != TAG_SWITCH)
                    continue;
                string switchId = node.Attributes[ATTRIBUTE_SWITCH_ID]?.Value;
                if (string.IsNullOrWhiteSpace(switchId))
                    throw new ConfigParsingException($"ID of switch (XML attribute: {ATTRIBUTE_SWITCH_ID}) can't be empty at {tagIndex}. <{TAG_SWITCH}> tag!");
                string switchLabel = node.Attributes[ATTRIBUTE_SWITCH_LABEL]?.Value;
                if (string.IsNullOrWhiteSpace(switchLabel))
                    throw new ConfigParsingException($"Label of switch (XML attribute: {ATTRIBUTE_SWITCH_LABEL}) can't be empty at {tagIndex}. <{TAG_SWITCH}> tag!");
                string switchIp = node.Attributes[ATTRIBUTE_SWITCH_IP]?.Value;
                if (string.IsNullOrWhiteSpace(switchIp))
                    throw new ConfigParsingException($"IP address of switch (XML attribute: {ATTRIBUTE_SWITCH_IP}) can't be empty at {tagIndex}. <{TAG_SWITCH}> tag!");
                if (!REGEXP_IP_ADDRESS.IsMatch(switchIp))
                    throw new ConfigParsingException($"IP address of switch (XML attribute: {ATTRIBUTE_SWITCH_IP}) is invalid at {tagIndex}. <{TAG_SWITCH}> tag!");
                string switchPortStr = node.Attributes[ATTRIBUTE_SWITCH_PORT]?.Value;
                if (string.IsNullOrWhiteSpace(switchPortStr))
                    throw new ConfigParsingException($"Port of switch (XML attribute: {ATTRIBUTE_SWITCH_PORT}) can't be empty at {tagIndex}. <{TAG_SWITCH}> tag!");
                if (!int.TryParse(switchPortStr, out int switchPort))
                    throw new ConfigParsingException($"Port of switch (XML attribute: {ATTRIBUTE_SWITCH_PORT}) is invalid at {tagIndex}. <{TAG_SWITCH}> tag!");
                string switchCommunityRead = node.Attributes[ATTRIBUTE_SWITCH_COMMUNITY_READ]?.Value;
                if (string.IsNullOrWhiteSpace(switchCommunityRead))
                    throw new ConfigParsingException($"Read community string of switch (XML attribute: {ATTRIBUTE_SWITCH_COMMUNITY_READ}) can't be empty at {tagIndex}. <{TAG_SWITCH}> tag!");
                string switchCommunityWrite = node.Attributes[ATTRIBUTE_SWITCH_COMMUNITY_WRITE]?.Value;
                if (string.IsNullOrWhiteSpace(switchCommunityWrite))
                    throw new ConfigParsingException($"Write community string of switch (XML attribute: {ATTRIBUTE_SWITCH_COMMUNITY_WRITE}) can't be empty at {tagIndex}. <{TAG_SWITCH}> tag!");
                string switchMethodAccessVlanMembership = node.Attributes[ATTRIBUTE_SWITCH_ACCESS_VLAN_MEMBERSHIP]?.Value;
                string switchMethodPersist = node.Attributes[ATTRIBUTE_SWITCH_METHOD_PERSIST]?.Value;
                Switch @switch = new Switch(switchId, switchLabel, switchIp, switchPort, switchCommunityRead, switchCommunityWrite, switchMethodAccessVlanMembership, switchMethodPersist);
                switches.Add(switchId, @switch);
                tagIndex++;
            }
            return switches;
        }

        private readonly Regex REGEXP_IP_ADDRESS = new Regex(@"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private Dictionary<int, UserVlan> loadVlans(XmlNode parentNode)
        {
            Dictionary<int, UserVlan> vlans = new Dictionary<int, UserVlan>();
            int tagIndex = 1;
            foreach (XmlNode node in parentNode.ChildNodes)
            {
                if (node.LocalName != TAG_VLAN)
                    continue;
                string vlanIdStr = node.Attributes[ATTRIBUTE_VLAN_ID]?.Value;
                if (string.IsNullOrWhiteSpace(vlanIdStr))
                    throw new ConfigParsingException($"ID of VLAN (XML attribute: {ATTRIBUTE_VLAN_ID}) can't be empty at {tagIndex}. <{TAG_VLAN}> tag!");
                if (!int.TryParse(vlanIdStr, out int vlanId) || (vlanId < 1) || (vlanId > 4095))
                    throw new ConfigParsingException($"ID of VLAN (XML attribute: {ATTRIBUTE_VLAN_ID}) must be a valid integer between 1 and 4095 at {tagIndex}. <{TAG_VLAN}> tag!");
                string vlanName = node.Attributes[ATTRIBUTE_VLAN_NAME]?.Value;
                if (string.IsNullOrWhiteSpace(vlanName))
                    throw new ConfigParsingException($"Name of VLAN (XML attribute: {ATTRIBUTE_VLAN_NAME}) can't be empty at {tagIndex}. <{TAG_VLAN}> tag!");
                UserVlan vlan = new UserVlan(vlanId, vlanName);
                vlans.Add(vlanId, vlan);
                tagIndex++;
            }
            return vlans;
        }

        private List<UserPort> loadPorts(XmlNode parentNode, Dictionary<string, Switch> switches, Dictionary<int, UserVlan> vlans)
        {
            List<UserPort> ports = new List<UserPort>();
            int tagIndex = 1;
            foreach (XmlNode node in parentNode.ChildNodes)
            {
                if (node.LocalName != TAG_PORT)
                    continue;
                string portLabel = node.Attributes[ATTRIBUTE_PORT_LABEL]?.Value;
                if (string.IsNullOrWhiteSpace(portLabel))
                    throw new ConfigParsingException($"Label of port (XML attribute: {ATTRIBUTE_PORT_LABEL}) can't be empty at {tagIndex}. <{TAG_PORT}> tag!");
                string portSwitch = node.Attributes[ATTRIBUTE_PORT_SWITCH]?.Value;
                if (!switches.TryGetValue(portSwitch, out Switch @switch))
                    throw new ConfigParsingException($"Couldn't find switch with ID \"{portSwitch}\" for port at {tagIndex}. <{TAG_PORT}> tag!");
                string portIndexStr = node.Attributes[ATTRIBUTE_PORT_INDEX]?.Value;
                if (string.IsNullOrWhiteSpace(portIndexStr))
                    throw new ConfigParsingException($"Index of port (XML attribute: {ATTRIBUTE_PORT_INDEX}) can't be empty at {tagIndex}. <{TAG_PORT}> tag!");
                if (!int.TryParse(portIndexStr, out int portIndex))
                    throw new ConfigParsingException($"Index of port (XML attribute: {ATTRIBUTE_PORT_INDEX}) is invalid at {tagIndex}. <{TAG_PORT}> tag!");
                string portVlans = node.Attributes[ATTRIBUTE_PORT_VLANS]?.Value;
                List<UserVlan> vlansForPort = new List<UserVlan>();
                string[] vlanIds = portVlans.Split(',');
                foreach (string _vlanId in vlanIds)
                {
                    string vlanId = _vlanId;
                    bool exclude = vlanId.StartsWith('!');
                    if (exclude)
                        vlanId = vlanId.Substring(1);
                    if (vlanId == "all")
                    {
                        if (exclude)
                            vlansForPort.RemoveAll(v => vlans.Values.Contains(v));
                        else
                            vlansForPort.AddRange(vlans.Values);
                    }
                    else
                    {
                        int.TryParse(vlanId, out int vlanIdInt);
                        if (!vlans.TryGetValue(vlanIdInt, out UserVlan vlan))
                            throw new ConfigParsingException($"Couldn't find VLAN with ID \"{vlanId}\" for port at {tagIndex}. <{TAG_PORT}> tag!");
                        if (exclude)
                            vlansForPort.RemoveAll(v => (v == vlan));
                        else
                            vlansForPort.Add(vlan);
                    }
                }
                UserPort port = new UserPort(portLabel, @switch, portIndex, vlansForPort.Distinct().OrderBy(v => v.ID));
                ports.Add(port);
                tagIndex++;
            }
            return ports;
        }

    }
}
