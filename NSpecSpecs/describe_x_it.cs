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
    public class describe_x_it
    {
        private class SpecClass : nspec
        {
            public void method_level_context()
            {
                xit["should be pending"] = () => { };
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
            PendingExamples().Count().should_be(1);
        }

        [Test]
        public void spec_name_should_reflect_name_specified_in_ActionRegister()
        {
            PendingExamples().First().cast_to<Example>().Spec.should_be("should be pending");
        }

        private IEnumerable<Example> PendingExamples()
        {
            return classContext.Contexts.First().Examples.Where(e => e.Pending);
        }
    }
}
