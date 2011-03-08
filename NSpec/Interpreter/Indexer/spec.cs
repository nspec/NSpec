using NSpec.Domain;

namespace NSpec.Interpreter.Indexer
{
    public class spec : SpecInterpreterBase
    {
        protected ActionRegister before;
        protected ActionRegister when;
        protected ActionRegister specify;
        protected ActionRegister given;
        protected ActionRegister with;
        protected ActionRegister after;
        protected ActionRegister scenario;
        protected ActionRegister act;

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

            act = new ActionRegister((f, a) =>
            {
                Context.Act = a;
                Context.ActFrequency = f;
            });

            when = new ActionRegister(AddContext);
            with = new ActionRegister(AddContext);
            given = new ActionRegister(AddContext);
            scenario = new ActionRegister(AddContext);

            specify = new ActionRegister((name,action)=> Exercise(new Example(name),action));
        }
    }
}