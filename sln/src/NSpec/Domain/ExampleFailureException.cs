using System;

namespace NSpec.Domain
{
    public class ExampleFailureException : Exception
    {
        public static ExampleFailureException FromContext(Exception contextException)
        {
            return new ExampleFailureException(
                $"Context Failure: {contextException.Message}",
                contextException);
        }

        public static ExampleFailureException FromContextAndExample(
            Exception contextException, Exception exampleException)
        {
            return new ExampleFailureException(
                $"Context Failure: {contextException.Message}, Example Failure: {exampleException.Message}",
                contextException);
        }

        public ExampleFailureException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
