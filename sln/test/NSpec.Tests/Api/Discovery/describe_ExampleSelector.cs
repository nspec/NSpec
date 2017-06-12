using FluentAssertions;
using NSpec.Api.Discovery;
using NUnit.Framework;
using System.Collections.Generic;

namespace NSpec.Tests.Api.Discovery
{
    [TestFixture]
    [Category("ExampleSelector")]
    public class describe_ExampleSelector
    {
        ExampleSelector selector;

        IEnumerable<DiscoveredExample> actuals;

        readonly string testAssemblyPath;

        public describe_ExampleSelector()
        {
            testAssemblyPath = ApiTestData.testAssemblyPath;
        }

        [SetUp]
        public void setup()
        {
            selector = new ExampleSelector(testAssemblyPath);

            actuals = selector.Start();
        }

        [Test]
        public void it_should_return_discovered_examples()
        {
            var expecteds = ApiTestData.allDiscoveredExamples;

            actuals.ShouldBeEquivalentTo(expecteds);
        }
    }
}
