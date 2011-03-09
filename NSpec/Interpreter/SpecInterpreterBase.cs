using System;
using System.Linq.Expressions;
using NSpec.Domain;
using NSpec.Extensions;

namespace NSpec.Interpreter
{
    public class SpecInterpreterBase
    {
        protected void Exercise(Example example, Action action)
        {
            Context.Befores();

            Context.Acts();

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

        protected void xspecify(string pending)
        {
            Console.WriteLine("PENDING - {0}".With(pending));
        }

        protected void xit(string pending)
        {
            xspecify(pending);
        }

        protected void specify(Expression<Action> exp)
        {
            specify(Parse(exp), exp.Compile());
        }

        protected void specify(string name, Action exp)
        {
            Exercise(new Example(name), exp);
        }

        private string Parse(Expression<Action> exp)
        {
            var body = exp.Body.ToString();

            var cut = body.IndexOf(").");

            return body.Substring(cut+1, body.Length - cut-1).Replace(")"," ").Replace("."," ").Replace("(","").Replace("  "," ").Trim();
        }

        protected void AddContext(string name, Action action)
        {
            level++;

            var newContext = new Context(name,level);

            Context.AddContext(newContext);

            var beforeContext = Context;

            Context = newContext;

            action();

            level--;

            Context = beforeContext;
        }

        private int level;
        public Context Context { get; set; }
    }
}