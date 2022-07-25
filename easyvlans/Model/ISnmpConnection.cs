using Lextm.SharpSnmpLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model
{
    public interface ISnmpConnection
    {
        Task<List<Variable>> BulkWalkAsync(string objectIdentifierStr);
        Task SetAsync(List<Variable> variables);
    }
}
