using System;
using NSpec.Extensions;

namespace NSpec.Interpreter.Method
{
    public class spec : SpecInterpreterBase
    {
        protected void should(string spec, Action action)
        {
            Exercise(new Example("should {0}".With(spec)), action);
        }

        protected void when(string name,Action action)
        {
            AddContext(name,action,"when");
        }

        protected void given(string name, Action action)
        {
            AddContext(name, action, "given");
        }

        protected void xshould(string format, Action action)
        {
        }

        protected void before(Action action)
        {
            Context.Before = action;
        }
    }
}
