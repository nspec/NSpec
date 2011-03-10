using System.Linq;
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
        private ContextBuilder builder;

        private class SpecClass : spec
        {
            public void public_method() { }
            private void private_method() { }
        }

        [SetUp]
        public void setup()
        {
            var finder = MockRepository.GenerateMock<ISpecFinder>();

            finder.Stub(f => f.SpecClasses()).Return(new[] { typeof(SpecClass) });

            finder.Stub(f => f.Except).Return(new SpecFinder().Except);

            builder = new ContextBuilder(finder);

            builder.Build();
        }

        [Test]
        public void it_should_create_a_context_for_the_spec_class()
        {
            builder.Contexts.should_contain(c => c.Name == "SpecClass");
        }

        [Test]
        public void it_should_add_the_public_method_as_a_sub_context()
        {
            builder.Contexts.First().Contexts.should_contain(c => c.Name == "public_method");
        }

        [Test]
        public void it_should_not_create_a_sub_context_for_the_private_method()
        {
            builder.Contexts.should_not_contain(c => c.Name == "private_method");
        }
    }
}