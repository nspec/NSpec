using System;
using System.Linq;
using NSpec.Assertions;
using NSpec.Domain;
using NSpec.Extensions;
using NUnit.Framework;

namespace NSpecNUnit
{
    [TestFixture]
    public class when_counting_failures
    {
        [Test]
        public void given_nested_contexts_and_the_child_has_a_failure()
        {
            var child = new Context("child"); 

            child.AddExample(new Example("") {Exception = new Exception()});

            var parent = new Context("parent");

            parent.AddContext(child);

            parent.Failures().Count().should_be(1);
        }
    }
}