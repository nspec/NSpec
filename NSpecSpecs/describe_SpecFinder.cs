using System;
using NSpec;
using NSpec.Domain;
using NUnit.Framework;
using Rhino.Mocks;
using describe_SomeNameSpace;
using describe_OtherNameSpace;
using System.Collections.Generic;

namespace NSpecNUnit
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
            reflector.AssertWasCalled(r => r.GetTypesFrom(someDLL));
        }

        [Test]
        public void it_should_include_classes_that_implement_nspec_and_have_paramterless_void_methods()
        {
            GivenDllContains(typeof(SpecClass));

            finder.SpecClasses().should_contain(typeof(SpecClass));
        }

        [Test]
        public void it_should_exclude_classes_that_inherit_from_nspec_but_have_no_parameterless_methods()
        {
            GivenDllContains(typeof(SpecClassWithNoParameterLessMethods));

            finder.SpecClasses().should_be_empty();
        }

        [Test]
        public void it_should_exclude_classes_that_do_not_inherit_from_nspec()
        {
            GivenDllContains(typeof(NonSpecClass));

            finder.SpecClasses().should_be_empty();
        }

        [Test]
        public void it_should_exclude_classes_that_have_no_void_methods()
        {
            GivenDllContains(typeof(SpecClassWithNoVoidMethods));

            finder.SpecClasses().should_be_empty();
        }
    }

    [TestFixture]
    public class when_filtering_specs : when_finding_specs
    {
        [Test]
        public void it_should_filter_in()
        {
            GivenDllContains(typeof(AnotherSpecClass));

            GivenFilter(typeof(AnotherSpecClass).Name);

            finder.SpecClasses().should_contain(typeof(AnotherSpecClass));
        }

        [Test]
        public void it_should_filter_out()
        {
            GivenDllContains(typeof(SpecClass));

            GivenFilter(typeof(AnotherSpecClass).Name);

            finder.SpecClasses().should_be_empty();
        }
    }

    [TestFixture]
    public class when_finding_specs_based_on_regex : when_finding_specs
    {
        [SetUp]
        public void Setup()
        {
            GivenDllContains(typeof(SomeClass),
                typeof(SomeOtherClass),
                typeof(SomeClassInOtherNameSpace));
        }

        [Test]
        public void it_should_find_all_specs_if_regex_is_not_specified()
        {
            GivenFilter("");

            TheSpecClasses()
                .should_contain(typeof(SomeClass))
                .should_contain(typeof(SomeOtherClass))
                .should_contain(typeof(SomeClassInOtherNameSpace));
        }

        [Test]
        public void it_should_find_specs_that_end_with_specified_regex()
        {
            GivenFilter("OtherClass$");

            TheSpecClasses()
                .should_not_contain(typeof(SomeClass))
                .should_contain(typeof(SomeOtherClass))
                .should_not_contain(typeof(SomeClassInOtherNameSpace));
        }

        [Test]
        public void it_should_find_specs_that_contain_namespace()
        {
            GivenFilter("describe_SomeNameSpace");

            TheSpecClasses()
                .should_contain(typeof(SomeClass))
                .should_contain(typeof(SomeOtherClass))
                .should_not_contain(typeof(SomeClassInOtherNameSpace));
        }
    }

    public class when_finding_specs
    {
        protected void GivenDllContains(params Type[] types)
        {
            reflector = MockRepository.GenerateMock<IReflector>();

            RhinoMocksExtensions.Stub(reflector, r => r.GetTypesFrom("")).IgnoreArguments().Return(types);

            someDLL = "an nspec project dll";

            finder = new SpecFinder(someDLL, reflector);
        }

        protected void GivenFilter(string filter)
        {
            finder = new SpecFinder(someDLL, reflector, filter);
        }

        protected IEnumerable<Type> TheSpecClasses()
        {
            return finder.SpecClasses();
        }

        protected ISpecFinder finder;
        protected IReflector reflector;
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

    class SomeOtherClass : nspec
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