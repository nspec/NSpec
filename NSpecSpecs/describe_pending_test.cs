using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NSpec;
using NSpec.Domain;
using NSpec.Assertions;
using Rhino.Mocks;

namespace NSpecNUnit
{
    [TestFixture]
    public class describe_pending_example
    {
        private class SpecClass : spec
        {
            public void method_level_context()
            {
                it["should be pending"] = pending;
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
            PendingExamples().Count().should_be(1);
        }

        [Test]
        public void spec_name_should_reflect_name_specified_in_ActionRegister()
        {
            PendingExamples().First().cast_to<Example>().Spec.should_be("should be pending");
        }

        private IEnumerable<object> PendingExamples()
        {
            return classContext.Contexts.First().AllPendings();
        }
    }
}
