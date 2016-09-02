using System;

namespace NSpec.Domain
{
    public class ContextBareCodeException : Exception
    {
        public ContextBareCodeException(Exception innerException)
            : base(bareCodeMessage, innerException)
        { }

        const string bareCodeMessage =
            "While building your test spec, code outside of any test hook threw an exception. " +
            "The whole class or context failed building, and this failing test case took its place. " +
            "Original exception details can be found in 'InnerException' here." +
            "Please double check your test code and consider running it within 'before' or 'act' hooks.";
    }
}
