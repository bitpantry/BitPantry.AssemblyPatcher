using System.Collections.Generic;
using System.Xml.Serialization;

namespace BitPantry.AssemblyPatcher.Configuration
{
    /// <summary>
    /// A custom configuration section handler used to define custom version part generators
    /// </summary>
    [XmlRoot]
    public class AssemblyPatcherConfiguration : ConfigurationHandler
    {
        [XmlArray("partGenerators")]
        [XmlArrayItem("add")]
        public List<VersionPartGeneratorConfigurationItem> Items { get; set; }
    }
}
