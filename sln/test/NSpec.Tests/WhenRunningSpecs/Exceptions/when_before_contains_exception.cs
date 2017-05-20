using NSpec.Domain;
using NUnit.Framework;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;

namespace NSpec.Tests.WhenRunningSpecs.Exceptions
{
    [TestFixture]
    [Category("RunningSpecs")]
    public class when_before_contains_exception : when_running_specs
    {
        class BeforeThrowsSpecClass : nspec
        {
            void method_level_context()
            {
                before = () => { throw new BeforeException(); };

                it["should fail this example because of before"] = () =>
                {
                    ExamplesRun.Add("should fail this example because of before");
                    Assert.That(true, Is.True);
                };

                it["should also fail this example because of before"] = () =>
                {
                    ExamplesRun.Add("should also fail this example because of before");
                    Assert.That(true, Is.True);
                };

                it["overrides exception from same level it"] = () =>
                {
                    ExamplesRun.Add("overrides exception from same level it");
                    throw new ItException();
                };

                context["exception thrown by both before and nested before"] = () =>
                {
                    before = () => { throw new NestedBeforeException(); };

                    it["overrides exception from nested before"] = () =>
                    {
                        ExamplesRun.Add("overrides exception from nested before");
                        Assert.That(true, Is.True);
                    };
                };

                context["exception thrown by both before and nested act"] = () =>
                {
                    act = () => { throw new ActException(); };

                    it["overrides exception from nested act"] = () =>
                    {
                        ExamplesRun.Add("overrides exception from nested act");
                        Assert.That(true, Is.True);
                    };
                };

                context["exception thrown by both before and nested it"] = () =>
                {
                    it["overrides exception from nested it"] = () =>
                    {
                        ExamplesRun.Add("overrides exception from nested it");
                        throw new ItException();
                    };
                };

                context["exception thrown by both before and nested after"] = () =>
                {
                    it["overrides exception from nested after"] = () =>
                    {
                        ExamplesRun.Add("overrides exception from nested after");
                        Assert.That(true, Is.True);
                    };

                    after = () => { throw new AfterException(); };
                };
            }

            public static List<string> ExamplesRun = new List<string>();
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(BeforeThrowsSpecClass));
        }

        [Test]
        public void the_example_level_failure_should_indicate_a_context_failure()
        {
            classContext.AllExamples().Should().OnlyContain(e => e.Exception is ExampleFailureException);
        }

        [Test]
        public void examples_with_only_before_failure_should_fail_because_of_before()
        {
            classContext.AllExamples()
                .Where(e => new []
                {
                    "should fail this example because of before",
                    "should also fail this example because of before",
                }.Contains(e.Spec))
                .Should().OnlyContain(e => e.Exception.InnerException is BeforeException);
        }

        [Test]
        public void it_should_throw_exception_from_before_not_from_same_level_it()
        {
            TheExample("overrides exception from same level it")
                .Exception.InnerException.Should().BeOfType<BeforeException>();
        }

        [Test]
        public void it_should_throw_exception_from_before_not_from_nested_before()
        {
            TheExample("overrides exception from nested before")
                .Exception.InnerException.Should().BeOfType<BeforeException>();
        }

        [Test]
        public void it_should_throw_exception_from_before_not_from_nested_act()
        {
            TheExample("overrides exception from nested act")
                .Exception.InnerException.Should().BeOfType<BeforeException>();
        }

        [Test]
        public void it_should_throw_exception_from_before_not_from_nested_it()
        {
            TheExample("overrides exception from nested it")
                .Exception.InnerException.Should().BeOfType<BeforeException>();
        }

        [Test]
        public void it_should_throw_exception_from_before_not_from_nested_after()
        {
            TheExample("overrides exception from nested after")
                .Exception.InnerException.Should().BeOfType<BeforeException>();
        }

        [Test]
        public void examples_should_fail_for_formatter()
        {
            formatter.WrittenExamples.Should().OnlyContain(e => e.Failed);
        }

        [Test]
        public void examples_body_should_not_run()
        {
            BeforeThrowsSpecClass.ExamplesRun.Should().BeEmpty();
        }
    }
}
