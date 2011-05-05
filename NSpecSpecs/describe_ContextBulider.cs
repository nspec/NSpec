using System.Linq;
using NSpec;
using NSpec.Domain;
using NUnit.Framework;
using Rhino.Mocks;

namespace NSpecNUnit.when_building_contexts
{
    [TestFixture]
    public class when_building_contexts
    {
        public class child : parent { }
        public class sibling : parent { }
        public class parent : nspec { }

        [SetUp]
        public void setup()
        { 
            finder = MockRepository.GenerateMock<ISpecFinder>();

            finder.Stub(f => f.SpecClasses()).IgnoreArguments().Return(new[] { typeof(child), typeof(parent), typeof(sibling) });

            builder = new ContextBuilder(finder);

            builder.Contexts();
        }

        [Test]
        public void should_get_specs_from_specFinder()
        {
            finder.AssertWasCalled(f => f.SpecClasses());
        }

        [Test]
        public void the_primary_context_should_be_parent()
        {
            builder.Contexts().First().Name.should_be(typeof(parent).Name);
        }

        [Test]
        public void the_parent_should_have_the_child_context()
        {
            builder.Contexts().First().Contexts.First().Name.should_be(typeof(child).Name);
        }

        [Test]
        public void it_should_only_have_the_parent_once()
        {
            builder.Contexts().Count().should_be(1);
        }

        [Test]
        public void it_should_have_the_sibling()
        {
            builder.Contexts().First().Contexts.should_contain(c=>c.Name==typeof(sibling).Name);
        }

        private ISpecFinder finder;
        private ContextBuilder builder;
    }

    [TestFixture]
    public class when_finding_method_level_examples
    {
        class class_with_method_level_example : nspec
        {
            void it_should_be_considered_an_example()
            {

            }

            void specify_should_be_considered_as_an_example()
            {
                
            }

            void IT_SHOULD_BE_CASE_INSENSITIVE()
            {
                
            }
        }

        [SetUp]
        public void setup()
        {
            finder = MockRepository.GenerateMock<ISpecFinder>();

            finder.Stub(f => f.SpecClasses()).IgnoreArguments().Return(new[] { typeof(class_with_method_level_example) });

            builder = new ContextBuilder(finder);

            builder.Contexts();
        }
        	
        [Test]
        public void should_find_method_level_example_if_the_method_name_starts_with_the_word_IT()
        {
            ShouldContainExample("it should be considered an example");
        }

        [Test]
        public void should_find_method_level_example_if_the_method_starts_with_SPECIFY()
        {
            ShouldContainExample("specify should be considered as an example");
        }

        [Test]
        public void should_match_method_level_example_ignoring_case()
        {
            ShouldContainExample("IT SHOULD BE CASE INSENSITIVE");
        }

        [Test]
        public void should_exclude_methods_that_start_with_ITs_from_child_context()
        {
            builder.Contexts().First().Contexts.Count.should_be(0);
        }

        private void ShouldContainExample(string exampleName)
        {
            builder.Contexts().First().Examples.Any(s => s.Spec == exampleName);
        }

        private ISpecFinder finder;
        private ContextBuilder builder;
    }

    [TestFixture]
    public class when_building_method_contexts
    {
        private Context classContext;

        private class SpecClass : nspec
        {
            public void public_method() { }

            void private_method() { }

            void before_each() { }

            void act_each() { }
        }

        [SetUp]
        public void setup()
        {
            var finder = MockRepository.GenerateMock<ISpecFinder>();

            var builder = new ContextBuilder(finder);

            classContext = new Context("class");

            builder.BuildMethodContexts(classContext, typeof(SpecClass));
        }

        [Test]
        public void it_should_add_the_public_method_as_a_sub_context()
        {
            classContext.Contexts.should_contain(c => c.Name == "public method");
        }

        [Test]
        public void it_should_not_create_a_sub_context_for_the_private_method()
        {
            classContext.Contexts.should_contain(c => c.Name == "private method");
        }

        [Test]
        public void it_should_disregard_method_called_before_each()
        {
            classContext.Contexts.should_not_contain(c => c.Name == "before each");
        }

        [Test]
        public void it_should_disregard_method_called_act_each()
        {
            classContext.Contexts.should_not_contain(c => c.Name == "act each");
        }
    }
}