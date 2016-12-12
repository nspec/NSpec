using System;
using System.Linq;
using NSpec;
using NUnit.Framework;
using NSpecSpecs.describe_RunningSpecs.Exceptions;
using FluentAssertions;

namespace NSpecSpecs.WhenRunningSpecs
{
    [TestFixture]
    [Category("RunningSpecs")]
    [Category("Pending")]
    public class describe_it_behaviour_in_xcontext : when_running_specs
    {
        class SpecClass : nspec
        {
            void method_level_context()
            {
                xcontext["sub context"] = () =>
                {
                    it["needs an example or it gets filtered"] =
                        () => Assert.That(true, Is.True);
                };
            }
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(SpecClass));
        }

        [Test]
        public void the_example_should_be_pending()
        {
            methodContext.Contexts.First().Examples.First().Pending.Should().Be(true);
        }
    }

    [TestFixture]
    [Category("RunningSpecs")]
    [Category("Pending")]
    public class describe_xcontext : when_running_specs
    {
        class SpecClass : nspec
        {
            public static string output = string.Empty;
            public static Action MethodLevelBefore = () => { throw new KnownException("this should not run."); };
            public static Action SubContextBefore = () => { throw new KnownException("this should not run."); };

            void method_level_context()
            {
                before = MethodLevelBefore;

                xcontext["sub context"] = () =>
                {
                    before = SubContextBefore;

                    it["needs an example or it gets filtered"] =
                        () => Assert.That(true, Is.True);
                };
            }
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(SpecClass));
        }

        [Test]
        public void it_should_not_run_befores_on_pending_context()
        {
            methodContext.AllExamples().First().Exception.Should().Be(null);
        }
    }
}
