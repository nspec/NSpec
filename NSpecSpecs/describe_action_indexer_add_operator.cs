using System;
using System.Linq;
using NSpec.Domain.Extensions;
using NUnit.Framework;
using NSpec;
using NSpec.Domain;
using System.Collections.Generic;


namespace NSpecNUnit
{
    [TestFixture]
    public class describe_action_indexer_add_operator
    {
        private class SpecClass : nspec
        {
            public void method_level_context()
            {
                specify = () => "Hello".should_be("Hello");
            }
        }

        private Context classContext;
        private Context methodContext;

        [SetUp]
        public void setup()
        {
            classContext = new Context(typeof(SpecClass));

            var method = typeof(SpecClass).Methods().Single(m => m.Name == "method_level_context");

            methodContext = new Context(method);

            classContext.AddContext(methodContext);

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
