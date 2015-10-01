using System.Linq;
using NSpec;
using NSpec.Domain;
using NSpec.Domain.Formatters;
using NUnit.Framework;
using Rhino.Mocks;
using System.Threading.Tasks;
using System;

namespace NSpecSpecs.WhenRunningSpecs
{
    [TestFixture]
    [Category("RunningSpecs")]
    public class describe_method_level_examples : describe_method_level_examples_common_cases
    {
        class SpecClass : nspec
        {
            public static bool first_example_executed, last_example_executed;

            void it_should_be_an_example()
            {
                first_example_executed = true;
                "hello".should_be("hello");
            }

            void it_should_be_failing()
            {
                last_example_executed = true;
                "hello".should_not_be("hello");
            }
        }

        [SetUp]
        public void setup()
        {
            RunWithReflector(typeof(SpecClass));
        }

        protected override bool FirstExampleExecuted { get { return SpecClass.first_example_executed; } }
        protected override bool LastExampleExecuted { get { return SpecClass.last_example_executed; } }
    }

    public abstract class describe_method_level_examples_common_cases : when_running_method_level_examples
    {
        protected abstract bool FirstExampleExecuted { get; }
        protected abstract bool LastExampleExecuted { get; }

        [Test]
        public void the_class_context_should_contain_a_class_level_example()
        {
            classContext.Examples.Count.should_be(2);
        }

        [Test]
        public void there_should_be_only_one_failure()
        {
            classContext.Failures().Count().should_be(1);
        }

        [Test]
        public void should_execute_first_example()
        {
            FirstExampleExecuted.should_be_true();
        }

        [Test]
        public void should_execute_last_example()
        {
            LastExampleExecuted.should_be_true();
        }

        [Test]
        public void the_last_example_should_be_failing()
        {
            classContext.Examples.Last().Exception.should_cast_to<AssertionException>();
        }

        [Test]
        public void the_stack_trace_for_last_example_should_be_the_the_original_stack_trace()
        {
            classContext.Examples.Last().Exception.StackTrace.should_not_match("^.*at NSpec.Domain.Example");
        }
    }

    public abstract class when_running_method_level_examples
    {
        protected void RunWithReflector(Type specClassType)
        {
            IReflector reflector = MockRepository.GenerateMock<IReflector>();

            reflector.Stub(r => r.GetTypesFrom()).Return(new[] { specClassType });

            var contextBuilder = new ContextBuilder(new SpecFinder(reflector), new DefaultConventions());

            classContext = contextBuilder.Contexts().First();

            classContext.Build();

            classContext.Run(new SilentLiveFormatter(), failFast: false);
        }

        protected Context classContext;
    }
}
