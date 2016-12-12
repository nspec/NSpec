using System.Linq;
using NSpec;
using NUnit.Framework;
using FluentAssertions;

namespace NSpecSpecs.WhenRunningSpecs
{
    [TestFixture]
    [Category("RunningSpecs")]
    [Category("Pending")]
    public class describe_xdescribe : when_running_specs
    {
        class SpecClass : nspec
        {
            void method_level_context()
            {
                xdescribe["sub context"] = () =>
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
}
