using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model
{

    internal interface IMethodFactory<TMethodInterface>
        where TMethodInterface : IMethod
    {
        TMethodInterface CreateInstance(Switch @switch);
    }

    internal sealed class MethodFactory<TMethodBase, TMethod> : IMethodFactory<TMethodBase>
        where TMethodBase : IMethod
        where TMethod : TMethodBase, new()
    {
        public TMethodBase CreateInstance(Switch @switch) => new TMethod() { Switch = @switch };
    }

}
