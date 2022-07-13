using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model
{
    internal sealed class PersistChangesMethods : MethodCollection<IPersistChangesMethod, PersistChangesWritememMethod>
    {
        public static PersistChangesMethods Instance { get; } = new();
        private PersistChangesMethods() { }
        protected override void registerMethods()
        {
            registerMethod<PersistChangesCiscoCopyMethod>();
            registerMethod<PersistChangesDlinkDgs121024axMethod>();
        }
    }
}
