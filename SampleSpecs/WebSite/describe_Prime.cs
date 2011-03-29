using NSpec;

class describe_Prime : nspec
{
    void when_factoring_primes()
    {
        new Tuples<int, int[]>
            {
                { 0, new int[] { } },
                { 1, new int[] { } },
                { 2, new[] { 2 } },
                { 3, new[] { 3 } },
                { 4, new[] { 2, 2 } },
                { 5, new[] { 5 } },
                { 6, new[] { 2, 3 } },
                { 7, new[] { 7 }},
                { 8, new[] { 2, 2, 2 } },
                { 9, new[] { 3, 3 } },
            }.Do( (given, expected) => 
                it["{0} should be {1}".With(given, expected)] = () => 
                    Prime.Factors(given).should_be(expected)
            );
    }
}