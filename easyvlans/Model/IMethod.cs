using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model
{
    internal interface IMethod<TMethodType>
        where TMethodType : IMethod<TMethodType>
    {
        string Name { get; }
        TMethodType GetInstance(Switch @switch);
    }
}
