using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BitPantry.AssemblyPatcher.Tests
{
    [TestClass]
    public class VersionPatternTests
    {
        [TestMethod]
        public void LoadVersionPattern_VersionPatternLoaded()
        {
            var pattern = new VersionPattern(Constants.VersionPatternString);
           
            Assert.AreEqual(pattern.Major.ToString(), "1");
            Assert.AreEqual(pattern.Major.Type, VersionPatternElementType.Literal);
            Assert.AreEqual(pattern.Major.Part, "1");

            Assert.AreEqual(pattern.Minor.ToString(), AssemblyPatcher.Constants.PassThroughVersionPartGenerationToken);
            Assert.AreEqual(pattern.Minor.Type, VersionPatternElementType.Token);
            Assert.AreEqual(pattern.Minor.Part, AssemblyPatcher.Constants.PassThroughVersionPartGenerationToken);

            Assert.AreEqual(pattern.Build.ToString(), AssemblyPatcher.Constants.IncrementingVersionPartGeneratorToken);
            Assert.AreEqual(pattern.Build.Type, VersionPatternElementType.Token);
            Assert.AreEqual(pattern.Build.Part, AssemblyPatcher.Constants.IncrementingVersionPartGeneratorToken);

            Assert.AreEqual(pattern.Revision.ToString(), Constants.TestVersionPartGeneratorToken);
            Assert.AreEqual(pattern.Revision.Type, VersionPatternElementType.Token);
            Assert.AreEqual(pattern.Revision.Part, Constants.TestVersionPartGeneratorToken);
        }
    }
}
