using easyvlans.Model.SwitchOperationMethods;

namespace easyvlans.Modules
{
    public class SwopmSnmpDlinkdgs1210Module : IModule
    {
        public void Init()
        {
            SnmpAccessVlanMembershipMethodRegister.Instance.RegisterFactory(new SnmpAccessVlanMembershipDlinkDgs1210Method.Factory());
            SnmpPersistChangesMethodRegister.Instance.RegisterFactory(new SnmpPersistChangesDlinkDgs1210Method.Factory());
        }
    }
}
