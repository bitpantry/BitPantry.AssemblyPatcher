using System;
using System.Linq;

namespace BitPantry.AssemblyPatcher
{
    class Program
    {
        

        static void Main(string[] args)
        {
            if (args.Count() < 3 || args.Count() > 4)
            {
                ShowHelp();
            }
            else
            {
                VersionPatcher.Patch(
                    args[0],    // solution directory path
                    args[1],    // target project file
                    args[2],    // version pattern
                    args.Count() == 4 ? args[3] : null);    // alternate assembly info file path
            }
        }

        private static void ShowHelp()
        {
            Console.WriteLine();
            Console.WriteLine("Invalid invocation"); // TODO: Add content
            Console.WriteLine();
            Console.WriteLine();
        }

    }
}
