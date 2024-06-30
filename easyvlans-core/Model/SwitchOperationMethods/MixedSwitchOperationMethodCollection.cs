namespace easyvlans.Model.SwitchOperationMethods
{
    public class MixedSwitchOperationMethodCollection : ISwitchOperationMethodCollection
    {

        public IReadInterfaceStatusMethod ReadInterfaceStatusMethod { get; init; }
        public IReadConfigMethod ReadConfigMethod { get; init; }
        public ISetPortToVlanMethod SetPortToVlanMethod { get; init; }
        public IPersistChangesMethod PersistChangesMethod { get; init; }

        public static MixedSwitchOperationMethodCollection Create(IEnumerable<ISwitchOperationMethodCollection> operationMethods, out MethodCounts methodCounts)
        {
            List<IReadInterfaceStatusMethod> readInterfaceStatusMethods = new();
            List<IReadConfigMethod> readConfigMethods = new();
            List<ISetPortToVlanMethod> setPortToVlanMethods = new();
            List<IPersistChangesMethod> persistChangesMethods = new();
            foreach (ISwitchOperationMethodCollection cm in operationMethods)
            {
                if (cm.ReadInterfaceStatusMethod != null)
                    readInterfaceStatusMethods.Add(cm.ReadInterfaceStatusMethod);
                if (cm.ReadConfigMethod != null)
                    readConfigMethods.Add(cm.ReadConfigMethod);
                if (cm.SetPortToVlanMethod != null)
                    setPortToVlanMethods.Add(cm.SetPortToVlanMethod);
                if (cm.PersistChangesMethod != null)
                    persistChangesMethods.Add(cm.PersistChangesMethod);
            }
            methodCounts = new()
            {
                ReadInterfaceStatusMethodCount = readInterfaceStatusMethods.Count,
                ReadConfigMethodCount = readConfigMethods.Count,
                SetPortToVlanMethodCount = setPortToVlanMethods.Count,
                PersistChangesMethodCount = persistChangesMethods.Count,
            };
            return new MixedSwitchOperationMethodCollection()
            {
                ReadInterfaceStatusMethod = readInterfaceStatusMethods.FirstOrDefault(),
                ReadConfigMethod = readConfigMethods.FirstOrDefault(),
                SetPortToVlanMethod = setPortToVlanMethods.FirstOrDefault(),
                PersistChangesMethod = persistChangesMethods.FirstOrDefault()
            };
        }

        public struct MethodCounts
        {
            public int ReadInterfaceStatusMethodCount { get; init; }
            public int ReadConfigMethodCount { get; init; }
            public int SetPortToVlanMethodCount { get; init; }
            public int PersistChangesMethodCount { get; init; }
        }

    }
}
