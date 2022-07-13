using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model
{
    public record Vlan(int ID, string Name)
    {
        public string Label => $"{ID} - {Name}";
    }
}
