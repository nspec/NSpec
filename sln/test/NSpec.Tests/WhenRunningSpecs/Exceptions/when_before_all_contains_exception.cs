using NSpec.Domain;
using NUnit.Framework;
using FluentAssertions;
using System.Collections.Generic;

namespace NSpec.Tests.WhenRunningSpecs.Exceptions
{
    [TestFixture]
    [Category("RunningSpecs")]
    public class when_before_all_contains_exception : when_running_specs
    {
        class BeforeAllThrowsSpecClass : nspec
        {
            void method_level_context()
            {
                beforeAll = () => { throw new BeforeAllException(); };

                // just by its presence, this will enforce tests as it should never be reported
                afterAll = () => { throw new AfterAllException(); };

                it["should fail this example because of beforeAll"] = () =>
                {
                    ExamplesRun.Add("should fail this example because of beforeAll");
                    Assert.That(true, Is.True);
                };

                it["should also fail this example because of beforeAll"] = () =>
                {
                    ExamplesRun.Add("should also fail this example because of beforeAll");
                    Assert.That(true, Is.True);
                };

                it["overrides exception from same level it"] = () =>
                {
                    ExamplesRun.Add("overrides exception from same level it");
                    throw new ItException();
                };

                context["exception thrown by both beforeAll and nested before"] = () =>
                {
                    before = () => { throw new BeforeException(); };

                    it["overrides exception from nested before"] = () =>
                    {
                        ExamplesRun.Add("overrides exception from nested before");
                        Assert.That(true, Is.True);
                    };
                };

                context["exception thrown by both beforeAll and nested act"] = () =>
                {
                    act = () => { throw new ActException(); };

                    it["overrides exception from nested act"] = () =>
                    {
                        ExamplesRun.Add("verrides exception from nested act");
                        Assert.That(true, Is.True);
                    };
                };

                context["exception thrown by both beforeAll and nested it"] = () =>
                {
                    it["overrides exception from nested it"] = () =>
                    {
                        ExamplesRun.Add("overrides exception from nested it");
                        throw new ItException();
                    };
                };

                context["exception thrown by both beforeAll and nested after"] = () =>
                {
                    it["overrides exception from nested after"] = () =>
                    {
                        ExamplesRun.Add("exception thrown by both beforeAll and nested after");
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
            Run(typeof(BeforeAllThrowsSpecClass));
        }

        [Test]
        public void the_example_level_failure_should_indicate_a_context_failure()
        {
            TheExample("should fail this example because of beforeAll")
                .Exception.Should().BeOfType<ExampleFailureException>();
            TheExample("should also fail this example because of beforeAll")
                .Exception.Should().BeOfType<ExampleFailureException>();
            TheExample("overrides exception from same level it")
                .Exception.Should().BeOfType<ExampleFailureException>();
            TheExample("overrides exception from nested before")
                .Exception.Should().BeOfType<ExampleFailureException>();
            TheExample("overrides exception from nested act")
                .Exception.Should().BeOfType<ExampleFailureException>();
            TheExample("overrides exception from nested it")
                .Exception.Should().BeOfType<ExampleFailureException>();
            TheExample("overrides exception from nested after")
                .Exception.Should().BeOfType<ExampleFailureException>();
        }

        [Test]
        public void examples_with_only_before_all_failure_should_fail_because_of_before_all()
        {
            TheExample("should fail this example because of beforeAll")
                .Exception.InnerException.Should().BeOfType<BeforeAllException>();
            TheExample("should also fail this example because of beforeAll")
                .Exception.InnerException.Should().BeOfType<BeforeAllException>();
        }

        [Test]
        public void it_should_throw_exception_from_before_all_not_from_same_level_it()
        {
            TheExample("overrides exception from same level it")
                .Exception.InnerException.Should().BeOfType<BeforeAllException>();
        }

        [Test]
        public void it_should_throw_exception_from_before_all_not_from_nested_before()
        {
            TheExample("overrides exception from nested before")
                .Exception.InnerException.Should().BeOfType<BeforeAllException>();
        }

        [Test]
        public void it_should_throw_exception_from_before_all_not_from_nested_act()
        {
            TheExample("overrides exception from nested act")
                .Exception.InnerException.Should().BeOfType<BeforeAllException>();
        }

        [Test]
        public void it_should_throw_exception_from_before_all_not_from_nested_it()
        {
            TheExample("overrides exception from nested it")
                .Exception.InnerException.Should().BeOfType<BeforeAllException>();
        }

        [Test]
        public void it_should_throw_exception_from_before_all_not_from_nested_after()
        {
            TheExample("overrides exception from nested after")
                .Exception.InnerException.Should().BeOfType<BeforeAllException>();
        }

        [Test]
        public void examples_should_fail_for_formatter()
        {
            formatter.WrittenExamples.Should().OnlyContain(e => e.Failed);
        }

        [Test]
        public void examples_body_should_not_run()
        {
            BeforeAllThrowsSpecClass.ExamplesRun.Should().BeEmpty();
        }
    }
}
