using NSpec;

class describe_nested_classes : nspec
{
    protected string output = string.Empty;

    void before_each()
    {
        output += "root_class|";
    }

    void it_should_append_before_each()
    {
        output.should_be("root_class|");
    }

    class child_class : describe_nested_classes
    {
        void before_each()
        {
            output += "child_class|";
        }

        void it_should_append_before_each()
        {
            output.should_be("root_class|child_class|");
        }

        class child_child_class : child_class
        {
            void before_each()
            {
                output += "child_child_class|";
            }

            void it_should_append_before_each()
            {
                output.should_be("root_class|child_class|child_child_class|");
            }
        }
    }
}