using Lextm.SharpSnmpLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model.SwitchOperationMethods
{
    public interface ISnmpConnection
    {
        Task<List<Variable>> WalkAsync(string objectIdentifierStr);
        Task SetAsync(List<Variable> variables);
    }
}
