using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSpec;

namespace SampleSpecs.WebSite
{
    class class_level_before : nspec
    {
        List<int> ints = null;

        before<dynamic> each = (s) =>
        {
            s.ints = new List<int>();
        };

        public void list_manipulations()
        {
            specify = () => ints.Count.should_be(0);

            context["number in collection"] = () =>
            {
                before = () => ints.Add(15);

                specify = () => ints.Count.should_be(1);
            };
        }
    }
}
