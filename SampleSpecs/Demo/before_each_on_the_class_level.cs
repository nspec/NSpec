using System.Collections.Generic;
using NSpec;
using NSpec.Assertions.nUnit;

namespace SampleSpecs.Demo
{
    class before_each_on_the_class_level : nspec
    {
        List<int> ints = null;

        void before_each()
        {
            ints = new List<int>();
        }

        void it_should_run_before_on_class_level()
        {
            before = () => ints.Add(12);

            specify = () => ints.Count.should_be(1);
        }
    }
}
