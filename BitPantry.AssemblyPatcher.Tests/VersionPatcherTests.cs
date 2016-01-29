using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BitPantry.AssemblyPatcher.Tests
{
    [TestClass]
    public class VersionPatcherTests
    {
        [TestMethod]
        public void PatchFile_FilePatched()
        {
            using (var tempAssemblyInfo = new TemporaryFile(Resources.Resources.AssemblyInfo_cs))
            {
                var originalVersion = new AssemblyInfoFile(tempAssemblyInfo.FilePath).AssemblyVersion;

                VersionPatcher.Patch(
                    Constants.SolutionRootTestPath,
                    Constants.TargetProjectFileTestPath,
                    Constants.VersionPatternString,
                    tempAssemblyInfo.FilePath);

                var file = new AssemblyInfoFile(tempAssemblyInfo.FilePath);

                Assert.AreEqual(file.AssemblyVersion.Version.Major, 1); // literal
                Assert.AreEqual(file.AssemblyVersion.Version.Minor, originalVersion.Version.Minor); // pass through
                Assert.AreEqual(file.AssemblyVersion.Version.Build, originalVersion.Version.Build + 1); // incrementing
                Assert.AreEqual(file.AssemblyVersion.Version.Revision, originalVersion.Version.Revision); // test
            }
        }
    }
}
