using Gallio.Model.Helpers;
using NSpec.GallioAdapter.Model;

namespace NSpec.GallioAdapter.Services
{
    class NSpecTestDriver : SimpleTestDriver 
    {
        protected override string FrameworkName
        {
            get { return "NSpec"; }
        }

        protected override TestExplorer CreateTestExplorer()
        {
            return new NSpecTestExplorer();
        }

        protected override TestController CreateTestController()
        {
            return new DelegatingTestController( test => new NSpecController() );
        }
    }
}