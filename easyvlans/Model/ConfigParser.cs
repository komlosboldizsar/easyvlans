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

        private Dictionary<string, Switch> allSwitches = new Dictionary<string, Switch>();
        private Dictionary<string, Vlan> allVlans = new Dictionary<string, Vlan>();

        public void LoadConfig()
        {
            if (File.Exists(FILE_CONFIG))
            {
                try
                {
                    allSwitches.Clear();
                    allVlans.Clear();
                    XmlDocument doc = new XmlDocument();
                    doc.Load(FILE_CONFIG);
                    XmlNode root = doc.DocumentElement;
                    if (root.LocalName != TAG_ROOT)
                        return;
                    foreach (XmlNode node in root.ChildNodes)
                    {
                        switch (node.LocalName)
                        {
                            case TAG_SWITCHES:
                                loadSwitches(node);
                                break;
                            case TAG_VLANS:
                                loadVlans(node);
                                break;
                            case TAG_PORTS:
                                loadPorts(node);
                                break;
                        }
                    }
                }
                catch
                {

                }
            }
            else
            {
                // file not exists
            }
        }

        private void loadSwitches(XmlNode parentNode)
        {
            foreach (XmlNode node in parentNode.ChildNodes)
            {
                if (node.LocalName != TAG_SWITCH)
                    continue;
                string switchId = node.Attributes[ATTRIBUTE_SWITCH_ID]?.Value;
                string switchLabel = node.Attributes[ATTRIBUTE_SWITCH_LABEL]?.Value;
                string switchIp = node.Attributes[ATTRIBUTE_SWITCH_IP]?.Value;
                Switch @switch = new Switch(switchId, switchLabel, switchIp);
                loadSwitchAccessData(node, @switch);
            }
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

        private void loadVlans(XmlNode parentNode)
        {
            foreach (XmlNode node in parentNode.ChildNodes)
            {
                if (node.LocalName != TAG_VLAN)
                    continue;
                string vlanId = node.Attributes[ATTRIBUTE_VLAN_ID]?.Value;
                string vlanName = node.Attributes[ATTRIBUTE_VLAN_NAME]?.Value;
                Vlan vlan = new Vlan(vlanId, vlanName);
            }
        }

        private void loadPorts(XmlNode parentNode)
        {
            foreach (XmlNode node in parentNode.ChildNodes)
            {
                if (node.LocalName != TAG_PORT)
                    continue;
                string portLabel = node.Attributes[ATTRIBUTE_PORT_LABEL]?.Value;
                string portSwitch = node.Attributes[ATTRIBUTE_PORT_SWITCH]?.Value;
                string portIndex = node.Attributes[ATTRIBUTE_PORT_INDEX]?.Value;
                string portVlans = node.Attributes[ATTRIBUTE_PORT_VLANS]?.Value;
                allSwitches.TryGetValue(portSwitch, out Switch @switch);
                List<Vlan> vlans = new List<Vlan>();
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
                            vlans.RemoveAll(v => allVlans.Values.Contains(v));
                        else
                            vlans.AddRange(allVlans.Values);
                    }
                    else
                    {
                        allVlans.TryGetValue(vlanId, out Vlan vlan);
                        if (exclude)
                            vlans.RemoveAll(v => (v == vlan));
                        else
                            vlans.Add(vlan);
                    }
                }
                Port port = new Port(portLabel, @switch, portIndex, vlans.Distinct().OrderBy(v => v.ID));
            }
        }

    }
}
