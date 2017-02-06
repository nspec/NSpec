using FluentAssertions;
using NSpec.Domain;
using NSpec.Tests.WhenRunningSpecs.Exceptions;
using NUnit.Framework;
using System.Linq;

namespace NSpec.Tests
{
    [TestFixture]
    [Category("ContextCollection")]
    public class describe_ContextCollection
    {
        private ContextCollection contexts;

        [SetUp]
        public void setup()
        {
            contexts = new ContextCollection();

            var context = new Context();

            context.AddExample(new ExampleBaseWrap());

            context.AddExample(new ExampleBaseWrap { Pending = true });

            context.AddExample(new ExampleBaseWrap { Exception = new KnownException() });

            context.Tags.Add(Tags.Focus);

            contexts.Add(context);
        }

        [Test]
        public void should_aggregate_examples()
        {
            contexts.Examples().Count().Should().Be(3);
        }

        [Test]
        public void is_marked_with_focus()
        {
            contexts.AnyTaggedWithFocus().Should().BeTrue();
        }

        [Test]
        public void should_aggregate_failures()
        {
            contexts.Failures().Count().Should().Be(1);
        }

        [Test]
        public void should_aggregate_pendings()
        {
            contexts.Pendings().Count().Should().Be(1);
        }

        [Test]
        public void should_trim_skipped_contexts()
        {
            contexts.Add(new Context());
            contexts[0].AddExample(new ExampleBaseWrap());
            contexts[0].Examples[0].HasRun = true;
            contexts.Count().Should().Be(2);
            contexts.TrimSkippedContexts();
            contexts.Count().Should().Be(1);
        }
    }
}