using System;
using System.Linq.Expressions;
using NSpec.Domain;
using NSpec.Extensions;

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

            Context.AddExample(example);

            try
            {
                action();
            }
            catch (Exception e)
            {
                example.Exception = e;
            }

            Context.Afters();
        }

        protected void xspecify(Expression<Action> exp)
        {
            Console.WriteLine("PENDING - {0}".With(Parse(exp)));
        }

        protected void specify(Expression<Action> exp)
        {
            specify(Parse(exp),exp);
        }

        protected void specify(string name,Expression<Action> exp)
        {
            Exercise(new Example( name),exp.Compile());
        }

        private string Parse(Expression<Action> exp)
        {
            var body = exp.Body.ToString();

            var cut = body.IndexOf(").");

            return body.Substring(cut+1, body.Length - cut-1).Replace(")"," ").Replace("."," ").Replace("(","").Replace("  "," ").Trim();
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