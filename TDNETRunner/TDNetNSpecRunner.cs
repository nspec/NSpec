using System.Reflection;
using TestDriven.Framework;
namespace TDNETRunner
{
    public class TDNetNSpecRunner : ITestRunner
    {
        public TestRunState RunAssembly(ITestListener testListener, Assembly assembly)
        {
            testListener.WriteLine("td.net run assembly",new Category());

            return TestRunState.NoTests;
        }

        public TestRunState RunNamespace(ITestListener testListener, Assembly assembly, string ns)
        {
            testListener.WriteLine("td.net run namespace",new Category());

            return TestRunState.NoTests;
        }

        public TestRunState RunMember(ITestListener testListener, Assembly assembly, MemberInfo member)
        {
            testListener.WriteLine("td.net run member",new Category());

            return TestRunState.NoTests;
        }
    }
}