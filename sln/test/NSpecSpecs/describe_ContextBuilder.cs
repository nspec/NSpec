using FluentAssertions;
using Moq;
using NSpec;
using NSpec.Domain;
using NSpecSpecs;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NSpecSpecs
{
    [TestFixture]
    public class describe_ContextBuilder
    {
        protected Mock<ISpecFinder> finder;

        protected ContextBuilder builder;

        protected List<Type> typesForFinder;

        protected List<Context> contexts;

        [SetUp]
        public void setup_base()
        {
            finder = new Mock<ISpecFinder>();

            typesForFinder = new List<Type>();

            finder.Setup(f => f.SpecClasses()).Returns(typesForFinder);

            DefaultConventions conventions = new DefaultConventions();

            conventions.Initialize();

            builder = new ContextBuilder(finder.Object, conventions);
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
            finder.Verify(f => f.SpecClasses());
        }

        [Test]
        public void the_primary_context_should_be_parent()
        {
            TheContexts().First().ShouldBeNamedAfter(typeof(parent));
        }

        [Test]
        public void the_parent_should_have_the_child_context()
        {
            TheContexts().First().Contexts.First().ShouldBeNamedAfter(typeof(child));
        }

        [Test]
        public void it_should_only_have_the_parent_once()
        {
            TheContexts().Count().Should().Be(1);
        }

        [Test]
        public void it_should_have_the_sibling()
        {
            TheContexts().First().Contexts.Should().Contain(c => c.Name == typeof(sibling).Name);
        }

    }

    [TestFixture]
    public class when_finding_method_level_examples : describe_ContextBuilder
    {
        class class_with_method_level_example : nspec
        {
            void it_should_be_considered_an_example() { }

            void specify_should_be_considered_as_an_example() { }

            // -----

            async Task it_should_be_considered_an_example_with_async() { await Task.Delay(0); }

            async Task<long> it_should_be_considered_an_example_with_async_result() { await Task.Delay(0); return 0L; }

            async void it_should_be_considered_an_example_with_async_void() { await Task.Delay(0); }

            async Task specify_should_be_considered_as_an_example_with_async() { await Task.Delay(0); }
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
        public void should_find_async_method_level_example_if_the_method_name_starts_with_the_word_IT()
        {
            ShouldContainExample("it should be considered an example with async");
        }

        [Test]
        public void should_find_async_method_level_example_if_the_method_name_starts_with_the_word_IT_and_it_returns_result()
        {
            ShouldContainExample("it should be considered an example with async result");
        }

        [Test]
        public void should_find_async_method_level_example_if_the_method_name_starts_with_the_word_IT_and_it_returns_void()
        {
            ShouldContainExample("it should be considered an example with async void");
        }

        [Test]
        public void should_find_method_level_example_if_the_method_starts_with_SPECIFY()
        {
            ShouldContainExample("specify should be considered as an example");
        }

        [Test]
        public void should_find_async_method_level_example_if_the_method_starts_with_SPECIFY()
        {
            ShouldContainExample("specify should be considered as an example with async");
        }

        [Test]
        public void should_exclude_methods_that_start_with_ITs_from_child_context()
        {
            TheContexts().First().Contexts.Count.Should().Be(0);
        }

        private void ShouldContainExample(string exampleName)
        {
            TheContexts().First().Examples.Any(s => s.Spec == exampleName);
        }
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
            var finder = new Mock<ISpecFinder>();

            DefaultConventions defaultConvention = new DefaultConventions();

            defaultConvention.Initialize();

            var builder = new ContextBuilder(finder.Object, defaultConvention);

            classContext = new Context("class");

            builder.BuildMethodContexts(classContext, typeof(SpecClass));
        }

        [Test]
        public void it_should_add_the_public_method_as_a_sub_context()
        {
            classContext.Contexts.Should().Contain(c => c.Name == "public method");
        }

        [Test]
        public void it_should_not_create_a_sub_context_for_the_private_method()
        {
            classContext.Contexts.Should().Contain(c => c.Name == "private method");
        }

        [Test]
        public void it_should_disregard_method_called_before_each()
        {
            classContext.Contexts.Should().NotContain(c => c.Name == "before each");
        }

        [Test]
        public void it_should_disregard_method_called_act_each()
        {
            classContext.Contexts.Should().NotContain(c => c.Name == "act each");
        }
    }

    [TestFixture]
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
            classContext.Tags.Should().Contain("@class-tag");
        }

        [Test]
        public void it_should_tag_method_context()
        {
            var methodContext = TheContexts()[0].Contexts[0];
            methodContext.Tags.Should().Contain("@method-tag");
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
            TheContexts().First().ShouldBeNamedAfter(typeof(base_spec));
        }

        [Test]
        public void the_next_context_should_be_derived_spec()
        {
            TheContexts().First().Contexts.First().ShouldBeNamedAfter(typeof(child_spec));
        }

        [Test]
        public void the_next_next_context_should_be_derived_spec()
        {
            TheContexts().First().Contexts.First().Contexts.First().ShouldBeNamedAfter(typeof(grand_child_spec));
        }
    }

    public static class InheritanceExtentions
    {
        public static void ShouldBeNamedAfter(this Context context, Type expectedType)
        {
            string actual = context.Name;
            string expected = expectedType.Name.Replace("_", " ");

            actual.Should().Be(expected);
        }
    }
}
