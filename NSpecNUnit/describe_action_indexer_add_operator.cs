using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NSpec;
using NSpec.Domain;
using Rhino.Mocks;
using NSpec.Assertions;

namespace NSpecNUnit
{
    [TestFixture]
    public class describe_action_indexer_add_operator
    {
        private class SpecClass : spec
        {
            public void method_level_context()
            {
                it += () => "Hello".should_be("Hello");
            }
        }

        private Context classContext;

        [SetUp]
        public void setup()
        {
            var finder = MockRepository.GenerateMock<ISpecFinder>();

            finder.Stub(f => f.Except).Return(new SpecFinder().Except);

            finder.Stub(s => s.SpecClasses()).Return(new[] { typeof(SpecClass) });

            var builder = new ContextBuilder(finder);

            classContext = new Context("class");

            classContext.Type = typeof(SpecClass);

            builder.BuildMethodContexts(classContext, typeof(SpecClass));

            classContext.Run();
        }

        [Test]
        public void should_contain_pending_test()
        {
            TheExamples().Count().should_be(1);
        }

        [Test]
        public void spec_name_should_reflect_name_specified_in_ActionRegister()
        {
            TheExamples().First().cast_to<Example>().Spec.should_be("Hello should be Hello");
        }

        private IEnumerable<object> TheExamples()
        {
            return classContext.Contexts.First().AllExamples();
        }
    }
}
