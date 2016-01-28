using System.Xml.Serialization;

namespace BitPantry.AssemblyPatcher.Configuration
{
    /// <summary>
    /// A configuration handler item used for defining custom version part generators
    /// </summary>
    [XmlRoot]
    public class VersionPartGeneratorConfigurationItem
    {
        [XmlAttribute("token")]
        public string Token { get; set; }

        [XmlAttribute("type")]
        public string Type { get; set; }
    }
}
