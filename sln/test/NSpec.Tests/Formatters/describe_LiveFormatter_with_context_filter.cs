using System.Collections.Generic;
using System.Reflection;
using NSpec.Domain;
using NSpec.Domain.Formatters;
using NUnit.Framework;
using FluentAssertions;

namespace NSpec.Tests.Formatters
{
    [TestFixture]
    public class describe_LiveFormatter_with_context_filter
    {
        class liveconsole_sample_spec : nspec
        {
            void a_context_with_a_pending_example()
            {
                it["pending example"] = todo;
            }

            void a_context_with_a_grandchild_example()
            {
                context["a context with an example"] = () =>
                {
                    it["liveconsole: 1 is 1"] = () => Assert.That(true, Is.True);
                };
            }

            void a_context_without_an_example() { }
        }

        [SetUp]
        public void Setup()
        {
            formatter = new FormatterStub();

            var invocation = new RunnerInvocation(
                dll: typeof(describe_LiveFormatter_with_context_filter).GetTypeInfo().Assembly.Location,
                tags: typeof(liveconsole_sample_spec).Name,
                formatter: formatter,
                failFast: false);

            contexts = invocation.Run();
        }

        [Test]
        public void it_writes_the_example()
        {
            formatter.WrittenExamples.Should().Contain(e => e.Spec == "liveconsole: 1 is 1");
        }

        [Test]
        public void it_writes_contexts_with_examples()
        {
            formatter.WrittenContexts.Should().Contain(c => c.Name == "a context with an example");
        }

        [Test]
        public void it_writes_context_with_grandchild_examples()
        {
            formatter.WrittenContexts.Should().Contain(c => c.Name == "a context with a grandchild example");
        }

        [Test]
        public void it_skips_contexts_without_examples()
        {
            formatter.WrittenContexts.Should().NotContain(c => c.Name == "a context without an example");
        }

        [Test]
        public void it_skips_contexts_that_were_not_included()
        {
            formatter.WrittenContexts.Should().NotContain(c => c.Name == "SampleSpec");
        }

        [Test]
        public void it_skips_examples_whose_contexts_were_not_included()
        {
            formatter.WrittenExamples.Should().NotContain(e => e.Spec == "an excluded example by ancestry");
        }

        [Test]
        public void it_writes_the_pending_example()
        {
            formatter.WrittenExamples.Should().Contain(e => e.Spec == "pending example");
        }

        FormatterStub formatter;
        ContextCollection contexts;
    }
}