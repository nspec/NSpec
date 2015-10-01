using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSpec;
using NUnit.Framework;
using NSpecSpecs.WhenRunningSpecs;
using System.Threading.Tasks;

namespace NSpecSpecs.describe_RunningSpecs
{
    [TestFixture]
    public class describe_overriding_exception : when_running_specs
    {
        class SpecClass : nspec
        {
            void before_each()
            {
                throw new InvalidOperationException("Exception to replace.");
            }

            void specify_method_level_failure()
            {
                "1".should_be("1");
            }

            async Task specify_async_method_level_failure()
            {
                await Task.Delay(0);

                "1".should_be("1");
            }

            public override Exception ExceptionToReturn(Exception originalException)
            {
                return new ArgumentException("Redefined exception.", originalException);
            }
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(SpecClass));
        }

        [Test]
        public void the_examples_exception_is_replaced_with_exception_provided_in_override()
        {
            TheExample("specify method level failure").Exception.InnerException.GetType().should_be(typeof(ArgumentException));
        }

        [Test]
        public void the_examples_exception_is_replaced_with_exception_provided_in_override_if_async_method()
        {
            TheExample("specify async method level failure").Exception.InnerException.GetType().should_be(typeof(ArgumentException));
        }
    }
}
