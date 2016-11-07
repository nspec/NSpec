using FluentAssertions;
using NSpec;
using System;

public class describe_specifications : nspec
{
    void when_creating_specifications()
    {
        //some of these specifications are meant to fail so you can see what the output looks like
        it["true should be false"] = () => true.Should().BeFalse();
        it["enumerable should be empty"] = () => new int[] { }.Should().BeEmpty();
        it["enumerable should contain 1"] = () => new[] { 1 }.Should().Contain(1);
        it["enumerable should not contain 1"] = () => new[] { 1 }.Should().NotContain(1);
        it["1 should be 2"] = () => 1.Should().Be(2);
        it["1 should be 1"] = () => 1.Should().Be(1);
        it["1 should not be 1"] = () => 1.Should().NotBe(1);
        it["1 should not be 2"] = () => 1.Should().NotBe(2);
        it["\"\" should not be null"] = () => "".Should().NotBeNull();
        it["some object should not be null"] = () => someObject.Should().NotBeNull();
        //EXPERIMENTAL - specify only takes a lambda and does
        //its best to make a sentence out of the code. YMMV.
        specify = () => "ninja".Should().NotBe("pirate", String.Empty);
    }
    object someObject = null;
}

public static class describe_specifications_output
{
    public static string Output = @"
describe specifications
  when creating specifications
    true should be false (__ms) - FAILED - Expected: False, But was: True
    enumerable should be empty (__ms)
    enumerable should contain 1 (__ms)
    enumerable should not contain 1 (__ms) - FAILED - Expected: not collection containing 1, But was: < 1 >
    1 should be 2 (__ms) - FAILED - Expected: 2, But was: 1
    1 should be 1 (__ms)
    1 should not be 1 (__ms) - FAILED - Expected: not equal to 1, But was: 1
    1 should not be 2 (__ms)
    """" should not be null (__ms)
    some object should not be null (__ms) - FAILED - Expected: not null, But was: null
    ninja should not be pirate (__ms)

**** FAILURES ****

nspec. describe specifications. when creating specifications. true should be false.
Expected: False, But was: True

nspec. describe specifications. when creating specifications. enumerable should not contain 1.
Expected: not collection containing 1, But was: < 1 >

nspec. describe specifications. when creating specifications. 1 should be 2.
Expected: 2, But was: 1

nspec. describe specifications. when creating specifications. 1 should not be 1.
Expected: not equal to 1, But was: 1

nspec. describe specifications. when creating specifications. some object should not be null.
Expected: not null, But was: null

11 Examples, 5 Failed, 0 Pending
";
    public static int ExitCode = 1;
}
