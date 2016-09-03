using System.IO;
using System.Xml;

namespace NSpec.Compatibility
{
    public class XmlWriteWrapper
    {
        public XmlWriteWrapper(StringWriter sw)
        {
#if NETSTANDARD1_6
            Xml = XmlWriter.Create(sw);
#else
            Xml = new XmlTextWriter(sw);
#endif
        }

#if NETSTANDARD1_6
        public XmlWriter Xml { get; private set; }
#else
        public XmlTextWriter Xml { get; private set; }
#endif
    }
}
