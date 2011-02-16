using System;

namespace NSpec.Interpreter.Indexer
{
    public class ActionRegister
    {
        private readonly Action<string, Action> actionSetter;
        public ActionRegister(Action<string,Action> actionSetter)
        {
            this.actionSetter = actionSetter;
        }
        private string key;

        public Action this[string indexer]
        {
            set
            {
                key = indexer;
                actionSetter(key,value);
            }
        }
        public Action each
        {
            set { actionSetter("each",value); }
        }

        //TODO:make it behave differently as expected
        public Action all
        {
            set { actionSetter("all",value); }
        }
    }
}