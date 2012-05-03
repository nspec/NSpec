using Gallio.Model;
using Gallio.Model.Tree;
using NSpec.Domain;
using Reflector = Gallio.Common.Reflection.Reflector;

namespace NSpec.GallioAdapter.Model
{
    public class NSpecContextTest : Test
    {
        readonly Context _context;

        public Context Context
        {
            get { return _context; }
        }

        public NSpecContextTest( Context context )
            : base( context.Name, null )
        {
            this.Kind = TestKinds.Fixture;
            this._context = context;
        }
    }
}
