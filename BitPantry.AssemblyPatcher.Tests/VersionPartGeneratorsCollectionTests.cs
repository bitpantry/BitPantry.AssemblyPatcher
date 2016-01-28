using System;
using System.Linq;
using BitPantry.AssemblyPatcher.Configuration;
using BitPantry.AssemblyPatcher.Tests.TestGenerators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BitPantry.AssemblyPatcher.Tests
{
    [TestClass]
    public class VersionPartGeneratorsCollectionTests
    {
        [TestMethod]
        public void InstantiateVersionPartGeneratorsCollection_Instantiated()
        {
            var col = new VersionPartGeneratorCollection();
        }

        [TestMethod]
        [ExpectedException(typeof(System.Collections.Generic.KeyNotFoundException))]
        public void GetGeneratorFromEmptyCollection_KeyNotFoundException()
        {
            var col = new VersionPartGeneratorCollection();
            var gen = col["noElements"];
        }

        [TestMethod]
        public void InstantiateWithCollectionInitializer_InstantiatedAndAdded()
        {
            var col = new VersionPartGeneratorCollection {{Constants.TestVersionPartGeneratorToken, new TestGenerator()}};
            Assert.IsNotNull(col[Constants.TestVersionPartGeneratorToken]);
        }

        [TestMethod]
        [ExpectedException(typeof(VersionElementGeneratorLoadException))]
        public void AddDuplicateToken_VersionElementGeneratorLoadException()
        {
            var col = new VersionPartGeneratorCollection { { Constants.TestVersionPartGeneratorToken, new TestGenerator() } };
            col.Add(Constants.TestVersionPartGeneratorToken, new TestGenerator());
        }

        [TestMethod]
        [ExpectedException(typeof(VersionElementGeneratorLoadException))]
        public void AddNonExistingType_VersionElementGeneratorLoadException()
        {
            var col = new VersionPartGeneratorCollection();
            col.Add(Constants.TestVersionPartGeneratorToken, "noType");
        }

        [TestMethod]
        [ExpectedException(typeof(VersionElementGeneratorLoadException))]
        public void AddInvalidToken_VersionElementGeneratorLoadException()
        {
            var col = new VersionPartGeneratorCollection();
            col.Add("!passThrough", "noType");
        }

        [TestMethod]
        public void LoadCustomConfiguration_Loaded()
        {
            var generatorAppConfig = (AssemblyPatcherConfiguration)
                System.Configuration.ConfigurationManager.GetSection(
                    AssemblyPatcher.Constants.VersionPartGeneratorConfigurationSectionName);

            Assert.IsNotNull(generatorAppConfig);
            
            Assert.IsNotNull(generatorAppConfig.Items);
            Assert.IsTrue(generatorAppConfig.Items.Count() == 1);
            Assert.IsTrue(generatorAppConfig.Items[0].Token == Constants.TestVersionPartGeneratorToken);
            Assert.IsTrue(Type.GetType(generatorAppConfig.Items[0].Type, false, true) == typeof(TestGenerator));
        }
    }
}
