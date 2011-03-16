using System;
using System.Linq;
using NSpec;
using NSpec.Domain;
using NSpec.Extensions;
using NSpec.Interpreter.Indexer;
using NUnit.Framework;
using Rhino.Mocks;

namespace NSpecNUnit.when_building_contexts
{
    public class child : parent{}
    public class sibling: parent{}
    public class parent : spec{}

    [TestFixture]
    public class when_building_contexts
    {
        private ISpecFinder finder;
        private ContextBuilder2 context;

        [SetUp]
        public void setup()
        { 
            finder = MockRepository.GenerateMock<ISpecFinder>();

            finder.Stub(f => f.SpecClasses()).IgnoreArguments().Return(new[] { typeof(child), typeof(parent), typeof(sibling) });

            finder.Stub(f => f.Except).Return(new SpecFinder().Except);

            context = new ContextBuilder2(finder);
        }

        [Test]
        public void should_get_specs_from_specFinder()
        {
            finder.AssertWasCalled(f => f.SpecClasses());
        }

        [Test]
        public void should_pass_the_filter_to_specFinder()
        {
            var filter = "spec_filter";

            context = new ContextBuilder2(finder,filter);

            finder.AssertWasCalled(f=>f.SpecClasses(filter));
        }

        [Test]
        public void the_primary_context_should_be_parent()
        {
            context.First().Name.should_be(typeof(parent).Name);
        }

        [Test]
        public void the_child_should_have_a_context()
        {
            context.First().Contexts.First().Name.should_be(typeof(child).Name);
        }

        [Test]
        public void it_should_only_have_the_parent_once()
        {
            context.Count().should_be(1);
        }

        [Test]
        public void it_should_have_the_sibling()
        {
            context.First().Contexts.should_contain(c=>c.Name==typeof(sibling).Name);
        }
    }
}