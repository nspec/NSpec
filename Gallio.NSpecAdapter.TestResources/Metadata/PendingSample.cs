using NSpec;

namespace NSpec.GallioAdapter.TestResources.Metadata
{
    /// <summary>
    /// A simple test fixture
    /// </summary>
    class A_pending_test : nspec
    {
        /// <summary>
        /// A pending test
        /// </summary>
        void These_tests_are_pending()
        {
            it["I am pending"] = todo;
            xit["I am also pending"] = () => "".should_be( "Blah" );;
        }
    }
}