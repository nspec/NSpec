using NSpec;

// TODO rename containing folder. Consider wrapping in namespace

[Tag("describe_act")]
public class describe_batman_sound_effects_as_text : nspec
{
    void they_are_loud_and_emphatic()
    {
        //act runs after all the befores, and before each spec
        //declares a common act (arrange, act, assert) for all subcontexts
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

public static class describe_batman_sound_effects_as_text_output
{
    public static string Output = @"
describe batman sound effects as text
  they are loud and emphatic
    given bam
      should be BAM!!! (__ms)
    given whack
      should be WHACK!!! (__ms)

2 Examples, 0 Failed, 0 Pending
";
    public static int ExitCode = 0;
}
