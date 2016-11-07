using FluentAssertions;
using NSpec;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;

namespace NSpecSpecs.describe_RunningSpecs
{
    public class describe_Levels_Inheritance : when_running_specs
    {
        class parent_context : nspec { }

        class child_context : parent_context
        {
            void it_is()
            {
                Assert.That("is", Is.EqualTo("is"));
            }
        }

        [SetUp]
        public void Setup()
        {
            Run(new[] { typeof(parent_context), typeof(child_context) });
        }

        [Test]
        public void parent_class_is_level_1()
        {
            TheContext("parent context").Level.Should().Be(1);
        }

        [Test]
        public void child_class_is_level_2()
        {
            TheContext("child context").Level.Should().Be(2);
        }
    }
}