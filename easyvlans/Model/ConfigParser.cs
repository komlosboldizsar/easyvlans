using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

        private const string TAG_SWITCHACCESS_TELNET = "telnet";
        private const string TAG_SWITCHACCESS_SSH_USERNAMEPASSWORD = "ssh_up";
        private const string TAG_SWITCHACCESS_SSH_KEYPAIR = "ssh_kp";
        
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
                        return null;
                    Dictionary<string, Switch> switches = null;
                    Dictionary<string, Vlan> vlans = null;
                    List<Port> ports = null;
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
                    return new Config(switches, vlans, ports);
                }
                catch
                {
                    return null;
                }
            }
            else
            {
                // file not exists
                return null;
            }
        }

        private Dictionary<string, Switch> loadSwitches(XmlNode parentNode)
        {
            Dictionary<string, Switch> switches = new Dictionary<string, Switch>();
            foreach (XmlNode node in parentNode.ChildNodes)
            {
                if (node.LocalName != TAG_SWITCH)
                    continue;
                string switchId = node.Attributes[ATTRIBUTE_SWITCH_ID]?.Value;
                string switchLabel = node.Attributes[ATTRIBUTE_SWITCH_LABEL]?.Value;
                string switchIp = node.Attributes[ATTRIBUTE_SWITCH_IP]?.Value;
                Switch @switch = new Switch(switchId, switchLabel, switchIp);
                loadSwitchAccessData(node, @switch);
                switches.Add(switchId, @switch);
            }
            return switches;
        }

        private void loadSwitchAccessData(XmlNode switchNode, Switch @switch)
        {
            foreach (XmlNode node in switchNode.ChildNodes)
            {
                SwitchAccessMode sam;
                switch (node.LocalName)
                {
                    case TAG_SWITCHACCESS_TELNET:
                        sam = new SamTelnet(/*TODO*/);
                        break;
                    case TAG_SWITCHACCESS_SSH_USERNAMEPASSWORD:
                        sam = new SamSshUsernamePassword(/*TODO*/);
                        break;
                    case TAG_SWITCHACCESS_SSH_KEYPAIR:
                        sam = new SamSshKeypair(/*TODO*/);
                        break;
                }
            }
        }

        private Dictionary<string, Vlan> loadVlans(XmlNode parentNode)
        {
            Dictionary<string, Vlan> vlans = new Dictionary<string, Vlan>();
            foreach (XmlNode node in parentNode.ChildNodes)
            {
                if (node.LocalName != TAG_VLAN)
                    continue;
                string vlanId = node.Attributes[ATTRIBUTE_VLAN_ID]?.Value;
                string vlanName = node.Attributes[ATTRIBUTE_VLAN_NAME]?.Value;
                Vlan vlan = new Vlan(vlanId, vlanName);
                vlans.Add(vlanId, vlan);
            }
            return vlans;
        }

        private List<Port> loadPorts(XmlNode parentNode, Dictionary<string, Switch> switches, Dictionary<string, Vlan> vlans)
        {
            List<Port> ports = new List<Port>();
            foreach (XmlNode node in parentNode.ChildNodes)
            {
                if (node.LocalName != TAG_PORT)
                    continue;
                string portLabel = node.Attributes[ATTRIBUTE_PORT_LABEL]?.Value;
                string portSwitch = node.Attributes[ATTRIBUTE_PORT_SWITCH]?.Value;
                string portIndex = node.Attributes[ATTRIBUTE_PORT_INDEX]?.Value;
                string portVlans = node.Attributes[ATTRIBUTE_PORT_VLANS]?.Value;
                switches.TryGetValue(portSwitch, out Switch @switch);
                List<Vlan> vlansForPort = new List<Vlan>();
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
                        vlans.TryGetValue(vlanId, out Vlan vlan);
                        if (exclude)
                            vlansForPort.RemoveAll(v => (v == vlan));
                        else
                            vlansForPort.Add(vlan);
                    }
                }
                Port port = new Port(portLabel, @switch, portIndex, vlansForPort.Distinct().OrderBy(v => v.ID));
                ports.Add(port);
            }
            return ports;
        }

    }
}
