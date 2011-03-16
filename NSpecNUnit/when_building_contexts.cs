using NSpec;
using NSpec.Domain;
using NSpec.Extensions;
using NUnit.Framework;
using Rhino.Mocks;

namespace NSpecNUnit
{
    [TestFixture]
    public class when_building_contexts
    {
        private Context classContext;

        private class SpecClass : spec
        {
            public void public_method() { }
            private void private_method() { }
        }

        [SetUp]
        public void setup()
        {
            var finder = MockRepository.GenerateMock<ISpecFinder>();

            finder.Stub(f => f.Except).Return(new SpecFinder().Except);

            var builder = new ContextBuilder(finder);

            classContext = new Context("class");

            builder.BuildMethodContexts(classContext,typeof(SpecClass));
        }

        [Test]
        public void it_should_add_the_public_method_as_a_sub_context()
        {
            classContext.Contexts.should_contain(c => c.Name == "public_method");
        }

        [Test]
        public void it_should_not_create_a_sub_context_for_the_private_method()
        {
            classContext.Contexts.should_not_contain(c => c.Name == "private_method");
        }
    }
}