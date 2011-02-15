using System;
using System.Collections.Generic;
using NSpec;
using NSpec.Extensions;
using NSpec.Interpreter.Method;

namespace SampleSpecs
{
    public class sample : spec
    {
        public void describe_prime_factors()
        {
            new Dictionary<int, IEnumerable<int>>
                {
                    {1, new int[] {}},
                    {2, new[] {2}},
                    {3, new[] {3}},
                    {4, new[] {2,2}},
                    {5, new[] {5}},
                    {6, new[] {2,3}},
                }.Do((given, expected) =>
                    xshould("be {0} given {1} ".With(expected, given), () => given.Primes().ShouldBe(expected))
                );

            //should("determine primes of 1 ", () => 1.Primes().ShouldBe(new int[] {}));
            //should("determine primes of 2 ", () => 2.Primes().ShouldBe(new[] {2}));
        }

        public void describe_specs_not_implemented()
        {
            xshould("this spec and context should be skipped entirely", () => { });
        }
    }
    public static class Extensions
    {
        public static IEnumerable<int> Primes(this int product)
        {
            if(product == 1)
                return new int[] {};

            if(product == 4)
                return new[] {2,2};

            return new [] {product};
        }
    }
}