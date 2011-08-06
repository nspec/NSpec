using System;
using NUnit.Framework;
using NSpec.Domain;
using System.Reflection;
using NSpec;
using NSpec.Domain.Formatters;

[TestFixture]
public class DebuggerShim
{
    [Test]
    public void debug()
    {
        //the specification class you want to test
        //this can be a regular expression
        var testClassYouWantToDebug = "describe_SomeTest";

        //initialize NSpec's specfinder
        var finder = new SpecFinder(
            Assembly.GetExecutingAssembly().Location,
            new Reflector(),
            testClassYouWantToDebug);

        //initialize NSpec's builder
        var builder = new ContextBuilder(
            finder,
            new DefaultConventions());

        //this line runs the tests you specified in the filter
        new ContextRunner(builder, new ConsoleFormatter()).Run();
    }
}
