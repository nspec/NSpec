using System;
using NSpec;

namespace NSpecSpecs
{
    public class describe_output_capture : nspec
    {
        void output_capture()
        {
            it["should capture output"] = () =>
            {
                Console.WriteLine("this is console output");
            };
        }
    }

    public static class describe_output_capture_output
    {
        public static string Output = @"
describe output capture
  output capture
    should capture output
      //Console output
      this is console output

1 Examples, 0 Failed, 0 Pending
";
    }
}