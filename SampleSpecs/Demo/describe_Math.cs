using NSpec;
using NSpec.Assertions.nUnit;

class describe_Math : nspec
{
    void verify_strictly_increasing_numbers()
    {
        new[]
        { 
            1, 2, 3, 
            4, 5, 6, 
            7, 8, 9
        }.EachConsecutive2(
            (smaller, larger) =>
                it["{0} should be greater than {1}".With(larger, smaller)] =
                    () => larger.should_be_greater_than(smaller));
    }
}