---
layout: page
title:
---


## Table of Contents ##
<hr />

- [Getting Started](#getting-started)
- [Why NSpec?](#why-nspec)
  * [Consistent With Modern Testing Frameworks](#consistent-with-modern-testing-frameworks)
  * [Noise Free Tests](#noise-free-tests)
  * [Fluid Test Structures](#fluid-test-structures)
- [Features](#features)
  * [Assertions](#assertions)
  * [Before](#before)
  * [Context](#context)
  * [Pending Tests](#pending-tests)
  * [Helper Methods](#helper-methods)
  * [Act](#act)
  * [Inheritance](#inheritance)
  * [Class Level](#class-level)
  * [Debugger Support](#debugger-support)
  * [Console App](#console-app)
- [Async/await support](#asyncawait-support)
  * [Class Level](#class-level-1)
  * [Context level](#context-level)
- [Data-driven test cases](#data-driven-test-cases)
- [Additional info](#additional-info)
  * [Order of execution](#order-of-execution)

## Getting Started ##
<hr />

- Open Visual Studio.
- Create a class library project.
- Open the Package Manager Console and type:

```
Install-Package nspec
```

```
Install-Package FluentAssertions
```

- Create a class file called `my_first_spec.cs` and put the following code in it:

<script src="https://gist.github.com/amirrajan/573054053513fd7fbfe5430127212c9b.js"></script>

- Then run the tests using `NSpecRunner.exe`:

```
>NSpecRunner.exe YourClassLibraryName\bin\debug\YourClassLibraryName.dll
my first spec
  asserts at the method level
  describe nesting
    asserts in a method
    more nesting
      also asserts in a lambda
```

## Why NSpec? ##
<hr />

### Consistent With Modern Testing Frameworks ##
<hr />

If you've used any of the following testing frameworks, you'll feel
right at home with NSpec:

- RSpec
- Minitest
- Jasmine
- Mocha
- FunSpec

### Noise Free Tests ##
<hr />

In NSpec, there is no need for access modifiers on tests, and no need to decorate test methods with attributes.

For example, this NUnit/XUnit test:

<script src="https://gist.github.com/amirrajan/83ce1df8d7e2b077da0d7ef2e51f9d14.js"></script>

Would be written like this in NSpec (notice that there are no access
modifiers or attributes):

<script src="https://gist.github.com/amirrajan/5ba036142600fe75c658e4ff31630ff7.js"></script>

### Fluid Test Structures ###
<hr />

You can nest a lambda within a method (you can defer inheritance hierarchies):

Here is an NSpec test that has nested structures:

<script src="https://gist.github.com/amirrajan/48da608ece38990add614549ec33d4eb.js"></script>

Here is what you'd have to write the test above in NUnit/XUnit. It's
gross and poopy. Specifically:

- Attributes on every class.
- Attributes for setup methods and test methods.
- Access modifiers on every class and method.
- Setup method names have to be unique to ensure there are no
  collisions in the inheritance hierarchy.
- Inheritance hierarchy isn't visually obvious.

<script src="https://gist.github.com/amirrajan/7934a30a49ef52d35d566c050c393139.js"></script>

## Features ##
<hr />

Lets take a look at some features of NSpec.

### Assertions ###
<hr />

NSpec has some simple assertions, but you should really just use
[FluentAssertions](http://www.fluentassertions.com/). You can build
your own assertions by using extention
methods. For example:

<script src="https://gist.github.com/amirrajan/6710f0237a101487ebf5dac882da45c0.js"></script>

### Before ###
<hr />

Want to do some setup before tests are run? Use `before`. The state of
the class is reset with each test case (side effects/mutations don't spill over).

<script src="https://gist.github.com/amirrajan/b488af0929424f67588e3aa4be757c2c.js"></script>

### Context ###
<hr />

Test hierarchies are communicated through the `context` keyword. If a
method contains underscores, then it will be picked up by NSpec. Any
method that starts with `it_` or `specify_` will be treated as just a
simple NUnit/XUnit style test case.

<script src="https://gist.github.com/amirrajan/c30019b07bb13958b85f3fdcb07690b8.js"></script>

### Pending Tests ###
<hr />

You can ignore tests by preceding any structure with an `x`. Or you can
use the `todo` keyword provided by NSpec.

<script src="https://gist.github.com/amirrajan/ebd12173b818c21b093ec6a081d8e7a5.js"></script>

### Helper Methods ###
<hr />

Title cased (conventional C#) methods are ignored my NSpec.

<script src="https://gist.github.com/amirrajan/cb3087a12310cd29842d300391e4c873.js"></script>

### Act ###
<hr />

Here's a fancy feature. Sometimes, what is done to a class remains the
same, but the setup varies. You can use `act`. Each nested
context will execute `act` before assertions are run.

<script src="https://gist.github.com/amirrajan/3c35123d0052f794a12afda2f75477a9.js"></script>

### Inheritance ###
<hr />

Being able to nest tests is awesome. But you'll can always use
inheritance to "flatten" tests if needed.

<script src="https://gist.github.com/amirrajan/23193314d218c5ec47a38d336325a913.js"></script>

### Class Level ###
<hr />

All test structures are supported at the class level too. Here is how
you'd write a `before`, `act`, and `it/specify` at the class level.

<script src="https://gist.github.com/amirrajan/a94d9567ff32e78e140edb31c72e7049.js"></script>

### Debugger Support ###
<hr />

If you want to hook into the debugger quickly. Just place the
following line inside of your tests. When you run `NSpecRunner.exe`,
the debugger will pop right up:

```
System.Diagnostics.Debugger.Launch()
```

NSpec also includes `DebuggerShim.cs` when you install it via Nuget. So you can use TDD.NET/ReSharper to run your tests.

### Console App ###
<hr />

Or you can do something even fancier, and build your own console
app! Instead of creating a Class Library for the test project,
create a Console Project. Add the following code in `Program.cs`:

<script src="https://gist.github.com/amirrajan/236cbaafef2c7c2195b47c41cbf9c918.js"></script>

Then you can debug everything like you would any other program. More
importantly, creating your own console app gives you the _power_ to
tailor input and output to your liking using NSpecs's API/constructs.

## Async/await support

Your NSpec tests can run asynchronous code too.

### Class level

At a class level, you still declare hook methods with same names, but they must be *asynchronous* and return `async Task`, instead of `void`:

```c#
public class an_example_with_async_hooks_at_class_level : nspec
{
  // Note the different return type and modifier

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

For all sync test hooks at class level you can find its corresponding async one, just by turning its signature to async:

| Sync  | Async |
| --- | --- |
| `void before_all()` | `async Task before_all()` |
| `void before_each()` | `async Task before_each()` |
| `void act_each()` | `async Task act_each()` |
| `void it_xyz()` | `async Task it_xyz()` |
| `void specify_xyz()` | `async Task specify_xyz()` |
| `void after_each()` | `async Task after_each()` |
| `void after_all()` | `async Task after_all()` |

Throughout the test class you can run both sync and async expectations as needed, so you can freely mix `void it_xyz()` and `async Task it_abc()`.

Given a class context, for each test execution phase (_before all_/ _before_/ _act_/ _after_/ _after all_) you can choose to run either sync or async code according to your needs: so in the same class context you can mix e.g. `void before_all()` with `async Task before_each()`, `void act_each()` and `async Task after_each()`.
What you **can't** do is to assign both sync and async hooks for the same phase, in the same class context: so e.g. the following will not work and break your build at compile time (for the same rules of method overloading):

```c#
public class a_wrong_example_mixing_async_hooks_at_class_level : nspec
{
  // Watch out, this example will not work

  void before_each() // this one, together with ...
  {
    SetupScenario();
  }

  async Task before_each() // ... this other, will cause an error
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

### Context level

At a context and sub-context level, you need to set _asynchronous_ test hooks provided by NSpec, instead of the synchronous ones:

```c#
public class an_example_with_async_hooks_at_context_level : nspec
{
  void given_some_context()
  {
    // Note the 'Async' suffix

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

Given a single context, for each test execution phase (_before all_/ _before_/ _act_/ _after_/ _after all_) you can choose to run either sync or async code according to your needs: so in the same context you can mix e.g. `beforeAll` with `beforeAsync`, `act` and `afterAsync`.
What you **can't** do is to assign both sync and async hooks for the same phase, in the same context: so e.g. the following will not work and throw an exception at runtime:

```c#
public class a_wrong_example_mixing_async_hooks_at_context_level : nspec
{
  // Watch out, this example will not work

  void given_some_scenario()
  {
    // this one, together with ...
    before = () => SetupScenario();

    // ... this other, will cause an error
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

If you want to dig deeper for any level, whether class- or context-, you might directly have a look at how async support is tested in NSpec unit tests.
Just look for `nspec`-derived classes in following files:

* [NSpecSpecs/describe_RunningSpecs/describe_async_*](https://github.com/nspec/NSpec/tree/master/NSpecSpecs/describe_RunningSpecs)
* [NSpecSpecs/describe_RunningSpecs/describe_before_and_after/async_*](https://github.com/nspec/NSpec/tree/master/NSpecSpecs/describe_before_and_after)
* [NSpecSpecs/describe_RunningSpecs/Exceptions/when_async_*](https://github.com/nspec/NSpec/tree/master/NSpecSpecs/describe_RunningSpecs/Exceptions)

## Data-driven test cases

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

## Additional info

### Order of execution

Please have a look at [this wiki page](https://github.com/nspec/NSpec/wiki/Execution-Orders) for an overview on which test hooks are executed when: execution order in xSpec family frameworks can get tricky when dealing with more complicated test configurations, like inherithing from an abstract test class or mixing `before_each` with `before_all` at different context levels.
