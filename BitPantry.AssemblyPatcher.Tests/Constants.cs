namespace BitPantry.AssemblyPatcher.Tests
{
    class Constants
    {

// Use of compiler directive because for some reason AppVeyor can't find the resource under the bin directory
// not sure if it's getting copied or not
#if DEBUG
        public const string AssemblyInfoFilePath = @"resources\assemblyInfo.cs.tst";
#else
        public const string AssemblyInfoFilePath = @"..\..\resources\assemblyInfo.cs.tst";
#endif
        public const string TestVersionPartGeneratorToken = "{testGenerator}";

        public static readonly string VersionPatternString = string.Format("1.{{#}}.{0}.{1}",
            AssemblyPatcher.Constants.IncrementingVersionPartGeneratorToken,
            TestVersionPartGeneratorToken);

        public const string SolutionRootTestPath = "..\\..\\..\\";
        public const string TargetProjectFileTestPath = "..\\..\\BitPantry.AssemblyPatcher.Tests.csproj";
    }
}
