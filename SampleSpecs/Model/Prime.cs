using System.Collections.Generic;
using System.Linq;


public static class Prime
{
    public static IEnumerable<int> Factors(int number)
    {
        if (number == 1 || number == 0) return new int[] { };

        for (int i = 2; i < number; i++)
            if (number % i == 0) return new[] { i }.Concat(Factors(number / i));

        return new[] { number };
    }
}