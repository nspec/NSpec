using FluentAssertions;
using NSpec.Domain;
using NUnit.Framework;
using System;

namespace NSpecSpecs
{
    [TestFixture]
    [Category("Example")]
    public class when_parsing_expressions
    {
        [Test]
        public void should_clear_quotes()
        {
            new Example(() => Assert.That("hello", Is.EqualTo("hello"))).Spec
                .Should().Be("That hello, EqualTo hello");
        }

        // no 'specify' available for AsyncExample, hence no way to test that on AsyncExample
    }

    [TestFixture]
    [Category("Example")]
    public class describe_ExampleBase
    {
        [Test]
        public void should_concatenate_its_contexts_name_into_a_full_name()
        {
            var context = new Context("context name");

            var example = new ExampleBaseWrap("example name");

            context.AddExample(example);

            example.FullName().Should().Be("context name. example name.");
        }

        [Test]
        public void should_be_marked_as_pending_if_parent_context_is_pending()
        {
            var context = new Context("pending context", null, isPending: true);

            var example = new ExampleBaseWrap("example name");

            context.AddExample(example);

            example.Pending.Should().BeTrue();
        }

        [Test]
        public void should_be_marked_as_pending_if_any_parent_context_is_pending()
        {
            var parentContext = new Context("parent pending context", null, isPending: true);
            var context = new Context("not pending");
            var example = new ExampleBaseWrap("example name");

            parentContext.AddContext(context);

            context.AddExample(example);

            example.Pending.Should().BeTrue();
        }
    }
}