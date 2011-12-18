using System;
using NUnit.Framework;
using NSpec.Domain;
using System.Reflection;
using NSpec;
using NSpec.Domain.Formatters;
using System.Linq;

[TestFixture]
public class DebuggerShim
{
    [Test]
    public void debug()
    {
        //the specification class you want to test
        //this can be a regular expression
        var testClassYouWantToDebug = "describe_specifications";

        var finder = new SpecFinder(Assembly.GetExecutingAssembly().Location, new Reflector());

        var invocation = new RunnerInvocation(testClassYouWantToDebug,
                                new SilentLiveFormatter(), finder, false);

        var contexts = invocation.Run();

        //assert that there aren't any failures
        contexts.Failures().Count().should_be(0);
    }
}
