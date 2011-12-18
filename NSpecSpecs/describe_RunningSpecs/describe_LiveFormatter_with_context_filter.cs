using System.Collections.Generic;
using System.Reflection;
using NSpec;
using NSpec.Domain;
using NSpec.Domain.Formatters;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;

namespace NSpecSpecs.describe_RunningSpecs
{
    [TestFixture]
    public class describe_LiveFormatter_with_context_filter : when_running_specs
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
                    it["1 is 1"] = () => 1.Is(1);
                };
            }

            void a_context_without_an_example() { }
        }

        [SetUp]
        public void Setup()
        {
            formatter = new FormatterStub();

            var finder = new SpecFinder(Assembly.GetExecutingAssembly().Location, new Reflector());

            var invocation = new RunnerInvocation(typeof(liveconsole_sample_spec).Name, formatter, finder, false);

            contexts = invocation.Runner().Run(invocation.Builder().Contexts().Build());
        }

        [Test]
        public void it_writes_the_example()
        {
            formatter.WrittenExamples.should_contain(contexts.FindExample("1 is 1"));
        }

        [Test]
        public void it_writes_contexts_with_examples()
        {
            formatter.WrittenContexts.should_contain(contexts.Find("a context with an example"));
        }

        [Test]
        public void it_writes_context_with_grandchild_examples()
        {
            formatter.WrittenContexts.should_contain(contexts.Find("a context with a grandchild example"));
        }

        [Test]
        public void it_skips_contexts_without_examples()
        {
            formatter.WrittenContexts.should_not_contain(c => c.Name == "a context without an example");
        }

        [Test]
        public void it_skips_contexts_that_were_not_included()
        {
            formatter.WrittenContexts.should_not_contain(c => c.Name == "SampleSpec");
        }

        [Test]
        public void it_skips_examples_whose_contexts_were_not_included()
        {
            formatter.WrittenExamples.should_not_contain(e => e.Spec == "an excluded example by ancestry");
        }

        [Test]
        public void it_writes_the_pending_example()
        {
            formatter.WrittenExamples.should_contain(contexts.FindExample("pending example"));
        }

        protected FormatterStub formatter;
    }

    public class FormatterStub : IFormatter, ILiveFormatter
    {
        public List<Context> WrittenContexts;
        public List<Example> WrittenExamples;

        public FormatterStub()
        {
            WrittenContexts = new List<Context>();
            WrittenExamples = new List<Example>();
        }

        public void Write(ContextCollection contexts)
        {
        }

        public void Write(Context context)
        {
            WrittenContexts.Add(context);
        }

        public void Write(Example example, int level)
        {
            WrittenExamples.Add(example);
        }
    }
}