using NSpec;

class describe_class_level : nspec
{
    //before and act can also be declared at the class level like so
    void before_each()
    {
        sequence = "arrange, ";
    }
    void act_each()
    {
        sequence += "act";
    }
    void given_befores_and_acts_run_in_the_correct_sequence()
    {
        specify = () => sequence.should_be("arrange, act");
    }
    string sequence;
}