using LibraryUnderTest;
using NSpec;
using Shouldly;

namespace LibrarySpecs
{
    public class describe_ConstantGenerator : nspec
    {
        ConstantGenerator generator;

        void before_each()
        {
            generator = new ConstantGenerator();
        }

        void Given_number_is_requested_twice()
        {
            int first = -1;
            int second = -2;

            act = () =>
            {
                first = generator.GetNumber();
                second = generator.GetNumber();
            };

            it["Should return same number"] = () => first.ShouldBe(second);
        }
    }
}
