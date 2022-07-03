using Lextm.SharpSnmpLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Helpers
{
    public static class SnmpVariableHelpers
    {

        public static IdParts GetIdParts(this Variable variable)
        {
            string variableId = variable.Id.ToString();
            int lastDot = variableId.LastIndexOf('.');
            string firstPart = variableId.Substring(0, lastDot);
            string lastPart = variableId.Substring(lastDot + 1);
            int.TryParse(lastPart, out int lastPartInt);
            return new IdParts(firstPart, lastPartInt);
        }

        public record IdParts(string NodeId, int RowId);

    }
}
