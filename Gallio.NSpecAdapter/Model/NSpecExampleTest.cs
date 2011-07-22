using Gallio.Model;
using NSpec.Domain;
using Reflector = Gallio.Common.Reflection.Reflector;

namespace NSpec.GallioAdapter.Model
{
    public class NSpecExampleTest : NSpecTest
    {
        readonly Example _specification;
        public Example Specification { get { return _specification; } }

        public NSpecExampleTest( Example specification )
            : base( specification.Spec, Reflector.Wrap( specification.MethodLevelExample ) )
        {
            this.Kind = TestKinds.Test;
            this.IsTestCase = true;
            _specification = specification;
        }
    }
}