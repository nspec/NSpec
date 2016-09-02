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
                throw new AsyncMismatchException(
                    "'it[]' cannot be set to an async delegate, please use 'itAsync[]' instead");
            }

            action();
        }

        public override void RunPending(nspec nspec)
        {
            // don't run example body, as this example is being skipped;
            // just check for consistency in passed example body

            if (IsAsync)
            {
                throw new AsyncMismatchException(
                    "'xit' cannot be set to an async delegate, please use 'xitAsync' instead");
            }
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
