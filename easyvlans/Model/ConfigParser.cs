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
        private const string ATTRIBUTE_SWITCH_COMMUNITY_STRING = "community_string";
        private const string ATTRIBUTE_SWITCH_ACCESS_VLAN_MEMBERSHIP = "method_access_vlan_membership";
        private const string ATTRIBUTE_SWITCH_METHOD_PERSIST = "method_persist";

        private const string TAG_VLANS = "vlans";
        private const string TAG_VLAN = "vlan";
        private const string ATTRIBUTE_VLAN_ID = "id";
        private const string ATTRIBUTE_VLAN_NAME = "name";
        private const string TAG_VLANSET = "vlanset";
        private const string ATTRIBUTE_VLANSET_ID = "id";
        private const string ATTRIBUTE_VLANSET_VLANS = "vlans";

        private const string TAG_PORTS = "ports";
        private const string TAG_PORT = "port";
        private const string ATTRIBUTE_PORT_LABEL = "label";
        private const string ATTRIBUTE_PORT_SWITCH = "switch";
        private const string ATTRIBUTE_PORT_INDEX = "index";
        private const string ATTRIBUTE_PORT_VLANS = "vlans";

        private const string TAG_PAGE = "page";
        private const string ATTRIBUTE_PAGE_TITLE = "title";
        private const string ATTRIBUTE_PAGE_DEFAULT = "default";

        private const string STR_TRUE = "true";

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
                    Dictionary<string, List<UserVlan>> vlansets = null;
                    List<UserPort> ports = null;
                    List<UserPortPage> pages = null;
                    foreach (XmlNode node in root.ChildNodes)
                    {
                        switch (node.LocalName)
                        {
                            case TAG_SWITCHES:
                                switches = loadSwitches(node);
                                break;
                            case TAG_VLANS:
                                (vlans, vlansets) = loadVlansAndVlanssets(node);
                                break;
                            case TAG_PORTS:
                                (ports, pages) = loadPortsAndPages(node, switches, vlans, vlansets);
                                break;
                        }
                    }
                    if (switches == null)
                        throw new ConfigParsingException("Couldn't load switches from configuration XML!");
                    if (switches == null)
                        throw new ConfigParsingException("Couldn't load VLANs from configuration XML!");
                    if (ports == null)
                        throw new ConfigParsingException("Couldn't load ports from configuration XML!");
                    return new Config(switches, vlans, ports, pages);
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
                string switchCommunityRead = node.Attributes[ATTRIBUTE_SWITCH_COMMUNITY_STRING]?.Value;
                if (string.IsNullOrWhiteSpace(switchCommunityRead))
                    throw new ConfigParsingException($"Community string of switch (XML attribute: {ATTRIBUTE_SWITCH_COMMUNITY_STRING}) can't be empty at {tagIndex}. <{TAG_SWITCH}> tag!");
                string switchMethodAccessVlanMembership = node.Attributes[ATTRIBUTE_SWITCH_ACCESS_VLAN_MEMBERSHIP]?.Value;
                string switchMethodPersist = node.Attributes[ATTRIBUTE_SWITCH_METHOD_PERSIST]?.Value;
                Switch @switch = new Switch(switchId, switchLabel, switchIp, switchPort, switchCommunityRead, switchMethodAccessVlanMembership, switchMethodPersist);
                switches.Add(switchId, @switch);
                tagIndex++;
            }
            return switches;
        }

        private readonly Regex REGEXP_IP_ADDRESS = new Regex(@"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private (Dictionary<int, UserVlan>, Dictionary<string, List<UserVlan>>) loadVlansAndVlanssets(XmlNode parentNode)
        {
            Dictionary<int, UserVlan> vlans = new();
            Dictionary<string, List<UserVlan>> vlansets = new();
            int tagIndexVlan = 1, tagIndexVlanset = 1;
            foreach (XmlNode node in parentNode.ChildNodes)
            {
                if (node.LocalName == TAG_VLAN)
                {
                    UserVlan vlan = loadVlan(node, tagIndexVlan++);
                    vlans.Add(vlan.ID, vlan);
                }
                else if (node.LocalName == TAG_VLANSET)
                {
                    (string vlansetId, List<UserVlan> vlansetVlans) = loadVlanset(node, tagIndexVlanset++, vlans);
                    vlansets.Add(vlansetId, vlansetVlans);
                }
            }
            return (vlans, vlansets);
        }

        private UserVlan loadVlan(XmlNode node, int tagIndex)
        {
            string vlanIdStr = node.Attributes[ATTRIBUTE_VLAN_ID]?.Value;
            if (string.IsNullOrWhiteSpace(vlanIdStr))
                throw new ConfigParsingException($"ID of VLAN (XML attribute: {ATTRIBUTE_VLAN_ID}) can't be empty at {tagIndex}. <{TAG_VLAN}> tag!");
            if (!int.TryParse(vlanIdStr, out int vlanId) || (vlanId < 1) || (vlanId > 4095))
                throw new ConfigParsingException($"ID of VLAN (XML attribute: {ATTRIBUTE_VLAN_ID}) must be a valid integer between 1 and 4095 at {tagIndex}. <{TAG_VLAN}> tag!");
            string vlanName = node.Attributes[ATTRIBUTE_VLAN_NAME]?.Value;
            if (string.IsNullOrWhiteSpace(vlanName))
                throw new ConfigParsingException($"Name of VLAN (XML attribute: {ATTRIBUTE_VLAN_NAME}) can't be empty at {tagIndex}. <{TAG_VLAN}> tag!");
            return new UserVlan(vlanId, vlanName);
        }

        private (string, List<UserVlan>) loadVlanset(XmlNode node, int tagIndex, Dictionary<int, UserVlan> vlans)
        {
            string vlansetId = node.Attributes[ATTRIBUTE_VLANSET_ID]?.Value;
            if (string.IsNullOrWhiteSpace(vlansetId))
                throw new ConfigParsingException($"ID of VLAN set (XML attribute: {ATTRIBUTE_VLANSET_ID}) can't be empty at {tagIndex}. <{TAG_VLANSET}> tag!");
            string vlansetFilterStr = node.Attributes[ATTRIBUTE_VLANSET_VLANS]?.Value;
            List<UserVlan> vlansetVlans = filterVlans(vlansetFilterStr, vlans, null, "VLAN set", tagIndex, TAG_VLANSET);
            return (vlansetId, vlansetVlans);
        }

        private (List<UserPort>, List<UserPortPage>) loadPortsAndPages(XmlNode parentNode, Dictionary<string, Switch> switches, Dictionary<int, UserVlan> vlans, Dictionary<string, List<UserVlan>> vlansets)
        {
            List<UserPort> ports = new();
            List<UserPortPage> portPages = new();
            int tagIndexPort = 1, tagIndexPortPage = 1;
            foreach (XmlNode node in parentNode.ChildNodes)
            {
                if (node.LocalName == TAG_PORT)
                {
                    ports.Add(loadPort(node, tagIndexPort++, null, switches, vlans, vlansets));
                }
                else if (node.LocalName == TAG_PAGE)
                {
                    UserPortPage portPage = loadPortPage(node, tagIndexPortPage++);
                    portPages.Add(portPage);
                    foreach (XmlNode childNode in node.ChildNodes)
                        if (childNode.LocalName == TAG_PORT)
                            ports.Add(loadPort(childNode, tagIndexPort++, portPage, switches, vlans, vlansets));
                }
            }
            return (ports, portPages);
        }

        private UserPort loadPort(XmlNode node, int tagIndex, UserPortPage page, Dictionary<string, Switch> switches, Dictionary<int, UserVlan> vlans, Dictionary<string, List<UserVlan>> vlansets)
        {
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
            List<UserVlan> vlansForPort = filterVlans(portVlans, vlans, vlansets, "port", tagIndex, TAG_PORT);
            return new UserPort(portLabel, @switch, portIndex, vlansForPort, page);
        }

        private List<UserVlan> filterVlans(string filterString, Dictionary<int, UserVlan> vlans, Dictionary<string, List<UserVlan>> vlansets, string filteringForWhat, int filteringForWhatTagIndex, string filteringForWhatTagString)
        {
            List<UserVlan> filteredVlans = new();
            string[] keys = filterString.Split(',');
            foreach (string _key in keys)
            {
                string key = _key;
                bool exclude = key.StartsWith('!');
                if (exclude)
                    key = key.Substring(1);
                bool set = false;
                if (vlansets != null)
                {
                    set = key.StartsWith('#');
                    if (set)
                        key = key.Substring(1);
                }
                if (set && (vlansets == null))
                    throw new ConfigParsingException($"Not allowed to use VLAN sets in the filter string for {filteringForWhat} at {filteringForWhatTagIndex}. <{filteringForWhatTagString}> tag!");
                if (set)
                {
                    if (vlansets.TryGetValue(key, out List<UserVlan> vlanset))
                    {
                        if (exclude)
                            filteredVlans.RemoveAll(v => vlanset.Contains(v));
                        else
                            filteredVlans.AddRange(vlanset);
                    }
                    else
                    {
                        throw new ConfigParsingException($"Couldn't find VLAN set with ID \"{key}\" for {filteringForWhat} at {filteringForWhatTagIndex}. <{filteringForWhatTagString}> tag!");
                    }
                }
                else if (key == "all")
                {
                    if (exclude)
                        filteredVlans.RemoveAll(v => vlans.Values.Contains(v));
                    else
                        filteredVlans.AddRange(vlans.Values);
                }
                else
                {
                    int.TryParse(key, out int vlanIdInt);
                    if (!vlans.TryGetValue(vlanIdInt, out UserVlan vlan))
                        throw new ConfigParsingException($"Couldn't find VLAN with ID \"{key}\" for {filteringForWhat} at {filteringForWhatTagIndex}. <{filteringForWhatTagString}> tag!");
                    if (exclude)
                        filteredVlans.RemoveAll(v => (v == vlan));
                    else
                        filteredVlans.Add(vlan);
                }
            }
            return filteredVlans.Distinct().OrderBy(v => v.ID).ToList();
        }

        private UserPortPage loadPortPage(XmlNode node, int tagIndex)
        {
            string pageTitle = node.Attributes[ATTRIBUTE_PAGE_TITLE]?.Value;
            if (string.IsNullOrWhiteSpace(pageTitle))
                throw new ConfigParsingException($"Title of page (XML attribute: {ATTRIBUTE_PAGE_TITLE}) can't be empty at {tagIndex}. <{TAG_PAGE}> tag!");
            string pageDefaultString = node.Attributes[ATTRIBUTE_PAGE_DEFAULT]?.Value;
            bool pageDefault = (pageDefaultString == STR_TRUE);
            return new UserPortPage(pageTitle, pageDefault);
        }

    }
}
