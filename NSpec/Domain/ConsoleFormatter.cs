using System;
using System.Linq;
using NSpec.Domain.Extensions;

namespace NSpec.Domain
{
    public class ConsoleFormatter
    {
        public void Write(ContextCollection contexts)
        {
            contexts.Do(c => Console.WriteLine(Write(c)));

            Console.WriteLine(FailureSummary(contexts));

            Console.WriteLine(Summary(contexts));
        }

        public string Write(Context context, int level = 1)
        {
            var result = "";

            if (level == 1) result += Environment.NewLine;

            result += context.Name;

            context.Examples.Do(e => result += Write(e, level));

            context.Contexts.Do(c => result += Environment.NewLine + indent.Times(level) + Write(c, level + 1));

            return result;
        }

        public string Write(Example e, int level = 1)
        {
            var failure = e.Exception == null ? "" : " - FAILED - {0}".With(e.Exception.CleanMessage());

            var whiteSpace = Environment.NewLine + indent.Times(level);

            return e.Pending ? whiteSpace + e.Spec + " - PENDING" : whiteSpace + e.Spec + failure;
        }

        public string FailureSummary(ContextCollection contexts)
        {
            if (contexts.Failures().Count() == 0) return "";

            var summary = Environment.NewLine + "**** FAILURES ****" + Environment.NewLine;

            contexts.Failures().Do(f => summary += WriteFailure(f));

            return summary;
        }

        public string WriteFailure(Example example)
        {
            var failure = Environment.NewLine + example.FullName().Replace("_", " ") + Environment.NewLine;

            failure += example.Exception.CleanMessage() +
                Environment.NewLine + example.Exception.GetOrFallback( e=> e.StackTrace,"").Split('\n')
                    .Where(l => !new[] { "NSpec.Domain","NSpec.AssertionExtensions","NUnit.Framework" }.Any(l.Contains))
                    .Flatten("\n") + Environment.NewLine;

            return failure;
        }

        public string Summary(ContextCollection contexts)
        {
            return "{0} Examples, {1} Failed, {2} Pending".With(
                contexts.Examples().Count(),
                contexts.Failures().Count(),
                contexts.Pendings().Count()
            );
        }

        private string indent = "  ";
    }
}