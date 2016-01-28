using System;
using System.Collections.Generic;
using System.Linq;

namespace BitPantry.AssemblyPatcher
{
    /// <summary>
    /// The version type of an assembly info version
    /// </summary>
    public enum VersionType
    {
        AssemblyVersion,
        AssemblyFileVersion
    }

    /// <summary>
    /// Represents an assembly info version component of an AssemblyInfo.cs file. i.e., "[assembly: AssemblyVersion("1.0.0.0")]"
    /// </summary>
    public class AssemblyInfoVersion
    {
        /// <summary>
        /// The version type of the assembly info version being represented
        /// </summary>
        public VersionType Type { get; set; }

        /// <summary>
        /// The version being represented
        /// </summary>
        public Version Version { get; set; }

        /// <summary>
        /// The line number index in the target file contents where this version is located
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// Instantiates a new version of the AssemblyInfoVersion for the target file contents. The constructor locates the line
        /// number containing the target version type and loads the type into memory for patching.
        /// </summary>
        /// <param name="targetFileContents">The target file lines</param>
        /// <param name="type">The type of version to load</param>
        public AssemblyInfoVersion(string[] targetFileContents, VersionType type)
        {
            Type = type;

            var searchString = string.Format("{0}assembly: {1}(\"", AppConfigFacade.AttributeDemarcationCharacters[0], type);
            var targetLine = targetFileContents.FirstOrDefault(l => l.StartsWith(searchString, StringComparison.OrdinalIgnoreCase));

            if (string.IsNullOrEmpty(targetLine))
                throw new ArgumentException(string.Format("The given target file contents does not have the \"{0}\" assembly version type.", type));

            Index = new List<string>(targetFileContents).IndexOf(targetLine);
            Version = new Version(targetLine.Replace(searchString, string.Empty).Replace(string.Format("\"){0}", AppConfigFacade.AttributeDemarcationCharacters[1]), string.Empty).Trim());
        }

        /// <summary>
        /// Sets the major version
        /// </summary>
        /// <param name="majorVersion">The new major version to set</param>
        public void SetMajorVersion(int majorVersion)
        {
            Version = new Version(majorVersion, Version.Minor, Version.Build, Version.Revision);
        }

        /// <summary>
        /// Sets the minor version
        /// </summary>
        /// <param name="minorVersion">The new minor version to set</param>
        public void SetMinorVersion(int minorVersion)
        {
            Version = new Version(Version.Major, minorVersion, Version.Build, Version.Revision);
        }

        /// <summary>
        /// Sets the build number
        /// </summary>
        /// <param name="buildNumber">The new build number to set</param>
        public void SetBuildNumber(int buildNumber)
        {
            Version = new Version(Version.Major, Version.Minor, buildNumber, Version.Revision);
        }

        /// <summary>
        /// Sets the revision
        /// </summary>
        /// <param name="revision">The new revision to set</param>
        public void SetRevision(int revision)
        {
            Version = new Version(Version.Major, Version.Minor, Version.Build, revision);
        }

        /// <summary>
        /// Serializes as the string representation ofthe assembly version line
        /// </summary>
        /// <returns>The string representation ofthe assembly version line</returns>
        public override string ToString()
        {
            return string.Format("{0}assembly: {1}(\"{2}.{3}.{4}.{5}\"){6}",
                AppConfigFacade.AttributeDemarcationCharacters[0],
                Type,
                Version.Major,
                Version.Minor,
                Version.Build,
                Version.Revision,
                AppConfigFacade.AttributeDemarcationCharacters[1]);
        }
    }
}
