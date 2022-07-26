using easyvlans.Model.SwitchOperationMethods;
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

        private const string TAG_SETTINGS = "settings";
        private const string TAG_SETTINGS_SNMP = "snmp";
        private const string ATTRIBUTE_SETTINGS_SNMP_ENABLED = "enabled";
        private const string ATTRIBUTE_SETTINGS_SNMP_PORT = "port";
        private const string ATTRIBUTE_SETTINGS_SNMP_COMMUNITY_READ = "community_read";
        private const string ATTRIBUTE_SETTINGS_SNMP_COMMUNITY_WRITE = "community_write";

        private const string TAG_SWITCHES = "switches";
        private const string TAG_SWITCH = "switch";
        private const string ATTRIBUTE_SWITCH_ID = "id";
        private const string ATTRIBUTE_SWITCH_LABEL = "label";
        private const string ATTRIBUTE_SWITCH_REMOTE_INDEX = "remote_index";

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
        private const string ATTRIBUTE_PORT_REMOTE_INDEX = "remote_index";

        private const string TAG_PAGE = "page";
        private const string ATTRIBUTE_PAGE_TITLE = "title";
        private const string ATTRIBUTE_PAGE_DEFAULT = "default";

        private const string STR_TRUE = "true";
        private const string STR_FALSE = "false";

        public Config LoadConfig()
        {
            if (File.Exists(FILE_CONFIG))
            {
                try
                {
                    XmlDocument doc = new();
                    doc.Load(FILE_CONFIG);
                    XmlNode root = doc.DocumentElement;
                    if (root.LocalName != TAG_ROOT)
                        throw new ConfigParsingException("Root tag of configuration XML is invalid!");
                    Config.SettingsGroups settings = null;
                    Dictionary<string, Switch> switches = null;
                    Dictionary<int, Vlan> vlans = null;
                    Dictionary<string, List<Vlan>> vlansets = null;
                    List<Port> ports = null;
                    List<PortPage> pages = null;
                    foreach (XmlNode node in root.ChildNodes)
                    {
                        switch (node.LocalName)
                        {
                            case TAG_SETTINGS:
                                settings = loadSettings(node);
                                break;
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
                    if (settings == null)
                        throw new ConfigParsingException("Couldn't load settings from configuration XML!");
                    if (switches == null)
                        throw new ConfigParsingException("Couldn't load switches from configuration XML!");
                    if (vlans == null)
                        throw new ConfigParsingException("Couldn't load VLANs from configuration XML!");
                    if (ports == null)
                        throw new ConfigParsingException("Couldn't load ports from configuration XML!");
                    return new Config(settings, switches, vlans, ports, pages);
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

        private Config.SettingsGroups loadSettings(XmlNode parentNode)
        {
            Config.SettingsGroups settings = new();
            foreach (XmlNode node in parentNode.ChildNodes)
            {
                switch (node.LocalName)
                {
                    case TAG_SETTINGS_SNMP:
                        if (settings.Snmp != null)
                            throw new ConfigParsingException($"Multiple SNMP setting tags (<{TAG_SETTINGS}>/<{TAG_SETTINGS_SNMP}>)!");
                        settings.Snmp = loadSnmpSettings(node);
                        break;
                }
            }
            return settings;
        }

        private Config.SnmpSettings loadSnmpSettings(XmlNode node)
        {
            Config.SnmpSettings settings = new Config.SnmpSettings();
            string enabledStr = node.Attributes[ATTRIBUTE_SETTINGS_SNMP_ENABLED]?.Value;
            if (enabledStr != null)
            {
                settings.Enabled = enabledStr switch
                {
                    STR_TRUE => true,
                    STR_FALSE => false,
                    _ => throw new ConfigParsingException($"Invalid value for SNMP setting: <{TAG_SETTINGS}>/<{TAG_SETTINGS_SNMP}>, attribute: {ATTRIBUTE_SETTINGS_SNMP_ENABLED}! It must be '{STR_TRUE}' or '{STR_FALSE}'.")
                };
            }
            string portStr = node.Attributes[ATTRIBUTE_SETTINGS_SNMP_PORT]?.Value;
            if (portStr != null)
            {
                if (!int.TryParse(portStr, out int port) || (port < 1) || (port > 65535))
                    throw new ConfigParsingException($"Invalid value for SNMP setting: <{TAG_SETTINGS}>/<{TAG_SETTINGS_SNMP}>, attribute: {ATTRIBUTE_SETTINGS_SNMP_PORT}! It must be an integer between 1 and 65535.");
                settings.Port = port;
            }
            string communityRead = node.Attributes[ATTRIBUTE_SETTINGS_SNMP_COMMUNITY_READ]?.Value;
            if (communityRead != null)
            {
                if (string.IsNullOrWhiteSpace(communityRead))
                    throw new ConfigParsingException($"Invalid value for SNMP setting: <{TAG_SETTINGS}>/<{TAG_SETTINGS_SNMP}>, attribute: {ATTRIBUTE_SETTINGS_SNMP_COMMUNITY_READ}! It must be a non-empty string.");
                settings.CommunityRead = communityRead;
            }
            string communityWrite = node.Attributes[ATTRIBUTE_SETTINGS_SNMP_COMMUNITY_WRITE]?.Value;
            if (communityWrite != null)
            {
                if (string.IsNullOrWhiteSpace(communityWrite))
                    throw new ConfigParsingException($"Invalid value for SNMP setting: <{TAG_SETTINGS}>/<{TAG_SETTINGS_SNMP}>, attribute: {ATTRIBUTE_SETTINGS_SNMP_COMMUNITY_WRITE}! It must be a non-empty string.");
                settings.CommunityWrite = communityWrite;
            }
            return settings;
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
                string remoteIndexStr = node.Attributes[ATTRIBUTE_SWITCH_REMOTE_INDEX]?.Value;
                int? remoteIndex = null;
                if (remoteIndexStr != null)
                {
                    if (string.IsNullOrWhiteSpace(remoteIndexStr))
                        throw new ConfigParsingException($"Remote index of switch (XML attribute: {ATTRIBUTE_SWITCH_REMOTE_INDEX}) can't be empty at {tagIndex}. <{TAG_SWITCH}> tag!");
                    if (!int.TryParse(remoteIndexStr, out int remoteIndexInt) || (remoteIndexInt < 1))
                        throw new ConfigParsingException($"Remote index of switch (XML attribute: {ATTRIBUTE_SWITCH_REMOTE_INDEX}) must be a valid positive integer at {tagIndex}. <{TAG_SWITCH}> tag!");
                    remoteIndex = remoteIndexInt;
                }
                Switch @switch = new(switchId, switchLabel, remoteIndex);
                @switch.OperationMethodCollection = loadSwitchOperationMethods(node, @switch).FirstOrDefault();
                switches.Add(switchId, @switch);
                tagIndex++;
            }
            return switches;
        }

        private IEnumerable<ISwitchOperationMethodCollection> loadSwitchOperationMethods(XmlNode switchNode, Switch @switch)
        {
            foreach (XmlNode childNode in switchNode.ChildNodes)
            {
                ISwitchOperationMethodCollection switchOperationMethodCollection = SwitchOperationMethodCollectionRegister.Instance.GetMethodInstance(childNode, @switch);
                if (switchOperationMethodCollection == null)
                    throw new ConfigParsingException($"No switch operation method found with code \"{childNode.LocalName}\"."); // TODO: better error reporting
                yield return switchOperationMethodCollection;
            }
        }

        private (Dictionary<int, Vlan>, Dictionary<string, List<Vlan>>) loadVlansAndVlanssets(XmlNode parentNode)
        {
            Dictionary<int, Vlan> vlans = new();
            Dictionary<string, List<Vlan>> vlansets = new();
            int tagIndexVlan = 1, tagIndexVlanset = 1;
            foreach (XmlNode node in parentNode.ChildNodes)
            {
                if (node.LocalName == TAG_VLAN)
                {
                    Vlan vlan = loadVlan(node, tagIndexVlan++);
                    vlans.Add(vlan.ID, vlan);
                }
                else if (node.LocalName == TAG_VLANSET)
                {
                    (string vlansetId, List<Vlan> vlansetVlans) = loadVlanset(node, tagIndexVlanset++, vlans);
                    vlansets.Add(vlansetId, vlansetVlans);
                }
            }
            return (vlans, vlansets);
        }

        private Vlan loadVlan(XmlNode node, int tagIndex)
        {
            string vlanIdStr = node.Attributes[ATTRIBUTE_VLAN_ID]?.Value;
            if (string.IsNullOrWhiteSpace(vlanIdStr))
                throw new ConfigParsingException($"ID of VLAN (XML attribute: {ATTRIBUTE_VLAN_ID}) can't be empty at {tagIndex}. <{TAG_VLAN}> tag!");
            if (!int.TryParse(vlanIdStr, out int vlanId) || (vlanId < 1) || (vlanId > 4095))
                throw new ConfigParsingException($"ID of VLAN (XML attribute: {ATTRIBUTE_VLAN_ID}) must be a valid integer between 1 and 4095 at {tagIndex}. <{TAG_VLAN}> tag!");
            string vlanName = node.Attributes[ATTRIBUTE_VLAN_NAME]?.Value;
            if (string.IsNullOrWhiteSpace(vlanName))
                throw new ConfigParsingException($"Name of VLAN (XML attribute: {ATTRIBUTE_VLAN_NAME}) can't be empty at {tagIndex}. <{TAG_VLAN}> tag!");
            return new(vlanId, vlanName);
        }

        private (string, List<Vlan>) loadVlanset(XmlNode node, int tagIndex, Dictionary<int, Vlan> vlans)
        {
            string vlansetId = node.Attributes[ATTRIBUTE_VLANSET_ID]?.Value;
            if (string.IsNullOrWhiteSpace(vlansetId))
                throw new ConfigParsingException($"ID of VLAN set (XML attribute: {ATTRIBUTE_VLANSET_ID}) can't be empty at {tagIndex}. <{TAG_VLANSET}> tag!");
            string vlansetFilterStr = node.Attributes[ATTRIBUTE_VLANSET_VLANS]?.Value;
            List<Vlan> vlansetVlans = filterVlans(vlansetFilterStr, vlans, null, "VLAN set", tagIndex, TAG_VLANSET);
            return (vlansetId, vlansetVlans);
        }

        private (List<Port>, List<PortPage>) loadPortsAndPages(XmlNode parentNode, Dictionary<string, Switch> switches, Dictionary<int, Vlan> vlans, Dictionary<string, List<Vlan>> vlansets)
        {
            List<Port> ports = new();
            List<PortPage> portPages = new();
            int tagIndexPort = 1, tagIndexPortPage = 1;
            foreach (XmlNode node in parentNode.ChildNodes)
            {
                if (node.LocalName == TAG_PORT)
                {
                    ports.Add(loadPort(node, tagIndexPort++, null, switches, vlans, vlansets));
                }
                else if (node.LocalName == TAG_PAGE)
                {
                    PortPage portPage = loadPortPage(node, tagIndexPortPage++);
                    portPages.Add(portPage);
                    foreach (XmlNode childNode in node.ChildNodes)
                        if (childNode.LocalName == TAG_PORT)
                            ports.Add(loadPort(childNode, tagIndexPort++, portPage, switches, vlans, vlansets));
                }
            }
            return (ports, portPages);
        }

        private Port loadPort(XmlNode node, int tagIndex, PortPage page, Dictionary<string, Switch> switches, Dictionary<int, Vlan> vlans, Dictionary<string, List<Vlan>> vlansets)
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
            List<Vlan> vlansForPort = filterVlans(portVlans, vlans, vlansets, "port", tagIndex, TAG_PORT);
            string remoteIndexStr = node.Attributes[ATTRIBUTE_PORT_REMOTE_INDEX]?.Value;
            int? remoteIndex = null;
            if (remoteIndexStr != null)
            {
                if (string.IsNullOrWhiteSpace(remoteIndexStr))
                    throw new ConfigParsingException($"Remote index of port (XML attribute: {ATTRIBUTE_PORT_REMOTE_INDEX}) can't be empty at {tagIndex}. <{TAG_PORT}> tag!");
                if (!int.TryParse(remoteIndexStr, out int remoteIndexInt) || (remoteIndexInt < 1))
                    throw new ConfigParsingException($"Remote index of port (XML attribute: {ATTRIBUTE_PORT_REMOTE_INDEX}) must be a valid positive integer at {tagIndex}. <{TAG_PORT}> tag!");
                remoteIndex = remoteIndexInt;
            }
            return new(portLabel, @switch, portIndex, vlansForPort, page, remoteIndex);
        }

        private List<Vlan> filterVlans(string filterString, Dictionary<int, Vlan> vlans, Dictionary<string, List<Vlan>> vlansets, string filteringForWhat, int filteringForWhatTagIndex, string filteringForWhatTagString)
        {
            List<Vlan> filteredVlans = new();
            string[] keys = filterString.Split(',');
            foreach (string _key in keys)
            {
                string key = _key;
                bool exclude = key.StartsWith('!');
                if (exclude)
                    key = key[1..];
                bool set = false;
                if (vlansets != null)
                {
                    set = key.StartsWith('#');
                    if (set)
                        key = key[1..];
                }
                if (set && (vlansets == null))
                    throw new ConfigParsingException($"Not allowed to use VLAN sets in the filter string for {filteringForWhat} at {filteringForWhatTagIndex}. <{filteringForWhatTagString}> tag!");
                if (set)
                {
                    if (vlansets.TryGetValue(key, out List<Vlan> vlanset))
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
                    if (!vlans.TryGetValue(vlanIdInt, out Vlan vlan))
                        throw new ConfigParsingException($"Couldn't find VLAN with ID \"{key}\" for {filteringForWhat} at {filteringForWhatTagIndex}. <{filteringForWhatTagString}> tag!");
                    if (exclude)
                        filteredVlans.RemoveAll(v => (v == vlan));
                    else
                        filteredVlans.Add(vlan);
                }
            }
            return filteredVlans.Distinct().OrderBy(v => v.ID).ToList();
        }

        private PortPage loadPortPage(XmlNode node, int tagIndex)
        {
            string pageTitle = node.Attributes[ATTRIBUTE_PAGE_TITLE]?.Value;
            if (string.IsNullOrWhiteSpace(pageTitle))
                throw new ConfigParsingException($"Title of page (XML attribute: {ATTRIBUTE_PAGE_TITLE}) can't be empty at {tagIndex}. <{TAG_PAGE}> tag!");
            string pageDefaultString = node.Attributes[ATTRIBUTE_PAGE_DEFAULT]?.Value;
            bool pageDefault = (pageDefaultString == STR_TRUE);
            return new(pageTitle, pageDefault);
        }

    }
}
