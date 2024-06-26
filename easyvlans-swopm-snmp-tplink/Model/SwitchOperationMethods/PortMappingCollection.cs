﻿namespace easyvlans.Model.SwitchOperationMethods
{

    internal class PortMappingCollection
    {

        private readonly List<PortMapping> _mappings = new();

        public PortMappingCollection(IEnumerable<PortMapping> mappings)
            => _mappings.AddRange(mappings);

        public PortMapping LookupByLocalIndex(int localIndex)
        {
            foreach (PortMapping mapping in _mappings)
                if ((localIndex >= mapping.LocalIndex) && (localIndex < mapping.LocalIndex + mapping.Count))
                    return mapping;
            return null;
        }

        public PortMapping LookupBySnmpIndex(int snmpIndex)
        {
            foreach (PortMapping mapping in _mappings)
                if ((snmpIndex >= mapping.SnmpIndex) && (snmpIndex < mapping.SnmpIndex + mapping.Count))
                    return mapping;
            return null;
        }

    }

}
