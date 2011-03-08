using NSpec.Domain;

namespace NSpec.Interpreter.Indexer
{
    public class spec : SpecInterpreterBase
    {
        protected ActionRegister before;
        protected ActionRegister after;
        protected ActionRegister act;

        protected ActionRegister context;
        protected ActionRegister describe;

        protected ActionRegister specify;
        protected ActionRegister it;

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

            context = new ActionRegister(AddContext);
            describe = new ActionRegister(AddContext);

            specify = new ActionRegister((name,action)=> Exercise(new Example(name),action));
            it = new ActionRegister((name,action)=> Exercise(new Example(name),action));
        }
    }
}