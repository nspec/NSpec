using System.Linq;
using System.Text.RegularExpressions;
using NSpec.Domain;
using FluentAssertions;

namespace NSpec.Tests
{
    public static class SpecExtensions
    {
        public static Context Find(this ContextCollection contextCollection, string name)
        {
            return contextCollection.AllContexts().SingleOrDefault(c => c.Name == name);
        }

        public static ExampleBase FindExample(this ContextCollection contextCollection, string name)
        {
            return contextCollection.Examples().SingleOrDefault(e => e.Spec == name);
        }

        public static void ShouldHavePassed(this ExampleBase example)
        {
            (example.HasRun && example.Exception == null).Should().BeTrue();
        }

        public static void ShouldHaveFailed(this ExampleBase example)
        {
            (example.HasRun && example.Exception == null).Should().BeFalse();
        }

        public static string RegexReplace(this string input, string pattern, string replace)
        {
            return Regex.Replace(input, pattern, replace);
        }
    }
}