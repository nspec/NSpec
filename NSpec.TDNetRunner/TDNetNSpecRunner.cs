using System;
using System.Reflection;
using NSpec.Domain;
using NSpec.Domain.Formatters;
using TestDriven.Framework;

namespace NSpec.TDNetRunner
{
    public class TDNetNSpecRunner : ITestRunner
    {
        public TestRunState RunAssembly(ITestListener testListener, Assembly assembly)
        {
            testListener.WriteLine("td.net run assembly", new Category());

            return TestRunState.Success;
        }

        public TestRunState RunNamespace(ITestListener testListener, Assembly assembly, string ns)
        {
            return Run(assembly, ns, testListener);
        }

        private TestRunState Run(Assembly assembly, string filter, ITestListener testListener)
        {
            var finder = new SpecFinder(assembly, new Reflector(), filter);

            var builder = new ContextBuilder(finder, new DefaultConventions());

            var contexts = builder.Contexts();

            contexts.Build();

            contexts.Run();

            var formatter = new ConsoleFormatter(testListener);

            formatter.Write(contexts);

            return contexts.Result();
        }

        public TestRunState RunMember(ITestListener testListener, Assembly assembly, MemberInfo member)
        {
            return Run(assembly, member.Name, testListener);
        }
    }
}