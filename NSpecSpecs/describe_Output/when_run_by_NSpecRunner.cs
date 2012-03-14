using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace NSpecSpecs.describe_Output
{
    public class when_run_by_NSpecRunner 
    {
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