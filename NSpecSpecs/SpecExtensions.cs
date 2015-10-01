using System.Text.RegularExpressions;
using NSpec;
using NSpec.Domain;

namespace NSpecSpecs
{
    public static class SpecExtensions
    {
        public static void should_have_passed(this ExampleBase example)
        {
            (example.HasRun && example.Exception == null).is_true();
        }

        public static void should_have_failed(this ExampleBase example)
        {
            (example.HasRun && example.Exception == null).is_false();
        }

        public static string RegexReplace(this string input, string pattern, string replace)
        {
            return Regex.Replace(input, pattern, replace);
        }
    }
}