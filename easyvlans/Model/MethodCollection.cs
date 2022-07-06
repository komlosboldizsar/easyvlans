using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model
{
    internal abstract class MethodCollection<TMethod, TDefaultMethod>
        where TMethod : class, IMethod<TMethod>
        where TDefaultMethod : TMethod, new()
    {

        public MethodCollection()
        {
            knownMethodsDictionary = new();
            knownMethodsDictionary.Add(DefaultMethod.Name, DefaultMethod);
            foreach (TMethod method in knownMethods)
                knownMethodsDictionary.Add(method.Name, method);
        }

        public TMethod Get(string name, Switch @switch)
        {
            if (name == null)
                return DefaultMethod.GetInstance(@switch);
            knownMethodsDictionary.TryGetValue(name, out TMethod method);
            return method?.GetInstance(@switch);
        }

        public TMethod DefaultMethod { get; } = new TDefaultMethod();

        private Dictionary<string, TMethod> knownMethodsDictionary;

        protected abstract TMethod[] knownMethods { get; }

    }
}
