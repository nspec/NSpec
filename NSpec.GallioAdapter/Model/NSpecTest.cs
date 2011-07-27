using Gallio.Common.Reflection;
using Gallio.Model.Tree;

namespace NSpec.GallioAdapter.Model
{
    public abstract class NSpecTest : Test
    {
        protected NSpecTest( string name, ICodeElementInfo codeElement ) : base( name, codeElement )
        {
        }
    }
}