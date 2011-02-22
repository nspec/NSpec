using System.Collections.Generic;
using NSpec.Extensions;
using NSpec.Interpreter.Indexer;

namespace NSpecSpec
{
    public class prime_kata : spec
    {
        public void prime_factors()
        {
            new Dictionary<int, int[]>
            {
                {1,new int[]{}},
            }.Do((given, expected) =>
                specify("{0} should be {1}".With(given,expected),() => Primes(given).should_be(expected)));
        }

        private IEnumerable<int> Primes(int num)
        {
            return new int[] {};
        }
    }
}