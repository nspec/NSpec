using System;
using System.Collections.Generic;
using NSpec.Extensions;
using NSpec.Interpreter.Indexer;

namespace NSpecSpec
{
    public class prime_kata : spec
    {
        public void prime_factors()
        {
            //throwing an exception in a contextmethod is not handled gracefully
            //var dict = new Dictionary<int, string> {{1, "1"}, {1, "sdfg"}};
            new Tuples<int, int[]>
            {
                {1,new int[]{}},
                {2,new[]{2}},
            }.Do((given, expected) =>
                specify("{0} should be {1}".With(given,expected),() => Primes(given).should_be(expected)));
        }

        private IEnumerable<int> Primes(int num)
        {
            return new int[] {};
        }
    }
}