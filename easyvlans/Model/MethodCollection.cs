using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model
{
    internal abstract class MethodCollection<TMethodInterface, TDefaultMethod>
        where TMethodInterface : class, IMethod
        where TDefaultMethod : TMethodInterface, new()
    {

        public MethodCollection()
        {
            knownMethodFactoriesDictionary.Add(defaultMethodFactory.CreateInstance(null).Name, defaultMethodFactory);
        }

        public TMethodInterface GetInstance(string name, Switch @switch)
        {
            if (name == null)
                return GetDefaultMethodInstance(@switch);
            knownMethodFactoriesDictionary.TryGetValue(name, out IMethodFactory<TMethodInterface> methodFactory);
            return methodFactory?.CreateInstance(@switch);
        }

        private IMethodFactory<TMethodInterface> defaultMethodFactory = new MethodFactory<TMethodInterface, TDefaultMethod>();
        public TMethodInterface GetDefaultMethodInstance(Switch @switch) => defaultMethodFactory.CreateInstance(@switch);

        private Dictionary<string, IMethodFactory<TMethodInterface>> knownMethodFactoriesDictionary = new();

        protected void registerMethod<TMethod>() where TMethod : TMethodInterface, new()
        {
            MethodFactory<TMethodInterface, TMethod> factory = new();
            knownMethodFactoriesDictionary.Add(factory.CreateInstance(null).Name, factory);
        }

        protected abstract void registerMethods();

    }
}
