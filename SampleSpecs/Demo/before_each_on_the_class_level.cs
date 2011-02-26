using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSpec.Interpreter.Indexer;
using NSpec.Extensions;

namespace SampleSpecs.Demo
{
    public class before_each_on_the_class_level : spec
    {
        private List<int> ints = null;

        before<dynamic> each = (c) =>
        {
            c.ints = new List<int>();
        };

        public void it_should_run_before_on_class_level()
        {
            before.each = () => ints.Add(12);
            
            specify(() => ints.Count.should_be(1));
        }
    }
}
