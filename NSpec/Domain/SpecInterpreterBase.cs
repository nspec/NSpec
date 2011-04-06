using System;

namespace NSpec.Domain
{
    public class SpecInterpreterBase
    {
        protected void Exercise(Example example)
        {
            Context.AddExample(example);

            if(!example.Pending)
                example.Run(Context);
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