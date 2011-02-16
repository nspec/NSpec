using System;

namespace NSpec.Interpreter.Indexer
{
    public class spec : SpecInterpreterBase
    {
        protected ActionRegister before;
        protected ActionRegister when;
        protected ActionRegister given;
        private ActionRegister after;

        public spec()
        {
            before = new ActionRegister( (f,b) =>
            {
                Context.BeforeFrequency = f;
                Context.Before = b;
            });

            after = new ActionRegister( (f, a) =>
            {
                Context.AfterFrequency = f;
                Context.After = a;
            });

            when = new ActionRegister(AddContext("when"));
            given = new ActionRegister(AddContext("given"));
        }

        private Action<string, Action> AddContext(string prefix)
        {
            return (name, action) => AddContext(name, action, prefix);
        }
    }
}