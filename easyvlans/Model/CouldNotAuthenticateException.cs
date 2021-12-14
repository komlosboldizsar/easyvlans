using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model
{
    class CouldNotAuthenticateException : Exception
    {
        public CouldNotAuthenticateException() { }
        public CouldNotAuthenticateException(string message) : base(message) { }
    }
}
