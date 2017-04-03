using NSpec.Compatibility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NSpec.Domain.Formatters
{
    public class XUnitFormatter : IFormatter
    {
        public XUnitFormatter()
        {
            Options = new Dictionary<string, string>();
        }

        public void Write(ContextCollection contexts)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            XmlWriteWrapper xmlWrapper = new XmlWriteWrapper(sw);

            var xml = xmlWrapper.Xml;

            xml.WriteStartElement("testsuites");
            xml.WriteAttributeString("tests", contexts.Examples().Count().ToString());
            xml.WriteAttributeString("errors", "0");
            xml.WriteAttributeString("failures", contexts.Failures().Count().ToString());
            xml.WriteAttributeString("skip", contexts.Pendings().Count().ToString());

            contexts.Do(c => this.BuildContext(xmlWrapper, c));

            xml.WriteEndElement();
            xml.Flush();

            var results = sb.ToString();
            bool didWriteToFile = false;
            if (Options.ContainsKey("file"))
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), Options["file"]);

                using (var stream = new FileStream(filePath, FileMode.Create))
                using (var writer = new StreamWriter(stream, Encoding.Unicode))
                {
                    writer.WriteLine(results);
                    Console.WriteLine("Test results published to: {0}".With(filePath));
                }

                didWriteToFile = true;
            }
            if (didWriteToFile && Options.ContainsKey("console"))
                Console.WriteLine(results);

            if (!didWriteToFile)
                Console.WriteLine(results);
        }

        public IDictionary<string, string> Options { get; set; }

        void BuildContext(XmlWriteWrapper xmlWrapper, Context context)
        {
            var xml = xmlWrapper.Xml;

            if (context.Level == 1)
            {
                xml.WriteStartElement("testsuite");
                xml.WriteAttributeString("tests", context.AllExamples().Count().ToString());
                xml.WriteAttributeString("name", context.Name);
                xml.WriteAttributeString("errors", "0");
                xml.WriteAttributeString("failures", context.Failures().Count().ToString());
            }

            context.Examples.Do(e => this.BuildSpec(xmlWrapper, e));
            context.Contexts.Do(c => this.BuildContext(xmlWrapper, c));

            if (context.Level == 1)
            {
                xml.WriteEndElement();
            }
        }

        void BuildSpec(XmlWriteWrapper xmlWrapper, ExampleBase example)
        {
            var xml = xmlWrapper.Xml;

            xml.WriteStartElement("testcase");

            string testName = example.Spec;
            StringBuilder className = new StringBuilder();
            ComposePartialName(example.Context, className);

            xml.WriteAttributeString("classname", className.ToString());
            xml.WriteAttributeString("name", testName);
            xml.WriteAttributeString("time", example.Duration.TotalSeconds.ToString("F2"));

            if (example.Exception != null)
            {
                xml.WriteStartElement("failure");
                xml.WriteAttributeString("type", example.Exception.GetType().Name);
                xml.WriteAttributeString("message", example.Exception.Message);
                xml.WriteString(example.Exception.ToString());
                xml.WriteEndElement();
            }

            if (!string.IsNullOrWhiteSpace(example.CapturedOutput))
            {
                xml.WriteStartElement("system-out");
                xml.WriteCData("\n" + example.CapturedOutput);
                xml.WriteEndElement();
            }

            xml.WriteEndElement();
        }

        private void ComposePartialName(Context context, StringBuilder contextName)
        {
            if (context.Level <= 1) { return; }

            ComposePartialName(context.Parent, contextName);
            if (contextName.Length > 0) { contextName.Append(", "); }

            contextName.Append(context.Name);
        }
    }
}
