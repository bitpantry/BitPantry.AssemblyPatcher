using System;
using System.IO;
using BitPantry.AssemblyPatcher.Configuration;
using BitPantry.AssemblyPatcher.PartGenerators;

namespace BitPantry.AssemblyPatcher
{

    /// <summary>
    /// A utility class for accessing all version patching logic
    /// </summary>
    public static class VersionPatcher
    {
        static readonly VersionPartGeneratorCollection Generators = new VersionPartGeneratorCollection();

        static VersionPatcher()
        {
            // initialize generators

            Generators.Add(Constants.IncrementingVersionPartGeneratorToken, typeof(IncrementingVersionPartGenerator));
            Generators.Add(Constants.TfsChangeSetIdVersionPartGeneratorToken, typeof(TfsChangeSetIdVersionPartGenerator));
            Generators.Add(Constants.PassThroughVersionPartGenerationToken, typeof(PassThroughVersionPartGenerator));

            // load configured parsers

            var generatorAppConfig = (AssemblyPatcherConfiguration)
                System.Configuration.ConfigurationManager.GetSection(
                    Constants.VersionPartGeneratorConfigurationSectionName);
            if (generatorAppConfig == null) return;

            foreach (var item in generatorAppConfig.Items)
                Generators.Add(item.Token, item.Type);
        }

        /// <summary>
        /// Patches the assembly indicated by the provided path information
        /// </summary>
        /// <param name="solutionDirectoryPath">The directory path to the solution</param>
        /// <param name="targetProjectFilePath">The file path to the target csproj of the project to patch</param>
        /// <param name="versionPattern">The version format - i.e., "1.0.{token}.{token}"</param>
        /// <param name="assemblyInfoFilePath">If the assembly info file is different than the default filename 
        /// and location, provide the full path and filename</param>
        /// <returns>The patched assembly info file object</returns>
        public static AssemblyInfoFile Patch(
            string solutionDirectoryPath, 
            string targetProjectFilePath, 
            string versionPattern,
            string assemblyInfoFilePath = null)
        {
            // validate solution directory path

            if (!Directory.Exists(solutionDirectoryPath))
                throw new ArgumentException(string.Format("Could not find solution root, \"{0}\"", solutionDirectoryPath));

            // validate target project directory path

            if (!File.Exists(targetProjectFilePath))
                throw new ArgumentException(string.Format("Could not find target project file, \"{0}\"",
                    targetProjectFilePath));

            // validate assemblyInfoFilePath

            if (string.IsNullOrEmpty(assemblyInfoFilePath))
                assemblyInfoFilePath = Path.Combine(Path.GetDirectoryName(targetProjectFilePath), "properties",
                    Constants.AssemblyInfoFileName);

            if (!File.Exists(assemblyInfoFilePath))
                throw new ArgumentException(string.Format("Could not find target assembly info file, \"{0}\"",
                    targetProjectFilePath));

            // patch the assembly

            var assemblyInfoFile = new AssemblyInfoFile(assemblyInfoFilePath);
            var pattern = new VersionPattern(versionPattern);
            var ctx = new VersionPartPatchingContext(
                solutionDirectoryPath,
                targetProjectFilePath);

            PatchVersion(assemblyInfoFile.AssemblyVersion, pattern, ctx);
            PatchVersion(assemblyInfoFile.AssemblyFileVersion, pattern, ctx);

            assemblyInfoFile.Save();

            return assemblyInfoFile;
        }

        /// <summary>
        /// Patches a given AssemblyInfoVersion using the given pattern and generation context
        /// </summary>
        /// <param name="version">The AssemblyInfoVersion to patch</param>
        /// <param name="pattern">The pattern to apply when patching the version</param>
        /// <param name="ctx">The version part generation context</param>
        static void PatchVersion(AssemblyInfoVersion version, VersionPattern pattern, VersionPartPatchingContext ctx)
        {
            version.SetMajorVersion(GetVersionPart(version.Version.Major, pattern.Major, ctx));
            version.SetMinorVersion(GetVersionPart(version.Version.Minor, pattern.Minor, ctx));
            version.SetBuildNumber(GetVersionPart(version.Version.Build, pattern.Build, ctx));
            version.SetRevision(GetVersionPart(version.Version.Revision, pattern.Revision, ctx));
        }

        /// <summary>
        /// Gets the version part given the specified pattern part, context, and current value
        /// </summary>
        /// <param name="currentPartValue">The current value of the version part</param>
        /// <param name="partPattern">the pattern part to patch with</param>
        /// <param name="ctx">The generation context</param>
        /// <returns>The newly generated version part</returns>
        static int GetVersionPart(int currentPartValue, VersionPartPattern partPattern, VersionPartPatchingContext ctx)
        {
            return partPattern.Type == VersionPatternElementType.Literal 
                ? int.Parse(partPattern.Part) 
                : GeneratePart(partPattern.Part, currentPartValue, ctx);
        }

        /// <summary>
        /// Generates a version part using the given token and current part value
        /// </summary>
        /// <param name="token">The token from the version format that should be applied</param>
        /// <param name="currentPartValue">The current token value</param>
        /// <param name="ctx">The version part generation context</param>
        /// <returns>The newly generated version part vlalue</returns>
        static int GeneratePart(string token, int currentPartValue, VersionPartPatchingContext ctx)
        {
            var generator = Generators[token];
            if (generator == null)
                throw new InvalidProgramException(string.Format("A {0} could not be found for token {1}",
                    typeof (IVersionPartGenerator).Name, token));

            return generator.Generate(currentPartValue, ctx);
        }

    }
}
