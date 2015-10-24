using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpecSpecs.describe_RunningSpecs.Exceptions
{
    class BeforeAllException : Exception
    {
        public BeforeAllException() : base("BeforeAllException") { }
    }

    class BeforeException : Exception
    {
        public BeforeException() : base("BeforeException") { }
    }

    class ActException : Exception
    {
        public ActException() : base("ActException") { }
    }

    class ItException : Exception
    {
        public ItException() : base("ItException") { }
    }

    class AfterException : Exception
    {
        public AfterException() : base("AfterException") { }
    }

    class AfterAllException : Exception
    {
        public AfterAllException() : base("AfterAllException") { }
    }
}
