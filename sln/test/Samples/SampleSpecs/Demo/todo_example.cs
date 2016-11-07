using FluentAssertions;
using NSpec;
using System;

namespace SampleSpecs.Demo
{
    class todo_example : nspec
    {
        void soon()
        {
            it["everyone will have a drink"] = todo;
            xspecify = ()=> true.Should().BeFalse(String.Empty);
        }
    }
}
