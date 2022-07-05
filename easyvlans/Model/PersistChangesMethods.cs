using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model
{
    internal class PersistChangesMethods : MethodCollection<IPersistChangesMethod>
    {
        public static PersistChangesMethods Instance { get; } = new();
        private PersistChangesMethods() { }
        public override IPersistChangesMethod DefaultMethod { get; } = new PersistChangesWritememMethod();
        protected override IPersistChangesMethod[] knownMethods { get; } = new IPersistChangesMethod[]
        {
            new PersistChangesCiscoCopyMethod()
        };

    }
}
