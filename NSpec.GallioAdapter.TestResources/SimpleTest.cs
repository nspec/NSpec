using NSpec;

/// <summary>
/// A simple test fixture
/// </summary>
class A_simple_test : nspec
{
    /// <summary>
    /// A passing test
    /// </summary>
    void This_test_should_pass()
    {
        it["Yeah, it passes!"] = () => true.should_be_true();

        it["Debug Me"] = () =>
        {
            true.should_be_true();
        };
    }

    /// <summary>
    /// A failing test
    /// </summary>
    void This_test_should_fail()
    {
        it["Yeah, it failed as expected."] = () => true.should_be_false();
    }
}