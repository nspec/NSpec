using System;

namespace NSpec.Domain
{
    public class ContextFailureException : Exception
    {
        public ContextFailureException( string message, Exception innerException )
            : base( message, innerException )
        {
        }
    }
}