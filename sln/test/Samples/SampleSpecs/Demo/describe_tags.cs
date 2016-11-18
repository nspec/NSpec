using FluentAssertions;
using NSpec;

namespace SampleSpecs.Demo
{
    class describe_tags : nspec
    {
        public void tags_at_context_level()
        {
            // NOTE: you have to run the nspec runner with a tag filter to see how
            // tags can be used to filter which contexts and examples are executed:
            //     nspecrunner <path-to-specs-dll> --tag mytag-one,~mytag-two

            context["when tags are specified at the context level", "mytag-one"] = () =>
            {
                it["tags all examples within that context"] = () => { true.Should().BeTrue(); };

                context["when tags are nested", "mytag-two"] = () =>
                {
                    it["tags all the nested examples and nested contexts cumlatively"] = () => { true.Should().BeTrue(); };
                };
            };

        }
    }
}
