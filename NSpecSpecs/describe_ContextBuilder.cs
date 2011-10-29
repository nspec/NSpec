using System.Linq;
using NSpec;
using NSpec.Domain;
using NUnit.Framework;
using Rhino.Mocks;
using System.Collections.Generic;
using System;
using NSpecSpecs;

namespace NSpecNUnit.when_building_contexts
{
    [TestFixture]
    public class describe_ContextBuilder
    {
        protected ISpecFinder finder;

        protected ContextBuilder builder;

        protected List<Type> typesForFinder;

        protected List<Context> contexts;

        [SetUp]
        public void setup_base()
        {
            finder = MockRepository.GenerateMock<ISpecFinder>();

            typesForFinder = new List<Type>();

            finder.Stub(f => f.SpecClasses()).IgnoreArguments().Return(typesForFinder);

            DefaultConventions conventions = new DefaultConventions();

            conventions.Initialize();

            builder = new ContextBuilder(finder, conventions);
        }

        public void GivenTypes(params Type[] types)
        {
            typesForFinder.AddRange(types);
        }

        public IList<Context> TheContexts()
        {
            return builder.Contexts();
        }
    }

    [TestFixture]
    [Category("ContextBuilder")]
    public class when_building_contexts : describe_ContextBuilder
    {
        public class child : parent { }

        public class sibling : parent { }

        public class parent : nspec { }

        [SetUp]
        public void setup()
        {
            GivenTypes(typeof(child), typeof(sibling), typeof(parent));

            TheContexts();
        }

        [Test]
        public void should_get_specs_from_specFinder()
        {
            finder.AssertWasCalled(f => f.SpecClasses());
        }

        [Test]
        public void the_primary_context_should_be_parent()
        {
            TheContexts().First().Name.should_be(typeof(parent).Name);
        }

        [Test]
        public void the_parent_should_have_the_child_context()
        {
            TheContexts().First().Contexts.First().Name.should_be(typeof(child).Name);
        }

        [Test]
        public void it_should_only_have_the_parent_once()
        {
            TheContexts().Count().should_be(1);
        }

        [Test]
        public void it_should_have_the_sibling()
        {
            TheContexts().First().Contexts.should_contain(c => c.Name == typeof(sibling).Name);
        }

    }

    [TestFixture]
    [Category("ContextBuilder")]
    public class when_finding_method_level_examples : describe_ContextBuilder
    {
        class class_with_method_level_example : nspec
        {
            void it_should_be_considered_an_example() { }

            void specify_should_be_considered_as_an_example() { }
        }

        [SetUp]
        public void setup()
        {
            GivenTypes(typeof(class_with_method_level_example));
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
        public void should_exclude_methods_that_start_with_ITs_from_child_context()
        {
            TheContexts().First().Contexts.Count.should_be(0);
        }

        private void ShouldContainExample(string exampleName)
        {
            TheContexts().First().Examples.Any(s => s.Spec == exampleName);
        }
    }

    [TestFixture]
    [Category("ContextBuilder")]
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

            DefaultConventions defaultConvention = new DefaultConventions();

            defaultConvention.Initialize();

            var builder = new ContextBuilder(finder, defaultConvention);

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

    [TestFixture]
    [Category("ContextBuilder")]
    public class when_building_class_and_method_contexts_with_tag_attributes : describe_ContextBuilder
    {
        [Tag("@class-tag")]
        class SpecClass : nspec
        {
            [Tag("@method-tag")]
            void public_method() { }
        }

        [SetUp]
        public void setup()
        {
            GivenTypes(typeof(SpecClass));
        }

        [Test]
        public void it_should_tag_class_context()
        {
            var classContext = TheContexts()[0];
            classContext.Tags.should_contain_tag("@class-tag");
        }

        [Test]
        public void it_should_tag_method_context()
        {
            var methodContext = TheContexts()[0].Contexts[0];
            methodContext.Tags.should_contain_tag("@method-tag");
        }
    }

    [TestFixture]
    [Category("ContextBuilder")]
    public class describe_second_order_inheritance : describe_ContextBuilder
    {
        class base_spec : nspec { }

        class child_spec : base_spec { }

        class grand_child_spec : child_spec { }

        [SetUp]
        public void setup()
        {
            GivenTypes(typeof(base_spec),
                typeof(child_spec),
                typeof(grand_child_spec));
        }

        [Test]
        public void the_root_context_should_be_base_spec()
        {
            TheContexts().First().Name.should_be(typeof(base_spec));
        }

        [Test]
        public void the_next_context_should_be_derived_spec()
        {
            TheContexts().First().Contexts.First().Name.should_be(typeof(child_spec));
        }

        [Test]
        public void the_next_next_context_should_be_derived_spec()
        {
            TheContexts().First().Contexts.First().Contexts.First().Name.should_be(typeof(grand_child_spec));
        }
    }
    public static class InheritanceExtentions
    {
        public static void should_be(this string actualName, Type expectedType)
        {
            Assert.AreEqual(expectedType.Name.Replace("_", " "), actualName);
        }

    }
}