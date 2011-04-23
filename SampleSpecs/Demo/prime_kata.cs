using System.Collections.Generic;
using System.Linq;
using NSpec;

namespace SampleSpecs.Demo
{
    class prime_kata : nspec
    {
        void prime_factors()
        {
            //throwing an unhandled exception should now not cause the runner to crash
            //var dict = new Dictionary<int, string> { { 1, "1" }, { 1, "sdfg" } };

            new Dictionary<int, int[]>
            {
                { 1, new int[]{}},
                { 2, new[]{ 2 }},
                { 3, new[]{ 3 }},
                { 4, new[]{ 2, 2 }},
                { 5, new[]{ 5 }},
                { 6, new[]{ 2,3 }},
                { 7, new[]{ 7 }},
                { 8, new[]{ 2,2,2 }},
                { 9, new[]{ 3,3 }},
            }.Do((given, expected) =>
                it["{0} should be {1}".With(given, expected)] = () => Primes(given).should_be(expected)
                );
        }

        IEnumerable<int> Primes(int num)
        {
            if (num == 1) return new int[] { };

            for (int i = 2; i < num;i++ )
                if (num % i == 0) return new[] { i }.Concat(Primes(num / i));

            return new[] { num };
        }
    }
}