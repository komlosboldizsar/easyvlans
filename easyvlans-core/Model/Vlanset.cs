using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model
{
    public class Vlanset : List<Vlan>
    {
        public string ID { get; init; }
    }
}
