using System;
using System.Reflection;
using NSpec;
using NSpec.Domain;
using NSpec.Domain.Formatters;
using NUnit.Framework;

namespace NSpecSpecs.ClassContextBug
{
    [TestFixture]
    public class NestContextsTests
    {
        [Test]
        public void debug()
        {
            //the specification class you want to test
            //this can be a regular expression
            var testClassYouWantToDebug = "NSpecSpecs.ClassContextBug.Child";

            //initialize NSpec's specfinder
            var finder = new SpecFinder(
                Assembly.GetExecutingAssembly().Location,
                new Reflector(),
                testClassYouWantToDebug );

            //initialize NSpec's builder
            var builder = new ContextBuilder( finder, new DefaultConventions() );

            //this line runs the tests you specified in the filter
            TestFormatter formatter = new TestFormatter();
            new ContextRunner( builder, formatter ).Run();

            Context child = formatter.Contexts[0].Contexts[1].Contexts[1];
            Assert.That( child.Name, Is.EqualTo( "Child" ) );
            Assert.That( child.Contexts.Count, Is.EqualTo( 1 ) );
            Assert.That( child.Contexts[0].Name, Is.EqualTo( "Child Context" ) );
        }
    }

    public class TestFormatter : IFormatter
    {
        public ContextCollection Contexts { get; set; }

        public void Write( ContextCollection contexts )
        {
            this.Contexts = contexts;
        }
    }
}