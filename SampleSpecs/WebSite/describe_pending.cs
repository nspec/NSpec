using NSpec;

namespace SampleSpecs.WebSite
{
    class describe_pending : nspec
    {
        void when_creating_pending_specifications()
        {
            xit["pending spec"] = () => "".should_be("something else");
            it["another pending spec"] = todo;
        }
    }
}