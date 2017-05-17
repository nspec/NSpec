using NSpec.Domain;
using NUnit.Framework;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;

namespace NSpec.Tests.WhenRunningSpecs.Exceptions
{
    [TestFixture]
    [Category("RunningSpecs")]
    public class when_after_contains_exception : when_running_specs
    {
        class AfterThrowsSpecClass : nspec
        {
            void method_level_context()
            {
                after = () => { throw new AfterException(); };

                it["should fail this example because of after"] = () =>
                {
                    ExamplesRun.Add("should fail this example because of after");
                    Assert.That(true, Is.True);
                };

                it["should also fail this example because of after"] = () =>
                {
                    ExamplesRun.Add("should also fail this example because of after");
                    Assert.That(true, Is.True);
                };

                it["preserves exception from same level it"] = () =>
                {
                    ExamplesRun.Add("preserves exception from same level it");
                    throw new ItException();
                };

                context["exception thrown by both after and nested before"] = () =>
                {
                    before = () => { throw new BeforeException(); };

                    it["preserves exception from nested before"] = () =>
                    {
                        ExamplesRun.Add("preserves exception from nested before");
                        Assert.That(true, Is.True);
                    };
                };

                context["exception thrown by both after and nested act"] = () =>
                {
                    act = () => { throw new ActException(); };

                    it["preserves exception from nested act"] = () =>
                    {
                        ExamplesRun.Add("preserves exception from nested act");
                        Assert.That(true, Is.True);
                    };
                };

                context["exception thrown by both after and nested it"] = () =>
                {
                    it["preserves exception from nested it"] = () => 
                    {
                        ExamplesRun.Add("preserves exception from nested it");
                        throw new ItException();
                    };
                };

                context["exception thrown by both after and nested after"] = () =>
                {
                    it["preserves exception from nested after"] = () =>
                    {
                        ExamplesRun.Add("preserves exception from nested after");
                        Assert.That(true, Is.True);
                    };

                    after = () => { throw new NestedAfterException(); };
                };
            }

            public static List<string> ExamplesRun = new List<string>();
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(AfterThrowsSpecClass));
        }

        [Test]
        public void the_example_level_failure_should_indicate_a_context_failure()
        {
            classContext.AllExamples().Should().OnlyContain(e => e.Exception is ExampleFailureException);
        }

        [Test]
        public void examples_with_only_after_failure_should_fail_because_of_after()
        {
            classContext.AllExamples()
                .Where(e => new []
                {
                    "should fail this example because of after",
                    "should also fail this example because of after",
                }.Contains(e.Spec))
                .Should().OnlyContain(e => e.Exception.InnerException is AfterException);
        }

        [Test]
        public void it_should_throw_exception_from_same_level_it_not_from_after()
        {
            TheExample("preserves exception from same level it")
                .Exception.InnerException.Should().BeOfType<ItException>();
        }

        [Test]
        public void it_should_throw_exception_from_nested_before_not_from_after()
        {
            TheExample("preserves exception from nested before")
                .Exception.InnerException.Should().BeOfType<BeforeException>();
        }

        [Test]
        public void it_should_throw_exception_from_nested_act_not_from_after()
        {
            TheExample("preserves exception from nested act")
                .Exception.InnerException.Should().BeOfType<ActException>();
        }

        [Test]
        public void it_should_throw_exception_from_nested_it_not_from_after()
        {
            TheExample("preserves exception from nested it")
                .Exception.InnerException.Should().BeOfType<ItException>();
        }

        [Test]
        public void it_should_throw_exception_from_nested_after_not_from_after()
        {
            TheExample("preserves exception from nested after")
                .Exception.InnerException.Should().BeOfType<NestedAfterException>();
        }

        [Test]
        public void examples_should_fail_for_formatter()
        {
            formatter.WrittenExamples.Should().OnlyContain(e => e.Failed);
        }

        [Test]
        public void examples_body_should_still_run()
        {
            string[] expecteds = new[]
            {
                "should fail this example because of after",
                "should also fail this example because of after",
                "preserves exception from same level it",
                "preserves exception from nested it",
                "preserves exception from nested after",
            };

            AfterThrowsSpecClass.ExamplesRun.ShouldBeEquivalentTo(expecteds);
        }
    }
}
