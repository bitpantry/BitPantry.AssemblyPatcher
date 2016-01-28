namespace BitPantry.AssemblyPatcher
{
    public interface IVersionPartGenerator
    {
        /// <summary>
        /// Generates a version part
        /// </summary>
        /// <param name="currentPartValue">The current value of the part</param>
        /// <param name="ctx">The context of the operation</param>
        /// <returns>The result of the generation operation</returns>
        int Generate(int currentPartValue, VersionPartPatchingContext ctx);
    }
}
