using System;
using System.Linq.Expressions;

namespace NSpec.Array
{
    public class spec
    {
        protected ActionRegister before;
        protected ActionRegister when;

        protected void specify(Expression<Action> exp)
        {
            var body = exp.Body.ToString();

            var cut = body.IndexOf(").");

            var spec = body.Substring(cut+1, body.Length - cut-1).Replace(")"," ").Replace("."," ").Replace("(","").Replace("  "," ").Trim();

            //Exercise(new Example( spec),exp.Compile());

            //soon = new DynamicSpec();
        }
    }
}