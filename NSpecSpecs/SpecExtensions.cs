using NSpec;
using NSpec.Domain;

namespace NSpecSpecs
{
    public static class SpecExtensions
    {
        public static void should_not_have_failed(this Example example)
        {
            example.Exception.should_be(null);
        }
    }
}