using System;
using System.Linq.Expressions;

namespace NSpec.Interpreter
{
    public class SpecInterpreterBase
    {
        public SpecInterpreterBase()
        {
            soon = new DynamicSpec();
        }

        protected void Exercise(Example example, Action action)
        {
            Context.Befores();

            Context.Examples.Add(example);

            try
            {
                action();
            }
            catch (Exception e)
            {
                example.Exception = e;
            }
        }

        protected void specify(Expression<Action> exp)
        {
            var body = exp.Body.ToString();

            var cut = body.IndexOf(").");

            var spec = body.Substring(cut+1, body.Length - cut-1).Replace(")"," ").Replace("."," ").Replace("(","").Replace("  "," ").Trim();

            Exercise(new Example( spec),exp.Compile());
        }

        protected void AddContext(string name, Action action, string prefix)
        {
            level++;

            var newContext = new Context(name,level,prefix);

            Context.Contexts.Add(newContext);

            newContext.Parent = Context;

            var beforeContext = Context;

            Context = newContext;

            action();

            level--;

            Context = beforeContext;
        }

        private int level;
        public dynamic soon;
        public Context Context { get; set; }
    }
}