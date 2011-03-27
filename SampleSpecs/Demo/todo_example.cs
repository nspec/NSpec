using NSpec;

namespace SampleSpecs.Demo
{
    class todo_example : nspec
    {
        void soon()
        {
            it["everyone will have a drink"] = todo;
        }
    }
}