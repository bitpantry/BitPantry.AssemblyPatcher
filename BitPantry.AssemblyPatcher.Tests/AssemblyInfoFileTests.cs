using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BitPantry.AssemblyPatcher.Tests
{
    [TestClass]
    public class AssemblyInfoFileTests
    {
        [TestMethod]
        public void LoadAssemblyInfoFile_LoadedAssemblyInfoFile()
        {
            var file = new AssemblyInfoFile(Constants.AssemblyInfoFilePath);

            Assert.IsTrue(System.IO.File.Exists(file.AssemblyInfoFilePath));

            Assert.IsTrue(file.AssemblyVersion.ToString() == "[assembly: AssemblyVersion(\"1.2.3.4\")]");
            Assert.IsTrue(file.AssemblyVersion.Version.Major == 1);
            Assert.IsTrue(file.AssemblyVersion.Version.Minor == 2);
            Assert.IsTrue(file.AssemblyVersion.Version.Build == 3);
            Assert.IsTrue(file.AssemblyVersion.Version.Revision == 4);

            Assert.IsTrue(file.AssemblyFileVersion.ToString() == "[assembly: AssemblyFileVersion(\"5.6.7.8\")]");
            Assert.IsTrue(file.AssemblyFileVersion.Version.Major == 5);
            Assert.IsTrue(file.AssemblyFileVersion.Version.Minor == 6);
            Assert.IsTrue(file.AssemblyFileVersion.Version.Build == 7);
            Assert.IsTrue(file.AssemblyFileVersion.Version.Revision == 8);
        }

    }
}
