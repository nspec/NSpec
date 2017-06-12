using System;

namespace NSpec.Domain
{
    public class AsyncMismatchException : Exception
    {
        public AsyncMismatchException(string message)
            : base(message) { }
    }
}
