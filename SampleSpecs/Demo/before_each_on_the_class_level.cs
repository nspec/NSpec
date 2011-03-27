using System.Collections.Generic;
using NSpec;

namespace SampleSpecs.Demo
{
    public class before_each_on_the_class_level : nspec
    {
        private List<int> ints = null;

        public void before_each()
        {
            ints = new List<int>();
        }

        public void it_should_run_before_on_class_level()
        {
            before = () => ints.Add(12);

            specify = () => ints.Count.should_be(1);
        }
    }
}
