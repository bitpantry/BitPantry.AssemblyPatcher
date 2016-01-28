using System;

namespace BitPantry.AssemblyPatcher
{

    /// <summary>
    /// An exception that represents a version part generator load exception
    /// </summary>
    public class VersionElementGeneratorLoadException : Exception
    {
        /// <summary>
        /// The token that the given type should generate for
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// The type of the version part generator which failed to load
        /// </summary>
        public string ParserType { get; set; }

        public VersionElementGeneratorLoadException(string token, string generatorType, string message) : base(message) { Token = token; ParserType = generatorType; }
        public VersionElementGeneratorLoadException(string token, string generatorType, string message, Exception innerEx) : base(message, innerEx) { Token = token; ParserType = generatorType; }
    }
    
}
