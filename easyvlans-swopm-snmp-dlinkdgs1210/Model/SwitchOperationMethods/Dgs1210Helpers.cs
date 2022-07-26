namespace easyvlans.Model.SwitchOperationMethods
{
    internal static class Dgs1210Helpers
    {
        public static void GenerateOid(ref string outputMember, string template, IDgs1210Method method)
            => outputMember = string.Format(template, method.MibSubtreeIndex);
    }
}
