using easyvlans.Model.SwitchOperationMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Modules
{
    public class SwopmSnmpModule : IModule
    {
        public void Init()
        {
            SwitchOperationMethodCollectionRegister.Instance.RegisterFactory(new SnmpV1SwitchOperationMethodCollection.Factory());
            SwitchOperationMethodCollectionRegister.Instance.RegisterFactory(new SnmpV2SwitchOperationMethodCollection.Factory());
        }
    }
}
