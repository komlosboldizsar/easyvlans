using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model
{
    class CouldNotConnectException : Exception
    {
        public CouldNotConnectException() { }
        public CouldNotConnectException(string message) : base(message) { }
    }
}
