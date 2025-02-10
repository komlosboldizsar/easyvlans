using BToolbox.Helpers;
using BToolbox.Model;
using EmberPlusProviderClassLib;
using EmberPlusProviderClassLib.EmberHelpers;
using EmberPlusProviderClassLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model.Remote.EmberPlus
{
    internal class VlanToPortMatrix
    {

        private OneToNMatrix _matrix;
        private readonly Dictionary<int, string> _vlans;
        private readonly Dictionary<int, string> _ports;
        private readonly Dictionary<string, int> _vlans_reverse;
        private readonly Dictionary<string, int> _ports_reverse;
        private readonly MyEmberPlusProvider provider;

        public VlanToPortMatrix(MyEmberPlusProvider remoteModule, int index, string identifier, EmberNode parentNode, EmberPlusProvider emberTree)
        {

            _vlans = new();
            _ports = new();
            provider = remoteModule;
            int max_vlan = remoteModule.Vlans.Keys.Max();

            for (int i = 1; i <= max_vlan; i++)
            {
                string name = $"VLAN{i}";
                _vlans.Add(i, name);
            }

            for (int i = 1; i <= remoteModule.Ports.Max(i => i.Key); i++)
            {
                string name = $"Port{i}";
                _ports.Add(i, name);
            }

            _matrix = parentNode.AddMatrixOneToN(index, identifier, _vlans, _ports, emberTree, remoteConnector: RemoteConnector);
            remoteModule.Ports.Foreach(p => { p.Value.CurrentVlanChanged += WatchdPortVlanChanged; });
            _vlans_reverse = _vlans.ReverseKeysValues();
            _ports_reverse = _ports.ReverseKeysValues();

        }

        private void WatchdPortVlanChanged(Port port, Vlan vlan)
        {
            if ((port.RemoteIndex == null) || (vlan == null))
                return;
            if (port.HasComplexMembership)
                _matrix.Connect(port.RemoteIndex.Value,0);
            else
                _matrix.Connect(port.RemoteIndex.Value, vlan.ID);
        }

        private bool RemoteConnector(Signal target, IEnumerable<Signal> sources, Matrix matrix)
        {

            /*  Signal firstSource = sources.FirstOrDefault();
              if (firstSource == null)
                  return false;
              if (!_ocpMappings.TryGetValue(target.Number, out string deviceId))
                  return false;
              if (!_watchedOcps.TryGetValue(deviceId, out Ocp ocp))
                  return false;
              _ = Task.Run(() => ocp.SetNumberAsync(firstSource.Number + 1));
              return false*/

            Signal firstSource = sources.FirstOrDefault();
            if (firstSource == null)
                return false;
            if (!provider.Ports.TryGetValue(target.Number, out Port? port))
                return false;
            if ((port == null) || !provider.Vlans.TryGetValue(firstSource.Number, out Vlan vlan))
                return false;
            if (vlan == null)
                return false;

            _ = Task.Run(async () => {
                await port.SetVlanTo(vlan);
                if (provider.AutoPersist)
                    await port.Switch.PersistChangesAsync();
            });

            return false;

        }

    }

}
