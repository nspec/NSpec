using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.Tests.describe_RunningSpecs.Exceptions
{
    class BeforeAllException : Exception
    {
        public BeforeAllException() : base("BeforeAllException") { }
    }

    class BeforeException : Exception
    {
        public BeforeException() : base("BeforeException") { }
    }

    class NestedBeforeException : Exception
    {
        public NestedBeforeException() : base("NestedBeforeException") { }
    }

    class ActException : Exception
    {
        public ActException() : base("ActException") { }
    }

    class NestedActException : Exception
    {
        public NestedActException() : base("NestedActException") { }
    }

    class ItException : Exception
    {
        public ItException() : base("ItException") { }
    }

    class AfterException : Exception
    {
        public AfterException() : base("AfterException") { }
    }

    class NestedAfterException : Exception
    {
        public NestedAfterException() : base("NestedAfterException") { }
    }

    class AfterAllException : Exception
    {
        public AfterAllException() : base("AfterAllException") { }
    }

    class KnownException : Exception
    {
        public KnownException() : base() { }
        public KnownException(string message) : base(message) { }
        public KnownException(string message, Exception inner) : base(message, inner) { }
    }

    class SomeOtherException : Exception
    {
        public SomeOtherException() : base() { }
        public SomeOtherException(string message) : base(message) { }
    }
}
