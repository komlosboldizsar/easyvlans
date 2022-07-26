using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model.SwitchOperationMethods
{
    internal sealed class SnmpPersistChangesMethodRegister : MethodRegisterBase<ISnmpPersistChangesMethod, ISnmpPersistChangesMethod.IFactory>
    {
        public static SnmpPersistChangesMethodRegister Instance { get; } = new();
        private SnmpPersistChangesMethodRegister() { }
        public ISnmpPersistChangesMethod GetMethodInstance(string code, ISnmpSwitchOperationMethodCollection parent) => getFactory(code)?.GetInstance(parent);
        protected override ISnmpPersistChangesMethod.IFactory[] KnownFactories { get; } = new ISnmpPersistChangesMethod.IFactory[]
        {
            new SnmpPersistChangesWritememMethod.Factory(),
            new SnmpPersistChangesCiscoCopyMethod.Factory(),
            new SnmpPersistChangesDlinkDgs121024axMethod.Factory()
        };
    }
}
