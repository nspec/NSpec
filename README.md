# NSpec

NSpec is a BDD framework for .NET of the xSpec (context/specification) flavor. NSpec is intended to be used to drive development through specifying behavior at the unit level. NSpec is heavily inspired by RSpec and built upon the NUnit assertion library.

NSpec is written by [Matt Florence](http://twitter.com/mattflo) and [Amir Rajan] (http://twitter.com/amirrajan). It's shaped and benefited by hard work from our [contributors](https://github.com/mattflo/NSpec/contributors)

## Additional info

### Execution order

Please have a look at [this wiki page](https://github.com/mattflo/NSpec/wiki/Execution-Orders) for an overview on which test hooks are executed when: execution order in xSpec family frameworks can get tricky when dealing with more complicated test configurations, like inherithing from an abstract test class or mixing `before_each` with `before_all` at different context levels.

### Data-driven test cases

Test frameworks of the xUnit family have dedicated attributes in order to support data-driven test cases (so-called *theories*). NSpec, as a member of the xSpec family, does not make use of attributes and instead obtains the same result with a set of expectations automatically created through code. In detail, to set up a data-driven test case with NSpec you just: 

1. build a set of data points;
1. name and assign an expectation for each data point by looping though the whole set.

Any NSpec test runner will be able to detect all the (aptly) named expectations and run them. Here you can see a sample test case, where we took advantage of `NSpec.Each<>` class and `NSpec.Do()` extension to work more easily with data point enumeration, and `NSpec.With()` extension to have an easier time composing text:

```c#
public class describe_prime_factors : nspec
{
  void given_first_ten_integer_numbers()
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
```

## Contributing

See [contributing](CONTRIBUTING.md) doc page.
