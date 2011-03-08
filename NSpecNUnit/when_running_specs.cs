using System;
using System.Linq;
using NSpec;
using NSpec.Domain;
using NSpec.Extensions;
using NSpec.Interpreter.Indexer;
using NUnit.Framework;
using Rhino.Mocks;

namespace NSpecNUnit
{
    [TestFixture]
    public class when_finding_specs
    {
        private SpecFinder finder;
        private IReflector reflector;
        private string someDLL;

        [SetUp]
        public void setup()
        {
            GivenDllContains();
        }

        private void GivenDllContains(params Type[] types)
        {
            reflector = MockRepository.GenerateMock<IReflector>();

            reflector.Stub(r => r.GetTypesFrom("")).IgnoreArguments().Return(types);

            someDLL = "an nspec project dll";

            finder = new SpecFinder(someDLL, reflector);
        }

        [Test]
        public void it_should_get_types_from_reflection()
        {
            reflector.AssertWasCalled(r=>r.GetTypesFrom(someDLL));
        }

        [Test]
        public void it_should_include_classes_that_implement_spec_and_have_public_methods()
        {
            GivenDllContains(typeof(specClass));

            finder.SpecClasses().should_contain(typeof(specClass));
        }

        [Test]
        public void it_should_exclude_classes_that_inherit_from_spec_but_have_no_public_methods()
        {
            GivenDllContains(typeof(specClassWithNoPublicMethods));

            finder.SpecClasses().should_be_empty();
        }

        [Test]
        public void it_should_exclude_classes_that_do_not_inherit_from_spec()
        {
            GivenDllContains(typeof(nonSpecType));

            finder.SpecClasses().should_be_empty();
        }

        [Test,Ignore]
        public void it_should_create_a_context_for_the_specClass()
        {
            finder.Contexts.should_contain(c => c.Name == "specClass");
        }

        [Test,Ignore]
        public void it_should_add_the_public_method_as_a_sub_context()
        {
            TheRootContext().Contexts.should_contain( c=>c.Name=="public_method");
        }

        [Test,Ignore]
        public void it_should_not_create_a_sub_context_for_the_private_method()
        {
            TheRootContext().Contexts.should_not_contain(c=>c.Name=="private_method");
        }

        private Context TheRootContext()
        {
            return finder.Contexts.First(c=>c.Name=="specClass");
        }
    }

    public class nonSpecType{}
    public class specClassWithNoPublicMethods : spec 
    {
        private void private_method() { }
    }
    public class specClass : spec
    {
        public void public_method() { }
        private void private_method() { }
    }
}