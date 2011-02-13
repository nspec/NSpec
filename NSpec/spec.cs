using System;
using System.Linq.Expressions;
using NSpec.Extensions;

namespace NSpec
{
    public class spec
    {
        protected void should(string spec, Action action)
        {
            Exercise(new Example("should {0}".With(spec)), action);
        }

        protected void specify(Expression<Action> exp)
        {
            var body = exp.Body.ToString();

            var cut = body.IndexOf(").");

            var spec = body.Substring(cut+1, body.Length - cut-1).Replace(")"," ").Replace("."," ").Replace("(","").Replace("  "," ").Trim();

            Exercise(new Example( spec),exp.Compile());

            soon = new DynamicSpec();
        }

        private void Exercise(Example example, Action action)
        {
            currentContext.Befores();

            currentContext.Examples.Add(example);

            try
            {
                action();
            }
            catch (Exception e)
            {
                example.Exception = e;
            }
        }

        protected void when(string name,Action action)
        {
            AddContext(name,action,"when");
        }

        protected void given(string name, Action action)
        {
            AddContext(name, action, "given");
        }

        private void AddContext(string name, Action action, string prefix)
        {
            level++;

            var newContext = new Context(name,level,prefix);

            currentContext.Contexts.Add(newContext);

            newContext.Parent = currentContext;

            var beforeContext = currentContext;

            currentContext = newContext;

            action();

            level--;

            currentContext = beforeContext;
        }

        protected void xshould(string format, Action action)
        {
        }

        private Context currentContext;

        private int level;

        public dynamic soon;

        public void SetContext(Context context)
        {
            currentContext = context;
        }

        protected void before(Action action)
        {
            currentContext.Before = action;
        }

        protected void no_op()
        {
        }
    }
}
