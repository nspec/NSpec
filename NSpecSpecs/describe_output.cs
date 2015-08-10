using System.Diagnostics;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using System;
using NSpec;
using SampleSpecs.Bug;

namespace NSpecSpecs
{
    [TestFixture]
    public class describe_output
    {
        [Test,
        TestCase(typeof(my_first_spec_output)),
        TestCase(typeof(describe_specifications_output)),
        TestCase(typeof(describe_before_output)),
        TestCase(typeof(describe_contexts_output)),
        TestCase(typeof(describe_pending_output)),
        TestCase(typeof(describe_helpers_output)),
        TestCase(typeof(describe_batman_sound_effects_as_text_output)),
        TestCase(typeof(describe_class_level_output)),
        TestCase(typeof(given_the_sequence_continues_with_2_output)),
        TestCase(typeof(describe_exception_output)),
        TestCase(typeof(describe_context_stack_trace_output)),
        TestCase(typeof(describe_ICollection_output)),
        TestCase(typeof(describe_changing_stacktrace_message_output)),
        TestCase(typeof(describe_changing_failure_exception_output)),
        TestCase(typeof(describe_focus_output))]
        public void output_verification(Type output)
        {
            var expectedOutput = output.GetField("Output").GetValue(output);

            var tag = output.Name.Replace("_output", "");

            //if the example happens to be the focus example, then the focus project
            if (tag == "describe_focus") Run(tag, FullDllPath(@"\..\..\..\SampleSpecsFocus\bin\Debug\SampleSpecsFocus.dll""")).Is(expectedOutput);

            else Run(tag).Is(expectedOutput);
        }

        public string CurrentPath()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase.Replace("file:///", "").Replace("/", @"\"));
        }

        public string FullDllPath(string dllPath)
        {
            return @"""" + CurrentPath() + dllPath;
        }

        public string FullRunnerPath()
        {
            return @"""" + CurrentPath() + @"\..\..\..\NSpecRunner\bin\Debug\NSpecRunner.exe""";
        }

        public string Run(string tag, string testDllPath = null)
        {
            var process = new Process();

            testDllPath = testDllPath ?? FullDllPath(@"\..\..\..\SampleSpecs\bin\Debug\SampleSpecs.dll""");

            var exePath = FullRunnerPath();

            var arguments = testDllPath + " --tag " + tag;

            if(tag == "") arguments = testDllPath;

            process.StartInfo = new ProcessStartInfo
            {
                FileName = exePath,
                Arguments = arguments,
                RedirectStandardInput = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            process.Start();

            process.WaitForExit();

            var output = process.StandardOutput.ReadToEnd();

            return output.RegexReplace("in .*SampleSpecs", "in SampleSpecs");
        }

        [Test]
        public void describe_regex_replace()
        {
            "in c:\\SampleSpecs".RegexReplace("in .*SampleSpecs","in SampleSpecs").Is("in SampleSpecs");
        }
    }
}
