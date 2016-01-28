using System.IO;

namespace BitPantry.AssemblyPatcher
{
    /// <summary>
    /// Used to load an AssemblyInfo.cs file into memory for patching
    /// </summary>
    public class AssemblyInfoFile
    {
        /// <summary>
        /// The path to the AssemblyInfo.cs file
        /// </summary>
        public string AssemblyInfoFilePath { get; private set; }
        
        /// <summary>
        /// Represents the AssemblyVersion attribute in the target file
        /// </summary>
        public AssemblyInfoVersion AssemblyVersion { get; private set; }

        /// <summary>
        /// Represents the AssemblyFileVersion attribute in the target file
        /// </summary>
        public AssemblyInfoVersion AssemblyFileVersion { get; private set; }

        /// <summary>
        /// Creates a new instance of the AssemblyInfoFile class for the target AssemblyInfo.cs file
        /// </summary>
        /// <param name="assemblyInfoFilePath">The path to the target AssemblyInfo.cs file</param>
        public AssemblyInfoFile(string assemblyInfoFilePath)
        {
            AssemblyInfoFilePath = assemblyInfoFilePath;

            var fileContent = File.ReadAllLines(AssemblyInfoFilePath);
            AssemblyVersion = new AssemblyInfoVersion(fileContent, VersionType.AssemblyVersion);
            AssemblyFileVersion = new AssemblyInfoVersion(fileContent, VersionType.AssemblyFileVersion);
        }

        /// <summary>
        /// Persists the current state of the AssemblyInfo.cs file back to the target file location
        /// </summary>
        public void Save()
        {
            var fileContent = File.ReadAllLines(AssemblyInfoFilePath);
            fileContent[AssemblyVersion.Index] = AssemblyVersion.ToString();
            fileContent[AssemblyFileVersion.Index] = AssemblyFileVersion.ToString();

            File.WriteAllLines(AssemblyInfoFilePath, fileContent);
        }

    }
}
