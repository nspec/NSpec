using NSpec.Compatibility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NSpec.Domain.Formatters
{
    public class XmlFormatter : IFormatter
    {
        public void Write(ContextCollection contexts)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            XmlWriteWrapper xmlWrapper = new XmlWriteWrapper(sw);

            var xml = xmlWrapper.Xml;

            xml.WriteStartElement("Contexts");
            xml.WriteAttributeString("TotalSpecs", contexts.Examples().Count().ToString());
            xml.WriteAttributeString("TotalFailed", contexts.Failures().Count().ToString());
            xml.WriteAttributeString("TotalPending", contexts.Pendings().Count().ToString());

            xml.WriteAttributeString("RunDate", DateTime.Now.ToString());
            contexts.Do(c => this.BuildContext(xmlWrapper, c));
            xml.WriteEndElement();

            Console.WriteLine(sb.ToString());
        }

        public IDictionary<string, string> Options { get; set; }

        void BuildContext(XmlWriteWrapper xmlWrapper, Context context)
        {
            var xml = xmlWrapper.Xml;

            xml.WriteStartElement("Context");
            xml.WriteAttributeString("Name", context.Name);

            if (context.Examples.Count > 0)
            {
                xml.WriteStartElement("Specs");
            }
            context.Examples.Do(e => this.BuildSpec(xmlWrapper, e));
            if (context.Examples.Count > 0)
            {
                xml.WriteEndElement();
            }

            context.Contexts.Do(c => this.BuildContext(xmlWrapper, c));

            xml.WriteEndElement();
        }

        void BuildSpec(XmlWriteWrapper xmlWrapper, ExampleBase example)
        {
            var xml = xmlWrapper.Xml;

            xml.WriteStartElement("Spec");
            xml.WriteAttributeString("Name", example.Spec);

            if (example.Exception != null)
            {
                xml.WriteAttributeString("Status", "Failed");
                xml.WriteStartElement("Exception");
                xml.WriteCData(example.Exception.ToString());
                xml.WriteEndElement();
            }
            else if (example.Pending)
            {
                xml.WriteAttributeString("Status", "Pending");
            }
            else
            {
                xml.WriteAttributeString("Status", "Passed");
            }

            xml.WriteEndElement();
        }
    }
}