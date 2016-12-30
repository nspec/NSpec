---
layout: page
title:
---

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

### Or ###

Or you can do something even fancier, and build your own console
app! Instead of creating a Class Library for the test project,
create a Console Project. Add the following code in `Program.cs`:

<script src="https://gist.github.com/amirrajan/236cbaafef2c7c2195b47c41cbf9c918.js"></script>

Then you can debug everything like you would any other program. More
importantly, creating your own console app gives you the _power_ to
tailor input and output to your liking using NSpecs's API/constructs.
