using System.Linq;
using NSpec;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;
using FluentAssertions;
using System;

namespace NSpecSpecs.describe_RunningSpecs
{
    [TestFixture]
    public class describe_skipped_before_alls_when_excluded_by_tag : when_running_specs
    {
        class InnocentBystander : nspec
        {
            public static string sequence = "";

            void before_all()
            {
                sequence = "should not run innocent bystander before_all";
            }

            void context_bystander()
            {
                it["should not run because of tags"] = () => "not tagged".Should().Be("not tagged");
            }
        }

        class Target : nspec
        {
            void it_specifies_something()
            {
                specify = () => true.Should().BeTrue(String.Empty);
            }
        }

        [SetUp]
        public void Setup()
        {
            tags = "Target";
            Run(typeof(Target), typeof(InnocentBystander));
        }

        [Test]
        public void should_skip_innocent_bystander_before_all()
        {
            InnocentBystander.sequence.Should().Be("");
        }
    }

    [TestFixture]
    public class describe_skipped_after_alls_when_excluded_by_tag : when_running_specs
    {
        class InnocentBystander : nspec
        {
            public static string sequence = "";

            void context_bystander()
            {
                it["should not run because of tags"] = () => "not tagged".Should().Be("not tagged");
            }

            void after_all()
            {
                sequence = "should not run innocent bystander after_all";
            }
        }

        class Target : nspec
        {
            void it_specifies_something()
            {
                specify = () => true.Should().BeTrue(String.Empty);
            }
        }

        [SetUp]
        public void Setup()
        {
            tags = "Target";
            Run(typeof(Target), typeof(InnocentBystander));
        }

        [Test]
        public void should_skip_innocent_bystander_after_all()
        {
            InnocentBystander.sequence.Should().Be("");
        }
    }
}
