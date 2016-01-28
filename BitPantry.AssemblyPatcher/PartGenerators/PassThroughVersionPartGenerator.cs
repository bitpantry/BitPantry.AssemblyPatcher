namespace BitPantry.AssemblyPatcher.PartGenerators
{
    /// <summary>
    /// Simply returns the current part value
    /// </summary>
    public class PassThroughVersionPartGenerator : IVersionPartGenerator
    {
        public int Generate(int currentPartValue, VersionPartPatchingContext ctx)
        {
            return currentPartValue;
        }
    }
}
