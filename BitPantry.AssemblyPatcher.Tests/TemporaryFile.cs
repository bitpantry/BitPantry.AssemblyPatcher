using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitPantry.AssemblyPatcher.Tests
{
    /// <summary>
    /// Used for creating a temporary file for unit testing. The class attempts to clean up the file after
    /// the test is complete
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class TemporaryFile : IDisposable
    {
        public string FilePath { get; private set; }

        public TemporaryFile(byte[] contents = null)
        {
            FilePath = Path.Combine(Path.GetTempPath(), Path.GetTempFileName());
            File.WriteAllBytes(FilePath, contents ?? new byte[0]);
        }

        public void Dispose()
        {
            if(File.Exists(FilePath))
                File.Delete(FilePath);
        }
    }
}
