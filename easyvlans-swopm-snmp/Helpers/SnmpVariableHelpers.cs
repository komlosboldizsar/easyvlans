using Lextm.SharpSnmpLib;

namespace easyvlans.Helpers
{
    public static class SnmpVariableHelpers
    {

        public static IdParts GetIdParts(this Variable variable)
        {
            string variableId = variable.Id.ToString();
            int lastDot = variableId.LastIndexOf('.');
            string firstPart = variableId[..lastDot];
            string lastPart = variableId[(lastDot + 1)..];
            int.TryParse(lastPart, out int lastPartInt);
            return new IdParts(firstPart, lastPartInt);
        }

        public record IdParts(string NodeId, int RowId);

    }
}
