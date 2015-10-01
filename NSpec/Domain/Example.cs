using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NSpec.Domain
{
    public class Example : ExampleBase
    {
        public override void Run(nspec nspec)
        {
            action();
        }

        public Example(Expression<Action> expr, bool pending = false) 
            : this(Parse(expr), null, expr.Compile(), pending) {}

        public Example(string name = "", string tags = "", Action action = null, bool pending = false)
            : base(name, tags, pending)
        {
            this.action = action;
        }

        Action action;
    }
}
