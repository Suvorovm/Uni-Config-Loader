using System.Xml.Serialization;

namespace Config.Descriptor
{
    [XmlRoot("configLoadDescriptor")]
    public class ConfigLoadDescriptor
    {
        [XmlAttribute("provider")]
        public string ActiveProvider;
    }
}