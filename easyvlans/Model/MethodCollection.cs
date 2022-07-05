using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model
{
    internal abstract class MethodCollection<TMethod>
        where TMethod : IMethod
    {

        public MethodCollection()
        {
            knownMethodsDictionary = new();
            knownMethodsDictionary.Add(DefaultMethod.Name, DefaultMethod);
            foreach (TMethod method in knownMethods)
                knownMethodsDictionary.Add(method.Name, method);
        }

        public TMethod Get(string name)
        {
            if (name == null)
                return DefaultMethod;
            knownMethodsDictionary.TryGetValue(name, out TMethod method);
            return method;
        }

        public abstract TMethod DefaultMethod { get; }

        private Dictionary<string, TMethod> knownMethodsDictionary;

        protected abstract TMethod[] knownMethods { get; }

    }
}
