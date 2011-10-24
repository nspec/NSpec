using System;
using NSpec.Domain.Formatters;

namespace NSpecRunner
{
    public class Wrapper : MarshalByRefObject
    {
        public void Execute(string dll, string classFilter, string tagsFilter, IFormatter outputFormatter, Action<string, string, string, IFormatter> action)
        {
            action( dll, classFilter, tagsFilter, outputFormatter );
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}