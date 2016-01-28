using System;
using System.Linq;

namespace BitPantry.AssemblyPatcher
{
    /// <summary>
    /// The pattern to apply when patching the assembly version
    /// </summary>
    public class VersionPattern
    {
        /// <summary>
        /// The major version pattern
        /// </summary>
        public VersionPartPattern Major { get; set; }

        /// <summary>
        /// The minor version pattern
        /// </summary>
        public VersionPartPattern Minor { get; set; }

        /// <summary>
        /// The build number version pattern
        /// </summary>
        public VersionPartPattern Build { get; set; }

        /// <summary>
        /// The revision version pattern
        /// </summary>
        public VersionPartPattern Revision { get; set; }

        /// <summary>
        /// Creats a new version pattern from a given string representation
        /// </summary>
        /// <param name="patternString">The pattern string to parse into the version pattern</param>
        public VersionPattern(string patternString)
        {
            if(string.IsNullOrEmpty(patternString))
                throw new ArgumentException("The pattern string is null or empty");

            var pieces = patternString.Split('.');
            if(pieces.Count() != 4)
                throw new ArgumentException(string.Format("Was expecting exactly 4 elements in the version pattern string, instead found {0} for pattern \"{1}\"", pieces.Count(), patternString));

            Major = new VersionPartPattern(pieces[0]);
            Minor = new VersionPartPattern(pieces[1]);
            Build = new VersionPartPattern(pieces[2]);
            Revision = new VersionPartPattern(pieces[3]);
        }

        /// <summary>
        /// returns the string representation of the version pattern
        /// </summary>
        /// <returns>The string representation of the version pattern</returns>
        public override string ToString()
        {
            return string.Format("{0} :: {1}",
                this.GetType().FullName,
                string.Format("{0}.{1}.{2}.{3}",
                    Major,
                    Minor,
                    Build,
                    Revision));
        }
    }
}