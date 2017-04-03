using FluentAssertions;
using NSpec.Domain;
using NSpec.Domain.Formatters;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace NSpec.Tests
{
    [TestFixture]
    public class describe_XUnitFormatter
    {
        class xunit_formatter_sample_spec : nspec
        {
            void a_context_with_a_pending_example()
            {
                it["pending example"] = todo;
            }

            void a_context_with_a_grandchild_example()
            {
                context["a context with an example"] = () =>
                {
                    it["is passing"] = () => Assert.That(true, Is.True);
                };
            }

            void a_context_without_an_example() { }
        }

        [SetUp]
        public void Setup()
        {
            formatter = new XUnitFormatter();

            string outDirPath = Path.Combine(
                Path.GetTempPath(),
                "NSpec.Tests",
                nameof(describe_XUnitFormatter));

            Directory.CreateDirectory(outDirPath);

            outFilePath = Path.Combine(
                outDirPath,
                Path.ChangeExtension(Path.GetRandomFileName(), "xml"));

            formatter.Options = new Dictionary<string, string>()
            {
                { "file", outFilePath },
            };

            var invocation = new RunnerInvocation(
                dll: typeof(describe_XUnitFormatter).GetTypeInfo().Assembly.Location,
                tags: typeof(xunit_formatter_sample_spec).Name,
                formatter: formatter,
                failFast: false);

            contexts = invocation.Run();
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(outFilePath))
            {
                File.Delete(outFilePath);
            }
        }

        [Test]
        public void all_output_is_flushed_to_file()
        {
            string actual = File.ReadAllText(outFilePath);

            actual.Should().EndWith("</testsuite></testsuites>\r\n");
        }

        [Test]
        public void output_file_starts_with_utf16_bom()
        {
            var utf16Encoding = new UnicodeEncoding(bigEndian: false, byteOrderMark: true);

            byte[] expected = utf16Encoding.GetPreamble();

            byte[] actual = new byte[expected.Length];

            using (var fstream = new FileStream(outFilePath, FileMode.Open))
            {
                fstream.Read(actual, 0, actual.Length);

                actual.ShouldBeEquivalentTo(expected);
            }
        }

        XUnitFormatter formatter;
        string outFilePath;
        ContextCollection contexts;
    }
}
