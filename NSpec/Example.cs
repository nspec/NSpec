using System;
using System.Linq;

namespace NSpec
{
    public class Example
    {
        public string Spec { get; set; }
        public Exception Exception { get; set; }

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

            Extensions.Do<string>(exception.Message
                                .Split(Environment.NewLine.ToCharArray()[0])
                                .Where(l => !string.IsNullOrEmpty(l.Trim())), l => s+=l.Trim()+ " ");

            return s;
        }
    }
}