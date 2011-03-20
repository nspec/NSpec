using System;
using System.Linq.Expressions;
using NSpec.Extensions;

namespace NSpec.Domain
{
    public class ActionRegister
    {
        private readonly Action<string, Action> actionSetter;

        public ActionRegister(Action<string, Action> actionSetter)
        {
            this.actionSetter = actionSetter;
        }

        public static ActionRegister operator +(ActionRegister register, Expression<Action> expression)
        {
            string name = Parse(expression);
            register[name] = expression.Compile();

            return register;
        }

        public static string Parse(Expression<Action> exp)
        {
            var body = exp.Body.ToString();

            var cut = body.IndexOf(").");

            var sentance = body.Substring(cut + 1, body.Length - cut - 1).Replace(")", " ").Replace(".", " ").Replace("(", " ").Replace("  ", " ").Trim().Replace("_", " ").Replace("\"", " ");

            while (sentance.Contains("  ")) sentance = sentance.Replace("  ", " ");

            return sentance.Trim();
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