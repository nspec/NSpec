using System;
using System.Linq;
using NSpec.Domain.Extensions;
using System.Collections.Generic;

namespace NSpec.Domain.Formatters
{
    [Serializable]
    public class ConsoleFormatter : IFormatter, ILiveFormatter
    {
        public Action<string> WriteLineDelegate { get; set; }

        public ConsoleFormatter()
        {
            WriteLineDelegate = Console.WriteLine;
        }

        public void Write(ContextCollection contexts)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            WriteLineDelegate(FailureSummary(contexts));
            Console.ForegroundColor = ConsoleColor.White;
            WriteLineDelegate(Summary(contexts));
            Console.ResetColor();
        }

        public IDictionary<string, string> Options { get; set; }
        public void Write(Context context)
        {
            if (context.Level == 1) WriteLineDelegate("");

            WriteLineDelegate(indent.Times(context.Level - 1) + context.Name);

            if (!string.IsNullOrEmpty(context.CapturedOutput))
            {
                WriteLineDelegate(indent.Times(context.Level - 1) + "//Console output");
                foreach (var l in context.CapturedOutput.TrimEnd('\n').Split('\n'))
                {
                    WriteLineDelegate(indent.Times(context.Level - 1) + l);
                }
            }
        }

        public void Write(ExampleBase e, int level)
        {
            var noFailure = e.Exception == null;

            var failureMessage = noFailure ? "" : " - FAILED - {0}".With(e.Exception.CleanMessage());

            var whiteSpace = indent.Times(level);

            string duration;
            if (e.Duration.TotalMinutes > 1)
            {
                duration = string.Format(" ({0}min {1}s)", e.Duration.Minutes, e.Duration.Seconds);
            }
            else if (e.Duration.TotalSeconds > 1)
            {
                duration = string.Format(" ({0:F0}s)", e.Duration.TotalSeconds);
            }
            else
            {
                duration = string.Format(" ({0:F0}ms)", e.Duration.TotalMilliseconds);
            }

            var result = e.Pending ? whiteSpace + e.Spec + " - PENDING" : whiteSpace + e.Spec + duration + failureMessage;

            Console.ForegroundColor = ConsoleColor.Green;

            if (!noFailure) Console.ForegroundColor = ConsoleColor.Red;

            if (e.Pending) Console.ForegroundColor = ConsoleColor.Yellow;

            WriteLineDelegate(result);

            Console.ForegroundColor = ConsoleColor.White;

            if (!string.IsNullOrWhiteSpace(e.CapturedOutput))
            {
                WriteLineDelegate(indent.Times(level + 1) + "//Console output");
                foreach (var line in e.CapturedOutput.TrimEnd('\n').Split('\n'))
                {
                    WriteLineDelegate(indent.Times(level + 1) + line);
                }
            }
        }

        public string FailureSummary(ContextCollection contexts)
        {
            if (contexts.Failures().Count() == 0) return "";

            var summary = "\n" + "**** FAILURES ****" + "\n";

            contexts.Failures().Do(f => summary += WriteFailure(f));

            return summary;
        }

        public string WriteFailure(ExampleBase example)
        {
            var failure = "\n" + example.FullName().Replace("_", " ") + "\n";

            failure += example.Exception.CleanMessage() + "\n";

            var stackTrace = FailureLines(example.Exception);

            stackTrace.AddRange(FailureLines(example.Exception.InnerException));

            var flattenedStackTrace = stackTrace.Flatten("\n").TrimEnd() + "\n";

            failure += example.Context.GetInstance().StackTraceToPrint(flattenedStackTrace);

            return failure;
        }

        List<string> FailureLines(Exception exception)
        {
            if (exception == null) return new List<string>();

            return exception
                .GetOrFallback(e => e.StackTrace, "")
                .Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Where(l => !internalNameSpaces.Any(l.Contains)).ToList();
        }

        public string Summary(ContextCollection contexts)
        {
            var summary = "{0} Examples, {1} Failed, {2} Pending".With(
                contexts.Examples().Count(),
                contexts.Failures().Count(),
                contexts.Pendings().Count()
                );

            if (contexts.AnyTaggedWithFocus())
            {
                summary += "\n" + "\n" + @"NSpec found context/examples tagged with ""focus"" and only ran those.";
            }

            return summary;
        }

        public string FocusNotification(ContextCollection contexts)
        {
            return "";
        }

        string indent = "  ";

        string[] internalNameSpaces = new[]
        {
            "NSpec.Domain",
            "NSpec.AssertionExtensions",
            "NUnit.Framework",
            "NSpec.Extensions"
        };
    }
}
