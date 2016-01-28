namespace BitPantry.AssemblyPatcher.PartGenerators
{
    /// <summary>
    /// An incrementing version part generator
    /// </summary>
    class IncrementingVersionPartGenerator : IVersionPartGenerator
    {
        /// <summary>
        /// Accepts the current version part value and increments it by one, ensuring that the value remains the
        /// acceptable version part range. If the value + 1 exceeds the maximum acceptable value, then the value
        /// will be set to 0.
        /// </summary>
        /// <param name="currentPartValue">The current value of the part being incremented</param>
        /// <param name="ctx">The part generation context</param>
        /// <returns>The updated value of the version part</returns>
        public int Generate(int currentPartValue, VersionPartPatchingContext ctx)
        {
            var value = currentPartValue;
            value++;
            if (value > Constants.MaximumVersionNumber) value = 1;

            return value;
        }
    }
}
