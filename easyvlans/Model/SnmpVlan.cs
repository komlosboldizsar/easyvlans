using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model
{
    public class SnmpVlan
    {
        public int ID { get; init; }
        public byte[] EgressPorts { get; set; }
        public byte[] UntaggedPorts { get; set; }
        public UserVlan UserVlan { get; set; }
        public SnmpVlan(int id) => ID = id;
    }
}
