using System;

namespace NSpec.Domain
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
    }
}