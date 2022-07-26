using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model.SwitchOperationMethods
{
    internal interface ISnmpMethod : IMethod
    {
        string Code { get; }
        public new interface IFactory<TMethodInterface> : IMethod.IFactory<TMethodInterface>
            where TMethodInterface : ISnmpMethod
        {
            TMethodInterface GetInstance(ISnmpSwitchOperationMethodCollection parent);
        }
    }
}
