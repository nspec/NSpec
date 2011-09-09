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

        //initialize the root context
        var contexts = builder.Contexts();

        //build the tests
        contexts.Build();

        //run the tests that were found
        contexts.Run();

        //print the output
        new ConsoleFormatter().Write(contexts);

        //assert that there aren't any failures
        contexts.Failures().Count().should_be(0);
    }
}
