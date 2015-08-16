using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace NSpec.Domain.Formatters
{
    [Serializable]
    public class XmlFormatter : IFormatter
    {
        public void Write(ContextCollection contexts)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            XmlTextWriter xml = new XmlTextWriter(sw);

            xml.WriteStartElement("Contexts");
            xml.WriteAttributeString("TotalSpecs", contexts.Examples().Count().ToString());
            xml.WriteAttributeString("TotalFailed", contexts.Failures().Count().ToString());
            xml.WriteAttributeString("TotalPending", contexts.Pendings().Count().ToString());

            xml.WriteAttributeString("RunDate", DateTime.Now.ToString());
            contexts.Do(c => this.BuildContext(xml, c));
            xml.WriteEndElement();

            Console.WriteLine(sb.ToString());
        }

        void BuildContext(XmlTextWriter xml, Context context)
        {
            xml.WriteStartElement("Context");
            xml.WriteAttributeString("Name", context.Name);

            if (context.Examples.Count > 0)
            {
                xml.WriteStartElement("Specs");
            }
            context.Examples.Do(e => this.BuildSpec(xml, e));
            if (context.Examples.Count > 0)
            {
                xml.WriteEndElement();
            }

            context.Contexts.Do(c => this.BuildContext(xml, c));

            xml.WriteEndElement();
        }

        void BuildSpec(XmlTextWriter xml, ExampleBase example)
        {
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