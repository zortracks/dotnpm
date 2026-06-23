using System.IO;

namespace DotNpm.Contexts {

    internal sealed class PackageContext {
        public DirectoryInfo WorkingDirectory { get; internal set; }
    }
}