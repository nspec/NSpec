using System;
using NSpec;

public class describe_capturing_example_console : nspec
{
    void output_capture()
    {
        it["should capture output"] = () =>
        {
            Console.WriteLine("this is console output");
        };
    }
}

public static class describe_capturing_example_console_output
{
    public static string Output = @"
describe capturing example console
  output capture
    should capture output (__ms)
      //Console output
      this is console output

1 Examples, 0 Failed, 0 Pending
";
}
