using System;
using NSpec;
using NSpec.Domain;
using NUnit.Framework;
using Rhino.Mocks;

namespace NSpecNUnit
{
    public class SpecClass : nspec
    {
        public void public_method() { }
        private void private_method() { }
    }
    public class AnotherSpecClass : nspec 
    {
        public void public_method() { }
    }
    public class NonSpecClass{}
    public class SpecClassWithNoPublicMethods : nspec 
    {
        private void private_method() { }
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
            reflector.AssertWasCalled(r=>r.GetTypesFrom(someDLL));
        }

        [Test]
        public void it_should_include_classes_that_implement_spec_and_have_public_methods()
        {
            GivenDllContains(typeof(SpecClass));

            finder.SpecClasses().should_contain(typeof(SpecClass));
        }

        [Test]
        public void it_should_exclude_classes_that_inherit_from_spec_but_have_no_public_methods()
        {
            GivenDllContains(typeof(SpecClassWithNoPublicMethods));

            finder.SpecClasses().should_be_empty();
        }

        [Test]
        public void it_should_exclude_classes_that_do_not_inherit_from_spec()
        {
            GivenDllContains(typeof(NonSpecClass));

            finder.SpecClasses().should_be_empty();
        }
    }

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

        protected ISpecFinder finder;
        protected IReflector reflector;
        protected string someDLL;
    }
}