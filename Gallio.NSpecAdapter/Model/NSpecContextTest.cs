using Gallio.Model;
using NSpec.Domain;
using Reflector = Gallio.Common.Reflection.Reflector;

namespace NSpec.GallioAdapter.Model
{
    public class NSpecContextTest : NSpecTest
    {
        readonly Context _context;

        public Context Context
        {
            get { return _context; }
        }

        public NSpecContextTest( Context context )
            : base( context.Name, Reflector.Wrap( context.GetType() ) )
        {
            this.Kind = TestKinds.Fixture;
            this._context = context;
        }
    }
}
