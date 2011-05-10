using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NSpec;
using NSpec.Domain;
using Rhino.Mocks;

namespace NSpecSpecs.WhenRunningSpecs
{
    [TestFixture]
    [Category("RunningSpecs")]
    public class describe_method_level_examples
    {
        class SpecClass : nspec
        {
            void it_should_be_an_example()
            {
                "hello".should_be("hello");
            }

            void it_should_be_failing()
            {
                "hello".should_not_be("hello");
            }
        }

        [SetUp]
        public void setup()
        {
            reflector = MockRepository.GenerateMock<IReflector>();

            RhinoMocksExtensions.Stub(reflector, r => r.GetTypesFrom("")).IgnoreArguments().Return(new[] { typeof(SpecClass) });

            var contextBuilder = new ContextBuilder(new SpecFinder("", reflector), new DefaultConvention());

            classContext = contextBuilder.Contexts().First();

            classContext.Run();
        }

        [Test]
        public void the_class_context_should_contain_a_class_level_example()
        {
            classContext.Examples.Count.should_be(2);
        }

        [Test]
        public void the_first_example_should_have_been_executed()
        {
            classContext.Examples.First().WasExecuted.should_be_true();
        }

        [Test]
        public void the_last_example_should_have_been_executed()
        {
            classContext.Examples.First().WasExecuted.should_be_true();
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

        private Context classContext;

        private IReflector reflector;
    }
}
