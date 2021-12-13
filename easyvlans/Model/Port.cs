using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model
{

    public class Port
    {

        public string Label { get; init; }
        public Switch Switch { get; init; }
        public string Index { get; init; }
        public List<Vlan> Vlans { get; } = new List<Vlan>();

        public Port(string label, Switch @switch, string index, IEnumerable<Vlan> vlans)
        {
            Label = label;
            Switch = @switch;
            Index = index;
            Vlans.AddRange(vlans);
        }

    }

}
