using NSpec;

class describe_inheritance : base_spec
{
    void before_each()
    {
        executionSequence += "2";
    }
    void act_each()
    {
        executionSequence += "5";
    }
    void some_context()
    {
        before = () => executionSequence += "3";

        it["executionSequence should be \"12345\""] =
            () => executionSequence.should_be("12345");
    }
}

class base_spec : nspec
{
    void before_each()
    {
        executionSequence = "1";
    }
    void act_each()
    {
        executionSequence += "4";
    }
    protected string executionSequence;
}
