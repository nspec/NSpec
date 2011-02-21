using System;
using System.Collections.Generic;
using System.Linq;
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
                {2,new[]{2}},
                {3,new[]{3}},
                {4,new[]{2,2}},
                {5,new[]{5}},
                {6,new[]{2,3}},
                {7,new[]{7}},
                {8,new[]{2,2,2}},
                {9,new[]{3,3}},
            }.Do((given, expected) =>
                specify("{0} should be {1}".With(given,expected),() => Primes(given).should_be(expected)));
        }

        private IEnumerable<int> Primes(int num)
        {
            if (num == 1) return new int[] { };

            for(int divisor=2;divisor<num;divisor++)
                if (num % divisor == 0) return new[] {divisor}.Concat(Primes(num/=divisor));

            return new[] { num };
        }
    }
}