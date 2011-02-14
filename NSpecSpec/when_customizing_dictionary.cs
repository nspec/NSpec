using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace NSpecSpec
{
    [TestFixture]
    public class when_registering_actions
    {
        [Test]
        public void should_work()
        {
            Write(() => { Console.WriteLine("I'm special");});
            Write<string>(really => { Console.WriteLine("I'm different");});
            Write<string>(really => { Console.WriteLine("I'm still different");});
        }

        private void Write<T>(Action<T> action)
        {
            Console.WriteLine(action.GetHashCode());
        }
        private void Write(Action action)
        {
            Console.WriteLine(action.GetHashCode());
        }
    }

    public class CustomDictionary :Dictionary<string,Action>
    {
    }
}