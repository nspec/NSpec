using NSpec;
using NSpec.Domain;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpecSpecs.describe_RunningSpecs.Exceptions
{
    [TestFixture]
    [Category("RunningSpecs")]
    public class describe_unexpected_exception_in_after : when_running_specs
    {
        private class SpecClass : nspec
        {
            void method_level_context()
            {
                context["When same exception thrown in after"] = () =>
                {
                    before = () => { throw new KnownException(); };

                    it["fails because of same exception thrown again in after"] = expect<KnownException>();

                    after = () => { throw new KnownException(); };
                };

                context["When different exception thrown in after"] = () =>
                {
                    before = () => { throw new KnownException(); };

                    it["fails because of different exception thrown in after"] = expect<KnownException>();

                    after = () => { throw new SomeOtherException(); };
                };
            }
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(SpecClass));
        }

        [Test]
        [Ignore("Exception in after is hidden by previously met expect<>")]
        public void should_fail_because_of_same_exception_in_after()
        {
            var example = TheExample("fails because of same exception thrown again in after");

            example.Exception.should_not_be_null();
            example.Exception.GetType().should_be(typeof(ExampleFailureException));
        }

        [Test]
        [Ignore("Exception in after is hidden by previously met expect<>")]
        public void should_fail_because_of_different_exception_in_after()
        {
            var example = TheExample("fails because of different exception thrown in after");

            example.Exception.should_not_be_null();
            example.Exception.GetType().should_be(typeof(ExampleFailureException));
        }
    }
}
