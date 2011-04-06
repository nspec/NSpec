using NSpec;

class describe_act : nspec
{
    void acts_run_after_befores_but_before_specs()
    {
        //allows you to declare a common "act" as in
        //arrange - act - assert for all subcontexts
        act = () => sequence += "2";
        context["given the sequence starts with 1"] = () =>
        {
            before = () => sequence = "1";
            it["the sequence should be \"12\""] = 
                () => sequence.should_be("12");
        };
        context["given the sequence starts with 2"] = () =>
        {
            before = () => sequence = "2";
            it["the sequence should be \"22\""] = 
                () => sequence.should_be("22");
        };
    }
    string sequence;
}