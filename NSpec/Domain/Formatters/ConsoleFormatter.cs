using System;
using System.Linq;
using NSpec.Domain.Extensions;
using System.Collections.Generic;

namespace NSpec.Domain.Formatters
{
    [Serializable]
    public class ConsoleFormatter : IFormatter, ILiveFormatter
    {
        public void Write(ContextCollection contexts)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(FailureSummary(contexts));
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(Summary(contexts));
        }

        public void Write(Context context)
        {
            if (context.Level == 1) Console.WriteLine();

            Console.WriteLine(indent.Times(context.Level - 1) + context.Name);
        }

        public void Write(Example e, int level)
        {
            var failure = e.Exception == null ? "" : " - FAILED - {0}".With(e.Exception.CleanMessage());

            var whiteSpace = indent.Times(level);

            var result = e.Pending ? whiteSpace + e.Spec + " - PENDING" : whiteSpace + e.Spec + failure;
            if (failure != "")
                Console.ForegroundColor = ConsoleColor.Red;
            else
                if (e.Pending)
                    Console.ForegroundColor = ConsoleColor.Yellow;
                else
                    Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine(result);

            Console.ForegroundColor = ConsoleColor.White;
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

            failure += example.Exception.CleanMessage() + Environment.NewLine;

            var stackTrace = FailureLines(example.Exception);

            stackTrace.AddRange(FailureLines(example.Exception.InnerException));

            var flattenedStackTrace = stackTrace.Flatten(Environment.NewLine).TrimEnd() + Environment.NewLine;

            failure += flattenedStackTrace;

            return failure;
        }

        List<string> FailureLines(Exception exception)
        {
            if (exception == null) return new List<string>();

            return exception
                .GetOrFallback(e => e.StackTrace, "")
                .Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Where(l => !internalNameSpaces.Any(l.Contains)).ToList();
        }

        public string Summary(ContextCollection contexts)
        {
            return "{0} Examples, {1} Failed, {2} Pending".With(
                contexts.Examples().Count(),
                contexts.Failures().Count(),
                contexts.Pendings().Count()
                );
        }

        string indent = "  ";

        string[] internalNameSpaces =
            new[]
                {
                    "NSpec.Domain",
                    "NSpec.AssertionExtensions",
                    "NUnit.Framework",
                    "NSpec.Extensions"
                };
    }
}