using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model
{

    public class Vlan
    {

        public string ID { get; init; }
        public string Name { get; init; }

        public Vlan(string id, string name)
        {
            ID = id;
            Name = name;
        }

    }

}
