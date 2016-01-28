namespace BitPantry.AssemblyPatcher.Tests.TestGenerators
{
    /// <summary>
    /// Used to test the version part generation context and simply passed the current part value through
    /// </summary>
    public class TestGenerator : IVersionPartGenerator
    {
        public int Generate(int currentPartValue, VersionPartPatchingContext ctx)
        {
            return currentPartValue;
        }
    }
}
