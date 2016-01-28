namespace BitPantry.AssemblyPatcher
{
    /// <summary>
    /// The version pattern element type
    /// </summary>
    public enum VersionPatternElementType
    {
        /// <summary>
        /// Literal version pattern element types are substituted for the current version part value - no other logic is applied
        /// </summary>
        Literal, 

        /// <summary>
        /// Tokens tie to a specific version part generator which should be used to generate the version part
        /// </summary>
        Token
    }

    /// <summary>
    /// Represents a pattern for a single version part
    /// </summary>
    public class VersionPartPattern
    {
        /// <summary>
        /// The raw part value used to instantiate the version part pattern
        /// </summary>
        public string Part { get; private set; }

        /// <summary>
        /// The version pattern element type
        /// </summary>
        public VersionPatternElementType Type { get; private set; }

        /// <summary>
        /// Creates a new VersionPartPattern class using the given version pattern part value
        /// </summary>
        /// <param name="part">The version pattern part value</param>
        public VersionPartPattern(string part)
        {
            Type = VersionPatternElementType.Literal;

            if(part.Trim().StartsWith("{") && part.Trim().EndsWith("}"))
                Type = VersionPatternElementType.Token;

            Part = part;
        }

        /// <summary>
        /// Returns the version pattern part value
        /// </summary>
        /// <returns>The version pattern part value</returns>
        public override string ToString()
        {
            return Part.Trim();
        }
    }
}
