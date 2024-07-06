using easyvlans.Helpers;
using Lextm.SharpSnmpLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model.SwitchOperationMethods
{
    public static class TableProcessHelpers
    {

        public static void ProcessTableRows<T>(IEnumerable<Variable> rows, Dictionary<int, T> resultStoreDictionary, Func<int, T> resultModelCreate, Action<string, Variable, T> processRow)
        {
            foreach (Variable row in rows)
            {
                SnmpVariableHelpers.IdParts idParts = row.GetIdParts();
                T resultModel = resultStoreDictionary.GetAnyway(idParts.RowId, resultModelCreate);
                processRow(idParts.NodeId, row, resultModel);
            }
        }

    }
}
