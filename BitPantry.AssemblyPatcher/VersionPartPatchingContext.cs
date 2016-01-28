namespace BitPantry.AssemblyPatcher
{
    /// <summary>
    /// Provides the context of the generation operation
    /// </summary>
    public class VersionPartPatchingContext
    {
        /// <summary>
        /// The root directory of the solution
        /// </summary>
        public string SolutionRootPath { get; set; }

        /// <summary>
        /// The target project file
        /// </summary>
        public string TargetProjectFile { get; set; }


        public VersionPartPatchingContext(string solutionRootPath, string targetProjectFile)
        {
            SolutionRootPath = solutionRootPath;
            TargetProjectFile = targetProjectFile;
        }

        public override string ToString()
        {
            return string.Format("{0} :: \"{1}\" \"{2}\"",
                this.GetType().FullName,
                SolutionRootPath,
                TargetProjectFile);
        }
    }
}
