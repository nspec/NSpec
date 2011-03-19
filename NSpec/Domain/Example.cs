using System;
using System.Linq;
using NSpec.Extensions;

namespace NSpec.Domain
{
    public class Example
    {
        public Example(string spec = "", bool pending = false)
        {
            Spec = spec;
            Pending = pending;
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

        public void Run(Context context)
        {
            fullContext = context.FullContext();

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

        private string fullContext;

        public string FullSpec()
        {
            return fullContext + " - " + Spec;
        }
    }
}