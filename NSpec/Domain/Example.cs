using NSpec.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NSpec.Domain
{
    public class Example : ExampleBase
    {
        public override void Run(nspec nspec)
        {
            if (IsAsync)
            {
                throw new AsyncMismatchException("'it[]' cannot be set to an async delegate, please use 'itAsync[]' instead");
            }

            action();
        }

        public override bool IsAsync
        {
            get { return action.IsAsync(); }
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
