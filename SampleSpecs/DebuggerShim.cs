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
        var tagOrClassName = "describe_specifications";

        var invocation = new RunnerInvocation(Assembly.GetExecutingAssembly().Location, tagOrClassName);

        var contexts = invocation.Run();

        //assert that there aren't any failures
        contexts.Failures().Count().should_be(0);
    }
}
