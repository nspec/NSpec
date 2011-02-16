using System;

namespace NSpec.Interpreter.Indexer
{
    public class spec : SpecInterpreterBase
    {
        protected ActionIndexer before;
        protected When when;
        protected When given;
        protected string each;
        private string all;
        private ActionIndexer after;

        public spec()
        {
            each = "each";
            all = "all";

            before = new ActionIndexer( b => Context.Before = b);
            after = new ActionIndexer( a => Context.After = a);
            when = new When(AddContext("when"));
            given = new When(AddContext("given"));
        }

        private Action<string, Action> AddContext(string prefix)
        {
            return (name, action) => AddContext(name, action, prefix);
        }
    }
}