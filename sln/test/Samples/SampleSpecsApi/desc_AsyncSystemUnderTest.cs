using NSpec;
using SampleSpecsApi.SampleSystem;
using System.Threading.Tasks;

namespace SampleSpecsApi
{
    // Do not move the following spec classes around, to avoid rewriting line numbers. This should be stuck at line nr. 7
    public class AsyncSpec : nspec
    {
        protected AsyncSystemUnderTest systemUnderTest;

        void before_each()
        {
            systemUnderTest = new AsyncSystemUnderTest();
        }

        async Task it_async_method_example()
        { // # 18
            bool actual = await systemUnderTest.IsAlwaysTrueAsync();

            actual.ShouldBeTrue();
        } // # 22

        void method_context()
        { // # 25
            itAsync["async context example"] = async () =>
            { // # 27
                bool actual = await systemUnderTest.IsAlwaysTrueAsync();

                actual.ShouldBeTrue();
            }; // # 31
        } // # 32
    }

    // Do not move the preceding spec classes around, to avoid rewriting line numbers

    // TODO add sample with `itAsync[...] = expectAsync<...>();` : is it possible to get source file path and line number?
}
