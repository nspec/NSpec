using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSpec;

public class describe_focus : nspec
{
    [Tag("focus")]
    void it_is_run() { }

    void it_is_not_run() { }
}

public static class describe_focus_output
{
    public static string Output = @"
describe focus
  it is run

1 Examples, 0 Failed, 0 Pending

NSpec found context/examples tagged with ""focus"" and only ran those.
";
}
