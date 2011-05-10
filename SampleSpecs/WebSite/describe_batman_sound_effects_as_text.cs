using NSpec;

class describe_batman_sound_effects_as_text : nspec
{
    void they_are_loud_and_emphatic()
    {
        //"when" runs after all the befores, and before each spec
        //allows you to declare a common "when" 
        act = () => sound = sound.ToUpper() + "!!!";
        context["given bam"] = () =>
        {
            before = () => sound = "bam";
            it["should be BAM!!!"] = 
                () => sound.should_be("BAM!!!");
        };
        context["given whack"] = () =>
        {
            before = () => sound = "whack";
            it["should be WHACK!!!"] = 
                () => sound.should_be("WHACK!!!");
        };
    }
    string sound;
}