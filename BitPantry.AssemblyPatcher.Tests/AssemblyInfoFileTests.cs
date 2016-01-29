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

            //Assert.IsTrue(System.IO.File.Exists(file.AssemblyInfoFilePath));

            Assert.AreEqual(file.AssemblyVersion.ToString(), "[assembly: AssemblyVersion(\"1.2.3.4\")]");
            Assert.AreEqual(file.AssemblyVersion.Version.Major, 1);
            Assert.AreEqual(file.AssemblyVersion.Version.Minor, 2);
            Assert.AreEqual(file.AssemblyVersion.Version.Build, 3);
            Assert.AreEqual(file.AssemblyVersion.Version.Revision, 4);
            Assert.AreEqual(file.AssemblyFileVersion.ToString(), "[assembly: AssemblyFileVersion(\"5.6.7.8\")]");
            Assert.AreEqual(file.AssemblyFileVersion.Version.Major, 5);
            Assert.AreEqual(file.AssemblyFileVersion.Version.Minor, 6);
            Assert.AreEqual(file.AssemblyFileVersion.Version.Build, 7);
            Assert.AreEqual(file.AssemblyFileVersion.Version.Revision, 8);
        }

    }
}
