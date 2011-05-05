using System;
using System.Linq.Expressions;
using System.Reflection;

namespace NSpec.Domain
{
    public class Example
    {
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
            try
            {
                context.Befores();

                context.Acts();

                ExecuteAction(context);

                context.Afters();
            }
            catch (PendingExampleException)
            {
                Pending = true;
            }
            catch (Exception e)
            {
                Exception = e;
            }
        }

        public string FullName()
        {
            return Context.FullContext() + ". " + Spec + ".";
        }

        public bool Pending { get; set; }

        public string Spec { get; set; }

        public Exception Exception { get; set; }

        private Action action;

        public void ExecuteAction(Context context)
        {
            WasExecuted = true;

            if(MethodLevelExample != null)
            {
                try
                {
                    MethodLevelExample.Invoke(context.NSpecInstance, null);    
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

        public Context Context { get; set; }

        public MethodInfo MethodLevelExample { get; set; }

        public bool WasExecuted { get; private set; }
    }
}