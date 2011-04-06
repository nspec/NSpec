using System;
using NSpec;

class describe_expected_exception : nspec
{
    void given_a_null_string()
    {
        it["should throw nullref"] =
            expect<NullReferenceException>(() => nullString.Trim());
    }
    string nullString;
}