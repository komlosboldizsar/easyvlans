using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model
{
    internal class PersistChangesMethods
    {

        public static PersistChangesMethods Instance { get; } = new();

        private PersistChangesMethods()
        {
            knownMethodsDictionary = new();
            knownMethodsDictionary.Add(DefaultMethod.Name, DefaultMethod);
            foreach (IPersistChangesMethod method in knownMethods)
                knownMethodsDictionary.Add(method.Name, method);
        }

        public IPersistChangesMethod Get(string name)
        {
            if (name == null)
                return DefaultMethod;
            knownMethodsDictionary.TryGetValue(name, out IPersistChangesMethod method);
            return method;
        }

        public IPersistChangesMethod DefaultMethod { get; } = new PersistChangesGeneralMethod();

        private Dictionary<string, IPersistChangesMethod> knownMethodsDictionary;

        private IPersistChangesMethod[] knownMethods = new IPersistChangesMethod[]
        {
            new PersistChangesCiscoCopyMethod()
        };
 
    }
}
