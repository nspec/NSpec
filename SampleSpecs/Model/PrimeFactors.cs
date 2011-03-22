using NSpec;
using System.Collections.Generic;
using System.Linq;


public class PrimeFactors
{
    public IEnumerable<int> Factor(int number)
    {
        if (number == 1 || number == 0) return new int[] { };

        for (int i = 2; i < number; i++)
            if (number % i == 0) return new[] { i }.Concat(Factor(number / i));

        return new[] { number };
    }
}