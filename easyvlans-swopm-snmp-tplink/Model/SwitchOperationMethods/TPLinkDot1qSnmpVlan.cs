namespace easyvlans.Model.SwitchOperationMethods
{
    internal class TPLinkDot1qSnmpVlan
    {

        public int ID { get; init; }
        public TPLinkDot1qSnmpVlan(int id) => ID = id;

        public Vlan UserVlan { get; set; }

        private PortList _tagPorts;
        public string TagPorts
        {
            get => _tagPorts.OriginalString;
            set => _tagPorts = new(value);
        }

        private PortList _untagPorts;
        public string UntagPorts
        {
            get => _untagPorts.OriginalString;
            set => _untagPorts = new(value);
        }

        public bool ContainsTag(TPLinkDot1qSnmpPort snmpPort) => _tagPorts.Contains(snmpPort);
        public bool ContainsUntag(TPLinkDot1qSnmpPort snmpPort) => _untagPorts.Contains(snmpPort);

        public class PortList
        {

            public readonly string OriginalString;
            private List<Entry> _entries = new();

            public PortList(string portListString)
            {
                OriginalString = portListString;
                foreach (string aggregatedEntry in portListString.Split(","))
                {
                    string[] aggregatedEntryParts = aggregatedEntry.Trim().Split("/");
                    if (aggregatedEntryParts.Length != 3)
                        continue;
                    string prefix = $"{aggregatedEntryParts[0]}/{aggregatedEntryParts[1]}";
                    string[] portNumbers = aggregatedEntryParts[2].Split("-");
                    if (portNumbers.Length > 2)
                        continue;
                    if (!int.TryParse(portNumbers[0], out int startPortNumber))
                        continue;
                    int endPortNumber = startPortNumber;
                    if (portNumbers.Length > 1)
                        if (!int.TryParse(portNumbers[1], out endPortNumber))
                            continue;
                    _entries.Add(new Entry(prefix, startPortNumber, endPortNumber));
                }
            }

            public bool Contains(string threePartId)
            {
                string[] idParts = threePartId.Split("/");
                if (idParts.Length != 3)
                    return false;
                string prefix = $"{idParts[0]}/{idParts[1]}";
                if (!int.TryParse(idParts[2], out int portNumber))
                    return false;
                foreach (Entry entry in _entries)
                    if (entry.Contains(prefix, portNumber))
                        return true;
                return false;
            }

            public bool Contains(TPLinkDot1qSnmpPort snmpPort) => Contains($"1/0/{snmpPort.ID}");

            private record Entry(string Prefix, int StartPortNumber, int EndPortNumber)
            {
                public bool Contains(string prefix, int portNumber)
                    => ((Prefix == prefix) && (portNumber >= StartPortNumber) && (portNumber <= EndPortNumber));
            }

        }

        

    }
}
