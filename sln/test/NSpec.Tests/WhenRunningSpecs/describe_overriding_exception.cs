using System;
using NUnit.Framework;
using System.Threading.Tasks;
using NSpec.Tests.WhenRunningSpecs.Exceptions;
using FluentAssertions;

namespace NSpec.Tests.WhenRunningSpecs
{
    [TestFixture]
    public class describe_overriding_exception : when_running_specs
    {
        class SpecClass : nspec
        {
            void before_each()
            {
                throw new SomeOtherException("Exception to replace.");
            }

            void specify_method_level_failure()
            {
                Assert.That(true, Is.True);
            }

            async Task specify_async_method_level_failure()
            {
                await Task.Delay(0);

                Assert.That(true, Is.True);
            }

            public override Exception ExceptionToReturn(Exception originalException)
            {
                return new KnownException("Redefined exception.", originalException);
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
            TheExample("specify method level failure").Exception.InnerException.Should().BeOfType<KnownException>();
        }

        [Test]
        public void the_examples_exception_is_replaced_with_exception_provided_in_override_if_async_method()
        {
            TheExample("specify async method level failure").Exception.InnerException.Should().BeOfType<KnownException>();
        }
    }
}
