using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSpec;

namespace SampleSpecs.WebSite
{
    class describe_expected_exception : nspec
    {
        void when_expecting_exception()
        {
            it["exception being thrown should pass test"] = 
                expect<NullReferenceException>(() => someNullString.Trim());
        }
        string someNullString = null;
    }
}
