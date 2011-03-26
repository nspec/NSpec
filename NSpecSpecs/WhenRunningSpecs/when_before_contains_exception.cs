using System;
using System.Linq;
using NUnit.Framework;
using NSpec;
using NSpec.Domain;

namespace NSpecSpecs.WhenRunningSpecs
{
    [TestFixture]
    public class when_before_contains_exception : when_running_specs
    {
        private class SpecClass : nspec
        {
            public void method_level_context()
            {
                before = () => { throw new InvalidOperationException(); };

                it["should fail this example because of before"] = () => "1".should_be("1");

                it["should also fail this example because of before"] = () => "1".should_be("1");
            }
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(SpecClass));
        }

        [Test]
        public void it_should_fail_all_examples_in_before()
        {
            TheExample("should fail this example because of before").Exception.GetType().should_be(typeof(InvalidOperationException));
            TheExample("should also fail this example because of before").Exception.GetType().should_be(typeof(InvalidOperationException));
        }

        private Example TheExample(string name)
        {
            return classContext.Contexts.First().AllExamples().Single(s => s.Spec == name);
        }
    }
}
