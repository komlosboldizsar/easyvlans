using easyvlans.Model.SwitchOperationMethods;

namespace easyvlans.Modules
{
    public class SwopmSnmpDlinkdgs1210Module : IModule
    {
        public void Init()
        {
            SnmpAccessVlanMembershipMethodRegister.Instance.RegisterFactory(new SnmpAccessVlanMembershipDlinkDgs121024axMethod.Factory());
            SnmpAccessVlanMembershipMethodRegister.Instance.RegisterFactory(new SnmpAccessVlanMembershipDlinkDgs121048axMethod.Factory());
            SnmpAccessVlanMembershipMethodRegister.Instance.RegisterFactory(new SnmpAccessVlanMembershipDlinkDgs121052bxMethod.Factory());
            SnmpPersistChangesMethodRegister.Instance.RegisterFactory(new SnmpPersistChangesDlinkDgs121024axMethod.Factory());
            SnmpPersistChangesMethodRegister.Instance.RegisterFactory(new SnmpPersistChangesDlinkDgs121048axMethod.Factory());
            SnmpPersistChangesMethodRegister.Instance.RegisterFactory(new SnmpPersistChangesDlinkDgs121052bxMethod.Factory());
        }
    }
}
