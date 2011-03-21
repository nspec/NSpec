using System;
using System.Collections.Generic;
using System.Linq;

namespace NSpec.Domain
{
    public class ConsoleFormatter
    {
        public void Write(IList<Context> contexts)
        {
            contexts.Do(c => Console.WriteLine(Write(c)));

            Console.WriteLine();

            Console.WriteLine(FailureSummary(contexts));

            Console.WriteLine(Summary(contexts));
        }

        public string Write(Context context, int level = 1)
        {
            var result = context.Name.Replace("_", " ");

            context.Examples.Do(e => result += Write(e, level));

            context.Contexts.Do(c => result += Environment.NewLine + indent.Times(level) + Write(c, level + 1));

            return result;
        }

        public string Write(Example e, int level = 1)
        {
            var failure = e.Exception == null ? "" : " - FAILED - {0}".With(WriteException(e.Exception));

            var whiteSpace = Environment.NewLine + indent.Times(level);

            return e.Pending ? whiteSpace + e.Spec + " - PENDING" : whiteSpace + e.Spec + failure;
        }

        public string WriteFailure(Example example)
        {
            var failure = example.FullSpec().Replace("_", " ") + Environment.NewLine;

            failure += example.Exception + Environment.NewLine + Environment.NewLine;

            return failure;
        }

        public string WriteException(Exception e)
        {
            var exc = e.Message.Trim().Replace(Environment.NewLine, ", ").Trim();

            while (exc.Contains("  ")) exc = exc.Replace("  ", " ");

            return exc;
        }

        private string FailureSummary(IEnumerable<Context> contexts)
        {
            if (contexts.SelectMany(c => c.Failures()).Count() == 0) return Environment.NewLine;

            var summary = "**** FAILURES ****" + Environment.NewLine + Environment.NewLine;

            contexts.SelectMany(c => c.Failures()).Do(f => summary += WriteFailure(f));

            return summary;
        }

        public string Summary(IList<Context> contexts)
        {
            return "{0} Examples, {1} Failed, {2} Pending".With(
                contexts.SelectMany(c => c.AllExamples()).Count(),
                contexts.SelectMany(c => c.Failures()).Count(),
                contexts.SelectMany(c => c.AllPendings()).Count()
            );
        }

        private string indent = "  ";
    }
}