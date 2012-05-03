using Gallio.Model;
using Gallio.Model.Tree;
using NSpec.Domain;
using Reflector = Gallio.Common.Reflection.Reflector;

namespace NSpec.GallioAdapter.Model
{
    public class NSpecExampleTest : Test
    {
        readonly Example _example;
        public Example Example { get { return this._example; } }

        public NSpecExampleTest( Example example )
            : base( example.Spec, null )
        {
            this.Kind = TestKinds.Test;
            this.IsTestCase = true;
            this._example = example;
        }
    }
}