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

        var invocation = new RunnerInvocation(Assembly.GetExecutingAssembly().Location,
                                testClassYouWantToDebug,
                                new SilentLiveFormatter());

        var contexts = invocation.Runner().Run();

        //assert that there aren't any failures
        contexts.Failures().Count().should_be(0);
    }
}
