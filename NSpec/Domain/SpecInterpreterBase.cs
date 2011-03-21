using System;
using System.Linq.Expressions;

namespace NSpec.Domain
{
    public class SpecInterpreterBase
    {
        protected void Exercise(Example example)
        {
            Context.AddExample(example);

            example.Run(Context);
        }

        protected void Pending(Example example)
        {
            Context.AddExample(example);
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