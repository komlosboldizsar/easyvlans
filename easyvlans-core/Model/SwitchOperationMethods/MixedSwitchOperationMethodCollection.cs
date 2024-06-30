namespace easyvlans.Model.SwitchOperationMethods
{
    public class MixedSwitchOperationMethodCollection : ISwitchOperationMethodCollection
    {

        public IReadInterfaceStatusMethod ReadInterfaceStatusMethod { get; init; }
        public IReadVlanMembershipMethod ReadVlanMembershipMethod { get; init; }
        public ISetPortToVlanMethod SetPortToVlanMethod { get; init; }
        public IPersistChangesMethod PersistChangesMethod { get; init; }

        public static MixedSwitchOperationMethodCollection Create(IEnumerable<ISwitchOperationMethodCollection> operationMethods, out MethodCounts methodCounts)
        {
            List<IReadInterfaceStatusMethod> readInterfaceStatusMethods = new();
            List<IReadVlanMembershipMethod> readVlanMembershipMethods = new();
            List<ISetPortToVlanMethod> setPortToVlanMethods = new();
            List<IPersistChangesMethod> persistChangesMethods = new();
            foreach (ISwitchOperationMethodCollection cm in operationMethods)
            {
                if (cm.ReadInterfaceStatusMethod != null)
                    readInterfaceStatusMethods.Add(cm.ReadInterfaceStatusMethod);
                if (cm.ReadVlanMembershipMethod != null)
                    readVlanMembershipMethods.Add(cm.ReadVlanMembershipMethod);
                if (cm.SetPortToVlanMethod != null)
                    setPortToVlanMethods.Add(cm.SetPortToVlanMethod);
                if (cm.PersistChangesMethod != null)
                    persistChangesMethods.Add(cm.PersistChangesMethod);
            }
            methodCounts = new()
            {
                ReadInterfaceStatusMethodCount = readInterfaceStatusMethods.Count,
                ReadVlanMembershipMethodCount = readVlanMembershipMethods.Count,
                SetPortToVlanMethodCount = setPortToVlanMethods.Count,
                PersistChangesMethodCount = persistChangesMethods.Count,
            };
            return new MixedSwitchOperationMethodCollection()
            {
                ReadInterfaceStatusMethod = readInterfaceStatusMethods.FirstOrDefault(),
                ReadVlanMembershipMethod = readVlanMembershipMethods.FirstOrDefault(),
                SetPortToVlanMethod = setPortToVlanMethods.FirstOrDefault(),
                PersistChangesMethod = persistChangesMethods.FirstOrDefault()
            };
        }

        public struct MethodCounts
        {
            public int ReadInterfaceStatusMethodCount { get; init; }
            public int ReadVlanMembershipMethodCount { get; init; }
            public int SetPortToVlanMethodCount { get; init; }
            public int PersistChangesMethodCount { get; init; }
        }

    }
}
