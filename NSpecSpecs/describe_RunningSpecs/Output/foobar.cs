using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Diagnostics;
using NSpec;
using System.Reflection;
using System.IO;

namespace NSpecSpecs.describe_RunningSpecs.Output
{
    [TestFixture]
    public class describe_before
    {
        [Test]
        public void output_verification()
        {
            RunSample("describe_before").output_is(@"
                describe before
                  they run before each example
                    number should be 2
                    number should be 1

                2 Examples, 0 Failed, 0 Pending
                ");
        }

        public string RunSample(string tag)
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

    public static class TestOutputExtensions
    {
        public static void output_is(this string output, string expected)
        {
            expected = expected.Replace("\t", "    ");

            var leadingSpaces = expected.Replace(Environment.NewLine, "").IndexOfAny("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToArray());

            expected = expected.EachLine().Select(s => s.Length >= leadingSpaces ? s.Remove(0, leadingSpaces) : s).Join();

            output.should_be(expected);
        }

        public static IEnumerable<string> EachLine(this string s)
        {
            return s.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        }

        public static string Join(this IEnumerable<string> strings)
        {
            return string.Join(Environment.NewLine, strings);
        }
    }
}
