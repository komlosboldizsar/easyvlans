using BToolbox.Logger;
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
    class SwitchPersist
    {

        private MyEmberPlusProvider _module;
        private Dictionary<BooleanParameter, Switch> _switches;
        private EmberNode _node;
        private EmberPlusProvider _provider;

        public SwitchPersist(MyEmberPlusProvider provider, EmberNode node, EmberPlusProvider emberProvider)
        {

            _module = provider;
            _node = node;
            _provider = emberProvider;
            _switches = new();

            foreach (KeyValuePair<int, Switch> @switch in provider.Switches)
            {
                EmberNode en = new(@switch.Value.RemoteIndex.Value, node, @switch.Value.Label, _provider);
                BooleanParameter bp = new(1, en, "Save Changes", _provider.dispatcher, true, remoteSetter: saveItemSetted);
                node.AddChild(en);
                node.AddChild(bp);
                _switches.Add(bp, @switch.Value);
            }
        
        }

        private bool saveItemSetted(bool arg, BooleanParameter parameter)
        {
            if (arg) { 
                _ = Task.Run(async () =>
                {
                    _switches.TryGetValue(parameter, out Switch @switch);
                    if (@switch != null)
                        await @switch.PersistChangesAsync();
                });
                parameter.Value = false;
            }
            return false;
        }

    }

}
