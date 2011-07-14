using System;

namespace NSpecRunner
{
    public class Wrapper : MarshalByRefObject
    {
        public void Execute(string dll, string filter, Action<string, string> action)
        {
            action(dll, filter);
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}