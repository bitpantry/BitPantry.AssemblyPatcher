namespace BitPantry.AssemblyPatcher.Tests
{
    class Constants
    {
        public const string AssemblyInfoFilePath = @"resources\assemblyInfo.cs.tst";

        public const string TestVersionPartGeneratorToken = "{testGenerator}";

        public static readonly string VersionPatternString = string.Format("1.{{#}}.{0}.{1}",
            AssemblyPatcher.Constants.IncrementingVersionPartGeneratorToken,
            TestVersionPartGeneratorToken);

        public const string SolutionRootTestPath = "..\\..\\..\\";
        public const string TargetProjectFileTestPath = "..\\..\\BitPantry.AssemblyPatcher.Tests.csproj";
    }
}
