using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model
{
    class SamSshKeypair : SwitchAccessMode 
    {
        public override Task Connect() => throw new NotImplementedException();
        public override Task Authenticate() => throw new NotImplementedException();
        public override void WriteLine(string line) => throw new NotImplementedException();
        public override Task<string[]> ReadLines() => throw new NotImplementedException();
    }
}
