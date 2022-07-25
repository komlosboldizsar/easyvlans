using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model
{
    public interface ISetPortToVlanMethod : ISwitchOperationMethod
    {
        Task<bool> DoAsync(Port port, Vlan vlan);
    }
}
