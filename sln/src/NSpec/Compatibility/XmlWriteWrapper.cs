using System.IO;
using System.Xml;

namespace NSpec.Compatibility
{
    public class XmlWriteWrapper
    {
        public XmlWriteWrapper(StringWriter sw)
        {
#if NET451
            Xml = new XmlTextWriter(sw);
#else
            Xml = XmlWriter.Create(sw);
#endif
        }

#if NET451
        public XmlTextWriter Xml { get; private set; }
#else
        public XmlWriter Xml { get; private set; }
#endif
    }
}
