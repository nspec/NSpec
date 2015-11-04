using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpecSpecs.describe_RunningSpecs.Exceptions
{
    class KnownException : Exception
    {
        public KnownException() : base() { }
        public KnownException(string message) : base(message) { }
    }

    class SomeOtherException : Exception
    {
        public SomeOtherException() : base() { }
    }
}
