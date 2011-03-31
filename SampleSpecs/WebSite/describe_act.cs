using NSpec;

class describe_act : nspec
{
    string sentence = null;

    void before_each()
    {
        sentence = string.Empty;

        sentence += "before | ";
    }

    void act_each()
    {
        sentence += "act | ";
    }

    void sentance_manipulation()
    {
        specify = () => sentence.should_be("before | act | ");

        context["acts go in order"] = () =>
        {
            act = () => sentence += "context level act | ";

            specify = () => sentence.should_be("before | act | context level act | ");

            context["befores execute before acts"] = () =>
            {
                before = () => sentence += "context before | ";

                specify = () => sentence.should_be("before | context before | act | context level act | ");
            };
        };
    }
}