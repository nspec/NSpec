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
    public class when_running_specs
    {
        private SpecFinder finder;

        [SetUp]
        public void setup()
        {
            var reflector = MockRepository.GenerateMock<IReflector>();

            reflector.Stub(r => r.GetTypesFrom("")).IgnoreArguments().Return(new[] { typeof(nonSpecType), typeof(specClass) });

            finder = new SpecFinder("a fake dll", reflector);

            finder.Run();
        }
        [Test]
        public void it_should_get_types_from_reflection()
        {
            finder.Types.should_be(new []{typeof(nonSpecType),typeof(specClass)});
        }

        [Test]
        public void it_should_filter_the_classes_that_implement_spec()
        {
            finder.SpecClasses().should_be(new []{typeof(specClass)});
        }

        [Test]
        public void it_should_create_a_context_for_the_specClass_using_a_naming_convention()
        {
            finder.Contexts.should_contain(c => c.Name == "given specClass");
        }

        [Test]
        public void it_should_add_the_public_method_as_a_sub_context()
        {
            TheRootContext().Contexts.should_contain( c=>c.Name=="given public_method");
        }

        [Test]
        public void it_should_not_create_a_sub_context_for_private_methods()
        {
            TheRootContext().Contexts.should_not_contain(c=>c.Name=="given private_method");
        }

        private Context TheRootContext()
        {
            return finder.Contexts.First(c=>c.Name=="given specClass");
        }
    }

    public class nonSpecType{}
    public class specClass : spec
    {
        public void public_method() { }
        private void private_method() { }
    }
}