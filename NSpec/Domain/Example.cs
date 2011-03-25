using System;
using System.Linq.Expressions;

namespace NSpec.Domain
{
    public class Example
    {
        public Example(Expression<Action> expr) : this(Parse(expr), expr.Compile()){ }

        public Example(string name = "", Action action=null, bool pending = false)
        {
            Action = action;
            Spec = name;
            Pending = pending;
        }

        public static string Parse(Expression<Action> exp)
        {
            var body = exp.Body.ToString();

            var cut = body.IndexOf(").");

            var sentance = body.Substring(cut + 1, body.Length - cut - 1).Replace(")", " ").Replace(".", " ").Replace("(", " ").Replace("  ", " ").Trim().Replace("_", " ").Replace("\"", " ");

            while (sentance.Contains("  ")) sentance = sentance.Replace("  ", " ");

            return sentance.Trim();
        }

        public void Run(Context context)
        {
            context.Befores();

            context.Acts();

            try
            {
                Action();
            }
            catch(PendingExampleException)
            {
                Pending = true;
            }
            catch (Exception e)
            {
                Exception = e;
            }

            context.Afters();
        }

        public string FullName()
        {
            return Context.FullContext() + ". " + Spec + ".";
        }

        public bool Pending { get; set; }
        public string Spec { get; set; }
        public Exception Exception { get; set; }
        public Action Action { get; set; }
        public Context Context{get;set;}
    }
}