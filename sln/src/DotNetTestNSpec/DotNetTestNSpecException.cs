using System;

namespace DotNetTestNSpec
{
    public class DotNetTestNSpecException : Exception
    {
        public DotNetTestNSpecException(string message)
            : base(message)
        { }

        public DotNetTestNSpecException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
