using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model
{
    internal class PersistChangesMethods : MethodCollection<IPersistChangesMethod, PersistChangesWritememMethod>
    {
        public static PersistChangesMethods Instance { get; } = new();
        private PersistChangesMethods() { }
        protected override IPersistChangesMethod[] knownMethods { get; } = new IPersistChangesMethod[]
        {
            new PersistChangesCiscoCopyMethod(),
            new PersistChangesDlinkDgs121024axMethod()
        };
    }
}
