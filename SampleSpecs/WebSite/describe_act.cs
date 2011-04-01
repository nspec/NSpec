using NSpec;

class describe_act : nspec
{
    //class level act
    void act_each()
    {
        executionSequence += "3";
    }

    void acts_are_executed_after_all_befores()
    {
        before = () => executionSequence = "1";
        act = () => executionSequence += "4";
        context["even if before is in a nested context"] = () =>
        {
            before = () => executionSequence += "2";
            it["the execution sequence should be \"1234\""] = 
                () => executionSequence.should_be("1234");
        };
    }
    string executionSequence;
}