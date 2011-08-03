using System;
using NSpec.Domain.Formatters;

namespace NSpecRunner
{
    public class Wrapper : MarshalByRefObject
    {
        public void Execute(string dll, string filter, IFormatter outputFormatter, Action<string, string, IFormatter> action)
        {
            action(dll, filter, outputFormatter);
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}