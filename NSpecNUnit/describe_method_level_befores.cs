using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NSpec;
using Rhino.Mocks;
using NSpec.Domain;
using NSpec.Extensions;

namespace NSpecNUnit
{
    [TestFixture]
    public class describe_method_level_befores
    {
        private Context classContext;

        private class SpecClass : spec
        {
            public static Action MethodLevelBefore = () => { };
            public static Action SubContextBefore = () => { };

            public void method_level_context()
            {
                before = MethodLevelBefore;

                context["sub context"] = () => 
                {
                    before = SubContextBefore;
                };
            }
        }

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
        public void it_should_set_method_level_before()
        {
            TheMethodLevelContext().Before.should_be(SpecClass.MethodLevelBefore);
        }

        [Test]
        public void it_should_set_before_on_sub_context()
        {
            TheMethodLevelContext().Contexts.First().Before.should_be(SpecClass.SubContextBefore);
        }

        public Context TheMethodLevelContext()
        {
            return classContext.Contexts.First();
        }
    }
}
