using DotNetTestNSpec;
using DotNetTestNSpec.Parsing;
using FluentAssertions;
using NUnit.Framework;
using System;

namespace DotNetTestNSpecSpecs.Parsing
{
    public class describe_ArgumnetParser
    {
        protected CommandLineOptions actual = null;

        protected const string projectValue = @"Path\To\Some\Project";
    }

    [TestFixture]
    [Category("ArgumentParser")]
    public class when_only_dotnet_test_args_found : describe_ArgumnetParser
    {
        [SetUp]
        public void setup()
        {
            string[] args =
            {
                projectValue,
                "--parentProcessId", "123",
                "--port", "456",
            };

            var parser = new ArgumentParser();

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_dotnet_test_args_only()
        {
            var expected = new CommandLineOptions()
            {
                Project = projectValue,
                ParentProcessId = 123,
                Port = 456,
                NSpecArgs = new string[0],
                UnknownArgs = new string[0],
            };

            actual.ShouldBeEquivalentTo(expected);
        }
    }

    [TestFixture]
    [Category("ArgumentParser")]
    public class when_dotnet_test_project_arg_missing : describe_ArgumnetParser
    {
        [SetUp]
        public void setup()
        {
            string[] args =
            {
                "--parentProcessId", "123",
                "--port", "456",
            };

            var parser = new ArgumentParser();

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_args_with_null_project()
        {
            var expected = new CommandLineOptions()
            {
                Project = null,
                ParentProcessId = 123,
                Port = 456,
                NSpecArgs = new string[0],
                UnknownArgs = new string[0],
            };

            actual.ShouldBeEquivalentTo(expected);
        }
    }

    [TestFixture]
    [Category("ArgumentParser")]
    public class when_some_dotnet_test_arg_missing : describe_ArgumnetParser
    {
        [SetUp]
        public void setup()
        {
            string[] args =
            {
                projectValue,
                "--parentProcessId", "123",
            };

            var parser = new ArgumentParser();

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_found_args_only()
        {
            var expected = new CommandLineOptions()
            {
                Project = projectValue,
                ParentProcessId = 123,
                Port = null,
                NSpecArgs = new string[0],
                UnknownArgs = new string[0],
            };

            actual.ShouldBeEquivalentTo(expected);
        }
    }

    [TestFixture]
    [Category("ArgumentParser")]
    public class when_dotnet_test_arg_value_missing : describe_ArgumnetParser
    {
        ArgumentParser parser = null;
        string[] args = null;

        [SetUp]
        public void setup()
        {
            args = new string[]
            {
                projectValue,
                "--parentProcessId", "123",
                "--port",
            };

            parser = new ArgumentParser();
        }

        [Test]
        public void it_should_throw()
        {
            Assert.Throws<ArgumentException>(() => parser.Parse(args));
        }
    }

    [TestFixture]
    [Category("ArgumentParser")]
    public class when_dotnet_test_and_nspec_args_found : describe_ArgumnetParser
    {
        [SetUp]
        public void setup()
        {
            string[] args =
            {
                projectValue,
                "--parentProcessId", "123",
                "--port", "456",
                "--",
                "SomeClassName",
                "--tag",
                "tag1,tag2,tag3",
            };

            var parser = new ArgumentParser();

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_dotnet_test_and_nspec_args()
        {
            var expected = new CommandLineOptions()
            {
                Project = projectValue,
                ParentProcessId = 123,
                Port = 456,
                NSpecArgs = new string[]
                {
                    "SomeClassName",
                    "--tag",
                    "tag1,tag2,tag3",
                },
                UnknownArgs = new string[0],
            };

            actual.ShouldBeEquivalentTo(expected);
        }
    }

    [TestFixture]
    [Category("ArgumentParser")]
    public class when_no_nspec_args_found_after_separator : describe_ArgumnetParser
    {
        [SetUp]
        public void setup()
        {
            string[] args =
            {
                projectValue,
                "--parentProcessId", "123",
                "--port", "456",
                "--",
            };

            var parser = new ArgumentParser();

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_dotnet_test_args_only()
        {
            var expected = new CommandLineOptions()
            {
                Project = projectValue,
                ParentProcessId = 123,
                Port = 456,
                NSpecArgs = new string[0],
                UnknownArgs = new string[0],
            };

            actual.ShouldBeEquivalentTo(expected);
        }
    }

    [TestFixture]
    [Category("ArgumentParser")]
    public class when_unknown_args_found_before_separator : describe_ArgumnetParser
    {
        [SetUp]
        public void setup()
        {
            string[] args =
            {
                projectValue,
                "unknown1",
                "--parentProcessId", "123",
                "--port", "456",
                "unknown2",
            };

            var parser = new ArgumentParser();

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_dotnet_test_and_unknown_args()
        {
            var expected = new CommandLineOptions()
            {
                Project = projectValue,
                ParentProcessId = 123,
                Port = 456,
                NSpecArgs = new string[0],
                UnknownArgs = new string[]
                {
                    "unknown1",
                    "unknown2",
                },
            };

            actual.ShouldBeEquivalentTo(expected);
        }
    }
}
