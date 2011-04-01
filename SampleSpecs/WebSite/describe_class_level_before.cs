using NSpec;

class describe_class_level_before : nspec
{
    //nspec will look for a method that is named before_each()
    //the object will be initialized for each test
    void before_each()
    {
        someObject = new object();
    }
    void class_level_befores_run_before_each_context()
    {
        it["some object should not be null"] = () => someObject.should_not_be_null();
    }
    object someObject;
}