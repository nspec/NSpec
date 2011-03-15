using System;
using System.Linq;
using NSpec.Extensions;

namespace NSpec.Domain
{
    public class Example
    {
        public Example(string spec)
        {
            Spec = spec;
        }

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
                .Where(l => !string.IsNullOrEmpty(l.Trim())).Do( l => s+=l.Trim()+ " ");

            return s;
        }

        public string Spec { get; set; }
        public Exception Exception { get; set; }

        public Action Action { get; set; }

        public void Run(Context context)
        {
            context.Befores();

            context.Acts();

            try
            {
                Action();
            }
            catch (Exception e)
            {
                Exception = e;
            }

            context.Afters();

        }
    }
}