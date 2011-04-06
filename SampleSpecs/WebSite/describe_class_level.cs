using NSpec;

class describe_class_level : nspec
{
    //before and act can also be declared at the class level like so
    void before_each()
    {
        sequence = "1";
    }
    void act_each()
    {
        sequence += "2";
    }
    void given_the_before_and_act_run()
    {
        it["sequence should be \"12\""] = () => sequence.should_be("12");
    }
    string sequence;
}