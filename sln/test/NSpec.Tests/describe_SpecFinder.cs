using System;
using System.Collections.Generic;
using System.Linq;
using NSpec;
using NSpec.Domain;
using NUnit.Framework;
using describe_OtherNameSpace;
using describe_SomeNameSpace;
using Moq;
using FluentAssertions;

namespace NSpec.Tests
{
    public class SpecClass : nspec
    {
        public void public_method() { }
        private void private_method() { }
    }

    public class AnotherSpecClass : nspec
    {
        void public_method() { }
    }

    public class NonSpecClass { }

    public class SpecClassWithNoVoidMethods : nspec
    {
        string parameter_less_method() { return ""; }
    }

    public class SpecClassWithNoParameterLessMethods : nspec
    {
        void private_method(string parameter) { }

        public void public_method(string parameter) { }
    }

    [TestFixture]
    [Category("SpecFinder")]
    public class without_filtering : when_finding_specs
    {
        [SetUp]
        public void setup()
        {
            GivenDllContains();
        }

        [Test]
        public void it_should_get_types_from_reflection()
        {
            reflector.Verify(r => r.GetTypesFrom());
        }

        [Test]
        public void it_should_include_classes_that_implement_nspec_and_have_paramterless_void_methods()
        {
            GivenDllContains(typeof(SpecClass));

            finder.SpecClasses().Should().Contain(typeof(SpecClass));
        }

        [Test]
        public void it_should_exclude_classes_that_inherit_from_nspec_but_have_no_parameterless_methods()
        {
            GivenDllContains(typeof(SpecClassWithNoParameterLessMethods));

            finder.SpecClasses().Should().BeEmpty();
        }

        [Test]
        public void it_should_exclude_classes_that_do_not_inherit_from_nspec()
        {
            GivenDllContains(typeof(NonSpecClass));

            finder.SpecClasses().Should().BeEmpty();
        }

        [Test]
        public void it_should_exclude_classes_that_have_no_void_methods()
        {
            GivenDllContains(typeof(SpecClassWithNoVoidMethods));

            finder.SpecClasses().Should().BeEmpty();
        }
    }

    [TestFixture]
    [Category("SpecFinder")]
    public class when_filtering_specs : when_finding_specs
    {
        [Test]
        public void it_should_filter_in()
        {
            GivenDllContains(typeof(AnotherSpecClass));

            GivenFilter(typeof(AnotherSpecClass).Name);

            finder.SpecClasses().Should().Contain(typeof(AnotherSpecClass));
        }

        [Test]
        public void it_should_filter_out()
        {
            GivenDllContains(typeof(SpecClass));

            GivenFilter(typeof(AnotherSpecClass).Name);

            finder.SpecClasses().Should().BeEmpty();
        }
    }

    [TestFixture]
    [Category("SpecFinder")]
    public class when_finding_specs_based_on_regex : when_finding_specs
    {
        [SetUp]
        public void Setup()
        {
            GivenDllContains(typeof(SomeClass),
                typeof(SomeDerivedClass),
                typeof(SomeClassInOtherNameSpace),
                typeof(SomeDerivedDerivedClass));
        }

        [Test]
        public void it_should_find_all_specs_if_regex_is_not_specified()
        {
            GivenFilter("");

            TheSpecClasses()
                .Should().Contain(typeof(SomeClass))
                .And.Contain(typeof(SomeDerivedClass))
                .And.Contain(typeof(SomeClassInOtherNameSpace));
        }

        [Test]
        public void it_should_find_specs_for_derived_class_and_include_base_class()
        {
            GivenFilter("DerivedClass$");

            TheSpecClasses()
                .Should().Contain(typeof(SomeClass))
                .And.Contain(typeof(SomeDerivedClass))
                .And.Contain(typeof(SomeDerivedDerivedClass))
                .And.NotContain(typeof(SomeClassInOtherNameSpace));

            TheSpecClasses().Count().Should().Be(3);
        }

        [Test]
        public void it_should_find_specs_that_contain_namespace()
        {
            GivenFilter("describe_SomeNameSpace");

            TheSpecClasses()
                .Should().Contain(typeof(SomeClass))
                .And.Contain(typeof(SomeDerivedClass))
                .And.NotContain(typeof(SomeClassInOtherNameSpace));
        }

        [Test]
        public void it_should_find_distinct_specs()
        {
            GivenFilter("Derived");

            TheSpecClasses()
                .Should().Contain(typeof(SomeClass))
                .And.Contain(typeof(SomeDerivedClass))
                .And.Contain(typeof(SomeDerivedDerivedClass));

            TheSpecClasses().Count().Should().Be(3);
        }
    }

    public class when_finding_specs
    {
        protected void GivenDllContains(params Type[] types)
        {
            reflector = new Mock<IReflector>();

            reflector.Setup(r => r.GetTypesFrom()).Returns(types);

            someDLL = "an nspec project dll";

            finder = new SpecFinder(reflector.Object);
        }

        protected void GivenFilter(string filter)
        {
            finder = new SpecFinder(reflector.Object, filter);
        }

        protected IEnumerable<Type> TheSpecClasses()
        {
            return finder.SpecClasses();
        }

        protected ISpecFinder finder;
        protected Mock<IReflector> reflector;
        protected string someDLL;
    }
}

namespace describe_SomeNameSpace
{
    class SomeClass : nspec
    {
        void context_method()
        {
        }
    }

    class SomeDerivedClass : SomeClass
    {
        void context_method()
        {
        }
    }

    class SomeDerivedDerivedClass : SomeClass
    {
        void context_method()
        {
        }
    }
}

namespace describe_OtherNameSpace
{
    class SomeClassInOtherNameSpace : nspec
    {
        void context_method()
        {
        }
    }
}