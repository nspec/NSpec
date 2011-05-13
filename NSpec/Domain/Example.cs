using System;
using System.Linq.Expressions;
using System.Reflection;

namespace NSpec.Domain
{
    public class Example
    {
        public static string Parse(Expression<Action> exp)
        {
            var body = exp.Body.ToString();

            var cut = body.IndexOf(").");

            var sentence = body.Substring(cut + 1, body.Length - cut - 1).Replace(")", " ").Replace(".", " ").Replace("(", " ").Replace("  ", " ").Trim().Replace("_", " ").Replace("\"", " ");

            while (sentence.Contains("  ")) sentence = sentence.Replace("  ", " ");

            return sentence.Trim();
        }

        public void Run()
        {
            WasExecuted = true;

            if (MethodLevelExample != null)
            {
                try
                {
                    MethodLevelExample.Invoke(Context.NSpecInstance, null);
                }
                catch (Exception e)
                {
                    Exception = e.InnerException;
                }
            }
            else
            {
                action();
            }
        }

        public string FullName()
        {
            return Context.FullContext() + ". " + Spec + ".";
        }

        public Example(Expression<Action> expr) : this(Parse(expr), expr.Compile()) { }

        public Example(string name = "", Action action = null, bool pending = false)
        {
            this.action = action;

            Spec = name;

            Pending = pending;
        }

        public Example(MethodInfo methodLevelExample)
        {
            Spec = methodLevelExample.Name.Replace("_", " ");

            MethodLevelExample = methodLevelExample;
        }

        public bool Pending { get; set; }
        public string Spec { get; set; }
        public Exception Exception { get; set; }
        public Context Context { get; set; }
        public MethodInfo MethodLevelExample { get; set; }
        public bool WasExecuted { get; private set; }

        Action action;
    }
}