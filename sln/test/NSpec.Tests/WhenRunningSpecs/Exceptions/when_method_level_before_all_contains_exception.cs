using System.Linq;
using NSpec.Domain;
using NUnit.Framework;
using FluentAssertions;

namespace NSpec.Tests.WhenRunningSpecs.Exceptions
{
    static class MethodBeforeAllThrows
    {
        public class SpecClass : nspec
        {
            void before_all()
            {
                throw new BeforeAllException();
            }

            void should_fail_this_example()
            {
                it["should fail"] = () => Assert.That("hello", Is.EqualTo("hello"));
            }

            void should_also_fail_this_example()
            {
                it["should also fail"] = () => Assert.That("hello", Is.EqualTo("hello"));
            }
        }

        public class ChildSpecClass : SpecClass
        {
            void it_should_fail_because_of_parent()
            {
                Assert.That(true, Is.True);
            }
        }
    }

    [TestFixture]
    [Category("RunningSpecs")]
    public class when_method_level_before_all_contains_exception : when_running_specs
    {
        [SetUp]
        public void setup()
        {
            Run(typeof(MethodBeforeAllThrows.SpecClass));
        }

        [Test]
        public void the_first_example_should_fail_with_framework_exception()
        {
            var example = classContext.AllExamples().First();

            example.Exception.Should().BeAssignableTo<ExampleFailureException>();
        }

        [Test]
        public void the_second_example_should_fail_with_framework_exception()
        {
            var example = classContext.AllExamples().Skip(1).First();

            example.Exception.Should().BeAssignableTo<ExampleFailureException>();
        }
    }

    [TestFixture]
    [Category("RunningSpecs")]
    public class when_parent_method_level_before_all_contains_exception : when_running_specs
    {
        [SetUp]
        public void setup()
        {
            Run(typeof(MethodBeforeAllThrows.ChildSpecClass));
        }

        [Test]
        public void the_example_level_failure_should_indicate_a_context_failure()
        {
            var example = TheExample("it should fail because of parent");

            example.Exception.Should().BeOfType<ExampleFailureException>();
        }

        [Test]
        public void examples_with_only_before_all_failure_should_fail_because_of_before_all()
        {
            var example = TheExample("it should fail because of parent");

            example.Exception.InnerException.Should().BeOfType<BeforeAllException>();
        }
    }
}
