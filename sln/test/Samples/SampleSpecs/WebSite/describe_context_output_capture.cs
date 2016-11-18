using System;
using NSpec;

public class describe_context_output_capture : nspec
{
    void before_all()
    {
        Console.WriteLine("this is before all");
    }
    void output_capture()
    {
        beforeAll = () =>
        {
            Console.WriteLine("this is context before all");
        };

        beforeEach = () =>
        {
            Console.WriteLine("this is before each");
        };

        it["should capture output"] = () =>
        {
            Console.WriteLine("this is console output");
        };
    }
}

public static class describe_context_output_capture_output
{
    public static string Output = @"
describe context output capture
//Console output
this is before all
  output capture
  //Console output
  this is context before all
    should capture output (__ms)
      //Console output
      this is before each
      this is console output

1 Examples, 0 Failed, 0 Pending
";
}
