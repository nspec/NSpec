using System.Reflection;
using NSpec;
using NSpec.Domain;
using NSpec.Domain.Formatters;
using NUnit.Framework;

namespace NSpecSpecs.ClassContextBug
{
    public class NestContextsTests
    {
        public void debug()
        {
            //the specification class you want to test
            //this can be a regular expression
            var testClassYouWantToDebug = "NSpecSpecs.ClassContextBug.Child";

            //initialize NSpec's specfinder
            var finder = new SpecFinder(
                new Reflector(Assembly.GetExecutingAssembly().Location),
                testClassYouWantToDebug);

            //initialize NSpec's builder
            var builder = new ContextBuilder(finder, new DefaultConventions());

            //this line runs the tests you specified in the filter
            var noTagsFilter = new Tags();
            TestFormatter formatter = new TestFormatter();
            new ContextRunner(noTagsFilter, formatter, false).Run(builder.Contexts().Build());

            Context grandParent = formatter.Contexts[0];
            Assert.That(grandParent.Name, Is.EqualTo("Grand Parent"));
            Assert.That(grandParent.Contexts.Count, Is.EqualTo(2));
            Assert.That(grandParent.Contexts[0].Name, Is.EqualTo("Grand Parent Context"));
            Assert.That(grandParent.Contexts[1].Name, Is.EqualTo("Parent"));
            Assert.That(grandParent.Contexts[0].Examples[0].Spec, Is.EqualTo("TestValue should be \"Grand Parent!!!\""));
            Assert.That(grandParent.Contexts[0].Examples[0].Exception, Is.Null);
            Assert.That(grandParent.Contexts[0].Examples[0].Pending, Is.False);

            Context parent = formatter.Contexts[0].Contexts[1];
            Assert.That(parent.Name, Is.EqualTo("Parent"));
            Assert.That(parent.Contexts.Count, Is.EqualTo(2));
            Assert.That(parent.Contexts[0].Name, Is.EqualTo("Parent Context"));
            Assert.That(parent.Contexts[1].Name, Is.EqualTo("Child"));
            Assert.That(parent.Contexts[0].Examples[0].Spec, Is.EqualTo("TestValue should be \"Grand Parent.Parent!!!@@@\""));
            Assert.That(parent.Contexts[0].Examples[0].Exception, Is.Null);
            Assert.That(parent.Contexts[0].Examples[0].Pending, Is.False);

            Context child = formatter.Contexts[0].Contexts[1].Contexts[1];
            Assert.That(child.Name, Is.EqualTo("Child"));
            Assert.That(child.Contexts.Count, Is.EqualTo(1));
            Assert.That(child.Contexts[0].Name, Is.EqualTo("Child Context"));
            Assert.That(child.Contexts[0].Examples[0].Spec, Is.EqualTo("TestValue should be \"Grand Parent.Parent.Child!!!@@@###\""));
            Assert.That(child.Contexts[0].Examples[0].Exception, Is.Null);
            Assert.That(child.Contexts[0].Examples[0].Pending, Is.False);
        }
    }

    public class TestFormatter : IFormatter
    {
        public ContextCollection Contexts { get; set; }

        public void Write(ContextCollection contexts)
        {
            this.Contexts = contexts;
        }
    }
}