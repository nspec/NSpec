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
    public class describe_implicit_befores : when_running_specs
    {
        class SpecClass : nspec
        {
            void method_level_context()
            {
                List<int> ints = new List<int>();
                ints.Add(5);

                it["should have two entries"] = () =>
                {
                    ints.Add(16);
                    ints.Count.Should().Be(1);
                };

                specify = () => ints.Count.Should().Be(1, String.Empty);
            }
        }

        [Test, Ignore("It cannot be tested")]
        public void should_give_each_specify_a_new_instance_of_spec()
        {
            Run(typeof(SpecClass));
            Assert.Inconclusive("I dont think this is possible....");
            TheMethodContextExamples().First().ShouldHavePassed();
            TheMethodContextExamples().Last().ShouldHavePassed();
        }

        private IEnumerable<ExampleBase> TheMethodContextExamples()
        {
            return classContext.Contexts.First().AllExamples();
        }
    }
}
