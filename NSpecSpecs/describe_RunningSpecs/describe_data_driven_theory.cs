using NSpec;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;

namespace NSpecSpecs.describe_RunningSpecs
{
    [TestFixture]
    [Category("RunningSpecs")]
    public class describe_data_driven_theory : when_running_specs
    {
        public class describe_prime_factors : nspec
        {
            void when_determining_prime_factors()
            {
                new Each<int, int[]>
                {
                    { 0, new int[] { } },
                    { 1, new int[] { } },
                    { 2, new[] { 2 } },
                    { 3, new[] { 3 } },
                    { 4, new[] { 2, 2 } },
                    { 5, new[] { 5 } },
                    { 6, new[] { 2, 3 } },
                    { 7, new[] { 7 } },
                    { 8, new[] { 2, 2, 2 } },
                    { 9, new[] { 3, 3 } },

                }.Do((given, expected) =>
                    it["{0} should be {1}".With(given, expected)] = () => given.Primes().should_be(expected)
                );
            }
        }

        [SetUp]
        public void Setup()
        {
            Run(typeof(describe_prime_factors));
        }

        [Test]
        public void all_data_points_succeeds()
        {
            foreach (var example in AllExamples())
            {
                example.HasRun.should_be_true();
                example.Exception.should_be_null();
            }
        }
    }
}
