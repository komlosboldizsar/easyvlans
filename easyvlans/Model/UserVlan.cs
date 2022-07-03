using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model
{
    public class UserVlan
    {
        public int ID { get; init; }
        public string Name { get; init; }
        public UserVlan(int id, string name)
        {
            ID = id;
            Name = name;
        }
    }
}
