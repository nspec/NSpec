using System.Collections.Generic;
using System.Linq;


public static class PrimeFactors
{
    public static IEnumerable<int> Primes(this int number)
    {
        if (number == 1 || number == 0) return new int[] { };

        for (int i = 2; i < number; i++)
            if (number % i == 0) return new[] { i }.Concat(Primes(number / i));

        return new[] { number };
    }
}