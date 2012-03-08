using System;

namespace NSpec.Domain
{
    public class ActionRegister
    {
        public ActionRegister(Action<string, string, Action> actionSetter)
        {
            this.actionSetter = actionSetter;
        }

        Action<string, string, Action> actionSetter;

        public Action this[string key]
        {
            set { actionSetter(key, null, value); }
        }

        public Action this[string key, string tags]
        {
            set { actionSetter(key, tags, value); }
        }
    }
}