using System.Xml;

namespace EventCorp.Clients.Models
{
    public class XmlStreamModel : StreamModel
    {
        public XmlReader CreateXmlReader()
        {
            return XmlReader.Create(Stream);
        }
    }
}