using FluentAssertions;
using NUnit.Framework;

namespace NSpec.Tests.WhenRunningSpecs
{
    public class describe_levels : when_running_specs
    {
        class describe_numbers : nspec
        {
            void method_level_context()
            {
                it["1 is 1"] = () => Assert.That(true, Is.True);

                context["except in crazy world"] = () =>
                {
                    it["1 is 2"] = () => Assert.That(1, Is.EqualTo(2));
                };
            }
        }

        [SetUp]
        public void Setup()
        {
            Run(typeof(describe_numbers));
        }

        [Test]
        public void classes_that_directly_inherit_nspec_have_level_1()
        {
            TheContext("describe numbers").Level.Should().Be(1);
        }

        [Test]
        public void method_level_contexts_have_one_level_deeper()
        {
            TheContext("method level context").Level.Should().Be(2);
        }

        [Test]
        public void and_nested_contexts_one_more_deep()
        {
            TheContext("except in crazy world").Level.Should().Be(3);
        }
    }
}