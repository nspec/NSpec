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
        TestCase(typeof(describe_expected_exception_output)),
        TestCase(typeof(describe_context_stack_trace_output))]
        public void output_verification(Type output)
        {
            var expectedOutput = output.GetField("Output").GetValue(output);

            var tag = output.Name.Replace("_output", "");

            Run(tag).Is(expectedOutput);
        }

        public string Run(string tag)
        {
            var process = new Process();

            var currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase.Replace("file:///", "").Replace("/", @"\"));

            var testDllPath = @"""" + currentPath + @"\..\..\..\SampleSpecs\bin\Debug\SampleSpecs.dll""";

            var exePath = @"""" +  currentPath + @"\..\..\..\NSpecRunner\bin\Debug\NSpecRunner.exe""";

            process.StartInfo = new ProcessStartInfo
                                    {
                                        FileName = exePath,
                                        Arguments = testDllPath + " --tag " + tag,
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

            return output;
        }
    }
}