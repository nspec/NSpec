using System.Diagnostics;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using System;
using NSpec;

namespace NSpecSpecs.describe_Output
{
    [TestFixture]
    public class when_run_by_NSpecRunner 
    {
        [Test, 
        TestCase(typeof(describe_before_expected))]
        public void output_verification(Type output)
        {
            var expectedOutput = output.GetField("Output").GetValue(output);

            var tag = output.Name.Replace("_expected", "");

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