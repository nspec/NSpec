---
layout: default
---

NSpec is lambda based testing framework similar to RSpec and Mocha.

##Getting Started
<hr />

<ol>
  <li>Create a new C# console applications.</li>
  <li>Set target framework to .Net 4.5.</li>
  <li>Install NSpec via Nuget.</li>
  <li>Install FluentAssertions via Nuget (or whatever assertion library you prefer).</li>
  <li>
    Paste the following code into Program.cs: <br/><br/>
    <script src="https://gist.github.com/amirrajan/7ee4777788e6a5d76fee.js"></script>
  </li>
  <li>Run the exe. Customize it as you wish.</li>
</ol>

##Why NSpec?
<hr />

###Consistent With Other Lambda Based Testing Frameworks
<hr />

If you've used RSpec, Mocha, FunSpec, or Jasmine, you'll feel right at home with NSpec.

###Less Ceremony
<hr />

- No need for access modifiers on tests.
- NSpec test can live side by side with test from other testing frameworks (in the same project).
- No need to decorate tests with attributes.
- Easy transition from xUnit, NUnit, and MSTest frameworks.

NUnit, XUnit, MSTest _force_ you to place attributes and public access modifiers in your test suites:

    [TestFixture]
    public class describe_NUnit
    {
        [Test]
        public void it_just_works()
        {
            Assert.True();
        }
    }

NSpec uses conventions (any method that contains underscores), making your tests cleaner:

    class describe_NSpec : nspec
    {
        void it_just_works()
        {
            Assert.True();
        }
    }

Sometimes just being able to added another test case within a method body is good enough. This will defer the need to create a complex inheritance hierarchy for your tests.

NUnit, XUnit, MSTest _force_ inheritance if you have shared setup:

    [TestFixture]
    public class describe_NUnit
    {
        [SetUp]
        public void before_each()
        {
            Console.WriteLine("I run before each test.");
        }

        [Test]
        public void it_works_here()
        {
            Assert.True();
        }
    }

    [TestFixture]
    public class category_of_examples : describe_NUnit
    {
        [SetUp]
        public void before_each_for_this_context()
        {
            Console.WriteLine("I run before each test defined in this context.");
        }

        [Test]
        public void it_also_works_here()
        {
            Assert.True();
        }
    }

NSpec can do that too (with less code):

    class describe_NSpec : nspec
    {
        void before_each()
        {
            Console.WriteLine("I run before each test.");
        }

        void it_works_here()
        {
            Assert.True();
        }
    }

    class category_of_examples : describe_NSpec
    {
        void before_each()
        {
            Console.WriteLine("I run before each test defined in this context.");
        }

        void it_also_works_here()
        {

            Assert.True();
        }
    }

However, NSpec *also* provides a more concise option (inline lambdas):

    class describe_NSpec : nspec
    {
        void before_each()
        {
            Console.WriteLine("I run before each test.");
        }

        void it_works_here()
        {
            Assert.True();
        }

        void a_category_of_examples()
        {
            before = () =>
                Console.WriteLine("I run before each test defined in this context.");

            it["also works here"] = () => Assert.True();
        }
    }
