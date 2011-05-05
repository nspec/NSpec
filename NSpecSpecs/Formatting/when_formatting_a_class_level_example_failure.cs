using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NSpec;

namespace NSpecSpecs.Formatting
{
    [TestFixture]
    public class when_formatting_a_class_level_example_failure
    {
        class SpecClass : nspec
        {
            string value;

            void before_each()
            {
                value = "applied";
            }

            void it_should_be_failing()
            {
                "hello".should_not_be("hello");
            }
        }
    }
}
