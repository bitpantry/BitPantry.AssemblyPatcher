using System.Configuration;
using BitPantry.Parsing.Strings;

namespace BitPantry.AssemblyPatcher
{
    /// <summary>
    /// A facade for interacting with the app / web configuration file
    /// </summary>
    static class AppConfigFacade
    {
        /// <summary>
        /// The attribute demarcation characters in the source code for the AssemblyInfo.cs file being patched. The default
        /// is '[' and ']'. For VB.NET, for example, this would need to be set to '&gt;' and '&lt;'. 
        /// </summary>
        public static char[] AttributeDemarcationCharacters { get { return StringParsing.Parse<char[]>(GetString("BitPantry.AssemblyVersioningTool.AttributionDemarcationCharacters", "[,]")); } }

        private static string GetString(string key, string defaultValue = null)
        {
            var value = ConfigurationManager.AppSettings.Get(key);
            return value ?? defaultValue;
        }
    }
}
