using DotNetTestNSpec;
using DotNetTestNSpec.Parsing;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;

namespace DotNetTestNSpecSpecs.Parsing
{
    public class describe_NSpecArgumentParser
    {
        protected NSpecCommandLineOptions actual = null;

        protected const string someClassName = @"someClassName";
        protected const string someTags = "tag1,tag2,tag3";
        protected const string someFormatterName = @"someFormatterName";
    }

    [TestFixture]
    [Category("NSpecArgumentParser")]
    public class when_only_nspec_args_found : describe_NSpecArgumentParser
    {
        [SetUp]
        public void setup()
        {
            string[] args =
            {
                someClassName,
                "--tag", someTags,
                "--failfast",
                "--formatter=" + someFormatterName,
                "--formatterOptions:optName1=optValue1",
                "--formatterOptions:optName2",
                "--formatterOptions:optName3=optValue3",
            };

            var parser = new NSpecArgumentParser();

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_nspec_args_only()
        {
            var expected = new NSpecCommandLineOptions()
            {
                ClassName = someClassName,
                Tags = someTags,
                FailFast = true,
                FormatterName = someFormatterName,
                FormatterOptions = new Dictionary<string, string>()
                {
                    { "optName1", "optValue1" },
                    { "optName2", "optName2" },
                    { "optName3", "optValue3" },
                },
                UnknownArgs = new string[0],
            };

            actual.ShouldBeEquivalentTo(expected);
        }
    }

    [TestFixture]
    [Category("NSpecArgumentParser")]
    public class when_class_name_arg_missing : describe_NSpecArgumentParser
    {
        [SetUp]
        public void setup()
        {
            string[] args =
            {
                "--tag", someTags,
                "--failfast",
                "--formatter=" + someFormatterName,
                "--formatterOptions:optName1=optValue1",
                "--formatterOptions:optName2",
                "--formatterOptions:optName3=optValue3",
            };

            var parser = new NSpecArgumentParser();

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_args_with_null_class_name()
        {
            var expected = new NSpecCommandLineOptions()
            {
                ClassName = null,
                Tags = someTags,
                FailFast = true,
                FormatterName = someFormatterName,
                FormatterOptions = new Dictionary<string, string>()
                {
                    { "optName1", "optValue1" },
                    { "optName2", "optName2" },
                    { "optName3", "optValue3" },
                },
                UnknownArgs = new string[0],
            };

            actual.ShouldBeEquivalentTo(expected);
        }
    }

    [TestFixture]
    [Category("NSpecArgumentParser")]
    public class when_tags_arg_missing : describe_NSpecArgumentParser
    {
        [SetUp]
        public void setup()
        {
            string[] args =
            {
                someClassName,
                "--failfast",
                "--formatter=" + someFormatterName,
                "--formatterOptions:optName1=optValue1",
                "--formatterOptions:optName2",
                "--formatterOptions:optName3=optValue3",
            };

            var parser = new NSpecArgumentParser();

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_args_with_empty_tags()
        {
            var expected = new NSpecCommandLineOptions()
            {
                ClassName = someClassName,
                Tags = null,
                FailFast = true,
                FormatterName = someFormatterName,
                FormatterOptions = new Dictionary<string, string>()
                {
                    { "optName1", "optValue1" },
                    { "optName2", "optName2" },
                    { "optName3", "optValue3" },
                },
                UnknownArgs = new string[0],
            };

            actual.ShouldBeEquivalentTo(expected);
        }
    }

    [TestFixture]
    [Category("NSpecArgumentParser")]
    public class when_failfast_arg_missing : describe_NSpecArgumentParser
    {
        [SetUp]
        public void setup()
        {
            string[] args =
            {
                someClassName,
                "--tag", someTags,
                "--formatter=" + someFormatterName,
                "--formatterOptions:optName1=optValue1",
                "--formatterOptions:optName2",
                "--formatterOptions:optName3=optValue3",
            };

            var parser = new NSpecArgumentParser();

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_args_with_failfast_false()
        {
            var expected = new NSpecCommandLineOptions()
            {
                ClassName = someClassName,
                Tags = someTags,
                FailFast = false,
                FormatterName = someFormatterName,
                FormatterOptions = new Dictionary<string, string>()
                {
                    { "optName1", "optValue1" },
                    { "optName2", "optName2" },
                    { "optName3", "optValue3" },
                },
                UnknownArgs = new string[0],
            };

            actual.ShouldBeEquivalentTo(expected);
        }
    }

    [TestFixture]
    [Category("NSpecArgumentParser")]
    public class when_formatter_arg_missing : describe_NSpecArgumentParser
    {
        [SetUp]
        public void setup()
        {
            string[] args =
            {
                someClassName,
                "--tag", someTags,
                "--failfast",
                "--formatterOptions:optName1=optValue1",
                "--formatterOptions:optName2",
                "--formatterOptions:optName3=optValue3",
            };

            var parser = new NSpecArgumentParser();

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_args_with_null_formatter()
        {
            var expected = new NSpecCommandLineOptions()
            {
                ClassName = someClassName,
                Tags = someTags,
                FailFast = true,
                FormatterName = null,
                FormatterOptions = new Dictionary<string, string>()
                {
                    { "optName1", "optValue1" },
                    { "optName2", "optName2" },
                    { "optName3", "optValue3" },
                },
                UnknownArgs = new string[0],
            };

            actual.ShouldBeEquivalentTo(expected);
        }
    }

    [TestFixture]
    [Category("NSpecArgumentParser")]
    public class when_formatter_options_arg_missing : describe_NSpecArgumentParser
    {
        [SetUp]
        public void setup()
        {
            string[] args =
            {
                someClassName,
                "--tag", someTags,
                "--failfast",
                "--formatter=" + someFormatterName,
            };

            var parser = new NSpecArgumentParser();

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_args_with_empty_formatter_options()
        {
            var expected = new NSpecCommandLineOptions()
            {
                ClassName = someClassName,
                Tags = someTags,
                FailFast = true,
                FormatterName = someFormatterName,
                FormatterOptions = new Dictionary<string, string>(),
                UnknownArgs = new string[0],
            };

            actual.ShouldBeEquivalentTo(expected);
        }
    }

    [TestFixture]
    [Category("NSpecArgumentParser")]
    public class when_unknown_args_found_after_classname : describe_NSpecArgumentParser
    {
        [SetUp]
        public void setup()
        {
            string[] args =
            {
                someClassName,
                "unknown1",
                "--tag", someTags,
                "--failfast",
                "unknown2",
                "--formatter=" + someFormatterName,
                "--formatterOptions:optName1=optValue1",
                "--formatterOptions:optName2",
                "--formatterOptions:optName3=optValue3",
                "unknown3",
            };

            var parser = new NSpecArgumentParser();

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_nspec_and_unknown_args()
        {
            var expected = new NSpecCommandLineOptions()
            {
                ClassName = someClassName,
                Tags = someTags,
                FailFast = true,
                FormatterName = someFormatterName,
                FormatterOptions = new Dictionary<string, string>()
                {
                    { "optName1", "optValue1" },
                    { "optName2", "optName2" },
                    { "optName3", "optValue3" },
                },
                UnknownArgs = new string[]
                {
                    "unknown1",
                    "unknown2",
                    "unknown3",
                },
            };

            actual.ShouldBeEquivalentTo(expected);
        }
    }
}
