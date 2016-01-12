using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Linq;
using NUnit.Framework;
using System;
using NSpec;
using NSpec.Domain;
using NSpec.Domain.Formatters;
using SampleSpecs.Bug;

namespace NSpecSpecs
{
    [TestFixture]
    public class describe_output
    {
        [Test,
         TestCase(typeof(my_first_spec_output),
                  new [] { typeof(my_first_spec) },
                  ""),
         TestCase(typeof(describe_specifications_output),
                  new [] { typeof(describe_specifications) },
                  ""),
         TestCase(typeof(describe_before_output),
                  new [] { typeof(describe_before) },
                  ""),
         TestCase(typeof(describe_contexts_output),
                  new [] { typeof(describe_contexts) },
                  ""),
         TestCase(typeof(describe_pending_output),
                  new [] { typeof(describe_pending) },
                  ""),
         TestCase(typeof(describe_helpers_output),
                  new [] { typeof(describe_helpers) },
                  ""),
         TestCase(typeof(describe_async_helpers_output),
                  new [] { typeof(describe_async_helpers) },
                  ""),
         TestCase(typeof(describe_batman_sound_effects_as_text_output),
                  new [] { typeof(describe_batman_sound_effects_as_text) },
                  ""),
         TestCase(typeof(describe_class_level_output),
                  new [] { typeof(describe_class_level) },
                  ""),
         TestCase(typeof(given_the_sequence_continues_with_2_output),
                  new []
                  {
                      typeof(given_the_sequence_continues_with_2),
                      typeof( given_the_sequence_starts_with_1)
                  },
                  ""),
         TestCase(typeof(describe_exception_output),
                  new [] { typeof(describe_exception) },
                  ""),
         TestCase(typeof(describe_context_stack_trace_output),
                  new [] { typeof(describe_context_stack_trace) },
                  ""),
         TestCase(typeof(describe_ICollection_output),
                  new []
                  {
                      typeof(describe_ICollection),
                      typeof(describe_LinkedList),
                      typeof(describe_List)
                  },
                  ""),
         TestCase(typeof(describe_changing_stacktrace_message_output),
                  new [] { typeof(describe_changing_stacktrace_message) },
                  ""),
         TestCase(typeof(describe_changing_failure_exception_output),
                  new [] { typeof(describe_changing_failure_exception) },
                  ""),
         TestCase(typeof(describe_focus_output),
                  new [] { typeof(describe_focus) },
                  "focus")]
        
        public void output_verification(Type output, Type []testClasses, string tags)
        {
            var finder = new SpecFinder(testClasses, "");
            var builder = new ContextBuilder(finder, new Tags().Parse(tags), new DefaultConventions());
            var consoleFormatter = new ConsoleFormatter();

            var actual = new System.Collections.Generic.List<string>();
            consoleFormatter.WriteLineDelegate = actual.Add;

            var runner = new ContextRunner(builder, consoleFormatter, false);
            runner.Run(builder.Contexts().Build());

            var expectedString = ScrubStackTrace(ScrubNewLines(output.GetField("Output").GetValue(null) as string));
            var actualString = ScrubStackTrace(String.Join("\n", actual)).Trim();
            actualString.should_be(expectedString);

            var guid = Guid.NewGuid();
        }

        static string ScrubNewLines(string s)
        {
            return s.Trim().Replace("\r\n", "\n").Replace("\r", "");
        }

        static string ScrubStackTrace(string s)
        {
            // Sort of a patch here: it could actually be generalized to more languages

            var withoutStackTrace = s.Split('\n')
                .Where(a => !a.Trim().StartsWith("at "))      // English OS
                .Where(a => !a.Trim().StartsWith("in "));     // Italian OS

            return String.Join("\n", withoutStackTrace).Replace("\r", "");
        }
    }
}
