using System;
using System.Linq;
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

        public bool Pending { get; set; }

        public override string ToString()
        {
            var example = string.Format("\t{0}", Spec);

            if (Exception != null)
                example += string.Format(" - {0}", Clean(Exception));

            return example;
        }

        private string Clean(Exception exception)
        {
            var s = "";

            exception
                .Message
                .Split(Environment.NewLine.ToCharArray()[0])
                .Where(l => !string.IsNullOrEmpty(l.Trim())).Do(l => s += l.Trim() + " ");

            return s;
        }

        public string Spec { get; set; }
        public Exception Exception { get; set; }

        public Action Action { get; set; }

        public Context Context{get;set;}

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
    }
}