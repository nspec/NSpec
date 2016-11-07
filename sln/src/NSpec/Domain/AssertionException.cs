using System;

namespace NSpec.Domain
{
    public class AssertionException : Exception
    {
        public AssertionException(string message)
            : base(message)
        { }
    }
}
