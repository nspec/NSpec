using System;
using NSpec.Domain;

namespace NSpecRunner
{
    public class Wrapper : MarshalByRefObject
    {
        public void Execute(RunnerInvocation invocation, Action<RunnerInvocation> action)
        {
            action(invocation);
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}