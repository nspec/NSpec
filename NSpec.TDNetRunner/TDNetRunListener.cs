using System;
using NSpec;
using TestDriven.Framework;

namespace NSpec.TDNetRunner
{
    public class TDNetRunListener : ITestListener
    {
        public void WriteLine(string text, Category category)
        {
            Console.WriteLine("writeline {0} {1}".With(text, category));
        }

        public void TestFinished(TestResult summary)
        {
            Console.WriteLine("writeline {0}".With(summary));
        }

        public void TestResultsUrl(string resultsUrl)
        {
            Console.WriteLine("writeline {0}".With(resultsUrl));
        }
    }
}
