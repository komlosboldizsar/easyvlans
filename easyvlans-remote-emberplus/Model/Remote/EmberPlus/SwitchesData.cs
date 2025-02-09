using EmberPlusProviderClassLib;
using EmberPlusProviderClassLib.EmberHelpers;
using EmberPlusProviderClassLib.Model;
using EmberPlusProviderClassLib.Model.Parameters;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model.Remote.EmberPlus
{
    class SwitchesData
    {
        private MyEmberPlusProvider _module;
        private Dictionary<BooleanParameter, Switch> _switches;
        private EmberNode _node;
        private EmberPlusProvider _provider;
        public SwitchesData(MyEmberPlusProvider provider, EmberNode node, EmberPlusProvider emberProvider)
        {
            _module = provider;
            _node = node;
            _provider = emberProvider;
            _switches = new();
            foreach (KeyValuePair<int, Switch> @switch in provider.Switchs)
            {
                EmberNode en = new EmberNode(@switch.Value.RemoteIndex.Value, node, @switch.Value.Label, _provider);
                BooleanParameter bp = new(1, node, "Save Changes", _provider.dispatcher, true);
                en.AddChild(bp);
                node.AddChild(en);
                _switches.Add(bp, @switch.Value);
            }
        }

        private bool saveItemSetted(bool arg, BooleanParameter parameter)
        {
            _ = new Task(() => {
                Switch @switch;
                _switches.TryGetValue(parameter, out @switch);
                if (@switch != null)
                    @switch.PersistChangesAsync();
                parameter.SetValueRemote(false);
            });
            return false;
        }
    }

}
