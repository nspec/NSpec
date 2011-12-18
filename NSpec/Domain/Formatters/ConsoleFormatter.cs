using System;
using System.Linq;
using NSpec.Domain.Extensions;

namespace NSpec.Domain.Formatters
{
    [Serializable]
    public class ConsoleFormatter : IFormatter, ILiveFormatter
    {
        public void Write(ContextCollection contexts)
        {
            Console.WriteLine(FailureSummary(contexts));

            Console.WriteLine(Summary(contexts));
        }

        public void Write(Context context)
        {
            if (context.Level == 1) Console.WriteLine();

            Console.WriteLine(indent.Times(context.Level - 1) + context.Name);
        }

        public void Write(Example e, int level)
        {
            var failure = e.ExampleLevelException == null ? "" : " - FAILED - {0}".With(e.ExampleLevelException.CleanMessage());

            var whiteSpace = indent.Times(level);

            var result = e.Pending ? whiteSpace + e.Spec + " - PENDING" : whiteSpace + e.Spec + failure;

            Console.WriteLine(result);
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

            failure += example.ExampleLevelException.CleanMessage() + Environment.NewLine;

            var stackTrace =
                example.ExampleLevelException
                       .GetOrFallback(e => e.StackTrace, "").Split('\n')
                       .Where(l => !internalNameSpaces.Any(l.Contains));

            var flattenedStackTrace = stackTrace.Flatten(Environment.NewLine).TrimEnd() + Environment.NewLine;

            failure += flattenedStackTrace;

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

        private string[] internalNameSpaces =
            new[] 
            { 
                "NSpec.Domain", 
                "NSpec.AssertionExtensions", 
                "NUnit.Framework" 
            };
    }
}
