using System;
using System.Linq.Expressions;
using System.Reflection;

namespace NSpec.Domain
{
    public class Example
    {
        public static string Parse(Expression expressionBody)
        {
            var body = expressionBody.ToString();

            var cut = body.IndexOf(").");

            var sentence = body.Substring(cut + 1, body.Length - cut - 1).Replace(")", " ").Replace(".", " ").Replace("(", " ").Replace("  ", " ").Trim().Replace("_", " ").Replace("\"", " ");

            while (sentence.Contains("  ")) sentence = sentence.Replace("  ", " ");

            return sentence.Trim();
        }

        public static string Parse(Expression<Action> exp)
        {
            return Parse(exp.Body);
        }

        public void Run(nspec nspec)
        {
            if (MethodLevelExample != null)
            {
                try
                {
                    MethodLevelExample.Invoke(nspec, null);
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

        public bool Pending;
        public string Spec;
        public Exception Exception;
        public Context Context;
        public MethodInfo MethodLevelExample;

        Action action;
    }
}