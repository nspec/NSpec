using System;
using System.Linq;
using NSpec.Domain;
using NSpec.Extensions;
using NUnit.Framework;

namespace NSpecNUnit.Context
{
    [TestFixture]
    public class when_counting_failures
    {
        [Test]
        public void given_nested_contexts_and_the_child_has_a_failure()
        {
            var child = new NSpec.Domain.Context("child"); 

            child.AddExample(new Example("") {Exception = new Exception()});

            var parent = new NSpec.Domain.Context("parent");

            parent.AddContext(child);

            parent.Failures().Count().should_be(1);
        }
    }
}