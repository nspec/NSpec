# NSpec

NSpec is a BDD framework for .NET of the xSpec (context/specification) flavor. NSpec is intended to be used to drive development through specifying behavior at the unit level. NSpec is heavily inspired by RSpec and built upon the NUnit assertion library.

NSpec is written by [Matt Florence](http://twitter.com/mattflo) and [Amir Rajan](http://twitter.com/amirrajan). It's shaped and benefited by hard work from our [contributors](https://github.com/mattflo/NSpec/contributors)

## Additional info

### Execution order

Please have a look at [this wiki page](https://github.com/mattflo/NSpec/wiki/Execution-Orders) for an overview on which test hooks are executed when: execution order in xSpec family frameworks can get tricky when dealing with more complicated test configurations, like inherithing from an abstract test class or mixing `before_each` with `before_all` at different context levels.

### Async/await support

Your NSpec tests can run asynchronous code too.

#### Context level

At a class level, you still declare hook methods with same names, but they must be *asynchronous* and return `async Task`, instead of `void`:

```c#
public class an_example_with_async_hooks_at_class_level : nspec
{
  async Task before_each()
  {
    await SetupScenarioAsync();
  }

  async Task act_each()
  {
    await DoActAsync();
  }

  void it_should_do_something()
  {
    DoSomething();
  }

  async Task it_should_do_something_async()
  {
    await DoSomethingAsync();
  }

  void after_each()
  {
    CleanupScenario();
  }

  // ...
}
```

For all sync test hooks at class level you can find its corresponding async one, just by switching its signature:

| Sync  | Async |
| --- | --- |
| `void before_all_each()` | `async Task before_all_each()` |
| `void before_each()` | `async Task before_each()` |
| `void act_each()` | `async Task act_each()` |
| `void it_xyz()` | `async Task it_xyz()` |
| `void specify_xyz()` | `async Task specify_xyz()` |
| `void after_each()` | `async Task after_each()` |
| `void after_all_each()` | `async Task after_all_each()` |

Throughout the test class you can run both sync and async expectations as needed, so you can freely mix `void it_xyz` and `async Task it_abc`.
Given a class context, for each test execution phase (before all/ before/ act/ after/ after all) you can choose to run either sync or async code according to your needs: so in the same class context you can mix e.g. `void before_all()` with `async Task before_each()`, `void act_each()` and `async task after_each()`. 
What you can't do is to assign both sync and async hooks for the same phase, in the same class context: so e.g. the following will not work and break your build at compile time, for the same rules of method overloading:

```c#
public class a_wrong_example_mixing_async_hooks_at_class_level : nspec
{
  // following two together will cause an error

  void before_each()
  {
    SetupScenario();
  }

  async Task before_each()
  {
    await SetupScenarioAsync();
  }

  async Task act_each()
  {
    await DoActAsync();
  }

  void it_should_do_something()
  {
    DoSomething();
  }

  async Task it_should_do_something_async()
  {
    await DoSomethingAsync();
  }

  void after_each()
  {
    CleanupScenario();
  }

  // ...
}
```

#### Context level

At a context and sub-context level, you need to set _asynchronous_ test hooks provided by NSpec, instead of the synchronous ones:

```c#
public class an_example_with_async_hooks_at_context_level : nspec
{
  void given_some_context()
  {
    beforeAsync = async () => await SetupScenarioAsync();

    it["should do something"] = () => DoSomething();

    itAsync["should do something async"] = async () => await DoSomethingAsync();

    context["given some nested scenario"] = () => 
    {
       before = () => SetupNestedScenario();

       actAsync = async () => await DoActAsync();

       itAsync["is not yet implemented async"] = todoAsync;

       afterAsync = async () => await CleanupNestedScenarioAsync();
    };

    after = () => CleanupScenario();
  }

  // ...
}
```

For almost all sync test hooks and helpers you can find its corresponding async one:

| Sync  | Async |
| --- | --- |
| `beforeAll` | `beforeAllAsync` |
| `before` | `beforeAsync` |
| `beforeEach` | `beforeEachAsync` |
| `act` | `actAsync` |
| `it` | `itAsync` |
| `xit` | `xitAsync` |
| `expect` | `expectAsync` |
| `todo` | `todoAsync` |
| `after` | `afterAsync` |
| `afterEach` | `afterEachAsync` |
| `afterAll` | `afterAllAsync` |
| `specify` | Not available |
| `xspecify` | Not available |
| `context` | Not needed, context remains sync |
| `xcontext` | Not needed, context remains sync |
| `describe` | Not needed, context remains sync |
| `xdescribe` | Not needed, context remains sync |

Throughout the whole test class you can run both sync and async expectations as needed, so you can freely mix `it[]` and `itAsync[]`.
Given a single context, for each test execution phase (before all/ before/ act/ after/ after all) you can choose to run either sync or async code according to your needs: so in the same context you can mix e.g. `beforeAll` with `beforeAsync`, `act` and `afterAsync`. 
What you can't do is to assign both sync and async hooks for the same phase, in the same context: so e.g. the following will not work and throw an exception at runtime:

```c#
public class a_wrong_example_mixing_async_hooks_at_context_level : nspec
{
  void given_some_scenario()
  {
    // following two together will cause an error
    before = () => SetupScenario();

    beforeAsync = async () => await SetupScenarioAsync();

    it["should do something sync"] = () => DoSomething();

    itAsync["should do something async"] = async () => await DoSomethingAsync();

    context["given some nested scenario"] = () => 
    {
       before = () => SetupNestedScenario();

       itAsync["is not yet implemented async"] = todoAsync;

       afterAsync = async () => await CleanupNestedScenarioAsync();
    };

    after = () => CleanupScenario();
  }

  // ...
}
```

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

## License

[MIT](license.txt)
