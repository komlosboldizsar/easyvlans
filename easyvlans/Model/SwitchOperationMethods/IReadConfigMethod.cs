using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model.SwitchOperationMethods
{
    public interface IReadConfigMethod : ISwitchOperationMethod
    {
        Task DoAsync();
    }
}
