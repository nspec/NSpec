using System.Collections.Generic;
using System.Linq;
using NSpec;
using NSpec.Domain;
using NUnit.Framework;
using FluentAssertions;
using System;

namespace NSpecSpecs.WhenRunningSpecs
{
    [TestFixture]
    [Category("RunningSpecs")]
    public class describe_action_indexer_add_operator : when_running_specs
    {
        private class SpecClass : nspec
        {
            void method_level_context()
            {
                it["Should have this name"] = () => Assert.That(true, Is.True);
            }
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(SpecClass));
        }

        [Test]
        public void should_contain_pending_test()
        {
            TheExamples().Count().Should().Be(1);
        }

        [Test]
        public void spec_name_should_reflect_name_specified_in_ActionRegister()
        {
            TheExamples().First().Should().BeAssignableTo<Example>();

            var example = (Example)TheExamples().First();

            example.Spec.Should().Be("Should have this name");
        }

        // no 'specify' available for AsyncExample, hence no need to test that on ExampleBase

        private IEnumerable<object> TheExamples()
        {
            return classContext.Contexts.First().AllExamples();
        }
    }
}
