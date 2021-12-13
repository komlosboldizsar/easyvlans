using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model
{

    public class Switch
    {

        public string ID { get; init; }
        public string Label { get; init; }
        public string IP { get; init; }
        private List<SwitchAccessMode> accessModes = new List<SwitchAccessMode>();

        public Switch(string id, string label, string ip)
        {
            ID = id;
            Label = label;
            IP = ip;
        }

        public void AddAccessMode(SwitchAccessMode sam)
        {
            accessModes.Add(sam);
        }

        public void PersistChanges()
        {
            throw new NotImplementedException();
        }

    }

}
