using System;

namespace NSpec.Interpreter.Array
{
    public class spec : SpecInterpreterBase
    {
        protected ActionRegister before;
        protected When when;
        protected When given;
        protected string each;
        private string all;

        public spec()
        {
            each = "each";
            all = "all";

            before = new ActionRegister( b => Context.Before = b);
            when = new When(AddContext("when"));
            given = new When(AddContext("given"));
        }

        private Action<string, Action> AddContext(string prefix)
        {
            return (name, action) => AddContext(name, action, prefix);
        }
    }
}