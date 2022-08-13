using B.XmlDeserializer.Attributes;
using B.XmlDeserializer.Exceptions;

namespace easyvlans.Model.Deserializers
{
    internal static class VlansetFilter
    {

        public static List<Vlan> FilterVlans(string filterString, IDictionary<int, Vlan> vlans, IDictionary<string, Vlanset> vlansets, Action<DeserializationException> invalidRelationHandler)
        {
            List<Vlan> filteredVlans = new();
            foreach (string key in filterString.Split(','))
            {
                try
                {
                    handleKey(key, filteredVlans, vlans, vlansets);
                }
                catch (DeserializationException ex)
                {
                    invalidRelationHandler(ex);
                }
            }
            return filteredVlans.Distinct().OrderBy(v => v.ID).ToList();
        }

        private static void handleKey(string key, List<Vlan> filteredVlans, IDictionary<int, Vlan> vlans, IDictionary<string, Vlanset> vlansets)
        {
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
                throw new AttributeValueInvalidException("Not allowed to reference VLAN sets in the filter string for VLAN sets.");
            if (set)
                handleSet(key, exclude, filteredVlans, vlansets);
            else if (key == KEY_ALL)
                handleAll(exclude, filteredVlans, vlans);
            else
                handleElse(key, exclude, filteredVlans, vlans);
        }

        private const string KEY_ALL = "all";

        private static void handleSet(string key, bool exclude, List<Vlan> filteredVlans, IDictionary<string, Vlanset> vlansets)
        {
            if (!vlansets.TryGetValue(key, out Vlanset vlanset))
                throw new RelatedObjectNotFoundException(key, typeof(Vlanset));
            if (exclude)
                filteredVlans.RemoveAll(v => vlanset.Contains(v));
            else
                filteredVlans.AddRange(vlanset);
        }

        private static void handleAll(bool exclude, List<Vlan> filteredVlans, IDictionary<int, Vlan> vlans)
        {
            if (exclude)
                filteredVlans.RemoveAll(v => vlans.Values.Contains(v));
            else
                filteredVlans.AddRange(vlans.Values);
        }

        private static void handleElse(string key, bool exclude, List<Vlan> filteredVlans, IDictionary<int, Vlan> vlans)
        {
            if (!int.TryParse(key, out int vlanIdInt) || (vlanIdInt < 1) || (vlanIdInt > 4095))
                throw new AttributeValueInvalidException("Keys for VLANS in the filter string must be integers between 1 and 4095.");
            if (!vlans.TryGetValue(vlanIdInt, out Vlan vlan))
                throw new RelatedObjectNotFoundException(key, typeof(Vlan));
            if (exclude)
                filteredVlans.RemoveAll(v => v == vlan);
            else
                filteredVlans.Add(vlan);
        }

    }
}
