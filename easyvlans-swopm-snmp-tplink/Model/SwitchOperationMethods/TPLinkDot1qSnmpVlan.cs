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

        public bool ContainsTag(TPLinkDot1qThreePartPortId portId) => _tagPorts.Contains(portId);
        public bool ContainsUntag(TPLinkDot1qThreePartPortId portId) => _untagPorts.Contains(portId);

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

            public bool Contains(TPLinkDot1qThreePartPortId portId)
                => _entries.Any(e => e.Contains(portId));

            private record Entry(string Prefix, int StartPortNumber, int EndPortNumber)
            {
                public bool Contains(TPLinkDot1qThreePartPortId portId)
                    => ((Prefix == portId.Prefix) && (portId.Counter >= StartPortNumber) && (portId.Counter <= EndPortNumber));
            }

        }



    }
}
