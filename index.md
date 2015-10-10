---
layout: default
---

NSpec is lambda based testing framework similar to RSpec and Mocha. NSpec puts a heavy emphasis on simplicity and low ceremony.

<ul>
  <li><a href="#helloworld">Getting Started</a></li>
  <li><a href="#why">Why NSpec?</a></li>
  <li><a href="#specifications">Specifications</a></li>
  <li><a href="#before">Befores</a></li>
  <li><a href="#contexts">Contexts</a></li>
  <li><a href="#pending">Pendings</a></li>
  <li><a href="#helpers">Helpers</a></li>
  <li><a href="#act">Act</a></li>
  <li><a href="#class_level">Class Level</a></li>
  <li><a href="#inheritance">Inheritance</a></li>
  <li><a href="#exception">Exceptions</a></li>
  <li><a href="#summary">Summary</a></li>
</ul>

##Getting Started

<ol>
  <li>Create a new C# console applications.</li>
  <li>Set target framework to .Net 4.5.</li>
  <li>Install NSpec via Nuget.</li>
  <li>Install FluentAssertions via Nuget (or whatever assertion library you prefer).</li>
  <li>
    Paste the following code into Program.cs
    <pre>TODO</pre>
  </li>
  <li>Run.</li>
</ol>

##Why NSpec?

###Consistent With Other Lambda Based Testing Frameworks

If you've used any of the following testing frameworks,
you'll feel right at home with NSpec:

- RSpec
- Minitest
- Jasmine
- Mocha

###Less Ceremony

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
            Assert.AreEqual("hello", "hello"); //this is an arbitrary assertion
        }
    }

NSpec uses conventions (any method that contains underscores), making your tests cleaner:

    class describe_NSpec : nspec
    {
        void it_just_works()
        {
            Assert.AreEqual("hello", "hello"); //this is an arbitrary assertion
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
            Assert.AreEqual("hello", "hello");
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
            Assert.AreEqual("hello", "hello");
        }

        [Test]
        public void it_works_here_too()
        {
            Assert.AreEqual("hello", "hello");
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
            "hello".should_be("hello");
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
            "hello".should_be("hello");
        }

        void it_works_here_too()
        {
            "hello".should_be("hello");
        }
    }

However, NSpec *also* provides a more concise option:

class describe_NSpec : nspec
{
    void before_each()
    {
        Console.WriteLine("I run before each test.");
    }

    void it_works_here()
    {
        "hello".should_be("hello");
    }

    void a_category_of_examples()
    {
        before = () =&gt; Console.WriteLine("I run before each test defined in this context.");

        it["also works here"] = () => "hello".should_be("hello");

        it["works here too"] = () => "hello".should_be("hello");
    }
}

<p>
  {% include timestamp.html %}
</p>
