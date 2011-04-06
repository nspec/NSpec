using NSpec;

class given_the_sequence_continues_with_2 : given_the_sequence_starts_with_1
{
    void before_each()
    {
        sequence += "2";
    }
    void given_the_sequence_continues_with_3()
    {
        before = () => sequence += "3";

        //the befores run in the order you would expect
        it["sequence should be \"123\""] =
            () => sequence.should_be("123");
    }
}

class given_the_sequence_starts_with_1 : nspec
{
    void before_each()
    {
        sequence = "1";
    }
    protected string sequence;
}
