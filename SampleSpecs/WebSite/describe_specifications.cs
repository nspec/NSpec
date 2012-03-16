using NSpec;

class describe_specifications : nspec
{
    void when_creating_specifications()
    {
        //some of these specifications are meant to fail so you can see what the output looks like
        it["true should be false"] = () => true.should_be_false();
        it["enumerable should be empty"] = () => new int[] { }.should_be_empty();
        it["enumerable should contain 1"] = () => new[] { 1 }.should_contain(1);
        it["enumerable should not contain 1"] = () => new[] { 1 }.should_not_contain(1);
        it["1 should be 2"] = () => 1.should_be(2);
        it["1 should be 1"] = () => 1.should_be(1);
        it["1 should not be 1"] = () => 1.should_not_be(1);
        it["1 should not be 2"] = () => 1.should_not_be(2);
        it["\"\" should not be null"] = () => "".should_not_be_null();
        it["some object should not be null"] = () => someObject.should_not_be_null();
        //EXPERIMENTAL - specify only takes a lambda and does 
        //its best to make a sentence out of the code. YMMV.
        specify = ()=> "ninja".should_not_be("pirate");
    }
    object someObject = null;
}

public static class describe_specifications_expected
{
    public static string Output = @"
describe specifications
  when creating specifications
    true should be false - FAILED - Expected: False, But was: True
    enumerable should be empty
    enumerable should contain 1
    enumerable should not contain 1 - FAILED - Expected: not collection containing 1, But was: < 1 >
    1 should be 2 - FAILED - Expected: 2, But was: 1
    1 should be 1
    1 should not be 1 - FAILED - Expected: not 1, But was: 1
    1 should not be 2
    """" should not be null
    some object should not be null - FAILED - Expected: not null, But was: null
    ninja should not be pirate

**** FAILURES ****

nspec. describe specifications. when creating specifications. true should be false.
Expected: False, But was: True
   at describe_specifications.<when_creating_specifications>b__0() in c:\Users\Administrator\Documents\Visual Studio 2010\Projects\NSpec\SampleSpecs\WebSite\describe_specifications.cs:line 8

nspec. describe specifications. when creating specifications. enumerable should not contain 1.
Expected: not collection containing 1, But was: < 1 >
   at describe_specifications.<when_creating_specifications>b__3() in c:\Users\Administrator\Documents\Visual Studio 2010\Projects\NSpec\SampleSpecs\WebSite\describe_specifications.cs:line 11

nspec. describe specifications. when creating specifications. 1 should be 2.
Expected: 2, But was: 1
   at describe_specifications.<when_creating_specifications>b__4() in c:\Users\Administrator\Documents\Visual Studio 2010\Projects\NSpec\SampleSpecs\WebSite\describe_specifications.cs:line 12

nspec. describe specifications. when creating specifications. 1 should not be 1.
Expected: not 1, But was: 1
   at describe_specifications.<when_creating_specifications>b__6() in c:\Users\Administrator\Documents\Visual Studio 2010\Projects\NSpec\SampleSpecs\WebSite\describe_specifications.cs:line 14

nspec. describe specifications. when creating specifications. some object should not be null.
Expected: not null, But was: null
   at describe_specifications.<when_creating_specifications>b__9() in c:\Users\Administrator\Documents\Visual Studio 2010\Projects\NSpec\SampleSpecs\WebSite\describe_specifications.cs:line 17

11 Examples, 5 Failed, 0 Pending
";

}