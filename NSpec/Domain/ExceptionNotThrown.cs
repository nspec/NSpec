using System;

namespace NSpec.Domain
{
    public class ExceptionNotThrown : Exception
    {
        public ExceptionNotThrown(string message)
            : base(message) {}
    }
}