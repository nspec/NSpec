using NSpec;

class describe_method_level_examples : nspec
{
    void it_should_run_methods_that_start_with_IT_as_an_assertion()
    {
        @"this is a method level assertion (starts with ""it_"")".should_not_be_empty();
    }

    void specify_that_methods_that_start_with_SPECIFY_should_run_as_assertion()
    {
        @"this is a method level assertion (starts with ""specify_"")".should_not_be_empty();
    }
}
