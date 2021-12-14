using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model
{
    public abstract class SwitchAccessMode
    {
        public abstract Task Connect();
        public abstract Task Authenticate();
        public abstract void WriteLine(string line);
        public abstract Task<string[]> ReadLines();
    }
}
