using NSpec;
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

        [SetUp]
        public void setup()
        {
            var reflector = MockRepository.GenerateMock<IReflector>();

            reflector.Stub(r => r.GetTypesFrom("")).IgnoreArguments().Return(new[] { typeof(nonSpecType), typeof(specType) });

            finder = new SpecFinder("a fake dll", reflector);
        }
        [Test]
        public void it_should_get_types_from_reflection()
        {
            finder.Types.should_be(new []{typeof(nonSpecType),typeof(specType)});
        }

        [Test]
        public void it_should_filter_out_the_classes_that_implement_spec()
        {
            finder.SpecClasses().should_be(new []{typeof(specType)});
        }
    }

    public class nonSpecType{}
    public class specType: spec{}
}