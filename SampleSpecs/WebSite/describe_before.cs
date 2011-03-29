using NSpec;

class describe_before : nspec
{
    void they_run_before_each_example()
    {
        before = () => number = 1;
        it["number should be 2"] = () => (++number).should_be(2);
        it["number should be 1"] = () => number.should_be(1);
    }
    int number;
}