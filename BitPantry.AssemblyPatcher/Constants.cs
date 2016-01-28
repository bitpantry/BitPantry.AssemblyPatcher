namespace BitPantry.AssemblyPatcher
{
    /// <summary>
    /// Global application constants
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The name of the version part generators custom configuration section
        /// </summary>
        public static readonly string VersionPartGeneratorConfigurationSectionName = "assemblyPatcher";

        /// <summary>
        /// The maximum version part value possible
        /// </summary>
        public readonly static int MaximumVersionNumber = 65535; // http://blogs.msdn.com/b/msbuild/archive/2007/01/03/why-are-build-numbers-limited-to-65535.aspx

        /// <summary>
        /// The token for the built in incrementing version part generator
        /// </summary>
        public static readonly string IncrementingVersionPartGeneratorToken = "{increment}";

        /// <summary>
        /// The token for the built in TFS change set id version part generator
        /// </summary>
        public static readonly string TfsChangeSetIdVersionPartGeneratorToken = "{tfsChangeSet}";

        /// <summary>
        /// The token for the built in pass through version part generator
        /// </summary>
        public static readonly string PassThroughVersionPartGenerationToken = "{#}";

        /// <summary>
        /// The default name for the AssemblyInfo.cs file
        /// </summary>
        public static readonly string AssemblyInfoFileName = "AssemblyInfo.cs";
    }
}
